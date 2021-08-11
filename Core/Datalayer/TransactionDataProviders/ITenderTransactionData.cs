using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ITenderTransactionData : IDataProviderBase<DataEntity>
    {
        void Insert(IConnectionManager entry, ITenderLineItem tenderItem, PosTransaction transaction);
    }
}