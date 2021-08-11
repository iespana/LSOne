using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class PaymentTransactionData : SqlServerDataProviderBase, IPaymentTransactionData
    {
        private static string BaseSql
        {
            get
            {
                return @"SELECT tpt.TRANSACTIONID
                        ,tpt.LINENUM
                        ,ISNULL(tpt.RECEIPTID, '') AS RECEIPTID
                        ,ISNULL(tpt.STATEMENTCODE, '') AS STATEMENTCODE, 
                        ISNULL(tpt.CARDTYPEID, '') AS CARDTYPEID
                        ,ISNULL(tpt.EXCHRATE, 0) AS EXCHRATE
                        ,ISNULL(tpt.TENDERTYPE, 0) AS TENDERTYPE
                        ,ISNULL(tpt.AMOUNTTENDERED, 0) AS AMOUNTTENDERED
                        ,ISNULL(tpt.CURRENCY, '') AS CURRENCY
                        ,ISNULL(tpt.AMOUNTCUR, 0) AS AMOUNTCUR
                        ,ISNULL(tpt.CARDORACCOUNT, '') AS CARDORACCOUNT
                        ,tpt.TRANSDATE
                        ,ISNULL(tpt.SHIFT, '') AS SHIFT
                        ,tpt.SHIFTDATE
                        ,tpt.STAFF
                        ,tpt.STORE
                        ,tpt.TERMINAL
                        ,ISNULL(tpt.TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS
                        ,ISNULL(tpt.CHANGELINE, 0) AS CHANGELINE
                        ,ISNULL(tpt.MARKUPAMOUNT, 0) AS MARKUPAMOUNT
                        ,ISNULL(tpt.AUTHENTICATIONCODE, '') AS AUTHENTICATIONCODE
                        ,ISNULL(tpt.BATCHID, '') AS BATCHID
                        ,ISNULL(tpt.EFTAUTHENTICATIONCODE, '') AS EFTAUTHENTICATIONCODE
                        ,ISNULL(tpt.GIFTCARDID, '') AS GIFTCARDID
                        ,ISNULL(tpt.CREDITVOUCHERID, '') AS CREDITVOUCHERID
                        ,ISNULL(tpt.LOYALTYCARDID, '') AS LOYALTYCARDID
                        ,ISNULL(tpt.EXCHRATEMST, 0) AS EXCHRATEMST
                        ,ISNULL(tpt.AMOUNTMST, 0) AS AMOUNTMST
                        ,ISNULL(tpt.LOYALTYPOINTS, 0) AS LOYALTYPOINTS
                        ,ISNULL(tpt.COMMENT, '') AS COMMENT
                        ,ISNULL(tpt.CARDNUMBERHIDDEN, 0) AS CARDNUMBERHIDDEN
                        ,ISNULL(ct.NAME, ISNULL(tpt.CARDTYPEID,'<unknown>')) as CARDNAME 
                        ,ISNULL(tpt.CARDISSUER, '') AS CARDISSUER
                        ,ISNULL(tpt.EXPIRYDATE, '') AS EXPIRYDATE
                        ,ISNULL(tpt.CARDHOLDERNAME, '') AS CARDHOLDERNAME
                        ,ISNULL(tpt.CARDENTRYTYPE, 0) AS CARDENTRYTYPE
                        ,ISNULL(tpt.CARDBIN, '') AS CARDBIN
                        ,ISNULL(tpt.MERCHANTID, '') AS MERCHANTID
                        ,ISNULL(tpt.AUTHCODE, '') AS AUTHCODE
                        ,ISNULL(tpt.MESSAGE, '') AS MESSAGE
                        ,ISNULL(tpt.RESPONSECODE, '') AS RESPONSECODE
                        ,ISNULL(tpt.DRIVERID, '') AS DRIVERID
                        ,ISNULL(tpt.VEHICLEID, '') AS VEHICLEID
                        ,ISNULL(tpt.ODOMETERREADING, 0) AS ODOMETERREADING
                        ,ISNULL(tpt.DESCRIPTION, 0) AS DESCRIPTION
                        ,ISNULL(tt.TYPE, 0) as TRANSACTIONTYPE
                        FROM RBOTRANSACTIONPAYMENTTRANS tpt 
                        JOIN RBOTRANSACTIONTABLE tt on tt.TRANSACTIONID = tpt.TRANSACTIONID and tt.STORE = tpt.STORE and tt.TERMINAL = tpt.TERMINAL and tt.DATAAREAID = tpt.DATAAREAID
                        LEFT OUTER JOIN RBOSTORETENDERTYPECARDTABLE ct on tpt.TENDERTYPE = ct.TENDERTYPEID AND tpt.CARDTYPEID = ct.CARDTYPEID AND tpt.STORE = ct.STOREID  AND tpt.DATAAREAID = ct.DATAAREAID  
                        ";
            }
        }

        private static void PopulatePaymentTransactions(IDataReader dr, PaymentTransaction transaction)
        {
              transaction.TransactionID = (string)dr["TRANSACTIONID"];
              transaction.LineNumber = (decimal)dr["LINENUM"];
              transaction.ReceiptID = (string)dr["RECEIPTID"];
              transaction.StatementCode = (string)dr["STATEMENTCODE"];
              transaction.CardTypeID = (string)dr["CARDTYPEID"];
              transaction.ExchangeRate = (decimal)dr["EXCHRATE"];
              transaction.TenderType = (string)dr["TENDERTYPE"];
              transaction.AmountTenderd = (decimal)dr["AMOUNTTENDERED"];
              transaction.Currency = (string)dr["CURRENCY"];
              transaction.AmountOfCurrency = (decimal)dr["AMOUNTCUR"];
              transaction.CardOrAccount = (string)dr["CARDORACCOUNT"];
              transaction.TransactionDate = (DateTime)dr["TRANSDATE"];
              transaction.Shift = (string)dr["SHIFT"];
              transaction.ShiftDate = (DateTime)dr["SHIFTDATE"];
              transaction.Employee = (string)dr["STAFF"];
              transaction.StoreID = (string)dr["STORE"];
              transaction.TerminalID = (string)dr["TERMINAL"];
              transaction.TransactionStatus = (PaymentTransaction.TransactionStatuses)dr["TRANSACTIONSTATUS"];
              transaction.ChangeLine = ((byte)dr["CHANGELINE"] != 0);
              transaction.MarkupAmount = (decimal)dr["MARKUPAMOUNT"];
              transaction.AuthenticationCode = (string)dr["AUTHENTICATIONCODE"];
              transaction.BatchID = (string)dr["BATCHID"];
              transaction.EFTAuthenticationCode = (string)dr["EFTAUTHENTICATIONCODE"];
              transaction.GiftCardID = (string)dr["GIFTCARDID"];
              transaction.CreditVoucherID = (string)dr["CREDITVOUCHERID"];
              transaction.LoyaltyCardID = (string)dr["LOYALTYCARDID"];
              transaction.EXCHRATEMST = (decimal)dr["EXCHRATEMST"];
              transaction.AMOUNTMST = (decimal)dr["AMOUNTMST"];
              transaction.LoyalityPoints = (decimal)dr["LOYALTYPOINTS"];
              transaction.Comment = (string)dr["COMMENT"];
              transaction.CardNumberHidden = ((byte)dr["CARDNUMBERHIDDEN"] != 0);
              transaction.CardName = (string)dr["CARDNAME"];
              transaction.CardIssuer = (string)dr["CARDISSUER"];
              transaction.ExpiryDate = (string)dr["EXPIRYDATE"];
              transaction.CardHolderName = (string)dr["CARDHOLDERNAME"];
              transaction.CardEntry = (CardEntryTypesEnum)dr["CARDENTRYTYPE"];
              transaction.CardBIN = (string)dr["CARDBIN"];
              transaction.MerchantID = (string)dr["MERCHANTID"];
              transaction.CardAuthenticationCode = (string)dr["AUTHCODE"];
              transaction.CardMessage = (string)dr["MESSAGE"];
              transaction.CardResponeCode = (string)dr["RESPONSECODE"];
              transaction.DriverID = (string)dr["DRIVERID"];
              transaction.VechicleID = (string)dr["VEHICLEID"];
              transaction.ODOMeterReading = (int)dr["ODOMETERREADING"];
              transaction.Description = (string)dr["DESCRIPTION"];
              transaction.TransactionType = (TypeOfTransaction)dr["TRANSACTIONTYPE"];
        }

        public virtual PaymentTransaction Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " WHERE tpt.DATAAREAID = @dataAreaID AND tpt.TRANSACTIONID = @transactionID AND tpt.LINENUM = @lineID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", id);
                MakeParam(cmd, "lineID", id.SecondaryID);

                return Get<PaymentTransaction>(entry, cmd, id, PopulatePaymentTransactions, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<PaymentTransaction> GetAll(IConnectionManager entry, RecordIdentifier transactionId, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + " WHERE tpt.DATAAREAID = @dataAreaID AND tpt.TRANSACTIONID = @transactionID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", transactionId);

                return Execute<PaymentTransaction>(entry, cmd, CommandType.Text, PopulatePaymentTransactions);
            }
        }

        public virtual List<PaymentTransaction> GetForStatement(IConnectionManager entry, RecordIdentifier statementId, RecordIdentifier storeId, string currency, string tenderType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + @" WHERE tpt.DATAAREAID = @dataAreaID AND tt.STATEMENTID = @statementId AND tt.STORE = @storeId 
                                        and tpt.CURRENCY = @currency and tpt.TENDERTYPE = @tenderType and tpt.TRANSACTIONSTATUS = " + (int)PaymentTransaction.TransactionStatuses.Normal;
                
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementId", statementId);
                MakeParam(cmd, "storeId", storeId);
                MakeParam(cmd, "currency", currency);
                MakeParam(cmd, "tenderType", tenderType);

                return Execute<PaymentTransaction>(entry, cmd, CommandType.Text, PopulatePaymentTransactions);
            }
        }

        /// <summary>
        /// Gets all payment lines included in the given statement by store, terminal, currency and tender type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="statementId">The ID of the statement</param>
        /// <param name="storeId">The ID of the store</param>
        /// <param name="terminalId">The ID of the terminal</param>
        /// <param name="currency">The currency used for payment</param>
        /// <param name="tenderType">The payment type</param>
        /// <returns></returns>
        public virtual List<PaymentTransaction> GetForStatementAndTerminal(IConnectionManager entry, RecordIdentifier statementId, RecordIdentifier storeId, RecordIdentifier terminalId, string currency, string tenderType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + @" WHERE tpt.DATAAREAID = @dataAreaID AND tt.STATEMENTID = @statementId AND tt.STORE = @storeId AND tt.ENTRYSTATUS <> 5  
                                        and tpt.CURRENCY = @currency and tpt.TENDERTYPE = @tenderType and tpt.TERMINAL =  @terminalId
                                            and tpt.TRANSACTIONSTATUS = " + (int)PaymentTransaction.TransactionStatuses.Normal + " ORDER BY tpt.TRANSDATE";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementId", statementId);
                MakeParam(cmd, "storeId", storeId);
                MakeParam(cmd, "terminalId", terminalId);
                MakeParam(cmd, "currency", currency);
                MakeParam(cmd, "tenderType", tenderType);                

                return Execute<PaymentTransaction>(entry, cmd, CommandType.Text, PopulatePaymentTransactions);
            }
        }

        public virtual List<PaymentTransaction> GetForStoreAndPeriod(IConnectionManager entry, string storeId, DateTime from, DateTime to, DateFilterTypeEnum dateFilterType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql + @"WHERE tpt.DATAAREAID = @dataAreaID AND tt.STORE = @storeId AND tpt.TRANSACTIONSTATUS = " + (int)PaymentTransaction.TransactionStatuses.Normal;

                switch (dateFilterType)
                {
                    case DateFilterTypeEnum.TransDate:
                        cmd.CommandText += " AND tt.TRANSDATE >= @startDate AND tt.TRANSDATE <= @endDate";
                        break;
                    case DateFilterTypeEnum.BusinessDay:
                        cmd.CommandText += " AND tt.BUSINESSDAY >= @startDate AND tt.BUSINESSDAY <= @endDate";
                        break;
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeId);
                MakeParam(cmd, "startDate", from < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : from, SqlDbType.DateTime);
                MakeParam(cmd, "endDate", to, SqlDbType.DateTime);

                return Execute<PaymentTransaction>(entry, cmd, CommandType.Text, PopulatePaymentTransactions);
            }
        }

        public virtual DateTime GetNextCountingDate(IConnectionManager entry, RecordIdentifier transactionType, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT MAX(TRANSDATE) AS DATE FROM RBOTRANSACTIONTENDERDECLA20165
                                    WHERE DATAAREAID = @dataAreaID AND TERMINAL = @terminalID AND STORE = @storeID
                                    UNION
                                    SELECT MAX(TRANSDATE) AS DATE FROM RBOTRANSACTIONTABLE
                                    WHERE DATAAREAID = @dataAreaID AND TERMINAL = @terminalID AND STORE = @storeID AND TYPE = @transactionType";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "terminalID", terminalID);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "transactionType", transactionType);
                List<Date> dates = Execute(entry, cmd, CommandType.Text, 
                    delegate(IDataReader dr, Date date) 
                    {
                        date.DataBaseDateTime = dr["DATE"]; 
                    });
                return dates.Max().ToAxaptaSQLDate(true);
            }   
        }

        public virtual List<PaymentTransaction> GetRequiredDropAmounts(IConnectionManager entry, DateTime startDate, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT SUM(AMOUNTTENDERED) AS AMOUNTTENDERED, TENDERTYPE, 
                                    p.CURRENCY, SUM(p.AMOUNTCUR) AS AMOUNTCUR, p.EXCHRATE
                                    FROM RBOTRANSACTIONPAYMENTTRANS p, RBOSTORETENDERTYPETABLE t
                                    WHERE p.TENDERTYPE = t.TENDERTYPEID 
                                    AND p.DATAAREAID = t.DATAAREAID 
                                    AND p.DATAAREAID = @dataAreaID
                                    AND p.STORE = t.STOREID
                                    AND p.STORE = @storeID
                                    AND p.TERMINAL = @terminalID 
                                    AND p.TRANSDATE > @startDate
                                    AND p.TRANSDATE <= GETDATE()
                                    AND p.TRANSACTIONSTATUS = 0 
                                    AND p.TENDERTYPE in (select TENDERTYPEID from RBOSTORETENDERTYPETABLE where STOREID = @storeID and COUNTINGREQUIRED = 1) 
                                    GROUP BY p.TENDERTYPE, p.CURRENCY, p.EXCHRATE";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                MakeParam(cmd, "startDate", startDate, SqlDbType.DateTime);
                return Execute<PaymentTransaction>(entry, cmd, CommandType.Text, PopulateDropPaymentTransaction);
            }
        }

        private static void PopulateDropPaymentTransaction(IDataReader dr, PaymentTransaction transaction)
        {
            transaction.ExchangeRate = (decimal)dr["EXCHRATE"];
            transaction.TenderType = (string)dr["TENDERTYPE"];
            transaction.Currency = (string)dr["CURRENCY"];
            transaction.AmountTenderd = (decimal)dr["AMOUNTTENDERED"];
            transaction.AmountOfCurrency = (decimal)dr["AMOUNTCUR"];
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            throw new NotImplementedException();
        }

        public virtual void Save(IConnectionManager entry, PaymentTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
