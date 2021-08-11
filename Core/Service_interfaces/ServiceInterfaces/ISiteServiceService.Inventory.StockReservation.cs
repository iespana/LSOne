using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        List<InventoryAdjustment> GetInventoryAdjustmentJournalList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeId,
            InventoryJournalTypeEnum journalType,
            int journalStatus,
            InventoryAdjustmentSorting sortBy,
            bool sortedBackwards,
            bool closeConnection);

        List<InventoryJournalTransaction> GetJournalTransactionList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalTransactionId,
            InventoryJournalTransactionSorting sortBy,
            bool sortedBackwards,
            bool noExcludedInventoryItems,
            bool closeConnection);

        DataEntity GetJournalTransactionReasonById(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier id,
            bool closeConnection);

        void CloseInventoryAdjustment(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalId,
            bool closeConnection);

        bool InventoryAdjustmentReasonIsInUse(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier reasonID,
            bool closeCnnection);

        void DeleteInventoryAdjustmentReason(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier reasonID,
            bool closeCnnection);

        PurchaseOrderLinesDeleteResult DeletePurchaseOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderLineID,
            bool closeConnection);

        InventoryAdjustment GetInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection);

        InventoryAdjustment SaveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection);

        void ReserveStockTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransaction reservation, RecordIdentifier storeID, InventoryTypeEnum typeOfTransactionLine, bool closeConnection);

        void ReserveStockItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, decimal adjustment, InventoryTypeEnum inventoryType,
            decimal costPrice, decimal netSalesPricePerItem, decimal salesPricePerItem, string unitID, decimal adjustmentInventoryUnit, RecordIdentifier reasonCode, string reasonText, string reference, bool closeConnection);

        List<ReasonCode> GetReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTypeEnum inventoryType, bool closeConnection);

        void SaveInventoryAdjustmentReason(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection);

        RecordIdentifier SaveInventoryAdjustmentReasonReturnID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection);

        void UpdateReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<ReasonCode> reasonCodes, InventoryJournalTypeEnum inventoryType, bool closeConnection);


        void MoveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection);

        /// <summary>
        /// Returns the sum of reservations at a supplied store
        /// </summary> 
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item </param>
        /// <param name="storeID">The store</param>
        /// <param name="inventoryUnitID">The inventory unit ID</param>
        /// <param name="journalType">The type of reservation</param>
        /// <returns></returns>
        decimal GetSumOfReservedItemByStore(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
           RecordIdentifier itemID,
           RecordIdentifier storeID,
           RecordIdentifier inventoryUnitID,
           InventoryJournalTypeEnum journalType,
           bool closeConnection
           );
    }
}
