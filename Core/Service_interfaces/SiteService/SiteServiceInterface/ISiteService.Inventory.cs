using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {

        /// <summary>
        /// Gets the status of inventory for the supplied item
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <param name="storeID">Store ID</param>
        /// <param name="regioNID">Region ID</param>
        /// <param name="sort">How to sort</param>
        /// <param name="backwardsSort">Sort direction</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>            
        [OperationContract]
        List<InventoryStatus> GetInventoryListForItemAndStore(
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier regionID,
            InventorySorting sort,
            bool backwardsSort,
            LogonInfo logonInfo);

        /// <summary>
        /// Gets the status of inventory for the supplied item
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <param name="storeID">Store ID</param>
        /// <param name="regioNID">Region ID</param>
        /// <param name="sort">How to sort</param>
        /// <param name="backwardsSort">Sort direction</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>            
        [OperationContract]
        List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier regionID,
            InventorySorting sort,
            bool backwardsSort,
            LogonInfo logonInfo);


        /// <summary>
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <param name="rowFrom">Start index</param>
        /// <param name="rowTo">End index</param>
        /// <param name="total">Total number of records</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        [OperationContract]
        List<InventoryStatus> GetInventoryListForStore(
            LogonInfo logonInfo,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total);

        /// <summary>
        /// Gets the inventory unit for the item
        /// </summary>
        /// <param name="itemId">The item</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        RecordIdentifier GetInventoryUnitId(RecordIdentifier itemId, LogonInfo logonInfo);

        /// <summary>
        /// Gets the sum of item reservation
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <param name="storeID">The store</param>
        /// <param name="inventoryUnitID">The inventory unit id</param>
        /// <param name="journalType">the type</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetSumOfReservedItemByStore(
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            RecordIdentifier inventoryUnitID,
            InventoryJournalTypeEnum journalType,
            LogonInfo logonInfo);

        /// <summary>
        /// Updates the inventory unit of the item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">The item ID</param>
        /// <param name="conversionFactor">The conversion unit</param>
        [OperationContract]
        void UpdateInventoryUnit(LogonInfo logonInfo, RecordIdentifier itemID, decimal conversionFactor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="storeID"></param>
        /// <param name="inventoryGroup"></param>
        /// <param name="inventoryGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryStatus> GetInventoryStatuses(LogonInfo logonInfo, RecordIdentifier storeID,
            InventoryGroup inventoryGroup, RecordIdentifier inventoryGroupID);

        /// <summary>
        /// Saves the inventory Transaction to the central database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transaction">The transaction to save</param>
        [OperationContract]
        void SaveInventoryTransaction(LogonInfo logonInfo, InventoryTransaction transaction);

        /// <summary>
        /// Retrieves inventory transaction information 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transactionID">The ID of the transaction</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="terminalID">The ID of the terminal</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransaction> GetInventoryTransactionsFromTransaction(LogonInfo logonInfo,
            RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID);

        /// <summary>
        /// Marks a transaction to be excluded from inventory calculations
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transactionID">The ID of the transaction</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="terminalID">The ID of the terminal</param>
        [OperationContract]
        void MarkTransactionAsInventoryUpdated(LogonInfo logonInfo, RecordIdentifier transactionID,
            RecordIdentifier storeID, RecordIdentifier terminalID);

        /// <summary>
        /// Gets itemledgerentries for the given searchparameters
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemSearch">The search manifest</param>
        /// <returns>The Item Ledger</returns>
        [OperationContract]
        List<ItemLedger> GetItemLedgerList(LogonInfo logonInfo, ItemLedgerSearchParameters itemSearch);


        /// <summary>
        /// Gets the total number of ledger entries for the given item
        /// </summary>        
        /// <param name="logonInfo">The login information for the database</param>
        /// /// <param name="itemID">The unique ID for the item to be checked</param>
        [OperationContract]
        int GetLedgerEntryCountForItem(LogonInfo logonInfo, RecordIdentifier itemID);


        [OperationContract]
        List<InventoryStatus> GetInventoryStatus(
            RecordIdentifier itemID,
            RecordIdentifier variantID,
            RecordIdentifier regionID,
            LogonInfo logonInfo);

        /// <summary>
        /// Get the inventory on hand for an item in inventory unit, including unposted purchase orders and store transfers
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">ID of the item for which to get the inventory</param>
        /// <param name="storeID">ID of the store for which to get the inventory</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetEffectiveInventoryForItem(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID);
    }
}
