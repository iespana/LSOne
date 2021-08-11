using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StoreTransfersPage : Controls.ContainerControl, ITabViewV2
    {
        private InventoryTransferType tabType;
        private StoreTransferTypeEnum pageType;
        private Setting searchBarSetting;
        private static Guid SortSettingID = new Guid("7BCA224B-CC8C-4DD3-AED3-E364F9DCCA22");
        private static Guid SendingBarSettingID = new Guid("F62AD158-988A-4486-BEEF-8486CAF9DD5D");
        private static Guid ReceivingBarSettingID = new Guid("EF166553-CAF7-4129-8DC9-70087CD14A75");
        private static Guid AllOrdersBarSettingID = new Guid("20F36FB5-39C9-4A75-BCA0-02B031CDAA63");
        private RecordIdentifier selectedID;
		private Store defaultStore;
        private Timer searchTimer;
        Setting sortSetting;
        public delegate void RefreshContextBarHandler();
        public RefreshContextBarHandler OnRefreshContextBar;
        private bool refreshDataOnTabSelected = false;
        private bool isSelectedTabView = false;
        private readonly StoreTransfersPermissionManager storeTransfersPermissionManager;

        public StoreTransfersPage(InventoryTransferType inventoryTransferType, StoreTransferTypeEnum storeTransferType)
        {
            InitializeComponent();

            this.tabType = inventoryTransferType;
            this.pageType = storeTransferType;

            storeTransfersPermissionManager = new StoreTransfersPermissionManager(PluginEntry.DataModel, pageType, tabType);

            searchBar.BuddyControl = lvItems;

            switch (tabType)
            {
                case InventoryTransferType.Outgoing:
                    lvItems.Columns.RemoveAt(7);

                    if (pageType == StoreTransferTypeEnum.Request)
                    {
                        lvItems.Columns[3].HeaderText = Resources.RequestingStore;
                        lvItems.Columns[4].HeaderText = Resources.FromStore;
                    }

                    contextButtons.Context = ButtonTypes.EditAddRemove;
                    contextButtons.Location = new Point(contextButtons.Location.X - 30, contextButtons.Location.Y);
                    break;
                case InventoryTransferType.Incoming:
                    if (pageType == StoreTransferTypeEnum.Order)
                    {
                        // In this case, change the three headers text
                        string receivingHeaderText = lvItems.Columns[4].HeaderText;
                        lvItems.Columns[4].HeaderText = lvItems.Columns[3].HeaderText;
                        lvItems.Columns[3].HeaderText = receivingHeaderText;
                        lvItems.Columns[8].HeaderText = Resources.SentDate;

                    }
                    else
                    {
                        lvItems.Columns[3].HeaderText = Resources.FromStore;
                        lvItems.Columns[4].HeaderText = Resources.RequestingStore;
                    }
                    break;
                case InventoryTransferType.SendingAndReceiving:
                    break;
                case InventoryTransferType.Finished:
                    lvItems.Columns[7].HeaderText = Resources.ReceivedDate;

                    if (pageType == StoreTransferTypeEnum.Request)
                    {
                        lvItems.Columns[3].HeaderText = Resources.RequestingStore;
                        lvItems.Columns[4].HeaderText = Resources.FromStore;
                    }
                    break;
                default:
                    break;
            }
            searchTimer = new Timer();
            searchTimer.Tick += SearchTimerOnTick;
            searchTimer.Interval = 1;

			contextButtons.AddButtonEnabled = (pageType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests));
            defaultStore = null;
            if (!string.IsNullOrWhiteSpace((string)PluginEntry.DataModel.CurrentStoreID))
            {
                searchBar.DefaultNumberOfSections = 2;
                defaultStore = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
            }
            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();
        }

        public void OnTabViewSelected(bool selected)
        {
            isSelectedTabView = selected;

            if(isSelectedTabView && refreshDataOnTabSelected)
            {
                refreshDataOnTabSelected = false;
                LoadItems();
            }
        }

        private void SearchTimerOnTick(object sender, EventArgs eventArgs)
        {
            searchTimer.Stop();
            searchTimer.Enabled = false;
            LoadItems();
        }

        public InventoryTransferType TransferType { get { return tabType; } }

        public static ITabView CreateOrderSendingInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Outgoing, StoreTransferTypeEnum.Order);
        }

        public static ITabView CreateOrderReceivingInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Incoming, StoreTransferTypeEnum.Order);
        }

        public static ITabView CreateClosedOrdersInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Finished, StoreTransferTypeEnum.Order);
        }

        public static ITabView CreateRequestSendingInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Outgoing, StoreTransferTypeEnum.Request);
        }

        public static ITabView CreateRequestReceivingInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Incoming, StoreTransferTypeEnum.Request);
        }

        public static ITabView CreateClosedRequestsInstance(object sender, ViewCore.Controls.TabControl.Tab tab)
        {
            return new StoreTransfersPage(InventoryTransferType.Finished, StoreTransferTypeEnum.Request);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {

        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                SortSettingID,
                SettingType.UISetting, lvItems.CreateSortSetting(1, true));

            lvItems.SortSetting = sortSetting.Value;

            pageType = (StoreTransferTypeEnum)internalContext;
            selectedID = context;
        }

        public void OnClose()
        {

        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            if (contextButtons.Context == ButtonTypes.EditAddRemove)
            {
                ExtendedMenuItem addItem = new ExtendedMenuItem(
                        Resources.Add,
                        100,
                        new EventHandler(contextButtons_AddButtonClicked))
                {
                    Image = ContextButtons.GetAddButtonImage(),
                    Enabled = contextButtons.AddButtonEnabled,
                    Default = true
                };

                menu.Items.Add(addItem);
            }

            ExtendedMenuItem item = new ExtendedMenuItem(
                    Resources.Edit,
                    200,
                    new EventHandler(contextButtons_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = contextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            if(tabType != InventoryTransferType.Finished)
            {
                item = new ExtendedMenuItem(
                               tabType == InventoryTransferType.Incoming ? Resources.Reject : Resources.Delete,
                               300,
                               new EventHandler(contextButtons_RemoveButtonClicked));

                item.Enabled = contextButtons.RemoveButtonEnabled;
                item.Image = ContextButtons.GetRemoveButtonImage();

                menu.Items.Add(item);
            }
            
            if (tabType == InventoryTransferType.Outgoing)
            {
                item = new ExtendedMenuItem(
                   pageType == StoreTransferTypeEnum.Order ? Resources.SendTransferOrder : Resources.SendTransferRequest,
                   300,
                   new EventHandler(SendTransferOrderOrRequest));

                item.Enabled = CanSendSelectedRows() && (pageType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests));

                menu.Items.Add(item);
            }
            else if (tabType == InventoryTransferType.Incoming)
            {
                item = new ExtendedMenuItem(
                    pageType == StoreTransferTypeEnum.Order ? Resources.ReceiveTransferOrder : Resources.CreateTransferOrder,
                    200,
                    new EventHandler(ReceiveTransferOrderOrRequest));

                item.Enabled = CanReceiveSelectedRows() && (pageType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests));

                menu.Items.Add(item);
            }

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ReceiveTransferOrderOrRequest(object sender, EventArgs e)
        {
            if (pageType == StoreTransferTypeEnum.Order)
            {
                PluginOperations.ReceiveTransferOrders(GetSelectedIDs());
            }
            else
            {
                PluginOperations.CreateTransferOrdersFromRequests(GetSelectedIDs());
            }
        }

        private List<RecordIdentifier> GetSelectedIDs()
        {
            List<RecordIdentifier> result = new List<RecordIdentifier>();

            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                result.Add(((DataEntity)lvItems.Row(lvItems.Selection.GetRowIndex(i)).Tag).ID);
            }

            return result;
        }

        private List<InventoryTransferOrder> GetSelectedOrders()
        {            
            List<InventoryTransferOrder> result = new List<InventoryTransferOrder>();

            for (int i = 0; i < lvItems.Selection.Count; i++)
            {                
                result.Add((InventoryTransferOrder)lvItems.Row(lvItems.Selection.GetRowIndex(i)).Tag);
            }

            return result;
        }

        private List<InventoryTransferRequest> GetSelectedRequests()
        {
            List<InventoryTransferRequest> result = new List<InventoryTransferRequest>();

            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                result.Add((InventoryTransferRequest)lvItems.Row(lvItems.Selection.GetRowIndex(i)).Tag);
            }

            return result;
        }

        private void SendTransferOrderOrRequest(object sender, EventArgs e)
        {
            if (pageType == StoreTransferTypeEnum.Order)
            {
                PluginOperations.SendTransferOrders(GetSelectedIDs());
            }
            else
            {
                PluginOperations.SendTransferRequests(GetSelectedIDs());
            }
        }        

        private List<ReasonCode> GetReasonCodes()
        {
            try
            {
                List<ReasonCode> reasonCodes = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetReasonCodes(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
                    InventoryJournalTypeEnum.Transfer,
                    true);

                UpdateReasonCodes(reasonCodes);

                return reasonCodes;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return null;
            }
        }

        protected virtual void UpdateReasonCodes(List<ReasonCode> reasonCodes)
        {
            bool updateHO = false;
            ReasonCode toUpdate = reasonCodes.FirstOrDefault(f => f.ID == MissingInTransferReasonConstants.ConstMissingInTransferReasonID && f.Text == MissingInTransferReasonConstants.ConstMissingInTransferReasonID);
            if (toUpdate != null)
            {
                toUpdate.Text = Resources.ReasonCodeMissingInTransit;
                updateHO = true;
            }

            if (updateHO)
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).UpdateReasonCodes(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(PluginEntry.DataModel),
                    reasonCodes,
                    InventoryJournalTypeEnum.Transfer,
                    true);
            }
        }

        public void LoadItems()
        {
            ViewBase parentView = (ViewBase)Parent.Parent.Parent;

            try
            {
                InventoryTransferFilterExtended filter = GetSearchFilter();

                switch (pageType)
                {
                    case StoreTransferTypeEnum.Order:
                        parentView.ShowProgress((sender1, e1) => LoadTransferOrders(filter), parentView.GetLocalizedSearchingText());                        
                        break;
                    case StoreTransferTypeEnum.Request:
                        parentView.ShowProgress((sender1, e1) => LoadTransferRequests(filter), parentView.GetLocalizedSearchingText());                        
                        break;
                }
            }
            finally
            {
                parentView.HideProgress();
            }
        }

        private void LoadTransferRequests(InventoryTransferFilterExtended filter)
        {
            try
            {
                int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

                if (lvItems.SortColumn != null)
                {
                    filter.SortBy = GetSort(sortColumnIndex);
                }

                int groupCount;

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                List<InventoryTransferRequest> transferRequests = service.SearchInventoryTransferRequestsAdvanced(
                    PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(),
                    out groupCount,
                    filter,
                    true);

                lvItems.ClearRows();

                itemDataScroll.RefreshState(transferRequests, groupCount);

                foreach (InventoryTransferRequest request in transferRequests)
                {
                    Row row = new Row();
                    Bitmap statusImage = Resources.dot_grey_16;

                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            if(request.Rejected)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            else if (request.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_green_16;
                            }
                            else if (request.Sent)
                            {
                                statusImage = Resources.dot_yellow_16;
                            }
                            break;
                        case InventoryTransferType.Incoming:
                            if (DateTime.Now.Date > request.ExpectedDelivery.Date && !request.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            else if (!request.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_yellow_16;
                            }
                            else
                            {
                                statusImage = Resources.dot_green_16;
                            }
                            break;
                        case InventoryTransferType.SendingAndReceiving:
                            break;
                        case InventoryTransferType.Finished:
                            if (request.InventoryTransferOrderCreated)
                            {
                                statusImage = Resources.dot_finished_16;
                            }
                            else if (request.Rejected)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            break;
                        default:
                            break;
                    }
                    
                    row.AddCell(new ExtendedCell(string.Empty, statusImage));
                    row.AddText(request.ID.ToString());
                    row.AddText(request.Text);

                    if(tabType == InventoryTransferType.Incoming)
                    {
                        row.AddText(request.ReceivingStoreName);
                        row.AddText(request.SendingStoreName);
                    }
                    else
                    {
                        row.AddText(request.SendingStoreName);
                        row.AddText(request.ReceivingStoreName);
                    }

                    row.AddText(request.NoOfItems.ToString());

                    // Status
                    if (request.Rejected)
                    {
                        row.AddText(Resources.Rejected);
                    }
                    else if (request.InventoryTransferOrderCreated)
                    {
                        row.AddText(Resources.Closed);
                    }
                    else if (request.FetchedByReceivingStore)
                    {
                        row.AddText(Resources.Viewed);
                    }
                    else if (request.Sent)
                    {
                        row.AddText(Resources.Requested);
                    }
                    else
                    {
                        row.AddText(Resources.New);
                    }

                    if (tabType == InventoryTransferType.Incoming)
                    {
                        row.AddText(request.ExpectedDelivery.ToShortDateString());
                    }

                    if (tabType == InventoryTransferType.Finished)
                    {
                        row.AddText("-");
                    }

                    row.AddText(request.SentDate.ToShortDateString());

                    row.Tag = request;
                    lvItems.AddRow(row);

                    if (selectedID == request.ID)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                    }
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            lvItems_SelectionChanged(this, EventArgs.Empty);
            lvItems.AutoSizeColumns();
        }

        private InventoryTransferOrderSortEnum GetSort(int sortColumnIndex)
        {
            switch (tabType)
            {
                case InventoryTransferType.Outgoing:
                    switch (sortColumnIndex)
                    {
                        case 1:
                            return InventoryTransferOrderSortEnum.Id;
                        case 2:
                            return InventoryTransferOrderSortEnum.Description;
                        case 3:
                            return InventoryTransferOrderSortEnum.SendingStore;
                        case 4:
                            return InventoryTransferOrderSortEnum.ReceivingStore;
                        case 5:
                            return InventoryTransferOrderSortEnum.ItemLines;
                        case 6:
                            return InventoryTransferOrderSortEnum.Description;
                        case 7:
                            return InventoryTransferOrderSortEnum.SentDate;
                    }
                    break;
                case InventoryTransferType.Finished:
                    switch (sortColumnIndex)
                    {
                        case 1:
                            return InventoryTransferOrderSortEnum.Id;
                        case 2:
                            return InventoryTransferOrderSortEnum.Description;
                        case 3:
                            return InventoryTransferOrderSortEnum.SendingStore;
                        case 4:
                            return InventoryTransferOrderSortEnum.ReceivingStore;
                        case 5:
                            return InventoryTransferOrderSortEnum.ItemLines;
                        case 6:
                            return InventoryTransferOrderSortEnum.Description;
                        case 7:
                            return InventoryTransferOrderSortEnum.ReceivedDate;
                        case 8:
                            return InventoryTransferOrderSortEnum.SentDate;
                    }
                    break;
                case InventoryTransferType.Incoming:
                    switch (sortColumnIndex)
                    {
                        case 1:
                            return InventoryTransferOrderSortEnum.Id;
                        case 2:
                            return InventoryTransferOrderSortEnum.Description;
                        case 3:
                            return InventoryTransferOrderSortEnum.ReceivingStore;
                        case 4:
                            return InventoryTransferOrderSortEnum.SendingStore;
                        case 5:
                            return InventoryTransferOrderSortEnum.ItemLines;
                        case 6:
                            return InventoryTransferOrderSortEnum.Description;
                        case 7:
                            return InventoryTransferOrderSortEnum.ExpectedDelivery;
                        case 8:
                            return InventoryTransferOrderSortEnum.SentDate;
                    }
                    break;
            }
            return InventoryTransferOrderSortEnum.Id;
        }

        private void LoadTransferOrders(InventoryTransferFilterExtended filter)
        {
            try
            {
                int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

                if (lvItems.SortColumn != null)
                {
                    filter.SortBy = GetSort(sortColumnIndex);
                }

                int groupCount;

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                List<InventoryTransferOrder> transferOrders = service.SearchInventoryTransferOrdersAdvanced(
                    PluginEntry.DataModel, 
                    PluginOperations.GetSiteServiceProfile(), 
                    out groupCount, 
                    filter, 
                    true);

                lvItems.ClearRows();

                itemDataScroll.RefreshState(transferOrders, groupCount);

                foreach (InventoryTransferOrder order in transferOrders)
                {
                    Row row = new Row();
                    Bitmap statusImage = Resources.dot_grey_16;

                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            if (order.Rejected)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            else if (order.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_green_16;
                            }
                            else if (order.Sent)
                            {
                                statusImage = Resources.dot_yellow_16;
                            }
                            break;
                        case InventoryTransferType.Incoming:
                            if (DateTime.Now.Date > order.ExpectedDelivery.Date && !order.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            else if (!order.FetchedByReceivingStore)
                            {
                                statusImage = Resources.dot_yellow_16;
                            }
                            else
                            {
                                statusImage = Resources.dot_green_16;
                            }
                            break;
                        case InventoryTransferType.SendingAndReceiving:
                            break;
                        case InventoryTransferType.Finished:
                            if (order.Received)
                            {
                                statusImage = Resources.dot_finished_16;
                            }
                            else if (order.Rejected)
                            {
                                statusImage = Resources.dot_red_16;
                            }
                            break;
                        default:
                            break;
                    }

                    row.AddCell(new ExtendedCell(string.Empty, statusImage));
                    row.AddText(order.ID.ToString());
                    row.AddText(order.Text);

                    if (tabType == InventoryTransferType.Incoming)
                    {
                        row.AddText(order.ReceivingStoreName);
                        row.AddText(order.SendingStoreName);
                    }
                    else
                    {
                        row.AddText(order.SendingStoreName);
                        row.AddText(order.ReceivingStoreName);
                    }

                    row.AddText(order.NoOfItems.ToString());

                    // Status
                    if(order.Rejected)
                    {
                        row.AddText(Resources.Rejected);
                    }
                    else if (order.Received)
                    {
                        row.AddText(Resources.Closed);
                    }
                    else if (order.FetchedByReceivingStore)
                    {
                        row.AddText(Resources.Viewed);
                    }
                    else if (order.Sent)
                    {
                        row.AddText(Resources.Sent);
                    }
                    else
                    {
                        row.AddText(Resources.New);
                    }

                    if (tabType == InventoryTransferType.Incoming)
                    {
                        row.AddText(order.ExpectedDelivery.ToShortDateString());
                    }

                    if(tabType == InventoryTransferType.Finished)
                    {
                        row.AddText(order.Received ? order.ReceivingDate.ToShortDateString() : "-");
                    }

                    row.AddText((tabType == InventoryTransferType.Outgoing && !order.Sent) ? "-" : order.SentDate.ToShortDateString());

                    row.Tag = order;
                    lvItems.AddRow(row);

                    if (selectedID == order.ID)
                    {
                        lvItems.Selection.Set(lvItems.RowCount - 1);
                    }
                }
            }
            catch
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lvItems_SelectionChanged(this, EventArgs.Empty);
            lvItems.AutoSizeColumns();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "InventoryTransferOrder" || objectName == "InventoryTransferRequest" || objectName == "InventoryTransferRequestLine" || objectName == "InventoryTransferOrderLine" || objectName == "SyncFromSAP")
            {
                if(isSelectedTabView)
                {
                    LoadItems();
                }
                else
                {
                    refreshDataOnTabSelected = true;
                }
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            string newSortSetting = lvItems.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, SortSettingID, SettingsLevel.User, sortSetting);
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = lvItems.Selection.Count == 1
                          ? (pageType == StoreTransferTypeEnum.Order ? ((InventoryTransferOrder)lvItems.Selection[0].Tag).ID : ((InventoryTransferRequest)lvItems.Selection[0].Tag).ID)
                          : null;
            
            contextButtons.EditButtonEnabled =  lvItems.Selection.Count == 1;
            contextButtons.RemoveButtonEnabled = (pageType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests)) && CanDeleteSelectedRows();
            
            OnRefreshContextBar?.Invoke();
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            contextButtons_EditButtonClicked(sender, EventArgs.Empty);
        }

        #region Search

        private Guid GetBarSettingID()
        {
            switch (tabType)
            {
                case InventoryTransferType.Outgoing:
                    return SendingBarSettingID;
                case InventoryTransferType.Incoming:
                    return ReceivingBarSettingID;
                case InventoryTransferType.SendingAndReceiving:
                    return Guid.Empty;
                case InventoryTransferType.Finished:
                    return AllOrdersBarSettingID;
                default:
                    return Guid.Empty;
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "idOrDescription", ConditionType.ConditionTypeEnum.Text));
            switch (pageType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            searchBar.AddCondition(new ConditionType(Resources.SendingStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.ReceivingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                        case InventoryTransferType.Incoming:
                            searchBar.AddCondition(new ConditionType(Resources.ReceivingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.SendingStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                        case InventoryTransferType.Finished:
                            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.SendingStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.ReceivingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            searchBar.AddCondition(new ConditionType(Resources.RequestingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.FromStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                        case InventoryTransferType.Incoming:
                            searchBar.AddCondition(new ConditionType(Resources.FromStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.RequestingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                        case InventoryTransferType.Finished:
                            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.RequestingStore, "ReceivingStore", ConditionType.ConditionTypeEnum.Unknown));
                            searchBar.AddCondition(new ConditionType(Resources.FromStore, "SendingStore", ConditionType.ConditionTypeEnum.Unknown));
                            break;
                    }
                    break;
            }

            searchBar.AddCondition(new ConditionType(Resources.ItemLines, "ItemLines", ConditionType.ConditionTypeEnum.NumericRange));
            searchBar.AddCondition(new ConditionType(Resources.SentDate, "SentDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

            List<object> statuses = new List<object>
            {
                Resources.All
            };

            switch (tabType)
            {
                case InventoryTransferType.Outgoing:
                    statuses.Add(Resources.New);
                    statuses.Add(pageType == StoreTransferTypeEnum.Order ? Resources.Sent : Resources.Requested);
                    statuses.Add(Resources.Viewed);
                    break;
                case InventoryTransferType.Incoming:
                    searchBar.AddCondition(new ConditionType(Resources.ExpectedDate, "ExpectedDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));

                    statuses.Add(pageType == StoreTransferTypeEnum.Order ? Resources.Sent : Resources.Requested);
                    statuses.Add(Resources.Viewed);
                    break;
                case InventoryTransferType.SendingAndReceiving:
                    break;
                case InventoryTransferType.Finished:
                    statuses.Add(Resources.Closed);
                    statuses.Add(Resources.Rejected);
                    break;
                default:
                    break;
            }

            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statuses, 0, 0, false));

            searchBar_LoadDefault(this, EventArgs.Empty);

            searchTimer.Enabled = true;
            searchTimer.Start();
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, GetBarSettingID(), SettingType.UISetting, "");

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

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, GetBarSettingID(), SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            DataEntity selectedData = null;

            args.UnknownControl = new DualDataComboBox();
            args.UnknownControl.Size = new Size(200, 21);
            args.MaxSize = 200;
            args.AutoSize = false;
            var dualComboBox = ((DualDataComboBox)args.UnknownControl);
            dualComboBox.ShowDropDownOnTyping = true;

            switch (args.TypeKey)
            {
                case "Store":
                    selectedData = GetSelectedDataForStore();
                    dualComboBox.RequestData += Store_RequestData;
                    break;
                case "SendingStore":
                    selectedData = GetSelectedDataForSendingStore();
                    dualComboBox.RequestData += SendingStore_RequestData;
                    break;
                case "ReceivingStore":
                    selectedData = GetSelectedDataForReceivingStore();
                    dualComboBox.RequestData += ReceivingStore_RequestData;
                    break;
            }

            dualComboBox.SelectedData = selectedData ?? new DataEntity("", "");
        }

        private DataEntity GetSelectedDataForReceivingStore()
        {
            DataEntity selectedData = null;

            switch (pageType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (tabType)
                    {
                        case InventoryTransferType.Incoming:
                            selectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                        default:
                            selectedData = defaultStore == null || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores) ?
                                new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            selectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                        default:
                            selectedData = defaultStore == null || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores) ?
                                new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                    }
                    break;
            }

            return selectedData;
        }

        private DataEntity GetSelectedDataForSendingStore()
        {
            DataEntity selectedData = null;

            switch (pageType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (tabType)
                    {
                        case InventoryTransferType.Outgoing:
                            selectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                        default:
                            selectedData = defaultStore == null || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores) ?
                                new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    switch (tabType)
                    {
                        case InventoryTransferType.Incoming:
                            selectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                        default:
                            selectedData = defaultStore == null || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores) ?
                                new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                    }
                    break;
            }

            return selectedData;
        }

        private DataEntity GetSelectedDataForStore()
        {
            DataEntity selectedData = null;

            switch (pageType)
            {
                case StoreTransferTypeEnum.Order:
                case StoreTransferTypeEnum.Request:
                    switch (tabType)
                    {
                        case InventoryTransferType.Finished:
                            selectedData = defaultStore == null ? new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                        default:
                            selectedData = defaultStore == null || PluginEntry.DataModel.HasPermission(Permission.ManageTransfersForAllStores) ?
                                new DataEntity(null, Resources.AllStores) : new DataEntity(defaultStore.ID, defaultStore.Text);
                            break;
                    }
                    break;
            }

            return selectedData;
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            itemDataScroll.Reset();
            LoadItems();
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
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
                case "Store":
                case "SendingStore":
                case "ReceivingStore":
                    var dualComboBox = ((DualDataComboBox)args.UnknownControl);
                    args.HasSelection = dualComboBox.SelectedData.ID != "" && dualComboBox.SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            var dualComboBox = ((DualDataComboBox)args.UnknownControl);

            switch (args.TypeKey)
            {
                case "Store":
                    dualComboBox.RequestData -= Store_RequestData;
                    break;
                case "SendingStore":
                    dualComboBox.RequestData -= SendingStore_RequestData;
                    break;
                case "ReceivingStore":
                    dualComboBox.RequestData -= ReceivingStore_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            bool hasAccessToAllStores = false;
            DataEntity selectedData = null;

            switch (args.TypeKey)
            {
                case "Store":
                    hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("Store");
                    selectedData = hasAccessToAllStores ?   Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                            new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
                case "SendingStore":
                    hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("SendingStore");
                    selectedData = hasAccessToAllStores ?   Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                            new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
                case "ReceivingStore":
                    hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore");
                    selectedData = hasAccessToAllStores ?   Providers.StoreData.Get(PluginEntry.DataModel, args.Selection) ?? new DataEntity(null, Resources.AllStores) :
                                                            new DataEntity(defaultStore.ID, defaultStore.Text);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = selectedData ?? new DataEntity("", "");
        }

        private void Store_RequestData(object sender, EventArgs eventArgs)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;

            var hasAccessToAllStores = storeTransfersPermissionManager.HasAccessToAllStores("Store");
            var stores = hasAccessToAllStores ? PluginOperations.GetAllStores() : PluginOperations.GetCurrentStore();

            ((DualDataComboBox)sender).SetData(stores, null);
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

        #endregion Search

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            if(pageType == StoreTransferTypeEnum.Order)
            {
                PluginOperations.ShowInventoryTransferOrderWizard(sender, e);
            }
            else
            {
                PluginOperations.ShowInventoryTransferRequestWizard(sender, e);
            }
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (contextButtons.EditButtonEnabled)
            {
                switch (pageType)
                {
                    case StoreTransferTypeEnum.Order:
                        PluginOperations.ShowInventoryTransferOrderItemsView((InventoryTransferOrder)lvItems.Selection[0].Tag, tabType);
                        break;
                    case StoreTransferTypeEnum.Request:
                        PluginOperations.ShowInventoryTransferRequestsItemsView((InventoryTransferRequest)lvItems.Selection[0].Tag, tabType);
                        break;
                }
            }
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(pageType == StoreTransferTypeEnum.Order)
            {
                PluginOperations.DeleteInventoryTransferOrders(GetSelectedIDs(), tabType == InventoryTransferType.Incoming);
            }
            else
            {
                PluginOperations.DeleteInventoryTransferRequests(GetSelectedIDs(), tabType == InventoryTransferType.Incoming);
            }
        }

        private bool CanDeleteSelectedRows()
        {
            if (lvItems.Selection.Count >= 1)
            {
                if(pageType == StoreTransferTypeEnum.Order)
                {
                    List<InventoryTransferOrder> selected = GetSelectedOrders();

                    if(tabType == InventoryTransferType.Outgoing)
                    {
                        return selected.Any(x => !x.Sent);
                    }
                    else if(tabType == InventoryTransferType.Incoming)
                    {
                        return selected.Any(x => !x.Rejected && !x.Received);
                    }
                }
                else
                {
                    List<InventoryTransferRequest> selected = GetSelectedRequests();

                    if(tabType == InventoryTransferType.Outgoing)
                    {
                        return selected.Any(x => !x.FetchedByReceivingStore);
                    }
                    else if (tabType == InventoryTransferType.Incoming)
                    {
                        return selected.Any(x => !x.Rejected && !x.InventoryTransferOrderCreated);
                    }
                }
            }

            return false;
        }

        public bool CanSendSelectedRows()
        {
            if (lvItems.Selection.Count >= 1)
            {
                if (tabType == InventoryTransferType.Outgoing)
                {
                    if (pageType == StoreTransferTypeEnum.Order)
                    {
                        List<InventoryTransferOrder> selectedOrders = GetSelectedOrders();
                        if (selectedOrders.Any(x => !x.Sent && !x.Rejected && x.NoOfItems > 0))
                        {
                            return true;
                        }                        
                    }
                    else
                    {
                        List<InventoryTransferRequest> selectedRequests = GetSelectedRequests();
                        if (selectedRequests.Any(x => !x.Rejected && x.NoOfItems > 0))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CanReceiveSelectedRows()
        {
            if (lvItems.Selection.Count >= 1)
            {
                if (tabType == InventoryTransferType.Incoming)
                {
                    if (pageType == StoreTransferTypeEnum.Order)
                    {
                        List<InventoryTransferOrder> selected = GetSelectedOrders();
                        if (selected.Any(a => !a.Received && !a.Rejected))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        List<InventoryTransferRequest> selected = GetSelectedRequests();
                        if (selected.Any(a => !a.InventoryTransferOrderCreated && !a.Rejected))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private InventoryTransferFilterExtended GetSearchFilter()
        {
            InventoryTransferFilterExtended filter = new InventoryTransferFilterExtended();
            filter.RowFrom = itemDataScroll.StartRecord;
            filter.RowTo = itemDataScroll.EndRecord + 1;
            filter.SortDescending = !lvItems.SortedAscending;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            // Currently, the user is able to remove the filters, so make sure the filters are limited to the resources the user has access to
            if (tabType == InventoryTransferType.Finished)
            {
                if (!storeTransfersPermissionManager.HasAccessToAllStores("Store"))
                {
                    filter.StoreID = PluginEntry.DataModel.CurrentStoreID;
                }
            }
            else
            {
                // Do not restrict the filters SendingStore and ReceivingStore to the same StoreID on the Finish tab because it results in displaying nothing in the grid
                if (!storeTransfersPermissionManager.HasAccessToAllStores("SendingStore"))
                {
                    filter.SendingStoreID = PluginEntry.DataModel.CurrentStoreID;
                }
                if (!storeTransfersPermissionManager.HasAccessToAllStores("ReceivingStore"))
                {
                    filter.ReceivingStoreID = PluginEntry.DataModel.CurrentStoreID;
                }
            }

            filter.TransferFilterType = tabType;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "idOrDescription":
                        filter.DescriptionOrID = result.StringValue;
                        filter.DescriptionOrIDBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Store":
                        filter.StoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
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
                    case "SentDate":
                        filter.SentFrom = result.Date.Checked ? result.Date.Value.Date : (DateTime?)null;
                        filter.SentTo = result.DateTo.Checked ? result.DateTo.Value.Date : (DateTime?)null;
                        break;
                    case "ExpectedDate":
                        filter.ExpectedFrom = result.Date.Checked ? result.Date.Value.Date : (DateTime?)null;
                        filter.ExpectedTo = result.DateTo.Checked ? result.DateTo.Value.Date : (DateTime?)null;
                        break;
                    case "ItemLines":
                        filter.ItemsFrom = (decimal)result.FromValue;
                        filter.ItemsTo = (decimal)result.ToValue;
                        break;
                    case "Status":
                        if(result.ComboSelectedIndex != 0)
                        {
                            switch(result.ComboSelectedIndex)
                            {
                                case 1:
                                    filter.Status = tabType == InventoryTransferType.Outgoing
                                        ? TransferOrderStatusEnum.New 
                                        : (tabType == InventoryTransferType.Incoming ? TransferOrderStatusEnum.Sent : TransferOrderStatusEnum.Closed);
                                    break;
                                case 2:
                                    filter.Status = tabType == InventoryTransferType.Outgoing
                                        ? TransferOrderStatusEnum.Sent
                                        : (tabType == InventoryTransferType.Incoming ? TransferOrderStatusEnum.Received : TransferOrderStatusEnum.Rejected);
                                    break;
                                case 3:
                                    filter.Status = TransferOrderStatusEnum.Received;
                                    break;
                            }
                        }
                        break;
                }
            }

            return filter;
        }

        public void ContextMessage(string message)
        {
            switch(message)
            {
                case "SendTransfer":
                    SendTransferOrderOrRequest(this, EventArgs.Empty);
                    break;
                case "CreateTransfer":
                    ReceiveTransferOrderOrRequest(this, EventArgs.Empty);
                    break;
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

            itemDataScroll.Reset();

            LoadItems();
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            LoadItems();
        }
    }
}