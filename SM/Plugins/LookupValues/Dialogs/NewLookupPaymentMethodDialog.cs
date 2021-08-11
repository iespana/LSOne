using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class NewLookupPaymentMethodDialog : DialogBase
    {
        private RecordIdentifier paymentMethodID;

        public NewLookupPaymentMethodDialog()
        {
            paymentMethodID = RecordIdentifier.Empty;

            InitializeComponent();
        }

        public RecordIdentifier PaymentMethodID
        {
            get{return paymentMethodID;}
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            paymentMethodID = PluginOperations.SavePaymentMethod(tbDescription.Text, (PaymentMethodDefaultFunctionEnum)cmbDefaultFunction.SelectedIndex);

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
            btnOK.Enabled = (tbDescription.Text.Length > 0 && cmbDefaultFunction.SelectedIndex >= 0);
        }

        private void cmbDefaultFunction_SelectedIndexChanged(object sender, EventArgs e)
        {            
        }
    }
}