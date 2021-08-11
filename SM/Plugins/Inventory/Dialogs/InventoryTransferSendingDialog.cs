using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class InventoryTransferSendingDialog : DialogBase
    {
        private InventoryTransferOrder transferOrder;
        private DataEntity creatingStoreDataEntity;

        public InventoryTransferSendingDialog()
        {
            InitializeComponent();
            cmbReceivingStore.SelectedData = new DataEntity("", "");
            cmbSendingStore.SelectedData = new DataEntity("", "");
            cmbRequest.SelectedData = new DataEntity("", "");
            cmbTransferOrder.SelectedData = new DataEntity("", "");
        }

        public InventoryTransferSendingDialog(DataEntity creatingStoreDataEntity)
            :this()
        {
            this.creatingStoreDataEntity = creatingStoreDataEntity;
        }

        public InventoryTransferSendingDialog(InventoryTransferOrder transferOrder)
            : this()
        {
            this.transferOrder = transferOrder;

            rbFromRequst.Enabled = false;
            rbCreateEmptyTransfer.Checked = true;
            rbCopyExisting.Enabled = false;
            cmbReceivingStore.SelectedData = new DataEntity(transferOrder.ReceivingStoreId, transferOrder.ReceivingStoreName);
            cmbSendingStore.SelectedData = new DataEntity(transferOrder.SendingStoreId, transferOrder.SendingStoreName);
        }

        public InventoryTransferOrder InventoryTransferOrder
        {
            get { return transferOrder; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (transferOrder == null)
            {
                transferOrder = new InventoryTransferOrder();
            }

            if (rbFromRequst.Checked)
            {
                RecordIdentifier newInventoryTransferOrderId;
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                    newInventoryTransferOrderId = inventoryService.CreateInventoryTransferRequest(PluginEntry.DataModel, cmbRequest.SelectedData.ID, Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile));
                }
                catch (DataException)
                {
                    MessageDialog.Show(Properties.Resources.CannotCreateInventoryTransferRequestDescription, Properties.Resources.CannoCreateInventoryTransferRequest, MessageBoxIcon.Exclamation);
                    return;
                }
                
                transferOrder.ID = newInventoryTransferOrderId;
            }
            else if (rbCreateEmptyTransfer.Checked)
            {
                transferOrder.SendingStoreId = cmbSendingStore.SelectedData.ID;
                transferOrder.ReceivingStoreId = cmbReceivingStore.SelectedData.ID;
                transferOrder.CreationDate = DateTime.Now;
                transferOrder.CreatedBy = (!PluginEntry.DataModel.IsHeadOffice) ? PluginEntry.DataModel.CurrentStoreID : "";
                
                Providers.InventoryTransferOrderData.Save(PluginEntry.DataModel, transferOrder);
            }
            else if (rbCopyExisting.Checked)
            {
                RecordIdentifier newInventoryTransferOrderId;
                IInventoryService inventoryService = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                try
                {
                    newInventoryTransferOrderId = inventoryService.CreateInventoryTransferCopy(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), cmbTransferOrder.SelectedData.ID, true);
                }
                catch (DataException)
                {
                    MessageDialog.Show(Properties.Resources.CannotCreateInventoryTransferRequestDescription, Properties.Resources.CannoCreateInventoryTransferRequest, MessageBoxIcon.Exclamation);
                    return;
                }

                transferOrder.ID = newInventoryTransferOrderId;
            }

            DialogResult = DialogResult.OK;
            Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
         
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (transferOrder == null)
            {
                if (rbFromRequst.Checked)
                {
                    btnOK.Enabled = cmbRequest.SelectedData.ID != "";
                }
                else if (rbCreateEmptyTransfer.Checked)
                {
                    btnOK.Enabled = cmbReceivingStore.SelectedData.ID != "" &&
                                    cmbSendingStore.SelectedData.ID != "";
                }
                else if (rbCopyExisting.Checked)
                {
                    btnOK.Enabled = cmbTransferOrder.SelectedData.ID != "";
                }
            }
            else
            {
                if (rbFromRequst.Checked)
                {
                    btnOK.Enabled = cmbRequest.SelectedData.ID != transferOrder.InventoryTransferRequestId;
                }
                else if (rbCreateEmptyTransfer.Checked)
                {
                    btnOK.Enabled = cmbReceivingStore.SelectedData.ID != transferOrder.ReceivingStoreId ||
                                    cmbSendingStore.SelectedData.ID != transferOrder.SendingStoreId;
                }
                else if (rbCopyExisting.Checked)
                {
                    btnOK.Enabled = cmbTransferOrder.SelectedData.ID != "";
                }
            }
        }

        private void cmbRequest_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier storeId = RecordIdentifier.Empty;

            if (!PluginEntry.DataModel.IsHeadOffice && creatingStoreDataEntity != null)
            {
                storeId = creatingStoreDataEntity.ID;
            }
            
            List<InventoryTransferRequest> listOfRequestsForStore = Providers.InventoryTransferRequestData.GetListForStore(
                PluginEntry.DataModel,
                new List<RecordIdentifier> {storeId},
                InventoryTransferType.ToReceive,
                InventoryTransferOrderSortEnum.Id,
                false);


            List<InventoryTransferRequest> requestsWithNoOrderCreated = listOfRequestsForStore.Where(request => !request.InventoryTransferOrderCreated).ToList();

            // Create list of Data entities that we can show in the combo box
            List<DataEntity> dataList = new List<DataEntity>();
            foreach (InventoryTransferRequest request in requestsWithNoOrderCreated)
            {
                DataEntity dataEntity = new DataEntity();
                dataEntity.ID = request.ID;
                dataEntity.Text += lblReceivingStore.Text + request.SendingStoreName;
                dataList.Add(dataEntity);
            }
            cmbRequest.SetData(dataList,null);
        }

        private void rbFromRequst_CheckedChanged(object sender, EventArgs e)
        {
            lblRequest.Enabled = rbFromRequst.Checked;
            cmbRequest.Enabled = rbFromRequst.Checked;
            CheckEnabled(sender, e);
        }

        private void rbCreateEmptyTransfer_CheckedChanged(object sender, EventArgs e)
        {
            cmbReceivingStore.Enabled = rbCreateEmptyTransfer.Checked;
            lblReceivingStore.Enabled = cmbReceivingStore.Enabled;
            lblSendingStore.Enabled = cmbReceivingStore.Enabled;
            cmbSendingStore.Enabled = rbCreateEmptyTransfer.Checked && PluginEntry.DataModel.IsHeadOffice;
            //lblSendingStore.Enabled = cmbSendingStore.Enabled;

            if (rbCreateEmptyTransfer.Checked)
            {
                if (creatingStoreDataEntity != null)
                {
                    cmbSendingStore.SelectedData = creatingStoreDataEntity;
                }
            }
            else
            {
                cmbSendingStore.SelectedData = new DataEntity("","");
                cmbReceivingStore.SelectedData = new DataEntity("","");
            }

            CheckEnabled(sender, e);
        }

        private void rbCopyExisting_CheckedChanged(object sender, EventArgs e)
        {
            lblTransferOrder.Enabled = rbCopyExisting.Checked;
            cmbTransferOrder.Enabled = rbCopyExisting.Checked;
            CheckEnabled(sender, e);
        }

        private void cmbSendingStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> data = Providers.StoreData.GetList(PluginEntry.DataModel, cmbReceivingStore.SelectedData.ID);
            cmbSendingStore.SetData(data, null);
        }

        private void cmbReceivingStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> data = Providers.StoreData.GetList(PluginEntry.DataModel, cmbSendingStore.SelectedData.ID);
            cmbReceivingStore.SetData(data, null);
        }

        private void cmbTransferOrder_RequestData(object sender, EventArgs e)
        {
            RecordIdentifier storeId = RecordIdentifier.Empty;

            if (!PluginEntry.DataModel.IsHeadOffice && creatingStoreDataEntity != null)
            {
                storeId = creatingStoreDataEntity.ID;
            }
            
            List<InventoryTransferOrder> listOfOrdersFinished = Providers.InventoryTransferOrderData.GetListForStore(
                PluginEntry.DataModel,
                new List<RecordIdentifier> {storeId},
                InventoryTransferType.Finished,
                InventoryTransferOrderSortEnum.CreatedDate,
                false);
            
            List<InventoryTransferOrder> listOfOrdersSending = Providers.InventoryTransferOrderData.GetListForStore(
                PluginEntry.DataModel,
                new List<RecordIdentifier> { storeId },
                InventoryTransferType.Sending,
                InventoryTransferOrderSortEnum.CreatedDate,
                false);

            IEnumerable<InventoryTransferOrder> listOfOrders = listOfOrdersFinished.Concat(listOfOrdersSending);
            listOfOrders = listOfOrders.OrderByDescending(p => p.CreationDate);

            // Create list of Data entities that we can show in the combo box
            List<DataEntity> dataList = new List<DataEntity>();
            foreach (InventoryTransferOrder request in listOfOrders)
            {
                DataEntity dataEntity = new DataEntity();
                dataEntity.ID = request.ID;
                dataEntity.Text += request.CreationDate.ToShortDateString() + "       " + request.ReceivingStoreName;
                dataList.Add(dataEntity);
            }
            cmbTransferOrder.SetData(dataList, null);
        }

        private void cmbTransferOrder_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, e);
        }

        private void cmbTransferOrder_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, e);
        }

    }
}
