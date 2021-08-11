using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Infocodes.Properties;

namespace LSOne.ViewPlugins.Infocodes.Views
{
    public partial class CrossAndModifierInfocodesView : ViewBase
    {
        private RecordIdentifier selectedID = "";
        private UsageCategoriesEnum activeUsageCategory;
        private bool firstload;

        public CrossAndModifierInfocodesView(UsageCategoriesEnum usageCategory)
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Close |
                         ViewAttributes.Help;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;

            lvGroupSubcodes.ContextMenuStrip = new ContextMenuStrip();
            lvGroupSubcodes.ContextMenuStrip.Opening += lvItems_Opening;

            lvGroups.SmallImageList = PluginEntry.Framework.GetImageList();
            lvGroupSubcodes.SmallImageList = PluginEntry.Framework.GetImageList();

            ReadOnly = !PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            firstload = true;
            this.activeUsageCategory = usageCategory;
            switch (usageCategory)
            {
                case UsageCategoriesEnum.CrossSelling: 
                    cmbInfocodeGroupType.SelectedIndex = 1;
                    HeaderText = Properties.Resources.CrossSellInfocodeGroups;
                    break;
                case UsageCategoriesEnum.ItemModifier: 
                    cmbInfocodeGroupType.SelectedIndex = 2;
                    HeaderText = Properties.Resources.ItemModifierGroups;
                    break;
                default: cmbInfocodeGroupType.SelectedIndex = 0; break;
            }

            //HeaderIcon = Properties.Resources.InfoCodes16;

            lvGroups.Columns[0].Tag = InfocodeSorting.InfocodeID;

            lvGroupSubcodes.Columns[0].Tag = InfocodeSubcodeSorting.TriggerCode;
            lvGroupSubcodes.Columns[1].Tag = InfocodeSubcodeSorting.ListType;
            lvGroupSubcodes.Columns[2].Tag = InfocodeSubcodeSorting.Description;
            lvGroupSubcodes.Columns[3].Tag = InfocodeSubcodeSorting.Prompt;

        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("InfocodeGroupsByUsageCategory", new RecordIdentifier((int)activeUsageCategory), Properties.Resources.Infocodes, false));
            contexts.Add(new AuditDescriptor("InformationSubCodes", RecordIdentifier.Empty, Properties.Resources.SubCodes, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                switch (activeUsageCategory)
                {
                    case UsageCategoriesEnum.CrossSelling: return Resources.CrossSellInfocodeGroups;
                    case UsageCategoriesEnum.ItemModifier: return Resources.ItemModifierGroups;
                    case UsageCategoriesEnum.None: return Resources.None;
                    default: return "";
                }
            }
        }

        private string GetCategoryText(UsageCategoriesEnum category)
        {
            switch (category)
            {
                case UsageCategoriesEnum.CrossSelling: return Resources.CrossSelling;
                case UsageCategoriesEnum.ItemModifier: return Resources.ItemModifiers;
                case UsageCategoriesEnum.None: return Resources.None;
                default: return "";
            }
        }

