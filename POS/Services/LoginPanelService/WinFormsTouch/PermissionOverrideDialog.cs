using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.Settings;
using System;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.Peripherals;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.LoginPanel.WinFormsTouch
{
    public partial class PermissionOverrideDialog : TouchBaseForm
    {
        private bool tokenLogin;
        private readonly PermissionInfo permissionInfo;
        private bool init;
        private bool scannerEnabled;

        public SecureString Password
        {
            get
            {
                return SecureStringHelper.FromString(tbPassword.Text);
            }
        }

        public string Login
        {
            get
            {
                return tbUser.Text;
            }
            set
            {
                tbUser.Text = value;
            }
        }

        public PermissionOverrideDialog()
        {
            InitializeComponent();
        }

        public PermissionOverrideDialog(PermissionInfo info)
            : this()
        {
            permissionInfo = info;
            btnCancel.Visible = !info.LockPermissionOverride;

            init = false;
            this.TokenLogin = SettingsContainer<ApplicationSettings>.Instance.TokenLoginForManagerOverride;
            init = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IDialogService dialogService = (IDialogService)DLLEntry.DataModel.Service(ServiceType.DialogService);
            bool userIsValid;
            bool userLockedOut;
            bool isTokenUser;

            if (tokenLogin)
            {
                userIsValid = DLLEntry.DataModel.VerifyCredentials("", AuthenticationToken.CreateHash(Login), out userLockedOut, out isTokenUser);
            }
            else
            {
                userIsValid = DLLEntry.DataModel.VerifyCredentials(Login, Password, out userLockedOut);
            }

            if (userLockedOut)
            {
                dialogService.ShowMessage(Properties.Resources.AccountDisabled, Properties.Resources.AccountDisabledError, MessageBoxButtons.OK, MessageDialogType.Generic);
                tbUser.Focus();
                return;
            }

            if (userIsValid)
            {
                if (tokenLogin)
                {
                    if (DLLEntry.DataModel.HasPermission(permissionInfo.GUID, AuthenticationToken.CreateHash(tbUser.Text), out isTokenUser))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }
                }
                else
                {
                    if ((DLLEntry.DataModel.HasPermission(permissionInfo.GUID, Login, Password)))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }
                }
            }

            dialogService.ShowMessage(Properties.Resources.WrongManagerLogin, Properties.Resources.AuthenticationFailed, MessageBoxButtons.OK, MessageDialogType.Generic);
            tbUser.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
            btnOK.Enabled = (TokenLogin || tbPassword.Text.Length > 0) && tbUser.Text.Length > 0;
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            ISettings settings = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));
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

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && btnOK.Enabled)
            {
                btnOK_Click(sender, e);
            }
        }

        private void tbUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && btnOK.Enabled)
            {
                btnOK_Click(sender, e);
            }
        }

        private void tbUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if(!tokenLogin)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    tbPassword.Focus();
                }
                else
                {
                    tbUser.Text = StringExtensions.TrackBeforeSeparator(tbUser.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                    btnOK_Click(sender, e);
                }
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
                if (value != tokenLogin || init == false)
                {
                    lblPassword.Visible = !value;
                    tbPassword.Visible = !value;

                    tokenLogin = value;

                    if (value)
                    {
                        // Note we cannot use UseSystemPasswordChar here since this is a multiline box which does not work at all with UseSystemPasswordChar
                        // Its a why is the sky blue thing, it just is.
                        tbUser.PasswordChar = '•';
                        lblUser.Text = Properties.Resources.Token;
                        btnSwitchLoginMethod.Image = Properties.Resources.Password_48;
                    }
                    else
                    {
                        tbUser.PasswordChar = '\0';
                        lblUser.Text = Properties.Resources.User;
                        btnSwitchLoginMethod.Image = Properties.Resources.Login_48;
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

        private void PermissionOverrideDialog_Load(object sender, EventArgs e)
        {
            if(TokenLogin)
            {
                EnableBarcodeScanner();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if(TokenLogin)
            {
                DisableBarcodeScanner();
            }

            base.OnClosed(e);
        }
    }
}
