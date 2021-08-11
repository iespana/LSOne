using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using System.Threading.Tasks;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        /// <summary>
        /// Searches for Stock counting entries 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="filter">Filter settings container</param>
        /// <param name="totalRecordsMatching">out: how many rows there are in total</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>The list of inventory journal transactions</returns>
        public virtual List<InventoryJournalTransaction> SearchJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransactionFilter filter, out int totalRecordsMatching,  bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SearchJournalTransactions(entry, siteServiceProfile, filter, out totalRecordsMatching, closeConnection);
        }

        /// <summary>
        /// Updates the on hand inventory of all journal transactions in a stock counting journal 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalID">Entry of the stock counting journal</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void RefreshStockCountingJournalInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).RefreshStockCountingJournalInventoryOnHand(entry, siteServiceProfile, journalID, closeConnection);
        }

        /// <summary>
        /// Get stock counting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">The journal ID to be get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>The inventory adjustment</returns>
        public virtual InventoryAdjustment GetStockCounting(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalId,
            bool closeConnection
            )
        {
            return Interfaces.Services.SiteServiceService(entry).GetStockCounting(entry, siteServiceProfile, journalId, closeConnection);
        }

        /// <summary>
        /// Get a list of journals based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="filter">Filter container</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="totalRecordsMatching">Total number of records that matched the search criteria</param>
        /// <returns></returns>
        public virtual List<InventoryAdjustment> GetJournalListAdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustmentFilter filter, bool closeConnection, out int totalRecordsMatching)
        {
            return Interfaces.Services.SiteServiceService(entry).GetJournalListAdvancedSearch(entry, siteServiceProfile, filter, closeConnection, out totalRecordsMatching);
        }

        /// <summary>
        /// Get the current processing status for a journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">ID fo the journal to check</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual AdjustmentStatus GetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetAdjustmentStatus(entry, siteServiceProfile, journalId, closeConnection);
        }

        /// <summary>
        /// Set the processing status for a journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">ID fo the journal to set</param>
        /// <param name="status">The status to set</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual void SetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, InventoryProcessingStatus status, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SetAdjustmentStatus(entry, siteServiceProfile, journalId, status, closeConnection);
        }

        /// <summary>
        /// Deletes a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">The stock counting ID to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        public virtual DeleteJournalResult DeleteStockCounting(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier stockCountingID,
            bool closeConnection
            )
        {
            return Interfaces.Services.SiteServiceService(entry).DeleteStockCounting(entry, siteServiceProfile, stockCountingID, closeConnection);
        }

        /// <summary>
        /// Deletes multiple inventory journal transaction lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="IDs">The IDs of the inventory journal to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual DeleteJournalTransactionsResult DeleteMultipleJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> IDs, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).DeleteMultipleJournalTransactions(entry, siteServiceProfile, IDs, closeConnection);
        }

        /// <summary>
        /// Posts multiple stock counting lines
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ijTransactions">The lines to post</param>
        /// <param name="storeId">The store the lines are for</param>
        /// <returns>Status and indicates whether the journal still has unposted lines</returns>
        public virtual PostStockCountingLinesContainer PostMultipleStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
           List<InventoryJournalTransaction> ijTransactions, RecordIdentifier storeId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PostMultipleStockCountingLines(entry, siteServiceProfile, ijTransactions, storeId, closeConnection);
        }

        /// <summary>
        /// Post all lines from a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        public virtual PostStockCountingLinesContainer PostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PostAllStockCountingLines(entry, siteServiceProfile, stockCountingJournalID, closeConnection);
        }

        /// <summary>
        /// Post all lines from a stock counting journal asynchronously
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <returns>Result of the operation</returns>
        public virtual async Task<PostStockCountingLinesContainer> AsyncPostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID)
        {
            return await Interfaces.Services.SiteServiceService(entry).AsyncPostAllStockCountingLines(entry, siteServiceProfile, stockCountingJournalID);
        }

        /// <summary>
        /// Checks if an inventory adjustment exists
        /// </summary> 
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual bool InventoryAdjustmentExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier stockCountingID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).InventoryAdjustmentExists(entry, siteServiceProfile, stockCountingID, closeConnection);
        }

        /// <summary>
        /// Get inventory journal transaction by ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalTransactionId">The journal transaction ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Returns the inventory journal transaction for the supplied ID</returns>
        public virtual InventoryJournalTransaction GetInventoryJournalTransaction(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier journalTransactionId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryJournalTransaction(entry, siteServiceProfile, journalTransactionId, closeConnection);
        }

        /// <summary>
        /// Saves a stock counting line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="ijTransaction">The line to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void SaveJournalTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryJournalTransaction ijTransaction, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SaveJournalTransaction(entry, siteServiceProfile, ijTransaction, closeConnection);
        }

        /// <summary>
		/// Create a new stock counting journal and copy all lines from an existing journal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="storeID">Store ID for the new journal</param>
		/// <param name="description">Description of the new journal</param>
		/// <param name="existingAdjustmentID">Existing journal from which to copy the lines</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created journal</returns>
        public virtual CreateStockCountingContainer CreateStockCountingFromExistingAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).CreateStockCountingFromExistingAdjustment(entry, siteServiceProfile, storeID, description, existingAdjustmentID, closeConnection);
        }

        /// <summary>
        /// Create a new stock counting journal and copy all lines from an existing journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="storeID">Store ID for the new journal</param>
        /// <param name="description">Description of the new journal</param>
        /// <param name="filter">Container with desired item filters</param>
        public virtual CreateStockCountingContainer CreateStockCountingFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, InventoryTemplateFilterContainer filter, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).CreateStockCountingFromFilter(entry, siteServiceProfile, storeID, description, filter, closeConnection);
        }
 
        /// <summary>
        /// Create a new stock counting journal based on a given template
        /// </summary>
        /// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="template">The stock counting template</param>
        /// <returns>Status and ID of the created journal</returns>
        public virtual CreateStockCountingContainer CreateStockCountingFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateListItem template, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).CreateStockCountingFromTemplate(entry, siteServiceProfile, template, closeConnection);
        }

        /// <summary>
        /// Compresses all lines on a stock counting journal, that have the same item and unit
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">Stock counting journal id</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Operation result</returns>
        public virtual CompressAdjustmentLinesResult CompressAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingID, bool closeConnection)
        {
                return Interfaces.Services.SiteServiceService(entry).CompressAllStockCountingLines(entry, siteServiceProfile, stockCountingID, closeConnection);
        }
    }
}
