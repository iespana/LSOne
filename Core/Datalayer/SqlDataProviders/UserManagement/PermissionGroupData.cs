using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class PermissionGroupData : SqlServerDataProviderBase, IPermissionGroupData
    {
        private static void PopulatePermissionGroup(IDataReader dr, DataEntity entity)
        {
            entity.ID = (Guid)dr["GUID"];
            entity.Text = (string)dr["Name"];
        }

        public virtual DataEntity GetByPermission(IConnectionManager entry, RecordIdentifier permissionGuid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT G.GUID, G.NAME FROM PERMISSIONGROUP AS G 
                                    JOIN PERMISSIONS PER ON G.DATAAREAID = PER.DATAAREAID AND PER.PermissionGroupGUID = G.GUID 
                                    WHERE G.DATAAREAID = @dataAreaID AND PER.GUID = @permissionGuid";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "permissionGuid", (Guid)permissionGuid, SqlDbType.UniqueIdentifier);
                return Get<DataEntity>(entry, cmd, permissionGuid, PopulatePermissionGroup, CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }
    }
}
