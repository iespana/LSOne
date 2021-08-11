using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService
    {
        InventoryTransferRequest GetInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferRequestId, bool closeConnection);

        RecordIdentifier SaveInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequest inventoryTransferRequest, bool closeConnection);

        SendTransferOrderResult SendTransferRequest(
            IConnectionManager entry,            
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestID, 
            bool closeConnection);

        SendTransferOrderResult SendTransferRequests(
            IConnectionManager entry,            
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> requestIDs, 
            bool closeConnection);

        DeleteTransferResult DeleteTransferRequest(
            IConnectionManager entry,
            RecordIdentifier transferRequestId,
            bool reject,
            SiteServiceProfile siteServiceProfile);

        DeleteTransferResult DeleteTransferRequests(
            IConnectionManager entry,
            List<RecordIdentifier> transferRequestIds,
            bool reject,
            SiteServiceProfile siteServiceProfile);

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="line">The transfer request line you want to get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        InventoryTransferRequestLine GetTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequestLine line, bool closeConnection);

        bool ItemWithSameParametersExistsInTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequestLine line, bool closeConnection);

        SaveTransferOrderLineResult SaveInventoryTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequestLine inventoryTransferRequestLine, out RecordIdentifier newLineID, bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferRequestLine(
            IConnectionManager entry, 
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestLineId,
            bool closeConnection);

        SaveTransferOrderLineResult DeleteInventoryTransferRequestLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> inventoryTransferRequestLineIDs,
            bool closeConnection);

        List<InventoryStatus> GetInventoryStatuses(
           IConnectionManager entry,
           SiteServiceProfile siteServiceProfile,
           RecordIdentifier storeID,
           InventoryGroup inventoryGroup,
           RecordIdentifier inventoryGroupID,
           bool closeConnection);

        /// <summary>
        /// Search inventory transfer requests based on a filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferRequest> SearchInventoryTransferRequests(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection);

        /// <summary>
        /// Search inventory transfer requests based on an extended filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferRequest> SearchInventoryTransferRequestsExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Search inventory transfer requests based on an extended filter with pagination
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="totalRecordsMatching">Total number of matching records based on the search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns></returns>
        List<InventoryTransferRequest> SearchInventoryTransferRequestsAdvanced(IConnectionManager entry, SiteServiceProfile siteServiceProfile, out int totalRecordsMatching, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Creates a transfer request from a specific order
        /// </summary>        
        /// <param name="createdBy">If empty then the order is being created on head office, otherwise the Store ID</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer request. If empty then the sending store ID from the old transfer request is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer request. If empty then the receiving store ID from the old transfer request is used</param>
        /// <param name="description">The description to be assigned to the new transfer request. If empty then the description from the old transfer request is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer request. If empty then DateTime.Now + 3 days is used</param>        
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>        
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferRequestFromOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newRequestID,
            bool closeConnection);

        /// <summary>
        /// Creates a transfer request from a specific order
        /// </summary>                
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>        
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>        
        /// <param name="requestInformation">Reqeust header to be used for the copying. If this is null then the information of the copied order header is used. </param>
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferRequestFromOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection);

        /// <summary>
        /// Copies information from one transfer request to another. This function takes individual properties of a transfer request header and creates
        /// a header object before calling the Site Service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>   
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer request. If empty then the sending store ID from the old transfer request is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer request. If empty then the receiving store ID from the old transfer request is used</param>
        /// <param name="description">The description to be assigned to the new transfer request. If empty then the description from the old transfer request is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer request. If empty then DateTime.Now + 3 days is used</param>
        /// <param name="createdBy">If empty then the transfer request is being copied by head office otherwise this should be the store ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>        
        /// <returns></returns>        
        CreateTransferOrderResult CopyTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newRequestID,
            bool closeConnection);

        /// <summary>
        /// Copies information from one transfer request to another
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>
        /// <param name="requestInformation">A request header object that has all the information needed to create a request header at head office. </param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>        
        /// <returns></returns>  
        CreateTransferOrderResult CopyTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection);

        /// <summary>
        /// Creates a transfer request and adds the items from the filter to the request
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="requestInformation">A request header object that has all the information needed to create a request header at head office. </param>        
        /// <param name="filter">Container with filter IDs</param>     
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>       
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferRequestFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferRequest requestInformation,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newRequestID,
            bool closeConnection);

        /// <summary>
        /// Creates a transfer request and adds the items from the filter to the request
        /// </summary>
        /// <param name="entry">The entry into the database</param>        
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer request.</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer request.</param>
        /// <param name="description">The description to be assigned to the new transfer request</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer request. If empty then DateTime.Now + 3 days is used</param>
        /// <param name="createdBy">If empty then the transfer request is being copied by head office otherwise this should be the store ID</param>
        /// <param name="filter">Container with filter IDs</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <param name="siteServiceProfile">The site service profile that should be used to connect to the site service</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>       
        /// <returns></returns>
        CreateTransferOrderResult CreateTransferRequestFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newRequestID,
            bool closeConnection);


        /// <summary>
		/// Create a new transfer request based on a given template
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="requestInformation">A request header object that has all the information needed to create a request header at head office. </param> 
		/// <param name="template">The transfer request template</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created transfer request</returns>
        CreateTransferOrderResult CreateTransferRequestFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequest requestInformation, TemplateListItem template, ref RecordIdentifier newRequestID, bool closeConnection);
    }
}