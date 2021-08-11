using DevExpress.XtraDashboardLayout;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class AdjustmentLineDialog : DialogBase
    {
        private InventoryJournalTransaction journalTransactionLine;
        private InventoryJournalTypeEnum journalType;
        private CostCalculation costCalculation;

        private RecordIdentifier selectedJournalID;
        private RecordIdentifier storeId;
        private RecordIdentifier inventoryUnitID;
        private RecordIdentifier itemRecId;
        private RecordIdentifier origUnitId;
        private RetailItem retailItem;
        private BarCode barCode;
        private List<Unit> convertableUnits;

        private decimal adjustmentInOrigInventoryUnit;
        private string selectedItemId = "";
        private string selectedItemText = "";
        private string reasonId = "";
        private string reasonText = "";
        private bool lockEvent = false;
        private bool enterPressed = false;

        private AdjustmentLineDialog()
        {
            InitializeComponent();
            cmbUnit.SelectedData = new DataEntity("", "");
            cmbRelation.SelectedData = new DataEntity("", "");
            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
        }

        public AdjustmentLineDialog(RecordIdentifier selectedJournalID, RecordIdentifier storeId, InventoryJournalTypeEnum typeOfAdjustment)
            : this()
        {
            journalType = typeOfAdjustment;

            ntbQuantity.MinValue = -100000;
            ntbQuantity.MaxValue = 100000;
            switch (journalType)
            {
                case InventoryJournalTypeEnum.Reservation:
                    Text = Resources.StockReservationLine;
                    break;
                case InventoryJournalTypeEnum.Parked:
                    Text = Resources.ParkedInventoryLine;
                    ntbQuantity.MinValue = -100000;
                    ntbQuantity.MaxValue = 0;
                    break;
                default:
                    break;
            }

            this.storeId = storeId;
            this.selectedJournalID = selectedJournalID;

            btnOK.Enabled = false;
            CheckCmbEnabledStates();
        }

#region DialogBase

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

#endregion

        private void AdjustmentLineDialog_KeyUp(object sender, KeyEventArgs e)
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

        private void AdjustmentLineDialog_Paint(object sender, PaintEventArgs e)
        {
            Color borderColor = Color.FromArgb(164, 164, 164);

            Rectangle r = new Rectangle(pnlSwitch.Location.X - 1, pnlSwitch.Location.Y - 1, pnlSwitch.Width + 1, pnlSwitch.Height + 1);
            using (Pen p = new Pen(borderColor, 1))
            {
                e.Graphics.DrawRectangle(p, r);
            }
        }

        private void tbBarcode_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBarcode.Text))
            {
                tbBarcode.SelectAll();
            }
        }

        private void tbBarcode_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBarcode.Text))
            {
                tbBarcode.SelectAll();
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
                    cbCreateAnother.Select();
                }
                else
                {
                    cmbRelation.Select();
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
                CheckBarCode();
            }

            lockEvent = false;
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbRelation.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.InventoryItems, textInitallyHighlighted, null, true);
        }

        private void cmbRelation_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                itemRecId = ((DataEntity)e.Data).ID;
                selectedItemId = (string)((DataEntity)e.Data).ID;
                selectedItemText = ((DataEntity)e.Data).Text;
                e.TextToDisplay = selectedItemText;
            }

            CheckEnabled();
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbUnit.Text = "";

            if (cmbRelation.SelectedData.ID != "")
            {
                lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;

                cmbVariantNumber.SelectedData = new DataEntity();

                retailItem = PluginOperations.GetRetailItem(cmbRelation.SelectedData.ID);
                if (retailItem == null || retailItem.InventoryExcluded)
                {
                    return;
                }

                origUnitId = retailItem.InventoryUnitID;
                selectedItemText = retailItem.Text;

                inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbRelation.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

                // If we only have a single convertable inventory unit for the item we should select it by default
                if ((string)inventoryUnitID != null)
                {
                    convertableUnits = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbRelation.SelectedData.ID, inventoryUnitID);
                    cmbUnit.SelectedData = convertableUnits.Count > 1 ? convertableUnits.First(i => i.ID == inventoryUnitID) : convertableUnits[0];
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
                    errorProvider1.Clear();
                }
                else if (sender is TextBox)
                {
                    cmbRelation.SelectedData.ID = retailItem.ID;
                    cmbRelation.Text = retailItem.Text;
                    if (retailItem.ItemType == ItemTypeEnum.Item && retailItem.VariantName != "")
                    {
                        cmbVariantNumber.SelectedData.ID = retailItem.ID;
                        cmbVariantNumber.Text = retailItem.VariantName;
                        lblVariantNumber.Enabled = cmbVariantNumber.Enabled = true;
                        cmbVariantNumber_SelectedDataChanged(sender, e);
                    }
                }

                if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                {
                    lblVariantNumber.Enabled = cmbVariantNumber.Enabled = true;
                }
            }

            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);

            CheckEnabled();
            CheckCmbEnabledStates();
            VariantWantsFocus();
        }

        private void cmbRelation_Leave(object sender, EventArgs e)
        {
            if (retailItem != null)
            {
                if (!lockEvent)
                {
                    if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                    {
                        cmbVariantNumber.Focus();
                    }
                    else
                    {
                        ntbQuantity.Focus();
                    }
                }
            }
            lockEvent = false;
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
        }

        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbVariantNumber.SelectedData.ID != "")
            {
                retailItem = PluginOperations.GetRetailItem((cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID);

                inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID, RetailItem.UnitTypeEnum.Inventory);

                if (retailItem == null)
                {
                    return;
                }

                UpdateBarCode(retailItem);

                if ((string)retailItem.InventoryUnitID != null)
                {
                    convertableUnits = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbRelation.SelectedData.ID, inventoryUnitID);
                    cmbUnit.SelectedData = convertableUnits.Count > 1 ? convertableUnits.First(i => i.ID == inventoryUnitID) : convertableUnits[0];
                }
            }

            var unit = Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitID);
            cmbUnit.SelectedData = unit;

            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);

            CheckEnabled();
            CheckCmbEnabledStates();
        }

        private void cmbReason_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                reasonId = (string)((DataEntity)e.Data).ID;
                reasonText = ((DataEntity)e.Data).Text;
                e.TextToDisplay = reasonText;
            }
        }

        private void cmbReason_RequestData(object sender, EventArgs e)
        {
            List<ReasonCode> reasonList = null;
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                reasonList = service.GetReasonCodesList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true,
                                                        new List<ReasonActionEnum> { ReasonActionHelper.GetEquivalent(journalType) }, 
                                                        forPOS : null, open : true);
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            cmbReason.SetData(reasonList, null);
        }

        private void cmbReason_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
            btnEditReasonCode.Enabled = (cmbReason.Text != "");
        }

        private void cmbReason_SelectedDataCleared(object sender, EventArgs e)
        {
            btnEditReasonCode.Enabled = false;
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ((itemRecId != null) && (cmbUnit.Text != "") && (ntbQuantity.Text != "") && (ntbQuantity.Text != "-"))
                {
                    adjustmentInOrigInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel,
                                                                                              itemRecId,
                                                                                              cmbUnit.SelectedData.ID,
                                                                                              origUnitId,
                                                                                              Convert.ToDecimal(
                                                                                                  ntbQuantity.Text));
                }
                else
                {
                    adjustmentInOrigInventoryUnit = 0M;
                }
            }
            catch
            {
                adjustmentInOrigInventoryUnit = 0M;
            }

            CheckEnabled();
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (cmbRelation.SelectedData != null)
            {
                inventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbRelation.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

                if ((string)inventoryUnitID == null)
                {
                    MessageDialog.Show(Resources.InventoryUnitMissing);
                    return;
                }

                IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbRelation.SelectedData.ID,inventoryUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

                TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                    cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                    data,
                    new string[] { Resources.Convertible, Resources.All },
                    250);

                cmbUnit.SetData(data, pnl);
            }
        }

        private void cmbUnitId_SelectedDataChanged(object sender, EventArgs e)
        {
            bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(
                                            PluginOperations.CreateItemDataEntity(cmbRelation, cmbVariantNumber), 
                                            cmbUnit.SelectedData.ID);
            if (!unitConversionExists)
            {
                MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                cmbUnit.Text = "";
            }
            else
            {
                adjustmentInOrigInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, 
                                                                                                    itemRecId, 
                                                                                                    cmbUnit.SelectedData.ID, 
                                                                                                    origUnitId, 
                                                                                                    Convert.ToDecimal(ntbQuantity.Value));
                ntbQuantity.DecimalLetters = ((Unit)cmbUnit.SelectedData).MaximumDecimals;
            }

            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);

            CheckEnabled();
        }

        private void btnAddEditReasonCode_Click(object sender, EventArgs e)
        {
            ReasonCodeDialogBehaviour action = ReasonCodeDialogBehaviour.Add;
            if(sender is ContextButton && ((ContextButton)sender).Context == ButtonType.Edit)
            {
                action = ReasonCodeDialogBehaviour.Edit;
            }

            RecordIdentifier selectedReasonID = cmbReason.SelectedData != null
                                            ? ((ReasonCode)cmbReason.SelectedData).ID
                                            : RecordIdentifier.Empty;

            using (ReasonCodeDialog dlg = new ReasonCodeDialog(action, TransformJournalType(journalType), selectedReasonID))
            {
                DialogResult result = dlg.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    try
                    {
                        cmbReason_RequestData(sender, e);
                        if (dlg.LastReasonCode != null && !RecordIdentifier.IsEmptyOrNull(dlg.LastReasonCode.ID))
                        {
                            cmbReason.SelectedData = dlg.LastReasonCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + Environment.NewLine + ex.Message);
                    }
                }
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            decimal adjustment = Convert.ToDecimal(ntbQuantity.Text);

            ntbQuantity.Text = (adjustment * (-1)).ToString();

            CheckEnabled();
        }

        // Keep this in sync with autotest action CreateInventoryJournal
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                if (cbCreateAnother.Checked)
                {
                    SetDefaults();
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "ItemInventoryLine", selectedJournalID, null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool Save()
        {
            RecordIdentifier itemID = cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty ? PluginOperations.GetReadableItemID(cmbVariantNumber) : selectedItemId;
            DataEntity selectedUnit = (DataEntity)cmbUnit.SelectedData;
            decimal adjustmentQuantity = Convert.ToDecimal(ntbQuantity.Text);
            bool lineSaved = false;
            costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation).IntValue;

            try
            {
                RetailItem retailItem = PluginOperations.GetRetailItem(itemID);
                if (retailItem.IsAssemblyItem)
                {
                    RetailItemAssembly assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(PluginEntry.DataModel, itemID, storeId);
                    if (assembly != null)
                    {
                        if (SaveAssemblyComponents(retailItem, adjustmentQuantity, selectedUnit.ID))
                        {
                            lineSaved = true;
                        }
                        else
                        {
                            MessageDialog.Show(Resources.AssemblyItemDoesNotContainComponents);
                        }
                    }
                    else
                    {
                        MessageDialog.Show(Resources.AssemblyItemIsNotDefinedForThisStore);
                    }
                }
                else
                {
                    CreateAndPostAdjustmentLine(itemID, adjustmentQuantity, selectedUnit.ID, selectedUnit.Text);
                    lineSaved = true;
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            return lineSaved;
        }

        private bool SaveAssemblyComponents(RetailItem retailItem, decimal itemQuantity, RecordIdentifier itemUnitID)
        {
            bool lineAdded = false;

            var assemblyQuantity = Providers.UnitConversionData.ConvertQtyBetweenUnits(
                PluginEntry.DataModel, retailItem.ID, itemUnitID, retailItem.SalesUnitID, itemQuantity);

            var assembly = Providers.RetailItemAssemblyData.GetAssemblyForItem(
                PluginEntry.DataModel, retailItem.ID, storeId, false);

            if (assembly != null)
            {
                foreach (var component in Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, assembly.ID))
                {
                    var componentQuantity = component.Quantity * assemblyQuantity;

                    var componentItem = PluginOperations.GetRetailItem(component.ItemID, false);
                    if (componentItem.IsAssemblyItem)
                    {
                        lineAdded |= SaveAssemblyComponents(componentItem, componentQuantity, component.UnitID);
                    }
                    else if (componentItem.ItemType != ItemTypeEnum.Service && componentItem.ItemType != ItemTypeEnum.MasterItem)
                    {
                        Unit componentUnit = Providers.UnitData.Get(PluginEntry.DataModel, component.UnitID);
                        CreateAndPostAdjustmentLine(component.ItemID, componentQuantity, component.UnitID, componentUnit.Text);
                        lineAdded = true;
                    }
                }
            }

            return lineAdded;
        }

        private void CreateAndPostAdjustmentLine(RecordIdentifier itemID, decimal adjustmentQuantity, RecordIdentifier unitID, string unitDescription)
        {
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            journalTransactionLine = new InventoryJournalTransaction();
            journalTransactionLine.JournalId = selectedJournalID;
            journalTransactionLine.TransDate = DateTime.Now;
            journalTransactionLine.StaffID = PluginEntry.DataModel.CurrentUser.StaffID;

            journalTransactionLine.ItemId = itemID;
            journalTransactionLine.ReasonId = reasonId;
            journalTransactionLine.ReasonText = cmbReason.Text;

            if (costCalculation == CostCalculation.Manual)
            {
                journalTransactionLine.CostPrice = Providers.RetailItemData.GetItemPrice(PluginEntry.DataModel, journalTransactionLine.ItemId).PurchasePrice;
            }
            else
            {
                RetailItemCost itemCost = service.GetRetailItemCost(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalTransactionLine.ItemId, storeId, false);
                journalTransactionLine.CostPrice = itemCost == null ? 0 : itemCost.Cost;
            }

            // Unit description needs to be set so that the backwards notification works
            Unit selectedUnit = new Unit(unitID, unitDescription, 0, 0);
            journalTransactionLine.SetUnit(selectedUnit);

            if (journalType == InventoryJournalTypeEnum.Parked && adjustmentQuantity > 0) //for parked inventory we substract from inventory
            {
                adjustmentQuantity *= -1;
            }
            journalTransactionLine.Adjustment = adjustmentQuantity;
            journalTransactionLine.InventoryUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, journalTransactionLine.ItemId, RetailItem.UnitTypeEnum.Inventory);
            service.GetInventoryOnHand(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, storeId, false);
            journalTransactionLine.AdjustmentInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel,
                                                                                                    journalTransactionLine.ItemId,
                                                                                                    journalTransactionLine.UnitID,
                                                                                                    journalTransactionLine.InventoryUnitID,
                                                                                                    journalTransactionLine.Adjustment);

            service.PostInventoryAdjustmentLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalTransactionLine, storeId, GetInventoryType(journalType), true);
        }

        private void SetDefaults()
        {
            tbBarcode.Text = "";
            cmbRelation.SelectedData = new DataEntity("", "");
            cmbVariantNumber.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            ntbQuantity.Text = "0";
            errorProvider1.Clear();

            lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;

            btnOK.Enabled = false;

            tbBarcode.Focus();
        }

        private void CheckBarCode()
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text))
            {
                return;
            }

            RecordIdentifier ItemID = RecordIdentifier.Empty;
            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel,
                    BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    ItemID = barCode.ItemID;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
                {
                    ItemID = tbBarcode.Text;
                }
                else if (cmbRelation.SelectedData.ID == RecordIdentifier.Empty)
                {
                    cmbRelation.SelectedData = new DataEntity("", "");
                    cmbVariantNumber.SelectedData = new DataEntity("", "");
                    lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;
                    return;
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
                cmbRelation.SelectedData = new DataEntity { ID = ItemID };
                cmbRelation_SelectedDataChanged(tbBarcode, EventArgs.Empty);
            }

            VariantWantsFocus();

            lockEvent = false;
        }

        private void UpdateBarCode(RetailItem retailItem)
        {
            barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);
            if (barCode != null)
            {
                tbBarcode.Text = (string)barCode.ItemBarCode;
            }
            else
            {
                tbBarcode.Text = "";
            }
        }

        // Keep this in sync with autotest action CreateInventoryJournal
        private InventoryTypeEnum GetInventoryType(InventoryJournalTypeEnum journalType)
        {
            switch (journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    return InventoryTypeEnum.Adjustment;
                case InventoryJournalTypeEnum.Reservation:
                    return InventoryTypeEnum.Reservation;
                case InventoryJournalTypeEnum.Parked:
                    return InventoryTypeEnum.Parked;
                default:
                    return InventoryTypeEnum.Adjustment;
            }
        }

        /// <summary>
        /// Returns the corresponding reason code action for the given journal type.
        /// </summary>
        /// <param name="journalType"></param>
        /// <returns></returns>
        private static ReasonActionEnum TransformJournalType(InventoryJournalTypeEnum journalType)
        {
            ReasonActionEnum defaultAction = ReasonActionEnum.Adjustment;

            switch (journalType)
            {
                case InventoryJournalTypeEnum.Reservation:
                    defaultAction = ReasonActionEnum.StockReservation;
                    break;
                case InventoryJournalTypeEnum.Parked:
                    defaultAction = ReasonActionEnum.ParkedInventory;
                    break;
                case InventoryJournalTypeEnum.Adjustment:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported conversion from journal type to reason code action");
            }

            return defaultAction;
        }

        // Keep this in sync with autotest action CreateInventoryJournal
        private void DisplayHelpText()
        {
            string messageTemplate = string.Empty;

            if(ntbQuantity.Text == "" || ntbQuantity.Text == "-" || cmbUnit.Text == "")
            {
                lblSwitch.Text = "";
                btnSwitch.Enabled = false;
                return;
            }

            decimal adjustment;
            bool enableSwitch = true;
            if(!Decimal.TryParse(ntbQuantity.Text, out adjustment))
            {
                return;
            }

            switch (journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    messageTemplate = adjustment > 0
                                        ? Resources.InventoryAdjustmentPositiveCount
                                        : Resources.InventoryAdjustmentNegativeCount;
                    break;
                case InventoryJournalTypeEnum.Reservation:
                    messageTemplate = adjustment > 0
                                        ? Resources.StockReservationPositiveCount
                                        : Resources.StockReservationNegativeCount;
                    break;
                case InventoryJournalTypeEnum.Parked:
                        messageTemplate = adjustment > 0
                                        ? Resources.ParkedInventoryPositiveCount
                                        : Resources.ParkedInventoryNegativeCount;
                    enableSwitch = adjustment > 0;
                    btnOK.Enabled = adjustment < 0;
                    break;
            }

            DataEntity unit = (DataEntity)cmbUnit.SelectedData;
            lblSwitch.Text = string.Format(messageTemplate, Math.Abs(adjustment), unit.Text);
            btnSwitch.Enabled = enableSwitch;
        }

        // Keep this in sync with autotest action CreateInventoryJournal
        private void CheckEnabled()
        {
            bool enabled;

            enabled = cmbRelation.Text != "";
            enabled = enabled && (
                (
                    cmbVariantNumber.Enabled
                    && !RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedData.ID.PrimaryID)
                    && cmbVariantNumber.SelectedData.ID.PrimaryID != ""
                )
                || (
                    !cmbVariantNumber.Enabled
                    && RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedData.ID)
                    )
                );
            enabled = enabled && (cmbReason.Text != "");
            enabled = enabled && ((ntbQuantity.Text != "") && (ntbQuantity.Value != 0));
            enabled = enabled && (cmbUnit.Text != "");

            btnOK.Enabled = enabled;

            DisplayHelpText();
        }

        private void CheckCmbEnabledStates()
        {
            bool cmbUnitEnabled;

            cmbUnitEnabled = cmbRelation.Text != "" && convertableUnits.Count > 1;

            cmbUnit.Enabled = cmbUnitEnabled;
        }

        private void VariantWantsFocus()
        {
            if (
                cmbVariantNumber.Enabled
                && (cmbVariantNumber.SelectedDataID == null || string.IsNullOrEmpty((string)cmbVariantNumber.SelectedDataID))
                && retailItem.ItemType == ItemTypeEnum.MasterItem
               )
            {
                cmbVariantNumber.Focus();
                return;
            }

            ReasonWantsFocus();
        }

        private void ReasonWantsFocus()
        {
            if (cmbRelation.SelectedData != null && cmbReason.SelectedData == null)
            {
                cmbReason.Focus();
                return;
            }

            ntbQuantity.Focus();
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            DecimalLimit unitDecimaLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbQuantity.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}