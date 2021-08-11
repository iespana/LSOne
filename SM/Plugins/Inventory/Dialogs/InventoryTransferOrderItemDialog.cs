using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class InventoryTransferOrderItemDialog : DialogBase
    {
        private RecordIdentifier inventoryTransferId;
        private RetailItem retailItem;
        private List<Unit> units;

        public InventoryTransferOrderItemDialog(InventoryTransferOrderLine inventoryTransferOrderLine, bool editingWhenReceiving = false)
            : this(inventoryTransferOrderLine.InventoryTransferId)
        {
            InventoryTransferOrderLine = inventoryTransferOrderLine;

            retailItem = PluginOperations.GetRetailItem(inventoryTransferOrderLine.ItemId);

            if (retailItem == null)
            {
                return;
            }

            if (RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {
                cmbItem.SelectedData = new DataEntity(retailItem.ID, retailItem.Text);

                cmbItem_SelectedDataChanged(cmbItem, null);
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

            cmbItem.Enabled = false;
            cmbUnit.SelectedData = new DataEntity(inventoryTransferOrderLine.UnitId, inventoryTransferOrderLine.UnitName);

            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            ntbSendingQuantity.SetValueWithLimit(inventoryTransferOrderLine.QuantitySent, quantityLimiter);
            ntbReceivingQuantity.SetValueWithLimit(inventoryTransferOrderLine.QuantityReceived, quantityLimiter);


            if (editingWhenReceiving)
            {
                cmbItem.Enabled = false;
                cmbUnit.Enabled = false;
                cmbVariant.Enabled = false;
                ntbSendingQuantity.Enabled = false;
                lblQuantitySending.Enabled = false;
                lblReceivingQuantity.Visible = true;
                ntbReceivingQuantity.Visible = true;
            }
        }

        public InventoryTransferOrderItemDialog(RecordIdentifier inventoryTransferId)
        {
            InitializeComponent();
            this.inventoryTransferId = inventoryTransferId;
            cmbItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
        }

        public InventoryTransferOrderLine InventoryTransferOrderLine { get; set; }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool addingNew = false;
            if (InventoryTransferOrderLine == null)
            {
                InventoryTransferOrderLine checkLine = new InventoryTransferOrderLine
                {
                    InventoryTransferId = inventoryTransferId,
                    ItemId = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant),
                    UnitId = cmbUnit.SelectedData.ID
                };
                bool similarItemExists = Providers.InventoryTransferOrderLineData.ItemWithSameParametersExists(PluginEntry.DataModel, checkLine);
                if (similarItemExists)
                {
                    errorProvider1.SetError(cmbItem, Properties.Resources.SimilarItemExistsInTransferOrder);
                    return;
                }

                InventoryTransferOrderLine = new InventoryTransferOrderLine();
                InventoryTransferOrderLine.InventoryTransferId = inventoryTransferId;
                addingNew = true;
            }

            // Make sure we are not changing the item properties to item properties that already exist in the transfer
            if (!addingNew)
            {
                // If some of the item properties changed we need to check
                if (InventoryTransferOrderLine.ItemId != cmbItem.SelectedData.ID ||
                    InventoryTransferOrderLine.UnitId != cmbUnit.SelectedData.ID)
                {
                    InventoryTransferOrderLine checkLine = new InventoryTransferOrderLine
                    {
                        InventoryTransferId = inventoryTransferId,
                        ItemId = cmbItem.SelectedData.ID,
                        UnitId = cmbUnit.SelectedData.ID
                    };

                    bool similarItemExists = Providers.InventoryTransferOrderLineData.ItemWithSameParametersExists(PluginEntry.DataModel, checkLine);
                    if (similarItemExists)
                    {
                        errorProvider1.SetError(cmbItem, Properties.Resources.SimilarItemExistsInTransferOrder);
                        return;
                    }
                }
            }

            RecordIdentifier target = (cmbVariant.SelectedData != null && !string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue))
                                        ? PluginOperations.GetReadableItemID(cmbVariant)
                                        : PluginOperations.GetReadableItemID(cmbItem);

            InventoryTransferOrderLine.ItemId = target;
            InventoryTransferOrderLine.UnitId = cmbUnit.SelectedData.ID;
            InventoryTransferOrderLine.QuantitySent = (decimal)ntbSendingQuantity.Value;
            InventoryTransferOrderLine.QuantityReceived = (decimal)ntbReceivingQuantity.Value;


            Providers.InventoryTransferOrderLineData.Save(PluginEntry.DataModel, InventoryTransferOrderLine);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKBtnEnabled(object sender, EventArgs e)
        {

            btnOK.Enabled = cmbItem.SelectedData.ID != "" &&

                (cmbVariant.SelectedData.ID != "" || !cmbVariant.Enabled) &&
                            cmbUnit.SelectedData.ID != "" &&
                            ntbSendingQuantity.Value != 0;



            if (InventoryTransferOrderLine != null)
            {
                RecordIdentifier target = PluginOperations.GetSelectedItemID(cmbItem, cmbVariant);
                btnOK.Enabled = btnOK.Enabled &&
                                (target != InventoryTransferOrderLine.ItemId ||
                                cmbUnit.SelectedData.ID != InventoryTransferOrderLine.UnitId ||
                                ntbSendingQuantity.Value != (double)InventoryTransferOrderLine.QuantitySent ||
                                ntbReceivingQuantity.Value != (double)InventoryTransferOrderLine.QuantityReceived);
            }

            errorProvider1.Clear();
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
                initialSearchText = ((DataEntity)cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText, SearchTypeEnum.InventoryItems, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            if (sender is DualDataComboBox)
            {
                GetUnitInfoForItem((DualDataComboBox) sender, false);
            }
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (sender is DualDataComboBox)
            {
                GetUnitInfoForItem((DualDataComboBox) sender, true);
            }
        }

        private void GetUnitInfoForItem(DualDataComboBox control, bool isVariant)
        {
            if (!(control is DualDataComboBox))
            {
                return;
            }

            retailItem = PluginOperations.GetRetailItem(control.SelectedData.ID);
            if (retailItem == null)
            {
                return;
            }

            cmbUnit.Enabled = true;
            lblUnit.Enabled = true;
            SetUnitForItem(retailItem.ID);
            if (!isVariant)
            {
                cmbVariant.Enabled = control.SelectedData.ID != "" && retailItem.ItemType == ItemTypeEnum.MasterItem;
                lblVariant.Enabled = cmbVariant.Enabled;
            }

            if (string.IsNullOrEmpty(retailItem.HeaderItemID.StringValue))
            {
                cmbVariant.SelectedData = new DataEntity("", "");
            }

            OKBtnEnabled(null, null);
        }

        private void SetUnitForItem(RecordIdentifier itemID)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Inventory);

            Unit unit = units.FirstOrDefault(f => f.ID == itemInventoryUnit) ?? new Unit();
            cmbUnit.SelectedData = unit;
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

            IEnumerable<DataEntity>[] unitData = new IEnumerable<DataEntity>[]
                        {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItem.SelectedData.ID,itemInventoryUnit),
                         units};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData.ID,
                unitData,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                200);

            cmbUnit.SetData(unitData, pnl);
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

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            Unit unit = (Unit)cmbUnit.SelectedData;
            RecordIdentifier unitID = unit.ID;

            if (!CheckUnitConversion(unitID))
            {
                cmbUnit.SelectedData = new DataEntity("", "");
            }
            OKBtnEnabled(sender, e);
        }

        private bool CheckUnitConversion(RecordIdentifier unitIDToCheck)
        {
            if (retailItem == null)
            {
                return false;
            }

            //If the unit selected is one of the units already on the item then there is no need to create a conversion rule
            if (cmbUnit.SelectedData != null)
            {
                Unit unit = (Unit)cmbUnit.SelectedData;
                if (retailItem.SalesUnitID == unit.ID || retailItem.InventoryUnitID == unit.ID || retailItem.PurchaseUnitID == unit.ID)
                {
                    return true;
                }
            }

            bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(new DataEntity(retailItem.ID, retailItem.Text), unitIDToCheck);
            if (!unitConversionExists)
            {
                MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
                return false;
            }

            return true;
        }
    }
}
