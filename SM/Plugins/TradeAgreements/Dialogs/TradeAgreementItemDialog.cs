using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts.MultiEditing;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreementItemDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier itemID;
        RetailItem item;
        WeakReference unitEditor;
        WeakReference currencyEditor;
        TradeAgreementEntry agreement;
        bool deleteMode;

        public static TradeAgreementItemDialog CreateForDeleting(RecordIdentifier itemID)
        {
            return new TradeAgreementItemDialog(itemID,true);
        }

        public static TradeAgreementItemDialog CreateForAdding(RecordIdentifier itemID)
        {
            return new TradeAgreementItemDialog(itemID,false);
        }

        public static TradeAgreementItemDialog CreateForEditing(RecordIdentifier itemID, RecordIdentifier agreementID)
        {
            return new TradeAgreementItemDialog(itemID, agreementID);
        }

        private TradeAgreementItemDialog(RecordIdentifier id, RecordIdentifier agreementID)
            : this(id,false)
        {
            DecimalLimit priceLimiter;
            Unit unit;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID, TradeAgreementRelation.PriceSales);

            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);

            cmbAccountCode.SelectedIndex = (int)agreement.AccountCode;
            cmbAccountSelection.SelectedData = new DataEntity(agreement.AccountRelation,agreement.AccountName);

            if(agreement.IsVariantItem)
            {
                cmbVariantNumber.SelectedData = new DataEntity(agreement.ItemRelation, agreement.VariantName); 
            }

            unit = Providers.UnitData.Get(PluginEntry.DataModel, agreement.UnitID);

            if (unit == null)
            {
                unit = new Unit(agreement.UnitID, (string)agreement.UnitID, 0,0);
            }

            cmbUnit.SelectedData = unit;
            cmbUnit_SelectedDataChanged(this, EventArgs.Empty);

            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbQuantity.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));
            ntbPrice.Text = agreement.Amount.FormatWithLimits(priceLimiter);
            ntbPriceWithVAT.Text = agreement.AmountIncludingTax.FormatWithLimits(priceLimiter);
            ntbMiscCharges.Text = agreement.Markup.FormatWithLimits(priceLimiter);

            CheckEnabled(this, EventArgs.Empty);
        }

        private TradeAgreementItemDialog(RecordIdentifier itemID, bool deleteMode)
            : this()
        {
            agreement = null;
            this.deleteMode = deleteMode;

            CompanyInfo companyInfo;
            this.itemID = itemID;
            
            cmbUnit.SelectedData = new Unit();

            if (itemID == RecordIdentifier.Empty)
            {
                Header = Resources.PriceConficMultiDescription;

                // We are in multiedit mode
                cmbUnit.Enabled = false;
                cmbUnit.SelectedData = new DataEntity(RecordIdentifier.Empty, Resources.SameAsOnItem);
            }
            else
            {
                item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                Unit salesUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.SalesUnitID);
                cmbUnit.SelectedData = salesUnit;

                Text += " - " + item.Text;
            }

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
            cmbAccountSelection.SelectedData = new DataEntity("","");
            cmbAccountCode.SelectedIndex = 2;

            if (itemID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
                cmbVariantNumber.Visible = false;
                lblVariant.Visible = false;

                if(deleteMode)
                {
                    lblPrice.Visible = false;
                    lblPriceMiscCharges.Visible = false;
                    lblPriceWithTax.Visible = false;

                    lfPriceLink.Visible = false;

                    ntbMiscCharges.Visible = false;
                    ntbPriceWithVAT.Visible = false;
                    ntbPrice.Visible = false;

                    Height -= 60;

                    Text = Resources.DeleteTASalesPrice;
                    Header = Resources.SetupConditionsForDeletingPrice;
                }
            }
            else
            {
                if (item.ItemType == ItemTypeEnum.MasterItem )
                {
                    cmbVariantNumber.Enabled = true;
                    cmbVariantNumber.SelectedData = new DataEntity("", "");
                    lblVariant.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    cmbVariantNumber.Enabled = false;
                    lblVariant.ForeColor = SystemColors.GrayText;
                }
            }
        } 

        public TradeAgreementItemDialog()
        {
            IPlugin plugin;

            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            unitEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddUnit.Visible = (unitEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);
            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddCurrency.Visible = (currencyEditor != null);

            ntbQuantity.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
            ntbPrice.DecimalLetters = 
                ntbPriceWithVAT.DecimalLetters = 
                ntbMiscCharges.DecimalLetters =
                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }

        public RecordIdentifier ID
        {
            get
            {
                return agreement.ID;
            }
        }

        public TradeAgreementEntry AgreementEntry
        {
            get
            {
                return agreement;
            }
        }

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.AccountCode = (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex;
            taEntry.AccountRelation = cmbAccountSelection.SelectedData.ID;
            taEntry.UnitID = cmbUnit.SelectedData.ID;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.PriceSales;
            taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
            RecordIdentifier target;
            if (cmbVariantNumber.SelectedData!= null &&cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty && cmbVariantNumber.SelectedData.ID != "")
            {
                if (cmbVariantNumber.SelectedData is MasterIDEntity)
                {
                    target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;
                }
                else
                {
                    target = (cmbVariantNumber.SelectedData as DataEntity).ID;
                }
            }
            else
            {
                target = itemID;
            }
            taEntry.ItemRelation = target;
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();

            if (!deleteMode)
            {
                taEntry.Amount = (decimal)ntbPrice.Value;
                taEntry.AmountIncludingTax = (decimal)ntbPriceWithVAT.Value;
                taEntry.Markup = (decimal)ntbMiscCharges.Value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ActiveControl != btnOK)
            {
                btnOK.Focus();
            }

            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Resources.FromDateCannotBeHigherThanToDate);
                return;
            }

            // If we are in single edit mode then we need to check unit conversion rules
            if (itemID != RecordIdentifier.Empty && !PluginOperations.ConversionRuleToInventoryUnitExists(item, cmbUnit.SelectedData.ID))
            {
                return;
            }

            if (agreement == null)
            {
                TradeAgreementEntry testAgreement;

                if (itemID == RecordIdentifier.Empty)
                {
                    testAgreement = new TradeAgreementEntryMultiEdit();
                    (testAgreement as TradeAgreementEntryMultiEdit).CalculateFromPrice = ntbPrice.Enabled;
                    testAgreement.AccountName = cmbAccountSelection.SelectedData.Text;
                }
                else
                {
                    testAgreement = new TradeAgreementEntry();
                }

                PopulateTAEntry(testAgreement);

                // If we are in single edit mode then we check if item exists.
                if (itemID != RecordIdentifier.Empty && Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testAgreement.OldID))
                {
                    if (cmbAccountSelection.Enabled)
                    {
                        errorProvider1.SetError(cmbCurrency, Resources.TradeAgreementExists1);
                    }
                    else
                    {
                        errorProvider1.SetError(cmbCurrency, Resources.TradeAgreementExists2);
                    }
                    return;
                }

                agreement = testAgreement; // We dont assign the class variable until now so it can remain null if there was error.
            }
            else
            {
                PopulateTAEntry(agreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, agreement.OldID, agreement.ID))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }
            }

            if (itemID != RecordIdentifier.Empty)
            {
                // In single edit mode then we save the record.
                Providers.TradeAgreementData.Save(PluginEntry.DataModel, agreement, Permission.ManageDiscounts);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            cmbCurrency.SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbAccountCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAccountSelection.Enabled = cmbAccountCode.SelectedIndex != 2;

            cmbAccountSelection.SelectedData = new DataEntity("", "");

            if (cmbAccountCode.SelectedIndex == 2)
            {
                cmbAccountSelection.SetUseInternalDropDown(true);
            }
            else if (cmbAccountCode.SelectedIndex == 0)
            {
                cmbAccountSelection.SetUseInternalDropDown(false);
            }
            else
            {
                cmbAccountSelection.SetUseInternalDropDown(true);
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbAccountSelection_DropDown(object sender, DropDownEventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 0)
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
                    initialSearchText = ((DataEntity)cmbAccountSelection.SelectedData).ID;
                    textInitallyHighlighted = true;
                }

                e.ControlToEmbed = new SingleSearchPanel(
                    PluginEntry.DataModel,
                    false,
                    initialSearchText,
                    SearchTypeEnum.Customers,
                    textInitallyHighlighted);
            }
            else if (cmbAccountCode.SelectedIndex == 1)
            {
                
            }
        }

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier salesUnitID;

            cmbUnit.SetWidth(200);

            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Sales);

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    {Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,itemID,salesUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel).Cast<DataEntity>()};

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbUnit.SelectedData != null ? cmbUnit.SelectedData.ID : "",
                data,
                new string[] { Resources.Convertible, Resources.All },
                250);

            cmbUnit.SetData(data, pnl);
        }

        private void cmbUnit_SelectedDataChanged(object sender, EventArgs e)
        {
            if(cmbUnit.SelectedData.ID != "")
            {
                ntbQuantity.AllowDecimal = ((Unit)cmbUnit.SelectedData).MaximumDecimals > 0;
                ntbQuantity.DecimalLetters = ((Unit)cmbUnit.SelectedData).MaximumDecimals;
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbAccountSelection_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (agreement == null)
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "" &&
                    cmbAccountCode.SelectedIndex >= 0 &&
                    ((cmbAccountSelection.Enabled && cmbAccountSelection.SelectedData.ID != "") || !cmbAccountSelection.Enabled) &&
                    cmbUnit.SelectedData.ID != "";
            }
            else
            {
                btnOK.Enabled = (cmbCurrency.SelectedData.ID != "" &&
                   cmbAccountCode.SelectedIndex >= 0 &&
                   ((cmbAccountSelection.Enabled && cmbAccountSelection.SelectedData.ID != "") || !cmbAccountSelection.Enabled) &&
                   cmbUnit.SelectedData.ID != "") &&
                   (cmbCurrency.SelectedData.ID != agreement.Currency ||
                   (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex != agreement.AccountCode ||
                   cmbAccountSelection.SelectedData.ID != agreement.AccountRelation ||
                   cmbUnit.SelectedData.ID != agreement.UnitID ||
                   Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate ||
                   Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate ||
                   (decimal)ntbQuantity.Value != agreement.QuantityAmount ||
                   (decimal)ntbPrice.Value != agreement.Amount ||
                   (decimal)ntbPriceWithVAT.Value != agreement.AmountIncludingTax ||
                   (decimal)ntbMiscCharges.Value != agreement.Markup);
            }
        }

        private void cmbVariantNumber_FormatData(object sender, DropDownFormatDataArgs e)
        {
            e.TextToDisplay = (((DualDataComboBox)sender).SelectedData)[0];
        }

        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            cmbVariantNumber.SelectedData = new DataEntity();

        }
      
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);            
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void ntbPrice_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);

            if (itemID == RecordIdentifier.Empty)
            {
                // We are in multiedit
                ntbPriceWithVAT.Enabled = (ntbPrice.Text.Length == 0);

                if(ntbPrice.Text.Length != 0)
                {
                    ntbPriceWithVAT.Text = "";
                }
            }
        }

        private void cmbAccountSelection_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbAccountSelection_RequestData(object sender, EventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 1)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup), null);
            }
        }

        private void ntbPrice_CalculateAndSetPriceWithTax(object sender, EventArgs e)
        {
            if (itemID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
            }
            else
            {
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                decimal priceWithoutTax = (decimal)ntbPrice.Value;
                decimal taxAmount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTaxForAmount(PluginEntry.DataModel, item.SalesTaxItemGroupID, priceWithoutTax);

                decimal priceWithTax = priceWithoutTax + taxAmount;

                ntbPriceWithVAT.Text = priceWithTax.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                // Display errors if necessary
                bool defaultStoreExists;
                bool defaultStoreHasTaxGroup;
                bool itemHasTaxGroup;

                RecordIdentifier itemSalesTaxGroupID = item.SalesTaxItemGroupID;
                RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, defaultStoreID);

                defaultStoreExists = defaultStoreID != "";
                defaultStoreHasTaxGroup = defaultStoreTaxGroup != "";
                itemHasTaxGroup = (itemSalesTaxGroupID != "");

                ShowErrorMessages(defaultStoreExists, defaultStoreHasTaxGroup, itemHasTaxGroup);
            }
        }

        private void ntbPriceWithVAT_CalculateAndSetPriceWithoutTax(object sender, EventArgs e)
        {
            if (itemID == RecordIdentifier.Empty)
            {
                // We are in multiedit mode
            }
            else
            {
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                decimal priceWithTax = (decimal)ntbPriceWithVAT.Value;
                decimal priceWithoutTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, item.SalesTaxItemGroupID, priceWithTax);

                ntbPrice.Text = priceWithoutTax.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

                // Display errors if necessary
                bool defaultStoreExists = true;
                bool defaultStoreHasTaxGroup = true;
                bool itemHasTaxGroup = true;

                RecordIdentifier itemSalesTaxGroupID = item.SalesTaxItemGroupID;
                RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, defaultStoreID);

                defaultStoreExists = defaultStoreID != "";
                defaultStoreHasTaxGroup = defaultStoreTaxGroup != "";
                itemHasTaxGroup = (itemSalesTaxGroupID != "");

                ShowErrorMessages(defaultStoreExists, defaultStoreHasTaxGroup, itemHasTaxGroup);
            }
        }

        private void ShowErrorMessages(bool defaultStoreExists, bool defaultStoreHasTaxGroup, bool itemHasTaxGroup)
        {
            if (!defaultStoreExists)
            {
                errorProvider1.SetError(ntbPriceWithVAT, Resources.NoDefaultStoreSelected);
                return;
            }

            if (!defaultStoreHasTaxGroup)
            {
                errorProvider1.SetError(ntbPriceWithVAT, Resources.DefaultStoreHasNoTaxGroupAttachedToIt);
                return;
            }

            if (!itemHasTaxGroup)
            {
                errorProvider1.SetError(ntbPriceWithVAT, Resources.ItemHasNoTaxGroupAttachedToIt);
                return;
            }
            errorProvider1.Clear();
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitEditor.IsAlive)
            {
                ((IPlugin)unitEditor.Target).Message(this, "AddUnits", cmbUnit.SelectedData.ID);
            }
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void ntbPriceWithVAT_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);

            if (itemID == RecordIdentifier.Empty)
            {
                // We are in multiedit
                ntbPrice.Enabled = (ntbPriceWithVAT.Text.Length == 0);

                if (ntbPriceWithVAT.Text.Length != 0)
                {
                    ntbPrice.Text = "";
                }
            }
        }

        private void ntbMiscCharges_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                item.ItemType == ItemTypeEnum.MasterItem ?
                item.MasterID :
                item.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }
    }
}