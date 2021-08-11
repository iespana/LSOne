using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ITenderDeclarationData : IDataProviderBase<TenderDeclarationTransaction>
    {
        decimal GetLastTenderedAmount(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeId = null);
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        void Delete(IConnectionManager entry, TenderLineItem item);
        void Save(IConnectionManager entry, TenderDeclarationTransaction transaction, TenderLineItem tenderItem);
    }
}