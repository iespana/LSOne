using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using Parameters = LSOne.DataLayer.BusinessObjects.Companies.Parameters;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ProfitLossDialog : DialogBase
    {
        private RecordIdentifier journalID = "";
        private InventoryJournalTypeEnum typeOfAdjustment;

        LSOne.DataLayer.BusinessObjects.UserManagement.User currentUser;
        IProfileSettings settings = PluginEntry.DataModel.Settings;

        InventoryAdjustment journalEntry;

        public ProfitLossDialog(InventoryJournalTypeEnum typeOfAdjustment)
        {
            InitializeComponent();

            cmbStore.SelectedData = new DataEntity(RecordIdentifier.Empty, "");

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                Store currentStore = Providers.StoreData.Get(PluginEntry.DataModel, (string)PluginEntry.DataModel.CurrentStoreID);
                cmbStore.SelectedData = new DataEntity(currentStore.ID, currentStore.Text);
            }
            bool enabled = PluginEntry.DataModel.HasPermission(Permission.ManageInventoryAdjustmentsForAllStores);
            cmbStore.Enabled = enabled;

            currentUser = Providers.UserData.Get(PluginEntry.DataModel, (Guid)PluginEntry.DataModel.CurrentUser.ID);
            tbEmployee.Text = settings.NameFormatter.Format(currentUser.Name);

            this.typeOfAdjustment = typeOfAdjustment;

            if(typeOfAdjustment == InventoryJournalTypeEnum.Reservation)
            {
                Text = Resources.StockReservation;
                Header = Resources.EnterDescriptionForStockReservation;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            journalEntry = new InventoryAdjustment();
            journalEntry.Text = tbDescription.Text;
            journalEntry.CreatedDateTime = DateTime.Now;
            journalEntry.StoreId = cmbStore.SelectedData.ID;
            journalEntry.PostedDateTime = Date.Empty;
            journalEntry.JournalType = typeOfAdjustment;

            try
            {
                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                service.SaveInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), journalEntry, true);

                journalID = journalEntry.ID;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
        }

        public RecordIdentifier JournalID
        {
            get { return journalID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text != "" && cmbStore.SelectedData.ID != RecordIdentifier.Empty;
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }
    }
}
