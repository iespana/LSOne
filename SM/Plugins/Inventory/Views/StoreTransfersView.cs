using LSOne.ViewCore;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using LSOne.Controls;
using System;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class StoreTransfersView : ViewBase
    {
        private StoreTransferTypeEnum transferType;
        private ViewCore.Controls.TabControl.Tab sendingTab;
        private ViewCore.Controls.TabControl.Tab receivingTab;
        private ViewCore.Controls.TabControl.Tab allOrdersTab;
        bool canRefreshContextBar = false;

        private RecordIdentifier selectedStoreTransferID = RecordIdentifier.Empty;

        public StoreTransfersView(StoreTransferTypeEnum transferType, RecordIdentifier storeTransferId)
            : this(transferType)
        {
            selectedStoreTransferID = storeTransferId;

            ReadOnly = !(transferType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests));
        }

        public StoreTransfersView(StoreTransferTypeEnum transferType)
        {
            InitializeComponent();
            this.transferType = transferType;

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }

        public string Description
        {
            get
            {
                return transferType == StoreTransferTypeEnum.Order ? Properties.Resources.TransferOrders : Properties.Resources.TransferRequests;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return Description;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Description;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return (int)transferType;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            switch (transferType)
            {
                case StoreTransferTypeEnum.Order:
                    contexts.Add(new AuditDescriptor("InventoryTransferOrders", RecordIdentifier.Empty, Properties.Resources.InventoryTransferOrders));
                    contexts.Add(new AuditDescriptor("InventoryTransferOrderLines", RecordIdentifier.Empty, Properties.Resources.InventoryTransferOrderLines));
                    break;
                case StoreTransferTypeEnum.Request:
                    contexts.Add(new AuditDescriptor("InventoryTransferRequests", RecordIdentifier.Empty, Properties.Resources.InventoryTransferRequests));
                    contexts.Add(new AuditDescriptor("InventoryTransferRequestLines", RecordIdentifier.Empty, Properties.Resources.InventoryTransferRequestLines));
                    break;
                default:
                    break;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                sendingTab = transferType == StoreTransferTypeEnum.Order
                            ? new ViewCore.Controls.TabControl.Tab(Properties.Resources.OutgoingOrders, ViewPages.StoreTransfersPage.CreateOrderSendingInstance)
                            : new ViewCore.Controls.TabControl.Tab(Properties.Resources.OutgoingRequests, ViewPages.StoreTransfersPage.CreateRequestSendingInstance);
                receivingTab = transferType == StoreTransferTypeEnum.Order
                            ? new ViewCore.Controls.TabControl.Tab(Properties.Resources.IncomingOrders, ViewPages.StoreTransfersPage.CreateOrderReceivingInstance)
                            : new ViewCore.Controls.TabControl.Tab(Properties.Resources.IncomingRequests, ViewPages.StoreTransfersPage.CreateRequestReceivingInstance);
                allOrdersTab = transferType == StoreTransferTypeEnum.Order
                            ? new ViewCore.Controls.TabControl.Tab(Properties.Resources.ClosedOrders, ViewPages.StoreTransfersPage.CreateClosedOrdersInstance)
                            : new ViewCore.Controls.TabControl.Tab(Properties.Resources.ClosedRequests, ViewPages.StoreTransfersPage.CreateClosedRequestsInstance);

                tabSheetTabs.AddTab(sendingTab);
                tabSheetTabs.AddTab(receivingTab);
                tabSheetTabs.AddTab(allOrdersTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, null, transferType);
            }

            HeaderText = Description;
            tabSheetTabs.SetData(isRevert, selectedStoreTransferID, transferType);
        }

        private void RefreshContextBar()
        {
            if(canRefreshContextBar)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
            else
            {
                canRefreshContextBar = true;
            }
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "InventoryTransferOrder" || objectName == "InventoryTransferRequest" || objectName == "InventoryTransferRequestLine" || objectName == "InventoryTransferOrderLine" || objectName == "SyncFromSAP")
            {
                tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                switch (transferType)
                {
                    case StoreTransferTypeEnum.Order:
                        arguments.Add(new ContextBarItem(Properties.Resources.TransferRequests, "TransferRequests", PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests), PluginOperations.ShowStoreTransfersRequestsView), 100);
                        break;
                    case StoreTransferTypeEnum.Request:
                        arguments.Add(new ContextBarItem(Properties.Resources.TransferOrders, "TransferOrders", PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders), PluginOperations.ShowStoreTransfersOrdersView), 100);
                        break;
                    default:
                        break;
                }

                arguments.Add(new ContextBarItem(Properties.Resources.InventoryInTransit, null, PluginOperations.ShowInventoryInTransitView), 200);
            }

            if (arguments.CategoryKey == GetType().ToString() + ".Actions")
            {
                ViewPages.StoreTransfersPage currentPage = (ViewPages.StoreTransfersPage)tabSheetTabs.SelectedTab.View;

                if(currentPage.TransferType == InventoryTransferType.Outgoing)
                {
                    arguments.Add(new ContextBarItem(transferType == StoreTransferTypeEnum.Order ? Properties.Resources.SendTransferOrder : Properties.Resources.SendTransferRequest,
                        null, currentPage.CanSendSelectedRows() && (transferType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests)),
                        SendSendTransferMessage), 100);                    
                }
                else if (currentPage.TransferType == InventoryTransferType.Incoming)
                {
                    arguments.Add(new ContextBarItem(transferType == StoreTransferTypeEnum.Order ? Properties.Resources.ReceiveTransferOrder : Properties.Resources.CreateTransferOrder,
                        null, currentPage.CanReceiveSelectedRows() && (transferType == StoreTransferTypeEnum.Order ? PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) : PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests)),
                        SendCreateTransferMessage), 100);
                }
            }

            base.OnSetupContextBarItems(arguments);
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Properties.Resources.Action, GetType().ToString() + ".Actions"), 200);
            base.OnSetupContextBarHeaders(arguments);
        }

        protected override void OnClose()
        {
            if (sendingTab.View != null)
            {
                ((ViewPages.StoreTransfersPage)sendingTab.View).OnRefreshContextBar -= RefreshContextBar;
            }

            if (receivingTab.View != null)
            {
                ((ViewPages.StoreTransfersPage)receivingTab.View).OnRefreshContextBar -= RefreshContextBar;
            }

            if (allOrdersTab.View != null)
            {
                ((ViewPages.StoreTransfersPage)allOrdersTab.View).OnRefreshContextBar -= RefreshContextBar;
            }

            tabSheetTabs.SendCloseMessage();
            base.OnClose();
        }

        private void tabSheetTabs_SelectedTabChanged(object sender, EventArgs e)
        {
            ViewPages.StoreTransfersPage currentPage = (ViewPages.StoreTransfersPage)tabSheetTabs.SelectedTab.View;

            if(currentPage.OnRefreshContextBar == null)
            {
                currentPage.OnRefreshContextBar += RefreshContextBar;
            }

            if(canRefreshContextBar)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }

            if(sendingTab != null && sendingTab.View != null)
            {
                ViewPages.StoreTransfersPage sendingPage = (ViewPages.StoreTransfersPage)sendingTab.View;
                sendingPage.OnTabViewSelected(sendingTab == tabSheetTabs.SelectedTab);
            }

            if(receivingTab != null && receivingTab.View != null)
            {
                ViewPages.StoreTransfersPage receiving = (ViewPages.StoreTransfersPage)receivingTab.View;
                receiving.OnTabViewSelected(receivingTab == tabSheetTabs.SelectedTab);
            }

            if(allOrdersTab != null && allOrdersTab.View != null)
            {
                ViewPages.StoreTransfersPage allOrdersPage = (ViewPages.StoreTransfersPage)allOrdersTab.View;
                allOrdersPage.OnTabViewSelected(allOrdersTab == tabSheetTabs.SelectedTab);
            }

        }

        private void SendCreateTransferMessage(object sender, ContextBarClickEventArguments args)
        {
            ViewPages.StoreTransfersPage currentPage = (ViewPages.StoreTransfersPage)tabSheetTabs.SelectedTab.View;
            currentPage.ContextMessage("CreateTransfer");
        }

        private void SendSendTransferMessage(object sender, ContextBarClickEventArguments args)
        {            
            ViewPages.StoreTransfersPage currentPage = (ViewPages.StoreTransfersPage)tabSheetTabs.SelectedTab.View;
            currentPage.ContextMessage("SendTransfer");
        }
    }
}
