using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewPurchaseOrder : UserControl, IWizardPage
    {
        WizardBase parent;
        private InventoryTypeAction inventoryTypeAction;

        public event EventHandler RequestFinish;

        public NewPurchaseOrder(WizardBase parent, InventoryTypeAction inventoryTypeAction) : base()
        {
            InitializeComponent();

            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;
        }

        #region IWizardPage Members

        public InventoryTypeAction InventoryTypeAction
        {
            get { return inventoryTypeAction; }
        }

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

        private void btnManagePO_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PurchaseOrdersView());
            inventoryTypeAction.Action = InventoryActionEnum.Manage;
            RequestFinish?.Invoke(this, EventArgs.Empty);
        }

        private void btnExistingPO_Click(object sender, EventArgs e)
        {
            inventoryTypeAction.Action = InventoryActionEnum.GenerateFromExisting;
            parent.AddPage(new ExistingPOSearch(parent, inventoryTypeAction));
        }

        private void btnEmptyPO_Click(object sender, EventArgs e)
        {
            inventoryTypeAction.Action = InventoryActionEnum.New;
            parent.AddPage(new NewEmptyPO(parent, inventoryTypeAction));
        }

        private void btnWorksheets_Click(object sender, EventArgs e)
        {
            inventoryTypeAction.Action = InventoryActionEnum.GenerateFromTemplate;
            parent.AddPage(new NewPOFromWorksheet(parent, inventoryTypeAction));
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            inventoryTypeAction.Action = InventoryActionEnum.GenerateFromFilter;
            parent.AddPage(new CreateFromFilter(parent, inventoryTypeAction));
        }

        private void btnViewPurchaseOrderTemplates_Click(object sender, EventArgs e)
        {
            inventoryTypeAction.Action = InventoryActionEnum.GenerateFromTemplate;
            parent.AddPage(new NewInventoryDocumentFromTemplate(parent, inventoryTypeAction));
        }
    }
}
