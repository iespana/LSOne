using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.Services.Interfaces
{
	public partial interface IInventoryService
	{

		/// <summary>
		/// Saves an inventory adjustment line that is attached to a inventory journal transaction
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="inventoryJournalTrans">The inventory adjustment journal</param>
		/// <param name="storeId">The store the adjustment should be applied to</param>
		/// <param name="typeOfAdjustmentLine">What type of inventory adjustment is it</param>
		void PostInventoryAdjustmentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransaction inventoryJournalTrans,
					RecordIdentifier storeId, InventoryTypeEnum typeOfAdjustmentLine, bool closeConnection);

		/// <summary>
		/// Get a list of journals based on a filter
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="filter">Filter container</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="totalRecordsMatching">Total number of records that matched the search criteria</param>
		/// <returns></returns>
		List<InventoryAdjustment> GetJournalListAdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustmentFilter filter, bool closeConnection, out int totalRecordsMatching);

		/// <summary>
		/// Get the current processing status for a journal
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="journalId">ID fo the journal to check</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
		AdjustmentStatus GetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, bool closeConnection);

		/// <summary>
		/// Set the processing status for a journal
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="journalId">ID fo the journal to set</param>
		/// <param name="status">The status to set</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
		void SetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, InventoryProcessingStatus status, bool closeConnection);

		/// <summary>
		/// Checks if an inventory adjustment exists
		/// </summary> 
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="stockCountingID">The ID of the document</param>
		/// <returns></returns>
		bool InventoryAdjustmentExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingID, bool closeConnection);

		/// <summary>
		/// Deletes an inventory journal only if it's empty (has no lines).
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation.</param>
		/// <param name="entry">The entry into the database.</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open.</param>
		/// <param name="journalId">The id of the inventory journal that should be deleted.</param>
		/// <returns>False if the journal has lines or an error occurred when deleting it. True otherwise.</returns>
		bool DeleteInventoryJournal(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, bool closeConnection);

		/// <summary>
		/// Paginated search through inventory journal based on the given <see cref="InventoryJournalSearch">search criteria object</see>
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="journalType"></param>
		/// <param name="searchCriteria"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortBackwards"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		List<InventoryAdjustment> AdvancedSearch(
			IConnectionManager entry, SiteServiceProfile siteServiceProfile,
			InventoryJournalTypeEnum journalType,
			InventoryJournalSearch searchCriteria,
			InventoryAdjustmentSorting sortBy,
			bool sortBackwards,
			int rowFrom,
			int rowTo,
			out int totalRecords,
			bool closeConnection);

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object used in adjustments, reserved and parked inventory</see>
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="searchCriteria">Object containing search criterias</param>
		/// <param name="totalRecords"></param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> AdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalLineSearch searchCriteria, out int totalRecords, bool closeConnection);

		/// <summary>
		/// Updates the journal line status (Open, Posted or Partially Posted) and, if needed, the line's master id.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineNum">Curent line id</param>
		/// <param name="masterId">Current line master id (if available)</param>
		/// <param name="newStatus">The new status</param>
		/// <returns>The <see cref="Guid">MasterID</see> of the updated journal line</returns>
		/// <remarks>Used by the Parked Inventory > Move to Inventory functionality</remarks>
		RecordIdentifier UpdateStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
									RecordIdentifier journalId,
									RecordIdentifier lineNum,
									RecordIdentifier masterId,
									InventoryJournalStatus newStatus,
									bool closeConnection);

		/// <summary>
		/// Returns the already moved-2-inventory items for a given parked journal line
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineMasterId">The journal line Master id( of type <see cref="Guid"/>) </param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		List<InventoryJournalTransaction> GetMovedToInventoryLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
																	RecordIdentifier journalId,
																	RecordIdentifier lineMasterId,
																	InventoryJournalTransactionSorting sortBy,
																	bool sortBackwards,
																	bool closeConnection);
	}
}
