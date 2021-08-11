using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class ReadDesignDialog : DialogBase
    {
        private bool allowAutoCheck;

        public ReadDesignDialog()
        {
            InitializeComponent();
        }


        public DialogResult ShowDialog(IWin32Window owner, bool allowUpdate, string existingDescription)
        {
            allowAutoCheck = false;

            rbUpdateExisting.Checked = allowUpdate;
            rbUpdateExisting.Enabled = allowUpdate;
            
            rbCreateNew.Checked = !allowUpdate;

            string existingText = string.Empty;
            if (!string.IsNullOrEmpty(existingDescription))
            {
                rbUpdateExisting.Text = string.Format(Properties.Resources.ReadDesignUpdateExistingNamedMsg, existingText);
            }
            else
            {
                rbUpdateExisting.Text = Properties.Resources.ReadDesignUpdateExistingMsg;
            }

            tbDescription.Clear();

            allowAutoCheck = true;
            return ShowDialog(owner);
        }


        public bool ReadTablesAndFields
        {
            get { return rbTablesAndFields.Checked; }
            set { rbTablesAndFields.Checked = value; }
        }

        public bool UpdateExistingDatabaseDesign
        {
            get { return rbUpdateExisting.Checked; }
        }

        public string NewDescription
        {
            get { return tbDescription.Text; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbCreateNew.Checked && tbDescription.TextLength == 0)
            {
                errorProvider.SetError(tbDescription, Properties.Resources.ReadDesignDescriptionMissingMsg);
                tbDescription.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            AutoCheck(rbCreateNew);
        }

        private void AutoCheck(RadioButton radioButton)
        {
            if (allowAutoCheck && !radioButton.Checked)
                radioButton.Checked = true;
        }

        private void tbDescription_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.SetError(tbDescription, string.Empty);
        }

    }
}

