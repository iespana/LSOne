using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Replenishment;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewStoreTransfer : UserControl, IWizardPage
    {
        private WizardBase parent;
        private InventoryTemplateFilterContainer filter;
        private RecordIdentifier existingStoreTransferID;
        private TemplateListItem template;
        private readonly StoreTransfersPermissionManager storeTransfersPermissionManager;
        private readonly Dictionary<string, string> sendingAndReceivingStoreMapping;

        /// <summary>
        /// New empty store transfer
        /// </summary>
        /// <param name="parent">Parent wizard base</param>
        /// <param name="transferType">Type of transfer</param>
        public NewStoreTransfer(WizardBase parent, StoreTransferTypeEnum transferType)
        {
            InitializeComponent();
            this.parent = parent;

            storeTransfersPermissionManager = new StoreTransfersPermissionManager(PluginEntry.DataModel, transferType, InventoryTransferType.Outgoing);

            sendingAndReceivingStoreMapping = new Dictionary<string, string>
            {
                [lblSendingStore.Text] = "SendingStore",
                [lblReceivingStore.Text] = "ReceivingStore"
            };

            if (transferType == StoreTransferTypeEnum.Request)
            {
                lblSendingStore.Text = Properties.Resources.RequestingStoreLabel;
                lblReceivingStore.Text = Properties.Resources.FromStoreLabel;

                sendingAndReceivingStoreMapping[lblSendingStore.Text] = "ReceivingStore";
                sendingAndReceivingStoreMapping[lblReceivingStore.Text] = "SendingStore";
            }
        }

        /// <summary>
        /// New store transfer created from filter
        /// </summary>
        /// <param name="parent">Parent wizard base</param>
        /// <param name="filter">Container with IDs to filter</param>
        /// <param name="transferType">Type of transfer</param>
        public NewStoreTransfer(WizardBase parent, InventoryTemplateFilterContainer filter, StoreTransferTypeEnum transferType) : this(parent, transferType)
        {
            this.filter = filter;
        }

        /// <summary>
        /// New store transfer created from template
        /// </summary>
        /// <param name="parent">Parent wizard base</param>
        /// <param name="template">Template item</param>
        /// <param name="transferType">Type of transfer</param>
        public NewStoreTransfer(WizardBase parent, TemplateListItem template, StoreTransferTypeEnum transferType) : this(parent, transferType)
        {
            this.template = template;
        }

        /// <summary>
        /// New store transfer from existing request
        /// </summary>
        /// <param name="parent">Parent wizard base</param>
        /// <param name="transferRequest">Existing transfer request</param>
        /// <param name="transferType">Type of transfer</param>
        public NewStoreTransfer(WizardBase parent, InventoryTransferRequest transferRequest, StoreTransferTypeEnum transferType) : this(parent, transferType)
        {
            tbDescription.Text = transferRequest.Text;
            // ONE-9398 - when creating orders from transfer request, switch the sending and the receiving stores
            if (transferType == StoreTransferTypeEnum.Order)
            {
                cmbSendingStore.SelectedData = new DataEntity(transferRequest.ReceivingStoreId, transferRequest.ReceivingStoreName);
                cmbReceivingStore.SelectedData = new DataEntity(transferRequest.SendingStoreId, transferRequest.SendingStoreName);
            }
            else
            {
                cmbSendingStore.SelectedData = new DataEntity(transferRequest.SendingStoreId, transferRequest.SendingStoreName);
                cmbReceivingStore.SelectedData = new DataEntity(transferRequest.ReceivingStoreId, transferRequest.ReceivingStoreName);
            }
            existingStoreTransferID = transferRequest.ID;
            SetDefaultDeliveryTime(cmbSendingStore.SelectedDataID);
        }

        /// <summary>
        /// New store transfer from existing order
        /// </summary>
        /// <param name="parent">Parent wizard base</param>
        /// <param name="transferOrder">Existing transfer order</param>
        /// <param name="transferType">Type of transfer</param>
        public NewStoreTransfer(WizardBase parent, InventoryTransferOrder transferOrder, StoreTransferTypeEnum transferType) : this(parent, transferType)
        {
            tbDescription.Text = transferOrder.Text;
            // ONE-9398 - when creating transfer request from orders, switch the sending and the receiving stores
            if (transferType == StoreTransferTypeEnum.Request)
            {
                cmbSendingStore.SelectedData = new DataEntity(transferOrder.ReceivingStoreId, transferOrder.ReceivingStoreName);
                cmbReceivingStore.SelectedData = new DataEntity(transferOrder.SendingStoreId, transferOrder.SendingStoreName);
            }
            else
            {
                cmbSendingStore.SelectedData = new DataEntity(transferOrder.SendingStoreId, transferOrder.SendingStoreName);
                cmbReceivingStore.SelectedData = new DataEntity(transferOrder.ReceivingStoreId, transferOrder.ReceivingStoreName);
            }
            existingStoreTransferID = transferOrder.ID;
            SetDefaultDeliveryTime(cmbSendingStore.SelectedDataID);
        }

        public RecordIdentifier SendingStoreID { get { return cmbSendingStore.SelectedDataID; } }
        public RecordIdentifier ReceivingStoreID { get { return cmbReceivingStore.SelectedDataID; } }
        public string Description { get { return tbDescription.Text; } }
        public DateTime ExpectedDelivery { get { return dtExpectedDelivery.Value; } }
        public InventoryTemplateFilterContainer Filter { get { return filter; } }
        public RecordIdentifier ExistingStoreTransferID { get { return existingStoreTransferID; } }
        public TemplateListItem Template { get { return template; } }

        #region IWizardPage Members
        public bool HasFinish
        {
            get { return true; }
        }

        public bool HasForward
        {
            get { return false; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            CheckEnabled();
            tbDescription.Focus();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return null;
        }

        public void ResetControls()
        {

        }

        #endregion

        private void cmbSendingStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> data = Providers.StoreData.GetList(PluginEntry.DataModel, cmbReceivingStore.SelectedDataID ?? "");
            cmbSendingStore.SetData(data, null);
        }

        private void cmbReceivingStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> data = Providers.StoreData.GetList(PluginEntry.DataModel, cmbSendingStore.SelectedDataID ?? "");
            cmbReceivingStore.SetData(data, null);
        }

        private void cmbReceivingStore_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbSendingStore_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
            SetDefaultDeliveryTime(cmbSendingStore.SelectedDataID);
        }

        private void CheckEnabled()
        {
            parent.NextEnabled = !RecordIdentifier.IsEmptyOrNull(cmbReceivingStore.SelectedDataID)
                              && !RecordIdentifier.IsEmptyOrNull(cmbSendingStore.SelectedDataID)
                              && dtExpectedDelivery.Value.Date >= DateTime.Now.Date;
        }

        private void NewStoreTransfer_Load(object sender, EventArgs e)
        {
            if(!PluginEntry.DataModel.IsHeadOffice)
            {
                DataEntity currentStore = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                cmbSendingStore.SelectedData = currentStore;
                cmbSendingStore_SelectedDataChanged(sender, e);

                cmbSendingStore.Enabled = storeTransfersPermissionManager.HasAccessToAllStores(sendingAndReceivingStoreMapping[lblSendingStore.Text]);
                cmbReceivingStore.Enabled = storeTransfersPermissionManager.HasAccessToAllStores(sendingAndReceivingStoreMapping[lblReceivingStore.Text]);
            }

            if(Template != null)
            {
                InventoryTemplate inventoryTemplate = Providers.InventoryTemplateData.Get(PluginEntry.DataModel, Template.TemplateID);

                if(PluginEntry.DataModel.IsHeadOffice)
                {
                    DataEntity tempalteStore = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, Template.StoreID);
                    cmbSendingStore.SelectedData = tempalteStore;
                    cmbSendingStore_SelectedDataChanged(sender, e);
                }

                if(!RecordIdentifier.IsEmptyOrNull(inventoryTemplate.DefaultStore) && inventoryTemplate.DefaultStore != PluginEntry.DataModel.CurrentStoreID)
                {
                    DataEntity defaultReceivingStore = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, inventoryTemplate.DefaultStore);

                    if(defaultReceivingStore != null)
                    {
                        cmbReceivingStore.SelectedData = defaultReceivingStore;
                        cmbReceivingStore_SelectedDataChanged(sender, e);
                    }
                }
            }
        }

        private void SetDefaultDeliveryTime(RecordIdentifier storeId)
        {
            if(!RecordIdentifier.IsEmptyOrNull(storeId))
            {
                Store store = Providers.StoreData.Get(PluginEntry.DataModel, storeId, CacheType.CacheTypeNone, UsageIntentEnum.Minimal);
                dtExpectedDelivery.Value = store.StoreTransferExpectedDeliveryDate();
            }
        }

        private void dtExpectedDelivery_ValueChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}