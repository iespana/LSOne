using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class StoreTransferReceivingItemDialog : DialogBase
    {
        private DecimalLimit quantityLimiter;
        private InventoryTransferOrderLine inventoryTransferOrderLine;

        private SiteServiceProfile siteServiceProfile;
        private IInventoryService inventoryService;

        public StoreTransferReceivingItemDialog(InventoryTransferOrderLine orderLine)
        {
            InitializeComponent();

            quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
            siteServiceProfile = PluginOperations.GetSiteServiceProfile();
            OrderLine = orderLine;

            SetQuantityAllowDecimals(orderLine.QuantityReceived);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public InventoryTransferOrderLine OrderLine
        {
            get { return inventoryTransferOrderLine; }
            set
            {
                inventoryTransferOrderLine = value;
                PopulateOrderLine();
            }
        }

        public bool ReceiveAnother
        {
            get
            {
                return chkReceiveAnother.Checked;
            }
        }

        private void PopulateOrderLine()
        {
            RetailItem retailItem = PluginOperations.GetRetailItem(inventoryTransferOrderLine.ItemId);

            BarCode bc = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, inventoryTransferOrderLine.ItemId, CacheType.CacheTypeApplicationLifeTime);

            if(bc != null)
            {
                txtBarcode.Text = (string)bc.ID;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(retailItem.ID, retailItem.Text);
            }
            else
            {
                RetailItem headerItem = PluginOperations.GetRetailItem(retailItem.HeaderItemID);
                if (headerItem == null)
                {
                    return;
                }

                cmbItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                cmbVariant.SelectedData = new DataEntity(
                                        inventoryTransferOrderLine.ItemId,
                                        string.IsNullOrEmpty(inventoryTransferOrderLine.VariantName) ? retailItem.VariantName : inventoryTransferOrderLine.VariantName);
            }
            
            cmbUnit.SelectedData = new DataEntity(inventoryTransferOrderLine.UnitId, inventoryTransferOrderLine.UnitName);

            ntbSendingQuantity.SetValueWithLimit(inventoryTransferOrderLine.QuantitySent, quantityLimiter);
            ntbReceivingQuantity.SetValueWithLimit(inventoryTransferOrderLine.QuantityReceived, quantityLimiter);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(inventoryService == null)
            {
                inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            }

            try
            {
                SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;

                if (inventoryTransferOrderLine.QuantityReceived != (decimal)ntbReceivingQuantity.Value) //Save if the value was changed
                {
                    inventoryTransferOrderLine.QuantityReceived = (decimal)ntbReceivingQuantity.Value;
                    RecordIdentifier newLineID = RecordIdentifier.Empty;
                    result = inventoryService.SaveInventoryTransferOrderLine(PluginEntry.DataModel, siteServiceProfile, inventoryTransferOrderLine, true, out newLineID, true);                    

                    if (result != SaveTransferOrderLineResult.Success)
                    {
                        PluginOperations.ShowInventoryTransferLineErrorResultMessage(result, true);
                    }
                }

                DialogResult = result == SaveTransferOrderLineResult.Success ? DialogResult.OK : DialogResult.Cancel;
                Close();
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ntbReceivingQuantity_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = inventoryTransferOrderLine.QuantityReceived != (decimal)ntbReceivingQuantity.Value;
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbReceivingQuantity.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}
