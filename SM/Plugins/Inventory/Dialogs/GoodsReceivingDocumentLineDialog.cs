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
using LSOne.DataLayer.BusinessObjects.Profiles;
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
    public partial class GoodsReceivingDocumentLineDialog : DialogBase
    {
        GoodsReceivingDocument goodsReceivingDocument;
        GoodsReceivingDocumentLine goodsReceivingDocumentLine;
        private RetailItem retailItem;
        private bool variantWantsFocus;
        bool isNewLine;
        private DialogResult dialogResult;
        private RecordIdentifier barcodeUnitID;

        private bool lockEvent = false;
        private bool enterPressed = false;
        private List<Unit> UnitListForItemAndVariant;

        private SiteServiceProfile siteServiceProfile;
        IInventoryService service = null;
        private bool includedInPurchaseOrder;

        public GoodsReceivingDocumentLineDialog(GoodsReceivingDocument goodsReceivingDocument, GoodsReceivingDocumentLine goodsReceivingDocumentLine)
            : this()
        {
            isNewLine = false;
            cmbItem.Enabled = false;
            cmbVariant.Enabled = false;
            cmbUnit.Enabled = false;
            tbBarcode.Enabled = false;
            chkCreateAnother.Visible = false;
            chkCreateAnother.CheckState = CheckState.Unchecked;
            this.goodsReceivingDocument = goodsReceivingDocument;
            this.goodsReceivingDocumentLine = goodsReceivingDocumentLine;

            LoadData();
        }

        public GoodsReceivingDocumentLineDialog(GoodsReceivingDocument goodsReceivingDocument)
           : this()
        {
            isNewLine = true;
            chkCreateAnother.Visible = true;
            chkCreateAnother.Checked = true;
            this.goodsReceivingDocument = goodsReceivingDocument;
        }

        private GoodsReceivingDocumentLineDialog()
        {
            siteServiceProfile = PluginOperations.GetSiteServiceProfile();
            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            InitializeComponent();
            SetDefaults();

            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
            barcodeUnitID = RecordIdentifier.Empty;
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbItem.SelectedData.ID != "")
            {
                retailItem = PluginOperations.GetRetailItem(cmbItem.SelectedData.ID);

                if (retailItem == null || retailItem.InventoryExcluded)
                {
                    cmbItem.SelectedData = new DataEntity("", "");
                    cmbItem.SelectedDataID = null;
                    tbBarcode.Focus();
                    return;
                }

                bool hasVariant = retailItem.ItemType == ItemTypeEnum.MasterItem;
                
                lblVariant.Enabled = cmbVariant.Enabled = hasVariant;
                cmbUnit.Enabled = (retailItem != null);

                dtpReceivedDate.Enabled = true;
                ntbReceivedQuantity.Enabled = true;

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
                else if (sender is TextBox)
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
                        lblVariant.Enabled = cmbVariant.Enabled = true;
                    }
                }

                if (hasVariant && RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID))
                {
                    VariantWantsFocus();
                }
                else if (retailItem.VariantName != "")
                {
                    lblVariant.Enabled = cmbVariant.Enabled = true;
                    cmbVariant.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);
                    VariantWantsFocus();
                }
                else
                {
                    cmbVariant.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                }
                if (hasVariant && RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID) )
                {
                    variantWantsFocus = true;
                }

                ReloadAndSetUnitList();
                UnitWantsFocus();
                CheckEnabled(sender,e);
                SetQuantityAllowDecimals((decimal)ntbOrderedQuantity.Value, ntbOrderedQuantity);
            }
        }

        private void LoadData()
        {
            retailItem = PluginOperations.GetRetailItem(goodsReceivingDocumentLine.purchaseOrderLine.ItemID);

            if (retailItem == null)
            {
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(
                                        goodsReceivingDocumentLine.purchaseOrderLine.ItemID,
                                        goodsReceivingDocumentLine.purchaseOrderLine.ItemName);

                cmbItem_SelectedDataChanged(this, null);
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
                                        goodsReceivingDocumentLine.purchaseOrderLine.ItemID,
                                        goodsReceivingDocumentLine.purchaseOrderLine.VariantName);

                if (isNewLine)
                {
                    cmbItem_SelectedDataChanged(this, null);
                }
                cmbVariant_SelectedDataChanged(this, null);
            }

            dtpReceivedDate.Value = goodsReceivingDocumentLine.ReceivedDate;

            Unit unit = new Unit();
            unit.ID = goodsReceivingDocumentLine.purchaseOrderLine.UnitID;
            unit.Text = goodsReceivingDocumentLine.purchaseOrderLine.UnitName;
            cmbUnit.SelectedData = unit;
            cmbUnit.Enabled = false;

            SetQuantityAllowDecimals(goodsReceivingDocumentLine.purchaseOrderLine.Quantity, ntbOrderedQuantity);
            SetQuantityAllowDecimals(goodsReceivingDocumentLine.ReceivedQuantity, ntbReceivedQuantity);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

            if (retailItem.ItemType == ItemTypeEnum.MasterItem)
            {
                btnOK.Enabled = (string)target != ""
                            && (ntbReceivedQuantity.Value > 0)
                            && (cmbUnit.SelectedData != null)
                            && (cmbVariant.Enabled && !string.IsNullOrEmpty(cmbVariant.SelectedDataID.StringValue));
            }
            else if (retailItem.ItemType == ItemTypeEnum.Item)
            {
                btnOK.Enabled = (string) target != ""
                                && (ntbReceivedQuantity.Value > 0)
                                && (cmbUnit.SelectedData != null);
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SelectedID
        {
            get { return goodsReceivingDocumentLine.ID; }
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        private void btnOK_Click(object sender, EventArgs e)
        {
            RecordIdentifier goodsReceivingDocumentID = (string) goodsReceivingDocument.GoodsReceivingID;

            RecordIdentifier itemID = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);
            Unit unit = (Unit) cmbUnit.SelectedData;
            RecordIdentifier unitID = unit.ID;
            RecordIdentifier storeID = goodsReceivingDocument.StoreID;

            decimal receivedQuantity = (decimal) ntbReceivedQuantity.Value;

            if (receivedQuantity == 0)
            {
                MessageDialog.Show(Resources.InsertQuantity);
                return;
            }

            bool overReceivingOK = PluginOperations.CheckMaxOverReceivedOK(
                goodsReceivingDocumentID,
                goodsReceivingDocument.PurchaseOrderID,
                itemID,
                unitID,
                receivedQuantity,
                goodsReceivingDocumentLine == null ? RecordIdentifier.Empty : goodsReceivingDocumentLine.LineNumber
                );

            if (!overReceivingOK)
            {
                return;
            }

            DateTime receivedDate = dtpReceivedDate.Value;

            RecordIdentifier lineNumber = goodsReceivingDocumentLine?.LineNumber;
            if (!includedInPurchaseOrder)
            {
                PurchaseOrderLine purchaseOrderLine;
                PurchaseOrder purchaseOrder =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetPurchaseOrder(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                            goodsReceivingDocumentID, false);
                
                purchaseOrderLine = new PurchaseOrderLine();
                purchaseOrderLine.PurchaseOrderID = goodsReceivingDocumentID;
                purchaseOrderLine.VendorItemID = (string)purchaseOrder.VendorID;

                purchaseOrderLine.ItemID = (string)itemID;
                purchaseOrderLine.UnitID = (string)unitID;
                purchaseOrderLine.Quantity = 0;
                purchaseOrderLine.DiscountAmount = 0;
                purchaseOrderLine.DiscountPercentage = 0;
                purchaseOrderLine.TaxCalculationMethod = TaxCalculationMethodEnum.NoTax;
                purchaseOrderLine.TaxAmount = 0;
                purchaseOrderLine.UnitPrice = 0;

                lineNumber =  Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SavePurchaseOrderLine(
                     PluginEntry.DataModel,
                     PluginOperations.GetSiteServiceProfile(),
                     purchaseOrderLine,
                     true);
            }
            
            goodsReceivingDocumentLine = PluginOperations.SaveGoodsReceivingDocumentLine(
                goodsReceivingDocumentID,
                itemID,
                unitID,
                lineNumber,
                (string) storeID,
                receivedQuantity,
                receivedDate);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                "GoodsReceivingDocumentLine",
                goodsReceivingDocument.ID,
                null);

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

        private void SetDefaults()
        {
            cmbItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            tbBarcode.Text = "";
            ntbOrderedQuantity.Value = 0;
            ntbReceivedQuantity.Value = 0;

            includedInPurchaseOrder = false;
            goodsReceivingDocumentLine = null;

            btnOK.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (!RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID))
            {
                retailItem = PluginOperations.GetRetailItem((cmbVariant.SelectedData as MasterIDEntity).ReadadbleID);
            }

            ReloadAndSetUnitList();
            SetQuantityAllowDecimals((decimal)ntbOrderedQuantity.Value, ntbOrderedQuantity);

            CheckEnabled(this, EventArgs.Empty);

            UnitWantsFocus();
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            Unit unit = (Unit)cmbUnit.SelectedData;
            RecordIdentifier unitID = unit.ID;

            ntbReceivedQuantity.DecimalLetters = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, unitID).Max;

            CheckIfPurchaseOrderLineExists(cmbItem.SelectedData.ID, unitID);

            CheckEnabled(this, EventArgs.Empty);

            SetQuantityAllowDecimals((decimal)ntbOrderedQuantity.Value, ntbOrderedQuantity);

            ntbReceivedQuantity.Focus();
        }

        private void ReloadAndSetUnitList()
        {
            // Reload the unit list
            RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

            try
            {
                UnitListForItemAndVariant = service.GetUnitsForPurchaserOrderItemVariant(PluginEntry.DataModel,
                    siteServiceProfile,
                    goodsReceivingDocument.PurchaseOrderID,
                    target, true);

                if (!includedInPurchaseOrder)
                {

                    PurchaseOrder purchaseOrder =
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .GetPurchaseOrder(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                goodsReceivingDocument.PurchaseOrderID, false);

                    UnitListForItemAndVariant = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetDistinctUnitsForVendorItem(
                        PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        purchaseOrder.VendorID,
                        target,
                        true);

                    if (UnitListForItemAndVariant.Count == 0)
                    {
                        UnitListForItemAndVariant = Providers.UnitConversionData.GetConvertableTo(
                            PluginEntry.DataModel, target,
                            Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, target,RetailItem.UnitTypeEnum.Inventory));
                    }
                }

                // Display the top item of the Unit list in the unit combo box

                Unit unit ;
                if (goodsReceivingDocumentLine == null)
                {
                    unit = GetUnit(retailItem);
                }
                else
                {
                    unit =
                        UnitListForItemAndVariant.FirstOrDefault(
                            f => f.ID == goodsReceivingDocumentLine.purchaseOrderLine.UnitID);
                }

                if (cmbUnit.SelectedData != unit)
                {
                    cmbUnit.SelectedData = unit;
                }
               
                cmbUnit.Enabled = (UnitListForItemAndVariant.Count > 1);
                
                SetOrderedQtyBasedOnItemInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private Unit GetUnit(RetailItem item)
        {
            Unit unit = new Unit();
            //If the barcode scanned has a unit on it use that
            if (barcodeUnitID != RecordIdentifier.Empty)
            {
                unit = UnitListForItemAndVariant.FirstOrDefault(f => f.ID == barcodeUnitID);
                if (unit != null)
                {
                    return unit;
                }
            }

            //If the purchase unit ID is in the list then use that
            RecordIdentifier purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, item.ID, RetailItem.UnitTypeEnum.Purchase);
            unit = UnitListForItemAndVariant.FirstOrDefault(f => f.ID == purchaseUnitId);
            if (unit != null)
            {
                return unit;
            }

            //otherwise use the first unit
            unit = UnitListForItemAndVariant.FirstOrDefault();
            if (unit != null)
            {
                return unit;
            }

            return new Unit();
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            cmbUnit.SetData(UnitListForItemAndVariant, null);
        }
   
        private void VariantWantsFocus()
        {
            if (cmbVariant.Enabled && (cmbVariant.SelectedDataID == null || string.IsNullOrEmpty((string)cmbVariant.SelectedDataID)))
            {
                cmbVariant.Focus();
                return;
            }
            UnitWantsFocus();
        }

        private void UnitWantsFocus()
        {
            if (cmbUnit.Enabled && (cmbUnit.SelectedDataID == null || string.IsNullOrEmpty((string)cmbUnit.SelectedDataID)))
            {
                cmbUnit.Focus();
                return;
            }
            ntbReceivedQuantity.Focus();
        }

        private void SetOrderedQtyBasedOnItemInfo()
        {
            errorProvider1.Clear();
            RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

            Unit unit = (Unit) cmbUnit.SelectedData;

            CheckIfPurchaseOrderLineExists(target, unit.ID);
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        private void CheckIfPurchaseOrderLineExists(RecordIdentifier itemID, RecordIdentifier unitID)
        {
            errorProvider1.Clear();

            RecordIdentifier purchaseOrderID = goodsReceivingDocument.PurchaseOrderID;
            try
            {
                RecordIdentifier purchaseOrderLineNumber =
                    service.GetPurchaseOrderLineNumberFromItemInfo(PluginEntry.DataModel, siteServiceProfile,
                        purchaseOrderID,
                        itemID,
                        unitID,
                        false);

                if (purchaseOrderLineNumber == "0")
                {
                    if (!cmbVariant.Enabled || (cmbVariant.SelectedDataID != RecordIdentifier.Empty))
                    {
                        errorProvider1.SetError(tbBarcode, Resources.ItemNotInPurchaseOrderWillBeAdded);
                        includedInPurchaseOrder = false;
                        ntbOrderedQuantity.Value = 0;
                    }
                }
                else
                {
                    includedInPurchaseOrder = true;
                    RecordIdentifier purchaseOrderLineID = new RecordIdentifier(purchaseOrderID, purchaseOrderLineNumber);

                    PurchaseOrderLine pol = service.GetPurchaseOrderLine(
                        PluginEntry.DataModel, 
                        siteServiceProfile,
                        purchaseOrderLineID, 
                        PluginEntry.DataModel.CurrentStoreID, 
                        false, 
                        true);

                    ntbOrderedQuantity.Value = (double)pol.Quantity;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void cmbItem_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "" || cmbItem.SelectedData == null)
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity) cmbItem.SelectedData).ID;
                textInitallyHighlighted = true;
            }
            try
            {
                SingleSearchPanel control1 = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText,
                  SearchTypeEnum.InventoryItems,
                  goodsReceivingDocument.PurchaseOrderID, SingleSearchPanel.FilterEnum.ItemsForSpecificPurchaseOrder,
                  textInitallyHighlighted);

                SingleSearchPanel control2 = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.InventoryItems, textInitallyHighlighted, null, true);

                e.ControlToEmbed = new TabbedCustomDualDataPanel(new IControlClosable[] { control1, control2 }, new[] {Resources.Ordered, Resources.All }, 350, 284);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
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
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
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

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            if (btnCancel.Focused)
            {
                return;
            }

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

            lockEvent = true;
            RecordIdentifier ItemID = RecordIdentifier.Empty;
            barcodeUnitID = RecordIdentifier.Empty;

            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    ItemID = barCode.ItemID;
                    barcodeUnitID = barCode.UnitID;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
                {
                    ItemID = tbBarcode.Text;
                }
            }
            else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
            {
                ItemID = tbBarcode.Text;
            }
            string error = string.Empty;
            bool itemValid = PluginOperations.CheckRetailItem(ItemID, out error);

            if (!itemValid)
            {
                errorProvider1.SetError(tbBarcode, error);
                if (enterPressed)
                {
                    tbBarcode.Focus();
                }
                enterPressed = false;
                return;
            }

            if (ItemID != RecordIdentifier.Empty)
            {
                cmbItem.SelectedData = new DataEntity { ID = ItemID };
                cmbItem_SelectedDataChanged(sender, e);

                if (retailItem != null && !retailItem.InventoryExcluded)
                {
                    cmbItem.Select();
                }

                if (variantWantsFocus)
                {

                    cmbVariant.Focus();
                    variantWantsFocus = false;
                }
                else if (retailItem != null && !retailItem.InventoryExcluded)
                {
                    ntbReceivedQuantity.Focus();
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
                        cmbVariant.Focus();
                    }
                    else
                    {
                        ntbReceivedQuantity.Focus();
                    }
                }
            }
            lockEvent = false;
        }

        private void GoodsReceivingDocumentLineDialog_Shown(object sender, EventArgs e)
        {
            //For some bizare reason the received date component always got the focus even if the 
            //"edit" constructor specifically set the focus on received quantity. 
            //The only way to to get the focus on the received quantity was to put this here
            if (!isNewLine)
            {
                ntbReceivedQuantity.Focus();
            }
        }

        private void GoodsReceivingDocumentLineDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(tbBarcode.Text))
                {
                    enterPressed = true;
                    tbBarcode_KeyDown(sender, e);
                }
            }
        }

        private void tbBarcode_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBarcode.Text))
            {
                tbBarcode.SelectAll();
            }
        }

        private void tbBarcode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBarcode.Text))
            {
                tbBarcode.SelectAll();
            }
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue, NumericTextBox textBox)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            textBox.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}