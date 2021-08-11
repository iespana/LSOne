using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.PaymentLimitations.Dialogs
{
    public partial class CopyLimitationsDialog : DialogBase
    {
        public RecordIdentifier LimitationCode;
        private RecordIdentifier paymentMethodID;

        public CopyLimitationsDialog(RecordIdentifier paymentMethodID)
        {
            InitializeComponent();
            this.paymentMethodID = paymentMethodID;

            btnOK.Enabled = false;
        }

        private void CheckEnabled()
        {
            errorProvider1.Clear();

            if (cmbCopyFrom.SelectedData == null || cmbCopyFrom.SelectedDataID == RecordIdentifier.Empty)
            {
                errorProvider1.SetError(cmbCopyFrom, Properties.Resources.LimitationCodeNeedsToBeEntered);
                btnOK.Enabled = false;
                return;
            }

            if (cmbAddTo.SelectedData == null || cmbAddTo.SelectedDataID == RecordIdentifier.Empty)
            {
                errorProvider1.SetError(cmbAddTo, Properties.Resources.LimitationCodeNeedsToBeEntered);
                btnOK.Enabled = false;
                return;
            }

            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<PaymentMethodLimitation> limitationsToCopy = Providers.PaymentLimitationsData.GetListForRestrictionCode(PluginEntry.DataModel, cmbCopyFrom.SelectedDataID, RecordIdentifier.Empty);
            List<PaymentMethodLimitation> addtoLimitations = Providers.PaymentLimitationsData.GetListForRestrictionCode(PluginEntry.DataModel, cmbAddTo.SelectedDataID, RecordIdentifier.Empty);

            PaymentMethodLimitation addTo = addtoLimitations.Any() ? addtoLimitations[0] : new PaymentMethodLimitation();
            
            foreach (PaymentMethodLimitation limitation in limitationsToCopy)
            {
                PaymentMethodLimitation exists = addtoLimitations.FirstOrDefault(f => f.TenderID == paymentMethodID && f.Type == limitation.Type && f.RelationMasterID == limitation.RelationMasterID);
                if (exists == null)
                {
                    PaymentMethodLimitation newLimitation = PluginOperations.CreateNewPaymentMethodLimitation(RecordIdentifier.Empty, addTo.LimitationMasterID, addTo.RestrictionCode, paymentMethodID, limitation.Include, limitation.Type, limitation.RelationMasterID, limitation.TaxExempt);
                    Providers.PaymentLimitationsData.Save(PluginEntry.DataModel, newLimitation);
                }
                else if (exists.Include != limitation.Include)
                {
                    exists.Include = limitation.Include;
                    Providers.PaymentLimitationsData.Save(PluginEntry.DataModel, exists);
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbLimitationCode_RequestData(object sender, EventArgs e)
        {
            cmbCopyFrom.SetData(Providers.PaymentLimitationsData.GetRestrictionCodeListForTender(PluginEntry.DataModel, RecordIdentifier.Empty).OrderBy(o => o.Text), null);
        }

        private void dualDataComboBox1_RequestData(object sender, EventArgs e)
        {
            cmbAddTo.SetData(Providers.PaymentLimitationsData.GetRestrictionCodeListForTender(PluginEntry.DataModel, paymentMethodID).OrderBy(o => o.Text), null);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbAddTo_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}
