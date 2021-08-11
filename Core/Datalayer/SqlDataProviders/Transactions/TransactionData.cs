using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class TransactionData : SqlServerDataProviderBase, ITransactionData
    {
        private const string BaseSql = @"Select distinct t.TRANSACTIONID, 
                    t.STORE, 
                    t.TERMINAL, 
                    t.TYPE, 
                    ISNULL(t.CURRENCY,'') as CURRENCY, 
                    ISNULL(t.RECEIPTID,'') as RECEIPTID,
                    ISNULL(t.STAFF,'') as STAFF, 
                    ISNULL(t.CREATEDONPOSTERMINAL,'') as CREATEDONPOSTERMINAL,
                    ISNULL(t.ENTRYSTATUS,0) as ENTRYSTATUS, 
                    ISNULL(t.SHIFT,'') as SHIFT,
                    ISNULL(t.STATEMENTCODE,'') as STATEMENTCODE,
                    ISNULL(t.OPENDRAWER,0) as OPENDRAWER, 
                    t.TRANSDATE,
                    t.SHIFTDATE,
                    ISNULL(t.EXCHRATE,0.0) as EXCHRATE,
                    ISNULL(t.CUSTACCOUNT,'') as CUSTACCOUNT, 
                    ISNULL(t.CUSTPURCHASEORDER,'') as CUSTPURCHASEORDER,
                    ISNULL(t.NETAMOUNT,0.0) as NETAMOUNT, 
                    ISNULL(t.GROSSAMOUNT,0.0) as GROSSAMOUNT,
                    ISNULL(t.SALESORDERAMOUNT,0.0) as SALESORDERAMOUNT, 
                    ISNULL(t.SALESINVOICEAMOUNT,0.0) as SALESINVOICEAMOUNT,
                    ISNULL(t.INCOMEEXPENSEAMOUNT,0.0) as INCOMEEXPENSEAMOUNT, 
                    ISNULL(t.ROUNDEDAMOUNT,0.0) as ROUNDEDAMOUNT,
                    ISNULL(t.SALESPAYMENTDIFFERENCE,0.0) as SALESPAYMENTDIFFERENCE, 
                    ISNULL(t.PAYMENTAMOUNT,0.0) as PAYMENTAMOUNT,
                    t.MARKUPAMOUNT,
                    ISNULL(t.MARKUPDESCRIPTION,'') as MARKUPDESCRIPTION, 
                    ISNULL(t.AMOUNTTOACCOUNT,0.0) as AMOUNTTOACCOUNT,
                    ISNULL(t.TOTALDISCAMOUNT,0.0) as TOTALDISCAMOUNT, 
                    ISNULL(t.NUMBEROFITEMS,0.0) as NUMBEROFITEMS,
                    ISNULL(t.INVOICECOMMENT,'') as INVOICECOMMENT,
                    ISNULL(t.RECEIPTEMAIL,'') as RECEIPTEMAIL, 
                    t.OILTAX,
                    t.TAXINCLINPRICE,
                    ISNULL(t.CREATEDDATE, '01.01.1900') as ENDDATETIME
                    FROM RBOTRANSACTIONTABLE t ";

        public virtual TypeOfTransaction GetTransactionType(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            IDataReader dr = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "select [TYPE] as TRANSACTIONTYPE " +
                    "from RBOTRANSACTIONTABLE " +
                    "where DATAAREAID = @dataAreaID and TRANSACTIONID = @transactionID and STORE = @storeID and TERMINAL = @terminalID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "terminalID", (string)terminalID);

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        return (TypeOfTransaction)dr["TRANSACTIONTYPE"];
                    }

                    return TypeOfTransaction.Internal;
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

        /// <summary>
        /// Gets a list of transactions optimized for Journals, i.e. only the relavant fields are loaded into the Transaction Business Object. 
        /// </summary>
        /// <param name="entry">Connections parameters for the database</param>
        /// <param name="numberOfTransactions">The maximum number of transactions returned</param>
        /// <param name="terminalId">The terminal where those transactions were conducted.</param>
        /// <param name="fromDate">Search constraint, limits the results to; from that date(inclusive)</param>
        /// <param name="toDate">Search constraint, limits the results to; to that date(inclusive)</param>
        /// <param name="receipt">Search constraint, the set should only return the transaction with that receipt number</param>
        /// <param name="fromTransaction">Search constraint, returns transactions lower than the constrain.</param>
        /// <param name="storeId">The store the transactions were concluded in</param>
        /// <param name="type">Search constraint, the set should only return the transactions of this type</param>
        /// <param name="staff">Search constraint, the set should only return the transactions created by this staff</param>
        /// <param name="minAmount">Search constraint, the set should only return the transactions with gross amount greater than <paramref name="minAmount"/></param>
        /// <param name="maxAmount">Search constraint, the set should only return the transactions with gross amount greater than <paramref name="maxAmount"/></param>
        /// <returns>List of transactions optimized for journal</returns>
        public List<Transaction> GetJournalTransactions
            (IConnectionManager entry, int numberOfTransactions, RecordIdentifier terminalId = null, DateTime ?fromDate = null, DateTime ?toDate = null, 
            RecordIdentifier receipt = null, RecordIdentifier fromTransaction = null, RecordIdentifier storeId = null,
            TypeOfTransaction? type = null, string staff = null, decimal? minAmount = null, decimal? maxAmount = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP(@NUMBEROFTRANSACTIONS) T.TRANSDATE, T.STAFF, T.RECEIPTID, T.TRANSACTIONID, T.GROSSAMOUNT, T.NETAMOUNT, T.TYPE, T.CURRENCY, T.STORE, T.TERMINAL, ISNULL(U.LOGIN, '') AS LOGIN
                                    FROM RBOTRANSACTIONTABLE T LEFT OUTER JOIN USERS U ON U.STAFFID = T.STAFF
                                    WHERE T.DATAAREAID = @DATAAREAID AND
                                    (T.TYPE = 2 OR T.TYPE = 3 OR T.TYPE = 14 OR T.TYPE = 15 OR T.TYPE = 22) AND T.ENTRYSTATUS = 0 ";
                if (receipt != null && receipt != "")
                {
                    cmd.CommandText += @"AND RECEIPTID = @RECEIPTFROMID ";
                    MakeParam(cmd, "RECEIPTFROMID", receipt);
                }
                if (fromTransaction != null && fromTransaction != "")
                {
                    cmd.CommandText += @"AND TRANSACTIONID <= @TRANSACTIONID ";
                    MakeParam(cmd, "TRANSACTIONID", fromTransaction);
                }
                if (fromDate != null && fromDate.Value.Ticks != 0)
                {
                    cmd.CommandText += @"AND TRANSDATE >= @FROMDATE ";
                    MakeParam(cmd, "FROMDATE", fromDate.Value, SqlDbType.DateTime);
                }
                if (toDate != null && toDate.Value.Ticks != 0)
                {
                    cmd.CommandText += @"AND TRANSDATE <= @TODATE ";
                    MakeParam(cmd, "TODATE", toDate.Value, SqlDbType.DateTime);
                }
                if (storeId != null)
                {
                    cmd.CommandText += @"AND STORE = @STOREID ";
                    MakeParam(cmd, "STOREID", storeId);
                }
                if (terminalId != null)
                {
                    cmd.CommandText += @"AND TERMINAL = @TERMINALID ";
                    MakeParam(cmd, "TERMINALID", terminalId);                    
                }
                if (type != null)
                {
                    cmd.CommandText += @"AND TYPE = @TYPE ";
                    MakeParam(cmd, "TYPE", (int)type);
                }
                if (staff != null)
                {
                    cmd.CommandText += @"AND STAFF = @STAFF ";
                    MakeParam(cmd, "STAFF", staff);
                }
                if (minAmount != null)
                {
                    cmd.CommandText += @"AND GROSSAMOUNT >= @MINAMOUNT ";
                    MakeParam(cmd, "MINAMOUNT", minAmount);
                }
                if (maxAmount != null)
                {
                    cmd.CommandText += @"AND GROSSAMOUNT <= @MAXAMOUNT ";
                    MakeParam(cmd, "MAXAMOUNT", maxAmount);
                }

                if ((toDate != null && toDate.Value.Ticks != 0) || (fromDate != null && fromDate.Value.Ticks != 0))
                {
                    cmd.CommandText += @"ORDER BY TRANSDATE DESC";
                }
                else
                {
                    cmd.CommandText += @"ORDER BY RECEIPTID DESC";
                }
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "NUMBEROFTRANSACTIONS", numberOfTransactions);
                return Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransactionForJournal);
            }
        }

        public virtual List<Transaction> GetSalesTransactionsForStatementID(IConnectionManager entry, RecordIdentifier statementID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                        BaseSql +
                        "Join RBOTRANSACTIONSALESTRANS s on s.DATAAREAID = t.DATAAREAID and s.TRANSACTIONID = t.TRANSACTIONID and s.STORE = t.STORE and s.TERMINALID = t.TERMINAL " +
                        "where t.DATAAREAID = @dataAreaID and t.STATEMENTID = @statementID and t.ENTRYSTATUS = 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", (string)statementID);

                return Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public virtual List<Transaction> GetTransactionsForStatementID(IConnectionManager entry, RecordIdentifier statementID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                        BaseSql +
                        "where t.DATAAREAID = @dataAreaID and t.STATEMENTID = @statementID and t.ENTRYSTATUS = 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", (string)statementID);

                return Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public virtual Transaction GetRetailTransHeader(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                        BaseSql +
                        "where t.DATAAREAID = @dataAreaID and TRANSACTIONID = @transactionID and STORE = @storeID and TERMINAL = @terminalID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionId);
                MakeParam(cmd, "storeID", (string)storeId);
                MakeParam(cmd, "terminalID", (string)terminalId);

                var transactions = Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransaction);

                return transactions.Count > 0 ? transactions[0] : null;
            }
        }

        public virtual List<Transaction> GetTransactionsFromType(IConnectionManager entry, RecordIdentifier storeId, TypeOfTransaction transactionType, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                        BaseSql +
                        @"where t.DATAAREAID = @dataAreaID and t.STORE = @storeID and t.TYPE = @type
                        ORDER BY t.TRANSDATE " + (sortBackwards ? "DESC" : "ASC");

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeId);
                MakeParam(cmd, "type", (int)transactionType);

                return Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public virtual List<Transaction> GetSalesTransactions(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier terminalId, DateTime? periodStart, DateTime? periodEnd)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  "where t.DATAAREAID = @dataAreaID and t.ENTRYSTATUS = 0 and t.TYPE = @salesTransactionType ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "salesTransactionType", (int)TypeOfTransaction.Sales);

                if (periodStart != null && periodStart.Value.Ticks != 0)
                {
                    cmd.CommandText += @"AND TRANSDATE >= @periodStart ";
                    MakeParam(cmd, "periodStart", periodStart.Value, SqlDbType.DateTime);
                }
                if (periodEnd != null && periodEnd.Value.Ticks != 0)
                {
                    cmd.CommandText += @"AND TRANSDATE <= @periodEnd ";
                    MakeParam(cmd, "periodEnd", periodEnd.Value, SqlDbType.DateTime);
                }
                if (storeId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += @"AND STORE = @storeId ";
                    MakeParam(cmd, "storeId", storeId);
                }

                if (terminalId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += @"AND TERMINAL = @terminalID ";
                    MakeParam(cmd, "terminalID", terminalId);   
                }
                return Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransaction);
            }
        }

        public virtual int GetNumberOfItemsForTransaction(IConnectionManager entry, RecordIdentifier id, bool excludeVoidedAndReturns)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT SUM(QTY) AS QUANTITY " +
                                  "FROM RBOTRANSACTIONSALESTRANS " +
                                  "WHERE TRANSACTIONID = @transactionID AND STORE = @storeID AND TERMINALID = @terminalID ";
                if (excludeVoidedAndReturns)
                {
                    cmd.CommandText += "AND TRANSACTIONSTATUS = 0 AND QTY < 0 ";
                }
                MakeParam(cmd, "transactionID", id.PrimaryID);
                MakeParam(cmd, "storeID", id.SecondaryID.PrimaryID);
                MakeParam(cmd, "terminalID", id.SecondaryID.SecondaryID.PrimaryID);
                object result = entry.Connection.ExecuteScalar(cmd);
                return result is DBNull ? 0 : -Convert.ToInt32(result);
            }
        }

        private static void PopulateTransactionForJournal(IDataReader dr, Transaction transaction)
        {
            transaction.TransactionDate = Date.FromAxaptaDate(dr["TRANSDATE"]);
            transaction.EmployeeID = (string)dr["STAFF"];
            transaction.CurrencyID = (string)dr["CURRENCY"];
            transaction.TransactionID = (string)dr["TRANSACTIONID"];
            transaction.ReceiptID = (string)dr["RECEIPTID"];
            transaction.NetAmount = (decimal) dr["NETAMOUNT"];
            transaction.NetAmountWithTax = (decimal)dr["GROSSAMOUNT"];
            transaction.Type = (TypeOfTransaction)dr["TYPE"];
            transaction.StoreID = (string)dr["STORE"];
            transaction.TerminalID = (string)dr["TERMINAL"];
            transaction.Login = (string)dr["LOGIN"];
        }

        private static void PopulateTransaction(IDataReader dr, Transaction transaction)
        {
            transaction.TransactionID = (string)dr["TRANSACTIONID"];
            transaction.StoreID = (string)dr["STORE"];
            transaction.TerminalID = (string)dr["TERMINAL"];
            transaction.Type = (TypeOfTransaction)dr["TYPE"];
            transaction.CurrencyID = (string)dr["CURRENCY"];
            transaction.ReceiptID = (string)dr["RECEIPTID"];
            transaction.EmployeeID = (string)dr["STAFF"];
            transaction.CreatedOnPosTerminal = (string)dr["CREATEDONPOSTERMINAL"];
            transaction.EntryStatus = (TransactionStatus)dr["ENTRYSTATUS"];
            transaction.TransactionDate = Date.FromAxaptaDate(dr["TRANSDATE"]);
            transaction.ShiftID = (string)dr["SHIFT"];
            transaction.ShiftDate = Date.FromAxaptaDate(dr["SHIFTDATE"]);
            transaction.OpenDrawer = ((byte)dr["OPENDRAWER"] != 0);
            transaction.StatementCode = (string)dr["STATEMENTCODE"];
            transaction.ExchangeRate = (decimal)dr["EXCHRATE"];
            transaction.CustomerID = (string)dr["CUSTACCOUNT"];
            transaction.CustomerPurchRequestId = (string)dr["CUSTPURCHASEORDER"];
            transaction.NetAmount = (decimal)dr["NETAMOUNT"];
            transaction.NetAmountWithTax = (decimal)dr["GROSSAMOUNT"];
            transaction.SalesOrderAmounts = (decimal)dr["SALESORDERAMOUNT"];
            transaction.SalesInvoceAmounts = (decimal)dr["SALESINVOICEAMOUNT"];
            transaction.IncomeExpenseAmounts = (decimal)dr["INCOMEEXPENSEAMOUNT"];
            transaction.RoundingDifference = (decimal)dr["ROUNDEDAMOUNT"];
            transaction.RoundingSalePmtDiff = (decimal)dr["SALESPAYMENTDIFFERENCE"];
            transaction.PaymentAmount = (decimal)dr["PAYMENTAMOUNT"];

            if (dr["MARKUPAMOUNT"] == DBNull.Value)
            {
                transaction.HasMarkup = false;
                transaction.MarkupAmount = 0.0m;
            }
            else
            {
                transaction.HasMarkup = true;
                transaction.MarkupAmount = (decimal)dr["MARKUPAMOUNT"];
            }

            transaction.MarkupDescription = (string)dr["MARKUPDESCRIPTION"];
            transaction.AmountToAccount = (decimal)dr["AMOUNTTOACCOUNT"];
            transaction.TotalDiscountAmount = (decimal)dr["TOTALDISCAMOUNT"];
            transaction.NumberOfItems = (decimal)dr["NUMBEROFITEMS"];
            transaction.InvoiceComment = (string)dr["INVOICECOMMENT"];
            transaction.ReceiptEmailAddress = (string)dr["RECEIPTEMAIL"];
            transaction.EndDateTime = (DateTime)dr["ENDDATETIME"];

            if (dr["OILTAX"] == DBNull.Value)
            {
                transaction.HasOilTax = false;
                transaction.OilTax = 0.0m;
            }
            else
            {
                transaction.HasOilTax = true;
                transaction.OilTax = (decimal)dr["OILTAX"];
            }

            if (dr["TAXINCLINPRICE"] == DBNull.Value)
            {
                transaction.HasTaxIncludedInPriceFlag = false;
                transaction.TaxIncludedInPrice = false;
            }
            else
            {
                transaction.HasTaxIncludedInPriceFlag = true;
                transaction.TaxIncludedInPrice = ((byte)dr["TAXINCLINPRICE"] != 0);
            }
        }

        public virtual void PerformEOD(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, DateTime timeOfEOD)
        {
            const int eodTypeInt = 12;

            // Create the transaction ID for the EOD transaction
            var transactionID = DataProviderFactory.Instance.GenerateNumber<ITransactionData, Transaction>(entry);

            // Save the EOD
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONTABLE") {StatementType = StatementType.Insert};

            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("TRANSACTIONID", (string)transactionID);
            statement.AddKey("STORE", (string)storeID);
            statement.AddKey("TERMINAL", (string)terminalID);

            statement.AddField("TYPE", eodTypeInt, SqlDbType.Int);
            statement.AddField("TRANSDATE", timeOfEOD, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void MarkTransactionAsInventoryUpdated(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONTABLE") {StatementType = StatementType.Update};

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("TRANSACTIONID", (string)transactionId);
            statement.AddCondition("STORE", (string)storeID);
            statement.AddCondition("TERMINAL", (string)terminalID);

            statement.AddField("INVENTORYUPDATED", true, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Checks if an unconcluded transaction exists in POSISTRANSACTION table. If true then the POS was exited abnormally i.e a crash
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual bool UnconcludedTransactionExists(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = 
                    "select COUNT(*) " +
                    "from POSISTRANSACTION " +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var noOfRecords = (int)entry.Connection.ExecuteScalar(cmd);

                return noOfRecords > 0;
            }
        }

        public virtual DateTime? UnpostedTransactionExists(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT
	                    MIN(TRANSDATE)
	                    FROM RBOTRANSACTIONTABLE 
	                    WHERE 
		                    DATAAREAID = 'LSR' AND (
			                    STATEMENTID  = '' OR STATEMENTID IS NULL) AND 
		                    ENTRYSTATUS NOT IN (1,5)";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                object obj = entry.Connection.ExecuteScalar(cmd);

                if (obj == DBNull.Value)
                {
                    return null;
                }
                return (DateTime) obj;
            }
        }

        private static bool EODTransactionExists(IConnectionManager entry, RecordIdentifier eodTransactionID)
        {
            return RecordExists(entry, "RBOTRANSACTIONTABLE", "TRANSACTIONID", eodTransactionID);
        }

        public virtual bool TransactionHasBeenReturnedOrContainsReturns(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier terminalID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM RBOTRANSACTIONSALESTRANS A " +
                                  "WHERE " +
                                  "((A.RETURNTRANSACTIONID = @transactionID AND A.TRANSACTIONSTATUS = 0)" +
                                  "OR (A.TRANSACTIONID = @transactionID AND A.QTY > 0 AND A.TRANSACTIONSTATUS = 0)) " +
                                  "AND A.TERMINALID = @terminalID " +
                                  "AND A.DATAAREAID = @dataAreaID";
                MakeParam(cmd, "transactionID", (string)transactionID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "terminalID", (string)terminalID);
                IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
                try
                {
                    return dr.Read();
                }
                finally
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="zReportID">The unique ID of a Z report</param>
        /// <returns>
        /// If true then the Z report ID already exists in the transaction tables
        /// </returns>
        public virtual bool ZReportExists(IConnectionManager entry, RecordIdentifier zReportID)
        {
            return RecordExists(entry, "RBOTRANSACTIONTABLE", "ZREPORTID", zReportID);            
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return EODTransactionExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "EODTRANSACTIONID"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTRANSACTIONTABLE", "TRANSACTIONID", sequenceFormat, startingRecord, numberOfRecords);
        }

        /// <summary>
        /// Checks if an assembly item has been sold, Returns true if it has been sold, false if not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="assemblyID">ID of the assembly item you want to check for</param>
        /// <returns></returns>
        public virtual bool AssemblyItemIsActive(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            return RecordExists(entry, "RBOTRANSACTIONSALESTRANS", "ASSEMBLYID", assemblyID);
        }

        public Transaction GetLastRetailTransaction(IConnectionManager entry)
        {
     
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 1 T.TRANSDATE, T.STAFF, T.RECEIPTID, T.TRANSACTIONID, T.GROSSAMOUNT, T.NETAMOUNT, T.TYPE, T.CURRENCY, T.STORE, T.TERMINAL, ISNULL(U.Login, '') AS LOGIN
                                    FROM RBOTRANSACTIONTABLE T LEFT OUTER JOIN USERS U ON U.STAFFID = T.STAFF
                                    WHERE T.DATAAREAID = @dataAreaID AND
                                    (T.TYPE = 2 OR T.TYPE = 3 OR T.TYPE = 14 OR T.TYPE = 15) AND T.ENTRYSTATUS = 0 
                                    ORDER BY T.TRANSDATE DESC";
            
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                var list = Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransactionForJournal);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                return null;
            }
        
        }

        public Transaction GetLastTransaction(IConnectionManager entry)
        {

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 1 T.TRANSDATE, T.STAFF, T.RECEIPTID, T.TRANSACTIONID,T. GROSSAMOUNT, T.NETAMOUNT, T.TYPE, T.CURRENCY, T.STORE, T.TERMINAL, ISNULL(U.Login, '') AS LOGIN
                                    FROM RBOTRANSACTIONTABLET LEFT OUTER JOIN USERS U ON U.STAFFID = T.STAFF
                                    WHERE T.DATAAREAID = @dataAreaID
                                    AND T.ENTRYSTATUS = 0 
                                    ORDER BY T.TRANSDATE DESC";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                var list = Execute<Transaction>(entry, cmd, CommandType.Text, PopulateTransactionForJournal);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                return null;
            }

        }
    }
}
