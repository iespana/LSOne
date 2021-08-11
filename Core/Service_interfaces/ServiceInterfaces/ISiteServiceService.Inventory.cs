using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        List<InventoryStatus> GetInventoryStatus(
            IConnectionManager entry
            , SiteServiceProfile siteServiceProfile
            , RecordIdentifier itemID
            , RecordIdentifier variantID
            , RecordIdentifier regionID
            , bool closeConnection);

        /// <summary>
        /// Gets the amount of available inventory
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item to check </param>
        /// <param name="storeID">The store to find available inventory for</param>
        /// <returns></returns>
        decimal GetInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

        /// <summary>
        /// Get the status of inventory for item and store sorted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item </param>
        /// <param name="storeID">The store</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">The sort method</param>
        /// <param name="backwardsSort">The sort direction</param>
        /// <returns></returns>
        List<InventoryStatus> GetInventoryListForItemAndStore(
        IConnectionManager entry, SiteServiceProfile siteServiceProfile,
        RecordIdentifier itemID,
        RecordIdentifier storeID,
        RecordIdentifier regionID,
        InventorySorting sort,
        bool backwardsSort, bool closeConnection);

        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <param name="rowFrom">Start index</param>
        /// <param name="rowTo">End index</param>
        /// <param name="total">Total number of records</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total, bool closeConnection);

        /// <summary>
        /// Returns the inventory unit ID for a specific item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemId">The unique ID for the item to be checked</param>
        /// <returns></returns>
        RecordIdentifier GetInventoryUnitId(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, bool closeConnection);

        void UpdateInventoryUnit(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, decimal conversionFactor, bool closeConnection);

        /// <summary>
        /// Get the inventory on hand for an item in inventory unit, including unposted purchase orders and store transfers
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to get the inventory</param>
        /// <param name="storeID">ID of the store for which to get the inventory</param>
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <returns></returns>
        decimal GetEffectiveInventoryForItem(IConnectionManager entry,
             SiteServiceProfile siteServiceProfile,
             RecordIdentifier itemID,
             RecordIdentifier storeID,
             bool closeConnection);


        void SaveInventoryTransaction(IConnectionManager entry,
             SiteServiceProfile siteServiceProfile, InventoryTransaction transaction,
             bool closeConnection);

        List<InventoryTransaction> GetInventoryTransactionsFromTransaction(IConnectionManager entry,
             SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID,
             bool closeConnection);

        void MarkTransactionAsInventoryUpdated(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionId,
         RecordIdentifier storeID, RecordIdentifier terminalID, bool closeConnection);

        List<ItemLedger> GetItemLedgerList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ItemLedgerSearchParameters itemSearch, bool closeConnection);

        /// <summary>
        /// Gets the total number of ledger entries for the given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// /// <param name="itemID">The unique ID for the item to be checked</param>
        /// <returns></returns>
        int GetLedgerEntryCountForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Get the status of inventory for item and store sorted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item </param>
        /// <param name="storeID">The store</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">The sort method</param>
        /// <param name="backwardsSort">The sort direction</param>
        /// <returns></returns>
        List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(
        IConnectionManager entry, SiteServiceProfile siteServiceProfile,
        RecordIdentifier itemID,
        RecordIdentifier storeID,
        RecordIdentifier regionID,
        InventorySorting sort,
        bool backwardsSort, bool closeConnection);

        /// <summary>
        /// Gets the amount of available inventory
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item to check </param>
        /// <param name="storeID">The store to find available inventory for</param>
        /// <returns></returns>
        decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);
    }
}
