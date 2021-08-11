using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ISafeDropReversalTransactionData : IDataProviderBase<SafeDropReversalTransaction>
    {
        void Insert(IConnectionManager entry, TenderLineItem tenderItem, SafeDropReversalTransaction transaction);
    }
}