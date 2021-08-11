using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Settings;
using LSOne.Peripherals;
using System;
using System.Drawing;
using System.Security;
using System.Windows.Forms;

namespace LSOne.Services.LoginPanel.Pages
{
    public partial class AuthorizeOperationPage : UserControl, IAuthorizeOperationPage
    {
        private SQLServerLoginEntry loginEntry;
        private bool tokenLogin;
        private readonly PermissionInfo permissionInfo;
        private bool init;
        private readonly POSOperations posOperation;

        private bool isUserFocused;
        private bool isPasswordFocused;
        private bool scannerEnabled;

        public event LogonFormEventHandler OperationPerformed;

        protected SecureString Password
        {
            get
            {
                return SecureStringHelper.FromString(tbPassword.Text.Trim());
            }
        }

        protected string Login
        {
            get
            {
                return tbUser.Text.Trim();
            }
            set
            {
                tbUser.Text = value;
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
                        tbUser.PasswordChar = '\0';
                        tbUser.UseSystemPasswordChar = true;
                        lblUser.Text = Properties.Resources.Token;
                        btnSwitchLoginMethod.BackgroundImage = Properties.Resources.Password_48;
                        btnOK.Location = new Point(btnOK.Location.X, tbUser.Location.Y + 50);
                    }
                    else
                    {
                        tbUser.UseSystemPasswordChar = false;
                        lblUser.Text = Properties.Resources.User;
                        btnSwitchLoginMethod.BackgroundImage = Properties.Resources.Login_48;
                        btnOK.Location = new Point(btnOK.Location.X, tbPassword.Location.Y + 50);
                    }
                }
            }
        }

        public UserControl Panel
        {
            get { return this; }
        }

        protected AuthorizeOperationPage()
        {
            InitializeComponent();

            DoubleBuffered = true;
            btnOK.Enabled = false;

            tbUser.DrawBorder = false;
            tbPassword.DrawBorder = false;
            btnSwitchLoginMethod.DrawBorder = false;
            scannerEnabled = false;
        }

        public AuthorizeOperationPage(SQLServerLoginEntry loginEntry, POSOperations operation)
            : this()
        {
            this.loginEntry = loginEntry;
            posOperation = operation;

            switch (posOperation)
            {
                case POSOperations.ApplicationExit:
                    lblHeader.Text = Properties.Resources.ExitPanelTitle;
                    lblNote.Text = Properties.Resources.ExitPanelText;
                    permissionInfo = new PermissionInfo(Permission.ExitPOSApplication);
                    break;
                case POSOperations.RestartComputer:
                    lblHeader.Text = Properties.Resources.RestartPanelTitle;
                    lblNote.Text = Properties.Resources.RestartPanelText;
                    permissionInfo = new PermissionInfo(Permission.RestartComputer);
                    break;
                case POSOperations.ShutDownComputer:
                    lblHeader.Text = Properties.Resources.ShutdownPanelTitle;
                    lblNote.Text = Properties.Resources.ShutdownPanelText;
                    permissionInfo = new PermissionInfo(Permission.ShutdownComputer);
                    break;
                case POSOperations.ActivateTrainingMode:
                    lblHeader.Text = Properties.Resources.ActivateTrainingPanelTitle;
                    lblNote.Text = Properties.Resources.ActivateTrainingPanelText;
                    permissionInfo = new PermissionInfo(Permission.ActivateTrainingMode);
                    break;
                default:
                    break;
            }

            init = false;
            this.TokenLogin = SettingsContainer<ApplicationSettings>.Instance.TokenLogin;
            init = true;
        }

        private bool AllowTrainingMode()
        {
            if (posOperation == POSOperations.ActivateTrainingMode)
            {
                //Fiscalization can not allow training mode f.ex. Norwegian fiscalization does nto allow it
                IFiscalService fiscalService = (IFiscalService)DLLEntry.DataModel.Service(ServiceType.FiscalService);
                if (fiscalService != null && fiscalService.IsActive())
                {
                    return fiscalService.IsOperationAllowed(DLLEntry.DataModel, POSOperations.ActivateTrainingMode);
                }
            }

            return true;
        }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            IConnectionManager server = DLLEntry.DataModel;
            bool operationAllowed = false;
            if(server.Connection == null)
            {
                server.OpenDatabaseConnection(loginEntry.ServerName, 
                                              loginEntry.WindowsAuthentication, 
                                              loginEntry.Login, 
                                              loginEntry.Password, 
                                              loginEntry.DatabaseName, 
                                              loginEntry.ConnectionType, 
                                              loginEntry.DataAreaID);
            }

            
            if (!AllowTrainingMode())
            {
                IDialogService dialogService = (IDialogService)server.Service(ServiceType.DialogService);
                dialogService.ShowMessage(Properties.Resources.NotAllowedTrainingMode, Properties.Resources.TrainingMode, MessageBoxButtons.OK, MessageDialogType.Generic);

                return;
            }
            

            if (tokenLogin)
            {
                bool tokenIsUser;
                if ((server.HasPermission(permissionInfo.GUID, AuthenticationToken.CreateHash(tbUser.Text), out tokenIsUser)))
                {
                    server.LogOff();
                    DoOperation();
                    operationAllowed = true;
                }
            }
            else
            {
                if ((server.HasPermission(permissionInfo.GUID, Login, Password)))
                {
                    server.LogOff();
                    DoOperation();
                    operationAllowed = true;
                }
            }
            
            if(!operationAllowed)
            {
                IDialogService dialogService = (IDialogService)server.Service(ServiceType.DialogService);
                dialogService.ShowMessage(Properties.Resources.WrongManagerLogin, Properties.Resources.AuthenticationFailed, MessageBoxButtons.OK, MessageDialogType.Generic);

                tbUser.Focus();
            }
        }

        private void DoOperation()
        {
            if (OperationPerformed != null)
            {
                LogonFormEventArguments args = new LogonFormEventArguments();
                switch(posOperation)
                {
                    case POSOperations.ApplicationExit:
                        args.Operation = LogonFormOperation.Exit;
                        break;
                    case POSOperations.RestartComputer:
                        args.Operation = LogonFormOperation.Reboot;
                        break;
                    case POSOperations.ShutDownComputer:
                        args.Operation = LogonFormOperation.ShutDown;
                        break;
                    case POSOperations.ActivateTrainingMode:
                        args.Operation = LogonFormOperation.ActivateTraining;
                        break;
                    default:
                        args.Operation = LogonFormOperation.LogOn;
                        break;
                }

                OperationPerformed(this, args);
            }
        }

        private void tbUser_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbUser;
            touchKeyboard.DelayedEnabled = true;

            isUserFocused = true;
            pnlAuthorize.Invalidate();
        }

        private void tbUser_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;

            isUserFocused = false;
            pnlAuthorize.Invalidate();
        }

        private void tbPassword_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbPassword;
            touchKeyboard.DelayedEnabled = true;

            isPasswordFocused = true;
            pnlAuthorize.Invalidate();
        }

        private void tbPassword_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;

            isPasswordFocused = false;
            pnlAuthorize.Invalidate();
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
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (!tokenLogin)
                {
                    tbPassword.Focus();
                }
                else
                {
                    tbUser.Text = StringExtensions.TrackBeforeSeparator(tbUser.Text, DLLEntry.Settings.HardwareProfile.StartTrack1, DLLEntry.Settings.HardwareProfile.Separator1, DLLEntry.Settings.HardwareProfile.EndTrack1);
                    btnOK_Click(sender, e);
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

        protected override void OnResize(EventArgs e)
        {
            pnlAuthorize.Location = new Point((Width / 2) - (pnlAuthorize.Width / 2), (Height / 2) - (pnlAuthorize.Height / 2) - 130);

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
                        touchKeyboard.Location = new Point(4, touchKeyboard.Location.Y);
                        touchKeyboard.Width = Width - 8;
                    }
                }
            }
            catch
            {
                // We suppress this form sizing exeption
            }
        }

        private void pnlAuthorize_Paint(object sender, PaintEventArgs e)
        {
            Pen normalPen = new Pen(ColorPalette.POSTextColor, 2);
            Pen focusedPen = new Pen(ColorPalette.POSFocusedBorderColor, 2);

            e.Graphics.DrawLine(isUserFocused ? focusedPen : normalPen, new Point(tbUser.Location.X, tbUser.Bottom + 1), new Point(btnSwitchLoginMethod.Right, btnSwitchLoginMethod.Bottom + 1));
            if (tbPassword.Visible)
            {
                e.Graphics.DrawLine(isPasswordFocused ? focusedPen : normalPen, new Point(tbPassword.Location.X, tbPassword.Bottom + 1), new Point(tbPassword.Right, tbPassword.Bottom + 1));
            }

            normalPen.Dispose();
            focusedPen.Dispose();
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

        private void AuthorizeOperationPage_Enter(object sender, EventArgs e)
        {
            if (tokenLogin)
            {
                EnableBarcodeScanner();
            }
        }

        private void AuthorizeOperationPage_Leave(object sender, EventArgs e)
        {
            DisableBarcodeScanner();
        }
    }
}