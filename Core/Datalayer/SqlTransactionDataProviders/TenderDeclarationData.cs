using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class TenderDeclarationData : SqlServerDataProviderBase, ITenderDeclarationData
    {
        public virtual decimal GetLastTenderedAmount(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeId = null)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT TOP 1 ISNULL(T.AMOUNTTENDERED, 0) AS AMOUNTTENDERED " +
                                  "FROM RBOTRANSACTIONTENDERDECLA20165 T " +
                                  "INNER JOIN RBOTRANSACTIONTABLE R ON T.TRANSACTIONID = R.TRANSACTIONID AND T.TERMINAL = R.TERMINAL AND T.STORE = R.STORE ";
                if (tenderTypeId != null)
                {
                    cmd.CommandText += "WHERE T.TENDERTYPE = @tenderType ";
                    MakeParam(cmd, "tenderType", tenderTypeId.DBValue, tenderTypeId.DBType);
                }
                cmd.CommandText += "ORDER BY R.CREATEDDATE DESC ";

                object result = entry.Connection.ExecuteScalar(cmd);
                return result != null ? (decimal) result : 0;
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM" };
            return RecordExists(entry, "RBOTRANSACTIONTENDERDECLA20165", fields, id);
        }

        public virtual void Delete(IConnectionManager entry, TenderLineItem item)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM" };
            var id = new RecordIdentifier(item.Transaction.TransactionId, (decimal)item.LineId);
            DeleteRecord(entry, "RBOTRANSACTIONTENDERDECLA20165", fields, id, "");
        }

        public virtual void Save(IConnectionManager entry, TenderDeclarationTransaction transaction, TenderLineItem tenderItem)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONTENDERDECLA20165");

            if (Exists(entry, new RecordIdentifier(transaction.TransactionId, tenderItem.LineId)))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", transaction.TransactionId);
                statement.AddCondition("LINENUM", (decimal)tenderItem.LineId, SqlDbType.Decimal);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("TRANSACTIONID", transaction.TransactionId);
                statement.AddKey("LINENUM", (decimal)tenderItem.LineId, SqlDbType.Decimal);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            statement.AddField("RECEIPTID", transaction.ReceiptId);
            statement.AddField("POSCURRENCY", transaction.StoreCurrencyCode);
            statement.AddField("TENDERTYPE", tenderItem.TenderTypeId);
            statement.AddField("AMOUNTCUR", tenderItem.Amount, SqlDbType.Decimal);
            statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
            statement.AddField("EXCHRATEMST", tenderItem.ExchrateMST, SqlDbType.Decimal);
            statement.AddField("CURRENCY", tenderItem.CurrencyCode);

            if (tenderItem.ForeignCurrencyAmount == 0)
            {
                statement.AddField("EXCHRATE", transaction.StoreExchangeRate * 100, SqlDbType.Decimal);
                statement.AddField("AMOUNTTENDERED", tenderItem.Amount, SqlDbType.Decimal);
            }
            else
            {
                statement.AddField("EXCHRATE", tenderItem.ExchangeRate, SqlDbType.Decimal);
                statement.AddField("AMOUNTTENDERED", tenderItem.ForeignCurrencyAmount, SqlDbType.Decimal);
            }

            statement.AddField("TRANSDATE", tenderItem.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", (tenderItem.BeginDateTime.Hour * 3600) + (tenderItem.BeginDateTime.Minute * 60) + tenderItem.BeginDateTime.Second, SqlDbType.Int); 
            statement.AddField("SHIFT", transaction.ShiftId);
            statement.AddField("SHIFTDATE", transaction.ShiftDate, SqlDbType.DateTime);
            statement.AddField("STAFF", (string)transaction.Cashier.ID);
            statement.AddField("STORE", transaction.StoreId);
            statement.AddField("TERMINAL", transaction.TerminalId);
            statement.AddField("TRANSACTIONSTATUS", tenderItem.Voided ? 1 : 0, SqlDbType.Int);
            statement.AddField("STATEMENTID", "");
            statement.AddField("QTY", 1, SqlDbType.Decimal);
            statement.AddField("EXPECTEDAMOUNT", tenderItem.ExpectedTenderDeclarationAmount, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
