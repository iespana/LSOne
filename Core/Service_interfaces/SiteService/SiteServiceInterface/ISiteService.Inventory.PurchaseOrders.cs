using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Searches for purchase order lines based on the search criteria in PurchaseOrderLineSearch parameter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">The search parameters used to find the lines</param>
        /// <param name="sortBy">How is the result to be sorted</param>
        /// <param name="sortBackwards">If true then the result will be sorted backwards</param>
        /// <param name="totalRecordsMatching">Number of records</param>
        /// <returns>A list of all lines found</returns>
        [OperationContract]
        List<PurchaseOrderLine> GetPurchaseOrderLines(LogonInfo logonInfo, PurchaseOrderLineSearch searchCriteria, PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching);

        /// <summary>
        /// Checks if a specific purchase order line has a goods receiving line against it already. 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        /// <returns>If the purchase order line has a goods receiving document line against it the function returns true</returns>
        [OperationContract]
        bool PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID);

        /// <summary>
        /// Deletes a specific purchase order line. PurchaseOrderLineHasPostedGoodsReceivingDocumentLine should be run first to make sure that no goods receiving lines are attached to this PO line
        /// </summary>
        /// <param name="logonInfo">Login information for the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        [OperationContract]
        bool DeletePurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID);

        /// <summary>
        /// Retrieves information about a purchase order header
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The unique ID for the purchase order</param>
        /// <returns></returns>
        [OperationContract]
        PurchaseOrder GetPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Copies all item lines and misc charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="fromPurchaseOrderID"></param>
        /// <param name="newPurchaseOrder"></param>
        /// <param name="storeID"></param>
        /// <param name="taxCalculationMethod"></param>
        [OperationContract]
        void CopyLinesAndBetweenMiscChargesPurchaseOrders(LogonInfo logonInfo, RecordIdentifier fromPurchaseOrderID,
            PurchaseOrder newPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod);

        /// <summary>
        /// Generates a new purchase order ID
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <returns>New purchase order ID</returns>
        [OperationContract]
        RecordIdentifier GeneratePurchaseOrderID(LogonInfo logonInfo);

        /// <summary>
        /// Generates a new purchase order line ID
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <returns>New purchase order ID</returns>
        [OperationContract]
        RecordIdentifier GeneratePurchaseOrderLineID(LogonInfo logonInfo);
        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="logonInfo">The entry into the database</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <returns>Purchase order</returns>
        [OperationContract]
        PurchaseOrder SaveAndReturnPurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder);

        /// <summary>
        /// Saves a purchase order header
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrder">The purchase order to be saved</param>
        [OperationContract]
        void SavePurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder);

        /// <summary>
        /// Returns true if a goods receiving document exists for this purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <returns>Returns true if a goods receiving document exists for this purchase order</returns>
        [OperationContract]
        bool PurchaseOrderHasGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Creates and posts a goods receiving document for all lines within the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrder">The purchase order that is to be posted and received</param>
        /// <returns>Result of the posting operation</returns>
        [OperationContract]
        GoodsReceivingPostResult PostAndReceiveAPurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder);

        /// <summary>
        /// Returns true if the purchase order has any items lines
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <returns>Return true if any item lines are on the purchase order</returns>
        [OperationContract]
        bool PurchaseOrderHasPurchaseOrderLines(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Goes through all the purchase order lines and updates either the discount amount and discount percentage on each line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <param name="storeID">The ID of the store the purchase order belongs to</param>
        /// <param name="discountPercentage">The discount % that should be used for updating. If null then this value is ignored</param>
        /// <param name="discountAmount">The discount amount that should be used for updating. If null then this value is ignored</param>
        [OperationContract]
        void ChangeDiscountsForPurchaseOrderLines(
            LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier storeID,
            decimal? discountPercentage,
            decimal? discountAmount);

        /// <summary>
        /// Deletes a specific miscellanious charge on a purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscChargeID">Unique ID of the miscellanious charge line to be deleted</param>
        [OperationContract]
        void DeletePurchaseOrderMiscCharges(LogonInfo logonInfo, RecordIdentifier purchaseOrderMiscChargeID);

        /// <summary>
        /// Gets a purchase order misc charges for a given purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        [OperationContract]
        List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            bool includeReportFormatting);

        [OperationContract]
        InventoryTemplate GetInventoryTemplateForPOWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetId);

        [OperationContract]
        List<PurchaseWorksheetLine> GetPurchaseWorksheetLineData(
            LogonInfo logonInfo,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards);

        [OperationContract]
        List<PurchaseWorksheetLine> GetPurchaseWorksheetLineDataPaged(
            LogonInfo logonInfo,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo);

        /// <summary>
        /// Gets information about a specific miscellanious charge
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscChargeID">The ID of the misc charge being retrieved</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>Information about the msc charge</returns>
        [OperationContract]
        PurchaseOrderMiscCharges GetPurchaseOrderMiscCharge(LogonInfo logonInfo, RecordIdentifier purchaseOrderMiscChargeID, bool includeReportFormatting);

        /// <summary>
        /// Saves a purchase order misc. charge. If no line number is on the object a new ID will be created
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscCharge">The misc. charge to be saved</param>
        [OperationContract]
        void SavePurchaseOrderMiscCharge(LogonInfo logonInfo, PurchaseOrderMiscCharges purchaseOrderMiscCharge);


        /// <summary>
        /// Retrieves the sum of all ordered items per purchase order that is attached to a goods receiving document. 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTotals> GetOrderedTotals(LogonInfo logonInfo, int numberOfDocuments);

        /// <summary>
        /// Return the total number of purchase orders that are in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>the total number of purchase orders that are in the database</returns>
        [OperationContract]
        int GetTotalNumberOfProductOrders(LogonInfo logonInfo);

        [OperationContract]
        List<PurchaseOrder> PurchaseOrderAdvancedSearch(
            LogonInfo logonInfo,
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
        /// Saves a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLine">The purchase order line that is to be saved</param>
        /// <returns>The linenumber of the purchase order line</returns>
        [OperationContract]
        RecordIdentifier SavePurchaseOrderLine(LogonInfo logonInfo, PurchaseOrderLine purchaseOrderLine);

        /// <summary>
        /// Gets the relevant units for the item in the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="itemID">The itemID</param>
        /// <returns></returns>
        [OperationContract]
        List<Unit> GetUnitsForPurchaserOrderItemVariant(LogonInfo logonInfo, RecordIdentifier purchaseOrderID,
            RecordIdentifier itemID);

        /// <summary>
        /// Gets the line number of the item in the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order</param>
        /// <param name="retailItemID">The retail Item ID</param>
        /// <param name="unitID">The relevant unit</param>
        /// <returns></returns>
        [OperationContract]
        string GetPurchaseOrderLineNumberFromItemInfo(
            LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID,
            RecordIdentifier unitID);

        /// <summary>
        /// Searches for items that are part of the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order</param>
        /// <param name="searchString">The string to search for</param>
        /// <param name="rowFrom">The start row</param>
        /// <param name="rowTo">The end row</param>
        /// <param name="beginsWith">If the way to compare the search string </param>
        /// <returns></returns>
        [OperationContract]
        List<DataEntity> SearchItemsInPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID,
            string searchString, int rowFrom, int rowTo, bool beginsWith);

        /// <summary>
        /// Search items that belong to the supplied vendor
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="vendorID">The vendor</param>
        /// <param name="searchString">The search string</param>
        /// <param name="itemGroupId">Filter by item group</param>
        /// <param name="rowFrom">the start row</param>
        /// <param name="rowTo">the end row</param>
        /// <param name="beginsWith">If the way to compare the search string </param>
        /// <returns></returns>
        [OperationContract]
        List<DataEntity> SearchRetailItemsForVendor(LogonInfo logonInfo, RecordIdentifier vendorID, string searchString,
            RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith);

        /// <summary>
        /// Gets a purchase order line
        /// </summary> 
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">The purchase order line ID</param>
        /// <param name="storeID">The store ID </param>
        /// <param name="includeReportFormatting">If formatting information should be included</param>
        /// <returns></returns>
        [OperationContract]
        PurchaseOrderLine GetPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID, RecordIdentifier storeID, bool includeReportFormatting);

        /// <summary>
        /// Create a new purchase order from an filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
        /// <param name="filter">Container with desired item filters</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
        /// <returns>Status and ID of the created purchase order</returns>
        [OperationContract]
        CreatePurchaseOrderResult CreatePurchaseOrderFromFilter(LogonInfo logonInfo, PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter, out RecordIdentifier newOrderID);

        /// <summary>
        /// Create a new purchase order based on a given template.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
        /// <param name="template">The purchase order template.</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
        /// <returns>Status and ID of the created purchase order.</returns>
        [OperationContract]
        CreatePurchaseOrderResult CreatePurchaseOrderFromTemplate(LogonInfo logonInfo, PurchaseOrder purchaseOrderHeader, TemplateListItem template, out RecordIdentifier newOrderID);
    }
}
