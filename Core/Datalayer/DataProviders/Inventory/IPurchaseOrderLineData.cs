using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IPurchaseOrderLineData : IDataProvider<PurchaseOrderLine>, ISequenceable
    {
        /// <summary>
        /// Gets a list of PurchaseOrderLines for a given PurchaseOrderID. The list is sorted by the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The purchase order ID to get purchase order lines by</param>
        /// <param name="storeID">The store that the purchar order lines are tied to</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param> 
        /// <returns>A list of PurchaseOrderLines for a given PurchaseOrderID</returns>
        List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID, 
            RecordIdentifier storeID, PurchaseOrderLineSorting sortBy, bool sortBackwards, bool includeReportFormatting);

        List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID, 
            RecordIdentifier storeID, bool includeReportFormatting);

        List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, PurchaseOrderLineSearch searchCriteria,
            PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching);

        /// <summary>
        /// Gets a purchase order line with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID of the purchase order line to get</param>
        /// <param name="storeID">The ID of the store that we are ordering for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order line with a given ID</returns>
        PurchaseOrderLine Get(IConnectionManager entry, RecordIdentifier purchaseOrderLineID, RecordIdentifier storeID, bool includeReportFormatting);

        void CopyLinesBetweenPurchaseOrders(IConnectionManager entry, RecordIdentifier fromPurchaseOrderID, PurchaseOrder toPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod);

        bool PurchaseOrderLineWithSameItemExists(
            IConnectionManager entry,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID,
            RecordIdentifier unitID);

        string GetPurchaseOrderLineNumberFromItemInfo(
            IConnectionManager entry,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID,
            RecordIdentifier unitID);

        /// <summary>
        /// Tells you if a given purchase order line has corresponding goods receiving lines
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID of the purchase order line to check for goods receiving lines</param>
        /// <returns>Whether a given purchase order line has corresponding goods receiving lines</returns>
        bool GoodsReceivingLineExists(IConnectionManager entry, RecordIdentifier purchaseOrderLineID);

        decimal GetSumofOrderedItembyStore(IConnectionManager entry, 
            RecordIdentifier itemID, 
            RecordIdentifier storeID, 
            bool includeReportFormatting,
            RecordIdentifier inventoryUnitId);

        bool HasPostedGoodsReceivingDocumentLines(IConnectionManager entry, RecordIdentifier purchaseOrderLineID);

        void ChangeDiscountsForPurchaseOrderLines(
            IConnectionManager entry, 
            RecordIdentifier purchaseOrderID, 
            RecordIdentifier storeID,
            decimal? discountPercentage, 
            decimal? discountAmount);

        /// <summary>
        /// Returns a summary of a total number of items ordered on each purchase order
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="numberOfDocuments">The list of summaries will be limited to this number of purchase order documents</param>
        /// <returns></returns>
        List<InventoryTotals> GetOrderedTotals(IConnectionManager entry, int numberOfDocuments);

        /// <summary>
        /// Updates a single line with a picture ID based on the transaction ID and line IDs from the mobile inventory app
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
        /// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
        /// <param name="pictureID">The ID of the picture to set on the line</param>
        [LSOneUsage(CodeUsage.LSCommerce)]
        void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID);
    }
}