using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Properties;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class PurchaseWorksheetLineDialog : DialogBase
    {
        private PurchaseWorksheetLine line;
        private PurchaseWorksheet purchaseWorksheet;
        private InventoryTemplate inventoryTemplate;
        private bool enabled;
        private DialogResult dialogResult = DialogResult.Cancel;
        private RecordIdentifier purchaseUnitId;
        private RecordIdentifier inventoryUnitId;
        private RecordIdentifier salesUnitId;
        private RecordIdentifier selectedItemID;
        private RetailItem retailItem;

        public PurchaseWorksheetLineDialog(RecordIdentifier purchaseWorksheetId, RecordIdentifier purchaseWorksheetLineId)
            : this(purchaseWorksheetId)
        {
            try
            {
                line = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseWorksheetLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseWorksheetLineId, true);
            }
            catch (Exception ex )
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" +  ex.Message);
                return;
            }

            retailItem = GetRetailItem(line.Item.ID);

            this.selectedItemID = line.Item.ID;
            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = line.Item;
            }
            else
            {
                RetailItem headerItem = GetRetailItem(retailItem.HeaderItemID);
                this.selectedItemID = retailItem.ID;
                cmbItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                cmbVariant.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);
            }

            tbBarcode.Text = line.BarCodeNumber;
            cmbUnit.SelectedData = line.Unit;
            cmbVendor.SelectedData = line.Vendor;
            ntbSuggestedQty.Value = (double)line.SuggestedQuantity;
            
            SetQuantityAllowDecimals(line.Quantity);
            
            cmbUnit.Enabled = inventoryTemplate.ChangeUomInLine;
            cmbVendor.Enabled = inventoryTemplate.ChangeVendorInLine;
            cmbItem.Enabled = false;
            cmbVariant.Enabled = false;
            tbBarcode.Enabled = false;
            btnOK.Enabled = false;
            enabled = false;
            chkCreateAnother.CheckState = CheckState.Unchecked;

            purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Purchase);
            inventoryUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);
            salesUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Sales);

            lblInventoryUnit.Text = (Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitId)).Text;

            AcceptButton = btnOK;
            chkCreateAnother.Visible = false;
        }

        public PurchaseWorksheetLineDialog(RecordIdentifier purchaseWorksheetId)
        {
            InitializeComponent();

            try
            {
                purchaseWorksheet = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseWorksheetId,
                               false);
                inventoryTemplate = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseWorksheet.InventoryTemplateID,
                                true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            cmbItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVendor.SelectedData = new DataEntity("", "");
            cmbVendor.Enabled = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (line == null)
            {
                enabled = (string)cmbItem.SelectedData.ID != "" &&
                                (string)cmbUnit.SelectedData.ID != "" &&
                                (!cmbVendor.Enabled || (string)cmbVendor.SelectedData.ID != "") &&
                                (!cmbVariant.Enabled || cmbVariant.SelectedData != null) &&
                                ntbQty.Value != 0;
            }
            else
            {
                enabled = ((cmbItem.SelectedData.ID != line.Item.ID ||
                                cmbUnit.SelectedData.ID != line.Unit.ID ||
                                cmbVendor.SelectedData.ID != line.Vendor.ID ||
                                ntbQty.Value != 0) 
                        && (cmbUnit.SelectedData.ID != ""));
            }

            btnOK.Enabled = enabled;
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
                initialSearchText = cmbItem.SelectedData.Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                                                        false,
                                                        initialSearchText,
                                                        SearchTypeEnum.InventoryItems,
                                                        textInitallyHighlighted, null, false, false);
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (cmbItem.SelectedData != null)
            {
                var unitId = (purchaseUnitId != "") ? purchaseUnitId : inventoryUnitId;

                IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbItem.SelectedData.ID, unitId),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

                TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                    cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                    data,
                    new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                    250);

                cmbUnit.SetData(data, pnl);
            }
        }

        private void cmbVendor_RequestData(object sender, EventArgs e)
        {
            try
            {
                var itemVendors = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                true);
                cmbVendor.SetData(itemVendors, null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedData.ID);
            purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Purchase);
            inventoryUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);
            salesUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Sales);

            if (retailItem == null || retailItem.InventoryExcluded)
            {
                cmbItem.SelectedData = new DataEntity("", "");
                cmbItem.SelectedDataID = null;
                tbBarcode.Focus();
                return;
            }

            cmbUnit.Enabled = (retailItem != null);
            cmbVendor.Enabled = ((retailItem != null) && retailItem.ItemType != ItemTypeEnum.AssemblyItem);
            if (!cmbVendor.Enabled)
            {
                cmbVendor.SelectedData = new DataEntity("", "");
            }

            CheckEnabled(sender, e);

            if (sender is DualDataComboBox)
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
            else if (sender is TextBox)
            {
                cmbItem.SelectedData.Text = retailItem.Text;
                cmbItem.Text = retailItem.Text;
            }

            if (line == null)
            {
                SetDefaultValues(retailItem);
            }

            lblInventoryUnit.Text = (Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitId)).Text;
            SetQuantityAllowDecimals((decimal)ntbQty.Value);

        }

        private void SetDefaultValues(RetailItem retailItem)
        {
            try
            {
                #region Unit

                RecordIdentifier unitID = "";

                switch (inventoryTemplate.UnitSelection)
                {
                    case UnitSelectionEnum.InventoryUnit:
                        unitID = inventoryUnitId;
                        break;
                    case UnitSelectionEnum.PurchaseUnit:
                        unitID = purchaseUnitId;
                        break;
                    case UnitSelectionEnum.SalesUnit:
                        unitID = salesUnitId;
                        break;
                }

                if (unitID != "")
                {
                    cmbUnit.SelectedData = new DataEntity(unitID,
                        (Providers.UnitData.Get(PluginEntry.DataModel, unitID)).Text);
                }
                else if (inventoryUnitId != "") //Try to fallback to inventory unit
                {
                    cmbUnit.SelectedData = new DataEntity(inventoryUnitId,
                        (Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitId)).Text);
                }
                else
                {
                    cmbUnit.SelectedData = new DataEntity("", "");
                }

                #endregion Unit

                SetVendor(retailItem);

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
                cmbVariant.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem;

                if (inventoryTemplate.CalculateSuggestedQuantity)
                {
                    ntbSuggestedQty.Value = (double)PluginOperations.GetSuggestedQuantity(retailItem.ID, purchaseWorksheet.StoreId, retailItem.InventoryUnitID);
                }
                selectedItemID = cmbItem.SelectedDataID;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void SetVendor(RetailItem retailItem)
        {
            var inventoryTemplateVendor = PluginOperations.GetInventoryTemplatesSelectedVendor(inventoryTemplate.ID);
            bool templateVendorHasItem = false;
            if (inventoryTemplateVendor != null)
            {
                templateVendorHasItem =
                     Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).VendorHasItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), retailItem.ID,
                        inventoryTemplateVendor.ID,
                            false);
            }
            if (templateVendorHasItem)
            {
                cmbVendor.SelectedData = inventoryTemplateVendor;
            }
            else if (retailItem.DefaultVendorID != "")
            {
                var vendor = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), retailItem.DefaultVendorID, true);
                if (vendor != null)
                {
                    cmbVendor.SelectedData = new DataEntity(vendor.ID, vendor.Text);
                }
            }
            else
            {
                cmbVendor.SelectedData = new DataEntity("", "");
            }
        }

        private void tbBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
                if (barcodeService != null)
                {
                    BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                    if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                    {
                        cmbItem.SelectedData = new DataEntity { ID = barCode.ItemID };
                        cmbItem_SelectedDataChanged(sender, e);
                    }
                }
            }
        }

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    cmbItem.SelectedData = new DataEntity { ID = barCode.ItemID };
                    cmbItem_SelectedDataChanged(sender, e);
                }
            }
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            bool hasUnitConversion = PluginOperations.UnitConversionWithInventoryUnitExists((DataEntity)cmbItem.SelectedData, cmbUnit.SelectedData.ID);

            if (!hasUnitConversion)
            {
                MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                cmbUnit.SelectedData = new DataEntity("", "");
            }
         
            if (hasUnitConversion)
            {
                CheckEnabled(sender, e);
                SetQuantityAllowDecimals((decimal)ntbQty.Value);
            }
            
        }

        private void SetDefaults()
        {
            line = null;
            cmbItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVendor.SelectedData = new DataEntity("", "");

            tbBarcode.Text = "";
            ntbSuggestedQty.Value = 0;
            ntbQty.Value = 0;

            cmbVariant.Enabled = false;
            btnOK.Enabled = false;
        }

        private bool Save()
        {
            RecordIdentifier target = GetSelectedItemID(cmbItem, cmbVariant);

            if (line == null)
            {
                RetailItem retailItem = GetRetailItem(target);
                if (retailItem.IsAssemblyItem)
                {
                    List<DataEntity> componentsWithoutVendor = new List<DataEntity>();
                    bool lineAdded = SaveAssemblyComponents(retailItem, (decimal)ntbQty.Value, cmbUnit.SelectedDataID, componentsWithoutVendor);

                    if (componentsWithoutVendor.Count > 0)
                    {
                        string message = Properties.Resources.ComponentsNotAddedHaveNoDefaultVendor + Environment.NewLine;

                        foreach (var item in componentsWithoutVendor)
                        {
                            message += String.Format("{0}   - {1} - {2}", Environment.NewLine, item.ID, item.Text);
                        }

                        MessageDialog.Show(message);
                    }

                    if (lineAdded)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(
                            this, DataEntityChangeType.Add, "PurchaseWorksheet", null, null);
                    }
                }
                else
                {
                    SaveLine(
                        tbBarcode.Text,
                        target,
                        (decimal)ntbQty.Value,
                        cmbUnit.SelectedData.ID,
                        cmbVendor.SelectedData.ID,
                        createNew: true);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(
                        this, DataEntityChangeType.Add, "PurchaseWorksheetLine", line.ID, null);
                }
            }
            else
            {
                SaveLine(
                    tbBarcode.Text,
                    target,
                    (decimal)ntbQty.Value,
                    cmbUnit.SelectedData.ID,
                    cmbVendor.SelectedData.ID,
                    createNew: false);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                                                        "PurchaseWorksheet",
                                                        line.ID,
                                                        null);
            }

            return true;
        }

        private void SaveLine(string barcode, RecordIdentifier itemID, decimal quantity, RecordIdentifier unitID, RecordIdentifier vendorID, bool createNew = false)
        {
            if (createNew)
            {
                line = new PurchaseWorksheetLine();
                line.PurchaseWorksheetID = purchaseWorksheet.ID;
                line.StoreID = purchaseWorksheet.StoreId;
            }

            line.BarCodeNumber = barcode;
            line.Item.ID = itemID;
            line.Unit.ID = unitID;
            line.Vendor.ID = vendorID;
            line.Quantity = quantity;
            line.ManuallyEdited = true;

            if (inventoryTemplate.CalculateSuggestedQuantity)
            {
                line.SuggestedQuantity = (decimal) ntbSuggestedQty.Value;
            }

            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryWorksheetLine(
                PluginEntry.DataModel,
                PluginOperations.GetSiteServiceProfile(),
                line,
                true);
        }


        private bool SaveAssemblyComponents(RetailItem retailItem, decimal itemQuantity, RecordIdentifier itemUnitID, List<DataEntity> componentsWithoutVendor)
        {
            bool itemAdded = false;

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                PluginEntry.DataModel, retailItem.ID, itemUnitID, retailItem.SalesUnitID, itemQuantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(
                PluginEntry.DataModel, retailItem.ID, purchaseWorksheet.StoreId, false);

            if (assembly != null)
            {
                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = GetRetailItem(component.ItemID);
                    if (componentItem.IsAssemblyItem)
                    {
                        itemAdded |= SaveAssemblyComponents(componentItem, componentQuantity, component.UnitID, componentsWithoutVendor);
                    }
                    else if (componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        if (RecordIdentifier.IsEmptyOrNull(componentItem.DefaultVendorID))
                        {
                            componentsWithoutVendor.Add(new DataEntity(componentItem.ID, componentItem.Text));
                        }
                        else
                        {
                            var barcode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, componentItem.ID)?.Text ?? "";
                            if (component.UnitID != componentItem.PurchaseUnitID)
                            {
                                componentQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                                    PluginEntry.DataModel, componentItem.ID, component.UnitID, componentItem.PurchaseUnitID, componentQuantity);
                            }

                            SaveLine(
                                barcode, 
                                componentItem.ID,
                                componentQuantity, 
                                componentItem.PurchaseUnitID, 
                                componentItem.DefaultVendorID, 
                                createNew: true);

                            itemAdded = true;
                        }
                    }
                }
            }

            return itemAdded;
        }

        private RetailItem GetRetailItem(RecordIdentifier itemID)
        {
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;
            Close();
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
                initialSearchText = cmbVariant.SelectedData != null ? ((DataEntity)cmbVariant.SelectedData).Text : RecordIdentifier.Empty;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.MasterID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs,
                textInitallyHighlighted, true);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            try
            {
                RetailItem variantItem = GetRetailItem(cmbVariant.SelectedDataID);
                SetVendor(variantItem);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }

            SetQuantityAllowDecimals((decimal)ntbQty.Value);
            CheckEnabled(this, EventArgs.Empty);
        }

        private RecordIdentifier GetSelectedItemID(DualDataComboBox itemComboBox, DualDataComboBox variantComboBox)
        {

            if (variantComboBox.SelectedData != null
                && !string.IsNullOrEmpty(variantComboBox.SelectedData.ID.StringValue))
            {
                return GetReadableItemID(variantComboBox);

            }

            return GetReadableItemID(itemComboBox);
        }

        private RecordIdentifier GetReadableItemID(DualDataComboBox comboBox)
        {
            if (comboBox.SelectedData is MasterIDEntity)
            {
                return (comboBox.SelectedData as MasterIDEntity).ReadadbleID;
            }

            return (comboBox.SelectedData as DataEntity).ID;
        }
        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbQty.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }

    }
}
