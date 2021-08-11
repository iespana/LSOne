using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    public partial class StoreKdsPage : UserControl, ITabView
    {
        private Store store;

        public StoreKdsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreKdsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (LSOne.DataLayer.BusinessObjects.StoreManagement.Store)internalContext;
            cmbKitchenManagerProfile.SelectedData = new DataEntity(store.KitchenServiceProfileID, store.KitchenServiceProfileName);
            SetEditButtonEnabled();
        }

        public bool DataIsModified()
        {
            if (cmbKitchenManagerProfile.SelectedData.ID != store.KitchenServiceProfileID) return true;

            return false;
        }

        public bool SaveData()
        {
            store.KitchenServiceProfileID = cmbKitchenManagerProfile.SelectedData.ID;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "KitchenServiceProfile" && changeHint == DataEntityChangeType.Delete)
            {
                if (changeIdentifier == cmbKitchenManagerProfile.SelectedData.ID)
                {
                    cmbKitchenManagerProfile.SelectedData = new DataEntity(Guid.Empty, "");
                    SetEditButtonEnabled();
                }
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void SetEditButtonEnabled()
        {
            btnEditKitchenManagerProfile.Enabled = (cmbKitchenManagerProfile.SelectedData.ID != Guid.Empty);
        }

        private void cmbKitchenManagerProfile_RequestData(object sender, EventArgs e)
        {
            List<KitchenServiceProfile> items = Providers.KitchenDisplayTransactionProfileData.GetList(PluginEntry.DataModel);
            cmbKitchenManagerProfile.SetData(items.OrderBy(p => p.Text), null, true);
        }

        private void btnEditKitchenManagerProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowKitchenServiceProfileView(cmbKitchenManagerProfile.SelectedData.ID);
        }

        private void cmbKitchenManagerProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbKitchenManagerProfile.SelectedData.ID == RecordIdentifier.Empty)
            {
                cmbKitchenManagerProfile.SelectedData.ID = Guid.Empty;
            }

            SetEditButtonEnabled();
        }

        private void cmbKitchenManagerProfile_RequestClear(object sender, EventArgs e)
        {

        }

        private void btnAddKitchenManagerProfile_Click(object sender, EventArgs e)
        {
            RecordIdentifier newProfileID = PluginOperationsHelper.AddKitchenServiceProfile();

            if(!RecordIdentifier.IsEmptyOrNull(newProfileID))
            {
                KitchenServiceProfile newProfile = Providers.KitchenDisplayTransactionProfileData.Get(PluginEntry.DataModel, newProfileID);
                cmbKitchenManagerProfile.SelectedData = newProfile;
                SetEditButtonEnabled();
            }
        }
    }
}
