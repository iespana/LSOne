using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.TouchButtons.Dialogs;
using LSOne.ViewPlugins.TouchButtons.Properties;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Drawing;
using LSOne.Controls.Cells;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    public partial class ButtonMenusView : ViewBase
    {
        private RecordIdentifier selectedPosMenuHeaderId = "";
        private DeviceTypeEnum deviceType = DeviceTypeEnum.POS;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("5ACEEE5B-46E7-49AC-BAA0-FF264B2759A1");

        public ButtonMenusView(RecordIdentifier posMenuHeaderId)
            : this()
        {
            selectedPosMenuHeaderId = posMenuHeaderId;
            HeaderText = Resources.POSButtonGridMenus;
        }

        public ButtonMenusView(RecordIdentifier posMenuHeaderId, DeviceTypeEnum deviceType)
          :  this(posMenuHeaderId)
        {
            this.deviceType = deviceType;
            HeaderText = deviceType == DeviceTypeEnum.POS
                            ? Resources.POSButtonGridMenus
                            : Resources.MobileInvButtonGridMenus;
        }

        public ButtonMenusView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Resources.POSButtonGridMenus;

            lvPosMenuHeaders.SetSortColumn(0, false);
            lvPosMenuHeaders.SortColumn.Tag = PosMenuHeaderSorting.MenuID;

            lvPosMenuHeaders.Columns[0].Tag = PosMenuHeaderSorting.MenuID;
            lvPosMenuHeaders.Columns[1].Tag = PosMenuHeaderSorting.MenuDescription;
            lvPosMenuHeaders.Columns[4].Tag = PosMenuHeaderSorting.StyleName;

            lvPosMenuHeaders.ContextMenuStrip = new ContextMenuStrip();
            lvPosMenuHeaders.ContextMenuStrip.Opening += lvPosMenuHeaders_Opening;

            searchBar.BuddyControl = lvPosMenuHeaders;

            btnsEditAddRemovePosMenu.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus);
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            if (deviceType == DeviceTypeEnum.POS)
                return new HelpSettings("POSButtonMenusView");
            else
                return new HelpSettings("InvButtonMenusView");
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosMenuHeaders", RecordIdentifier.Empty, LogicalContextName, false));
            contexts.Add(new AuditDescriptor("PosMenuLines", RecordIdentifier.Empty, Resources.POSButtonGridMenuButtons, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return deviceType == DeviceTypeEnum.POS
                            ? Resources.POSButtonGridMenus
                            : Resources.MobileInvButtonGridMenus;
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
                case "PosButtonGridMenuHeader":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add)
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
            RecordIdentifier oldSelectedId = selectedPosMenuHeaderId;

            int sortBy = lvPosMenuHeaders.Columns.IndexOf(lvPosMenuHeaders.SortColumn);
            sortBy -= sortBy > 1 ? 2 : 0; //Columns "Columns" and "Rows" are not sortable

            PosMenuHeaderFilter filter = new PosMenuHeaderFilter();

            filter.SortBy = (PosMenuHeaderSorting)sortBy;
            filter.SortBackwards = !lvPosMenuHeaders.SortedAscending;
            filter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
            filter.MainMenu = null;
            filter.DeviceType = (int)deviceType;

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

            lvPosMenuHeaders.ClearRows();
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
                row.AddText(header.ImportDateTime != null ? header.ImportDateTime.ToString() : "");
                row.AddCell(new CheckBoxCell(header.MainMenu, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.Tag = header;

                if (header.ID == oldSelectedId)
                {
                    selectedRow = lvPosMenuHeaders.RowCount;
                }

                lvPosMenuHeaders.AddRow(row);

            }

            if (selectedRow != -1)
            {
                lvPosMenuHeaders.Selection.Set(0, selectedRow);
                lvPosMenuHeaders.CalculateLayout();
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
                    Resources.EditText,
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

            item = new ExtendedMenuItem(
                   Resources.CreateNewStyle,
                   600,
                   CreateStyleFromHeader);

            item.Enabled = btnsEditAddRemovePosMenu.EditButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PosButtonGridMenuHeaderList", lvPosMenuHeaders.ContextMenuStrip, lvPosMenuHeaders);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvPosMenuHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool singleSelection = lvPosMenuHeaders.Selection.Count == 1;
            bool hasSelection = lvPosMenuHeaders.Selection.Count > 0;

            selectedPosMenuHeaderId = hasSelection ? ((PosMenuHeader)(lvPosMenuHeaders.Row(lvPosMenuHeaders.Selection.FirstSelectedRow).Tag)).ID : "";

            btnsEditAddRemovePosMenu.EditButtonEnabled = singleSelection && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ViewPosMenus);
            btnPreview.Enabled = btnCreateStylesFromHeader.Enabled = btnsEditAddRemovePosMenu.EditButtonEnabled;
            btnsEditAddRemovePosMenu.RemoveButtonEnabled = hasSelection && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus);
        }

        private void CreateStyleFromHeader(object sender, EventArgs e)
        {
            CreateStyle(selectedPosMenuHeaderId);
        }

        private void btnsEditAddRemovePosMenu_AddButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier newID = PluginOperations.NewPosButtonGridMenu(deviceType);
            if(newID != RecordIdentifier.Empty)
            {
                PluginOperations.ShowPosButtonGridView(newID);
            }
        }

        private void btnsEditAddRemovePosMenu_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosButtonGridView(selectedPosMenuHeaderId);
        }

        private void btnsEditAddRemovePosMenu_RemoveButtonClicked(object sender, EventArgs e)
        {
            List<RecordIdentifier> idsToRemove = new List<RecordIdentifier>();
            for (int i = 0; i < lvPosMenuHeaders.Selection.Count; i++)
            {
                idsToRemove.Add(((PosMenuHeader)lvPosMenuHeaders.Selection[i].Tag).ID);
            }
            PluginOperations.DeletePosButtonGridMenu(idsToRemove);
        }
        
        private void btnPreview_Click(object sender, EventArgs e)
        {
            var dlg = new PosButtonGridMenuPreviewDialog(selectedPosMenuHeaderId);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);

        }

        public static void CreateStyle(RecordIdentifier stylePosMenuHeaderId)
        {
            var dlg = new NewStyleFromHeaderDialog();

            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                var selectedPosMenuHeaderId = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, stylePosMenuHeaderId);
                var posStyle = new PosStyle();


                posStyle.Text = dlg.StyleHeader;

                //// Button attributes
                posStyle.FontName = selectedPosMenuHeaderId.FontName;
                posStyle.FontSize = selectedPosMenuHeaderId.FontSize;
                posStyle.FontBold = selectedPosMenuHeaderId.FontBold;
                posStyle.FontItalic = selectedPosMenuHeaderId.FontItalic;
                posStyle.ForeColor = selectedPosMenuHeaderId.ForeColor;
                posStyle.FontCharset = selectedPosMenuHeaderId.FontCharset;
                posStyle.BackColor = selectedPosMenuHeaderId.BackColor;
                posStyle.BackColor2 = selectedPosMenuHeaderId.BackColor2;
                posStyle.GradientMode = selectedPosMenuHeaderId.GradientMode;
                posStyle.Shape = selectedPosMenuHeaderId.Shape;

                Providers.PosStyleData.Save(PluginEntry.DataModel, posStyle);
            }
        }

        private void lvPosMenuHeaders_HeaderClicked(object sender, ColumnEventArgs args)
        {
            bool ascending = lvPosMenuHeaders.SortColumn != args.Column || !lvPosMenuHeaders.SortedAscending;
            lvPosMenuHeaders.SetSortColumn(args.Column, ascending);
            LoadData(false);
        }

        private void lvPosMenuHeaders_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvPosMenuHeaders.Selection.Count > 0 && btnsEditAddRemovePosMenu.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenu_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnCreateStylesFromHeader_Click(object sender, EventArgs e)
        {
            CreateStyleFromHeader(sender, e);
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
            searchBar.AddCondition(new ConditionType(Properties.Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.MainMenu, "MainMenu", ConditionType.ConditionTypeEnum.Checkboxes, Properties.Resources.Yes, false));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Style, "Style", ConditionType.ConditionTypeEnum.Unknown));
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
