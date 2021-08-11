using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IUserMigrationCommands : IDataProviderBase<POSUser>
    {
        List<POSUser> GetNonUserPosUsers(IConnectionManager entry);
        List<LSOne.DataLayer.BusinessObjects.UserManagement.User> GetNonPosUserUsers(IConnectionManager entry);
    }
}