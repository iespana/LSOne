using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface ICardNumberSerieData : IDataProviderBase<CardNumberSerie>
    {
        /// <summary>
        /// Gets a specific payment type for a given store or all payment types for a given store.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="cardTypeID"></param>
        /// <returns></returns>
        List<CardNumberSerie> GetNumberSeries(IConnectionManager entry, RecordIdentifier cardTypeID);

        void Delete(IConnectionManager entry, RecordIdentifier fullSerieIdentifier);
        bool Exists(IConnectionManager entry, RecordIdentifier fullSerieIdentifier);
        void Save(IConnectionManager entry, RecordIdentifier fullSerieIdentifier, CardNumberSerie serie);
    }
}