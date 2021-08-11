using System;
using System.Collections.Generic;
using System.Security;
using System.ServiceProcess;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects.Enums;
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

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
	public partial class ConnectionPage : UserControl, ITabView
	{
		private SiteServiceProfile profile;

		public ConnectionPage()
		{
			InitializeComponent();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new ConnectionPage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			profile = (SiteServiceProfile)internalContext;
			tbHost.Text = profile.SiteServiceAddress;
			ntbPort.Value = profile.SiteServicePortNumber;
			ntbTimeout.Value = profile.Timeout;
			ntbMaxMessageSize.Value = profile.MaxMessageSize;
		}

		public bool DataIsModified()
		{
			return
				(tbHost.Text != profile.SiteServiceAddress) ||
				(ntbPort.Value != profile.SiteServicePortNumber) ||
				(ntbTimeout.Value != profile.Timeout) ||
				(ntbMaxMessageSize.Value != profile.MaxMessageSize);
		}

		public bool SaveData()
		{
			profile.SiteServiceAddress = tbHost.Text;
			profile.SiteServicePortNumber = (int)ntbPort.Value;
			profile.Timeout = (int)ntbTimeout.Value;
			profile.MaxMessageSize = (int)ntbMaxMessageSize.Value;
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

		private void OnServerValuesChanged(object sender, EventArgs e)
		{
			btnTestConnection.Enabled = btnConfiguration.Enabled = btnStart.Enabled = btnStop.Enabled = 
				tbHost.Text.Length > 0 && ntbPort.Value != 0 && ntbMaxMessageSize.Value > 0;

            if (string.IsNullOrEmpty(tbHost.Text))
            {
                errorProvider1.SetError(tbHost, Properties.Resources.SiteServiceHostFieldEmpty);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

		private void btnTestConnection_Click(object sender, EventArgs e)
		{
			PluginEntry.Framework.ViewController.CurrentView.ShowProgress(TestConnection, Properties.Resources.ConnectingToStoreServer);
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				var ctrl = new ServiceController("LSOneSiteService", tbHost.Text.Trim());
				if (ctrl.Status == ServiceControllerStatus.Running)
				{
					ctrl.Stop();
					ctrl.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(60));
				}
				MessageDialog.Show(string.Format(Properties.Resources.SiteServiceOnHostHasBeenStopped, tbHost.Text.Trim()));
			}
			catch (Exception ex)
			{
				MessageDialog.Show(string.Format(Properties.Resources.SiteServiceError, ex.Message));
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			try
			{
				var timeout = TimeSpan.FromSeconds(60);
				var ctrl = new ServiceController("LSOneSiteService", tbHost.Text.Trim());
				if (ctrl.Status == ServiceControllerStatus.Stopped)
				{
					ctrl.Start();
					ctrl.WaitForStatus(ServiceControllerStatus.Running, timeout);
				}
				MessageDialog.Show(string.Format(Properties.Resources.SiteServiceOnHostHasBeenStarted, tbHost.Text.Trim()));
			}
			catch (Exception ex)
			{
				MessageDialog.Show(string.Format(Properties.Resources.SiteServiceError, ex.Message));
			}
		}

		private void TestConnection(object sender, EventArgs e)
		{
			PluginEntry.InitializeSiteServiceProfile();

			var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
		   
			var result = service.TestConnection(PluginEntry.DataModel, tbHost.Text.Trim(), (ushort)ntbPort.Value);

			PluginEntry.Framework.ViewController.CurrentView.HideProgress();

			if (result == ConnectionEnum.Success)
			{
				MessageDialog.Show(Properties.Resources.ConnectionToStoreServerWorked);
			}
			else if (result == ConnectionEnum.ConnectionFailed)
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer);
			}
			else if (result == ConnectionEnum.ExternalConnectionFailed)
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToExternalService);
				IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ExternalSystem", null);

				if (plugin != null)
				{
					plugin.Message(null, "ViewExternalSytemSettingsView", null);
				}
			}
			else if (result == ConnectionEnum.ClientTimeNotSynchronized)
			{
				MessageDialog.Show(Properties.Resources.ClientTimeNotSynchronizedMessage);
			}
			else
			{
				MessageDialog.Show(Properties.Resources.CouldNotConnectToDataBase);
			}
		}

		private void btnConfiguration_Click(object sender, EventArgs e)
		{
			bool openConfigDialog = false;
			SecureString securePassword;

			using (AuthorizationDialog authDlg = new AuthorizationDialog(tbHost.Text.Trim(), (ushort) ntbPort.Value))
			{
				DialogResult dlgResult = authDlg.ShowDialog();
				openConfigDialog = (dlgResult == DialogResult.OK);
				securePassword = authDlg.AdministrativeSecureHash;
			}

			if (openConfigDialog)
			{
				new SiteManagerConfigDialog(tbHost.Text.Trim(), (short) ntbPort.Value, securePassword).ShowDialog();
			}
		}
	}
}