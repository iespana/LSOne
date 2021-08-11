using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ISafeDropTransactionData : IDataProviderBase<SafeDropTransaction>
    {
        void Insert(IConnectionManager entry, TenderLineItem tenderItem, SafeDropTransaction transaction);
    }
}