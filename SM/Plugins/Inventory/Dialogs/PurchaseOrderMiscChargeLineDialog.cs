using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class PurchaseOrderMiscChargeLineDialog : DialogBase
    {
        PurchaseOrder purchaseOrder;
        PurchaseOrderMiscCharges miscCharge;

        // Edit purchase order misc charge
        public PurchaseOrderMiscChargeLineDialog(PurchaseOrder purchaseOrder, PurchaseOrderMiscCharges purchaseOrderMiscCharge)
            : this()
        {
            miscCharge = purchaseOrderMiscCharge ?? new PurchaseOrderMiscCharges(purchaseOrder.PurchaseOrderID);
            this.purchaseOrder = purchaseOrder;

            this.miscCharge.PriceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            tbPurchaseID.Text = (string)purchaseOrder.PurchaseOrderID;
            ntbAmount.SetValueWithLimit(miscCharge.Amount, miscCharge.PriceLimiter);
            cmbType.SelectedIndex = (int)miscCharge.Type;
            tbReason.Text = miscCharge.Reason;
            cmbSalesTaxGroup.SelectedData = miscCharge.TaxGroupID != RecordIdentifier.Empty ? Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, miscCharge.TaxGroupID) : new DataEntity();
        }

        private PurchaseOrderMiscChargeLineDialog()
        {
            InitializeComponent();

            cmbSalesTaxGroup.SelectedData = new DataEntity();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            try
            {
                miscCharge.Type = (PurchaseOrderMiscCharges.PurchaseOrderMiscChargesEnum) cmbType.SelectedIndex;
                miscCharge.Reason = (string) tbReason.Text;
                miscCharge.Amount = (decimal) ntbAmount.Value;
                miscCharge.TaxGroupID = cmbSalesTaxGroup.SelectedData == null ? RecordIdentifier.Empty : cmbSalesTaxGroup.SelectedData.ID;

                RecordIdentifier vendorsSalesTaxGroupID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorsSalesTaxGroupID(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrder.VendorID,
                    true);

                miscCharge.TaxAmount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculateTaxBetweenSalesTaxGroups(
                    PluginEntry.DataModel,
                    (decimal) ntbAmount.Value,
                    vendorsSalesTaxGroupID,
                    miscCharge.TaxGroupID);

                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SavePurchaseOrderMiscCharge(
                    PluginEntry.DataModel, 
                    PluginOperations.GetSiteServiceProfile(),
                    miscCharge, 
                    true);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKBtnEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (ntbAmount.Value > 0);
        }

        private void cmbItemSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel),null);
        }

        private void cmbItemSalesTaxGroup_RequestClear(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SelectedData = new DataEntity();
        }

    }
}
