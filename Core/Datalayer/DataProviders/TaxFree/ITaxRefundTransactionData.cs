using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TaxFree
{
    public interface ITaxRefundTransactionData : IDataProvider<TaxRefundTransaction>
    {
        TaxRefundTransaction Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);
        TaxRefundTransaction GetForTaxRefundID(IConnectionManager entry, RecordIdentifier taxRefundID, CacheType cacheType = CacheType.CacheTypeNone);
        TaxRefundTransaction GetForTransactionID(IConnectionManager entry, RecordIdentifier transactionID, bool includeCanceled, CacheType cacheType = CacheType.CacheTypeNone);
        bool ExistsForTransactionID(IConnectionManager entry, RecordIdentifier transactionID, bool includeCanceledRefunds);
    }
}
