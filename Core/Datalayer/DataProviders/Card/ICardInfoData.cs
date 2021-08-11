using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Card
{
    public interface ICardInfoData : IDataProviderBase<CardInfo>, ISequenceable
    {
        List<CardInfo> GetAll(IConnectionManager entry); 

        //CardInfo Get(IConnectionManager entry, RecordIdentifier cardID);

        void Delete(IConnectionManager entry, string cardTypeID);
        bool Exists(IConnectionManager entry, RecordIdentifier cardTypeID);
        bool InUse(IConnectionManager entry, RecordIdentifier currencyID);
        void Save(IConnectionManager entry, CardInfo cardInfo);
    }
}