        private string GetTriggerText(TriggeringEnum trigger)
        {
            switch (trigger)
            {
                case TriggeringEnum.Automatic: return Resources.Automatic;
                case TriggeringEnum.Manual: return Resources.Manual;
                default: return "";
            }
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvGroups.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("InfocodeList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ShowTargetInfocode(object sender, EventArgs args)
        {
            InfocodeSubcode subCode = Providers.InfocodeSubcodeData.Get(PluginEntry.DataModel, SelectedItemID);
            PluginOperations.ShowInfocodes(subCode.TriggerCode);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "GroupSubcode":
                    LoadLines();
                    break;
                case "Infocode":
                    LoadItems(InfocodeSorting.InfocodeID,false, true, changeIdentifier.PrimaryID);
                    break;

            }
        }    

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvGroupSubcodes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right click here
            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAddItem_Click);
            item.Enabled = btnsContextButtonsItems.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemoveItem_Click);
            item.Enabled = btnsContextButtonsItems.RemoveButtonEnabled;
            item.Image = ContextButtons.GetRemoveButtonImage();
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                Properties.Resources.ViewTargetInfocode,
                500,
                ShowTargetInfocode);

            item.Enabled = lvGroupSubcodes.SelectedItems.Count > 0;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InfocodeSubcodeList", lvGroupSubcodes.ContextMenuStrip, lvGroupSubcodes);

            e.Cancel = (menu.Items.Count == 0);
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
            LoadItems(InfocodeSorting.InfocodeID, false, true, selectedID);

        }

        private void LoadItems(InfocodeSorting sortBy, bool backwards, bool doBestFit, RecordIdentifier idToSelect)
        {
            List<Infocode> items;

            if (activeUsageCategory == UsageCategoriesEnum.None)
            {
                items = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new InputTypesEnum[] { InputTypesEnum.Group }, sortBy, backwards,RefTableEnum.All);
            }
            else
            {
                items = Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategoriesEnum[] { activeUsageCategory }, new InputTypesEnum[] { InputTypesEnum.Group }, sortBy, backwards,RefTableEnum.All);
            }
            

            lvGroups.Items.Clear();
            foreach (Infocode item in items)
            {
                ListViewItem listItem = new ListViewItem((string)item.ID);
                listItem.SubItems.Add(GetCategoryText(item.UsageCategory));
                listItem.SubItems.Add(item.Text);
                listItem.SubItems.Add(item.ExplanatoryHeaderText);
                listItem.SubItems.Add(GetTriggerText(item.Triggering));
                listItem.SubItems.Add("");//TODO OK Pressed Action
                listItem.SubItems.Add(item.MinSelection.ToString());
                listItem.SubItems.Add(item.MaxSelection.ToString());
                listItem.SubItems.Add(""); //TODO LinkedInfoCode
                listItem.Tag = item.ID;
                listItem.ImageIndex = -1;

                lvGroups.Add(listItem);

                if (idToSelect == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;                   
                }
            }

            lvGroups.Columns[lvGroups.SortColumn].ImageIndex = (backwards ? 1 : 0);                

            if (doBestFit)
            {
                lvGroups.BestFitColumns();
            }

            lvGroups_SelectedIndexChanged(this, EventArgs.Empty);
            firstload = false;
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = (lvGroups.SelectedItems.Count != 0) && !ReadOnly;
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;

            if (lvGroups.SelectedItems.Count > 0)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvGroupSubcodes.Visible = true;
                    btnsContextButtonsItems.Visible = true;
                    btnViewTargetInfocode.Visible = true;
                    lblNoSelection.Visible = false;
                }

                btnsContextButtons.AddButtonEnabled = !ReadOnly;

                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvGroupSubcodes.Visible = false;
                btnsContextButtonsItems.Visible = false;
                btnViewTargetInfocode.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void LoadLines()
        {
            if (lvGroups.SelectedItems.Count == 0)
            {
                return;
            }            

            if (SelectedGroupID != RecordIdentifier.Empty)
            {
                List<InfocodeSubcode> items = Providers.InfocodeSubcodeData.GetListForInfocode(
                    PluginEntry.DataModel, 
                    SelectedGroupID,
                    (InfocodeSubcodeSorting)lvGroupSubcodes.Columns[lvGroupSubcodes.SortColumn].Tag,
                    lvGroupSubcodes.SortedBackwards
                    );

                lvGroupSubcodes.Items.Clear();

                ListViewItem listItem = null;
                foreach (InfocodeSubcode item in items)
                {
                    listItem = new ListViewItem(item.TriggerCode.ToString());
                    listItem.SubItems.Add(GetCategoryText(item.UsageCategory));
                    listItem.SubItems.Add(item.Text);
                    listItem.SubItems.Add(item.InfocodePrompt);
                    listItem.Tag = new RecordIdentifier(item.InfocodeId, item.SubcodeId);
                    listItem.ImageIndex = -1;

                    lvGroupSubcodes.Add(listItem);
                }

                lvGroupSubcodes.BestFitColumns();

                lvGroupSubcodes_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private RecordIdentifier SelectedGroupID
        {
            get { return (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : RecordIdentifier.Empty; }
        }

        private RecordIdentifier SelectedItemID
        {
            get { return (RecordIdentifier)lvGroupSubcodes.SelectedItems[0].Tag; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewInfocodeGroup(activeUsageCategory);
            LoadItems(InfocodeSorting.InfocodeID, lvGroups.SortedBackwards, true, selectedID);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (lvGroups.SelectedItems.Count > 0)
            {
                PluginOperations.NewGroupToInfocodeListConnection(SelectedGroupID);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowGroupInfocode((RecordIdentifier)SelectedGroupID);
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteInfocodeGroupQuestion,
                Properties.Resources.DeleteInfocodeCaption) == DialogResult.Yes)
            {
                Providers.InfocodeData.Delete(PluginEntry.DataModel, SelectedGroupID);
                LoadItems(InfocodeSorting.InfocodeID, lvGroups.SortedBackwards, true, selectedID);
            }
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lvGroupSubcodes.SelectedItems.Count > 0)
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteInfocodeSubcodeQuestion,
                    Properties.Resources.DeleteInfocodeSubcodeCaption) == DialogResult.Yes)
                {
                    Providers.InfocodeSubcodeData.Delete(PluginEntry.DataModel, SelectedItemID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "GroupSubcode", selectedID, null);

                }
            }
        }

        
        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            PluginOperations.ShowGroupInfocode((RecordIdentifier)SelectedGroupID);
        }

        private void lvGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // We only want to sort based on the first column.
            if (e.Column > 0)
            {
                return;
            }

            if (lvGroups.SortColumn == e.Column)
            {
                lvGroups.SortedBackwards = !lvGroups.SortedBackwards;
            }
            else
            {
                lvGroups.SortedBackwards = false;
            }

            if (lvGroups.SortColumn != -1)
            {
                lvGroups.Columns[lvGroups.SortColumn].ImageIndex = 2;
                lvGroups.SortColumn = e.Column;

            }

            LoadItems((InfocodeSorting)lvGroups.Columns[e.Column].Tag, lvGroups.SortedBackwards, true, selectedID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.EditInfocodes, new ContextbarClickEventHandler((ContextbarClickEventHandler)ShowInfocodesView)), 500);
            }
        }

        private void ShowInfocodesView(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.InfocodesView());
        }

        private void lvGroupSubcodes_Click(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = lvGroupSubcodes.SelectedItems.Count > 0 && !ReadOnly;
        }

        private void cmbInfocodeGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {

            activeUsageCategory = (UsageCategoriesEnum)cmbInfocodeGroupType.SelectedIndex;


            

            switch (activeUsageCategory)
            {
                case UsageCategoriesEnum.CrossSelling:
                    cmbInfocodeGroupType.SelectedIndex = 1;
                    HeaderText = Properties.Resources.CrossSellInfocodeGroups;
                    break;
                case UsageCategoriesEnum.ItemModifier:
                    cmbInfocodeGroupType.SelectedIndex = 2;
                    HeaderText = Properties.Resources.ItemModifierGroups;
                    break;
                case UsageCategoriesEnum.None:
                    cmbInfocodeGroupType.SelectedIndex = 0;
                    HeaderText = Properties.Resources.All;
                    break;

            }

            if (!firstload)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
            PluginEntry.Framework.ViewController.RefreshState();
            LoadItems(InfocodeSorting.InfocodeID, lvGroups.SortedBackwards, true, selectedID);

           
        }

        protected override void OnClose()
        {
            lvGroups.SmallImageList = null;
            lvGroupSubcodes.SmallImageList = null;

            base.OnClose();
        }

        private void lvGroupSubcodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewTargetInfocode.Enabled = lvGroupSubcodes.SelectedItems.Count > 0;
            btnsContextButtonsItems.RemoveButtonEnabled = lvGroupSubcodes.SelectedItems.Count > 0 && !ReadOnly;
            btnViewTargetInfocode.Enabled = btnsContextButtonsItems.RemoveButtonEnabled;
        }

        private void lvGroupSubcodes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvGroupSubcodes.SortColumn)
            {
                lvGroupSubcodes.SortedBackwards = !lvGroupSubcodes.SortedBackwards;
                lvGroupSubcodes.Columns[e.Column].ImageIndex = (lvGroupSubcodes.SortedBackwards) ? 1 : 0;
            }
            else
            {
                lvGroupSubcodes.Columns[lvGroupSubcodes.SortColumn].ImageIndex = 2;
                lvGroupSubcodes.Columns[e.Column].ImageIndex = 0;
                lvGroupSubcodes.SortColumn = e.Column;
                lvGroupSubcodes.SortedBackwards = false;
            }
            LoadLines();
        }

        private void btnViewTargetInfocode_Click(object sender, EventArgs e)
        {
            ShowTargetInfocode(sender, e);
        }
    }
}
