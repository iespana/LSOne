using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class TerminalSspSettingPage : UserControl, ITabView
    {
        private Terminal terminal;

        public TerminalSspSettingPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalSspSettingPage();
        }

        public bool DataIsModified()
        {
            if (cmbHoSiteServiceProfile.SelectedData.ID != terminal.TransactionServiceProfileID) return true;
            if (cmbStoreSiteServiceProfile.SelectedData.ID != terminal.HospTransServiceProfileID) return true;
            
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;
            cmbHoSiteServiceProfile.SelectedData = new DataEntity(terminal.TransactionServiceProfileID, terminal.TransactionServiceProfileName);
            cmbStoreSiteServiceProfile.SelectedData = new DataEntity(terminal.HospTransServiceProfileID, terminal.HospTransServiceProfileName);
        }

        public void OnClose()
        {
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            terminal.TransactionServiceProfileID = cmbHoSiteServiceProfile.SelectedData.ID;
            terminal.TransactionServiceProfileName = cmbHoSiteServiceProfile.SelectedData.Text;
            terminal.HospTransServiceProfileID = cmbStoreSiteServiceProfile.SelectedData.ID;
            terminal.HospTransServiceProfileName = cmbStoreSiteServiceProfile.SelectedData.Text;
            return true;
        }

        public void SaveUserInterface()
        {
        }

        private void cmb_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.SiteServiceProfileData.GetList(PluginEntry.DataModel);

            ((DualDataComboBox)sender).SetData(items,
                null, true);
        }

        private void cmb_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity();
        }

        private void btnEditHoSiteServiceProfile_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowTransactionServiceProfilesSheet(cmbHoSiteServiceProfile.SelectedData.ID);
        }

        private void btnEditStoreSiteServiceProfile_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowTransactionServiceProfilesSheet(cmbStoreSiteServiceProfile.SelectedData.ID);
        }

    }
}
