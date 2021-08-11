using System;
using System.Data.SqlClient;
using System.Security;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.SqlConnector.MiniConnectionManager
{
    public class MiniSqlServerConnectionManager : SqlServerConnectionManager
    {
        private bool allowAudit;

        public MiniSqlServerConnectionManager()
        {

        }

        public MiniSqlServerConnectionManager(IConnection connection)
        {
            Connection = connection;
        }

        public override bool HasPermission(string permissionUUID)
        {
            return true;
        }

        public override bool HasPermission(string permissionUUID, string login, System.Security.SecureString password)
        {
            return true;
        }

        public override bool HasPermission(string permissionUUID, string tokenHash, out bool tokenIsUser)
        {
            tokenIsUser = false;

            return true;
        }

        public LoginResult Login(string connectionString, ConnectionUsageType connectionUsage, string dataAreaID, bool firstNameFirst, bool allowAudit = false)
        {
            try
            {
                this.allowAudit = allowAudit;
                CreateSqlServerConnection(connectionString, dataAreaID);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    return LoginResult.CouldNotConnectToDatabase;
                }
                return LoginResult.UnknownServerError;
            }

            LoginSetup(connectionUsage);
            return LoginResult.Success;
        }

        public LoginResult Login(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, ConnectionUsageType connectionUsage, bool firstNameFirst, bool disableReplicationActionCreation, bool allowAudit = false)
        {
            try
            {
                this.allowAudit = allowAudit;
                CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, disableReplicationActionCreation);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    return LoginResult.CouldNotConnectToDatabase;
                }
                return LoginResult.UnknownServerError;
            }

            LoginSetup(connectionUsage);
            return LoginResult.Success;
        }

        private void LoginSetup(ConnectionUsageType connectionUsage)
        {
            User user = null;
            SqlServerUserProfile userProfile = new SqlServerUserProfile(); // We just make a empty user profile
            this.ConnectionUsage = connectionUsage;

            userProfile.Settings.Add(GenericConnector.Constants.Settings.WriteAuditing, "0");
            userProfile.Settings.Add(GenericConnector.Constants.Settings.NamingFormat, "0");

            user = new User("framework", new Guid("d1b6a7f0-3bcd-11e3-aa6e-0800200c9a66"), "framework", false, userProfile, false, 900);

            SetUserID((Guid)user.ID, false);

            ((ProfileSettings)Settings).Populate(user.Profile.Settings, Cache);

            CurrentUser = user;
        }

        protected override void CreateSqlServerConnection(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, bool retry = false)
        {
            Connection = new MiniSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, allowAudit,DisableReplicationActionCreation);
            ConnectLostHandler();
        }

        protected override void CreateSqlServerConnection(string connectionString, string dataAreaID)
        {
            Connection = new MiniSqlServerConnection(connectionString, dataAreaID, allowAudit, DisableReplicationActionCreation);
            ConnectLostHandler();
        }
        
    }
}
