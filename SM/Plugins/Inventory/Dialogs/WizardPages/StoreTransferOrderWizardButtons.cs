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
    public partial class StoreTransferOrderWizardButtons : UserControl, IWizardPage
    {
        WizardBase parent;
        private StoreTransferActionWrapper storeTransferAction;

        public event EventHandler RequestFinish;

        public StoreTransferOrderWizardButtons(WizardBase parent, StoreTransferActionWrapper storeTransferAction)
        {
            InitializeComponent();

            this.parent = parent;
            this.storeTransferAction = storeTransferAction;

            var canCreateOrders = PluginEntry.DataModel.HasPermission(Permission.EditInventoryTransfersOrders);
            btnNewTransferOrder.Enabled = canCreateOrders;
            btnGenerateFromRequest.Enabled = canCreateOrders;
            btnGenerateFromOrder.Enabled = canCreateOrders;
            btnGenerateFromFilter.Enabled = canCreateOrders;
            btnViewStoreTransferTemplates.Enabled = canCreateOrders;
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

        private void btnManageTransferOrder_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.Manage;
            RequestFinish?.Invoke(this, EventArgs.Empty);
        }

        private void btnNewTransferOrder_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.CreateNew;
            parent.AddPage(new NewStoreTransfer(parent, StoreTransferTypeEnum.Order));
        }

        private void btnGenerateFromRequest_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromRequest;
            parent.AddPage(new ExistingStoreTransferSearch(parent, storeTransferAction.Action, StoreTransferTypeEnum.Order));
        }

        private void btnGenerateFromOrder_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromOrder;
            parent.AddPage(new ExistingStoreTransferSearch(parent, storeTransferAction.Action, StoreTransferTypeEnum.Order));
        }

        private void btnGenerateFromFilter_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromFilter;
            parent.AddPage(new CreateStoreTransferFromFilter(parent, StoreTransferTypeEnum.Order));
        }

        private void btnViewStoreTransferTemplates_Click(object sender, EventArgs e)
        {
            storeTransferAction.Action = StoreTransferActionEnum.GenerateFromTemplate;
            parent.AddPage(new NewInventoryDocumentFromTemplate(parent, new InventoryTypeAction { Action = InventoryActionEnum.GenerateFromTemplate, InventoryType = InventoryEnum.StoreTransfer, StoreTransferType = StoreTransferTypeEnum.Order }));
        }
    }
}
