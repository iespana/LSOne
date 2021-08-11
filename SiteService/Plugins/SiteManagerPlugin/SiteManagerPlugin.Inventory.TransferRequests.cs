using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Replenishment;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Gets the transfer request with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferRequestId"></param>
        /// <returns></returns>
        public virtual InventoryTransferRequest GetInventoryTransferRequest(LogonInfo logonInfo, RecordIdentifier transferRequestId)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferRequestId)}: {transferRequestId}");
                return Providers.InventoryTransferRequestData.Get(dataModel, transferRequestId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual InventoryTransferRequestContainer GetInventoryTransferRequestContainer(IConnectionManager entry, RecordIdentifier transferRequestId)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferRequestId)}: {transferRequestId}");
                // Get information about the transfer request including item lines for the order          
                List<InventoryTransferRequestContainer> requests = GetInventoryTransferRequestsAndLines(entry, new List<RecordIdentifier> { transferRequestId });

                if (requests.Count > 0)
                {
                    return requests[0];
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual RecordIdentifier SaveInventoryTransferRequest(IConnectionManager dataModel, InventoryTransferRequest inventoryTransferRequest)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.InventoryTransferRequestData.Save(dataModel, inventoryTransferRequest);

                Utils.Log(this, "Transfer request created: " + inventoryTransferRequest.ID);
                return inventoryTransferRequest.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the given inventory request
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferRequest"></param>
        /// <returns></returns>
        public virtual RecordIdentifier SaveInventoryTransferRequest(LogonInfo logonInfo, InventoryTransferRequest inventoryTransferRequest)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveInventoryTransferRequest(dataModel, inventoryTransferRequest);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Checks whether an item with the same parameters (eg: same unit) exists in the transfer request containing the given transfer request line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual bool ItemWithSameParametersExistsInTransferRequest(LogonInfo logonInfo, InventoryTransferRequestLine line)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferRequestLineData.ItemWithSameParametersExists(dataModel, line);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the given transfer request line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferRequestLine"></param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult SaveInventoryTransferRequestLine(LogonInfo logonInfo, InventoryTransferRequestLine inventoryTransferRequestLine, out RecordIdentifier newLineID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            newLineID = RecordIdentifier.Empty;

            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveInventoryTransferRequestLine(dataModel, inventoryTransferRequestLine, ref newLineID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual SaveTransferOrderLineResult SaveInventoryTransferRequestLine(IConnectionManager dataModel, InventoryTransferRequestLine inventoryTransferRequestLine, ref RecordIdentifier newLineID)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                InventoryTransferRequest request = Providers.InventoryTransferRequestData.Get(dataModel, inventoryTransferRequestLine.InventoryTransferRequestId);

                if (request == null)
                {
                    Utils.Log(this, "Request not found");
                    return SaveTransferOrderLineResult.NotFound;
                }

                if (request.FetchedByReceivingStore)
                {
                    Utils.Log(this, "Request already received, a new line cannot be added");
                    return SaveTransferOrderLineResult.TransferOrderAlreadyReceived;
                }

                if (request.Rejected)
                {
                    Utils.Log(this, "Request already rejected, a new line cannot be added");
                    return SaveTransferOrderLineResult.TransferOrderAlreadyRejected;
                }

                Providers.InventoryTransferRequestLineData.Save(dataModel, inventoryTransferRequestLine);
                newLineID = inventoryTransferRequestLine.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return SaveTransferOrderLineResult.Success;
        }

        /// <summary>
        /// Gets a list of lines for a given transfer request ID, matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferRequestID"></param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(
            LogonInfo logonInfo,
            RecordIdentifier transferRequestID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferRequestID)}: {transferRequestID}");
                return Providers.InventoryTransferRequestLineData.GetRequestLinesForInventoryTransferAdvanced(dataModel, transferRequestID, out totalRecordsMatching, filter);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line">The transfer request line you want to get</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        public virtual InventoryTransferRequestLine GetTransferRequestLine(LogonInfo logonInfo, InventoryTransferRequestLine line)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(line)}: {line}");
                return Providers.InventoryTransferRequestLineData.GetLine(dataModel, line);
            }
            catch (Exception e)
            {

                Utils.LogException(this, e);
                throw GetChannelError(e);

            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a transfer request line with a given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferRequestLineId"></param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLine(LogonInfo logonInfo, RecordIdentifier inventoryTransferRequestLineId)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTransferRequestLineId)}: {inventoryTransferRequestLineId}");
                InventoryTransferRequestLine requestLine = Providers.InventoryTransferRequestLineData.Get(dataModel, inventoryTransferRequestLineId);

                if (requestLine == null)
                {
                    return SaveTransferOrderLineResult.NotFound;
                }

                InventoryTransferRequest request = Providers.InventoryTransferRequestData.Get(dataModel, requestLine.InventoryTransferRequestId);

                if (request != null && request.FetchedByReceivingStore)
                {
                    return SaveTransferOrderLineResult.TransferOrderAlreadyReceived;
                }

                if (request != null && request.Rejected)
                {
                    return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
                }

                Providers.InventoryTransferRequestLineData.Delete(dataModel, inventoryTransferRequestLineId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return SaveTransferOrderLineResult.Success;
        }

        /// <summary>
        /// Deletes a given list of transfer request lines
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferRequestLineIDs"></param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult DeleteInventoryTransferRequestLines(LogonInfo logonInfo, List<RecordIdentifier> inventoryTransferRequestLineIDs)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                InventoryTransferRequest request = null;

                foreach (RecordIdentifier inventoryTransferRequestLineId in inventoryTransferRequestLineIDs)
                {
                    InventoryTransferRequestLine requestLine = Providers.InventoryTransferRequestLineData.Get(dataModel, inventoryTransferRequestLineId);

                    if (requestLine == null)
                    {
                        Utils.Log(this, "Request line not found - ID: " + inventoryTransferRequestLineId);
                        result = SaveTransferOrderLineResult.NotFound;
                        continue;
                    }

                    if(request == null)
                    {
                        Utils.Log(this, "Get information about transfer request header - ID: " + requestLine.InventoryTransferRequestId);
                        request = Providers.InventoryTransferRequestData.Get(dataModel, requestLine.InventoryTransferRequestId);
                    }

                    if (request != null && request.FetchedByReceivingStore)
                    {
                        Utils.Log(this, "Request already sent, line cannot be deleted");
                        result = SaveTransferOrderLineResult.TransferOrderAlreadySent;
                        break;
                    }

                    if (request != null && request.Rejected)
                    {
                        Utils.Log(this, "Request already rejected, line cannot be deleted");
                        result = SaveTransferOrderLineResult.TransferOrderAlreadyRejected;
                        break;
                    }

                    Providers.InventoryTransferRequestLineData.Delete(dataModel, inventoryTransferRequestLineId);
                }                
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return result;
        }

        /// <summary>
        /// Detects whether a transfer request line has changed and if it did, updates it in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferRequestLine"></param>
        public virtual void SaveInventoryTransferRequestLineIfChanged(LogonInfo logonInfo, InventoryTransferRequestLine inventoryTransferRequestLine)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.InventoryTransferRequestLineData.SaveIfChanged(dataModel, inventoryTransferRequestLine);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of transfer request matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequests(LogonInfo logonInfo, InventoryTransferFilter filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferRequestData.Search(dataModel, filter);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of transfer request matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsExtended(LogonInfo logonInfo, InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferRequestData.Search(dataModel, filter);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of transfer request matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferRequest> SearchInventoryTransferRequestsAdvanced(
            LogonInfo logonInfo,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferRequestData.AdvancedSearch(dataModel, out totalRecordsMatching, filter);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Sends a list of given transfer requests
        /// </summary>
        /// <param name="requestIDs"></param>
        /// <param name="sendingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual SendTransferOrderResult SendInventoryTransferRequests(
            List<RecordIdentifier> requestIDs,
            DateTime sendingTime, LogonInfo logonInfo)
        {
            SendTransferOrderResult currentSendingResult = SendTransferOrderResult.Success;
            SendTransferOrderResult errorSendingResult = SendTransferOrderResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                foreach (RecordIdentifier ID in requestIDs)
                {
                    Utils.Log(this, "Sending transfer request ID: " + ID);
                    currentSendingResult = SendInventoryTransferRequest(ID, sendingTime, logonInfo);
                    if (currentSendingResult != SendTransferOrderResult.Success && currentSendingResult > errorSendingResult)
                    {
                        Utils.Log(this, "Error sending transfer request");
                        errorSendingResult = currentSendingResult;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return errorSendingResult;
        }

        /// <summary>
        /// Sends a transfer request with the given ID
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="sendingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual SendTransferOrderResult SendInventoryTransferRequest(
            RecordIdentifier requestID,
            DateTime sendingTime, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestID)}: {requestID}");
                InventoryTransferRequestContainer transferRequest = GetInventoryTransferRequestContainer(dataModel, requestID);
                if (transferRequest == null)
                {
                    Utils.Log(this, "Transfer request not found");
                    return SendTransferOrderResult.NotFound;
                }                                

                // Exists previously and has been fetched by the receiving store. Not possible to resent
                if (transferRequest.InventoryTransferRequest.FetchedByReceivingStore)
                {
                    Utils.Log(this, "Transfer request fetched by receiving store, request cannot be sent");
                    return SendTransferOrderResult.FetchedByReceivingStore;
                }

                if (transferRequest.InventoryTransferRequest.Sent && transferRequest.InventoryTransferRequestLines.All(x => x.Sent))
                {
                    Utils.Log(this, "Transfer request already sent");
                    return SendTransferOrderResult.TransferAlreadySent;
                }

                if (transferRequest.InventoryTransferRequest.Rejected)
                {
                    Utils.Log(this, "Transfer request has been rejected, cannot be sent");
                    return SendTransferOrderResult.TransferOrderIsRejected;
                }

                if (!transferRequest.InventoryTransferRequestLines.Any())
                {
                    Utils.Log(this, "Transfer request has no items, cannot be sent");
                    return SendTransferOrderResult.NoItemsOnTransfer;
                }

                // If any lines on the transfer request have Sent quantity as 0, then the request cannot be sent
                if (transferRequest.InventoryTransferRequestLines.Any(a => a.QuantityRequested == 0))
                {
                    Utils.Log(this, "Transfer request has lines that has Requested Qty == 0, cannot be sent");
                    return SendTransferOrderResult.LinesHaveZeroSentQuantity;
                }

                // If any of the items on the transfer request have a unit conversion error, the transfer request cannot be sent
                if (!UnitConversionOnTransferRequest(dataModel, transferRequest.InventoryTransferRequest.ID))
                {
                    Utils.Log(this, "Transfer request has unit conversion error on it, cannot be sent");
                    return SendTransferOrderResult.UnitConversionError;
                }

                //******************************************************

                // The transfer request is ready to be updated and set as sent                

                //******************************************************

                // Mark the request as sent, save it and save the transfer lines
                transferRequest.InventoryTransferRequest.Sent = true;
                transferRequest.InventoryTransferRequest.SentDate = sendingTime;
                Providers.InventoryTransferRequestData.Save(dataModel, transferRequest.InventoryTransferRequest);
                Utils.Log(this, "Transfer request updated and saved");

                Providers.InventoryTransferRequestLineData.MarkTransferRequestLinesAsSent(dataModel, transferRequest.InventoryTransferRequest.ID);

                Utils.Log(this, "Transfer request lines updated and saved");
                return SendTransferOrderResult.Success;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes a given list of transfer requests
        /// </summary>
        /// <param name="inventoryTransferRequestIds"></param>
        /// <param name="reject"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual DeleteTransferResult DeleteInventoryTransferRequests(
            List<RecordIdentifier> inventoryTransferRequestIds, bool reject, LogonInfo logonInfo)
        {
            DeleteTransferResult currentDeleteResult = DeleteTransferResult.Success;
            DeleteTransferResult errorDeleteResult = DeleteTransferResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                foreach (RecordIdentifier ID in inventoryTransferRequestIds)
                {
                    currentDeleteResult = DeleteInventoryTransferRequest(ID, reject, logonInfo);
                    if (currentDeleteResult != DeleteTransferResult.Success && currentDeleteResult > errorDeleteResult)
                    {
                        Utils.Log(this, "Error deleting transfer request");
                        errorDeleteResult = currentDeleteResult;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return DeleteTransferResult.ErrorDeletingTransfer;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return errorDeleteResult;
        }
        
        /// <summary>
        /// Deletes a transfer request with the given ID
        /// </summary>
        /// <param name="inventoryTransferRequestId"></param>
        /// <param name="reject"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual DeleteTransferResult DeleteInventoryTransferRequest(
            RecordIdentifier inventoryTransferRequestId, bool reject, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTransferRequestId)}: {inventoryTransferRequestId}");
                InventoryTransferRequest transferRequest = Providers.InventoryTransferRequestData.Get(dataModel, inventoryTransferRequestId);

                if (transferRequest == null)
                {
                    Utils.Log(this, "Request not found");
                    return DeleteTransferResult.NotFound;
                }

                if (transferRequest.Rejected)
                {
                    Utils.Log(this, "Request already rejected, not changes done");
                    return DeleteTransferResult.Success;
                }

                if (transferRequest.InventoryTransferOrderCreated)
                {
                    Utils.Log(this, "Request already used to create a transfer order, cannot be deleted");
                    return DeleteTransferResult.Received;
                }

                if (reject)
                {
                    Utils.Log(this, "Request is to be rejected, not deleted");

                    transferRequest.Rejected = true;
                    Providers.InventoryTransferRequestData.Save(dataModel, transferRequest);
                }
                else
                {
                    if (transferRequest.FetchedByReceivingStore)
                    {
                        Utils.Log(this, "Request feteched by store or already rejected, cannot be deleted");
                        return DeleteTransferResult.FetchedByReceivingStore;
                    }

                    Providers.InventoryTransferRequestData.Delete(dataModel, inventoryTransferRequestId);
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return DeleteTransferResult.Success;
        }

        /// <summary>
        /// Gets a list of transfer requests together with their lines, based on a given list of identifiers of documents to fetch
        /// </summary>
        /// <param name="listOfTransferIdsToFetch"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<InventoryTransferRequestContainer> GetInventoryTransferRequestsAndLines(
            List<RecordIdentifier> listOfTransferIdsToFetch, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return GetInventoryTransferRequestsAndLines(dataModel, listOfTransferIdsToFetch);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return new List<InventoryTransferRequestContainer>();
        }

        protected virtual bool UnitConversionOnTransferRequest(IConnectionManager entry, RecordIdentifier transferRequestID)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                List<string> errors = Providers.InventoryTransferRequestData.CheckUnitConversionsForTransferRequest(entry, transferRequestID);

                if (errors.Any())
                {
                    errors.ForEach(err => Utils.Log(this, err, LogLevel.Error));
                    return false;
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual List<InventoryTransferRequestContainer> GetInventoryTransferRequestsAndLines(
            IConnectionManager dataModel,
            List<RecordIdentifier> listOfTransferIdsToFetch)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                List<InventoryTransferRequestContainer> result = new List<InventoryTransferRequestContainer>();

                List<InventoryTransferRequest> inventoryTransferRequests =
                    Providers.InventoryTransferRequestData.GetFromList(dataModel, listOfTransferIdsToFetch, InventoryTransferOrderSortEnum.Id, false);

                foreach (InventoryTransferRequest inventoryTransferRequest in inventoryTransferRequests)
                {
                    InventoryTransferRequestContainer container = new InventoryTransferRequestContainer
                    {
                        InventoryTransferRequest = inventoryTransferRequest,
                        InventoryTransferRequestLines =
                            Providers.InventoryTransferRequestLineData.GetListForInventoryTransferRequest(
                                dataModel,
                                inventoryTransferRequest.ID,
                                InventoryTransferOrderLineSortEnum.ItemName,
                                false)
                    };
                    result.Add(container);
                }

                return result;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return new List<InventoryTransferRequestContainer>();
        }

        /// <summary>
        /// Creates a transfer request based on a transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderIDtoCopy"></param>
        /// <param name="requestInformation"></param>
        /// <param name="newRequestID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CreateTransferRequestFromOrder(
            LogonInfo logonInfo,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferRequest requestInformation,
            out RecordIdentifier newRequestID)
        {
            newRequestID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(orderIDtoCopy)}: {orderIDtoCopy}");

                // Get information about the request that is to be copied
                InventoryTransferContainer container = GetInventoryTransferOrderContainer(dataModel, orderIDtoCopy);

                if (container == null)
                {
                    Utils.Log(this, "Transfer order could not be found");
                    return CreateTransferOrderResult.RequestNotFound;
                }

                RecordIdentifier returnOrderID = RecordIdentifier.Empty;
                // Create the transfer order from the request
                CreateTransferOrderResult result = CreateTransferRequestFromOrder(dataModel, container, requestInformation, ref returnOrderID);

                Utils.Log(this, "Transfer request created - ID: " + returnOrderID);
                // Send the ID back to the caller
                newRequestID = returnOrderID;
                return result;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                newRequestID = RecordIdentifier.Empty;
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual CreateTransferOrderResult CreateTransferRequestFromOrder(
            IConnectionManager dataModel,
            InventoryTransferContainer orderToCopy,
            InventoryTransferRequest requestInformation,
            ref RecordIdentifier newRequestID)
        {
            newRequestID = RecordIdentifier.Empty;

            try
            {
                Utils.Log(this, Utils.Starting);
                InventoryTransferOrder order = orderToCopy.InventoryTransferOrder;
                List<InventoryTransferOrderLine> orderLines = orderToCopy.InventoryTransferLines;

                if (order == null)
                {
                    Utils.Log(this, "Order to copy not found");
                    return CreateTransferOrderResult.OrderNotFound;
                }

                // If the request information that is sent in is valid then use that to create the request header
                if (requestInformation != null)
                {
                    Utils.Log(this, "Use information received to create a request header");
                    requestInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, requestInformation.ExpectedDelivery, requestInformation.SendingStoreId);
                    requestInformation.CreationDate = DateTime.Now;

                    requestInformation.ID = SaveInventoryTransferRequest(dataModel, requestInformation);
                }
                else
                {
                    Utils.Log(this, "Create an order header using the information from the request to be copied");
                    requestInformation = CreateTransferRequestHeader(dataModel,
                                                                order.SendingStoreId,
                                                                order.ReceivingStoreId,
                                                                order.Text,
                                                                order.ExpectedDelivery,
                                                                RecordIdentifier.Empty,
                                                                RecordIdentifier.Empty);
                }

                // If the request information still hasn't been successfully created
                if (requestInformation == null)
                {
                    Utils.Log(this, "Request header could not be created");
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, requestInformation))
                {
                    Utils.Log(this, "Not enough information on the request header to continue");
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }

                Utils.Log(this, "Request header created - ID: " + requestInformation.ID);
                // Send the ID back to the caller
                newRequestID = requestInformation.ID;

                Utils.Log(this, "Start copying order lines");
                Providers.InventoryTransferRequestLineData.CopyLinesFromOrder(dataModel, order.ID, newRequestID);
                Utils.Log(this, "Copying done");

                // Make sure that all the lines were copied and return information about how it went
                int orgNoOfLines = Providers.InventoryTransferOrderLineData.LineCount(dataModel, order.ID);
                int newOrderLines = Providers.InventoryTransferRequestLineData.LineCount(dataModel, newRequestID);
                Utils.Log(this, "Lines copied: " + newOrderLines.ToString());

                if (newOrderLines == 0)
                {
                    Utils.Log(this, "No lines copied");
                    return CreateTransferOrderResult.NoLinesCreated;
                }
                else if (orgNoOfLines != newOrderLines)
                {
                    Utils.Log(this, "Not all lines copied");
                    return CreateTransferOrderResult.NotAllLinesCreated;
                }             
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return CreateTransferOrderResult.Success;

        }

        protected virtual InventoryTransferRequest CreateTransferRequestHeader(IConnectionManager entry, RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                                                                               string description, DateTime expectedDelivery, RecordIdentifier createdBy, RecordIdentifier createdFromOrderID)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(sendingStoreID)}: {sendingStoreID}, {nameof(receivingStoreID)}: {receivingStoreID}, {nameof(description)}: {description}, {nameof(createdFromOrderID)}: {createdFromOrderID}");
                InventoryTransferRequest newRequest = new InventoryTransferRequest();

                newRequest.CreationDate = DateTime.Now;
                newRequest.InventoryTransferOrderId = createdFromOrderID;
                newRequest.SendingStoreId = sendingStoreID;
                newRequest.ReceivingStoreId = receivingStoreID;
                newRequest.Text = description;
                newRequest.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                newRequest.CreatedBy = createdBy;

                newRequest.ID = SaveInventoryTransferRequest(entry, newRequest);

                Utils.Log(this, "Transfer request created - ID: " + newRequest.ID);
                return newRequest;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return null;
            }
            finally
            {             
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual bool CheckTransferHeader(IConnectionManager entry, InventoryTransferRequest request)
        {
            try
            {
                Utils.Log(this, Utils.Starting);

                if (request == null)
                {
                    Utils.Log(this, "Request to be checked is null", LogLevel.Trace);
                    return false;
                }

                if (storeList == null || !storeList.Any())
                {
                    Utils.Log(this, "Retrieve store list for cache", LogLevel.Trace);
                    storeList = Providers.StoreData.GetList(entry);
                    if (storeList == null)
                    {
                        Utils.Log(this, "No stores found in database", LogLevel.Trace);
                        storeList = new List<DataEntity>();
                    }
                }

                if (RecordIdentifier.IsEmptyOrNull(request.SendingStoreId) || storeList.FirstOrDefault(f => f.ID == request.SendingStoreId) == null)
                {
                    Utils.Log(this, "No sending store ID on request header", LogLevel.Trace);
                    return false;
                }

                if (RecordIdentifier.IsEmptyOrNull(request.ReceivingStoreId) || storeList.FirstOrDefault(f => f.ID == request.ReceivingStoreId) == null)
                {
                    Utils.Log(this, "No receiving store ID on order header", LogLevel.Trace);
                    return false;
                }
            }            
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return true;
        }

        /// <summary>
        /// Copies or clones an existing transfer request
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestIDtoCopy"></param>
        /// <param name="requestInformation"></param>
        /// <param name="newRequestID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CopyTransferRequest(
            LogonInfo logonInfo,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferRequest requestInformation,
            out RecordIdentifier newRequestID)
        {
            newRequestID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestIDtoCopy)}: {requestIDtoCopy}");

                // Get information about the transfer request that is to be copied
                InventoryTransferRequestContainer orgRequestContainer = GetInventoryTransferRequestContainer(dataModel, requestIDtoCopy);
                InventoryTransferRequest orgRequest = orgRequestContainer.InventoryTransferRequest;

                if (orgRequestContainer == null || orgRequest == null)
                {
                    Utils.Log(this, "Request to copy not found");
                    return CreateTransferOrderResult.RequestNotFound;
                }

                // If the request information that is sent in is valid then use that to create an request header
                if (requestInformation != null)
                {
                    Utils.Log(this, "Use information received to create a request header");
                    requestInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, requestInformation.ExpectedDelivery, requestInformation.SendingStoreId);
                    requestInformation.CreationDate = DateTime.Now;
                    requestInformation.ID = SaveInventoryTransferRequest(dataModel, requestInformation);
                }
                else
                {
                    Utils.Log(this, "Create an request header using the information from the request to be copied");
                    requestInformation = CreateTransferRequestHeader(dataModel,
                                                                orgRequest.SendingStoreId,
                                                                orgRequest.ReceivingStoreId,
                                                                orgRequest.Text,
                                                                orgRequest.ExpectedDelivery,
                                                                RecordIdentifier.Empty,
                                                                RecordIdentifier.Empty);
                }

                // If the request still hasn't been successfully created
                if (requestInformation == null)
                {
                    Utils.Log(this, "Request could not be created");
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, requestInformation))
                {
                    Utils.Log(this, "Not enough information on the request header to continue");
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }

                Utils.Log(this, "New request created - ID: " + newRequestID);
                // Send the ID back to the caller
                newRequestID = requestInformation.ID;

                Utils.Log(this, "Start copying request lines");
                Providers.InventoryTransferRequestLineData.CopyLines(dataModel, requestIDtoCopy, newRequestID);
                Utils.Log(this, "Copying done");

                // Make sure that all the lines were copied and return information about how it went
                int orgNoOfLines = Providers.InventoryTransferRequestLineData.LineCount(dataModel, requestIDtoCopy);
                int newOrderLines = Providers.InventoryTransferRequestLineData.LineCount(dataModel, newRequestID);
                Utils.Log(this, "Lines copied: " + newOrderLines.ToString());

                if (newOrderLines == 0)
                {
                    Utils.Log(this, "No lines copied");
                    return CreateTransferOrderResult.NoLinesCreated;
                }
                else if (orgNoOfLines != newOrderLines)
                {
                    Utils.Log(this, "Not all lines copied");
                    return CreateTransferOrderResult.NotAllLinesCreated;
                }               

                return CreateTransferOrderResult.Success;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Creates a new transfer request based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestInformation"></param>
        /// <param name="filter"></param>
        /// <param name="newRequestID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CreateTransferRequestFromFilter(
            LogonInfo logonInfo,
            InventoryTransferRequest requestInformation,
            InventoryTemplateFilterContainer filter,
            out RecordIdentifier newRequestID)
        {
            newRequestID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                if (requestInformation != null && RecordIdentifier.IsEmptyOrNull(requestInformation.ID))
                {
                    requestInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, requestInformation.ExpectedDelivery, requestInformation.SendingStoreId);
                    requestInformation.CreationDate = DateTime.Now;
                    requestInformation.ID = SaveInventoryTransferRequest(dataModel, requestInformation);
                    Utils.Log(this, "New request created with ID: " + (string)requestInformation.ID);
                }

                if (requestInformation == null)
                {
                    Utils.Log(this, "Request header could not be created");
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, requestInformation))
                {
                    Utils.Log(this, "Not enough information on the request header to continue");
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }

                Utils.Log(this, "New request created - ID: " + newRequestID);
                // Send the ID back to the caller
                newRequestID = requestInformation.ID;

                if(filter.HasFilterCriteria())
                {
                    Utils.Log(this, "Saving lines starting");
                    int savedLinesCount = Providers.InventoryTransferData.CreateStoreTransferLinesFromFilter(dataModel, requestInformation.ID, requestInformation.SendingStoreId,  filter, StoreTransferTypeEnum.Request);
                    Utils.Log(this, $"Saved {savedLinesCount} lines");
                }

                return CreateTransferOrderResult.Success;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual CreateTransferOrderResult CreateTransferRequestFromTemplate(LogonInfo logonInfo, InventoryTransferRequest requestInformation, TemplateListItem template, out RecordIdentifier newOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Creating transfer request from template: " + template?.TemplateID);

                if (template == null
                    || RecordIdentifier.IsEmptyOrNull(template.TemplateID)
                    || !Providers.InventoryTemplateData.Exists(entry, template.TemplateID))
                {
                    Utils.Log(this, $"Template {template?.TemplateID?.StringValue} not found.");
                    newOrderID = RecordIdentifier.Empty;
                    return CreateTransferOrderResult.TemplateNotFound;
                }

                List<InventoryTemplateSectionSelection> filters = Providers.InventoryTemplateSectionSelectionData.GetList(entry, template.TemplateID);
                InventoryTemplateFilterContainer filter = new InventoryTemplateFilterContainer(filters);

                CreateTransferOrderResult result = CreateTransferRequestFromFilter(logonInfo, requestInformation, filter, out newOrderID);
                return result;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}