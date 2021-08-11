using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PaymentLimitations.Properties;

namespace LSOne.ViewPlugins.PaymentLimitations.Dialogs
{
    public partial class LimitationCodeDialog : DialogBase
    {
        private RecordIdentifier limitationCode;
        private bool taxExempt;

        public LimitationCodeDialog(RecordIdentifier limitationCode, bool taxExempt)
            :this()
        {
            this.limitationCode = limitationCode;
            chkTaxExempt.Checked = taxExempt;
            this.taxExempt = taxExempt;
            tbLimitationCode.ReadOnly = true;
            tbLimitationCode.Text = (string)limitationCode;
        }

        public LimitationCodeDialog()
        {
            InitializeComponent();
            limitationCode = RecordIdentifier.Empty;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier LimitationCode => limitationCode;

        public bool TaxExempt => taxExempt;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (RecordIdentifier.IsEmptyOrNull(limitationCode) && Providers.PaymentLimitationsData.RestrictionCodeExists(PluginEntry.DataModel, tbLimitationCode.Text.Trim()))
            {
                errorProvider1.SetError(tbLimitationCode, Resources.LimitationCodeAlreadyExists.Replace("{0}", tbLimitationCode.Text.Trim()));
                return;
            }

            DialogResult = DialogResult.OK;
            limitationCode = tbLimitationCode.Text.Trim();
            taxExempt = chkTaxExempt.Checked;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = tbLimitationCode.Text.Length > 0 && (limitationCode == RecordIdentifier.Empty || chkTaxExempt.Checked != taxExempt);
        }
    }
}
