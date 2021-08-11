using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
	public partial class InventoryService
	{

		public virtual List<InventoryAdjustment> GetInventoryAdjustmentJournalList(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier storeId,
			InventoryJournalTypeEnum journalType,
			int journalStatus,
			InventoryAdjustmentSorting sortBy,
			bool sortedBackwards,
			bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetInventoryAdjustmentJournalList(entry, siteServiceProfile, storeId, journalType, journalStatus, sortBy,
				sortedBackwards, closeConnection);
		}

		public virtual List<InventoryJournalTransaction> GetJournalTransactionList(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier journalTransactionId,
			InventoryJournalTransactionSorting sortBy,
			bool sortedBackwards,
			bool noExcludedInventoryItems,
			bool closeConnection
)
		{
			return Interfaces.Services.SiteServiceService(entry).GetJournalTransactionList(entry, siteServiceProfile, journalTransactionId, sortBy, sortedBackwards, noExcludedInventoryItems, closeConnection);
		}

		public virtual DataEntity GetJournalTransactionReasonById(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier id,
			bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetJournalTransactionReasonById(entry, siteServiceProfile, id, closeConnection);
		}

		public virtual void CloseInventoryAdjustment(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			RecordIdentifier journalId,
			bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).CloseInventoryAdjustment(entry, siteServiceProfile, journalId, closeConnection);
		}


		public virtual void ReserveStockTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransaction reservation, RecordIdentifier storeID, InventoryTypeEnum typeOfTransactionLine, bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).ReserveStockTransaction(entry, siteServiceProfile, reservation, storeID, typeOfTransactionLine, closeConnection);
		}

		public virtual void ReserveStockItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID,
			RecordIdentifier storeID, decimal adjustment, InventoryTypeEnum inventoryType,
			decimal costPrice, decimal netSalesPricePerItem, decimal salesPricePerItem,
			string unitID, decimal adjustmentInventoryUnit, RecordIdentifier reasonCode, string reasonText, string reference, bool closeConnection)
		{

			Interfaces.Services.SiteServiceService(entry).ReserveStockItem(entry, siteServiceProfile, itemID,
				storeID, adjustment, inventoryType,
				costPrice, netSalesPricePerItem, salesPricePerItem,
				unitID, adjustmentInventoryUnit, reasonCode, reasonText, reference, closeConnection);
		}

		public virtual InventoryAdjustment SaveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).SaveInventoryAdjustment(entry, siteServiceProfile, adjustmentJournal, closeConnection);
		}

		public virtual InventoryAdjustment GetInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetInventoryAdjustment(entry, siteServiceProfile, masterID, closeConnection);
		}

		public virtual void MoveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).MoveInventoryAdjustment(entry, siteServiceProfile, adjustmentJournal, closeConnection);
		}

		/// <summary>
		/// Deletes an inventory journal only if it's empty (has no lines).
		/// </summary>
		/// <param name="entry">The entry into the database.</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation.</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open.</param>
		/// <param name="journalId">The id of the inventory journal that should be deleted.</param>
		/// <returns>False if the journal has lines or an error occurred when deleting it. True otherwise.</returns>
		public virtual bool DeleteInventoryJournal(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).DeleteInventoryJournal(entry, siteServiceProfile, journalId, closeConnection);
		}

		public virtual void SaveInventoryAdjustmentReason(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).SaveInventoryAdjustmentReason(entry, siteServiceProfile, inventoryAdjustmentReason, closeConnection);
		}

		public virtual RecordIdentifier SaveInventoryAdjustmentReasonReturnID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).SaveInventoryAdjustmentReasonReturnID(entry, siteServiceProfile, inventoryAdjustmentReason, closeConnection);
		}

		public virtual List<ReasonCode> GetReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTypeEnum inventoryType, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetReasonCodes(entry, siteServiceProfile, inventoryType, closeConnection);
		}

		public virtual void UpdateReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<ReasonCode> reasonCodes, InventoryJournalTypeEnum inventoryType, bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).UpdateReasonCodes(entry, siteServiceProfile, reasonCodes, inventoryType, closeConnection);
		}

		public virtual bool InventoryAdjustmentReasonIsInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).InventoryAdjustmentReasonIsInUse(entry, siteServiceProfile, reasonID, closeConnection);
		}

		public virtual void DeleteInventoryAdjustmentReason(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
		{
			Interfaces.Services.SiteServiceService(entry).DeleteInventoryAdjustmentReason(entry, siteServiceProfile, reasonID, closeConnection);
		}

		public virtual decimal GetSumOfReservedItemByStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
			RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier inventoryUnitID,
			InventoryJournalTypeEnum journalType, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetSumOfReservedItemByStore(entry, siteServiceProfile, itemID, storeID, inventoryUnitID, journalType, closeConnection);
		}

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
		public virtual List<InventoryAdjustment> AdvancedSearch(
			IConnectionManager entry, SiteServiceProfile siteServiceProfile,
			InventoryJournalTypeEnum journalType,
			InventoryJournalSearch searchCriteria,
			InventoryAdjustmentSorting sortBy,
			bool sortBackwards,
			int rowFrom,
			int rowTo,
			out int totalRecords,
			bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).AdvancedSearch(entry, siteServiceProfile,
																				journalType, 
																				searchCriteria, 
																				sortBy, 
																				sortBackwards, 
																				rowFrom, 
																				rowTo, 
																				out totalRecords, 
																				closeConnection);
		}

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object used in adjustments, reserved and parked inventory</see>
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="searchCriteria">Object containing search criterias</param>
		/// <param name="totalRecords"></param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> AdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalLineSearch searchCriteria, out int totalRecords, bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).AdvancedSearch(entry, siteServiceProfile, searchCriteria, out totalRecords, closeConnection);
		}

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
		public virtual RecordIdentifier UpdateStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
									RecordIdentifier journalId,
									RecordIdentifier lineNum,
									RecordIdentifier masterId,
									InventoryJournalStatus newStatus,
									bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).UpdateStatus(entry, siteServiceProfile,
																				journalId,
																				lineNum,
																				masterId,
																				newStatus,
																				closeConnection);
		}

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
		public virtual List<InventoryJournalTransaction> GetMovedToInventoryLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
																	RecordIdentifier journalId,
																	RecordIdentifier lineMasterId,
																	InventoryJournalTransactionSorting sortBy,
																	bool sortBackwards,
																	bool closeConnection)
		{
			return Interfaces.Services.SiteServiceService(entry).GetMovedToInventoryLines(entry, siteServiceProfile,
																						journalId,
																						lineMasterId,
																						sortBy,
																						sortBackwards,
																						closeConnection);
		}
	}
}
