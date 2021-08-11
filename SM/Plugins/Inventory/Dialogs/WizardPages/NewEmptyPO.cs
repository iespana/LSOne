using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.Controls;
using System.Linq;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewEmptyPO : UserControl, IWizardPage
    {
        WizardBase parent;
        RecordIdentifier purchaseOrderID;
        InventoryTypeAction inventoryTypeAction;
        DateTime deliveryDate;
        private PurchaseOrder purchaseOrder;
        private InventoryTemplateFilterContainer filter;
        private TemplateListItem template;
        private Vendor selectedVendor;
        private bool copyPurchaseOrder;

        // When generating an empty PO
        public NewEmptyPO(WizardBase parent, InventoryTypeAction inventoryTypeAction)
            : this()
        {
            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;

            purchaseOrderID = RecordIdentifier.Empty;
            purchaseOrder = new PurchaseOrder();
            cmbVendor.SelectedData = new DataEntity("", "");
            cmbCurrency.SelectedData = new DataEntity("", "");

            cmbVendor.Enabled = false;
            cmbCurrency.Enabled = false;
        }

        // When copying an older PO
        public NewEmptyPO(WizardBase parent, PurchaseOrder purchaseOrder, InventoryTypeAction inventoryTypeAction)
            : this()
        {

            this.parent = parent;
            this.purchaseOrder = purchaseOrder;
            this.inventoryTypeAction = inventoryTypeAction;

            tbPurchaseOrderDescription.Text = purchaseOrder.Description;
            cmbStore.SelectedData = new DataEntity(purchaseOrder.StoreID, purchaseOrder.StoreName);
            cmbVendor.SelectedData = new DataEntity(purchaseOrder.VendorID, purchaseOrder.VendorName);
            cmbCurrency.SelectedData = new DataEntity(purchaseOrder.CurrencyCode, purchaseOrder.CurrencyDescription);
            ntbDiscountAmount.Value = (double)purchaseOrder.DefaultDiscountAmount;
            ntbDiscountPercentage.Value = (double)purchaseOrder.DefaultDiscountPercentage;

            try
            {
                selectedVendor = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseOrder.VendorID, true);
                dtpDeliveryDate.Value = DateTime.Now.AddBusinessDays(selectedVendor.DefaultDeliveryTime);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            label2.Enabled = false;
            copyPurchaseOrder = true;
        }

        // When generating a PO from item filter
        public NewEmptyPO(WizardBase parent, InventoryTemplateFilterContainer filter, InventoryTypeAction inventoryTypeAction)
            : this(parent, inventoryTypeAction)
        {
            this.filter = filter;
        }

        public NewEmptyPO(WizardBase parent, TemplateListItem template, InventoryTypeAction inventoryTypeAction)
            : this(parent, inventoryTypeAction)
        {
            this.template = template;
        }

        private NewEmptyPO()
        {
            InitializeComponent();
            copyPurchaseOrder = false;
        }


        #region IWizardPage Members
        public bool HasFinish
        {
            get { return true; }
        }

        public bool HasForward
        {
            get { return false; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            btnOkEnabled(this, EventArgs.Empty);
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            throw new NotImplementedException();
        }

        public void ResetControls()
        {

        }

        public RecordIdentifier SelectedVendorID { get { return selectedVendor.ID; } }
        public RecordIdentifier StoreID { get { return cmbStore.SelectedDataID; } }
        public RecordIdentifier CurrencyCode { get { return cmbCurrency.SelectedData.ID; } }
        public string Description { get { return tbPurchaseOrderDescription.Text; } }
        public Date OrderingDate { get { return Date.FromDateControl(dtpOrderingDate); } }
        public DateTime DeliveryDate { get { return dtpDeliveryDate.Value; } }
        public double DiscountPercentage { get { return ntbDiscountPercentage.Value; } }
        public double DiscountAmount { get { return ntbDiscountAmount.Value; } }
        public PurchaseOrder PurchaseOrder { get { return purchaseOrder; } }
        public InventoryTemplateFilterContainer Filter { get { return filter; } }
        public TemplateListItem Template { get { return template; } }

        #endregion

        public RecordIdentifier SelectedID
        {
            get { return purchaseOrderID; }
        }

        private void NewEmptyPO_Load(object sender, EventArgs e)
        {
            if (!PluginEntry.DataModel.IsHeadOffice)
            {
                cmbStore.SelectedData = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }
            else if(Template != null)
            {
                cmbStore.SelectedData = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, Template.StoreID);
            }

            if(!PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores))
            {
                cmbStore.Enabled = false;
            }

            cmbVendor.Enabled = cmbStore.SelectedData != null && (string)cmbStore.SelectedDataID != "" && !copyPurchaseOrder;
            tbPurchaseOrderDescription.Focus();
        }

        private void cmbVendor_SelectedDataChanged(object sender, EventArgs e)
        {
            try
            {
                selectedVendor = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), cmbVendor.SelectedData.ID, true);
                purchaseOrder.VendorID = (string)selectedVendor.ID;
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            if (selectedVendor != null)
            {
                DataEntity vendorCurrencyEntity = new DataEntity(selectedVendor.CurrencyID, selectedVendor.CurrencyDescription);
                cmbCurrency.SelectedData = vendorCurrencyEntity;
            }

            btnOkEnabled(sender, e);
            cmbCurrency.Enabled = true;

            deliveryDate = dtpOrderingDate.Value.AddBusinessDays(selectedVendor.DefaultDeliveryTime);
            dtpDeliveryDate.Value = deliveryDate;
        }

        private void btnOkEnabled(object sender, EventArgs e)
        {
            parent.NextEnabled = (cmbVendor.SelectedData.ID != "") &&
                            (cmbCurrency.SelectedData.ID != "") &&
                            (cmbStore.SelectedData.ID != "");
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {

            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            if ((string)cmbStore.SelectedDataID != "")
            {
                cmbVendor.Enabled = true;
            }
        }

        private void dtpOrderingDate_ValueChanged(object sender, EventArgs e)
        {
            if (cmbVendor.SelectedData.ID != "")
            {
                deliveryDate = dtpOrderingDate.Value.AddBusinessDays(selectedVendor.DefaultDeliveryTime);
                dtpDeliveryDate.Value = deliveryDate;
            }
        }

        private void cmbVendor_DropDown(object sender, Controls.DropDownEventArgs e)
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
                initialSearchText = cmbVendor.SelectedData.ID;
                textInitallyHighlighted = true;
            }

            var searchPanel = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Custom, textInitallyHighlighted, null, true);
            searchPanel.SetSearchHandler(VendorSearchHandler, initialSearchText.StringValue);
            e.ControlToEmbed = searchPanel;
        }

        private List<DataEntity> VendorSearchHandler(object sender, SingleSearchArgs e)
        {
            List<Vendor> vendors = new List<Vendor>();
            try
            {
                VendorSearch vendorSearch = new VendorSearch
                {
                    Description = e.SearchText.Tokenize(),
                    Deleted = false,
                    DescriptionBeginsWith = e.BeginsWith
                };

                vendors = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendors(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorSearch, true);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            return vendors.Cast<DataEntity>().ToList();
        }
    }
}
