using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Dialog for adding text answer to a suspended transaction
    /// </summary>
    public partial class AddressDialog : TouchBaseForm
    {
        private SuspensionTypeAdditionalInfo suspensionTypeInfo;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddressDialog(IConnectionManager entry, SuspensionTypeAdditionalInfo suspensionTypeInfo)
        {
            InitializeComponent();

            this.suspensionTypeInfo = suspensionTypeInfo;
            touchDialogBanner1.BannerText = suspensionTypeInfo.Text;

            if(suspensionTypeInfo.Required)
            {
                btnCancel.Visible = false;
                btnClear.Left = btnOK.Left;
                btnOK.Left = btnCancel.Left;
            }

            btnOK.Enabled = false;
            addressControlTouch.DataModel = entry;
        }

        /// <summary>
        /// Text answer
        /// </summary>
        public SuspendedTransactionAnswer GetAnswer()
        {
            return new SuspendedTransactionAnswer
            {
                RecordID = Guid.NewGuid(),
                Prompt = suspensionTypeInfo.Text,
                Address = addressControlTouch.AddressRecord,
                AddressFormat = addressControlTouch.AddressFormat
            };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            addressControlTouch.Clear();
        }

        private void addressControlTouch_ValueChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !addressControlTouch.AddressRecord.IsEmpty;
        }
    }
}
