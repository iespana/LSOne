using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IPriceDiscountGroupData : IDataProviderBase<PriceDiscountGroup>, ICompareListGetter<PriceDiscountGroup>, ISequenceable
    {
        List<DataEntity> GetGroupList(IConnectionManager entry, PriceDiscountModuleEnum module, PriceDiscGroupEnum type, bool orderByName = false);

        Dictionary<RecordIdentifier, string> GetGroupDictionary(IConnectionManager entry);

        List<PriceDiscountGroup> GetGroups(IConnectionManager entry, PriceDiscountModuleEnum module,
            PriceDiscGroupEnum type, int sortColumn, bool backwardsSort);

        bool ExistsForStore(IConnectionManager entry, RecordIdentifier storeID);
        RecordIdentifier GetIDFromGroupID(IConnectionManager entry, RecordIdentifier groupID);
        /// <summary>
        /// Tells you if a store is in a price group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to check for</param>
        /// <param name="groupId">The ID of the group to check</param>
        /// <returns>Whether the store with the given ID is in the price group with the given ID</returns>
        bool StoreIsInPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId);

        /// <summary>
        /// Removes a store with a given ID from a price group with a given ID
        /// </summary>
        /// <remarks>Requires the 'Edit price group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to remove from price group</param>
        /// <param name="groupId">The ID of the price group to remove from</param>
        void RemoveStoreFromPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId);

        void RemoveStoresFromPriceGroup(
            IConnectionManager entry,
            RecordIdentifier groupId);

        /// <summary>
        /// Adds a store with a given ID to a price group with a given ID. The level of the record is one higher than 
        /// 
        /// </summary>
        /// <remarks>Requires the 'Edit price group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store to add to a price group</param>
        /// <param name="groupId">The ID of the price group to add a store to</param>
        void AddStoreToPriceGroup(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier groupId);

        void UpdateStoreInPriceGroup(IConnectionManager entry, StoreInPriceGroup line);

        /// <summary>
        /// Gets the price group line with the given store id and price group id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">The ID of the store</param>
        /// <param name="groupId">The ID of the price group to get</param>
        /// <returns>The price group line for the given store and price group id</returns>
        StoreInPriceGroup GetStoreInPriceGroup(IConnectionManager entry, RecordIdentifier storeId,
            RecordIdentifier groupId);

        /// <summary>
        /// Gets a list of stores in a price group, ordered by store name.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the price group</param>
        /// <returns>A list of stores in a price group, ordered by store name</returns>
        List<StoreInPriceGroup> GetStoresInPriceGroup(IConnectionManager entry, RecordIdentifier groupId);

        /// <summary>
        /// Gets a list of price group lines for a given store id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">Id of the store</param>
        /// <param name="cacheType">Cache</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <returns>A list of price group lines, ordered by level</returns>
        List<StoreInPriceGroup> GetPriceGroupsForStore(IConnectionManager entry, RecordIdentifier storeId,
            CacheType cacheType = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal);

        /// <summary>
        /// Checks if a customer belongs in the specified price/discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupType">The group type to check for</param>
        /// <param name="groupID">The ID of the group</param>
        /// <param name="customerID">The ID of the customer</param>
        /// <returns></returns>
        bool CustomerExistsInGroup(IConnectionManager entry, PriceDiscGroupEnum groupType,  RecordIdentifier groupID, RecordIdentifier customerID);

        List<CustomerInGroup> GetCustomersInGroupList(IConnectionManager entry,
            PriceDiscGroupEnum type,
            string customerGroup,
            int? recordFrom,
            int? recordTo);

        List<CustomerInGroup> SearchCustomersNotInGroup(
            IConnectionManager entry,
            string searchText,
            int recordFrom,
            int recordTo,
            int type,
            string groupId);

        bool CustomerExists(IConnectionManager entry, RecordIdentifier id);
        void RemoveCustomerFromGroup(IConnectionManager entry, RecordIdentifier customerAccountNumber, PriceDiscGroupEnum type);

        void RemoveCustomersFromGroup(
            IConnectionManager entry, 
            RecordIdentifier groupId,
            PriceDiscGroupEnum type);

        void AddCustomerToGroup(IConnectionManager entry, RecordIdentifier customerAccountNumber, PriceDiscGroupEnum type, string groupId);
        bool Exists(IConnectionManager entry, PriceDiscountModuleEnum module,PriceDiscGroupEnum type, RecordIdentifier groupID);
        PriceDiscountGroup Get(IConnectionManager entry, RecordIdentifier id);

        DataEntity GetGroupID(IConnectionManager entry, RecordIdentifier groupId);

        void Save(IConnectionManager entry, PriceDiscountGroup group);
        void Delete(IConnectionManager entry, PriceDiscountGroup group);
    }
}