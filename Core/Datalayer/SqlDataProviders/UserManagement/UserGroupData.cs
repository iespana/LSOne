using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class UserGroupData : SqlServerDataProviderBase, IUserGroupData
    {
        private static void PopulateUserGroup(IDataReader dr, UserGroup userGroup)
        {
            userGroup.ID = (Guid)dr["GUID"];
            userGroup.Name = (string)dr["Name"];
            userGroup.IsAdminGroup = (bool)dr["IsAdminGroup"];
            userGroup.Locked = (bool)dr["Locked"];
        }

        private static void PopulateUserInGroup(IDataReader dr, UserInGroupResult inGroup)
        {
            inGroup.GroupName = (string)dr["GroupName"];
            inGroup.GroupGuid = (Guid)dr["GroupGuid"];
            inGroup.IsAdminGroup = (bool)dr["IsAdminGroup"];
            inGroup.IsInGroup = ((int)dr["IsInGroup"] == 1);
        }

        public virtual List<UserInGroupResult> GetUserGroupAssignments(IConnectionManager entry, Guid userGuid)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT UserGroupName AS GroupName, 
                                           UserGroupGUID AS GroupGuid, 
                                           IsAdminGroup, 
                                           IsInGroup = 1 
                                    FROM dbo.vSECURITY_UsersInGroup_1_0
                                    WHERE GUID = @UserGUID AND DATAAREAID = @dataareaID
                                    
                                    UNION
                                    
                                    SELECT ug.Name AS GroupName, 
                                           ug.GUID AS GroupGuid, 
                                           ug.IsAdminGroup, 
                                           IsInGroup = 0 
                                    FROM USERGROUPS ug
                                    WHERE ug.Deleted = 0 AND ug.DATAAREAID = @dataareaID AND NOT EXISTS(
	                                    SELECT 1 FROM dbo.vSECURITY_UsersInGroup_1_0 
	                                    WHERE ug.GUID = UserGroupGUID AND GUID = @UserGUID AND DATAAREAID = @dataareaID)
                                    ORDER BY GroupName";

                MakeParam(cmd, "UserGUID", userGuid, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);

                return Execute<UserInGroupResult>(entry, cmd, CommandType.Text, PopulateUserInGroup);
            }
        }

        public virtual List<UserGroup> AllGroups(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {

                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT GUID, NAME, ISADMINGROUP, LOCKED FROM vSECURITY_AllUserGroups_1_0 WHERE DATAAREAID = @dataAreaID ORDER BY Name";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<UserGroup>(entry, cmd, CommandType.Text, PopulateUserGroup);
            }
        }

        public virtual UserGroup Get(IConnectionManager entry, RecordIdentifier groupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT GUID, NAME, ISADMINGROUP, LOCKED FROM USERGROUPS WHERE GUID = @groupID AND DATAAREAID = @dataAreaID";
                MakeParam(cmd, "groupID", groupID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Get<UserGroup>(entry, cmd, groupID, PopulateUserGroup, CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        public virtual List<UserGroup> GetGroupsForUser(IConnectionManager entry, RecordIdentifier userID)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ug.GUID, ug.NAME, ug.ISADMINGROUP, ug.LOCKED FROM USERGROUPS ug 
                                    LEFT OUTER JOIN USERSINGROUP uig ON ug.GUID = uig.UserGroupGUID
                                    LEFT OUTER JOIN USERS u ON u.GUID = uig.UserGUID AND u.DATAAREAID = uig.DATAAREAID 
                                    WHERE u.GUID = @userID AND uig.DATAAREAID = @dataAreaID";

                MakeParam(cmd, "userID", userID.ToString());
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<UserGroup>(entry, cmd, CommandType.Text, PopulateUserGroup);
            }
        }

        public virtual void AddUser(IConnectionManager entry, Guid userGuid, Guid groupGuid)
        {
            ValidateSecurity(entry, Permission.SecurityAssignUserToGroup);

            var statement = new SqlServerStatement("USERSINGROUP", StatementType.Insert);

            statement.AddKey("GUID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("UserGUID", userGuid, SqlDbType.UniqueIdentifier);
            statement.AddField("UserGroupGUID", groupGuid, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);

            SqlConnector.DataProviders.UserData.InvalidateUserProfile(entry, userGuid);
        }

        public virtual void RemoveUser(IConnectionManager entry, Guid userGuid, Guid groupGuid)
        {
            ValidateSecurity(entry, Permission.SecurityAssignUserToGroup);

            var statement = new SqlServerStatement("USERSINGROUP", StatementType.Delete);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("UserGUID", userGuid, SqlDbType.UniqueIdentifier);
            statement.AddCondition("UserGroupGUID", groupGuid, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);

            SqlConnector.DataProviders.UserData.InvalidateUserProfile(entry, userGuid);
        }

        public virtual void Delete(IConnectionManager entry, Guid groupGuid)
        {
            ValidateSecurity(entry, Permission.SecurityDeleteUserGroups);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT GUID FROM vSECURITY_UsersInGroup_1_0 WHERE UserGroupGUID = @groupGuid AND DATAAREAID = @dataareaID";
                MakeParam(cmd, "groupGuid", groupGuid);
                MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);

                IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
                List<Guid> userGuids = new List<Guid>();

                while(dr.Read())
                {
                    userGuids.Add(AsGuid(dr["GUID"]));
                }

                dr.Close();
                dr.Dispose();

                foreach(Guid userGuid in userGuids)
                {
                    RemoveUser(entry, userGuid, groupGuid);
                }

                MarkAsDeleted(entry, "USERGROUPS", "GUID", groupGuid, Permission.SecurityDeleteUserGroups);
            }
        }

        public virtual void New(IConnectionManager entry, UserGroup group)
        {
            ValidateSecurity(entry, Permission.SecurityCreateUserGroups);

            if(!Exists(entry, (Guid)group.ID))
            {
                var statement = new SqlServerStatement("USERGROUPS", StatementType.Insert);
                statement.AddKey("GUID", (Guid)group.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("Name", group.Name);
                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual void Edit(IConnectionManager entry, UserGroup group)
        {
            ValidateSecurity(entry, Permission.SecurityEditUserGroups);

            if (group.Locked || group.IsAdminGroup)
            {
                return;
            }

            var statement = new SqlServerStatement("USERGROUPS", StatementType.Update);
            statement.AddField("Name", group.Name);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("GUID", (Guid)group.ID, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SetPermission(IConnectionManager entry, Guid groupGuid, Guid permissionGuid, GroupGrantMode mode)
        {
            ValidateSecurity(entry, Permission.SecurityGrantPermissions);

            try
            {
                if (!entry.HasPermission(Permission.SecurityGrantHigherPermissions))
                {
                    // We need to check if the permission that we are setting is higher than our own
                    if (!entry.HasPermission(Providers.UserData.GetPermissionCode(entry, permissionGuid)))
                    {
                        // This permission is above our rights
                        throw new PermissionException(Permission.SecurityGrantHigherPermissions);
                    }
                }

                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = mode == GroupGrantMode.Grant ? "spSECURITY_GrantGroupPermission_1_0" : "spSECURITY_DenyGroupPermission_1_0";

                    MakeParam(cmd, "GroupGUID", groupGuid);
                    MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "PermissionGUID", permissionGuid);

                    entry.Connection.ExecuteNonQuery(cmd, false);
                }
            }
            catch (Exception){ }
        }

        public virtual List<PermissionsAssignmentResult> GetPermissions(IConnectionManager entry, Guid userGroupID, string searchText)
        {
            return Providers.PermissionData.GetPermissionsForGroup(entry, userGroupID, searchText);
        }

        public virtual bool Exists(IConnectionManager entry, Guid groupGuid)
        {
            return RecordExists<UserGroup>(entry, "USERGROUPS", "GUID", groupGuid);
        }
    }
}
