using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IPurchaseOrderData : IDataProvider<PurchaseOrder>, ISequenceable
    {
        /// <summary>
        /// Searches for purchase orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">The starting row number to return</param>
        /// <param name="rowTo">The ending row number to return</param>
        /// <param name="sortBy">The column to sort by</param>
        /// <param name="sortBackwards">Indicates wether the results should be sorted backwards</param>
        /// <param name="itemCount">The number of results that the given parameters yielded</param>
        /// <param name="idOrDescription">A list of IDs and/or descriptions that the search should look for</param>
        /// <param name="idOrDescriptionBeginsWith">Indicates wether the search should look for records that begin with the values in <paramref name="idOrDescription"/></param>
        /// <param name="storeID">The store ID of the store that the purchase orders should belong to</param>
        /// <param name="vendorID">The vendor ID that the purchase orders should belong to</param>
        /// <param name="status">The status that the purchase orders should be in</param>
        /// <param name="deliveryDateFrom">Starting delivery date</param>
        /// <param name="deliveryDateTo">Ending delivery date</param>
        /// <param name="creationDateFrom">Starting creation date</param>
        /// <param name="creationDateTo">Ending creation date</param>
        /// <param name="onlySearchOpenAndNoGoodsReceivingDocument">If true then the search will only return purchase orders that are open and do note have a goods receiving document attached to them</param>
        /// <returns></returns>
        List<PurchaseOrder> AdvancedSearch(
            IConnectionManager entry,
            int rowFrom,
            int rowTo,
            InventoryPurchaseOrderSortEnums sortBy,
            bool sortBackwards, 
            out int itemCount,
            List<string> idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier storeID = null,
            RecordIdentifier vendorID = null,
            PurchaseStatusEnum? status = null,
            Date deliveryDateFrom = null,
            Date deliveryDateTo = null,
            Date creationDateFrom = null,
            Date creationDateTo = null,
            bool onlySearchOpenAndNoGoodsReceivingDocument = false
            );

        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <returns>Purchase order</returns>
        PurchaseOrder SaveAndReturnPurchaseOrder(IConnectionManager entry, PurchaseOrder purchaseOrder);

        /// <summary>
        /// Gets a list of all PurchaseOrders . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="includeReportFormatting">Set to true if you want address and name formatting, usually for reports</param>
        /// <returns>A list of all PurchaseOrders</returns>
        List<PurchaseOrder> GetPurchaseOrders(IConnectionManager entry, PurchaseOrderSorting sortBy, bool sortBackwards,bool includeReportFormatting);

        /// <summary>
        /// Gets a list of purchase orders for a specific vendor. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific vendor</returns>
        List<PurchaseOrder> GetPurchaseOrdersForVendor(IConnectionManager entry, RecordIdentifier vendorID, PurchaseOrderSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of purchase orders for a specific store. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific store</returns>
        List<PurchaseOrder> GetPurchaseOrdersForStore(IConnectionManager entry, RecordIdentifier storeID, PurchaseOrderSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of purchase orders for a specific store and a specific vendor. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all PurchaseOrders for a specific store and a specific vendor</returns>
        List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier vendorID, PurchaseOrderSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of all purchase orders for a store that are not linked to a goods receiving document. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <param name="includeLineTotals">True if the total quantity of items and total number of items should be included in the query. Used in OMNI</param>
        /// <returns>A list of all purchase orders for a store that are not linked to a goods receiving document</returns>
        List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(IConnectionManager entry, RecordIdentifier storeID, PurchaseOrderSorting sortBy, bool sortBackwards, bool includeLineTotals);

        /// <summary>
        /// Gets a purchase order with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get</param>
        /// <param name="includeReportFormatting">Set to true if you want address and name formatting, usually for reports</param>
        /// <returns>A purchase order with a given ID</returns>
        PurchaseOrder Get(IConnectionManager entry, RecordIdentifier purchaseOrderID, bool includeReportFormatting);

        List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaserOrderID, string searchString, int rowFrom, int rowTo, bool beginsWith);
        List<Unit> GetUnitsForPurchaserOrderItemVariant(IConnectionManager entry, RecordIdentifier purchaseOrderID, RecordIdentifier itemID);
        bool HasPostedGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier purchaseOrderID);
        bool HasGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier purchaseOrderID);
        bool HasPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Returns the total number of purchase orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>The total number of purchase orders</returns>
        int CountOrders(IConnectionManager entry);

        /// <summary>
        /// Creates purchase order lines based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">Purchase order id</param>
        /// <param name="filter">Filter container</param>
        /// <returns>Number of lines inserted</returns>
        int CreatePurchaseOrderLinesFromFilter(IConnectionManager entry, RecordIdentifier purchaseOrderID, InventoryTemplateFilterContainer filter);

        /// <summary>
        /// Set the processing status of a purchase order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">ID of the purchase order</param>
        /// <param name="status">New processing status</param>
        void SetPurchaseOrderProcessingStatus(IConnectionManager entry, RecordIdentifier purchaseOrderID, InventoryProcessingStatus status);

        /// <summary>
        /// Get the latest purchase order created by the omni mobile app and was not yet posted.
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="templateID">The template ID used for creating the stock counting journal.</param>
        /// <param name="storeID">The store ID in which the journal was created.</param>
        /// <returns>Return a purchase order</returns>
        PurchaseOrder GetOmniPurchaseOrderByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID);
    }
}