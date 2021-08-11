using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ItemVendorDialog : DialogBase
    {
        RecordIdentifier vendorID;
        RecordIdentifier itemID;
        RecordIdentifier inventoryUnitID;
        RecordIdentifier salesUnitId;
        VendorItem vendorItem;
        bool multiEdit;

        IInventoryService service;

        /// <summary>
        /// dialogType == vendor means the ID parameter is Retail Item ID 
        /// dialogType == RetailItem means the ID parameter is Vendor ID
        /// </summary>
        /// <param name="ID">Either vendor ID or retail item ID, depending on the dialogType</param>
        /// <param name="multiEdit"></param>
        public ItemVendorDialog(RecordIdentifier ID, bool multiEdit = false)
            : this()
        {
            cmbVendor.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = multiEdit
                ? new DataEntity(RecordIdentifier.Empty, Properties.Resources.SameAsSalesUnit)
                : new DataEntity("", "");

            this.multiEdit = multiEdit;

            itemID = ID;

            if (multiEdit)
            {
                cmbUnit.Enabled = false;
                tbID.Enabled = false;
                btnAddUnit.Visible = false;
            }
            else
            {
                SetUnitData(itemID);
            }

            btnAddItem.Visible = false;
        }

        public ItemVendorDialog(RecordIdentifier ID, VendorItem vendorItem, bool multiEdit = false)
            :this(ID,multiEdit)
        {
            this.vendorItem = vendorItem;

            tbID.Text = (string)vendorItem.VendorItemID;
            cmbUnit.SelectedData = new DataEntity(vendorItem.UnitID, vendorItem.UnitDescription);
            cmbVendor.SelectedDataID = vendorItem.VendorID;
            cmbVendor.Text = vendorItem.VendorDescription;
            cmbVendor.Enabled = false;

            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbPurchasePrice.SetValueWithLimit(vendorItem.DefaultPurchasePrice, priceLimiter);


        }
        public ItemVendorDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier VendorID
        {
            get { return vendorID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (vendorItem == null)
                {
                    vendorID = cmbVendor.SelectedData.ID;
                }
                else
                {
                    vendorID = vendorItem.VendorID;
                }
                RecordIdentifier oldRecordID = "";

                if (vendorItem != null)
                {
                    oldRecordID = vendorItem.ID;
                }

                if (!multiEdit)
                {
                    service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    if (service.VendorItemExistsExcludingCurrentRecord(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorID, tbID.Text, oldRecordID, true))
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.VendorItemCombinationExists);
                        return;
                    }

                    if (service.VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitIDAndVendorItemID(
                        PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        vendorID,
                        itemID,
                        cmbUnit.SelectedData.ID,
                        tbID.Text,
                        oldRecordID,
                        false)
                        )
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.VendorItemExists);
                        return;
                    }

                    if (service.VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(), vendorID, itemID, cmbUnit.SelectedData.ID, oldRecordID, true))
                    {
                        errorProvider1.SetError(cmbUnit, Resources.VendorItemAndUnitAlreadyExists);
                        return;
                    }

                    if (cmbUnit.SelectedDataID != inventoryUnitID && cmbUnit.SelectedDataID != salesUnitId)
                    {
                        RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID, CacheType.CacheTypeApplicationLifeTime);
                        DataEntity itemDataEntity = new DataEntity(item.ID, item.Text);
                        bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(itemDataEntity, cmbUnit.SelectedData.ID);
                        if (!unitConversionExists)
                        {
                            MessageDialog.Show(Resources.ConversionRuleMissing);
                            cmbUnit.Text = "";
                            return;
                        }
                    }

                }

                if (vendorItem == null)
                {
                    vendorItem = new VendorItem();
                    vendorItem.VendorID = vendorID;

                }
                vendorItem.VendorItemID = tbID.Text;
                vendorItem.RetailItemID = itemID;
                vendorItem.UnitID = cmbUnit.SelectedData.ID;

                vendorItem.VendorDescription = cmbVendor.SelectedData.Text;
                vendorItem.UnitDescription = cmbUnit.SelectedData.Text;
                vendorItem.DefaultPurchasePrice = (decimal)ntbPurchasePrice.Value;
                if (!multiEdit)
                {

                    vendorItem.VendorID = vendorID;
                    //Providers.VendorItemData.Save(PluginEntry.DataModel, vendorItem);
                    vendorItem.ID = service.SaveVendorItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorItem, true);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "VendorItem", vendorItem.ID, null);

                    // Check if this is the first item for the vendor, if so he should be the items default vendor
                    if (!Providers.RetailItemData.ItemHasDefaultVendor(PluginEntry.DataModel, itemID))
                    {
                        Providers.RetailItemData.SetItemsDefaultVendor(PluginEntry.DataModel, itemID, vendorItem.VendorID);
                    }
                    service.Disconnect(PluginEntry.DataModel);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public VendorItem VendorItem
        {
            get
            {
                return vendorItem;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            cmbUnit.Enabled =  (!(cmbVendor.SelectedData.ID == "") || vendorItem != null ) && !multiEdit;
            RetailItem item = null;
            if (!RecordIdentifier.IsEmptyOrNull(itemID))
            {
                item = PluginOperations.GetRetailItem(itemID);
                if (item == null)
                {
                    return;
                }
            }

            if (inventoryUnitID == null && item != null && !multiEdit)
            {
                errorProvider1.SetError(cmbVendor, Properties.Resources.InventoryUnitMissing);
                cmbUnit.Enabled = false;
                return;
            }

            if (vendorItem == null)
            {
                if (multiEdit)
                {
                    btnOK.Enabled = cmbVendor.SelectedData.ID != "";
                }
                else
                {
                    btnOK.Enabled = cmbVendor.SelectedData.ID != ""
                                    && cmbUnit.SelectedData.ID != "";
                }
            }
            else
            {
                btnOK.Enabled = cmbUnit.SelectedData.ID != ""
                    &&
                    (tbID.Text != vendorItem.VendorItemID ||
                     cmbUnit.SelectedData.ID != vendorItem.UnitID ||
                     (decimal)ntbPurchasePrice.Value != vendorItem.DefaultPurchasePrice);
            }
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            
        }
   
        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbRetailItemOrVendor_RequestData(object sender, EventArgs e)
        {
            try
            {
                service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                List<Vendor> listOfVendors = service.GetVendorsList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel), VendorSorting.Description, false, true);
                cmbVendor.SetData(listOfVendors, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void cmbRetailItemOrVendor_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void SetUnitData(RecordIdentifier itemId)
        {
            inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemId, RetailItem.UnitTypeEnum.Inventory);
            salesUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemId, RetailItem.UnitTypeEnum.Sales);
            var salesUnit = Providers.UnitData.Get(PluginEntry.DataModel, salesUnitId);
            if (salesUnit != null)
            {
                cmbUnit.SelectedData = salesUnit;
            }
            else
            {
                cmbUnit.Clear();
            }
        }

        private void cmbUnit_DropDown(object sender, DropDownEventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,itemID,inventoryUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                300);

            e.ControlToEmbed = pnl;
        }
    }
}
