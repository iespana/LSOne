using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class ExistingStoreTransferSearch : UserControl, IWizardPage
    {
        private static Guid BarSettingID = new Guid("AB4B3FFD-6C8F-4216-9C83-CDDE24BAE0A2");
        WizardBase parent;
        private StoreTransferActionEnum storeTransferAction;
        private Setting searchBarSetting;
        private StoreTransferTypeEnum transferType;
        private Store defaultStore;
        private readonly StoreTransfersPermissionManager storeTransfersPermissionManager;

        public ExistingStoreTransferSearch(WizardBase parent, StoreTransferActionEnum storeTransferAction, StoreTransferTypeEnum transferType) : base()
        {
            InitializeComponent();

            this.parent = parent;
            this.storeTransferAction = storeTransferAction;
            this.transferType = transferType;

            storeTransfersPermissionManager = new StoreTransfersPermissionManager(PluginEntry.DataModel, transferType, InventoryTransferType.Outgoing);

            lvItems.Columns[0].Tag = InventoryTransferOrderSortEnum.Id;
            lvItems.Columns[1].Tag = InventoryTransferOrderSortEnum.Description;
            lvItems.Columns[2].Tag = InventoryTransferOrderSortEnum.SendingStore;
            lvItems.Columns[3].Tag = InventoryTransferOrderSortEnum.ReceivingStore;
            lvItems.Columns[4].Tag = InventoryTransferOrderSortEnum.SentDate;
            lvItems.Columns[5].Tag = InventoryTransferOrderSortEnum.ReceivedDate;

            lvItems.SetSortColumn(0, true);

            searchBar.BuddyControl = lvItems;
            searchBar.FocusFirstInput();

            defaultStore = null;
            if (!RecordIdentifier.IsEmptyOrNull(PluginEntry.DataModel.CurrentStoreID))
            {
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }
        }

        #region IWizardPage Members

        public bool HasFinish
        {
            get { return false; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            searchBar_SearchClicked(this, EventArgs.Empty);
            CheckState();
        }

        private void CheckState()
        {
            parent.NextEnabled = lvItems.Selection.Count >= 1;
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return storeTransferAction == StoreTransferActionEnum.GenerateFromOrder
                ? new NewStoreTransfer(parent, (InventoryTransferOrder)lvItems.Selection[0].Tag, transferType)
                : new NewStoreTransfer(parent, (InventoryTransferRequest)lvItems.Selection[0].Tag, transferType);
        }

        public void ResetControls()
        {

        }

        #endregion

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));

            if(storeTransferAction == StoreTransferActionEnum.GenerateFromRequest)
            {
                searchBar.AddCondition(new ConditionType(Resources.RequestingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                searchBar.AddCondition(new ConditionType(Resources.FromStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
            }
            else
            {
                searchBar.AddCondition(new ConditionType(Resources.SendingStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                searchBar.AddCondition(new ConditionType(Resources.ReceivingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
            }

            searchBar.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

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
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "SendingStore":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = storeTransfersPermissionManager.HasAccessToAllStores("SendingStore") ?
                        new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                    ((DualDataComboBox)args.UnknownControl).RequestData += SendingStore_RequestData;
                    break;
                case "ReceivingStore":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore") ?
                        new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                    ((DualDataComboBox)args.UnknownControl).RequestData += ReceivingStore_RequestData;
                    break;
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void LoadItems()
        {
            lvItems.ClearRows();

            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[0], true);
            }

            InventoryTransferFilter filter = GetSearchFilter();

            if(storeTransferAction == StoreTransferActionEnum.GenerateFromOrder)
            {
                LoadTransferOrders(filter);
            }
            else
            {
                LoadTransferRequests(filter);
            }

            lvItems.AutoSizeColumns();
            lvItems_SelectionChanged(this, EventArgs.Empty);
        }

        private void LoadTransferOrders(InventoryTransferFilter filter)
        {
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                List<InventoryTransferOrder> transferOrders = service.SearchInventoryTransferOrders(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, true);

                foreach(InventoryTransferOrder order in transferOrders)
                {
                    Row row = new Row();
                    row.AddText(order.ID.ToString());
                    row.AddText(order.Text);
                    row.AddText(order.SendingStoreName);
                    row.AddText(order.ReceivingStoreName);
                    row.AddText(order.Sent ? order.SentDate.ToShortDateString() : "-");
                    row.AddText(order.Received ? order.ReceivingDate.ToShortDateString() : "-");
                    row.Tag = order;

                    lvItems.AddRow(row);
                }
            }
            catch
            {
                MessageBox.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void LoadTransferRequests(InventoryTransferFilter filter)
        {
            try
            {
                if (storeTransferAction == StoreTransferActionEnum.GenerateFromRequest)
                {
                    lvItems.Columns[5].HeaderText = "";
                    lvItems.Columns[5].Clickable = false;
                }
                lvItems.Columns[2].HeaderText = Resources.RequestingStore;
                lvItems.Columns[3].HeaderText = Resources.FromStore;

                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                List<InventoryTransferRequest> transferOrders = service.SearchInventoryTransferRequests(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, true);

                foreach (InventoryTransferRequest order in transferOrders)
                {
                    Row row = new Row();
                    row.AddText(order.ID.ToString());
                    row.AddText(order.Text);
                    row.AddText(order.SendingStoreName);
                    row.AddText(order.ReceivingStoreName);
                    row.AddText(order.Sent ? order.SentDate.ToShortDateString() : "-");
                    row.Tag = order;

                    lvItems.AddRow(row);
                }
            }
            catch
            {
                MessageBox.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private InventoryTransferFilter GetSearchFilter()
        {
            InventoryTransferFilter filter = new InventoryTransferFilter();

            filter.SortBy = (InventoryTransferOrderSortEnum)lvItems.SortColumn.Tag;
            filter.SortDescending = !lvItems.SortedAscending;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            if (!storeTransfersPermissionManager.HasAccessToAllStores("SendingStore"))
            {
                filter.SendingStoreID = PluginEntry.DataModel.CurrentStoreID;
            }
            if (!storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore"))
            {
                filter.ReceivingStoreID = PluginEntry.DataModel.CurrentStoreID;
            }

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "idOrDescription":
                        filter.DescriptionOrID = result.StringValue;
                        filter.DescriptionOrIDBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "SendingStore":
                        filter.SendingStoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "ReceivingStore":
                        filter.ReceivingStoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Date":
                        filter.FromDate = result.Date.Checked ? result.Date.Value.Date : (DateTime?)null;
                        filter.ToDate = result.DateTo.Checked ? result.DateTo.Value.Date : (DateTime?)null;
                        break;
                }
            }

            return filter;
        }

        private void SendingStore_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            var hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("SendingStore");
            var stores = hasAccessToAllStores ? PluginOperations.GetAllStores() : PluginOperations.GetCurrentStore();

            ((DualDataComboBox)sender).SetData(stores, null);
        }

        private void ReceivingStore_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            var hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore");
            var stores = hasAccessToAllStores ? PluginOperations.GetAllStores() : PluginOperations.GetCurrentStore();

            ((DualDataComboBox)sender).SetData(stores, null);
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "SendingStore":
                case "ReceivingStore":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "SendingStore":
                case "ReceivingStore":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "SendingStore":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= SendingStore_RequestData;
                    break;
                case "ReceivingStore":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= ReceivingStore_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            bool hasAccessToAllStores = false;
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "SendingStore":
                    hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("SendingStore");
                    entity = hasAccessToAllStores ? Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                    new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
                case "ReceivingStore":
                    hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore");
                    entity = hasAccessToAllStores ? Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                    new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            CheckState();
        }
    }
}