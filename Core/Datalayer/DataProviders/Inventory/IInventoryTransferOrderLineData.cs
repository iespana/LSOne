using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransferOrderLineData : IDataProvider<InventoryTransferOrderLine>
    {
        List<InventoryTransferOrderLine> GetListForInventoryTransfer(
            IConnectionManager entry, 
            RecordIdentifier inventoryTransferId, 
            InventoryTransferOrderLineSortEnum sortBy, 
            bool sortBackwards,
            bool getUnsentItemsOnly = false);

        bool ItemWithSameParametersExists(IConnectionManager entry, InventoryTransferOrderLine line);

        void SaveIfChanged(IConnectionManager entry, InventoryTransferOrderLine inventoryTransferOrderLine);

        /// <summary>
        /// Get transfer order lines based on an extended search filter
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <param name="totalRecordsMatching">Total number of records matching the search filter</param>
        /// <param name="filter">Filter object containing search criteria</param>
        /// <returns></returns>
        List<InventoryTransferOrderLine> GetOrderLinesForInventoryTransferAdvanced(IConnectionManager entry, RecordIdentifier transferOrderID, out int totalRecordsMatching, InventoryTransferFilterExtended filter);

        InventoryTransferOrderLine Get(IConnectionManager entry, RecordIdentifier transferLineId);

        /// <summary>
        /// Tries to find and return a transfer order line with matching unit of measure and item ID 
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="line">The line you want check if exists</param>
        /// <returns>A InventoryTransferOrderLine object, null if no matching line was found</returns>
        InventoryTransferOrderLine GetLine(IConnectionManager entry, InventoryTransferOrderLine line);

        /// <summary>
        /// Saves multiple transfer order lines at the same time 
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="lines">The transfer order lines to be saved</param>
        void SaveLines(IConnectionManager entry, List<InventoryTransferOrderLine> lines);

        /// <summary>
        /// Copies transfer order lines from one order to another
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="copyFrom">The ID of the transfer order to copy from</param>
        /// <param name="copyTo">The ID of the transfer order to copy to</param>                
        void CopyLines(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo);

        /// <summary>
        /// Copies transfer order lines from one order to another
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="copyFromRequest">The ID of the transfer request to copy from</param>
        /// <param name="copyToOrder">The ID of the transfer order to copy to</param>                
        void CopyLinesFromRequest(IConnectionManager entry, RecordIdentifier copyFromRequest, RecordIdentifier copyToOrder);

        /// <summary>
        /// Returns the number of lines on a specific transfer order
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="orderID">The ID of the transfer order</param>
        /// <returns>Number of lines</returns>
        int LineCount(IConnectionManager entry, RecordIdentifier orderID);

        /// <summary>
        /// Set the received quantity on each transfer order line to be equal to the sent quantity
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="tranferOrderID">Transfer order ID</param>
        void AutoSetQuantityOnTransferOrderLines(IConnectionManager entry, RecordIdentifier tranferOrderID);

        /// <summary>
        /// Mark all lines as sent and optionally set the receiving quantity equal to sending quantity, in a transfer order
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">Transfer order ID</param>
        /// <param name="populateReceivingQuantity">If true, receiving quantity of items will be set to the sending quantity</param>
        void SendTransferOrderLines(IConnectionManager entry, RecordIdentifier transferOrderID, bool populateReceivingQuantity);

        /// <summary>
		/// Imports transfer order lines from an xml file
		/// </summary>
		/// <param name="entry">The entry into the database</param>
        /// <param name="transferOrderID">ID of the transfer order in which to import lines</param>
		/// <param name="xmlData">XML data to import</param>
	    int ImportTransferOrderLinesFromXML(IConnectionManager entry, RecordIdentifier transferOrderID, string xmlData);

        /// <summary>
        /// Updates a single line with a picture ID based on the transaction ID and line IDs from the mobile inventory app
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
        /// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
        /// <param name="pictureID">The ID of the picture to set on the line</param>
        [LSOneUsage(CodeUsage.LSCommerce)]
        void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID);

        /// <summary>
        /// Calculate the item costs for each transfer order line when receiving a transfer order
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="transferOrderID">ID of the transfer order</param>
        void CalculateReceivingItemCosts(IConnectionManager entry, RecordIdentifier transferOrderID);
    }
}