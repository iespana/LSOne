using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.Inventory;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class StockCountingJournalSearch : UserControl, IWizardPage
    {
        WizardBase parent;
        private InventoryTypeAction inventoryTypeAction;
        private static Guid BarSettingID = new Guid("4B33A1B7-FBBE-4B33-8629-57B3E00BC1DA");
        private Setting searchBarSetting;
        RecordIdentifier selectedID = "";
        private Store defaultStore;

        public StockCountingJournalSearch(WizardBase parent, InventoryTypeAction inventoryTypeAction) : base()
        {
            InitializeComponent();

            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;

            defaultStore = null;
            if (!string.IsNullOrWhiteSpace((string)PluginEntry.DataModel.CurrentStoreID))
            {
                searchBar.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }

            searchBar.BuddyControl = lvStockCount;

            searchBar.FocusFirstInput();

            // Sort by CreatedDate descending by default
            lvStockCount.SetSortColumn(lvStockCount.Columns[3], false);
        }

        #region IWizardPage Members

        public bool HasFinish
        {
            get { return false; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            searchBar_SearchClicked(this, EventArgs.Empty);
            CheckState();
        }

        private void CheckState()
        {
            parent.NextEnabled = lvStockCount.Selection.Count >= 1;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return new NewEmptySCJ(parent, (InventoryAdjustment)lvStockCount.Row(lvStockCount.Selection.FirstSelectedRow).Tag, inventoryTypeAction);
        }

        public void ResetControls()
        {

        }

        #endregion

        public RecordIdentifier InventoryAdjustmentID
        {
            get { return ((InventoryAdjustment)lvStockCount.Row(lvStockCount.Selection.FirstSelectedRow).Tag).ID; }
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
                InventoryAdjustmentSorting.PostingDate
            };

            if (lvStockCount.SortColumn == null)
            {
                // Sort by CreatedDate descending by default
                lvStockCount.SetSortColumn(lvStockCount.Columns[3], false);
            }

            int sortColumnIndex = lvStockCount.Columns.IndexOf(lvStockCount.SortColumn);

            if (sortColumnIndex < columns.Length)
            {
                if (columns[sortColumnIndex] == InventoryAdjustmentSorting.ID || columns[sortColumnIndex] == null)
                {
                    filter.Sort = InventoryAdjustmentSorting.ID;
                }
                else
                {
                    filter.Sort = (InventoryAdjustmentSorting)((int)columns[sortColumnIndex]);
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
                }
            }

            int totalRecordsMatching;

            List<InventoryAdjustment> journals = null;

            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage,
                    () => journals = service.GetJournalListAdvancedSearch(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, true, out totalRecordsMatching));

                dlg.ShowDialog();

                lvStockCount.ClearRows();

                Row row;

                foreach (InventoryAdjustment journal in journals)
                {
                    row = new Row();
                    row.AddText((string)journal.ID);
                    row.AddText(journal.Text);
                    row.AddText(journal.StoreName);
                    row.AddCell(new DateTimeCell(journal.CreatedDateTime.ToShortDateString(), journal.CreatedDateTime));
                    row.AddText((journal.Posted == InventoryJournalStatus.Posted) ? Resources.Posted : Resources.NotPosted);
                    row.AddCell(new DateTimeCell(journal.PostedDateTime.ToShortDateString(), journal.PostedDateTime.DateTime));

                    row.Tag = journal;

                    lvStockCount.AddRow(row);

                    if (journal.ID == selectedID)
                    {
                        lvStockCount.Selection.Set(lvStockCount.RowCount - 1);
                    }
                }

                lvStockCount.AutoSizeColumns();
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.All);
            statusList.Add(Resources.Posted);
            statusList.Add(Resources.NotPosted);

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 2, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.CreatedDate, "CreationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.PostedDate, "PostedDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

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

            searchBar.GetLocalizedSavingText();
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
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

        private void lvStockCount_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void lvStockCount_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if(parent.NextEnabled)
            {
                parent.Next();
            }
        }
    }
}