using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransferOrderData : IDataProvider<InventoryTransferOrder>, ISequenceable
    {
        List<InventoryTransferOrder> GetFromList(
            IConnectionManager entry, 
            List<RecordIdentifier> transferIds,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards);

        List<InventoryTransferOrder> GetListForStore(
            IConnectionManager entry, 
            List<RecordIdentifier> storeIds, 
            InventoryTransferType transferType, 
            InventoryTransferOrderSortEnum orderSortBy, 
            bool sortBackwards);

        List<InventoryTransferOrder> GetInventoryInTransit(
            IConnectionManager entry,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards,
            RecordIdentifier storeId);

        InventoryTransferOrder Get(IConnectionManager entry, RecordIdentifier transferId);

        /// <summary>
        /// Search existing transfer orders based on a basic filter. Should be used when generating store transfers from existing transfer orders
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="filter">Inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferOrder> Search(IConnectionManager entry, InventoryTransferFilter filter);

        /// <summary>
        /// Search existing transfer orders based on a basic filter. Should be used when generating store transfers from existing transfer orders
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="totalRecordsMatching">Number of transfer orders</param>
        /// <param name="filter">Extended inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferOrder> AdvancedSearch(IConnectionManager entry, out int totalRecordsMatching, InventoryTransferFilterExtended filter);
        
        /// <summary>
        /// Search existing transfer orders based on an extended filter. Should be used when managing existing transfer orders.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="filter">Extended inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferOrder> Search(IConnectionManager entry, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Check if there are any items on a transfer order that do not have a valid unit conversion
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <returns>A list of error messages</returns>
        List<string> CheckUnitConversionsForTransferOrder(IConnectionManager entry, RecordIdentifier transferOrderID);

        /// <summary>
        /// Update inventory after sending, receiving or deleting a transfer order
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <param name="action">Action performed on the transfer order</param>
        /// <param name="adjustmentNeeded">True if an adjustment is required when receiving the transfer order because there are items with sent quantity not equal to received quantity</param>
        /// <param name="adjustmentReasonCode">Reason code for adjustment lines</param>
        /// <param name="adjustmentJournalID">The adjusment journal header ID where to add adjustment lines (must be created before calling this)</param>
        /// <returns></returns>
        bool UpdateInventoryForTransferOrder(IConnectionManager entry, RecordIdentifier transferOrderID, TransferOrderUpdateInventoryAction action, bool adjustmentNeeded, RecordIdentifier adjustmentReasonCode, RecordIdentifier adjustmentJournalID);

        /// <summary>
        /// Get total number of unreceived items for a list of transfer orders
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderIds">IDs of the transfer orders</param>
        /// <returns></returns>
        Dictionary<RecordIdentifier, decimal> GetTotalUnreceivedItemForTransferOrders(IConnectionManager entry, List<RecordIdentifier> transferOrderIds);

        /// <summary>
        /// Set the processing status of a transfer order
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">ID of the transfer order</param>
        /// <param name="status">New processing status</param>
        void SetTransferOrderProcessingStatus(IConnectionManager entry, RecordIdentifier transferOrderID, InventoryProcessingStatus status);

        /// <summary>
        /// Get the latest transfer order created by the omni mobile app and was not yet posted.
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="templateID">The template ID used for creating the stock counting journal.</param>
        /// <param name="storeID">The store ID in which the journal was created.</param>
        /// <returns>Return a transfer order</returns>
        InventoryTransferOrder GetOmniTransferOrderByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID);
    }
}