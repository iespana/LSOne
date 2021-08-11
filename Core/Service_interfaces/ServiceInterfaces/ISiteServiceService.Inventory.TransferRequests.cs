using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        InventoryTransferRequest GetInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferRequestId, bool closeConnection);

        RecordIdentifier SaveInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequest inventoryTransferRequest, bool closeConnection);


        SendTransferOrderResult SendInventoryTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestID,
            DateTime sendingTime,
            bool closeConnection);

        SendTransferOrderResult SendInventoryTransferRequests(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> requestIDs,
            DateTime sendingTime,
            bool closeConnection);

        DeleteTransferResult DeleteInventoryTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestId,
            bool reject,
            bool closeConnection
            );

        DeleteTransferResult DeleteInventoryTransferRequests(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> inventoryTransferRequestIds,
            bool reject,
            bool closeConnection
            );

        List<InventoryTransferRequestContainer> GetInventoryTransferRequestsAndLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> listOfTransferIdsToFetch,
            bool closeConnection);

        /// <summary>
        /// Get transfer request lines based on an extended search filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="transferRequestID">Transfer request ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferRequestID, out int totalRecordsMatching, InventoryTransferFilterExtended filter, bool closeConnection);

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="line">The transfer request line you want to get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the function finishes it's work</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        InventoryTransferRequestLine GetTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequestLine line, bool closeConnection);

        bool TransferOrderCreatedFromTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestId,
            RecordIdentifier inventoryTransferOrderId,
            bool closeConnection);

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
        void SaveInventoryTransferRequestLineIfChanged(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferRequestLine inventoryTransferRequestLine,
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
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>
        /// <param name="closeConnection">If true then the connection to the site service should be closed once the work is finished</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="newRequestID">The ID of the new transfer order that was created</param>
        /// <param name="requestInformation">A order header object that has information needed to create a request header at head office if the information such as receiving or sending store should be different then on the copied order. </param>        
        /// <returns></returns>        
        CreateTransferOrderResult CreateTransferRequestFromOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
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