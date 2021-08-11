using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using System.Data;
using LSOne.DataLayer.BusinessObjects.FileImport;
using System;
using LSOne.Utilities.Development;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Searches for Stock counting entries 
        /// </summary>
        /// <param name="filter">Filter settings container</param>
        /// <param name="totalRecordsMatching">out: how many rows there are in total</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryJournalTransaction> SearchJournalTransactions(InventoryJournalTransactionFilter filter, out int totalRecordsMatching, LogonInfo logonInfo);

        /// <summary>
        /// Updates the on hand inventory of all journal transactions in a stock counting journal 
        /// </summary>
        /// <param name="journalID">ID of the stock counting journal</param>
        /// <param name="logonInfo">The login information for the database</param>
        [OperationContract]
        void RefreshStockCountingJournalInventoryOnHand(RecordIdentifier journalID, LogonInfo logonInfo);

        /// <summary>
        /// Get a list of journals based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="filter">Filter container</param>
        /// <param name="totalRecordsMatching">Total number of records that matched the search criteria</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryAdjustment> GetJournalListAdvancedSearch(LogonInfo logonInfo, InventoryAdjustmentFilter filter, out int totalRecordsMatching);

        /// <summary>
        /// Get the current processing status for a journal
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="journalId">ID fo the journal to check</param>
        /// <returns></returns>
        [OperationContract]
        AdjustmentStatus GetAdjustmentStatus(LogonInfo logonInfo, RecordIdentifier journalId);

        /// <summary>
        /// Set the processing status for a journal
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="journalId">ID fo the journal to set</param>
        /// <param name="status">The status to set</param>
        /// <returns></returns>
        [OperationContract]
        void SetAdjustmentStatus(LogonInfo logonInfo, RecordIdentifier journalId, InventoryProcessingStatus status);

        /// <summary>
        /// Get stock counting
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="journalId">The journal ID to be get</param>
        /// <returns>The inventory adjustment</returns>
        [OperationContract]
        InventoryAdjustment GetStockCounting(LogonInfo logonInfo, RecordIdentifier journalId);

        /// <summary>
        /// Deletes a stock counting journal
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="stockCountingID">The stock counting ID to delete</param>
        /// <returns>Result of the operation.</returns>
        [OperationContract]
        DeleteJournalResult DeleteStockCounting(LogonInfo logonInfo, RecordIdentifier stockCountingID);

        /// <summary>
        /// Deletes multiple inventory journal transaction lines
        /// </summary>
        /// <param name="IDs">The IDs of the inventory journal to delete</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>Operation result</returns>
        [OperationContract]
        DeleteJournalTransactionsResult DeleteMultipleJournalTransactions(List<RecordIdentifier> IDs, LogonInfo logonInfo);

        /// <summary>
        /// Get inventory journal transaction by ID
        /// </summary>
        /// <param name="journalTransactionId">The journal transaction ID</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>Returns the inventory journal transaction for the supplied ID</returns>
        [OperationContract]
        InventoryJournalTransaction GetInventoryJournalTransaction(RecordIdentifier journalTransactionId, LogonInfo logonInfo);

        /// <summary>
        /// Get inventory on hand for the specified item ID and store ID
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Inventory on hand</returns>
        [OperationContract]
        decimal GetInventoryOnHand(RecordIdentifier itemID, RecordIdentifier storeID, LogonInfo logonInfo);

        /// <summary>
        /// Get inventory on hand for the specified item ID and store ID
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Inventory on hand</returns>
        [OperationContract]
        decimal GetInventoryOnHandForAssemblyItem(RecordIdentifier itemID, RecordIdentifier storeID, LogonInfo logonInfo);

        /// <summary>
        /// Save the inventory journal transaction
        /// </summary>
        /// <param name="ijTransaction">The inventory journal transaction to be saved</param>
        /// <param name="logonInfo">Login credentials</param>
        [OperationContract]
        void SaveJournalTransaction(InventoryJournalTransaction ijTransaction, LogonInfo logonInfo);

        /// <summary>
        /// Posts multiple stock counting lines
        /// </summary>
        /// <param name="ijTransactions">The lines to post</param>
        /// <param name="storeId">The store the lines are for</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Status and indicates whether the journal still has unposted lines</returns>
        [OperationContract]
        PostStockCountingLinesContainer PostMultipleStockCountingLines(List<InventoryJournalTransaction> ijTransactions, RecordIdentifier storeId, LogonInfo logonInfo);

        /// <summary>
        /// Post all lines from a stock counting journal
        /// </summary>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        PostStockCountingLinesContainer PostAllStockCountingLines(RecordIdentifier stockCountingJournalID, LogonInfo logonInfo);

        /// <summary>
        /// Post all lines from a stock counting journal asynchronously
        /// </summary>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <param name="callback">The callback method to be called when the operation is done</param>
        /// <param name="asyncState">User state (unique task identifier) used for asynchronous operations</param>
        /// <returns>Result of the operation</returns>
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginAsyncPostAllStockCountingLines(RecordIdentifier stockCountingJournalID, LogonInfo logonInfo, AsyncCallback callback, object asyncState);
        PostStockCountingLinesContainer EndAsyncPostAllStockCountingLines(IAsyncResult asyncResult);

        /// <summary>
        /// Returns true if the inventory adjustment with the provided ID exists
        /// </summary>
        /// <param name="stockCountingID">The stock counting ID to be checked</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Returns true if the inventory adjustment with the provided ID exists</returns>
        [OperationContract]
        bool InventoryAdjustmentExists(RecordIdentifier stockCountingID, LogonInfo logonInfo);

        [OperationContract]
        decimal GetSumofOrderedItembyStore(
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            bool includeReportFormatting,
            RecordIdentifier inventoryUnitId, 
            LogonInfo logonInfo);

        /// <summary>
        /// Create a new stock counting journal and copy all lines from an existing journal
        /// </summary>
        /// <param name="logonInfo">Login credentials</param>
        /// <param name="storeID">Store ID for the new journal</param>
        /// <param name="description">Description of the new journal</param>
        /// <param name="existingAdjustmentID">Existing journal from which to copy the lines</param>
        /// <returns>Status and ID of the created journal</returns>
        [OperationContract]
        CreateStockCountingContainer CreateStockCountingFromExistingAdjustment(LogonInfo logonInfo, RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID);

        /// <summary>
        /// Create a new stock counting journal and copy all lines from an existing journal
        /// </summary>
        /// <param name="logonInfo">Login credentials</param>
        /// <param name="storeID">Store ID for the new journal</param>
        /// <param name="description">Description of the new journal</param>
        /// <param name="filter">Container with desired item filters</param
        /// <returns>Status and ID of the created journal</returns>
        [OperationContract]
        CreateStockCountingContainer CreateStockCountingFromFilter(LogonInfo logonInfo, RecordIdentifier storeID, string description, InventoryTemplateFilterContainer filter);

        /// <summary>
        /// Create a new stock counting journal based on a given template
        /// </summary>
        /// <param name="logonInfo">Login credentials</param>
        /// <param name="template">The stock counting template</param>
        /// <returns>Status and ID of the created journal</returns>
        [OperationContract]
        CreateStockCountingContainer CreateStockCountingFromTemplate(LogonInfo logonInfo, TemplateListItem template);

        /// <summary>
		/// Create a new stock counting journal based on a given template
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="data">Data to import, parsed from excel file</param>
		/// <returns>A list of import results</returns>
        [OperationContract]
        List<ImportLogItem> ImportStockCountingFromExcel(LogonInfo logonInfo, DataTable data);

        /// <summary>
        /// Compresses all lines on a stock counting journal, that have the same item and unit
        /// </summary>
        /// <param name="logonInfo">Log-on information</param>
        /// <param name="stockCountingID">Stock counting journal id</param>
        /// <returns>Operation result</returns>
        [OperationContract]
        CompressAdjustmentLinesResult CompressAllStockCountingLines(LogonInfo logonInfo, RecordIdentifier stockCountingID);
    }
}