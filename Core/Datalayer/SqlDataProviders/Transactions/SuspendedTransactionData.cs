using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class SuspendedTransactionData : SqlServerDataProviderBase, ISuspendedTransactionData
    {
        private static string BaseSql
        {
            get
            {
                return "SELECT t.TRANSACTIONXML  " +
                            ", t.STAFF AS STAFFID  " +
                            ", t.STOREID  " +
                            ", t.TERMINALID " +
                            ", t.TRANSDATE AS TRANSACTIONDATE " +
                            ", ISNULL(t.RECALLEDBY,'') AS RECALLEDBYID " +
                            ", t.TRANSACTIONID " +
                            ", ISNULL(t.LOCALLYSAVED, 0) AS LOCALLYSAVED " +
                            ", ISNULL(t.DESCRIPTION, '') AS DESCRIPTION " +
                            ", ISNULL(t.ALLOWEOD, 3) AS ALLOWEOD " +
                            ", ISNULL(t.SUSPENSIONTYPEID, '') AS SUSPENSIONTYPEID " +
                            ", ISNULL(t.BALANCE, 0) AS BALANCE " +
                            ", ISNULL(t.BALANCEWITHTAX, 0) AS BALANCEWITHTAX " +
                            ", ISNULL(d.NAME,'') AS TERMINALNAME " +
                            ", ISNULL(s.NAME,'') AS STORENAME " +
                            ", addinfo.ID " +
                            ", ISNULL(addinfo.PROMPT,'') AS PROMPT " +
                            ", ISNULL(addinfo.FIELDORDER,'') AS FIELDORDER " +
                            ", ISNULL(addinfo.INFOTYPE,'') AS INFOTYPE " +
                            ", ISNULL(addinfo.INFOTYPESELECTION,'') AS INFOTYPESELECTION " +
                            ", ISNULL(addinfo.TEXTRESULT1,'') AS TEXTRESULT1 " +
                            ", ISNULL(addinfo.TEXTRESULT2,'') AS TEXTRESULT2 " +
                            ", ISNULL(addinfo.TEXTRESULT3,'') AS TEXTRESULT3 " +
                            ", ISNULL(addinfo.TEXTRESULT4,'') AS TEXTRESULT4 " +
                            ", ISNULL(addinfo.TEXTRESULT5,'') AS TEXTRESULT5 " +
                            ", ISNULL(addinfo.TEXTRESULT6,'') AS TEXTRESULT6 " +
                            ", ISNULL(addinfo.ADDRESSFORMAT,'') AS ADDRESSFORMAT " +
                            ", ISNULL(addinfo.DATERESULT,'01.01.1900') AS DATERESULT " +
                            ", ISNULL(c.NAME,'') AS NAME " +
                            ", ISNULL(c.FIRSTNAME,'') AS FIRSTNAME " +
                            ", ISNULL(c.MIDDLENAME,'') AS MIDDLENAME " +
                            ", ISNULL(c.LASTNAME,'') AS LASTNAME " +
                            ", ISNULL( c.NAMEPREFIX,'') AS NAMEPREFIX " +
                            ", ISNULL(c.NAMESUFFIX,'') AS NAMESUFFIX " +
                            ", ISNULL(stype.DESCRIPTION,'') AS SUSPENSIONTYPENAME" +
                            ", ISNULL(U.Login,'') AS LOGIN" +

                            " FROM POSISSUSPENDEDTRANSACTIONS t " +

                            " left outer join POSISSUSPENSIONTYPE stype on t.SUSPENSIONTYPEID = stype.ID and t.DATAAREAID = stype.DATAAREAID " +
                            " left outer join RBOSTORETABLE s on t.STOREID = s.STOREID and t.DATAAREAID = s.DATAAREAID  " +
                            " left outer join RBOTERMINALTABLE terminal on t.TERMINALID = terminal.TERMINALID and terminal.STOREID = t.STOREID and terminal.DATAAREAID = s.DATAAREAID " +
                            " left outer join POSISSUSPENDTRANSADDINFO addinfo on addinfo.TRANSACTIONID = t.TRANSACTIONID and addinfo.DATAAREAID = t.DATAAREAID " +
                            "   AND addinfo.FIELDORDER = (select MIN(FIELDORDER) as FIELDORDER from POSISSUSPENDTRANSADDINFO fo where fo.TRANSACTIONID = t.TRANSACTIONID and fo.DATAAREAID = t.DATAAREAID) " +
                            " left outer join RBOSTORETABLE st on t.STOREID = st.STOREID AND t.DATAAREAID = st.DATAAREAID " +
                            " left outer join RBOTERMINALTABLE d on t.TERMINALID = d.TERMINALID AND d.STOREID = t.STOREID AND t.DATAAREAID = d.DATAAREAID  " +
                            " left outer join CUSTOMER c on addinfo.TEXTRESULT1 = c.ACCOUNTNUM  and addinfo.DATAAREAID = c.DATAAREAID " +
                            " LEFT OUTER JOIN USERS U ON t.STAFF = U.STAFFID ";                        
            }
        }

        private static string ResolveSort(SuspendedTransaction.SortEnum sortEnum, bool sortBackwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case SuspendedTransaction.SortEnum.TransactionDate:
                    sortString = "order by t.TRANSDATE ASC ";
                    break;
                case SuspendedTransaction.SortEnum.StaffID:
                    sortString = "order by U.LOGIN ASC ";
                    break;
                case SuspendedTransaction.SortEnum.StoreID:
                    sortString = "order by t.STOREID ASC ";
                    break;
                case SuspendedTransaction.SortEnum.TerminalID:
                    sortString = "order by t.TERMINALID ASC ";
                    break;
                case SuspendedTransaction.SortEnum.RecalledByID:
                    sortString = "order by t.RECALLEDBYID ASC ";
                    break;
                case SuspendedTransaction.SortEnum.TransactionID:
                    sortString = "order by t.TRANSACTIONID ASC ";
                    break;
                case SuspendedTransaction.SortEnum.Description:
                    sortString = "order by t.DESCRIPTION ASC ";
                    break;
                case SuspendedTransaction.SortEnum.AllowEOD:
                    sortString = "order by t.ALLOWEOD ASC ";
                    break;
                case SuspendedTransaction.SortEnum.Active:
                    sortString = "order by t.ACTIVE ASC ";
                    break;
                case SuspendedTransaction.SortEnum.SuspensionTypeID:
                    sortString = "order by t.SUSPENSIONTYPEID ASC ";
                    break;
            }
            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateSuspendedTransaction(IConnectionManager entry,IDataReader dr, SuspendedTransaction suspendedTransaction,object param)
        {
            suspendedTransaction.ID = (string)dr["TRANSACTIONID"];
            suspendedTransaction.Text = (string)dr["DESCRIPTION"];
            suspendedTransaction.TransactionDate = (DateTime)dr["TRANSACTIONDATE"];
            suspendedTransaction.StaffID = (string)dr["STAFFID"];
            suspendedTransaction.StoreID = (string)dr["STOREID"];
            suspendedTransaction.TerminalID = (string)dr["TERMINALID"];
            suspendedTransaction.TransactionXML = dr["TRANSACTIONXML"].ToString();
            suspendedTransaction.RecalledByID = (string)dr["RECALLEDBYID"];
            suspendedTransaction.AllowStatementPosting = (SuspendedTransactionsStatementPostingEnum)dr["ALLOWEOD"];
            suspendedTransaction.StoreName = (string)dr["STORENAME"];
            suspendedTransaction.TerminalName = (string)dr["TERMINALNAME"];
            suspendedTransaction.SuspensionTypeID = (string)dr["SUSPENSIONTYPEID"];
            suspendedTransaction.SuspensionTypeName = (string)dr["SUSPENSIONTYPENAME"];
            suspendedTransaction.Balance = (decimal)dr["BALANCE"];
            suspendedTransaction.BalanceWithTax = (decimal)dr["BALANCEWITHTAX"];
            suspendedTransaction.IsLocallySuspended = (byte)dr["LOCALLYSAVED"] == 1;
            suspendedTransaction.Login = (string)dr["LOGIN"];

            var answer = new SuspendedTransactionAnswer();

            if (dr["ID"] != DBNull.Value)
            {
                SuspendedTransactionAnswerData.PopulateSuspendedTransactionAnswer(dr, answer);
            }
            else
            {
                answer.SerializationTextResult1 = "";
                answer.InformationType = SuspensionTypeAdditionalInfo.InfoTypeEnum.Text;
            }

            suspendedTransaction.Text = answer.ToString(entry.Settings.LocalizationContext);
        }

        public SuspendedTransaction Get(
            IConnectionManager entry, 
            RecordIdentifier transactionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSql +
                                  " WHERE t.DATAAREAID = @dataAreaID AND t.TRANSACTIONID = @transactionID ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", (string)transactionID);

                var suspendedTransactions = Execute<SuspendedTransaction>(entry, cmd, CommandType.Text, null,PopulateSuspendedTransaction);
                return suspendedTransactions.Count > 0 ? suspendedTransactions[0] : null;
            }
        }

        public int GetCount(
            IConnectionManager entry,
            RecordIdentifier storeId,
            RecordIdentifier terminalId,
            RecordIdentifier suspendedTransactionType,
            RetrieveSuspendedTransactions whatToRetrieve)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = " SELECT COALESCE(COUNT(TRANSACTIONID), 0) AS SUSPENSIONCOUNT " +
                                  " FROM POSISSUSPENDEDTRANSACTIONS " +
                                  " WHERE DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (storeId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND STOREID = @STOREID ";
                    MakeParam(cmd, "STOREID", storeId.ToString());
                }

                if (terminalId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND TERMINALID = @TERMINALID ";
                    MakeParam(cmd, "TERMINALID", (string)terminalId);
                }

                if (whatToRetrieve != RetrieveSuspendedTransactions.All)
                {
                    cmd.CommandText += " AND LOCALLYSAVED = @LOCALLYSAVED ";
                    MakeParam(cmd, "LOCALLYSAVED", whatToRetrieve == RetrieveSuspendedTransactions.OnlyLocallySaved ? 1 : 0);
                }

                if (!suspendedTransactionType.IsEmpty)
                {
                   cmd.CommandText += " AND SUSPENSIONTYPEID = @SUSPENSIONTYPEID ";
                   MakeParam(cmd, "SUSPENSIONTYPEID", (string)suspendedTransactionType);
                }

                return (int)entry.Connection.ExecuteScalar(cmd);                
            }
        }

        public List<SuspendedTransaction> GetList(
            IConnectionManager entry,
            SuspendedTransaction.SortEnum sortEnum, 
            bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSql + " Where t.DATAAREAID = @dataAreaID " +
                                  " AND (addinfo.id is NULL or addinfo.FIELDORDER in (Select min(FIELDORDER) from POSISSUSPENDTRANSADDINFO addinfo2 where addinfo2.TRANSACTIONID = addinfo.TRANSACTIONID and addinfo2.DATAAREAID = addinfo.DATAAREAID)) " +
                                  ResolveSort(sortEnum, sortBackwards); 

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<SuspendedTransaction>(entry, cmd, CommandType.Text,null, PopulateSuspendedTransaction);
            }
        }

        /// <summary>
        /// Get a list of suspended transactions for a given suspension transaction type and a certain store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="suspensionTransactionTypeID">The ID of the suspension transaction type </param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="sortEnum">Enum which decides in what order the result set is returned</param>
        /// <param name="sortBackwards">Whether to reverse the sorting of the result set</param>
        /// <param name="dateFrom">The from date of the transaction. Use Date.Empty to ignore</param>
        /// <param name="dateTo">The to date of the transaction. Use Date.Empty to ignore</param>
        /// <param name="terminalId"> The Id of the terminal</param>
        /// <returns></returns>
        public List<SuspendedTransaction> GetListForTypeAndStore(
            IConnectionManager entry,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID,
            Date dateFrom,
            Date dateTo,
            SuspendedTransaction.SortEnum sortEnum, 
            bool sortBackwards,
            RecordIdentifier terminalId = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                bool hasCondition = false;

                ValidateSecurity(entry);

                cmd.CommandText = BaseSql;

                if (suspensionTransactionTypeID != RecordIdentifier.Empty)
                {
                    cmd.CommandText +=  hasCondition ? " and " : " where ";
                    cmd.CommandText += "t.SUSPENSIONTYPEID = @suspensionTypeID ";
                                        
                    MakeParam(cmd, "suspensionTypeID", (string)suspensionTransactionTypeID);

                    hasCondition = true;
                }

                if (storeID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += hasCondition ? " and " : " where ";
                    cmd.CommandText += "t.STOREID = @storeID ";
                        
                    MakeParam(cmd, "storeID", (string)storeID);

                    hasCondition = true;
                }
                
                if (terminalId != null && terminalId != RecordIdentifier.Empty)
                {
                    cmd.CommandText += hasCondition ? " and " : " where ";
                    cmd.CommandText += "t.TERMINALID = @terminalID ";

                    MakeParam(cmd, "terminalID", (string)terminalId);
                    hasCondition = true;
                }

                if (dateFrom != Date.Empty)
                {
                    dateFrom = dateFrom.GetDateWithoutTime();

                    cmd.CommandText += hasCondition ? " and " : " where ";
                    cmd.CommandText += "t.TRANSDATE >= @fromDate ";

                    MakeParam(cmd, "fromDate", new DateTime(dateFrom.DateTime.Year, dateFrom.DateTime.Month, dateFrom.DateTime.Day, 0, 0, 0), SqlDbType.DateTime);

                    hasCondition = true;
                }

                if (dateTo != Date.Empty)
                {
                    dateTo = dateTo.GetDateWithoutTime();

                    cmd.CommandText += hasCondition ? " and " : " where ";
                    cmd.CommandText += "t.TRANSDATE <= @toDate ";

                    MakeParam(cmd, "toDate", new DateTime(dateTo.DateTime.Year, dateTo.DateTime.Month, dateTo.DateTime.Day, 23, 59, 59), SqlDbType.DateTime);

                    hasCondition = true;
                }

                if (suspensionTransactionTypeID == RecordIdentifier.Empty && dateFrom == Date.Empty && dateTo == Date.Empty && storeID == RecordIdentifier.Empty && terminalId == RecordIdentifier.Empty)
                {
                    cmd.CommandText += " WHERE t.DATAAREAID = @dataAreaId " +
                                   "  AND (addinfo.id is NULL or addinfo.FIELDORDER in (Select min(FIELDORDER) from POSISSUSPENDTRANSADDINFO addinfo2 where addinfo2.TRANSACTIONID = addinfo.TRANSACTIONID and addinfo2.DATAAREAID = addinfo.DATAAREAID)) "
                                   + ResolveSort(sortEnum, sortBackwards); 
                }
                else
                {
                    cmd.CommandText += " AND t.DATAAREAID = @dataAreaId " +
                                  "  AND (addinfo.id is NULL or addinfo.FIELDORDER in (Select min(FIELDORDER) from POSISSUSPENDTRANSADDINFO addinfo2 where addinfo2.TRANSACTIONID = addinfo.TRANSACTIONID and addinfo2.DATAAREAID = addinfo.DATAAREAID)) " +
                     ResolveSort(sortEnum, sortBackwards);
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<SuspendedTransaction>(entry, cmd, CommandType.Text, null,PopulateSuspendedTransaction);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier suspendedTransactionID)
        {
            return RecordExists(
                entry, 
                "POSISSUSPENDEDTRANSACTIONS", 
                "TRANSACTIONID", 
                suspendedTransactionID);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier suspendedTransactionID)
        {
            DeleteRecord(
                entry,
                "POSISSUSPENDEDTRANSACTIONS",
                "TRANSACTIONID",
                suspendedTransactionID,
                LSOne.DataLayer.BusinessObjects.Permission.ManageCentralSuspensions);
        }

        public virtual void Save(IConnectionManager entry, SuspendedTransaction suspendedTransaction)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCentralSuspensions);

            suspendedTransaction.Validate();

            var statement = new SqlServerStatement("POSISSUSPENDEDTRANSACTIONS");

            if (suspendedTransaction.ID == RecordIdentifier.Empty)
            {
                suspendedTransaction.ID = DataProviderFactory.Instance.GenerateNumber<ISuspendedTransactionData, SuspendedTransaction>(entry);
            }

            if (!Exists(entry, suspendedTransaction.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TRANSACTIONID", (string)suspendedTransaction.ID);
            }             
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("TRANSACTIONID", (string)suspendedTransaction.ID);
            }

            statement.AddField("STAFF", (string)suspendedTransaction.StaffID);
            statement.AddField("STOREID", (string)suspendedTransaction.StoreID);
            statement.AddField("TERMINALID", (string)suspendedTransaction.TerminalID);
            statement.AddField("RECALLEDBY", (string)suspendedTransaction.RecalledByID);
            statement.AddField("DESCRIPTION", suspendedTransaction.Text);
            statement.AddField("TRANSDATE", suspendedTransaction.TransactionDate, SqlDbType.DateTime);
            statement.AddField("ALLOWEOD", (int)suspendedTransaction.AllowStatementPosting, SqlDbType.TinyInt);
            statement.AddField("TRANSACTIONXML", suspendedTransaction.TransactionXML);
            statement.AddField("SUSPENSIONTYPEID", (string)suspendedTransaction.SuspensionTypeID);
            statement.AddField("BALANCE", suspendedTransaction.Balance, SqlDbType.Decimal);
            statement.AddField("BALANCEWITHTAX", suspendedTransaction.BalanceWithTax, SqlDbType.Decimal);
            statement.AddField("LOCALLYSAVED", suspendedTransaction.IsLocallySuspended ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);                        
        }

        /// <summary>
        /// Gets a list of all suspended transactions for a certain store and terminal over a certain period.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store to search for</param>
        /// <param name="terminalID">The terminal to search for. If this is RecordIdentifier.Empty then results for all terminals is given</param>
        /// <param name="fromDate">We get suspended transactions that are older then this date but younger then the toDate</param>
        /// <param name="toDate">We get suspended transactiosn that are newer then this date but older then the fromDate</param>
        /// <returns>list of all suspended transactions for a certain store and terminal</returns>
        public List<SuspendedTransaction> SuspendedTransactionsForStoreAndTerminal(
            IConnectionManager entry, 
            RecordIdentifier storeID, 
            RecordIdentifier terminalID,
            DateTime fromDate,
            DateTime toDate)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSql +
                                 "Where t.DATAAREAID = @dataAreaID  " +
                                 "AND t.STOREID = @storeID " + 
                                 "AND t.TRANSDATE > @fromDate " +
                                 "AND t.TRANSDATE < @toDate ";
                if (terminalID != RecordIdentifier.Empty)
                {
                    cmd.CommandText += "AND t.TERMINALID = @terminalID";
                    MakeParam(cmd, "terminalID", (string)terminalID);
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "fromDate", fromDate, SqlDbType.DateTime);
                MakeParam(cmd, "toDate", toDate, SqlDbType.DateTime);

                return Execute<SuspendedTransaction>(entry, cmd, CommandType.Text, null,PopulateSuspendedTransaction);
            }
        }

        #region ISequenceable Members
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "SUSPENDTRANS"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSISSUSPENDEDTRANSACTIONS", "TRANSACTIONID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
