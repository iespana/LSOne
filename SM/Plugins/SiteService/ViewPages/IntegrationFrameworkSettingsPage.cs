using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SiteService.Dialogs;
using ServiceType = LSOne.DataLayer.GenericConnector.Enums.ServiceType;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using ContainerControl = LSOne.Controls.ContainerControl;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    public partial class IntegrationFrameworkSettingsPage : ContainerControl, ITabViewV2
    {
        private SiteServiceProfile profile;
        private bool licenseValid = true;
        private bool tcpEnabled = false;
        private bool httpEnabled = false;
        private bool httpsEnabled = false;
        private bool httpsProtocolChanged = false;

        public IntegrationFrameworkSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new IntegrationFrameworkSettingsPage();
        }

        public bool DataIsModified()
        { 
            if(!licenseValid)
            {
                return false;
            }

            return httpsProtocolChanged ||
                   txtHttpPort.Text != profile.IFHttpPort ||
                   txtNetPort.Text != profile.IFTcpPort ||
                   chkHttp.Checked != httpEnabled ||
                   chkNetPort.Checked != tcpEnabled;
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (SiteServiceProfile)internalContext;
            txtHost.Text = profile.SiteServiceAddress;

            var licenseService = (ILicenseService)PluginEntry.DataModel.Service(ServiceType.LicenseService);

            DateTime ifValidation = licenseService.ValidateIntegrationFrameworkLicense(PluginEntry.DataModel);

            if (ifValidation == DateTime.MinValue) //Invalid license
            {
                licenseValid = false;
                MessageDialog.Show(Properties.Resources.UpgradeIFLicense);
                DisableView();
            }
            else
            {
                ParseProtocols();
                txtHttpPort.Text = profile.IFHttpPort;
                txtNetPort.Text = profile.IFTcpPort;
                chkNetPort.Checked = tcpEnabled;
                chkHttp.Checked = httpEnabled;
            }
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "TransactionServiceProfile" && changeHint != DataEntityChangeType.Delete && changeIdentifier == profile.ID)
            {
                profile = (SiteServiceProfile)param;
                txtHost.Text = profile.SiteServiceAddress;
            }
        }

        public bool SaveData()
        {
            profile.IFTcpPort = txtNetPort.Text.Trim();
            profile.IFHttpPort = txtHttpPort.Text.Trim();
            profile.IFProtocols = GetProtocols();

            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            try
            {
                ServiceConnectionStatus status = service.TestIntegrationFrameworkConnection(PluginEntry.DataModel, chkNetPort.Checked, chkHttp.Checked, GetBindingConfiguration());

                StringBuilder sb = new StringBuilder();

                if(chkNetPort.Checked)
                {
                    sb.Append(status.NetTcpConnectionSuccesfull ? Properties.Resources.IFNetTcpConnectionWorked : Properties.Resources.IFNetTcpConnectionFailed);
                }

                if(chkHttp.Checked)
                {
                    if(chkNetPort.Checked)
                    {
                        sb.AppendLine();
                    }

                    sb.Append(status.HttpConnectionSuccesfull ? Properties.Resources.IFHttpConnectionWorked : Properties.Resources.IFHttpConnectionFailed);
                }

                MessageDialog.Show(sb.ToString());
            }
            catch(Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void btnConfiguration_Click(object sender, EventArgs e)
        {
            using (IntegrationFrameworkConfigDialog configDlg = new IntegrationFrameworkConfigDialog(profile, GetBindingConfiguration()))
            {
                if (DialogResult.OK == configDlg.ShowDialog())
                {
                    httpsProtocolChanged = httpsEnabled != configDlg.UseHttps;
                    httpsEnabled = configDlg.UseHttps;
                }
            }
        }

        private void chkNetPort_CheckedChanged(object sender, EventArgs e)
        {
            txtNetPort.Enabled = chkNetPort.Checked;
        }

        private void chkHttp_CheckedChanged(object sender, EventArgs e)
        {
            txtHttpPort.Enabled = chkHttp.Checked;
        }

        private void DisableView()
        {
            foreach(System.Windows.Forms.Control ctrl in grpSettings.Controls)
            {
                ctrl.Enabled = false;
            }
        }

        private void TextBoxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ParseProtocols()
        {
            string[] protocols = profile.IFProtocols.Split(';');

            foreach(string protocol in protocols)
            {
                switch(protocol)
                {
                    case "net.tcp":
                        tcpEnabled = true;
                        break;
                    case "http":
                        httpEnabled = true;
                        break;
                    case "https":
                        httpsEnabled = true;
                        break;
                }
            }
        }

        private string GetProtocols()
        {
            bool hasProtocol = false;
            StringBuilder sb = new StringBuilder();

            if(chkNetPort.Checked)
            {
                sb.Append("net.tcp");
                hasProtocol = true;
            }

            if(chkHttp.Checked)
            {
                sb.Append(hasProtocol ? ";http" : "http");

                if (httpsEnabled)
                {
                    sb.Append(";https");
                }
            }

            return sb.ToString();
        }

        private WebserviceConfiguration GetBindingConfiguration()
        {
            WebserviceConfiguration config = new WebserviceConfiguration();

            UInt16 tcpPort = 0;
            UInt16.TryParse(txtNetPort.Text, out tcpPort);

            UInt16 httpPort = 0;
            UInt16.TryParse(txtHttpPort.Text, out httpPort);

            config.TcpPort = tcpPort;
            config.HttpPort = httpPort;
            config.Host = profile.SiteServiceAddress;
            config.UseHttps = httpsEnabled;
            config.SSLThumbnail = profile.IFSSLCertificateThumbnail;
            config.EnableTcp = chkNetPort.Checked;
            config.EnableHttp = chkHttp.Checked;

            return config;
        }
    }
}
