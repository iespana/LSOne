using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.DataEntities;
using System.Drawing;
using LSOne.Controls.Cells;
using LSOne.ViewPlugins.Hospitality.Properties;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class ButtonMenusView : ViewBase
    {
        private RecordIdentifier selectedPosMenuHeaderId = "";
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("9E270CCE-F138-459E-95E6-F26695EE18EB");

        public ButtonMenusView(RecordIdentifier posMenuHeaderId)
            : this()
        {
            selectedPosMenuHeaderId = posMenuHeaderId;
        }

        public ButtonMenusView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar | 
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.PosMenuButtons;

            lvPosMenuHeaders.Columns[0].Tag = PosMenuHeaderSorting.MenuID;
            lvPosMenuHeaders.Columns[1].Tag = PosMenuHeaderSorting.MenuDescription;
            lvPosMenuHeaders.Columns[4].Tag = PosMenuHeaderSorting.StyleName;

            lvPosMenuHeaders.ContextMenuStrip = new ContextMenuStrip();
            lvPosMenuHeaders.ContextMenuStrip.Opening += lvPosMenuHeaders_Opening;

            lvPosMenuHeaders.SetSortColumn(0, true);
            searchBar.BuddyControl = lvPosMenuHeaders;

            btnsEditAddRemovePosMenu.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            return new HelpSettings("HospButtonMenusView");
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosMenuHeaders", RecordIdentifier.Empty, Resources.HospitalityPosMenus, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.PosMenuButtons;
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
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PosMenuHeader":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedPosMenuHeaderId = changeIdentifier;
                    }

                    LoadData(false);
                    break;
            }
        }

        private void LoadItems()
        {
            int selectedRow = -1;
            RecordIdentifier oldSelectedID = selectedPosMenuHeaderId;

            if (lvPosMenuHeaders.SortColumn.Tag == null)
            {
                lvPosMenuHeaders.SortColumn.Tag = PosMenuHeaderSorting.MenuID;
            }

            lvPosMenuHeaders.ClearRows();

            PosMenuHeaderFilter filter = new PosMenuHeaderFilter();
            filter.MenuType = (int)MenuTypeEnum.Hospitality;
            filter.SortBy = (PosMenuHeaderSorting)lvPosMenuHeaders.SortColumn.Tag;
            filter.SortBackwards = !lvPosMenuHeaders.SortedAscending;
            filter.MainMenu = null;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        filter.Description = result.StringValue;
                        filter.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "MainMenu":
                        filter.MainMenu = result.CheckedValues[0];
                        break;
                    case "Style":
                        filter.StyleID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            // Get all sales types
            List<PosMenuHeader> posMenuHeaders = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter);

            foreach (PosMenuHeader header in posMenuHeaders)
            {
                var row = new Row();
                row.AddText((header.ID.ToString()));
                row.AddText(header.Text);
                row.AddText(header.Columns.ToString());
                row.AddText(header.Rows.ToString());
                if (header.StyleID != RecordIdentifier.Empty && header.StyleID != null)
                {
                    var style = Providers.PosStyleData.Get(PluginEntry.DataModel, header.StyleID);
                    if (style != null)
                        row.AddText(style.Text);
                }
                else
                {
                    row.AddText("");
                }
                row.AddCell(new CheckBoxCell(header.MainMenu, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                row.Tag = header;

                if (header.ID == oldSelectedID)
                {
                    selectedRow = lvPosMenuHeaders.RowCount;
                }

                lvPosMenuHeaders.AddRow(row);
            }

            if (selectedRow != -1)
            {
                lvPosMenuHeaders.Selection.Set(0, selectedRow);
                lvPosMenuHeaders.ScrollRowIntoView(selectedRow);
            }
            lvPosMenuHeaders.AutoSizeColumns();
            lvPosMenuHeaders_SelectedIndexChanged(this, EventArgs.Empty);
        }

        void lvPosMenuHeaders_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvPosMenuHeaders.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemovePosMenu_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemovePosMenu.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsEditAddRemovePosMenu_AddButtonClicked);

            item.Enabled = btnsEditAddRemovePosMenu.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsEditAddRemovePosMenu_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemovePosMenu.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Resources.Preview,
                    500,
                    btnPreview_Click);

            item.Enabled = btnPreview.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PosMenuHeaderList", lvPosMenuHeaders.ContextMenuStrip, lvPosMenuHeaders);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvPosMenuHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPosMenuHeaderId = lvPosMenuHeaders.Selection.Count > 0 ? ((PosMenuHeader)(lvPosMenuHeaders.Row(lvPosMenuHeaders.Selection.FirstSelectedRow).Tag)).ID : "";

            btnsEditAddRemovePosMenu.EditButtonEnabled = lvPosMenuHeaders.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus);
            btnsEditAddRemovePosMenu.RemoveButtonEnabled = lvPosMenuHeaders.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        private void btnsEditAddRemovePosMenu_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosMenu(this, EventArgs.Empty);
        }

        private void btnsEditAddRemovePosMenu_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosMenu(selectedPosMenuHeaderId);
        }

        private void btnsEditAddRemovePosMenu_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeletePosMenu(selectedPosMenuHeaderId);
        }

        private void lvPosMenuHeaders_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvPosMenuHeaders.Selection.Count > 0 && btnsEditAddRemovePosMenu.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenu_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.PosMenuPreviewDialog(selectedPosMenuHeaderId);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        private void lvPosMenuHeaders_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvPosMenuHeaders.SortColumn == args.Column)
            {
                lvPosMenuHeaders.SetSortColumn(args.Column, !lvPosMenuHeaders.SortedAscending);
                LoadData(false);
            }
            else
            {
                lvPosMenuHeaders.SetSortColumn(args.Column, true);
                LoadData(false);
            }
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
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.MainMenu, "MainMenu", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Yes, false));
            searchBar.AddCondition(new ConditionType(Resources.Style, "Style", ConditionType.ConditionTypeEnum.Unknown));
            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Style":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestClear += new EventHandler(DualDataComboBox_RequestClear);
                    ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Style_RequestData);
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != ""
                             && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Style":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Style_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Style":
                    entity = Providers.PosStyleData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void Style_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC"), null);
        }
    }
}