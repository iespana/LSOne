using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.ServiceModel;
using System.ServiceProcess;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SiteService.Properties;

using ServiceType = LSOne.DataLayer.GenericConnector.Enums.ServiceType;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
	public partial class SiteManagerConfigDialog : DialogBase
	{
		private readonly string host;
		private readonly short port;

		private readonly SiteServiceProfile siteServiceProfile;
		private string netTcpTimeoutConfiguration;

		/// <summary>
		/// Administrative secure password that is sent over to Site Service for configuration management purposes.
		/// </summary>
		private SecureString administrativeSecurePassword;

		protected SiteManagerConfigDialog()
		{
			InitializeComponent();
		}

		public SiteManagerConfigDialog( string host, short port, SecureString securePassword)
			: this()
		{
			this.host = host;
			this.port = port;
			administrativeSecurePassword = securePassword;

			siteServiceProfile = new SiteServiceProfile {SiteServiceAddress = host, SiteServicePortNumber = port};

			tbDbUser.Enabled = true;
			tbDbPwd.Enabled = true;
			xDbWinAuth.Checked = false;
			cbDbConType.SelectedIndex = 2;

			cmTruncate.Items.Clear();
			cmTruncate.Items.Add(Resources.TruncateEMailQueueEach);
			cmTruncate.Items.Add(Resources.TruncateEMailQueueDay);
			cmTruncate.Items.Add(Resources.TruncateEMailQueueWeek);
			cmTruncate.Items.Add(Resources.TruncateEMailQueueMonth);
			cmTruncate.Items.Add(Resources.TruncateEMailQueueNever);
		
			btnGetConfig_Click(this, EventArgs.Empty);
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void CheckEnabled(object sender, EventArgs e)
		{
			btnSmtpSettings.Enabled =
			btnSendConfig.Enabled = !string.IsNullOrWhiteSpace(tbDbServer.Text)
									&& !string.IsNullOrWhiteSpace(tbDbDatabase.Text)
									&&
									(xDbWinAuth.Checked ||
									 (
										 !string.IsNullOrWhiteSpace(tbDbUser.Text) &&
										 !string.IsNullOrWhiteSpace(tbDbPwd.Text)
									 )
									)
									&& !string.IsNullOrWhiteSpace(tbAreaid.Text)
				;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnGetConfig_Click(object sender, EventArgs e)
		{
			ShowProgress(
				(sender1, e1) =>
					PluginOperations.AdministrateSiteService(
						GetConfiguration, 
						() => { HideProgress(); },
						() =>
						{
							HideProgress(); 
							Close();
						}
					)
				, Resources.GettingConfig);
		}

		private void btnSendConfig_Click(object sender, EventArgs e)
		{
			ShowProgress((sender1, e1) => 
				PluginOperations.AdministrateSiteService(
					() =>
								{
									SendConfiguration();
									RestartService();
								},
					() => { HideProgress(); },
					() =>
					{
						HideProgress();
						Close();
					}
				)
				, Resources.SiteServiceConfigSending);
		}

		private void xDbWinAuth_CheckedChanged(object sender, EventArgs e)
		{
			tbDbUser.Enabled = !xDbWinAuth.Checked;
			tbDbPwd.Enabled = !xDbWinAuth.Checked;
			
			CheckEnabled(this, EventArgs.Empty);
		}

		private void btnSmtpSettings_Click(object sender, EventArgs e)
		{
			try
			{
				var storeID = new RecordIdentifier();
				var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
				var setting = service.GetEMailSetupForStore(PluginEntry.DataModel, siteServiceProfile, storeID, true);
				if (setting == null)
				{
					setting = new EMailSetting {StoreID = storeID};
				}

				var dlg = new SMTPSettingsDialog(setting);
				if (DialogResult.OK == dlg.ShowDialog())
				{
					service.SaveEMailSetupForStore(PluginEntry.DataModel, siteServiceProfile, setting, true);
				}
			}
			catch (Exception ex)
			{
				MessageDialog.Show(ex.Message);
			}
		}

		/// <summary>
		/// Heavy task handler that retrieves Site Service configuration;
		/// </summary>
		private void GetConfiguration()
		{
			var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

			var configurations =
				service.LoadConfiguration(PluginEntry.DataModel, 
										  host, (ushort) port, 
										  SecureStringHelper.ToString(administrativeSecurePassword));

			tbDbServer.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DatabaseServer],
				PluginEntry.ConfigurationCryptoKey);
			xDbWinAuth.Checked = bool.Parse(Cipher.Decrypt(
				configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication],
				PluginEntry.ConfigurationCryptoKey));
			tbDbUser.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DatabaseUser],
				PluginEntry.ConfigurationCryptoKey);
			tbDbPwd.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DatabasePassword],
				PluginEntry.ConfigurationCryptoKey);
			tbDbDatabase.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DatabaseName],
				PluginEntry.ConfigurationCryptoKey);
			cbDbConType.SelectedIndex =
				(int)Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DatabaseConnectionType],
					PluginEntry.ConfigurationCryptoKey).ToConnectionType();
			tbAreaid.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DataAreaID],
				PluginEntry.ConfigurationCryptoKey);

			ntbSendInterval.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.EMailSendInterval],
					PluginEntry.ConfigurationCryptoKey);
			ntbMaximumEmails.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.EMailMaximumBatch],
					PluginEntry.ConfigurationCryptoKey);
			ntbMaximumAttempts.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.EMailMaximumAttempts],
					PluginEntry.ConfigurationCryptoKey);


			tbExternalAddress.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.ExternalAddress],
					PluginEntry.ConfigurationCryptoKey);
			tbPrivateHashKey.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.PrivateHashKey],
				PluginEntry.ConfigurationCryptoKey);
			ntbMaxCount.Text = Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.MaxCount],
				PluginEntry.ConfigurationCryptoKey);
			ntbMaxUserConnections.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.MaxUserConnectionCount],
					PluginEntry.ConfigurationCryptoKey);
			netTcpTimeoutConfiguration =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.NetTcpTimeout],
					PluginEntry.ConfigurationCryptoKey);
			txtServiceOverride.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.SiteServicePluginOverride],
					PluginEntry.ConfigurationCryptoKey);
			ntbDaysToKeepLogs.Text =
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.DaysToKeepLogs],
					PluginEntry.ConfigurationCryptoKey);

			EMailTruncateSetting truncate;
			if (!Enum.TryParse(
				Cipher.Decrypt(configurations[SiteServiceConfigurationConstants.EMailTruncateQueue],
					PluginEntry.ConfigurationCryptoKey), out truncate))
				truncate = EMailTruncateSetting.Each; // Each
			switch (truncate)
			{
				case EMailTruncateSetting.Daily:
					cmTruncate.SelectedItem = Resources.TruncateEMailQueueDay;
					break;
				case EMailTruncateSetting.Weekly:
					cmTruncate.SelectedItem = Resources.TruncateEMailQueueWeek;
					break;
				case EMailTruncateSetting.Monthly:
					cmTruncate.SelectedItem = Resources.TruncateEMailQueueMonth;
					break;
				case EMailTruncateSetting.Never:
					cmTruncate.SelectedItem = Resources.TruncateEMailQueueNever;
					break;
				default:
					cmTruncate.SelectedItem = Resources.TruncateEMailQueueEach;
					break;
			}
		}

		private void SendConfiguration()
		{
				var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
				var configurations = new Dictionary<string, string>();

				configurations.Add(SiteServiceConfigurationConstants.ExternalAddress,
					Cipher.Encrypt(tbExternalAddress.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.PrivateHashKey,
					Cipher.Encrypt(tbPrivateHashKey.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.MaxCount,
					Cipher.Encrypt(ntbMaxCount.Value.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.MaxUserConnectionCount,
					Cipher.Encrypt(ntbMaxUserConnections.Value.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabaseServer,
					Cipher.Encrypt(tbDbServer.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabaseWindowsAuthentication,
					Cipher.Encrypt(xDbWinAuth.Checked.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabaseUser,
					Cipher.Encrypt(tbDbUser.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabasePassword,
					Cipher.Encrypt(tbDbPwd.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabaseName,
					Cipher.Encrypt(tbDbDatabase.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DatabaseConnectionType,
					Cipher.Encrypt(((ConnectionType)cbDbConType.SelectedIndex).ToEnglishString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.SiteManagerUser,
					Cipher.Encrypt("SiteServiceUser", PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.SiteManagerPassword,
					Cipher.Encrypt("", PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DataAreaID,
					Cipher.Encrypt(tbAreaid.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.Port,
					Cipher.Encrypt(port.ToString(), PluginEntry.ConfigurationCryptoKey));

				configurations.Add(SiteServiceConfigurationConstants.EMailSendInterval,
					Cipher.Encrypt(ntbSendInterval.Value.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.EMailMaximumBatch,
					Cipher.Encrypt(ntbMaximumEmails.Value.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.EMailMaximumAttempts,
					Cipher.Encrypt(ntbMaximumAttempts.Value.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.IsCloud,
					Cipher.Encrypt(false.ToString(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.NetTcpTimeout,
					Cipher.Encrypt(netTcpTimeoutConfiguration, PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.SiteServicePluginOverride,
					Cipher.Encrypt(txtServiceOverride.Text.Trim(), PluginEntry.ConfigurationCryptoKey));
				configurations.Add(SiteServiceConfigurationConstants.DaysToKeepLogs,
					Cipher.Encrypt(ntbDaysToKeepLogs.Text.Trim(), PluginEntry.ConfigurationCryptoKey));

				string truncate;
				if (cmTruncate.SelectedItem.ToString() == Resources.TruncateEMailQueueDay)
					truncate = EMailTruncateSetting.Daily.ToString();
				else if (cmTruncate.SelectedItem.ToString() == Resources.TruncateEMailQueueWeek)
					truncate = EMailTruncateSetting.Weekly.ToString();
				else if (cmTruncate.SelectedItem.ToString() == Resources.TruncateEMailQueueMonth)
					truncate = EMailTruncateSetting.Monthly.ToString();
				else if (cmTruncate.SelectedItem.ToString() == Resources.TruncateEMailQueueNever)
					truncate = EMailTruncateSetting.Never.ToString();
				else
					truncate = EMailTruncateSetting.Each.ToString();
				configurations.Add(SiteServiceConfigurationConstants.EMailTruncateQueue,
								  Cipher.Encrypt(truncate, PluginEntry.ConfigurationCryptoKey));

				service.SendConfiguration(PluginEntry.DataModel, 
										  host, (ushort) port, 
										  SecureStringHelper.ToString(administrativeSecurePassword),
										  configurations);
		}

		private void RestartService() //(object sender, EventArgs e)
		{
			var timeout = TimeSpan.FromSeconds(60);
			var ctrl = new ServiceController("LSOneSiteService", host);
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
	}
}
