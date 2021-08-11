using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.RetailItems.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class RetailGroupsView : ViewBase
    {
        private static Guid BarSettingID = new Guid("e87c7650-b17f-11e2-9e96-0800200c9a66");
        private Setting searchBarSetting;

        RecordIdentifier selectedID = "";

        public RetailGroupsView(RecordIdentifier groupId) :
            this()
        {
            selectedID = groupId;
        }

        public RetailGroupsView()
            : base()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.RetailGroups;

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();

            searchBar1.FocusFirstInput();
            searchBar1.BuddyControl = lvGroups;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageRetailGroups);

            btnsContextButtons.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailGroups", 0, Resources.RetailGroups, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.RetailGroups;
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
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Resources.Items, ViewPages.RetailGroupItemsPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, selectedID);
            }
            lvGroups.SetSortColumn(0, true);
            loadTab(isRevert);
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailGroup":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;
            }
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void LoadItems()
        {
            List<RetailGroup> groups;
            string idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            RecordIdentifier retailDepartmentID = null;
            RecordIdentifier taxGroupID = null;
            string validationPeriodID = null;

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch(result.ParameterKey)
                {
                    case "Description":
                        idOrDescription = result.StringValue;
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;

                    case "RetailDepartment":
                        retailDepartmentID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    case "TaxGroup":
                        taxGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;

                    

                    case "ValidationPeriod":
                        validationPeriodID = (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            int groupCount;

            try
            {
                // We try to get one more record then we intend to display, in order to see if there are more records available
                groups = Providers.RetailGroupData.AdvancedSearch(PluginEntry.DataModel, itemDataScroll.StartRecord,
                    itemDataScroll.EndRecord + 1,
                    (RetailGroupSorting) lvGroups.Columns.IndexOf(lvGroups.SortColumn), 
                    !lvGroups.SortedAscending,
                    out groupCount,
                    idOrDescription, idOrDescriptionBeginsWith,
                    retailDepartmentID,
                    taxGroupID, validationPeriodID);

                itemDataScroll.RefreshState(groups, groupCount);
                lvGroups.ClearRows();

                foreach (var group in groups)
                {
                    Row row = new Row();
                    row.AddText((string) group.ID);
                    row.AddText(group.Text);
                    row.AddText(group.RetailDepartmentName);

                    row.Tag = group;

                    lvGroups.AddRow(row);

                    if (selectedID == ((RetailGroup) row.Tag).ID)
                    {
                        lvGroups.Selection.Set(lvGroups.RowCount - 1);
                    }
                }
                lvGroups.AutoSizeColumns();

                lvGroups.Sort();

                lvGroups_SelectionChanged(this, EventArgs.Empty);
            }
            finally
            {
                HideProgress();
            }
        }

        private void loadTab(bool isRevert)
        {
            RetailGroup retailGroup;
            if (lvGroups.Selection.Count > 0)
            {
                retailGroup = (RetailGroup)lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag;
            }
            else
            {
                retailGroup = new RetailGroup();
            }
            selectedID = retailGroup.MasterID;
            tabSheetTabs.SetData(isRevert, selectedID, retailGroup); ;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewRetailGroup(this, EventArgs.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowRetailGroupView(((RetailGroup)lvGroups.Selection[0].Tag).MasterID);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Resources.DeleteRetailGroupQuestion,
                Resources.DeleteRetailGroup) == DialogResult.Yes)
            {
                int selectedIndex = lvGroups.Selection.FirstSelectedRow;
                int nextSelection = selectedIndex == (lvGroups.RowCount - 1) ? selectedIndex - 1 : selectedIndex + 1;

                PluginOperations.DeleteRetailGroup(((RetailGroup)lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag).ID);

                selectedID = nextSelection == -1? "" : ((RetailGroup)lvGroups.Row(nextSelection).Tag).ID;

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RetailGroup", null, null);
            }
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);


            menu.Items.Add(new ExtendedMenuItem("-", 2000));

            item = new ExtendedMenuItem(Resources.CopyID, 2010, CopyID);
            item.Enabled = (lvGroups.Selection.Count == 1);
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RetailGroupList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void CopyID(object sender, EventArgs args)
        {
            Clipboard.SetText((string)((RetailGroup)lvGroups.Row(lvGroups.Selection.FirstSelectedRow).Tag).ID);
        }

        public override ParentViewDescriptor CurrentViewDescriptor()
        {
            //return new ParentViewDescriptor(
            //        (int)displayType,
            //        LogicalContextName,
            //        null,
            //        new ShowParentViewHandler(PluginOperations.ShowCustomerPriceDiscountGroups));

            return null;
        }

        private void lvGroups_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(sender, EventArgs.Empty);
            }
        }

        private void lvGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                lvGroups.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups);
            if (lvGroups.Selection.Count > 0)
            {
                tabSheetTabs.Visible = true;
                groupPanelNoSelection.Visible = false;
                lblNoSelection.Visible = false;
                loadTab(true);
            }
            else
            {
                tabSheetTabs.Visible = false;
                groupPanelNoSelection.Visible = true;
                lblNoSelection.Visible = true;
            }
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.TaxGroup, "TaxGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.ValidationPeriod, "ValidationPeriod", ConditionType.ConditionTypeEnum.Unknown));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailDepartment":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += RetailDepartments_DropDown;
                    break;

                case "TaxGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += TaxGroup_RequestData;
                    break;

                case "ValidationPeriod":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += ValidationPeriod_RequestData;
                    break;
            }
        }

        void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailDepartments, "", textInitallyHighlighted);
        }

        void TaxGroup_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            ((DualDataComboBox)sender).SetData(Providers.ItemSalesTaxGroupData.GetList(PluginEntry.DataModel), null);
        }

        private void ValidationPeriod_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            ((DualDataComboBox)sender).SetData(Providers.DiscountPeriodData.GetList(PluginEntry.DataModel), null);
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "ValidationPeriod":
                case "SizeGroup":
                case "ColorGroup":
                case "StyleGroup":
                case "TaxGroup":
                case "VariantGroup":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "ValidationPeriod":
                case "SizeGroup":
                case "ColorGroup":
                case "StyleGroup":
                case "TaxGroup":
                case "VariantGroup":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailDepartment":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailDepartments_DropDown;
                    break;

                case "TaxGroup":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= TaxGroup_RequestData;
                    break;

                case "ValidationPeriod":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= ValidationPeriod_RequestData;
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "RetailDepartment":
                    entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "TaxGroup":
                    entity = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "ValidationPeriod":
                    var period = Providers.DiscountPeriodData.Get(PluginEntry.DataModel, args.Selection, CacheType.CacheTypeApplicationLifeTime);
                    entity = new DataEntity(period.ID, period.Text);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }
    }
}