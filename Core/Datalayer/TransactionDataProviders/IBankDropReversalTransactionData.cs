using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IBankDropReversalTransactionData : IDataProviderBase<BankDropReversalTransaction>
    {
        void Insert(IConnectionManager entry,TenderLineItem tenderItem, BankDropReversalTransaction transaction);
    }
}