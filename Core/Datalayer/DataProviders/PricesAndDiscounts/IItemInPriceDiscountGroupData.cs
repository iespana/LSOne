using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IItemInPriceDiscountGroupData : IDataProviderBase<ItemInPriceDiscountGroup>
    {
        List<ItemInPriceDiscountGroup> GetItemList(IConnectionManager entry, 
            PriceDiscGroupEnum type, 
            RecordIdentifier discountGroup, 
            int? RecordFrom, 
            int? RecordTo,
            out int count);

        bool ItemIsInGroup(IConnectionManager entry, RecordIdentifier itemID, int type, string excludedGroupId);
        List<ItemInPriceDiscountGroup> SearchItemsNotInGroup(IConnectionManager entry, string searchText, int numberOfRecords, int type, string excludedGroupId);
        void RemoveItemFromGroup(IConnectionManager entry, RecordIdentifier itemId, PriceDiscGroupEnum type);
        void RemoveAllItemsFromGroup(IConnectionManager entry, RecordIdentifier groupId, PriceDiscGroupEnum type);
        void AddItemToGroup(IConnectionManager entry, RecordIdentifier itemId, PriceDiscGroupEnum type, string groupId);
    }
}