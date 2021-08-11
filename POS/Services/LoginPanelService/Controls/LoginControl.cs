using LSOne.Controls;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Peripherals;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.LoginPanel.Controls
{
    internal partial class LoginControl : UserControl
    {
        private LoginResult errorCode;

        public event EventHandler OnLogin;
        public event EventHandler OnTokenLogin;
        public event EventHandler PasswordGotFocus;
        public event EventHandler PasswordLostFocus;
        public event EventHandler UserGotFocus;
        public event EventHandler UserLostFocus;

        private Timer clearTimer;
        private Timer hardwarePoll;

        private LicenseErrorDisplayType errorDisplayType;
        private bool tokenLogin;
        private bool isUserFocused;
        private bool isPasswordFocused;
        private bool scannerEnabled;

        public LoginControl()
        {
            errorCode = 0;
            tokenLogin = false;
            scannerEnabled = false;

            InitializeComponent();

            if (!DesignMode)
            {
                hardwarePoll = new Timer();
                hardwarePoll.Interval = 500;
                hardwarePoll.Tick += PollHardware;
                hardwarePoll.Start();
                pnlError.Visible = false;

                tbLogin.DrawBorder = false;
                tbPassword.DrawBorder = false;
                btnSwitchLoginMethod.DrawBorder = false;
            }
        }

        [Browsable(false)]
        public DateTime? ExpiryDate { get; set; }

        [Browsable(false)]
        public LicenseErrorDisplayType ErrorDisplayType
        {
            get { return errorDisplayType; }
            set
            {
                errorDisplayType = value;
                if (errorDisplayType != LicenseErrorDisplayType.NoError)
                {
                    ILicenseService service = (ILicenseService) DLLEntry.DataModel.Service(ServiceType.LicenseService);

                    lblError.Text = service.ErrorMessage;
                    pnlError.Visible = true;

                    if (service.ErrorMessage != "" && (errorDisplayType == LicenseErrorDisplayType.LicenseExpiresIn || errorDisplayType == LicenseErrorDisplayType.Expired))
                    {
                        demoLinkLabel.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "";
                    pnlError.Visible = false;
                    demoLinkLabel.Visible = false;
                }

                CenterErrorMessage();
            }
        }

        [Browsable(false)]
        public ShadeTextBoxTouch PasswordControl
        {
            get
            {
                return tbPassword;
            }
        }

        [Browsable(false)]
        public MSRTextBoxTouch UserControl
        {
            get
            {
                return tbLogin;
            }
        }

        private void ParentFormOnVisibleChanged(object sender, EventArgs eventArgs)
        {
            if (ParentForm != null && ParentForm.Visible)
            {
                hardwarePoll.Start();
            }
            else
            {
                hardwarePoll.Stop();
            }
        }

        private void LoginControl_Load(object sender, EventArgs e)
        {
            if (DLLEntry.Settings != null && DLLEntry.Settings.HardwareProfile != null)
            {
                tbLogin.StartCharacter = DLLEntry.Settings.HardwareProfile.StartTrack1;
                tbLogin.Seperator = DLLEntry.Settings.HardwareProfile.Separator1;
                tbLogin.EndCharacter = DLLEntry.Settings.HardwareProfile.EndTrack1;
                tbLogin.TrackSeperation = TrackSeperation.Before;
            }

            if (ParentForm != null)
            {
                ParentForm.VisibleChanged += ParentFormOnVisibleChanged;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen normalPen = new Pen(ColorPalette.POSTextColor, 2);
            Pen focusedPen = new Pen(ColorPalette.POSFocusedBorderColor, 2);

            e.Graphics.DrawLine(isUserFocused ? focusedPen : normalPen, new Point(tbLogin.Location.X, tbLogin.Bottom + 1), new Point(btnSwitchLoginMethod.Right, btnSwitchLoginMethod.Bottom + 1));
            if(tbPassword.Visible)
            {
                e.Graphics.DrawLine(isPasswordFocused ? focusedPen : normalPen, new Point(tbPassword.Location.X, tbPassword.Bottom + 1), new Point(tbPassword.Right, tbPassword.Bottom + 1));
            }

            normalPen.Dispose();
            focusedPen.Dispose();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ErrorCode = 0;

            if(tokenLogin)
            {
                if (OnTokenLogin != null)
                {
                    OnTokenLogin(this, EventArgs.Empty);
                }
            }
            else
            {
                if (OnLogin != null)
                {
                    OnLogin(this, EventArgs.Empty);
                }
            }
        }

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            if (tokenLogin)
            {
                btnLogin.Enabled = tbLogin.Text != "";
            }
            else
            {
                btnLogin.Enabled = tbLogin.Text != "" && tbPassword.Text != "";
            }
        }

        public string Login
        {
            get { return tbLogin.Text; }
            set { tbLogin.Text = value; }
        }

        public void FocusLogin()
        {
            tbLogin.Focus();
        }

        public string Password
        {
            get { return tbPassword.Text; }
        }

        public LoginResult ErrorCode
        {
            get { return errorCode; }
            set
            {
                errorCode = value;
                switch (value)
                {
                    case LoginResult.UserAuthenticationFailed:
                        tbPassword.Text = "";
                        tbPassword.Focus();
                        lblError.Text = Resources.AuthenticationFailed;
                        break;
                    case LoginResult.UserLockedOut:
                        lblError.Text = Resources.AccountDisabledLoginAttempts;
                        break;
                    case LoginResult.LoginDisabled:
                        lblError.Text = Resources.AccountDisabledError;
                        break;
                    case LoginResult.UnknownServerError:
                    case LoginResult.CouldNotConnectToDatabase:
                        lblError.Text = Resources.ConnectingToDatabaseError;
                        break;
                    case LoginResult.UserDoesNotMatchConnectionIntent:
                        lblError.Text = Resources.UserIsOnlyAllowedForServerConnections;
                        break;
                    case LoginResult.TokenNotFound:
                    case LoginResult.TokenIsUser:
                        tbLogin.Text = "";
                        tbLogin.Focus();
                        lblError.Text = Resources.TokenNotFound;
                        break;
                }

                if (errorCode != 0)
                {
                    pnlError.Visible = true;
                }
                else
                {
                    pnlError.Visible = false;
                }

                demoLinkLabel.Visible = false;

                CenterErrorMessage();
                this.Invalidate();
            }
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return && btnLogin.Enabled)
            {
                btnLogin_Click(this, EventArgs.Empty);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void tbLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                tbPassword.Focus();

                if (tbLogin.Text.Length > 0 && tokenLogin)
                {
                    if (OnTokenLogin != null)
                    {
                        tbLogin.Text = StringExtensions.TrackBeforeSeparator(tbLogin.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                        OnTokenLogin(this, EventArgs.Empty);
                    }
                }
            }
            else if (e.KeyCode == Keys.LineFeed && tokenLogin)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        public void DelayedClear()
        {
            // This method is needed for when barcode readers keep putting trash in the field milliseconds after the scan
            clearTimer = new Timer();
            clearTimer.Interval = 200;

            clearTimer.Tick += new EventHandler(clearTimer_Tick);

            clearTimer.Start();
        }

        private void clearTimer_Tick(object sender, EventArgs e)
        {
            clearTimer.Stop();

            clearTimer.Tick -= clearTimer_Tick;

            clearTimer = null;

            tbLogin.Text = "";
        }

        public void ClearUser()
        {
            tbLogin.Text = "";
        }

        public void ClearToken()
        {
            tbLogin.Text = "";
        }

        public void ClearPassword()
        {
            tbPassword.Text = "";
        }

        private void tbLogin_Leave(object sender, EventArgs e)
        {
            if (UserLostFocus != null)
            {
                UserLostFocus(this, EventArgs.Empty);
            }

            isUserFocused = false;
            Invalidate();
        }

        private void tbLogin_Enter(object sender, EventArgs e)
        {
            if (UserGotFocus != null)
            {
                    UserGotFocus(this, EventArgs.Empty);
            }

            isUserFocused = true;
            Invalidate();
        }

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            if (PasswordLostFocus != null)
            {
                PasswordLostFocus(this, EventArgs.Empty);
            }

            isPasswordFocused = false;
            Invalidate();
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            if (PasswordGotFocus != null)
            {
                PasswordGotFocus(this, EventArgs.Empty);
            }

            isPasswordFocused = true;
            Invalidate();
        }

        public void PressLogin()
        {
            if (btnLogin.Enabled)
            {
                btnLogin_Click(this, EventArgs.Empty);
            }
        }

        public void PollHardware(object sender, EventArgs args)
        {
            try
            {
                hardwarePoll.Enabled = false;
                if (DLLEntry.Settings == null || DLLEntry.Settings.HardwareProfile == null)
                {
                    return;
                }
                if (DLLEntry.Settings.HardwareProfile.DallasKeyConnected && DallasKey.NewMessages)
                {
                    ProccessDallasKey(DLLEntry.Settings);
                    DallasKey.NewMessages = false;
                }
            }
            finally
            {
                hardwarePoll.Enabled = true;
            }
        }

        private void ProccessDallasKey(ISettings settings)
        {
            string message = DallasKey.Messages.Count > 0 ? DallasKey.Messages.Last() : null;
            DallasKey.Messages.Clear();
            if (message != null && message != settings.HardwareProfile.DallasKeyRemovedMessage)
            {
                tbLogin.Text = message;
                if (tokenLogin && OnTokenLogin != null)
                {
                    OnTokenLogin(this, EventArgs.Empty);
                }
            }
        }

        private void tbLogin_VisibleChanged(object sender, EventArgs e)
        {
            if (TokenLogin)
            {
                tbLogin.Focus();
            }
            else if (tbLogin.Text.Length > 0)
            {
                tbPassword.Focus();
            }
            else
            {
                tbLogin.Focus();
            }    
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        internal bool TokenLogin
        {
            get
            {
                return tokenLogin;
            }
            set
            {
                if (value != tokenLogin)
                {
                    lblPassword.Visible = !value;
                    tbPassword.Visible = !value;

                    tokenLogin = value;

                    if(value)
                    {
                        // Note we cannot use UseSystemPasswordChar here since this is a multiline box which does not work at all with UseSystemPasswordChar
                        // Its a why is the sky blue thing, it just is.
                        tbLogin.PasswordChar = '\0';
                        tbLogin.UseSystemPasswordChar = true;
                        lblLogin.Text = Resources.Token;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Password_48;
                        btnLogin.Location = new Point(btnLogin.Location.X, tbLogin.Location.Y + 50);
                        pnlError.Location = new Point(pnlError.Location.X, btnLogin.Location.Y + 75);
                    }
                    else
                    {
                        tbLogin.UseSystemPasswordChar = false;
                        lblLogin.Text = Resources.User;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Login_48;
                        btnLogin.Location = new Point(btnLogin.Location.X, tbPassword.Location.Y + 50);
                        pnlError.Location = new Point(pnlError.Location.X, btnLogin.Location.Y + 75);
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

            tbLogin.Text = "";
            tbLogin.Focus();
            tbLogin_TextChanged(null, EventArgs.Empty);
        }

        private void demoLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://info.lsretail.com/buy-ls-one");
        }

        private void CenterErrorMessage()
        {
            if(lblError.Text != "")
            {
                //Here we center the label and ajust the location of the warning sign and "buy licence" link
                int offset = (pbError.Width - 16 - (demoLinkLabel.Visible ? demoLinkLabel.Width - 3 : 0)) / 2;
                
                lblError.Left = (pnlError.Width - lblError.Width) / 2 + offset;
                pbError.Left = lblError.Left - pbError.Width + 8;
                demoLinkLabel.Left = lblError.Right - 3;
            }
        }

        public void EnableBarcodeScanner()
        {
            if (tokenLogin && !scannerEnabled)
            {
                Scanner.ScannerMessageEvent += Scanner_ScannerMessageEvent;
                Scanner.ReEnableForScan();
                scannerEnabled = true;
            }
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
                tbLogin.Text = scanInfo.ScanDataLabel;
                PressLogin();
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void LoginControl_Enter(object sender, EventArgs e)
        {
            EnableBarcodeScanner();
        }

        private void LoginControl_Leave(object sender, EventArgs e)
        {
            DisableBarcodeScanner();
        }
    }
}
