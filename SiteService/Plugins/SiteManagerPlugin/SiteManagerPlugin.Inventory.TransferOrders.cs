using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        protected List<DataEntity> storeList;

        protected virtual InventoryTransferContainer GetInventoryTransferOrderContainer(IConnectionManager entry, RecordIdentifier transferID)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferID)}: {transferID}");

                // Get information about the transfer order including item lines for the order
                List<InventoryTransferContainer> transfers = GetInventoryTransferOrdersAndLines(entry, new List<RecordIdentifier> { transferID });

                if (transfers != null && transfers.Count > 0)
                {
                    Utils.Log(this, "Transfer order and lines found and returned");
                    return transfers[0];
                }
                
                return null;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return null;
        }

        /// <summary>
        /// Sends the transfer orders with the given identifiers
        /// </summary>
        /// <param name="transferIDs"></param>
        /// <param name="sendingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual SendTransferOrderResult SendTransferOrders(
            List<RecordIdentifier> transferIDs,
            DateTime sendingTime,
            LogonInfo logonInfo)
        {
            SendTransferOrderResult currentSendingResult = SendTransferOrderResult.Success;
            SendTransferOrderResult errorSendingResult = SendTransferOrderResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);

                foreach (RecordIdentifier ID in transferIDs)
                {
                    Utils.Log(this, "Sending order: " + (string)ID);
                    currentSendingResult = SendTransferOrder(ID, sendingTime, logonInfo);
                    if (currentSendingResult != SendTransferOrderResult.Success && currentSendingResult > errorSendingResult)
                    {
                        Utils.Log(this, "Error sending order: " + (string)ID);
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
        /// Sends the transfer order with the given ID
        /// </summary>
        /// <param name="transferID"></param>
        /// <param name="sendingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual SendTransferOrderResult SendTransferOrder(
            RecordIdentifier transferID,
            DateTime sendingTime,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferID)}: {transferID}");

                // ONE-10284 This validation block must be replicated in the OMNI server to avoid reactivating this bug (search for ONE-10284)

                // Get information about the item lines for the order
                InventoryTransferContainer transferOrder = GetInventoryTransferOrderContainer(dataModel, transferID);                
                if (transferOrder == null)
                {
                    Utils.Log(this, "Transfer order not found");
                    return SendTransferOrderResult.NotFound;
                }

                if(transferOrder.InventoryTransferOrder.ProcessingStatus != InventoryProcessingStatus.None)
                {
                    Utils.Log(this, "Transfer order is currently processing and cannot be sent.");
                    return SendTransferOrderResult.ErrorSendingTransferOrder;
                }

                // Has been fetched by the receiving store. Not possible to resend
                if (transferOrder.InventoryTransferOrder.FetchedByReceivingStore)
                {
                    Utils.Log(this, "Transfer order already fetched by receiving store, cannot be resent");
                    return SendTransferOrderResult.FetchedByReceivingStore;
                }

                if (transferOrder.InventoryTransferOrder.Sent)
                {
                    Utils.Log(this, "Transfer order already sent");
                    return SendTransferOrderResult.TransferAlreadySent;
                }

                if (transferOrder.InventoryTransferOrder.Rejected)
                {
                    Utils.Log(this, "Transfer order has been rejected, cannot be sent");
                    return SendTransferOrderResult.TransferOrderIsRejected;
                }

                if (!transferOrder.InventoryTransferLines.Any())
                {
                    Utils.Log(this, "Transfer order has no items, cannot be sent");
                    return SendTransferOrderResult.NoItemsOnTransfer;
                }

                // If any lines on the transfer order have Sent quantity as 0, then the order cannot be sent
                if (transferOrder.InventoryTransferLines.Any(a => a.QuantitySent == 0))
                {
                    Utils.Log(this, "Transfer order has lines that has Sent Qty == 0, cannot be sent");
                    return SendTransferOrderResult.LinesHaveZeroSentQuantity;
                }

                // If any of the items on the transfer order have a unit conversion error, the transfer order cannot be sent
                if (!UnitConversionOnTransferOrder(dataModel, transferOrder.ID))
                {
                    Utils.Log(this, "Transfer order has unit conversion error on it, cannot be sent");
                    return SendTransferOrderResult.UnitConversionError;
                }

                Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferOrder.InventoryTransferOrder.ID, InventoryProcessingStatus.Posting);

                //******************************************************

                // The transfer order is ready to be updated and set as sent                

                //******************************************************

                Utils.Log(this, "Transfer order ready to be sent");

                // Mark the transfer as sent, save it and save the transfer lines
                transferOrder.InventoryTransferOrder.Sent = true;
                transferOrder.InventoryTransferOrder.SentDate = sendingTime;
                Providers.InventoryTransferOrderData.Save(dataModel, transferOrder.InventoryTransferOrder);

                Utils.Log(this, "Transfer order header has been updated and sent");

                InventoryTemplate template = Providers.InventoryTemplateData.Get(dataModel, transferOrder.InventoryTransferOrder.TemplateID);

                Providers.InventoryTransferOrderLineData.SendTransferOrderLines(dataModel, transferOrder.InventoryTransferOrder.ID, template == null ? false : template.AutoPopulateTransferOrderReceivingQuantity);
                Utils.Log(this, "Transfer order lines have been updated and sent");
                
                bool updateInventorySuccess = Providers.InventoryTransferOrderData.UpdateInventoryForTransferOrder(dataModel, transferOrder.InventoryTransferOrder.ID, TransferOrderUpdateInventoryAction.Send, false, RecordIdentifier.Empty, RecordIdentifier.Empty);
                Utils.Log(this, "Updating the inventory finished");

                Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferOrder.InventoryTransferOrder.ID, InventoryProcessingStatus.None);

                if (!updateInventorySuccess)
                {
                    Utils.Log(this, "Failed to update inventory for transfer order " + transferOrder.InventoryTransferOrder.ID.StringValue, LogLevel.Error);
                    return SendTransferOrderResult.ErrorSendingTransferOrder;
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                if (dataModel != null)
                {
                    Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferID, InventoryProcessingStatus.None);
                }

                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return SendTransferOrderResult.Success;
        }

        protected virtual CreateTransferOrderResult CreateTransferOrderFromRequest(
            IConnectionManager dataModel,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID)
        {
            newOrderID = RecordIdentifier.Empty;            

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestIDtoCopy)}: {requestIDtoCopy}");

                // Get information about the request that is to be copied
                InventoryTransferRequestContainer container = GetInventoryTransferRequestContainer(dataModel, requestIDtoCopy);

                if (container == null)
                {
                    Utils.Log(this, "Transfer request not found");
                    return CreateTransferOrderResult.RequestNotFound;
                }

                Utils.Log(this, "Create the transfer order from the request");
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = CreateTransferOrderFromRequest(dataModel, container, orderInformation, ref returnOrderID);

                Utils.Log(this, "Transfer order created: " + returnOrderID);

                // Send the ID back to the caller
                newOrderID = returnOrderID;
                return result;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                newOrderID = RecordIdentifier.Empty;
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
                ReturnConnection(dataModel, out dataModel);
            }
        }

        /// <summary>
        /// Creates a transfer order based on a given transfer request
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="requestIDtoCopy"></param>
        /// <param name="orderInformation"></param>
        /// <param name="newOrderID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CreateTransferOrderFromRequest(
            LogonInfo logonInfo,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,
            out RecordIdentifier newOrderID)
        {
            newOrderID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestIDtoCopy)}: {requestIDtoCopy}");
                return CreateTransferOrderFromRequest(dataModel, requestIDtoCopy, orderInformation, ref newOrderID);                
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                newOrderID = RecordIdentifier.Empty;
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
                ReturnConnection(dataModel, out dataModel);
            }
        }

        protected virtual CreateTransferOrderResult CreateTransferOrderFromRequest(
            IConnectionManager dataModel,
            InventoryTransferRequestContainer requestToCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID)
        {
            newOrderID = RecordIdentifier.Empty;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestToCopy)}.ID: {requestToCopy.InventoryTransferRequest.ID}");

                InventoryTransferRequest request = requestToCopy.InventoryTransferRequest;
                List<InventoryTransferRequestLine> requestLines = requestToCopy.InventoryTransferRequestLines;

                if (request == null)
                {
                    Utils.Log(this, "Transfer request not found");
                    return CreateTransferOrderResult.RequestNotFound;
                }

                Utils.Log(this, "Information on original transfer request retrieved");

                // If the order information that is sent in is valid then use that to create an order header
                if (orderInformation != null)
                {
                    Utils.Log(this, "Use new transfer order header information for header");
                    // Update the order header with the transfer request ID that it is connected to in case it hasn't been done
                    orderInformation.InventoryTransferRequestId = request.ID;
                    orderInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, orderInformation.ExpectedDelivery, orderInformation.SendingStoreId);
                    orderInformation.CreationDate = DateTime.Now;

                    orderInformation.ID = SaveInventoryTransferOrder(dataModel, orderInformation);
                }
                else
                {
                    Utils.Log(this, "Create an order header using the information from the request to be copied");
                    // Create an order header using the information from the request to be copied
                    orderInformation = CreateTransferOrderHeader(dataModel,
                                                                request.SendingStoreId,
                                                                request.ReceivingStoreId,
                                                                request.Text,
                                                                request.ExpectedDelivery,
                                                                RecordIdentifier.Empty,
                                                                request.ID);
                }

                // If the order information still hasn't been successfully created
                if (orderInformation == null)
                {
                    Utils.Log(this, "The new transfer order could not be created");
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, orderInformation))
                {
                    Utils.Log(this, "Header information not sufficient, order cannot be copied");
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }
                                
                // Send the ID back to the caller
                newOrderID = orderInformation.ID;
                Utils.Log(this, "New order ID: " + newOrderID);

                Utils.Log(this, "Start copying request lines");                
                Providers.InventoryTransferOrderLineData.CopyLinesFromRequest(dataModel, requestToCopy.InventoryTransferRequest.ID, newOrderID);
                Utils.Log(this, "Copying done");

                // Make sure that all the lines were copied and return information about how it went
                int orgNoOfLines = Providers.InventoryTransferRequestLineData.LineCount(dataModel, requestToCopy.InventoryTransferRequest.ID);
                int newOrderLines = Providers.InventoryTransferOrderLineData.LineCount(dataModel, newOrderID);
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

                TransferOrderCreatedFromTransferRequest(dataModel, request.ID, orderInformation.ID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
                ReturnConnection(dataModel, out dataModel);
            }

            return CreateTransferOrderResult.Success;
        }

        /// <summary>
        /// Creates transfer orders based on given transfer requests
        /// </summary>
        /// <param name="requestIDs"></param>
        /// <param name="createdBy"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CreateTransferOrdersFromRequests(List<RecordIdentifier> requestIDs, RecordIdentifier createdBy, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                List<InventoryTransferRequestContainer> requests = GetInventoryTransferRequestsAndLines(requestIDs, logonInfo);

                if (requests == null || requests.Count == 0)
                {
                    Utils.Log(this, "None of the requests found");
                    return CreateTransferOrderResult.RequestNotFound;
                }

                Utils.Log(this, "Requests found, start creating orders from the requests");
                foreach (InventoryTransferRequestContainer container in requests)
                {
                    Utils.Log(this, "Request: " + container.InventoryTransferRequest.ID);

                    InventoryTransferRequest request = container.InventoryTransferRequest;
                    List<InventoryTransferRequestLine> requestLines = container.InventoryTransferRequestLines;

                    Utils.Log(this, "Create new transfer order header from request information");

                    // Create an order header using the information from the request to be copied
                    InventoryTransferOrder transferOrder = CreateTransferOrderHeader(dataModel,                                                                
                                                                request.ReceivingStoreId,
                                                                request.SendingStoreId,
                                                                request.Text,
                                                                request.ExpectedDelivery,
                                                                createdBy,
                                                                request.ID);

                    Utils.Log(this, "Create the transfer order from the request");
                    RecordIdentifier newOrderID = RecordIdentifier.Empty;
                    CreateTransferOrderFromRequest(dataModel, request.ID, transferOrder, ref newOrderID);                    

                    Utils.Log(this, "Update the transfer request with transfer order information");
                    TransferOrderCreatedFromTransferRequest(dataModel, request.ID, transferOrder.ID);
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

        /// <summary>
        /// Detects whether a transfer order line has changed and if it did, updates it in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferOrderLine"></param>
        public virtual void SaveInventoryTransferOrderLineIfChanged(LogonInfo logonInfo, InventoryTransferOrderLine inventoryTransferOrderLine)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.InventoryTransferOrderLineData.SaveIfChanged(dataModel, inventoryTransferOrderLine);
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

        #region Receiving transfer orders

        /// <summary>
        /// Receives the given transfer orders
        /// </summary>
        /// <param name="transferIDs"></param>
        /// <param name="receivingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual ReceiveTransferOrderResult ReceiveTransferOrders(
            List<RecordIdentifier> transferIDs,
            DateTime receivingTime,
            LogonInfo logonInfo
            )
        {
            ReceiveTransferOrderResult currentReceivingResult = ReceiveTransferOrderResult.Success;
            ReceiveTransferOrderResult errorReceivingResult = ReceiveTransferOrderResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                
                foreach (RecordIdentifier ID in transferIDs)
                {
                    Utils.Log(this, "Receiving transfer order: " + ID);
                    currentReceivingResult = ReceiveTransferOrder(ID, receivingTime, logonInfo);
                    if (currentReceivingResult != ReceiveTransferOrderResult.Success && currentReceivingResult > errorReceivingResult)
                    {
                        Utils.Log(this, "Error receiving transfer order");
                        errorReceivingResult = currentReceivingResult;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {                
                Utils.Log(this, Utils.Done);
            }

            return errorReceivingResult;
        }

        /// <summary>
        /// Receives the transfer order with the given ID
        /// </summary>
        /// <param name="transferID"></param>
        /// <param name="receivingTime"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual ReceiveTransferOrderResult ReceiveTransferOrder(
            RecordIdentifier transferID,
            DateTime receivingTime,
            LogonInfo logonInfo
            )
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferID)}: {transferID}");

                // ONE-10284 This validation block must be replicated in the OMNI server to avoid reactivating this bug (search for ONE-10284)

                // Get information about the item lines for the order
                InventoryTransferContainer transferOrder = GetInventoryTransferOrderContainer(dataModel, transferID);
                if (transferOrder == null)
                {
                    Utils.Log(this, "Transfer order was not found");
                    return ReceiveTransferOrderResult.NotFound;
                }

                if (transferOrder.InventoryTransferOrder.ProcessingStatus != InventoryProcessingStatus.None)
                {
                    Utils.Log(this, "Transfer order is currently processing and cannot be received.");
                    return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
                }

                // If it has already been received - perhaps by another client/app then there is no need to
                // go any further and the next transfer order can be processed.
                if (transferOrder.InventoryTransferOrder.Received)
                {
                    Utils.Log(this, "Transfer order has already been received");
                    return ReceiveTransferOrderResult.Received;
                }

                // If there are no items on the transfer order, then it cannot be received
                if (!transferOrder.InventoryTransferLines.Any())
                {
                    Utils.Log(this, "No item lines on transfer order, cannot be received");
                    return ReceiveTransferOrderResult.NoItemsOnTransfer;
                }

                // If any of the items on the transfer order have a unit conversion error, the transfer order cannot be received
                if (!UnitConversionOnTransferOrder(dataModel, transferOrder.ID))
                {
                    Utils.Log(this, "Transfer order has unit conversion error on it, cannot be sent");
                    return ReceiveTransferOrderResult.UnitConversionError;
                }

                Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferOrder.InventoryTransferOrder.ID, InventoryProcessingStatus.Posting);

                //******************************************************

                // The transfer order is ready to be updated and set as received                

                //******************************************************

                Utils.Log(this, "Transfer order ready to be updated and received");

                // Update the inventory transfer order
                InventoryTransferOrder toReceive = transferOrder.InventoryTransferOrder;
                toReceive.Received = true;
                toReceive.ReceivingDate = receivingTime;
                Providers.InventoryTransferOrderData.Save(dataModel, toReceive);

                List<InventoryTransferOrderLine> linesToReceive = transferOrder.InventoryTransferLines;

                bool adjustmentNeeded = linesToReceive.Any(c => c.QuantityReceived != c.QuantitySent);
                RecordIdentifier adjustmentID = RecordIdentifier.Empty;
                RecordIdentifier reasonCodeID = RecordIdentifier.Empty;

                if(adjustmentNeeded)
                {
                    Utils.Log(this, "Some lines don't have the same received and sent value - Inv Adjustment needs to be created");
                    Utils.Log(this, "Get reason code and inventory adjustment information");

                    List<ReasonCode> reasonCodes = GetReasonCodes(logonInfo, InventoryJournalTypeEnum.Transfer);
                    DataEntity reasonCode = reasonCodes.FirstOrDefault(f => f.ID == MissingInTransferReasonConstants.ConstMissingInTransferReasonID) ?? new DataEntity(MissingInTransferReasonConstants.ConstMissingInTransferReasonID, MissingInTransferReasonConstants.ConstMissingInTransferReasonID);

                    if(reasonCode != null)
                    {
                        reasonCodeID = reasonCode.ID;
                    }

                    InventoryAdjustment journal = GetInventoryAdjustment(logonInfo, transferOrder.InventoryTransferOrder.ID);

                    if (journal == null)
                    {
                        Utils.Log(this, "No inventory adjustment exists");
                        journal = new InventoryAdjustment();
                        journal.MasterID = Guid.NewGuid();
                        journal.Text = (string)transferOrder.InventoryTransferOrder.ID;
                        journal.JournalType = InventoryJournalTypeEnum.Adjustment;
                        journal.CreatedDateTime = DateTime.Now;
                        journal.StoreId = transferOrder.InventoryTransferOrder.ReceivingStoreId;
                        journal = SaveInventoryAdjustment(logonInfo, journal);
                        Utils.Log(this, "Inventory adjustment created");
                    }

                    adjustmentID = journal.ID;
                }

                Utils.Log(this, "Calculating received item costs");
                Providers.InventoryTransferOrderLineData.CalculateReceivingItemCosts(dataModel, transferOrder.InventoryTransferOrder.ID);

                Utils.Log(this, "Inventory movements created for all of the items on the transfer order");
                bool updateInventorySuccess = Providers.InventoryTransferOrderData.UpdateInventoryForTransferOrder(dataModel, transferOrder.InventoryTransferOrder.ID, TransferOrderUpdateInventoryAction.Receive, adjustmentNeeded, reasonCodeID, adjustmentID);

                Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferOrder.InventoryTransferOrder.ID, InventoryProcessingStatus.None);

                if (!updateInventorySuccess)
                {
                    Utils.Log(this, "Failed to update inventory for transfer order " + transferOrder.InventoryTransferOrder.ID.StringValue, LogLevel.Error);
                    return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
                }
            }
            catch (DataIntegrityException dex)
            {
                Utils.LogException(this, "Unit conversion error in UpdateInventoryWhenReceivingTransferOrder", dex);

                if (dataModel != null)
                {
                    Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferID, InventoryProcessingStatus.None);
                }

                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                if (dataModel != null)
                {
                    Providers.InventoryTransferOrderData.SetTransferOrderProcessingStatus(dataModel, transferID, InventoryProcessingStatus.None);
                }

                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
            return ReceiveTransferOrderResult.Success;
        }

        /// <summary>
        /// Automatically set the quantity on the transfer order lines
        /// </summary>
        /// <param name="transferID"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual AutoSetQuantityResult AutoSetQuantityOnTransferOrder(RecordIdentifier transferID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferID)}: {transferID}");

                // Get information about the item lines for the order
                InventoryTransferOrder transferOrder = Providers.InventoryTransferOrderData.Get(dataModel, transferID);
                if (transferOrder == null)
                {
                    Utils.Log(this, "Transfer order not found");
                    return AutoSetQuantityResult.NotFound;
                }

                if (transferOrder.Received)
                {
                    Utils.Log(this, "Transfer order already received");
                    return AutoSetQuantityResult.AlreadyReceived;
                }

                Providers.InventoryTransferOrderLineData.AutoSetQuantityOnTransferOrderLines(dataModel, transferOrder.ID);

                Utils.Log(this, "Transfer order update done");
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return AutoSetQuantityResult.ErrorAutoSettingQuantity;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return AutoSetQuantityResult.Success;
        }

        /// <summary>
        /// Checks whether the quantities are correct in a given list of transfer orders by comparing the sent and the received quantities
        /// </summary>
        /// <param name="transferIDs"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(
            List<RecordIdentifier> transferIDs,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                foreach (RecordIdentifier ID in transferIDs)
                {
                    Utils.Log(this, "Check transfer order: " + ID);
                    List<InventoryTransferOrderLine> transferItems = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(dataModel, ID, InventoryTransferOrderLineSortEnum.ItemName, false);
                    
                    if (transferItems != null && transferItems.Count(c => c.QuantitySent != c.QuantityReceived) > 0)
                    {
                        Utils.Log(this, "Quantity sent and quantity received not the same");
                        return ReceiveTransferOrderResult.QuantitiesReceivedNotAccurate;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
            return ReceiveTransferOrderResult.Success;
        }

        #endregion

        protected virtual bool UnitConversionOnTransferOrder(IConnectionManager entry, RecordIdentifier transferOrderID)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                List<string> errors = Providers.InventoryTransferOrderData.CheckUnitConversionsForTransferOrder(entry, transferOrderID);

                if(errors.Any())
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

        /// <summary>
        /// Deletes a given list of transfer orders
        /// </summary>
        /// <param name="transferIDs"></param>
        /// <param name="reject"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual DeleteTransferResult DeleteInventoryTransferOrders(
            List<RecordIdentifier> transferIDs, bool reject,
            LogonInfo logonInfo)
        {
            DeleteTransferResult currentDeleteResult = DeleteTransferResult.Success;
            DeleteTransferResult errorDeleteResult = DeleteTransferResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                foreach (RecordIdentifier ID in transferIDs)
                {                       
                    currentDeleteResult = DeleteInventoryTransferOrder(ID, reject, logonInfo);
                    if (currentDeleteResult != DeleteTransferResult.Success && currentDeleteResult > errorDeleteResult)
                    {
                        Utils.Log(this, "Error deleting transfer order: " + (string)ID);
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
        /// Deletes the transfer order with the given ID
        /// </summary>
        /// <param name="inventoryTransferOrderId"></param>
        /// <param name="reject"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual DeleteTransferResult DeleteInventoryTransferOrder(RecordIdentifier inventoryTransferOrderId, bool reject, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTransferOrderId)}: {inventoryTransferOrderId}");
                InventoryTransferOrder transferOrder = Providers.InventoryTransferOrderData.Get(dataModel, inventoryTransferOrderId);

                if (transferOrder == null)
                {
                    Utils.Log(this, "Transfer order not found");
                    return DeleteTransferResult.NotFound;
                }

                if (transferOrder.Rejected)
                {
                    Utils.Log(this, "Transfer order already rejected");
                    return DeleteTransferResult.Success;
                }

                if (transferOrder.Received)
                {
                    Utils.Log(this, "Transfer order already received, cannot be rejected");
                    return DeleteTransferResult.Received;
                }

                if (reject)
                {
                    Utils.Log(this, "Transfer order to be rejected not deleted");
                    
                    transferOrder.Rejected = true;
                    Providers.InventoryTransferOrderData.Save(dataModel, transferOrder);

                    bool updateInventorySuccess = Providers.InventoryTransferOrderData.UpdateInventoryForTransferOrder(dataModel, transferOrder.ID, TransferOrderUpdateInventoryAction.Reject, false, RecordIdentifier.Empty, RecordIdentifier.Empty);

                    if (!updateInventorySuccess)
                    {
                        Utils.Log(this, "Failed to revert inventory for transfer order " + inventoryTransferOrderId.StringValue, LogLevel.Error);
                        return DeleteTransferResult.ErrorDeletingTransfer;
                    }
                }
                else
                {
                    if (transferOrder.Sent)
                    {
                        Utils.Log(this, "Transfer order already sent, cannot be deleted");
                        return DeleteTransferResult.Sent;
                    }

                    Providers.InventoryTransferOrderData.Delete(dataModel, inventoryTransferOrderId);
                    Utils.Log(this, "Transfer order deleted");

                    // If the inventory transfer order was created from a request, we need to update this request so that the request is not still linked to the transfer order
                    InventoryTransferRequest transferRequest = Providers.InventoryTransferRequestData.Get(dataModel, transferOrder.InventoryTransferRequestId);
                    if (transferRequest != null)
                    {
                        Utils.Log(this, "Update connected transfer request to not be connected to a deleted transfer order");

                        transferRequest.InventoryTransferOrderCreated = false;
                        transferRequest.InventoryTransferOrderId = "";
                        Providers.InventoryTransferRequestData.Save(dataModel, transferRequest);
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
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }

            return DeleteTransferResult.Success;
        }

        /// <summary>
        /// Deletes the transfer order line with the given ID
        /// </summary>
        /// <param name="inventoryTransferOrderLineId"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(RecordIdentifier inventoryTransferOrderLineId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTransferOrderLineId)}: {inventoryTransferOrderLineId}");
                InventoryTransferOrderLine orderLine = Providers.InventoryTransferOrderLineData.Get(dataModel, inventoryTransferOrderLineId);

                if (orderLine == null)
                {
                    Utils.Log(this, "Order line not found");
                    return SaveTransferOrderLineResult.NotFound;
                }

                InventoryTransferOrder order = Providers.InventoryTransferOrderData.Get(dataModel, orderLine.InventoryTransferId);

                if (order != null && order.Sent)
                {
                    Utils.Log(this, "Transfer order already sent, order line cannot be deleted");
                    return SaveTransferOrderLineResult.TransferOrderAlreadySent;
                }

                if (order != null && order.Rejected)
                {
                    Utils.Log(this, "Transfer order already rejected, order line cannot be deleted");
                    return SaveTransferOrderLineResult.ErrorSavingTransferOrderLine;
                }

                Providers.InventoryTransferOrderLineData.Delete(dataModel, inventoryTransferOrderLineId);
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
        /// Deletes the given transfer order lines
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferOrderLineIds"></param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(LogonInfo logonInfo, List<RecordIdentifier> inventoryTransferOrderLineIds)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;

            try
            {
                Utils.Log(this, Utils.Starting);
                InventoryTransferOrder order = null;

                foreach (RecordIdentifier inventoryTransferOrderLineId in inventoryTransferOrderLineIds)
                {
                    InventoryTransferOrderLine orderLine = Providers.InventoryTransferOrderLineData.Get(dataModel, inventoryTransferOrderLineId);

                    if (orderLine == null)
                    {
                        Utils.Log(this, "Order line not found: " + (string)inventoryTransferOrderLineId);                            
                        result = SaveTransferOrderLineResult.NotFound;
                        continue;
                    }

                    if (order == null)
                    {
                        Utils.Log(this, "Get transfer order header information");
                        order = Providers.InventoryTransferOrderData.Get(dataModel, orderLine.InventoryTransferId);
                    }

                    if (order != null && order.Sent)
                    {
                        Utils.Log(this, "Order already sent, order line cannot be deleted");
                        result = SaveTransferOrderLineResult.TransferOrderAlreadySent;
                        break;
                    }


                    if (order != null && order.Rejected)
                    {
                        Utils.Log(this, "Order already rejected, order line cannot be deleted");
                        result = SaveTransferOrderLineResult.TransferOrderAlreadyRejected;
                        break;
                    }

                    Providers.InventoryTransferOrderLineData.Delete(dataModel, inventoryTransferOrderLineId);
                    Utils.Log(this, "Order line deleted: " + (string)inventoryTransferOrderLineId);
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
        /// Gets a list of transfer orders together with their lines, based on a given list of identifiers of documents to fetch
        /// </summary>
        /// <param name="listOfTransferIDsToFetch"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<InventoryTransferContainer> GetInventoryTransferOrdersAndLines(List<RecordIdentifier> listOfTransferIDsToFetch, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return GetInventoryTransferOrdersAndLines(dataModel, listOfTransferIDsToFetch);
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

            return null;
        }

        protected virtual List<InventoryTransferContainer> GetInventoryTransferOrdersAndLines(IConnectionManager dataModel, List<RecordIdentifier> listOfTransferIDsToFetch)
        {
            List<InventoryTransferContainer> result = new List<InventoryTransferContainer>();
            try
            {
                Utils.Log(this, Utils.Starting);

                List<InventoryTransferOrder> inventoryTransfers = Providers.InventoryTransferOrderData.GetFromList(dataModel, listOfTransferIDsToFetch, InventoryTransferOrderSortEnum.Id, false);

                Utils.Log(this, "Transfer order information retrieved");

                foreach (InventoryTransferOrder inventoryTransferOrder in inventoryTransfers)
                {
                    InventoryTransferContainer container = new InventoryTransferContainer
                    {
                        InventoryTransferOrder = inventoryTransferOrder,
                        InventoryTransferLines = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(
                            dataModel,
                            inventoryTransferOrder.ID,
                            InventoryTransferOrderLineSortEnum.ItemName,
                            false)
                    };
                    result.Add(container);
                }

                Utils.Log(this, "Transfer order container information retrieved");
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
            return result;
        }

        /// <summary>
        /// Gets the list of transfer orders that are in transit
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="sorting"></param>
        /// <param name="sortAscending"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrder> GetInventoryInTransit(LogonInfo logonInfo, InventoryTransferOrderSortEnum sorting, bool sortAscending, RecordIdentifier storeId)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                if (storeId == null)
                {
                    Utils.Log(this, "Retrieve inventory in transit for all stores");
                }
                else
                {
                    Utils.Log(this, "Retrieve inventory in transit for storeID: " + (string)storeId);
                }
                return Providers.InventoryTransferOrderData.GetInventoryInTransit(dataModel, sorting, sortAscending, storeId);
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
        /// Gets a list with quantity for each item that is not [fully] received in the given list of transfer orders
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferOrderIds"></param>
        /// <returns></returns>
        public virtual Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(LogonInfo logonInfo, List<RecordIdentifier> transferOrderIds)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferOrderData.GetTotalUnreceivedItemForTransferOrders(dataModel, transferOrderIds);
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
        /// Updates a transfer request as being created from a given transfer order
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="orderId"></param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual bool TransferOrderCreatedFromTransferRequest(
            RecordIdentifier requestId,
            RecordIdentifier orderId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return TransferOrderCreatedFromTransferRequest(dataModel, requestId, orderId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return false;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual bool TransferOrderCreatedFromTransferRequest(
            IConnectionManager dataModel,
            RecordIdentifier requestID,
            RecordIdentifier orderID)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(requestID)}: {requestID}, {nameof(orderID)}: {orderID}");
                InventoryTransferRequest transferRequest = Providers.InventoryTransferRequestData.Get(dataModel, requestID);

                if (transferRequest == null)
                {
                    Utils.Log(this, "Transfer request not found");
                    return false;
                }

                // An inventory transfer was already created.
                if (transferRequest.InventoryTransferOrderCreated)
                {
                    Utils.Log(this, "Transfer request already has an order attached to it");
                    return false;
                }

                Utils.Log(this, "Transfer request found");
                transferRequest.InventoryTransferOrderCreated = true;
                transferRequest.InventoryTransferOrderId = orderID;
                Providers.InventoryTransferRequestData.Save(dataModel, transferRequest);

                Utils.Log(this, "Transfer request updated with transfer order ID");
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
            return true;
        }       

        /// <summary>
        /// Returns information about a specific inventory transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="ID">The unique ID Of the transfer order</param>
        /// <returns>Information about a specific inventory transfer order</returns>
        public virtual InventoryTransferOrder GetInventoryTransferOrder(LogonInfo logonInfo, RecordIdentifier ID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(ID)}: {ID}");
                return Providers.InventoryTransferOrderData.Get(dataModel, ID);
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
        /// Tries to find and return a transfer order line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line">The transfer order line you want to get</param>
        /// <returns>A InventoryTransferOrderLine object, null if no matching line was found</returns>
        public virtual InventoryTransferOrderLine GetTransferOrderLine(LogonInfo logonInfo, InventoryTransferOrderLine line)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(line)}: {line}");
                return Providers.InventoryTransferOrderLineData.GetLine(dataModel, line);
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
        /// Returns order lines for a specific inventory transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="ID">The unique ID of the inventory transfer order</param>
        /// <param name="sortBy">How to sort the result list</param>
        /// <param name="sortBackwards">If true then the list is sorted backwards</param>
        /// <param name="getUnsentItemsOnly">If true then only order lines that have not been sent are returned</param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransfer(
            LogonInfo logonInfo,
            RecordIdentifier ID,
            InventoryTransferOrderLineSortEnum sortBy,
            bool sortBackwards,
            bool getUnsentItemsOnly)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            List<InventoryTransferOrderLine> inventoryTransferLines = new List<InventoryTransferOrderLine>();

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(ID)}: {ID}");
                inventoryTransferLines = Providers.InventoryTransferOrderLineData.GetListForInventoryTransfer(dataModel,
                    ID,
                    sortBy,
                    sortBackwards,
                    getUnsentItemsOnly);

                return inventoryTransferLines;
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
        /// Get a list of lines for a given transfer order ID, matching a given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="transferOrderID"></param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(
            LogonInfo logonInfo,
            RecordIdentifier transferOrderID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transferOrderID)}: {transferOrderID}");
                return Providers.InventoryTransferOrderLineData.GetOrderLinesForInventoryTransferAdvanced(dataModel, transferOrderID, out totalRecordsMatching, filter);
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
        /// Saves the given transfer order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferOrderLine"></param>
        /// <param name="isReceiving"></param>
        /// <param name="newLineID"></param>
        /// <returns></returns>
        public virtual SaveTransferOrderLineResult SaveInventoryTransferOrderLine(LogonInfo logonInfo, InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            newLineID = RecordIdentifier.Empty;

            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveInventoryTransferOrderLine(dataModel, inventoryTransferOrderLine, isReceiving, ref newLineID);
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

        protected virtual SaveTransferOrderLineResult SaveInventoryTransferOrderLine(IConnectionManager dataModel, InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, ref RecordIdentifier newLineID)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTransferOrderLine)}.ID: {inventoryTransferOrderLine.ID}");
                InventoryTransferOrder order = Providers.InventoryTransferOrderData.Get(dataModel, inventoryTransferOrderLine.InventoryTransferId);

                if (order == null)
                {
                    Utils.Log(this, "Transfer order for the line was not found - ID: " + (string)inventoryTransferOrderLine.InventoryTransferId);
                    return SaveTransferOrderLineResult.NotFound;
                }

                if (isReceiving && order.Received)
                {
                    Utils.Log(this, "Transfer order already received. A new transfer order line cannot be added");
                    return SaveTransferOrderLineResult.TransferOrderAlreadyReceived;
                }

                if (!isReceiving && order.Sent)
                {
                    Utils.Log(this, "Transfer order already sent. A new transfer order line cannot be added");
                    return SaveTransferOrderLineResult.TransferOrderAlreadySent;
                }

                if (order.Rejected || (isReceiving && !order.Sent))
                {
                    Utils.Log(this, "Transfer order already rejected. A new transfer order line cannot be added");
                    return SaveTransferOrderLineResult.TransferOrderAlreadyRejected;
                }

                Providers.InventoryTransferOrderLineData.Save(dataModel, inventoryTransferOrderLine);
                newLineID = inventoryTransferOrderLine.ID;
                Utils.Log(this, "Transfer order line added");
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
        /// Saves the given transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTransferOrder"></param>
        /// <returns></returns>
        public virtual RecordIdentifier SaveInventoryTransferOrder(LogonInfo logonInfo, InventoryTransferOrder inventoryTransferOrder)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return SaveInventoryTransferOrder(dataModel, inventoryTransferOrder);
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

        protected virtual RecordIdentifier SaveInventoryTransferOrder(IConnectionManager entry, InventoryTransferOrder inventoryTransferOrder)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.InventoryTransferOrderData.Save(entry, inventoryTransferOrder);

                Utils.Log(this, "Transfer order saved - ID:" + inventoryTransferOrder.ID);
                return inventoryTransferOrder.ID;                
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of transfer orders matching the given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrders(LogonInfo logonInfo, InventoryTransferFilter filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferOrderData.Search(dataModel, filter);
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
        /// Returns a list of transfer orders matching the given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersExtended(LogonInfo logonInfo, InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferOrderData.Search(dataModel, filter);
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
        /// Returns a list of transfer orders matching the given filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersAdvanced(
            LogonInfo logonInfo,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferOrderData.AdvancedSearch(dataModel, out totalRecordsMatching, filter);
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
        /// Copies or clones an existing transfer order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderIDtoCopy"></param>
        /// <param name="orderInformation"></param>
        /// <param name="newOrderID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CopyTransferOrder(
            LogonInfo logonInfo,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferOrder orderInformation,
            out RecordIdentifier newOrderID)
        {
            newOrderID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(orderIDtoCopy)}: {orderIDtoCopy}", LogLevel.Trace);
                // Get information about the transfer order that is to be copied
                InventoryTransferContainer orgOrderContainer = GetInventoryTransferOrderContainer(dataModel, orderIDtoCopy);
                InventoryTransferOrder orgOrder = orgOrderContainer.InventoryTransferOrder;
                               
                if (orgOrderContainer == null || orgOrder == null)
                {
                    Utils.Log(this, "Order to copy not found");
                    return CreateTransferOrderResult.OrderNotFound;
                }

                Utils.Log(this, "Information on original transfer order retrieved");

                // If the order information that is sent in is valid then use that to create an order header
                if (orderInformation != null)
                {
                    Utils.Log(this, "Use new transfer order header information for header");
                    orderInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, orderInformation.ExpectedDelivery, orderInformation.SendingStoreId);
                    orderInformation.CreationDate = DateTime.Now;
                    orderInformation.ID = SaveInventoryTransferOrder(dataModel, orderInformation);
                }
                else
                {
                    // Create an order header using the information from the order to be copied
                    Utils.Log(this, "Create an order header using the information from the order to be copied");
                    orderInformation = CreateTransferOrderHeader(dataModel,
                                                                orgOrder.SendingStoreId,
                                                                orgOrder.ReceivingStoreId,
                                                                orgOrder.Text,
                                                                orgOrder.ExpectedDelivery,
                                                                RecordIdentifier.Empty,
                                                                RecordIdentifier.Empty);
                }

                // If the order still hasn't been successfully created
                if (orderInformation == null)
                {
                    Utils.Log(this, "The new transfer order could not be created");
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, orderInformation))
                {
                    Utils.Log(this, "Header information not sufficient, order cannot be copied");
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }

                // Send the ID back to the caller
                newOrderID = orderInformation.ID;                                

                Utils.Log(this, "Start copying order lines");
                Providers.InventoryTransferOrderLineData.CopyLines(dataModel, orderIDtoCopy, newOrderID);
                Utils.Log(this, "Copying done");

                // Make sure that all the lines were copied and return information about how it went
                int orgNoOfLines = Providers.InventoryTransferOrderLineData.LineCount(dataModel, orderIDtoCopy);
                int newOrderLines = Providers.InventoryTransferOrderLineData.LineCount(dataModel, newOrderID);
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
        /// Creates a new transfer order based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="orderInformation"></param>
        /// <param name="filter"></param>
        /// <param name="newOrderID"></param>
        /// <returns></returns>
        public virtual CreateTransferOrderResult CreateTransferOrderFromFilter(
            LogonInfo logonInfo,
            InventoryTransferOrder orderInformation,
            InventoryTemplateFilterContainer filter,
            out RecordIdentifier newOrderID)
        {
            Utils.Log(this, Utils.Starting);

            newOrderID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {                
                if (orderInformation != null && RecordIdentifier.IsEmptyOrNull(orderInformation.ID))                
                {
                    orderInformation.ExpectedDelivery = ExpectedDeliveryDate(dataModel, orderInformation.ExpectedDelivery, orderInformation.SendingStoreId);
                    orderInformation.CreationDate = DateTime.Now;
                    orderInformation.ID = SaveInventoryTransferOrder(dataModel, orderInformation);

                    Utils.Log(this, "New order created with ID: " + (string)orderInformation.ID);
                }

                if (orderInformation == null)
                {
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }

                if (!CheckTransferHeader(dataModel, orderInformation))
                {
                    return CreateTransferOrderResult.HeaderInformationInsufficient;
                }

                // Send the ID back to the caller
                newOrderID = orderInformation.ID;
                
                if(filter.HasFilterCriteria())
                {
                    Utils.Log(this, "Saving lines starting");
                    int savedLinesCount = Providers.InventoryTransferData.CreateStoreTransferLinesFromFilter(dataModel, orderInformation.ID, orderInformation.SendingStoreId, filter, StoreTransferTypeEnum.Order);
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

        public virtual CreateTransferOrderResult CreateTransferOrderFromTemplate(LogonInfo logonInfo, InventoryTransferOrder orderInformation, TemplateListItem template, out RecordIdentifier newOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Creating transfer order from template: " + template?.TemplateID);

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

                CreateTransferOrderResult result = CreateTransferOrderFromFilter(logonInfo, orderInformation, filter, out newOrderID);
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

        protected virtual InventoryTransferOrder CreateTransferOrderHeader(
                    IConnectionManager entry, 
                    RecordIdentifier sendingStoreID, 
                    RecordIdentifier receivingStoreID,                                                                               
                    string description, 
                    DateTime expectedDelivery, 
                    RecordIdentifier createdBy, 
                    RecordIdentifier createdFromRequestID)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(sendingStoreID)}: {sendingStoreID}, {nameof(receivingStoreID)}: {receivingStoreID}, {nameof(description)}: {description}, {nameof(createdFromRequestID)}: {createdFromRequestID}");
                InventoryTransferOrder newOrder = new InventoryTransferOrder();

                newOrder.CreationDate = DateTime.Now;
                newOrder.InventoryTransferRequestId = createdFromRequestID;
                newOrder.SendingStoreId = sendingStoreID;
                newOrder.ReceivingStoreId = receivingStoreID;
                newOrder.Text = description;
                newOrder.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                newOrder.CreatedBy = createdBy;

                newOrder.ID = SaveInventoryTransferOrder(entry, newOrder);

                Utils.Log(this, "New transfer order header created: " + newOrder.ID);

                return newOrder;
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

        protected virtual DateTime ExpectedDeliveryDate(IConnectionManager entry, DateTime expectedDelivery, RecordIdentifier storeID)
        {
            try
            {
                Utils.Log(this, Utils.Starting);

                DateTime deliveryDate = DateTime.Now.AddDays(3);

                // If the expected date is null or has been set to the MinValue then retrieve the expected delivery date for the store and use that
                if (expectedDelivery == null || expectedDelivery == DateTime.MinValue || expectedDelivery.Date < DateTime.Now.Date)
                {                    
                    // Get the expected delivery date for the store
                    if (!RecordIdentifier.IsEmptyOrNull(storeID))
                    {
                        Store store = Providers.StoreData.Get(entry, storeID, CacheType.CacheTypeNone, UsageIntentEnum.Minimal);
                        return store.StoreTransferExpectedDeliveryDate();
                    }
                }
                else
                {
                    return expectedDelivery;
                }

                return deliveryDate;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        protected virtual bool CheckTransferHeader(IConnectionManager entry, InventoryTransferOrder order)
        {
            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);
                if (order == null)
                {
                    Utils.Log(this, "Order to be checked is null", LogLevel.Trace);
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

                if (RecordIdentifier.IsEmptyOrNull(order.SendingStoreId) || storeList.FirstOrDefault(f => f.ID == order.SendingStoreId) == null)
                {
                    Utils.Log(this, "No sending store ID on order header", LogLevel.Trace);
                    return false;
                }

                if (RecordIdentifier.IsEmptyOrNull(order.ReceivingStoreId) || storeList.FirstOrDefault(f => f.ID == order.ReceivingStoreId) == null)
                {
                    Utils.Log(this, "No receiving store ID on order header", LogLevel.Trace);
                    return false;
                }                
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);                
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return true;
        }

        public virtual int ImportTransferOrderLinesFromXML(LogonInfo logonInfo, RecordIdentifier transferOrderID, string xmlData)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.InventoryTransferOrderLineData.ImportTransferOrderLinesFromXML(entry, transferOrderID, xmlData);
            }
            catch(Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }
    }
}