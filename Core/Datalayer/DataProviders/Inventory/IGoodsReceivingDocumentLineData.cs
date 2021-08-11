using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IGoodsReceivingDocumentLineData : IDataProvider<GoodsReceivingDocumentLine>, ISequenceable
    {
        /// <summary>
        /// Returns the ID of the Purchase order the line belongs to
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineID">ID of the goods receaving document line</param>
        /// <returns></returns>
        RecordIdentifier GetPurchaseOrderIDForLine(IConnectionManager entry, RecordIdentifier lineID);

        /// <summary>
        /// Gets a list of goods receiving document lines for a given goods receiving document ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The goods receiving document ID to get goods receiving document lines by</param>
        /// <returns>A list of goods receiving document lines for a given goods receiving document ID</returns>
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID, bool includeReportFormatting = true);

        /// <summary>
        /// Gets a list of goods receiving document lines for a given goods receiving document ID. The list is sorted by the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The goods receiving document ID to get goods receiving document lines by</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of goods receiving document lines for a given goods receiving document ID</returns>
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(
            IConnectionManager entry,
            RecordIdentifier goodsReceivingDocumentID,
            GoodsReceivingDocumentLineSorting sortBy,
            bool sortBackwards,
            bool includeReportFormatting = true);

        /// <summary>
        /// Gets a goods receiving document line with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the goods receiving document line to get</param>
        /// <param name="storeID">The ID of the store that we are receiving at</param>
        /// <returns>A goods receiving document line with a given ID</returns>
        GoodsReceivingDocumentLine Get(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID, RecordIdentifier storeID);

        bool IsPosted(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentLineID);

        /// <summary>
        /// Create unposted goods receiving lines from purchase order lines.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order document</param>
        void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID);

        decimal GetReceivedItemQuantity(IConnectionManager entry, RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderLineID,  bool includeReportFormatting);

        List<InventoryTotals> GetReceivedTotals(IConnectionManager entry, int numberOfDocuments);

        List<GoodsReceivingDocumentLine> AdvancedSearch(IConnectionManager entry,
            GoodsReceivingDocumentLineSearch searchCriteria, GoodsReceivingDocumentLineSorting sortBy,
            bool sortBackwards, out int totalCount);
    }
}