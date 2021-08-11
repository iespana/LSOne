using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;

namespace LSOne.ViewPlugins.RetailItems.Views
{
	public partial class ItemsView : ViewBase
	{
		private static Guid SortSettingID = new Guid("275a14a0-b404-11e2-9e96-0800200c9a66");
		private static Guid DisplayVariantsSettingID = new Guid("50824dc8-e2c1-433b-b19b-43efbcc12ec2");
		private static Guid BarSettingID = new Guid("e87c7650-b17f-11e2-9e96-0800200c9a66");

		private Dictionary<RecordIdentifier, string> taxGroupNameCache;
		private Dictionary<RecordIdentifier, string> retailGroupCache;
		private Dictionary<RecordIdentifier, string> retailDepartmentCache;
		List<SimpleRetailItem> items;
		RecordIdentifier selectedID = "";
		Setting sortSetting;
		Setting displayVariantItemsSetting;
		private bool lockEvents = false;

		private Setting searchBarSetting;
		private Store priceStore;
		private bool actualSearchOptionValue;

		private Image variantItem16;
		private Image retailItemImage16;
		private Image masterItem16;
		private Image serviceItemImage16;
        private Image assemblyItemImage16;

		private SortEnum secondarySort;

		public ItemsView(RecordIdentifier selectedID)
			: this()
		{
			this.selectedID = selectedID;
		}

		private Dictionary<RecordIdentifier, string> ProcessDataEntityListToDictionary(List<MasterIDEntity> list)
		{
			Dictionary<RecordIdentifier, string> reply = new Dictionary<RecordIdentifier, string>();
			foreach (DataEntity entity in list)
			{
				reply.Add(entity.ID,entity.Text);
			}
			return reply;
		} 

		public ItemsView()
		{
			variantItem16 = Resources.itemVariant_16;
			retailItemImage16 = Resources.item_16;
			masterItem16 = Resources.header_item;
			serviceItemImage16 = Resources.service_items_16;
			assemblyItemImage16 = Resources.itemassembly_16px;
            actualSearchOptionValue = false;

			InitializeComponent();

			if (PluginEntry.DataModel.IsHeadOffice)
			{
				Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
				priceStore = Providers.StoreData.Get(PluginEntry.DataModel, parameters.LocalStore);
			}
			else
			{
				priceStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
			}

			taxGroupNameCache = Providers.TaxItemData.GetItemTaxGroupDictionary(PluginEntry.DataModel);
			retailGroupCache = ProcessDataEntityListToDictionary(Providers.RetailGroupData.GetMasterIDList(PluginEntry.DataModel));
			retailDepartmentCache = ProcessDataEntityListToDictionary(Providers.RetailDepartmentData.GetMasterIDList(PluginEntry.DataModel));

			Attributes = ViewAttributes.ContextBar | ViewAttributes.Close | ViewAttributes.Help;

			lvItems.ContextMenuStrip = new ContextMenuStrip();
			lvItems.ContextMenuStrip.Opening += lvItems_Opening;

			ReadOnly = !PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit);
			btnsContextButtons.AddButtonEnabled = !ReadOnly;

			searchBar1.BuddyControl = lvItems;

			itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
			itemDataScroll.Reset();

			searchBar1.FocusFirstInput();

			// Allow plugins to add to the Export and import buttons button
			PluginEntry.Framework.ContextMenuNotify("RetailExportButton", exportContextMenu, this);
			PluginEntry.Framework.ContextMenuNotify("RetailImportButton", importContextMenu, this);

			if (exportContextMenu.Items.Count > 0)
			{
				btnExport.Visible = true;
			}

			if (importContextMenu.Items.Count > 0)
			{
				btnImport.Visible = true;
			}            
		}

		private void SetSecondarySort(short secondarySortColumn, bool sortedAscending)
		{
			switch (secondarySortColumn)
			{
				case -1:
					secondarySort = SortEnum.None;
					break;
				case 2:
					secondarySort = SortEnum.VariantName;
					break;
				default:
					secondarySort = SortEnum.Description;
					break;
			}
			if (!sortedAscending && secondarySort != SortEnum.None)
			{
				secondarySort += 100;
			}
		}


		public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
		{
			//no customization required
		}

		protected override string LogicalContextName
		{
			get
			{
				return Resources.RetailItems;
			}
		}

		public override RecordIdentifier ID
		{
			get 
			{ 
				return RecordIdentifier.Empty;
			}
		}

		protected override void LoadData(bool isRevert)
		{
			sortSetting = PluginEntry.DataModel.Settings.GetSetting(
				PluginEntry.DataModel, 
				SortSettingID, 
				SettingType.UISetting, lvItems.CreateSortSetting(1,true));

			displayVariantItemsSetting = PluginEntry.DataModel.Settings.GetSetting(
				PluginEntry.DataModel,
				DisplayVariantsSettingID,
				SettingType.UISetting, "0");

			lockEvents = true;
			searchBar1.SearchOptionChecked = displayVariantItemsSetting.BoolValue;
			lockEvents = false;

			lvItems.SortSetting = sortSetting.Value;

			HeaderText = Resources.RetailItems;

			ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
		}

