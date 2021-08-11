using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class PurchaseOrderSettingsPage : UserControl, ITabView
    {
        private RecordIdentifier purchaseOrderID;
        private PurchaseOrder purchaseOrder;
        private PurchaseStatusEnum orgPurchaseOrderStatus;

        private bool lockWhileLoading;


        private PurchaseOrderSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PurchaseOrderSettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            lockWhileLoading = true;

            purchaseOrderID = context;
            purchaseOrder = (PurchaseOrder)internalContext;
            orgPurchaseOrderStatus = purchaseOrder.PurchaseStatus;
            
            tbStatus.Text = purchaseOrder.PurchaseStatusText;
            cmbVendor.SelectedData = new DataEntity(purchaseOrder.VendorID, purchaseOrder.VendorName);

            cmbStore.SelectedData = new DataEntity(purchaseOrder.StoreID, purchaseOrder.StoreName);

            cmbCurrency.SelectedData = new DataEntity(purchaseOrder.CurrencyCode, purchaseOrder.CurrencyDescription);
            dtpDeliveryDate.Value = purchaseOrder.DeliveryDate;
            purchaseOrder.OrderingDate.ToDateControl(dtpOrderingDate);

            ntbDiscountAmount.Value = (double)purchaseOrder.DefaultDiscountAmount;
            ntbDiscountPercentage.Value = (double)purchaseOrder.DefaultDiscountPercentage;

            tbDescription.Text = purchaseOrder.Description;
            tbPurchaseOrderID.Text = (string)purchaseOrder.PurchaseOrderID;

            SetFieldsEnabled();

            lockWhileLoading = false;
        }

        private void SetFieldsEnabled()
        {
            cmbStore.Enabled = false;
            cmbVendor.Enabled = false;
            ntbDiscountAmount.Enabled = purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open;
            ntbDiscountPercentage.Enabled = purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open;
            tbDescription.Enabled = purchaseOrder.PurchaseStatus == PurchaseStatusEnum.Open;
        }

        public bool DataIsModified()
        {
            if (dtpDeliveryDate.Value != purchaseOrder.DeliveryDate) return true;
            if (Date.FromDateControl(dtpOrderingDate) != purchaseOrder.OrderingDate) return true;
            if (cmbCurrency.SelectedData.ID != purchaseOrder.CurrencyCode) return true;
            if (ntbDiscountAmount.Value != (double)purchaseOrder.DefaultDiscountAmount) return true;
            if (ntbDiscountPercentage.Value != (double)purchaseOrder.DefaultDiscountPercentage) return true;
            if (!tbDescription.Text.Equals(purchaseOrder.Description, StringComparison.CurrentCultureIgnoreCase)) return true;

            return false;
        }

        public bool SaveData()
        {
            purchaseOrder.DeliveryDate = dtpDeliveryDate.Value;
            purchaseOrder.OrderingDate = Date.FromDateControl(dtpOrderingDate);
            purchaseOrder.CurrencyCode = cmbCurrency.SelectedData.ID;
            purchaseOrder.DefaultDiscountAmount = (decimal)ntbDiscountAmount.Value;
            purchaseOrder.DefaultDiscountPercentage = (decimal)ntbDiscountPercentage.Value;
            purchaseOrder.Description = tbDescription.Text;

            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseOrder" && changeIdentifier == purchaseOrderID)
            {
                PurchaseOrder changedOrder = ((PurchaseOrder) param);
                purchaseOrder.HasLines = changedOrder != null ? changedOrder.HasLines : purchaseOrder.HasLines;
            }
            else if (objectName == "PurchaseOrderStatus" && changeIdentifier.PrimaryID == purchaseOrder.PurchaseOrderID)
            {
                purchaseOrder.PurchaseStatus = (PurchaseStatusEnum)(int)param;
                tbStatus.Text = purchaseOrder.PurchaseStatusText;
                SetFieldsEnabled();
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        #endregion

        private void ntbDiscountPercentage_Leave(object sender, EventArgs e)
        {
            if (purchaseOrder.DefaultDiscountPercentage != (decimal)ntbDiscountPercentage.Value)
            {
                purchaseOrder.Dirty = true;
                purchaseOrder.DefaultDiscountPercentage = (decimal)ntbDiscountPercentage.Value;
                
                if (!lockWhileLoading && purchaseOrder.HasLines)
                {
                    if (QuestionDialog.Show(Properties.Resources.UpdateExistingDiscountQuestion, Properties.Resources.UpdateExistingDiscount) == DialogResult.Yes)
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).ChangeDiscountsForPurchaseOrderLines(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderID,
                            purchaseOrder.StoreID,
                            (decimal) ntbDiscountPercentage.Value,
                            null,
                            true);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderChangeDiscount", purchaseOrderID, null);
                }
            }
        }

        private void ntbDiscountAmount_Leave(object sender, EventArgs e)
        {
            if (purchaseOrder.DefaultDiscountAmount != (decimal)ntbDiscountAmount.Value)
            {
                purchaseOrder.Dirty = true;
                purchaseOrder.DefaultDiscountAmount = (decimal)ntbDiscountAmount.Value;
                
                if (!lockWhileLoading && purchaseOrder.HasLines)
                {
                    var dlgResult = QuestionDialog.Show(Properties.Resources.UpdateExistingDiscountQuestion, Properties.Resources.UpdateExistingDiscount);
                    if (dlgResult == DialogResult.Yes)
                    {                
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).ChangeDiscountsForPurchaseOrderLines(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderID,
                            purchaseOrder.StoreID,
                            null,
                            (decimal)ntbDiscountAmount.Value,
                            true);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderChangeDiscount", purchaseOrderID, null);
                }
            }
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        //private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (lockWhileLoading)
        //    {
        //        return;
        //    }
        //    purchaseOrder.PurchaseStatus = (PurchaseStatusEnum) cmbStatus.SelectedIndex;
        //    SetFieldsEnabled();
        //    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderStatus", purchaseOrder.ID, purchaseOrder.PurchaseStatus);
        //    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
        //}
    }
}
