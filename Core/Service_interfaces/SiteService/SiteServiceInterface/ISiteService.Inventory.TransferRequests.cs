using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        [OperationContract]
        InventoryTransferRequest GetInventoryTransferRequest(LogonInfo logonInfo, RecordIdentifier transferRequestId);

        [OperationContract]
        RecordIdentifier SaveInventoryTransferRequest(LogonInfo logonInfo, InventoryTransferRequest inventoryTransferRequest);

        [OperationContract]
        bool ItemWithSameParametersExistsInTransferRequest(LogonInfo logonInfo, InventoryTransferRequestLine line);
        
        [OperationContract]
        SendTransferOrderResult SendInventoryTransferRequests(
          List<RecordIdentifier> requestIDs,
          DateTime sendingTime, LogonInfo logonInfo);

        [OperationContract]
        SendTransferOrderResult SendInventoryTransferRequest(
          RecordIdentifier requestID,
          DateTime sendingTime, LogonInfo logonInfo);

        [OperationContract]
        DeleteTransferResult DeleteInventoryTransferRequest(
            RecordIdentifier inventoryTransferRequestId, bool reject, LogonInfo logonInfo);

        [OperationContract]
        DeleteTransferResult DeleteInventoryTransferRequests(
            List<RecordIdentifier> inventoryTransferRequestId, bool reject, LogonInfo logonInfo);

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line">The transfer order line you want to get</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        [OperationContract]
        InventoryTransferRequestLine GetTransferRequestLine(LogonInfo logonInfo, InventoryTransferRequestLine line);

        [OperationContract]
        SaveTransferOrderLineResult SaveInventoryTransferRequestLine(LogonInfo logonInfo, InventoryTransferRequestLine inventoryTransferRequestLine, out RecordIdentifier newLineID);

        [OperationContract]
        SaveTransferOrderLineResult DeleteInventoryTransferRequestLine(LogonInfo logonInfo,
            RecordIdentifier inventoryTransferRequestLineId);

        [OperationContract]
        SaveTransferOrderLineResult DeleteInventoryTransferRequestLines(LogonInfo logonInfo,
            List<RecordIdentifier> inventoryTransferRequestLineIDs);

        [OperationContract]
        void SaveInventoryTransferRequestLineIfChanged(LogonInfo logonInfo,
            InventoryTransferRequestLine inventoryTransferRequestLine);

        /// <summary>
        /// Search inventory transfer requests based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferRequest> SearchInventoryTransferRequests(LogonInfo logonInfo, InventoryTransferFilter filter);

        /// <summary>
        /// Search inventory transfer requests based on an extended filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferRequest> SearchInventoryTransferRequestsExtended(LogonInfo logonInfo, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Search inventory transfer requests based on an extended filter with pagination
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="totalRecordsMatching">Total number of matching records based on the search criteria</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferRequest> SearchInventoryTransferRequestsAdvanced(LogonInfo logonInfo, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Get transfer request lines based on an extended search filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferRequestID">Transfer request ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(LogonInfo logonInfo, RecordIdentifier transferRequestID, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Creates a transfer reqiest from a specific order
        /// </summary>                
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderIDtoCopy">The ID of the transfer order to be copied</param>
        /// <param name="requestInformation">A order header object that has information needed to create a request header at head office if the information such as receiving or sending store should be different then on the copied order. 
        /// If null then the information on the request to be copied is used </param>                        
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>
        [OperationContract]
        CreateTransferOrderResult CreateTransferRequestFromOrder(
            LogonInfo logonInfo,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
            out RecordIdentifier newRequestID);

        /// <summary>
        /// Copies information from one transfer request to another
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestIDtoCopy">The ID of the transfer request to be copied</param>
        /// <param name="requestInformation">A request header object that has information needed to create an order header at head office if the information such as receiving or sending store should be different then on the copied request. 
        /// If null then the information of the request to be copied is used </param>                        
        /// <param name="newRequestID">Returns the ID of the transfer request that has been created</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CopyTransferRequest(
            LogonInfo logonInfo,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferRequest requestInformation,
            out RecordIdentifier newRequestID);

        /// <summary>
        /// Creates a new transfer request using items from a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestInformation">A request header object that has information needed to create a request header at head office if the information such as receiving or sending store should be different then on the copied request. </param> 
        /// <param name="filter">Filter container from which to create the lines for the transfer request</param>
        /// <param name="newRequestID">Returns the ID of the transfer request that has been created</param>
        /// <returns></returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferRequestFromFilter(
            LogonInfo logonInfo,
            InventoryTransferRequest requestInformation,
            InventoryTemplateFilterContainer filter,
            out RecordIdentifier newRequestID);

        /// <summary>
        /// Create a new transfer request based on a given template
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestInformation">A request header object that has information needed to create a request header at head office.</param> 
        /// <param name="template">The transfer request template</param>
        /// <param name="newRequestID">The ID of the new transfer request that was created</param>
        /// <returns>Status and ID of the created transfer request</returns>
        [OperationContract]
        CreateTransferOrderResult CreateTransferRequestFromTemplate(LogonInfo logonInfo, InventoryTransferRequest requestInformation, TemplateListItem template, out RecordIdentifier newRequestID);

    }
}
