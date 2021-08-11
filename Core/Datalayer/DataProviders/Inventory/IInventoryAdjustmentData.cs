using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryAdjustmentData : IDataProvider<InventoryAdjustment>, ISequenceable
    {
        /// <summary>
        /// Gets a list of DataEntity that contains JOURNALID and Description only (for usage in ComboBoxes). The list is sorted by the column specified in the parameter 'sort'.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains the Journal Id  and its description</returns>
        List<DataEntity> GetList(IConnectionManager entry, InventoryAdjustmentSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of DataEntity that contains JOURNALID and Description. The list is sorted by JOURNALID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains JOURNALID and Description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of journals based on a filter
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="filter">Filter container</param>
        /// <param name="totalRecordsMatching">Total number of records that matched the search criteria</param>
        /// <returns></returns>
        List<InventoryAdjustment> GetJournalListAdvanced(IConnectionManager entry, InventoryAdjustmentFilter filter, out int totalRecordsMatching);

        /// <summary>
        /// Gets the complete Journal list
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="storeId">Store Id filter for result. Pass in RecordIdentifier.Empty to get for all stores</param>
        /// <param name="journalType">The journal type to fetch</param>
        /// <param name="sortBy">The sort parameter</param>
        /// <param name="sortedBackwards">True, if sorted backwards</param>
        /// <param name="journalStatus"></param>
        /// <returns>All existing journal rows</returns>        
        List<InventoryAdjustment> GetJournalList(IConnectionManager entry, 
            RecordIdentifier storeId, 
            InventoryJournalTypeEnum journalType,
            int journalStatus,
            InventoryAdjustmentSorting sortBy, 
            bool sortedBackwards);

        /// <summary>
        /// Marks an inventory adjustment as posted
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="journalId"></param>
        void PostAdjustment(IConnectionManager entry, RecordIdentifier journalId);

        /// <summary>
        /// Call a stored procedure to post an adjustment if there are no more unposted lines
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="journalId">ID of the journal to post</param>
        /// <returns>Result of the operation</returns>
        PostStockCountingResult PostAdjustmentWithCheck(IConnectionManager entry, RecordIdentifier journalId);

        /// <summary>
        /// Set the processing status of an adjustment
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="journalId">ID of the journal to set</param>
        /// <param name="status">Processing status</param>
        void SetAdjustmentProcessingStatus(IConnectionManager entry, RecordIdentifier journalId, InventoryProcessingStatus status);

        /// <summary>
        /// Get the current processing status for a journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="journalId">ID fo the journal to check</param>
        /// <returns></returns>
        AdjustmentStatus GetAdjustmentStatus(IConnectionManager entry, RecordIdentifier journalId);

        InventoryAdjustment Get(IConnectionManager entry, RecordIdentifier ID);

        /// <summary>
        /// Paginated search through inventory journal based on the given <see cref="InventoryJournalSearch">search criteria object</see>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="journalType"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="sortedBy"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        List<InventoryAdjustment> AdvancedSearch(
            IConnectionManager entry,
            InventoryJournalTypeEnum journalType,
            InventoryJournalSearch searchCriteria,
            InventoryAdjustmentSorting sortBy,
            bool sortBackwards,
            int rowFrom,
            int rowTo,
            out int totalRecords);

        /// <summary>
        /// Get the latest stock counting journal created by the omni mobile app and was not yet posted.
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="templateID">The template ID used for creating the stock counting journal.</param>
        /// <param name="storeID">The store ID in which the journal was created.</param>
        /// <returns>Return a stock counting adjustment</returns>
        InventoryAdjustment GetOmniInventoryAdjustmentByTemplate(IConnectionManager entry, RecordIdentifier templateID, RecordIdentifier storeID);

        /// <summary>
        /// Check if a journal is partially posted
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="journalID">The journal ID to check</param>
        /// <returns>True if the journal has any posted lines</returns>
        bool JournalIsPartiallyPosted(IConnectionManager entry, RecordIdentifier journalID);

        /// <summary>
        /// Compresses all lines on a stock counting journal, that have the same item and unit
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="journalID">Stock counting journal id</param>
        /// <returns>Operation result</returns>
        CompressAdjustmentLinesResult CompressAllStockCountingLines(IConnectionManager entry, RecordIdentifier journalID);
    }
}