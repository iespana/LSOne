using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory
{
    internal partial class PluginOperations
    {
        protected static RecordIdentifier CreatedBy()
        {
            return !PluginEntry.DataModel.IsHeadOffice ? PluginEntry.DataModel.CurrentStoreID : "";
        }

        #region Events
        public static void ShowInventoryTransferOrderWizard(object sender, EventArgs e)
        {
            if (TestSiteService())
            {
                StoreTransferWizard dlg = new StoreTransferWizard(PluginEntry.DataModel, StoreTransferTypeEnum.Order);
                dlg.ShowDialog();
            }
        }

        public static void ShowInventoryTransferRequestWizard(object sender, EventArgs e)
        {
            if (TestSiteService())
            {
                StoreTransferWizard dlg = new StoreTransferWizard(PluginEntry.DataModel, StoreTransferTypeEnum.Request);
                dlg.ShowDialog();
            }
        }

        internal static void ShowStoreTransfersView(StoreTransferTypeEnum transferType)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.StoreTransfersView(transferType, RecordIdentifier.Empty));
            }
        }

        internal static void ShowStoreTransfersView(StoreTransferTypeEnum transferType, RecordIdentifier storeTransferID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StoreTransfersView(transferType, storeTransferID));
        }

        public static void ShowInventoryTransferOrderItemsView(InventoryTransferOrder transferOrder, InventoryTransferType inventoryTransferType)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.StoreTransferView(transferOrder, inventoryTransferType));
            }
        }

        public static void ShowInventoryTransferRequestsItemsView(InventoryTransferRequest transferRequest, InventoryTransferType inventoryTransferType)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.StoreTransferView(transferRequest, inventoryTransferType));
            }
        }

        public static void ShowStoreTransfersOrdersView(object sender, EventArgs args)
        {
            ShowStoreTransfersView(StoreTransferTypeEnum.Order);
        }

        public static void ShowStoreTransfersRequestsView(object sender, EventArgs args)
        {
            ShowStoreTransfersView(StoreTransferTypeEnum.Request);
        }

        public static void ShowTransferOrderView(RecordIdentifier transferOrderID, InventoryTransferType type = InventoryTransferType.Outgoing)
        {
            InventoryTransferOrder transferOrder = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTransferOrder(PluginEntry.DataModel, GetSiteServiceProfile(), transferOrderID, true);

            if (transferOrder != null)
            {
                if (PluginEntry.Framework.ViewController.CurrentView.GetType() == typeof(Views.StoreTransferView))
                {
                    PluginEntry.Framework.ViewController.ReplaceView(PluginEntry.Framework.ViewController.CurrentView, new Views.StoreTransferView(transferOrder, type));
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new Views.StoreTransferView(transferOrder, type));
                }
            }
        }

        #endregion


        #region Error messages
        /// <summary>
        /// Shows a message dialog with the corresponing error message when saving an inventory transfer line
        /// </summary>
        /// <param name="result">The save result</param>
        /// <param name="isTransferOrder">True if the result is for a transfer order line. False if it is for a transfer request line</param>
        internal static void ShowInventoryTransferLineErrorResultMessage(SaveTransferOrderLineResult result, bool isTransferOrder)
        {
            switch (result)
            {
                case SaveTransferOrderLineResult.NotFound:
                    MessageDialog.Show(isTransferOrder ? Resources.TransferOrderOrLineNotFound : Resources.TransferRequestOrLineNotFound);
                    break;
                case SaveTransferOrderLineResult.TransferOrderAlreadySent:
                    MessageDialog.Show(isTransferOrder ? Resources.TransferOrderHasAlreadyBeenSent : Resources.TransferRequestFetchedByStore);
                    break;
                case SaveTransferOrderLineResult.ErrorSavingTransferOrderLine:
                    MessageDialog.Show(isTransferOrder ? Resources.UnableToSaveTransferOrderLine : Resources.UnableToSaveTransferRequestLine);
                    break;
                case SaveTransferOrderLineResult.TransferOrderAlreadyReceived:
                    MessageDialog.Show(isTransferOrder ? Resources.TransferOrderHasAlreadyBeenSent : Resources.TransferOrderAlreadyCreated);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Shows a message dialog with the corresponing error message when sending an inventory transfer
        /// </summary>
        /// <param name="result">The sending result</param>
        /// <param name="isTransferOrder">True if the result is for a transfer order. False if it is for a transfer request</param>
        internal static void SendInventoryTransferErrorResultMessage(SendTransferOrderResult result, bool isTransferOrder)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case SendTransferOrderResult.ErrorSendingTransferOrder:
                    if (isTransferOrder)
                    {
                        MessageDialog.Show(Resources.UnableToSendTransferOrder + CRLF + Resources.TransferOrderRequestMayAlreadyBeSentOrContainNoItems, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageDialog.Show(Resources.UnableToSendTransferRequest, MessageBoxIcon.Error);
                    }
                    break;
                case SendTransferOrderResult.LinesHaveZeroSentQuantity:
                    if (isTransferOrder)
                    {
                        MessageDialog.Show(Resources.UnableToSendTransferOrder + CRLF + Resources.OneOrMoreLinesHaveSentQuantityAsZero);
                    }
                    break;
                case SendTransferOrderResult.FetchedByReceivingStore:
                    MessageDialog.Show(isTransferOrder ? Resources.TransferOrderHasBeenOpenedByReceivingStoreAndCannotBeSentAgain : Resources.TransferRequestHasBeenOpenedByReceivingStoreAndCannotBeSentAgain);
                    break;
                case SendTransferOrderResult.NotFound:
                    MessageDialog.Show(isTransferOrder ? Resources.TransferOrderNotFound : Resources.TransferRequestNotFound);
                    break;
                case SendTransferOrderResult.TransferAlreadySent:
                    MessageDialog.Show(isTransferOrder ? Resources.UnableToSendTransferOrder + CRLF + Resources.TransferOrderHasAlreadyBeenSent : Resources.UnableToSendRequest + CRLF + Resources.TransferRequestHasAlreadyBeenSent);
                    break;
                case SendTransferOrderResult.NoItemsOnTransfer:
                    MessageDialog.Show(isTransferOrder ? Resources.UnableToSendTransferOrder + CRLF + Resources.TransferOrderHasNoItemLines : Resources.UnableToSendTransferOrder + CRLF + Resources.TransferRequestHasNoItemLines);
                    break;
                case SendTransferOrderResult.UnitConversionError:
                    MessageDialog.Show(Resources.UnableToSendTransferOrder + CRLF + Resources.MissinUnitConversionBetweenUnits);
                    break;
                case SendTransferOrderResult.TransferOrderIsRejected:
                    MessageDialog.Show(isTransferOrder ? Resources.UnableToSendTransferOrder + CRLF + Resources.RejectedTransferOrderCannotBeSent : Resources.UnableToSendTransferOrder + CRLF + Resources.RejectedTransferRequestCannotBeSent);
                    break;
            }
        }

        internal static void ReceiveTransferOrderErrorResultMessage(ReceiveTransferOrderResult result)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case ReceiveTransferOrderResult.ErrorReceivingTransferOrder:
                    MessageDialog.Show(Resources.UnabletoReceiveTransferOrder + CRLF + Resources.ErrorReceivingInventoryTransferOrder, MessageBoxIcon.Error);
                    break;
                case ReceiveTransferOrderResult.Received:
                    MessageDialog.Show(Resources.UnabletoReceiveTransferOrder + CRLF + Resources.TransferOrderAlreadyReceived, MessageBoxIcon.Error);
                    break;
                case ReceiveTransferOrderResult.NotFound:
                    MessageDialog.Show(Resources.UnabletoReceiveTransferOrder + CRLF + Resources.TransferOrderNotFound);
                    break;
                case ReceiveTransferOrderResult.QuantitiesReceivedNotAccurate:
                    //This error is handled in the ReceiveTransferOrder function
                    break;
                case ReceiveTransferOrderResult.UnitConversionError:
                    MessageDialog.Show(Resources.UnabletoReceiveTransferOrder + CRLF + Resources.MissinUnitConversionBetweenUnits);
                    break;
                case ReceiveTransferOrderResult.NoItemsOnTransfer:
                    MessageDialog.Show(Resources.UnabletoReceiveTransferOrder + CRLF + Resources.TransferOrderHasNoItemLines);
                    break;
            }
        }

        internal static void CreateTransferOrderMessage(CreateTransferOrderResult result, bool createTransferOrder)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case CreateTransferOrderResult.OrderNotFound:
                    MessageDialog.Show(createTransferOrder ? Resources.UnableToCreateTransferOrder : Resources.UnableToCreateTransferRequest
                                       + CRLF + Resources.TransferOrderToBeCopiedCouldNotBeFound);
                    break;

                case CreateTransferOrderResult.RequestNotFound:
                    MessageDialog.Show(createTransferOrder ? Resources.UnableToCreateTransferOrder : Resources.UnableToCreateTransferRequest
                                       + CRLF + Resources.TransferRequestToBeCopiedCouldNotBeFound);
                    break;
                case CreateTransferOrderResult.HeaderInformationInsufficient:
                    MessageDialog.Show(createTransferOrder ? Resources.UnableToCreateTransferOrder : Resources.UnableToCreateTransferRequest
                                       + CRLF + Resources.InformationSendingReceivingStoreMissing);
                    break;
                case CreateTransferOrderResult.ErrorCreatingTransferOrder:
                    MessageDialog.Show(createTransferOrder ? Resources.UnableToCreateTransferOrder : Resources.UnableToCreateTransferRequest);
                    break;
                case CreateTransferOrderResult.TemplateNotFound:
                    MessageDialog.Show(createTransferOrder ? Resources.UnableToCreateTransferOrder : Resources.UnableToCreateTransferRequest
                                        + CRLF + Resources.StoreTransferTemplateNotFound);
                    break;
            }
        }

        internal static void DeleteInventoryTransferErrorResultMessage(DeleteTransferResult result, bool isOrder)
        {
            string CRLF = "\n\r";

            switch (result)
            {
                case DeleteTransferResult.NotFound:
                    MessageDialog.Show(isOrder ? Resources.UnableToDeleteTransfer + CRLF + Resources.TransferOrderNotFound : Resources.UnableToDeleteRequest + CRLF + Resources.TransferRequestNotFound, MessageBoxIcon.Error);
                    break;
                case DeleteTransferResult.Sent:
                    MessageDialog.Show(isOrder ? Resources.UnableToDeleteTransfer + CRLF + Resources.SentTransferOrderCannotBeDeleted : Resources.UnableToDeleteRequest + CRLF + Resources.SentTransferRequestCannotBeDeleted, MessageBoxIcon.Error);
                    break;
                case DeleteTransferResult.FetchedByReceivingStore:
                    MessageDialog.Show(isOrder ? Resources.UnableToDeleteTransfer + CRLF + Resources.FetchedTransferOrderCannotBeDeleted : Resources.UnableToDeleteRequest + CRLF + Resources.FetchedTransferRequestCannotBeDeleted, MessageBoxIcon.Error);
                    break;
                case DeleteTransferResult.Received:
                    MessageDialog.Show(isOrder ? Resources.UnableToDeleteTransfer + CRLF + Resources.ReceivedTransferOrderCannotBeRejected : Resources.UnableToDeleteRequest + CRLF + Resources.ReceivedTransferRequestCannotBeRejected, MessageBoxIcon.Error);
                    break;
                case DeleteTransferResult.ErrorDeletingTransfer:
                    MessageDialog.Show(isOrder ? Resources.UnableToDeleteTransfer + CRLF + Resources.ErrorDeletingInventoryTransferOrder : Resources.UnableToDeleteRequest + CRLF + Resources.ErrorDeletingInventoryTransferRequest, MessageBoxIcon.Error);
                    break;
            }
        }

        #endregion


        #region Transfer orders

        internal static ReceiveTransferOrderResult ReceiveTransferOrder(InventoryTransferOrder transferOrder)
        {
            List<RecordIdentifier> ID = new List<RecordIdentifier>();
            ID.Add(transferOrder.ID);

            return ReceiveTransferOrders(ID);

        }

        internal static ReceiveTransferOrderResult ReceiveTransferOrders(List<RecordIdentifier> transferIDs)
        {
            if (!TestSiteService())
            {
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }

            bool receiveTransferOrder = true;
            bool promptUser = true;
            IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            ReceiveTransferOrderResult quantityResult = inventoryService.ReceiveTransferOrderQuantityIsCorrect(PluginEntry.DataModel, transferIDs, GetSiteServiceProfile());
            if (quantityResult == ReceiveTransferOrderResult.QuantitiesReceivedNotAccurate || quantityResult == ReceiveTransferOrderResult.SAPQuantitiesReceivedNotAccurate)
            {
                if (QuestionDialog.Show(Resources.ReceivedQuantityNotSameAsSentQuantity + " "
                                        + (quantityResult == ReceiveTransferOrderResult.SAPQuantitiesReceivedNotAccurate ? Resources.SAPReceivingQuantityUsedToAdjustInventory : Resources.InventoryAdjustmentWillBeCreatedForTheDifference) + "\r\n"
                                        + Resources.DoYouWantToContinue, Resources.ReceiveInventoryTransferOrder) == DialogResult.No)
                {
                    //The user has decided not to continue
                    return ReceiveTransferOrderResult.QuantitiesReceivedNotAccurate;
                }

                //The user selected YES to continue - we don't need to ask them again.
                promptUser = false;
            }

            //In some cases the user has already been asked the question and this functionality doesn't need to do it again
            if (promptUser)
            {
                receiveTransferOrder = (QuestionDialog.Show(Resources.ReceiveInventoryTransferQuestion, Resources.ReceiveInventoryTransferOrder) == DialogResult.Yes);
            }

            if (receiveTransferOrder)
            {
                try
                {
                    ReceiveTransferOrderResult result = ReceiveTransferOrderResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.ReceiveTransferOrders(PluginEntry.DataModel, transferIDs, GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case ReceiveTransferOrderResult.Success:
                            {
                                result = ReceiveTransferOrderResult.Success;
                            }
                            break;
                        default:
                            {
                                ReceiveTransferOrderErrorResultMessage(result);
                            }
                            break;
                    }

                    if (transferIDs.Count == 1) //Probably sent from the items view
                    {
                        InventoryTransferOrder updatedOrder = inventoryService.GetInventoryTransferOrder(PluginEntry.DataModel, GetSiteServiceProfile(), transferIDs[0], true);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", updatedOrder.ID, updatedOrder);
                    }
                    else
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", null, null);
                    }

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorSendingInventoryTransferOrder, MessageBoxIcon.Error);
                    return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
                }
            }

            return ReceiveTransferOrderResult.Success;
        }

        internal static AutoSetQuantityResult AutoSetQuantityOnTransferOrder(RecordIdentifier transferID)
        {
            if (!TestSiteService())
            {
                return AutoSetQuantityResult.ErrorAutoSettingQuantity;
            }

            if (QuestionDialog.Show(Resources.AutoSetQuantityQuestion, Resources.AutoSetQuantity) == DialogResult.Yes)
            {
                try
                {
                    IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    AutoSetQuantityResult result = AutoSetQuantityResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.AutoSetQuantityOnTransferOrder(PluginEntry.DataModel, transferID, GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case AutoSetQuantityResult.Success:
                            // OK
                            break;
                        case AutoSetQuantityResult.NotFound:
                            MessageDialog.Show(Resources.TransferOrderNotFound, MessageBoxIcon.Error);
                            break;
                        case AutoSetQuantityResult.AlreadyReceived:
                            MessageDialog.Show(Resources.TransferOrderHasAlreadyBeenReceived, MessageBoxIcon.Error);
                            break;
                        case AutoSetQuantityResult.ErrorAutoSettingQuantity:
                            MessageDialog.Show(Resources.ErrorAutoSettingQuantity, MessageBoxIcon.Error);
                            break;
                        default:
                            break;
                    }

                    InventoryTransferOrder updatedOrder = inventoryService.GetInventoryTransferOrder(PluginEntry.DataModel, GetSiteServiceProfile(), transferID, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", updatedOrder.ID, updatedOrder);

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorSendingInventoryTransferOrder, MessageBoxIcon.Error);
                    return AutoSetQuantityResult.ErrorAutoSettingQuantity;
                }
            }

            return AutoSetQuantityResult.Success;
        }

        internal static SendTransferOrderResult SendTransferOrders(List<RecordIdentifier> IDs)
        {
            if (!TestSiteService())
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }

            if (QuestionDialog.Show(Resources.SendInventoryTransferQuestion, Resources.SendInventoryTransfer) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    SendTransferOrderResult result = SendTransferOrderResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.SendTransferOrders(PluginEntry.DataModel, IDs, GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case SendTransferOrderResult.Success:
                            {
                                result = SendTransferOrderResult.Success;
                            }
                            break;
                        default:
                            {
                                SendInventoryTransferErrorResultMessage(result, true);
                            }
                            break;
                    }

                    if (IDs.Count == 1) //Probably sent from the items view
                    {
                        InventoryTransferOrder updatedOrder = inventoryService.GetInventoryTransferOrder(PluginEntry.DataModel, GetSiteServiceProfile(), IDs[0], true);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", updatedOrder.ID, updatedOrder);
                    }
                    else
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", null, null);
                    }

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorSendingInventoryTransferOrder, MessageBoxIcon.Error);
                    return SendTransferOrderResult.ErrorSendingTransferOrder;
                }
            }

            return SendTransferOrderResult.Success;
        }

        /// <summary>
        /// Sends a transfer order and prompts the user and displays error messages if something prevents the transfer order from being sent
        /// </summary>
        /// <param name="transferOrder">The order being sent</param>
        /// <returns></returns>
        internal static SendTransferOrderResult SendTransferOrder(InventoryTransferOrder transferOrder)
        {
            List<RecordIdentifier> ID = new List<RecordIdentifier>();
            ID.Add(transferOrder.ID);

            return SendTransferOrders(ID);
        }

        /// <summary>
        /// Takes in new values for the transfer order header and then copies the rest of the properties such as the item lines on the older transfer order to the new transfer order
        /// </summary>
        /// <param name="orderIDtoCopy">The ID of the request to be copied</param>
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order. If empty then the sending store ID from the old transfer order is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order. If empty then the receiving store ID from the old transfer order is used</param>
        /// <param name="description">The description to be assigned to the new transfer order. If empty then the description from the old transfer order is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>
        /// <returns></returns>
        internal static RecordIdentifier CopyTransferOrder(RecordIdentifier orderIDtoCopy, RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID, string description, DateTime expectedDelivery)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = inventoryService.CopyTransferOrder(PluginEntry.DataModel,
                                                                                        GetSiteServiceProfile(),
                                                                                        orderIDtoCopy,
                                                                                        sendingStoreID,
                                                                                        receivingStoreID,
                                                                                        description,
                                                                                        expectedDelivery,
                                                                                        CreatedBy(),
                                                                                        ref newOrderID,
                                                                                        true);

                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newOrderID;

                    default:
                        {
                            CreateTransferOrderMessage(result, true);
                        }
                        break;
                }

                return newOrderID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        /// <summary>
        /// Takes in new values for the transfer order header and then copies the rest of the properties such as the item lines on the request to the new transfer order
        /// </summary>
        /// <param name="requestIDtoCopy">The ID of the request to be copied</param>
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order. If empty then the sending store ID from the request is used</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order. If empty then the receiving store ID from the request is used</param>
        /// <param name="description">The description to be assigned to the new transfer order. If empty then the description from the request is used</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>
        /// <returns></returns>
        internal static RecordIdentifier CreateTransferOrderFromRequest(RecordIdentifier requestIDtoCopy, RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID, string description, DateTime expectedDelivery)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = inventoryService.CreateTransferOrderFromRequest(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                requestIDtoCopy,
                                                                sendingStoreID,
                                                                receivingStoreID,
                                                                description,
                                                                expectedDelivery,
                                                                CreatedBy(),
                                                                ref newOrderID,
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newOrderID;

                    default:
                        {
                            CreateTransferOrderMessage(result, true);
                        }
                        break;
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }

            return RecordIdentifier.Empty;
        }

        internal static CreateTransferOrderResult CreateTransferOrderFromRequest(InventoryTransferRequest request)
        {
            List<RecordIdentifier> ID = new List<RecordIdentifier>();
            ID.Add(request.ID);

            return CreateTransferOrdersFromRequests(ID);
        }

        internal static CreateTransferOrderResult CreateTransferOrdersFromRequests(List<RecordIdentifier> IDs)
        {
            if (!TestSiteService())
            {
                return CreateTransferOrderResult.ErrorCreatingTransferOrder;
            }

            if (QuestionDialog.Show(Resources.CreateInventoryTransferQuestion) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    CreateTransferOrderResult result = CreateTransferOrderResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.CreateTransferOrdersFromRequests(PluginEntry.DataModel, IDs,
                                                                                                                 CreatedBy(),
                                                                                                                 GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case CreateTransferOrderResult.Success:
                            {
                                result = CreateTransferOrderResult.Success;
                            }
                            break;
                        default:
                            {
                                CreateTransferOrderMessage(result, true);
                            }
                            break;
                    }


                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", null, null);

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorCreatingTransferOrder, MessageBoxIcon.Error);
                    return CreateTransferOrderResult.ErrorCreatingTransferOrder;
                }
            }

            return CreateTransferOrderResult.Success;
        }

        /// <summary>
        /// Takes in new values for the transfer order header and then adds the lines from the filter to the new order
        /// </summary>        
        /// <param name="sendingStoreID">The sending store ID to be assigned to the new transfer order. Cannot be empty</param>
        /// <param name="receivingStoreID">The receiving store ID to be assigned to the new transfer order. Cannot be empty</param>
        /// <param name="description">The description to be assigned to the new transfer order.</param>
        /// <param name="expectedDelivery">The expected delivery date to be assigned to the new transfer order. If empty then DateTime.Now + 3 days is used</param>
        /// <param name="filter">Container with filter IDs</param>
        /// <returns></returns>
        internal static RecordIdentifier CreateTransferOrderFromFilter(RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                                                                       string description, DateTime expectedDelivery, InventoryTemplateFilterContainer filter)
        {
            try
            {

                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;

                CreateTransferOrderResult result = inventoryService.CreateTransferOrderFromFilter(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                sendingStoreID,
                                                                receivingStoreID,
                                                                description,
                                                                expectedDelivery,
                                                                CreatedBy(),
                                                                filter,
                                                                ref newOrderID,
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newOrderID;

                    default:
                        {
                            CreateTransferOrderMessage(result, true);
                        }
                        break;
                }

                return newOrderID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static InventoryTransferOrder CreateTransferOrderHeader(RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                                                                       string description, DateTime expectedDelivery, bool closeConnection)
        {
            try
            {
                InventoryTransferOrder newOrder = new InventoryTransferOrder();

                newOrder.CreationDate = DateTime.Now;
                newOrder.SendingStoreId = sendingStoreID;
                newOrder.ReceivingStoreId = receivingStoreID;
                newOrder.Text = description;
                newOrder.ExpectedDelivery = expectedDelivery == null || expectedDelivery == DateTime.MinValue ? DateTime.Now.AddDays(3) : expectedDelivery;
                newOrder.CreatedBy = CreatedBy();

                newOrder.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveInventoryTransferOrder(
                                                                                                        PluginEntry.DataModel,
                                                                                                        GetSiteServiceProfile(),
                                                                                                        newOrder,
                                                                                                        closeConnection);

                return newOrder;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (closeConnection)
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                }
            }
        }
        internal static DeleteTransferResult DeleteInventoryTransferOrder(RecordIdentifier id, bool reject)
        {
            return DeleteInventoryTransferOrders(new List<RecordIdentifier> { id }, reject);
        }

        internal static DeleteTransferResult DeleteInventoryTransferOrders(List<RecordIdentifier> ids, bool reject)
        {
            if (!TestSiteService())
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }

            if (QuestionDialog.Show(Resources.DeleteInventoryTransferQuestion, Resources.DeleteInventoryTransfer) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    DeleteTransferResult result = DeleteTransferResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.DeleteTransferOrders(PluginEntry.DataModel, ids, reject, GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case DeleteTransferResult.Success:
                            {
                                result = DeleteTransferResult.Success;
                            }
                            break;
                        default:
                            {
                                DeleteInventoryTransferErrorResultMessage(result, true);
                            }
                            break;
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", null, null);

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorDeletingInventoryTransferOrder, MessageBoxIcon.Error);
                    return DeleteTransferResult.ErrorDeletingTransfer;
                }
            }

            return DeleteTransferResult.Success;
        }

        internal static DeleteTransferResult DeleteInventoryTransferRequest(RecordIdentifier id, bool reject)
        {
            return DeleteInventoryTransferRequests(new List<RecordIdentifier> { id }, reject);
        }

        internal static DeleteTransferResult DeleteInventoryTransferRequests(List<RecordIdentifier> ids, bool reject)
        {
            if (!TestSiteService())
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }

            if (QuestionDialog.Show(Resources.DeleteInventoryTransferRequestQuestion, Resources.DeleteInventoryTransferRequest) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    DeleteTransferResult result = DeleteTransferResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.DeleteTransferRequests(PluginEntry.DataModel, ids, reject, GetSiteServiceProfile()));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case DeleteTransferResult.Success:
                            {
                                result = DeleteTransferResult.Success;
                            }
                            break;
                        default:
                            {
                                DeleteInventoryTransferErrorResultMessage(result, false);
                            }
                            break;
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", null, null);

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorDeletingInventoryTransferRequest, MessageBoxIcon.Error);
                    return DeleteTransferResult.ErrorDeletingTransfer;
                }
            }

            return DeleteTransferResult.Success;
        }

        #endregion

        #region Transfer requests

        /// <summary>
        /// Sends a transfer request and prompts the user and displays error messages if something prevents the transfer request from being sent
        /// </summary>
        /// <param name="transferRequest">The request being sent</param>
        /// <returns></returns>
        internal static SendTransferOrderResult SendTransferRequests(List<RecordIdentifier> IDs)
        {
            if (!TestSiteService())
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }

            if (QuestionDialog.Show(Resources.SendInventoryTransferRequestQuestion, Resources.SendInventoryTransferRequest) == DialogResult.Yes)
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    SendTransferOrderResult result = SendTransferOrderResult.Success;
                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => result = inventoryService.SendTransferRequests(PluginEntry.DataModel, GetSiteServiceProfile(), IDs, true));
                    dlg.ShowDialog();

                    switch (result)
                    {
                        case SendTransferOrderResult.Success:
                            {
                                result = SendTransferOrderResult.Success;
                            }
                            break;
                        default:
                            {
                                SendInventoryTransferErrorResultMessage(result, true);
                            }
                            break;
                    }

                    if (IDs.Count == 1) //Probably sent from the items view
                    {
                        InventoryTransferRequest updatedRequest = inventoryService.GetInventoryTransferRequest(PluginEntry.DataModel, GetSiteServiceProfile(), IDs[0], true);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", updatedRequest.ID, updatedRequest);
                    }
                    else
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", null, null);
                    }

                    return result;
                }
                catch
                {
                    MessageDialog.Show(Resources.ErrorSendingInventoryTransferRequest, MessageBoxIcon.Error);
                    return SendTransferOrderResult.ErrorSendingTransferOrder;
                }
            }

            return SendTransferOrderResult.Success;
        }

        /// <summary>
        /// Sends a transfer request and prompts the user and displays error messages if something prevents the transfer request from being sent
        /// </summary>
        /// <param name="transferRequest">The request being sent</param>
        /// <param name="promptUser">If yes then the user is asked to confirm the sending of the transfer order</param>
        /// <param name="displayErrorMessage">If yes then an error message is displayed depending on the issue with the transfer order</param>
        /// <returns></returns>
        internal static SendTransferOrderResult SendTransferRequest(InventoryTransferRequest transferRequest)
        {
            List<RecordIdentifier> ID = new List<RecordIdentifier>();
            ID.Add(transferRequest.ID);

            return SendTransferRequests(ID);
        }

        internal static RecordIdentifier CreateTransferRequestFromOrder(RecordIdentifier orderIDtoCopy, RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                            string description, DateTime expectedDelivery)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newRequestID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = inventoryService.CreateTransferRequestFromOrder(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                orderIDtoCopy,
                                                                sendingStoreID,
                                                                receivingStoreID,
                                                                description,
                                                                expectedDelivery,
                                                                CreatedBy(),
                                                                ref newRequestID,
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newRequestID;

                    default:
                        CreateTransferOrderMessage(result, false);
                        break;
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }

            return RecordIdentifier.Empty;
        }

        internal static InventoryTransferRequest CreateTransferRequestHeader(RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                                                                       string description, DateTime expectedDelivery, bool closeConnection)
        {
            try
            {
                InventoryTransferRequest newRequest = new InventoryTransferRequest();

                newRequest.CreationDate = DateTime.Now;
                newRequest.SendingStoreId = sendingStoreID;
                newRequest.ReceivingStoreId = receivingStoreID;
                newRequest.Text = description;
                newRequest.ExpectedDelivery = expectedDelivery == null || expectedDelivery == DateTime.MinValue ? DateTime.Now.AddDays(3) : expectedDelivery;
                newRequest.CreatedBy = CreatedBy();

                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                newRequest.ID = inventoryService.SaveInventoryTransferRequest(PluginEntry.DataModel, GetSiteServiceProfile(PluginEntry.DataModel), newRequest, closeConnection);

                return newRequest;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (closeConnection)
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                }
            }
        }

        internal static RecordIdentifier CopyTransferRequest(RecordIdentifier requestIDtoCopy, RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
            string description, DateTime expectedDelivery)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newRequestID = RecordIdentifier.Empty;
                CreateTransferOrderResult result = inventoryService.CopyTransferRequest(PluginEntry.DataModel,
                                                                                        GetSiteServiceProfile(),
                                                                                        requestIDtoCopy,
                                                                                        sendingStoreID,
                                                                                        receivingStoreID,
                                                                                        description,
                                                                                        expectedDelivery,
                                                                                        CreatedBy(),
                                                                                        ref newRequestID,
                                                                                        true);

                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newRequestID;

                    default:
                        CreateTransferOrderMessage(result, true);
                        break;
                }

                return newRequestID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static RecordIdentifier CreateTransferRequestFromFilter(RecordIdentifier sendingStoreID, RecordIdentifier receivingStoreID,
                                                                       string description, DateTime expectedDelivery, InventoryTemplateFilterContainer filter)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newRequestID = RecordIdentifier.Empty;

                CreateTransferOrderResult result = inventoryService.CreateTransferRequestFromFilter(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                sendingStoreID,
                                                                receivingStoreID,
                                                                description,
                                                                expectedDelivery,
                                                                CreatedBy(),
                                                                filter,
                                                                ref newRequestID,
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newRequestID;

                    default:
                        CreateTransferOrderMessage(result, false);
                        break;
                }

                return newRequestID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static RecordIdentifier CreateTransferRequestFromTemplate(InventoryTransferRequest requestHeader, TemplateListItem template)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newRequestID = RecordIdentifier.Empty;
                requestHeader.CreatedBy = CreatedBy();

                CreateTransferOrderResult result = inventoryService.CreateTransferRequestFromTemplate(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                requestHeader,
                                                                template,
                                                                ref newRequestID,                                                                
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newRequestID;

                    default:
                        CreateTransferOrderMessage(result, false);
                        break;
                }

                return newRequestID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static RecordIdentifier CreateTransferOrderFromTemplate(InventoryTransferOrder orderHeader, TemplateListItem template)
        {
            try
            {
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                RecordIdentifier newOrderID = RecordIdentifier.Empty;
                orderHeader.CreatedBy = CreatedBy();

                CreateTransferOrderResult result = inventoryService.CreateTransferOrderFromTemplate(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                orderHeader,
                                                                template,
                                                                ref newOrderID,
                                                                true);
                switch (result)
                {
                    case CreateTransferOrderResult.Success:
                        return newOrderID;

                    default:
                        CreateTransferOrderMessage(result, true);
                        break;
                }

                return newOrderID;
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
                return RecordIdentifier.Empty;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        internal static List<DataEntity> GetAllStores()
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            stores.Insert(0, new DataEntity(null, Resources.AllStores));
            return stores;
        }

        internal static List<DataEntity> GetCurrentStore()
        {
            List<DataEntity> stores = new List<DataEntity>();
            stores.Add(Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            return stores;
        }

        #endregion
    }
}