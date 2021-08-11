using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
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
    public partial class PurchaseOrderLineDialog : DialogBase
    {
        private PurchaseOrder purchaseOrder;
        private PurchaseOrderLine purchaseOrderLine;
        private Vendor vendor;
        private RecordIdentifier barcodeUnitID;

        private DialogResult dialogResult = DialogResult.Cancel;
        private RetailItem retailItem;

        private List<Unit> unitListForItemAndVariant;
        private bool lockEvent = false;
        private WeakReference retailItemEditor;
        private bool editingLine;
        private RecordIdentifier selectedItemID;

        public PurchaseOrderLineDialog(PurchaseOrder purchaseOrder, PurchaseOrderLine purchaseOrderLine)
            : this(purchaseOrder)
        {
            editingLine = true;

            this.purchaseOrderLine = purchaseOrderLine;

            RetailItem retailItem = GetRetailItem(purchaseOrderLine.ItemID);

            this.selectedItemID = purchaseOrderLine.ItemID; 
            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(purchaseOrderLine.ItemID, purchaseOrderLine.ItemName);
            }
            else
            {
                RetailItem headerItem = GetRetailItem(retailItem.HeaderItemID);
                this.selectedItemID = retailItem.ID;
                cmbItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                cmbVariant.SelectedData = new DataEntity(purchaseOrderLine.ItemID, purchaseOrderLine.VariantName);
            }

            Unit unit = new Unit();
            unit.ID = purchaseOrderLine.UnitID;
            unit.Text = purchaseOrderLine.UnitName;
            cmbUnit.SelectedData = unit;

            cmbItem.Enabled = false;
            btnNewItem.Visible = false;
            cmbVariant.Enabled = false;

            SetItemInformation(this, null);

            cmbTax.SelectedIndex = (int) purchaseOrderLine.TaxCalculationMethod;

            DecimalLimit quantityLimiter = purchaseOrderLine.QuantityLimiter;
            DecimalLimit priceLimiter = purchaseOrderLine.PriceLimiter;
            DecimalLimit percentageLimiter = purchaseOrderLine.PercentageLimiter;

            ntbQuantity.SetValueWithLimit(purchaseOrderLine.Quantity, quantityLimiter);
            ntbPrice.SetValueWithLimit(purchaseOrderLine.UnitPrice, priceLimiter);
            ntbDiscountAmount.SetValueWithLimit(purchaseOrderLine.DiscountAmount, priceLimiter);
            ntbDiscountPercentage.SetValueWithLimit(purchaseOrderLine.DiscountPercentage, percentageLimiter);

            ntbPrice.Enabled = true;
            ntbQuantity.Enabled = true;
            ntbDiscountAmount.Enabled = true;
            ntbDiscountPercentage.Enabled = true;
            cmbTax.Enabled = true;
            btnOK.Enabled = false;

            SetQuantityAllowDecimals(purchaseOrderLine.Quantity);

            chkCreateAnother.Visible = false;
            chkCreateAnother.CheckState = CheckState.Unchecked;
        }

        public PurchaseOrderLineDialog(PurchaseOrder purchaseOrder)
            : this()
        {
            try
            {
                this.purchaseOrder = purchaseOrder;
                vendor = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrder.VendorID,
                    true);
            
                cmbItem.SelectedData = new DataEntity("", "");

                barcodeUnitID = RecordIdentifier.Empty;

                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                DecimalLimit percentageLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

                ntbPrice.SetValueWithLimit(0m, priceLimiter);
                ntbDiscountAmount.SetValueWithLimit(purchaseOrder.DefaultDiscountAmount, priceLimiter);
                ntbDiscountPercentage.SetValueWithLimit(purchaseOrder.DefaultDiscountPercentage, percentageLimiter);

                cmbTax.SelectedIndex = (int)vendor.TaxCalculationMethod;

                IPlugin purchaseOrderPlugin = PluginEntry.Framework.FindImplementor(this, "CanViewPurchaseOrders", null);
                IPlugin plugin = purchaseOrderPlugin != null ? PluginEntry.Framework.FindImplementor(this, "CanManagePurchaseOrders", null) : PluginEntry.Framework.FindImplementor(this, "CanCreateRetailItem", null);

                if (plugin != null)
                {
                    btnNewItem.Visible = true;
                    retailItemEditor = new WeakReference(plugin);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private PurchaseOrderLineDialog()
        {
            InitializeComponent();

            editingLine = false;
            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
        }
        

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SelectedID
        {
            get { return purchaseOrder.PurchaseOrderID; }
        }

        // Note: if saving algorithm for PO line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreatePurchaseOrder()
        private bool Save()
        {
            try
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

                // Get the item and save its latest purchase price
                RetailItem retailItem = GetRetailItem(target);

                if (retailItem.IsAssemblyItem)
                {
                    return SaveAssemblyComponents(retailItem, (decimal)ntbQuantity.Value, cmbUnit.SelectedDataID);
                }
                else
                {
                    return SaveLine(target);
                }
            }

            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        private bool SaveAssemblyComponents(RetailItem retailItem, decimal itemQuantity, RecordIdentifier itemUnitID)
        {
            bool lineSaved = false;

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                PluginEntry.DataModel, retailItem.ID, itemUnitID, retailItem.SalesUnitID, itemQuantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(
                PluginEntry.DataModel, retailItem.ID, purchaseOrder.StoreID, false);

            if (assembly != null)
            {
                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = GetRetailItem(component.ItemID);
                    if (componentItem.IsAssemblyItem)
                    {
                        lineSaved |= SaveAssemblyComponents(componentItem, componentQuantity, component.UnitID);
                    }
                    else if (componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        if (component.UnitID != componentItem.PurchaseUnitID)
                        {
                            componentQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                                PluginEntry.DataModel, componentItem.ID, component.UnitID, componentItem.PurchaseUnitID, componentQuantity);
                        }

                        lineSaved |= SaveLine(componentItem.ID, true, componentQuantity, (string)componentItem.PurchaseUnitID);
                    }
                }
            }

            return lineSaved;
        }

        private bool SaveLine(RecordIdentifier target, bool isComponent = false, decimal componentQuantity = 1, string componenUnitID = "")
        {
            try
            {
                RecordIdentifier purchaseOrderID = purchaseOrder.PurchaseOrderID;

                RecordIdentifier unitID = RecordIdentifier.Empty;
                if (cmbUnit.SelectedData != null)
                {
                    Unit unit = (Unit)cmbUnit.SelectedData;
                    unitID = unit.ID;
                }

                RecordIdentifier itemSalesTaxGroupID = Providers.RetailItemData.GetItemsItemSalesTaxGroupID(PluginEntry.DataModel, purchaseOrderLine == null ? target : purchaseOrderLine.ItemID);
                TaxCalculationMethodEnum taxCalculationMethod = (TaxCalculationMethodEnum)cmbTax.SelectedIndex;

                if (purchaseOrderLine == null)
                {
                    purchaseOrderLine = new PurchaseOrderLine();
                    purchaseOrderLine.PurchaseOrderID = purchaseOrderID;
                }

                purchaseOrderLine.ItemID = (string)target;
                purchaseOrderLine.UnitID = isComponent ? componenUnitID : (string)unitID;
                purchaseOrderLine.Quantity = isComponent ? componentQuantity : (decimal)ntbQuantity.Value;
                purchaseOrderLine.DiscountAmount = (decimal)ntbDiscountAmount.Value;
                purchaseOrderLine.DiscountPercentage = (decimal)ntbDiscountPercentage.Value;
                purchaseOrderLine.TaxCalculationMethod = taxCalculationMethod;
                purchaseOrderLine.UnitPrice = (decimal)ntbPrice.Value;

                decimal calcTaxFrom = (decimal)ntbPrice.Value;

                if (purchaseOrderLine.TaxCalculationMethod != TaxCalculationMethodEnum.NoTax)
                {
                    calcTaxFrom = purchaseOrderLine.GetDiscountedPrice();
                }

                ITaxService taxService = (ITaxService)PluginEntry.DataModel.Service(ServiceType.TaxService);

                purchaseOrderLine.TaxAmount = taxService.GetTaxAmountForPurchaseOrderLine(
                    PluginEntry.DataModel,
                    itemSalesTaxGroupID,
                    purchaseOrder.VendorID,
                    purchaseOrder.StoreID,
                    calcTaxFrom,
                    (decimal)ntbDiscountAmount.Value,
                    (decimal)ntbDiscountPercentage.Value,
                    taxCalculationMethod
                    );

                bool hasPostedGRDLs = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrderLine.ID,
                    true);

                // Here we check if we are trying to make the ordered quantity be smaller than the posted quantity.
                if (hasPostedGRDLs)
                {
                    List<GoodsReceivingDocumentLine> allGRDLsForPurchaseOrderLine =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseOrderLine.ID,
                            true);

                    decimal receivedQtyThatIsPosted = (from lines in allGRDLsForPurchaseOrderLine
                                                       where lines.Posted
                                                       select lines.ReceivedQuantity).Sum();
                    decimal newOrderedQty = purchaseOrderLine.Quantity;

                    if (receivedQtyThatIsPosted > newOrderedQty)
                    {
                        MessageDialog.Show(Resources.NewOrderedQtyLowerThanReceivedPostedQty);
                        return false;
                    }
                }

                // Check if the item being added to the purchase order is already part of the vendor items and if not create a new one
                VendorItem newVendorItem = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorItem(PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    vendor.ID,
                    target,
                    unitID,
                    true);

                if (newVendorItem == null)
                {
                    newVendorItem = new VendorItem();
                    newVendorItem.RetailItemID = target;
                    newVendorItem.UnitID = unitID;
                    newVendorItem.VendorID = vendor.ID;
                }

                // Update the price and order dates of the vendor item
                newVendorItem.LastItemPrice = (decimal)ntbPrice.Value;
                newVendorItem.LastOrderDate = Date.Now;

                // Save it to central database
                newVendorItem.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveVendorItem(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    newVendorItem,
                    true);

                purchaseOrderLine.VendorItemID = (string)newVendorItem.VendorItemID;

                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SavePurchaseOrderLine(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrderLine,
                    true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add,
                    "PurchaseOrderLine",
                    purchaseOrderLine.PurchaseOrderID,
                    purchaseOrderLine);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            return true;
        }

        private void SetDefaults()
        {
            cmbItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            tbBarcode.Text = "";
            ntbQuantity.Value = 0;
            ntbPrice.Value = 0;
            purchaseOrderLine = null;
            btnOK.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                if (chkCreateAnother.Checked)
                {
                    dialogResult = DialogResult.OK;
                    SetDefaults();
                    tbBarcode.Focus();
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;

            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (ntbQuantity.Value > 0)
                            && (cmbTax.SelectedIndex > -1)
                            && (cmbUnit.SelectedData != null)
                            && (!cmbVariant.Enabled || cmbVariant.SelectedData != null);
        }

        private RecordIdentifier GetPurchaseUnitID(RetailItem item)
        {
            RecordIdentifier purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, item.ID, RetailItem.UnitTypeEnum.Purchase);

            if (purchaseUnitId == null || string.IsNullOrEmpty((string) purchaseUnitId))
            {
                purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, item.ID, RetailItem.UnitTypeEnum.Inventory);
            }

            if (purchaseUnitId == null || string.IsNullOrEmpty((string) purchaseUnitId))
            {
                purchaseUnitId = item.PurchaseUnitID;
            }

            return purchaseUnitId;
        }

        private RetailItem GetRetailItem(RecordIdentifier itemID)
        {
            errorProvider1.Clear();
            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);

            if (item == null)
            {
                try
                {
                    item = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetRetailItem(PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        itemID,
                        true);
                        }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return null;
                }
            }

            return item;
        }

        private VendorItem GetVendorItem(RecordIdentifier vendorID, RecordIdentifier itemID)
        {
            try
            {
                return Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetFirstVendorItem(
                    PluginEntry.DataModel, 
                    PluginOperations.GetSiteServiceProfile(),
                    vendorID, 
                    itemID, 
                    true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return null;
            }
        }

        private void SetItemInformation(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)cmbItem.SelectedData.ID))
            {
                return;
            }

            if (cmbVariant.SelectedData != null && !string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue) && cmbVariant.SelectedData is MasterIDEntity)
            {
                retailItem = GetRetailItem((cmbVariant.SelectedData as MasterIDEntity).ReadadbleID);
            }
            else
            {
                retailItem = GetRetailItem(cmbItem.SelectedData.ID);
            }

            // Reload the Item and Unit
            if (retailItem == null || retailItem.InventoryExcluded)
            {
                retailItem = null;
                cmbItem.SelectedData = new DataEntity("", "");
                cmbItem.SelectedDataID = null;
                ItemWantsFocus();
                return;
            }

            //If the barcode had a specific unit then that should be used.
            RecordIdentifier purchaseUnitId = string.IsNullOrEmpty((string)barcodeUnitID) ? GetPurchaseUnitID(retailItem) : barcodeUnitID;

            if (retailItem.ItemType == ItemTypeEnum.MasterItem)
            {
                if (cmbVariant.SelectedData != null && cmbVariant.SelectedData.ID != selectedItemID)
                {
                    cmbVariant.SelectedData = new DataEntity("", "");
                }
            }
            else if (cmbItem.SelectedDataID != selectedItemID)
            {
                cmbVariant.SelectedData = new DataEntity("", "");
            }
            cmbVariant.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID);

            cmbUnit.Enabled = (retailItem != null);
            if (retailItem.ItemType == ItemTypeEnum.MasterItem && cmbItem.SelectedDataID != selectedItemID)
            {
                cmbUnit.Enabled = false;
            }

            if (sender is DualDataComboBox || e == null)
            {
                BarCode barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);
                if (barCode != null)
                {
                    tbBarcode.Text = (string)barCode.ItemBarCode;
                }
                else
                {
                    tbBarcode.Text = "";
                }

            }
            else if (sender is TextBox && !retailItem.InventoryExcluded)
            {
                if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {
                    cmbItem.SelectedData.Text = retailItem.Text;
                    cmbItem.Text = retailItem.Text;
                }
                else
                {
                    RecordIdentifier HeaderItemID = Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, retailItem.HeaderItemID);
                    cmbItem.SelectedData = new DataEntity(HeaderItemID, retailItem.Text);
                    cmbItem.Text = retailItem.Text;
                    cmbVariant.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);
                    cmbVariant.Text = retailItem.VariantName;
                    cmbVariant.Enabled = true;
                }
            }
            // Enable other controls
            ntbPrice.Enabled = true;
            ntbQuantity.Enabled = true;
            ntbDiscountPercentage.Enabled = true;
            ntbDiscountAmount.Enabled = true;
            cmbTax.Enabled = true;

            if (retailItem.ItemType != ItemTypeEnum.MasterItem && !retailItem.InventoryExcluded)
            {
                Unit unit;
                if (purchaseOrderLine != null)
                {
                    unit = Providers.UnitData.Get(PluginEntry.DataModel, purchaseOrderLine.UnitID);
                }
                else
                {
                    unit = Providers.UnitData.Get(PluginEntry.DataModel, purchaseUnitId);
                }
                cmbUnit.SelectedData = unit;

                try
                {
                    if (!editingLine)
                    {
                        ntbPrice.Value = (double)Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetDefaultPurchasePrice(
                                                PluginEntry.DataModel,
                                                PluginOperations.GetSiteServiceProfile(),
                                                retailItem.ID,
                                                vendor.ID,
                                                unit.ID,
                                                true);
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }

            if (retailItem.ItemType == ItemTypeEnum.MasterItem)
            {
                VariantWantsFocus();
            }
            else
            {
                UnitWantsFocus();
            }
            selectedItemID = cmbItem.SelectedDataID;
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            SetItemInformation(sender, e);
            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            SetItemInformation(sender, e);
            ReloadAndSetUnitList();

            cmbUnit.Enabled = true;
            Unit unit = Providers.UnitData.Get(PluginEntry.DataModel, GetPurchaseUnitID(retailItem));

            cmbUnit.SelectedData = unit;
            try
            {
                ntbPrice.Value =
                    (double)
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetDefaultPurchasePrice(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                (cmbVariant.SelectedData as MasterIDEntity)?.ReadadbleID, vendor.ID, unit.ID, true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);
            CheckEnabled(this, EventArgs.Empty);

            UnitWantsFocus();
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            Unit unit = (Unit) cmbUnit.SelectedData;
            RecordIdentifier unitID = unit.ID;

            try
            {
                ntbPrice.Value = (double) Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetDefaultPurchasePrice(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    retailItem.ID,
                    vendor.ID,
                    unitID,
                    true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

            if (CheckUnitConversion(unitID))
            {
                SetQuantityAllowDecimals((decimal)ntbQuantity.Value);

                CheckEnabled(this, EventArgs.Empty);

                PriceWantsFocus();
            }
            else
            {
                cmbUnit.SelectedData = new DataEntity("", "");
                CheckEnabled(this, EventArgs.Empty);
            }
        }

        private void ReloadAndSetUnitList()
        {
            try
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

                unitListForItemAndVariant = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetDistinctUnitsForVendorItem(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    purchaseOrder.VendorID,
                    target,
                    true);

                if (cmbVariant.Enabled && unitListForItemAndVariant.Count == 0)
                {
                    target = PluginOperations.GetReadableItemID(cmbItem);

                    unitListForItemAndVariant = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetDistinctUnitsForVendorItem(
                        PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        purchaseOrder.VendorID,
                        target,
                        true);
                }

                // Display the top item of the Unit list in the unit combo box
                Unit unit = (unitListForItemAndVariant.Count > 0) ? unitListForItemAndVariant[0] : new Unit(retailItem.PurchaseUnitID, retailItem.PurchaseUnitName, 0, 0);

                if (cmbUnit.SelectedData != unit)
                {
                    cmbUnit.SelectedData = unit;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }
        
        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            cmbUnit.SetData(unitListForItemAndVariant, null);
            
            var itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItem.SelectedData.ID,itemInventoryUnit),
                    Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Resources.Convertible, Resources.All },
                200);

            cmbUnit.SetData(data, pnl);
            
        }

        private bool CheckUnitConversion(RecordIdentifier unitIDtoCheck)
        {
            if (retailItem == null)
            {
                return false;
            }

            RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

            //If the unit selected is one of the units already on the item then there is no need to create a conversion rule
            if (cmbUnit.SelectedData != null)
            {
                Unit unit = (Unit)cmbUnit.SelectedData;
                if (retailItem.SalesUnitID == unit.ID || retailItem.InventoryUnitID == unit.ID || retailItem.PurchaseUnitID == unit.ID)
                {
                    return true;
                }
            }

            bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(PluginOperations.CreateItemDataEntity(cmbItem, cmbVariant), cmbUnit.SelectedData.ID);
            if (!unitConversionExists)
            {
                MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                cmbUnit.Text = "";
                return false;
            }

            return true;
        }

        private void ItemWantsFocus()
        {
            if(retailItem == null)
            {
                tbBarcode.Focus();
                return;
            }

            if (cmbItem.Enabled && (cmbItem.SelectedDataID == null || string.IsNullOrEmpty((string)cmbItem.SelectedDataID)))
            {
                cmbItem.Focus();
                return;
            }

            VariantWantsFocus();
        }

        private void VariantWantsFocus()
        {
            if (retailItem == null)
            {
                tbBarcode.Focus();
                return;
            }

            if (cmbVariant.Enabled && (cmbVariant.SelectedDataID == null || string.IsNullOrEmpty((string)cmbVariant.SelectedDataID)))
            {
                cmbVariant.Focus();
                return;
            }
            
            UnitWantsFocus();
        }

        private void UnitWantsFocus()
        {
            if (retailItem == null)
            {
                tbBarcode.Focus();
                return;
            }

            if (cmbUnit.Enabled && (cmbUnit.SelectedDataID == null || string.IsNullOrEmpty((string)cmbUnit.SelectedDataID)))
            {
                cmbUnit.Focus();
                return;
            }

            PriceWantsFocus();
        }

        private void PriceWantsFocus()
        {
            if (retailItem == null)
            {
                tbBarcode.Focus();
                return;
            }

            if (ntbPrice.Value < 0.001)
            {
                ntbPrice.Focus();
                return;
            }

            ntbQuantity.Focus();
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
        {
            List<RecordIdentifier> excludedItemIDs = new List<RecordIdentifier>();

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = cmbVariant.SelectedData != null ? ((DataEntity) cmbVariant.SelectedData).Text : RecordIdentifier.Empty;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem
                    ? retailItem.MasterID
                    : retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs,
                textInitallyHighlighted, true);
        }

        private void cmbItem_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity) cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            SingleSearchPanel control1 = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.InventoryItems,
                                                                purchaseOrder.VendorID, SingleSearchPanel.FilterEnum.ItemsForSpecificVendor, textInitallyHighlighted, true);

            SingleSearchPanel control2 = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.InventoryItems, textInitallyHighlighted, null, true);

            e.ControlToEmbed = new TabbedCustomDualDataPanel(new IControlClosable[] { control1, control2 }, new[] { Resources.FromVendor, Resources.All }, 350, 284);
        }

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            if (!lockEvent)
            {
                lockEvent = true;
                CheckBarCode(sender, e);
            }
            lockEvent = false;
        }

        private void CheckBarCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text))
            {
                return;
            }

            RecordIdentifier itemID = RecordIdentifier.Empty;
            barcodeUnitID = RecordIdentifier.Empty;

            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barcode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barcode != null && barcode.InternalType == BarcodeInternalType.Item)
                {
                    itemID = barcode.ItemID;
                    barcodeUnitID = barcode.UnitID;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
                {
                    itemID = tbBarcode.Text;
                }
                else
                {
                    VendorItem vendorItem = GetVendorItem(vendor.ID, tbBarcode.Text);
                    if (vendorItem != null)
                    {
                        itemID = vendorItem.RetailItemID;
                    }
                }
            }
            else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
            {
                itemID = tbBarcode.Text;
            }
            else
            {
                VendorItem vendorItem = GetVendorItem(vendor.ID, tbBarcode.Text);
                if (vendorItem != null)
                {
                    itemID = vendorItem.RetailItemID;
                }
            }
            if (itemID != RecordIdentifier.Empty)
            {
                cmbItem.SelectedData = new DataEntity { ID = itemID };
                cmbItem_SelectedDataChanged(sender, e);
                VariantWantsFocus();
            }
        }

        private void tbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Tab) || (e.KeyData == (Keys.Tab | Keys.Shift)) || e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (e.KeyData == (Keys.Tab | Keys.Shift))
                {
                    lockEvent = true;
                    chkCreateAnother.Select();
                }
                else
                {
                    cmbItem.Select();
                }
            }
        }

        private void cmbItem_Leave(object sender, EventArgs e)
        {
            if (retailItem != null)
            {
                if (!lockEvent)
                {
                    if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                    {
                        VariantWantsFocus();
                    }
                    else
                    {
                        UnitWantsFocus();
                    }
                }
            }
            lockEvent = false;
        }

        /// <summary>
        /// Saves the item to the central database both as a normal item and as a vendor item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private RetailItem SaveItem(RetailItem item)
        {
            try
            {
                Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).SaveRetailItem(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    item,
                    true);

                VendorItem vendorItem = new VendorItem
                {
                    RetailItemID = item.ID,
                    UnitID = item.InventoryUnitID,
                    VendorID = vendor.ID
                };

                //Save it to central database
                vendorItem.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveVendorItem(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    vendorItem,
                    true);

                return item;

            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format(Resources.ItemNotSavedToHeadOffice, item.Text) + "\r\n" + ex.Message);
            }

            return null;
        }

        private RetailItem SaveNewItem()
        {
            RecordIdentifier newItemID = (RecordIdentifier)((IPlugin)retailItemEditor.Target).Message(this, "CreateRetailItem", new object[] { false, false });
            if (newItemID != null && !string.IsNullOrEmpty((string)newItemID))
            {
                RetailItem item = GetRetailItem(newItemID); 
                if (item != null && (RecordIdentifier.IsEmptyOrNull(item.MasterID)))
                {
                    return SaveItem(item);
                }

                if (item != null && (!RecordIdentifier.IsEmptyOrNull(item.MasterID)))
                {
                    RetailItem headerItem = GetRetailItem(item.MasterID); 

                    headerItem = SaveItem(headerItem);

                    if (headerItem != null)
                    {
                        List<DataEntity> variantList = Providers.RetailItemData.GetItemVariantList(
                            PluginEntry.DataModel, headerItem.MasterID);

                        foreach (DataEntity variant in variantList)
                        {
                            RetailItem variantItem = GetRetailItem(variant.ID);
                            if (variantItem != null)
                            {
                                try
                                {
                                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel)
                                        .SaveRetailItem(
                                            PluginEntry.DataModel,
                                            PluginOperations.GetSiteServiceProfile(),
                                            variantItem,
                                            false);
                                }
                                catch (Exception ex)
                                {
                                    MessageDialog.Show(
                                        string.Format(Resources.ItemNotSavedToHeadOffice, variantItem.Text) + "\r\n" +
                                        ex.Message);
                                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                        .Disconnect(PluginEntry.DataModel);
                                    return item;
                                }
                                finally
                                {
                                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                        .Disconnect(PluginEntry.DataModel);
                                }
                            }
                        }

                        return headerItem;
                    }
                }
            }

            return null;
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            if (!retailItemEditor.IsAlive)
            {
                return;
            }

            RetailItem newItem = new RetailItem();

            newItem = SaveNewItem();

            if (newItem != null && !string.IsNullOrEmpty(newItem.Text) && !newItem.InventoryExcluded)
            {
                cmbItem.SelectedData = new DataEntity(newItem.ID, newItem.Text);
                SetItemInformation(this, null);
            }
        }

        private void PurchaseOrderLineDialog_Shown(object sender, EventArgs e)
        {
            if (editingLine)
            {
                ItemWantsFocus();
            }
        }
        
        private void PurchaseOrderLineDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(tbBarcode.Text))
                {
                    tbBarcode_KeyDown(sender, e);
                }
            }
        }

        private void cmbItem_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData != null ? cmbUnit.SelectedData.Text : retailItem.PurchaseUnitName));
            ntbQuantity.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}