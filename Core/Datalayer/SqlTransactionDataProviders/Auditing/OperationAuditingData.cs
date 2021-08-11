using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders.Auditing;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders.Auditing
{
    public class OperationAuditingData : SqlServerDataProviderBase, IOperationAuditingData
    {
        private static string BaseSql
        {
            get 
            {
                return @"SELECT UID,
                            TRANSACTIONID, 
                            ISNULL(LINENUM, 0) AS LINENUM, 
                            STORE, 
                            TERMINALID, 
                            USERID, 
                            ISNULL(MANAGERID, '') AS MANAGERID, 
                            OPERATIONID, 
                            CREATEDDATE
                            FROM RBOTRANSACTIONOPERATIONAUDIT "; }
        }

        protected virtual void PopulateOperationAuditingMinimal(IDataReader dr, OperationAuditing auditItem)
        {
            auditItem.UniqueID = AsGuid(dr["UID"]);
            auditItem.TransactionID = AsString(dr["TRANSACTIONID"]);
            auditItem.LineNum = AsInt(dr["LINENUM"]);
            auditItem.StoreID = AsString(dr["STORE"]);
            auditItem.TerminalID = AsString(dr["TERMINALID"]);
            auditItem.UserID = AsString(dr["USERID"]);
            auditItem.ManagerID = AsString(dr["MANAGERID"]);
            auditItem.OperationID = (POSOperations)AsInt(dr["OPERATIONID"]);
            auditItem.CreatedDate = AsDateTime(dr["CREATEDDATE"]);
        }

        protected virtual void PopulateOperationAuditing(IDataReader dr, OperationAuditing auditItem)
        {
            PopulateOperationAuditingMinimal(dr, auditItem);
            auditItem.OperationName = (string)dr["OPERATIONNAME"];
        }

        public virtual List<OperationAuditing> GetList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + @" WHERE DATAAREAID = @dataAreaID ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                if (storeID == null)
                {
                    cmd.CommandText += "AND STORE = @storeID ";
                    MakeParam(cmd, "storeID", storeID);
                }

                return Execute<OperationAuditing>(entry, cmd, CommandType.Text, PopulateOperationAuditingMinimal);
            }
        }

        public virtual List<OperationAuditing> Search(IConnectionManager entry, 
                                              RecordIdentifier store, 
                                              RecordIdentifier terminal, 
                                              RecordIdentifier operatorID, 
                                              DateTime fromDate, 
                                              DateTime toDate, 
                                              List<RecordIdentifier> operations, 
                                              int recordFrom, 
                                              int recordTo)
        {
            bool whereAdded = false;
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM(" +
                                  "SELECT UID, " +
                                  "TRANSACTIONID, " +
                                  "LINENUM, " +
                                  "STORE, " +
                                  "TERMINALID, " +
                                  "USERID, " +
                                  "MANAGERID, " +
                                  "OPERATIONID, " +
                                  "CREATEDDATE, " +
                                  "OPERATIONNAME, " +
                                  "ROW_NUMBER() over(order by CREATEDDATE DESC) as rownum FROM (" +
                                        "SELECT TOP 2147483647 RTA.UID, " +
                                        "RTA.TRANSACTIONID, " +
                                        "ISNULL(RTA.LINENUM, 0) AS LINENUM, " +
                                        "RTA.STORE, " +
                                        "RTA.TERMINALID, " +
                                        "RTA.USERID, " +
                                        "ISNULL(RTA.MANAGERID, '') AS MANAGERID, " +
                                        "RTA.OPERATIONID, " +
                                        "RTA.CREATEDDATE," +
                                        "ISNULL(PO.OPERATIONNAME, '') AS OPERATIONNAME " +
                                        "FROM RBOTRANSACTIONOPERATIONAUDIT RTA " +
                                        "LEFT OUTER JOIN POSISOPERATIONS PO ON PO.OPERATIONID = RTA.OPERATIONID ";
                if (store != null && !store.IsEmpty)
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.STORE = @store ";
                    MakeParam(cmd, "store", store);
                }

                if (terminal != null && !terminal.IsEmpty)
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.TERMINALID = @terminalID ";
                    MakeParam(cmd, "terminalID", terminal);
                }

                if (operatorID != null && !operatorID.IsEmpty)
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.USERID = @operatorID ";
                    MakeParam(cmd, "operatorID", operatorID);
                }

                if (fromDate != default(DateTime))
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.CREATEDDATE > @fromDate ";
                    MakeParam(cmd, "fromDate", fromDate, SqlDbType.DateTime);
                }

                if (toDate != default(DateTime))
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.CREATEDDATE < @toDate ";
                    MakeParam(cmd, "toDate", toDate, SqlDbType.DateTime);
                }

                if (operations != null && operations.Count > 0)
                {
                    cmd.CommandText += whereAdded ? "AND " : "WHERE ";
                    whereAdded = true;
                    cmd.CommandText += "RTA.OPERATIONID IN (";
                    for (int i = 0; i < operations.Count; i++)
                    {
                        cmd.CommandText += (string) operations[i];
                        if (i + 1 < operations.Count)
                        {
                            cmd.CommandText += ", ";
                        }
                    }
                    cmd.CommandText += ") ";
                }

                cmd.CommandText += ") AS QUERY) AS QUERY2 WHERE rownum BETWEEN @recordFrom AND @recordTo ORDER BY rownum ";
                MakeParam(cmd, "recordFrom", recordFrom, SqlDbType.Int);
                MakeParam(cmd, "recordTo", recordTo, SqlDbType.Int);
                return Execute<OperationAuditing>(entry, cmd, CommandType.Text, PopulateOperationAuditing);
            }
        }

        public virtual bool Exists(IConnectionManager entry, OperationAuditing item)
        {
            string[] fields = { "UID"};
            var id = new RecordIdentifier(item.UniqueID);
            return RecordExists(entry, "RBOTRANSACTIONOPERATIONAUDIT", fields, id);
        }

        public virtual void Save(IConnectionManager entry, OperationAuditing item)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONOPERATIONAUDIT");

            if (Guid.Empty != item.UniqueID && Exists(entry, item))
            {
                statement.AddCondition("UID", item.UniqueID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                if (Guid.Empty == item.UniqueID)
                    item.UniqueID = Guid.NewGuid();
                statement.AddKey("UID", item.UniqueID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("TRANSACTIONID", (string)item.TransactionID);
            statement.AddField("LINENUM", item.LineNum, SqlDbType.Int);
            statement.AddField("STORE", (string)item.StoreID);
            statement.AddField("TERMINALID", (string)item.TerminalID);
            statement.AddField("USERID", (string)item.UserID);
            statement.AddField("MANAGERID", item.ManagerID == null ? "" : (string)item.ManagerID);
            statement.AddField("OPERATIONID", (int)item.OperationID, SqlDbType.Int);
            statement.AddField("CREATEDDATE", item.CreatedDate, SqlDbType.DateTime);
            statement.AddField("DATAAREAID", entry.Connection.DataAreaId);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
