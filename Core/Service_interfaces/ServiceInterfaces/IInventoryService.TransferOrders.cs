using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService
    {
        SendTransferOrderResult SendTransferOrders(
           IConnectionManager entry,
           List<RecordIdentifier> inventoryTransferIds,
           SiteServiceProfile siteServiceProfile
           );

        SendTransferOrderResult SendTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferId,
            SiteServiceProfile siteServiceProfile
            );

        ReceiveTransferOrderResult ReceiveTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferID,
            SiteServiceProfile siteServiceProfile
            );

        ReceiveTransferOrderResult ReceiveTransferOrders(
            IConnectionManager entry,
            List<RecordIdentifier> inventoryTransferIDs,
            SiteServiceProfile siteServiceProfile
            );

        AutoSetQuantityResult AutoSetQuantityOnTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferID,
            SiteServiceProfile siteServiceProfile
            );

        ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(
            IConnectionManager entry,
            List<RecordIdentifier> transferIDs,
            SiteServiceProfile siteServiceProfile);

        DeleteTransferResult DeleteTransferOrder(
            IConnectionManager entry,
            RecordIdentifier transferOrderId,
            bool reject,
            SiteServiceProfile siteServiceProfile
            );

        DeleteTransferResult DeleteTransferOrders(
            IConnectionManager entry,
            List<RecordIdentifier> transferOrderIds,
            bool reject,
            SiteServiceProfile siteServiceProfile
            );

        /// <summary>
        /// Creates transfer orders from a list of requests
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDs">The list of requests that should be turned into transfer orders</param>
        /// <param name="createdBy">If empty then the orders are being created on head office, otherwise the Store ID</param>
        /// <param name="siteServiceProfile">The site service profile used to connect to the site service</param>
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferOrdersFromRequests(
           IConnectionManager entry,
           List<RecordIdentifier> requestIDs, 
           RecordIdentifier createdBy,
           SiteServiceProfile siteServiceProfile);

        /// <summary>
        /// Creates a transfer order from a specific request
        /// </summary>        
        /// <param name="createdBy">If empty then the order is being created on head office, otherwise the Store ID</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order. If empty then the sending store ID from the old transfer order is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order. If empty then the receiving store ID from the old transfer order is used</param>
        /// <param name="description">The description to be assigned to the new transfer order. If empty then the description from the old transfer order is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>        
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>        
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy, 
            RecordIdentifier sendingStoreID, 
            RecordIdentifier receivingStoreID, 
            string description, 
            DateTime expectedDelivery, 
            RecordIdentifier createdBy,
            ref RecordIdentifier newOrderID,
            bool closeConnection);

        /// <summary>
        /// Creates a transfer order from a specific request
        /// </summary>                
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>        
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>        
        /// <param name="orderInformation">Order header to be used for the copying. If this is null then the information of the copied request header is used. </param>
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection);

        /// <summary>
        /// Get the inventory on hand for an item in inventory unit, including unposted purchase orders and store transfers
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to get the inventory</param>
        /// <param name="storeID">ID of the store for which to get the inventory</param>
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <returns></returns>
        decimal GetEffectiveInventoryForItem(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID,
            RecordIdentifier storeID,
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

        List<InventoryTransferOrder> GetInventoryInTransit(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderSortEnum sorting,
            bool sortAscending,
            RecordIdentifier storeId,
            bool closeConnection);

        bool ItemWithSameParametersExistsInTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine line, bool closeConnection);

        SaveTransferOrderLineResult SaveInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID, bool closeConnection);

        RecordIdentifier SaveInventoryTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrder inventoryTransferOrder, bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTransferOrderId, bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> inventoryTransferOrderLineIds,
            bool closeConnection);

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
        /// Copies information from one transfer order to another. This function takes individual properties of a transfer order header and creates
        /// a header object before calling the Site Service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>   
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order. If empty then the sending store ID from the old transfer order is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order. If empty then the receiving store ID from the old transfer order is used</param>
        /// <param name="description">The description to be assigned to the new transfer order. If empty then the description from the old transfer order is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>
        /// <param name="createdBy">If empty then the transfer order is being copied by head office otherwise this should be the store ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>        
        /// <returns></returns>        
        CreateTransferOrderResult CopyTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy, 
            RecordIdentifier sendingStoreID, 
            RecordIdentifier receivingStoreID,
            string description, 
            DateTime expectedDelivery, 
            RecordIdentifier createdBy,
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
        /// <returns></returns>  
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
        /// Creates a transfer order and adds the items from the filter to the order
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order.</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order.</param>
        /// <param name="description">The description to be assigned to the new transfer order</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>
        /// <param name="createdBy">If empty then the transfer order is being copied by head office otherwise this should be the store ID</param>
        /// <param name="filter">Container with filter IDs</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newOrderID">The ID of the new transfer order that was created</param>       
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferOrderFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
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
	    int ImportTransferOrderLinesFromXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferOrderID, string xmlData,  bool closeConnection);
    }
}
