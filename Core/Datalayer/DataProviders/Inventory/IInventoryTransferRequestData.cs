using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransferRequestData : IDataProvider<InventoryTransferRequest>, ISequenceable
    {
        List<InventoryTransferRequest> GetFromList(
            IConnectionManager entry,
            List<RecordIdentifier> requestIds,
            InventoryTransferOrderSortEnum orderSortBy,
            bool sortBackwards);

        List<InventoryTransferRequest> GetListForStore(
            IConnectionManager entry,
            List<RecordIdentifier> storeIds, 
            InventoryTransferType transferRequestType, 
            InventoryTransferOrderSortEnum sortBy, 
            bool sortBackwards);

        InventoryTransferRequest Get(IConnectionManager entry, RecordIdentifier transferRequestId);

        /// <summary>
        /// Search existing transfer orders based on a basic filter. Should be used when generating store transfers from existing transfer orders
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="filter">Inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferRequest> Search(IConnectionManager entry, InventoryTransferFilter filter);

        /// <summary>
        /// Search existing transfer requests based on an extended filter. Should be used when managing existing transfer requests.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="totalRecordsMatching">Number of transfer orders</param>
        /// <param name="filter">Extended inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferRequest> AdvancedSearch(IConnectionManager entry, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Search existing transfer orders requests on an extended filter. Should be used when managing existing transfer requests.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="filter">Extended inventory transfer filter</param>
        /// <returns></returns>
        List<InventoryTransferRequest> Search(IConnectionManager entry, InventoryTransferFilterExtended filter);

        /// <summary>
        /// Check if there are any items on a transfer request that do not have a valid unit conversion
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferRequestID">Transfer request ID</param>
        /// <returns>A list of error messages</returns>
        List<string> CheckUnitConversionsForTransferRequest(IConnectionManager entry, RecordIdentifier transferRequestID);
    }
}