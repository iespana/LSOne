using LSOne.Controls.EventArguments;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Core;
using LSOne.POS.Processes;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.Settings;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LicenseType = LSOne.Services.Interfaces.LicenseType;

namespace LSOne.Services.LoginPanel.Pages
{
    internal partial class LogOnPage : UserControl, ILoginPanelPage
    {
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32")]
        private static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        private SQLServerLoginEntry loginEntry;
        public event LogonFormEventHandler OperationPerformed;
        private ISettings settings;
        private ILicenseService service;

        public delegate LoginPanelService.LoginHandlerResults LoginResultHandler(ref LoginResult result, ILoginPanelPage senderPanel);
        private LoginResultHandler loginResultHandler;

        public LogOnPage(SQLServerLoginEntry loginEntry, ISettings settings, bool tokenLogin, LoginResultHandler loginResultHandler)
            : this()
        {            
            loginControl.TokenLogin = tokenLogin;
            this.loginEntry = loginEntry;
            this.settings = settings;
            service = (ILicenseService)DLLEntry.DataModel.Service(ServiceType.LicenseService);
            loginControl.ErrorDisplayType = service.ErrorDisplayType;
            loginControl.ExpiryDate = service.LicenseExpiryDate;
            this.loginResultHandler = loginResultHandler;

            touchKeyboard.KeyboardMode = SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad ? LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric : LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Alphabet;            
        }

        public LogOnPage()
        {
            InitializeComponent();

            DoubleBuffered = true;
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

        public bool TokenLogin
        {
            get
            {
                return loginControl.TokenLogin;
            }
        }

        public string Password
        {
            get
            {
                return loginControl.Password;
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
                loginControl.ExpiryDate = service.LicenseExpiryDate;
                SetInfoPanel();
            }
        }

        private void SetInfoPanel()
        {
            int weight = 0;

            //Increase weight by powers of 2
            weight += DLLEntry.Settings.NeedsEFTSetup ? 1 : 0;
            weight += DLLEntry.Settings.HardwareProfileNeedsSetup ? 2 : 0;
            weight += DLLEntry.Settings.RestartDevices ? 4 : 0;

            if(weight > 0)
            {
                pnlInfo.Visible = true;

                switch(weight)
                {
                    case 1: lblInfo.Text = Resources.AtNextLogonEFT; break;
                    case 2: lblInfo.Text = Resources.AtNextLogonHardware; break;
                    case 3: lblInfo.Text = Resources.AtNextLogonHardwareAndEFT; break;
                    case 4: lblInfo.Text = Resources.AtNextLogonRestartDevices; break;
                    case 5: lblInfo.Text = Resources.AtNextLogonRestartAndEFT; break;
                    case 6: lblInfo.Text = Resources.AtNextLogonRestartAndHardware; break;
                    case 7: lblInfo.Text = Resources.AtNextLogonRestartAndHardwareAndEFT; break;
                }
            }
            else
            {
                pnlInfo.Visible = false;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            loginControl.Width = Width - 10;
            loginControl.Location = new Point((Width / 2) - (loginControl.Width / 2), (Height / 2) - (loginControl.Height / 2) - 130);

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
                         touchKeyboard.Location = new Point(10, touchKeyboard.Location.Y);
                         touchKeyboard.Width = Width - 20;
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

        private void HandleLoginResults(IConnectionManager entry, LoginResult result, bool isTokenLogin = false)
        {
            LoginPanelService.LoginHandlerResults handlerResults = loginResultHandler(ref result, this);

            if (result == LoginResult.Success || handlerResults.SuccessfulReLogon)
            {
                ISettings settings = (ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                ((POSApp)settings.POSApp).OnLoggedOn();

                loginControl.ErrorCode = 0; // To repaint any previous error messages                  

                if (handlerResults.UserCancelledPasswordChange)
                {
                    if (isTokenLogin)
                    {
                        loginControl.ClearToken();
                    }
                    loginControl.ClearPassword();
                    return;
                }

                if (OperationPerformed != null)
                {
                    LogonFormEventArguments eventArgs = new LogonFormEventArguments();
                    eventArgs.Operation = LogonFormOperation.LogOn;

                    OperationPerformed(this, eventArgs);

                    if (isTokenLogin)
                    {
                        loginControl.ClearToken();
                    }
                    loginControl.ClearPassword();
                    if (eventArgs.Cancel)
                    {
                        DLLEntry.DataModel.LogOff();
                        return;
                    }
                }

                SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad = (touchKeyboard.KeyboardMode == LSOne.Controls.TouchKeyboard.KeyboardModeEnum.Numeric);
            }
            else if (handlerResults.DisplayError)
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
            else
            {
                if (result == LoginResult.Success)
                {
                    Login = login;
                    loginControl.FocusLogin();
                    loginControl.ErrorCode = 0;
                }

                HandleLoginResults(DLLEntry.DataModel, result, true);
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

            HandleLoginResults(DLLEntry.DataModel, result);
        }

        public UserControl Panel
        {
            get { return this; }
        }

        private void PaintDiagonalRibbon()
        {
            IntPtr hdc = GetWindowDC(this.Handle);
            Graphics g = Graphics.FromHdc(hdc); //This allows us to paint on top of all controls

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
        }

        private void touchKeyboard_ObtainCultureName(object sender, CultureEventArguments args)
        {
            args.LayoutName = DLLEntry.StoreKeyboardLayoutName;
            args.CultureName = DLLEntry.StoreKeyboardCode;
        }

        private void touchKeyboard_EnterPressed(object sender, EventArgs e)
        {

        }

        private void LogOnPage_Load(object sender, EventArgs e)
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