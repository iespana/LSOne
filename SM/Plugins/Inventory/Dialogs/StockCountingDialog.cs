using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.StoreManagement;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class StockCountingDialog : DialogBase
    {
        RecordIdentifier journalID;
        
        IInventoryService service = null;

        public StockCountingDialog()
        {
            InitializeComponent();
            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                Store store = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);
                cmbStore.SelectedData = new DataEntity(store.ID, store.Text);
                cmbStore.Enabled = false;
                btnOK.Enabled = false;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier JournalID
        {
            get { return journalID; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InventoryAdjustment stockCounting = new InventoryAdjustment();
            stockCounting.JournalType = InventoryJournalTypeEnum.Counting;
            stockCounting.Text = tbDescription.Text;
            stockCounting.CreatedDateTime = DateTime.Now;
            stockCounting.StoreId = cmbStore.SelectedDataID;


            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            stockCounting =  service.SaveInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCounting,true);

            journalID = stockCounting.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            errorProvider1.Clear();
            btnOK.Enabled = cmbStore.SelectedDataID != RecordIdentifier.Empty
                            && !string.IsNullOrEmpty(tbDescription.Text.Trim());
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}
