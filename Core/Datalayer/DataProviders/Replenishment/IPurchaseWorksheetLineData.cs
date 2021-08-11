using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IPurchaseWorksheetLineData : IDataProviderBase<PurchaseWorksheetLine>
    {
        List<PurchaseWorksheetLine> GetList(
            IConnectionManager entry,
            RecordIdentifier worksheetId,
            bool includeDeletedItems);

        List<PurchaseWorksheetLine> GetList(
            IConnectionManager entry, 
            RecordIdentifier worksheetId, 
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards);

        List<PurchaseWorksheetLine> GetPagedList(
            IConnectionManager entry,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo);

        /// <summary>
        /// Get the inventory on hand for an item in inventory unit, including unposted purchase orders and store transfers
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemID">ID of the item for which to get the inventory</param>
        /// <param name="storeID">ID of the store for which to get the inventory</param>
        /// <returns></returns>
        decimal GetEffectiveInventoryForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID);
        RecordIdentifier GetPurchaseOrderUnit(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier vendorId);
        void DeleteForPurchaseWorksheet(IConnectionManager entry, RecordIdentifier purchaseWorksheetID);
        PurchaseWorksheetLine Get(IConnectionManager entry, RecordIdentifier worksheetLineId);
        RecordIdentifier Save(IConnectionManager entry, PurchaseWorksheetLine worksheetLine);

        /// <summary>
        /// Checks if there is any item in a purchase worksheet that is exluded from inventory operations (ex. Service items)
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="purchaseWorksheetID">Purchase worksheet ID</param>
        /// <returns></returns>
        bool PurchaseWorksheetHasInventoryExcludedItems(IConnectionManager entry, RecordIdentifier purchaseWorksheetID);

        /// <summary>
        /// Calculate suggested quantity for replenishment based on replenishment settings and current inventory on hand
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="inventoryOnHand">Current inventory on hand</param>
        /// <param name="replenishmentSetting">Replenishment settings</param>
        /// <returns></returns>
        decimal CalculateSuggestedQuantity(IConnectionManager entry, decimal inventoryOnHand, ItemReplenishmentSetting replenishmentSetting);
    }
}