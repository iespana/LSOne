using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.Controls;
using LSRetail.StoreController.SharedCore;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.BusinessObjects.BarCodes;
using LSRetail.StoreController.DataProviders.BarCodes;

namespace LSRetail.StoreController.BarCodes.Views
{
    public partial class BarCodeSetupView : ViewBase
    {
        public BarCodeSetupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;


            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            btnsContextButtons.AddButtonEnabled = !ReadOnly;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("BarCodeSetups", RecordIdentifier.Empty, Properties.Resources.BarCodeSetup, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.BarCodeSetup;
            }
        }

        public override Utilities.DataTypes.RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.BarCodeSetup;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {
                
            }



            HeaderText = Description;
            HeaderIcon = Properties.Resources.BarcodeImage;

            LoadItems(0, false);
            
        }

        protected override bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.

           

            return false;
        }

        protected override bool SaveData()
        {
            

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "BarCodeSetup":
                    LoadItems(lvItems.SortColumn, lvItems.SortedBackwards);
                    break;
            }
        }

        protected override void OnSetupContextBarItems(LSRetail.StoreController.SharedCore.EventArguments.ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarCodesMasks))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.ManageBarCodeMasks,
                        Properties.Resources.BarcodeImage,
                        new ContextbarClickEventHandler(PluginOperations.ShowBarCodeMaskSetup)), 300);
                }
            }
            else if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Add, btnsContextButtons.AddButtonImage, new ContextbarClickEventHandler(btnAdd_Click)), 10);
            }
        }

        private void LoadItems(int sortBy, bool backwards)
        {
            List<BarCodeSetup> setups;
            ListViewItem listItem;

            lvItems.Items.Clear();

            setups = Providers.BarCodeSetupData.GetBarcodes(PluginEntry.DataModel);

            foreach (BarCodeSetup setup in setups)
            {
                listItem = new ListViewItem((string)setup.ID);
                listItem.SubItems.Add(setup.Description);
                listItem.SubItems.Add(Datalayer.DataEntities.BarCodeTypes.BarCodeType.GetBarCodeType(setup.BarCodeType).Name);
                listItem.SubItems.Add(setup.FontName);
                listItem.SubItems.Add(setup.FontSize.ToString());
                listItem.SubItems.Add(setup.BarCodeMask);

                listItem.Tag = setup.ID;

                listItem.ImageIndex = -1;

                lvItems.Add(listItem);

                /*if (selectedID == (string)listItem.Tag)
                {
                    listItem.Selected = true;
                }*/
            }

            lvItems.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvItems.SortColumn = sortBy;

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);

            lvItems.BestFitColumns();

            lvItems.Columns[0].Width = - 2;
            
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = (lvItems.SelectedItems.Count > 0);
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
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
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems(e.Column, lvItems.SortedBackwards);
        }

        private void lvItems_SizeChanged(object sender, EventArgs e)
        {
            lvItems.Columns[lvItems.Columns.Count - 1].Width = -2;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowBarCodeSetupEditor((RecordIdentifier)lvItems.SelectedItems[0].Tag);
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteBarCodeSetup((RecordIdentifier)lvItems.SelectedItems[0].Tag);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewBarCodeSetup();
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuitem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuitem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));
            item.Image = btnsContextButtons.EditButtonImage;
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));
            item.Image = btnsContextButtons.AddButtonImage;
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));
            item.Image = btnsContextButtons.RemoveButtonImage;
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("BarCodeSetupList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;

            base.OnClose();
        }
        
    }
}
