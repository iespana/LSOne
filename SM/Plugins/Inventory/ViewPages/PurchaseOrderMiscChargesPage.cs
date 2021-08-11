using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class PurchaseOrderMiscChargesPage : UserControl, ITabView
    {
        RecordIdentifier purchaseOrderID;
        PurchaseOrder purchaseOrder;
        PurchaseStatusEnum purchaseOrderStatus;

        private PurchaseOrderMiscChargesPage()
        {
            InitializeComponent();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PurchaseOrderMiscChargesPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            purchaseOrderID = context;
            purchaseOrder = (PurchaseOrder)internalContext;

            purchaseOrderStatus = purchaseOrder.PurchaseStatus;

            LoadItems();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseOrderStatus" && changeIdentifier.PrimaryID == purchaseOrderID)
            {
                purchaseOrderStatus = (PurchaseStatusEnum)param;

                //This done to enable or disable the normal add and edit buttons
                lvItems_SelectionChanged(this, EventArgs.Empty);
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
            }
            else if (objectName == "PurchaseOrderTaxSettings")
            {
                LoadItems();
            }
        }

        public void OnClose()
        {
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //contexts.Add(new AuditDescriptor("PurchaseOrderMiscCharges", purchaseOrder.PurchaseOrderID, Properties.Resources.MiscCharges, false));
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void AddButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier taxGroupID = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, purchaseOrder.StoreID);

            PurchaseOrderMiscCharges miscCharged = new PurchaseOrderMiscCharges(purchaseOrderID);
            miscCharged.TaxGroupID = taxGroupID;

            Dialogs.PurchaseOrderMiscChargeLineDialog dlg = new Dialogs.PurchaseOrderMiscChargeLineDialog(purchaseOrder, miscCharged);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PurchaseOrderMiscCharges charge = (PurchaseOrderMiscCharges)lvItems.Rows[lvItems.Selection.FirstSelectedRow].Tag;

            Dialogs.PurchaseOrderMiscChargeLineDialog dlg = new Dialogs.PurchaseOrderMiscChargeLineDialog(purchaseOrder, charge);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderMiscCharges charge = (PurchaseOrderMiscCharges) lvItems.Rows[lvItems.Selection.FirstSelectedRow].Tag;

                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeletePurchaseOrderMiscCharges(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    charge.ID, true);

                LoadItems();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    100,
                    new EventHandler(AddButtonClicked));

            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    300,
                    new EventHandler(btnEdit_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    200,
                    new EventHandler(btnRemove_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PurchaseOrderMiscCharges", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadItems()
        {
            try
            {
                lvItems.ClearRows();

                List<PurchaseOrderMiscCharges> POMiscCharges = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetMischChargesForPurchaseOrder(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrder.PurchaseOrderID,
                    false,
                    true);

                if (lvItems.SortColumn == colType)
                {
                    POMiscCharges = POMiscCharges.OrderBy(o => o.TypeText).ToList();
                }

                Row row;

                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

                foreach (PurchaseOrderMiscCharges miscCharge in POMiscCharges)
                {
                    miscCharge.SetReportFormatting(priceLimiter);

                    row = new Row();

                    row.AddText(miscCharge.TypeText);
                    row.AddText(miscCharge.Reason);
                    row.AddCell(new NumericCell(miscCharge.FormattedAmount, miscCharge.Amount));
                    row.AddCell(new NumericCell(miscCharge.FormattedTaxAmount, miscCharge.TaxAmount));
                    row.AddText(miscCharge.TaxGroupName);
                    row.Tag = miscCharge;

                    lvItems.AddRow(row);
                }

                lvItems.Sort();
                lvItems.AutoSizeColumns();

                lvItems_SelectionChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled =
                (lvItems.Selection.Count != 0)
                && purchaseOrderStatus == PurchaseStatusEnum.Open
                && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);

            btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled;

            btnsEditAddRemove.AddButtonEnabled = (purchaseOrderStatus == PurchaseStatusEnum.Open);
        }
    }
}
