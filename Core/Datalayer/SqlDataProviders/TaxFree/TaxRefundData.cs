using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TaxFree
{
    public class TaxRefundData : SqlServerDataProviderBase, ITaxRefundData
    {
        private const string baseSQL = "SELECT ID, " +
                                       "ISNULL(STOREID, '') AS STOREID, " +
                                       "ISNULL(TERMINALID, '') AS TERMINALID, " +
                                       "ISNULL(CREATED, '1900-01-01') AS CREATED, " +
                                       "TOURISTID, " +
                                       "ISNULL(BOOKING, '') AS BOOKING, " +
                                       "ISNULL(RUNNING, '') AS RUNNING, " +
                                       "ISNULL(TOTAL, 0) AS TOTAL, " +
                                       "ISNULL(TAX, 0) AS TAX, " +
                                       "ISNULL(TAXREFUND, 0) AS TAXREFUND, " +
                                       "ISNULL(STATUS, 0) AS STATUS," +
                                       "ISNULL(COMMENT, '') AS COMMENT," +
                                       "ISNULL(STOREID, '') AS STOREID," +
                                       "ISNULL(TERMINALID, '') AS TERMINALID " +
                                       "FROM TAXREFUND ";

        protected virtual void Populate(IDataReader dr, TaxRefund item)
        {
            item.ID = AsGuid(dr["ID"]);
            item.StoreID = (string) dr["STOREID"];
            item.TerminalID = (string) dr["TERMINALID"];
            item.Created = AsDateTime(dr["CREATED"]);
            item.TouristID = AsGuid(dr["TOURISTID"]);
            item.Booking = AsString(dr["BOOKING"]);
            item.Running = AsString(dr["RUNNING"]);
            item.Total = (decimal) dr["TOTAL"];
            item.Tax = (decimal) dr["TAX"];
            item.TaxRefundValue = (decimal) dr["TAXREFUND"];
            item.Status = (TaxRefundStatus) (int) dr["STATUS"];
            item.Comment = (string) dr["COMMENT"];
        }

        public virtual List<TaxRefund> GetForTourist(IConnectionManager entry, RecordIdentifier id)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = baseSQL +
                                  "WHERE TOURISTID = @id";
                MakeParam(cmd, "id", id.DBValue, id.DBType);
                return Execute<TaxRefund>(entry, cmd, CommandType.Text, Populate);

                // TODO populate external classes
            }
        }

        public virtual void Save(IConnectionManager entry, TaxRefund item)
        {
            ValidateSecurity(entry);
            var statement = entry.Connection.CreateStatement("TAXREFUND");

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
            statement.AddField("STOREID", (string)item.StoreID);
            statement.AddField("TERMINALID", (string)item.TerminalID);
            statement.AddField("TOURISTID", item.TouristID.DBValue, item.TouristID.DBType);
            statement.AddField("CREATED", item.Created, SqlDbType.DateTime);
            statement.AddField("BOOKING", item.Booking);
            statement.AddField("RUNNING", item.Running);
            statement.AddField("TOTAL", item.Total, SqlDbType.Decimal);
            statement.AddField("TAX", item.Tax, SqlDbType.Decimal);
            statement.AddField("TAXREFUND", item.TaxRefundValue, SqlDbType.Decimal);
            statement.AddField("STATUS",(int)item.Status, SqlDbType.Int);
            statement.AddField("COMMENT", item.Comment);

            Save(entry, item, statement);

            if (item.Transactions != null)
            {
                foreach (TaxRefundTransaction taxRefundTransaction in item.Transactions)
                {
                    taxRefundTransaction.TaxRefundID = item.ID;
                    DataProviderFactory.Instance.Get<ITaxRefundTransactionData, TaxRefundTransaction>().Save(entry, taxRefundTransaction);
                }
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry);

            var statement = entry.Connection.CreateStatement("TAXREFUND", StatementType.Delete);

            statement.AddCondition("ID", ID.DBValue, ID.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID FROM TAXREFUND WHERE ID = @id";

                MakeParam(cmd, "id", ID.DBValue, ID.DBType);

                using (var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                    return dr.Read();
            }
        }

        public virtual bool RunningNumberExists(IConnectionManager entry, string bookingNumber, string runningNumber)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select BOOKING, RUNNING FROM TAXREFUND WHERE BOOKING = @booking AND RUNNING = @running";

                MakeParam(cmd, "booking", bookingNumber, SqlDbType.NVarChar);
                MakeParam(cmd, "running", runningNumber, SqlDbType.NVarChar);
                using (var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                    return dr.Read();
            }
        }

        public virtual TaxRefund GetFromRunningNumber(IConnectionManager entry, string bookingNumber, string runningNumber)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = baseSQL +
                                  "WHERE BOOKING = @booking AND RUNNING = @running";
                MakeParam(cmd, "booking", bookingNumber, SqlDbType.NVarChar);
                MakeParam(cmd, "running", runningNumber, SqlDbType.NVarChar);
                var result = Execute<TaxRefund>(entry, cmd, CommandType.Text, Populate);
                return (result.Count >= 1) ? result[0] : null;
            }
        }

        public virtual TaxRefund Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = baseSQL +
                                  "WHERE ID = @id";
                MakeParam(cmd, "id", id.DBValue, id.DBType);
                return Get<TaxRefund>(entry, cmd, id, Populate, CacheType.CacheTypeApplicationLifeTime, UsageIntentEnum.Normal);
            }
        }
    }
}
