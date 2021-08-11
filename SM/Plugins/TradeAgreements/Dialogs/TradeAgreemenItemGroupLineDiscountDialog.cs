using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreemenItemGroupLineDiscountDialog : DialogBase
    {
        RecordIdentifier storeID;
        RecordIdentifier itemID;
        TradeAgreementEntry agreement;
        WeakReference currencyEditor;

        public TradeAgreemenItemGroupLineDiscountDialog(RecordIdentifier id, RecordIdentifier agreementID)
            : this(id)
        {
            DecimalLimit priceLimiter;
            Unit unit;

            agreement = Providers.TradeAgreementData.Get(PluginEntry.DataModel, agreementID, TradeAgreementRelation.LineDiscSales);

            
            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);

            cmbAccountCode.SelectedIndex = (int)agreement.AccountCode;
            cmbAccountSelection.SelectedData = new DataEntity(agreement.AccountRelation,agreement.AccountName);

            unit = Providers.UnitData.Get(PluginEntry.DataModel, agreement.UnitID);

            if (unit == null)
            {
                unit = new Unit(agreement.UnitID, (string)agreement.UnitID, 0,0);
            }


            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            ntbQuantity.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity));
            ntbDiscount.Text = agreement.Amount.FormatWithLimits(priceLimiter);
            ntbDiscountPercentage1.Value = (double)agreement.Percent1;
            ntbDiscountPercentage2.Value = (double)agreement.Percent2;

            CheckEnabled(this, EventArgs.Empty);
        }

        public TradeAgreemenItemGroupLineDiscountDialog(RecordIdentifier id)
            : this()
        {
            agreement = null;

            
            CompanyInfo companyInfo;

            itemID = id;

            

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
            cmbAccountSelection.SelectedData = new DataEntity("","");
            cmbAccountCode.SelectedIndex = 2;
        }

        public TradeAgreemenItemGroupLineDiscountDialog()
            : base()
        {
            IPlugin plugin;

            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");


            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);

            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnAddCurrency.Visible = (currencyEditor != null);


            ntbQuantity.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity).Max;
            ntbDiscount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void PopulateTAEntry(TradeAgreementEntry taEntry)
        {
            taEntry.Currency = cmbCurrency.SelectedData.ID;
            taEntry.AccountCode = (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex;
            taEntry.AccountRelation = cmbAccountSelection.SelectedData.ID;
            taEntry.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taEntry.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taEntry.QuantityAmount = (decimal)ntbQuantity.Value;
            taEntry.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales;
            taEntry.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.Group;
            taEntry.ItemRelation = itemID;
            taEntry.Amount = (decimal)ntbDiscount.Value;
            taEntry.Percent1 = (decimal)ntbDiscountPercentage1.Value;
            taEntry.Percent2 = (decimal)ntbDiscountPercentage2.Value;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Date.FromDateControl(dtpFromDate) > Date.FromDateControl(dtpToDate) && !Date.FromDateControl(dtpToDate).IsEmpty)
            {
                errorProvider1.SetError(dtpFromDate, Properties.Resources.FromDateCannotBeHigherThanToDate);
                return;
            }

            if (agreement == null)
            {
                TradeAgreementEntry testAgreement = new TradeAgreementEntry();
                PopulateTAEntry(testAgreement);
                if (Providers.TradeAgreementData.DataContentExists(PluginEntry.DataModel, testAgreement.OldID))
                {
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
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
                    errorProvider1.SetError(cmbCurrency, Properties.Resources.TradeAgreementExists1);
                    return;
                }
            }
            
            Providers.TradeAgreementData.Save(PluginEntry.DataModel, agreement, Permission.ManageDiscounts);

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
                    ((cmbAccountSelection.Enabled && cmbAccountSelection.SelectedData.ID != "") || !cmbAccountSelection.Enabled);
            }
            else
            {
                btnOK.Enabled = (cmbCurrency.SelectedData.ID != "" &&
                   cmbAccountCode.SelectedIndex >= 0 &&
                   ((cmbAccountSelection.Enabled && cmbAccountSelection.SelectedData.ID != "") || !cmbAccountSelection.Enabled)) &&
                   (cmbCurrency.SelectedData.ID != agreement.Currency ||
                   (TradeAgreementEntryAccountCode)cmbAccountCode.SelectedIndex != agreement.AccountCode ||
                   cmbAccountSelection.SelectedData.ID != agreement.AccountRelation ||
                   Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate ||
                   Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate ||
                   (decimal)ntbQuantity.Value != agreement.QuantityAmount ||
                   (decimal)ntbDiscount.Value != agreement.Amount ||
                   (decimal)ntbDiscountPercentage1.Value != agreement.Percent1 ||
                   (decimal)ntbDiscountPercentage2.Value != agreement.Percent2);
            }
        }        

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbQuantity_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void ntbPrice_TextChanged(object sender, EventArgs e)
        {            
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbAccountSelection_RequestClear(object sender, EventArgs e)
        {

        }

        private void cmbAccountSelection_RequestData(object sender, EventArgs e)
        {
            if (cmbAccountCode.SelectedIndex == 1)
            {
                cmbAccountSelection.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.LineDiscountGroup), null);
            }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (currencyEditor.IsAlive)
            {
                ((IPlugin)currencyEditor.Target).Message(this, "AddCurrency", cmbCurrency.SelectedData.ID);
            }
        }               
   
    }
}
