using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.Services
{
    public partial class InventoryService 
    {
        public virtual InventoryTransferRequest GetInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferRequestId, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetInventoryTransferRequest(entry, siteServiceProfile, transferRequestId, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
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
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetTransferRequestLine(entry, siteServiceProfile, line, closeConnection);
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual RecordIdentifier SaveInventoryTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequest inventoryTransferRequest, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequest(entry, siteServiceProfile, inventoryTransferRequest, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual DeleteTransferResult DeleteTransferRequest(
         IConnectionManager entry,
         RecordIdentifier transferRequestId,
         bool reject,
         SiteServiceProfile siteServiceProfile
            )
        {            
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                return siteServiceService.DeleteInventoryTransferRequest(entry, siteServiceProfile, transferRequestId, reject, true);
            }
            catch
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual DeleteTransferResult DeleteTransferRequests(
         IConnectionManager entry,
         List<RecordIdentifier> transferRequestIds,
         bool reject,
         SiteServiceProfile siteServiceProfile
            )
        {
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                return siteServiceService.DeleteInventoryTransferRequests(entry, siteServiceProfile, transferRequestIds, reject, true);
            }
            catch
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual SaveTransferOrderLineResult SaveInventoryTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferRequestLine inventoryTransferRequestLine, out RecordIdentifier newLineID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferRequestLine(entry, siteServiceProfile, inventoryTransferRequestLine, out newLineID, closeConnection);
            }
            catch
            {
                newLineID = RecordIdentifier.Empty;
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferRequestID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetRequestLinesForInventoryTransferAdvanced(entry, siteServiceProfile, transferRequestID, out totalRecordsMatching, filter, closeConnection);
        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestLineID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferRequestLine(entry, siteServiceProfile, inventoryTransferRequestLineID, closeConnection);
            }
            catch
            {
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferRequestLineIDs, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferRequestLines(entry, siteServiceProfile, inventoryTransferRequestLineIDs, closeConnection);
            }
            catch
            {
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryStatus> GetInventoryStatuses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID,
            InventoryGroup inventoryGroup, RecordIdentifier inventoryGroupID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetInventoryStatuses(entry, siteServiceProfile, storeID, inventoryGroup, inventoryGroupID, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual bool ItemWithSameParametersExistsInTransferRequest(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
           InventoryTransferRequestLine line, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).ItemWithSameParametersExistsInTransferRequest(entry, siteServiceProfile, line, closeConnection);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequests(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferRequests(entry, siteServiceProfile, filter, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferRequestsExtended(entry, siteServiceProfile, filter, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferRequestsAdvanced(entry, siteServiceProfile, out totalRecordsMatching, filter, closeConnection);
            }            
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }        

        public virtual SendTransferOrderResult SendTransferRequests(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> requestIDs,
            bool closeConnection)
        {
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.SendInventoryTransferRequests(entry,
                                                                        siteServiceProfile,
                                                                        requestIDs,
                                                                        DateTime.Now,
                                                                        true);
            }
            catch
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual SendTransferOrderResult SendTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestID,
            bool closeConnection)
        {                       
            
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                
                return siteServiceService.SendInventoryTransferRequest(entry, 
                                                                        siteServiceProfile,
                                                                        requestID, 
                                                                        DateTime.Now, 
                                                                        true);
            }
            catch
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }            
        }        

        public virtual CreateTransferOrderResult CreateTransferOrdersFromRequests(
           IConnectionManager entry,
           List<RecordIdentifier> requestIDs,
           RecordIdentifier createdBy,
           SiteServiceProfile siteServiceProfile)
        {
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.CreateTransferOrdersFromRequests(
                                                        entry,
                                                        requestIDs,
                                                        createdBy,
                                                        siteServiceProfile,                                                        
                                                        true);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);                
            }
        }
        
        public virtual CreateTransferOrderResult CreateTransferRequestFromOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferRequest requestInformation = new InventoryTransferRequest();
                requestInformation.CreationDate = DateTime.Now;
                requestInformation.SendingStoreId = sendingStoreID;
                requestInformation.ReceivingStoreId = receivingStoreID;
                requestInformation.Text = description;
                requestInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                requestInformation.CreatedBy = createdBy;

                return CreateTransferRequestFromOrder(entry, siteServiceProfile, orderIDtoCopy, requestInformation, ref newRequestID, closeConnection);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.CreateTransferRequestFromOrder(
                                                        entry,
                                                        siteServiceProfile,
                                                        orderIDtoCopy,
                                                        requestInformation,
                                                        ref newRequestID,
                                                        closeConnection);                                                        
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CopyTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.CopyTransferRequest(
                                                        entry,
                                                        siteServiceProfile,
                                                        requestIDtoCopy,
                                                        requestInformation,
                                                        ref newRequestID,
                                                        closeConnection);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CopyTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferRequest requestInformation = new InventoryTransferRequest();
                requestInformation.CreationDate = DateTime.Now;
                requestInformation.SendingStoreId = sendingStoreID;
                requestInformation.ReceivingStoreId = receivingStoreID;
                requestInformation.Text = description;
                requestInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                requestInformation.CreatedBy = createdBy;

                return CopyTransferRequest(entry, siteServiceProfile, requestIDtoCopy, requestInformation, ref newRequestID, closeConnection);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newRequestID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferRequest requestInformation = new InventoryTransferRequest();
                requestInformation.SendingStoreId = sendingStoreID;
                requestInformation.ReceivingStoreId = receivingStoreID;
                requestInformation.Text = description;
                requestInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                requestInformation.CreatedBy = createdBy;

                return CreateTransferRequestFromFilter(entry, siteServiceProfile, requestInformation, filter, ref newRequestID, closeConnection);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
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
                return Interfaces.Services.SiteServiceService(entry).CreateTransferRequestFromFilter(entry, siteServiceProfile, requestInformation, filter, ref newRequestID, closeConnection);
            }
            catch
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferRequest requestInformation, TemplateListItem template, ref RecordIdentifier newRequestID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreateTransferRequestFromTemplate(entry, siteServiceProfile, requestInformation, template, ref newRequestID, closeConnection);
            }
            catch (Exception)
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }
    }
}