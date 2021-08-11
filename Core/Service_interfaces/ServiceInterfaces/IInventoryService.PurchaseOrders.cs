using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService
    {
        List<PurchaseOrderLine> GetPurchaseOrderLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            PurchaseOrderLineSearch searchCriteria,
            PurchaseOrderLineSorting sortBy,
            bool sortBackwards,
            out int totalRecordsMatching,
            bool closeConnection);

        /// <summary>
        /// Checks if a specific purchase order line has a goods receiving line against it already. 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>If the purchase order line has a goods receiving document line against it the function returns true</returns>
        bool PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection);

        /// <summary>
        /// Deletes a purchase order line if it has no goods receiving lineshas been registered against it
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>If true then the purchase order line has been deleted</returns>
        bool DeletePurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection);

        /// <summary>
        /// Retrieves information about a purchase order header
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID for the purchase order header</param>
        /// <returns>Information about the purchase order header</returns>
        PurchaseOrder GetPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// Saves a purchase order header
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrder">The purchase order header to be saved</param>
        void SavePurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrder, bool closeConnection);

        /// <summary>
        /// Copies all item lines and misc charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="fromPurchaseOrderID"></param>
        /// <param name="newPurchaseOrder"></param>
        /// <param name="storeID"></param>
        /// <param name="taxCalculationMethod"></param>
        /// <param name="closeConnection"></param>
        void CopyLinesAndBetweenMiscChargesPurchaseOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier fromPurchaseOrderID,
            PurchaseOrder newPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod, bool closeConnection);

        /// <summary>
        /// Generates a new purchase order ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>New purchase order ID</returns>
        RecordIdentifier GeneratePurchaseOrderID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Purchase order</returns>
        PurchaseOrder SaveAndReturnPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrder, bool closeConnection);

        /// <summary>
        /// Checks if a goods receiving document exists against a specific purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to be checked</param>
        /// <returns>Returns true if a goods receiving document exists</returns>
        bool PurchaseOrderHasGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// Creates and posts a goods receiving document for all lines within the purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrder">The purchase order that is to be posted and received</param>
        /// <returns>Result of the posting operation</returns>
        GoodsReceivingPostResult PostAndReceiveAPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrder, bool closeConnection);

        /// <summary>
        /// Returns true if the purchase order has any items lines
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <returns>Return true if any item lines are on the purchase order</returns>
        bool PurchaseOrderHasPurchaseOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// Goes through all the purchase order lines and updates either the discount amount and discount percentage on each line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <param name="storeID">The ID of the store the purchase order belongs to</param>
        /// <param name="discountPercentage">The discount % that should be used for updating. If null then this value is ignored</param>
        /// <param name="discountAmount">The discount amount that should be used for updating. If null then this value is ignored</param>
        void ChangeDiscountsForPurchaseOrderLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier storeID,
            decimal? discountPercentage,
            decimal? discountAmount,
            bool closeConnection);

        /// <summary>
        /// Deletes a specific miscellanious charge on a purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscChargeID">Unique ID of the miscellanious charge line to be deleted</param>
        void DeletePurchaseOrderMiscCharges(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderMiscChargeID, bool closeConnection);

        /// <summary>
        /// Gets a purchase order misc charges for a given purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            bool includeReportFormatting,
            bool closeConnection);

        InventoryTemplate GetInventoryTemplateForPOWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetId, bool closeConnection);

        List<PurchaseWorksheetLine> GetPurchaseWorksheetLineData(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            bool closeConnection);

        List<PurchaseWorksheetLine> GetPurchaseWorksheetLineDataPaged(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo,
            bool closeConnection);

        /// <summary>
        /// Gets information about a specific miscellanious charge
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscChargeID">The ID of the misc charge being retrieved</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>Information about the msc charge</returns>
        PurchaseOrderMiscCharges GetPurchaseOrderMiscCharge(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderMiscChargeID,
            bool includeReportFormatting,
            bool closeConnection);

        /// <summary>
        /// Saves a purchase order misc. charge. If no line number is on the object a new ID will be created
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscCharge">The misc. charge to be saved</param>
        void SavePurchaseOrderMiscCharge(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            PurchaseOrderMiscCharges purchaseOrderMiscCharge,
            bool closeConnection);

        /// <summary>
        /// Retrieves the sum of all ordered items per purchase order that is attached to a goods receiving document. 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        List<InventoryTotals> GetOrderedTotals(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            int numberOfDocuments,
            bool closeConnection);

        /// <summary>
        /// Return the total number of purchase orders that are in the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>the total number of purchase orders that are in the database</returns>
        int GetTotalNumberOfProductOrders(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection);

        /// <summary>
        /// Saves a specific purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLine">The purchase order line that is to be saved</param>
        RecordIdentifier SavePurchaseOrderLine(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            PurchaseOrderLine purchaseOrderLine,
            bool closeConnection);

        /// <summary>
        /// Gets the relevant unists for the purchase order and item
        /// </summary> 
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="itemID">The item</param>
        /// <returns></returns>
        List<Unit> GetUnitsForPurchaserOrderItemVariant(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Gets the Line number for the item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unitID">The unit ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="retailItemID">The item</param>
        /// <returns></returns>
        string GetPurchaseOrderLineNumberFromItemInfo(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID,
            RecordIdentifier unitID,
            bool closeConnection);

        /// <summary>
        /// Gets a purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLineID">The purchase order line ID</param>
        /// <param name="storeID">the store the purchase order belongs to </param>
        /// <param name="includeReportFormatting">If formatting information should be included</param>
        /// <returns></returns>
        PurchaseOrderLine GetPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID,
            RecordIdentifier storeID, bool includeReportFormatting, bool closeConnection);

        List<DataEntity> SearchRetailItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, string searchString,
            RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith, bool closeConnection);
        decimal GetSumofOrderedItembyStore(
                IConnectionManager entry,
                SiteServiceProfile siteServiceProfile,
                RecordIdentifier itemID,
                RecordIdentifier storeID,
                bool includeReportFormatting,
                RecordIdentifier inventoryUnitId,
                bool closeConnection);

        List<PurchaseOrder> PurchaseOrderAdvancedSearch(
           IConnectionManager entry,
           SiteServiceProfile siteServiceProfile,
           bool closeConnection,
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
           bool onlySearchOpenAndNoGoodsReceivingDocument = false);

        /// <summary>
        /// Searches for items that are part of the purchase order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseOrderID">The ID of the purchase order</param>
        /// <param name="searchString">The string to search for</param>
        /// <param name="rowFrom">The start row</param>
        /// <param name="rowTo">The end row</param>
        /// <param name="beginsWith">If the way to compare the search string </param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, string searchString, int rowFrom,
            int rowTo, bool beginsWith, bool closeConnection);

        RecordIdentifier GeneratePurchaseOrderLineID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            bool closeConnection);
        PurchaseOrder GetPurchaseOrderWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID,
            bool closeConnection);

          
        List<PurchaseOrderLine> GetPurchaseOrderLinesWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderStoreID,
            bool closeConnection);

        List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, RecordIdentifier vendorID,
            PurchaseOrderSorting purchaseOrderSorting, bool sortBackwards, bool closeConnection);

        List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrderWithSorting(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
          RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sorting, bool sortBackwards,
          bool includeReportFormatting, bool closeConnection);

        /// <summary>
		/// Create a new purchase order from an filter
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
		/// <param name="filter">Container with desired item filters</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created purchase order</returns>
		CreatePurchaseOrderResult CreatePurchaseOrderFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter, ref RecordIdentifier newOrderID, bool closeConnection);

        /// <summary>
        /// Create a new purchase order based on a given template.
        /// </summary>
        /// <param name="entry">The entry into the database.</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation.</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
        /// <param name="template">The purchase order template.</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Status and ID of the created purchase order.</returns>
        CreatePurchaseOrderResult CreatePurchaseOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection);
    }
}
