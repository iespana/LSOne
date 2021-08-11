using System;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewEmptyIJ : UserControl, IWizardPage
    {
        protected WizardBase parent;
        protected InventoryJournalTypeEnum journalType;
        protected bool manageAllStores = false;

        public NewEmptyIJ()
        {
            InitializeComponent();
        }

        public NewEmptyIJ(WizardBase parent, InventoryJournalTypeEnum type)
            : this()
        {
            this.parent = parent;
            this.journalType = type;

            string permission = "";
            switch(journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    permission = Permission.ManageInventoryAdjustmentsForAllStores;
                    break;
                case InventoryJournalTypeEnum.Reservation:
                    permission = Permission.ManageStockReservationsForAllStores;
                    break;
                case InventoryJournalTypeEnum.Parked:
                    permission = Permission.ManageParkedInventoryForAllStores;
                    break;
                default:
                    break;
            }
            manageAllStores = !string.IsNullOrWhiteSpace(permission) && PluginEntry.DataModel.HasPermission(permission);
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
            switch(journalType)
            {
                case InventoryJournalTypeEnum.Adjustment:
                    parent.Text = Resources.NewInventoryAdjustment;
                    break;
                case InventoryJournalTypeEnum.Reservation:
                    parent.Text = Resources.NewStockReservation;
                    break;
                case InventoryJournalTypeEnum.Parked:
                    parent.Text = Resources.NewParkedInventory;
                    break;
                default:
                    break;
            }
            
            btnOkEnabled(this, EventArgs.Empty);
            tbJournalDescription.Focus();
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

        // Keep this in sync with autotest action CreateInventoryJournal
        public InventoryAdjustment GetNewJournal()
        {
            var journalEntry = new InventoryAdjustment()
            {
                Text = tbJournalDescription.Text.Trim(),
                CreatedDateTime = DateTime.Now,
                StoreId = cmbStore.SelectedData.ID,
                PostedDateTime = Date.Empty,
                JournalType = journalType
            };

            return journalEntry;
        }

        private void NewEmptyIJ_Load(object sender, EventArgs e)
        {
            cmbStore.SelectedData = new DataEntity("", "");

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                Store currentStore = Providers.StoreData.Get(PluginEntry.DataModel, (string)PluginEntry.DataModel.CurrentStoreID);
                cmbStore.SelectedData = new DataEntity(currentStore.ID, currentStore.Text);
            }
            cmbStore.Enabled = manageAllStores;

            tbJournalDescription.Focus();
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            try
            {
                var stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                cmbStore.SetData(stores, null);
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            btnOkEnabled(sender, e);
        }

        private void tbJournalDescription_TextChanged(object sender, EventArgs e)
        {
            btnOkEnabled(sender, e);
        }

        private void btnOkEnabled(object sender, EventArgs e)
        {
            parent.NextEnabled = (!string.IsNullOrWhiteSpace(tbJournalDescription.Text)) 
                                    && (cmbStore.SelectedData.ID != "");
        }
    }
}