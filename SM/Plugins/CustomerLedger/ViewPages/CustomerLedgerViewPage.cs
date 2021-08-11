using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CustomerLedger.ViewPages
{
    internal partial class CustomerLedgerViewPage : UserControl, ITabView
    {
        private Parameters paramsData;
        public CustomerLedgerViewPage()
        {
            InitializeComponent();
            paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            paramsData.SiteServiceProfile = paramsData.SiteServiceProfile == "" ? RecordIdentifier.Empty : paramsData.SiteServiceProfile;

            btnRecreateCustLed.Enabled = PluginEntry.DataModel.HasPermission(Permission.CustomerLedgerEntriesEdit);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerLedgerViewPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }


        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show(Properties.Resources.LocalDBRecreateCustomerLedger, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            
            var recreateCustLedEntriesDialog = new Dialogs.RecreateCustomerLedgerDialog(true);
            recreateCustLedEntriesDialog.ShowDialog();
        }
    }
}
