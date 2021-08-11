using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class ActionPermissionData : SqlServerDataProviderBase, IActionPermissionData
    {
        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select OPERATIONID, ISNULL(OPERATIONNAME,'') as OPERATIONNAME " +
                                  "from POSISOPERATIONS " +
                                  "where DATAAREAID = @dataAreaId and UserOperation = @userOperation " +
                                  "order by " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "userOperation", 1, SqlDbType.Int);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "OPERATIONNAME", "OPERATIONID");
            }
        }

        private static void PopulateAction(IDataReader dr, ActionPermission action)
        {
            action.ID = (int)dr["OPERATIONID"];
            action.Text = (string)dr["OPERATIONNAME"];

            action.CheckUserAcccess = (bool)dr["CHECKUSERACCESS"];

            try
            {
                action.AccessRights = (ActionPermission.Access)dr["PERMISSIONID"];
            }
            catch(Exception)
            {
                action.AccessRights = ActionPermission.Access.Everyone;
            }
        }

        public virtual ActionPermission Get(IConnectionManager entry, RecordIdentifier id,CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select OPERATIONID, ISNULL(OPERATIONNAME,'') as OPERATIONNAME,ISNULL(PERMISSIONID,0) as PERMISSIONID,CHECKUSERACCESS " +
                    "from POSISOPERATIONS " +
                    "where DATAAREAID = @dataAreaId and OPERATIONID = @operationID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "operationID", (int)id, SqlDbType.Int);

                return Get<ActionPermission>(entry, cmd, id, PopulateAction, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual void Save(IConnectionManager entry, ActionPermission action)
        {
            var statement = new SqlServerStatement("POSISOPERATIONS");

            ValidateSecurity(entry, BusinessObjects.Permission.EditActionPermissions);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("OPERATIONID", (int)action.ID,SqlDbType.Int);

            statement.AddField("PERMISSIONID", (int)action.AccessRights,SqlDbType.Int);
            statement.AddField("PERMISSIONID2", (int)action.AccessRights, SqlDbType.Int);

            Save(entry, action, statement);
        }
    }
}
