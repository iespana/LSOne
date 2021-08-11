using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class EditTaxCodeDialog : DialogBase
    {
        TaxCode taxCode;


        public EditTaxCodeDialog(RecordIdentifier taxCodeID)
            : this()
        {
            taxCode = Providers.TaxCodeData.Get(PluginEntry.DataModel, taxCodeID);

            //tbID.Text = (string)taxCode.ID;
            tbDescription.Text = taxCode.Text;
            cmbType.SelectedIndex = (int)taxCode.TaxRoundOffType;
            ntbRoundoff.Text = taxCode.TaxRoundOff.FormatTruncated();
            tbReceiptDisplay.Text = taxCode.ReceiptDisplay;

            lblExample.Text = lblExample.Text + " 0" + (System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToCharArray())[0] + "01"; 
        }

        public EditTaxCodeDialog()
        {
            InitializeComponent();
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
            taxCode.Text = tbDescription.Text;
            taxCode.TaxRoundOff = (decimal)ntbRoundoff.Value;
            taxCode.TaxRoundOffType = (TaxCode.RoundoffTypeEnum)cmbType.SelectedIndex;
            taxCode.ReceiptDisplay = tbReceiptDisplay.Text;

            Providers.TaxCodeData.Save(PluginEntry.DataModel, taxCode);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text != taxCode.Text ||
                cmbType.SelectedIndex != (int)taxCode.TaxRoundOffType ||
                ntbRoundoff.Value != (double)taxCode.TaxRoundOff ||
                tbReceiptDisplay.Text != taxCode.ReceiptDisplay)
                && ntbRoundoff.Value != 0 && tbDescription.Text != "";
        }

       
    }
}
