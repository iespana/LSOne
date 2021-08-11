using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Tax
{
    public interface ITaxCodeData : IDataProvider<TaxCode>, ICompareListGetter<TaxCode>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all tax codes as data entities
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all tax codes as data entities</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all tax codes
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Defines the sort ordering of the results</param>
        /// <param name="backwardsSort">Defines if results should be sorted backwards</param>
        /// <param name="usage">Use normal or report if taxcode lines should be included.</param>
        /// <returns>A list of all tax codes</returns>
        List<TaxCode> GetTaxCodes(IConnectionManager entry, TaxCode.SortEnum sortEnum, bool backwardsSort, UsageIntentEnum usage = UsageIntentEnum.Minimal);

        TaxCode Get(IConnectionManager entry, RecordIdentifier taxCodeID, CacheType cacheType = CacheType.CacheTypeNone);

        List<TaxCode> GetTaxCodesForGroup(IConnectionManager entry, RecordIdentifier groupID);
    }
}