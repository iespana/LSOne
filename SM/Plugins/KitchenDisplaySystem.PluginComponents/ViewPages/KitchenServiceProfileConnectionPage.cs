using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.KitchenDisplaySystem.KdsClient;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    public partial class KitchenServiceProfileConnectionPage : UserControl, ITabView
    {
        private KitchenServiceProfile profile;

        public KitchenServiceProfileConnectionPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new KitchenServiceProfileConnectionPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (KitchenServiceProfile)internalContext;
            tbHost.Text = profile.KitchenServiceAddress;
            ntbPort.Value = profile.KitchenServicePort;
        }

        public bool DataIsModified()
        {
            if (tbHost.Text != profile.KitchenServiceAddress) return true;
            if (ntbPort.Value != profile.KitchenServicePort) return true;
            return false;
        }

        public bool SaveData()
        {
            profile.KitchenServiceAddress = tbHost.Text;
            profile.KitchenServicePort = (int)ntbPort.Value;
            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion
     
        private void TestConnection(object sender, EventArgs e)
        {
            Client kClient = new Client(tbHost.Text, (int)ntbPort.Value, 5);
            string message =  string.Empty;

            try
            {
                bool connected = kClient.Connect(tbHost.Text, "SiteManager", DeviceTypeEnum.POS, "");
                message = connected ? Properties.Resources.ConnectingToKitchenServiceWorked : Properties.Resources.ConnectingToKitchenServiceFailed;
            }
            catch
            {
                message = Properties.Resources.ConnectingToKitchenServiceFailed;
            }
            finally
            {
                kClient.Disconnect();
                PluginEntry.Framework.ViewController.CurrentView.HideProgress();
                MessageDialog.Show(message);
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.KitchenServiceDebugDialog {TCPSPort = (int) ntbPort.Value, Host = tbHost.Text};
            dlg.Show();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.CurrentView.ShowProgress(TestConnection, Properties.Resources.ConnectingToKitchenService);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ManageKitchenServiceConfig(tbHost.Text, (int)ntbPort.Value);
        }
    }
}
