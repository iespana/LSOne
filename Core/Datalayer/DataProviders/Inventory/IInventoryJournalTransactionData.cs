using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Inventory
{
	public interface IInventoryJournalTransactionData : IDataProvider<InventoryJournalTransaction>, ISequenceable
	{
		/// <summary>
		/// Updates inventory on hand for all items in a stock counting journal
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalTransactionId">A row from the header table.</param>
		void RefreshStockCountingJournalInventoryOnHand(IConnectionManager entry, RecordIdentifier journalTransactionId);

		/// <summary>
		/// Gets all InventJournalTrans lines for a certain InventJournalTable row
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalTransactionId">A row from the header table.</param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortedBackwards">Whether to sort the result backwards or not</param>
		/// <param name="noExcludedInventoryItems"></param>
		/// <returns></returns>
		List<InventoryJournalTransaction> GetJournalTransactionList(IConnectionManager entry, RecordIdentifier journalTransactionId, 
			InventoryJournalTransactionSorting sortBy, bool sortedBackwards, bool noExcludedInventoryItems);

        /// <summary>
        /// Gets all InventJournalTrans lines for a certain InventJournalTable row
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="journalTransactionId">A row from the header table</param>
        /// <returns></returns>
		List<InventoryJournalTransaction> GetJournalTransactionList(IConnectionManager entry, RecordIdentifier journalTransactionId);

		/// <summary>
		/// Returns the number of transaction lines in the given inventory journal.
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalID">Inventory journal identifier.</param>
		/// <param name="countInventoryExcludedItems">If true, lines that are excluded from inventory (like deleted or service items) will also be counted. By default they're not counted.</param>
		/// <returns>The number of transaction lines in the inventory journal or null if an error is encountered.</returns>
		/// <remarks>This method is guaranteed to not throw any exceptions but instead it returns null.</remarks>
		int? Count(IConnectionManager entry, RecordIdentifier journalID, bool countInventoryExcludedItems = false);

        /// <summary>
        /// Gets all posted InventJournalTrans lines for a certain InventJournalTable row and variant item
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="journalTransactionId">A row from the header table</param>
        /// <param name="itemID">The ID of the item to get the lines for</param>
        /// <param name="variantName">The variant name to get the lines for</param>
        /// <param name="storeID">The ID of the store</param>
        /// <returns></returns>
		List<InventoryJournalTransaction> GetPostedJournalTransactionsForTransaction(IConnectionManager entry,
			RecordIdentifier journalTransactionId, RecordIdentifier itemID, string variantName,
			RecordIdentifier storeID);

        /// <summary>
        /// Gets the total number of reserved items by store in the specified inventory unit
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="itemID">The ID of the item to get the sum for</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="inventoryUnitID">The inventory unit to convert to</param>
        /// <param name="journalType">The type of journal to get the sum for</param>
        /// <returns></returns>
		decimal GetSumOfReservedItemByStore(IConnectionManager entry,
			RecordIdentifier itemID,
			RecordIdentifier storeID,
			RecordIdentifier inventoryUnitID,
			InventoryJournalTypeEnum journalType
			);

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object used in adjustments, reserved and parked inventory</see>
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="searchCriteria">Object containing search criterias</param>
		/// <param name="totalRecords">Total records matching the search criteria</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> AdvancedSearch(IConnectionManager entry, InventoryJournalLineSearch searchCriteria, out int totalRecords);

		/// <summary>
		/// Search inventory journals based on filters used in stock counting operations
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="filter">Object containing search criterias</param>
		/// <param name="totalRecordsMatching">Total records matching the search criteria</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> SearchJournalTransactions(IConnectionManager entry, InventoryJournalTransactionFilter filter, out int totalRecordsMatching);

        /// <summary>
        /// Gets a single inventory journal transaction
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="journalTransactionId">The ID of the journal to get</param>
        /// <returns></returns>
		InventoryJournalTransaction Get(IConnectionManager entry, RecordIdentifier journalTransactionId);

        /// <summary>
        /// Checks if a journal has unposted lines
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="id">The ID of the journal to check</param>
        /// <returns>True if the journal has any unposted lines, false otherwise</returns>
		bool JournalHasUnPostedLines(IConnectionManager entry, RecordIdentifier id);

		/// <summary>
		/// Post all lines from a stock counting journal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
		/// <returns>Result of the operation</returns>
		PostStockCountingResult PostAllStockCountingLines(IConnectionManager entry, RecordIdentifier stockCountingJournalID);

		/// <summary>
		/// Copies lines from one stock counting journal to another
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="fromStockCountingJournalID">Stock counting journal id used to copy the lines.</param>
		/// <param name="toStockCountingJournalID">Stock counting journal id to which the lines should be copied</param>
		/// <param name="storeID">Store ID used by the destination journal</param>
		void CopyStockCountingJournalLines(IConnectionManager entry, RecordIdentifier fromStockCountingJournalID, RecordIdentifier toStockCountingJournalID, RecordIdentifier storeID);

		/// <summary>
		/// Creates stock counting lines based on a filter
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="stockCountingJournalID">Stock counting journal id for which to create the lines.</param>
		/// <param name="storeID">Store ID used by the destination journal</param>
		/// <param name="filter">Filter container</param>
		void CreateStockCountingJournalLinesFromFilter(IConnectionManager entry, RecordIdentifier stockCountingJournalID, RecordIdentifier storeID, InventoryTemplateFilterContainer filter);

		/// <summary>
		/// Imports stock counting lines from an xml file
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="xmlData">XML data to import</param>
		/// <param name="languageCode">Language code used for parsing the xml (ex. en-US). This is required because in some languages (ex. Icelandic) the default decimal separator is a comma and xml interprets it as a string instead of a numeric</param>
		/// <returns>Number of inserted rows</returns>
		int ImportStockCountingLinesFromXML(IConnectionManager entry, string xmlData, string languageCode);

		/// <summary>
		/// Saves multiple inventory journal lines
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="inventoryJournalTransactions">List of journal lines to be saved</param>
		void SaveMultipleLines(IConnectionManager entry, List<InventoryJournalTransaction> inventoryJournalTransactions);

		/// <summary>
		/// Deletes multiple inventory journal transaction lines
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="IDs">The IDs of the inventory journal to delete</param>
		void DeleteMultipleLines(IConnectionManager entry, List<RecordIdentifier> IDs);

		/// <summary>
		/// Updates the journal line status (Open, Posted or Partially Posted) and, if needed, the line's master id.
		/// </summary>
		/// <param name="entry">The entry connection into the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineNum">Curent line id</param>
		/// <param name="masterId">Current line master id (if available)</param>
		/// <param name="newStatus">The new status</param>
		/// <returns>The <see cref="Guid">MasterID</see> of the updated journal line</returns>
		/// <remarks>Used by the Parked Inventory > Move to Inventory functionality</remarks>
		RecordIdentifier UpdateStatus(IConnectionManager entry,
									RecordIdentifier journalId,
									RecordIdentifier lineNum,
									RecordIdentifier masterId,
									InventoryJournalStatus newStatus);

		/// <summary>
		/// Returns the already moved-2-inventory items for a given parked journal line
		/// </summary>
		/// <param name="entry">The entry connection into the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>		
		/// <param name="lineMasterId">The journal line Master id( of type <see cref="Guid"/>) </param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> GetMovedToInventoryLines(IConnectionManager entry,
																	RecordIdentifier journalId,
																	RecordIdentifier lineMasterId,
																	InventoryJournalTransactionSorting sortBy,
																	bool sortBackwards);

        /// <summary>
        /// Updates a single journal line with a picture ID based on the transaction ID and line IDs from the mobile inventory app
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
        /// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
        /// <param name="pictureID">The ID of the picture to set on the line</param>
        [LSOneUsage(CodeUsage.LSCommerce)]
        void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID);

    }
}