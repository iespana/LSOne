using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TaxFree
{
    public interface ITouristData : IDataProvider<Tourist>
    {
        Tourist Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);
        List<Tourist> GetByPassportID(IConnectionManager entry, RecordIdentifier id);
    }
}
