using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService : IService
    {
        List<PurchaseWorksheet> GetInventoryWorksheetList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);
        PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID, bool closeConnection);
        List<PurchaseWorksheet> GetInventoryWorksheetListByInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTemplateID, bool closeConnection);
        bool InventoryWorksheetExistsForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool closeConnection);
        PurchaseWorksheet GetPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier worksheetId, bool closeConnection);
        RecordIdentifier SaveInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseWorksheet worksheet, bool closeConnection);
        void DeleteInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier worksheetId, bool closeConnection);
        List<PurchaseWorksheetLine> GetInventoryWorksheetLineListSimple(
           IConnectionManager entry, SiteServiceProfile siteServiceProfile,
           RecordIdentifier worksheetId,
           bool includeDeletedItems, bool closeConnection);


        List<PurchaseWorksheetLine> GetInventoryWorksheetLineList(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards, bool closeConnection);

        void DeleteInventoryWorksheetLineForPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection);

        PurchaseWorksheetLine GetPurchaseWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier worksheetLineId, bool closeConnection);

        RecordIdentifier SaveInventoryWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseWorksheetLine worksheetLine, bool closeConnection);

        /// <summary>
        /// Post a purchase worksheet
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to post</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A container with operation result and number of created purchase orders</returns>
        PostPurchaseWorksheetContainer PostPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection);

        /// <summary>
        /// Checks if there is any item in a purchase worksheet that is exluded from inventory operations (ex. Service items)
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to check</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if there is an inventory excluded item</returns>
        bool PurchaseWorksheetHasInventoryExcludedItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection);

        /// <summary>
        /// Creates purchase worksheet lines based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseWorksheetID">Purchase worksheet ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Number of lines inserted</returns>
        int CreatePurchaseWorksheetLinesFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection);

        /// <summary>
        /// Refresh the lines in a purchase worksheet by recalculating suggested quantities and adding missing items from the filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        void RefreshPurchaseWorksheetLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection);

        /// <summary>
        /// Calculate suggested quantity for replenishment for an item in a specific store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item</param>
        /// <param name="storeID">ID of the store</param>
        /// <param name="unitID">ID of the unit in which to convert the result</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        decimal CalculateSuggestedQuantity(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier unitID, bool closeConnection);
    }
}
