using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Properties;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreementPricesCustGroupDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier id;
        TradeAgreementEntryAccountCode type;
        WeakReference unitEditor;
        WeakReference currencyEditor;
        TradeAgreementEntry agreement;
        RetailItem retailItem;

        public TradeAgreementPricesCustGroupDialog(RecordIdentifier id, RecordIdentifier agreementID, TradeAgreementEntryAccountCode type)
            : this(id,type)
        {
            DecimalLimit priceLimiter;
            Unit unit;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID, TradeAgreementRelation.PriceSales);

            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);

            cmbItem.SelectedData = new DataEntity(agreement.ItemRelation, agreement.ItemName);
            cmbItem_SelectedDataChanged(cmbItem, EventArgs.Empty);

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

        public TradeAgreementPricesCustGroupDialog(RecordIdentifier id, TradeAgreementEntryAccountCode type)
            : this()
        {
            RecordIdentifier salesUnit = "";
            int salesUnitDecimals = 0;
            string salesUnitText = "";
            CompanyInfo companyInfo;

            agreement = null;

            cmbUnit.SelectedData = new Unit();                        

            this.type = type;
            this.id = id;

            // Automatically select a unit if applies
            cmbUnit.SelectedData = new Unit(salesUnit, salesUnitText, salesUnitDecimals, salesUnitDecimals);

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
            cmbItem.SelectedData = new DataEntity("","");

            cmbVariantNumber.SelectedData = new Dimension();
        }

        public TradeAgreementPricesCustGroupDialog()
        {
            IPlugin plugin;

            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");

            ntbQuantity.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
            ntbPrice.DecimalLetters =
                ntbPriceWithVAT.DecimalLetters =
                ntbMiscCharges.DecimalLetters =
                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddUnits", null);
            unitEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddUnit.Visible = (unitEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);
            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddCurrency.Visible = (currencyEditor != null);
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

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.AccountCode = type;
            taEntry.AccountRelation = id;
            taEntry.UnitID = cmbUnit.SelectedData.ID;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.PriceSales;
            taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
            RecordIdentifier target;
            if (cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
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
                if (cmbItem.SelectedData is MasterIDEntity)
                {
                    target = (cmbItem.SelectedData as MasterIDEntity).ReadadbleID;
                }
                else
                {
                    target = (cmbItem.SelectedData as DataEntity).ID;
                }
            }
            taEntry.ItemRelation = target;
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taEntry.Amount = (decimal)ntbPrice.Value;
            taEntry.AmountIncludingTax = (decimal)ntbPriceWithVAT.Value;
            taEntry.Markup = (decimal)ntbMiscCharges.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Resources.FromDateCannotBeHigherThanToDate);
                return;
            }

            if (ActiveControl != btnOK)
            {
                btnOK.Focus();
            }

            if (agreement == null)
            {
                TradeAgreementEntry testAgreement = new TradeAgreementEntry();
                PopulateTAEntry(testAgreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testAgreement.OldID))
                {
                    errorProvider1.SetError(cmbCurrency, Resources.TradeAgreementExists1);
                    return;
                }

                agreement = new TradeAgreementEntry();
                PopulateTAEntry(agreement);
            }
            else
            {
                PopulateTAEntry(agreement);
                if ((Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, agreement.OldID, agreement.ID)))
                {
                    errorProvider1.SetError(cmbCurrency, Resources.TradeAgreementExists1);
                    return;
                }
            }
            
            Providers.TradeAgreementData.Save(PluginEntry.DataModel, agreement, Permission.ManageTradeAgreementPrices);

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

        private void cmbUnit_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier salesUnitID;

            cmbUnit.SetWidth(200);

            salesUnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, cmbItem.SelectedData.ID, RetailItem.UnitTypeEnum.Sales);

            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
                    { Providers.UnitConversionData.GetConvertableTo(PluginEntry.DataModel,cmbItem.SelectedData.ID,salesUnitID),
                     Providers.UnitData.GetAllUnits(PluginEntry.DataModel)};

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
                ntbQuantity.DecimalLetters = ((Unit)cmbUnit.SelectedData).MaximumDecimals;
            }

            CheckEnabled(sender, EventArgs.Empty);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (agreement == null)
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "" &&
                    cmbItem.SelectedData.ID != "" &&
                    cmbUnit.SelectedData.ID != "";
            }
            else
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "" &&
                    cmbItem.SelectedData.ID != "" &&
                    cmbUnit.SelectedData.ID != "" &&
                   (cmbCurrency.SelectedData.ID != agreement.Currency
                   || cmbUnit.SelectedData.ID != agreement.UnitID
                   || Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate 
                   || Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate
                   || (decimal)ntbQuantity.Value != agreement.QuantityAmount
                   || (decimal)ntbMiscCharges.Value != agreement.Markup
                   || (decimal)ntbPriceWithVAT.Value != agreement.AmountIncludingTax
                   || (decimal)ntbPrice.Value != agreement.Amount
                   || (string)cmbItem.SelectedData.ID != agreement.ItemID
                   );
            }
        }

        private void cmbVariantNumber_RequestClear(object sender, EventArgs e)
        {
            cmbVariantNumber.SelectedData = new Dimension();
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
        }

        private void cmbItem_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbUnit.Enabled = cmbItem.SelectedData.ID != "";
            if (cmbItem.SelectedData.ID != "")
            {
                retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedData.ID);
                if (retailItem == null)
                {
                    throw new DataIntegrityException(typeof(RetailItem), cmbItem.SelectedData.ID);
                }
                if (!RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
                {
                    cmbItem.SelectedData = new DataEntity(retailItem.HeaderItemID, retailItem.Text);
                    cmbVariantNumber.SelectedData = new DataEntity(retailItem.ID, retailItem.VariantName);
                }
                else
                {
                    cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
                }
                cmbVariantNumber.Enabled = cmbUnit.Enabled && (retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull( retailItem.HeaderItemID));
                lblVariant.ForeColor = cmbVariantNumber.Enabled
                    ? SystemColors.ControlText
                    : SystemColors.GrayText;

                if (cmbUnit.Enabled == false)
                {
                    cmbUnit.SelectedData = new Unit();
                    cmbVariantNumber.SelectedData = new Dimension();
                }
                else
                {
                    if (cmbUnit.SelectedData.ID == "")
                    {
                        RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedData.ID);
                        Unit itemSalesUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.SalesUnitID);

                        cmbUnit.SelectedData = itemSalesUnit;
                    }
                }
            }
            CheckEnabled(this, EventArgs.Empty);
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
                initialSearchText = cmbItem.SelectedData == null ? cmbItem.Text : ((DataEntity)cmbItem.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText,
                SearchTypeEnum.RetailItemsMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void ntbPrice_CalculateAndSetPriceWithTax(object sender, EventArgs e)
        {
            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedData.ID);
            if (item == null)
            {
                return;
            }
            decimal priceWithoutTax = (decimal)ntbPrice.Value;
            decimal taxAmount = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTaxForAmount(PluginEntry.DataModel, item.SalesTaxItemGroupID, priceWithoutTax);

            decimal priceWithTax = priceWithoutTax + taxAmount;

            ntbPriceWithVAT.Text = priceWithTax.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

            bool defaultStoreExists;
            bool defaultStoreHasTaxGroup;
            bool itemHasTaxGroup;

            RecordIdentifier itemSalesTaxGroupID = item != null ? item.SalesTaxItemGroupID : "";
            RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
            RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, defaultStoreID);

            defaultStoreExists = defaultStoreID != "";
            defaultStoreHasTaxGroup = defaultStoreTaxGroup != "";
            itemHasTaxGroup = (itemSalesTaxGroupID != "");

            ShowErrorMessages(defaultStoreExists, defaultStoreHasTaxGroup, itemHasTaxGroup);
        }

        private void ntbPriceWithVAT_CalculateAndSetPriceWithoutTax(object sender, EventArgs e)
        {
            RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbItem.SelectedData.ID);
            if (item == null)
            {
                return;
            }
            decimal priceWithTax = (decimal)ntbPriceWithVAT.Value;
            decimal priceWithoutTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemPriceForItemPriceWithTax(PluginEntry.DataModel, item.SalesTaxItemGroupID, priceWithTax);

            ntbPrice.Text = priceWithoutTax.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

            bool defaultStoreExists;
            bool defaultStoreHasTaxGroup;
            bool itemHasTaxGroup;

            RecordIdentifier itemSalesTaxGroupID = item != null ? item.SalesTaxItemGroupID : "";
            RecordIdentifier defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);
            RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetStoresSalesTaxGroupID(PluginEntry.DataModel, defaultStoreID);

            defaultStoreExists = defaultStoreID != "";
            defaultStoreHasTaxGroup = defaultStoreTaxGroup != "";
            itemHasTaxGroup = (itemSalesTaxGroupID != "");

            ShowErrorMessages(defaultStoreExists, defaultStoreHasTaxGroup, itemHasTaxGroup);
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

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            if (unitEditor.IsAlive)
            {
                ((IPlugin)unitEditor.Target).Message(this, "AddUnits", cmbUnit.SelectedData.ID);
            }
        }

        private void ntbPriceWithVAT_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void ntbMiscCharges_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, EventArgs.Empty);
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
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
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }
    }
}