using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Tax
{
    public interface ITaxCodeValueData : IDataProvider<TaxCodeValue>
    {
        /// <summary>
        /// Gets a list of all tax code values
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns></returns>
        List<TaxCodeValue> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of tax code values for a given tax code ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code to get tax values for</param>
        /// <param name="sortEnum">Determines the sort ordering of the results</param>
        /// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
        /// <returns>A list of tax code values for a given tax code ID</returns>
        List<TaxCodeValue> GetTaxCodeValues(IConnectionManager entry, RecordIdentifier taxCodeID, TaxCodeValue.SortEnum sortEnum, bool backwardsSort);

        /// <summary>
        /// Checks if a tax code value for a given tax code ID exists that intersects with the given time range
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">The tax code ID</param>
        /// <param name="fromDate">Start of time range</param>
        /// <param name="toDate">End of time range</param>
        /// <returns>Wether a tax code value for the tax code ID exists that intersects with the given time range</returns>
        bool RangeExists(IConnectionManager entry,RecordIdentifier taxCodeID, Date fromDate, Date toDate);

        /// <summary>
        /// Checks if a tax code value for a given tax code ID exists that intersects with the given time range. Excludes a single tax code value.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ignoredTaxCodeLine">The tax code value to ignore</param>
        /// <param name="taxCodeID">The tax code ID</param>
        /// <param name="fromDate">Start of time range</param>
        /// <param name="toDate">End of time range</param>
        /// <returns>Wether a tax code value for the tax code ID exists that intersects with the given time range. Excludes a single tax code value</returns>
        bool RangeExists(IConnectionManager entry, TaxCodeValue ignoredTaxCodeLine, RecordIdentifier taxCodeID, Date fromDate, Date toDate);

        /// <summary>
        /// Gets a tax code value by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeValueID">ID of the tax code value</param>
        /// <param name="cacheType">Cache type to use. Default is none</param>
        /// <returns>A tax code value with a given ID</returns>
        TaxCodeValue Get(IConnectionManager entry, RecordIdentifier taxCodeValueID,
            CacheType cacheType = CacheType.CacheTypeNone);
    }
}