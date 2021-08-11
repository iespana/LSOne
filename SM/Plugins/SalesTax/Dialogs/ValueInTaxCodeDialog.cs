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

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class ValueInTaxCodeDialog : DialogBase
    {
        RecordIdentifier taxCode;
        //TaxCodeValue existingTaxCodeLine;
        TaxCodeValue taxCodeLine;

        public ValueInTaxCodeDialog(RecordIdentifier taxCode, RecordIdentifier lineID)
            : this()
        {
            this.taxCode = taxCode;
            taxCodeLine = Providers.TaxCodeValueData.Get(PluginEntry.DataModel, lineID);

            taxCodeLine.FromDate.ToDateControl(dtpFromDate);
            taxCodeLine.ToDate.ToDateControl(dtpToDate);
            ntbValue.Text = taxCodeLine.Value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax));

            CheckChanged(this, EventArgs.Empty);
        }

        public ValueInTaxCodeDialog(RecordIdentifier taxCode)
            : this()
        {
            this.taxCode = taxCode;
        }

        private ValueInTaxCodeDialog()
        {
            InitializeComponent();

            ntbValue.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax).Max;
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
            if (dtpFromDate.Checked && dtpToDate.Checked)
            {
                if (dtpToDate.Value < dtpFromDate.Value)
                {
                    errorProvider1.SetError(dtpToDate, Properties.Resources.ToDateMayNotBeEarlierThanFromDate);
                    dtpToDate.Focus();
                    return;
                }
            }

            if (!dtpFromDate.Checked && dtpToDate.Checked)
            {
                errorProvider1.SetError(dtpFromDate, Properties.Resources.ToDateWithoutFromDateIsNotValid);
                dtpFromDate.Focus();
                return;
            }

            if (taxCodeLine == null)
            { // New taxcode, then we don't have to exclude anything from the range test
                if (Providers.TaxCodeValueData.RangeExists(PluginEntry.DataModel, (string)taxCode, Date.FromDateControl(dtpFromDate), Date.FromDateControl(dtpToDate)))
                {
                    errorProvider1.SetError(dtpFromDate, Properties.Resources.TaxCodeValueCoveringThisRangeAlreadyExists);
                    errorProvider2.SetError(dtpToDate, Properties.Resources.TaxCodeValueCoveringThisRangeAlreadyExists);
                    dtpFromDate.Focus();
                    return;
                }
            }
            else
            {
                if (Providers.TaxCodeValueData.RangeExists(PluginEntry.DataModel, taxCodeLine, (string)taxCode, Date.FromDateControl(dtpFromDate), Date.FromDateControl(dtpToDate)))
                {
                    errorProvider1.SetError(dtpFromDate, Properties.Resources.TaxCodeValueCoveringThisRangeAlreadyExists);
                    errorProvider2.SetError(dtpToDate, Properties.Resources.TaxCodeValueCoveringThisRangeAlreadyExists);
                    dtpFromDate.Focus();
                    return;
                }
            }
            
            
            if (taxCodeLine == null)
            {
                taxCodeLine = new TaxCodeValue();
                taxCodeLine.ID = Guid.NewGuid();
            }

            taxCodeLine.TaxCode = taxCode;
            taxCodeLine.FromDate = Date.FromDateControl(dtpFromDate);
            taxCodeLine.ToDate = Date.FromDateControl(dtpToDate);
            taxCodeLine.Value = (decimal)ntbValue.Value;

            Providers.TaxCodeValueData.Save(PluginEntry.DataModel, taxCodeLine);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            if (taxCodeLine != null)
            {
                btnOK.Enabled =
                    taxCodeLine.FromDate != Date.FromDateControl(dtpFromDate) ||
                    taxCodeLine.ToDate != Date.FromDateControl(dtpToDate) ||
                    taxCodeLine.Value != (decimal)ntbValue.Value;
            }
            else
            {
                btnOK.Enabled = true;
            }
            
        }

        private void DateChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();

            CheckChanged(this, EventArgs.Empty);
        }

        private void LimitChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();

            CheckChanged(this, EventArgs.Empty);
        }
       
    }
}
