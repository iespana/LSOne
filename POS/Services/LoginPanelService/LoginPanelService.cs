using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Configuration;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Exeptions;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.POS.Core;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.LoginPanel.Pages;
using LSOne.Services.LoginPanel.Properties;
using LSOne.Services.LoginPanel.WinFormsTouch;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LicenseType = LSOne.Services.Interfaces.LicenseType;
using LSOne.Utilities.Settings;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services.LoginPanel
{
    public partial class LoginPanelService : ILoginPanelService
    {
        internal delegate Tuple<List<User>, NameFormat> RefreshList(); 

        IErrorLog errorLog;

        private SQLServerLoginEntry loginEntry;
        internal static bool UseSiteServiceAuthentication = true;

        internal struct LoginHandlerResults
        {
            public bool DisplayError;
            public bool UserCancelledPasswordChange;
            public bool SuccessfulReLogon;
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {
                errorLog = value;
            }
        }

        public void Init(IConnectionManager entry)
        {
            DLLEntry.DataModel = entry;
        }

        public virtual ILoginPanelPage CreateLoginPage(SQLServerLoginEntry loginEntry, ISettings settings, bool tokenLogin)
        
{
            DLLEntry.Settings = settings;
            string licenseCode = settings.License.EncryptedLicenseCodeForCurrentHardware;

            this.loginEntry = loginEntry;

            Tuple<string, NameFormat, List<User>> loginSettings = GetSettings();

            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(loginSettings.Item1);
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;            

            SetSiteServiceConnectionInfoUnsecure();

            if (loginSettings.Item3 != null && loginSettings.Item3.Count > 0)
            {
                return new LogOnListPage(loginSettings.Item3, loginSettings.Item2, loginEntry, settings, refreshList, tokenLogin, LoginResultHandler);
            }

            return new LogOnPage(loginEntry, settings, tokenLogin, LoginResultHandler);
        }

        public virtual IAuthorizeOperationPage CreateAuthorizeOperationPage(SQLServerLoginEntry loginEntry, POSOperations posOperation)
        {
            return new AuthorizeOperationPage(loginEntry, posOperation);
        }

        private Tuple<string, NameFormat, List<User>> GetSettings()
        {
            string storeLanguage = "";
            string storeKeyboardCode = "";
            string storeKeyboardLayoutName = "";
            string licenseCode = DLLEntry.Settings.License.EncryptedLicenseCodeForCurrentHardware;
            NameFormat nameFormat = NameFormat.FirstNameFirst;
            RecordIdentifier licensePassword = new RecordIdentifier();
            DateTime licenseExpireDate;

            ILicenseService service = (ILicenseService)DLLEntry.DataModel.Service(ServiceType.LicenseService);

            List<User> loginUsers = null;

            try
            {
                loginUsers = Providers.UserData.GetLoginUsersUnsecure(
                    DLLEntry.DataModel,
                    loginEntry.ServerName,
                    loginEntry.WindowsAuthentication,
                    loginEntry.Login,
                    loginEntry.Password,
                    loginEntry.DatabaseName,
                    loginEntry.ConnectionType,
                    loginEntry.DataAreaID,
                    loginEntry.StoreID,
                    loginEntry.TerminalID,
                    licenseCode,
                    service.Version,
                    out nameFormat,
                    out storeLanguage,
                    out storeKeyboardCode,
                    out storeKeyboardLayoutName,
                    out licensePassword,
                    out licenseExpireDate);
            }
            catch (StoreMissingException)
            {
                throw;
            }
            catch (TerminalMissingException)
            {
                throw;
            }
            catch (FunctionalityProfileMissingExeption)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (DLLEntry.DataModel.ErrorLogger != null)
                {
                    DLLEntry.DataModel.ErrorLogger.LogMessageToFile(LogMessageType.Error, "Unsecure procedure failed.\n" + ex.Message, "LoginPanelService.GetSettings");
                }
                
                // We just suppress this one, it just means that we were unable to connect to the database 
                throw new DatabaseException("Error Connecting to database",ex,null) ;
                // as connection might not have been set up so we display the type in login panel
            }

            DLLEntry.StoreLanguage = storeLanguage;
            DLLEntry.StoreKeyboardCode = storeKeyboardCode;
            DLLEntry.StoreKeyboardLayoutName = storeKeyboardLayoutName;
            DLLEntry.Settings.License.LicensePassword = licensePassword;

            return new Tuple<string, NameFormat, List<User>>(storeLanguage, nameFormat, loginUsers);
        }

        private Tuple<List<User>, NameFormat> refreshList()
        {
            Tuple<string, NameFormat, List<User>> loginSettings = GetSettings();
            return new Tuple<List<User>, NameFormat>(loginSettings.Item3, loginSettings.Item2);
        }

        public virtual bool PermissionOverrideDialog(PermissionInfo info)
        {
            RecordIdentifier overriderID = null;
            return PermissionOverrideDialog(info, ref overriderID);
        }

        public virtual bool PermissionOverrideDialog(PermissionInfo info, ref RecordIdentifier overriderID)
        {
            DialogResult result = DialogResult.Yes;
            if (!info.LockPermissionOverride)
            {
                string message = Resources.UserLacksPermission +
                              Environment.NewLine +
                              Environment.NewLine +
                              Resources.TempPermissionLogin;

                IDialogService dialogService = (IDialogService) DLLEntry.DataModel.Service(ServiceType.DialogService);
                result = dialogService.ShowMessage(message, Resources.InsufficientRights, MessageBoxButtons.YesNo, MessageDialogType.Attention);
            }
            if (result != DialogResult.No)
            {
                using (PermissionOverrideDialog overrideDialog = new PermissionOverrideDialog(info))
                {
                    if (overriderID != null && !overriderID.IsEmpty && overriderID.DBValue is string)
                    {
                        overrideDialog.Login = (string)overriderID;
                    }
                    if (overrideDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (info.Transaction != null &&
                            DLLEntry.Settings.Store.OperationAuditSetting != OperationAuditEnum.Never)
                        {
                            info.PerformAudit(DLLEntry.DataModel, overrideDialog.Login);
                        }
                        overriderID = overrideDialog.Login;

                        SettingsContainer<ApplicationSettings>.Instance.TokenLoginForManagerOverride = overrideDialog.TokenLogin;
                     
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Displays the Lock terminal dialog. You should only be able to shut down the dialog upon succesfull login or log off.
        /// </summary>
        /// <returns>Did the user exit by loggin off.</returns>
        public virtual bool LockTerminal()
        {
            using (LockTerminalDialog lockTerminalDialog = new LockTerminalDialog(DLLEntry.DataModel.CurrentUser.UserName))
            {
                DialogResult result = lockTerminalDialog.ShowDialog();

                SettingsContainer<ApplicationSettings>.Instance.TokenLogin = lockTerminalDialog.TokenLogin;
                SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad = lockTerminalDialog.LoginWithNumPad;

                return (result != DialogResult.OK);
            }
        }

        public virtual bool SwitchUser()
        {
            using (SwitchUserDialog dialog = DLLEntry.Settings.FunctionalityProfile.ClearUserBetweenLogins ? new SwitchUserDialog() : new SwitchUserDialog(DLLEntry.DataModel.CurrentUser.Text))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ((Settings) DLLEntry.Settings).LoadPOSUser(DLLEntry.DataModel, dialog.StaffID, DLLEntry.Settings.Store.ID, false);
                    DLLEntry.DataModel.Settings.SetApplicationSettings(ApplicationSettingsConstants.POSApplication,  DLLEntry.Settings);

                    SettingsContainer<ApplicationSettings>.Instance.TokenLogin = dialog.TokenLogin;
                    SettingsContainer<ApplicationSettings>.Instance.LoginWithNumpad = dialog.LoginWithNumPad;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Executes an unsecure stored procedure to get the site service information and sets it on the data model.
        /// </summary>
        internal static void SetSiteServiceConnectionInfoUnsecure()
        {
            if (UseSiteServiceAuthentication)
            {
                SQLServerLoginEntry settings = SettingsContainer<ApplicationSettings>.Instance.CurrentLoginEntry;
                UseSiteServiceAuthentication = Providers.UserData.SetSiteServiceConnectionInfoUnsecure(
                    DLLEntry.DataModel,
                    DLLEntry.DataModel.CurrentStoreID,
                    settings.ServerName,
                    settings.WindowsAuthentication,
                    settings.Login,
                    settings.Password,
                    settings.DatabaseName,
                    settings.ConnectionType,
                    settings.DataAreaID,
                    out string address, 
                    out ushort port);

                DLLEntry.DataModel.SiteServiceAddress = address;
                DLLEntry.DataModel.SiteServicePortNumber = port;
            }
        }

        private LoginHandlerResults ChangePassword(LoginHandlerResults handlerResults, ILoginPanelPage sender, bool changeTroughSiteService)
        {
            using (ChangePasswordDialog dlg = new ChangePasswordDialog(SecureStringHelper.FromString(sender.Password), sender.TokenLogin))
            {
                if (dlg.ShowDialog(sender.Panel.ParentForm) == DialogResult.Cancel)
                {
                    DLLEntry.DataModel.LogOff();
                    handlerResults.UserCancelledPasswordChange = true;
                    return handlerResults;
                }

                if (UseSiteServiceAuthentication)
                {
                    if (changeTroughSiteService)
                    {
                        DLLEntry.DataModel.GetUserPasswordInfo(DLLEntry.DataModel.CurrentUser.ID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime);
                        Interfaces.Services.SiteServiceService(DLLEntry.DataModel).ChangePasswordForUser(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, DLLEntry.DataModel.CurrentUser.ID, passwordHash, false, expiresDate, lastChangeTime);
                    }
                    else
                    {
                        DLLEntry.DataModel.GetUserPasswordInfo(DLLEntry.DataModel.CurrentUser.ID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime);
                        DLLEntry.DataModel.ChangePasswordHashForOtherUser(DLLEntry.DataModel.CurrentUser.ID, passwordHash, false, expiresDate, lastChangeTime);
                    }
                }
            }
            return handlerResults;
        }

        private bool VerifyLogin(ILoginPanelPage sender)
        {
            string passwordHash;
            DateTime expiresDate;
            DateTime lastChangeTime;

            Interfaces.Services.SiteServiceService(DLLEntry.DataModel).GetUserPasswordChangeInfo(DLLEntry.DataModel,
                DLLEntry.Settings.SiteServiceProfile,
                DLLEntry.DataModel.CurrentUser.ID,
                out passwordHash,
                out expiresDate,
                out lastChangeTime);
            string enteredPasswordHash = DLLEntry.DataModel.GetPasswordHash(SecureStringHelper.FromString(sender.Password));
            DLLEntry.DataModel.ChangePasswordHashForOtherUser(DLLEntry.DataModel.CurrentUser.ID, passwordHash, false, expiresDate, lastChangeTime);

            return enteredPasswordHash == passwordHash;
        }

        private LoginHandlerResults LoginResultHandler(ref LoginResult result, ILoginPanelPage sender)
        {
            LoginHandlerResults handlerResults = new LoginHandlerResults();
            bool siteServiceConnected = false;
            handlerResults.DisplayError = false;

            if (result == LoginResult.Success)
            {                
                bool changeUserPassword = false;
                
                if (UseSiteServiceAuthentication)
                {
                    ConnectionEnum connectionResult = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnectionWithFeedback(DLLEntry.DataModel, DLLEntry.DataModel.SiteServiceAddress, DLLEntry.DataModel.SiteServicePortNumber);                    

                    if (connectionResult == ConnectionEnum.Success)
                    {
                        siteServiceConnected = true;
                        SiteServiceProfile siteServiceProfile = DLLEntry.Settings.SiteServiceProfile;
                        if (string.IsNullOrEmpty(siteServiceProfile.SiteServiceAddress) &&
                            !string.IsNullOrEmpty(DLLEntry.DataModel.SiteServiceAddress))
                        {
                            siteServiceProfile.SiteServiceAddress = DLLEntry.DataModel.SiteServiceAddress;
                            siteServiceProfile.SiteServicePortNumber = DLLEntry.DataModel.SiteServicePortNumber;
                        }
                        // Check if the user has changed his/her password on store level
                        changeUserPassword = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).UserNeedsToChangePassword(DLLEntry.DataModel, siteServiceProfile, DLLEntry.DataModel.CurrentUser.ID);
                    }
                    else
                    {
                        if (DLLEntry.DataModel.CurrentUser.ForcePasswordChange)
                        {
                            string message = Resources.ConnectToSiteServiceFailedForPasswordWarning;

                            DialogResult warningResult = Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, Resources.CouldNotConnectToSiteService, MessageBoxButtons.YesNo, MessageDialogType.ErrorWarning);

                            if (warningResult == DialogResult.No)
                            {
                                DLLEntry.DataModel.LogOff();
                                result = LoginResult.UserAuthenticationFailed;
                                handlerResults.UserCancelledPasswordChange = true;
                                return handlerResults;
                            }
                        }
                    }
                }

                if (DLLEntry.DataModel.CurrentUser.ForcePasswordChange || changeUserPassword)
                {
                    if (changeUserPassword)
                    {
                        if(!VerifyLogin(sender))
                        {
                            handlerResults.DisplayError = true;
                            result = LoginResult.UserAuthenticationFailed;
                            DLLEntry.DataModel.LogOff();
                            return handlerResults;
                        }
                    }
                    ((DataLayer.GenericConnector.DataEntities.User)DLLEntry.DataModel.CurrentUser).ForcePasswordChange = false;
                    handlerResults = ChangePassword(handlerResults, sender, siteServiceConnected);
                }

                InitPOSEngine(sender.Login, sender.Password);
            }
            else if (result == LoginResult.UserAuthenticationFailed)
            {
                handlerResults.DisplayError = true;

                if (UseSiteServiceAuthentication && DLLEntry.DataModel.CurrentUser.ID != Guid.Empty)
                {
                    ConnectionEnum connectionResult = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnection(DLLEntry.DataModel, DLLEntry.DataModel.SiteServiceAddress, DLLEntry.DataModel.SiteServicePortNumber);
                    if (connectionResult == ConnectionEnum.Success)
                    {
                        string passwordHash;
                        DateTime expiresDate;
                        DateTime lastChangeTime;

                        Interfaces.Services.SiteServiceService(DLLEntry.DataModel).GetUserPasswordChangeInfo(DLLEntry.DataModel,
                                                                                                             DLLEntry.Settings.SiteServiceProfile,
                                                                                                             DLLEntry.DataModel.CurrentUser.ID,
                                                                                                             out passwordHash,
                                                                                                             out expiresDate,
                                                                                                             out lastChangeTime);

                        // Unauthenticated users do not have a connection open, so we explicitly open the database connection here
                        DLLEntry.DataModel.OpenDatabaseConnection(loginEntry.ServerName, loginEntry.WindowsAuthentication, loginEntry.Login, loginEntry.Password, loginEntry.DatabaseName, loginEntry.ConnectionType, loginEntry.DataAreaID);

                        // ChangePasswordHashForOtherUser also changes the NeedsPasswordChange status of the user, and we need to make sure we are mirroring the store DB user status
                        bool userNeedsToChangePassword = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).UserNeedsToChangePassword(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, DLLEntry.DataModel.CurrentUser.ID);
                        DLLEntry.DataModel.ChangePasswordHashForOtherUser(DLLEntry.DataModel.CurrentUser.ID, passwordHash, userNeedsToChangePassword, expiresDate, lastChangeTime, false);

                        string enteredPasswordHash = DLLEntry.DataModel.GetPasswordHash(SecureStringHelper.FromString(sender.Password));

                        // If the hashes match then the user was trying to log on with the new password.
                        if (enteredPasswordHash == passwordHash)
                        {
                            result = LoginResult.Success;
                            handlerResults.DisplayError = false;
                            DLLEntry.DataModel.LogOff();
                            DLLEntry.DataModel.Login(loginEntry.ServerName,
                                                       loginEntry.WindowsAuthentication,
                                                       loginEntry.Login,
                                                       loginEntry.Password,
                                                       loginEntry.DatabaseName,
                                                       sender.Login,
                                                       SecureStringHelper.FromString(sender.Password),
                                                       loginEntry.ConnectionType,
                                                       ConnectionUsageType.UsageNormalClient,
                                                       loginEntry.DataAreaID);

                            handlerResults.SuccessfulReLogon = true;

                            if (DLLEntry.DataModel.CurrentUser.ForcePasswordChange)
                            {
                                handlerResults = ChangePassword(handlerResults, sender, siteServiceConnected);
                            }

                            InitPOSEngine(sender.Login, sender.Password);
                        }
                        else
                        {
                            DLLEntry.DataModel.LogOff();
                        }
                    }
                }
            }
            else if (result == LoginResult.UserLockedOut)
            {
                if (UseSiteServiceAuthentication)
                {

                    ConnectionEnum connectionResult = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnection(DLLEntry.DataModel, DLLEntry.DataModel.SiteServiceAddress, DLLEntry.DataModel.SiteServicePortNumber);

                    if (connectionResult == ConnectionEnum.Success)
                    {
                        Interfaces.Services.SiteServiceService(DLLEntry.DataModel).LockUser(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, DLLEntry.DataModel.CurrentUser.ID);
                    }
                    else
                    {
                        string message = Resources.ConnectToSiteServiceFailedForLockout;

                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK);
                    }
                }

                handlerResults.DisplayError = true;
            }
            else if (result == LoginResult.LoginDisabled)
            {
                if (UseSiteServiceAuthentication)
                {

                    ConnectionEnum connectionResult = Interfaces.Services.SiteServiceService(DLLEntry.DataModel).TestConnection(DLLEntry.DataModel, DLLEntry.DataModel.SiteServiceAddress, DLLEntry.DataModel.SiteServicePortNumber);

                    if (connectionResult == ConnectionEnum.Success)
                    {
                        //Interfaces.Services.SiteServiceService(DLLEntry.DataModel).LockUser(DLLEntry.DataModel, DLLEntry.Settings.SiteServiceProfile, DLLEntry.DataModel.CurrentUser.ID);
                    }
                    else
                    {
                        string message = Resources.ConnectToSiteServiceFailedForLockout;

                        Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(message, Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK);
                    }
                }

                handlerResults.DisplayError = true;
            }
            else if (result == LoginResult.UnknownServerError ||
                     result == LoginResult.CouldNotConnectToDatabase ||
                     result == LoginResult.UserDoesNotMatchConnectionIntent)
            {
                handlerResults.DisplayError = true;
            }

            return handlerResults;
        }

        private void InitPOSEngine(string login, string password)
        {
            DBConnection dbConn = new DBConnection
            {
                DBServer = loginEntry.ServerName,
                WindowsAuthentication = loginEntry.WindowsAuthentication,
                DBUser = loginEntry.Login,
                DBPassword = loginEntry.Password,
                DBName = loginEntry.DatabaseName,
                SystemUser = login,
                SystemPassword = SecureStringHelper.FromString(password),
                ConnectionType = loginEntry.ConnectionType,
                ConnectionUsageType = ConnectionUsageType.UsageNormalClient,
                DataAreaID = loginEntry.DataAreaID
            };

            ((Settings)DLLEntry.Settings).POSApp.InitializeEngine(dbConn);
        }
    }
}
