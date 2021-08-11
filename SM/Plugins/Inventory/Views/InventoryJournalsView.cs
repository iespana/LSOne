using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Views
{
	public partial class InventoryJournalsView : ViewBase
	{
		private readonly IConnectionManager dlgEntry;
		private readonly SiteServiceProfile dlgSiteService;
		private readonly InventoryJournalTypeEnum journalType;

		private static Guid BarSettingID = new Guid("05DB608A-1472-4C00-BEF3-52D9146A98DE");
		private Setting searchBarSetting;

		private RecordIdentifier selectedID = "";
		private Store defaultStore;
		private Timer searchTimer;
		private List<InventoryAdjustment> journalList;

		protected override string LogicalContextName
		{
			get
			{
				string typeOfAdjustment;

				switch(journalType)
				{
					case InventoryJournalTypeEnum.Adjustment:
						typeOfAdjustment = Resources.InventoryAdjustments;
						break;
					case InventoryJournalTypeEnum.Reservation:
						typeOfAdjustment = Resources.StockReservations;
						break;
					case InventoryJournalTypeEnum.Parked:
						typeOfAdjustment = Resources.ParkedInventory;
						break;
					default:
						typeOfAdjustment = string.Empty;
						break;
				}

				return typeOfAdjustment;
			}
		}

		protected override HelpSettings GetOnlineHelpSettings()
		{
			switch (journalType)
			{
				case InventoryJournalTypeEnum.Adjustment:
					return new HelpSettings("InventoryAdjustmentsView");

				case InventoryJournalTypeEnum.Reservation:
					return new HelpSettings("StockReservationsView");

				case InventoryJournalTypeEnum.Parked:
					return new HelpSettings("ParkedInventoriesView");
				default:
					return new HelpSettings(this.Name);
			}
		}

		public override RecordIdentifier ID
		{
			get
			{
				// If our sheet would be multi-instance sheet then we would return context identifier UUID here,
				// such as User.GUID that identifies that particular User. For single instance sheets we return 
				// RecordIdentifier.Empty to tell the framework that there can only be one instace of this sheet, which will
				// make the framework make sure there is only one instance in the viewstack.
				return RecordIdentifier.Empty;
			}
		}

		protected InventoryJournalsView()
		{
			InitializeComponent();
		}

		public InventoryJournalsView(IConnectionManager entry, InventoryJournalTypeEnum journalType, RecordIdentifier defaultStoreId)
			: this(entry, PluginOperations.GetSiteServiceProfile(), journalType, RecordIdentifier.Empty, defaultStoreId)
		{ }

		public InventoryJournalsView(IConnectionManager entry, SiteServiceProfile profile, InventoryJournalTypeEnum journalType, RecordIdentifier selectedId, RecordIdentifier defaultStoreId)
			: this()
		{
			this.dlgEntry = entry;
			this.dlgSiteService = profile;
			this.journalType = journalType;

			this.selectedID = selectedId;

			Attributes = ViewAttributes.Audit |
				ViewAttributes.ContextBar |
				ViewAttributes.Close |
				ViewAttributes.Help;

			lvJournals.ContextMenuStrip = new ContextMenuStrip();
			lvJournals.ContextMenuStrip.Opening += new CancelEventHandler(lvJournals_Opening);

			lvJournals.Columns[0].Tag = InventoryAdjustmentSorting.Posted;
			lvJournals.Columns[1].Tag = InventoryAdjustmentSorting.ID; 
			lvJournals.Columns[2].Tag = InventoryAdjustmentSorting.Description;
			lvJournals.Columns[3].Tag = InventoryAdjustmentSorting.CreatedDateTime;
			lvJournals.Columns[4].Tag = InventoryAdjustmentSorting.StoreName;

			lvJournals.SetSortColumn(0, true);

			searchBar.BuddyControl = lvJournals;
			searchBar.FocusFirstInput();

			defaultStore = null;
			if (!string.IsNullOrWhiteSpace((string)defaultStoreId))
			{
				if (dlgEntry.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores))
				{
					searchBar.DefaultNumberOfSections = 2;
				}
				defaultStore = Providers.StoreData.Get(dlgEntry, defaultStoreId);
			}

			searchTimer = new Timer();
			searchTimer.Tick += SearchTimerOnTick;
			searchTimer.Interval = 1;

			itemDataScroll.PageSize = dlgEntry.PageSize;
			itemDataScroll.Reset();

			HeaderText = LogicalContextName;

			if(journalType == InventoryJournalTypeEnum.Parked)
			{
				btnMoveToInventory.Visible = btnMoveToInventory.Enabled = true;
			}
		}

#region ViewBase

		protected override void LoadData(bool isRevert)
		{
			// We don't load the items through this method, but by using the timer
			// We do this because the data is loaded before the filter, causing the data to be loaded incorrectly
		}

		protected override bool DataIsModified()
		{
			return false;
		}

		protected override bool SaveData()
		{
			return true;
		}

		protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
		{

			if (arguments.CategoryKey == GetType() + ".View")
			{
				if (journalType == InventoryJournalTypeEnum.Parked && PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
				{
					arguments.Add(new ContextBarItem(Resources.MoveToMainInventory, "MoveToMainInventory", true, btnMoveToInventory_Click), 5000000);
				}
				else if (journalType == InventoryJournalTypeEnum.Adjustment 
						&& (PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
								|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustments)
								|| PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores)
								|| PluginEntry.DataModel.HasPermission(Permission.ViewInventoryAdjustments)
								|| PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments)))
				{
					arguments.Add(new ContextBarItem(Resources.ArchiveDocument, "ArchiveDocument", true, lnkArchiveDocument_Click), 5000000);
				}
			}
		}

		public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
		{
			if (objectName == "InventoryJournal" && changeAction == DataEntityChangeType.Add)
			{
				selectedID = changeIdentifier;
				LoadItems();
			}
		}

		#endregion

		private void searchBar_SetupConditions(object sender, EventArgs e)
		{
			List<object> statusList = new List<object>();
			statusList.Add(Resources.AllStatuses);
			statusList.Add(Resources.SearchBar_Value_Active);
			statusList.Add(journalType == InventoryJournalTypeEnum.Adjustment ? Resources.SearchBar_Value_Archived : Resources.SearchBar_Value_Posted);

			searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 1, 0, false));
			if (dlgEntry.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores))
			{
				searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
			}
			searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
			searchBar.AddCondition(new ConditionType(Resources.SearchBar_CreatedDate, "CreatedDate", ConditionType.ConditionTypeEnum.DateRange));

			searchBar_LoadDefault(this, EventArgs.Empty);

			searchTimer.Enabled = true;
			searchTimer.Start();
		}

		private void searchBar_LoadDefault(object sender, EventArgs e)
		{
			searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

			if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
			{
				searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
			}
		}

		private void searchBar_SaveAsDefault(object sender, EventArgs e)
		{
			string layoutString = searchBar.LayoutStringWithData;

			if (searchBarSetting.LongUserSetting != layoutString)
			{
				searchBarSetting.LongUserSetting = layoutString;
				searchBarSetting.UserSettingExists = true;

				PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
			}

			ShowTimedProgress(searchBar.GetLocalizedSavingText());
		}

		private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
		{
			switch (args.TypeKey)
			{
				case "Store":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData =  defaultStore == null && dlgEntry.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores) ? 
																			new DataEntity(null, Resources.AllStores) : 
																			new DataEntity(defaultStore.ID, defaultStore.Text);
					((DualDataComboBox)args.UnknownControl).RequestData += Store_RequestData;
					break;
			}
		}

		// Required to prevent some memory leak
		private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
		{
			switch (args.TypeKey)
			{
				case "Store":
					((DualDataComboBox)args.UnknownControl).RequestData -= Store_RequestData;
					break;
			}
		}

		private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
		{
			switch (args.TypeKey)
			{
				case "Store":
					args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
					break;
			}
		}

		private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
		{
			switch (args.TypeKey)
			{
				case "Store":
					args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
					break;
			}
		}

		private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
		{
			DataEntity entity = null;
			switch (args.TypeKey)
			{
				case "Store":
					entity = Providers.StoreData.Get(dlgEntry, args.Selection) ?? new DataEntity(null, Resources.AllStores);
					break;
			}
			((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
		}

		private void searchBar_SearchClicked(object sender, EventArgs e)
		{
			itemDataScroll.Reset();
			LoadItems();
		}

		private void Store_RequestData(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;
			List<DataEntity> stores = Providers.StoreData.GetList(dlgEntry);
			stores.Insert(0, new DataEntity(null, Resources.AllStores));
			((DualDataComboBox)sender).SetData(stores, null);
		}

		private void lvJournals_Opening(object sender, CancelEventArgs e)
		{
			ExtendedMenuItem item;
			ContextMenuStrip menu;

			menu = lvJournals.ContextMenuStrip;

			menu.Items.Clear();
			
			item = new ExtendedMenuItem(
					Resources.Edit,
					100,
					new EventHandler(btnsEditAddRemove_EditButtonClicked));

			item.Image = ContextButtons.GetEditButtonImage();
			item.Enabled = btnsEditAddRemove.EditButtonEnabled;
			item.Default = true;

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Resources.Add,
					200,
					new EventHandler(btnsEditAddRemove_AddButtonClicked));

			item.Enabled = btnsEditAddRemove.AddButtonEnabled;

			item.Image = ContextButtons.GetAddButtonImage();

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Resources.Delete,
					300,
					new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

			item.Image = ContextButtons.GetRemoveButtonImage();
			item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

			menu.Items.Add(item);

			if (journalType == InventoryJournalTypeEnum.Parked && PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
			{
				var lines = GetSelectedOpenJournals();
				if (lines != null && lines.Count > 0)
				{
					item = new ExtendedMenuItem(
						Resources.MoveToMainInventory,
						400,
						new EventHandler(btnMoveToInventory_Click));

					item.Enabled = btnMoveToInventory.Enabled;
					menu.Items.Add(item);
				}
			}

			PluginEntry.Framework.ContextMenuNotify("InventoryJournal", lvJournals.ContextMenuStrip, lvJournals);

			e.Cancel = (menu.Items.Count == 0);
		}

		private void lvJournals_SelectionChanged(object sender, EventArgs e)
		{
			btnsEditAddRemove.EditButtonEnabled = lvJournals.Selection.Count == 1;
			btnsEditAddRemove.RemoveButtonEnabled = lvJournals.Selection.Count > 0;

			if (lvJournals.Selection.Count == 1)
			{
				var selectedJournal = (InventoryAdjustment)lvJournals.Row(lvJournals.Selection.FirstSelectedRow).Tag;
				selectedID = selectedJournal.ID;

				btnsEditAddRemove.RemoveButtonEnabled = (selectedJournal.Posted != InventoryJournalStatus.Posted);
			}
			else if (lvJournals.Selection.Count > 1)
			{
				selectedID = null;
			}
		}

		private void lvJournals_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
		{
			if (btnsEditAddRemove.EditButtonEnabled)
			{
				btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
			}
		}

		private void lvJournals_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
		{
			if (lvJournals.SortColumn == args.Column)
			{
				lvJournals.SetSortColumn(args.Column, !lvJournals.SortedAscending);
			}
			else
			{
				lvJournals.SetSortColumn(args.Column, true);
			}

			itemDataScroll.Reset();
			LoadItems();
		}

		private void itemDataScroll_PageChanged(object sender, EventArgs e)
		{
			LoadItems();
		}

		private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
		{
			if (PluginOperations.TestSiteService())
			{
				InventoryTypeAction inventoryTypeAction = new InventoryTypeAction
				{
					InventoryType = InventoryEnum.InventoryJournal,
					Action = InventoryActionEnum.New
				};

				PluginOperations.ShowInventoryJournalWizard(inventoryTypeAction, journalType);
			}
		}

		private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
		{
			PluginOperations.ShowInventoryJournalView(journalType, selectedID);
		}

		private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
		{
			if (lvJournals.Selection.Count == 0)
				return;

			try
			{
				if(PluginOperations.TestSiteService(displayMsg : true))
				{
					var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

					List<RecordIdentifier> selectedJournals = GetSelectedJournalIDs();
					bool deleteSucceeded = true;
					foreach (var journalID in selectedJournals)
					{
						bool journalDeleted = service.DeleteInventoryJournal(dlgEntry, dlgSiteService, journalID, true);
						deleteSucceeded &= journalDeleted;
					}

					if (!deleteSucceeded)
					{
						MessageDialog.Show(selectedJournals.Count > 1 ? Resources.UnableToDeleteInventoryJournals : Resources.UnableToDeleteInventoryJournal);
					}

					LoadItems();
				}
			}
			catch (Exception ex)
			{
				MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
			}
		}

		private void btnMoveToInventory_Click(object sender, EventArgs e)
		{
			List<InventoryAdjustment> moveJournals = GetSelectedOpenJournals();

			if(moveJournals.Count == 0)
			{
				MessageDialog.Show(Resources.SelectItemToMoveToInventory);
				return;
			}

			DialogResult result;
			if(moveJournals.Count > 1)
			{
				result = MessageDialog.Show(Resources.MoveToInventoryMultipleLines, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (result == DialogResult.Cancel)
				{
					return;
				}
			}

			using (SelectReasonCodeDialog dlg = new SelectReasonCodeDialog(ReasonActionEnum.MainInventory))
			{
				result = dlg.ShowDialog();
				if (result == DialogResult.Cancel)
				{
					return;
				}

				PluginOperations.MoveToInventory(moveJournals, dlg.SelectedReasonCode);
			}

			LoadItems();
		}

		private void lnkArchiveDocument_Click(object sender, EventArgs e)
		{
			List<RecordIdentifier> journalsToArchive = new List<RecordIdentifier>();

			for (int i = 0; i < lvJournals.Selection.Count; i++)
			{
				InventoryAdjustment journal = (InventoryAdjustment)lvJournals.Selection[i].Tag;
				if (journal.Posted != InventoryJournalStatus.Posted)
				{
					journalsToArchive.Add(journal.ID);
				}
			}

			if (!journalsToArchive.Any())
			{
				MessageDialog.Show(Resources.SelectItemsForArchive);
				return;
			}

			try
			{
				IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

				foreach (RecordIdentifier journalID in journalsToArchive)
				{
					service.CloseInventoryAdjustment(dlgEntry, dlgSiteService, journalID, true);
				}

				LoadItems();
			}
			catch
			{
				MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
			}
		}

		private InventoryJournalSearch CreateSearchCriteria()
		{
			List<SearchParameterResult> results = searchBar.SearchParameterResults;

			InventoryJournalSearch searchCriteria = new InventoryJournalSearch();
			searchCriteria.StoreID =    PluginEntry.DataModel.IsHeadOffice || dlgEntry.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores) ? 
										RecordIdentifier.Empty : 
										PluginEntry.DataModel.CurrentStoreID;

			foreach (SearchParameterResult result in results)
			{
				switch (result.ParameterKey)
				{
					case "Store":
						if (dlgEntry.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores))
						{
							searchCriteria.StoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						}
						break;
					case "Description":
						searchCriteria.Description = new List<string> { result.StringValue };
						searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;
					case "Status":
						switch (result.ComboSelectedIndex)
						{
							case 1:
								searchCriteria.Status = (int)InventoryJournalStatus.Active;
								break;
							case 2:
								searchCriteria.Status = (int)InventoryJournalStatus.Posted;
								break;
							default:
								searchCriteria.Status = null;
								break;
						}
						break;
					case "CreatedDate":
						if (result.Date.Checked)
						{
							searchCriteria.CreatedDateFrom.DateTime = result.Date.Value.Date;
						}

						if (result.DateTo.Checked)
						{
							searchCriteria.CreatedDateTo.DateTime = result.DateTo.Value.Date.AddDays(1); 
						}
						break;
				}
			}

			return searchCriteria;
		}

		private List<InventoryAdjustment> GetSelectedOpenJournals()
		{
			if (lvJournals.Selection.Count == 0)
			{
				return new List<InventoryAdjustment>();
			}

			List<InventoryAdjustment> selection = new List<InventoryAdjustment>();

			for (int i = 0; i < lvJournals.Selection.Count; i++)
			{
				InventoryAdjustment line = (InventoryAdjustment)lvJournals.Selection[i].Tag;
				if(line.Posted != InventoryJournalStatus.Posted && line.Posted != InventoryJournalStatus.Closed)
				{
					selection.Add(line);
				}
			}

			return selection;
		}

		private List<RecordIdentifier> GetSelectedJournalIDs()
		{
			if (lvJournals.Selection.Count == 0)
			{
				return new List<RecordIdentifier>();
			}

			List<RecordIdentifier> selection = new List<RecordIdentifier>();

			for (int i = 0; i < lvJournals.Selection.Count; i++)
			{
				InventoryAdjustment line = (InventoryAdjustment)lvJournals.Selection[i].Tag;
				selection.Add(line.ID);
			}

			return selection;
		}

		private void LoadItems()
		{
			lvJournals.ClearRows();

			if (lvJournals.SortColumn == null)
			{
				lvJournals.SetSortColumn(lvJournals.Columns[0], true);
			}

			InventoryAdjustmentSorting sortBy = (InventoryAdjustmentSorting)lvJournals.SortColumn.Tag;
			bool sortBackwards = !lvJournals.SortedAscending;
			InventoryJournalSearch searchCriteria = CreateSearchCriteria();

			var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			try
			{
				int itemsCount = 0;
				int startRecord = itemDataScroll.StartRecord;
				int endRecord = itemDataScroll.EndRecord + 1;

				journalList = service.AdvancedSearch(dlgEntry, dlgSiteService,
																journalType,
																searchCriteria,
																sortBy, sortBackwards,
																startRecord, endRecord,
																out itemsCount,
																true);
				itemDataScroll.RefreshState(journalList, itemsCount);

				Row row;
				foreach (InventoryAdjustment item in journalList)
				{
					row = new Row();

					Bitmap statusImage = null;
					switch (item.Posted)
					{
						case InventoryJournalStatus.Posted:
							statusImage = Resources.dot_finished_16;
							break;
						case InventoryJournalStatus.PartialPosted:
							statusImage = Resources.dot_yellow_16;
							break;
						default:
							statusImage = Resources.dot_grey_16;
							break;
					}
					row.AddCell(new ExtendedCell(string.Empty, statusImage));

					row.AddText((string)item.ID);
					row.AddText(item.Text);
					row.AddText(item.CreatedDateTime.ToShortDateString() + " " + item.CreatedDateTime.ToShortTimeString());
					row.AddText(item.StoreName);

					row.Tag = item;

					lvJournals.AddRow(row);

					if (selectedID == item.ID)
					{
						lvJournals.Selection.Set(lvJournals.RowCount - 1);
					}
				}

				lvJournals.AutoSizeColumns();
				lvJournals_SelectionChanged(this, EventArgs.Empty);
			}
			catch
			{
				MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
			}
		}

		private void SearchTimerOnTick(object sender, EventArgs eventArgs)
		{
			searchTimer.Stop();
			searchTimer.Enabled = false;
			LoadItems();
		}
	}
}