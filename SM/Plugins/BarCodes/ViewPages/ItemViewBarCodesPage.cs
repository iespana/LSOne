using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.BarCodes.ViewPages
{
    internal partial class ItemViewBarCodesPage : UserControl, ITabView
    {
        RetailItem item;
        bool sizeAllowed;
        bool colorAllowed;
        bool styleAllowed;
        private Image defaultImage;
        private RecordIdentifier selectedID;

        public ItemViewBarCodesPage(TabControl owner)
            : this()
        {
                        
        }

        public ItemViewBarCodesPage()
        {
            sizeAllowed = false;
            colorAllowed = false;
            styleAllowed = false;

            InitializeComponent();

            lvBarCodes.ContextMenuStrip = new ContextMenuStrip();
            lvBarCodes.ContextMenuStrip.Opening += new CancelEventHandler(lvBarCodes_Opening);

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) && PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes);
            btnEditBarCodeSetup.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageBarcodeSetup);
            defaultImage = Properties.Resources.CheckSymbol;

        }

        public static ITabView CreateInstance(object sender,  TabControl.Tab tab)
        {
            return new ViewPages.ItemViewBarCodesPage((TabControl)sender);
        }


        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            cmbBarCodeSetup.SelectedData = Providers.BarCodeSetupData.Get(PluginEntry.DataModel, item.BarCodeSetupID) ?? new DataEntity(RecordIdentifier.Empty, "");

            LoadItems();
        }

        public bool DataIsModified()
        {
            if (cmbBarCodeSetup.SelectedData.ID != item.BarCodeSetupID)
            {
                item.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            item.BarCodeSetupID = cmbBarCodeSetup.SelectedData.ID;

            return true;
        }

        public void ChangeFieldVisibility()
        {
            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("ItemBarCodes", RecordIdentifier.Empty, Properties.Resources.BarCodes, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "BarCode" || objectName == "RetailItem")
            {
                if (item.ItemType == ItemTypeEnum.MasterItem)
                {
                    LoadItems();
                }
                else if (changeIdentifier.SecondaryID == item.ID)
                {
                    LoadItems();
                }
            }
        }

        public void OnClose()
        {
            defaultImage = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LoadItems()
        {
            List<BarCode> barCodes;

            lvBarCodes.ClearRows();


            if (item.ItemType == ItemTypeEnum.MasterItem)
            {
                barCodes = Providers.BarCodeData.GetListForHeaderItem(PluginEntry.DataModel, item.MasterID);
            }
            else
            {
                barCodes = Providers.BarCodeData.GetList(PluginEntry.DataModel, item.ID, BarCodeSorting.ItemBarCode, false);
            }

            Row row;
            foreach (BarCode barCode in barCodes)
            {
                row = new Row();

                row.AddCell(barCode.ShowForItem ? new ExtendedCell((string)barCode.ItemBarCode, defaultImage) : new ExtendedCell((string)barCode.ItemBarCode) {ImagePlaceHolderWidth = defaultImage.Width});
                row.AddText((string)barCode.BarCodeSetupID == "" ? "" : (string)barCode.BarCodeSetupID + " - " + barCode.BarCodeSetupDescription);
                row.AddText((string)barCode.ItemID);
                row.AddText(barCode.ItemName);
                row.AddText(barCode.VariantName);
                row.AddCell(new NumericCell((string)barCode.Quantity.FormatTruncated(), barCode.Quantity));
                row.AddText(barCode.UnitDescription);
                row.Tag = barCode;                

                lvBarCodes.AddRow(row);

                if (selectedID == barCode.ID)
                {
                    lvBarCodes.Selection.Set(lvBarCodes.RowCount - 1);
                }
            }

            lvBarCodes.AutoSizeColumns();
        }
        
        private void cmbBarCodeSetup_RequestData(object sender, EventArgs e)
        {
            cmbBarCodeSetup.SetData(Providers.BarCodeSetupData.GetList(PluginEntry.DataModel),
                null);
        }

        void lvBarCodes_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvBarCodes.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("BarCodeList", lvBarCodes.ContextMenuStrip, lvBarCodes);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewBarCode(item.ID, item.MasterID, sizeAllowed, colorAllowed, styleAllowed, (DataEntity)cmbBarCodeSetup.SelectedData, lvBarCodes.Rows.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier barCodeID = ((BarCode)lvBarCodes.Row(lvBarCodes.Selection.FirstSelectedRow).Tag).ID;

            Dialogs.BarCodeDialog dlg = new Dialogs.BarCodeDialog(barCodeID);
            dlg.EditingBarcodeFromMasterItem = item.ItemType == ItemTypeEnum.MasterItem;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "BarCodeEdit",barCodeID, null);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RecordIdentifier barCodeID = ((BarCode)lvBarCodes.Row(lvBarCodes.Selection.FirstSelectedRow).Tag).ItemBarcodeID;

            if (QuestionDialog.Show(
                Properties.Resources.DeleteBarCodeQuestion,
                Properties.Resources.DeleteBarCode) == DialogResult.Yes)
            {
                Providers.BarCodeData.Delete(PluginEntry.DataModel, barCodeID);

                LoadItems();
            }
        }

        private void btnEditBarCodeSetup_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowBarCodeSetup(this, EventArgs.Empty);
        }

        private void lvBarCodes_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = lvBarCodes.Selection.Count > 0 ? ((BarCode)lvBarCodes.Row(lvBarCodes.Selection.FirstSelectedRow).Tag).ID : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                lvBarCodes.Selection.Count > 0
                && PluginEntry.DataModel.HasPermission(Permission.ItemsEdit)
                && PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes);
        }

        private void lvBarCodes_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
