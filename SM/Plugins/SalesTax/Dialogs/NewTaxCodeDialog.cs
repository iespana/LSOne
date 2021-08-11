using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SalesTax.Properties;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class NewTaxCodeDialog : DialogBase
    {
        RecordIdentifier taxCodeID = RecordIdentifier.Empty;
        private bool manuallyEnterId = false;

        public NewTaxCodeDialog()
        {
            InitializeComponent();

            var parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterId = parameters.ManuallyEnterTaxCodeID;

            tbID.Visible = manuallyEnterId;
            lblID.Visible = manuallyEnterId;

            ntbRoundoff.Value = 0.01;

            ntbValue.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax).Max;
        }

        public RecordIdentifier TaxCodeID
        {
            get
            {
                return taxCodeID;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TaxCode taxCode;
            TaxCodeValue taxCodeLine;

            bool IsValid = true;

            if (manuallyEnterId)
            {
                if (tbID.Text.Trim() == "")
                {
                    DialogResult blnDialogResult = QuestionDialog.Show(Resources.IDMissingQuestion, Resources.IDMissing);
                    if (blnDialogResult != DialogResult.Yes)
                    {
                        IsValid = false;
                    }
                    else if (blnDialogResult == DialogResult.Yes)
                    {
                        IsValid = ValidateID(IsValid, true);
                    }
                }
                else
                {
                    IsValid = ValidateID(IsValid, false);
                }
            }
            else
            {
                IsValid = ValidateID(IsValid, true);
            }

            if (IsValid && dtpFromDate.Checked && dtpToDate.Checked)
            {
                if (dtpToDate.Value < dtpFromDate.Value)
                {
                    errorProvider1.SetError(dtpToDate, Resources.ToDateMayNotBeEarlierThanFromDate);
                    dtpToDate.Focus();
                    return;
                }
            }

            if (IsValid)
            {
                taxCode = new TaxCode();
                taxCode.ID = taxCodeID;
                taxCode.Text = tbDescription.Text;
                taxCode.TaxRoundOff = (decimal)ntbRoundoff.Value;
                taxCode.TaxRoundOffType = (TaxCode.RoundoffTypeEnum)cmbType.SelectedIndex;
                taxCode.ReceiptDisplay = tbReceiptDisplay.Text;

                Providers.TaxCodeData.Save(PluginEntry.DataModel, taxCode);
                taxCodeID = taxCode.ID;

                taxCodeLine = new TaxCodeValue();
                taxCodeLine.TaxCode = taxCode.ID;
                taxCodeLine.FromDate = Date.FromDateControl(dtpFromDate);
                taxCodeLine.ToDate = Date.FromDateControl(dtpToDate);
                taxCodeLine.Value = (decimal)ntbValue.Value;

                Providers.TaxCodeValueData.Save(PluginEntry.DataModel, taxCodeLine);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckChanged(this, EventArgs.Empty);
        }

        private void BoundariesChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text != ""
                && cmbType.SelectedIndex >= 0
                && ntbRoundoff.Value != 0;
        }

        private bool ValidateID(bool IsValid, bool blnDialogBox)
        {
            if (tbID.Enabled)
            {
                if (!tbID.Text.IsAlphaNumeric())
                {
                    errorProvider1.SetError(tbID, Resources.OnlyCharAndNumbers);
                    IsValid = false;
                }
            }

            if (!blnDialogBox)
            {
                taxCodeID = tbID.Text.Trim();
            }

            if (tbID.Enabled)
            {
                if (Providers.TaxCodeData.Exists(PluginEntry.DataModel, taxCodeID))
                {
                    errorProvider1.SetError(tbID, Resources.TaxCodeIDExists);
                    IsValid = false;
                }
            }

            return IsValid;
        }

    }
}
