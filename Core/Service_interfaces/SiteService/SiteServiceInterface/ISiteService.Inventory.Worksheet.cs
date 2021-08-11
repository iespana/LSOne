using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {

        [OperationContract]
        List<PurchaseWorksheet> GetInventoryWorksheetList(LogonInfo logonInfo);
        [OperationContract]
        PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(LogonInfo logonInfo, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID);
        [OperationContract]
        List<PurchaseWorksheet> GetInventoryWorksheetListByInventoryTemplate(LogonInfo logonInfo, RecordIdentifier inventoryTemplateID);
        [OperationContract]
        bool InventoryWorksheetExistsForStore(LogonInfo logonInfo, RecordIdentifier storeID);
        [OperationContract]
        PurchaseWorksheet GetPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier worksheetId);
        [OperationContract]
        RecordIdentifier SaveInventoryWorksheet(LogonInfo logonInfo, PurchaseWorksheet worksheet);
        [OperationContract]
        void DeleteInventoryWorksheet(LogonInfo logonInfo, RecordIdentifier worksheetId);

        [OperationContract]
        List<PurchaseWorksheetLine> GetInventoryWorksheetLineListSimple(
           LogonInfo logonInfo,
           RecordIdentifier worksheetId,
           bool includeDeletedItems);

        [OperationContract]
        List<PurchaseWorksheetLine> GetInventoryWorksheetLineList(
            LogonInfo logonInfo,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards);
        
        [OperationContract]
        void DeleteInventoryWorksheetLineForPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID);

        [OperationContract]
        PurchaseWorksheetLine GetPurchaseWorksheetLine(LogonInfo logonInfo, RecordIdentifier worksheetLineId);

        [OperationContract]
        RecordIdentifier SaveInventoryWorksheetLine(LogonInfo logonInfo, PurchaseWorksheetLine worksheetLine);

        /// <summary>
        /// Post a purchase worksheet
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to post</param>
        /// <returns>A container with operation result and number of created purchase orders</returns>
        [OperationContract]
        PostPurchaseWorksheetContainer PostPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID);

        /// <summary>
        /// Checks if there is any item in a purchase worksheet that is exluded from inventory operations (ex. Service items)
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to check</param>
        /// <returns>True if there is an inventory excluded item</returns>
        [OperationContract]
        bool PurchaseWorksheetHasInventoryExcludedItems(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID);

        /// <summary>
        /// Creates purchase worksheet lines based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">Purchase worksheet ID</param>
        /// <returns>Number of lines inserted</returns>
        [OperationContract]
        int CreatePurchaseWorksheetLinesFromFilter(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID);

        /// <summary>
        /// Refresh the lines in a purchase worksheet by recalculating suggested quantities and adding missing items from the filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet</param>
        /// <returns></returns>
        [OperationContract]
        void RefreshPurchaseWorksheetLines(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID);

        /// <summary>
        /// Calculate suggested quantity for replenishment for an item in a specific store
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">ID of the item</param>
        /// <param name="storeID">ID of the store</param>
        /// <param name="unitID">ID of the unit in which to convert the result</param>
        /// <returns></returns>
        [OperationContract]
        decimal CalculateSuggestedQuantity(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier unitID);
    }
}
