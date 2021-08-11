using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Transactions;
using System;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    /// <summary>
    /// Dialog for adding text answer to a suspended transaction
    /// </summary>
    public partial class TextDialog : TouchBaseForm
    {
        private SuspensionTypeAdditionalInfo suspensionTypeInfo;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextDialog(SuspensionTypeAdditionalInfo suspensionTypeInfo)
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
                Text = tbInput.Text
            };
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbInput.Text != "";
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
            tbInput.Text = "";
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            tbInput.Focus();
            SendKeys.Send("{ENTER}");
        }
    }
}
