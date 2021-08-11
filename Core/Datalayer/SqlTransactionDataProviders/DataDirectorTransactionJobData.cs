using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class DataDirectorTransactionJobData : SqlServerDataProviderBase, IDataDirectorTransactionJobData
    {
        protected void Populate(IDataReader dr, DataDirectorTransactionJob item)
        {
            item.ID = (Guid) dr["ID"];
            item.CreatedTime = (DateTime)dr["CREATEDDATE"];
            item.JobID = (Guid) dr["JOBID"];
            item.Parameters = (string) dr["PARAMETERS"];
            item.StoreID = (string)dr["STOREID"];
            item.TerminalID = (string)dr["TERMINALID"];
        }
        
        public virtual List<DataDirectorTransactionJob> GetPendingJobs(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ISNULL(ID, '00000000-0000-0000-0000-000000000000') AS ID, " +
                                  "ISNULL(CREATEDDATE, '1900-01-01') AS CREATEDDATE, " +
                                  "ISNULL(JOBID, '00000000-0000-0000-0000-000000000000') AS JOBID, " +
                                  "ISNULL(PARAMETERS,'') AS PARAMETERS, " +
                                  "ISNULL(STOREID, '') AS STOREID," +
                                  "ISNULL(TERMINALID, '') AS TERMINALID " +
                                  "FROM PENDINGDDJOBS";
                return Execute<DataDirectorTransactionJob>(entry, cmd, CommandType.Text, Populate);
            }
        }

        public virtual void Save(IConnectionManager entry, DataDirectorTransactionJob item)
        {
            ValidateSecurity(entry);
            var statement = entry.Connection.CreateStatement("PENDINGDDJOBS");
            bool isNew = false;
            if (item.ID == null || item.ID.IsEmpty)
            {
                isNew = true;
                item.ID = Guid.NewGuid();
            }
            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", item.ID.DBValue, item.ID.DBType);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", item.ID.DBValue, item.ID.DBType);
            }

            statement.AddField("CREATEDDATE", item.CreatedTime, SqlDbType.DateTime);
            statement.AddField("JOBID", item.JobID, SqlDbType.UniqueIdentifier);
            statement.AddField("PARAMETERS", item.Parameters);
            statement.AddField("STOREID", (string)item.StoreID);
            statement.AddField("TERMINALID", (string)item.TerminalID);

            Save(entry, item, statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry);

            entry.Cache.DeleteEntityFromCache(typeof(DataDirectorTransactionJob), ID);

            var statement = entry.Connection.CreateStatement("PENDINGDDJOBS", StatementType.Delete);

            statement.AddCondition("ID", ID.DBValue, ID.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT * FROM PENDINGDDJOBS WHERE ID = @id";

                MakeParam(cmd, "id", ID.DBValue, ID.DBType);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }
    }
}
