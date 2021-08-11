using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects.BarCodes;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class VendorItemDialog : DialogBase
    {
        RecordIdentifier vendorID;
        RecordIdentifier itemID;
        RecordIdentifier inventoryUnitID;
        RecordIdentifier salesUnitId;
        VendorItem vendorItem;
        private RetailItem retailItem;
        private RetailItem variantItem;
        private bool itemExistsLocally;
        private bool editVendorItem;
        DecimalLimit priceLimiter;

        private bool lockEvent = false;
        private DialogResult dialogResult = DialogResult.Cancel;
        private bool isEditing;

        public VendorItem VendorItem
        {
            get
            {
                return vendorItem;
            }
        }

        public RecordIdentifier VendorID
        {
            get { return vendorID; }
        }

        /// <summary>
        /// Used to create a new vendor item
        /// </summary>
        /// <param name="vendorID">The vendor ID</param>
        private VendorItemDialog(RecordIdentifier vendorID)
            : this()
        {
            cmbRetailItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity(RecordIdentifier.Empty, Resources.SameAsSalesUnit);

            itemID = RecordIdentifier.Empty;
            this.vendorID = vendorID;
            cmbRetailItem.ShowDropDownOnTyping = true;
        }

        /// <summary>
        /// Used to edit an exisiting vendor item
        /// </summary>
        /// <param name="vendorID"></param>
        /// <param name="internalID"></param>
        private VendorItemDialog(RecordIdentifier vendorID, RecordIdentifier internalID)
            : this()
        {
            itemID = RecordIdentifier.Empty;
            this.vendorID = vendorID;
            editVendorItem = true;
            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            vendorItem = service.GetVendorItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), internalID, true);
            isEditing = true;

            cmbUnit.Enabled = true;
            cmbUnit.SelectedData = new DataEntity(vendorItem.UnitID, vendorItem.UnitDescription);

            if (vendorItem.RetailItemID == "")
            {
                throw new DataIntegrityException(typeof(VendorItem), internalID);
            }

            retailItem = PluginOperations.GetRetailItem(vendorItem.RetailItemID);
            if (retailItem == null)
            {
                return;
            }

            chkCreateAnother.Checked = false;
            chkCreateAnother.Visible = false;
            itemExistsLocally = retailItem != null;

            if (itemExistsLocally)
            {
                if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {
                    cmbRetailItem.SelectedData = new DataEntity(vendorItem.RetailItemID, vendorItem.Text);
                }
                else
                {
                    RetailItem headerItem = PluginOperations.GetRetailItem(retailItem.HeaderItemID);
                    if (headerItem == null)
                    {
                        return;
                    }

                    cmbRetailItem.SelectedData = new DataEntity(headerItem.ID, headerItem.Text);
                    cmbVariant.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);

                    cmbRetailItem_SelectedDataChanged(this, EventArgs.Empty);
                    cmbVariant_SelectedDataChanged(this, EventArgs.Empty);
                    cmbVariant.Enabled = lblVariant.Enabled = false;
                }
            }
            else
            {
                cmbRetailItem.SelectedData = new DataEntity(null, vendorItem.Text);

                cmbVariant.SelectedData = new DataEntity(null, vendorItem.VariantName != "" ? vendorItem.VariantName : "");
            }

            tbID.Text = (string)vendorItem.VendorItemID;

            inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemExistsLocally ? vendorItem.RetailItemID : vendorItem.ID, RetailItem.UnitTypeEnum.Inventory);

            ntbDefaultCostPrice.SetValueWithLimit(vendorItem.DefaultPurchasePrice, priceLimiter);

            var convertableUnits = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, vendorItem.RetailItemID, inventoryUnitID);
            if (convertableUnits.Count == 1)
            {
                cmbUnit.SelectedData = convertableUnits[0];
            }

            cmbUnit.Enabled = label6.Enabled = itemExistsLocally;
            cmbRetailItem.Enabled = lblRelation.Enabled = false;

            CheckEnabled(this, EventArgs.Empty);
        }

        private VendorItemDialog()
        {
            InitializeComponent();

            itemExistsLocally = true;
            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
        }

        /// <summary>
        /// Returns an instance of the dialog that is used to create new vendor items
        /// </summary>
        /// <param name="vendorID">The ID of the vendor to create items for</param>
        /// <returns></returns>
        public static VendorItemDialog CreateForNew(RecordIdentifier vendorID)
        {
            return new VendorItemDialog(vendorID);
        }

        /// <summary>
        /// Returns an instance of the dialog that is used to edit existing vendor items
        /// </summary>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="vendorItemID">The ID of the item to edit</param>
        /// <returns></returns>
        public static VendorItemDialog CreateForEditing(RecordIdentifier vendorID, RecordIdentifier vendorItemID)
        {
            return new VendorItemDialog(vendorID, vendorItemID);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                if (chkCreateAnother.Checked)
                {
                    dialogResult = DialogResult.OK;
                    SetDefaults();
                    tbID.Focus();
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

        private void cmbRetailItem_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbRetailItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(
                PluginEntry.DataModel,
                false,
                initialSearchText,
                SearchTypeEnum.InventoryItems,
                textInitallyHighlighted, null, true);
        }

        private void cmbRetailItem_Leave(object sender, EventArgs e)
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
            else
            {
                ntbDefaultCostPrice.Focus();
            }

            lockEvent = false;
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {

        }

        private void cmbRetailItem_SelectedDataChanged(object sender, EventArgs e)
        {
            retailItem = PluginOperations.GetRetailItem(cmbRetailItem.SelectedData.ID);
            if (retailItem == null)
            {
                return;
            }

            cmbVariant.Enabled = lblVariant.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem;
            itemID = cmbRetailItem.SelectedData.ID;

            if (sender is DualDataComboBox || e == null)
            {
                BarCode barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);
                tbBarcode.Text = barCode != null ? (string)barCode.ItemBarCode : "";

                if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {
                    SetUnitData(cmbRetailItem.SelectedData.ID);
                }

                errorProvider1.Clear();
            }
            else if (sender is TextBox)
            {
                if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {
                    cmbRetailItem.SelectedData.Text = retailItem.Text;
                    cmbRetailItem.Text = retailItem.Text;
                    cmbVariant.SelectedData = new DataEntity("", "");
                    variantItem = null;
                    SetUnitData(cmbRetailItem.SelectedData.ID);
                }
                else
                {
                    RecordIdentifier HeaderItemID = Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, retailItem.HeaderItemID);
                    cmbRetailItem.SelectedData = new DataEntity(HeaderItemID, retailItem.Text);
                    cmbRetailItem.Text = retailItem.Text;
                    cmbVariant.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);
                    cmbVariant.Text = retailItem.VariantName;
                    cmbVariant.Enabled = true;
                    SetUnitData(cmbVariant.SelectedData.ID);
                }
            }

            if (retailItem.ItemType == ItemTypeEnum.MasterItem && String.IsNullOrEmpty(cmbVariant.SelectedData.ID.ToString()))
            {
                VariantWantsFocus();
            }
            else
            {
                UnitWantsFocus();
            }

            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbUnit_DropDown(object sender, DropDownEventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, itemID, inventoryUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Resources.Convertible, Resources.All },
                300);

            e.ControlToEmbed = pnl;
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            variantItem = PluginOperations.GetRetailItem(cmbVariant.SelectedData.ID);
            if (variantItem == null)
            {
                return;
            }

            if (!editVendorItem)
            {
                SetUnitData(cmbVariant.SelectedData.ID);
            }

            UnitWantsFocus();
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbDefaultCostPrice_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void tbBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(tbBarcode.Text))
            {
                tbBarcode_KeyDown(sender, e);
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
                    cmbRetailItem.Select();
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

        private void VendorItemDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(tbBarcode.Text))
                {
                    tbBarcode_KeyDown(sender, e);
                }
            }
        }

        private void SetDefaults()
        {
            tbID.Text = "";
            tbBarcode.Text = "";
            cmbRetailItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity(RecordIdentifier.Empty, Resources.SameAsSalesUnit);
            cmbUnit.Enabled = false;
            ntbDefaultCostPrice.Value = 0;

            btnOK.Enabled = false;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            cmbUnit.Enabled = !(cmbRetailItem.SelectedData.ID == "") && itemExistsLocally;

            if (retailItem != null && variantItem != null)
            {
                itemID = retailItem.ItemType == ItemTypeEnum.MasterItem ? variantItem.ID : retailItem.ID;
            }
            else
            {
                itemID = cmbRetailItem.SelectedData.ID;
            }

            if (inventoryUnitID == null && retailItem != null)
            {
                if (retailItem.ItemType != ItemTypeEnum.MasterItem)
                {
                    errorProvider1.SetError(cmbRetailItem, Resources.InventoryUnitMissing);
                    cmbUnit.Enabled = false;
                    return;
                }
                else if (variantItem != null)
                {
                    errorProvider1.SetError(cmbRetailItem, Resources.InventoryUnitMissing);
                    cmbUnit.Enabled = false;
                    return;
                }
            }

            if (vendorItem == null)
            {
                btnOK.Enabled = cmbRetailItem.SelectedData.ID != ""
                                && cmbUnit.SelectedData.ID != "";

            }
            else
            {
                bool itemTheSame = true;

                itemTheSame = itemID != vendorItem.RetailItemID;

                btnOK.Enabled = cmbRetailItem.SelectedData.ID != ""
                    && cmbUnit.SelectedData.ID != ""
                    &&
                    (tbID.Text != vendorItem.VendorItemID ||
                     itemTheSame ||
                     cmbUnit.SelectedData.ID != vendorItem.UnitID ||
                     (decimal)ntbDefaultCostPrice.Value != vendorItem.DefaultPurchasePrice);
            }

            // Prevent the user from assignign a variant header to the vendor item
            if (retailItem != null && retailItem.ItemType == ItemTypeEnum.MasterItem && variantItem == null)
            {
                btnOK.Enabled = false;
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
                errorProvider1.SetError(tbBarcode, error);
                tbBarcode.Focus();
                return;
            }

            if (ItemID != RecordIdentifier.Empty)
            {
                cmbRetailItem.SelectedData = new DataEntity { ID = ItemID };
            }
            cmbRetailItem_SelectedDataChanged(sender, e);
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

        private bool Save()
        {
            bool saved = false;

            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {
                itemID = cmbRetailItem.SelectedData.ID;

                itemID = itemExistsLocally ? (retailItem.ItemType == ItemTypeEnum.MasterItem ? variantItem.ID : retailItem.ID) : vendorItem.RetailItemID;

                RecordIdentifier oldRecordID = "";

                if (vendorItem != null)
                {
                    oldRecordID = vendorItem.ID;
                }

                if (service.VendorItemExistsExcludingCurrentRecordByVendorIDRetailIDAndUnitID(PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(), vendorID, itemID, cmbUnit.SelectedData.ID, oldRecordID, true))
                {
                    errorProvider1.SetError(cmbUnit, Resources.VendorItemAndUnitAlreadyExists);
                    return false;
                }

                if (tbID.Text != "" &&
                    service.VendorItemExistsExcludingCurrentRecord(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorID, tbID.Text, oldRecordID, true))
                {
                    errorProvider1.SetError(tbID, Resources.VendorItemExists);
                    return false;
                }

                //Create the vendor item as the checks below all use it - if the vendor item is new the following unit checks will error out
                if (!isEditing)
                {
                    vendorItem = new VendorItem();
                    vendorItem.VendorID = vendorID;
                }

                vendorItem.VendorItemID = tbID.Text;

                if (itemExistsLocally)
                {
                    vendorItem.RetailItemID = itemID;
                    vendorItem.UnitID = cmbUnit.SelectedData.ID;
                    vendorItem.VendorDescription = retailItem.ItemType == ItemTypeEnum.MasterItem
                        ? variantItem.VariantName
                        : retailItem.Text;
                }

                vendorItem.UnitDescription = cmbUnit.SelectedData.Text;
                vendorItem.DefaultPurchasePrice = (decimal)ntbDefaultCostPrice.Value;

                DataEntity retailItemDataEntity = new DataEntity(itemID, itemExistsLocally ? (retailItem.ItemType == ItemTypeEnum.MasterItem ? variantItem.Text : retailItem.Text) : vendorItem.Text);

                bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(retailItemDataEntity, cmbUnit.SelectedData.ID);
                if (!unitConversionExists)
                {
                    MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                    cmbUnit.Text = "";
                    return false;
                }

                vendorItem.ID = service.SaveVendorItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorItem, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "VendorItem",
                    vendorItem.ID, null);

                // Check if this is the first item for the vendor, if so he should be the items default vendor
                if (!Providers.RetailItemData.ItemHasDefaultVendor(PluginEntry.DataModel, itemID))
                {
                    Providers.RetailItemData.SetItemsDefaultVendor(PluginEntry.DataModel, itemID, vendorItem.VendorID);
                }

                saved = true;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                saved = false;
            }

            return saved;
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

            ntbDefaultCostPrice.Focus();
        }
    }
}