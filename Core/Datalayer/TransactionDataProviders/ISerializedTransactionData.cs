using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ISerializedTransactionData : IDataProviderBase<PosTransaction>
    {
        PosTransaction GetSerializedTransaction(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, PosTransaction transaction);
        bool UnconcludedTransactionExists(IConnectionManager entry);
        void DropSerializedTransactions(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID);
        void SaveSerializedTransaction(IConnectionManager entry, PosTransaction transaction);
    }
}