using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IUserGroupData : IDataProviderBase<UserGroup>
    {
        List<UserInGroupResult> GetUserGroupAssignments(IConnectionManager entry, Guid userGuid);
        List<UserGroup> AllGroups(IConnectionManager entry);
        List<UserGroup> GetGroupsForUser(IConnectionManager entry, RecordIdentifier userID);
        void AddUser(IConnectionManager entry, Guid userGuid, Guid groupGuid);
        void RemoveUser(IConnectionManager entry, Guid userGuid, Guid groupGuid);
        void Delete(IConnectionManager entry, Guid groupGuid);
        void New(IConnectionManager entry, UserGroup group);
        void Edit(IConnectionManager entry, UserGroup group);
        void SetPermission(IConnectionManager entry, Guid groupGuid, Guid permissionGuid, GroupGrantMode mode);
        List<PermissionsAssignmentResult> GetPermissions(IConnectionManager entry, Guid userGroupID, string searchText);
        UserGroup Get(IConnectionManager entry, RecordIdentifier ID);
    }
}