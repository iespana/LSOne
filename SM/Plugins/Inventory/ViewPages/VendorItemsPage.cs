using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;
using System.Drawing;
using System.Linq;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class VendorItemsPage : Controls.ContainerControl, ITabView
    {
        private static Guid BarSettingID = new Guid("4C4324CE-A1F1-4D8D-A77B-7991B6D74E1B");
        private Setting searchBarSetting;

        RecordIdentifier vendorID;
        RecordIdentifier selectedVendorItemID;
        Vendor vendor;
        List<VendorItem> items;
        WeakReference retailItemEditor;
        WeakReference unitConversionEditor;

        public VendorItemsPage()
        {
            IPlugin plugin;

            InitializeComponent();

            selectedVendorItemID = "";

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VendorEdit);

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            lvItems.Columns[0].Tag = VendorItemSorting.VendorItemID; //colVendorItemID
            lvItems.Columns[1].Tag = VendorItemSorting.Description; //colDescription
            lvItems.Columns[2].Tag = VendorItemSorting.Variant; //colVariant
            lvItems.Columns[3].Tag = VendorItemSorting.UnitDescription; //colUnit
            lvItems.Columns[4].Tag = VendorItemSorting.DefaultPurchasePrice; //colDefaultPurchasePrice
            lvItems.Columns[5].Tag = VendorItemSorting.ItemPrice; //colLastPurchPrice
            lvItems.Columns[6].Tag = VendorItemSorting.LastOrderDate; //colLastOrderingDate

            lvItems.SetSortColumn(0, true);
            searchBar.BuddyControl = lvItems;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);

            if (plugin != null)
            {
                btnViewRetailItem.Visible = true;
                retailItemEditor = new WeakReference(plugin);
            }

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditUnitConversions", null);

            if (plugin != null)
            {
                btnUnitConversions.Visible = true;
                unitConversionEditor = new WeakReference(plugin);
            }

            searchBar.FocusFirstInput();

            vendoritemsDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            vendoritemsDataScroll.Reset();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new VendorItemsPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        void LoadItems()
        {
            lvItems.ClearRows();

            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[0], true);
            }

            VendorItemSorting sortBy = (VendorItemSorting)lvItems.SortColumn.Tag;
            bool sortBackwards = !lvItems.SortedAscending;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;
            string idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            RecordIdentifier unitID = RecordIdentifier.Empty;
            Date dateFrom = Date.Empty;
            Date dateTo = Date.Empty;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        idOrDescription = result.StringValue;
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Unit":
                        unitID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Date":
                        if (result.Date.Checked)
                        {
                            dateFrom.DateTime = result.Date.Value.Date;
                        }

                        if (result.DateTo.Checked)
                        {
                            dateTo.DateTime = result.DateTo.Value.Date.AddDays(1).AddSeconds(-1);
                        }
                        break;
                }
            }

            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            Row row;
            try
            {
                int itemsCount = 0;
                int startRecord = vendoritemsDataScroll.StartRecord;
                int endRecord = vendoritemsDataScroll.EndRecord + 1;
                var viSearch = new VendorItemSearch
                {
                    VendorID = vendorID,
                    Description = (idOrDescription ?? string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                    DescriptionBeginsWith = idOrDescriptionBeginsWith,
                    UnitID = unitID,
                    LastOrderingDateFrom = dateFrom,
                    LastOrderingDateTo = dateTo
                };

                items = service.AdvancedSearch(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                                viSearch,
                                                sortBy, sortBackwards,
                                                startRecord, endRecord,
                                                out itemsCount,
                                                true);
                vendoritemsDataScroll.RefreshState(items, itemsCount);

                Style rowStyle;
                Style strikeThroughStyle = new Style(lvItems.DefaultStyle);
                strikeThroughStyle.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);
                foreach (VendorItem vendorItem in items)
                {
                    int numberOfPriceDecimals = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;

                    row = new Row();
                    rowStyle = vendorItem.RetailItemType == ItemTypeEnum.Service ? strikeThroughStyle : lvItems.DefaultStyle;

                    row.AddCell(new Cell((string)vendorItem.VendorItemID, rowStyle));
                    row.AddCell(new Cell(vendorItem.Text, rowStyle));
                    row.AddCell(new Cell(vendorItem.VariantName, rowStyle));
                    row.AddCell(new Cell(vendorItem.UnitDescription, rowStyle));
                    row.AddCell(new NumericCell(vendorItem.DefaultPurchasePrice.ToString("N" + numberOfPriceDecimals), rowStyle, vendorItem.DefaultPurchasePrice));
                    row.AddCell(new NumericCell(vendorItem.LastItemPrice.ToString("N" + numberOfPriceDecimals), rowStyle, vendorItem.LastItemPrice));
                    row.AddCell(new DateCell(vendorItem.LastOrderDate.ToShortDateString(), rowStyle, vendorItem.LastOrderDate));

                    row.Tag = vendorItem;

                    lvItems.AddRow(row);

                    if (selectedVendorItemID == vendorItem.ID)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                    }
                }

                lvItems.AutoSizeColumns();

                lvItems_SelectionChanged(this, EventArgs.Empty);
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            vendorID = context;
            vendor = (Vendor)internalContext;

            LoadItems();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("VendorItems", vendor.ID, Resources.RetailItems, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "VendorItem" || objectName == "RetailItem" || objectName == "VendorsForItem")
            {
                LoadItems();
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            VendorItemDialog dlg = VendorItemDialog.CreateForNew(vendorID);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            try
            {
                VendorItemDialog dlg = VendorItemDialog.CreateForEditing(vendorID, ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).ID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    LoadItems();
                }
            }
            catch (DataIntegrityException exception)
            {
                if (exception.EntityType == typeof(Dimension))
                {
                    MessageDialog.Show(Resources.VariationNoLongerExists);
                }
                else if (exception.EntityType == typeof(VendorItem))
                {
                    MessageDialog.Show(Resources.RetailItemNoLongerExists);
                }
                else if (exception.EntityType == typeof(RetailItem))
                {
                    MessageDialog.Show(Resources.UnableToOpenItem + " " + Resources.ItemNotInLocalDatabase);
                }
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                lvItems.Selection.Count == 1 ? Resources.DeleteVendorItemQuestion : Resources.DeleteVendorItemsQuestion,
                lvItems.Selection.Count == 1 ? Resources.DeleteVendorItem : Resources.DeleteVendorItems) == DialogResult.Yes)
            {
                try
                {
                    var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    List<VendorItem> itemsToDelete = new List<VendorItem>();
                    for (int i = 0; i < lvItems.Selection.Count; i++)
                    {
                        itemsToDelete.Add(((VendorItem)lvItems.Selection[i].Tag));
                    }

                    service.DeleteVendorItems(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemsToDelete.Select(x => x.ID).ToList(), true);

                    // Delete default vendor from retail items
                    foreach(VendorItem item in itemsToDelete)
                    {
                        RecordIdentifier itemDefaultVendor = service.GetItemsDefaultVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), item.RetailItemID, true);

                        if(item.VendorID.StringValue == itemDefaultVendor.StringValue)
                        {
                            service.SetItemsDefaultVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), item.RetailItemID, RecordIdentifier.Empty, true);
                        }
                    }

                    LoadItems();
                }
                catch
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
                }
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvItems.ContextMenuStrip;

            bool isServiceItem = false;
            if (!RecordIdentifier.IsEmptyOrNull(selectedVendorItemID))
            {
                var selectedLine = (VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
                isServiceItem = selectedLine == null ? false : selectedLine.RetailItemType == ItemTypeEnum.Service;
            }

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item;
            if (!isServiceItem)
            {
                item = new ExtendedMenuItem(
                        Resources.Edit,
                        100,
                        new EventHandler(btnsEditAddRemove_EditButtonClicked))
                {
                    Image = ContextButtons.GetEditButtonImage(),
                    Enabled = btnsEditAddRemove.EditButtonEnabled,
                    Default = true
                };

                menu.Items.Add(item);
            }

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

            menu.Items.Add(item);

            if (btnViewRetailItem.Visible)
            {
                menu.Items.Add(new ExtendedMenuItem("-", 400));

                item = new ExtendedMenuItem(
                        btnViewRetailItem.Text,
                        500,
                        new EventHandler(btnViewRetailItem_Click));

                item.Enabled = btnViewRetailItem.Enabled;

                menu.Items.Add(item);
            }

            if (btnViewRetailItem.Visible)
            {
                menu.Items.Add(new ExtendedMenuItem("-", 600));

                item = new ExtendedMenuItem(
                        btnUnitConversions.Text,
                        700,
                        new EventHandler(btnUnitConversions_Click));

                item.Enabled = btnUnitConversions.Enabled;

                menu.Items.Add(item);
            }

            item = new ExtendedMenuItem("-", 700);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.EditAll,
                    800,
                    btnEditAllLines_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.AddButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("VendorItemsList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnViewRetailItem_Click(object sender, EventArgs e)
        {
            RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).RetailItemID);

            if (retailItem == null)
            {
                MessageDialog.Show(Resources.ItemInformationNotLocally + " " + Resources.ToViewInformationUpdateItemMaster);
                return;
            }

            if (retailItemEditor.IsAlive)
            {
                List<IDataEntity> list = new List<IDataEntity>();

                foreach (VendorItem item in items)
                {
                    list.Add(new DataEntity(item.RetailItemID, ""));
                }

                ((IPlugin)retailItemEditor.Target).Message(this, "ViewItem", new object[] { ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).RetailItemID, list });
            }
        }

        private void btnUnitConversions_Click(object sender, EventArgs e)
        {
            if (unitConversionEditor.IsAlive)
            {
                ((IPlugin)unitConversionEditor.Target).Message(this, "ViewUnitConversions", ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).RetailItemID);
            }
        }

        private void btnEditAllLines_Click(object sender, EventArgs e)
        {
            var dlg = new ItemVendorMultiEditDialog(vendorID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.AddButtonEnabled && lvItems.Selection.Count == 1;
            btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.AddButtonEnabled && lvItems.Selection.Count > 0;
            btnViewRetailItem.Enabled = btnUnitConversions.Enabled = lvItems.Selection.Count == 1;

            if (lvItems.Selection.Count == 1)
            {
                selectedVendorItemID = ((VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag).ID;
            }
            else if (lvItems.Selection.Count > 1)
            {
                selectedVendorItemID = null;
            }

            if (lvItems.Selection.FirstSelectedRow > 0 && selectedVendorItemID != null)
            {
                var selectedLine = (VendorItem)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;
                if (selectedLine.RetailItemType == ItemTypeEnum.Service)
                {
                    btnsEditAddRemove.EditButtonEnabled = false;
                }
            }
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void vendoritemsDataScroll_PageChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.ItemUnit, "Unit", ConditionType.ConditionTypeEnum.Unknown));

            DateTime dateFrom = DateTime.Today.AddDays(-7);
            DateTime dateTo = DateTime.Today;
            searchBar.AddCondition(new ConditionType(Resources.ItemLastOrderingDate, "Date", ConditionType.ConditionTypeEnum.DateRange, true, dateFrom, true, dateTo));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            if (args.TypeKey == "Unit")
            {
                args.UnknownControl = new DualDataComboBox();
                args.UnknownControl.Size = new Size(200, 21);
                args.MaxSize = 200;
                args.AutoSize = false;
                ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Unit_DropDown);
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "Unit")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "Unit")
            {
                args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            if (args.TypeKey == "Unit")
            {
                ((DualDataComboBox)args.UnknownControl).RequestData -= Unit_DropDown;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;

            switch (args.TypeKey)
            {
                case "Unit":
                    entity = Providers.UnitData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            searchBar_SearchClicked(sender, e);
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void Unit_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.UnitData.GetList(PluginEntry.DataModel), null);
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
        }

        private void lvItems_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvItems.SortColumn == args.Column)
            {
                lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
            }
            else
            {
                lvItems.SetSortColumn(args.Column, true);
            }

            vendoritemsDataScroll.Reset();

            LoadItems();
        }
    }
}