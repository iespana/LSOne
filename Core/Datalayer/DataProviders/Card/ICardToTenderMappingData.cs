using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Card
{
    public interface ICardToTenderMappingData : IDataProvider<CardToTenderMapping>
    {
        List<DataEntity> GetList(IConnectionManager entry);
        CardToTenderMapping Get(IConnectionManager entry, RecordIdentifier brokerID, RecordIdentifier tenderType, CacheType cache = CacheType.CacheTypeNone);
    }
}