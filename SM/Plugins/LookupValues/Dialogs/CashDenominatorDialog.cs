using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class CashDenominatorDialog : DialogBase
    {
        private CashDenominator cashDenominator;

        public CashDenominatorDialog(CashDenominator cashDenominator)
            : this()
        {
            this.cashDenominator = cashDenominator;

            tbCurrency.Text = cashDenominator.CurrencyCode;
            cmbType.SelectedIndex = (int)cashDenominator.CashType;
            ntbAmount.Value = (double)cashDenominator.Amount;
            tbDenomination.Text = cashDenominator.Denomination;

            cmbType.Enabled =
            ntbAmount.Enabled = false;
        }

        public CashDenominatorDialog(RecordIdentifier currencyCode) : this()
        {
            tbCurrency.Text = (string)currencyCode;
        }

        public CashDenominatorDialog()
            : base()
        {
            InitializeComponent();
            ntbAmount.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
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
            if (cashDenominator == null && Providers.CashDenominatorData.Exists(PluginEntry.DataModel, new RecordIdentifier((decimal)ntbAmount.Value, new RecordIdentifier(cmbType.SelectedIndex, tbCurrency.Text))))
            {
                errorProvider1.SetError(tbCurrency, Properties.Resources.CashDeclarationExists);
                return;
            }

            if(cashDenominator == null)
            {
                cashDenominator = new CashDenominator();
                cashDenominator.CurrencyCode = tbCurrency.Text;
                cashDenominator.CashType = (CashDenominator.Type)cmbType.SelectedIndex;
                cashDenominator.Amount = (decimal)ntbAmount.Value;
            }

            cashDenominator.Denomination = tbDenomination.Text.Trim();

            Providers.CashDenominatorData.Save(PluginEntry.DataModel, cashDenominator);

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

            if(cashDenominator == null)
            {
                btnOK.Enabled = ntbAmount.Value != 0.0 && cmbType.SelectedIndex != -1;
            }
            else
            {
                btnOK.Enabled = cashDenominator.Denomination != tbDenomination.Text;
            }
        }

        
    }
}
