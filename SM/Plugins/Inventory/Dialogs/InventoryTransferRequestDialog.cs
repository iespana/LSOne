using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class InventoryTransferRequestDialog : DialogBase
    {
        private InventoryTransferRequest transferRequest;

        public InventoryTransferRequestDialog()
        {
            InitializeComponent();

            cmSendingStore.SelectedData = new DataEntity("", "");
            cmbReceivingStore.SelectedData = new DataEntity("", "");

            cmSendingStore.Enabled = PluginEntry.DataModel.IsHeadOffice;

        }

        public InventoryTransferRequestDialog(DataEntity creatingStoreDE)
            :this()
        {
            cmSendingStore.SelectedData = creatingStoreDE;
        }

        public InventoryTransferRequestDialog(RecordIdentifier transferRequestId)
            :this()
        {
            transferRequest = Providers.InventoryTransferRequestData.Get(PluginEntry.DataModel, transferRequestId);
            cmSendingStore.SelectedData = new DataEntity(transferRequest.SendingStoreId, transferRequest.SendingStoreName);
            cmbReceivingStore.SelectedData = new DataEntity(transferRequest.ReceivingStoreId, transferRequest.ReceivingStoreName);
        }

        public InventoryTransferRequest TransferRequest
        {
            get { return transferRequest; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (transferRequest == null)
            {
                transferRequest = new InventoryTransferRequest();
                transferRequest.CreationDate = DateTime.Now;
            }

            transferRequest.SendingStoreId = cmSendingStore.SelectedData.ID;
            transferRequest.ReceivingStoreId = cmbReceivingStore.SelectedData.ID;
            transferRequest.CreatedBy = (!PluginEntry.DataModel.IsHeadOffice) ? PluginEntry.DataModel.CurrentStoreID : "";
            
            Providers.InventoryTransferRequestData.Save(PluginEntry.DataModel, transferRequest);

            transferRequest.SendingStoreName = cmSendingStore.SelectedData.Text;
            transferRequest.ReceivingStoreName = cmbReceivingStore.SelectedData.Text;

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
            if (transferRequest == null)
            {
                btnOK.Enabled = cmbReceivingStore.SelectedData.ID != "" &&
                                cmSendingStore.SelectedData.ID != "";
            }
            else
            {
                btnOK.Enabled = cmbReceivingStore.SelectedData.ID != transferRequest.SendingStoreId ||
                                cmSendingStore.SelectedData.ID != transferRequest.ReceivingStoreId;
            }
        }

        private void cmbRequstingStore_RequestData(object sender, EventArgs e)
        {
            cmSendingStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel, cmbReceivingStore.SelectedData.ID), null);
        }

        private void cmbRequestedStore_RequestData(object sender, EventArgs e)
        {
            cmbReceivingStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel, cmSendingStore.SelectedData.ID), null);
        }
    }
}
