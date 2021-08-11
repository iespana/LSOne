using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class PurchaseOrdersView : ViewBase
    {
        private static Guid BarSettingID = new Guid("0C6542E5-18AC-4021-B73E-E05AD468A47E");
        private RecordIdentifier selectedID = "";
        private Setting searchBarSetting;
        private InventoryPurchaseOrderSortEnums secondarySort;
        private List<DataEntity> vendorsList;
        private Store defaultStore;

        public PurchaseOrdersView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
        }

        public PurchaseOrdersView()
        {
            InitializeComponent();
            
            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(lvItems_Opening);

            searchBar.BuddyControl = lvItems;
            searchBar.FocusFirstInput();

            defaultStore = null;
            if (!string.IsNullOrWhiteSpace((string)PluginEntry.DataModel.CurrentStoreID))
            {
                searchBar.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();
            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);

            try
            {
                vendorsList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendorList(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PurchaseOrders", 0, Properties.Resources.PurchaseOrders, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PurchaseOrders;
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
            if (isRevert)
            {
                
            }
            HeaderText = Properties.Resources.PurchaseOrders;
            searchBar_SearchClicked(null, EventArgs.Empty);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewPurchaseOrderWizard();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowPurchaseOrder((RecordIdentifier)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RecordIdentifier purchaseOrderID = (RecordIdentifier)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag;

            if(PluginOperations.DeletePurchaseOrder(purchaseOrderID) == PurchaseOrderLinesDeleteResult.CanBeDeleted)
            {
                LoadItems();
            }

        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PurchaseOrder":
                    LoadItems();
                    break;
            }
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnEdit_Click));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnAdd_Click));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnRemove_Click));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PurchaseOrders", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void SetSecondarySort(short secondarySortColumn, bool sortedAscending)
        {
            switch (secondarySortColumn)
            {
                case -1:
                    secondarySort = InventoryPurchaseOrderSortEnums.None;
                    break;
                case 2:
                    secondarySort = InventoryPurchaseOrderSortEnums.Description;
                    break;
                default:
                    secondarySort = InventoryPurchaseOrderSortEnums.Description;
                    break;
            }
            if (!sortedAscending && secondarySort != InventoryPurchaseOrderSortEnums.None)
            {
                secondarySort += 100;
            }
        }

        private void LoadItems()
        {
            InventoryPurchaseOrderSortEnums sort = InventoryPurchaseOrderSortEnums.None;
            secondarySort = InventoryPurchaseOrderSortEnums.None;

            List<string> idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            PurchaseStatusEnum? status = null;
            RecordIdentifier storeID = PluginEntry.DataModel.IsHeadOffice ? null : PluginEntry.DataModel.CurrentStoreID;
            RecordIdentifier vendorID = null;
            Date deliveryDateFrom = Date.Empty;
            Date deliveryDateTo = Date.Empty;
            Date creationDateFrom = Date.Empty;
            Date creationDateTo = Date.Empty;


            InventoryPurchaseOrderSortEnums?[] columns =
            {
                InventoryPurchaseOrderSortEnums.None,
                InventoryPurchaseOrderSortEnums.ID,
                InventoryPurchaseOrderSortEnums.Description,
                InventoryPurchaseOrderSortEnums.Store,
                InventoryPurchaseOrderSortEnums.Vendor,
                InventoryPurchaseOrderSortEnums.Status,
                InventoryPurchaseOrderSortEnums.DeliveryDate,
                InventoryPurchaseOrderSortEnums.CreationDate
            };

            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[1], true);
            }

            int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

            if (sortColumnIndex < columns.Length)
            {
                if (columns[sortColumnIndex] == InventoryPurchaseOrderSortEnums.None)
                {
                    sort = InventoryPurchaseOrderSortEnums.None;
                }
                else
                {
                    sort = (InventoryPurchaseOrderSortEnums)((int)columns[sortColumnIndex]);
                }
            }

            SetSecondarySort(lvItems.SortColumn.SecondarySortColumn, lvItems.SortedAscending);

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "idOrDescription":
                        idOrDescription = new List<string> {result.StringValue};
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                status = PurchaseStatusEnum.Open;
                                break;
                            case 2:
                                status = PurchaseStatusEnum.Closed;
                                break;
                            //case 3:
                            //    status = PurchaseStatusEnum.PartiallyRecieved;
                            //    break;
                            //case 4:
                            //    status = PurchaseStatusEnum.Placed;
                            //    break;
                            default:
                                status = null;
                                break;
                        }
                        break;
                    case "Store":
                        storeID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Vendor":
                        vendorID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "DeliveryDate":
                        deliveryDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        deliveryDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "CreationDate":
                        creationDateFrom = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        creationDateTo = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                }
            }

            if (!PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores) && !PluginEntry.DataModel.IsHeadOffice)
            {
                storeID = PluginEntry.DataModel.CurrentStoreID;
            }

            int itemCount;

            try
            {
                lvItems.ClearRows();

                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                List<PurchaseOrder> purchaseOrders = service.PurchaseOrderAdvancedSearch(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    true,
                    itemDataScroll.StartRecord,
                    itemDataScroll.EndRecord + 1,
                    sort,
                    !lvItems.SortedAscending,
                    out itemCount,
                    idOrDescription,
                    idOrDescriptionBeginsWith,
                    storeID,
                    vendorID,
                    status,
                    deliveryDateFrom,
                    deliveryDateTo,
                    creationDateFrom,
                    creationDateTo
                    );

                itemDataScroll.RefreshState(purchaseOrders, itemCount);

                Row row;

                foreach (PurchaseOrder purchaseOrder in purchaseOrders)
                {
                    row = new Row();

                    Bitmap statusImage = null;
                    switch (purchaseOrder.PurchaseStatus)
                    {
                        case PurchaseStatusEnum.Placed:
                            statusImage = Resources.dot_green_16;
                            break;
                        case PurchaseStatusEnum.PartiallyRecieved:
                            statusImage = Resources.dot_yellow_16;
                            break;
                        case PurchaseStatusEnum.Closed:
                            statusImage = Resources.dot_finished_16;
                            break;
                        default:
                            statusImage = Resources.dot_grey_16;
                            break;
                    }
                    row.AddCell(new ExtendedCell(string.Empty, statusImage));

                    row.AddText((string) purchaseOrder.PurchaseOrderID);
                    row.AddText(purchaseOrder.Description);
                    row.AddText(purchaseOrder.StoreName);
                    row.AddText(purchaseOrder.VendorName);
                    row.AddText(purchaseOrder.PurchaseStatusText);
                    row.AddCell(new DateTimeCell(purchaseOrder.DeliveryDate.ToShortDateString(), purchaseOrder.DeliveryDate));
                    row.AddCell(new DateTimeCell(purchaseOrder.CreatedDate.ToShortDateString(), purchaseOrder.CreatedDate));
                    row.Tag = purchaseOrder.ID;

                    lvItems.AddRow(row);

                    if (selectedID == (RecordIdentifier) row.Tag)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                    }
                }

                lvItems.AutoSizeColumns();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            finally
            {
                HideProgress();
            }

            lvItems_SelectionChanged(this, EventArgs.Empty);
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvItems.Selection.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders))
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvItems.Selection.Count > 0) ? (RecordIdentifier) lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag : "";
            btnsEditAddRemove.EditButtonEnabled = (lvItems.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);
            btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled;
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusList = new List<object>();
            statusList.Add(Resources.AllStatuses);
            statusList.Add(Resources.Open);
            statusList.Add(Resources.PurchaseStatus_Posted);
            //statusList.Add(Resources.PartiallyReceived);
            //statusList.Add(Resources.Placed);

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusList, 1, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.DeliveryDate, "DeliveryDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar.AddCondition(new ConditionType(Resources.CreationDate, "CreationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

            searchBar_LoadDefault(this, EventArgs.Empty);
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
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = defaultStore == null ? new DataEntity("", "") : new DataEntity(defaultStore.ID, defaultStore.Text);
                    ((DualDataComboBox) args.UnknownControl).RequestData += Store_RequestData;
                    break;
                case "Vendor":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox) args.UnknownControl).RequestData += Vendor_RequestData;
                    break;
            }
        }


        private void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;
            List<DataEntity> stores = new List<DataEntity>();

            if (PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrdersForAllStores) || PluginEntry.DataModel.IsHeadOffice)
            {
                stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                stores.Insert(0, new DataEntity(null, Resources.AllStores));
            }
            else
            {
                stores.Add(Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            }

            ((DualDataComboBox)sender).SetData(stores, null);
        }

        private void Vendor_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;
            ((DualDataComboBox) sender).SetData(vendorsList, null);
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= Store_RequestData;
                    break;
                case "Vendor":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= Vendor_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.HasSelection = ((DualDataComboBox) args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox) args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Vendor":
                    args.Selection = (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Vendor":
                    entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), args.Selection, true);
                    break;
            }
            ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
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

            itemDataScroll.Reset();

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }
    }
}
