using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IDiscountTransactionData : IDataProviderBase<DataEntity>
    {
        void Insert(IConnectionManager entry, ISaleLineItem saleLineItem, IDiscountItem discItem,int counter);
        List<IDiscountItem> GetDiscountItems(IConnectionManager entry, RecordIdentifier transactionID, IRetailTransaction transaction, ISaleLineItem saleLine, int lineNum);
    }
}