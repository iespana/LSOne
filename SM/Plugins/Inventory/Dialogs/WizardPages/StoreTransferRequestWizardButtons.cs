using System;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class StoreTransferRequestWizardButtons : UserControl, IWizardPage
    {
        WizardBase parent;
        private StoreTransferActionWrapper storeTransferAction;

        public event EventHandler RequestFinish;

        public StoreTransferRequestWizardButtons(WizardBase parent, StoreTransferActionWrapper storeTransferAction)
        {
            InitializeComponent();

            this.parent = parent;
            this.storeTransferAction = storeTransferAction;

            var canCreateTransfers = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransferRequests);
            btnNewTransferRequest.Enabled = canCreateTransfers;
            btnGenerateFromRequest.Enabled = canCreateTransfers;
            btnGenerateFromOrder.Enabled = canCreateTransfers;
            btnGenerateFromFilter.Enabled = canCreateTransfers;
            btnViewStoreTransferTemplates.Enabled = canCreateTransfers;
        }

        #region IWizardPage Members

        public bool HasFinish
        {
            get { return false; }
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

        private void btnManageTransferRequest_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.Manage;
            RequestFinish?.Invoke(this, EventArgs.Empty);
        }

        private void btnNewTransferRequest_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.CreateNew;
            parent.AddPage(new NewStoreTransfer(parent, StoreTransferTypeEnum.Request));
        }

        private void btnGenerateFromRequest_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromRequest;
            parent.AddPage(new ExistingStoreTransferSearch(parent, storeTransferAction.Action, StoreTransferTypeEnum.Request));
        }

        private void btnGenerateFromOrder_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromOrder;
            parent.AddPage(new ExistingStoreTransferSearch(parent, storeTransferAction.Action, StoreTransferTypeEnum.Request));
        }

        private void btnGenerateFromFilter_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromFilter;
            parent.AddPage(new CreateStoreTransferFromFilter(parent, StoreTransferTypeEnum.Request));
        }

        private void btnViewStoreTransferTemplates_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromTemplate;
            parent.AddPage(new NewInventoryDocumentFromTemplate(parent, new InventoryTypeAction { Action = InventoryActionEnum.GenerateFromTemplate, InventoryType = InventoryEnum.StoreTransfer, StoreTransferType = StoreTransferTypeEnum.Request }));
        }
    }
}
