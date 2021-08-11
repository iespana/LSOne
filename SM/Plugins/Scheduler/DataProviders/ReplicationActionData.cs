using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    public class ReplicationActionData : SqlServerDataProvider, IReplicationActionData
    {
        private static void PopulateReplicationAction(IDataReader dr, ReplicationAction action)
        {
            action.ActionId = (int) dr["ACTIONID"];
            action.ActionType = (ActionType) ((int) dr["ACTION"]);
            action.ActionTarget = (string) dr["OBJECTNAME"];
            string parameters = (string) dr["PARAMETERS"];
            action.Parameters = parameters.Split('|').ToList();
            action.DateCreated = (DateTime) dr["DATECREATED"];

        }

        public List<ReplicationAction> Get(IConnectionManager entry, string objectName)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);


            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT  
	                                ACTIONID,
	                                ACTION,
	                                OBJECTNAME,
	                                PARAMETERS,
	                                DATECREATED
                                  FROM REPLICATIONACTIONS
                                  WHERE OBJECTNAME =  @objectName
                                    ORDER BY ACTIONID DESC";

                MakeParam(cmd, "objectName", objectName);

                return Execute<ReplicationAction>(entry, cmd, CommandType.Text,
                    PopulateReplicationAction);


            }
        }

        private List<ReplicationAction> GetOlder(IConnectionManager entry, string objectName, string tableName, RecordIdentifier actionID)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);


            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = string.Format(@"SELECT  
	                                ACTIONID,
	                                ACTION,
	                                OBJECTNAME,
	                                PARAMETERS,
	                                DATECREATED
                                  FROM {0}
                                  WHERE OBJECTNAME =  @objectName
                                    AND ACTIONID <= @actionId  
                                    ORDER BY ACTIONID DESC", tableName);

                MakeParam(cmd, "objectName", objectName);
                MakeParam(cmd, "actionId",(int) actionID,SqlDbType.Int);
                 

                return Execute<ReplicationAction>(entry, cmd, CommandType.Text,
                    PopulateReplicationAction);


            }
        }


        public List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);


            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = string.Format(@"SELECT  
	                                ACTIONID,
	                                ACTION,
	                                OBJECTNAME,
	                                PARAMETERS,
	                                DATECREATED
                                  FROM {0}
                                  WHERE OBJECTNAME =  @objectName
                                    ORDER BY ACTIONID DESC", tableName);

                MakeParam(cmd, "objectName", objectName);

                return Execute<ReplicationAction>(entry, cmd, CommandType.Text,
                    PopulateReplicationAction);


            }
        }


        public List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName,
            RecordIdentifier subjob)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobView);


            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = string.Format(@"SELECT  
	                                ACTIONID,
	                                ACTION,
	                                OBJECTNAME,
	                                PARAMETERS,
	                                DATECREATED
                                  FROM {0}
                                  WHERE 
                                    OBJECTNAME =  @objectName
                                    AND ACTIONID > (
                                        SELECT ISNULL(MIN(COUNTER),0)
                                        FROM JSCREPCOUNTERS 
                                        WHERE SUBJOB = @subJob
                                     )
                                    ORDER BY ACTIONID DESC", tableName);

                MakeParam(cmd, "objectName", objectName);
                MakeParam(cmd, "subJob", (Guid) subjob);

                return Execute<ReplicationAction>(entry, cmd, CommandType.Text,
                    PopulateReplicationAction);


            }
        }

        public void Delete(IConnectionManager entry,  RecordIdentifier actionCounter)
        {
            DeleteRecord(entry,"REPLICATIONACTIONS", "ACTIONID", actionCounter, SchedulerPermissions.JobSubjobEdit);
        }

        public void DeleteOlder(IConnectionManager entry, string objectName, string tableName,
            RecordIdentifier actionCounter)
        {
            List<ReplicationAction> older = GetOlder(entry, objectName, tableName, actionCounter);
            foreach (var replicationAction in older)
            {
                DeleteRecord(entry, "REPLICATIONACTIONS", "ACTIONID", replicationAction.ActionId, SchedulerPermissions.JobSubjobEdit);
            }
        }

    }
}
