using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.POSUser.Dialogs
{
    public partial class EditEmailDialog : DialogBase
    {
        public string Email;

        public EditEmailDialog(string email)
        {
            InitializeComponent();
            Email = email;
            txtEmail.Text = email;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Email = txtEmail.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = txtEmail.Text.Trim() != Email && (string.IsNullOrEmpty(txtEmail.Text.Trim()) || txtEmail.Text.Trim().IsEmail());

            if(!string.IsNullOrEmpty(txtEmail.Text.Trim()) && !txtEmail.Text.Trim().IsEmail())
            {
                errorProvider1.SetError(txtEmail, Properties.Resources.EmailNotValid);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void txtEmail_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return && btnOK.Enabled)
            {
                btnOK_Click(this, EventArgs.Empty);
            }
        }
    }
}
