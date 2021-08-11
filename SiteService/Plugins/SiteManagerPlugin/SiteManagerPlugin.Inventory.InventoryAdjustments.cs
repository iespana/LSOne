using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Plugins.SiteManager.Properties;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
		/// <summary>
		/// Gets the inventory adjustments for the given search criteria
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="storeId"></param>
		/// <param name="journalType"></param>
		/// <param name="journalStatus"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortedBackwards"></param>
		/// <returns></returns>
		public virtual List<InventoryAdjustment> GetInventoryAdjustmentJournalList(
			LogonInfo logonInfo,
			RecordIdentifier storeId,
			InventoryJournalTypeEnum journalType,
			int journalStatus,
			InventoryAdjustmentSorting sortBy,
			bool sortedBackwards)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(storeId)}: {storeId}, {nameof(journalType)}: {journalType}, {nameof(journalStatus)}: {journalStatus}, {nameof(sortBy)}: {sortBy}, {nameof(sortedBackwards)}: {sortedBackwards}");

				return Providers.InventoryAdjustmentData.GetJournalList(entry, storeId, journalType, journalStatus, sortBy, sortedBackwards);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Gets all journal transaction lines for the given journal ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalTransactionId"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortedBackwards"></param>
		/// <param name="noExcludedInventoryItems"></param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> GetJournalTransactionList(
			LogonInfo logonInfo,
			RecordIdentifier journalTransactionId,
			InventoryJournalTransactionSorting sortBy,
			bool sortedBackwards, bool noExcludedInventoryItems)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalTransactionId)}: {journalTransactionId}, {nameof(sortBy)}: {sortBy}, {nameof(sortedBackwards)}: {sortedBackwards}, {nameof(noExcludedInventoryItems)} : {noExcludedInventoryItems}");

				return Providers.InventoryJournalTransactionData.GetJournalTransactionList(entry, journalTransactionId, sortBy, sortedBackwards, noExcludedInventoryItems);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Gets the journal transaction reason for the given ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual DataEntity GetJournalTransactionReasonById(LogonInfo logonInfo, RecordIdentifier id)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(id)}: {id}");

				return Providers.ReasonCodeData.GetReasonById(entry, id);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Saves the inventory adjustment
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="adjustmentJournal"></param>
		/// <returns></returns>
		public virtual InventoryAdjustment SaveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, Utils.Starting + (adjustmentJournal.ID == RecordIdentifier.Empty 
													? " Creating inventory adjustment: " + adjustmentJournal.Text 
													: " Saving inventory adjustment: " + adjustmentJournal.Text + ", ID: " + adjustmentJournal.ID.StringValue));


                if (adjustmentJournal.Posted != InventoryJournalStatus.Posted)
                {
					adjustmentJournal.CreatedDateTime = DateTime.Now;
					adjustmentJournal.PostedDateTime = Date.Empty;
				}
				Providers.InventoryAdjustmentData.Save(entry, adjustmentJournal);

				return GetInventoryAdjustment(logonInfo, adjustmentJournal.MasterID);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Updates the inventory adjustment to be for a different store.
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="adjustmentJournal"></param>
		/// <remarks>
		/// Used in Customer order functionality when a customer order is being fulfilled at another store than originally it was configured to be fulfilled at.
		/// </remarks>
		public virtual void MoveInventoryAdjustment(LogonInfo logonInfo, InventoryAdjustment adjustmentJournal)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, Utils.Starting);

				List<InventoryJournalTransaction> transactions = Providers.InventoryJournalTransactionData.GetJournalTransactionList(entry, adjustmentJournal.MasterID);

				if (transactions != null && transactions.Count > 0)
				{
					foreach (InventoryJournalTransaction jt in transactions)
					{
						List<InventoryJournalTransaction> items = transactions.Where(w => w.ItemId == jt.ItemId && w.UnitID == jt.UnitID && w.InventoryUnitID == jt.InventoryUnitID).ToList();
						if (items.Sum(s => s.AdjustmentInInventoryUnit) != decimal.Zero)
						{
							//Save the adjustment to the new store id
							jt.Adjustment = items.Sum(s => s.Adjustment);
							jt.AdjustmentInInventoryUnit = items.Sum(s => s.AdjustmentInInventoryUnit);
							ReserveStockTransaction(logonInfo, jt, adjustmentJournal.StoreId, InventoryTypeEnum.Reservation);

							//Zero out the old adjustment
							jt.Adjustment *= -1;
							jt.AdjustmentInInventoryUnit *= -1;
							ReserveStockTransaction(logonInfo, jt, jt.StoreId, InventoryTypeEnum.Reservation);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Gets the inventory adjustment by the given ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="masterID"></param>
		/// <returns></returns>
		public virtual InventoryAdjustment GetInventoryAdjustment(LogonInfo logonInfo, RecordIdentifier masterID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(masterID)}: {masterID}");

				return Providers.InventoryAdjustmentData.Get(entry, masterID);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Closes the inventory adjustment with the given ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId"></param>
		public virtual void CloseInventoryAdjustment(LogonInfo logonInfo, RecordIdentifier journalId)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}");

				Providers.InventoryAdjustmentData.PostAdjustment(entry, journalId);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Deletes an inventory journal only if it's empty (has no lines).
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">The id of the inventory journal that should be deleted</param>
		/// <returns>False if the journal has lines or an error occurred when deleting it. True otherwise.</returns>
		public virtual bool DeleteInventoryJournal(LogonInfo logonInfo, RecordIdentifier journalId)
		{
			bool result = false;
			string message = string.Empty;

			if (journalId == null || RecordIdentifier.IsEmptyOrNull(journalId)) 
				return true;

			IConnectionManager entry = GetConnectionManager(logonInfo);
			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}");

				int? linesCount = Providers.InventoryJournalTransactionData.Count(entry, journalId);
				if(linesCount.HasValue && linesCount == 0)
				{
					Providers.InventoryAdjustmentData.Delete(entry, journalId);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}

			return result;
		}

		/// <summary>
		/// Creates an inventory adjustment with the type Reservation.
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="reservation"></param>
		/// <param name="storeID"></param>
		/// <param name="typeOfTransactionLine"></param>
		/// <remarks>
		/// Used in Customer order functionality when a customer order is created and the stock for the order should be reserved
		/// </remarks>
		public virtual void ReserveStockTransaction(LogonInfo logonInfo, InventoryJournalTransaction reservation, RecordIdentifier storeID, InventoryTypeEnum typeOfTransactionLine)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}, {nameof(typeOfTransactionLine)}: {typeOfTransactionLine}");

				PostInventoryAdjustmentLine(logonInfo, reservation, storeID, typeOfTransactionLine);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Reserves stock for the item with the given ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="itemID"></param>
		/// <param name="storeID"></param>
		/// <param name="adjustment"></param>
		/// <param name="inventoryType"></param>
		/// <param name="costPrice"></param>
		/// <param name="netSalesPricePerItem"></param>
		/// <param name="salesPricePerItem"></param>
		/// <param name="unitID"></param>
		/// <param name="adjustmentInventoryUnit"></param>
		/// <param name="reasonCode"></param>
		/// <param name="reasonText"></param>
		/// <param name="reference"></param>
		public virtual void ReserveStockItem(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID, decimal adjustment, InventoryTypeEnum inventoryType,
			decimal costPrice, decimal netSalesPricePerItem, decimal salesPricePerItem, string unitID, decimal adjustmentInventoryUnit, RecordIdentifier reasonCode, string reasonText, string reference)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, 
					$"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}, {nameof(adjustment)}: {adjustment}, " + 
					$"{nameof(inventoryType)}: {inventoryType}, {nameof(costPrice)}: {costPrice}, {nameof(netSalesPricePerItem)}: {netSalesPricePerItem}, " + 
					$"{nameof(unitID)}: {unitID}, {nameof(adjustmentInventoryUnit)}: {adjustmentInventoryUnit}, {nameof(reasonCode)}: {reasonCode}, {nameof(reasonText)}: {reasonText}, {nameof(reference)}: {reference}");

				if (!string.IsNullOrWhiteSpace(reasonCode.StringValue))
				{
					if (!Providers.ReasonCodeData.Exists(entry, reasonCode))
					{
						ReasonCode reason = new ReasonCode
						{
							ID = reasonCode,
							Text = reasonText,
							IsSystemReasonCode = false,
							ShowOnPos = true
						};

						Providers.ReasonCodeData.Save(entry, reason);
					}
				}

				var item = Providers.RetailItemData.Get(entry, itemID);

				InventoryTransaction inventTrans = new InventoryTransaction(itemID, item.ItemType);
				inventTrans.StoreID = storeID;
				inventTrans.Adjustment = adjustment;
				inventTrans.Type = inventoryType;
				inventTrans.CostPricePerItem = costPrice;
				inventTrans.SalesPriceWithoutTaxPerItem = netSalesPricePerItem;
				inventTrans.SalesPriceWithTaxPerItem = salesPricePerItem;
				inventTrans.InventoryUnitID = unitID;
				inventTrans.AdjustmentInInventoryUnit = adjustmentInventoryUnit;
				inventTrans.ReasonCode = reasonCode;
				inventTrans.Reference = reference;

				Providers.InventoryTransactionData.Save(entry, inventTrans, true);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Saves the inventory adjustment reason
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="inventoryAdjustmentReason"></param>
		public virtual void SaveInventoryAdjustmentReason(LogonInfo logonInfo, DataEntity inventoryAdjustmentReason)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(inventoryAdjustmentReason)}: ID = {inventoryAdjustmentReason.ID}, Text = {inventoryAdjustmentReason}");

				ReasonCode reason = new ReasonCode
				{
					ID = inventoryAdjustmentReason.ID,
					Text = inventoryAdjustmentReason.Text,
					IsSystemReasonCode = false,
					ShowOnPos = true
				};

				Providers.ReasonCodeData.Save(entry, reason);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Saves the inventory adjustment reason and returns its ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="inventoryAdjustmentReason"></param>
		/// <returns></returns>
		public virtual RecordIdentifier SaveInventoryAdjustmentReasonReturnID(LogonInfo logonInfo, DataEntity inventoryAdjustmentReason)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(inventoryAdjustmentReason)}: ID = {inventoryAdjustmentReason.ID}, Text = {inventoryAdjustmentReason}");

				ReasonCode reason = new ReasonCode
				{
					ID = inventoryAdjustmentReason.ID,
					Text = inventoryAdjustmentReason.Text,
					IsSystemReasonCode = false,
					ShowOnPos = true
				};

				Providers.ReasonCodeData.Save(entry, reason);
				//TO DO: CREATE METHOD TO RETURN ID IF STILL NECESSARY
				return RecordIdentifier.Empty;
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Updates the system reason codes if they have a different description than the default system one (which is in English).
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="reasonCodes"></param>
		/// <param name="inventoryType"></param>
		/// <remarks>
		/// The POS usually uses these reason codes and will update the descriptions to be in the language of the store so that the user can see the description in their language.
		/// </remarks>
		public virtual void UpdateReasonCodes(LogonInfo logonInfo, List<ReasonCode> reasonCodes, InventoryJournalTypeEnum inventoryType)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(inventoryType)}: {inventoryType}");

				if (inventoryType == InventoryJournalTypeEnum.Reservation)
				{
					ReasonCode toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
					if (toUpdate != null)
					{
						toUpdate.IsSystemReasonCode = true;
						toUpdate.ShowOnPos = true;

						Providers.ReasonCodeData.Save(entry, toUpdate);
					}

					toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
					if (toUpdate != null)
					{
						toUpdate.IsSystemReasonCode = true;
						toUpdate.ShowOnPos = true;

						Providers.ReasonCodeData.Save(entry, toUpdate);
					}

					toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID && f.Text != CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
					if (toUpdate != null)
					{
						toUpdate.IsSystemReasonCode = true;
						toUpdate.ShowOnPos = true;

						Providers.ReasonCodeData.Save(entry, toUpdate);
					}
				}
				else if (inventoryType == InventoryJournalTypeEnum.Transfer)
				{
					ReasonCode toUpdate = reasonCodes.FirstOrDefault(f => f.ID == MissingInTransferReasonConstants.ConstMissingInTransferReasonID && f.Text != MissingInTransferReasonConstants.ConstMissingInTransferReasonID);
					if (toUpdate != null)
					{
						toUpdate.IsSystemReasonCode = true;
						toUpdate.ShowOnPos = true;

						Providers.ReasonCodeData.Save(entry, toUpdate);
					}
				}
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Returns true if the inventory adjustment reason is is use; otherwise returns false
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="reasonID"></param>
		/// <returns></returns>
		public virtual bool InventoryAdjustmentReasonIsInUse(LogonInfo logonInfo, RecordIdentifier reasonID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(reasonID)}: {reasonID}");

				return Providers.ReasonCodeData.ReasonCodeIsInUse(entry, reasonID);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Deletes the inventory adjustment reason
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="reasonID"></param>
		public virtual void DeleteInventoryAdjustmentReason(LogonInfo logonInfo, RecordIdentifier reasonID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(reasonID)}: {reasonID}");

				Providers.ReasonCodeData.Delete(entry, reasonID);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Retrieves system reason codes used for customer orders, transfers and service items. 
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="inventoryType"></param>
		/// <returns></returns>
		/// <remarks>
		/// If no system reason codes exist then they are created and saved to the database. 
		/// This function creates the reason codes with a description in English. 
		/// Once the Site Manager or POS start using these reason codes the descriptions will be updated to be in the language of the system using UpdateReasonCodes function.
		/// </remarks>
		public virtual List<ReasonCode> GetReasonCodes(LogonInfo logonInfo, InventoryJournalTypeEnum inventoryType)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(inventoryType)}: {inventoryType}");

				List<ReasonCode> reasonCodes = Providers.ReasonCodeData.GetList(entry);

				if (inventoryType == InventoryJournalTypeEnum.Reservation)
				{
					ReasonCode reserveStockToOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
					if (reserveStockToOrder == null)
					{
						reserveStockToOrder = new ReasonCode
						{
							ID = CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID,
							Text = CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID,
							IsSystemReasonCode = true,
							ShowOnPos = false
						};

						Providers.ReasonCodeData.Save(entry, reserveStockToOrder);
					}

					ReasonCode voidStockFromOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
					if (voidStockFromOrder == null)
					{
						voidStockFromOrder = new ReasonCode
						{
							ID = CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID,
							Text = CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID,
							IsSystemReasonCode = true,
							ShowOnPos = false
						};

						Providers.ReasonCodeData.Save(entry, voidStockFromOrder);
					}

					ReasonCode pickupFromOrder = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
					if (pickupFromOrder == null)
					{
						pickupFromOrder = new ReasonCode
						{
							ID = CustomerOrderReasonConstants.ConstPickupFromOrderReasonID,
							Text = CustomerOrderReasonConstants.ConstPickupFromOrderReasonID,
							IsSystemReasonCode = true,
							ShowOnPos = false
						};

						Providers.ReasonCodeData.Save(entry, pickupFromOrder);
					}

					reasonCodes.Clear();
					reasonCodes.Add(reserveStockToOrder);
					reasonCodes.Add(voidStockFromOrder);
					reasonCodes.Add(pickupFromOrder);
				}
				else if (inventoryType == InventoryJournalTypeEnum.Transfer)
				{
					ReasonCode missingInTransfer = reasonCodes.FirstOrDefault(f => f.ID == MissingInTransferReasonConstants.ConstMissingInTransferReasonID);
					if (missingInTransfer == null)
					{
						missingInTransfer = new ReasonCode
						{
							ID = MissingInTransferReasonConstants.ConstMissingInTransferReasonID,
							Text = MissingInTransferReasonConstants.ConstMissingInTransferReasonID,
							IsSystemReasonCode = true,
							ShowOnPos = false
						};

						Providers.ReasonCodeData.Save(entry, missingInTransfer);

						reasonCodes.Clear();
						reasonCodes.Add(missingInTransfer);
					}
				}
				else if(inventoryType == InventoryJournalTypeEnum.Adjustment)
				{
					ReasonCode itemIsService = reasonCodes.FirstOrDefault(f => f.ID == AdjustmentReasonConstants.ItemIsServiceReasonID);
					if(itemIsService == null)
					{
						itemIsService = new ReasonCode
						{
							ID = AdjustmentReasonConstants.ItemIsServiceReasonID,
							Text = AdjustmentReasonConstants.ItemIsServiceReasonID,
							IsSystemReasonCode = true,
							ShowOnPos = false
						};

						Providers.ReasonCodeData.Save(entry, itemIsService);
					}
				}

				return reasonCodes;
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Paginated search through inventory journals based on the journal type and given <see cref="InventoryJournalLineSearch">search criteria object</see>.
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalType"></param>
		/// <param name="searchCriteria"></param>
		/// <param name="sortedBy"></param>
		/// <param name="sortBackwards"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		public virtual List<InventoryAdjustment> AdvancedSearch(
								LogonInfo logonInfo,
								InventoryJournalTypeEnum journalType,
								InventoryJournalSearch searchCriteria,
								InventoryAdjustmentSorting sortBy,
								bool sortBackwards,
								int rowFrom,
								int rowTo,
								out int totalRecords)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalType)}: {journalType}, {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}, {nameof(rowFrom)}: {rowFrom}, {nameof(rowTo)}: {rowTo}");

				return Providers.InventoryAdjustmentData.AdvancedSearch(
					entry,
					journalType,
					searchCriteria,
					sortBy, sortBackwards,
					rowFrom, rowTo,
					out totalRecords);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object used in adjustments, reserved and parked inventory</see>
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="searchCriteria">Object containing search criterias</param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> AdvancedSearch(LogonInfo logonInfo, InventoryJournalLineSearch searchCriteria, out int totalRecords)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, Utils.Starting);

				return Providers.InventoryJournalTransactionData.AdvancedSearch(entry, searchCriteria, out totalRecords);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

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
		public virtual RecordIdentifier UpdateStatus(LogonInfo logonInfo,
													RecordIdentifier journalId,
													RecordIdentifier lineNum,
													RecordIdentifier masterId,
													InventoryJournalStatus newStatus)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}, {nameof(lineNum)}: {lineNum}, {nameof(masterId)}: {masterId}, {nameof(newStatus)}: {newStatus}");

				return Providers.InventoryJournalTransactionData.UpdateStatus(entry, journalId, lineNum, masterId, newStatus);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Returns the already moved-2-inventory items for a given parked journal line
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineMasterId">The journal line Master id( of type <see cref="Guid"/>) </param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> GetMovedToInventoryLines(LogonInfo logonInfo,
																				RecordIdentifier journalId,
																				RecordIdentifier lineMasterId,
																				InventoryJournalTransactionSorting sortBy,
																				bool sortBackwards)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}, {nameof(lineMasterId)}: {lineMasterId}, {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}");

				return Providers.InventoryJournalTransactionData.GetMovedToInventoryLines(entry, journalId, lineMasterId, sortBy, sortBackwards);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);

				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);

				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Generates a new inventory adjustment ID
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <returns>New inventory adjustment ID</returns>
		public virtual RecordIdentifier GenerateInventoryAdjustmentID(LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, Utils.Starting);
				return DataProviderFactory.Instance.GenerateNumber<IInventoryAdjustmentData, InventoryAdjustment>(entry);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);
				Utils.Log(this, Utils.Done);
			}
		}
    }
}