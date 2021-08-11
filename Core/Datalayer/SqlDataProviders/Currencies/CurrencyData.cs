using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Currencies
{
    /// <summary>
    /// Data provider class for currencies
    /// </summary>
    public class CurrencyData : SqlServerDataProviderBase, ICurrencyData
    {
        private static List<TableColumn> columnList = new List<TableColumn>
        {
            new TableColumn {ColumnName = "CURRENCYCODE", TableAlias = "C"},
            new TableColumn {ColumnName = "ISNULL(C.TXT,'')", TableAlias = "", ColumnAlias = "TXT"},
            new TableColumn {ColumnName = "ISNULL(C.SYMBOL,'')", TableAlias = "", ColumnAlias = "SYMBOL"},
            new TableColumn {ColumnName = "ISNULL(C.CURRENCYPREFIX,'')", TableAlias = "", ColumnAlias = "CURRENCYPREFIX"},
            new TableColumn {ColumnName = "ISNULL(C.CURRENCYSUFFIX,'')", TableAlias = "", ColumnAlias = "CURRENCYSUFFIX"},
            new TableColumn {ColumnName = "ISNULL(C.ROUNDOFFAMOUNT,0.0)", TableAlias = "", ColumnAlias = "ROUNDOFFAMOUNT"},
            new TableColumn {ColumnName = "iSNULL(C.ROUNDOFFTYPEAMOUNT, 0)", TableAlias = "", ColumnAlias = "ROUNDOFFTYPEAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(C.ROUNDOFFPURCH, 0)", TableAlias = "", ColumnAlias = "ROUNDOFFPURCH"},
            new TableColumn {ColumnName = "ISNULL(C.ROUNDOFFTYPEPURCH, 0)", TableAlias = "", ColumnAlias = "ROUNDOFFTYPEPURCH"},
            new TableColumn {ColumnName = "ISNULL(C.ROUNDOFFSALES, 0)", TableAlias = "", ColumnAlias = "ROUNDOFFSALES"},
            new TableColumn {ColumnName = "ISNULL(C.ROUNDOFFTYPESALES, 0)", TableAlias = "", ColumnAlias = "ROUNDOFFTYPESALES"}
        };

        private static string ResolveSort(CurrencySorting sort)
        {
            switch (sort)
            {
                case CurrencySorting.ID:
                    return "CURRENCYCODE";

                case CurrencySorting.Description:
                    return "TXT";
            }

            return "";
        }

        protected virtual void PopulateCurrencyInfoWithCount(IConnectionManager entry, IDataReader dr, Currency currencyInfo, ref int rowCount)
        {
            PopulateCurrencyInfo(dr, currencyInfo);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private static void PopulateCurrencyInfo(IDataReader dr, Currency currencyInfo)
        {
            currencyInfo.ID = (string)dr["CURRENCYCODE"];
            currencyInfo.Text = (string)dr["TXT"];
            currencyInfo.Symbol = (string)dr["SYMBOL"];
            currencyInfo.CurrencyPrefix = (string)dr["CURRENCYPREFIX"];
            currencyInfo.CurrencySuffix = (string)dr["CURRENCYSUFFIX"];

            if (string.IsNullOrEmpty(currencyInfo.Symbol))
            {
                currencyInfo.Symbol = string.IsNullOrEmpty(currencyInfo.CurrencyPrefix) ? currencyInfo.CurrencySuffix : currencyInfo.CurrencyPrefix;
            }

            currencyInfo.RoundOffAmount = (decimal)dr["ROUNDOFFAMOUNT"];
            currencyInfo.RoundOffTypeAmount = (int)dr["ROUNDOFFTYPEAMOUNT"];
            currencyInfo.RoundOffPurchase = (decimal)dr["ROUNDOFFPURCH"];
            currencyInfo.RoundOffTypePurchase = (int)dr["ROUNDOFFTYPEPURCH"];
            currencyInfo.RoundOffSales = (decimal)dr["ROUNDOFFSALES"];
            currencyInfo.RoundOffTypeSales = (int)dr["ROUNDOFFTYPESALES"];
        }

        /// <summary>
        /// Gets a list of DataEntity that contains CurrencyCode and Currency Description. The list is sorted by the column specified in the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, CurrencySorting sortBy, bool sortBackwards)
        {
            if (sortBy != CurrencySorting.ID && sortBy != CurrencySorting.Description)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, "CURRENCY", "TXT", "CURRENCYCODE", ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC"));
        }

        /// <summary>
        /// Gets a list of DataEntity that contains CurrencyCode and Currency Description. The list is sorted by CurrencyCode.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "CURRENCY", "TXT","CURRENCYCODE", "CURRENCYCODE");
        }

        public virtual List<Currency> GetList(IConnectionManager entry, UsageIntentEnum usage)
        {
            if (usage == UsageIntentEnum.Minimal)
            {
                List<DataEntity> returnList = GetList(entry);
                if (returnList != null)
                {
                    return returnList.Cast<Currency>().ToList();
                }
                return null;
            }
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CURRENCYCODE, ";
                cmd.CommandText += "ISNULL(TXT,'') AS TXT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFSALES, 0) as ROUNDOFFSALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFPURCH, 0) as ROUNDOFFPURCH, ";
                cmd.CommandText += "ISNULL(ROUNDOFFAMOUNT,0.0) as ROUNDOFFAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPESALES, 0) as ROUNDOFFTYPESALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEAMOUNT, 0) as ROUNDOFFTYPEAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEPURCH, 0) as ROUNDOFFTYPEPURCH, ";
                cmd.CommandText += "ISNULL(SYMBOL,'') AS SYMBOL, ";
                cmd.CommandText += "ISNULL(CURRENCYPREFIX,'') AS CURRENCYPREFIX, ";
                cmd.CommandText += "ISNULL(CURRENCYSUFFIX,'') AS CURRENCYSUFFIX ";
                cmd.CommandText += "FROM CURRENCY ";
                cmd.CommandText += "WHERE DATAAREAID = @dataareaid ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                return Execute<Currency>(entry, cmd, CommandType.Text, PopulateCurrencyInfo);
            }
        }

        /// <summary>
        /// Gets list of DataEntity that contains CurrencyCode and Currency Description excluding one givenF currency code.  The list is sorted by CurrencyCode.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludeID">ID of the currency to be excluded</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier excludeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select CURRENCYCODE, ISNULL(TXT,'') as TXT from CURRENCY where DATAAREAID = @dataAreaId and CURRENCYCODE <> @excludeID order by CURRENCYCODE";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "excludeID", (string)excludeID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "TXT", "CURRENCYCODE");
            }
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<Currency> GetCompareList(IConnectionManager entry, List<Currency> itemsToCompare)
        {
            return GetCompareListInBatches(entry, itemsToCompare, "CURRENCY", "CURRENCYCODE", columnList, null, PopulateCurrencyInfo);
        }

        /// <summary>
        /// Checks if a Currency with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the currency to check for</param>
        /// <returns>Whether a Currency with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<Currency>(entry, "CURRENCY", "CURRENCYCODE", id);
        }

        /// <summary>
        /// Checks if any store is has a specific currency as it's default currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyID">The unique ID of the currency to check for</param>
        public virtual bool InUse(IConnectionManager entry, RecordIdentifier currencyID)
        {
            return RecordExists(entry, "RBOSTORETABLE", "CURRENCY", currencyID);
        }

        /// <summary>
        /// Deletes a Currency with a given ID
        /// </summary>
        /// <remarks>Requires the 'CurrencyEdit' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the Currency to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<Currency>(entry, "CURRENCY", "CURRENCYCODE", id, Permission.CurrencyEdit);

            entry.Cache.DeleteTypeFromCache(typeof(Currency));
        }              

        /// <summary>
        /// Gets a Currency with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The ID of the Currency to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A Currency with a given ID</returns>
        public virtual Currency Get(IConnectionManager entry, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CURRENCYCODE, ";
                cmd.CommandText += "ISNULL(TXT,'') AS TXT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFSALES, 0) as ROUNDOFFSALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFPURCH, 0) as ROUNDOFFPURCH, ";
                cmd.CommandText += "ISNULL(ROUNDOFFAMOUNT,0.0) as ROUNDOFFAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPESALES, 0) as ROUNDOFFTYPESALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEAMOUNT, 0) as ROUNDOFFTYPEAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEPURCH, 0) as ROUNDOFFTYPEPURCH, ";
                cmd.CommandText += "ISNULL(SYMBOL,'') AS SYMBOL, ";
                cmd.CommandText += "ISNULL(CURRENCYPREFIX,'') AS CURRENCYPREFIX, ";
                cmd.CommandText += "ISNULL(CURRENCYSUFFIX,'') AS CURRENCYSUFFIX ";
                cmd.CommandText += "FROM CURRENCY ";
                cmd.CommandText += "WHERE DATAAREAID = @dataareaid AND CURRENCYCODE = @currcode ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "currcode", (string)currencyCode);

                return Get<Currency>(entry, cmd, currencyCode, PopulateCurrencyInfo,cacheType,UsageIntentEnum.Normal);
            }            
        }

        /// <summary>
        /// Gets the company currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>The company currency</returns>
        public virtual Currency GetCompanyCurrency(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT ISNULL(l.CURRENCYCODE,'') as CURRENCYCODE, ";
                cmd.CommandText += "ISNULL(r.TXT,'') AS TXT, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFSALES, 0) as ROUNDOFFSALES, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFPURCH, 0) as ROUNDOFFPURCH, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFAMOUNT, 0) as ROUNDOFFAMOUNT, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFTYPESALES, 0) as ROUNDOFFTYPESALES, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFTYPEAMOUNT, 0) as ROUNDOFFTYPEAMOUNT, ";
                cmd.CommandText += "ISNULL(r.ROUNDOFFTYPEPURCH, 0) as ROUNDOFFTYPEPURCH, ";
                cmd.CommandText += "ISNULL(r.SYMBOL,'') AS SYMBOL, ";
                cmd.CommandText += "ISNULL(r.CURRENCYPREFIX,'') AS CURRENCYPREFIX, ";
                cmd.CommandText += "ISNULL(r.CURRENCYSUFFIX,'') AS CURRENCYSUFFIX ";
                cmd.CommandText += "FROM COMPANYINFO l ";
                cmd.CommandText += "left outer join CURRENCY r on l.CURRENCYCODE = r.CURRENCYCODE and l.DATAAREAID = r.DATAAREAID ";
                cmd.CommandText += "WHERE l.DATAAREAID = @dataareaid ";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);

                return Get<Currency>(entry, cmd, "-", PopulateCurrencyInfo, cacheType,UsageIntentEnum.Normal);
            }            
        }

        /// <summary>
        /// Gets all currencies in the system except the store currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeCurrency">The unique ID of the store currency</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        public virtual List<Currency> GetNonStoreCurrencies(IConnectionManager entry, RecordIdentifier storeCurrency, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * from CURRENCY ";
                cmd.CommandText += "WHERE DATAAREAID = @dataareaid and CURRENCYCODE != @storecurrencycode";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "storecurrencycode", storeCurrency);

                return Execute<Currency>(entry, cmd, CommandType.Text, PopulateCurrencyInfo);
            }
        }

        /// <summary>
        /// Saves a given Currency into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyInfo">The Currency to save</param>
        public virtual void Save(IConnectionManager entry, Currency currencyInfo)
        {
            var statement = new SqlServerStatement("CURRENCY");

            ValidateSecurity(entry, Permission.CurrencyEdit);

            currencyInfo.Validate();
            statement.UpdateColumnOptimizer = currencyInfo;

            if (!Exists(entry, currencyInfo.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CURRENCYCODE", (string)currencyInfo.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CURRENCYCODE", (string)currencyInfo.ID);

                entry.Cache.DeleteTypeFromCache(typeof(Currency));
            }

            statement.AddField("TXT", currencyInfo.Text);
            statement.AddField("ROUNDOFFTYPEAMOUNT", currencyInfo.RoundOffTypeAmount, SqlDbType.Int);
            statement.AddField("ROUNDOFFTYPESALES", currencyInfo.RoundOffTypeSales, SqlDbType.Int);
            statement.AddField("ROUNDOFFTYPEPURCH", currencyInfo.RoundOffTypePurchase, SqlDbType.Int);
            statement.AddField("ROUNDOFFAMOUNT", currencyInfo.RoundOffAmount, SqlDbType.Decimal);
            statement.AddField("ROUNDOFFSALES", currencyInfo.RoundOffSales, SqlDbType.Decimal);
            statement.AddField("ROUNDOFFPURCH", currencyInfo.RoundOffPurchase, SqlDbType.Decimal);
            statement.AddField("CURRENCYSUFFIX", currencyInfo.CurrencySuffix);
            statement.AddField("CURRENCYPREFIX", currencyInfo.CurrencyPrefix);
            statement.AddField("SYMBOL", currencyInfo.Symbol);

            Save(entry, currencyInfo, statement);
        }

        public virtual List<Currency> LoadCurrencies(
                   IConnectionManager entry,
                   int rowFrom,
                   int rowTo,
                   out int totalRecordsMatching)
        {

            List<TableColumn> listColumns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "CURRENCYCODE", TableAlias = "l"},
                new TableColumn {ColumnName = "TXT", TableAlias = "l",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "ROUNDOFFSALES", TableAlias = "l",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "ROUNDOFFPURCH", TableAlias = "l",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "ROUNDOFFAMOUNT", TableAlias = "l",IsNull = true,NullValue = "0.0"},
                new TableColumn {ColumnName = "ROUNDOFFTYPESALES", TableAlias = "l",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "ROUNDOFFTYPEAMOUNT", TableAlias = "l",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "ROUNDOFFTYPEPURCH", TableAlias = "l",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "SYMBOL", TableAlias = "l",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "CURRENCYPREFIX", TableAlias = "l",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "CURRENCYSUFFIX", TableAlias = "l",IsNull = true,NullValue = "''"},
            };

            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in listColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = "ROW_NUMBER() OVER(order by l.CURRENCYCODE)",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName =
                        "COUNT(1) OVER(ORDER BY l.CURRENCYCODE RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
                    ColumnAlias = "ROW_COUNT"
                });

                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("CURRENCY", "l", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                int matchingRecords = 0;

                var reply = Execute<Currency, int>(entry, cmd, CommandType.Text, ref matchingRecords, PopulateCurrencyInfoWithCount);

                totalRecordsMatching = matchingRecords;
                return reply;
            }
        }
    }
}