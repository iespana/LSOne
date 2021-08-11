using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.BarCodes.Views
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

        public override RecordIdentifier ID
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
            LoadItems();
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
                    LoadItems();
                    break;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.ManageBarCodeMasks,
                        new ContextbarClickEventHandler(PluginOperations.ShowBarCodeMaskSetup)), 300);
                }
            }
            else if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), new ContextbarClickEventHandler(btnAdd_Click)), 10);
            }
        }

        private void LoadItems()
        {
            List<BarCodeSetup> setups;

            lvItems.ClearRows();

            setups = Providers.BarCodeSetupData.GetBarcodes(PluginEntry.DataModel);
            Row row;
            foreach (BarCodeSetup setup in setups)
            {
                row = new Row();
                row.AddText((string)setup.ID);
                row.AddText(setup.Description);
                row.AddText(Datalayer.DataEntities.BarCodeTypes.BarCodeType.GetBarCodeType(setup.BarCodeType).Name);
                row.AddText(setup.FontName);
                row.AddText(setup.FontSize.ToString());
                row.AddText(setup.BarCodeMaskDescription);
                row.AddText(setup.BarCodeMask);

                row.Tag = setup.ID;

                lvItems.AddRow(row);
            }


            lvItems_SelectionChanged(this, EventArgs.Empty);

            lvItems.AutoSizeColumns();

            lvItems.Columns[0].Width = - 2;
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowBarCodeSetupEditor((RecordIdentifier)lvItems.Selection[0].Tag);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteBarCodeSetup((RecordIdentifier)lvItems.Selection[0].Tag);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewBarCodeSetup();
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("BarCodeSetupList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = (lvItems.Selection.Count > 0);
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
