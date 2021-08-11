using System;
using System.ComponentModel;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class SettingsView : ViewBase
    {
        private SchedulerSettings settings;

        public SettingsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SettingsDescription;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.Settings;
            this.ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.SettingsEdit);

            settings = PluginEntry.SchedulerSettings;
            SettingsToForm(settings);
        }

        private void SettingsToForm(SchedulerSettings schedulerSettings)
        {
            ServerSettingsToForm(schedulerSettings.ServerSettings);
        }

        private void SettingsFromForm(SchedulerSettings schedulerSettings)
        {
            ServerSettingsFromForm(schedulerSettings.ServerSettings);
        }

        private void ServerSettingsToForm(ServerSettings serverSettings)
        {
            tbServerHost.Text = serverSettings.Host;
            cmbServerNetMode.SelectedIndex = (serverSettings.NetMode == NetMode.TCPS) ? 1 : 0;
            tbServerPort.Text = serverSettings.Port;
        }

        private void ServerSettingsFromForm(ServerSettings serverSettings)
        {
            serverSettings.Host = tbServerHost.Text;
            serverSettings.NetMode = (cmbServerNetMode.SelectedIndex == 0) ? NetMode.TCP : NetMode.TCPS;
            serverSettings.Port = string.IsNullOrEmpty(tbServerPort.Text) ? null : tbServerPort.Text;
        }

        private bool ServerSettingsModified(ServerSettings serverSettings)
        {
            if (serverSettings.Host != (string.IsNullOrEmpty(tbServerHost.Text) ? null : tbServerHost.Text))
                return true;

            if ((cmbServerNetMode.SelectedIndex == 0 && serverSettings.NetMode == NetMode.TCPS) ||
                (cmbServerNetMode.SelectedIndex == 1 && serverSettings.NetMode != NetMode.TCPS))
                return true;

            if (serverSettings.Port != (string.IsNullOrEmpty(tbServerPort.Text) ? null : tbServerPort.Text))
                return true;

            return false;
        }



        protected override bool DataIsModified()
        {
            return ServerSettingsModified(settings.ServerSettings);
        }


        protected override bool SaveData()
        {
            SettingsFromForm(settings);

            DataProviderFactory.Instance.Get<IInfoData, JscInfo>().Save(PluginEntry.DataModel, settings);
            
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "SchedulerSettings", RecordIdentifier.Empty, null);

            return true;
        }

        private void tbServerPort_TextChanged(object sender, EventArgs e)
        {
            lbServerDefaultPort.Visible = tbServerPort.TextLength == 0;
        }

        private void cmbServerNetMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            NetMode netMode = (cmbServerNetMode.SelectedIndex == 1) ? NetMode.TCPS : NetMode.TCP;

            int defaultPort = AppConfig.GetRouterPortByMode(netMode);
            lbServerDefaultPort.Text = defaultPort.ToString();
        }

        private void tbServerHost_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbServerHost.Text))
            {
                errorProvider.SetError(tbServerHost, Properties.Resources.SettingsServerHostEmpty);
            }
            else
            {
                errorProvider.SetError(tbServerHost, string.Empty);
            }
        }

        private void tbServerPort_Validating(object sender, CancelEventArgs e)
        {
            string errorText = string.Empty;

            if (tbServerPort.TextLength > 0)
            {
                int port;
                if (!int.TryParse(tbServerPort.Text, out port))
                {
                    errorText = Properties.Resources.SettingsServerInvalidPort;
                }
                else if (port <= 0)
                {
                    errorText = Properties.Resources.SettingsServerInvalidPort;
                }
            }

            errorProvider.SetError(tbServerPort, errorText);
        }


    }
}
