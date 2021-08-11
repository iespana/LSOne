using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.Settings;
using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    public partial class LockTerminalDialog : TouchBaseForm
    {
        private bool init;
        private bool tokenLogin;

        public LockTerminalDialog()
        {
            InitializeComponent();
        }

        public LockTerminalDialog(string userName)
        {
            init = false;
            InitializeComponent();
            tbUser.Text = userName;

            this.TokenLogin = SettingsContainer<ApplicationSettings>.Instance.TokenLogin;
            init = true;

            touchKeyboard.KeyboardMode = SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad ? TouchKeyboard.KeyboardModeEnum.Numeric : TouchKeyboard.KeyboardModeEnum.Alphabet;
        }

        internal bool TokenLogin
        {
            get
            {
                return tokenLogin;
            }
            set
            {
                if (value != tokenLogin || init == false)
                {
                    tokenLogin = value;

                    if (value)
                    {
                        // Note we cannot use UseSystemPasswordChar here since this is a multiline box which does not work at all with UseSystemPasswordChar
                        // Its a why is the sky blue thing, it just is.
                        tbPassword.UseSystemPasswordChar = false;
                        tbPassword.PasswordChar = '•';
                        lblPassword.Text = Properties.Resources.Token;
                        btnSwitchLoginMethod.BackgroundImage = Properties.Resources.Password_48;
                    }
                    else
                    {
                        tbPassword.PasswordChar = '\0';
                        tbPassword.UseSystemPasswordChar = true;
                        lblPassword.Text = Properties.Resources.Password;
                        btnSwitchLoginMethod.BackgroundImage = Properties.Resources.Login_48;
                    }
                }
            }
        }

        private void tbUser_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbUser;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbUser_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbPassword;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbPassword.Text.Length > 0 && tbUser.Text.Length > 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DLLEntry.Settings != null && DLLEntry.Settings.HardwareProfile != null)
            {
                tbPassword.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbPassword.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbPassword.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbPassword.TrackSeperation = TrackSeperation.Before;
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            ISettings settings = ((ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));
            if (settings == null)
            {
                return;
            }
            if (settings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = settings.UserProfile.KeyboardCode;
                args.LayoutName = settings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = settings.Store.KeyboardCode;
                args.LayoutName = settings.Store.KeyboardLayoutName;
            }
        }


        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                btnOK_Click(this, EventArgs.Empty);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool isLockedOut;
            bool tokenIsUser = false;

            if (tokenLogin)
            {
                if (DLLEntry.DataModel.VerifyCredentials(tbUser.Text, AuthenticationToken.CreateHash(tbPassword.Text), out isLockedOut, out tokenIsUser))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    if (isLockedOut)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.UserLockedOut, MessageBoxButtons.OK, MessageDialogType.Attention);
                    }
                    else
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.IncorrectPassword, MessageBoxButtons.OK, MessageDialogType.Attention);
                    }
                }
            }
            else
            {
                if (DLLEntry.DataModel.VerifyCredentials(tbUser.Text, SecureStringHelper.FromString(tbPassword.Text), out isLockedOut))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    if (isLockedOut)
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.UserLockedOut, MessageBoxButtons.OK, MessageDialogType.Attention);
                    }
                    else
                    {
                        ((IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService)).ShowMessage(Properties.Resources.IncorrectPassword, MessageBoxButtons.OK, MessageDialogType.Attention);
                    }
                }
            }
        }

        private void btnSwitchLoginMethod_Click(object sender, EventArgs e)
        {
            TokenLogin = !TokenLogin;

            tbPassword.Text = "";
            tbPassword.Focus();
        }

        public bool LoginWithNumPad
        {
            get
            {
                return touchKeyboard.KeyboardMode == TouchKeyboard.KeyboardModeEnum.Numeric;
            }
        }

    }
}
