using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.ViewPlugins.Inventory.ViewPages;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class StoreTransferView : ViewBase
    {
        private StoreTransferTypeEnum storeTransferType;
        private InventoryTransferType inventoryTransferType;
        private InventoryTransferOrder transferOrder;
        private InventoryTransferRequest transferRequest;

        private TabControl.Tab itemsTab;
        private TabControl.Tab summaryTab;

        private ViewPages.StoreTransferItemsPage itemsPage;

        public StoreTransferView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Save;

        }

        public StoreTransferView(InventoryTransferOrder transferOrder, InventoryTransferType inventoryTransferType)
            : this()
        {
            this.transferOrder = transferOrder;
            storeTransferType = StoreTransferTypeEnum.Order;
            this.inventoryTransferType = inventoryTransferType;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders);

            HeaderText = Resources.TransferOrder + ": " + (string.IsNullOrEmpty(transferOrder.Text) ? transferOrder.ID : transferOrder.Text);
        }

        public StoreTransferView(InventoryTransferRequest transferRequest, InventoryTransferType inventoryTransferType)
            : this()
        {
            this.transferRequest = transferRequest;
            storeTransferType = StoreTransferTypeEnum.Request;
            this.inventoryTransferType = inventoryTransferType;
            
            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests);

            HeaderText = Resources.TransferRequest + ": " + (string.IsNullOrEmpty(transferRequest.Text) ? transferRequest.ID : transferRequest.Text);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        protected override bool DataIsModified()
        {
            return tabSheetTabs.IsModified();
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();
            return true;
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {

            if (arguments.CategoryKey == GetType() + ".Related")
            {
                if(storeTransferType == StoreTransferTypeEnum.Order)
                {
                    arguments.Add(new ContextBarItem(Resources.TransferRequests, "TransferRequests", PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferRequests) , PluginOperations.ShowStoreTransfersRequestsView), 100);
                }
                else
                {
                    arguments.Add(new ContextBarItem(Resources.TransferOrders, "TransferOrders", PluginEntry.DataModel.HasPermission(Permission.ViewInventoryTransferOrders), PluginOperations.ShowStoreTransfersOrdersView), 100);
                }

                arguments.Add(new ContextBarItem(Resources.InventoryInTransit, null, PluginOperations.ShowInventoryInTransitView), 200);

                if (PluginEntry.Framework.CanRunOperation("ShowImageBank"))
                {
                    arguments.Add(new ContextBarItem(Resources.ImageBank, PluginOperations.ShowImageBankHandler), 300);
                }
            }

            if (arguments.CategoryKey == GetType() + ".Actions")
            {
                switch (storeTransferType)
                {
                    case StoreTransferTypeEnum.Order:
                        switch (inventoryTransferType)
                        {
                            case InventoryTransferType.Outgoing:
                                arguments.Add(new ContextBarItem(Resources.SendInventoryTransfer, "SendTransferOrder", CanSend() && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders), SendTransferOrder), 100);
                                break;
                            case InventoryTransferType.Incoming:
                                arguments.Add(new ContextBarItem(Resources.ReceiveTransferOrder, "ReceiveTransferOrder", CanSend() && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders), ReceiveTransferOrder), 100);
                                arguments.Add(new ContextBarItem(Resources.AutoSetQuantity, "AutoSetQuantity", CanSend() && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders) && PluginEntry.DataModel.HasPermission(Permission.AutoSetQuantityOnTransferOrder), AutoSetQuantity), 110);
                                Tag = inventoryTransferType;
                                break;
                        }
                        break;
                    case StoreTransferTypeEnum.Request:
                        switch (inventoryTransferType)
                        {
                            case InventoryTransferType.Outgoing:
                                arguments.Add(new ContextBarItem(Resources.SendTransferRequest, "SendTransferRequest", CanSend() && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests), SendTransferRequest), 100);
                                break;
                            case InventoryTransferType.Incoming:
                                arguments.Add(new ContextBarItem(Resources.CreateTransferOrder, "CreateTransferOrder", CanSend() && PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests), CreateTransferOrder), 100);
                                break;
                        }
                        break;
                }
            }
        }

        private bool CanSend()
        {
            if(itemsPage == null)
            {
                return false;
            }

            switch (storeTransferType)
            {
                case StoreTransferTypeEnum.Order:
                    switch (inventoryTransferType)
                    {
                        case InventoryTransferType.Outgoing:
                            return !transferOrder.Sent && !transferOrder.Rejected && itemsPage.HasRows();
                        case InventoryTransferType.Incoming:
                            return !transferOrder.Received && !transferOrder.Rejected && itemsPage.HasRows();
                    }
                    break;
                case StoreTransferTypeEnum.Request:
                    switch (inventoryTransferType)
                    {
                        case InventoryTransferType.Outgoing:
                            return !transferRequest.Rejected && itemsPage.HasRows() && (!transferRequest.Sent || (!transferRequest.FetchedByReceivingStore && itemsPage.HasUnsentRows()));
                        case InventoryTransferType.Incoming:
                            return !transferRequest.InventoryTransferOrderCreated && !transferRequest.Rejected && itemsPage.HasRows();
                    }
                    break;
            }

            return false;
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Resources.Action, GetType() + ".Actions"), 200);
            base.OnSetupContextBarHeaders(arguments);
        }

        private void SendTransferOrder(object sender, EventArgs e)
        {
            PluginOperations.SendTransferOrder(transferOrder);
        }

        private void SendTransferRequest(object sender, EventArgs e)
        {
            PluginOperations.SendTransferRequest(transferRequest);
        }

        private void CreateTransferOrder(object sender, EventArgs e)
        {
            if (PluginOperations.CreateTransferOrderFromRequest(transferRequest) == CreateTransferOrderResult.Success)
            {
                transferRequest = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTransferRequest(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferRequest.ID, true);
                PluginOperations.ShowTransferOrderView(transferRequest.InventoryTransferOrderId);
            }            
        }

        private void ReceiveTransferOrder(object sender, EventArgs e)
        {
            PluginOperations.ReceiveTransferOrder(transferOrder);
        }

        private void AutoSetQuantity(object sender, EventArgs e)
        {
            PluginOperations.AutoSetQuantityOnTransferOrder(transferOrder.ID);
        }

        protected override string LogicalContextName
        {
            get
            {
                switch (storeTransferType)
                {
                    case StoreTransferTypeEnum.Order:
                        return Resources.TransferOrders;
                    case StoreTransferTypeEnum.Request:
                        return Resources.TransferRequests;
                    default:
                        return "";
                }
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                switch (storeTransferType)
                {
                    case StoreTransferTypeEnum.Order:
                        return transferOrder.ID;
                    case StoreTransferTypeEnum.Request:
                        return transferRequest.ID;
                    default:
                        return RecordIdentifier.Empty;
                }
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if(!isRevert)
            {
                itemsTab = new TabControl.Tab(Resources.Items, ViewPages.StoreTransferItemsPage.CreateInstance);
                summaryTab = new TabControl.Tab(Resources.Summary, ViewPages.StoreTransferSummaryPage.CreateInstance);

                tabSheetTabs.AddTab(itemsTab);
                tabSheetTabs.AddTab(summaryTab);
                
                itemsPage = (ViewPages.StoreTransferItemsPage)itemsTab.View;
                itemsPage.OnRefreshContextBar += RefreshContextBar;
            }

            tabSheetTabs.SetData(isRevert, null, storeTransferType == StoreTransferTypeEnum.Order ? ((object)transferOrder, storeTransferType, inventoryTransferType) : ((object)transferRequest, storeTransferType, inventoryTransferType));
        }

        private void RefreshContextBar()
        {
            PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (((storeTransferType == StoreTransferTypeEnum.Order && objectName == "InventoryTransferOrder" && changeIdentifier == transferOrder.ID)
                || (storeTransferType == StoreTransferTypeEnum.Request && objectName == "InventoryTransferRequest" && changeIdentifier == transferRequest.ID)) && param != null)
            {
                if(storeTransferType == StoreTransferTypeEnum.Order)
                {
                    transferOrder = (InventoryTransferOrder)param;
                }
                else
                {
                    transferRequest = (InventoryTransferRequest)param;
                }
            }

            tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
        }

        public override List<IDataEntity> GetListSelection()
        {
            return (itemsTab.View as StoreTransferItemsPage).GetSelectedItems();
        }

        protected override void OnClose()
        {
            if(itemsPage != null)
            {
                itemsPage.OnRefreshContextBar -= RefreshContextBar;
            }

            tabSheetTabs.SendCloseMessage();
            base.OnClose();
        }
    }
}
