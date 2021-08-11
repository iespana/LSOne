using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class TradeAgreementTotalDiscountDialog : DialogBase
    {
        RecordIdentifier id;
        TradeAgreementEntryAccountCode type;
        TradeAgreementEntry agreement;
        WeakReference currencyEditor;

        public TradeAgreementTotalDiscountDialog(RecordIdentifier id, RecordIdentifier agreementID, TradeAgreementEntryAccountCode type)
            : this(id,type)
        {
            agreement = Providers.TradeAgreementData.GetTotalDiscount(PluginEntry.DataModel, agreementID);


            cmbCurrency.SelectedData = new DataEntity(agreement.Currency, agreement.CurrencyDescription);


            agreement.FromDate.ToDateControl(dtpFromDate);
            agreement.ToDate.ToDateControl(dtpToDate);


            ntbAmount.Text = agreement.QuantityAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            ntbDiscount.Text = agreement.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            
            ntbPercent1.Value = (double)agreement.Percent1;
            ntbPercent2.Value = (double)agreement.Percent2;

            CheckEnabled(this, EventArgs.Empty);
        }

        public TradeAgreementTotalDiscountDialog(RecordIdentifier id, TradeAgreementEntryAccountCode type)
            : this()
        {
            CompanyInfo companyInfo;

            agreement = null;

            this.type = type;
            this.id = id;

            companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,false);

            cmbCurrency.SelectedData = new DataEntity(companyInfo.CurrencyCode,companyInfo.CurrencyCodeText);
        }

        public TradeAgreementTotalDiscountDialog()
            : base()
        {
            IPlugin plugin;

            InitializeComponent();

            cmbCurrency.SelectedData = new DataEntity("", "");

            ntbAmount.DecimalLetters = ntbDiscount.DecimalLetters =
                PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);

            currencyEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnAddCurrency.Visible = (currencyEditor != null);
        }

        public RecordIdentifier ID
        {
            get
            {
                return agreement.ID;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
      
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void PopulateTAEntry(TradeAgreementEntry taAgreement)
        {
            taAgreement.Currency = cmbCurrency.SelectedData.ID;
            taAgreement.AccountCode = type;
            taAgreement.AccountRelation = id;
            taAgreement.FromDate = Date.FromDateControl(dtpFromDate).GetDateWithoutTime();
            taAgreement.ToDate = Date.FromDateControl(dtpToDate).GetDateWithoutTime();
            taAgreement.QuantityAmount = (decimal)ntbAmount.Value;
            taAgreement.Relation = TradeAgreementEntry.TradeAgreementEntryRelation.EndDiscSales;
            taAgreement.ItemCode = TradeAgreementEntry.TradeAgreementEntryItemCode.All;
            taAgreement.Amount = (decimal)ntbDiscount.Value;
            taAgreement.Percent1 = (decimal)ntbPercent1.Value;
            taAgreement.Percent2 = (decimal)ntbPercent2.Value;
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
                    agreement = null;
                    return;
                }

                agreement = new TradeAgreementEntry();
                PopulateTAEntry(agreement);
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
        
        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (agreement == null)
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "";
            }
            else
            {
                btnOK.Enabled = cmbCurrency.SelectedData.ID != "" &&
                    (
                     cmbCurrency.SelectedData.ID != agreement.Currency ||
                     Date.FromDateControl(dtpFromDate).GetDateWithoutTime() != agreement.FromDate ||
                     Date.FromDateControl(dtpToDate).GetDateWithoutTime() != agreement.ToDate ||
                     ntbAmount.Value != (double)agreement.QuantityAmount ||
                     ntbDiscount.Value != (double)agreement.Amount ||
                     ntbPercent1.Value != (double)agreement.Percent1 ||
                     ntbPercent2.Value != (double)agreement.Percent2
                    );
            }
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
