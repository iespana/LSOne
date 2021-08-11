using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.WinFormsTouch;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.Settings;
using LSOne.POS.Core;
using LSOne.Utilities.ColorPalette;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LSOne.Services.LoginPanel.Pages
{
    internal partial class LogOnListPage : UserControl, ILoginPanelPage
    {
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        private SQLServerLoginEntry loginEntry;
        public event LogonFormEventHandler OperationPerformed;
        private ISettings settings;
        private ILicenseService service;

        private LoginPanelService.RefreshList refreshList;

        public delegate LoginPanelService.LoginHandlerResults LoginResultHandler(ref LoginResult result, ILoginPanelPage senderPanel);
        private LoginResultHandler loginResultHandler;

        public LogOnListPage()
        {
            InitializeComponent();

            DoubleBuffered = true;

            touchKeyboard.KeyboardMode = SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad ? LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric : LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Alphabet;
        }        

        public LogOnListPage(List<User> loginUsers, NameFormat nameFormat, SQLServerLoginEntry loginEntry, ISettings settings,
            LoginPanelService.RefreshList refreshList, bool tokenLogin, LoginResultHandler loginResultHandler)
            : this()
        {
            service = (ILicenseService)DLLEntry.DataModel.Service(ServiceType.LicenseService);

            loginControl.TokenLogin = tokenLogin;
            loginControl.ExpiryDate = service.LicenseExpiryDate;
            loginControl.ErrorDisplayType = service.ErrorDisplayType;
            loginControl.RequestNewUserList += LoginControlOnRequestNewUserList;
            this.loginResultHandler = loginResultHandler;

            this.refreshList = refreshList;
            this.loginEntry = loginEntry;

            loginControl.SetUsers(loginUsers, nameFormat);

            this.settings = settings;
        }

        private void LoginControlOnRequestNewUserList(object sender, EventArgs eventArgs)
        {
            Tuple<List<User>, NameFormat> result = refreshList();
            loginControl.SetUsers(result.Item1, result.Item2);
        }

        public string Login
        {
            get
            {
                return loginControl.Login;
            }
            set
            {
                loginControl.Login = value;
            }
        }

        public string Password
        {
            get
            {
                return loginControl.Password;
            }
        }

        public bool TokenLogin
        {
            get
            {
                return loginControl.TokenLogin;
            }
        }

        public void ClearUser()
        {
            loginControl.ClearUser();
        }

        public void EnableBarcodeScanner()
        {
            loginControl.EnableBarcodeScanner();
        }

        public void DisableBarcodeScanner()
        {
            loginControl.DisableBarcodeScanner();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                loginControl.ErrorDisplayType = service.ErrorDisplayType;

                if (DLLEntry.Settings.NeedsEFTSetup || DLLEntry.Settings.HardwareProfileNeedsSetup)
                {
                    if (DLLEntry.Settings.NeedsEFTSetup && DLLEntry.Settings.HardwareProfileNeedsSetup)
                    {
                        lblInfo.Text = Properties.Resources.AtNextLogonHardwareAndEFT;
                    }
                    else if (DLLEntry.Settings.NeedsEFTSetup)
                    {
                        lblInfo.Text = Properties.Resources.AtNextLogonEFT;
                    }
                    else
                    {
                        lblInfo.Text = Properties.Resources.AtNextLogonHardware;
                    }

                    pnlInfo.Visible = true;
                }
                else
                {
                    pnlInfo.Visible = false;
                }
            }
        }

        private void loginControl_OnTokenLogin(object sender, EventArgs e)
        {
            string login = "";
            string passwordHash = "";
            LoginResult result = DLLEntry.DataModel.TokenLogin(
                loginEntry.ServerName,
                loginEntry.WindowsAuthentication,
                loginEntry.Login,
                loginEntry.Password,
                loginEntry.DatabaseName,
                AuthenticationToken.CreateHash(loginControl.Login),
                loginEntry.ConnectionType,
                ConnectionUsageType.UsageNormalClient,
                loginEntry.DataAreaID,
                out login,
                out passwordHash);

            if (result == LoginResult.TokenIsUser)
            {
                return;
            }
            else if (result == LoginResult.TokenNotFound)
            {
                loginControl.ErrorCode = result;
            }
            else if (result == LoginResult.Success)
            {
                LoginPanelService.LoginHandlerResults loginHandlerResults = loginResultHandler(ref result, this);

                if (result == LoginResult.Success || loginHandlerResults.SuccessfulReLogon)
                {
                    loginControl.ErrorCode = 0;

                    if (loginHandlerResults.UserCancelledPasswordChange)
                    {
                        loginControl.ClearPassword();
                        return;
                    }

                    Login = login;
                    if (OperationPerformed != null)
                    {
                        LogonFormEventArguments eventArgs = new LogonFormEventArguments { Operation = LogonFormOperation.LogOn };

                        OperationPerformed(this, eventArgs);

                        loginControl.ClearPassword();

                        if (eventArgs.Cancel)
                        {
                            DLLEntry.DataModel.LogOff();
                            return;
                        }

                        SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad = (touchKeyboard.KeyboardMode == LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric);
                    }
                }
            }
        }

        private void loginControl_OnLogin(object sender, EventArgs e)
        {
            LoginResult result = DLLEntry.DataModel.Login(
                loginEntry.ServerName,
                loginEntry.WindowsAuthentication,
                loginEntry.Login,
                loginEntry.Password,
                loginEntry.DatabaseName,
                loginControl.Login,
                SecureStringHelper.FromString(loginControl.Password),
                loginEntry.ConnectionType,
                ConnectionUsageType.UsageNormalClient,
                loginEntry.DataAreaID);

            LoginPanelService.LoginHandlerResults loginHandlerResults = loginResultHandler(ref result, this);

            if(result == LoginResult.Success || loginHandlerResults.SuccessfulReLogon)
            {
                loginControl.ErrorCode = 0;

                if (loginHandlerResults.UserCancelledPasswordChange)
                {                    
                    loginControl.ClearPassword();
                    return;
                }
                                       

                if (OperationPerformed != null)
                {
                    LogonFormEventArguments eventArgs = new LogonFormEventArguments { Operation = LogonFormOperation.LogOn };

                    OperationPerformed(this, eventArgs);

                    loginControl.ClearPassword();

                    if (eventArgs.Cancel)
                    {
                        DLLEntry.DataModel.LogOff();
                        return;
                    }

                    SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad = (touchKeyboard.KeyboardMode == LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric);
                }
            }
            else if (loginHandlerResults.DisplayError)
            {
                if (result == LoginResult.UserAuthenticationFailed ||
                    result == LoginResult.UserLockedOut ||
                    result == LoginResult.LoginDisabled ||
                    result == LoginResult.UnknownServerError ||
                    result == LoginResult.CouldNotConnectToDatabase ||
                    result == LoginResult.UserDoesNotMatchConnectionIntent)
                {
                    loginControl.ErrorCode = result;
                }
            }

            if (result != LoginResult.Success)
            {
                try
                {
                    DLLEntry.DataModel.LogOff();
                }
                catch (Exception)
                {


                }
            }
        }

        public UserControl Panel
        {
            get { return this; }
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            args.LayoutName = DLLEntry.StoreKeyboardLayoutName;
            args.CultureName = DLLEntry.StoreKeyboardCode;
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {
            loginControl.PressLogin();
        }

        protected override void OnResize(EventArgs e)
        {
            loginControl.Width = Width - 10;

            if(Height >= 1000)
            {
                loginControl.Height = Math.Max(650, Height - 500);
            }

            loginControl.Location = new Point((Width / 2) - (loginControl.Width / 2), (Height / 2) - (loginControl.Height / 2) - 110);

            base.OnResize(e);

            try
            {
                if (touchKeyboard != null)
                {
                    if (Width > 1400)
                    {
                        touchKeyboard.Location = new Point((Width - 1400) / 2, touchKeyboard.Location.Y);
                        touchKeyboard.Width = 1400;
                    }
                    else
                    {
                        touchKeyboard.Location = new Point(5, touchKeyboard.Location.Y);
                        touchKeyboard.Width = Width - 10;
                    }

                    lblVersion.Location = new Point(touchKeyboard.Right - lblVersion.Width + 6, lblVersion.Location.Y);
                }
            }
            catch
            {
                // We suppress this form sizing exeption
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintDiagonalRibbon();
        }

        private void PaintDiagonalRibbon()
        {
            IntPtr hdc = GetWindowDC(this.Handle);
            Graphics g = Graphics.FromHdc(hdc);  //This allows us to paint on top of all controls

            if (settings.TrainingMode)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddPolygon(new[]
                    {
                        new Point(Width - 160, 0),
                        new Point(Width - 100, 0),
                        new Point(Width, 100),
                        new Point(Width, 160)
                    });

                    g.FillPath(new SolidBrush(ColorPalette.NegativeNumber), path);
                }

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(Properties.Resources.TrainingMode, Font.FontFamily, (int)FontStyle.Bold, 14,
                        new Point(0, 0), StringFormat.GenericDefault);

                    using (Matrix m = new Matrix())
                    {
                        m.Rotate(45);
                        path.Transform(m);
                    }

                    using (Matrix m = new Matrix())
                    {
                        RectangleF pathBounds = path.GetBounds();

                        int x = Width - 80 - (int)(pathBounds.Width / 2);
                        int y = 80 - (int)(pathBounds.Height / 2);

                        m.Translate(x + 20, y - 20);
                        path.Transform(m);

                        SmoothingMode oldMode = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        g.FillPath(Brushes.White, path);
                        g.SmoothingMode = oldMode;
                    }
                }
            }

            if (service.LicenseType == LicenseType.Demo)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddPolygon(new[]
                    {
                        new Point(Width - 230, 0),
                        new Point(Width - 170, 0),
                        new Point(Width, 170),
                        new Point(Width, 230)
                    });

                    g.FillPath(new SolidBrush(ColorPalette.NegativeNumber), path);
                }

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(service.LicenseTypeName, Font.FontFamily, (int)FontStyle.Bold, 14,
                        new Point(0, 0), StringFormat.GenericDefault);

                    using (Matrix m = new Matrix())
                    {
                        m.Rotate(45);
                        path.Transform(m);
                    }

                    using (Matrix m = new Matrix())
                    {
                        RectangleF pathBounds = path.GetBounds();

                        int x = Width - 115 - (int)(pathBounds.Width / 2);
                        int y = 115 - (int)(pathBounds.Height / 2);

                        m.Translate(x + 20, y - 20);
                        path.Transform(m);

                        SmoothingMode oldMode = g.SmoothingMode;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        g.FillPath(Brushes.White, path);
                        g.SmoothingMode = oldMode;
                    }
                }
            }

            ReleaseDC(this.Handle, hdc);
        }

        private void LogOnListPage_Load(object sender, EventArgs e)
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                lblVersion.Text = "v." + assembly.GetName().Version;
            }
            catch
            {
                // We just suppress the exception here
            }
        }

        private void loginControl_Paint(object sender, PaintEventArgs e)
        {
            //Need to repaint over the login control of small resolutions
            PaintDiagonalRibbon();
        }
    }
}
