using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.RetailItemAssemblies.Dialogs
{
    public partial class NewEditAssemblyComponentDialog : DialogBase
    {
        private readonly RecordIdentifier assemblyID;
        private readonly RecordIdentifier parentItemID;
        private readonly RecordIdentifier originalItemID;
        private readonly List<RetailItemAssemblyComponent> componentsInAssembly;

        private RetailItem item;
        private RecordIdentifier itemSalesUnitID;
        private decimal itemBaseCost;
        private decimal costPerUnit;
        private decimal unitFactor = 1;
        private bool saveRecord;

        private DecimalLimit priceLimit;

        CostCalculation costCalculation;
        RetailItemCost weightedAverageCost;
        RecordIdentifier assemblyStoreID;

        public RetailItemAssemblyComponent AssemblyComponent { get; private set; }
        public bool CreateAnother { get; private set; }

        private RecordIdentifier AssemblyID => assemblyID ?? AssemblyComponent.AssemblyID;

        public NewEditAssemblyComponentDialog(RecordIdentifier assemblyID, List<RetailItemAssemblyComponent> componentsInAssembly, bool saveRecord, RecordIdentifier parentItemID, RecordIdentifier assemblyStoreID)
        {
            InitializeComponent();

            this.assemblyID = assemblyID;
            this.parentItemID = parentItemID;
            this.componentsInAssembly = componentsInAssembly;
            this.saveRecord = saveRecord;
            this.assemblyStoreID = assemblyStoreID;

            SetDecimalLimits();

            cmbItem.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");

            SetQuantityAllowDecimals(1);

            cbCreateAnother.Checked = CreateAnother = true;
            costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System).IntValue;
        }

        public NewEditAssemblyComponentDialog(RetailItemAssemblyComponent assemblyComponent, List<RetailItemAssemblyComponent> componentsInAssembly, bool saveRecord, RecordIdentifier parentItemID, RecordIdentifier assemblyStoreID)
        {
            InitializeComponent();

            AssemblyComponent = assemblyComponent;
            this.parentItemID = parentItemID;
            this.componentsInAssembly = componentsInAssembly;
            this.saveRecord = saveRecord;
            this.assemblyStoreID = assemblyStoreID;

            SetDecimalLimits();

            Text = Properties.Resources.EditAssemblyComponent;
            Header = Properties.Resources.EditAssemblyComponentInfo;

            cbCreateAnother.Visible = false;
            CreateAnother = false;

            costCalculation = (CostCalculation)PluginEntry.DataModel.Settings.GetSetting(PluginEntry.DataModel, Settings.CostCalculation, SettingsLevel.System).IntValue;

            originalItemID = assemblyComponent.ItemID;
            item = Providers.RetailItemData.Get(PluginEntry.DataModel, AssemblyComponent.ItemID);

            if(item.IsVariantItem)
            {
                cmbItem.SelectedData = new DataEntity(AssemblyComponent.HeaderItemID, AssemblyComponent.ItemName);
                cmbVariant.SelectedData = new DataEntity(AssemblyComponent.ItemID, AssemblyComponent.VariantName);
                cmbVariant.Enabled = true;
            }
            else
            {
                cmbItem.SelectedData = new DataEntity(AssemblyComponent.ItemID, AssemblyComponent.ItemName);
            }

            /// **** TODO: THIS IS TEMPORARY UNTIL NEXT RELEASE
            /// DO NOT ALLOW CHANGING OF UNIT OF COMPONENTS UNTIL WE FIGURE OUT HOW TO CORRECTLY UPDATE PRICES
            if(item.IsAssemblyItem)
            {
                cmbUnit.Enabled = false;
            }

            cmbUnit.SelectedData = new DataEntity(AssemblyComponent.UnitID, AssemblyComponent.UnitName);
            SetQuantityAllowDecimals(assemblyComponent.Quantity);

            itemSalesUnitID = item.SalesUnitID;
            itemBaseCost = item.PurchasePrice;
            costPerUnit = item.PurchasePrice;

            if(costCalculation != CostCalculation.Manual)
            {
                weightedAverageCost = Providers.RetailItemCostData.Get(PluginEntry.DataModel, item.ID, assemblyStoreID);
                costPerUnit = weightedAverageCost == null ? 0 : weightedAverageCost.Cost;
                itemBaseCost = weightedAverageCost == null ? 0 : weightedAverageCost.Cost;
            }

            ntbCostPerUnit.SetValueWithLimit(costPerUnit, priceLimit);
            UpdateUnitFactor();
            UpdateTotalCost();
        }

        private void SetDecimalLimits()
        {
            priceLimit = PluginEntry.DataModel.GetDecimalSetting(DataLayer.GenericConnector.Enums.DecimalSettingEnum.Prices);

            SetQuantityAllowDecimals(0m);
            ntbCostPerUnit.SetValueWithLimit(0m, priceLimit);
            ntbTotalCost.SetValueWithLimit(0m, priceLimit);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
            itemSalesUnitID = null;
            costPerUnit = 0;
            unitFactor = 1;
            cmbVariant.SelectedData = new DataEntity("", "");

            if(RecordIdentifier.IsEmptyOrNull(cmbItem.SelectedDataID))
            {
                cmbUnit.Enabled = false;
                cmbUnit.SelectedData = new DataEntity("", "");
            }
            else
            {
                cmbUnit.Enabled = true;
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedDataID);
                itemSalesUnitID = item.SalesUnitID;
                itemBaseCost = item.PurchasePrice;
                costPerUnit = item.PurchasePrice;

                if (costCalculation != CostCalculation.Manual)
                {
                    weightedAverageCost = Providers.RetailItemCostData.Get(PluginEntry.DataModel, item.ID, assemblyStoreID);
                    costPerUnit = weightedAverageCost == null ? 0 : weightedAverageCost.Cost;
                    itemBaseCost = weightedAverageCost == null ? 0 : weightedAverageCost.Cost;
                }

                cmbUnit.SelectedData = Providers.UnitData.Get(PluginEntry.DataModel, itemSalesUnitID);
                ntbCostPerUnit.SetValueWithLimit(costPerUnit, priceLimit);

                /// **** TODO: THIS IS TEMPORARY UNTIL NEXT RELEASE
                /// DO NOT ALLOW CHANGING OF UNIT OF COMPONENTS UNTIL WE FIGURE OUT HOW TO CORRECTLY UPDATE PRICES
                if (item.IsAssemblyItem)
                {
                    cmbUnit.Enabled = false;
                }
            }

            cmbVariant.Enabled = item != null && item.IsHeaderItem;

            UpdateTotalCost();
            CheckEnabled();
            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);
        }

        private void UpdateTotalCost()
        {
            ntbTotalCost.SetValueWithLimit((decimal)ntbQuantity.FullPrecisionValue * (decimal)ntbCostPerUnit.FullPrecisionValue, priceLimit);
        }

        private void CheckEnabled()
        {
            if(RecordIdentifier.IsEmptyOrNull(cmbItem.SelectedDataID) || RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedDataID) || ntbQuantity.Value <= 0)
            {
                btnOK.Enabled = false;
                return;
            }

            if(AssemblyComponent != null)
            {
                if(((AssemblyComponent.HeaderItemID == RecordIdentifier.Empty && AssemblyComponent.ItemID == cmbItem.SelectedDataID) 
                    || (AssemblyComponent.HeaderItemID != RecordIdentifier.Empty && AssemblyComponent.HeaderItemID == cmbItem.SelectedDataID && AssemblyComponent.ItemID == cmbVariant.SelectedDataID))
                    && AssemblyComponent.UnitID == cmbUnit.SelectedDataID
                    && AssemblyComponent.Quantity == (decimal)ntbQuantity.Value)
                {
                    btnOK.Enabled = false;
                    return;
                }
            }

            btnOK.Enabled = true;
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
            CheckEnabled();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(!ValidateAssemblyReference())
            {
                errorProvider.SetError(cmbItem, Properties.Resources.AssemblyItemReferenceError);
                return;
            }

            if(AssemblyComponent == null)
            {
                AssemblyComponent = new RetailItemAssemblyComponent();
                AssemblyComponent.AssemblyID = assemblyID;
            }

            AssemblyComponent.HeaderItemID = RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID) ? RecordIdentifier.Empty : cmbItem.SelectedDataID;
            AssemblyComponent.ItemID = RecordIdentifier.IsEmptyOrNull(cmbVariant.SelectedDataID) ? cmbItem.SelectedDataID : cmbVariant.SelectedDataID;
            AssemblyComponent.ItemName = cmbItem.SelectedData.Text;
            AssemblyComponent.VariantName = cmbVariant.Text;
            AssemblyComponent.UnitID = cmbUnit.SelectedDataID;
            AssemblyComponent.UnitName = cmbUnit.SelectedData.Text;
            AssemblyComponent.Quantity = (decimal)ntbQuantity.Value;
            AssemblyComponent.CostPerUnit = costPerUnit;

            if(saveRecord)
            {
                Providers.RetailItemAssemblyComponentData.Save(PluginEntry.DataModel, AssemblyComponent);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Add, "RetailItemAssemblyComponent", AssemblyComponent.ID, AssemblyComponent);
            }

            if(saveRecord && cbCreateAnother.Visible && cbCreateAnother.Checked)
            {
                ClearForm();

                //If we are not saving the record, close the dialog to allow the invoker to process the new assembly component
                if (!saveRecord)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateAssemblyReference()
        {
            if(saveRecord && item.IsAssemblyItem && item.ID != originalItemID)
            {
                return !Providers.RetailItemAssemblyComponentData.AssemblyItemCausesCircularReference(PluginEntry.DataModel, AssemblyID, parentItemID, item.ID);
            }

            return true;
        }

        private void ClearForm()
        {
            AssemblyComponent = null;
            cmbItem.SelectedData = new DataEntity("", "");
            cmbVariant.SelectedData = new DataEntity("", "");
            cmbUnit.SelectedData = new DataEntity("", "");
            SetQuantityAllowDecimals(1);
            ntbTotalCost.SetValueWithLimit(0, priceLimit);
            ntbCostPerUnit.SetValueWithLimit(0, priceLimit);
            itemSalesUnitID = null;
            costPerUnit = 0;
            unitFactor = 1;

            // Explicit call to update the state of OK button since the _SelectedDataChanged event is not called on comboboxes when setting the data from source code.
            CheckEnabled();
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

            e.ControlToEmbed = new SingleSearchPanel(
                PluginEntry.DataModel,
                false,
                initialSearchText,
                SearchTypeEnum.RetailItems,
                saveRecord 
                    ? Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, AssemblyID)
                    .Where(x => RecordIdentifier.IsEmptyOrNull(x.HeaderItemID)).Select(component => component.ItemID).ToList() 
                    : componentsInAssembly.Where(x => RecordIdentifier.IsEmptyOrNull(x.HeaderItemID)).Select(x => x.ItemID).ToList(),
                textInitallyHighlighted,
                true);


        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            if (itemSalesUnitID == null)
            {
                itemSalesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedDataID, RetailItem.UnitTypeEnum.Sales);
            }

            List<DataEntity> data = new List<DataEntity>();

            if (itemSalesUnitID != null)
            {
                data = Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, cmbItem.SelectedDataID, itemSalesUnitID).Cast<DataEntity>().ToList();
            }

            cmbUnit.SetData(data, null);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            UpdateUnitFactor();
            UpdateTotalCost();
            CheckEnabled();
            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);
        }

        private void cbCreateAnother_CheckedChanged(object sender, EventArgs e)
        {
            CreateAnother = cbCreateAnother.Checked;
        }

        private void UpdateUnitFactor()
        {
            unitFactor = 1;

            if(!RecordIdentifier.IsEmptyOrNull(cmbUnit.SelectedDataID) 
                && !RecordIdentifier.IsEmptyOrNull(cmbItem.SelectedDataID) 
                && !RecordIdentifier.IsEmptyOrNull(itemSalesUnitID) 
                && itemSalesUnitID != cmbUnit.SelectedDataID)
            {
                unitFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(PluginEntry.DataModel, itemSalesUnitID, cmbUnit.SelectedDataID, cmbItem.SelectedDataID);
            }

            costPerUnit = itemBaseCost * unitFactor;
            ntbCostPerUnit.SetValueWithLimit(costPerUnit, priceLimit);
        }

        private void cmbVariant_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            CheckEnabled();
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
            SetQuantityAllowDecimals((decimal)ntbQuantity.Value);
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
                item.ItemType == ItemTypeEnum.MasterItem ?
                item.MasterID :
                item.HeaderItemID,
                false,
                initialSearchText,
                SearchTypeEnum.RetailItemVariantReadableID,
                saveRecord
                    ? Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, AssemblyID).Select(component => component.ItemID).ToList()
                    : componentsInAssembly.Select(x => x.ItemID).ToList(),
                textInitallyHighlighted,
                true);
        }

        /// <summary>
        /// If the Unit allows decimals then the qty textbox should allow the user to enter decimals
        /// </summary>
        private void SetQuantityAllowDecimals(decimal qtyValue)
        {
            bool useGeneralSettings = false;
            if (cmbUnit.SelectedData == null || string.IsNullOrEmpty(cmbUnit.SelectedData.Text))
            {
                useGeneralSettings = true;
            }

            DecimalLimit unitDecimaLimit = useGeneralSettings ?
                PluginEntry.DataModel.GetDecimalSetting(DataLayer.GenericConnector.Enums.DecimalSettingEnum.Quantity) :
                Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, cmbUnit.SelectedData.Text));
            ntbQuantity.SetValueWithLimit(qtyValue, unitDecimaLimit);
        }
    }
}
