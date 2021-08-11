using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.SqlConnector
{
    public class ConnectionManagerTemporary : SqlServerConnectionManager, IConnectionManagerTemporary
    {
        internal ConnectionManagerTemporary(SqlServerConnectionManager parent, bool disableReplicationActionCreation)
            : base(parent)
        {
            profileWasUpdated = parent.profileWasUpdated;
            CurrentStoreID = parent.CurrentStoreID;
            CurrentUser = parent.CurrentUser;
            ServiceFactory = parent.ServiceFactory;
            Services = parent.Services;
            siteServiceAddress = parent.siteServiceAddress;
            CurrentTerminalID = parent.CurrentTerminalID;
            CurrentStaffID = parent.CurrentStaffID;
            siteServicePort = parent.siteServicePort;
            ErrorLogger = parent.ErrorLogger;
            isTouchClient = parent.IsTouchClient;
            isAdmin = parent.IsAdmin;

            Connection = new SqlServerConnection(parent.Connection as SqlServerConnection, disableReplicationActionCreation);
        }

        public void Close()
        {
            if (Connection as SqlServerConnection != null)
            {
                (Connection as SqlServerConnection).Close();
                Connection = null;
            }
        }
    }
}
