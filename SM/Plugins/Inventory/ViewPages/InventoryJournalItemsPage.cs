using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
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
using LSOne.Utilities.Expressions;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Linq;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class InventoryJournalItemsPage : UserControl, ITabView
    {
        private static Guid BarSettingID = new Guid("FCD61FC0-F8A4-4956-BA04-9E184D5D50D4");
        private Setting searchBarSetting;

        private InventoryAdjustment journal;
        private RecordIdentifier journalID;
        private List<InventoryJournalTransaction> items;
        private RecordIdentifier selectedID;

        public InventoryJournalItemsPage(TabControl owner)
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryJournalItemsPage((TabControl)sender);
        }

        #region ITabView interface
        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            journal = (InventoryAdjustment)internalContext;
            journalID = context;

            if (journal.Posted != InventoryJournalStatus.Posted)
            {
                lvItems.ContextMenuStrip = new ContextMenuStrip();
                lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);
            }

            btnsEditAddRemove.Enabled = btnMoveToInventory.Enabled = (journal.Posted != InventoryJournalStatus.Posted);

            if (journal.JournalType != InventoryJournalTypeEnum.Parked)
            {
                btnMoveToInventory.Enabled = false;
                btnMoveToInventory.Visible = false;
            }

            lvItems.Columns[0].Tag = InventoryJournalTransactionSorting.Posted;
            lvItems.Columns[1].Tag = InventoryJournalTransactionSorting.ItemId;
            lvItems.Columns[2].Tag = InventoryJournalTransactionSorting.ItemName;
            lvItems.Columns[3].Tag = InventoryJournalTransactionSorting.Variant;
            lvItems.Columns[4].Tag = InventoryJournalTransactionSorting.Quantity;
            lvItems.Columns[5].Tag = InventoryJournalTransactionSorting.UnitId;
            lvItems.Columns[6].Tag = InventoryJournalTransactionSorting.Barcode;
            lvItems.Columns[7].Tag = InventoryJournalTransactionSorting.Reason;
            lvItems.Columns[8].Tag = InventoryJournalTransactionSorting.PostedDate;
            lvItems.Columns[9].Tag = InventoryJournalTransactionSorting.Staff;
            lvItems.Columns[10].Tag = InventoryJournalTransactionSorting.Area;

            lvItems.SetSortColumn(0, true);

            searchBar.BuddyControl = lvItems;
            searchBar.FocusFirstInput();

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            LoadItems();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "ItemInventoryLine" && changeHint == DataEntityChangeType.Add && changeIdentifier == journalID)
            {
                LoadItems();
            }
        }

        public bool SaveData()
        {
            return false;
        }

        public void SaveUserInterface()
        {
            
        }
        #endregion

        private void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();
            
            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            if(journal.JournalType == InventoryJournalTypeEnum.Parked)
            {
                var lines = GetSelectedMovableJournalLines();
                if(lines != null && lines.Count > 0)
                {

                    item = new ExtendedMenuItem(
                        Resources.MoveToMainInventory,
                        200,
                        new EventHandler(btnMoveToInventory_Click));

                    item.Enabled = btnMoveToInventory.Enabled;
                    menu.Items.Add(item);
                }
            }

            PluginEntry.Framework.ContextMenuNotify("InventoryJournal", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.AllStatuses);
            statusList.Add(Resources.SearchBar_Value_Active);
            statusList.Add(Resources.SearchBar_Value_Posted);

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 0, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Variant, "Variant", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Barcode, "Barcode", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Quantity, "Quantity", ConditionType.ConditionTypeEnum.Numeric));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Unit, "Unit", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.ReasonCodes, "ReasonCode", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_PostedDate, "PostedDate", ConditionType.ConditionTypeEnum.DateRange));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Staff, "Staff", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.SearchBar_Area, "Area", ConditionType.ConditionTypeEnum.Unknown));

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

            ((ViewBase)Parent.Parent.Parent).ShowTimedProgress(searchBar.GetLocalizedSavingText());
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Unit":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(null, Resources.All);
                    ((DualDataComboBox)args.UnknownControl).RequestData += Unit_RequestData;
                    break;
                case "ReasonCode":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(null, Resources.AllReasonCodes);
                    ((DualDataComboBox)args.UnknownControl).RequestData += ReasonCode_RequestData;
                    break;
                case "Staff":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).DropDown += cmbEmployee__DropDown;
                    break;
                case "Area":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox)args.UnknownControl).RequestData += Area_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += ComboBox_RequestClear;
                    break;
            }
        }

        private void cmbEmployee__DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new EmployeeSearchPanel(PluginEntry.DataModel, e.DisplayText);
        }

        // Required to prevent some memory leak
        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Unit":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Unit_RequestData;
                    break;

                case "ReasonCode":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= ReasonCode_RequestData;
                    break;

                case "Staff":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Staff_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= ComboBox_RequestClear;
                    break;

                case "Area":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Area_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= ComboBox_RequestClear;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Unit":
                case "ReasonCode":
                case "Staff":
                case "Area":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Unit":
                case "ReasonCode":
                case "Staff":
                case "Area":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Unit":
                    entity = Providers.UnitData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.All);
                    break;
                case "ReasonCode":
                    entity = Providers.ReasonCodeData.GetReasonById(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllReasonCodes);
                    break;
                case "Staff":
                    entity = Providers.POSUserData.Get(PluginEntry.DataModel, args.Selection, UsageIntentEnum.Minimal);
                    break;
                case "Area":
                    entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            LoadItems();
        }

        private void Unit_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            List<DataEntity> units = Providers.UnitData.GetList(PluginEntry.DataModel);
            units.Insert(0, new DataEntity(null, Resources.All));

            ((DualDataComboBox)sender).SetData(units, null);
        }

        private void ReasonCode_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            var reasonCodes = Providers.ReasonCodeData.GetList(PluginEntry.DataModel,
                                                                new List<ReasonActionEnum> { ReasonActionHelper.GetEquivalent(journal.JournalType) },
                                                                forPOS: null, open: false);
            List<DataEntity> rcodes = reasonCodes.ConvertAll(rc => (DataEntity)rc);
            rcodes.Insert(0, new DataEntity(null, Resources.AllReasonCodes));

            ((DualDataComboBox)sender).SetData(rcodes, null);
        }

        private void Area_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryAreaLinesListItems(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true), null);
        }

        private void Staff_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.POSUserData.GetList(PluginEntry.DataModel), null);
        }

        private void ComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count == 1)
            {
                selectedID = ((InventoryJournalTransaction)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).ID;
            }
            else if (lvItems.Selection.Count > 1)
            {
                selectedID = null;
            }

            if (journal.JournalType == InventoryJournalTypeEnum.Parked)
            {
                btnMoveToInventory.Enabled = (lvItems.Selection.Count > 0);
            }
        }

        private void lvItems_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvItems.SortColumn == args.Column)
            {
                lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
            }
            else
            {
                lvItems.SetSortColumn(args.Column, true);
            }

            itemDataScroll.Reset();
            LoadItems();
        }
        
        private void lvItems_RowExpanded(object sender, RowEventArgs args)
        {
            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            InventoryJournalTransaction parentLine = (InventoryJournalTransaction)lvItems.Row(args.RowNumber).Tag;

            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            List<InventoryJournalTransaction> subRows = service.GetMovedToInventoryLines(
                                                                        PluginEntry.DataModel, 
                                                                        PluginOperations.GetSiteServiceProfile(),
                                                                        parentLine.JournalId,
                                                                        parentLine.MasterID,
                                                                        InventoryJournalTransactionSorting.ItemName,
                                                                        false,
                                                                        true);

            int nextRowNumber = args.RowNumber + 1;

            foreach (InventoryJournalTransaction item in subRows)
            {
                Row row = new Row();
                row.AddCell(new ExtendedCell(string.Empty));
                row.AddText((string)item.ItemId);
                row.AddText(item.ItemName);
                row.AddText(item.VariantName);
                row.AddText(item.Adjustment.FormatWithLimits(item.UnitQuantityLimiter));
                row.AddText(item.UnitDescription);
                row.AddText(item.Barcode);
                row.AddText(item.ReasonText);
                row.AddText(item.PostedDateTime.ToShortDateString() + " " + item.PostedDateTime.ToShortTimeString());
                row.AddText((string)item.StaffLogin + " - " + item.StaffName);
                row.AddText(item.AreaName);

                row.Tag = item;

                lvItems.InsertRow(nextRowNumber, row);
                items.Insert(nextRowNumber, item);

                nextRowNumber++;
            }

            lvItems.AutoSizeColumns();
        }

        private void lvItems_RowCollapsed(object sender, RowEventArgs args)
        {
            InventoryJournalTransaction parentLine = (InventoryJournalTransaction)lvItems.Row(args.RowNumber).Tag;

            int rowIndex = args.RowNumber + 1;

            while (rowIndex < lvItems.RowCount && parentLine.MasterID == ((InventoryJournalTransaction)lvItems.Row(rowIndex).Tag).ParentMasterID)
            {
                lvItems.RemoveRow(rowIndex);
                items.RemoveAt(rowIndex);
            }

            lvItems.AutoSizeColumns();
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var dialog = new AdjustmentLineDialog(journalID, journal.StoreId, journal.JournalType);
            dialog.ShowDialog();

            if(dialog.DialogResult == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnMoveToInventory_Click(object sender, EventArgs e)
        {
            List<InventoryJournalTransaction> itemsToMove = GetSelectedMovableJournalLines();

            if (!itemsToMove.Any())
            {
                MessageDialog.Show(Resources.SelectItemToMoveToInventory);
                return;
            }

            DialogResult result;
            if (itemsToMove.Count > 1)
            {
                result = MessageDialog.Show(Resources.MoveToInventoryMultipleLines, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                using (SelectReasonCodeDialog dlg = new SelectReasonCodeDialog(ReasonActionEnum.MainInventory))
                {
                    result = dlg.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    PluginOperations.MoveToInventory(journal, itemsToMove, dlg.SelectedReasonCode);
                }
            }
            else
            {
                InventoryJournalTransaction line = itemsToMove[0];
                using (MoveToMainInventoryDialog dlg = new MoveToMainInventoryDialog(line))
                {
                    result = dlg.ShowDialog(); //Saving logic made in dialog
                }
            }

            LoadItems();
        }

        private InventoryJournalLineSearch CreateSearchCriteria()
        {

            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[0], true);
            }

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            InventoryJournalLineSearch searchCriteria = new InventoryJournalLineSearch();
            searchCriteria.JournalId = journalID;
            searchCriteria.SortBy = (InventoryJournalTransactionSorting)lvItems.SortColumn.Tag;
            searchCriteria.SortBackwards = !lvItems.SortedAscending;
            searchCriteria.RowFrom = itemDataScroll.StartRecord;
            searchCriteria.RowTo = itemDataScroll.EndRecord + 1;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                searchCriteria.Status = (int)InventoryJournalStatus.Active;
                                break;
                            case 2:
                                searchCriteria.Status = (int)InventoryJournalStatus.Posted;
                                break;
                            default:
                                searchCriteria.Status = null;
                                break;
                        }
                        break;
                    case "Description":
                        searchCriteria.Description = new List<string> { result.StringValue };
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Variant":
                        searchCriteria.VariantDescription = result.StringValue.Tokenize();
                        searchCriteria.VariantDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Quantity":
                        searchCriteria.Quantity = (decimal)result.DoubleValue;
                        searchCriteria.QuantityOperator = result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals
                                        ? DoubleValueOperator.Equals
                                        : result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan
                                                ? DoubleValueOperator.GreaterThan
                                                : DoubleValueOperator.LessThan;
                        break;
                    case "Unit":
                        searchCriteria.UnitID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "ReasonCode":
                        searchCriteria.ReasonCodeID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "PostedDate":
                        if (result.Date.Checked)
                        {
                            searchCriteria.PostedDateFrom.DateTime = result.Date.Value.Date;
                        }

                        if (result.DateTo.Checked)
                        {
                            searchCriteria.PostedDateTo.DateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        break;
                    case "Staff":
                        searchCriteria.StaffLogin = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Area":
                        searchCriteria.AreaID = (Guid)((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Barcode":
                        searchCriteria.Barcode = result.StringValue.Tokenize();
                        searchCriteria.BarcodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                }
            }

            return searchCriteria;
        }

        /// <summary>
        /// Returns a list of all selected journal lines that can be moved to inventory. Return empty list if no record is selected.
        /// </summary>
        /// <returns></returns>
        private List<InventoryJournalTransaction> GetSelectedMovableJournalLines()
        {
            if (lvItems.Selection.Count == 0)
            {
                return new List<InventoryJournalTransaction>();
            }

            List<InventoryJournalTransaction> selection = new List<InventoryJournalTransaction>();
            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                InventoryJournalTransaction line = (InventoryJournalTransaction)lvItems.Selection[i].Tag;
                if ((line.ParentMasterID == null || line.ParentMasterID.IsEmpty) && Math.Abs(line.Adjustment) > line.MovedQuantity)
                {
                    selection.Add(line);
                }
            }

            return selection;
        }

        private void LoadItems()
        {
            lvItems.ClearRows();

            InventoryJournalLineSearch searchCriteria = CreateSearchCriteria();

            int itemsCount = 0;
            items = new List<InventoryJournalTransaction>();
            IInventoryService service = null;

            try
            {
                service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => items = service.AdvancedSearch(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), searchCriteria, out itemsCount, true));

                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show((service != null) ? Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetExceptionDisplayText(ex) : ex.Message);
                return;
            }

            itemDataScroll.RefreshState(items, itemsCount);

            Row row;
            foreach (InventoryJournalTransaction item in items)
            {
                row = new Row();

                Bitmap statusImage = null;
                switch(item.Status)
                {
                    case InventoryJournalStatus.PartialPosted:
                        statusImage = Resources.dot_yellow_16;
                        break;
                    case InventoryJournalStatus.Closed:
                        statusImage = Resources.dot_finished_16;
                        break;
                    case InventoryJournalStatus.Posted:
                        statusImage = (journal.JournalType == InventoryJournalTypeEnum.Parked ? Resources.dot_grey_16 : Resources.dot_finished_16);
                        break;
                    default:
                        statusImage = Resources.dot_grey_16;
                        break;
                }

                row.AddCell(new ExtendedCell(string.Empty, statusImage));

                if (item.MovedQuantity == 0)
                {
                    row.AddText((string)item.ItemId);
                }
                else
                {
                    row.AddCell(new VirtualCollapsableCell((string)item.ID, true));
                }
                row.AddText(item.ItemName);
                row.AddText(item.VariantName);
                row.AddText(item.Adjustment.FormatWithLimits(item.UnitQuantityLimiter));
                row.AddText(item.UnitDescription);
                row.AddText(item.Barcode);
                row.AddText(item.ReasonText);
                row.AddText(item.PostedDateTime.ToShortDateString() + " " + item.PostedDateTime.ToShortTimeString());
                row.AddText((string)item.StaffLogin + " - " + item.StaffName);
                row.AddText(item.AreaName);

                row.Tag = item;

                lvItems.AddRow(row);

                if (selectedID == item.ID)
                {
                    lvItems.Selection.Set(lvItems.RowCount - 1);
                }
            }

            lvItems.AutoSizeColumns();
            lvItems_SelectionChanged(this, EventArgs.Empty);
        }
    }
}