using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Infocodes.Properties;

namespace LSOne.ViewPlugins.Infocodes.Views
{
    public partial class InfocodesView : ViewBase
    {
        private static Guid BarSettingID = new Guid("210D397F-D76A-471A-A76F-AB1DBC465E9C");
        private Setting searchBarSetting;
        private RecordIdentifier selectedID = "";
        private List<Row> displayedInfocodes;
        private List<object> typeList;

        public InfocodesView(RecordIdentifier infocodeID)
            : this()
        {
            selectedID = infocodeID;
        }

        public InfocodesView()
        {
            InitializeComponent();

            lvInfocodes.ContextMenuStrip = new ContextMenuStrip();
            lvInfocodes.ContextMenuStrip.Opening += lvGroups_Opening;

            lvSubcodes.ContextMenuStrip = new ContextMenuStrip();
            lvSubcodes.ContextMenuStrip.Opening += lvItems_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);

            HeaderText = Properties.Resources.Infocodes;
            searchBar.BuddyControl = lvInfocodes;
            selectedID = RecordIdentifier.Empty;

        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Infocode;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Infocodes", RecordIdentifier.Empty, Properties.Resources.Infocodes, false));
            contexts.Add(new AuditDescriptor("InformationSubCodes", RecordIdentifier.Empty, Properties.Resources.SubCodes, false));
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvInfocodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right click here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodeList", lvInfocodes.ContextMenuStrip, lvInfocodes);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvSubcodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right click here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEditItem_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtonsItems.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAddItem_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtonsItems.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemoveItem_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtonsItems.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodeSubcodeList", lvSubcodes.ContextMenuStrip, lvSubcodes);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadItems();
        }

        private string GetTypeText(InputTypesEnum type)
        {
            switch (type)
            {
                case InputTypesEnum.Numeric:
                    return Resources.Numeric;
                case InputTypesEnum.Text:
                    return Resources.Text;
                case InputTypesEnum.Date:
                    return Resources.Date;
                case InputTypesEnum.Item:
                    return Resources.ItemSelect;
                case InputTypesEnum.Customer:
                    return Resources.CustomerSelec;
                case InputTypesEnum.AgeLimit:
                    return Resources.AgeLimit;
                case InputTypesEnum.SubCodeList:
                    return Resources.SelectionList;
                case InputTypesEnum.SubCodeButtons:
                    return Resources.SelectionButtons;
                default:
                    return Resources.Text;
            }
        }

        private string GetTriggerFunctions(TriggerFunctions triggerFunction)
        {
            switch (triggerFunction)
            {
                case TriggerFunctions.None:
                    return Resources.None;
                case TriggerFunctions.Item:
                    return Resources.Item;
                case TriggerFunctions.Infocode:
                    return Resources.Infocode;
                case TriggerFunctions.TaxGroup:
                    return Resources.TaxGroup;
                default:
                    return Resources.None;
            }
        }

        private void LoadItems()
        {
            displayedInfocodes = new List<Row>();
            lvInfocodes.ClearRows();

            List<Infocode> infocodes = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new InputTypesEnum[] { InputTypesEnum.Group }, false, InfocodeSorting.InfocodeDescription, false, RefTableEnum.All);

            Row row;
            foreach (Infocode infocode in infocodes)
            {
                row = new Row();
                row.AddText(infocode.Text);
                row.AddText(GetTypeText(infocode.InputType));
                row.Tag = infocode;

                displayedInfocodes.Add(row);
            }
            Search();
        }

        private void Search()
        {
            lvInfocodes.ClearRows();
            bool addItem;
            
            List<SearchParameterResult> results = searchBar.SearchParameterResults;
            foreach (Row row in displayedInfocodes)
            {
                addItem = true;
                foreach (SearchParameterResult result in results)
                {
                    switch (result.ParameterKey)
                    {
                        case "Infocode name":
                            if (result.StringValue != "")
                            {
                                addItem = false;
                                if (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith)
                                {
                                    if ((string.Compare(row[0].Text.Left(result.StringValue.Length), result.StringValue, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)) == 0)
                                    {
                                        addItem = true;
                                    }
                                }
                                else
                                {
                                    if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(row[0].Text, result.StringValue, CompareOptions.IgnoreCase) >= 0)
                                    {
                                        addItem = true;
                                    }
                                }
                            }
                            break;
                        case "Infocode type":
                            addItem = false;
                            switch (result.ComboSelectedIndex)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    if ((string.Compare(row[1].Text.Left(((string)typeList[result.ComboSelectedIndex]).Length), (string)typeList[result.ComboSelectedIndex], CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)) == 0)
                                    {
                                        addItem = true;
                                    }
                                    break;
                                default:
                                    addItem = true;
                                    break;
                            }
                            break;
                    }
                }
                if (addItem)
                {
                    lvInfocodes.AddRow(row);
                    Infocode infocodeRow = (Infocode) row.Tag;
                    if (infocodeRow != null && selectedID == infocodeRow.ID)
                    {
                        lvInfocodes.Selection.AddRows(lvInfocodes.RowCount-1, lvInfocodes.RowCount-1);
                    }
                }
            }
            lvInfocodes_SelectionChanged(this, EventArgs.Empty);
            lvInfocodes.AutoSizeColumns();
            lvInfocodes.ShowRowOnScreen = true;
        }


        private void LoadSubcodes()
        {
            if (lvInfocodes.RowCount == 0)
            {
                return;
            }

            if (SelectedInfocodeID != RecordIdentifier.Empty)
            {
                List<InfocodeSubcode> infocodeSubcodes = Providers.InfocodeSubcodeData.GetListForInfocode(
                    PluginEntry.DataModel,
                    SelectedInfocodeID,
                    InfocodeSubcodeSorting.Description,
                    false);

                lvSubcodes.ClearRows();

                Row row;

                foreach (InfocodeSubcode infocodeSubcode in infocodeSubcodes)
                {
                    row = new Row();
                    row.AddText(infocodeSubcode.Text);
                    row.AddText(infocodeSubcode.ItemName);
                    row.AddText(infocodeSubcode.VariantDescription);
                    row.AddText(GetTriggerFunctions(infocodeSubcode.TriggerFunction));
                    row.AddText(infocodeSubcode.TriggerCode.ToString());
                    row.Tag = infocodeSubcode;

                    lvSubcodes.AddRow(row);
                }
                lvSubcodes.AutoSizeColumns();

                lvSubcodes_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private RecordIdentifier SelectedInfocodeID
        {
            get { return (lvInfocodes.Selection.Count > 0) ? ((Infocode)lvInfocodes.Selection[0].Tag).ID : RecordIdentifier.Empty; }
        }

        private RecordIdentifier SelectedSubcode
        {
            get { return ((InfocodeSubcode)lvSubcodes.Selection[0].Tag).ID; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewInfocode(UsageCategoriesEnum.None);
            LoadItems();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (lvInfocodes.Selection.Count > 0)
            {
                PluginOperations.NewSubcode(SelectedInfocodeID);
                LoadSubcodes();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowInfocode(SelectedInfocodeID);
            LoadItems();
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (lvSubcodes.Selection.Count > 0)
            {
                PluginOperations.ShowSubcode(SelectedSubcode);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteInfocodeQuestion,
                Properties.Resources.DeleteInfocodeCaption) == DialogResult.Yes)
            {
                Providers.InfocodeData.Delete(PluginEntry.DataModel, SelectedInfocodeID);
                LoadItems();
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lvSubcodes.Selection.Count > 0)
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteInfocodeSubcodeQuestion,
                    Properties.Resources.DeleteInfocodeSubcodeCaption) == DialogResult.Yes)
                {
                    Providers.InfocodeSubcodeData.Delete(PluginEntry.DataModel, SelectedSubcode);
                    LoadSubcodes();
                }
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.EditGroups, ShowCrossAndModifierInfocodes), 500);
            }
        }

        private void ShowCrossAndModifierInfocodes(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CrossAndModifierInfocodesView(UsageCategoriesEnum.None));
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Infocode":
                    LoadItems();
                    break;
                case "Subcode":
                    LoadSubcodes();
                    break;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvSubcodes_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.EditButtonEnabled = (lvSubcodes.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView);
            btnsContextButtonsItems.RemoveButtonEnabled = (lvSubcodes.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);

            btnsContextButtonsItems.AddButtonEnabled =
                PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
        }

        private void lvInfocodes_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvInfocodes.Selection.Count > 0) ? ((Infocode)lvInfocodes.Selection[0].Tag).ID : selectedID;

            btnsContextButtons.EditButtonEnabled = (lvInfocodes.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView);
            btnsContextButtons.RemoveButtonEnabled = (lvInfocodes.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);


            if (lvInfocodes.Selection.Count > 0)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvSubcodes.Visible = true;
                    btnsContextButtonsItems.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadSubcodes();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvSubcodes.Visible = false;
                btnsContextButtonsItems.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void lvSubcodes_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            PluginOperations.ShowSubcode(SelectedSubcode);

        }

        private void lvInfocodes_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            PluginOperations.ShowInfocode(SelectedInfocodeID);
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            Search();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            typeList = new List<object>();
            typeList.Add(Resources.AllTypes);
            typeList.Add(Resources.Numeric);
            typeList.Add(Resources.Text);
            typeList.Add(Resources.Date);
            typeList.Add(Resources.ItemSelect);
            typeList.Add(Resources.CustomerSelec);
            typeList.Add(Resources.AgeLimit);
            typeList.Add(Resources.SelectionList);
            typeList.Add(Resources.SelectionButtons);

            searchBar.AddCondition(new ConditionType(Resources.InfocodeName, "Infocode name", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.InfocodeType, "Infocode type", ConditionType.ConditionTypeEnum.ComboBox, typeList, 0, 0, false));

            searchBar_LoadDefault(this, EventArgs.Empty);
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

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }
    }
}
