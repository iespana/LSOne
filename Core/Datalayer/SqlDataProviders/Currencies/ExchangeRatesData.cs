using System;
using System.Collections.Generic;
using System.Data;
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
    /// Data provider class for currency exchange rates
    /// </summary>
    public class ExchangeRatesData : SqlServerDataProviderBase, IExchangeRatesData
    {
        protected virtual void PopulateExchangeRateWithCount(IConnectionManager entry, IDataReader dr, ExchangeRate exchangeRate, ref int rowCount)

        {
            PopulateExchangeRate(dr, exchangeRate);
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
        private static void PopulateExchangeRate(IDataReader dr, ExchangeRate exchangeRate)
        {
            exchangeRate.FromDate = (DateTime)dr["FROMDATE"];
            exchangeRate.CurrencyCode = (string)dr["CURRENCYCODE"];
            exchangeRate.ExchangeRateValue = (decimal)dr["EXCHANGERATE"];
            exchangeRate.POSExchangeRateValue = (decimal)dr["POSEXCHANGERATE"];
        }

        /// <summary>
        /// Checks if a ExchangeRate with a given ID exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the ExchangeRate to check for</param>
        /// <returns>Whether an ExhangeRate with a given ID exists</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "EXCHRATES", new[]{"CURRENCYCODE","FROMDATE"}, id);
        }

        /// <summary>
        /// Deletes an ExhangeRate with a given ID
        /// </summary>
        /// <remarks>Requires the 'CurrencyEdit' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the ExchangeRate to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "EXCHRATES", new[] { "CURRENCYCODE", "FROMDATE" }, id,
                LSOne.DataLayer.BusinessObjects.Permission.CurrencyEdit);
        }

        /// <summary>
        /// Gets an ExhangeRate with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="exchangeRateID">The ID of the ExchangeRate to get</param>
        /// <returns>An ExchangeRate with a given ID</returns>
        public virtual ExchangeRate Get(IConnectionManager entry, RecordIdentifier exchangeRateID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CURRENCYCODE, 
                                    FROMDATE,
                                    ISNULL(EXCHRATE,0) AS EXCHANGERATE, 
                                    ISNULL(POSEXCHRATE,0) AS POSEXCHANGERATE 
                                    FROM EXCHRATES 
                                    WHERE DATAAREAID = @DATAAREAID 
                                    AND CURRENCYCODE = @CURRENCYCODE 
                                    AND CONVERT(DATE, FROMDATE) = @FROMDATE ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CURRENCYCODE", (string)exchangeRateID.PrimaryID);
                MakeParam(cmd, "FROMDATE", ((DateTime)exchangeRateID.SecondaryID).Date, SqlDbType.Date );

                var result = Execute<ExchangeRate>(entry, cmd, CommandType.Text, PopulateExchangeRate);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        public virtual List<ExchangeRate> GetAllExchangeRates(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CURRENCYCODE, 
                                    FROMDATE, 
                                    ISNULL(EXCHRATE,0) AS EXCHANGERATE, 
                                    ISNULL(POSEXCHRATE,0) AS POSEXCHANGERATE 
                                    FROM EXCHRATES 
                                    WHERE DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAARAEID", entry.Connection.DataAreaId);

                return Execute<ExchangeRate>(entry, cmd, CommandType.Text, PopulateExchangeRate);
            }
        }

        /// <summary>
        /// Gets a list of ExchangeRates for a given Currency code, sorted by a given column index. 'sortedBackwards' decides if we are using ascending or descending ordering.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The currency code to get exchange rates for</param>
        /// <param name="sortColumn">The index of the column to sort by. The columns are ["FROMDATE", "EXCHRATE", "POSEXCHRATE"]</param>
        /// <param name="sortedBackwards">Whether to sort in an descending or ascending order</param>
        /// <returns>A list of ExchangeRates for a Currency</returns>
        public virtual List<ExchangeRate> GetExchangeRates(IConnectionManager entry, RecordIdentifier currencyCode, int sortColumn, bool sortedBackwards)
        {
            ValidateSecurity(entry);

            string[] columns = { "FROMDATE", "EXCHRATE", "POSEXCHRATE" };

            string sort = "";

            if (sortColumn < columns.Length)
            {
                sort = " order by " + columns[sortColumn] + (sortedBackwards ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CURRENCYCODE, 
                                    FROMDATE, 
                                    ISNULL(EXCHRATE,0) AS EXCHANGERATE, 
                                    ISNULL(POSEXCHRATE,0) AS POSEXCHANGERATE 
                                    FROM EXCHRATES 
                                    WHERE DATAAREAID = @DATAAREAID 
                                    AND CURRENCYCODE = @CURRENCYCODE " + sort;

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CURRENCYCODE", (string)currencyCode);

                return Execute<ExchangeRate>(entry, cmd, CommandType.Text, PopulateExchangeRate);
            }
        }

        /// <summary>
        /// Gets the newest exchange rate for a given currency code.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCode">The currency code to get the exchange rate for</param>
        /// <param name="cacheType">Type of cache to be used</param>
        /// <returns>The exchange rate as decimal if found, else 1</returns>
        public virtual decimal GetExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode,CacheType cacheType = CacheType.CacheTypeNone)
        {
            bool exchangeRateExisted;

            return GetExchangeRate(entry, currencyCode, out exchangeRateExisted, cacheType);
        }

        /// <summary>
        /// Gets the newest exchange rate for a given currency code.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCode">The currency code to get the exchange rate for</param>
        /// <param name="exchangeRateExisted">True if the exchange rate existed, else false</param>
        /// <param name="cacheType">Type of cache to be used</param>
        /// <returns>The exchange rate as decimal if found, else 1</returns>
        public virtual decimal GetExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode, out bool exchangeRateExisted, CacheType cacheType = CacheType.CacheTypeNone)
        {
            exchangeRateExisted = false;

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP 1 CURRENCYCODE, FROMDATE, (EXCHRATE/100) AS EXCHANGERATE, (POSEXCHRATE/100) AS POSEXCHANGERATE 
                                    FROM EXCHRATES 
                                    WHERE DATAAREAID = @DATAAREAID 
                                    AND CURRENCYCODE = @CURRENCYCODE
                                    AND FROMDATE <= @NOW
                                    ORDER BY FROMDATE DESC";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CURRENCYCODE", (string)currencyCode);
                MakeParam(cmd, "NOW", DateTime.Now, SqlDbType.DateTime);

                ExchangeRate rate = Get<ExchangeRate>(entry, cmd, currencyCode, PopulateExchangeRate, cacheType, UsageIntentEnum.Normal);

                decimal exchangeRate = 1;
                if (rate != null)
                {
                    exchangeRate = rate.ExchangeRateValue;
                    decimal posExchangeRate = rate.POSExchangeRateValue;

                    if (posExchangeRate != 0)
                    {
                        exchangeRate = posExchangeRate;
                    }

                    exchangeRateExisted = true;
                }

                return exchangeRate;
            }
        }

        [Obsolete("Use overridden Save method", true)]
        public virtual void Save(IConnectionManager entry, ExchangeRate exchangeRate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves an ExchangeRate into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="exchangeRate">The ExchangeRate to save</param>
        /// <param name="oldDate">If this parameter has the value '0001.01.01' we are inserting a new ExchangeRate, otherwise
        /// we are updating an ExchangeRate that had this as the 'FromDate'</param>
        public virtual void Save(IConnectionManager entry, ExchangeRate exchangeRate, DateTime oldDate)
        {
            var statement = new SqlServerStatement("EXCHRATES");

            ValidateSecurity(entry, BusinessObjects.Permission.CurrencyEdit);

            // We are trying to insert a new record if oldDate has the year 0001
            if (oldDate < new DateTime(1000,1,1))
            {
                if (Exists(entry,exchangeRate.ID))
                    return;
                
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CURRENCYCODE", (string)exchangeRate.CurrencyCode);
                statement.AddKey("FROMDATE", (DateTime)exchangeRate.FromDate, SqlDbType.DateTime);
                statement.AddKey("GOVERNMENTEXCHRATE", 0.0, SqlDbType.Decimal);
                statement.AddKey("TRIANGULATION", 0, SqlDbType.TinyInt);
            }
            // We are trying to update an existing record
            else
            {
                if (!Exists(entry, new RecordIdentifier(exchangeRate.CurrencyCode,oldDate)))
                    return;

                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CURRENCYCODE", (string)exchangeRate.CurrencyCode);
                statement.AddCondition("FROMDATE", oldDate, SqlDbType.DateTime);
                statement.AddCondition("GOVERNMENTEXCHRATE", 0.0, SqlDbType.Decimal);
                statement.AddCondition("TRIANGULATION", 0, SqlDbType.TinyInt);

                statement.AddField("FROMDATE", (DateTime)exchangeRate.FromDate , SqlDbType.DateTime);
            }

            statement.AddField("EXCHRATE", exchangeRate.ExchangeRateValue, SqlDbType.Decimal);
            statement.AddField("POSEXCHRATE", exchangeRate.POSExchangeRateValue, SqlDbType.Decimal);
            
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets the conversion rate when converting from currencyCodeFrom to currencyCodeTo
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCodeFrom">ID of the currency to convert from</param>
        /// <param name="currencyCodeTo">ID of the currency to convert to</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public decimal ConversionRateBetweenCurrencies(
            IConnectionManager entry
            ,RecordIdentifier currencyCodeFrom
            ,RecordIdentifier currencyCodeTo
            ,CacheType cacheType = CacheType.CacheTypeNone)
        {
            var exchangeRateFrom = GetExchangeRate(entry, currencyCodeFrom, cacheType);
            var exchangeRateTo = GetExchangeRate(entry, currencyCodeTo, cacheType);

            if (exchangeRateFrom != 0)
            {
                return exchangeRateTo / exchangeRateFrom;
            }

            return 0;
        }

        public List<ExchangeRate> LoadExchangeRates(
                 IConnectionManager entry,
                 int rowFrom,
                 int rowTo,
                 out int totalRecordsMatching)
        {
            List<TableColumn> listColumns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "CURRENCYCODE", TableAlias = "E"},
                new TableColumn {ColumnName = "FROMDATE", TableAlias = "E"},

                new TableColumn {ColumnName = "EXCHRATE", TableAlias = "E",IsNull = true,NullValue = "0", ColumnAlias = "ExchangeRate"},
                new TableColumn {ColumnName = "POSEXCHRATE", TableAlias = "E",IsNull = true,NullValue = "0", ColumnAlias = "POSExchangeRate"},
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
                    ColumnName = "ROW_NUMBER() OVER(ORDER BY FROMDATE, CURRENCYCODE, DATAAREAID, GOVERNMENTEXCHRATE, TRIANGULATION)",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName =
                        "COUNT(1) OVER(ORDER BY FROMDATE, CURRENCYCODE, DATAAREAID, GOVERNMENTEXCHRATE, TRIANGULATION RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
                    ColumnAlias = "ROW_COUNT"
                });


                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "S.ROW BETWEEN @ROWFROM AND @ROWTO"
                });


                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("EXCHRATES", "E", "S"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);
                int matchingRecords = 0;

                var reply = Execute<ExchangeRate, int>(entry, cmd, CommandType.Text, ref matchingRecords,
                    PopulateExchangeRateWithCount);

                totalRecordsMatching = matchingRecords;
                return reply;
            }
        }

        public void DeleteAllExchangeRates(IConnectionManager entry)
        {
            ValidateSecurity(entry, Permission.CurrencyEdit);

            SqlServerStatement statement = new SqlServerStatement("EXCHRATES", StatementType.Delete);
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">>List of items you want to get from the database matching items</param>
        /// <returns></returns>
        public virtual List<ExchangeRate> GetCompareList(IConnectionManager entry, List<ExchangeRate> itemsToCompare)
        {
            var columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "FROMDATE", ColumnAlias = "FromDate", TableAlias = "E"},
                new TableColumn {ColumnName = "CURRENCYCODE", ColumnAlias = "CurrencyCode", TableAlias = "E"},
                new TableColumn {ColumnName = "EXCHRATE", ColumnAlias = "ExchangeRate", TableAlias = "E", IsNull = true, NullValue="0"},
                new TableColumn {ColumnName = "POSEXCHRATE", ColumnAlias = "POSExchangeRate", TableAlias = "E", IsNull = true, NullValue="0"},
            };

            return GetCompareListInBatches(entry, itemsToCompare, "EXCHRATES", new string[] { "CURRENCYCODE", "FROMDATE" }, columns, null, PopulateExchangeRate);
        }
    }
}