using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class SafeDropTransactionData : SqlServerDataProviderBase, ISafeDropTransactionData
    {
        public virtual void Insert(IConnectionManager entry, TenderLineItem tenderItem, SafeDropTransaction transaction)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONSAFETENDERTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)tenderItem.LineId, SqlDbType.Decimal);
            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId); 

            statement.AddField("RECEIPTID", "");
            statement.AddField("TENDERTYPE", tenderItem.TenderTypeId);
            statement.AddField("STATUSTYPE", 1, SqlDbType.Int); //Always to to 1 by the POS  
            statement.AddField("EXCHRATEMST", tenderItem.ExchrateMST, SqlDbType.Decimal);
            statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
            statement.AddField("AMOUNTMSTPOS", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
            statement.AddField("AMOUNTCUR", tenderItem.ForeignCurrencyAmount, SqlDbType.Decimal);
            statement.AddField("AMOUNTCURPOS", tenderItem.ForeignCurrencyAmount, SqlDbType.Decimal);
            statement.AddField("AMOUNTTENDERED", tenderItem.Amount, SqlDbType.Decimal);
            statement.AddField("AMOUNTTENDEREDPOS", tenderItem.Amount, SqlDbType.Decimal);

            if (tenderItem.ForeignCurrencyAmount == 0)
            {
                statement.AddField("CURRENCY", transaction.StoreCurrencyCode);
                statement.AddField("EXCHRATE", transaction.StoreExchangeRate * 100, SqlDbType.Decimal);
            }
            else  //then we are having foreign currency.....
            {
                statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                statement.AddField("EXCHRATE", tenderItem.ExchangeRate, SqlDbType.Decimal);
            }

            statement.AddField("TRANSDATE", tenderItem.BeginDateTime, SqlDbType.Date);
            statement.AddField("TRANSTIME", Conversion.TimeToInt(tenderItem.BeginDateTime), SqlDbType.Int);

            if (string.IsNullOrEmpty(transaction.ShiftId))
            {
                statement.AddField("SHIFT", "");
                statement.AddField("SHIFTDATE", 0, SqlDbType.Int);
            }
            else
            {
                statement.AddField("SHIFT", transaction.ShiftId);
                statement.AddField("SHIFTDATE", tenderItem.Transaction.ShiftDate, SqlDbType.DateTime);
            }

            statement.AddField("STAFF", (string)transaction.Cashier.ID);

            statement.AddField("TRANSACTIONSTATUS", tenderItem.Voided ? (int)TransactionStatus.Voided : (int)TransactionStatus.Normal, SqlDbType.Int);
            statement.AddField("STATEMENTID", "");
            statement.AddField("REPLICATED", (byte)0, SqlDbType.TinyInt);
            statement.AddField("QTY", (decimal)1, SqlDbType.Decimal);
            statement.AddField("ZREPORTID", "");
            statement.AddField("STATEMENTCODE", "");

            entry.Connection.ExecuteStatement(statement);

            TransactionProviders.TenderTransactionData.Insert(entry, tenderItem, transaction);
        }
    }
}