		protected override bool DataIsModified()
		{
			return false;
		}

		protected override bool SaveData()
		{
			return true;
		}

		public override void SaveUserInterface()
		{
			bool searchOptionValue;

			string newSortSetting = lvItems.SortSetting;

			if (newSortSetting != sortSetting.Value)
			{
				sortSetting.Value = newSortSetting;
				sortSetting.UserSettingExists = true;

				PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, SortSettingID, SettingsLevel.User, sortSetting);
			}

			if(!searchBar1.SearchOptionEnabled)
			{
				searchOptionValue = actualSearchOptionValue;
			}
			else
			{
				searchOptionValue = searchBar1.SearchOptionChecked;
			}

			if (displayVariantItemsSetting.BoolValue != searchOptionValue)
			{
				displayVariantItemsSetting.UserSettingExists = true;
				displayVariantItemsSetting.BoolValue = searchOptionValue;

				PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, DisplayVariantsSettingID, SettingsLevel.User, displayVariantItemsSetting);
			}
		}

		public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
		{
			switch (objectName)
			{
				case "RetailItem":
				case "ItemType":
				    if (this == PluginEntry.Framework.ViewController.CurrentView)
				    {
				        ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
				    }
				    else
				    {
				        LoadItems();
				    }
				    break;
				case "ItemSalesTaxGroupSalesTaxGroup":
					taxGroupNameCache = Providers.TaxItemData.GetItemTaxGroupDictionary(PluginEntry.DataModel);
					ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
					break;
				case "RetailGroup":
					retailGroupCache = ProcessDataEntityListToDictionary(Providers.RetailGroupData.GetMasterIDList(PluginEntry.DataModel));
					ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
					break;
				case "RetailDepartment":
					retailDepartmentCache = ProcessDataEntityListToDictionary(Providers.RetailDepartmentData.GetMasterIDList(PluginEntry.DataModel));
					ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
					break;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			PluginOperations.NewItem();
		}

		private void lvItems_SelectionChanged(object sender, EventArgs e)
		{
			selectedID = (lvItems.Selection.Count == 1) ? ((SimpleRetailItem)lvItems.Selection[0].Tag).ID : "";
			bool deleted = CheckDeleted();
			bool hasViewPermission = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsView);
			bool hasEditPermission = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);
			bool hasBulkEditPermission = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.MultiEditItems) && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit);

			btnsContextButtons.EditButtonEnabled = (((lvItems.Selection.Count == 1) && hasViewPermission) || ((lvItems.Selection.Count > 1) && hasBulkEditPermission)) && !deleted;
			btnsContextButtons.RemoveButtonEnabled = (lvItems.Selection.Count >= 1) && hasEditPermission;
			btnCopyID.Enabled = lvItems.Selection.Count == 1;
		}

		private bool CheckDeleted()
		{
			for (int i = 0; i < lvItems.Selection.Count; i++)
			{
				if (((SimpleRetailItem)lvItems.Selection[i].Tag).Deleted)
				{
					return true;
				}
			}
			return false;
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (lvItems.Selection.Count == 1)
			{
				PluginOperations.ShowItemSheet(((SimpleRetailItem)lvItems.Selection[0].Tag).ID, items.Cast<IDataEntity>());
			}
			else
			{
				List<IDataEntity> selectedItems = new List<IDataEntity>();

				SimpleRetailItem selectedItem;
				for (int i = 0; i < lvItems.Selection.Count; i++)
				{
					selectedItem = items[lvItems.Selection.GetSelectedItem(i)];

					if(selectedItem.ItemType == ItemTypeEnum.Service)
					{
						MessageDialog.Show(Resources.MultiEditServiceItemsText + ".", 
											MessageBoxButtons.OK, 
											MessageBoxIcon.Stop);
						return;
					}

					selectedItems.Add((IDataEntity)selectedItem);
				}

				PluginOperations.BulkEditItems(selectedItems);
			}
		}

		void ExpandCollapseRow(object sender, EventArgs args)
		{
			VirtualCollapsableCell cell = (lvItems.Row(lvItems.Selection.FirstSelectedRow)[0] as VirtualCollapsableCell);

			cell.SetCollapsed(lvItems, lvItems.Selection.FirstSelectedRow, !cell.Collapsed);
		}

		void lvItems_Opening(object sender, CancelEventArgs e)
		{
			var menu = lvItems.ContextMenuStrip;

			menu.Items.Clear();

			// We can optionally add our own items right here
			var item = new ExtendedMenuItem(
					Resources.Edit,
					100,
					btnEdit_Click)
				{
					Image = ContextButtons.GetEditButtonImage(),
					Enabled = btnsContextButtons.EditButtonEnabled,
					Default = true
				};
			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Resources.Add,
					200,
					btnAdd_Click)
				{
					Image = ContextButtons.GetAddButtonImage(),
					Enabled = btnsContextButtons.AddButtonEnabled
				};
			menu.Items.Add(item);

			string deleteDescription = Resources.Delete;

			if (lvItems.Selection.Count > 0)
			{
				deleteDescription = ((SimpleRetailItem) lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).Deleted
					? Resources.Undelete
					: Resources.Delete;
			}

			item = new ExtendedMenuItem(
					deleteDescription,
					300,
					btnRemove_Click)
				{
					Image = ContextButtons.GetRemoveButtonImage(),
					Enabled = btnsContextButtons.RemoveButtonEnabled
				};
			menu.Items.Add(item);

			if (btnExport.Visible)
			{
				menu.Items.Add(new ExtendedMenuItem("-", 1000));

				item = new ExtendedMenuItem(btnExport.Text, 1010, "RetailExportButton");
				item.Enabled = btnExport.Enabled;
				item.Image = btnExport.Image;

				menu.Items.Add(item);
			}

			if (btnImport.Visible)
			{
				menu.Items.Add(new ExtendedMenuItem("-", 1000));

				item = new ExtendedMenuItem(btnImport.Text, 1010, "RetailImportButton");
				item.Enabled = btnImport.Enabled;
				item.Image = btnImport.Image;

				menu.Items.Add(item);
			}

			menu.Items.Add(new ExtendedMenuItem("-", 2000));

			item = new ExtendedMenuItem(Resources.CopyID, 2010, CopyID);
			item.Enabled = btnCopyID.Enabled;
			menu.Items.Add(item);

			if(lvItems.Selection.Count > 0 && lvItems.Row(lvItems.Selection.FirstSelectedRow)[0] is VirtualCollapsableCell)
			{
				menu.Items.Add(new ExtendedMenuItem("-", 3000));

				if ((lvItems.Row(lvItems.Selection.FirstSelectedRow)[0] as VirtualCollapsableCell).Collapsed)
				{
					item = new ExtendedMenuItem(Resources.Expand, 3010, ExpandCollapseRow);
				}
				else
				{
					item = new ExtendedMenuItem(Resources.Collapse, 3010, ExpandCollapseRow);
				}

				menu.Items.Add(item);
			}

			PluginEntry.Framework.ContextMenuNotify("RetailMenuList", lvItems.ContextMenuStrip, this);

			e.Cancel = (menu.Items.Count == 0);


		}

		private void CopyID(object sender, EventArgs args)
		{
			Clipboard.SetText(lvItems.Row(lvItems.Selection.FirstSelectedRow)[0].Text);
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (lvItems.Selection.Count == 1)
			{
				PluginOperations.DeleteItem(((SimpleRetailItem)lvItems.Selection[0].Tag).ID, ((SimpleRetailItem)lvItems.Selection[0].Tag).MasterID);
			}
			else
			{
				List<SimpleRetailItem> selectedIDs = new List<SimpleRetailItem>();

				for(int i = 0; i < lvItems.Selection.Count; i++)
				{
					selectedIDs.Add(((SimpleRetailItem)lvItems.Selection[i].Tag));
				}
				if (IsDeleted(selectedIDs))
				{
					PluginOperations.UndeleteItems(selectedIDs);
				}
				else
				{
					PluginOperations.DeleteItems(selectedIDs);
				}
			}
		}

		private bool IsDeleted(List<SimpleRetailItem> items)
		{
			foreach (SimpleRetailItem item in items)
			{
				if (Providers.RetailItemData.IsDeleted(PluginEntry.DataModel, item.MasterID))
				{
					return true;
				}
			}
			return false;
		}

		public override int GetListSelectionCount()
		{
			return lvItems.Selection.Count;
		}

		public override List<RecordIdentifier> GetListSelectionKeys()
		{
			var res = new List<RecordIdentifier>();
			for (int i = 0; i < lvItems.Selection.Count; i++)
			{
				res.Add(((SimpleRetailItem)lvItems.Selection[i].Tag).ID);
			}
			return res;
		}

		public override object Message(string message, object param)
		{
			if (message == "ShowVariants")
			{
				return searchBar1.SearchOptionChecked;
			}
			return null;
		}

		public override List<RecordIdentifier> GetListKeys()
		{
			string idOrDescription = null;
			bool idOrDescriptionBeginsWith = true;
			RecordIdentifier retailGroupID = null;
			RecordIdentifier retailDepartmentID = null;
			RecordIdentifier taxGroupID = null;
			RecordIdentifier variantGroupID = null;
			RecordIdentifier vendorID = null;
			string barCode = null;
			bool barCodeBeginsWith = true;
			RecordIdentifier specialGroup = null;
			List<string> attributeSearch = null;
			bool attributeBeginsWith = true;
			List<SearchParameterResult> results = null;

			// We invoke here since this has to be thread safe when used for Export
			searchBar1.Invoke(new Action(()=> { results = searchBar1.SearchParameterResults; }));

			foreach (SearchParameterResult result in results)
			{
				switch (result.ParameterKey)
				{
					case "Description":
						idOrDescription = result.StringValue;
						idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;

					case "RetailGroup":
						retailGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "RetailDepartment":
						retailDepartmentID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "TaxGroup":
						taxGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;
				 
					case "Vendor":
						vendorID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "BarCode":
						barCode = result.StringValue;
						barCodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;

					case "SpecialGroup":
						specialGroup = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;
					case "Variant":
						attributeSearch = result.StringValue.Tokenize();
						attributeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;
				}
			}

			return Providers.RetailItemData.AdvancedSearchIDOnly(PluginEntry.DataModel,
																idOrDescription, idOrDescriptionBeginsWith, 
																retailGroupID, retailDepartmentID,
																taxGroupID, 
																variantGroupID, 
																vendorID, 
																barCode, barCodeBeginsWith, 
																specialGroup,
																true, null, false,
																attributeSearch, attributeBeginsWith, null, true);
		}

		private void LoadItems()
		{
			SortEnum sort = SortEnum.None;
			secondarySort = SortEnum.None;

			string idOrDescription = null;
			bool idOrDescriptionBeginsWith = true;
			DecimalLimit priceLimiter;
		  
			RecordIdentifier retailGroupID = null;
			RecordIdentifier retailDepartmentID = null;
			RecordIdentifier taxGroupID = null;
			RecordIdentifier variantGroupID = null;
			RecordIdentifier vendorID = null;
			string barCode = null;
			bool barCodeBeginsWith = true;
			string variantSearch = null;
			bool variantBeginsWith = true;
			RecordIdentifier specialGroup = null;
			List<SearchFlagEntity> searchFlags = null;
			RecordIdentifier currentlySelectedID = selectedID; // Preserve the selection since ClearRows() will clear the selection
			ItemTypeEnum? itemTypeFilter = null;

			lvItems.ClearRows();

			selectedID = currentlySelectedID;

			SortEnum?[] columns = new SortEnum?[] { SortEnum.ID , SortEnum.Description, SortEnum.VariantName, SortEnum.RetailGroup, SortEnum.RetailDepartment, SortEnum.TaxGroup, SortEnum.PriceIncludingTax };

			if (lvItems.SortColumn == null)
			{
				lvItems.SetSortColumn(lvItems.Columns[1], true);
			}

			int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

			if (sortColumnIndex < columns.Length)
			{
				if (columns[sortColumnIndex] == SortEnum.None)
				{
					sort = SortEnum.None;
				}
				else
				{
					sort = (SortEnum)((int)columns[sortColumnIndex] + (lvItems.SortedAscending ? 0 : 100)); // 100 added for Desc
				}
			}

			SetSecondarySort(lvItems.SortColumn.SecondarySortColumn, lvItems.SortedAscending);

			priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
			List<SearchParameterResult> results = searchBar1.SearchParameterResults;

			foreach (SearchParameterResult result in results)
			{
				switch(result.ParameterKey)
				{
					case "Description":
						idOrDescription = result.StringValue;
						idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;

					case "ItemType":
						switch (result.ComboSelectedIndex)
						{
							case 1:
								itemTypeFilter = ItemTypeEnum.Item;
								break;
							case 2:
								itemTypeFilter = ItemTypeEnum.Service;
								break;
							case 3:
								itemTypeFilter = ItemTypeEnum.AssemblyItem;
								break;
						}
						break;

					case "RetailGroup":
						retailGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "RetailDepartment":
						retailDepartmentID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "TaxGroup":
						taxGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "Vendor":
						vendorID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "BarCode":
						barCode = result.StringValue;
						barCodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
						break;

					case "SpecialGroup":
						specialGroup = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
						break;

					case "Variant":

						variantSearch = result.StringValue;

						variantBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);

						break;
					case "Attribute":
						ItemSearchFlagSelectionList list = ((DualDataComboBox)result.UnknownControl).SelectedData as ItemSearchFlagSelectionList;
						if (list != null)
						{
							searchFlags = list.GetItemStates();
						}

						break;
				}
			}

			int itemCount;

			try
			{
				// We try to get one more record then we intend to display, in order to see if there are more records available
				items = Providers.RetailItemData.AdvancedSearch(PluginEntry.DataModel, itemDataScroll.StartRecord,
																					   itemDataScroll.EndRecord + 1,
																					   sort,
																					   out itemCount,
																					   idOrDescription: idOrDescription,
																					   idOrDescriptionBeginsWith: idOrDescriptionBeginsWith,
																					   includeHeaders: true,
																					   retailGroupID: retailGroupID,
																					   retailDepartmentID: retailDepartmentID,
																					   taxGroupID: taxGroupID,
																					   variantGroupID: variantGroupID,
																					   vendorID: vendorID,
																					   barCode: barCode,
																					   barCodeBeginsWith: barCodeBeginsWith,
																					   specialGroup: specialGroup,
																					   includeVariants: searchBar1.SearchOptionChecked,
																					   VariantDescription: variantSearch,
																					   VariantDescriptionBeginsWith: variantBeginsWith,
																					   searchFlags: searchFlags,
																					   secondarySearch: secondarySort,
																					   itemTypeFilter: itemTypeFilter,
																					   isSearchBarControlSearch: true
																					   );

				itemDataScroll.RefreshState(items, itemCount);

				bool canEdit = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit);

				for (int indexOfRow = 0; indexOfRow < items.Count; indexOfRow ++)
				{
					SimpleRetailItem item = items[indexOfRow];

					Row row = new Row();

                    Image itemImage = null;

                    switch (item.ItemType)
                    {
                        case ItemTypeEnum.Item:
                            itemImage = item.HeaderItemID == RecordIdentifier.Empty ? retailItemImage16 : variantItem16;
                            break;
                        case ItemTypeEnum.Service:
                            itemImage = serviceItemImage16;
                            break;
                        case ItemTypeEnum.MasterItem:
                            itemImage = masterItem16;
                            break;
                        case ItemTypeEnum.BOM:
                        case ItemTypeEnum.AssemblyItem:
                            itemImage = assemblyItemImage16;
                            break;
                        default:
                            itemImage = retailItemImage16;
                            break;
                    }
                    
					VirtualCollapsableCell cellToAdd = null;
                    switch (item.ItemType)
                    {
                        case ItemTypeEnum.MasterItem:
                            if (searchBar1.SearchOptionChecked)
                            {
								row.AddCell(new ExtendedCell((string)item.ID, itemImage) { ImageHorizontalOffset = 16 });
							}
                            else
                            {
								cellToAdd = new VirtualCollapsableCell((string)item.ID, itemImage, true) { ImageHorizontalOffset = 4 };
								row.AddCell(cellToAdd);
							}
                            break;
                        default:
							row.AddCell(new ExtendedCell((string)item.ID, itemImage) { ImageHorizontalOffset = 16 });
							break;
                    }

                    row.AddText(item.Text);
					row.AddText(item.VariantName);

					if (item.RetailGroupMasterID != RecordIdentifier.Empty && retailGroupCache.ContainsKey(item.RetailGroupMasterID))
					{
						string retailGroupName = retailGroupCache[item.RetailGroupMasterID];
						row.AddText(retailGroupName);
					}
					else
					{
						row.AddText("");
					}
					if (item.RetailDepartmentMasterID != "" && retailDepartmentCache.ContainsKey(item.RetailDepartmentMasterID))
					{
						string RetailDepartmentName = retailDepartmentCache[item.RetailDepartmentMasterID];
						row.AddText(RetailDepartmentName);
					}
					else
					{
						row.AddText("");
					}
					if (item.SalesTaxItemGroupID != "" && taxGroupNameCache.ContainsKey(item.SalesTaxItemGroupID))
					{
						string TaxGroupNamde = taxGroupNameCache[item.SalesTaxItemGroupID];
						row.AddText(TaxGroupNamde);
					}
					else
					{
						row.AddText("");
					}

					if (priceStore.StorePriceSetting == Store.StorePriceSettingsEnum.PricesExcludeTax)
					{
						row.AddText(item.SalesPrice.FormatWithLimits(priceLimiter));
					}
					else
					{
						row.AddText(item.SalesPriceIncludingTax.FormatWithLimits(priceLimiter));
					}

					if (canEdit)
					{
						IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Resources.Delete);

						row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
					}
					else
					{
						row.AddText("");
					}

					row.Tag = item;

					lvItems.AddRow(row);

					/*
					 * Remember the current row count because it will get changed after the SetCollapsed call
					 * We will then need to set the indexOfRow at its "true" value, by skipping the already added
					 * items (the variants)
					 */
					int originalRowCount = lvItems.RowCount - 1;
                    if (cellToAdd != null)
                    {
						cellToAdd.SetCollapsed(lvItems, originalRowCount, true);
						indexOfRow += lvItems.RowCount - 1 - originalRowCount;	 
                    }

                    if (selectedID == ((SimpleRetailItem) row.Tag).ID)
					{
						lvItems.Selection.Set(lvItems.RowCount - 1);
					}

                }

				lvItems_SelectionChanged(this, EventArgs.Empty);
				
				lvItems.AutoSizeColumns();			
            }
            finally
			{
				HideProgress();
			}

			btnExport.Enabled = (lvItems.RowCount > 0);
        }

        private void lvItems_HeaderClicked(object sender, ColumnEventArgs args)
		{
			if (lvItems.SortColumn == args.Column)
			{
				lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
			}
			else
			{
				lvItems.SetSortColumn(args.Column, true);
			}

			itemDataScroll.Reset();

			ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
		}

		protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
		{
			arguments.Add(new ContextBarHeader(Resources.Actions, this.GetType().ToString() + ".Actions"), 5);
		}

		protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
		{
			if (arguments.CategoryKey == GetType() + ".View")
			{
				if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit))
				{
					arguments.Add(new ContextBarItem(Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
				}
			}
			else if (arguments.CategoryKey == base.GetType().ToString() + ".Actions")
			{
				// Only if the user has edit permissions
				if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ItemsEdit))
				{
					arguments.Add(new ContextBarItem(
									  Resources.ActionsImportImages,
									  ActionsImportImages_Handler), 200);

				}
			}
		}

		private void ActionsImportImages_Handler(object sender, ContextBarClickEventArguments args)
		{
			PluginOperations.ImportImages(sender,args);
		}

		private void ActionsCreateCombinations_Handler(object sender, ContextBarClickEventArguments args)
		{
			ShowProgress(CreateCombinationsProgressHandler, Resources.Processing);
		}

		private void CreateCombinationsProgressHandler(object sender, EventArgs args)
		{
			/*foreach (SimpleRetailItem item in items)
			{
				if (Providers.RetailItemData.ItemHasDimentionGroup(PluginEntry.DataModel, item))
				{
					PluginOperations.CreateCombinations(sender, args, item);
				}
			}
			HideProgress();*/
		}

		private void OnPageScrollPageChanged(object sender, EventArgs e)
		{
			ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
		}

		private void ItemsView_Load(object sender, EventArgs e)
		{
		}

		private void searchBar1_SetupConditions(object sender, EventArgs e)
		{
			List<object> itemTypeList = new List<object>();
			itemTypeList.Add(Resources.AllTypes);
			itemTypeList.Add(Resources.Item);
			itemTypeList.Add(Resources.ServiceItem);
			itemTypeList.Add(Resources.AssemblyItem);

			searchBar1.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
			searchBar1.AddCondition(new ConditionType(Resources.Variant, "Variant", ConditionType.ConditionTypeEnum.Text));
			searchBar1.AddCondition(new ConditionType(Resources.ItemType, "ItemType", ConditionType.ConditionTypeEnum.ComboBox, itemTypeList, 0, 0, false));
			searchBar1.AddCondition(new ConditionType(Resources.RetailGroup, "RetailGroup", ConditionType.ConditionTypeEnum.Unknown));
			searchBar1.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment", ConditionType.ConditionTypeEnum.Unknown));
			searchBar1.AddCondition(new ConditionType(Resources.TaxGroup, "TaxGroup", ConditionType.ConditionTypeEnum.Unknown));
			searchBar1.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
			searchBar1.AddCondition(new ConditionType(Resources.BarCode, "BarCode", ConditionType.ConditionTypeEnum.Text));
			searchBar1.AddCondition(new ConditionType(Resources.SpecialGroup, "SpecialGroup", ConditionType.ConditionTypeEnum.Unknown));
			searchBar1.AddCondition(new ConditionType(Resources.Attribute, "Attribute", ConditionType.ConditionTypeEnum.Unknown));


			searchBar1_LoadDefault(this, EventArgs.Empty);
		}

		private void searchBar1_LoadDefault(object sender, EventArgs e)
		{
			searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting,"");

			if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
			{
				searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
			}
		}

		private void searchBar1_SaveAsDefault(object sender, EventArgs e)
		{
			string layoutString = searchBar1.LayoutStringWithData;

			if (searchBarSetting.LongUserSetting != layoutString)
			{
				searchBarSetting.LongUserSetting = layoutString;
				searchBarSetting.UserSettingExists = true;

				PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
			}

			ShowTimedProgress(searchBar1.GetLocalizedSavingText());
		}

		private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
		{
			switch (args.TypeKey)
			{
				case "RetailGroup":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

					((DualDataComboBox)args.UnknownControl).DropDown +=new DropDownEventHandler(RetailGroup_DropDown);
					break;


				case "RetailDepartment":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

					((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailDepartments_DropDown);
					break;

				case "SpecialGroup":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

					((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(SpecialGroups_DropDown);
					break;

				case "Vendor":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

					((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Vendors_RequestData);
					break;

				case "TaxGroup":
					args.UnknownControl = new DualDataComboBox();
					args.UnknownControl.Size = new Size(200, 21);
					args.MaxSize = 200;
					args.AutoSize = false;
					((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
					((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

					((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(TaxGroup_RequestData);
					break;

				case "Attribute":
					var dualDataComboBox = new DualDataComboBox();
				   
					dualDataComboBox.MaxLength = 32767;
					dualDataComboBox.SelectedData = null;
					dualDataComboBox.SkipIDColumn = true;
					
					dualDataComboBox.ShowDropDownOnTyping = true;

					ItemSearchFlagSelectionList selectionList = new ItemSearchFlagSelectionList(Providers.RetailItemData.ItemSearchFlags);
					dualDataComboBox.SelectedData = selectionList;
					dualDataComboBox.DropDown += Attribute_DropDown;
					dualDataComboBox.SelectedDataChanged += Attribute_SelectedDataChanged;

					dualDataComboBox.Size = new Size(200, 21);
					args.UnknownControl = dualDataComboBox;
					args.MaxSize = 200;
					args.AutoSize = false;

					break;

			}
		}

		private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
		{
			switch (args.TypeKey)
			{
				case "RetailGroup":
				case "RetailDepartment":
				case "SpecialGroup":
				case "Vendor":
				case "TaxGroup":
					 args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
					break;

				case "Attribute":
					args.HasSelection = true;
					break;
			}
		}

		private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
		{
			switch (args.TypeKey)
			{
				case "RetailGroup":
				case "RetailDepartment":
				case "SpecialGroup":
				case "Vendor":
				case "TaxGroup":
				case "Attribute":
					args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
					break;
			}
		}

		private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
		{
			DataEntity entity = null;
			switch (args.TypeKey)
			{
				case "RetailGroup":
					entity = Providers.RetailGroupData.Get(PluginEntry.DataModel, args.Selection);
					break;
				case "RetailDepartment":
					entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
					break;
				case "SpecialGroup":
					entity = Providers.SpecialGroupData.Get(PluginEntry.DataModel, args.Selection);
					break;
				case "Vendor":
					entity = Providers.VendorData.Get(PluginEntry.DataModel, args.Selection);
					break;
				case "TaxGroup":
					entity = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
					break;
					

			}
			((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
		}

		private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
		{
			switch (args.TypeKey)
			{
				case "RetailGroup":
					((DualDataComboBox)args.UnknownControl).DropDown -= RetailGroup_DropDown;
					break;

				case "RetailDepartment":
					((DualDataComboBox)args.UnknownControl).DropDown -= RetailDepartments_DropDown;
					break;

				case "SpecialGroup":
					((DualDataComboBox)args.UnknownControl).DropDown -= SpecialGroups_DropDown;
					break;

				case "Vendor":
					((DualDataComboBox)args.UnknownControl).RequestData -= Vendors_RequestData;
					break;

				case "TaxGroup":
					((DualDataComboBox)args.UnknownControl).RequestData -= TaxGroup_RequestData;
					break;

				case "Attribute":
					((DualDataComboBox)args.UnknownControl).DropDown -= Attribute_DropDown;
					((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= Attribute_SelectedDataChanged;
					break;
			}
		}


		void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;
			e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailDepartmentsMasterID, "");
		}

		void RetailGroup_DropDown(object sender, DropDownEventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;
			e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailGroupsMasterID, "");
		}

		void SpecialGroups_DropDown(object sender, DropDownEventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;
			e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.SpecialGroups, "");
		}

		void Vendors_RequestData(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;

			((DualDataComboBox)sender).SetData(Providers.VendorData.GetList(PluginEntry.DataModel),null);
		}

		void TaxGroup_RequestData(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SkipIDColumn = true;

			((DualDataComboBox)sender).SetData(Providers.ItemSalesTaxGroupData.GetList(PluginEntry.DataModel), null);
		}


		void Attribute_DropDown(object sender, DropDownEventArgs e)
		{
			ItemSearchFlagSelectionList list = ((DualDataComboBox)sender).SelectedData as ItemSearchFlagSelectionList;
			if (list != null)
			{
				e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
			}
		}

		void Attribute_SelectedDataChanged(object sender, EventArgs e)
		{
			List<SearchFlagEntity> selectedItems;
			ItemSearchFlagSelectionList selectionList;


			if (((DualDataComboBox)sender).SelectedData != null)
			{
				selectionList = (ItemSearchFlagSelectionList)((DualDataComboBox)sender).SelectedData;

				selectedItems = selectionList.GetItemStates();

			}
		}
		private void searchBar1_SearchClicked(object sender, EventArgs e)
		{
			itemDataScroll.Reset();
			ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
		}

		private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
		{
			if (btnsContextButtons.EditButtonEnabled)
			{
				btnEdit_Click(this, EventArgs.Empty);
			}
		}

		private void lvItems_CellAction(object sender, CellEventArgs args)
		{
			if (args.Cell is IconButtonCell && !args.IsKeyboardAction)
			{
				PluginOperations.DeleteItem(((SimpleRetailItem)lvItems.Row(args.RowNumber).Tag).ID, ((SimpleRetailItem)lvItems.Row(args.RowNumber).Tag).MasterID);
			}
		}

		public override List<IDataEntity> GetListSelection()
		{
			var res = new List<IDataEntity>();
			for (int i = 0; i < lvItems.Selection.Count; i++)
			{
				res.Add(Providers.RetailItemData.Get(PluginEntry.DataModel, ((SimpleRetailItem)lvItems.Selection[i].Tag).MasterID));
			}
			return res;
		}

		private void btnExport_MouseDown(object sender, MouseEventArgs e)
		{
			exportContextMenu.Items.Clear();

			PluginEntry.Framework.ContextMenuNotify("RetailExportButton", exportContextMenu, this);

			exportContextMenu.Show(btnExport, 0, btnExport.Height);
		}

		private void btnExport_Click(object sender, EventArgs e)
		{

		}

		private void btnImport_MouseDown(object sender, MouseEventArgs e)
		{
			importContextMenu.Items.Clear();

			PluginEntry.Framework.ContextMenuNotify("RetailImportButton", importContextMenu, this);

			importContextMenu.Show(btnImport, 0, btnImport.Height);
		}

		private void searchBar1_SearchOptionChanged(object sender, EventArgs e)
		{
			if (!lockEvents)
			{
				searchBar1_SearchClicked(this, EventArgs.Empty);
			}
		}

		private void lvItems_RowExpanded(object sender, RowEventArgs args)
		{
			DecimalLimit priceLimiter;
			priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
			RecordIdentifier itemMasterID = ((SimpleRetailItem)lvItems.Row(args.RowNumber).Tag).MasterID;

			SortEnum sort = SortEnum.None;

			SortEnum?[] columns = new SortEnum?[] { SortEnum.ID, SortEnum.Description, SortEnum.None, SortEnum.RetailGroup, SortEnum.RetailDepartment, SortEnum.TaxGroup, SortEnum.PriceIncludingTax };

			int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

			if (sortColumnIndex < columns.Length)
			{
				if (columns[sortColumnIndex] == SortEnum.None)
				{
					sort = SortEnum.None;
				}
				else
				{
					sort = (SortEnum)((int)columns[sortColumnIndex] + (lvItems.SortedAscending ? 0 : 100)); // 100 added for Desc
				}

			}

		
			List<SimpleRetailItem> subRows = Providers.RetailItemData.GetItemVariants(PluginEntry.DataModel, itemMasterID, sort,false);

			bool canEdit = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit);

			int nextRowNumber = args.RowNumber + 1;

			

			foreach (SimpleRetailItem item in subRows)
			{
				Row row = new Row();

				row.AddCell(new ExtendedCell((string)item.ID, variantItem16) { ImageHorizontalOffset = 32 });

				row.AddText(item.Text);
				row.AddText(item.VariantName);

				if (item.RetailGroupMasterID != RecordIdentifier.Empty && retailGroupCache.ContainsKey(item.RetailGroupMasterID))
				{
					string retailGroupName = retailGroupCache[item.RetailGroupMasterID];
					row.AddText(retailGroupName);
				}
				else
				{
					row.AddText("");
				}
				if (item.RetailDepartmentMasterID != "" && retailDepartmentCache.ContainsKey(item.RetailDepartmentMasterID))
				{
					string RetailDepartmentName = retailDepartmentCache[item.RetailDepartmentMasterID];
					row.AddText(RetailDepartmentName);
				}
				else
				{
					row.AddText("");
				}
				if (item.SalesTaxItemGroupID != "" && taxGroupNameCache.ContainsKey(item.SalesTaxItemGroupID))
				{
					string TaxGroupNamde = taxGroupNameCache[item.SalesTaxItemGroupID];
					row.AddText(TaxGroupNamde);
				}
				else
				{
					row.AddText("");
				}
			   
				row.AddText(item.SalesPriceIncludingTax.FormatWithLimits(priceLimiter));

				if (canEdit)
				{
					IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete);

					row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
				}
				else
				{
					row.AddText(""); 
				}

				row.Tag = item;

				lvItems.InsertRow(nextRowNumber,row);
				items.Insert(nextRowNumber, item);

				if (selectedID == ((SimpleRetailItem)row.Tag).ID)
				{
					lvItems.Selection.Set(lvItems.RowCount - 1);
				}

				nextRowNumber++;
			}

			lvItems.AutoSizeColumns();
		}

		private void lvItems_RowCollapsed(object sender, RowEventArgs args)
		{
			RecordIdentifier itemMasterID = ((SimpleRetailItem)lvItems.Row(args.RowNumber).Tag).MasterID;

			int rowIndex = args.RowNumber + 1;

			while(rowIndex < lvItems.RowCount && ((SimpleRetailItem)lvItems.Row(rowIndex).Tag).HeaderItemID == itemMasterID)
			{
				lvItems.RemoveRow(rowIndex);
				items.RemoveAt(rowIndex);
			}

			lvItems.AutoSizeColumns();
		}

		private void searchBar1_SearchTypesChanged(object sender, EventArgs e)
		{
			SetSearchBarShowVariantEnabledState();
		}

		private void searchBar1_ControlRemoved(object sender, ControlEventArgs e)
        {
			SetSearchBarShowVariantEnabledState();
		}

		private void SetSearchBarShowVariantEnabledState()
		{
			bool hasNoVariantDescription = !searchBar1.UsesBar("Variant");

			if (searchBar1.SearchOptionEnabled != hasNoVariantDescription)
			{
				if (hasNoVariantDescription)
				{
					searchBar1.SearchOptionEnabled = true;
					searchBar1.SearchOptionChecked = actualSearchOptionValue;
				}
				else
				{
					actualSearchOptionValue = searchBar1.SearchOptionChecked;
					searchBar1.SearchOptionEnabled = false;
					searchBar1.SearchOptionChecked = true;
				}
			}
		}
	}
}
