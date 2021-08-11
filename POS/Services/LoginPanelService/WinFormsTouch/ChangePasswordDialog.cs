using System;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.Cryptography;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    public partial class ChangePasswordDialog : TouchBaseForm
    {
        SecureString oldPassword;
        bool isTokenLogin;
        
        public ChangePasswordDialog(SecureString oldPassword, bool isTokenLogin = false)
        {
            this.oldPassword = oldPassword;
            this.isTokenLogin = isTokenLogin;

            InitializeComponent();

            if (isTokenLogin)
            {
                lblOldPassword.Visible = true;
                tbOldPassword.Visible = true;
                tbOldPassword.Select();
            }
            else
            {
                Height -= 87;
            }
        }

        private ChangePasswordDialog()
        {
            InitializeComponent();
        }

        private void tbNewPassword_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbNewPassword;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbNewPassword_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void tbConfirmPassword_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbConfirmPassword;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbConfirmPassword_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if(ValidForChange() != btnChange.Enabled)
            {
                btnChange.Enabled = !btnChange.Enabled;
            }
        }

        private bool ValidForChange()
        {
            return (tbNewPassword.Text.Length >= 4 && tbConfirmPassword.Text == tbNewPassword.Text) 
                && !DLLEntry.DataModel.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbNewPassword.Text));
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (!ValidForChange())
            {
                return;
            }
            SecureString oldPass = isTokenLogin ? SecureStringHelper.FromString(tbOldPassword.Text) : oldPassword;
            bool passwordChanged = DLLEntry.DataModel.ChangePassword(oldPass, SecureStringHelper.FromString(tbNewPassword.Text));
            if (!passwordChanged)
            {
                if (isTokenLogin)
                {
                    bool isLockedOut;
                    bool oldPassIsCorrect = DLLEntry.DataModel.VerifyCredentials(DLLEntry.DataModel.CurrentUser.UserName, oldPass, out isLockedOut);
                    if (!oldPassIsCorrect)
                    {
                        DialogResult dialogResult = ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(
                            Properties.Resources.IncorrectOldPasswordMessage, Properties.Resources.IncorrectOldPassword,
                            MessageBoxButtons.OK, MessageDialogType.ErrorWarning);

                        if (dialogResult == DialogResult.OK)
                        {
                            tbOldPassword.Clear();
                            tbOldPassword.Select();
                        }  
                    }
                }
                //TODO put up some messagebox
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            args.LayoutName = DLLEntry.StoreKeyboardLayoutName;
            args.CultureName = DLLEntry.StoreKeyboardCode;
        }

        private void tbOldPassword_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbOldPassword;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbOldPassword_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void ChangePasswordDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((Keys)e.KeyChar).Equals(Keys.Enter))
            {
                btnChange_Click(sender, EventArgs.Empty);
            }
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            if (btnChange.Enabled)
            {

                btnChange_Click(sender, e);
            }
        }
    }
}
