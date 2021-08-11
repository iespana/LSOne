using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
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
    public partial class StockCountingLineDialog : DialogBase
    {
        InventoryAdjustment stockCounting;
        private InventoryJournalTransaction stockCountingLine;
        private bool enabled;
        private bool variantWantsFocus;
        private DialogResult dialogResult = DialogResult.Cancel;
        private RecordIdentifier storeID;
        private RecordIdentifier purchaseUnitId;
        private RecordIdentifier inventoryUnitId;
        private RetailItem retailItem;
        private bool lockEvent = false;
        private bool variantItem = false;
        private int numberOfSaves = 0;

        private IInventoryService service = null;

        public AdjustmentStatus JournalStatus { get; private set; }

        public StockCountingLineDialog(RecordIdentifier stockCountingID, RecordIdentifier storeID, RecordIdentifier inventoryJournalTransactionID)
            : this(stockCountingID, storeID)
        {
            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            stockCountingLine = service.GetInventoryJournalTransaction(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), inventoryJournalTransactionID, true);

            RetailItem retailItem = PluginOperations.GetRetailItem(stockCountingLine.ItemId);
            if (retailItem == null)
            {
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(stockCountingLine.ItemId, stockCountingLine.ItemName);

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
                cmbVariant.SelectedData = new DataEntity(stockCountingLine.ItemId, stockCountingLine.VariantName);

                cmbItem_SelectedDataChanged(this, null);
                cmbVariant_SelectedDataChanged(this, null);
                variantItem = true;
            }

            cmbUnit.SelectedData = new DataEntity(stockCountingLine.UnitID, stockCountingLine.UnitDescription);
            chkCreateAnother.Enabled = chkCreateAnother.Visible = false;
            chkCreateAnother.CheckState = CheckState.Unchecked;
            cmbItem.Enabled = false;
            cmbVariant.Enabled = false;
            tbBarcode.Enabled = false;
            ntbQty.Value = (double)stockCountingLine.Counted;

            SetQuantityAllowDecimals(stockCountingLine.Counted);
        }

        public StockCountingLineDialog(RecordIdentifier stockCountingID, RecordIdentifier storeID)
        {
            InitializeComponent();
            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            stockCounting = service.GetInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCountingID, true);
            stockCountingLine = null;
            cmbItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");

            this.storeID = storeID;

            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (cmbVariant.Enabled && string.IsNullOrEmpty(cmbVariant.SelectedDataID.StringValue))
            {
                btnOK.Enabled = false;
                return;
            }

            RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);

            enabled = (string)target != ""
                      && cmbUnit.SelectedData != null
                      && (string)cmbUnit.SelectedData.ID != ""
                      && ntbQty.Value >= 0;

            btnOK.Enabled = enabled;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            (bool CanEdit, AdjustmentStatus Status) processingStatus = PluginOperations.CheckJournalProcessingStatus(stockCounting.ID);
            JournalStatus = processingStatus.Status;

            if(processingStatus.CanEdit)
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

                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "AddEditStockCountLine", stockCounting.ID, null);
                }
            }
            else
            {
                DialogResult = DialogResult.Abort;
                Close();
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
                initialSearchText = cmbItem.SelectedData.ID;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.InventoryItems, textInitallyHighlighted, null, true);
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (cmbItem.SelectedData != null)
            {
                var unitId = (purchaseUnitId != "") ? purchaseUnitId : inventoryUnitId;

                var data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbItem.SelectedData.ID, unitId),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

                TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                    cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                    data,
                    new string[] { Resources.Convertible, Resources.All },
                    250);

                cmbUnit.SetData(data, pnl);
            }
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {

            SetItemInformation(sender, e);

            SetQuantityAllowDecimals((decimal)ntbQty.Value);

            CheckEnabled(this, EventArgs.Empty);
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

        private void SetItemInformation(object sender, EventArgs e)
        {
            bool isVariant = false;
            if (cmbVariant.SelectedData != null && !string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue))
            {
                retailItem = PluginOperations.GetRetailItem((cmbVariant.SelectedData as MasterIDEntity).ReadadbleID);
                isVariant = true;
            }
            else
            {
                retailItem = PluginOperations.GetRetailItem(cmbItem.SelectedData.ID);
            }

            if (retailItem == null || retailItem.InventoryExcluded)
            {
                return;
            }

            purchaseUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, retailItem.ID, RetailItem.UnitTypeEnum.Purchase);
            inventoryUnitId = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, retailItem.ID, RetailItem.UnitTypeEnum.Sales);
            bool hasVariant = retailItem.ItemType == ItemTypeEnum.MasterItem;

            if (!isVariant)
            {
                cmbVariant.Enabled = hasVariant;
            }
            else
            {
                cmbVariant.Enabled = true;
            }

            cmbUnit.Enabled = (retailItem != null);
            CheckEnabled(sender, e);

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

                errorProvider.Clear();
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
                    cmbVariant.Enabled = true;
                }
            }

            var unit = Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitId);
            cmbUnit.SelectedData = unit;

            if (hasVariant && String.IsNullOrEmpty(cmbVariant.SelectedData.ID.ToString()))
            {
                variantWantsFocus = true;
                cmbVariant.Focus();
            }
            else
            {
                ntbQty.Focus();
            }
        }

        private void CheckBarCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text))
            {
                return;
            }

            RecordIdentifier ItemID = RecordIdentifier.Empty;
            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);

            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel, BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    ItemID = barCode.ItemID;
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
                errorProvider.SetError(tbBarcode, error);
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
                else
                {
                    ntbQty.Focus();
                }
            }
            else
            {
                cmbItem_SelectedDataChanged(sender, e);
            }
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            bool hasUnitConversion = PluginOperations.UnitConversionWithInventoryUnitExists(PluginOperations.CreateItemDataEntity(cmbItem, cmbVariant), cmbUnit.SelectedData.ID);

            if (!hasUnitConversion)
            {
                cmbUnit.SelectedData = new DataEntity("", "");
                SetQuantityAllowDecimals((decimal)ntbQty.Value);
            }

            CheckEnabled(sender, e);
        }

        private void SetDefaults()
        {
            cmbItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            tbBarcode.Text = "";
            errorProvider.Clear();
            ntbQty.Value = 0;

            btnOK.Enabled = false;
        }

        private bool Save()
        {
            DataEntity selectedItem;

            if ((cmbVariant.Enabled || variantItem) && !string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue))
            {
                selectedItem = new DataEntity(PluginOperations.GetReadableItemID(cmbVariant), cmbVariant.SelectedData.Text);
            }
            else
            {
                selectedItem = new DataEntity(PluginOperations.GetReadableItemID(cmbItem), cmbItem.SelectedData.Text);
            }

            var selectedUnit = (DataEntity)cmbUnit.SelectedData;
            var countedItems = (decimal)ntbQty.Value;

            RetailItem retailItem = PluginOperations.GetRetailItem(selectedItem.ID);
            if (retailItem.IsAssemblyItem)
            {
                RetailItemAssembly assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(PluginEntry.DataModel, selectedItem.ID, storeID);
                if (assembly == null)
                {
                    MessageDialog.Show(Resources.AssemblyItemIsNotDefinedForThisStore);
                    return false;
                }

                numberOfSaves += SaveAssemblyComponents(retailItem, countedItems, selectedUnit.ID);
            }
            else
            {
                PluginOperations.SaveJournalTransaction(stockCounting.ID, storeID, selectedItem.ID, selectedUnit.ID, inventoryUnitId, 
                                                        countedItems, stockCountingLine?.LineNum, stockCountingLine?.AreaID, stockCountingLine?.PictureID);

                numberOfSaves++;
            }

            lblFeedback.Text = numberOfSaves == 1 ? Resources.OneLineHasBeenAdded : string.Format(Resources.LinesHaveBeenAdded, numberOfSaves);

            return true;
        }

        private int SaveAssemblyComponents(RetailItem retailItem, decimal itemQuantity, RecordIdentifier itemUnitID)
        {
            int saveCount = 0;

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                PluginEntry.DataModel, retailItem.ID, itemUnitID, retailItem.SalesUnitID, itemQuantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(
                PluginEntry.DataModel, retailItem.ID, storeID, false);

            if (assembly != null)
            {
                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = PluginOperations.GetRetailItem(component.ItemID, false);
                    if (componentItem.IsAssemblyItem)
                    {
                        saveCount += SaveAssemblyComponents(componentItem, componentQuantity, component.UnitID);
                    }
                    else if (componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        PluginOperations.SaveJournalTransaction(stockCounting.ID, storeID, component.ItemID, component.UnitID, 
                                                                componentItem.InventoryUnitID, componentQuantity);

                        saveCount++;
                    }
                }
            }

            return saveCount;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = (dialogResult == DialogResult.OK)
                               ? DialogResult = DialogResult.OK
                               : DialogResult = DialogResult.Cancel;
            Close();

            var isAddition = stockCountingLine == null;
            if (isAddition && numberOfSaves > 0)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "AddEditStockCountLine", stockCounting.ID, null);
            }
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, e);

            if (sender is DualDataComboBox || e == null)
            {
                BarCode barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, PluginOperations.GetReadableItemID(cmbVariant));

                if (barCode != null)
                {
                    tbBarcode.Text = (string)barCode.ItemBarCode;
                }
                else
                {
                    tbBarcode.Text = "";
                }
                errorProvider.Clear();
            }
            if (cmbUnit.SelectedData == null || String.IsNullOrEmpty(cmbUnit.SelectedData.ID.ToString()))
            {
                cmbUnit.Focus();
            }
            if (btnOK.Enabled)
            {
                ntbQty.Focus();
            }

            SetItemInformation(sender, e);

            SetQuantityAllowDecimals((decimal)ntbQty.Value);
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

        private void cmbItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                tbBarcode.Focus();
            }
            lockEvent = true;
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
                        ntbQty.Focus();
                    }
                }
            }
            lockEvent = false;
        }

        private void cmbVariant_Leave(object sender, EventArgs e)
        {
            if (retailItem != null)
            {
                if (!lockEvent)
                {
                    ntbQty.Focus();
                }
            }
            lockEvent = false;
        }

        private void cmbVariant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                cmbItem.Focus();
            }
        }

        bool enterPressed = false;
        private void tbBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(tbBarcode.Text))
                {
                    enterPressed = true;
                    cmbItem.Select();
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
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbQty.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}