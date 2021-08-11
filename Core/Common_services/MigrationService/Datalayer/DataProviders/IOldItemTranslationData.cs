using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IOldItemTranslationData : IDataProvider<OldItemTranslation>
    {
        List<OldItemTranslation> GetList(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone);
        List<OldItemTranslation> GetListByCultureName(IConnectionManager entry, string cultureName, CacheType cacheType = CacheType.CacheTypeNone);
        string GetItemTranslationByCultureName(IConnectionManager entry, RecordIdentifier itemId, string cultureName, CacheType cacheType = CacheType.CacheTypeNone);

        OldItemTranslation Get(IConnectionManager entry, RecordIdentifier itemTranslationID, CacheType cacheType = CacheType.CacheTypeNone);
    }
}