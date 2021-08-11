using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    /// <summary>
    /// Defines an empty stock counting journal wizard page.
    /// </summary>
    public partial class NewEmptySCJ : UserControl, IWizardPage
    {
        WizardBase parent;
        InventoryTypeAction inventoryTypeAction;
        private InventoryAdjustment stockCountingJournal;
        private InventoryTemplateFilterContainer filter;

        /// <summary>
        /// Initializes a new instance of an empty stock counting journal wizard page.
        /// </summary>
        /// <param name="parent">The parent wizard instance.</param>
        /// <param name="inventoryTypeAction">The type of inventory action.</param>
        public NewEmptySCJ(WizardBase parent, InventoryTypeAction inventoryTypeAction)
            : this()
        {
            this.parent = parent;
            this.inventoryTypeAction = inventoryTypeAction;
        }

        // When copying an older stock counting journal
        public NewEmptySCJ(WizardBase parent, InventoryAdjustment stockCountingJournal, InventoryTypeAction inventoryTypeAction)
            : this()
        {

            this.parent = parent;
            this.stockCountingJournal = stockCountingJournal;
            this.inventoryTypeAction = inventoryTypeAction;

            tbStockCountingJournalDescription.Text = stockCountingJournal.Text;
            cmbStore.SelectedData = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, stockCountingJournal.StoreId);
        }

        // When generating a stock counting journal from item filter
        public NewEmptySCJ(WizardBase parent, InventoryTemplateFilterContainer filter, InventoryTypeAction inventoryTypeAction)
            : this(parent, inventoryTypeAction)
        {
            this.filter = filter;
        }

        private NewEmptySCJ()
        {
            InitializeComponent();
            
        }


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
            btnOkEnabled(this, EventArgs.Empty);
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            throw new NotImplementedException();
        }

        public void ResetControls()
        {

        }

        /// <summary>
        /// Gets the selected store Id.
        /// </summary>
        public RecordIdentifier StoreID { get { return cmbStore.SelectedDataID; } }

        /// <summary>
        /// Gets the text of the description.
        /// </summary>
        public string Description { get { return tbStockCountingJournalDescription.Text; } }

        public InventoryAdjustment StockCountingJournal { get { return stockCountingJournal; } }

        public InventoryTemplateFilterContainer Filter { get { return filter; } }

        #endregion

        private void NewEmptySCJ_Load(object sender, EventArgs e)
        {
            if (!PluginEntry.DataModel.IsHeadOffice)
            {
                cmbStore.SelectedData = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                cmbStore.Enabled = false;
            }
            tbStockCountingJournalDescription.Focus();
        }

        private void btnOkEnabled(object sender, EventArgs e)
        {
            if (cmbStore.SelectedData != null)
            {
                parent.NextEnabled = (cmbStore.SelectedData.ID != "");
            }
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            btnOkEnabled(sender, e);
        }
    }
}
