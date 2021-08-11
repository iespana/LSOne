using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls.Rows;
using LSOne.ViewPlugins.SerialNumbers.Properties;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using System.Drawing;
using LSOne.ViewCore.Dialogs;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.ViewPlugins.SerialNumbers.Dialogs;

namespace LSOne.ViewPlugins.SerialNumbers.Views
{
    public partial class SerialNumbersView : ViewBase
    {
        private static Guid BarSettingID = new Guid("c1077d9e-bce6-4fb3-8323-833fe83ed3ba");

        private RecordIdentifier selectedID = "";
        private List<SerialNumber> serialNumbers;
        private Setting searchBarSetting;
        private SerialNumberFilter searchCriteria;

        public SerialNumbersView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.SerialNumbersViewHeader;

            searchBarSerialNumbers.BuddyControl = lvSerialNumbers;

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            searchBarSerialNumbers.FocusFirstInput();

            lvSerialNumbers.ContextMenuStrip = new ContextMenuStrip();
            lvSerialNumbers.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            AddGridColumns();
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SerialNumbers", 0, Resources.SerialNumbersViewHeader, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.SerialNumbersViewHeader;
            }
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Resources.Actions, this.GetType().ToString() + ".Actions"), 5);
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
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "SerialNumbers":
                    if (changeIdentifier != RecordIdentifier.Empty)
                    {
                        selectedID = lvSerialNumbers.Rows.FindIndex(p => ((SerialNumber)p.Tag).ID == changeIdentifier);
                    }
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
            }
        }

        private SerialNumberFilter GetFilter()
        {
            List<SearchParameterResult> results = searchBarSerialNumbers.SearchParameterResults;

            searchCriteria = new SerialNumberFilter();
            searchCriteria.ManualEntrySet = false;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        searchCriteria.Description = result.StringValue;
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Variant":
                        searchCriteria.Variant = result.StringValue;
                        searchCriteria.VariantBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "SerialNumber":
                        searchCriteria.SerialNumber = result.StringValue;
                        searchCriteria.SerialNumberBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Type":
                        if (result.ComboSelectedIndex == 0)
                        {
                            searchCriteria.SerialType = null;
                        }
                        else
                        {
                            searchCriteria.SerialType = (TypeOfSerial)(result.ComboSelectedIndex - 1);
                        }
                        break;
                    case "ManualEntry":
                        searchCriteria.ManualEntrySet = true;
                        searchCriteria.ManualEntry = result.CheckedValues[0];
                        break;
                    case "Sold":
                        if (result.Date.Checked && !result.Time.Checked)
                        {
                            searchCriteria.SoldStartDate = result.Date.Value.Date;
                        }
                        if (result.DateTo.Checked && !result.TimeTo.Checked)
                        {
                            searchCriteria.SoldEndDate = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        if (result.Date.Checked && result.Time.Checked)
                        {
                            searchCriteria.SoldStartDate = new DateTime(result.Date.Value.Year, result.Date.Value.Month,
                                result.Date.Value.Day, result.Time.Value.Hour, result.Time.Value.Minute,
                                result.Time.Value.Second);
                        }
                        if (result.DateTo.Checked && result.TimeTo.Checked)
                        {
                            searchCriteria.SoldEndDate = new DateTime(result.DateTo.Value.Year, result.DateTo.Value.Month,
                                result.DateTo.Value.Day, result.TimeTo.Value.Hour, result.TimeTo.Value.Minute,
                                result.TimeTo.Value.Second);
                        }
                        break;
                    case "Reference":
                        searchCriteria.Reference = result.StringValue;
                        searchCriteria.ReferenceBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "RetailGroup":
                        searchCriteria.RetailGroup = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "RetailDepartment":
                        searchCriteria.RetailDepartment = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "SpecialGroup":
                        searchCriteria.SpecialGroup = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "BarCode":
                        searchCriteria.Barcode = result.StringValue;
                        searchCriteria.BarcodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Deleted":
                        searchCriteria.ShowDeletedItems = result.CheckedValues[0];
                        break;
                }
            }

            return searchCriteria;
        }

        private void AddGridColumns()
        {
            lvSerialNumbers.Columns.Add(new Column(Resources.ItemID, true) { InternalSort = true, Tag = SerialNumberSorting.ItemID });
            lvSerialNumbers.Columns.Add(new Column(Resources.Description, true) { InternalSort = true, Tag = SerialNumberSorting.ItemDescription });
            lvSerialNumbers.Columns.Add(new Column(Resources.Variant, true) { InternalSort = true, Tag = SerialNumberSorting.ItemVariant });
            lvSerialNumbers.Columns.Add(new Column(Resources.SerialNumber, true) { InternalSort = true, Tag = SerialNumberSorting.SerialNumber });
            lvSerialNumbers.Columns.Add(new Column(Resources.Type, true) { InternalSort = true, Tag = SerialNumberSorting.TypeOfSerial });
            lvSerialNumbers.Columns.Add(new Column(Resources.Sold, true) { InternalSort = true, Tag = SerialNumberSorting.Sold });
            lvSerialNumbers.Columns.Add(new Column(Resources.Reference, true) { InternalSort = true, Tag = SerialNumberSorting.Reference });
            lvSerialNumbers.Columns.Add(new Column(Resources.ManualEntry, true) { InternalSort = true, Tag = SerialNumberSorting.ManualEntry });
        }

        private List<SerialNumber> GetItems(bool paged = true)
        {
            searchCriteria = GetFilter();
            if (lvSerialNumbers.SortColumn == null)
            {
                lvSerialNumbers.SetSortColumn(lvSerialNumbers.Columns[0], true);
            }

            searchCriteria.SortBy = (SerialNumberSorting)lvSerialNumbers.SortColumn.Tag;
            searchCriteria.SortAscending = lvSerialNumbers.SortedAscending;

            int itemsCount = 0;
            searchCriteria.RowFrom = itemDataScroll.StartRecord;
            searchCriteria.RowTo = paged ? itemDataScroll.EndRecord + 1 : int.MaxValue;
            List<SerialNumber> items = Providers.SerialNumberData.GetListByFilter(PluginEntry.DataModel, searchCriteria, out itemsCount);

            if (paged)
            {
                itemDataScroll.RefreshState(items, itemsCount);
            }

            return items;
        }

        private void LoadItems()
        {
            lvSerialNumbers.ClearRows();

            try
            {
                serialNumbers = GetItems(true);

                Row row;
                foreach (SerialNumber serialNumber in serialNumbers)
                {
                    row = new Row();
                    row.AddText(serialNumber.ItemID);
                    row.AddText(serialNumber.ItemDescription);
                    row.AddText(serialNumber.ItemVariant);
                    row.AddText(serialNumber.SerialNo);
                    row.AddText(SerialNumber.GetTypeOfSerialString(serialNumber.SerialType));
                    if (serialNumber.UsedDate.HasValue)
                    {
                        row.AddCell(new DateTimeCell(serialNumber.UsedDate.Value.ToString(), serialNumber.UsedDate.Value));
                    }
                    else
                    {
                        row.AddText(string.Empty);
                    }
                    row.AddText(serialNumber.Reference);
                    row.AddCell(new CheckBoxCell(serialNumber.ManualEntry, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                    row.Tag = serialNumber;

                    lvSerialNumbers.AddRow(row);

                    if (lvSerialNumbers.Rows.IndexOf(row) == selectedID)
                    {
                        lvSerialNumbers.Selection.AddRows(lvSerialNumbers.RowCount - 1, lvSerialNumbers.RowCount - 1);
                    }
                }

                lvSerialNumbers.AutoSizeColumns();
                lvSerialNumbers_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MessageDialog.Show(Resources.LoadItemsError + "\r\n" + ex.Message);
            }
            finally
            {
                HideProgress();
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers))
            {
                SerialNumber sn = (SerialNumber)lvSerialNumbers.Rows[lvSerialNumbers.Selection.FirstSelectedRow].Tag;
                SerialNumberDialog editSerialNumber = new SerialNumberDialog(sn.ID);
                editSerialNumber.ShowDialog();
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers))
            {
                SerialNumberDialog addSerialNumber = new SerialNumberDialog(RecordIdentifier.Empty);
                addSerialNumber.ShowDialog();
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers))
            {
                if (QuestionDialog.Show(
                        Resources.DeleteSerialNumberQuestion,
                        Resources.DeleteSerialNumber)
                    == DialogResult.Yes)
                {
                    RecordIdentifier id = GetSelectedID();
                    SerialNumber contextSerialNumber = Providers.SerialNumberData.Get(PluginEntry.DataModel, id);
                    if (contextSerialNumber.Reserved || contextSerialNumber.UsedDate.HasValue)
                    {
                        MessageDialog.Show(Resources.SerialNumberReserved);
                        return;
                    }
                    Providers.SerialNumberData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "SerialNumbers", id, null);
                }
            }
        }

        private RecordIdentifier GetSelectedID()
        {
            if (lvSerialNumbers.Selection.Count != 1)
            {
                return RecordIdentifier.Empty;
            }

            return ((SerialNumber)lvSerialNumbers.Selection[0].Tag).ID;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvSerialNumbers.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SerialNumbersList", lvSerialNumbers.ContextMenuStrip, lvSerialNumbers);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Actions")
            {
                arguments.Add(new ContextBarItem(Resources.ImportSerialNumbers, "ImportSerialNumbers", PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers), ImportSerialNumbers), 1);
                arguments.Add(new ContextBarItem(Resources.ExportSerialNumbers, "ExportSerialNumbers", true, ExportSerialNumbers), 2);
            }
        }

        private void ImportSerialNumbers(object sender, EventArgs e)
        {
            SerialNumberImportDialog importDialog = new SerialNumberImportDialog();
            importDialog.ShowDialog();
        }

        private void ExportSerialNumbers(object sender, EventArgs e)
        {
            PluginOperationArguments args = new PluginOperationArguments("", GetItems(false));
            PluginEntry.Framework.RunOperation("ExportSerialNumbers", this, args);
        }

        #region Serial numbers list view events 

        private void lvSerialNumbers_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvSerialNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool canEditDelete = false;
            if (lvSerialNumbers.Selection.Count == 1)
            {
                SerialNumber selectedSerialNumber = (SerialNumber)lvSerialNumbers.Selection[0].Tag;
                canEditDelete = !selectedSerialNumber.UsedDate.HasValue;
            }
            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers) && canEditDelete;
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers) && canEditDelete;
            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageSerialNumbers);
        }

        private void lvSerialNumbers_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            itemDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        #endregion

        #region SearchBar events

        private void searchBarSerialNumbers_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBarSerialNumbers.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBarSerialNumbers_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBarSerialNumbers.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBarSerialNumbers_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBarSerialNumbers_SetupConditions(object sender, EventArgs e)
        {
            searchBarSerialNumbers.MaxNumberOfSections = 11;

            List<object> allSerialTypes = new List<object>() { Resources.All, SerialNumber.GetTypeOfSerialString(TypeOfSerial.SerialNumber), SerialNumber.GetTypeOfSerialString(TypeOfSerial.RFIDTag) };

            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Variant, "Variant", ConditionType.ConditionTypeEnum.Text));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.SerialNumber, "SerialNumber", ConditionType.ConditionTypeEnum.Text));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Type, "Type", ConditionType.ConditionTypeEnum.ComboBox, allSerialTypes, 0, 0, false));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.ManualEntry, "ManualEntry", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Manual, false));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Sold, "Sold", ConditionType.ConditionTypeEnum.DateAndTimeRange));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Reference, "Reference", ConditionType.ConditionTypeEnum.Text));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.RetailGroup, "RetailGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment", ConditionType.ConditionTypeEnum.Unknown));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.SpecialGroup, "SpecialGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Barcode, "BarCode", ConditionType.ConditionTypeEnum.Text));
            searchBarSerialNumbers.AddCondition(new ConditionType(Resources.Deleted, "Deleted", ConditionType.ConditionTypeEnum.Checkboxes, Resources.ShowDeletedItems, false));

            searchBarSerialNumbers_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBarSerialNumbers_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "SpecialGroup":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBarSerialNumbers_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "SpecialGroup":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBarSerialNumbers_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailGroup_DropDown;
                    break;

                case "RetailDepartment":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailDepartments_DropDown;
                    break;

                case "SpecialGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= SpecialGroups_DropDown;
                    break;
            }
        }

        private void searchBarSerialNumbers_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    entity = Providers.RetailGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "RetailDepartment":
                    entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "SpecialGroup":
                    entity = Providers.SpecialGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailDepartmentsMasterID, "");
        }

        void RetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailGroupsMasterID, "");
        }

        void SpecialGroups_DropDown(object sender, DropDownEventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.SpecialGroups, "");
        }

        private void searchBarSerialNumbers_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailGroup_DropDown);
                    break;

                case "RetailDepartment":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailDepartments_DropDown);
                    break;

                case "SpecialGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(SpecialGroups_DropDown);
                    break;
            }
        }

        #endregion
    }
}