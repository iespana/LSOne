using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IItemTranslationData : IDataProvider<ItemTranslation>
    {
        List<ItemTranslation> GetList(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone);
        List<ItemTranslation> GetListByCultureName(IConnectionManager entry, string cultureName, CacheType cacheType = CacheType.CacheTypeNone);
        string GetItemTranslationByCultureName(IConnectionManager entry, RecordIdentifier itemId, string cultureName, CacheType cacheType = CacheType.CacheTypeNone);

        ItemTranslation Get(IConnectionManager entry, RecordIdentifier itemTranslationID, CacheType cacheType = CacheType.CacheTypeNone);
    }
}