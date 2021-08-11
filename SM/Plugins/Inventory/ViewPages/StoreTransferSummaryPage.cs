using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StoreTransferSummaryPage : Controls.ContainerControl, ITabViewV2
    {
        private StoreTransferTypeEnum storeTransferType;
        private InventoryTransferType inventoryTransferType;
        private InventoryTransferOrder transferOrder;
        private InventoryTransferRequest transferRequest;

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreTransferSummaryPage();
        }

        public StoreTransferSummaryPage()
        {
            InitializeComponent();
        }

        public bool DataIsModified()
        {
            if (storeTransferType == StoreTransferTypeEnum.Order)
            {
                return transferOrder.Text != tbDescription.Text || transferOrder.ExpectedDelivery.Date != dtDeliveryDate.Value.Date;
            }
            else
            {
                return transferRequest.Text != tbDescription.Text || transferRequest.ExpectedDelivery.Date != dtDeliveryDate.Value.Date;
            }
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
            (object internalObject, StoreTransferTypeEnum StoreTransferType, InventoryTransferType InventoryTransferType) info = (ValueTuple<object, StoreTransferTypeEnum, InventoryTransferType>)internalContext;

            storeTransferType = info.StoreTransferType;
            inventoryTransferType = info.InventoryTransferType;

            if (storeTransferType == StoreTransferTypeEnum.Order)
            {
                transferOrder = (InventoryTransferOrder)info.internalObject;
                tbDescription.Enabled = 
                dtDeliveryDate.Enabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders);

                tbID.Text = transferOrder.ID.StringValue;
                tbDescription.Text = transferOrder.Text;
                dtDeliveryDate.Value = transferOrder.ExpectedDelivery;
                SetTransferOrderStatus();
            }
            else
            {
                transferRequest = (InventoryTransferRequest)info.internalObject;
                inventoryTransferType = info.InventoryTransferType;
                tbDescription.Enabled =
                dtDeliveryDate.Enabled = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests);

                tbID.Text = transferRequest.ID.StringValue;
                tbDescription.Text = transferRequest.Text;
                dtDeliveryDate.Value = transferRequest.ExpectedDelivery;

                lblDeliveryDate.Text = Properties.Resources.DueDate + ":";
                SetTransferRequestStatus();
            }
        }

        public void OnClose()
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (param != null &&
                ((storeTransferType == StoreTransferTypeEnum.Order && objectName == "InventoryTransferOrder" && changeIdentifier == transferOrder.ID)
                || (storeTransferType == StoreTransferTypeEnum.Request && objectName == "InventoryTransferRequest" && changeIdentifier == transferRequest.ID)))
            {
                if (storeTransferType == StoreTransferTypeEnum.Order)
                {
                    transferOrder = (InventoryTransferOrder)param;
                    SetTransferOrderStatus();
                }
                else
                {
                    transferRequest = (InventoryTransferRequest)param;
                    SetTransferRequestStatus();
                }
            }
        }

        public bool SaveData()
        {
            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                if (storeTransferType == StoreTransferTypeEnum.Order)
                {
                    transferOrder.Text = tbDescription.Text;
                    transferOrder.ExpectedDelivery = dtDeliveryDate.Value;

                    service.SaveInventoryTransferOrder(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferOrder, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferOrder", transferOrder.ID, transferOrder);
                }
                else
                {
                    transferRequest.Text = tbDescription.Text;
                    transferRequest.ExpectedDelivery = dtDeliveryDate.Value;

                    service.SaveInventoryTransferRequest(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), transferRequest, true);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "InventoryTransferRequest", transferRequest.ID, transferRequest);
                }
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer, MessageBoxIcon.Error);
            }

            return true;
        }

        public void SaveUserInterface()
        {

        }

        private void SetTransferOrderStatus()
        {
            string status = string.Empty;
            Color textColor = Color.Black;

            switch (inventoryTransferType)
            {
                case InventoryTransferType.Outgoing:
                    if (transferOrder.Rejected)
                    {
                        status = Properties.Resources.Rejected;
                    }
                    else if (transferOrder.FetchedByReceivingStore)
                    {
                        status = Properties.Resources.FetchedByReceivingStore;
                    }
                    else if (transferOrder.Sent)
                    {
                        status = Properties.Resources.Sent;
                    }
                    else
                    {
                        status = Properties.Resources.New;
                    }
                    break;
                case InventoryTransferType.Incoming:
                    if (DateTime.Now.Date > transferOrder.ExpectedDelivery.Date && !transferOrder.FetchedByReceivingStore)
                    {
                        textColor = Color.Red;
                        status = Properties.Resources.DeliveryDateExceeded;
                    }
                    else if (!transferOrder.FetchedByReceivingStore)
                    {
                        status = Properties.Resources.ToBeFetchedByReceivingStore;
                    }
                    else if(!transferOrder.Received)
                    {
                        status = Properties.Resources.FetchedByReceivingStore;
                    }
                    else
                    {
                        status = Properties.Resources.Received;
                    }
                    break;
                case InventoryTransferType.SendingAndReceiving:
                    break;
                case InventoryTransferType.Finished:
                    if (transferOrder.Received)
                    {
                        status = Properties.Resources.Received;
                    }
                    else if (transferOrder.Rejected)
                    {
                        status = Properties.Resources.Rejected;
                    }
                    break;
                default:
                    break;
            }

            tbStatus.ForeColor = textColor;
            tbStatus.Text = status;
        }

        private void SetTransferRequestStatus()
        {
            string status = string.Empty;
            Color textColor = Color.Black;

            switch (inventoryTransferType)
            {
                case InventoryTransferType.Outgoing:
                    if (transferRequest.Rejected)
                    {
                        status = Properties.Resources.Rejected;
                    }
                    else if (transferRequest.FetchedByReceivingStore)
                    {
                        status = Properties.Resources.FetchedByReceivingStore;
                    }
                    else if (transferRequest.Sent)
                    {
                        status = Properties.Resources.Sent;
                    }
                    else
                    {
                        status = Properties.Resources.New;
                    }
                    break;
                case InventoryTransferType.Incoming:
                    if (DateTime.Now.Date > transferRequest.ExpectedDelivery.Date && !transferRequest.FetchedByReceivingStore)
                    {
                        textColor = Color.Red;
                        status = Properties.Resources.DeliveryDateExceeded;
                    }
                    else if (!transferRequest.FetchedByReceivingStore)
                    {
                        status = Properties.Resources.ToBeFetchedByReceivingStore;
                    }
                    else
                    {
                        status = Properties.Resources.Received;
                    }
                    break;
                case InventoryTransferType.SendingAndReceiving:
                    break;
                case InventoryTransferType.Finished:
                    if (transferRequest.InventoryTransferOrderCreated)
                    {
                        status = Properties.Resources.TransferOrderCreated;
                    }
                    else if (transferRequest.Rejected)
                    {
                        status = Properties.Resources.Rejected;
                    }
                    break;
                default:
                    break;
            }

            tbStatus.ForeColor = textColor;
            tbStatus.Text = status;
        }
    }
}
