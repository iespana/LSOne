using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Card
{
    public interface ICardTypeData : IDataProvider<CardType>, ISequenceable
    {
        bool CardNameExists(IConnectionManager entry, string description);
        List<DataEntity> GetList(IConnectionManager entry);
        CardType Get(IConnectionManager entry, RecordIdentifier cardID);
    }
}
