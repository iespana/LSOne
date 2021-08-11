using System.Linq;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual InventoryTransferRequest GetInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferRequestId, bool closeConnection)
        {
            InventoryTransferRequest result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTransferRequest(CreateLogonInfo(entry), transferRequestId), closeConnection);

            return result;
        }

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="line">The transfer request line you want to get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        public virtual InventoryTransferRequestLine GetTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequestLine line, bool closeConnection)
        {
            InventoryTransferRequestLine result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetTransferRequestLine(CreateLogonInfo(entry), line), closeConnection);

            return result;
        }

        public virtual RecordIdentifier SaveInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequest inventoryTransferRequest, bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryTransferRequest(CreateLogonInfo(entry), inventoryTransferRequest), closeConnection);
            return result;
        }

        public virtual DeleteTransferResult DeleteInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTransferRequestId, bool reject, bool closeConnection)
        {
            DeleteTransferResult result = DeleteTransferResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferRequest(inventoryTransferRequestId, reject, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual DeleteTransferResult DeleteInventoryTransferRequests(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferRequestIds, bool reject, bool closeConnection)
        {
            DeleteTransferResult result = DeleteTransferResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferRequests(inventoryTransferRequestIds, reject, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual bool ItemWithSameParametersExistsInTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequestLine line, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.ItemWithSameParametersExistsInTransferRequest(CreateLogonInfo(entry), line), closeConnection);

            return result;
        }

        public virtual SaveTransferOrderLineResult SaveInventoryTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequestLine inventoryTransferRequestLine, out RecordIdentifier newLineID, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            RecordIdentifier returnLineID = RecordIdentifier.Empty;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryTransferRequestLine(CreateLogonInfo(entry), inventoryTransferRequestLine, out returnLineID), closeConnection);
            newLineID = returnLineID;
            return result;

        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestLineId, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferRequestLine(CreateLogonInfo(entry), inventoryTransferRequestLineId), closeConnection);
            return result;

        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferRequestLineIDs, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferRequestLines(CreateLogonInfo(entry), inventoryTransferRequestLineIDs), closeConnection);
            return result;
        }

        public virtual List<InventoryStatus> GetInventoryStatuses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID,
            InventoryGroup inventoryGroup, RecordIdentifier inventoryGroupID, bool closeConnection)
        {
            List<InventoryStatus> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result =
                        server.GetInventoryStatuses(CreateLogonInfo(entry), storeID, inventoryGroup, inventoryGroupID),
                closeConnection);
            return result;

        }

        public virtual void SaveInventoryTransferRequestLineIfChanged(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequestLine inventoryTransferRequestLine, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveInventoryTransferRequestLineIfChanged(CreateLogonInfo(entry), inventoryTransferRequestLine), closeConnection);
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequests(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection)
        {
            List<InventoryTransferRequest> result = new List<InventoryTransferRequest>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferRequests(CreateLogonInfo(entry), filter), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection)
        {
            List<InventoryTransferRequest> result = new List<InventoryTransferRequest>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferRequestsExtended(CreateLogonInfo(entry), filter), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsAdvanced(IConnectionManager entry, SiteServiceProfile siteServiceProfile, out int totalRecordsMatching, InventoryTransferFilterExtended filter, bool closeConnection)
        {
            List<InventoryTransferRequest> result = new List<InventoryTransferRequest>();

            totalRecordsMatching = 0;
            int totalRecordsMatchingCopy = totalRecordsMatching;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferRequestsAdvanced(CreateLogonInfo(entry), out totalRecordsMatchingCopy, filter), closeConnection);

            totalRecordsMatching = totalRecordsMatchingCopy;

            return result;
        }

        public virtual SendTransferOrderResult SendInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestID, DateTime sendingTime, bool closeConnection)
        {
            SendTransferOrderResult result = SendTransferOrderResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SendInventoryTransferRequest(requestID, sendingTime, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual SendTransferOrderResult SendInventoryTransferRequests(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> requestIDs, DateTime sendingTime, bool closeConnection)
        {
            SendTransferOrderResult result = SendTransferOrderResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SendInventoryTransferRequests(requestIDs, sendingTime, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<InventoryTransferRequestContainer> GetInventoryTransferRequestsAndLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> listOfTransferIdsToFetch,
            bool closeConnection)
        {
            List<InventoryTransferRequestContainer> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTransferRequestsAndLines(listOfTransferIdsToFetch, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferRequestID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            List<InventoryTransferRequestLine> result = new List<InventoryTransferRequestLine>();

            totalRecordsMatching = 0;
            int totalRecordsMatchingCopy = totalRecordsMatching;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetRequestLinesForInventoryTransferAdvanced(CreateLogonInfo(entry), transferRequestID, out totalRecordsMatchingCopy, filter), closeConnection);

            totalRecordsMatching = totalRecordsMatchingCopy;

            return result;
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            CreateTransferOrderResult result = CreateTransferOrderResult.Success;
            RecordIdentifier returnRequestID = RecordIdentifier.Empty;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferRequestFromOrder(CreateLogonInfo(entry), orderIDtoCopy, requestInformation, out returnRequestID), closeConnection);
            newRequestID = returnRequestID;

            return result;
        }

        public virtual CreateTransferOrderResult CopyTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            RecordIdentifier returnRequestID = RecordIdentifier.Empty;

            CreateTransferOrderResult result = CreateTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CopyTransferRequest(CreateLogonInfo(entry), requestIDtoCopy, requestInformation, out returnRequestID), closeConnection);

            newRequestID = returnRequestID;
            return result;
        }
        
        public virtual CreateTransferOrderResult CreateTransferRequestFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferRequest requestInformation,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {
                RecordIdentifier returnRequestID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = CreateTransferOrderResult.Success;
                
                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferRequestFromFilter(CreateLogonInfo(entry), requestInformation, filter, out returnRequestID), closeConnection);

                newRequestID = returnRequestID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequest requestInformation, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                CreateTransferOrderResult result = CreateTransferOrderResult.Success;
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;

                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferRequestFromTemplate(CreateLogonInfo(entry), requestInformation, template, out returnOrderID), closeConnection);

                newOrderID = returnOrderID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }
    }
}
