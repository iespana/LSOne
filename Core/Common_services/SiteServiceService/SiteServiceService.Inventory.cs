using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        #region Inventory operations

        public virtual List<InventoryStatus> GetInventoryListForItemAndStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort, bool backwardsSort, bool closeConnection)
        {
            List<InventoryStatus> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryListForItemAndStore(itemID, storeID, regionID, sort, backwardsSort, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort, bool backwardsSort, bool closeConnection)
        {
            List<InventoryStatus> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryListForAssemblyItemAndStore(itemID, storeID, regionID, sort, backwardsSort, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

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
        public virtual List<InventoryStatus> GetInventoryListForStore(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total,
            bool closeConnection)
        {
            List<InventoryStatus> result = null;
            int rowCount = 0;

            DoRemoteWork(entry, siteServiceProfile, 
                        () => result = server.GetInventoryListForStore(CreateLogonInfo(entry), 
                                                                        storeID, 
                                                                        sort, backwardsSort, 
                                                                        rowFrom, rowTo, 
                                                                        out rowCount),
                        closeConnection);
            total = rowCount;

            return result;
        }

        public virtual RecordIdentifier GetInventoryUnitId(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryUnitId( itemID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual void UpdateInventoryUnit(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            decimal conversionFactor, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () =>  server.UpdateInventoryUnit( CreateLogonInfo(entry), itemID,conversionFactor), closeConnection);

        }

        public virtual decimal GetEffectiveInventoryForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            decimal result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetEffectiveInventoryForItem(CreateLogonInfo(entry), itemID, storeID), closeConnection);

            return result;
        }

        public virtual void PostInventoryAdjustmentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryJournalTransaction inventoryJournalTrans, RecordIdentifier storeID, InventoryTypeEnum typeOfAdjustmentLine, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.PostInventoryAdjustmentLine(CreateLogonInfo(entry), inventoryJournalTrans,storeID, typeOfAdjustmentLine), closeConnection);

        }

        public virtual decimal GetInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            RecordIdentifier storeID, bool closeConnection)
        {
            decimal result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryOnHand(itemID, storeID, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
            RecordIdentifier storeID, bool closeConnection)
        {
            decimal result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryOnHandForAssemblyItem(itemID, storeID, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual decimal GetSumofOrderedItembyStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool includeReportFormatting, RecordIdentifier inventoryUnitId,
            bool closeConnection)
        {
            decimal result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSumofOrderedItembyStore(itemID,storeID,includeReportFormatting,inventoryUnitId,CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual void SaveInventoryTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
          InventoryTransaction transaction, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () =>  server.SaveInventoryTransaction(CreateLogonInfo(entry), transaction), closeConnection);

        }

        public virtual List<InventoryTransaction> GetInventoryTransactionsFromTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool closeConnection)
        {
            List<InventoryTransaction> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTransactionsFromTransaction(CreateLogonInfo(entry),transactionID,storeID,terminalID), closeConnection);

            return result;
        }

        public virtual void MarkTransactionAsInventoryUpdated(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.MarkTransactionAsInventoryUpdated(CreateLogonInfo(entry), transactionID, storeID, terminalID), closeConnection);
        }
        public List<ItemLedger> GetItemLedgerList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
          ItemLedgerSearchParameters itemSearch, bool closeConnection)
        {
            List<ItemLedger> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemLedgerList(CreateLogonInfo(entry), itemSearch), closeConnection);

            return result;
        }

        public int GetLedgerEntryCountForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection)
        {
            int result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetLedgerEntryCountForItem(CreateLogonInfo(entry), itemID), closeConnection);

            return result;
        }

        /// <summary>
        /// Generates a new inventory adjustment ID
        /// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>New inventory adjustment ID</returns>
        public virtual RecordIdentifier GenerateInventoryAdjustmentID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            RecordIdentifier id = RecordIdentifier.Empty;
            DoRemoteWork(entry, siteServiceProfile, () => id = server.GenerateInventoryAdjustmentID(CreateLogonInfo(entry)), closeConnection);
            return id;
        }

        #endregion
    }
}
