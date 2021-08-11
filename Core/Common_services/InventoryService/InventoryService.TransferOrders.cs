using System;
using System.Linq;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
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
        public virtual SendTransferOrderResult SendTransferOrders(
           IConnectionManager entry,
           List<RecordIdentifier> inventoryTransferIds,
           SiteServiceProfile siteServiceProfile)
        {
            DateTime sendingTime = DateTime.Now;
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.SendTransferOrders(
                                                        entry,
                                                        siteServiceProfile,
                                                        inventoryTransferIds,
                                                        sendingTime,
                                                        true);
            }
            catch (Exception)
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);                
            }
        }

        public virtual SendTransferOrderResult SendTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferId,
            SiteServiceProfile siteServiceProfile)
        {
            DateTime sendingTime = DateTime.Now;
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.SendTransferOrder(
                                                        entry,
                                                        siteServiceProfile,
                                                        inventoryTransferId,
                                                        sendingTime,
                                                        true);
            }
            catch (Exception)
            {
                return SendTransferOrderResult.ErrorSendingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(
            IConnectionManager entry,
            List<RecordIdentifier> transferIDs,
            SiteServiceProfile siteServiceProfile)
        {
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.ReceiveTransferOrderQuantityIsCorrect(
                                                        entry,
                                                        transferIDs,
                                                        siteServiceProfile,
                                                        true);
            }
            catch (Exception)
            {
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrders(
            IConnectionManager entry,
            List<RecordIdentifier> inventoryTransferIDs,
            SiteServiceProfile siteServiceProfile
            )
        {
            DateTime receivingTime = DateTime.Now;
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.ReceiveTransferOrders(
                                                        entry,
                                                        siteServiceProfile,
                                                        inventoryTransferIDs,
                                                        receivingTime,
                                                        true);
            }
            catch (Exception)
            {
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferID,
            SiteServiceProfile siteServiceProfile
            )
        {
            DateTime receivingTime = DateTime.Now;
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.ReceiveTransferOrder(
                                                        entry,
                                                        siteServiceProfile,
                                                        inventoryTransferID,
                                                        receivingTime,
                                                        true);
            }
            catch (Exception)
            {
                return ReceiveTransferOrderResult.ErrorReceivingTransferOrder;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual AutoSetQuantityResult AutoSetQuantityOnTransferOrder(
            IConnectionManager entry,
            RecordIdentifier inventoryTransferID,
            SiteServiceProfile siteServiceProfile
            )
        {
            DateTime receivingTime = DateTime.Now;
            try
            {

                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);

                return siteServiceService.AutoSetQuantityOnTransferOrder(
                                                        entry,
                                                        siteServiceProfile,
                                                        inventoryTransferID,
                                                        true);
            }
            catch (Exception)
            {
                return AutoSetQuantityResult.ErrorAutoSettingQuantity;
            }
            finally
            {
                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual DeleteTransferResult DeleteTransferOrder(
            IConnectionManager entry,
            RecordIdentifier transferOrderId,
            bool reject,
            SiteServiceProfile siteServiceProfile
            )
        {
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                return siteServiceService.DeleteInventoryTransferOrder(entry, siteServiceProfile, transferOrderId, reject, true);
            }
            catch (Exception)
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }
            finally
            {

                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }

        public virtual DeleteTransferResult DeleteTransferOrders(
            IConnectionManager entry,
            List<RecordIdentifier> transferOrderIds,
            bool reject,
            SiteServiceProfile siteServiceProfile
            )
        {
            try
            {
                ISiteServiceService siteServiceService = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
                return siteServiceService.DeleteInventoryTransferOrders(entry, siteServiceProfile, transferOrderIds, reject, true);
            }
            catch (Exception)
            {
                return DeleteTransferResult.ErrorDeletingTransfer;
            }
            finally
            {

                Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
            }
        }
        

        /// <summary>
        /// Returns information about a specific inventory transfer order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ID">The unique ID Of the transfer order</param>
        /// <returns>Information about a specific inventory transfer order</returns>
        public virtual InventoryTransferOrder GetInventoryTransferOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier ID,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetInventoryTransferOrder(entry, siteServiceProfile, ID, closeConnection);
            }
            catch (Exception)
            {
                return new InventoryTransferOrder();
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
        /// Tries to find and return a transfer order line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="line">The transfer order line you want to get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>A InventoryTransferOrderLine object, null if no matching line was found</returns>
        public virtual InventoryTransferOrderLine GetTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrderLine line, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetTransferOrderLine(entry, siteServiceProfile, line, closeConnection);
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
        public virtual List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransfer(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier ID,
            InventoryTransferOrderLineSortEnum sortBy,
            bool sortBackwards,
            bool getUnsentItemsOnly,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetOrderLinesForInventoryTransfer(entry, siteServiceProfile, ID, sortBy, sortBackwards, getUnsentItemsOnly, closeConnection);
            }
            catch (Exception)
            {
                return new List<InventoryTransferOrderLine>();
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferOrder> GetInventoryInTransit(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, InventoryTransferOrderSortEnum sorting, bool sortaAcending, RecordIdentifier storeId, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).GetInventoryInTransit(entry, siteServiceProfile, sorting, sortaAcending, storeId, closeConnection);
            }
            catch (Exception)
            {
                return new List<InventoryTransferOrder>();
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual bool ItemWithSameParametersExistsInTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderLine line, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).ItemWithSameParametersExistsInTransferOrder(entry, siteServiceProfile, line, closeConnection);
            }
            catch (Exception)
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

        public virtual SaveTransferOrderLineResult SaveInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrderLine(entry, siteServiceProfile, inventoryTransferOrderLine, isReceiving, out newLineID, closeConnection);
            }
            catch (Exception)
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

        public virtual RecordIdentifier SaveInventoryTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrder inventoryTransferOrder, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SaveInventoryTransferOrder(entry, siteServiceProfile, inventoryTransferOrder, closeConnection);
            }
            catch (Exception)
            {
                return RecordIdentifier.Empty;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferOrderId, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferOrderLine(entry, siteServiceProfile, inventoryTransferOrderId, closeConnection);
            }
            catch (Exception)
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

        public SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferOrderLineIds, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).DeleteInventoryTransferOrderLines(entry, siteServiceProfile, inventoryTransferOrderLineIds, closeConnection);
            }
            catch (Exception)
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

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferOrders(entry, siteServiceProfile, filter, closeConnection);
            }
            catch (Exception)
            {
                return new List<InventoryTransferOrder>();
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferOrdersExtended(entry, siteServiceProfile, filter, closeConnection);
            }
            catch (Exception)
            {
                return new List<InventoryTransferOrder>();
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).SearchInventoryTransferOrdersAdvanced(entry, siteServiceProfile, out totalRecordsMatching, filter, closeConnection);
            }
            catch (Exception)
            {
                totalRecordsMatching = 0;
                return new List<InventoryTransferOrder>();
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreateTransferOrderResult CopyTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferOrder orderInformation = new InventoryTransferOrder();
                orderInformation.SendingStoreId = sendingStoreID;
                orderInformation.ReceivingStoreId = receivingStoreID;
                orderInformation.Text = description;
                orderInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                orderInformation.CreatedBy = createdBy;

                return CopyTransferOrder(entry, siteServiceProfile, orderIDtoCopy, orderInformation, ref newOrderID, closeConnection);
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

        public virtual CreateTransferOrderResult CopyTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CopyTransferOrder(entry, siteServiceProfile, orderIDtoCopy, orderInformation, ref newOrderID, closeConnection);
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

        public virtual CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferOrder orderInformation = new InventoryTransferOrder();
                orderInformation.SendingStoreId = sendingStoreID;
                orderInformation.ReceivingStoreId = receivingStoreID;
                orderInformation.Text = description;
                orderInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                orderInformation.CreatedBy = createdBy;
                orderInformation.CreationDate = DateTime.Now;

                return CreateTransferOrderFromRequest(entry, siteServiceProfile, requestIDtoCopy, orderInformation, ref newOrderID, closeConnection);
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

        public virtual CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreateTransferOrderFromRequest(entry, siteServiceProfile, requestIDtoCopy, orderInformation, ref newOrderID, closeConnection);
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

        public virtual CreateTransferOrderResult CreateTransferOrderFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTransferOrder orderInformation,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreateTransferOrderFromFilter(entry, siteServiceProfile, orderInformation, filter, ref newOrderID, closeConnection);
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

        public virtual CreateTransferOrderResult CreateTransferOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrder orderInformation, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreateTransferOrderFromTemplate(entry, siteServiceProfile, orderInformation, template, ref newOrderID, closeConnection);
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


        public virtual List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferOrderID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetOrderLinesForInventoryTransferAdvanced(entry, siteServiceProfile, transferOrderID, out totalRecordsMatching, filter, closeConnection);
        }

        public virtual CreateTransferOrderResult CreateTransferOrderFromFilter(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier sendingStoreID,
            RecordIdentifier receivingStoreID,
            string description,
            DateTime expectedDelivery,
            RecordIdentifier createdBy,
            InventoryTemplateFilterContainer filter,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            try
            {
                InventoryTransferOrder orderInformation = new InventoryTransferOrder();
                orderInformation.SendingStoreId = sendingStoreID;
                orderInformation.ReceivingStoreId = receivingStoreID;
                orderInformation.Text = description;
                orderInformation.ExpectedDelivery = ExpectedDeliveryDate(entry, expectedDelivery, sendingStoreID);
                orderInformation.CreatedBy = createdBy;

                return CreateTransferOrderFromFilter(entry, siteServiceProfile, orderInformation, filter, ref newOrderID, closeConnection);
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

        protected virtual DateTime ExpectedDeliveryDate(IConnectionManager entry, DateTime expectedDelivery, RecordIdentifier storeID)
        {
            DateTime deliveryDate = DateTime.Now.AddDays(3);
            //If the expected date is null or has been set to the MinValue then retrieve the expected delivery date for the store and use that
            if (expectedDelivery == null || expectedDelivery == DateTime.MinValue || expectedDelivery.Date < DateTime.Now.Date)
            {
                //Get the expected delivery date for the store
                if (!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    Store store = Providers.StoreData.Get(entry, storeID, CacheType.CacheTypeNone, UsageIntentEnum.Minimal);
                    if (store != null)
                    {
                        return store.StoreTransferExpectedDeliveryDate();
                    }
                }
            }
            else
            {
                return expectedDelivery;
            }

            return deliveryDate;
        }

        public virtual Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> transferOrderIds, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetTotalUnreceivedItemForTransferOrders(entry, siteServiceProfile, transferOrderIds, closeConnection);
        }

        public virtual int ImportTransferOrderLinesFromXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferOrderID, string xmlData, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).ImportTransferOrderLinesFromXML(entry, siteServiceProfile, transferOrderID, xmlData, closeConnection);
        }
    }
}
