using System;
using System.Collections.Generic;
using System.Security;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IUserData : IDataProviderBase<User>, ISequenceable
    {
        User Get(IConnectionManager entry, Guid userID);
        RecordIdentifier CreateStaffMemberForPOS(IConnectionManager entry, RecordIdentifier staffID, Name name, RecordIdentifier userProfileID);
        bool Exists(IConnectionManager entry, string login);
        bool IsUserLoginValid(IConnectionManager entry, RecordIdentifier userLogin);
        bool GuidExists(IConnectionManager entry, Guid guid);
        List<User> AllUsers(IConnectionManager entry);
        List<User> GetUsersInGroup(IConnectionManager entry, Guid groupGuid);

        List<User> GetLoginUsersUnsecure(
            IConnectionManager entry,
            string dataSource,
            bool windowsAuthentication,
            string sqlServerLogin,
            SecureString sqlServerPassword,
            string databaseName,
            ConnectionType connectionType,
            string dataAreaID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            RecordIdentifier licenseCode,
            string version,
            out NameFormat nameFormat,
            out string storeLanguage,
            out string storeKeyboardCode,
            out string storeKeyboardLayoutName,
            out RecordIdentifier licensePassword,
            out DateTime licenseExpireDate);

        List<User> FindUsers(IConnectionManager entry, string firstName, string middleName, string lastName, string suffix, string loginName, int maxCount);
        List<User> FindUsersFromNameOrLogin(IConnectionManager entry, string text, int maxCount);
        User FindUserFromStaffID(IConnectionManager entry, string staffID);
        void SetPermission(IConnectionManager entry, Guid userGuid, Guid permissionGuid, UserGrantMode mode);
        string GetPermissionCode(IConnectionManager entry, Guid permissionGuid);
        void SetEnabled(IConnectionManager entry, Guid userGuid, bool value);
        void Delete(IConnectionManager entry, Guid userGuid);

        (Guid UserID, RecordIdentifier StaffID) New(
            IConnectionManager entry,
            string login,
            string password,
            Name name,
            List<Guid> securityGroups,
            bool isActiveDirectoryUser,
            bool isServerUser,
            string email);

        void Save(IConnectionManager entry, User user);

        int GetUserCount(IConnectionManager entry);

        int GetLockedOutUserCount(IConnectionManager entry, int lockoutThreshold);

        List<User> Search(IConnectionManager entry, UserSearchFilter filter);

        List<User> GetUsersByUserProfile(IConnectionManager entry, RecordIdentifier userProfileID);

        /// <summary>
        /// Executes an unsecure stored procedure to get the site service information.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="storeID"></param>
        /// <param name="serverName"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionType"></param>
        /// <param name="dataAreaID"></param>
        /// <param name="siteServiceAddress">returns the site service host address</param>
        /// <param name="siteServicePortNumber">returns the site service port number</param>
        /// <returns>true if the information was found</returns>
        bool SetSiteServiceConnectionInfoUnsecure(IConnectionManager entry, RecordIdentifier storeID,
            string serverName, bool windowsAuthentication, string login, SecureString password, string databaseName,
            ConnectionType connectionType, string dataAreaID, out string siteServiceAddress,
            out ushort siteServicePortNumber);
    }
}