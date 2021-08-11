using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Properties;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class SetVendorDialog : DialogBase
    {
        RecordIdentifier selectedItemID;
        private RecordIdentifier selectedVendorID;
        private List<VendorItem> vendorItems;

        public SetVendorDialog(RecordIdentifier itemID)
        {
            InitializeComponent();

            selectedItemID = itemID;

            vendorItems = new List<VendorItem>();
            cmbVendor.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Providers.RetailItemData.SetItemsDefaultVendor(PluginEntry.DataModel, selectedItemID, selectedVendorID);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbVendor_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == RecordIdentifier.Empty || ((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = ((DataEntity)e.Data).Text;
            }
            else
            {
                e.TextToDisplay = (string)((DataEntity)e.Data).ID + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void cmbVendor_RequestData(object sender, EventArgs e)
        {
            try
            {
                vendorItems = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorsForItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), selectedItemID,
                                                               VendorItemSorting.Description, false,true);
                List<DataEntity> vendors = new List<DataEntity>();
                foreach (var vendorItem in vendorItems)
                {
                    vendors.Add(new DataEntity(vendorItem.ID, vendorItem.VendorDescription));
                }

                cmbVendor.SetData(vendors, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
          
        }

        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddItemToVendor", null);
            if (plugin != null)
            {
                plugin.Message(this, "CanAddItemToVendor", selectedItemID);
            }
        }

        private void cmbVendor_SelectedDataChanged(object sender, EventArgs e)
        {
            try
            {
                VendorItem selectedVendorItem = vendorItems.Find(x => x.ID == cmbVendor.SelectedData.ID);
                selectedVendorID = selectedVendorItem.VendorID;
                cmbUnit.SelectedData = new DataEntity(selectedVendorItem.UnitID, selectedVendorItem.UnitDescription);

                CheckEnabled(sender, e);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (cmbVendor.SelectedData.ID != "") &&
                            (cmbUnit.SelectedData.ID != "");
        }
    }
}
