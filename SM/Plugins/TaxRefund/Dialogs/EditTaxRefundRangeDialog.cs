using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.TaxRefund.Dialogs
{
    public partial class EditTaxRefundRangeDialog : DialogBase
    {
        private TaxRefundRange range;
        private List<TaxRefundRange> knownRanges;

        public EditTaxRefundRangeDialog()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                ntbFrom.DecimalLetters = ntbTo.DecimalLetters = ntbTaxRefundRange.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
                ntbTaxRefundPct.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax).Max;
            }

            ntbFrom.MaxValue = ntbTo.MaxValue = ntbTaxRefundRange.MaxValue = double.MaxValue;
        }

        public EditTaxRefundRangeDialog(TaxRefundRange range, List<TaxRefundRange> knowRanges)
            : this()
        {
            this.range = range;
            this.knownRanges = knowRanges;
            if (range != null)
            {
                ntbFrom.Value = (double) range.ValueFrom;
                ntbTo.Value = (double) range.ValueTo;
                ntbTaxRefundRange.Value = (double) range.TaxRefund;
                ntbTaxRefundPct.Value = (double) range.TaxRefundPercentage;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (range == null)
            {
                range = new TaxRefundRange();
            }
            range.ValueFrom = (decimal) ntbFrom.Value;
            range.ValueTo = (decimal) ntbTo.Value;
            range.TaxRefund = (decimal) ntbTaxRefundRange.Value;
            Providers.TaxRefundRangeData.Save(PluginEntry.DataModel, range);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ntb_TextChanged(object sender, EventArgs e)
        {
            bool enabled;
            if (knownRanges == null)
            {
                enabled = !Providers.TaxRefundRangeData.Exists(PluginEntry.DataModel, (decimal) ntbFrom.Value, (decimal) ntbTo.Value);
            }
            else
            {
                enabled = !knownRanges.Any(r => (r.ValueFrom >= (decimal) ntbFrom.Value && r.ValueTo <= (decimal) ntbFrom.Value) ||
                    (r.ValueFrom <= (decimal) ntbTo.Value && r.ValueTo >= (decimal) ntbTo.Value));
            }

            btnOK.Enabled = enabled && ntbFrom.Value <= ntbTo.Value;
        }

        private void OnTaxPercentageChanged(object sender, EventArgs e)
        {
            ntbTaxRefundRange.Enabled = ntbTaxRefundPct.Value == (double)0;
        }
    }
}
