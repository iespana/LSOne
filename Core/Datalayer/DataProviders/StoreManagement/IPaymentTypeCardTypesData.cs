using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IPaymentTypeCardTypesData : IDataProviderBase<StoreCardType>
    {
        List<DataEntity> GetUnusedCardTypesForTender(IConnectionManager entry, RecordIdentifier storeID,
            RecordIdentifier tenderTypeID,
            RecordIdentifier selectedCardType);

        StoreCardType GetCardForTenderType(IConnectionManager entry, RecordIdentifier storeID,
            RecordIdentifier tenderTypeID, RecordIdentifier cardTypeID);

        List<StoreCardType> GetCardListForTenderType(IConnectionManager entry, RecordIdentifier storeID,
            RecordIdentifier tenderTypeID);

        bool Exists(IConnectionManager entry, StoreCardType cardType);

        void Save(IConnectionManager entry, StoreCardType cardType);

        void Delete(IConnectionManager entry, RecordIdentifier storeTenderAndCardIdentifier);
    }
}