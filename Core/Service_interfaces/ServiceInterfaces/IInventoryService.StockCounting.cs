using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService
	{
        /// <summary>
        /// Get stock counting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">The journal ID to be get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>The inventory adjustment</returns>
		InventoryAdjustment GetStockCounting(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier journalId,
			bool closeConnection
			);

        /// <summary>
        /// Deletes a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">The stock counting ID to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
		DeleteJournalResult DeleteStockCounting(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier stockCountingID,
			bool closeConnection
			);

		/// <summary>
		/// Searches for Stock  counting entries 
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
        /// <param name="filter">Filter settings container</param>
		/// <param name="totalRecordsMatching">out: how many rows there are in total</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> SearchJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransactionFilter filter, out int totalRecordsMatching, bool closeConnection);

		/// <summary>
		/// Updates the on hand inventory of all journal transactions in a stock counting journal 
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="journalID">Entry of the stock counting journal</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		void RefreshStockCountingJournalInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalID, bool closeConnection);

		/// <summary>
		/// Deletes multiple inventory journal transaction lines
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
		/// <param name="IDs">The IDs of the inventory journal to delete</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>       
		/// <returns>Operation result</returns>
		DeleteJournalTransactionsResult DeleteMultipleJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> IDs, bool closeConnection);

		/// <summary>
		/// Saves a stock counting line
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="ijTransaction">The line to save</param>
		void SaveJournalTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
			 InventoryJournalTransaction ijTransaction, bool closeConnection);

        /// <summary>
        /// Posts multiple stock counting lines
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ijTransactions">The lines to post</param>
        /// <param name="storeId">The store the lines are for</param>
        /// <returns>Status and indicates whether the journal still has unposted lines</returns>
        PostStockCountingLinesContainer PostMultipleStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
		   List<InventoryJournalTransaction> ijTransactions, RecordIdentifier storeId, bool closeConnection);

        /// <summary>
        /// Post all lines from a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        PostStockCountingLinesContainer PostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID, bool closeConnection);

        /// <summary>
        /// Post all lines from a stock counting journal asynchronously
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <returns>Result of the operation</returns>
        Task<PostStockCountingLinesContainer> AsyncPostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID);

        /// <summary>
        /// Get inventory journal transaction by ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalTransactionId">The journal transaction ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Returns the inventory journal transaction for the supplied ID</returns>
		InventoryJournalTransaction GetInventoryJournalTransaction(IConnectionManager entry,
			SiteServiceProfile siteServiceProfile, RecordIdentifier journalTransactionId, bool closeConnection);

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
		CreateStockCountingContainer CreateStockCountingFromExistingAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID, bool closeConnection);

		/// <summary>
		/// Create a new stock counting journal from an filter
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="storeID">Store ID for the new journal</param>
		/// <param name="description">Description of the new journal</param>
		/// <param name="filter">Container with desired item filters</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created journal</returns>
		CreateStockCountingContainer CreateStockCountingFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, InventoryTemplateFilterContainer filter, bool closeConnection);

		/// <summary>
		/// Create a new stock counting journal based on a given template.
		/// </summary>
		/// <param name="entry">The entry into the database.</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation.</param>
		/// <param name="template">The stock counting template.</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created journal.</returns>
		CreateStockCountingContainer CreateStockCountingFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateListItem template, bool closeConnection);

		/// <summary>
		/// Compresses all lines on a stock counting journal, that have the same item and unit
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="stockCountingID">Stock counting journal id</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Operation result</returns>
		CompressAdjustmentLinesResult CompressAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingID, bool closeConnection);
	}
}