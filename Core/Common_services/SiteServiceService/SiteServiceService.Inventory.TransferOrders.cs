using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.Utilities.Development;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual bool ItemWithSameParametersExistsInTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderLine line, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ItemWithSameParametersExistsInTransferOrder( CreateLogonInfo(entry),line), closeConnection);
            return result;
        }

        public virtual SaveTransferOrderLineResult SaveInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderLine inventoryTransferOrderLine, bool isReceiving, out RecordIdentifier newLineID, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            RecordIdentifier returnLineID = RecordIdentifier.Empty;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryTransferOrderLine(CreateLogonInfo(entry), inventoryTransferOrderLine, isReceiving, out returnLineID), closeConnection);
            newLineID = returnLineID;
            return result;

        }        

        public virtual RecordIdentifier SaveInventoryTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrder inventoryTransferOrder, bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryTransferOrder(CreateLogonInfo(entry), inventoryTransferOrder), closeConnection);
            return result;
        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferOrderId, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferOrderLine(inventoryTransferOrderId, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual SaveTransferOrderLineResult DeleteInventoryTransferOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferOrderLineIds, bool closeConnection)
        {
            SaveTransferOrderLineResult result = SaveTransferOrderLineResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferOrderLines(CreateLogonInfo(entry), inventoryTransferOrderLineIds), closeConnection);
            return result;
        }

        public virtual void SaveInventoryTransferOrderLineIfChanged(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTransferOrderLine inventoryTransferOrderLine, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveInventoryTransferOrderLineIfChanged(CreateLogonInfo(entry),inventoryTransferOrderLine), closeConnection);
        }

        public virtual List<InventoryStatus> GetInventoryStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier variantID, RecordIdentifier regionID, bool closeConnection)
        {
            List<InventoryStatus> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryStatus(itemID, variantID, regionID, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual SendTransferOrderResult SendTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> transferIDs, DateTime sendingTime,bool closeConnection)
        {
            SendTransferOrderResult result = SendTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SendTransferOrders(transferIDs, sendingTime, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual SendTransferOrderResult SendTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferID, DateTime sendingTime, bool closeConnection)
        {
            SendTransferOrderResult result = SendTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SendTransferOrder(transferID, sendingTime, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferID, DateTime receivingTime, bool closeConnection)
        {
            ReceiveTransferOrderResult result = ReceiveTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReceiveTransferOrder(transferID, receivingTime, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> transferIDs, DateTime receivingTime, bool closeConnection)
        {
            ReceiveTransferOrderResult result = ReceiveTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReceiveTransferOrders(transferIDs, receivingTime, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual AutoSetQuantityResult AutoSetQuantityOnTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferID, bool closeConnection)
        {
            AutoSetQuantityResult result = AutoSetQuantityResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.AutoSetQuantityOnTransferOrder(transferID, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual ReceiveTransferOrderResult ReceiveTransferOrderQuantityIsCorrect(IConnectionManager entry, List<RecordIdentifier> transferIDs, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            ReceiveTransferOrderResult result = ReceiveTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReceiveTransferOrderQuantityIsCorrect(transferIDs, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual CreateTransferOrderResult CreateTransferOrdersFromRequests(
           IConnectionManager entry,
           List<RecordIdentifier> requestIDs,
           RecordIdentifier createdBy,
           SiteServiceProfile siteServiceProfile,
           bool closeConnection)
        {
            CreateTransferOrderResult result = CreateTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferOrdersFromRequests(requestIDs, createdBy, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual CreateTransferOrderResult CreateTransferOrderFromRequest(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier requestIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            CreateTransferOrderResult result = CreateTransferOrderResult.Success;
            RecordIdentifier returnOrderID = RecordIdentifier.Empty;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferOrderFromRequest(CreateLogonInfo(entry), requestIDtoCopy, orderInformation, out returnOrderID), closeConnection);
            newOrderID = returnOrderID;

            return result;
        }
        

        public virtual DeleteTransferResult DeleteInventoryTransferOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTransferOrderId, bool reject, bool closeConnection)
        {
            DeleteTransferResult result = DeleteTransferResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferOrder(inventoryTransferOrderId, reject, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual DeleteTransferResult DeleteInventoryTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> inventoryTransferOrderIds, bool reject, bool closeConnection)
        {
            DeleteTransferResult result = DeleteTransferResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteInventoryTransferOrders(inventoryTransferOrderIds, reject, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferContainer> GetInventoryTransfersAndLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            List<RecordIdentifier> listOfTransferIdsToFetch,
            bool closeConnection)
        {
            List<InventoryTransferContainer> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTransferOrdersAndLines(listOfTransferIdsToFetch, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferOrder> GetInventoryInTransit(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, InventoryTransferOrderSortEnum sorting, bool sortAscending, RecordIdentifier storeId, bool closeConnection)
        {
            List<InventoryTransferOrder> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryInTransit(CreateLogonInfo(entry), sorting, sortAscending, storeId), closeConnection);
            return result;
        }
       
        public virtual bool TransferOrderCreatedFromTransferRequest(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTransferRequestId,
            RecordIdentifier inventoryTransferOrderId,
            bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.TransferOrderCreatedFromTransferRequest(inventoryTransferRequestId, inventoryTransferOrderId, CreateLogonInfo(entry)), closeConnection);
            return result;
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
            InventoryTransferOrder result = new InventoryTransferOrder();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTransferOrder(CreateLogonInfo(entry), ID), closeConnection);
            return result;
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
            InventoryTransferOrderLine result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetTransferOrderLine(CreateLogonInfo(entry), line), closeConnection);

            return result;
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
            List<InventoryTransferOrderLine> result = new List<InventoryTransferOrderLine>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOrderLinesForInventoryTransfer(CreateLogonInfo(entry), ID, sortBy, sortBackwards, getUnsentItemsOnly), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier transferOrderID,
            out int totalRecordsMatching,
            InventoryTransferFilterExtended filter,
            bool closeConnection)
        {
            List<InventoryTransferOrderLine> result = new List<InventoryTransferOrderLine>();

            totalRecordsMatching = 0;
            int totalRecordsMatchingCopy = totalRecordsMatching;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOrderLinesForInventoryTransferAdvanced(CreateLogonInfo(entry), transferOrderID, out totalRecordsMatchingCopy, filter), closeConnection);

            totalRecordsMatching = totalRecordsMatchingCopy;

            return result;
        }

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilter filter, bool closeConnection)
        {
            List<InventoryTransferOrder> result = new List<InventoryTransferOrder>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferOrders(CreateLogonInfo(entry), filter), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersExtended(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferFilterExtended filter, bool closeConnection)
        {
            List<InventoryTransferOrder> result = new List<InventoryTransferOrder>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferOrdersExtended(CreateLogonInfo(entry), filter), closeConnection);
            return result;
        }

        public virtual List<InventoryTransferOrder> SearchInventoryTransferOrdersAdvanced(
        IConnectionManager entry,
        SiteServiceProfile siteServiceProfile,
        out int totalRecordsMatching,
        InventoryTransferFilterExtended filter,
        bool closeConnection)
        {
            List<InventoryTransferOrder> result = new List<InventoryTransferOrder>();

            totalRecordsMatching = 0;
            int totalRecordsMatchingCopy = totalRecordsMatching;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchInventoryTransferOrdersAdvanced(CreateLogonInfo(entry), out totalRecordsMatchingCopy, filter), closeConnection);

            totalRecordsMatching = totalRecordsMatchingCopy;

            return result;
        }
        

        public virtual CreateTransferOrderResult CopyTransferOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier orderIDtoCopy,
            InventoryTransferOrder orderInformation,
            ref RecordIdentifier newOrderID,
            bool closeConnection)
        {
            RecordIdentifier returnOrderID = RecordIdentifier.Empty;

            CreateTransferOrderResult result = CreateTransferOrderResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CopyTransferOrder(CreateLogonInfo(entry), orderIDtoCopy, orderInformation, out returnOrderID), closeConnection);

            newOrderID = returnOrderID;
            return result;
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
                CreateTransferOrderResult result = CreateTransferOrderResult.Success;
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;

                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferOrderFromFilter(CreateLogonInfo(entry), orderInformation, filter, out returnOrderID), closeConnection);

                newOrderID = returnOrderID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }

        public virtual CreateTransferOrderResult CreateTransferOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTransferOrder orderInformation, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                CreateTransferOrderResult result = CreateTransferOrderResult.Success;
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;

                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateTransferOrderFromTemplate(CreateLogonInfo(entry), orderInformation, template, out returnOrderID), closeConnection);

                newOrderID = returnOrderID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }

        public virtual Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> transferOrderIds, bool closeConnection)
        {
            Dictionary<RecordIdentifier, decimal> result = new Dictionary<RecordIdentifier, decimal>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetTotalUnreceivedItemForTransferOrders(CreateLogonInfo(entry), transferOrderIds), closeConnection);
            return result;
        }

	    public virtual int ImportTransferOrderLinesFromXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transferOrderID, string xmlData, bool closeConnection)
        {
            int result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ImportTransferOrderLinesFromXML(CreateLogonInfo(entry), transferOrderID, xmlData), closeConnection);
            return result;
        }
    }
}
