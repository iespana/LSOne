using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Currencies
{
    public interface ICurrencyData : IDataProvider<Currency>, ICompareListGetter<Currency>
    {
        /// <summary>
        /// Gets a list of DataEntity that contains CurrencyCode and Currency Description. The list is sorted by the column specified in the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        List<DataEntity> GetList(IConnectionManager entry, CurrencySorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of DataEntity that contains CurrencyCode and Currency Description. The list is sorted by CurrencyCode.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        List<Currency> GetList(IConnectionManager entry, UsageIntentEnum usage);

        /// <summary>
        /// Gets list of DataEntity that contains CurrencyCode and Currency Description excluding one givenF currency code.  The list is sorted by CurrencyCode.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludeID">ID of the currency to be excluded</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier excludeID);

        /// <summary>
        /// Checks if any store is has a specific currency as it's default currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyID">The unique ID of the currency to check for</param>
        bool InUse(IConnectionManager entry, RecordIdentifier currencyID);

        /// <summary>
        /// Gets a Currency with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">The ID of the Currency to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A Currency with a given ID</returns>
        Currency Get(IConnectionManager entry, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the company currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>The company currency</returns>
        Currency GetCompanyCurrency(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets all currencies in the system except the store currency
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeCurrency">The unique ID of the store currency</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        List<Currency> GetNonStoreCurrencies(IConnectionManager entry, RecordIdentifier storeCurrency, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gives the specified segment of all currencies
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">starting row</param>
        /// <param name="rowTo">the end row</param>
        /// <param name="totalRecordsMatching">how many rows are there</param>
        /// <returns></returns>
        List<Currency> LoadCurrencies(
            IConnectionManager entry,
            int rowFrom,
            int rowTo,
            out int totalRecordsMatching);
    }
}