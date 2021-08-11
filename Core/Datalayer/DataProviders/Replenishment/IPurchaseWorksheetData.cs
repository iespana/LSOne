using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IPurchaseWorksheetData : IDataProviderBase<PurchaseWorksheet>, ISequenceable
    {
        List<PurchaseWorksheet> GetList(IConnectionManager entry);
        List<PurchaseWorksheet> GetListByInventoryTemplate(IConnectionManager entry, RecordIdentifier inventoryTemplateID);
        void DeleteForInverotryTemplate(IConnectionManager entry, RecordIdentifier inventoryTemplateID);
        void DeleteForStore(IConnectionManager entry, RecordIdentifier storeID);
        bool ExistsForStore(IConnectionManager entry, RecordIdentifier storeID);
        PurchaseWorksheet Get(IConnectionManager entry, RecordIdentifier worksheetId);
        List<PurchaseWorksheet> GetWorksheetsFromTemplateID(IConnectionManager entry, RecordIdentifier inventoryTemplateID);
        PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(IConnectionManager entry, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID);
        RecordIdentifier Save(IConnectionManager entry, PurchaseWorksheet worksheet);
        void Delete(IConnectionManager entry, RecordIdentifier worksheetId);


        /// <summary>
        /// Creates purchase worksheet lines based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseWorksheetID">Purchase worksheet ID</param>
        /// <param name="filter">Filter container</param>
        /// <returns>Number of lines inserted</returns>
        int CreatePurchaseWorksheetLinesFromFilter(IConnectionManager entry, RecordIdentifier purchaseWorksheetID, InventoryTemplateFilterContainer filter);

        /// <summary>
        /// Refresh the lines in a purchase worksheet by recalculating suggested quantities
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet</param>
        /// <returns></returns>
        void RefreshPurchaseWorksheetLines(IConnectionManager entry, RecordIdentifier purchaseWorksheetID);
    }
}