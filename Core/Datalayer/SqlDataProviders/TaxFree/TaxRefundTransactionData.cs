using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TaxFree
{
    public class TaxRefundTransactionData : SqlServerDataProviderBase, ITaxRefundTransactionData
    {
        private string BaseSql
        {
            get
            {
                return "SELECT t.ID, " +
                       "ISNULL(t.TAXREFUNDDATAID, '00000000-0000-0000-0000-000000000000') AS TAXREFUNDDATAID, " +
                       "ISNULL(t.STOREID, '') AS STOREID, " +
                       "ISNULL(t.TERMINALID, '') AS TERMINALID, " +
                       "ISNULL(t.TRANSACTIONID, '') AS TRANSACTIONID, " +
                       "ISNULL(t.TOTAL, 0) AS TOTAL, " +
                       "ISNULL(t.TAX, 0) AS TAX, " +
                       "ISNULL(t.TAXREFUND, 0) AS TAXREFUND " +
                       "FROM TAXREFUNDTRANSACTION t ";
            }
        }

        protected virtual void Populate(IDataReader dr, TaxRefundTransaction item)
        {
            item.ID = (Guid) dr["ID"];
            item.TaxRefundID = (Guid) dr["TAXREFUNDDATAID"];
            item.TerminalID = (string) dr["TERMINALID"];
            item.StoreID = (string) dr["STOREID"];
            item.TransactionID = (string) dr["TRANSACTIONID"];
            item.Total = (decimal) dr["TOTAL"];
            item.TotalTax = (decimal) dr["TAX"];
            item.TotalTaxRefund = (decimal) dr["TAXREFUND"];
        }

        public virtual TaxRefundTransaction Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + "WHERE t.ID = @id";
                MakeParam(cmd, "id", ID.DBValue, ID.DBType);
                return Get<TaxRefundTransaction>(entry, cmd, ID, Populate, cacheType, UsageIntentEnum.Normal);
            }
        }
        public virtual TaxRefundTransaction GetForTaxRefundID(IConnectionManager entry, RecordIdentifier taxRefundID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + "WHERE t.TAXREFUNDDATAID = @id";
                MakeParam(cmd, "id", taxRefundID.DBValue, taxRefundID.DBType);
                return Get<TaxRefundTransaction>(entry, cmd, taxRefundID, Populate, cacheType, UsageIntentEnum.Normal);
            }
        }
        public virtual TaxRefundTransaction GetForTransactionID(IConnectionManager entry, RecordIdentifier transactionID, bool includeCanceled, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql;
                if (!includeCanceled)
                {
                    cmd.CommandText += "INNER JOIN TAXREFUND TR ON TR.ID = t.TAXREFUNDDATAID AND TR.STATUS = 0 ";
                } 
                cmd.CommandText += "WHERE t.TRANSACTIONID = @id";
                MakeParam(cmd, "id", transactionID.DBValue, transactionID.DBType);
                
                return Get<TaxRefundTransaction>(entry, cmd, transactionID, Populate, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual void Save(IConnectionManager entry, TaxRefundTransaction item)
        {
            ValidateSecurity(entry);
            var statement = entry.Connection.CreateStatement("TAXREFUNDTRANSACTION");
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

            statement.AddField("TAXREFUNDDATAID", item.TaxRefundID.DBValue, SqlDbType.UniqueIdentifier);
            statement.AddField("TERMINALID", (string)item.TerminalID);
            statement.AddField("STOREID", (string)item.StoreID);
            statement.AddField("TRANSACTIONID", (string)item.TransactionID);
            statement.AddField("TOTAL", item.Total, SqlDbType.Decimal);
            statement.AddField("TAX", item.TotalTax, SqlDbType.Decimal);
            statement.AddField("TAXREFUND", item.TotalTaxRefund, SqlDbType.Decimal);

            Save(entry, item, statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry);

            entry.Cache.DeleteEntityFromCache(typeof(TaxRefundTransaction), ID);

            var statement = entry.Connection.CreateStatement("TAXREFUNDTRANSACTION", StatementType.Delete);

            statement.AddCondition("ID", ID.DBValue, ID.DBType);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID FROM TAXREFUNDTRANSACTION WHERE ID = @id";

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

        public virtual bool ExistsForTransactionID(IConnectionManager entry, RecordIdentifier transactionID, bool includeCanceledRefunds)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select t.TRANSACTIONID FROM TAXREFUNDTRANSACTION t ";
                if (!includeCanceledRefunds)
                {
                    cmd.CommandText += "INNER JOIN TAXREFUND TR ON TR.ID = t.TAXREFUNDDATAID AND TR.STATUS = 0 ";
                }

                cmd.CommandText += "WHERE t.TRANSACTIONID = @id";

                MakeParam(cmd, "id", transactionID.DBValue, transactionID.DBType);
                
                using (var dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                    return dr.Read();
            }
        }
    }
}
