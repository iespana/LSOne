using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        SendTransferOrderResult SendTransferOrders(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> transferIDs,
            DateTime sendingTime,
            bool closeConnection);

        SendTransferOrderResult SendTransferOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferID,
            DateTime sendingTime,
            bool closeConnection);

        ReceiveTransferOrderResult ReceiveTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferID,
            DateTime receivingTime,
            bool closeConnection);

        ReceiveTransferOrderResult ReceiveTransferOrders(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> transferIDs,
            DateTime receivingTime,
            bool closeConnection);

        AutoSetQuantityResult AutoSetQuantityOnTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferID,
            bool closeConnection);

        ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(
            IConnectionManager entry,
            List<RecordIdentifier> transferIDs,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection);

        DeleteTransferResult DeleteInventoryTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferOrderId,
            bool reject,
            bool closeConnection
            );

        DeleteTransferResult DeleteInventoryTransferOrders(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> inventoryTransferOrderIds,
            bool reject,
            bool closeConnection
            );

        List<InventoryTransferContainer> GetInventoryTransfersAndLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> listOfTransferIdsToFetch,
            bool closeConnection);

        List<InventoryTransferOrder> GetInventoryInTransit(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderSortEnum sorting,
            bool sortAscending,
            RecordIdentifier storeId,
            bool closeConnection);

        /// <summary>
        /// Returns information about a specific inventory transfer order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ID">The unique ID Of the transfer order</param>
        /// <returns>Information about a specific inventory transfer order</returns>
        InventoryTransferOrder GetInventoryTransferOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier ID,
            bool closeConnection);

        /// <summary>
        /// Returns order lines for a specific inventory transfer order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ID">The unique ID of the inventory transfer order</param>
        /// <param name="sortBy">How to sort the result list</param>
        /// <param name="sortBackwards">If true then the list is sorted backwards</param>
        /// <param name="getUnsentItemsOnly">If true then only order lines that have not been sent are returned</param>
        /// <returns></returns>
        List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransfer(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier ID,
            InventoryTransferOrderLineSortEnum sortBy,
            bool sortBackwards,
            bool getUnsentItemsOnly,
            bool closeConnection);

        /// <summary>
        /// Tries to find and return a transfer order line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="line">The transfer order line you want to get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns>A InventoryTransferOrderLine object, null if no matching line was found</returns>
        InventoryTransferOrderLine GetTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine line, bool closeConnection);

        bool ItemWithSameParametersExistsInTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine line, bool closeConnection);        

        /// <summary>
        /// Saves a transfer order line
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="inventoryTransferOrderLine">The transfer order line to save </param>
        /// <param name="isReceiving">Indicates of the transfer order associated to this line is sent and the line is edited during the receiving process of the order</param>
        /// <param name="newLineID">ID of the created line</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        SaveTransferOrderLineResult SaveInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID, bool closeConnection);        

        RecordIdentifier SaveInventoryTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrder inventoryTransferOrder, bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,RecordIdentifier inventoryTransferOrderId, bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> inventoryTransferOrderLineIds,
            bool closeConnection);

        void SaveInventoryTransferOrderLineIfChanged(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine inventoryTransferOrderLine, bool closeConnection);

        /// <summary>
        /// Get transfer order lines based on an extended search filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferOrderID, out int totalRecordsMatching, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Search inventory transfer orders based on a filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferOrder> SearchInventoryTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection);

        /// <summary>
        /// Search inventory transfer orders based on an extended filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferOrder> SearchInventoryTransferOrdersExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Search inventory transfer orders based on an extended filter with pagination
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="totalRecordsMatching">Total number of matching records based on the search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferOrder> SearchInventoryTransferOrdersAdvanced(IConnectionManager entry, SiteServiceProfile siteServiceProfile, out int totalRecordsMatching, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Creates transfer orders from a list of requests
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDs">The list of requests that should be turned into transfer orders</param>
        /// <param name="createdBy">If empty then the orders are being created on head office, otherwise the Store ID</param>
        /// <param name="siteServiceProfile">The site service profile used to connect to the site service</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed</param>
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferOrdersFromRequests(
           IConnectionManager entry,
           List<RecordIdentifier> requestIDs,
           RecordIdentifier createdBy,
           SiteServiceProfile siteServiceProfile,
           bool closeConnection);       

        /// <summary>
        /// Creates a transfer order from a specific request
        /// </summary>                
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>
        /// <param name="orderInformation">A order header object that has all the information needed to create an order header at head office. </param>                
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>        
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,            
            ref RecordIdentifier newOrderID,
            bool closeConnection);        

        /// <summary>
        /// Copies information from one transfer order to another
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>
        /// <param name="orderInformation">A order header object that has all the information needed to create an order header at head office. </param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>        
        /// <returns>Returns the ID of the transfer order that has been created</returns>  
        CreateTransferOrderResult CopyTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection);

        /// <summary>
        /// Creates a transfer order and adds the items from the filter to the order
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="orderInformation">A order header object that has all the information needed to create an order header at head office. </param>
        /// <param name="filter">Container with filter IDs</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>       
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferOrderFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferOrder orderInformation,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newOrderID,
            bool closeConnection);

        /// <summary>
        /// Get total number of unreceived items for a list of transfer orders
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="transferOrderIds">IDs of the transfer orders</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> transferOrderIds, bool closeConnection);

        /// <summary>
		/// Create a new transfer order based on a given template
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="orderInformation">An order header object that has all the information needed to create an order header at head office. </param>
		/// <param name="template">The transfer order template</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created transfer order</returns>
        CreateTransferOrderResult CreateTransferOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrder orderInformation, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection);

        /// <summary>
        /// Imports transfer order lines from an xml file
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="transferOrderID">ID of the transfer order in which to import lines</param>
        /// <param name="xmlData">XML data to import</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        int ImportTransferOrderLinesFromXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferOrderID, string xmlData, bool closeConnection);
    }
}
