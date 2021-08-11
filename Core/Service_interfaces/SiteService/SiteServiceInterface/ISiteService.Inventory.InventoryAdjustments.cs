using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
	public partial interface ISiteService
	{
		[OperationContract]
		void ReserveStockTransaction(
			LogonInfo logonInfo,
			InventoryJournalTransaction reservation,
			RecordIdentifier storeID,
			InventoryTypeEnum typeOfTransactionLine);

		[OperationContract]
		void ReserveStockItem(
			LogonInfo logonInfo,
			RecordIdentifier itemID,
			RecordIdentifier storeID,
			decimal adjustment,
			InventoryTypeEnum inventoryType,
			decimal costPrice,
			decimal netSalesPricePerItem,
			decimal salesPricePerItem,
			string unitID,
			decimal adjustmentInventoryUnit,
			RecordIdentifier reasonCode,
			string reasonText,
			string reference);

		[OperationContract]
		InventoryAdjustment GetInventoryAdjustment(
			LogonInfo logonInfo,
			RecordIdentifier masterID);

		[OperationContract]
		List<InventoryAdjustment> GetInventoryAdjustmentJournalList(
			LogonInfo logonInfo,
			RecordIdentifier storeId,
			InventoryJournalTypeEnum journalType,
			int journalStatus,
			InventoryAdjustmentSorting sortBy,
			bool sortedBackwards);

		[OperationContract]
		List<InventoryJournalTransaction> GetJournalTransactionList(
			LogonInfo logonInfo,
			RecordIdentifier journalTransactionId,
			InventoryJournalTransactionSorting sortBy,
			bool sortedBackwards,
			bool noExcludedInventoryItems);

		[OperationContract]
		void DeleteInventoryAdjustmentReason(LogonInfo logonInfo, RecordIdentifier reasonID);

		[OperationContract]
		bool InventoryAdjustmentReasonIsInUse(LogonInfo logonInfo, RecordIdentifier reasonID);

		[OperationContract]
		DataEntity GetJournalTransactionReasonById(LogonInfo logonInfo, RecordIdentifier id);

		[OperationContract]
		InventoryAdjustment SaveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal);

		/// <summary>
		/// Deletes an inventory journal only if it's empty (has no lines).
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">The id of the inventory journal that should be deleted</param>
		/// <returns>False if the journal has lines or an error occurred when deleting it. True otherwise.</returns>
		[OperationContract]
		bool DeleteInventoryJournal(LogonInfo logonInfo, RecordIdentifier journalId);

		[OperationContract]
		List<ReasonCode> GetReasonCodes(LogonInfo logonInfo, InventoryJournalTypeEnum inventoryType);

		[OperationContract]
		RecordIdentifier SaveInventoryAdjustmentReasonReturnID(LogonInfo logonInfo, DataEntity inventoryAdjustmentReason);

		[OperationContract]
		void SaveInventoryAdjustmentReason(LogonInfo logonInfo, DataEntity inventoryAdjustmentReason);

		[OperationContract]
		void UpdateReasonCodes(LogonInfo logonInfo, List<ReasonCode> reasonCodes, InventoryJournalTypeEnum inventoryType);

		[OperationContract]
		void CloseInventoryAdjustment(LogonInfo logonInfo, RecordIdentifier journalId);

		[OperationContract]
		void MoveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal);

		/// <summary>
		/// Saves an inventory adjustment line that is attached to a inventory journal transaction
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="inventoryJournalTrans">The inventory adjustment journal</param>
		/// <param name="storeId">The store the adjustment should be applied to</param>
		/// <param name="typeOfAdjustmentLine">What type of inventory adjustment is it</param>
		[OperationContract]
		void PostInventoryAdjustmentLine(LogonInfo logonInfo, 
										InventoryJournalTransaction inventoryJournalTrans,
										RecordIdentifier storeId, 
										InventoryTypeEnum typeOfAdjustmentLine);

		/// <summary>
		/// Paginated search through inventory journals based on the journal type and given <see cref="InventoryJournalSearch">search criteria object</see>.
		/// </summary>
		/// <param name="logonInfo"></param>
		/// <param name="journalType"></param>
		/// <param name="searchCriteria"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortBackwards"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		[OperationContract(Name = "InventoryJournalsAdvancedSearch")]
		List<InventoryAdjustment> AdvancedSearch(LogonInfo logonInfo,
												InventoryJournalTypeEnum journalType,
												InventoryJournalSearch searchCriteria,
												InventoryAdjustmentSorting sortBy,
												bool sortBackwards,
												int rowFrom,
												int rowTo,
												out int totalRecords);

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object used in adjustments, reserved and parked inventory</see>.
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="searchCriteria"></param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		[OperationContract(Name = "InventoryJournalAdvancedSearch")]
		List<InventoryJournalTransaction> AdvancedSearch(LogonInfo logonInfo, InventoryJournalLineSearch searchCriteria, out int totalRecords);

		/// <summary>
		/// Updates the journal line status (Open, Posted or Partially Posted) and, if needed, the line's master id.
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineNum">Curent line id</param>
		/// <param name="masterId">Current line master id (if available)</param>
		/// <param name="newStatus">The new status</param>
		/// <returns>The <see cref="Guid">MasterID</see> of the updated journal line</returns>
		/// <remarks>Used by the Parked Inventory > Move to Inventory functionality</remarks>
		[OperationContract]
		RecordIdentifier UpdateStatus(LogonInfo logonInfo,
									RecordIdentifier journalId,
									RecordIdentifier lineNum,
									RecordIdentifier masterId,
									InventoryJournalStatus newStatus);

		/// <summary>
		/// Returns the already moved-2-inventory items for a given parked journal line
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineMasterId">The journal line Master id( of type <see cref="Guid"/>) </param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		[OperationContract]
		List<InventoryJournalTransaction> GetMovedToInventoryLines(LogonInfo logonInfo,
																	RecordIdentifier journalId,
																	RecordIdentifier lineMasterId,
																	InventoryJournalTransactionSorting sortBy,
																	bool sortBackwards);

		/// <summary>
		/// Generates a new inventory adjustment ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <returns>New inventory adjustment ID</returns>
		[OperationContract]
		RecordIdentifier GenerateInventoryAdjustmentID(LogonInfo logonInfo);
    }
}
