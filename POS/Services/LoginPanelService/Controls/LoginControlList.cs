using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;

namespace LSOne.Services.LoginPanel.Controls
{
    internal partial class LoginControlList : UserControl
    {
        private LoginResult errorCode;
        private LicenseErrorDisplayType errorDisplayType;

        public event EventHandler OnLogin;
        public event EventHandler OnTokenLogin;
        public event EventHandler PasswordGotFocus;
        public event EventHandler PasswordLostFocus;
        public event EventHandler RequestNewUserList;

        private Timer hardwarePoll;
        private bool tokenLogin;
        private string login;
        private bool isPasswordFocused;
        private bool scannerEnabled;

        public LoginControlList()
        {
            errorCode = 0;
            tokenLogin = false;
            scannerEnabled = false;

            InitializeComponent();

            if(!DesignMode)
            {
                hardwarePoll = new Timer { Interval = 500 };
                hardwarePoll.Tick += PollHardware;
                hardwarePoll.Start();

                pnlError.Visible = false;
                lvUsers.HeaderHeight = 1;
                lvUsers.DefaultRowHeight = 30;
                tbPassword.DrawBorder = false;
                btnSwitchLoginMethod.DrawBorder = false;
            }
        }

        [Browsable(false)]
        public LicenseErrorDisplayType ErrorDisplayType
        {
            get { return errorDisplayType; }
            set
            {
                errorDisplayType = value;
                if (errorDisplayType != LicenseErrorDisplayType.NoError)
                {
                    ILicenseService service = (ILicenseService)DLLEntry.DataModel.Service(ServiceType.LicenseService);

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
        public DateTime? ExpiryDate { get; set; }

        private void ParentFormOnVisibleChanged(object sender, EventArgs eventArgs)
        {
            if (ParentForm.Visible)
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
            if (ParentForm != null)
            {
                ParentForm.VisibleChanged += ParentFormOnVisibleChanged;
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

        public void SetUsers(List<User> loginUsers, NameFormat nameFormat)
        {
            lvUsers.ClearRows();
            Row row;

            INameFormatter formatter = (nameFormat == NameFormat.FirstNameFirst ? (INameFormatter)new FirstNameFirstFormatter() : (INameFormatter)new LastNameFirstFormatter());

            foreach (User user in loginUsers)
            {
                row = new Row();
                row.AddText(user.Login);
                row.AddText(formatter.Format(user.Name));

                lvUsers.AddRow(row);
            }

            lvUsers.AutoSizeColumns();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Pen normalPen = new Pen(ColorPalette.POSTextColor, 2);
            Pen focusedPen = new Pen(ColorPalette.POSFocusedBorderColor, 2);

            e.Graphics.DrawLine(isPasswordFocused ? focusedPen : normalPen, new Point(tbPassword.Location.X, tbPassword.Bottom + 1), new Point(btnSwitchLoginMethod.Right, btnSwitchLoginMethod.Bottom + 1));

            normalPen.Dispose();
            focusedPen.Dispose();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            ErrorCode = LoginResult.Success;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ErrorCode = 0;

            if (tokenLogin)
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


        public string Login
        {
            get
            {
                if (tokenLogin)
                {
                    return tbPassword.Text;
                }
                else
                {
                    return lvUsers.Selection.Count > 0 ? lvUsers.Row(lvUsers.Selection.FirstSelectedRow)[0].Text : "";
                }
            }
            set 
            {
                if (tokenLogin)
                {
                    login = value;
                    return;
                }
                for (int i = 0; i < lvUsers.RowCount; i++)
                {
                    if (lvUsers.Row(lvUsers.Selection.FirstSelectedRow)[(uint)i].Text == value)
                    {
                        lvUsers.Selection.Set(i);
                        break;
                    }
                }
            }
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
                        tbPassword.Text = "";
                        tbPassword.Focus();
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
            if (tokenLogin)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    tbPassword.Focus();

                    if (tbPassword.Text.Length > 0)
                    {
                        if (OnTokenLogin != null)
                        {
                            tbPassword.Text = StringExtensions.TrackBeforeSeparator(tbPassword.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                            OnTokenLogin(this, EventArgs.Empty);
                        }
                    }
                }
                else if (e.KeyCode == Keys.LineFeed)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return && btnLogin.Enabled)
            {
                btnLogin_Click(this, EventArgs.Empty);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        public void ClearUser()
        {
            lvUsers.Selection.Clear();
        }

        public void ClearPassword()
        {
            tbPassword.Text = "";
        }

        private void lvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if(!TokenLogin)
            {
                bool enabled = (lvUsers.Selection.Count > 0);
                btnLogin.Enabled = enabled;
                if (enabled)
                {
                    BeginInvoke(new MethodInvoker(SetPasswordFocus));
                }
            }
        }

        private void SetPasswordFocus()
        {
            tbPassword.Focus();
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (tokenLogin)
            {
                btnLogin.Enabled = (tbPassword.Text.Length > 0);
            }
            else
            {
                btnLogin.Enabled = (lvUsers.Selection.Count > 0);
            }
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

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            if (PasswordLostFocus != null)
            {
                PasswordLostFocus(this, EventArgs.Empty);
            }

            isPasswordFocused = false;
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
                hardwarePoll.Enabled = true;
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
            if (message != null && message != "" && message != settings.HardwareProfile.DallasKeyRemovedMessage)
            {
                Login = message;
                DallasKey.Messages.Clear();
                if (tokenLogin && OnTokenLogin != null)
                {
                    OnTokenLogin(this, EventArgs.Empty);
                }
            }
        }

        private void lvUsers_RowDoubleClick(object sender, RowEventArgs args)
        {
            lvUsers_SelectionChanged(sender, EventArgs.Empty);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (RequestNewUserList != null)
            {
                RequestNewUserList(this, EventArgs.Empty);
            }
            lvUsers.Focus();
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
                    tokenLogin = value;

                    if (value)
                    {
                        lblPassword.Text = Resources.Token;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Password_48;
                        lvUsers.DefaultStyle.BackColor = ColorPalette.POSDisabledTextBox;
                        
                        lvUsers.Enabled = false;
                    }
                    else
                    {
                        lblPassword.Text = Resources.Password;
                        btnSwitchLoginMethod.BackgroundImage = Resources.Login_48;
                        lvUsers.DefaultStyle.BackColor = ColorPalette.POSBackgroundColor;
                        lvUsers.Enabled = true;
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

            tbPassword.Text = "";
            tbPassword.Focus();

            tbPassword_TextChanged(null, EventArgs.Empty);
        }

        private void demoLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://info.lsretail.com/buy-ls-one");
        }

        private void CenterErrorMessage()
        {
            if (lblError.Text != "")
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
                tbPassword.Text = scanInfo.ScanDataLabel;
                PressLogin();
            }
            finally
            {
                Scanner.ReEnableForScan();
            }
        }

        private void LoginControlList_Enter(object sender, EventArgs e)
        {
            EnableBarcodeScanner();
        }

        private void LoginControlList_Leave(object sender, EventArgs e)
        {
            DisableBarcodeScanner();
        }
    }
}
