using System;
using System.Collections.Generic;
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
    public partial class TerminalKdsPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalKdsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.TerminalKdsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext; 
            cmbKitchenDisplayProfile.SelectedData = new DataEntity(terminal.KitchenServiceProfileID, terminal.KitchenServiceProfileName);
            BtnEnabled();
        }

        public bool DataIsModified()
        {
            if (cmbKitchenDisplayProfile.SelectedData.ID != terminal.KitchenServiceProfileID) return true;

            return false;
        }

        public bool SaveData()
        {
            terminal.KitchenServiceProfileID = cmbKitchenDisplayProfile.SelectedData.ID;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.Edit && objectName == "KitchenManagerProfile" && changeIdentifier == cmbKitchenDisplayProfile.SelectedData.ID)
            {
                cmbKitchenDisplayProfile.SelectedData = (KitchenServiceProfile)param;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnEditKitchenManagerProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowKitchenServiceProfileView(cmbKitchenDisplayProfile.SelectedData.ID);
        }

        private void cmbKitchenDisplayProfile_RequestData(object sender, EventArgs e)
        {
            List<KitchenServiceProfile> items = Providers.KitchenDisplayTransactionProfileData.GetList(PluginEntry.DataModel);
            cmbKitchenDisplayProfile.SetData(items, null, true);
        }

        private void cmbKitchenDisplayProfile_RequestClear(object sender, EventArgs e)
        {
            cmbKitchenDisplayProfile.SelectedData = new DataEntity();
        }

        private void cmbKitchenDisplayProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            BtnEnabled();
        }

        private void BtnEnabled()
        {
            btnEditKitchenManagerProfile.Enabled = (terminal.KitchenServiceProfileID != Guid.Empty);            
        }
        
    }
}
