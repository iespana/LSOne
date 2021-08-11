using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Currencies
{
    public interface IExchangeRatesData : IDataProvider<ExchangeRate>, ICompareListGetter<ExchangeRate>
    {
        List<ExchangeRate> GetAllExchangeRates(IConnectionManager entry);

        /// <summary>
        /// Gets a list of ExchangeRates for a given Currency code, sorted by a given column index. 'sortedBackwards' decides if we are using ascending or descending ordering.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The currency code to get exchange rates for</param>
        /// <param name="sortColumn">The index of the column to sort by. The columns are ["FROMDATE", "EXCHRATE", "POSEXCHRATE"]</param>
        /// <param name="sortedBackwards">Whether to sort in an descending or ascending order</param>
        /// <returns>A list of ExchangeRates for a Currency</returns>
        List<ExchangeRate> GetExchangeRates(IConnectionManager entry, RecordIdentifier currencyCode, int sortColumn, bool sortedBackwards);

        /// <summary>
        /// Gets the newest exchange rate for a given currency code.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCode">The currency code to get the exchange rate for</param>
        /// <param name="cacheType">Type of cache to be used</param>
        /// <returns>The exchange rate as decimal if found, else 1</returns>
        decimal GetExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode,CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the newest exchange rate for a given currency code.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCode">The currency code to get the exchange rate for</param>
        /// <param name="exchangeRateExisted">True if the exchange rate existed, else false</param>
        /// <param name="cacheType">Type of cache to be used</param>
        /// <returns>The exchange rate as decimal if found, else 1</returns>
        decimal GetExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode, out bool exchangeRateExisted, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Saves an ExchangeRate into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="exchangeRate">The ExchangeRate to save</param>
        /// <param name="oldDate">If this parameter has the value '0001.01.01' we are inserting a new ExchangeRate, otherwise
        /// we are updating an ExchangeRate that had this as the 'FromDate'</param>
        void Save(IConnectionManager entry, ExchangeRate exchangeRate, DateTime oldDate);

        /// <summary>
        /// Gets the conversion rate when converting from currencyCodeFrom to currencyCodeTo
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCodeFrom">ID of the currency to convert from</param>
        /// <param name="currencyCodeTo">ID of the currency to convert to</param>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        decimal ConversionRateBetweenCurrencies(
            IConnectionManager entry
            ,RecordIdentifier currencyCodeFrom
            ,RecordIdentifier currencyCodeTo
            ,CacheType cacheType = CacheType.CacheTypeNone);

        ExchangeRate Get(IConnectionManager entry, RecordIdentifier exchangeRateID);

        /// <summary>
        /// Gives the specified segment of all exchange rates
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">starting row</param>
        /// <param name="rowTo">the end row</param>
        /// <param name="totalRecordsMatching">how many rows are there</param>
        /// <returns></returns>
        List<ExchangeRate> LoadExchangeRates(
            IConnectionManager entry,
            int rowFrom,
            int rowTo,
            out int totalRecordsMatching);

        /// <summary>
        /// WARNING! USE WITH CAUTION! 
        /// Deletes all existing exchange rates
        /// </summary>
        /// <param name="entry">Database connection</param>
        void DeleteAllExchangeRates(IConnectionManager entry);
    }
}