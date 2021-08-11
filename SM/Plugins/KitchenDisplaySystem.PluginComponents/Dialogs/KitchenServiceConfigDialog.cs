using System;
using System.Windows.Forms;
using LSOne.KitchenDisplaySystem.KdsClient;
using LSOne.KitchenDisplaySystem.KdsCommon;
using LSOne.KitchenDisplaySystem.KdsCommon.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class KitchenServiceConfigDialog : DialogBase
    {
        private IKdsClient kClient;
        private KitchenServiceConfig kitchenServiceConfig;

        public KitchenServiceConfigDialog(string ksHost, int ksPort)
        {
            InitializeComponent();
            kClient = new Client(ksHost, ksPort, 5);
            kitchenServiceConfig = new KitchenServiceConfig();
            tbHost.Text = ksHost;
            ntbPort.Value = ksPort;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnGetConfig.Enabled = !string.IsNullOrWhiteSpace(tbHost.Text) 
                                && !string.IsNullOrWhiteSpace(ntbPort.Text);

            btnSendConfig.Enabled =
                                   !string.IsNullOrWhiteSpace(tbHost.Text)
                                && !string.IsNullOrWhiteSpace(ntbPort.Text)
                                && !string.IsNullOrWhiteSpace(ntbDSPort.Text)
                                && !string.IsNullOrWhiteSpace(ntbPOSPort.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnGetConfig_Click(object sender, EventArgs e)
        {
            btnGetConfig.Enabled = false;
            ShowProgress(GetConfig, Properties.Resources.GettingConfig);
            btnGetConfig.Enabled = true;
        }

        private void GetConfig(object sender, EventArgs e)
        {
            kitchenServiceConfig = kClient.GetConfig();

            if (kitchenServiceConfig == null)
            {
                kitchenServiceConfig = new KitchenServiceConfig();
                MessageDialog.Show(Properties.Resources.CouldNotGetConfig);
            }

            ntbDSPort.Text = kitchenServiceConfig.DisplayStationBasePort.ToString();
            ntbPOSPort.Text = kitchenServiceConfig.PosBasePort.ToString();

            HideProgress();
        }

        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            kitchenServiceConfig.DisplayStationBasePort = Convert.ToInt32(ntbDSPort.Text);
            kitchenServiceConfig.PosBasePort = Convert.ToInt32(ntbPOSPort.Text);

            bool configSentSuccessfully = kClient.SetConfig(kitchenServiceConfig);
            if (configSentSuccessfully)
            {
                MessageDialog.Show(Properties.Resources.ConfigSent);
                Close();
            }
            else
            {
                MessageDialog.Show(Properties.Resources.CouldNotSendConfig, MessageBoxIcon.Error);
            }
        }

    }
}
