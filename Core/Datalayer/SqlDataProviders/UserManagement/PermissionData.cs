using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class PermissionData : SqlServerDataProviderBase, IPermissionData
    {
        private static void PopulatePermissions(IDataReader dr, PermissionsAssignmentResult result)
        {
            string permissionCode;

            if ((bool)dr["CodeIsEncrypted"])
            {
                //TODO Handle this properly
                permissionCode = (string)dr["PermissionCode"];
            }
            else
            {
                permissionCode = (string)dr["PermissionCode"];
            }

            result.PermissionGuid = (Guid)dr["GUID"];
            result.Description = (string)dr["Description"];
            result.HasPermission = (PermissionState)dr["HasPermission"];
            result.PermissionGroupName = (string)dr["PermissionGroupName"];
            result.PermissionCode = permissionCode;
        }

        public virtual List<PermissionsAssignmentResult> GetPermissionsForGroup(IConnectionManager entry, Guid groupGuid, string searchText)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand("spSECURITY_GetGroupPermissions_1_0"))
            {
                MakeParam(cmd, "groupGuid", groupGuid, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "baseLanguage",
                          Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", Thread.CurrentThread.CurrentUICulture.Name);
                MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);

                var allPermissions = Execute<PermissionsAssignmentResult>(entry, cmd, CommandType.StoredProcedure,
                                                                          PopulatePermissions);

                var filteredPermissions = 
                    allPermissions.Where(
                        x => (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(x.Description, searchText, CompareOptions.IgnoreCase) >= 0));

                return filteredPermissions.ToList();
            }
        }

        public virtual List<PermissionsAssignmentResult> GetPermissionsForUser(IConnectionManager entry, Guid userGuid, string searchText)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand("spSECURITY_GetUserPermissions_1_0"))
            {
                MakeParam(cmd, "userGuid", userGuid, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "baseLanguage",
                          Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
                MakeParam(cmd, "language", Thread.CurrentThread.CurrentUICulture.Name);
                MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);

                var allPermissions = Execute<PermissionsAssignmentResult>(entry, cmd, CommandType.StoredProcedure,
                                                                          PopulatePermissions);
  
                var filteredPermissions =
                    allPermissions.Where(
                        x => (CultureInfo.CurrentUICulture.CompareInfo.IndexOf(x.Description, searchText, CompareOptions.IgnoreCase) >= 0));

                return filteredPermissions.ToList();
            }
        }
    }
}
