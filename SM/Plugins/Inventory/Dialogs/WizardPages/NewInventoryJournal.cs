using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class NewInventoryJournal : UserControl, IWizardPage
    {
        WizardBase parent;

        public NewInventoryJournal()
        {
            InitializeComponent();
        }

        public NewInventoryJournal(WizardBase parent) : this()
        {
            this.parent = parent;

            btnInvAdjustment.Enabled = PluginOperations.HasInventoryJournalPermission(InventoryJournalTypeEnum.Adjustment);
            btnStockReservation.Enabled = PluginOperations.HasInventoryJournalPermission(InventoryJournalTypeEnum.Reservation);
            btnParked.Enabled = PluginOperations.HasInventoryJournalPermission(InventoryJournalTypeEnum.Parked);
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
            parent.Text = Properties.Resources.NewInventoryJournal;
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

        private void btnInvAdjustment_Click(object sender, EventArgs e)
        {
            parent.AddPage(new NewEmptyIJ(parent, InventoryJournalTypeEnum.Adjustment));
        }

        private void btnStockReservation_Click(object sender, EventArgs e)
        {
            parent.AddPage(new NewEmptyIJ(parent, InventoryJournalTypeEnum.Reservation));
        }

        private void btnParked_Click(object sender, EventArgs e)
        {
            parent.AddPage(new NewEmptyIJ(parent, InventoryJournalTypeEnum.Parked));
        }
    }
}
