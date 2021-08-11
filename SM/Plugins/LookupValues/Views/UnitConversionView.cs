using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class UnitConversionView : ViewBase
    {

        private RecordIdentifier selectedID = "";

        public UnitConversionView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
            cmbShowConversionsFor.SelectedIndex = 1;

            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, selectedID);
            cmbItem.SelectedData = new DataEntity(item.ID, item.Text);

            LoadItems();
        }

        public UnitConversionView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvGroups_Opening;

            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageUnits);

            cmbShowConversionsFor.SelectedIndex = 0;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UnitConversion", RecordIdentifier.Empty, Properties.Resources.UnitConversions, false));
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Units, new ContextbarClickEventHandler(PluginOperations.ShowUnitsView)), 10);
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.UnitConversions;
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
            if (isRevert)
            {
                
            }

            HeaderText = Properties.Resources.UnitConversions;
            //HeaderIcon = Properties.Resources.UnitConversion16;

            LoadItems();

            
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.UnitConversionDialog dlg = PluginOperations.NewUnitConversionDetailed(cmbShowConversionsFor.SelectedIndex == 1 ? (DataEntity)cmbItem.SelectedData : null, "", "");

            //if (dlg != null)
            //{
            //    // 0 means we are dealing with all items
            //    if ((string)selectedID.PrimaryID == "")
            //    {
            //        cmbShowConversionsFor.SelectedIndex = 0;
            //    }
            //    else
            //    {
            //        cmbShowConversionsFor.SelectedIndex = 1;
            //        cmbItem.SelectedData = dlg.Item;
            //    }
            //}
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Dialogs.UnitConversionDialog dlg = new Dialogs.UnitConversionDialog((RecordIdentifier)lvItems.SelectedItems[0].Tag);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteUnitConversionRuleQuestion,
                Properties.Resources.DeleteUnitConversionRule) == DialogResult.Yes)
            {
                Providers.UnitConversionData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvItems.SelectedItems[0].Tag);

                LoadItems();
            }

        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "UnitConversions":
                    if (changeHint == DataEntityChangeType.Add)
                    {
                        // 0 means we are dealing with all items
                        if (changeIdentifier.PrimaryID == "")
                        {
                            cmbShowConversionsFor.SelectedIndex = 0;
                        }
                        else
                        {
                            cmbShowConversionsFor.SelectedIndex = 1;
                            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, changeIdentifier.PrimaryID);
                            cmbItem.SelectedData = new DataEntity(item.ID, item.Text);
                        }
                    }
                    LoadItems();
                    break;
            }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvItems.SelectedItems.Count > 0) ? (RecordIdentifier)lvItems.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = (lvItems.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.ManageUnits);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count != 0)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
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

            PluginEntry.Framework.ContextMenuNotify("UnitConversionsList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadItems()
        {
            List<UnitConversion> unitConversions;
            ListViewItem item;

            lvItems.Items.Clear();

            // 0 means all items, 1 means a specific item
            if (cmbShowConversionsFor.SelectedIndex == 0)
            {
                unitConversions = Providers.UnitConversionData.GetUnitConversions(PluginEntry.DataModel, lvItems.SortColumn, lvItems.SortedBackwards);
            }
            else
            {
                unitConversions = Providers.UnitConversionData.GetUnitConversions(PluginEntry.DataModel, cmbItem.SelectedData.ID, lvItems.SortColumn, lvItems.SortedBackwards);
            }

            

            foreach (var unitConversion in unitConversions)
            {
                item = new ListViewItem(unitConversion.ItemID == "" ? Properties.Resources.GlobalRule : unitConversion.ItemName);
                item.SubItems.Add(unitConversion.FromUnitName);
                item.SubItems.Add(unitConversion.ToUnitName);
                item.SubItems.Add(unitConversion.Factor.ToString("F2"));
                item.Tag = unitConversion.ID;
                item.ImageIndex = -1;

                lvItems.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);

            lvItems.BestFitColumns();

            lvGroups_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                if (lvItems.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;

                    lvItems.SortColumn = e.Column;
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems();
        }

        private void cmbItem_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
            PluginEntry.DataModel,
            false,
            initialSearchText,
            SearchTypeEnum.RetailItems,
            textInitallyHighlighted);
        }

        private void cmbShowConversionsFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            // All items
            if (cmbShowConversionsFor.SelectedIndex == 0)
            {
                lblItem.Enabled = false;
                cmbItem.Enabled = false;

                cmbItem.SelectedData = new DataEntity("", "");

                LoadItems();
            }
            // Specific item
            else if (cmbShowConversionsFor.SelectedIndex == 1)
            {
                lblItem.Enabled = true;
                cmbItem.Enabled = true;

                lvItems.Items.Clear();
            }
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void cmbItem_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbItem_TextChanged(object sender, EventArgs e)
        {
            //cmbItem.ShowDropDown();
            //string key = e.KeyChar.ToString();
            //cmbItem_DropDown(key, null);
        }

        private void cmbItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            
        }

        //private void cmbItem_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.SuppressKeyPress = true;
        //    cmbItem.ShowDropDown(((char)e.KeyValue).ToString());
        //}

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;

            base.OnClose();
        }
    }
}
