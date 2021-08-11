using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.Utilities.Development;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        [OperationContract]
        SendTransferOrderResult SendTransferOrders(
            List<RecordIdentifier> transferIDs,
            DateTime sendingTime,
            LogonInfo logonInfo);

        [OperationContract]
        SendTransferOrderResult SendTransferOrder(            
            RecordIdentifier transferID,
            DateTime sendingTime,
            LogonInfo logonInfo);

        [OperationContract]
        ReceiveTransferOrderResult ReceiveTransferOrder(
            RecordIdentifier transferID,
            DateTime receivingTime,
            LogonInfo logonInfo);

        [OperationContract]
        ReceiveTransferOrderResult ReceiveTransferOrders(
            List<RecordIdentifier> transferIDs,
            DateTime receivingTime,
            LogonInfo logonInfo);

        [OperationContract]
        AutoSetQuantityResult AutoSetQuantityOnTransferOrder(
            RecordIdentifier transferID,
            LogonInfo logonInfo);

        [OperationContract]
        ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(
            List<RecordIdentifier> transferIDs,            
            LogonInfo logonInfo);

        [OperationContract]
        DeleteTransferResult DeleteInventoryTransferOrder(
            RecordIdentifier inventoryTransferOrderId, bool reject, LogonInfo logonInfo);

        [OperationContract]
        DeleteTransferResult DeleteInventoryTransferOrders(
            List<RecordIdentifier> inventoryTransferOrderIds, bool reject, LogonInfo logonInfo);

        [OperationContract]
        List<InventoryTransferContainer> GetInventoryTransferOrdersAndLines(
            List<RecordIdentifier> listOfTransferIdsToFetch, LogonInfo logonInfo);

        [OperationContract]
        List<InventoryTransferOrder> GetInventoryInTransit
            (LogonInfo logonInfo, InventoryTransferOrderSortEnum sorting, bool sortAscending, RecordIdentifier storeId);      

        [OperationContract]
        List<InventoryTransferRequestContainer> GetInventoryTransferRequestsAndLines(
            List<RecordIdentifier> listOfTransferIdsToFetch, LogonInfo logonInfo);

        [OperationContract]
        bool TransferOrderCreatedFromTransferRequest(
            RecordIdentifier requestId,
            RecordIdentifier orderId, LogonInfo logonInfo);

        /// <summary>
        /// Creates transfer orders from a list of requests
        /// </summary>
        /// <param name="requestIDs">The list of requests that should be turned into transfer orders</param>
        /// <param name="createdBy">If empty then the orders are being created on head office, otherwise the Store ID</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferOrdersFromRequests(List<RecordIdentifier> requestIDs, RecordIdentifier createdBy, LogonInfo logonInfo);

        /// <summary>
        /// Creates a transfer order from a specific request
        /// </summary>        
        /// <param name="createdBy">If empty then the order is being created on head office, otherwise the Store ID</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>
        /// <param name="orderInformation">A order header object that has information needed to create an order header at head office if the information such as receiving or sending store should be different then on the copied order. 
        /// If null then the information of the order to be copied is used </param>                        
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferOrderFromRequest(LogonInfo logonInfo, 
            RecordIdentifier requestIDtoCopy, 
            InventoryTransferOrder orderInformation,             
            out RecordIdentifier newOrderID);

        /// <summary>
        /// Returns information about a specific inventory transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="ID">The unique ID Of the transfer order</param>
        /// <returns>Information about a specific inventory transfer order</returns>
        [OperationContract]
        InventoryTransferOrder GetInventoryTransferOrder(LogonInfo logonInfo, RecordIdentifier ID);        

        /// <summary>
        /// Returns order lines for a specific inventory transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="ID">The unique ID of the inventory transfer order</param>
        /// <param name="sortBy">How to sort the result list</param>
        /// <param name="sortBackwards">If true then the list is sorted backwards</param>
        /// <param name="getUnsentItemsOnly">If true then only order lines that have not been sent are returned</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransfer(
            LogonInfo logonInfo, 
            RecordIdentifier ID, 
            InventoryTransferOrderLineSortEnum sortBy, 
            bool sortBackwards, 
            bool getUnsentItemsOnly);

        /// <summary>
        /// Tries to find and return a transfer order line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line">The transfer order line you want to get</param>
        /// <returns>A InventoryTransferOrderLine object, null if no matching line was found</returns>
        [OperationContract]
        InventoryTransferOrderLine GetTransferOrderLine(LogonInfo logonInfo, InventoryTransferOrderLine line);

        /// <summary>
        /// Get transfer order lines based on an extended search filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(LogonInfo logonInfo, RecordIdentifier transferOrderID, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        [OperationContract]
        bool ItemWithSameParametersExistsInTransferOrder(LogonInfo logonInfo,InventoryTransferOrderLine line);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferOrderLine">The transfer order line to save</param>
        /// <param name="isReceiving">Indicates of the transfer order associated to this line is sent and the line is edited during the receiving process of the order</param>
        /// <param name="newLineID">ID of the created line</param>
        /// <returns></returns>
        [OperationContract]
        SaveTransferOrderLineResult SaveInventoryTransferOrderLine(LogonInfo logonInfo, InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID);


        [OperationContract]
        RecordIdentifier SaveInventoryTransferOrder(LogonInfo logonInfo, InventoryTransferOrder inventoryTransferOrder);

        [OperationContract]
        SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(RecordIdentifier inventoryTransferOrderId, LogonInfo logonInfo);

        [OperationContract]
        SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(LogonInfo logonInfo, List<RecordIdentifier> inventoryTransferOrderLineIds);

        [OperationContract]
        PurchaseOrder GetPurchaseOrderWithReportFormatting(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        [OperationContract]
        List<PurchaseOrderLine> GetPurchaseOrderLinesWithReportFormatting(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderStoreID);

        [OperationContract]
        List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(LogonInfo logonInfo, RecordIdentifier storeID,
            RecordIdentifier vendorID, PurchaseOrderSorting purchaseOrderSorting, bool sortBackwards);

        [OperationContract]
        List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrderWithSorting(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sorting, bool sortBackwards,
            bool includeReportFormatting);

        [OperationContract]
        void SaveInventoryTransferOrderLineIfChanged(LogonInfo logonInfo,
            InventoryTransferOrderLine inventoryTransferOrderLine);

        /// <summary>
        /// Search inventory transfer orders based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferOrder> SearchInventoryTransferOrders(LogonInfo logonInfo, InventoryTransferFilter filter);

        /// <summary>
        /// Search inventory transfer orders based on an extended filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferOrder> SearchInventoryTransferOrdersExtended(LogonInfo logonInfo, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Search inventory transfer orders based on an extended filter with pagination
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="totalRecordsMatching">Total number of matching records based on the search criteria</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferOrder> SearchInventoryTransferOrdersAdvanced(LogonInfo logonInfo, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Copies information from one transfer order to another
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>
        /// <param name="orderInformation">A order header object that has information needed to create an order header at head office if the information such as receiving or sending store should be different then on the copied order. 
        /// If null then the information of the order to be copied is used </param>                        
        /// <param name="newOrderID">Returns the ID of the transfer order that has been created</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CopyTransferOrder(
            LogonInfo logonInfo, 
            RecordIdentifier orderIDtoCopy, 
            InventoryTransferOrder orderInformation,            
            out RecordIdentifier newOrderID);

        /// <summary>
        /// Creates a new transfer order using items from a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderInformation">A order header object that has information needed to create an order header at head office if the information such as receiving or sending store should be different then on the copied order. </param>
        /// <param name="filter">Filter container from which to create the lines for the transfer order</param>
        /// <param name="newOrderID">Returns the ID of the transfer order that has been created</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferOrderFromFilter(
            LogonInfo logonInfo,
            InventoryTransferOrder orderInformation,
            InventoryTemplateFilterContainer filter,
            out RecordIdentifier newOrderID);

        /// <summary>
        /// Get total number of unreceived items for a list of transfer orders
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferOrderIds">IDs of the transfer orders</param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(LogonInfo logonInfo, List<RecordIdentifier> transferOrderIds);


        /// <summary>
        /// Create a new transfer order based on a given template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderInformation">A order header object that has information needed to create an order header at head office. </param>
        /// <param name="template">The transfer order template</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>
        /// <returns>Status and ID of the created transfer order</returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferOrderFromTemplate(LogonInfo logonInfo, InventoryTransferOrder orderInformation, TemplateListItem template, out RecordIdentifier newOrderID);


        /// <summary>
        /// Imports transfer order lines from an xml file
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferOrderID">ID of the transfer order in which to import lines</param>
        /// <param name="xmlData">XML data to import</param>
        [OperationContract]
        int ImportTransferOrderLinesFromXML(LogonInfo logonInfo, RecordIdentifier transferOrderID, string xmlData);
    }
}
