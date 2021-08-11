using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class StoreSspSettingPage : UserControl, ITabView
    {
        private Store store;

        public StoreSspSettingPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreSspSettingPage();
        }

        public bool DataIsModified()
        {
            if (cmbTransactionServiceProfile.SelectedData.ID != store.TransactionServiceProfileID) return true;
            
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;
            cmbTransactionServiceProfile.SelectedData = new DataEntity(store.TransactionServiceProfileID, store.TransactionServiceProfileName);
        }

        public void OnClose()
        {
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            store.TransactionServiceProfileID = (string)cmbTransactionServiceProfile.SelectedData.ID;
            store.TransactionServiceProfileName = cmbTransactionServiceProfile.SelectedData.Text;
            return true;
        }

        public void SaveUserInterface()
        {
        }

        private void cmbTransactionServiceProfile_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.SiteServiceProfileData.GetList(PluginEntry.DataModel);

            cmbTransactionServiceProfile.SetData(items,
                null, true);
        }

        private void btnEditTransactionProfiles_Click(object sender, System.EventArgs e)
        {
            PluginOperations.ShowTransactionServiceProfilesSheet(cmbTransactionServiceProfile.SelectedData.ID);
        }

        private void cmbTransactionServiceProfile_RequestClear(object sender, EventArgs e)
        {
            cmbTransactionServiceProfile.SelectedData = new DataEntity();
        }
        
    }
}
