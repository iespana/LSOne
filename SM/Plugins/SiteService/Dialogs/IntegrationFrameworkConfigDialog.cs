using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects.Profiles;
using System;
using System.Windows.Forms;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.ViewCore.Interfaces;
using System.ServiceProcess;
using LSOne.Utilities.Cryptography;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects;
using System.Collections.Generic;
using System.Security;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    public partial class IntegrationFrameworkConfigDialog : DialogBase
    {
        private SiteServiceProfile siteServiceProfile;
        private UInt16 tcpPort;
        private UInt16 httpPort;
        private bool useHttps;
        private SecureString sslThumbnail;
        private Dictionary<string, string> configurations;
        private bool enableTcp;
        private bool enableHttp;
        private bool configurationChanged;

        public IntegrationFrameworkConfigDialog(SiteServiceProfile profile, WebserviceConfiguration config)
        {
            InitializeComponent();

            siteServiceProfile = profile;

            tcpPort = config.TcpPort;
            httpPort = config.HttpPort;
            useHttps = config.UseHttps;
            sslThumbnail = config.SSLThumbnail;
            enableTcp = config.EnableTcp;
            enableHttp = config.EnableHttp;
            configurationChanged = false;

            cmbCertificateLocation.SelectedIndex = 1;
            cmbCertificateStore.SelectedIndex = 4;

            OnGetConfigClick(this, EventArgs.Empty);
        }

        public bool UseHttps
        {
            get { return useHttps; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = configurationChanged ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void OnGetConfigClick(object sender, EventArgs e)
        {
            btnGetConfig.Enabled = false;
            ShowProgress(GetConfig, Properties.Resources.GettingConfig);
            btnGetConfig.Enabled = true;
        }

        private void GetConfig(object sender, EventArgs e)
        {
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(DataLayer.GenericConnector.Enums.ServiceType.SiteServiceService);

            try
            {
                configurations = service.LoadIFConfiguration(PluginEntry.DataModel, GetBindingConfiguration());

                chkHttps.Checked = configurations[SiteServiceConfigurationConstants.IFEnforceHttps] == "true";
                txtCertificateThumbnail.Text = configurations[SiteServiceConfigurationConstants.IFCertificateThumbnail];

                int certificateLocationIndex = cmbCertificateLocation.FindStringExact(configurations[SiteServiceConfigurationConstants.IFCertificateStoreLocation]);
                int certificateStoreIndex = cmbCertificateLocation.FindStringExact(configurations[SiteServiceConfigurationConstants.IFCertificateStoreName]);

                cmbCertificateLocation.SelectedIndex = certificateLocationIndex == -1 ? 1 : certificateLocationIndex;
                cmbCertificateStore.SelectedIndex = certificateStoreIndex == -1 ? 4 : certificateStoreIndex;

                HideProgress();
                ShowProgress(OnLoaded, Properties.Resources.SiteServiceConfigGet);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
                HideProgress();
            }
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            HideProgress();
        }

        private void OnSendConfigClick(object sender, EventArgs e)
        {
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(DataLayer.GenericConnector.Enums.ServiceType.SiteServiceService);

            try
            {
                WebserviceConfiguration bindingConfig = GetBindingConfiguration();
                configurations = service.LoadIFConfiguration(PluginEntry.DataModel, bindingConfig);

                configurations[SiteServiceConfigurationConstants.IFEnforceHttps] = chkHttps.Checked.ToString().ToLower();
                configurations[SiteServiceConfigurationConstants.IFCertificateThumbnail] = txtCertificateThumbnail.Text;
                configurations[SiteServiceConfigurationConstants.IFCertificateStoreLocation] = cmbCertificateLocation.SelectedItem.ToString();
                configurations[SiteServiceConfigurationConstants.IFCertificateStoreName] = cmbCertificateStore.SelectedItem.ToString();
                configurations[SiteServiceConfigurationConstants.IFEnableNetTcpEndpoint] = enableTcp.ToString().ToLower();
                configurations[SiteServiceConfigurationConstants.IFEnableHttpEndpoint] = enableHttp.ToString().ToLower();

                service.SendIFConfiguration(PluginEntry.DataModel, bindingConfig, configurations);

                //Update local configs
                useHttps = chkHttps.Checked;
                sslThumbnail = SecureStringHelper.FromString(txtCertificateThumbnail.Text);

                siteServiceProfile.IFSSLCertificateThumbnail = sslThumbnail;
                Providers.SiteServiceProfileData.SaveSSLThumbnail(PluginEntry.DataModel, siteServiceProfile);

                configurationChanged = true;
                ShowProgress(RestartService, Properties.Resources.SiteServiceConfigSend);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void RestartService(object sender, EventArgs e)
        {
            try
            {
                var timeout = TimeSpan.FromSeconds(60);
                var ctrl = new ServiceController("LSOneSiteService", siteServiceProfile.SiteServiceAddress);
                if (ctrl.Status == ServiceControllerStatus.Running)
                {
                    ctrl.Stop();
                    ctrl.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                if (ctrl.Status == ServiceControllerStatus.Stopped)
                {
                    ctrl.Start();
                    ctrl.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format(Properties.Resources.SiteServiceError, ex.Message));
            }
            HideProgress();
        }

        private void chkHttps_CheckedChanged(object sender, EventArgs e)
        {
            txtCertificateThumbnail.Enabled =
            cmbCertificateLocation.Enabled =
            cmbCertificateStore.Enabled = chkHttps.Checked;
        }

        private WebserviceConfiguration GetBindingConfiguration()
        {
            WebserviceConfiguration config = new WebserviceConfiguration();

            config.TcpPort = tcpPort;
            config.HttpPort = httpPort;
            config.Host = siteServiceProfile.SiteServiceAddress;
            config.UseHttps = useHttps;
            config.SSLThumbnail = sslThumbnail;

            return config;
        }
    }
}
