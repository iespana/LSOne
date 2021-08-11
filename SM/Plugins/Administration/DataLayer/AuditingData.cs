using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;
using LSOne.ViewPlugins.Administration.QueryResults;

namespace LSOne.ViewPlugins.Administration.DataLayer
{
    internal class AuditingData : SqlServerDataProviderBase, IAuditingData
    {
        public DeleteAuditLogsResult DeleteAuditLogs(IConnectionManager entry, int commandTimeout, DateTime toDate)
        {
            SqlCommand cmd;

            ValidateSecurity(entry, Permission.SecurityManageAuditLogs);

            cmd = new SqlCommand("spSECURITY_DeleteAuditLogs_1_0");

            cmd.CommandTimeout = commandTimeout;

            MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
            MakeParam(cmd, "toDate", toDate,SqlDbType.DateTime);

            try
            {
                entry.Connection.ExecuteNonQuery(cmd,false);
            }
            catch(DatabaseException ex) when (ex.InnerException is SqlException && (ex.InnerException as SqlException).Number == -2)
            {
                return DeleteAuditLogsResult.TimeoutException;
            }
            catch
            {
                return DeleteAuditLogsResult.UnknownException;
            }

            return DeleteAuditLogsResult.Success;
        }

        public AuditLogResult GetAuditLog(IConnectionManager entry, string logName, RecordIdentifier contextID,string userName,DateTime from,DateTime to)
        {
            SqlCommand cmd;
            IDataReader dr = null;
            List<string> columnNames = new List<string>();
            List<int> columnIndex = new List<int>();
            List<AuditLogRecord> records = new List<AuditLogRecord>();
            AuditLogRecord record;
            string name;
            ArrayList recordFields;
            bool hasSpecialOperationColumn = false;

            ValidateSecurity(entry, Permission.SecurityViewAuditLogs);

            if (contextID.HasSecondaryID)
            {
                // What if we have 3 ID's ?
                if (contextID.SecondaryID.HasSecondaryID)
                {
                    cmd = new SqlCommand("spAuditing_ReadLogsIV_1_0");
                    MakeParam(cmd, "cmd", "spAUDIT_ViewLog_" + logName);
                    MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "context", contextID.ToString());
                    MakeParam(cmd, "context2", contextID.SecondaryID.ToString());
                    MakeParam(cmd, "context3", contextID.SecondaryID.SecondaryID.ToString());
                }
                else
                {
                    cmd = new SqlCommand("spAuditing_ReadLogsIII_1_0");
                    MakeParam(cmd, "cmd", "spAUDIT_ViewLog_" + logName);
                    MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "context", contextID.ToString());
                    MakeParam(cmd, "context2", contextID.SecondaryID.ToString());
                }
            }
            else
            {
                if (contextID.IsGuid)
                {
                    cmd = new SqlCommand("spAuditing_ReadLogs_1_0");
                    MakeParam(cmd, "cmd", "spAUDIT_ViewLog_" + logName);
                    MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "context", (Guid)contextID);
                }
                else
                {
                    cmd = new SqlCommand("spAuditing_ReadLogsII_1_0");
                    MakeParam(cmd, "cmd", "spAUDIT_ViewLog_" + logName);
                    MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "context", contextID.ToString());
                }
            }
            
            MakeParam(cmd, "user", userName);
            MakeParam(cmd, "from", from, SqlDbType.DateTime);
            MakeParam(cmd, "to", to, SqlDbType.DateTime);

            try
            {
                dr = entry.Connection.ExecuteReader(cmd);

                columnNames = new List<string>();
                columnIndex = new List<int>();
                records = new List<AuditLogRecord>();

                if (dr != null)
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        name = dr.GetName(i);

                        if (name == "AuditSpecialOperation")
                        {
                            hasSpecialOperationColumn = true;
                        }
                        else
                        {
                            if (name != "AuditID" && name != "AuditUserGUID" &&
                               name != "AuditUserLogin" && name != "AuditDate" && name != "AuditUserLogin2")
                            {
                                columnIndex.Add(i);
                                columnNames.Add(name);
                            }
                        }
                    }
                }

                while (dr.Read())
                {
                    recordFields = new ArrayList();

                    foreach (int index in columnIndex)
                    {
                        if (dr[index] is DBNull)
                        {
                            if (hasSpecialOperationColumn)
                            {
                                if ((bool)dr["AuditSpecialOperation"])
                                {
                                    recordFields.Add("<Not touched>");
                                }
                                else
                                {
                                    recordFields.Add("");  // No value set
                                }
                            }
                            else
                            {
                                recordFields.Add(""); // No value set
                            }
                        }
                        else
                        {
                            recordFields.Add(dr[index]);
                        }
                    }

                    record = new AuditLogRecord(
                        (int)dr["AuditID"],
                        (Guid)dr["AuditUserGUID"],
                        (dr["AuditUserLogin"] is DBNull || (string)dr["AuditUserLogin"] == "") ? (string)dr["AuditUserLogin2"] : (string)dr["AuditUserLogin"],
                        (DateTime)dr["AuditDate"],
                        recordFields);

                    records.Add(record);
                }
            }
            catch
            {
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                }
            }

            return new AuditLogResult(columnNames, records);
        }
    }
}