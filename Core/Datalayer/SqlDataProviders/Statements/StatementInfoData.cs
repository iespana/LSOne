using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.DataLayer.DataProviders.TenderDeclaration;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Statements
{
    /// <summary>
    /// Data provider class for statements
    /// </summary>
    public class StatementInfoData : SqlServerDataProviderBase, IStatementInfoData
    {
        private static string ResolveSort(StatementInfoSorting sort, bool backwards)
        {
            string sortstring = "";
            switch (sort)
            {
                case StatementInfoSorting.ID:
                    sortstring = "STATEMENTID";
                    break;
                case StatementInfoSorting.StartingTime:
                    sortstring = "PERIODSTARTINGTIME";
                    break;
                case StatementInfoSorting.EndingTime:
                    sortstring = "PERIODENDINGTIME";
                    break;
                case StatementInfoSorting.CalculatedTime:
                    sortstring = "CALCULATEDTIME";
                    break;
                case StatementInfoSorting.PostingDate:
                    sortstring =  "POSTINGDATE";
                    break;
            }
            sortstring += (backwards ? " DESC" : " ASC");
            return sortstring;
        }

        private static string SelectFields()
        {
            return @"SELECT STATEMENTID, 
                                ISNULL(STOREID,'') AS StoreID,
                                ISNULL(CALCULATEDTIME,'01.01.1900') AS CalculatedTime,
                                ISNULL(POSTINGDATE,'01.01.1900') AS PostingDate,
                                ISNULL(PERIODSTARTINGTIME,'01.01.1900') AS PeriodStartingTime,
                                ISNULL(PERIODENDINGTIME,'01.01.1900') AS PeriodEndingTime,
                                ISNULL(ERPPROCESSED,'0') AS ERPPROCESSED,
                                ISNULL(POSTED,'0') AS Posted,
                                ISNULL(CALCULATED,'0') AS CALCULATED,
                                ISNULL(TOTYPE, '0') AS TOTYPE,
                                ISNULL(FROMTYPE, '0') AS FROMTYPE,
                                ISNULL(TOPERIODTYPE, '0') AS TOPERIODTYPE,
                                ISNULL(FROMPERIODTYPE, '0') AS FROMPERIODTYPE
                                FROM RBOSTATEMENTTABLE ";
        }

        private static void PopulateStatementInfo(IDataReader dr, StatementInfo statement)
        {
            statement.ID = (string)dr["STATEMENTID"];
            statement.CalculatedTime = (DateTime)dr["CALCULATEDTIME"];
            statement.PostingDate = new Date((DateTime)dr["POSTINGDATE"]);
            statement.StoreID = (string)dr["STOREID"];
            statement.StartingTime = (DateTime)dr["PERIODSTARTINGTIME"];
            statement.EndingTime = (DateTime)dr["PERIODENDINGTIME"];
            statement.Posted = ((byte)dr["POSTED"] == 1);
            statement.Calculated = ((byte)dr["CALCULATED"] == 1); 
            statement.ERPProcessed = ((byte)dr["ERPPROCESSED"] == 1);
            statement.ToType = (StatementPeriodFormEnum)((int)dr["TOTYPE"]);
            statement.FromType = (StatementPeriodFormEnum)((int)dr["FROMTYPE"]);
            statement.ToPeriodType = (StatementPeriodTypeEnum)((int)dr["TOPERIODTYPE"]);
            statement.FromPeriodType = (StatementPeriodTypeEnum)((int)dr["FROMPERIODTYPE"]);
        }

        private static void PopulateStatementHeader(IDataReader dr, StatementHeader statement)
        {
            statement.ID = (string)dr["STATEMENTID"];
            statement.StoreID = (string)dr["STOREID"];
            statement.StartingTime = (DateTime)dr["PERIODSTARTINGTIME"];
            statement.EndingTime = (DateTime)dr["PERIODENDINGTIME"];
        }

        private static void PopulateStatementCountInfo(IDataReader dr, StatementCountInfo countInfo)
        {
            countInfo.StoreID = (string)dr["STOREID"];
            countInfo.StatementCount = (int)dr["STATEMENTCOUNT"];
        }

        // TODO fix it so that the transaction type is an enum sorted by the EODEnums
        private static void PopulateStatementTransaction(IDataReader dr, StatementTransaction statementTransaction)
        {
            statementTransaction.TransactionNumber = (string)dr["TransactionID"];
            statementTransaction.TransactionType = (int)dr["TYPE"];
            //statementTransaction.TenderTypeName = (string)dr["TenderTypeName"];
            statementTransaction.CurrencyCode = (string)dr["CURRENCYCODE"];
            statementTransaction.Amount = (decimal)dr["Amount"];
            statementTransaction.TransactionTime = (DateTime)dr["Date"];
            statementTransaction.StaffID = (string)dr["Staff"];
            statementTransaction.TerminalID = (string)dr["Terminal"];
            statementTransaction.TenderTypeID = (string)dr["TenderTypeID"];
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            // Unmark all the transactions
            UnmarkTransactions(entry, statementID.PrimaryID, storeID);

            // Delete the statement lines
            Providers.StatementLineData.DeleteLines(entry, statementID);

            // Delete the statement itself
            DeleteRecord(entry, "RBOSTATEMENTTABLE", "STATEMENTID", statementID, Permission.RunEndOfDay);
        }

        public virtual void ClearCalculatedUnpostedStatement(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            // Unmark all the transactions
            UnmarkTransactions(entry, statementID.PrimaryID, storeID);

            // Delete the statement lines
            Providers.StatementLineData.DeleteLines(entry, statementID);

            // Set the statement as not calculated
            var statement = Get(entry, statementID);
            statement.Calculated = false;
            Save(entry, statement);
        }

        /// <summary>
        /// Removes the statement markings for the given statement
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="statementID">The statement ID</param>
        /// <param name="storeID">The ID of the store that the statement was originally posted for</param>
        public virtual void UnmarkPostedStatement(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            // Unmark all the transactions
            UnmarkTransactions(entry, statementID.PrimaryID, storeID);

            // Delete the statement lines
            Providers.StatementLineData.DeleteLines(entry, statementID);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier statementID)
        {
            return RecordExists(entry, "RBOSTATEMENTTABLE",  "STATEMENTID" , statementID);
        }

        public virtual StatementInfo Get(IConnectionManager entry, RecordIdentifier statementID)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() + @"WHERE DATAAREAID = @dataAreaId AND STATEMENTID = @statementID ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "statementID", (string)statementID);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            var statement = (result.Count > 0) ? result[0] : null;

            // Get statement lines and add them to the statement
            if (statement != null)
            {
                statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        public virtual StatementInfo GetLastStatement(IConnectionManager entry, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() + @"WHERE DATAAREAID = @dataAreaId AND STOREID = @storeID ORDER BY PERIODENDINGTIME DESC";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);

                return (result.Count > 0) ? result[0] : null;
            }
        }

        public virtual List<StatementInfo> GetStatements(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum, bool sortBackwards, StatementTypeEnum statementType)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() + @"WHERE DATAAREAID = @dataAreaID AND STOREID = @storeID AND POSTED = @posted
                                    ORDER BY " + ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "posted", (statementType == StatementTypeEnum.PostedStatements) ? 1 : 0, SqlDbType.Int);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            foreach (var statement in result)
            {
                if (statement != null)
                {
                    statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
                }
            }

            return result;
        }

        public virtual List<StatementInfo> GetAllStatements(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum, bool sortBackwards)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() + @"WHERE DATAAREAID = @dataAreaID AND STOREID = @storeID
                                    ORDER BY " + ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            foreach (var statement in result)
            {
                if (statement != null)
                {
                    statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
                }
            }

            return result;
        }

        public List<StatementHeader> GetPostedStatementHeaders(IConnectionManager entry, RecordIdentifier storeID, DateTime startTime, DateTime endTime)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                    @"select 
                                        STATEMENTID, 
                                        ISNULL(STOREID,'') AS StoreID,
                                        ISNULL(PERIODSTARTINGTIME,'01.01.1900') AS PeriodStartingTime,
                                        ISNULL(PERIODENDINGTIME,'01.01.1900') AS PeriodEndingTime
                                    from RBOSTATEMENTTABLE
                                    where STOREID = @storeID AND POSTED = 1 
                                    AND PERIODSTARTINGTIME BETWEEN @startTime and @endTime
                                    AND PERIODENDINGTIME BETWEEN @startTime and @endTime
                                    ORDER BY PERIODSTARTINGTIME DESC"; 

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "startTime", startTime, SqlDbType.DateTime);
                MakeParam(cmd, "endTime", endTime, SqlDbType.DateTime);

                return Execute<StatementHeader>(entry, cmd, CommandType.Text, PopulateStatementHeader);
            }
        }

        public List<StatementCountInfo> GetPostedStatementCount(IConnectionManager entry, DateTime startTime, DateTime endTime)
        {
            List<StatementCountInfo> result;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select s.STOREID, ISNULL(A.STATEMENTCOUNT,0) as STATEMENTCOUNT from RBOSTORETABLE s 
                        outer apply
                        (
	                        select count('x') as STATEMENTCOUNT from RBOSTATEMENTTABLE st
	                        where st.STOREID = s.STOREID and 
	                        st.PERIODSTARTINGTIME BETWEEN @startTime and  @endTime and
	                        st.PERIODENDINGTIME BETWEEN @startTime and @endTime 
                        group by st.STOREID 
                        ) A ";

                MakeParam(cmd, "startTime", startTime, SqlDbType.DateTime);
                MakeParam(cmd, "endTime", endTime, SqlDbType.DateTime);

                result = Execute<StatementCountInfo>(entry, cmd, CommandType.Text, PopulateStatementCountInfo);
            }

            return result;
        }

        public List<StatementInfo> GetStatementsForPeriod(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum,
            bool sortBackwards, StatementTypeEnum statementType, DateTime startTime, DateTime endTime)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() +
                                    @"WHERE DATAAREAID = @dataAreaID 
                                    AND STOREID = @storeID 
                                    AND POSTED = @posted 
                                    AND PERIODSTARTINGTIME BETWEEN @startTime and @endTime
                                    AND PERIODENDINGTIME BETWEEN @startTime and @endTime 
                                    ORDER BY " + ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "posted", (statementType == StatementTypeEnum.PostedStatements) ? 1 : 0, SqlDbType.Int);
                MakeParam(cmd, "startTime", startTime, SqlDbType.DateTime);
                MakeParam(cmd, "endTime", endTime, SqlDbType.DateTime);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            foreach (var statement in result)
            {
                if (statement != null)
                {
                    statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
                }
            }

            return result;
        }

        public virtual List<StatementInfo> GetUnprocessedERPStatementsFromAllStores(IConnectionManager entry, StatementTypeEnum statementType)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() +
                                  "WHERE DATAAREAID = @dataAreaID AND POSTED = @posted and (ERPPROCESSED <> 1 or ERPPROCESSED is null) ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "posted", (statementType == StatementTypeEnum.PostedStatements) ? 1 : 0, SqlDbType.Int);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            foreach (StatementInfo statement in result)
            {
                if (statement != null)
                {
                    statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
                }
            }

            return result;
        }

        public virtual void Save(IConnectionManager entry, StatementInfo statementInfo)
        {
            var statement = new SqlServerStatement("RBOSTATEMENTTABLE");

            ValidateSecurity(entry, Permission.RunEndOfDay);

            bool isNew = false;
            if (statementInfo.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                statementInfo.ID = DataProviderFactory.Instance.GenerateNumber<IStatementInfoData, StatementInfo>(entry);
            }

            if (isNew || !Exists(entry, statementInfo.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STATEMENTID", (string)statementInfo.ID);
                //statement.AddKey("STOREID", (string)statementInfo.StoreID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STATEMENTID", (string)statementInfo.ID);
                //statement.AddCondition("STOREID", (string)statementInfo.StoreID);
            }

            statement.AddField("CALCULATEDTIME", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("POSTINGDATE", statementInfo.PostingDate.DateTime, SqlDbType.DateTime);
            statement.AddField("PERIODSTARTINGTIME", statementInfo.StartingTime, SqlDbType.DateTime);
            statement.AddField("PERIODENDINGTIME", statementInfo.EndingTime, SqlDbType.DateTime);
            statement.AddField("POSTED", statementInfo.Posted, SqlDbType.TinyInt);
            statement.AddField("CALCULATED", statementInfo.Calculated, SqlDbType.TinyInt);
            statement.AddField("STOREID", (string)statementInfo.StoreID);
            statement.AddField("ERPPROCESSED", statementInfo.ERPProcessed, SqlDbType.TinyInt);
            statement.AddField("TOTYPE", statementInfo.ToType, SqlDbType.Int);
            statement.AddField("FROMTYPE", statementInfo.FromType, SqlDbType.Int);
            statement.AddField("TOPERIODTYPE", statementInfo.ToPeriodType, SqlDbType.Int);
            statement.AddField("FROMPERIODTYPE", statementInfo.FromPeriodType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Updates the calculated and calculatedTime properties of a StatementInfo object
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="statementInfo">The StatementInfo object to update</param>
        public virtual void UpdateCalculatedInfo(IConnectionManager entry, StatementInfo statementInfo)
        {
            var statement = new SqlServerStatement("RBOSTATEMENTTABLE");

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementInfo.ID);

            statement.AddField("CALCULATEDTIME", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("CALCULATED", statementInfo.Calculated, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void UpdatePostedInfo(IConnectionManager entry, StatementInfo statementInfo)
        {
            var statement = new SqlServerStatement("RBOSTATEMENTTABLE");

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementInfo.ID);

            statement.AddField("POSTED", statementInfo.Posted, SqlDbType.TinyInt);
            statement.AddField("POSTINGDATE", statementInfo.PostingDate.DateTime, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual List<StatementTransaction> GetTransactionsWithoutStatementIDs(IConnectionManager entry, DateTime startTime, DateTime endTime, RecordIdentifier storeID)
        {
            // Make a big left out join query on the transaction table and run the result through the populate transaction function
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select t.TransactionID, t.TYPE, t.TenderTypeID, " +
                    "t.Amount, t.Date, t.Staff, t.Terminal, t.CurrencyCode " +
                    "From (select  " +
                    "COALESCE(p.TRANSACTIONID,t.TRANSACTIONID, b.TRANSACTIONID, s.TRANSACTIONID, '') as TransactionID, " +
                    "tt.TYPE as TYPE, " +
                    "COALESCE(p.TENDERTYPE,t.TENDERTYPE, b.TENDERTYPE, s.TENDERTYPE, '') as TenderTypeID, " +
                    "COALESCE(p.CURRENCY,t.CURRENCY, b.CURRENCY, s.CURRENCY, '') as CurrencyCode, " +
                    "COALESCE(p.AMOUNTCUR,t.AMOUNTTENDERED, b.AMOUNTCUR, s.AMOUNTCUR, '0') as Amount, " +
                    "COALESCE(p.TRANSDATE,t.TRANSDATE, b.TRANSDATE, s.TRANSDATE, '01.01.1900') as Date, " +
                    "COALESCE(p.STAFF,t.STAFF, b.STAFF, s.STAFF, '') as Staff, " +
                    "COALESCE(p.TERMINAL,t.TERMINAL, b.TERMINAL, s.TERMINAL, '') as Terminal, " +
                    "tt.DATAAREAID as DATAAREAID, " +
                    "tt.STORE as StoreID " +
                    "from RBOTRANSACTIONTABLE  tt " +
                    "left outer join RBOTRANSACTIONPAYMENTTRANS p on tt.TRANSACTIONID = p.TRANSACTIONID and tt.DATAAREAID = p.DATAAREAID and tt.STORE = p.STORE and tt.TERMINAL = p.TERMINAL and p.TRANSACTIONSTATUS = 0 and tt.ENTRYSTATUS = 0 " +
                    "left outer join RBOTRANSACTIONTENDERDECLA20165 t on tt.TRANSACTIONID = t.TRANSACTIONID and tt.DATAAREAID = t.DATAAREAID and tt.STORE = t.STORE and tt.TERMINAL = t.TERMINAL and t.TRANSACTIONSTATUS = 0 and tt.ENTRYSTATUS = 0 " +
                    "left outer join RBOTRANSACTIONBANKEDTENDE20338 b on tt.TRANSACTIONID = b.TRANSACTIONID and tt.DATAAREAID = b.DATAAREAID and tt.STORE = b.STORE and tt.TERMINAL = b.TERMINAL and b.TRANSACTIONSTATUS = 0 and tt.ENTRYSTATUS = 0 " +
                    "left outer join RBOTRANSACTIONSAFETENDERTRANS s on tt.TRANSACTIONID = s.TRANSACTIONID and tt.DATAAREAID = s.DATAAREAID and tt.STORE = s.STORE and tt.TERMINAL = s.TERMINAL and s.TRANSACTIONSTATUS = 0  and tt.ENTRYSTATUS = 0 " +
                    "where tt.TRANSDATE between @startTime and @endTime " +
                    "and tt.DATAAREAID = @dataAreaId " +
                    "and tt.ENTRYSTATUS = 0 " +
                    //"and tt.ENTRYSTATUS <> 5 " +
                    "AND (tt.type in (2,3,4,5,6,7,16,17,20,21,22)) " +
                    //"and (tt.TYPE between 2 and 7 or tt.type between 16 and 17 or tt.type between 20 and 22) " +
                    "and tt.STATEMENTID = '' " +
                    "and tt.STORE = @storeID) t ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "startTime", startTime, SqlDbType.DateTime);
                MakeParam(cmd, "endTime", endTime, SqlDbType.DateTime);
                MakeParam(cmd, "storeID", (string) storeID);

                var regularTransactions = Execute<StatementTransaction>(entry, cmd, CommandType.Text,
                    PopulateStatementTransaction);

                // Get all the transactions from SCTenderDeclarations for the given period

                var tenderDeclarationsForPeriod = DataProviderFactory.Instance.Get<ITenderDeclarationData, Tenderdeclaration>().GetAllTenderDeclarationsWithoutStatementIDForAPeriod(entry, storeID, startTime,
                                                                               endTime);

                var tenderDeclarationsTransactions = CreateTransactionsFromTenderDeclarations(tenderDeclarationsForPeriod);

                // Combine the lists
                var combinedList = regularTransactions;

                foreach (var trans in tenderDeclarationsTransactions)
                {
                    combinedList.Add(trans);
                }

                return combinedList;
            }
        }

        private static IEnumerable<StatementTransaction> CreateTransactionsFromTenderDeclarations(IEnumerable<Tenderdeclaration> tds)
        {
            var transactions = new List<StatementTransaction>();
            foreach (var td in tds)
            {
                foreach (TenderdeclarationLine tdl in td.TenderDeclarationLines)
                {
                    var transaction = new StatementTransaction
                        {
                            TransactionNumber = "",
                            TransactionType = 7,
                            TenderTypeID = (string) tdl.PaymentTypeID,
                            CurrencyCode = tdl.Denominator.CurrencyCode,
                            StaffID = "",
                            TerminalID = td.TerminalID,
                            Amount = tdl.Denominator.Amount*tdl.Quantity,
                            IsSCTenderDeclaration = true,
                            TransactionTime = (DateTime) tdl.CountedDateTime
                        };

                    transactions.Add(transaction);   
                }
            }

            return transactions;
        }

        /// <summary>
        /// Marks transactions that belong to the given statement
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="startTime">Transactions from startTime to endTime are considered</param>
        /// <param name="endTime">Transactions from startTime to endTime are considered</param>
        /// <param name="storeID">Transactions from this store are considered</param>
        /// <param name="statementID">The transactions are marked with this statement ID</param>
        public virtual void MarkStatementTransactions(IConnectionManager entry, DateTime startTime, DateTime endTime, RecordIdentifier storeID, RecordIdentifier statementID)
        {
            // This stored procedure marks transactions in the following tables
            // 'RBOTRANSACTIONTABLE'
            // 'RBOTRANSACTIONPAYMENTTRANS'
            // 'RBOTRANSACTIONSAFETENDERTRANS'
            // 'RBOTRANSACTIONBANKEDTENDE20338'
            // 'RBOTRANSACTIONTENDERDECLA20165'
            // 'RBOTRANSACTIONSALESTRANS'

            // Mark all transactions within the period having an empty statmentID
            using (var cmd = entry.Connection.CreateCommand("spStatement_MarkTransactions"))
            {
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "StatementID", (string)statementID);
                MakeParam(cmd, "StoreID", (string)storeID);
                MakeParam(cmd, "StartingDate", startTime, SqlDbType.DateTime);
                MakeParam(cmd, "EndDate", endTime, SqlDbType.DateTime);

                entry.Connection.ExecuteNonQuery(cmd, false);
            }
        }

        private static void UnmarkTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            UnmarkTransTableTransactions(entry, statementID, storeID);
            UnmarkPaymentTransactions(entry, statementID, storeID);
            UnmarkSafeTransactions(entry, statementID, storeID);
            UnmarkTenderDeclTransactions(entry, statementID, storeID);
            UnmarkBankedTransactions(entry, statementID, storeID);
            UnmarkSalesTransactions(entry, statementID, storeID);
        }

        private static void UnmarkTransTableTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONTABLE");

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);
            
            statement.AddField("STATEMENTID", "");

            entry.Connection.ExecuteStatement(statement);
        }

        private static void UnmarkPaymentTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONPAYMENTTRANS") {StatementType = StatementType.Update};

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);
            
            statement.AddField("STATEMENTID", "");

            entry.Connection.ExecuteStatement(statement);
        }

        private static void UnmarkSafeTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONBANKEDTENDE20338")
                {
                    StatementType = StatementType.Update
                };

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);
            
            statement.AddField("STATEMENTID", "");

            entry.Connection.ExecuteStatement(statement);
        }

        private static void UnmarkTenderDeclTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            // Unmark the regular tender declarations
            var statement = new SqlServerStatement("RBOTRANSACTIONTENDERDECLA20165")
                {
                    StatementType = StatementType.Update
                };

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);
            
            statement.AddField("STATEMENTID", "");

            entry.Connection.ExecuteStatement(statement);

            // Unmark the SC tender declarations
            var tenderDeclarations = DataProviderFactory.Instance.Get<ITenderDeclarationData,Tenderdeclaration>().GetTenderDeclarationsForAStatementID(entry, storeID, (string)statementID);

            foreach (var td in tenderDeclarations)
            {
                td.StatementID = "";
                DataProviderFactory.Instance.Get<ITenderDeclarationData, Tenderdeclaration>().Save(entry, td);
            }
        }

        private static void UnmarkBankedTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONSAFETENDERTRANS")
                {
                    StatementType = StatementType.Update
                };

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);
            
            statement.AddField("STATEMENTID", "");

            entry.Connection.ExecuteStatement(statement);
        }

        private static void UnmarkSalesTransactions(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONSALESTRANS") { StatementType = StatementType.Update };

            ValidateSecurity(entry, Permission.RunEndOfDay);

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("STATEMENTID", (string)statementID);
            statement.AddCondition("STORE", (string)storeID);

            statement.AddField("STATEMENTID", "0");

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void ERPProcessStatement(IConnectionManager entry, RecordIdentifier statementID)
        {
            var statementToProcess = Get(entry, statementID);
            statementToProcess.ERPProcessed = true;
            Save(entry, statementToProcess);
        }

        public virtual List<StatementInfo> GetPostedStatementsAfterDate(IConnectionManager entry, DateTime date, RecordIdentifier storeID = null)
        {
            ValidateSecurity(entry);

            List<StatementInfo> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SelectFields() + $"WHERE DATAAREAID = @dataAreaID AND POSTED = 1 AND POSTINGDATE > @date";

                if(storeID != null)
                {
                    cmd.CommandText += " AND STOREID = @storeID";
                    MakeParam(cmd, "storeID", (string)storeID);
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "date", date.Date, SqlDbType.Date);

                result = Execute<StatementInfo>(entry, cmd, CommandType.Text, PopulateStatementInfo);
            }

            foreach (var statement in result)
            {
                if (statement != null)
                {
                    statement.statementLines = Providers.StatementLineData.GetStatementLines(entry, statement.ID);
                }
            }

            return result;
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public virtual RecordIdentifier SequenceID
        {
            get { return "StatementInfo"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOSTATEMENTTABLE", "STATEMENTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}