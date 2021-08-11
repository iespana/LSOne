using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.DataProviders.Ledger;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.Ledger
{
	/// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="CustomerLedgerEntries"/>
	/// </summary>
    public class CustomerLedgerEntriesData : SqlServerDataProviderBase, ICustomerLedgerEntriesData
	{
        private static List<TableColumn> BaseColumns = new List<TableColumn>()
        {
            new TableColumn { ColumnName = "ENTRYNO", TableAlias = "U", ColumnAlias = "ENTRYNO", IsNull = true, NullValue = "0" },
            new TableColumn { ColumnName = "DATAAREAID", TableAlias = "U", ColumnAlias = "DATAAREAID", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "POSTINGDATE", TableAlias = "U", ColumnAlias = "POSTINGDATE", IsNull = true, NullValue = "'1900-01-01 00:00:00.000'" },
            new TableColumn { ColumnName = "CUSTOMER", TableAlias = "U", ColumnAlias = "CUSTOMER", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "TYPE", TableAlias = "U", ColumnAlias = "TYPE", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "DOCUMENTNO", TableAlias = "U", ColumnAlias = "DOCUMENTNO", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "DESCRIPTION", TableAlias = "U", ColumnAlias = "DESCRIPTION", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "CURRENCY", TableAlias = "U", ColumnAlias = "CURRENCY", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "CURRENCYAMOUNT", TableAlias = "U", ColumnAlias = "CURRENCYAMOUNT", IsNull = true, NullValue = "0" },
            new TableColumn { ColumnName = "AMOUNT", TableAlias = "U", ColumnAlias = "AMOUNT", IsNull = true, NullValue = "0" },
            new TableColumn { ColumnName = "REMAININGAMOUNT", TableAlias = "U", ColumnAlias = "REMAININGAMOUNT", IsNull = true, NullValue = "0" },
            new TableColumn { ColumnName = "STOREID", TableAlias = "U", ColumnAlias = "STOREID", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "TERMINALID", TableAlias = "U", ColumnAlias = "TERMINALID", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "TRANSACTIONID", TableAlias = "R", ColumnAlias = "TRANSACTIONID", IsNull = true, NullValue = "ISNULL(U.TRANSACTIONID, '')" },
            new TableColumn { ColumnName = "RECEIPTID", TableAlias = "U", ColumnAlias = "RECEIPTID", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "STATUS", TableAlias = "U", ColumnAlias = "STATUS", IsNull = true, NullValue = "0" },
            new TableColumn { ColumnName = "USERID", TableAlias = "U", ColumnAlias = "USERID", IsNull = true, NullValue = "'00000000-0000-0000-0000-000000000000'" },
        };

        protected virtual void Populate(IDataReader dr, CustomerLedgerEntries rec)
		{
            rec.ID = (int)dr["ENTRYNO"];
            rec.DataAreaId = (string)dr["DATAAREAID"];
            rec.PostingDate = (DateTime)dr["POSTINGDATE"];
            rec.Customer = (string)dr["CUSTOMER"];
            rec.EntryType = (CustomerLedgerEntries.TypeEnum)dr["TYPE"];
            rec.DocumentNo = (string)dr["DOCUMENTNO"];
            rec.Description = (string)dr["DESCRIPTION"];
            rec.Currency = (string)dr["CURRENCY"];
            rec.CurrencyAmount = (decimal)dr["CURRENCYAMOUNT"];
            rec.Amount = (decimal)dr["AMOUNT"];
            rec.RemainingAmount = (decimal)dr["REMAININGAMOUNT"];
            rec.StoreId = (string)dr["STOREID"];
            rec.TerminalId = (string)dr["TERMINALID"];
            rec.TransactionId = (string)dr["TRANSACTIONID"];
            rec.ReceiptId = (string)dr["RECEIPTID"];
            rec.Status = (CustomerLedgerEntries.StatusEnum)dr["STATUS"];
            rec.UserId = (Guid)dr["USERID"];
		}

        protected virtual void PopulateWithCount(IConnectionManager entry, IDataReader dr, CustomerLedgerEntries item, ref int rowCount)
        {
            Populate(dr, item);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["ROW_COUNT"];
            }
        }

        /// <summary>
        /// Gets the specified entry.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerLedgerEntryID">Customer ledger etries number</param>
        /// <returns>An instance of <see cref="CustomerLedgerEntries"/></returns>
        public virtual CustomerLedgerEntries Get(IConnectionManager entry, RecordIdentifier customerLedgerEntryID)
		{
            List<CustomerLedgerEntries> result;

			using (var cmd = entry.Connection.CreateCommand())
			{
                Condition condition = new Condition
                {
                    Operator = "AND",
                    ConditionValue = "U.ENTRYNO = @entryno AND U.DATAAREAID = @dataAreaId"
                };

                List<Join> joins = new List<Join>
                {
                    new Join
                    {
                        JoinType = "LEFT",
                        Table = "RBOTRANSACTIONTABLE",
                        TableAlias = "R",
                        Condition = "R.RECEIPTID = U.RECEIPTID AND R.TERMINAL = U.TERMINALID AND R.RECEIPTID = U.RECEIPTID AND R.CUSTACCOUNT = U.CUSTOMER AND R.ENTRYSTATUS <> 5"
                    }
                };

                MakeParam(cmd, "entryno", (string)customerLedgerEntryID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("CUSTOMERLEDGERENTRIES", "U"),
                    QueryPartGenerator.InternalColumnGenerator(BaseColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(condition),
                    string.Empty);

                result = Execute<CustomerLedgerEntries>(entry, cmd, CommandType.Text, Populate);
			}

			return result.Count > 0 ? result[0] : null;
		}

        public List<CustomerLedgerEntries> GetList(IConnectionManager entry, RecordIdentifier customerId, out int totalRecords, CustomerLedgerFilter filter)
        {
            List<CustomerLedgerEntries> result;
            totalRecords = 0;

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                List<TableColumn> columns = new List<TableColumn>();
                List<Join> joins = new List<Join>();
                List<Condition> externalConditions = new List<Condition>();

                string sort = "U.POSTINGDATE ASC";
                string externalSort = "SS.POSTINGDATE ASC";

                if (filter.SortBackwards)
                {
                    sort = sort.Replace("ASC", "DESC");
                    externalSort = externalSort.Replace("ASC", "DESC");
                }

                joins.Add(new Join
                {
                    JoinType = "LEFT",
                    Table = "RBOTRANSACTIONTABLE",
                    TableAlias = "R",
                    Condition = "R.RECEIPTID = U.RECEIPTID AND R.TERMINAL = U.TERMINALID AND R.RECEIPTID = U.RECEIPTID AND R.CUSTACCOUNT = U.CUSTOMER AND R.ENTRYSTATUS <> 5"
                });

                columns.AddRange(BaseColumns);
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("ROW_NUMBER() OVER(ORDER BY {0})", sort),
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = string.Format("COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )", sort),
                    ColumnAlias = "ROW_COUNT"
                });
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "SS.ROW BETWEEN @rowFrom AND @rowTo"
                });
                MakeParam(cmd, "rowFrom", filter.RowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", filter.RowTo, SqlDbType.Int);

                #region filtering

                if (!string.IsNullOrEmpty(filter.Receipt))
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "(U.RECEIPTID LIKE @receiptID OR U.DOCUMENTNO LIKE @docNo)",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "receiptID", filter.Receipt + "%");
                    MakeParam(cmd, "docNo", filter.Receipt + "%");
                }

                if (filter.StoreID != null && filter.StoreID.StringValue != "")
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.STOREID = @storeId",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "storeId", filter.StoreID.StringValue);
                }

                if (filter.TerminalID != null && filter.TerminalID.StringValue != "")
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.TERMINALID = @terminalId",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "terminalId", filter.TerminalID.StringValue);
                }

                if (filter.FromDate != null && !filter.FromDate.IsEmpty)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.POSTINGDATE >= @dateFrom",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "dateFrom", filter.FromDate.DateTime.Date, SqlDbType.DateTime);
                }

                if (filter.ToDate != null && !filter.ToDate.IsEmpty)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.POSTINGDATE < @toDate",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "toDate", filter.ToDate.DateTime.Date.AddDays(1).AddSeconds(-1), SqlDbType.DateTime);
                }

                if (customerId != null && customerId.StringValue != "")
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.CUSTOMER = @customerId",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "customerId", customerId.StringValue);
                }

                if (filter.Types > 0)
                {
                    string typeStr = "";                
                    if ((filter.Types & 1) != 0)
                    {
                        typeStr = "0,2";
                    }
                    if ((filter.Types & 2) != 0)
                    {
                        if (typeStr != "")
                        {
                            typeStr += ",";
                        }
                        typeStr += "1";
                    }
                    if ((filter.Types & 4) != 0)
                    {
                        if (typeStr != "")
                        {
                            typeStr += ",";
                        }
                        typeStr += "3,4";
                    }

                    conditions.Add(new Condition
                    {
                        ConditionValue = "U.TYPE IN(" + typeStr + ")",
                        Operator = "AND"
                    });
                }

                #endregion filtering

                conditions.Add(new Condition
                {
                    ConditionValue = "U.DATAAREAID = @dataAreaId",
                    Operator = "AND"
                });

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("CUSTOMERLEDGERENTRIES", "U", "SS"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "SS"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Format("ORDER BY {0}", externalSort));

                result = Execute<CustomerLedgerEntries, int>(entry, cmd, CommandType.Text, ref totalRecords, PopulateWithCount);
            }
            return result;
        }

        /// <summary>
        /// ID is the entryNo in the database
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ID"></param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "CUSTOMERLEDGERENTRIES", "ENTRYNO", ID, new[] { BusinessObjects.Permission.VoidPayment, BusinessObjects.Permission.CustomerLedgerEntriesEdit });
        }

	    public virtual bool Exists(IConnectionManager entry, RecordIdentifier customerLedgerEntryID)
		{
            return RecordExists(entry, "CUSTOMERLEDGERENTRIES", "ENTRYNO", customerLedgerEntryID);
		}

        public virtual void Save(IConnectionManager entry, CustomerLedgerEntries custLedgerEtr)
		{
			bool isNew = false;
            var statement = entry.Connection.CreateStatement("CUSTOMERLEDGERENTRIES");

            if (custLedgerEtr.ID.IsEmpty)
            {
				isNew = true;
			}

			if (isNew || !Exists(entry, custLedgerEtr.ID))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
			}
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ENTRYNO", custLedgerEtr.EntryNo, SqlDbType.Int);
			}

            statement.AddField("POSTINGDATE", custLedgerEtr.PostingDate, SqlDbType.DateTime);
            statement.AddField("CUSTOMER", (string)custLedgerEtr.Customer);
            statement.AddField("TYPE", custLedgerEtr.EntryType, SqlDbType.Int);
            statement.AddField("DOCUMENTNO", (string)custLedgerEtr.DocumentNo);
            statement.AddField("DESCRIPTION", custLedgerEtr.Description);
            statement.AddField("CURRENCY", custLedgerEtr.Currency);
            statement.AddField("CURRENCYAMOUNT", custLedgerEtr.CurrencyAmount, SqlDbType.Decimal);
            statement.AddField("AMOUNT", custLedgerEtr.Amount, SqlDbType.Decimal);
            statement.AddField("REMAININGAMOUNT", custLedgerEtr.RemainingAmount, SqlDbType.Decimal);
            statement.AddField("STOREID", (string)custLedgerEtr.StoreId);
            statement.AddField("TERMINALID", (string)custLedgerEtr.TerminalId);
            statement.AddField("TRANSACTIONID", (string)custLedgerEtr.TransactionId);
            statement.AddField("RECEIPTID", (string)custLedgerEtr.ReceiptId);
            statement.AddField("STATUS", custLedgerEtr.Status, SqlDbType.Int);
            statement.AddField("USERID", custLedgerEtr.UserId, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
		}

        public virtual int GetMaxEntryNo(IConnectionManager entry)
        {
            decimal? result = 0;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT 
                    MAX([ENTRYNO]) 
                    FROM [CUSTOMERLEDGERENTRIES] 
                    WHERE DATAAREAID = @dataAreaId; ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var returnValue = entry.Connection.ExecuteScalar(cmd);

                if (DBNull.Value != returnValue)
                    result = (int?)returnValue;
            }

            return (result == null) ? 0 : (int)result;
        }

        public virtual bool UpdateRemainingAmount(IConnectionManager entry, RecordIdentifier CustID)
		{
			int? ret;

            using (var cmd = entry.Connection.CreateCommand("spCUSTOMER_UpdateRemainingAmount"))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CustID", (string)CustID);

				// Return value as parameter
				var returnValue = entry.Connection.CreateParam("returnVal", SqlDbType.Int);
				returnValue.Direction = ParameterDirection.ReturnValue;
				cmd.Parameters.Add(returnValue);

				entry.Connection.ExecuteNonQuery(cmd, false);

				ret = (int?)returnValue.Value;
			}

			// ret:
			// 0 - OK
			// 1 - "Update error";

			return (ret==0);
		}

        /// <summary>
        /// Gets customer total sales
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Total sales</returns>
        public virtual decimal GetCustomerTotalSales(IConnectionManager entry, RecordIdentifier customerId)
        {
            decimal? result = 0;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT SUM(Amount)                                
                 FROM CUSTOMERLEDGERENTRIES 
                    WHERE TYPE IN (1,2,3) AND DATAAREAID = @dataAreaId AND [CUSTOMER] = @customerId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerId", customerId);

                var returnValue = entry.Connection.ExecuteScalar(cmd);
                if (DBNull.Value != returnValue)
                    result = (decimal?)returnValue;
            }

            return (result == null) ? 0 : (decimal)result;
        }

        /// <summary>
        /// Gets customer balance from local database
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Customer balance</returns>
        public virtual decimal GetCustomerBalance(IConnectionManager entry, RecordIdentifier customerId)
        {
            decimal? result = 0;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT SUM(RemainingAmount) 
                                    FROM CUSTOMERLEDGERENTRIES 
                                    WHERE TYPE IN (0,1,2) AND STATUS=1 AND DATAAREAID = @dataAreaId AND [CUSTOMER] = @customerId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerId", customerId);

                var returnValue = entry.Connection.ExecuteScalar(cmd);
                if (DBNull.Value != returnValue)
                    result = (decimal?)returnValue;
            }

            return (result == null) ? 0 : (decimal)result;

        }

        public virtual int RecreateCustomerLedger(IConnectionManager entry, RecordIdentifier customerID, RecordIdentifier statementID)
        {
            int? ret;

            using (var cmd = entry.Connection.CreateCommand("spCUSTOMER_RecreateCustomerLedger"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "customerID", customerID);
                MakeParam(cmd, "statementID", statementID);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "UserID", new Guid(entry.CurrentUser.ID.ToString()), SqlDbType.UniqueIdentifier);

                // Return value as parameter
                var returnValue = entry.Connection.CreateParam("returnVal", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnValue);

                entry.Connection.ExecuteNonQuery(cmd, false);

                ret = (int?)returnValue.Value;
            }

            // ret:
            // 0 - OK
            // 3 - "Insert error";

            return (int)ret;
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CUSTLEDGERENTRIES"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CUSTOMERLEDGERENTRIES", "ENTRYNO", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}

