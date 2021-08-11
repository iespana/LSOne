using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Inventory.Properties;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory
{
	internal partial class PluginOperations
	{
		private static SiteServiceProfile siteServiceProfile;

		public static bool TestSiteService(bool displayMsg = true)
		{
			return TestSiteService(PluginEntry.DataModel, displayMsg).Item1;
		}

		public static ValueTuple<bool, string> TestSiteService(IConnectionManager entry, bool displayMsg = true)
		{
			bool serviceActive = false;
			bool serviceValid;

			ConnectionEnum result = ConnectionEnum.DatabaseConnectionFailed;
			string message = Resources.CouldNotConnectToStoreServer + "\r\n" + Resources.PressF1ForFurtherInformation;

			SiteServiceProfile ssProfile = GetSiteServiceProfile(entry);
			bool serviceConfigured = ssProfile != null;
			if (serviceConfigured)
			{
				IInventoryService service = (IInventoryService)entry.Service(ServiceType.InventoryService);
				result = service.TestConnection(entry, ssProfile.SiteServiceAddress, (ushort)ssProfile.SiteServicePortNumber);
				PluginEntry.Framework.ViewController.CurrentView.HideProgress();

				switch (result)
				{
					case ConnectionEnum.Success:
						serviceActive = true;
						message = string.Empty;
						break;
					case ConnectionEnum.ExternalConnectionFailed:
						message = Resources.CouldNotConnectToExternalService;
						break;
					case ConnectionEnum.ClientTimeNotSynchronized:
						message = Properties.Resources.ClientTimeNotSynchronizedMessage;
						break;
					case ConnectionEnum.ConnectionFailed:
					default:
						break;
				}
			}
			else
			{
				message = Resources.NoStoreServerIsSetUp;
			}

			serviceValid = serviceActive && serviceConfigured;
			if (!serviceValid)
			{
				if (displayMsg)
				{
					MessageDialog.Show(message);
				}

				if (result == ConnectionEnum.ExternalConnectionFailed)
				{
					IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ExternalSystem", null);

					if (plugin != null)
					{
						plugin.Message(null, "ViewExternalSytemSettingsView", null);
					}
				}
				else
				{
					IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

					if (plugin != null)
					{
						plugin.Message(null, "ViewSiteServiceTab", null);
					}
				}	
			}

			return ValueTuple.Create(serviceValid, message);
		}

		internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
		{
			TabControl.Tab tab;

			switch (args.ContextName)
			{
				case "LSOne.ViewPlugins.RetailItems.Views.ItemView":
					tab = new TabControl.Tab(Resources.Inventory, ItemTabKey.Inventory.ToString(), ViewPages.ItemInventoryPage.CreateInstance);
					if (((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.MasterItem || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.Service || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.AssemblyItem || args.IsMultiEdit)
					{
						tab.Enabled = false;
					}
					// Tab is disabled for multiedit

					args.Add(tab, 120);

					if(PluginEntry.DataModel.HasPermission(Permission.ManageVendorsOnItems))
					{
						tab = new TabControl.Tab(Resources.Vendors, ViewPages.ItemVendorPage.CreateInstance);
						if (((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.MasterItem || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.Service || ((RetailItem)args.InternalContext).ItemType == ItemTypeEnum.AssemblyItem)
						{
							tab.Enabled = false;
						}
						args.Add(tab, 125);
					}
					break;
				case "LSOne.ViewPlugins.Administration.Views.AdministrationView":
					if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
					{
						args.Add(new TabControl.Tab(Resources.InventorySettings, "InventorySettingsTab", ViewPages.InventoryAdminPage.CreateInstance), 90);
					}
					break;
				case "LSOne.ViewPlugins.Store.Views.StoreView":
					args.Add(new TabControl.Tab(Resources.Inventory, "StoreInventoryTab", ViewPages.StoreInventoryPage.CreateInstance), 280);
					break;
				case "LSOne.ViewPlugins.Store.Views.InventoryJournalView":
					args.Add(new TabControl.Tab(Resources.Items, "InventoryJournalItemsTab", ViewPages.InventoryJournalItemsPage.CreateInstance), 300);
					args.Add(new TabControl.Tab(Resources.Summary, "InventoryJournalSummaryTab", ViewPages.InventoryJournalSummaryPage.CreateInstance), 310);
					break;
			}
		}             

		public static void ShowStockLevelReport(object sender, EventArgs args)
		{
			PluginEntry.Framework.ViewController.Add(new Views.StockLevelReportView());
		}

		public static void ShowInventoryInTransitView(object sender, EventArgs args)
		{
			if (TestSiteService())
			{
				PluginEntry.Framework.ViewController.Add(new Views.InventoryInTransitView());
			}
		}               

		
		internal static bool LineInPreviousLines(List<InventoryJournalTransaction> oldLines, RecordIdentifier itemId,
										 RecordIdentifier variantId, RecordIdentifier unitId, RecordIdentifier storeId)
		{
			foreach (InventoryJournalTransaction line in oldLines)
			{
				RecordIdentifier lineStoreId = line.StoreId;
				if (line.ItemId == itemId && line.UnitID == unitId && lineStoreId == storeId)
				{
					return true;
				}
			}

			return false;
		}

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        internal static bool UnitConversionWithInventoryUnitExists(DataEntity itemDataEntity, RecordIdentifier unitId)
		{
			RecordIdentifier itemInventoryUnit = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemDataEntity.ID, RetailItem.UnitTypeEnum.Inventory);
			while (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemDataEntity.ID, unitId, itemInventoryUnit))
			{
				IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(null, "CanAddUnitConversionsSiteService", null);

				if (unitConversionAdder != null)
				{
					if (QuestionDialog.Show(Resources.UnitConversionQuestion, Resources.UnitConversionRuleMissing) == DialogResult.Yes)
					{
						if (!(bool)unitConversionAdder.Message(null, "AddUnitConversionIncludeSiteService", new object[] { itemDataEntity, itemInventoryUnit, unitId }))
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
				else
				{
					MessageDialog.Show(Resources.UnitConversionRuleMissingAlert);
					return false;
				}
			}

			return true;
		}

		internal static bool UnitConversionExists(DataEntity itemDataEntity, RecordIdentifier fromUnitID, RetailItem.UnitTypeEnum fromUnitModuleType, RecordIdentifier itemInventoryUnitID, bool invUnitIsNew = false)
		{
			bool oldInvUnitToNewUnitRuleExists = Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, itemDataEntity.ID, fromUnitID, itemInventoryUnitID);
			if (!oldInvUnitToNewUnitRuleExists)
			{
				string unitConversionQuestion = "";
				switch (fromUnitModuleType)
				{
					case RetailItem.UnitTypeEnum.Inventory:
						unitConversionQuestion = Resources.UnitConversionMissingOldIuToNewIu;
						break;
					case RetailItem.UnitTypeEnum.Purchase:
						unitConversionQuestion = (invUnitIsNew ? Resources.UnitConversionMissingPuToNewIu : Resources.UnitConversionMissingPuToOldIu);
						break;
					case RetailItem.UnitTypeEnum.Sales:
						unitConversionQuestion = (invUnitIsNew ? Resources.UnitConversionMissingSuToNewIu : Resources.UnitConversionMissingSuToOldIu);
						break;
				}

				if (QuestionDialog.Show(unitConversionQuestion, Resources.UnitConversionRuleMissing) == DialogResult.Yes)
				{
					IPlugin unitConversionAdder = PluginEntry.Framework.FindImplementor(null, "CanAddUnitConversionsSiteService", null);

					if (unitConversionAdder != null)
					{
						UnitConversion unitConversionAdded = (UnitConversion)unitConversionAdder.Message(null, "AddUnitConversionNoSave", new object[] { itemDataEntity, itemInventoryUnitID, fromUnitID });

						oldInvUnitToNewUnitRuleExists = unitConversionAdded != null;

						if (!oldInvUnitToNewUnitRuleExists)
						{
							string errorMessage = "";
							switch (fromUnitModuleType)
							{
								case RetailItem.UnitTypeEnum.Inventory:
									errorMessage = Resources.UnitConvRuleOldInvToNewInvMissingError;
									break;
								case RetailItem.UnitTypeEnum.Purchase:
									errorMessage = (invUnitIsNew ? Resources.UnitConvRuleNewPurchaseToNewInvMissingError : Resources.UnitConvRuleNewPurchaseToOldInvMissingError);
									break;
								case RetailItem.UnitTypeEnum.Sales:
									errorMessage = (invUnitIsNew ? Resources.UnitConvRuleNewSalesToNewInvMissingError : Resources.UnitConvRuleNewSalesToOldInvMissingError);
									break;
							}
							MessageDialog.Show(errorMessage);
						}
					}
				}
			}

			return oldInvUnitToNewUnitRuleExists;
		}        

		internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
		{
			args.Add(new Category(Resources.GeneralSetup, "General setup", null), 100);
		}

		internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
		{
			if (args.CategoryKey == "General setup")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.VendorView)
					|| PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders)
					|| PluginEntry.DataModel.HasPermission(Permission.StockCounting)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments)
					|| PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageStockReservations)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory)
					)
				{
					args.Add(new Item(Resources.Inventory, "Inventory", null), 300);
				}
			}
		}

		internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
		{
			if (args.CategoryKey == "General setup" && args.ItemKey == "Inventory")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
					|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores))
				{
					args.Add(new ItemButton(Resources.InventoryAdjustments, Resources.InventoryAdjustmentDescription, ShowInventoryAdjustmentsView), 100);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageStockReservations))
				{
					args.Add(new ItemButton(Resources.StockReservation, Resources.StockReservationDescription, ShowStockReservationsView), 200);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
				{
					args.Add(new ItemButton(Resources.ParkedInventory, Resources.ParkedInventoryTooltipDescription, ShowParkedInventoryView), 250);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.StockCounting))
				{
					args.Add(new ItemButton(Resources.StockCounting, Resources.StockCountingDescription, ShowStockCountingWizard), 300);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
				{
					args.Add(new ItemButton(Resources.PurchaseOrders, Resources.PurchaseOrdersDescription, ShowPurchaseOrderWizard), 400);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments))
				{
					args.Add(new ItemButton(Resources.GoodsReceivingDocuments, Resources.GoodsReceivingDocumentDescription, ShowGoodsReceivingDocuments), 500);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.VendorView))
				{
					args.Add(new ItemButton(Resources.ViewAllVendors, Resources.ViewAllVendorsDescription, ShowVendorsView), 600);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests))
				{
					args.Add(
						new ItemButton(Resources.TransferRequests, Resources.InventoryRequestsDescription,
									   ShowInventoryTransferRequestWizard), 700);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders))
				{
					args.Add(
						new ItemButton(Resources.TransferOrders, Resources.InventoryOrdersDescription,
									   ShowInventoryTransferOrderWizard), 800);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests) ||
					PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests) ||
					PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders) ||
					PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders))
				{
					args.Add(new ItemButton(Resources.InventoryInTransit, Resources.InventoryInTransitDescription, ShowInventoryInTransitView), 900);
				}
			}
		}

		internal static void AddRibbonPages(object sender, RibbonPageArguments args)
		{
			args.Add(new Page(Resources.Inventory, "Inventory"), 400);
		}

		internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
		{
			if (args.PageKey == "Inventory")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.VendorView) ||
					PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments) ||
					PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders) ||
					PluginEntry.DataModel.HasPermission(Permission.StockCounting) ||
					PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments)
					)
				{
					args.Add(new PageCategory(Resources.Purchase, "Purchase"), 100);
					args.Add(new PageCategory(Resources.Transfers, "Transfers"), 200);
					args.Add(new PageCategory(Resources.Stock, "Stock"), 300);
					args.Add(new PageCategory(Resources.Journals, "Journals"), 500);
				}
			}
		}

		internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
		{
			if (args.PageKey == "Inventory" && args.CategoryKey == "Purchase")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
				{
					args.Add(new CategoryItem(
							Resources.PurchaseOrders,
							Resources.PurchaseOrders,
							Resources.PurchaseOrdersTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							null,
							Resources.purchase_orders_32,
							ShowPurchaseOrderWizard,
							"PurchaseOrders"), 10);
				}
				if (PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments))
				{
					args.Add(new CategoryItem(
							Resources.Receiving,
							Resources.Receiving,
							Resources.GoodsReceivingDocumentTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							null,
							Resources.receiving_32,
							ShowGoodsReceivingDocuments,
							"GoodsReceivingDocuments"), 20);
				}
				if (PluginEntry.DataModel.HasPermission(Permission.VendorView))
				{
					args.Add(new CategoryItem(
							Resources.Vendors,
							Resources.Vendors,
							Resources.VendorsTooltipDescription,
							CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
							Resources.vendors_16,
							null,
							ShowVendorsView,
							"ViewAllVendors"), 30);
				}
			}
			if (args.PageKey == "Inventory" && args.CategoryKey == "Transfers")
			{

				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests))
				{
					args.Add(new CategoryItem(
							Resources.Requests,
							Resources.TransferRequests,
							Resources.InventoryRequestsTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							Resources.requests_16,
							null,
							ShowInventoryTransferRequestWizard,
							"inventoryTransferRequests"), 10);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders))
				{
					args.Add(new CategoryItem(
							Resources.Orders,
							Resources.TransferOrders,
							Resources.InventoryOrdersTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							Resources.orders_16,
							null,
							ShowInventoryTransferOrderWizard,
							"inventoryTransferOrders"), 20);
				}
				if (PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests) ||
					PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests) ||
					PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders) ||
					PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders))
				{
					args.Add(new CategoryItem(
						Resources.InTransit,
						Resources.InventoryInTransit,
						Resources.InventoryInTransitTooltipDescription,
						CategoryItem.CategoryItemFlags.Button,
						Resources.in_transit_16,
						null,
						ShowInventoryInTransitView,
						"inventoryInTransit"), 30);
				}
			}
			if (args.PageKey == "Inventory" && args.CategoryKey == "Stock")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.StockCounting))
				{
					args.Add(new CategoryItem(
							Resources.Counting,
							Resources.StockCounting,
							Resources.StockCountingTooltipDescription,
							 CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
							null,
							null,
							ShowStockCountingWizard,
							"StockCounting"), 20);
				}

				if(PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
				{
					args.Add(new CategoryItem(
						Resources.ReasonCodes,
						Resources.ReasonCodes,
						Resources.ReasonCodesTooltipDescription,
						CategoryItem.CategoryItemFlags.Button,
						ShowReasonCodesView,
						"ReasonCodes"), 50);
				}
			}

			if (args.PageKey == "Inventory" && args.CategoryKey == "Journals")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
						|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores)
						|| PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments)
						|| PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments)
						|| PluginEntry.DataModel.HasPermission(Permission.ManageStockReservations)
						|| PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
				{
					args.Add(new CategoryItem(
							Resources.Journals,
							Resources.JournalsTitle,
							Resources.JournalsTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							null,
							Resources.journals_32,
							ShowInventoryJournalWizard,
							"Journals"), 5);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
						|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores)
						|| PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments)
						|| PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments))
				{
					args.Add(new CategoryItem(
							Resources.Adjustments,
							Resources.InventoryAdjustments,
							Resources.InventoryAdjustmentTooltipDescription,
							CategoryItem.CategoryItemFlags.Button | CategoryItem.CategoryItemFlags.BeginOfGroup,
							null,
							null,
							ShowInventoryAdjustmentsView,
							"ViewJPL"), 10);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageStockReservations))
				{
					args.Add(new CategoryItem(
							Resources.Reservation,
							Resources.StockReservation,
							Resources.StockReservationTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							null,
							null,
							ShowStockReservationsView,
							"StockReservation"), 20);
				}

				if (PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
				{
					args.Add(new CategoryItem(
							Resources.Parked,
							Resources.ParkedInventory,
							Resources.ParkedInventoryTooltipDescription,
							CategoryItem.CategoryItemFlags.Button,
							null,
							null,
							ShowParkedInventoryView,
							"ParkedInventory"), 30);
				}
			}
		}        

		internal static void ConstructMenus(object sender, MenuConstructionArguments args)
		{
			
		}

		
		/// <summary>
		/// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
		public static SiteServiceProfile GetSiteServiceProfile(IConnectionManager entry)
		{
			if (siteServiceProfile == null)
			{
				Parameters parameters = Providers.ParameterData.Get(entry);
				if (parameters != null)
				{
					siteServiceProfile = Providers.SiteServiceProfileData.Get(entry, parameters.SiteServiceProfile);
				}
			}

			return siteServiceProfile;
		}

		/// <summary>
		/// Returns the selected site service profile for the Site Manager. If no site service profile has been selected then the function returns null
		/// </summary>
		/// <returns>The Site managers site service profile. Returns null if no site service profile has been selected</returns>
		public static SiteServiceProfile GetSiteServiceProfile()
		{
			if (siteServiceProfile == null)
			{
				Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
				if (parameters != null)
				{
					siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
				}
			}

			return siteServiceProfile;
		}
		/// <summary>
		/// Resets the site service profile so that it is read again from the database
		/// </summary>
		public static void ResetSiteServiceProfile()
		{
			siteServiceProfile = null;
		}

		public static List<string> GetFilesFromDefaultFolder(Setting workingFolder, List<string> fileTypeFilters)
		{
			List<string> fileNames = new List<string>();

			if (workingFolder != null && !string.IsNullOrEmpty(workingFolder.Value) && Directory.Exists(workingFolder.Value))
			{
				fileNames = Directory.GetFiles(workingFolder.Value).Where(s => fileTypeFilters.Any(s.EndsWith)).ToList();
			}
			return fileNames;
		}

		public static bool IsDefaultProfileValid(RecordIdentifier defaultProfileMasterId)
		{
			return Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, defaultProfileMasterId)
				.Any();
		}

		public static DataEntity CreateItemDataEntity(DualDataComboBox itemComboBox, DualDataComboBox variantComboBox)
		{
			if (variantComboBox.Enabled && variantComboBox.SelectedData != null
				&& !string.IsNullOrEmpty(variantComboBox.SelectedData.ID.StringValue))
			{
				return new DataEntity(GetReadableItemID(variantComboBox), variantComboBox.SelectedData.Text);
			}

			return new DataEntity(GetReadableItemID(itemComboBox), itemComboBox.SelectedData.Text);
		}

		/// <summary>
		/// Returns the readable item ID (not GUID) for the combobox that is displaying either an item list or a variant item list
		/// </summary>
		/// <param name="comboBox">The combobox that is to be checked</param>
		/// <returns>The item ID selected</returns>
		public static RecordIdentifier GetReadableItemID(DualDataComboBox comboBox)
		{
			if (comboBox.SelectedData is MasterIDEntity)
			{
				return (comboBox.SelectedData as MasterIDEntity).ReadadbleID;
			}

			return (comboBox.SelectedData as DataEntity).ID;
		}

		/// <summary>
		/// Returns the selected item ID. If the variant combo box is enabled and has a selection the ID for that item is returned
		/// otherwise the item from the item combo box is returned
		/// </summary>
		/// <param name="itemComboBox"></param>
		/// <param name="variantComboBox"></param>
		/// <returns></returns>
		public static RecordIdentifier GetSelectedItemID(DualDataComboBox itemComboBox, DualDataComboBox variantComboBox)
		{

			if (variantComboBox.SelectedData != null
				&& !string.IsNullOrEmpty(variantComboBox.SelectedData.ID.StringValue))
			{
				return GetReadableItemID(variantComboBox);

			}

			return GetReadableItemID(itemComboBox);
		}

		public static bool CheckRetailItem(RecordIdentifier itemID, out string error)
		{
			error = string.Empty;
			RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID, true);
			if (retailItem != null && !retailItem.InventoryExcluded)
			{
				return true;
			}

			try
			{
				retailItem = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetRetailItemIncludeDeleted(
					PluginEntry.DataModel,
					GetSiteServiceProfile(PluginEntry.DataModel),
					itemID, true);

				if (retailItem != null && !retailItem.InventoryExcluded)
				{
					return true;
				}

				if (retailItem == null)
				{
					error = Properties.Resources.InformationAboutItemCannotBeFound;
					return false;
				}

				if (retailItem.InventoryExcluded)
				{
					error = Properties.Resources.ItemCannotBeAdded;
					return false;
				}

				return false;
			}
			catch (Exception ex)
			{

				MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
				error = Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message;
				return false;
			}
		}

		/// <summary>
		/// Returns information about the item. If it cannot be found locally the site service will be used to retrieve information about the item
		/// </summary>
		/// <param name="itemID">The unique ID of the item to retrieve</param>
		/// <param name="displayErrorMsg"></param>
		/// <returns></returns>
		public static RetailItem GetRetailItem(RecordIdentifier itemID, bool displayErrorMsg = true)
		{
			RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
			if (retailItem != null)
			{
				return retailItem;
			}

			try
			{
				retailItem = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetRetailItem(
					PluginEntry.DataModel,
					GetSiteServiceProfile(PluginEntry.DataModel),
					itemID, true);

				if (retailItem != null && !retailItem.InventoryExcluded)
				{
					return retailItem;
				}

				if (displayErrorMsg && retailItem == null)
				{
					var result = MessageDialog.Show(Properties.Resources.InformationAboutItemCannotBeFound);
				}
				return null;
			}
			catch (Exception ex)
			{
				if (displayErrorMsg)
				{
					MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
				}
				return null;
			}
		}

		/// <summary>
		/// Zeroes out items in the inventory by creating an inventory adjustment.
		/// </summary>
		/// <param name="itemID"></param>
		/// <param name="storeID"></param>
		/// <param name="reasonID"></param>
		/// <returns></returns>
		public static void ZeroOutInventory(RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier reasonID)
		{
			if (itemID != RecordIdentifier.Empty
				&& (PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments) || PluginEntry.DataModel.HasPermission(Permission.ManageItemTypes)))
			{
				IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

				RetailItem item = PluginOperations.GetRetailItem(itemID);
				decimal itemOnHand = service.GetInventoryOnHand(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, storeID, false);
				if (itemOnHand == 0)
				{
					return;
				}

				List<ReasonCode> reasons = service.GetReasonCodes(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), InventoryJournalTypeEnum.Adjustment, false);
				DataEntity reason = reasons.FirstOrDefault(f => f.ID == reasonID);

				InventoryAdjustment journalEntry = new InventoryAdjustment();
				journalEntry.Text = reason.Text;
				journalEntry.StoreId = storeID;
				journalEntry.JournalType = InventoryJournalTypeEnum.Adjustment;
				journalEntry.PostedDateTime = Date.Empty;
				journalEntry.CreatedDateTime = DateTime.Now;

				InventoryAdjustment newAdjustmentJournal = service.SaveInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalEntry, true);

				InventoryJournalTransaction journalTransactionLine = new InventoryJournalTransaction();
				journalTransactionLine.JournalId = newAdjustmentJournal.ID;
				journalTransactionLine.StaffID = PluginEntry.DataModel.CurrentStaffID;
				journalTransactionLine.TransDate = DateTime.Now;
				journalTransactionLine.ItemId = itemID;
				journalTransactionLine.Adjustment = (-1) * itemOnHand;
				journalTransactionLine.AdjustmentInInventoryUnit = (-1) * itemOnHand;
				journalTransactionLine.ReasonId = reasonID;
				journalTransactionLine.ReasonText = reason.Text;
				journalTransactionLine.SetUnit(new Unit(item.InventoryUnitID, item.InventoryUnitName, 0, 0));

				service.PostInventoryAdjustmentLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalTransactionLine, storeID, InventoryTypeEnum.Adjustment, true);
				PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", itemID, journalTransactionLine);

				service.CloseInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newAdjustmentJournal.ID, true);

				service.Disconnect(PluginEntry.DataModel);
			}
		}
        
        /// <summary>
        /// Used to show the image bank from a view's context bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void ShowImageBankHandler(object sender, EventArgs e)
        {
            PluginOperationArguments args = new PluginOperationArguments(RecordIdentifier.Empty, ImageTypeEnum.Inventory, true);
            PluginEntry.Framework.RunOperation("ShowImageBank", null, args);
            PluginEntry.Framework.ViewController.Add(args.ResultView);
        }

        internal static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments args)
        {
            if (args.CategoryKey == "LSOne.ViewPlugins.Administration.Views.AdministrationView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    Type administrationType = args.View.GetType();
                    PropertyInfo info = administrationType.GetProperty("InventoryTabSelected");
                    bool inventoryTabSelected = (bool)info.GetValue(args.View);

                    if (inventoryTabSelected)
                    {
                        args.Add(new ContextBarItem(Resources.ArchiveItemCosts, ArchiveItemCosts), 400);
                    }
                }
            }
        }

        private static void ArchiveItemCosts(object sender, ContextBarClickEventArguments args)
        {
            if(MessageDialog.Show(Resources.ArchiveItemCostsQuestion, Resources.ArchiveItemCosts, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(TestSiteService())
                {
                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).ArchiveItemCosts(PluginEntry.DataModel, GetSiteServiceProfile(), true);
                }
            }
        }
    }
}
