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
    public partial class InventoryTransferRequestItemDialog : DialogBase
    {
        private RecordIdentifier inventoryTransferRequestId;
        private RetailItem retailItem;
        private List<Unit> units;
        public InventoryTransferRequestItemDialog(InventoryTransferRequestLine inventoryTransferRequestLine)
            : this(inventoryTransferRequestLine.InventoryTransferRequestId)
        {
            InventoryTransferRequestLine = inventoryTransferRequestLine;

            retailItem = PluginOperations.GetRetailItem(inventoryTransferRequestLine.ItemId);
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
                                        inventoryTransferRequestLine.ItemId,
                                        string.IsNullOrEmpty(inventoryTransferRequestLine.VariantName) ? retailItem.VariantName : inventoryTransferRequestLine.VariantName);

            }
          

          
            cmbUnit.SelectedData = new DataEntity(inventoryTransferRequestLine.UnitId, inventoryTransferRequestLine.UnitName);

            DecimalLimit quantityLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            ntbRequestingQuantity.SetValueWithLimit(inventoryTransferRequestLine.QuantityRequested, quantityLimiter);
        }

        public InventoryTransferRequestItemDialog(RecordIdentifier inventoryTransferRequestId)
        {
            InitializeComponent();
            this.inventoryTransferRequestId = inventoryTransferRequestId;
            cmbItem.SelectedData = new DataEntity("","");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
        }

        public InventoryTransferRequestLine InventoryTransferRequestLine { get; set; }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool addingNew = false;
            if (InventoryTransferRequestLine == null)
            {
                InventoryTransferRequestLine checkLine = new InventoryTransferRequestLine
                {
                    InventoryTransferRequestId = inventoryTransferRequestId,
                    ItemId = GetSelectedItemID(),
                    UnitId = cmbUnit.SelectedData.ID
                };

                bool similarItemExists = Providers.InventoryTransferRequestLineData.ItemWithSameParametersExists(PluginEntry.DataModel, checkLine);
                if (similarItemExists)
                {
                    errorProvider1.SetError(cmbItem, Properties.Resources.SimilarItemExistsInTransferOrder);
                    return;
                }

                InventoryTransferRequestLine = new InventoryTransferRequestLine();
                InventoryTransferRequestLine.InventoryTransferRequestId = inventoryTransferRequestId;
                addingNew = true;
            }

            // Make sure we are not changing the item properties to item properties that already exist in the transfer
            if (!addingNew)
            {
                // If some of the item properties changed we need to check
                if ((!string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue) && InventoryTransferRequestLine.ItemId != cmbVariant.SelectedData.ID) ||
                    (string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue) && InventoryTransferRequestLine.ItemId != cmbItem.SelectedData.ID) ||
                    InventoryTransferRequestLine.UnitId != cmbUnit.SelectedData.ID)
                {
                    InventoryTransferRequestLine checkLine = new InventoryTransferRequestLine
                    {
                        InventoryTransferRequestId = inventoryTransferRequestId,
                        ItemId = GetSelectedItemID(),
                        UnitId = cmbUnit.SelectedData.ID
                    };
                    
                    bool similarItemExists = Providers.InventoryTransferRequestLineData.ItemWithSameParametersExists(PluginEntry.DataModel, checkLine);
                    if (similarItemExists)
                    {
                        errorProvider1.SetError(cmbItem, Properties.Resources.SimilarItemExistsInTransferOrder);
                        return;
                    }
                }
            }

            InventoryTransferRequestLine.ItemId = GetSelectedItemID();
            InventoryTransferRequestLine.UnitId = cmbUnit.SelectedData.ID;
            InventoryTransferRequestLine.QuantityRequested = (decimal) ntbRequestingQuantity.Value;
            
            Providers.InventoryTransferRequestLineData.Save(PluginEntry.DataModel, InventoryTransferRequestLine);
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
                            ntbRequestingQuantity.Value != 0;

            if (InventoryTransferRequestLine != null)
            {
                RecordIdentifier target = GetSelectedItemID();


                btnOK.Enabled = btnOK.Enabled &&
                                (target != InventoryTransferRequestLine.ItemId ||
                                cmbUnit.SelectedData.ID != InventoryTransferRequestLine.UnitId ||
                                ntbRequestingQuantity.Value != (double)InventoryTransferRequestLine.QuantityRequested);
            }

            errorProvider1.Clear();
        }

     

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Inventory);

            IEnumerable<DataEntity>[] unitData = new IEnumerable<DataEntity>[]
                        {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItem.SelectedData.ID,itemInventoryUnit),
                         Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData.ID,
                unitData,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                200);

            cmbUnit.SetData(unitData, pnl);
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

        private RecordIdentifier GetSelectedItemID()
        {
            if(!string.IsNullOrEmpty(cmbVariant.SelectedData.ID.StringValue))
            {
                return PluginOperations.GetReadableItemID(cmbVariant);
            }
            else
            {
                return PluginOperations.GetReadableItemID(cmbItem);
            }
        }
    }
}
