using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IUserGroupData : IDataProviderBase<UserGroup>
    {
        /// <summary>
        /// Get id and name of selected Pos Users from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of Pos Users id and name</returns>
        List<DataEntity> GetPosUserPermissionGroupList(IConnectionManager entry);

        /// <summary>
        /// Get pos user group list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>list of users </returns>
        List<UserGroup> GetPosUserGroupList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Save all selected users into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posUserList">userIds</param>
        void SavePosUserGroups(IConnectionManager entry, List<UserGroup> posUserList);

        void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATEPERMISSION");
    }
}