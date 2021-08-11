using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransferRequestLineData : IDataProvider<InventoryTransferRequestLine>
    {
        /// <summary>
        /// Get transfer request lines based on an extended search filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferRequestID">Transfer request ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        List<InventoryTransferRequestLine> GetRequestLinesForInventoryTransferAdvanced(IConnectionManager entry, RecordIdentifier transferRequestID, out int totalRecordsMatching, InventoryTransferFilterExtended filter);
        
        List<InventoryTransferRequestLine> GetListForInventoryTransferRequest(
            IConnectionManager entry, 
            RecordIdentifier inventoryTransferRequestId, 
            InventoryTransferOrderLineSortEnum sortBy, 
            bool sortBackwards);
        
        bool ItemWithSameParametersExists(IConnectionManager entry, InventoryTransferRequestLine line);
        void SaveIfChanged(IConnectionManager entry, InventoryTransferRequestLine inventoryTransferRequestLine);

        InventoryTransferRequestLine Get(IConnectionManager entry, RecordIdentifier requestLineId);

        /// <summary>
        /// Tries to find and return a transfer request line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="line">The line you want check if exists</param>
        /// <returns>A InventoryTransferRequestLine object, null if no matching line was found</returns>
        InventoryTransferRequestLine GetLine(IConnectionManager entry, InventoryTransferRequestLine line);

        /// <summary>
        /// Copies transfer request lines from one order to another
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="copyFrom">The ID of the transfer request to copy from</param>
        /// <param name="copyTo">The ID of the transfer request to copy to</param>                
        void CopyLines(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo);

        /// <summary>
        /// Copies transfer order lines to a new transfer request
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="copyFromOrder">The ID of the transfer order to copy from</param>
        /// <param name="copyToRequest">The ID of the transfer request to copy to</param>                
        void CopyLinesFromOrder(IConnectionManager entry, RecordIdentifier copyFromOrder, RecordIdentifier copyToRequest);

        /// <summary>
        /// Returns the number of lines on a specific transfer request
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="requestID">The ID of the transfer order</param>
        /// <returns>Number of lines</returns>
        int LineCount(IConnectionManager entry, RecordIdentifier requestID);

        /// <summary>
        /// Mark all lines as sent, in a transfer request
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferRequestID">Transfer request ID</param>
        void MarkTransferRequestLinesAsSent(IConnectionManager entry, RecordIdentifier transferRequestID);
    }
}