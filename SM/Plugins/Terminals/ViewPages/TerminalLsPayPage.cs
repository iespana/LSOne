using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LSPay.Client;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    public partial class TerminalsLsPayPage : UserControl, ITabView
    {
        private List<DataEntity> lsPayPluginList;
        private Terminal terminal;

        public TerminalsLsPayPage()
        {
            InitializeComponent();
            lsPayPluginList = new List<DataEntity>();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TerminalsLsPayPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;

            chkUseLocalServer.Checked = terminal.LSPayUseLocalServer;
            chkRefRefunds.Checked = terminal.LSPaySupportReferenceRefund;

            SetLocalOrHwProfileServer();

            cmbPlugin.SelectedData = terminal.LSPayPlugin;
        }

        private void SetLocalOrHwProfileServer()
        {
            if (chkUseLocalServer.Checked)
            {
                tbServerName.Text = terminal.LSPayServerName;
                ntbServerPort.Value = terminal.LSPayServerPort;
            }
            else
            {
                var hwProfile = Providers.HardwareProfileData.Get(PluginEntry.DataModel, terminal.HardwareProfileID);
                tbServerName.Text = hwProfile.EftServerName;
                ntbServerPort.Value = hwProfile.EftServerPort;
            }

            tbServerName.Enabled = ntbServerPort.Enabled = chkUseLocalServer.Checked;
        }

        public bool DataIsModified()
        {
            if (chkUseLocalServer.Checked)
            {
                if ((tbServerName.Text != "") && (tbServerName.Text != terminal.LSPayServerName)) return true;
                if ((ntbServerPort.Value != 0) && (ntbServerPort.Value != terminal.LSPayServerPort)) return true;
            }
            else
            {
                if (terminal.LSPayUseLocalServer) return true;
            }

            if ((GetSelectedLSPayPlugin().ID != "") && (GetSelectedLSPayPlugin().ID != (terminal.LSPayPlugin?.ID ?? "")))
            {
                return true;
            }

            if (chkRefRefunds.Checked != terminal.LSPaySupportReferenceRefund) return true;

            return false;
        }

        public bool SaveData()
        {
            terminal.LSPayServerName = tbServerName.Text;
            terminal.LSPayServerPort = (int)ntbServerPort.Value;
            terminal.LSPayUseLocalServer = chkUseLocalServer.Checked;
            terminal.LSPaySupportReferenceRefund = chkRefRefunds.Checked;
            terminal.LSPayPlugin = GetSelectedLSPayPlugin();

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

        private void chkUseLocalServer_CheckedChanged(object sender, System.EventArgs e)
        {
            SetLocalOrHwProfileServer();
        }

        private void cmbPluginID_RequestData(object sender, System.EventArgs e)
        {
            cmbPlugin.SetData(lsPayPluginList, null);
        }

        private void btnGetPluginList_Click(object sender, System.EventArgs e)
        {
            string serverName = tbServerName.Text != "" ? tbServerName.Text : "localhost";
            int serverPort = ntbServerPort.Value != 0 ? (int)ntbServerPort.Value : 57729;

            async void getPluginsAction()
            {
                LSPayClient payClient = new LSPayClient(serverName, serverPort.ToString());
                await payClient.Admin.StartAsync();
                if (payClient.ConnectionStatus == ConnectionState.Connected)
                {
                    var plugins = await payClient.Admin.GetAllPaymentPluginsAsync();
                    lsPayPluginList = plugins.Select(pluginInfo => new DataEntity(pluginInfo.ID, $"{pluginInfo.ID} ({pluginInfo.PSPName})")).ToList();
                }
                else
                {
                    lsPayPluginList.Clear();
                }
            }

            using (var dlg = new SpinnerDialog(
                Properties.Resources.RetrievingPluginListFromLsPayService, 
                getPluginsAction))
            {
                dlg.ShowDialog();
            }
        }

        private void cmbPlugin_RequestClear(object sender, EventArgs e)
        {
            //This event is required just so deleting works in the control
        }

        private DataEntity GetSelectedLSPayPlugin()
        {
            if (cmbPlugin.SelectedData.Text != cmbPlugin.Text)
            {
                //If the text is different from the one in the selected data, then the user typed something manually in the dropdown
                cmbPlugin.SelectedData = new DataEntity(cmbPlugin.Text, cmbPlugin.Text);
            }

            return (DataEntity)cmbPlugin.SelectedData;
        }
    }
}
