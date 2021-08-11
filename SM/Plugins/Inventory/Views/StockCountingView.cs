using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class StockCountingView : ViewBase
    {
        private static Guid BarSettingID = new Guid("4B33A1B7-FBBE-4B33-8629-57B3E00BC1DA");
        private Setting searchBarSetting;
        RecordIdentifier selectedID = "";
        private Store defaultStore;

        public StockCountingView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Help;

            HeaderText = Resources.StockCounting;

            defaultStore = null;
            if (!string.IsNullOrWhiteSpace((string)PluginEntry.DataModel.CurrentStoreID))
            {
                searchBar.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }

            lvStockCount.ContextMenuStrip = new ContextMenuStrip();
            lvStockCount.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            searchBar.BuddyControl = lvStockCount;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StockCounting);

            if (!PluginEntry.DataModel.HasPermission(Permission.StockCounting))
            {
                btnsEditAddRemove.AddButtonEnabled = false;
                errorProvider.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider.SetError(btnsEditAddRemove, Resources.MissingStockCountingPermission);
            }

            // Sort by CreatedDate descending by default
            lvStockCount.SetSortColumn(lvStockCount.Columns[4], false);
        }        

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Vendors", 0, Resources.Vendors, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.StockCounting;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            searchBar_SearchClicked(null, EventArgs.Empty);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "JournalTrans":
                    LoadItems();
                    break;
            }
        }

        private void LoadItems()
        {
            InventoryAdjustmentFilter filter = new InventoryAdjustmentFilter();
            filter.PagingSize = PluginEntry.DataModel.PageSize;
            filter.Sort = InventoryAdjustmentSorting.ID;
            filter.JournalType = InventoryJournalTypeEnum.Counting;

            InventoryAdjustmentSorting?[] columns =
            {
                InventoryAdjustmentSorting.ID,
                InventoryAdjustmentSorting.Description,
                InventoryAdjustmentSorting.StoreName,
                InventoryAdjustmentSorting.CreatedDateTime,
                InventoryAdjustmentSorting.Posted,
                InventoryAdjustmentSorting.PostingDate,
                InventoryAdjustmentSorting.ProcessingStatus
            };

            if (lvStockCount.SortColumn == null)
            {
                // Sort by CreatedDate descending by default
                lvStockCount.SetSortColumn(lvStockCount.Columns[4], false);
            }

            // Set filter fields variable
            int sortColumnIndex = lvStockCount.Columns.IndexOf(lvStockCount.SortColumn);

            if (sortColumnIndex < columns.Length)
            {
                if (columns[sortColumnIndex - 1] == InventoryAdjustmentSorting.ID || columns[sortColumnIndex - 1] == null)
                {
                    filter.Sort = InventoryAdjustmentSorting.ID;
                }
                else
                {
                    filter.Sort = (InventoryAdjustmentSorting)((int)columns[sortColumnIndex - 1]);
                }
            }

            filter.SortBackwards = !lvStockCount.SortedAscending;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                filter.Status = InventoryJournalStatus.Posted;
                                break;
                            case 2:
                                filter.Status = InventoryJournalStatus.Active;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Store":
                        filter.StoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "idOrDescription":
                        filter.IdOrDescription.Add(result.StringValue);
                        filter.IdOrDescriptionBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "CreationDate":
                        filter.CreationDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        filter.CreationDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "PostedDate":
                        filter.PostedDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        filter.PostedDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "ProcessingStatus":
                        if(result.ComboSelectedIndex != 0)
                        {
                            filter.ProcessingStatus = (InventoryProcessingStatus)(result.ComboSelectedIndex - 1);
                        }
                        break;
                }
            }

            int totalRecordsMatching;

            List<InventoryAdjustment> journals = null;

            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                journals = service.GetJournalListAdvancedSearch(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, true, out totalRecordsMatching);

                lvStockCount.ClearRows();

                if (totalRecordsMatching > filter.PagingSize)
                {
                    lblMsg.Text = Resources.NumberOfStockCountingReturned.Replace("#1", Conversion.ToStr(filter.PagingSize)).Replace("#2", Conversion.ToStr(totalRecordsMatching));
                }

                foreach (InventoryAdjustment journal in journals)
                {
                    Row row = new Row();

                    Bitmap statusImage = null;
                    switch (journal.Posted)
                    {
                        case InventoryJournalStatus.Posted:
                            statusImage = Resources.dot_finished_16;
                            break;
                        case InventoryJournalStatus.Active:
                        default:
                            statusImage = journal.ProcessingStatus == InventoryProcessingStatus.None ? Resources.dot_grey_16 : Resources.dot_yellow_16;
                            break;
                    }

                    row.AddCell(new ExtendedCell(string.Empty, statusImage));
                    row.AddText((string)journal.ID);
                    row.AddText(journal.Text);
                    row.AddText(journal.StoreName);
                    row.AddCell(new DateTimeCell(journal.CreatedDateTime.ToShortDateString(), journal.CreatedDateTime));
                    row.AddText((journal.Posted == InventoryJournalStatus.Posted) ? Resources.Posted : Resources.NotPosted);
                    row.AddCell(new DateTimeCell(journal.PostedDateTime.ToShortDateString(), journal.PostedDateTime.DateTime));
                    row.AddText(GetProcessingText(journal.ProcessingStatus));

                    row.Tag = journal;

                    lvStockCount.AddRow(row);

                    if (journal.ID == selectedID)
                    {
                        lvStockCount.Selection.Set(lvStockCount.RowCount - 1);
                    }
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            finally
            {
                HideProgress();
            }
            lvStockCounts_SelectionChanged(this, EventArgs.Empty);
            lvStockCount.AutoSizeColumns();
        }

        private string GetProcessingText(InventoryProcessingStatus status)
        {
            switch (status)
            {
                case InventoryProcessingStatus.None:
                    return Resources.None;
                case InventoryProcessingStatus.Compressing:
                    return Resources.Compressing;
                case InventoryProcessingStatus.Posting:
                    return Resources.Posting;
                case InventoryProcessingStatus.Importing:
                    return Resources.Importing;
                case InventoryProcessingStatus.Other:
                    return Resources.Other;
                default:
                    return "";
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            var journalId = ((InventoryAdjustment)lvStockCount.Rows[lvStockCount.Selection.FirstSelectedRow].Tag).ID;
            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                InventoryAdjustment journal = service.GetStockCounting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalId, true);

                selectedID = journal.ID;

                PluginOperations.ShowStockCountingDetailView(journal.ID);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewStockCountingWizard();
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.RemoveStockCountingQuestion, Resources.RemoveStockCounting) == DialogResult.Yes)
            {
                try
                {
                    var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    DeleteJournalResult result = service.DeleteStockCounting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), ((InventoryAdjustment)(lvStockCount.Rows[lvStockCount.Selection.FirstSelectedRow].Tag)).ID, true);

                    if(result == DeleteJournalResult.PartiallyPosted)
                    {
                        MessageDialog.Show(Resources.PartiallyPostedJournalCannotBeDeleted);
                    }
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
            }
            LoadItems();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStockCount.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(Resources.Edit, 100, new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.Add, 200, new EventHandler(btnsEditAddRemove_AddButtonClicked));
            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.Delete, 300, new EventHandler(btnsEditAddRemove_RemoveButtonClicked));
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;
            menu.Items.Add(item);
            
            PluginEntry.Framework.ContextMenuNotify("StockCountingView", lvStockCount.ContextMenuStrip, lvStockCount);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvStockCounts_SelectionChanged(object sender, EventArgs e)
        {
            InventoryAdjustment inventoryAdjustment = lvStockCount.Selection.Count > 0 ? (InventoryAdjustment)lvStockCount.Row(lvStockCount.Selection.FirstSelectedRow).Tag : null;
            btnsEditAddRemove.EditButtonEnabled = lvStockCount.Selection.Count > 0;
            btnsEditAddRemove.RemoveButtonEnabled = lvStockCount.Selection.Count > 0 && inventoryAdjustment.Posted != InventoryJournalStatus.Posted && !inventoryAdjustment.IsPartiallyPosted;
        }

        private void lvStockCount_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.All);
            statusList.Add(Resources.Posted);
            statusList.Add(Resources.NotPosted);

            List<object> processingStatusList = new List<object>();
            processingStatusList.Add(Resources.All);
            processingStatusList.Add(Resources.None);
            processingStatusList.Add(Resources.Compressing);
            processingStatusList.Add(Resources.Posting);
            processingStatusList.Add(Resources.Importing);
            processingStatusList.Add(Resources.Other);

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 2, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.CreatedDate, "CreationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.PostedDate, "PostedDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.ProcessingStatus, "ProcessingStatus", ConditionType.ConditionTypeEnum.ComboBox, processingStatusList, 0, 0, false));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar.GetLocalizedSavingText());
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                    ((DualDataComboBox)args.UnknownControl).RequestData += Store_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Store_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            stores.Insert(0, new DataEntity(null, Resources.AllStores));
            ((DualDataComboBox)sender).SetData(stores, null);
        }
    }
}