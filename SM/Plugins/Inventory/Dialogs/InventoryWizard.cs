using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs.WizardPages;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class InventoryWizard : WizardBase
	{
		private InventoryTypeAction inventoryTypeAction;

		public InventoryWizard(IConnectionManager connection, InventoryTypeAction inventoryTypeAction, InventoryJournalTypeEnum journalType = InventoryJournalTypeEnum.Adjustment)
			: base(connection)
		{
			InitializeComponent();
		  
			this.inventoryTypeAction = inventoryTypeAction;

			switch (inventoryTypeAction.InventoryType)
			{
				case InventoryEnum.StockCounting:
					NewStockCountingJournal stockCountingPage = new NewStockCountingJournal(this, inventoryTypeAction);
					stockCountingPage.RequestFinish += Page_RequestFinish;
					AddPage(stockCountingPage);
					break;
				case InventoryEnum.PurchaseOrder:
					NewPurchaseOrder page = new NewPurchaseOrder(this, inventoryTypeAction);
					page.RequestFinish += Page_RequestFinish;
					AddPage(page);
					break;
				case InventoryEnum.GoodsReceiving:
					throw new NotImplementedException("No functionality for Goods receiving has been implemented");
				case InventoryEnum.InventoryJournal:
					if (inventoryTypeAction.Action == InventoryActionEnum.Manage)
					{
						NewInventoryJournal journalPage = new NewInventoryJournal(this);
						AddPage(journalPage);
					}
					else if( inventoryTypeAction.Action == InventoryActionEnum.New)
					{
						NewEmptyIJ journalPage = new NewEmptyIJ(this, journalType);
						AddPage(journalPage);
						ActiveControl = (NewEmptyIJ)CurrentPage;
					}
					break;
				case null:
					break;
			}
		}

		private void Page_RequestFinish(object sender, EventArgs e)
		{
			Finish();
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		protected override HelpSettings GetOnlineHelpSettings()
		{
            switch (inventoryTypeAction.InventoryType)
			{
				case InventoryEnum.StockCounting:
					return new HelpSettings("StockCountingWizard");

				case InventoryEnum.PurchaseOrder:
					return new HelpSettings("PurchaseOrderWizard");

				case InventoryEnum.InventoryJournal:
					return new HelpSettings("InventoryJournalsWizard");

				default:
					return new HelpSettings(this.Name);
			}
		}

		#region OnStockCountingFinish
		private void OnStockCountingFinish(List<IWizardPage> pages, ref bool cancelAction)
		{
			switch (inventoryTypeAction.Action)
			{
				case InventoryActionEnum.Manage:
					break;

				case InventoryActionEnum.New:
					NewEmptySCJ newStockCountingJournalResult = (NewEmptySCJ)pages[1];
					PluginOperations.CreateNewStockCountingAdjustment(newStockCountingJournalResult.StoreID, newStockCountingJournalResult.Description);
					break;

				case InventoryActionEnum.GenerateFromExisting:
					NewEmptySCJ newStockCountingCopyResult = (NewEmptySCJ)pages[2];
					StockCountingJournalSearch stockCountingJournalSearchPage = (StockCountingJournalSearch)pages[1];
					PluginOperations.CreateNewStockCountingAdjustmentFromExisting(newStockCountingCopyResult.StoreID, newStockCountingCopyResult.Description, stockCountingJournalSearchPage.InventoryAdjustmentID);
					break;

				case InventoryActionEnum.GenerateFromTemplate:
					TemplateListItem template = ((NewInventoryDocumentFromTemplate)pages[1]).SelectedTemplate;
					PluginOperations.CreateNewStockCountingAdjustmentFromTemplate(template);
					break;

				case InventoryActionEnum.GenerateFromFilter:
					NewEmptySCJ newStockCountingResult = (NewEmptySCJ)pages[2];
					PluginOperations.CreateNewStockCountingAdjustmentFromFilter(newStockCountingResult.StoreID, newStockCountingResult.Description, newStockCountingResult.Filter);
					break;

				case InventoryActionEnum.ExcelImport:
					ImportSCJFromFile importSCJFromFile = (ImportSCJFromFile)pages[1];
					List<ImportDescriptor> importDescriptors = importSCJFromFile.ImportDescriptors;
					List<ImportFileListItem> selection = importSCJFromFile.ImportFilesListContent;
					FolderItem folderItem;
					List<ImportFileItem> files = new List<ImportFileItem>();
					InventoryAdjustment selectedStockCountingJournal = importSCJFromFile.SelectedStockCountingJournal;
					foreach (ImportFileListItem file in selection)
					{
						if (importSCJFromFile.WorkingFolder.Value != null)
						{
							folderItem = new FolderItem(Path.Combine(importSCJFromFile.WorkingFolder.Value, file.FullFileName));
						}
						else
						{
							folderItem = new FolderItem(file.FullFileName);
						}
						files.Add(new ImportFileItem(folderItem, file.ProfileId, selectedStockCountingJournal));
					}
					ImportDescriptor descriptor = importDescriptors.FirstOrDefault();
					descriptor.Importer(PluginEntry.DataModel, files);

					break;
				case InventoryActionEnum.Other:
					break;
				case null:
					break;
			}
		}

		#endregion

		#region OnPurchaseOrderFinish
		private void OnPurchaseOrderFinish(List<IWizardPage> pages, ref bool cancelAction)
		{
			IInventoryService service;

			switch (inventoryTypeAction.Action)
			{
				case InventoryActionEnum.New:
					service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

					try
					{
						NewEmptyPO newPurchaseOrderResult;
						PurchaseOrder purchaseOrder = new PurchaseOrder();
						newPurchaseOrderResult = (NewEmptyPO)pages[1];

						purchaseOrder.StoreID = newPurchaseOrderResult.StoreID;
						purchaseOrder.VendorID = (string)newPurchaseOrderResult.SelectedVendorID;
						purchaseOrder.CurrencyCode = newPurchaseOrderResult.CurrencyCode;
						purchaseOrder.Description = newPurchaseOrderResult.Description;
						purchaseOrder.DefaultDiscountPercentage = (decimal)newPurchaseOrderResult.DiscountPercentage;
						purchaseOrder.DefaultDiscountAmount = (decimal)newPurchaseOrderResult.DiscountAmount;
						purchaseOrder.DeliveryDate = newPurchaseOrderResult.DeliveryDate;
						purchaseOrder.OrderingDate = newPurchaseOrderResult.OrderingDate;

						purchaseOrder = service.SaveAndReturnPurchaseOrder(PluginEntry.DataModel,
							PluginOperations.GetSiteServiceProfile(), purchaseOrder, true);

						if(PluginEntry.Framework.ViewController.CurrentView.GetType() == typeof(Views.PurchaseOrderView))
						{
							PluginEntry.Framework.ViewController.ReplaceView(PluginEntry.Framework.ViewController.CurrentView, new Views.PurchaseOrderView(purchaseOrder.ID));
						}
						else
						{
							PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrderView(purchaseOrder.ID));
						}
					}
					catch
					{
						MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
					}

					break;

				case InventoryActionEnum.GenerateFromExisting:
					service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
					try
					{
						NewEmptyPO newPurchaseOrderResult = (NewEmptyPO)pages[2];
						RecordIdentifier purchaseOrderIDToCopy = ((ExistingPOSearch)pages[1]).PurchaseOrderID;
						PurchaseOrder purchaseOrder = new PurchaseOrder();

						purchaseOrder = newPurchaseOrderResult.PurchaseOrder;
						purchaseOrder.StoreID = newPurchaseOrderResult.StoreID;
						purchaseOrder.VendorID = (string)newPurchaseOrderResult.SelectedVendorID;
						purchaseOrder.CurrencyCode = newPurchaseOrderResult.CurrencyCode;
						purchaseOrder.Description = newPurchaseOrderResult.Description;
						purchaseOrder.DefaultDiscountPercentage = (decimal)newPurchaseOrderResult.DiscountPercentage;
						purchaseOrder.DefaultDiscountAmount = (decimal)newPurchaseOrderResult.DiscountAmount;
						purchaseOrder.DeliveryDate = newPurchaseOrderResult.DeliveryDate;
						purchaseOrder.OrderingDate = newPurchaseOrderResult.OrderingDate;
						purchaseOrder.Orderer = (Guid)PluginEntry.DataModel.CurrentUser.ID;
						purchaseOrder.PurchaseStatus = PurchaseStatusEnum.Open;

						RecordIdentifier newPurchaseOrderID =
							service.GeneratePurchaseOrderID(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true);

						Vendor vendor = service.GetVendor(PluginEntry.DataModel,
							PluginOperations.GetSiteServiceProfile(), purchaseOrder.VendorID, true);

						purchaseOrder.PurchaseOrderID = newPurchaseOrderID;

						service.CopyLinesAndBetweenMiscChargesPurchaseOrders(PluginEntry.DataModel,
							PluginOperations.GetSiteServiceProfile(), purchaseOrderIDToCopy, purchaseOrder,
							newPurchaseOrderResult.StoreID, vendor.TaxCalculationMethod, false);


						purchaseOrder = service.SaveAndReturnPurchaseOrder(PluginEntry.DataModel,
							PluginOperations.GetSiteServiceProfile(), purchaseOrder, true);

						PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrderView(purchaseOrder.ID));
					}
					catch
					{
						MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
					}

					break;

				case InventoryActionEnum.GenerateFromTemplate:
                    if(pages[1] is NewPOFromWorksheet)
                    {
                        NewPOFromWorksheet worksheetResult = (NewPOFromWorksheet)pages[1];
                        RecordIdentifier worksheet = worksheetResult.SelectedPurchaseWorksheetId;

                        if (PluginEntry.Framework.CanRunOperation("ViewPOWorksheet"))
                        {
                            PluginEntry.Framework.RunOperation("ViewPOWorksheet", this, new PluginOperationArguments(worksheet, null));
                        }
                    }
					else
                    {
                        try
                        {
                            NewEmptyPO newPOFromTemplateResult = (NewEmptyPO)pages[2];
                            PurchaseOrder purchaseOrderFromTemplate = PopulatePurchaseOrderHeader(newPOFromTemplateResult);

                            RecordIdentifier result = null;
                            SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage,
                                                                 () => result = PluginOperations.CreatePurchaseOrderFromTemplate(purchaseOrderFromTemplate, newPOFromTemplateResult.Template));
                            dlg.ShowDialog();


                            if (result != null)
                            {
                                PluginOperations.ShowPurchaseOrder(result);
                            }
                        }
                        catch
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                        }
                    }

					break;

				case InventoryActionEnum.GenerateFromFilter:
                    try
                    {
                        NewEmptyPO newPOFromFilterResult = (NewEmptyPO)pages[2];
                        newPOFromFilterResult.Filter.LimitRows = false;
                        newPOFromFilterResult.Filter.LimitToFirst50Rows = false;
                        PurchaseOrder purchaseOrderFromFilter = PopulatePurchaseOrderHeader(newPOFromFilterResult);

                        RecordIdentifier result = null;
                        SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage,
                                                             () => result = PluginOperations.CreatePurchaseOrderFromFilter(purchaseOrderFromFilter, newPOFromFilterResult.Filter));
                        dlg.ShowDialog();


                        if (result != null)
                        {
                            PluginOperations.ShowPurchaseOrder(result);
                        }
                    }
                    catch
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                    }

                    break;
				case InventoryActionEnum.Manage:
				case InventoryActionEnum.ExcelImport:
				case InventoryActionEnum.Other:
				case null:
					break;
			}
			PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
		}

        #endregion

        #region OnGoodsReceivingFinish

        private void OnGoodsReceivingFinish(List<IWizardPage> pages, ref bool cancelAction)
		{
			switch (inventoryTypeAction.Action)
			{
				case InventoryActionEnum.Manage:
				case InventoryActionEnum.GenerateFromExisting:
				case InventoryActionEnum.GenerateFromTemplate:
				case InventoryActionEnum.GenerateFromFilter:
				case InventoryActionEnum.ExcelImport:
				case InventoryActionEnum.Other:
				case InventoryActionEnum.New:
					throw new NotImplementedException("This action has not been implemented");
				case null:
					break;
			}
		}

        #endregion

        #region OnInventoryJournalFinish

        // Keep this in sync with autotest action CreateInventoryJournal
        private void OnInventoryJournalFinish(List<IWizardPage> pages, ref bool cancelAction)
		{
			try
			{
				NewEmptyIJ newJournalResult = inventoryTypeAction.Action == InventoryActionEnum.Manage 
												? (NewEmptyIJ)pages[1]
												: (NewEmptyIJ)pages[0];

				InventoryAdjustment newJournal = newJournalResult?.GetNewJournal();

				var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
				var savedJournal = service.SaveInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newJournal, true);
				
				PluginEntry.Framework.ViewController.Add(new Views.InventoryJournalsView(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
																						newJournal.JournalType, 
																						newJournal.ID, PluginEntry.DataModel.CurrentStoreID));

				PluginEntry.Framework.ViewController.NotifyDataChanged(
								this,
								DataEntityChangeType.Add,
								"InventoryJournal",
								savedJournal.ID,
								null);
			}
			catch
			{
				MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
			}
		}

		#endregion

		protected override void OnFinish(List<IWizardPage> pages, ref bool cancelAction)
		{
			switch (inventoryTypeAction.InventoryType)
			{
				case InventoryEnum.StockCounting:
					OnStockCountingFinish(pages, ref cancelAction);
					break;
				case InventoryEnum.PurchaseOrder:
					OnPurchaseOrderFinish(pages, ref cancelAction);
					break;
				case InventoryEnum.GoodsReceiving:
					OnGoodsReceivingFinish(pages, ref cancelAction);
					break;
				case InventoryEnum.InventoryJournal:
					OnInventoryJournalFinish(pages, ref cancelAction);
					break;
			}
		}

        private PurchaseOrder PopulatePurchaseOrderHeader(NewEmptyPO newPO)
        {
            return new PurchaseOrder
            {
                StoreID = newPO.StoreID,
                VendorID = (string)newPO.SelectedVendorID,
                CurrencyCode = newPO.CurrencyCode,
                Description = newPO.Description,
                DefaultDiscountPercentage = (decimal)newPO.DiscountPercentage,
                DefaultDiscountAmount = (decimal)newPO.DiscountAmount,
                DeliveryDate = newPO.DeliveryDate,
                OrderingDate = newPO.OrderingDate
            };
        }
	}
}