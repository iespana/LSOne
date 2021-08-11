using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ISaleLineItemData : IDataProviderBase<DataEntity>
    {
        LinkedList<SaleLineItem> GetLineItemsForRetailTransaction(IConnectionManager entry, RetailTransaction transaction, 
            bool limitStaffListToStore);

        ItemSale GetAdditionalItemInfo(IConnectionManager entry, ISaleLineItem saleLineItem, string currentCultureName);
        bool Exists(IConnectionManager entry, ISaleLineItem item);
        void Save(IConnectionManager entry, ISaleLineItem saleItem, PosTransaction transaction);

        void MarkItemsAsReturned(IConnectionManager entry, List<ISaleLineItem> returnedItems);
    }
}