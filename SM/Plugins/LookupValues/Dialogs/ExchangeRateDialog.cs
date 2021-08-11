using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class ExchangeRateDialog : DialogBase
    {
        ExchangeRate exchangeRate = null;
        bool newRecord;
        DateTime oldDate;

        public ExchangeRateDialog(RecordIdentifier currencyCode, DateTime startDate)
            : this()
        {
            newRecord = false;

            exchangeRate = Providers.ExchangeRatesData.Get(PluginEntry.DataModel, new RecordIdentifier(currencyCode,startDate));

            tbToCurrency.Text = (string)exchangeRate.CurrencyCode;
            dtpStartDate.Value = (DateTime)exchangeRate.FromDate;
            ntbExchangeRate.Value = (double)exchangeRate.ExchangeRateValue / 100;
            ntbPOSExchangeRate.Value = (double)exchangeRate.POSExchangeRateValue / 100;

            oldDate = (DateTime)exchangeRate.FromDate;
        }

        public ExchangeRateDialog(RecordIdentifier currencyCode)
            : this()
        {
            newRecord = true;

            oldDate = new DateTime(1, 1, 1);

            tbToCurrency.Text = (string)currencyCode;

        }

        private ExchangeRateDialog()
            : base()
        {
            exchangeRate = null;

            InitializeComponent();

            SetCompanyCurrency();
        }

        private void SetCompanyCurrency()
        {
            Currency companyCurrency = Providers.CurrencyData.GetCompanyCurrency(PluginEntry.DataModel);

            if (companyCurrency == null || companyCurrency.ID == "")
            {
                throw new DataIntegrityException(Properties.Resources.CompanyCurrencyMissing);
            }
            
            tbFromCurrency.Text = (string)companyCurrency.ID;
        }

        public RecordIdentifier selectedId
        {
            get { return exchangeRate != null ? exchangeRate.ID : RecordIdentifier.Empty; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if (newRecord == true)
            {
                if (Providers.ExchangeRatesData.Exists(PluginEntry.DataModel, new RecordIdentifier(tbToCurrency.Text, dtpStartDate.Value.Date)))
                {
                    errorProvider1.SetError(dtpStartDate, Properties.Resources.ExchangeRateExists);
                    return;
                }
            
                exchangeRate = new ExchangeRate();
                exchangeRate.CurrencyCode = tbToCurrency.Text;
            }

            exchangeRate.FromDate = dtpStartDate.Value.Date;

            // We multiply by 100 because the db stores exchange values for buying 100 of a currency, but we show it
            // to the user like he is buying just 1.
            exchangeRate.ExchangeRateValue = (decimal)ntbExchangeRate.Value * 100;
            exchangeRate.POSExchangeRateValue = (decimal)ntbPOSExchangeRate.Value * 100;

            Providers.ExchangeRatesData.Save(PluginEntry.DataModel, exchangeRate, oldDate);


            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (exchangeRate == null)
            {
                btnOK.Enabled = tbFromCurrency.Text != "" && tbToCurrency.Text != "" &&
                                dtpStartDate.Value != null && ntbExchangeRate.Value != 0;
            }
            else
            {
                btnOK.Enabled = dtpStartDate.Value != null && ntbExchangeRate.Value != 0;
            }
        }
        
    }
}
