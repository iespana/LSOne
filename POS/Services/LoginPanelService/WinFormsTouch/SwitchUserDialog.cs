using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Peripherals;
using LSOne.POS.Core;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.Properties;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Settings;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    public partial class SwitchUserDialog : TouchBaseForm
    {
        private bool tokenLogin;
        private bool init;
        private bool scannerEnabled;

        public SwitchUserDialog()
        {
            InitializeComponent();
        }

        public SwitchUserDialog(string userName)
            : this()
        {
            init = false;

            this.TokenLogin = SettingsContainer<ApplicationSettings>.Instance.TokenLogin;

            if (!tokenLogin)
            {
                tbUser.Text = userName;
                tbPassword.Select();
            }

            touchKeyboard.KeyboardMode = SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad ? LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric : LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Alphabet;

            init = true;
        }

        public RecordIdentifier StaffID { get { return DLLEntry.DataModel.CurrentUser.StaffID; } }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DLLEntry.Settings != null && DLLEntry.Settings.HardwareProfile != null)
            {
                tbUser.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbUser.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbUser.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbUser.TrackSeperation = TrackSeperation.Before;
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


        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if(tokenLogin)
                {
                    tbUser.Text = StringExtensions.TrackBeforeSeparator(tbUser.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                    btnOK_Click(sender, e);
                }
                else if (((Control)sender).Parent == tbPassword)
                {
                    btnOK_Click(this, EventArgs.Empty);
                }
                else if(((Control)sender).Parent == tbUser)
                {
                    tbPassword.Focus();
                }
            }     
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            LoginResult result;

            if (tokenLogin)
            {
                result = DLLEntry.DataModel.TokenSwitchUser(AuthenticationToken.CreateHash(tbUser.Text));
            }
            else
            {
                result = DLLEntry.DataModel.SwitchUser(tbUser.Text, SecureStringHelper.FromString(tbPassword.Text));
            }

            resolveResults(result);
        }


        private void resolveResults(LoginResult result)
        {
            switch (result)
            {
                case LoginResult.CouldNotConnectToDatabase:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.ConnectingToDatabaseError;
                    break;
                case LoginResult.Success:
                    if (DLLEntry.DataModel.CurrentUser.ForcePasswordChange)
                    {
                        using (ChangePasswordDialog dlg = new ChangePasswordDialog(SecureStringHelper.FromString(tbPassword.Text)))
                        {
                            if (dlg.ShowDialog(this) == DialogResult.Cancel)
                            {
                                DLLEntry.DataModel.LogOff();
                                return;
                            }
                        }
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case LoginResult.UnknownServerError:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.ConnectingToDatabaseError;
                    break;
                case LoginResult.UserAuthenticationFailed:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.AuthenticationFailed;
                    tbPassword.Text = "";
                    tbPassword.Select();
                    break;
                case LoginResult.UserDoesNotMatchConnectionIntent:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.UserIsOnlyAllowedForServerConnections;
                    break;
                case LoginResult.UserLockedOut:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.AccountDisabledLoginAttempts;
                    break;
                case LoginResult.LoginDisabled:
                    tepIncorrectLogin.Visible = true;
                    tepIncorrectLogin.ErrorText = Resources.AccountDisabledError;
                    break;

            }
        }

        internal bool TokenLogin
        {
            get
            {
                return tokenLogin;
            }
            set
            {
                if (value != tokenLogin ||  init == false)
                {
                    lblPassword.Visible = !value;
                    tbPassword.Visible = !value;

                    tokenLogin = value;

                    if (value)
                    {
                        // Note we cannot use UseSystemPasswordChar here since this is a multiline box which does not work at all with UseSystemPasswordChar
                        // Its a why is the sky blue thing, it just is.
                        tbUser.PasswordChar = '•';
                        lblUser.Text = Resources.Token;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Password_48;
                    }
                    else
                    {
                        tbUser.PasswordChar = '\0';
                        lblUser.Text = Resources.User;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Login_48;
                    }
                }
            }
        }

        private void btnSwitchLoginMethod_Click(object sender, EventArgs e)
        {
            TokenLogin = !TokenLogin;

            if (TokenLogin)
            {
                EnableBarcodeScanner();
            }
            else
            {
                DisableBarcodeScanner();
            }

            tbUser.Text = "";
            tbUser.Focus();
        }

        public bool LoginWithNumPad
        {
            get
            {
                return touchKeyboard.KeyboardMode == TouchKeyboard.KeyboardModeEnum.Numeric;
            }
        }

        private void EnableBarcodeScanner()
        {
            if (!scannerEnabled)
            {
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
            }
            scannerEnabled = true;
        }

        public void DisableBarcodeScanner()
        {
            if (scannerEnabled)
            {
                Scanner.ScannerMessageEvent -= Scanner_ScannerMessageEvent;
                Scanner.DisableForScan();
                scannerEnabled = false;
            }
        }

        private void Scanner_ScannerMessageEvent(ScanInfo scanInfo)
        {
            try
            {
                Scanner.DisableForScan();
                tbUser.Text = scanInfo.ScanDataLabel;
                btnOK_Click(this, EventArgs.Empty);
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void SwitchUserDialog_Load(object sender, EventArgs e)
        {
            if (TokenLogin)
            {
                EnableBarcodeScanner();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (TokenLogin)
            {
                DisableBarcodeScanner();
            }

            base.OnClosed(e);
        }
    }
}
