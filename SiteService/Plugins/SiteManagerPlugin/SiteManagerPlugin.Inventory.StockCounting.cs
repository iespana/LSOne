using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.AsyncResults;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
		/// <summary>
		/// Gets stock counting
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="journalId">The journal ID to be get</param>
		/// <returns>The inventory adjustment</returns>
		public virtual InventoryAdjustment GetStockCounting(LogonInfo logonInfo, RecordIdentifier journalId)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}");
				return Providers.InventoryAdjustmentData.Get(entry, journalId);
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
		/// Deletes stock counting
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="stockCountingID">The stock counting ID to be deleted</param>
		/// <returns></returns>
		public virtual DeleteJournalResult DeleteStockCounting(LogonInfo logonInfo, RecordIdentifier stockCountingID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(stockCountingID)}: {stockCountingID}");

				if(Providers.InventoryAdjustmentData.JournalIsPartiallyPosted(entry, stockCountingID))
				{
					Utils.Log(this, "Journal is partially posted and cannot be deleted");
					return DeleteJournalResult.PartiallyPosted;
				}

				Providers.InventoryAdjustmentData.Delete(entry, stockCountingID);
				return DeleteJournalResult.Success;
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
		/// Searches for stock counting entries 
		/// </summary>
		/// <param name="filter">Filter settings container</param>
		/// <param name="totalRecordsMatching">out: how many rows there are in total</param>
		/// <param name="logonInfo">The login information for the database</param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> SearchJournalTransactions(InventoryJournalTransactionFilter filter, out int totalRecordsMatching, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				var paramValues =
					$"{nameof(filter.RowFrom)}: {filter.RowFrom}, {nameof(filter.RowTo)}: {filter.RowTo}, {nameof(filter.Sort)}: {filter.Sort}, {nameof(filter.SortBackwards)}: {filter.SortBackwards}, {nameof(filter.IdOrDescriptions)}: {(filter.IdOrDescriptions.Count == 0 ? string.Empty : string.Join(";", filter.IdOrDescriptions))}, " +
					$"{nameof(filter.IdOrDescriptionStartsWith)}: {filter.IdOrDescriptionStartsWith}, {nameof(filter.Variants)}: {(filter.Variants.Count == 0 ? string.Empty : string.Join(";", filter.Variants))}, {nameof(filter.VariantStartsWith)}: {filter.VariantStartsWith}, {nameof(filter.CountedSet)}: {filter.CountedSet}, " +
					$"{nameof(filter.Counted)}: {filter.Counted}, {nameof(filter.CountedComparison)}: {filter.CountedComparison}, {nameof(filter.InventoryOnHandSet)}: {filter.InventoryOnHandSet}, {nameof(filter.InventoryOnHand)}: {filter.InventoryOnHand}, {nameof(filter.InventoryOnHandComparison)}: {filter.InventoryOnHandComparison}, " +
					$"{nameof(filter.DifferenceSet)}: {filter.DifferenceSet}, {nameof(filter.Difference)}: {filter.Difference}, {nameof(filter.DifferenceComparison)}: {filter.DifferenceComparison}, {nameof(filter.DifferencePercentageSet)}: {filter.DifferencePercentageSet}, {nameof(filter.DifferencePercentage)}: {filter.DifferencePercentage}, " +
					$"{nameof(filter.DifferencePercentageComparison)}: {filter.DifferencePercentageComparison}, {nameof(filter.PostedSet)}: {filter.PostedSet}, {nameof(filter.Posted)}: {filter.Posted}, {nameof(filter.DateFrom)}: {filter.DateFrom?.ToShortDateString()}, {nameof(filter.DateTo)}: {filter.DateTo?.ToShortDateString()}, " +
					$"{nameof(filter.RetailGroupID)}: {filter.RetailGroupID}, {nameof(filter.RetailDepartmentID)}: {filter.RetailDepartmentID}, {nameof(filter.StaffID)}: {filter.StaffID}, {nameof(filter.AreaID)}: {filter.AreaID}";

				Utils.Log(this, $"{Utils.Starting} {paramValues}");

				return Providers.InventoryJournalTransactionData.SearchJournalTransactions(entry, filter, out totalRecordsMatching);
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
		/// Updates inventory on hand for all items in a stock counting journal
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalTransactionId">A row from the header table.</param>
		public virtual void RefreshStockCountingJournalInventoryOnHand(RecordIdentifier journalID, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);
			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalID)}: {journalID}");
				Providers.InventoryJournalTransactionData.RefreshStockCountingJournalInventoryOnHand(entry, journalID);
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
		/// Gets journal list, advanced search
		/// </summary>
		/// <param name="logonInfo">The login information for the database</param>
		/// <param name="filter">The filter</param>
		/// <param name="totalRecordsMatching">The total number of returned rows</param>
		/// <returns>The filtered journal list</returns>
		public virtual List<InventoryAdjustment> GetJournalListAdvancedSearch(LogonInfo logonInfo, InventoryAdjustmentFilter filter,  out int totalRecordsMatching)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				string paramValues =
					$"{nameof(filter.JournalType)}: {filter.JournalType}, {nameof(filter.PagingSize)}: {filter.PagingSize}, {nameof(filter.Sort)}: {filter.Sort}, {nameof(filter.SortBackwards)}: {filter.SortBackwards}, " +
					$"{nameof(filter.IdOrDescription)}: {(filter.IdOrDescription.Count > 0 ? string.Empty : string.Join(";", filter.IdOrDescription))}, {nameof(filter.IdOrDescriptionBeginsWith)}: {filter.IdOrDescriptionBeginsWith}, " +
					$"{nameof(filter.Status)}: {filter.Status}, {nameof(filter.ProcessingStatus)}: {filter.ProcessingStatus}, {nameof(filter.StoreID)}: {filter.StoreID}, {nameof(filter.CreationDateFrom)}: {filter.CreationDateFrom?.ToShortDateString()}, " +
					$"{nameof(filter.CreationDateTo)}: {filter.CreationDateTo?.ToShortDateString()}, {nameof(filter.PostedDateFrom)}: {filter.PostedDateFrom?.ToShortDateString()}, {nameof(filter.PostedDateTo)}: {filter.PostedDateTo?.ToShortDateString()}";
				Utils.Log(this, $"{Utils.Starting} {paramValues}");

				return Providers.InventoryAdjustmentData.GetJournalListAdvanced(entry, filter, out totalRecordsMatching);
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
		/// Gets adjustment status for journal
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="journalId">The journal ID</param>
		/// <returns>The adjustment status</returns>
		public virtual AdjustmentStatus GetAdjustmentStatus(LogonInfo logonInfo, RecordIdentifier journalId)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}");
				return Providers.InventoryAdjustmentData.GetAdjustmentStatus(entry, journalId);
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
		/// Sets adjustment status for journal
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="journalId">The journal ID</param>
		/// <param name="status">The status to set</param>
		/// <returns></returns>
		public virtual void SetAdjustmentStatus(LogonInfo logonInfo, RecordIdentifier journalId, InventoryProcessingStatus status)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalId)}: {journalId}");
				Providers.InventoryAdjustmentData.SetAdjustmentProcessingStatus(entry, journalId, status);
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
		/// Deletes multiple inventory journal transaction lines
		/// </summary>
		/// <param name="IDs">The IDs of the inventory journal to delete</param>
		/// <param name="logonInfo">Login credentials</param>
		public virtual DeleteJournalTransactionsResult DeleteMultipleJournalTransactions(List<RecordIdentifier> IDs, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, Utils.Starting);

				if (IDs.Select(x => (string)x.PrimaryID).Distinct().Count() > 1)
				{
					Utils.Log(this, "Deleting lines failed due to passing more than one journal. Only one journal is allowed");
					return DeleteJournalTransactionsResult.ErrorPostingLinesDueToMixingJournals;
				}

				Providers.InventoryJournalTransactionData.DeleteMultipleLines(entry, IDs);

				return DeleteJournalTransactionsResult.Success;
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
		/// Returns true if the inventory adjustment with the provided ID exists
		/// </summary>
		/// <param name="stockCountingID">The stock counting ID to be checked</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Returns true if the inventory adjustment with the provided ID exists</returns>
		public virtual bool InventoryAdjustmentExists(RecordIdentifier stockCountingID, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(stockCountingID)}: {stockCountingID}");
				return Providers.InventoryAdjustmentData.Exists(entry, stockCountingID);
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
		/// Posts multiple stock counting lines
		/// </summary>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="entry">The entry into the database</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <param name="ijTransactions">The lines to post</param>
		/// <param name="storeId">The store the lines are for</param>
		/// <returns>Status and indicates whether the journal still has unposted lines</returns>
		public virtual PostStockCountingLinesContainer PostMultipleStockCountingLines(List<InventoryJournalTransaction> ijTransactions, RecordIdentifier storeId, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);
			
			try
			{
				Utils.Log(this, $"{Utils.Starting} Posting multiple stock counting lines.");

				if(ijTransactions.Select(x => (string)x.JournalId).Distinct().Count() > 1)
				{
					Utils.Log(this, "Posting lines failed due to passing more than one journal. Only one journal is allowed");
					return new PostStockCountingLinesContainer { Result = PostStockCountingResult.ErrorPostingLinesDueToMixingJournals };
				}

				if (!ijTransactions.Any())
				{
					Utils.Log(this, "Posting lines failed due to passing an empty list of journals");
					return new PostStockCountingLinesContainer { Result = PostStockCountingResult.ErrorPostingLinesDueToEmptyList };
				}

                CostCalculation costCalculation = (CostCalculation)entry.Settings.GetSetting(entry, Settings.CostCalculation).IntValue;

				RecordIdentifier journalId = ijTransactions.First().JournalId;
				Providers.InventoryAdjustmentData.SetAdjustmentProcessingStatus(entry, journalId, InventoryProcessingStatus.Posting);

				Dictionary<string, string> itemIDs = Providers.RetailItemData.GetUnitIDsForItems(entry, RetailItem.UnitTypeEnum.Inventory);

				Utils.Log(this, "Unit IDs for items retrieved, posting starting");

				foreach (var ijTransaction in ijTransactions)
				{
					CalculateAdjustmentForLine(entry, ijTransaction, storeId, itemIDs);

                    if(costCalculation == CostCalculation.Manual)
                    {
                        ijTransaction.CostPrice = Providers.RetailItemData.GetItemPrice(entry, ijTransaction.ItemId).PurchasePrice;
                    }
                    else
                    {
                        RetailItemCost itemCost = Providers.RetailItemCostData.Get(entry, ijTransaction.ItemId, storeId);
                        ijTransaction.CostPrice = itemCost == null ? 0 : itemCost.Cost;
                    }

					ijTransaction.Posted = true;
					ijTransaction.PostedDateTime = DateTime.Now;

					Providers.InventoryJournalTransactionData.Save(entry, ijTransaction);

					var inventoryTransaction = new InventoryTransaction(ijTransaction.ItemId, ijTransaction.ItemType)
					{
						Guid = Guid.NewGuid(),
						PostingDate = DateTime.Now,
						StoreID = storeId,
						Adjustment = ijTransaction.Adjustment,
						AdjustmentInInventoryUnit = ijTransaction.AdjustmentInInventoryUnit,
						Type = InventoryTypeEnum.StockCount,
						CostPricePerItem = ijTransaction.CostPrice,
						ReasonCode = ijTransaction.ReasonId,
						AdjustmentUnitID = ijTransaction.UnitID,
						Reference = (string)ijTransaction.JournalId
					};
					
					Providers.InventoryTransactionData.Save(entry, inventoryTransaction);
				}

				Providers.InventoryAdjustmentData.SetAdjustmentProcessingStatus(entry, journalId, InventoryProcessingStatus.None);
				PostStockCountingResult postResult = Providers.InventoryAdjustmentData.PostAdjustmentWithCheck(entry, journalId);

				Utils.Log(this, "Posting finished");

				//We return success even if the post result could not post the journal, because we could be posting just some of the lines in the journal
				return new PostStockCountingLinesContainer { Result = PostStockCountingResult.Success, HasUnpostedLines = postResult != PostStockCountingResult.Success };
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
		/// Posts all lines of a stock counting journal
		/// </summary>
		/// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Result of the operation</returns>
		public virtual PostStockCountingLinesContainer PostAllStockCountingLines(RecordIdentifier stockCountingJournalID, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Posting all lines for stock counting journal ID: {stockCountingJournalID}");
				PostStockCountingLinesContainer result = new PostStockCountingLinesContainer();

				try
				{
					result.Result = Providers.InventoryJournalTransactionData.PostAllStockCountingLines(entry, stockCountingJournalID);
				}
				catch(Exception innerEx)
				{
					Utils.LogException(this, innerEx);
					result.Result = PostStockCountingResult.ErrorPostingJournal;
				}

				Utils.Log(this, "Posting finished");
				return result;
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
		/// Posts all lines from a stock counting journal asynchronously
		/// </summary>
		/// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="callback">The callback method to be called when the operation is done</param>
		/// <param name="asyncState">An object containing data to be used by the beginMethod delegate</param>
		/// <returns>Result of the operation</returns>
		public virtual IAsyncResult BeginAsyncPostAllStockCountingLines(RecordIdentifier stockCountingJournalID, LogonInfo logonInfo, AsyncCallback callback, object asyncState)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Posting all lines for stock counting journal ID: {stockCountingJournalID}");

				var temporaryConnection = entry.CreateTemporaryConnection();

				Task<CompletedAsyncResult<PostStockCountingLinesContainer>> task = Task.Run(() =>
				{
					PostStockCountingLinesContainer postLinesResult = new PostStockCountingLinesContainer();
				
					try
					{
						postLinesResult.Result = Providers.InventoryJournalTransactionData.PostAllStockCountingLines(temporaryConnection, stockCountingJournalID);
						postLinesResult.HasUnpostedLines = postLinesResult.Result != PostStockCountingResult.Success;
					}
					catch (Exception innerEx)
					{
						Utils.LogException(this, innerEx);
						postLinesResult.Result = PostStockCountingResult.ErrorPostingJournal;
					}
					finally
					{
						temporaryConnection?.Close();
					}
				
					Utils.Log(this, "Posting finished");
					return new CompletedAsyncResult<PostStockCountingLinesContainer>(postLinesResult, asyncState);
				});
				
				return task.ContinueWith(r => callback(task.Result));
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
		/// Unwraps the result returned by BeginAsyncPostAllStockCountingLines(), which is called asynchronously
		/// </summary>
		/// <param name="asyncResult"></param>
		/// <returns></returns>
		public virtual PostStockCountingLinesContainer EndAsyncPostAllStockCountingLines(IAsyncResult asyncResult)
		{
			CompletedAsyncResult<PostStockCountingLinesContainer> result = (CompletedAsyncResult<PostStockCountingLinesContainer>)asyncResult;

			Utils.Log(this, "End of post all stock counting lines");
			return result.Data;
		}

		private void CalculateAdjustments(IConnectionManager entry, InventoryJournalTransaction ijTransaction,
			RecordIdentifier storeId)
		{            
			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(ijTransaction)}: {ijTransaction.ID}");
				// Refresh the inventory on hand and adjustment properties of the transaction
				var inventoryOnHand = Providers.InventoryData.GetInventoryOnHand(entry, ijTransaction.ItemId, storeId);
				ijTransaction.InventOnHandInInventoryUnits = inventoryOnHand;
				Utils.Log(this, "On hand inventory updated");

				decimal countInInventoryUnit =
					Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, ijTransaction.ItemId, ijTransaction.UnitID,
						ijTransaction.InventoryUnitID, ijTransaction.Counted);

				ijTransaction.AdjustmentInInventoryUnit = countInInventoryUnit - ijTransaction.InventOnHandInInventoryUnits;

				Utils.Log(this, "Adjustment in inventory unit calculated");

				// Here we calculate the adjustment in inventory units already posted for this item.
				decimal adjustmentsInInventoryUnitsAlreadyPostedForItem = 0M;

				var previousLinesInStockCounting = Providers.InventoryJournalTransactionData.GetJournalTransactionList(entry, ijTransaction.JournalId);                

				foreach (InventoryJournalTransaction previousLine in previousLinesInStockCounting)
				{
					if (previousLine.ItemId == ijTransaction.ItemId
						&& previousLine.VariantName == ijTransaction.VariantName
						&& previousLine.StoreId == ijTransaction.StoreId
						&& previousLine.Posted)
					{
						RecordIdentifier inventoryUnitID = Providers.RetailItemData.GetItemUnitID(entry, previousLine.ItemId,
							RetailItem.UnitTypeEnum.Inventory);

						decimal alreadyCountedInInventoryUnit =
							Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, previousLine.ItemId,
								previousLine.UnitID, inventoryUnitID, previousLine.Counted);

						adjustmentsInInventoryUnitsAlreadyPostedForItem += alreadyCountedInInventoryUnit;
					}
				}

				ijTransaction.AdjustmentInInventoryUnit += adjustmentsInInventoryUnitsAlreadyPostedForItem;
				Utils.Log(this, "Adjustments in inventory units already posted for this item found");

				decimal adjustment = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, ijTransaction.ItemId,
						ijTransaction.InventoryUnitID, ijTransaction.UnitID, ijTransaction.AdjustmentInInventoryUnit);

				ijTransaction.Adjustment = adjustment;

			}
			finally
			{
				Utils.Log(this, Utils.Done);
			}
		}

		private void CalculateAdjustmentForLine(IConnectionManager entry, InventoryJournalTransaction ijTransaction,
			RecordIdentifier storeId, Dictionary<string, string> itemUnitIDs)
		{            
			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(ijTransaction)}: {ijTransaction.ID}");

				// Refresh the inventory on hand and adjustment properties of the transaction
				var inventoryOnHand = Providers.InventoryData.GetInventoryOnHand(entry, ijTransaction.ItemId, storeId);
				ijTransaction.InventOnHandInInventoryUnits = inventoryOnHand;
				Utils.Log(this, "On hand inventory updated");

				decimal countInInventoryUnit =
					Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, ijTransaction.ItemId, ijTransaction.UnitID,
						ijTransaction.InventoryUnitID, ijTransaction.Counted);

				ijTransaction.AdjustmentInInventoryUnit = countInInventoryUnit - ijTransaction.InventOnHandInInventoryUnits;
				Utils.Log(this, "Adjustment in inventory unit calculated");

				decimal adjustmentsInInventoryUnitsAlreadyPostedForItem = 0M;

				var previousLinesInStockCounting = Providers.InventoryJournalTransactionData.GetPostedJournalTransactionsForTransaction(entry, ijTransaction.JournalId, ijTransaction.ItemId, ijTransaction.VariantName, ijTransaction.StoreId);

				foreach (InventoryJournalTransaction previousLine in previousLinesInStockCounting)
				{

					RecordIdentifier inventoryUnitID = itemUnitIDs[(string)previousLine.ItemId];

					decimal alreadyCountedInInventoryUnit =
						Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, previousLine.ItemId,
							previousLine.UnitID, inventoryUnitID, previousLine.Counted);

					adjustmentsInInventoryUnitsAlreadyPostedForItem += alreadyCountedInInventoryUnit;
				}

				ijTransaction.AdjustmentInInventoryUnit += adjustmentsInInventoryUnitsAlreadyPostedForItem;
				Utils.Log(this, "Adjustments in inventory units already posted for this item found");

				decimal adjustment = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, ijTransaction.ItemId,
					ijTransaction.InventoryUnitID, ijTransaction.UnitID, ijTransaction.AdjustmentInInventoryUnit);

				ijTransaction.Adjustment = adjustment;
			}
			finally
			{
				Utils.Log(this, Utils.Done);
			}
		}

		/// <summary>
		/// Gets inventory journal transaction by ID
		/// </summary>
		/// <param name="journalTransactionId">The journal transaction ID</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Returns the inventory journal transaction for the supplied ID</returns>
		public virtual InventoryJournalTransaction GetInventoryJournalTransaction(RecordIdentifier journalTransactionId, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(journalTransactionId)}: {journalTransactionId}");
				return Providers.InventoryJournalTransactionData.Get(entry, journalTransactionId);
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
		/// Gets inventory on hand for the specified item ID and store ID
		/// </summary>
		/// <param name="itemID">The item ID</param>
		/// <param name="storeID">The store ID</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Inventory on hand</returns>
		public virtual decimal GetInventoryOnHand(RecordIdentifier itemID, RecordIdentifier storeID, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID} - {nameof(storeID)}: {storeID}");
				return Providers.InventoryData.GetInventoryOnHand(entry, itemID, storeID);
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
		/// Gets inventory on hand for the specified list of item ID's and store ID
		/// </summary>
		/// <param name="itemIDs">The list of item ID's</param>
		/// <param name="storeID">The store ID</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Inventory on hand</returns>
		public virtual List<InventoryRetailItem> GetInventoryOnHandForItems(List<RecordIdentifier> itemIDs, RecordIdentifier storeID, bool includeInventoryOnHand, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				return Providers.RetailItemData.GetInventoryRetailItems(entry, itemIDs, new List<RecordIdentifier>(), storeID, includeInventoryOnHand);
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
		/// Gets inventory on hand for the specified item ID and store ID
		/// </summary>
		/// <param name="itemID">The item ID</param>
		/// <param name="storeID">The store ID</param>
		/// <param name="logonInfo">Login credentials</param>
		/// <returns>Inventory on hand</returns>
		public virtual decimal GetInventoryOnHandForAssemblyItem(RecordIdentifier itemID, RecordIdentifier storeID, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID} - {nameof(storeID)}: {storeID}");
                return Providers.InventoryData.GetInventoryOnHandForAssemblyItem(entry, itemID, storeID);
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
        /// Saves the inventory journal transaction
        /// </summary>
        /// <param name="ijTransaction">The inventory journal transaction to be saved</param>
        /// <param name="logonInfo">Login credentials</param>
        public virtual void SaveJournalTransaction(InventoryJournalTransaction ijTransaction, LogonInfo logonInfo)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(ijTransaction)}: {ijTransaction.ID}");
				Providers.InventoryJournalTransactionData.Save(entry, ijTransaction);
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
		/// Creates a new stock counting journal and copy all lines from an existing journal
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="storeID">Store ID for the new journal</param>
		/// <param name="description">>Description of the new journal</param>
		/// <param name="existingAdjustmentID">Existing journal ID from which to copy the lines</param>
		/// <returns></returns>
		public virtual CreateStockCountingContainer CreateStockCountingFromExistingAdjustment(LogonInfo logonInfo, RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Creating stock counting adjustment: " + description);

				if(!Providers.InventoryAdjustmentData.Exists(entry, existingAdjustmentID))
				{
					Utils.Log(this, "Journal to copy not found.");
					return new CreateStockCountingContainer(CreateStockCountingResult.JournalToCopyNotFound, RecordIdentifier.Empty);
				}

				InventoryAdjustment stockCounting = new InventoryAdjustment();
				stockCounting.JournalType = InventoryJournalTypeEnum.Counting;
				stockCounting.StoreId = storeID;
				stockCounting.Text = description;
				stockCounting.CreatedDateTime = DateTime.Now;
				stockCounting.PostedDateTime = Date.Empty;

				Utils.Log(this, "Saving stock counting journal.");

				Providers.InventoryAdjustmentData.Save(entry, stockCounting);

				CreateStockCountingContainer result = new CreateStockCountingContainer(CreateStockCountingResult.Success, stockCounting.ID);

				Utils.Log(this, "Saving stock counting lines");

				try
				{
					Providers.InventoryJournalTransactionData.CopyStockCountingJournalLines(entry, existingAdjustmentID, stockCounting.ID, stockCounting.StoreId);
				}
				catch(Exception innerEx)
				{
					Utils.LogException(this, innerEx);
					result.Result = CreateStockCountingResult.ErrorCreatingStockCounting;
				}

				Utils.Log(this, "Checking number of saved lines");
				int originalNumberOfLines = Providers.InventoryJournalTransactionData.Count(entry, existingAdjustmentID) ?? 0;
				int addedNumberOfLines = Providers.InventoryJournalTransactionData.Count(entry, stockCounting.ID) ?? 0;

				if(addedNumberOfLines == 0 && originalNumberOfLines != 0)
				{
					Utils.Log(this, "No lines created.");
					result.Result = CreateStockCountingResult.NoLinesCreated;
				}
				else if(originalNumberOfLines != addedNumberOfLines)
				{
					Utils.Log(this, "Not lines could be created.");
					result.Result = CreateStockCountingResult.NotAllLinesCreated;
				}

				return result;
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
		/// Creates a new stock counting journal based on a filter
		/// </summary>
		/// <param name="logonInfo">Login credentials</param>
		/// <param name="storeID">Store ID for the new journal</param>
		/// <param name="description">Description of the new journal</param>
		/// <param name="filter">Container with desired item filters</param>
		public virtual CreateStockCountingContainer CreateStockCountingFromFilter(LogonInfo logonInfo,
																				RecordIdentifier storeID,
																				string description,
																				InventoryTemplateFilterContainer filter)
		{
			if (RecordIdentifier.IsEmptyOrNull(storeID)) throw new ArgumentNullException(nameof(storeID));

			if (filter == null) throw new ArgumentNullException(nameof(filter));

			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Creating stock counting adjustment: " + description);

				CreateStockCountingContainer result = CreateNewStockCounting(entry, storeID, description, filter);

				return result;
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
		/// Creates a new stock counting journal based on a given template
		/// </summary>
		/// <param name="logonInfo">Log-on information.</param>
		/// <param name="templateID">Inventory template ID.</param>
		/// <returns></returns>
		public virtual CreateStockCountingContainer CreateStockCountingFromTemplate(LogonInfo logonInfo, TemplateListItem template)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Creating stock counting adjustment from template: " + template?.TemplateID);

				if (template == null
					|| RecordIdentifier.IsEmptyOrNull(template.TemplateID)
					|| !Providers.InventoryTemplateData.Exists(entry, template.TemplateID))
				{
					Utils.Log(this, $"Template {template?.TemplateID?.StringValue} not found.");
					return new CreateStockCountingContainer(CreateStockCountingResult.TemplateNotFound, RecordIdentifier.Empty);
				}

				List<InventoryTemplateSectionSelection> filters = Providers.InventoryTemplateSectionSelectionData.GetList(entry, template.TemplateID);
				InventoryTemplateFilterContainer filter = new InventoryTemplateFilterContainer(filters);

				CreateStockCountingContainer result = CreateNewStockCounting(entry,
																			template.StoreID,
																			template.TemplateName, // description
																			filter,
																			template.TemplateID);

				return result;
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
		/// Imports stock counting from the DataTable provided in the parameter <paramref name="data"/>
		/// </summary>
		/// <param name="logonInfo">Log-on information</param>
		/// <param name="data">The data to be imported</param>
		/// <returns>A log for each processed row in the <paramref name="data"/></returns>
		public virtual List<ImportLogItem> ImportStockCountingFromExcel(LogonInfo logonInfo, DataTable data)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} Importing stock counting lines from excel...");
				List<ImportLogItem> logItems = new List<ImportLogItem>();

				const string StockCountingIDColumn = "STOCK COUNTING ID";
				const string DescriptionColumn = "DESCRIPTION";
				const string ItemIDColumn = "ITEM ID";
				const string BarcodeColumn = "BARCODE";
				const string UnitIDColumn = "UNIT ID";
				const string CountedColumn = "COUNTED";
				const string VariantDescriptionColumn = "VARIANT DESCRIPTION";

				RecordIdentifier stockCountingID = RecordIdentifier.Empty;
				RecordIdentifier itemID;
				RecordIdentifier barcode = RecordIdentifier.Empty;
				RecordIdentifier storeID = RecordIdentifier.Empty;

				string variantName = string.Empty;
				string description = string.Empty;

				Dictionary<RecordIdentifier, InventoryRetailItem> retailItems = new Dictionary<RecordIdentifier, InventoryRetailItem>();

				string[] mandatoryFields = new string[]
				{
					StockCountingIDColumn, UnitIDColumn, CountedColumn
				};

				for(int i = data.Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = data.Rows[i];
					RecordIdentifier newStockCountingID = row.GetStringValue(StockCountingIDColumn);
					itemID = row.GetStringValue(ItemIDColumn);
					int lineNumber = data.GetRowNumber(row, "LINE NUMBER");

					if(data.Columns.Contains(BarcodeColumn))
                    {
						barcode = row.GetStringValue(BarcodeColumn);
					}

					if (data.Columns.Contains(DescriptionColumn))
					{
						description = row.GetStringValue(DescriptionColumn);
					}

					if (data.Columns.Contains(VariantDescriptionColumn))
					{
						variantName = row.GetStringValue(VariantDescriptionColumn);
						description += " " + variantName;
					}

					//Either item ID or barcode must have a value
					if(RecordIdentifier.IsEmptyOrNull(itemID) && RecordIdentifier.IsEmptyOrNull(barcode))
                    {
						if (lineNumber > 0)
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", ItemIDColumn + "/" + BarcodeColumn),
								lineNumber, description));
						}
						else
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", ItemIDColumn + "/" + BarcodeColumn)));
						}

						data.Rows.RemoveAt(i);
						continue;
					}

					if(!CheckMandatoryFields(data, row, itemID, mandatoryFields, true, logItems, lineNumber, description))
					{
						data.Rows.RemoveAt(i);
						continue;
					}

					if (stockCountingID != newStockCountingID)
					{
						stockCountingID = newStockCountingID;
						InventoryAdjustment inventoryAdjustment = Providers.InventoryAdjustmentData.Get(entry, stockCountingID);

						if (inventoryAdjustment == null)
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.StockCountNotExist, lineNumber, description));
							data.Rows.RemoveAt(i);
							continue;
						}
						else if (storeID != inventoryAdjustment.StoreId)
						{
							List<RecordIdentifier> itemIDs = data.AsEnumerable().Select(x => new RecordIdentifier(x.GetStringValue(ItemIDColumn))).Where(x => x != "").ToList();
							List<RecordIdentifier> barcodes = new List<RecordIdentifier>();

							if(data.Columns.Contains(BarcodeColumn))
                            {
								barcodes = data.AsEnumerable().Select(x => new RecordIdentifier(x.GetStringValue(BarcodeColumn))).Where(x => x != "").ToList();
							}

							retailItems = Providers.RetailItemData.GetInventoryRetailItems(entry, itemIDs, barcodes, inventoryAdjustment.StoreId, false).ToDictionary(x => x.ID);
						}
					}

					InventoryRetailItem item = null;

					if (RecordIdentifier.IsEmptyOrNull(itemID) && !RecordIdentifier.IsEmptyOrNull(barcode))
					{
						item = retailItems.FirstOrDefault(x => x.Value.Barcode == barcode).Value;

						if(item != null)
                        {
							itemID = item.ID;
							row[ItemIDColumn] = Convert.ToDouble(item.ID.StringValue);
                        }
					}

					if (item != null || retailItems.TryGetValue(itemID, out item))
					{
						if (item != null && item.Deleted)
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.StockCountingLineItemDeleted, lineNumber, description));
							data.Rows.RemoveAt(i);
							continue;
						}

						if (item != null && item.ItemType == ItemTypeEnum.MasterItem)
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.NoStockForHeaderItems, lineNumber, description));
							data.Rows.RemoveAt(i);
							continue;
						}

						if (item != null && item.ItemType == ItemTypeEnum.Service)
						{
							logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.StockCountingLineServiceItem, lineNumber, description));
							data.Rows.RemoveAt(i);
							continue;
						}
					}
					else
					{
						logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, itemID, Properties.Resources.CannotRetrieveRetailItem, lineNumber, description));
						data.Rows.RemoveAt(i);
						continue;
					}
				}

				data.AcceptChanges();

				string xml = data.ToXMLString();

				try
				{
					string languageCode = CultureInfo.CurrentCulture.Name;

					Utils.Log(this, $"Calling ImportStockCountingLinesFromXML stored procedure with language code: {languageCode}");

					int insertedRows = Providers.InventoryJournalTransactionData.ImportStockCountingLinesFromXML(entry, xml, languageCode);

					if(insertedRows > 0)
					{
						logItems.Add(new ImportLogItem(ImportAction.Inserted, data.TableName, "", "", count: insertedRows));
					}

					int skippedRows = data.Rows.Count - insertedRows;

					if(skippedRows != 0)
					{
						logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, "", Properties.Resources.SkippedDuplicateRows, count: skippedRows));
					}
				}
				catch(Exception innerEx)
				{
					Utils.LogException(this, innerEx);
					logItems.Add(new ImportLogItem(ImportAction.Skipped, data.TableName, "", Properties.Resources.ErrorImportingStockCountingLines + Environment.NewLine + innerEx.Message));
				}

				return logItems;
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
		/// Posts inventory adjustment line
		/// </summary>
		/// <param name="logonInfo">Log-on information</param>
		/// <param name="inventoryJournalTrans">The inventory journal transaction</param>
		/// <param name="storeId">The store ID</param>
		/// <param name="typeOfAdjustmentLine">The type of adjustment line</param>
		public virtual void PostInventoryAdjustmentLine(LogonInfo logonInfo, InventoryJournalTransaction inventoryJournalTrans, RecordIdentifier storeId, InventoryTypeEnum typeOfAdjustmentLine)
		{
			IConnectionManager entry = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting}");
				PostInventoryJournalTransLine(entry, inventoryJournalTrans);
				UpdateInventoryFromInventoryJournalTransLine(entry, inventoryJournalTrans, storeId, typeOfAdjustmentLine);
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
		/// Compresses all lines on a stock counting journal, that have the same item and unit
		/// </summary>
		/// <param name="logonInfo">Log-on information</param>
		/// <param name="stockCountingID">Stock counting journal id</param>
		/// <returns>Operation result</returns>
		public virtual CompressAdjustmentLinesResult CompressAllStockCountingLines(LogonInfo logonInfo, RecordIdentifier stockCountingID)
		{
			IConnectionManager dataModel = GetConnectionManager(logonInfo);

			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(stockCountingID)}: {stockCountingID}");
				return Providers.InventoryAdjustmentData.CompressAllStockCountingLines(dataModel, stockCountingID);   
			}
			catch (Exception e)
			{
				Utils.LogException(this, e);
				return CompressAdjustmentLinesResult.ErrorCompressingLines;
			}
			finally
			{
				ReturnConnection(dataModel, out dataModel);
				Utils.Log(this, Utils.Done);
			}
		}

		private void PostInventoryJournalTransLine(IConnectionManager entry, InventoryJournalTransaction inventoryJournalTrans)
		{
			try
			{
				Utils.Log(this, $"{Utils.Starting}");
				inventoryJournalTrans.Posted = true;
				inventoryJournalTrans.PostedDateTime = DateTime.Now;
				Providers.InventoryJournalTransactionData.Save(entry, inventoryJournalTrans);
			}
			finally
			{
				Utils.Log(this, Utils.Done);
			}
		}

		private void UpdateInventoryFromInventoryJournalTransLine(IConnectionManager entry, InventoryJournalTransaction inventoryJournalTrans, RecordIdentifier storeId, InventoryTypeEnum typeOfTransLine)
		{
			try
			{
				Utils.Log(this, $"{Utils.Starting} {nameof(inventoryJournalTrans)}: {inventoryJournalTrans.ID} - {nameof(storeId)}: {storeId}");
				var it = new InventoryTransaction(inventoryJournalTrans.ItemId, inventoryJournalTrans.ItemType)
				{
					Guid = Guid.NewGuid(),
					PostingDate = DateTime.Now,
					StoreID = storeId,
					Adjustment = inventoryJournalTrans.Adjustment,
					AdjustmentInInventoryUnit = inventoryJournalTrans.AdjustmentInInventoryUnit,
					Type = typeOfTransLine,
					CostPricePerItem = inventoryJournalTrans.CostPrice,
					ReasonCode = inventoryJournalTrans.ReasonId,
					AdjustmentUnitID = inventoryJournalTrans.UnitID,
					Reference = (string)inventoryJournalTrans.JournalId
				};

				Providers.InventoryTransactionData.Save(entry, it);
			}
			finally
			{
				Utils.Log(this, Utils.Done);
			}
		}
	}
}