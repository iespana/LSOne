using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IDiscountPeriodData : IDataProvider<DiscountPeriod>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing IDs and descriptions of all discount periods. The list is ordered by descriptions
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all discount periods</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all discount periods, ordered by a given field name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">The field name to orderd results by</param>
        /// <returns>A list of all discount periods, ordered by a given field name</returns>
        List<DiscountPeriod> GetDiscountPeriods(IConnectionManager entry, string sort);

        bool IsDiscountPeriodValid(IConnectionManager entry, RecordIdentifier discountPeriodID, DateTime dateTime);

        DiscountPeriod Get(IConnectionManager entry, RecordIdentifier discountPeriodID,
            CacheType cacheType = CacheType.CacheTypeNone);
    }
}