using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TaxFree
{
    public interface ITaxRefundRangeData : IDataProvider<TaxRefundRange>
    {
        TaxRefundRange Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);
        TaxRefundRange GetForValue(IConnectionManager entry, decimal value, CacheType cacheType = CacheType.CacheTypeNone);
        TaxRefundRange GetForValue(IConnectionManager entry, decimal value, decimal taxvalue, CacheType cacheType = CacheType.CacheTypeNone);
        List<TaxRefundRange> GetAll(IConnectionManager entry);
        bool Exists(IConnectionManager entry, decimal valueFrom, decimal valueTo);
    }
}
