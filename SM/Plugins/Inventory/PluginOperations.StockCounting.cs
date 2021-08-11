using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory
{
	internal partial class PluginOperations
	{
		#region Events

		public static void ShowStockCountingView(object sender, EventArgs args)
		{
			if (TestSiteService())
			{
				PluginEntry.Framework.ViewController.Add(new Views.StockCountingView());
			}
		}

		public static void ShowStockCountingWizard(object sender, EventArgs args)
		{
			if (TestSiteService())
			{
				NewStockCountingWizard();
			}
		}

		#endregion Events

		public static void NewStockCountingWizard()
		{
			if (PluginEntry.DataModel.HasPermission(Permission.StockCounting))
			{
				InventoryTypeAction inventoryTypeAction = new InventoryTypeAction { InventoryType = InventoryEnum.StockCounting };

				Dialogs.InventoryWizard dlg = new Dialogs.InventoryWizard(PluginEntry.DataModel, inventoryTypeAction);
				dlg.Text = Properties.Resources.NewCountingStockTitle;
				PluginEntry.Framework.SuspendSearchBarClosing();

				dlg.ShowDialog();

				PluginEntry.Framework.ResumeSearchBarClosing();
			}
		}

		public static void CreateNewStockCountingAdjustment(RecordIdentifier storeID, string description)
		{
			IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			try
            {
                InventoryAdjustment stockCounting = new InventoryAdjustment();
                stockCounting.JournalType = InventoryJournalTypeEnum.Counting;
                stockCounting.StoreId = storeID;
                stockCounting.Text = description;

                SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, () => stockCounting = inventoryService.SaveInventoryAdjustment(PluginEntry.DataModel, GetSiteServiceProfile(), stockCounting, true));
                dlg.ShowDialog();

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
                OpenStockCountingView(stockCounting.ID);
            }
            catch (Exception)
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
			}
		}

        public static void CreateNewStockCountingAdjustmentFromExisting(RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID)
		{
			IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			try
			{
				CreateStockCountingContainer result = null;
				SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, () => result = inventoryService.CreateStockCountingFromExistingAdjustment(PluginEntry.DataModel, GetSiteServiceProfile(), storeID, description, existingAdjustmentID, true));
				dlg.ShowDialog();

				if (CheckCreateStockCountingStatus(result.Result))
				{
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
                    OpenStockCountingView(result.CreatedStockCountingID);
                }
			}
			catch (Exception)
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
			}
		}

		public static void CreateNewStockCountingAdjustmentFromFilter(RecordIdentifier storeID, string description, InventoryTemplateFilterContainer filter)
		{
			IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			try
			{
				CreateStockCountingContainer result = null;
				SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, 
													() => result = inventoryService
																		.CreateStockCountingFromFilter(PluginEntry.DataModel, 
																									   GetSiteServiceProfile(), 
																									   storeID, 
																									   description, 
																									   filter, 
																									   true));
				dlg.ShowDialog();

				if(CheckCreateStockCountingStatus(result.Result))
				{
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
                    OpenStockCountingView(result.CreatedStockCountingID);
                }
			}
			catch (Exception)
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
			}
		}

		public static void CreateNewStockCountingAdjustmentFromTemplate(TemplateListItem template)
		{
			IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			try
			{
				CreateStockCountingContainer result = null;

				SpinnerDialog dlg = new SpinnerDialog(Resources.GeneratingStockCountingFromFilter, 
													() => result = service.CreateStockCountingFromTemplate(PluginEntry.DataModel,
																										GetSiteServiceProfile(), 
																										template, 
																										true));
				dlg.ShowDialog();

				if (result != null && result.ItemNotFoundLocally)
				{
					MessageDialog.Show(Resources.SomeItemsMissingFromStockCountingJournal + " " +
									   Resources.UpdateItemMasterToIncludeItems);
				}

				if (CheckCreateStockCountingStatus(result.Result))
				{
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
                    OpenStockCountingView(result.CreatedStockCountingID);
                }
			}
			catch (Exception)
			{
				MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
			}
		}

		public static void ShowStockCountingDetailView(RecordIdentifier stockCountingID)
		{
			PluginEntry.Framework.ViewController.Add(new Views.StockCountingDetailView(stockCountingID));
		}

        /// <summary>
        /// Add or edit an existing transaction
        /// </summary>
        /// <param name="journalID"></param>
        /// <param name="storeID"></param>
        /// <param name="itemID"></param>
        /// <param name="unitID"></param>
        /// <param name="inventoryUnitID"></param>
        /// <param name="itemCount">How many items have been counted</param>
        /// <param name="lineNumber">Provide null for new transaction</param>
        /// <param name="areaID">Provide null for new transaction</param>
        /// <param name="pictureID">The ID of the picture to set on the line. Provide null to remove a picture from a line</param>
        /// <param name="closeConnection">Provide false if the connection should stay open</param>
        public static void SaveJournalTransaction(
            RecordIdentifier journalID, 
            RecordIdentifier storeID,
            RecordIdentifier itemID,
            RecordIdentifier unitID,
            RecordIdentifier inventoryUnitID,
            decimal itemCount, 
            RecordIdentifier lineNumber = null,
            Guid? areaID = null,
            RecordIdentifier pictureID = null,
            bool closeConnection = true)
        {
            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            InventoryJournalTransaction journalTransaction = CreateJournalTransaction(service, journalID, storeID, itemID, unitID, inventoryUnitID, itemCount, lineNumber, areaID, pictureID);
            
            service.SaveJournalTransaction(PluginEntry.DataModel, GetSiteServiceProfile(), journalTransaction, closeConnection);
        }

        /// <summary>
        /// Update a list of journals; only Counted and UnitID is updated
        /// </summary>
        /// <param name="inventoryJournalTransactions">The list of journals to update</param>
        /// <param name="counted"></param>
        /// <param name="unitID"></param>
        /// <param name="shouldUpdateCounted">True if Counted should be updated</param>
        /// <param name="shouldUpdateUnitID">True if UnitID should be updated</param>
        public static void SaveJournalTransactions(
            List<InventoryJournalTransaction> inventoryJournalTransactions, 
            decimal counted,
            RecordIdentifier unitID,
            bool shouldUpdateCounted, 
            bool shouldUpdateUnitID)
        {
            IConnectionManagerTemporary threadedConnection = PluginEntry.DataModel.CreateTemporaryConnection();
            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            var totalCount = inventoryJournalTransactions.Count();

            // Prepare the progress dialog information
            using (ProgressDialog dlg = new ProgressDialog(Resources.SavingStockcountingLines, Resources.SavingCounter, totalCount))
            {
                Action saveAction = () =>
                {
                    int count = 1;

                    foreach (InventoryJournalTransaction entity in inventoryJournalTransactions)
                    {
                        if (shouldUpdateCounted)
                        {
                            entity.Counted = counted;
                        }
                        if (shouldUpdateUnitID && unitID != null && unitID != RecordIdentifier.Empty)
                        {
                            entity.UnitID = unitID;
                        }

                        service.SaveJournalTransaction(PluginEntry.DataModel, GetSiteServiceProfile(), entity, false);
                        dlg.Report(count, totalCount);

                        count++;
                    }
                    service.Disconnect(PluginEntry.DataModel);
                    threadedConnection.Close();
                };

                dlg.ProgressTask = Task.Run(saveAction);
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }
        }

        /// <summary>
        /// Remove journal transactions
        /// </summary>
        /// <param name="transactionsIDsToRemove">The IDs of the transactions to be removed</param>
        public static void RemoveJournalTransactions(List<RecordIdentifier> transactionsIDsToRemove)
        {
            SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () =>
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                service.DeleteMultipleJournalTransactions(PluginEntry.DataModel, GetSiteServiceProfile(), transactionsIDsToRemove, true);
            });
            dlg.ShowDialog();
        }

        /// <summary>
        /// Post multiple stock counting lines
        /// </summary>
        /// <param name="transactionsToPost">The lines to be posted</param>
        /// <param name="storeId"></param>
        /// <returns>Status and indicates whether the journal still has unposted lines</returns>
        public static PostStockCountingLinesContainer PostMultipleStockCountingLines(List<InventoryJournalTransaction> transactionsToPost, RecordIdentifier storeId)
        {
            PostStockCountingLinesContainer result = null;

            SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () =>
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                result = service.PostMultipleStockCountingLines(PluginEntry.DataModel, GetSiteServiceProfile(), transactionsToPost, storeId, true);
            });

            dlg.ShowDialog();

            CheckPostStockCountingStatus(result.Result);

            return result;
        }

        public static (bool CanEdit, AdjustmentStatus Status) CheckJournalProcessingStatus(RecordIdentifier journalID)
        {
            IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            (bool CanEdit, AdjustmentStatus Status) result = (CanEdit: true, Status: new AdjustmentStatus());

            result.Status = service.GetAdjustmentStatus(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalID, true);

            if(result.Status.ProcessingStatus != InventoryProcessingStatus.None || result.Status.InventoryStatus != InventoryJournalStatus.Active)
            {
                MessageDialog.Show(Resources.JournalCannotBeUpdated, MessageBoxIcon.Warning);
                result.CanEdit = false;
            }

            return result;
        }

        private static InventoryJournalTransaction CreateJournalTransaction(
            IInventoryService service,
            RecordIdentifier journalID,
            RecordIdentifier storeID,
            RecordIdentifier itemID,
            RecordIdentifier unitID,
            RecordIdentifier inventoryUnitID,
            decimal itemCount,
            RecordIdentifier lineNumber = null,
            Guid? areaID = null,
            RecordIdentifier pictureID = null)
        {
            InventoryJournalTransaction journalTransactionLine = new InventoryJournalTransaction();

            journalTransactionLine.JournalId = journalID;
            journalTransactionLine.LineNum = lineNumber ?? RecordIdentifier.Empty;
            journalTransactionLine.StaffID = PluginEntry.DataModel.CurrentUser.StaffID;
            journalTransactionLine.TransDate = DateTime.Now;
            journalTransactionLine.ItemId = itemID;
            journalTransactionLine.UnitID = unitID;

            journalTransactionLine.InventOnHandInInventoryUnits = service.GetInventoryOnHand(PluginEntry.DataModel, GetSiteServiceProfile(), itemID, storeID, true);
            journalTransactionLine.Counted = itemCount;
            journalTransactionLine.Adjustment =
                Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, itemID, unitID, inventoryUnitID, journalTransactionLine.Counted)
                - journalTransactionLine.InventOnHandInInventoryUnits;
            journalTransactionLine.AreaID = areaID ?? Guid.Empty;
            journalTransactionLine.PictureID = pictureID ?? RecordIdentifier.Empty;

            return journalTransactionLine;
        }

        private static void OpenStockCountingView(RecordIdentifier stockCountingId)
        {
            var viewController = PluginEntry.Framework.ViewController;
            var stockCountingDetailView = new Views.StockCountingDetailView(stockCountingId);
            if (viewController.CurrentView.GetType() == typeof(Views.StockCountingDetailView))
            {
                viewController.ReplaceView(viewController.CurrentView, stockCountingDetailView);
            }
            else
            {
                viewController.Add(stockCountingDetailView);
            }
        }

        /// <summary>
        /// Check the status of a create stock counting operation
        /// </summary>
        /// <param name="status">Current status of the operation</param>
        /// <returns>TRUE if the stock counting journal was created, even if the lines were not created or partially created</returns>
        private static bool CheckCreateStockCountingStatus(CreateStockCountingResult status)
		{
			switch (status)
			{
				case CreateStockCountingResult.Success: return true;
				case CreateStockCountingResult.JournalToCopyNotFound: MessageDialog.Show(Resources.StockCountingNotFound, MessageBoxIcon.Error); return false;
				case CreateStockCountingResult.TemplateNotFound: MessageDialog.Show(Resources.StockCountingTemplateNotFound, MessageBoxIcon.Error); return false;
				case CreateStockCountingResult.NoLinesCreated: MessageDialog.Show(Resources.StockCountingNoLinesCreated, MessageBoxIcon.Error); return true;
				case CreateStockCountingResult.NotAllLinesCreated: MessageDialog.Show(Resources.StockCountingNotAllLinesCreated, MessageBoxIcon.Error); return true;
				case CreateStockCountingResult.ErrorCreatingStockCounting: MessageDialog.Show(Resources.ErrorCreatingStockCounting, MessageBoxIcon.Error); return false;
				default: return false;
			}
		}

        public static void CheckPostStockCountingStatus(PostStockCountingResult status)
        {
            switch (status)
            {
                case PostStockCountingResult.Success: break;
                case PostStockCountingResult.JournalNotFound: MessageDialog.Show(Resources.StockCountingNotFound, MessageBoxIcon.Error); break;
                case PostStockCountingResult.InvalidAdjustmentType: MessageDialog.Show(Resources.InvalidAdjustmentType, MessageBoxIcon.Error); break;
                case PostStockCountingResult.JournalAlreadyPosted: MessageDialog.Show(Resources.StockCountingAlreadyPosted, MessageBoxIcon.Error); break;
                case PostStockCountingResult.ErrorCompressingJournalLines: MessageDialog.Show(Resources.ErrorCompressingStockCountingLines, MessageBoxIcon.Error); break;
                case PostStockCountingResult.ErrorPostingJournal: MessageDialog.Show(Resources.ErrorPostingJournal, MessageBoxIcon.Error); break;
                case PostStockCountingResult.ErrorPostingLinesDueToMixingJournals: MessageDialog.Show(Resources.ErrorPostingMultipleJournal, MessageBoxIcon.Error); break;
                case PostStockCountingResult.ErrorPostingLinesDueToEmptyList: MessageDialog.Show(Resources.ErrorPostingEmptyList, MessageBoxIcon.Error); break;
                case PostStockCountingResult.ErrorPostingJournalDueToUnpostedLines: MessageDialog.Show(Resources.ErrorPostingJournalWithUnpostedLines, MessageBoxIcon.Error); break;
                case PostStockCountingResult.JournalCurrentlyProcessing: MessageDialog.Show(Resources.ErrorPostingJournalProcessing, MessageBoxIcon.Error); break;
                default: break;
            }
        }

        public static void ResetProcessingStatus(RecordIdentifier journalID, InventoryProcessingStatus currentStatus)
        {
            if (currentStatus == InventoryProcessingStatus.None) return;

            string question = "";

            switch (currentStatus)
            {
                case InventoryProcessingStatus.Compressing:
                    question = Resources.ResetProcessingStatusCompressingQuestion;
                    break;
                case InventoryProcessingStatus.Posting:
                    question = Resources.ResetProcessingStatusPostingQuestion;
                    break;
                case InventoryProcessingStatus.Importing:
                    question = Resources.ResetProcessingStatusImportingQuestion;
                    break;
                case InventoryProcessingStatus.Other:
                    question = Resources.ResetProcessingStatusOtherQuestion;
                    break;
            }

            if(QuestionDialog.Show(question, Resources.ResetProcessingStatus) == DialogResult.Yes)
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                service.SetAdjustmentStatus(PluginEntry.DataModel, GetSiteServiceProfile(), journalID, InventoryProcessingStatus.None, false);

                InventoryAdjustment updatedAdjustment = service.GetInventoryAdjustment(PluginEntry.DataModel, GetSiteServiceProfile(), journalID, true);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "StockCounting", updatedAdjustment.ID, updatedAdjustment);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.None, "JournalTrans", RecordIdentifier.Empty, null);
            }
        }

        public static void RefreshJournalInventoryOnHand(RecordIdentifier stockCountingJournalID)
        {
            if (QuestionDialog.Show(Resources.UpdateJournalOnHandInventory + " " + Resources.FewMinutesMessage +". ") == DialogResult.Yes)
            {
                try
                {
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => service.RefreshStockCountingJournalInventoryOnHand(PluginEntry.DataModel, GetSiteServiceProfile(), stockCountingJournalID, true));

                    dlg.ShowDialog();
                }
                catch
                {
                    MessageBox.Show(Resources.CouldNotConnectToStoreServer);
                    return;
                }
            }
        }
    }
}