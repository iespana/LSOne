using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class RemoveTenderTransactionData : SqlServerDataProviderBase, IRemoveTenderTransactionData
    {
        public virtual void Insert(IConnectionManager entry, TenderLineItem tenderItem, PosTransaction transaction)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONPAYMENTTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)tenderItem.LineId, SqlDbType.Decimal);
            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);          

            statement.AddField("RECEIPTID", transaction.ReceiptId);
            statement.AddField("STATEMENTCODE", "");

            if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
            {
                statement.AddField("EXCHRATE", 100.0M, SqlDbType.Decimal);
            }
            else
            {
                statement.AddField("EXCHRATE", tenderItem.ExchangeRate, SqlDbType.Decimal);
            }

            statement.AddField("EXCHRATEMST", tenderItem.ExchrateMST, SqlDbType.Decimal);
            statement.AddField("TENDERTYPE", tenderItem.TenderTypeId);
            statement.AddField("AMOUNTTENDERED", tenderItem.Amount, SqlDbType.Decimal);
            statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);

            if (tenderItem.ForeignCurrencyAmount == 0)
            {
                statement.AddField("AMOUNTCUR", tenderItem.Amount, SqlDbType.Decimal);
            }
            else
            {
                statement.AddField("AMOUNTCUR", tenderItem.ForeignCurrencyAmount, SqlDbType.Decimal);
            }

            statement.AddField("TRANSDATE", tenderItem.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", Conversion.TimeToInt(tenderItem.BeginDateTime), SqlDbType.Int);

            statement.AddField("SHIFT", transaction.ShiftId);

            if (string.IsNullOrEmpty(transaction.ShiftId))
            {
                statement.AddField("SHIFTDATE", 0, SqlDbType.Int);
            }
            else
            {
                statement.AddField("SHIFTDATE", transaction.ShiftDate, SqlDbType.DateTime);
            }

            statement.AddField("STAFF", (string)transaction.Cashier.ID);

            if (transaction is RemoveTenderTransaction)
            {
                statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
            }
            else if (transaction is FloatEntryTransaction)
            {
                statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
            }

            statement.AddField("TRANSACTIONSTATUS", tenderItem.Voided ? (int)TransactionStatus.Voided : (int)TransactionStatus.Normal, SqlDbType.Int);

            statement.AddField("STATEMENTID", "");
            statement.AddField("MANAGERKEYLIVE", (byte)0, SqlDbType.TinyInt);
            statement.AddField("CHANGELINE", (byte)0, SqlDbType.TinyInt);

            statement.AddField("COUNTER", tenderItem.LineId, SqlDbType.Int);
            statement.AddField("MESSAGENUM", 0, SqlDbType.Int);
            statement.AddField("REPLICATED", (byte)0, SqlDbType.TinyInt);
            statement.AddField("QTY", (decimal)1, SqlDbType.Decimal);
            statement.AddField("ZREPORTID", "");

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
