using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Tax;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.DialogPages
{
    public partial class NewRetailItemGeneralPage : UserControl, IDialogTabViewWithRequiredFields, IMessageTabExtension
    {
        RetailItem retailItem;
        RecordIdentifier itemID;
        public event EventHandler RequiredInputValidate;
        bool manuallyEnterId;
        WeakReference unitAdder;
        RecordIdentifier defaultStoreId;
        bool pricesAreWithTax;
        DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);


        public NewRetailItemGeneralPage()
        {
            itemID = RecordIdentifier.Empty;

            InitializeComponent();
            Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);

            manuallyEnterId = parameters.ManuallyEnterItemID;

            tbID.Visible = manuallyEnterId;
            lblID.Visible = manuallyEnterId;
            cmbRetailGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbInventoryUnit.SelectedData = new DataEntity("", "");
            cmbSalesUnit.SelectedData = new DataEntity("", "");
            cmbItemSalesTaxGroup.SelectedData = new DataEntity("", "");

            ntbPrice.SetValueWithLimit(0.0M, priceLimiter);

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);

            if (plugin != null)
            {
                btnAddInventoryUnit.Visible = true;
                btnAddSalesUnit.Visible = true;
                unitAdder = new WeakReference(plugin);
            }
            else
            {
                unitAdder = null;
            }

            var itemTypes = new object[]
            {
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Service),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.MasterItem),
                    ItemTypeHelper.ItemTypeToString(ItemTypeEnum.AssemblyItem)
            };
            cmbItemType.Items.AddRange(itemTypes);
            cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!manuallyEnterId)
            {
                tbDescription.Select();
            }
            else
            {
                tbID.Select();
            }
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new NewRetailItemGeneralPage();
        }

        public RecordIdentifier ItemID
        {
            get { return itemID; }
        }

        public bool DialogCheckEnabled
        {
            get { return true ; }
        }

        public bool DataIsModified()
        {
            // This will never be called since we are on dialog
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            // This will never be called since we are on dialog
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem) internalContext;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
        }

        public bool SaveData()
        {

            if (manuallyEnterId)
            {
                retailItem.ID = tbID.Text.Trim();
            }

            retailItem.Text = tbDescription.Text;
            retailItem.ExtendedDescription = tbExtendedDescription.Text;
            retailItem.RetailGroupMasterID = cmbRetailGroup.SelectedData.ID;
            if (retailItem.RetailGroupMasterID != RecordIdentifier.Empty)
            {
                RetailGroup retailGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, retailItem.RetailGroupMasterID);

                if (retailGroup != null)
                {
                    retailItem.ValidationPeriodID = retailGroup.ValidationPeriod;
                    retailItem.ProfitMargin = retailGroup.ProfitMargin;
                    retailItem.RetailDepartmentMasterID = retailGroup.RetailDepartmentMasterID;
                    retailItem.TareWeight = retailGroup.TareWeight;

                    if (!RecordIdentifier.IsEmptyOrNull(retailItem.RetailDepartmentMasterID))
                    {
                        RetailDepartment retaiDepartment = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, retailItem.RetailDepartmentMasterID);

                        if (retaiDepartment != null)
                        {
                            retailItem.RetailDivisionMasterID = retaiDepartment.RetailDivisionMasterID;
                        }
                    }
                }
            }
            retailItem.SalesTaxItemGroupID = cmbItemSalesTaxGroup.SelectedData.ID;
            retailItem.InventoryUnitID = retailItem.PurchaseUnitID = cmbInventoryUnit.SelectedData.ID;
            retailItem.SalesUnitID = cmbSalesUnit.SelectedData.ID;

            if (defaultStoreId == null)
            {
                defaultStoreId = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

                if (defaultStoreId != null && defaultStoreId != "")
                {
                    pricesAreWithTax = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, defaultStoreId);
                }
            }

            RecordIdentifier itemSalesTaxGroupId = retailItem.SalesTaxItemGroupID;

            decimal priceWithTax;
            decimal priceWithoutTax;
            if (pricesAreWithTax)
            {
                priceWithTax = ntbPrice.FullPrecisionValue;

                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                priceWithoutTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceFromPriceWithTax(
                    PluginEntry.DataModel,
                    priceWithTax,
                    retailItem.SalesTaxItemGroupID,
                    defaultStoreTaxGroup);
            }
            else
            {
                priceWithoutTax = ntbPrice.FullPrecisionValue;

                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                priceWithTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceWithTax(
                    PluginEntry.DataModel,
                    priceWithoutTax,
                    itemSalesTaxGroupId,
                    defaultStoreTaxGroup,
                    false,
                    0.0m,
                    priceLimiter);
            }

            retailItem.SalesPrice = priceWithoutTax;
            retailItem.SalesPriceIncludingTax = priceWithTax;
            itemID = retailItem.ID;

            return true;
        }

        public void SaveSecondaryRecords()
        {
            UnitConversion unitConversion = null;
            if (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemID, cmbInventoryUnit.SelectedData.ID, cmbSalesUnit.SelectedData.ID))
            {
                IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(this, "CanAddUnitConversions", null);

                if (unitConversionAdder != null)
                {

                    if (QuestionDialog.Show(
                        Properties.Resources.UnitConversionQuestion,
                        Properties.Resources.UnitConversionRuleMissing) == DialogResult.Yes)
                    {
                        unitConversion = (UnitConversion)unitConversionAdder.Message(this, "AddUnitConversionNoSave", new object[] { retailItem, cmbInventoryUnit.SelectedData.ID, cmbSalesUnit.SelectedData.ID });

                        if (unitConversion == null)
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageDialog.Show(Properties.Resources.UnitConversionRuleMissingAlert);
                    return;
                }
            }

            if (unitConversion != null)
            {
                unitConversion.ItemID = retailItem.ID;
                Providers.UnitConversionData.Save(PluginEntry.DataModel, unitConversion);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "UnitConversions", unitConversion.ID, null);
            }

            // block replenishment for service items
            if (retailItem.ItemType == ItemTypeEnum.Service)
            {
                IPlugin replenishmentPlugin = PluginEntry.Framework.FindImplementor(this, "CanBlockItemReplenishment", null);
                replenishmentPlugin.Message(this, "BlockItemReplenishmentNewServiceItem", retailItem.ID);
            }
        }

        public void SaveUserInterface()
        {
            
        }

        void IDialogTabViewWithRequiredFields.RequiredFieldsAreValid(FieldValidationArguments args)
        {
            if (manuallyEnterId && !string.IsNullOrEmpty(tbID.Text.Trim()))
            {
                if (!tbID.Text.IsAlphaNumeric())
                {
                    errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                    args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                    args.ResultDescription = Properties.Resources.OnlyCharAndNumbers;
                    return;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbID.Text.Trim()))
                {
                    errorProvider1.SetError(tbID, Properties.Resources.ItemIDExists);
                    args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                    args.ResultDescription = Properties.Resources.ItemIDExists;
                    return;
                }
            }

            if (tbDescription.Text.Trim() == "")
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.FieldMissing;
                args.ResultDescription = lblDescription.Text.Replace(":", "");
            }
            else if (cmbInventoryUnit.SelectedData.ID == "")
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.FieldMissing;
                args.ResultDescription = lblInventoryUnit.Text.Replace(":", "");
            }
            else
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            // Here we somehow want to trigger checkenabled on the parent
            if (RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        private void cmbRetailGroup_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbRetailGroup.SelectedData == null && string.IsNullOrEmpty(cmbRetailGroup.SelectedDataID.StringValue))
            {
                return;
            }

            RetailGroup selectedGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, cmbRetailGroup.SelectedDataID);
            if (!string.IsNullOrEmpty(selectedGroup.ItemSalesTaxGroupId.StringValue))
            {
                ItemSalesTaxGroup taxGroup = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, selectedGroup.ItemSalesTaxGroupId, CacheType.CacheTypeApplicationLifeTime);
                if (taxGroup != null)
                {
                    cmbItemSalesTaxGroup.SelectedData = new DataEntity(taxGroup.ID, taxGroup.Text);
                }
            }

            CheckEnabled(sender, e);
        }

        private void cmbRetailGroup_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

        private void cmbRetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,false, "", SearchTypeEnum.RetailGroupsMasterID, "", true);
        }


        private void btnAddRetailGroup_Click(object sender, EventArgs e)
        {
            NewRetailGroupDialog dlg = new NewRetailGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                RetailGroup retailGroup = dlg.retailGroup;
                cmbRetailGroup.SelectedData = new DataEntity(retailGroup.MasterID, retailGroup.Text);

                cmbRetailGroup_SelectedDataChanged(this, EventArgs.Empty);
            }
        }

        private void cmbInventoryUnit_RequestData(object sender, EventArgs e)
        {
            cmbInventoryUnit.SetData(Providers.UnitData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbInventoryUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbSalesUnit.SelectedData.ID == "")
            {
                cmbSalesUnit.SelectedData = cmbInventoryUnit.SelectedData;
                lblSalesUnit.Enabled = true;
                cmbSalesUnit.Enabled = true;
                btnAddSalesUnit.Enabled = true;
            }

            CheckEnabled(sender, e);
        }

        private void btnAddInventoryUnit_Click(object sender, EventArgs e)
        {
            object unit = ((IPlugin)unitAdder.Target).Message(this, "AddUnits", null);

            if (unit != null)
            {
                Unit unitID = (Unit)unit;
                cmbInventoryUnit.SelectedData = unitID;
                cmbInventoryUnit_SelectedDataChanged(sender, e);
            }
        }

        private void cmbSalesUnit_DropDown(object sender, DropDownEventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
            {
                Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel, "", cmbInventoryUnit.SelectedData.ID),
                Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()
            };

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbSalesUnit.SelectedData != null ? cmbSalesUnit.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.Convertible, Properties.Resources.All },
                300);

            e.ControlToEmbed = pnl;
        }

        private void cmbSalesUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbInventoryUnit.SelectedData.ID == "")
            {
                cmbInventoryUnit.SelectedData = cmbSalesUnit.SelectedData;
            }

            CheckEnabled(sender, e);
        }

        private void btnAddSalesUnit_Click(object sender, EventArgs e)
        {
            object unit = ((IPlugin)unitAdder.Target).Message(this, "AddUnits", null);

            if (unit != null)
            {
                Unit unitID = (Unit)unit;
                cmbSalesUnit.SelectedData = unitID;
            }
        }

        private void cmbItemSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbItemSalesTaxGroup.SetData(Providers.ItemSalesTaxGroupData.GetList(PluginEntry.DataModel), null);
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "RetailItemIsMasterItem":
                    cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(ItemTypeEnum.MasterItem);

                    return null;
                case "RetailItemIsItem":
                    cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item);

                    return null;
                case "RetailItemIsAssemblyItem":
                    cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(ItemTypeEnum.AssemblyItem);

                    return null;
                case "CreateAnother":
                    tbID.Text = "";
                    tbDescription.Text = "";
                    ntbPrice.SetValueWithLimit(0.0M, priceLimiter);
                    tbExtendedDescription.Text = "";
                    if (!manuallyEnterId)
                    {
                        tbDescription.Select();
                    }
                    else
                    {
                        tbID.Select();
                    }

                    cmbItemType.SelectedItem = ItemTypeHelper.ItemTypeToString(ItemTypeEnum.Item);

                    return null;
                case "GetItemSalesUnitAndPrice":
                    handled = true;
                    return new ValueTuple<decimal, DataEntity>((decimal)ntbPrice.Value, new DataEntity(cmbSalesUnit.SelectedDataID, cmbSalesUnit.Text));
            }
            return null;
        }

        private void cmbItemType_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (retailItem != null && cmbItemType.SelectedItem != null)
            {
                retailItem.ItemType = ItemTypeHelper.StringToItemType((string)cmbItemType.SelectedItem);
            }
        }
    }
}
