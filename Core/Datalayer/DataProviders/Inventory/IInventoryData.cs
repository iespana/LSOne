using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryData : IDataProviderBase<InventoryTransaction>
    {
        /// <summary>
        /// Gets inventory status for an item in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item's ID</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        List<InventoryStatus> GetInventoryListForItemAndStore(
            IConnectionManager entry, 
            RecordIdentifier itemID, 
            RecordIdentifier storeID, 
            RecordIdentifier regionID,
            InventorySorting sort, 
            bool backwardsSort);

        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort);

        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <param name="rowFrom">Start index</param>
        /// <param name="rowTo">End index</param>
        /// <param name="total">Total number of records</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total);

        decimal GetInventoryOnHand(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID);

        void UpdateInventoryUnit(IConnectionManager entry, RecordIdentifier itemID, decimal conversionFactor);

        /// <summary>
        /// Gets inventory statuses for items in the selected inventory group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item to get statuses for</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned.</param>
        /// <returns>Inventory statues for items in the selected inventory group</returns>
        List<InventoryStatus> GetInventoryStatusesForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier regionID);

        /// <summary>
        /// Gets inventory statuses for items in the selected inventory group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store to get inventory status for. RecordIdentifier.Empty and you get statuses for all stores</param>
        /// <param name="inventoryGroup">By which group do we wish to filter on</param>
        /// <param name="inventoryGroupID">The ID of the group to filter on</param>
        /// <returns>Inventory statues for items in the selected inventory group</returns>
        List<InventoryStatus> GetInventoryStatuses(
            IConnectionManager entry, 
            RecordIdentifier storeID, 
            InventoryGroup inventoryGroup, 
            RecordIdentifier inventoryGroupID);

        RecordIdentifier GetInventoryUnitId(IConnectionManager entry, RecordIdentifier itemId);

        #region Assemblies

        /// <summary>
        /// Gets inventory status for an assembly item in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The assembly item's ID</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier regionID,
            InventorySorting sort,
            bool backwardsSort);

        /// <summary>
        /// Get inventory on hand for an assembly item in the specified store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Assembly item ID</param>
        /// <param name="storeID">Store ID</param>
        /// <returns></returns>
        decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID);

        /// <summary>
        /// Gets inventory statuses for assembly items in the selected inventory group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The assembly item to get statuses for</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned.</param>
        /// <returns>Inventory statues for items in the selected inventory group</returns>
        List<InventoryStatus> GetInventoryListForAssemblyItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier regionID);

        #endregion
    }
}