using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.ServiceModel;

using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.Cryptography;

using LSRetail.SiteService.SiteServiceInterface;

namespace LSOne.SiteService.Plugins.SiteManager
{
	public partial class SiteManagerPlugin
	{
		/// <summary>
		/// Encryption key for the Site Service configurations.
		/// </summary>
		private const string ConfigurationCryptoKey = "DFB8EFC8-1584-49DD-A9B1-EC01A3FBFF9F";

		/// <summary>
		/// Delimiter for the administrative password that also contains the timestamp.
		/// </summary>
		private const string AdministrativePasswordDelimiter = "###";

		/// <summary>
		/// Validates an administrative password by returning an encrypted UNIX timestamp.
		/// </summary>
		/// <param name="administrativePassword"></param>
		/// <exception cref="FaultException">If <param name="administrativePassword"></param> is null or empty string or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <returns></returns>
		public string AdministrativeLogin(string administrativePassword)
		{
			if (isCloudInstallation)
			{
				return string.Empty;
			}
			
			ValidateAdministrativePassword(administrativePassword, withTimestamp: false, 
							 defaultPassword: configurations[SiteServiceConfigurationConstants.Administration],
							 defaultTimeout: configurations[SiteServiceConfigurationConstants.AdministrationTimeout]);
			long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			return Cipher.Encrypt(timestamp.ToString(), ConfigurationCryptoKey);
		}

		/// <summary>
		/// Returns the Site Service configurations from configuration file.
		/// </summary>
		/// <param name="administrativePassword">Authorization password set at install time for retrieving the settings.</param>
		/// <exception cref="FaultException">If <param name="administrativePassword"></param> is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="FaultException">If timestamp is older than <see cref="AdministrativeSessionTimeout">default 2 hours</see> returns <i>Administrative session expired</i></exception>
		/// <returns></returns>
		public Dictionary<string, string> LoadConfiguration(string administrativePassword)
		{
			if (isCloudInstallation || configurations == null)
			{
				return null;
			}
			
			ValidateAdministrativePassword(administrativePassword, withTimestamp: true, 
							defaultPassword: configurations[SiteServiceConfigurationConstants.Administration],
							defaultTimeout: configurations[SiteServiceConfigurationConstants.AdministrationTimeout]);

			Dictionary<string, string> encryptedConfigurations = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> configuration in configurations)
			{
				//skip administrative keys so they're not overwritten
				if (string.Compare(configuration.Key, SiteServiceConfigurationConstants.Administration, StringComparison.InvariantCultureIgnoreCase) == 0
				    || string.Compare(configuration.Key, SiteServiceConfigurationConstants.AdministrationTimeout, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					continue;
				}

				encryptedConfigurations.Add(configuration.Key, Cipher.Encrypt(configuration.Value, ConfigurationCryptoKey));
			}

			return encryptedConfigurations;
		}

		/// <summary>
		/// Updates Site Service configurations from configuration file.
		/// </summary>
		/// <param name="administrativePassword">Authorization password set at install time for saving the passed settings.</param>
		/// <param name="fileConfigurations">List of settings to be saved in Site Service configuration file.</param>
		/// <returns></returns>
		/// <exception cref="FaultException">If <param name="administrativePassword"></param> is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="FaultException">If timestamp is older than <see cref="AdministrativeSessionTimeout">default 2 hours</see> returns <i>Administrative session expired</i></exception>
		public void SendConfiguration(string administrativePassword, Dictionary<string, string> fileConfigurations)
		{
			if (isCloudInstallation)
			{
				return;
			}
			
			ValidateAdministrativePassword(administrativePassword, withTimestamp: true, 
						defaultPassword: configurations[SiteServiceConfigurationConstants.Administration],
						defaultTimeout: configurations[SiteServiceConfigurationConstants.AdministrationTimeout]);

			if (fileConfigurations != null)
			{
				Dictionary<string, string> decryptedConfigurations = new Dictionary<string, string>();
				foreach (KeyValuePair<string, string> configuration in fileConfigurations)
				{
					//skip administrative keys so they're not overwritten
					if (string.Compare(configuration.Key, SiteServiceConfigurationConstants.Administration, StringComparison.InvariantCultureIgnoreCase) == 0
					    || string.Compare(configuration.Key, SiteServiceConfigurationConstants.AdministrationTimeout, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						continue;
					}

					decryptedConfigurations.Add(configuration.Key, Cipher.Decrypt(configuration.Value, ConfigurationCryptoKey));
				}
				decryptedConfigurations.Add(SiteServiceConfigurationConstants.Administration, configurations[SiteServiceConfigurationConstants.Administration]);
				decryptedConfigurations.Add(SiteServiceConfigurationConstants.AdministrationTimeout, configurations[SiteServiceConfigurationConstants.AdministrationTimeout]);

				var configWriter = new ConfigurationWriter();
				WriteConfigurations(decryptedConfigurations, configWriter);
				ConfigurationFile.SaveConfigFile(configWriter.XmlPortion);

				Utils.Log(this, "Configuration file saved", LogLevel.Trace);
			}
			else
			{
				Utils.Log(this, "Configuration file not saved because no configuration list was provided.", LogLevel.Trace);
			}

		}

		public void ReloadConfigurations(Dictionary<string, string> configurations)
		{
			if (isCloudInstallation)
			{
				return;
			}
			if (wcfHost != null)
			{
				Unload();
			}
			Load(configurations);
		}

		public virtual bool VerifyConfigurations(Dictionary<string, string> configurations)
		{
			if (isCloudInstallation)
			{
				return false;
			}
			bool dirty = false;

			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.ExternalAddress, "");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabaseServer, "localhost");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabaseWindowsAuthentication, "false");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabaseUser, "sa");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabasePassword, "mydatabasepassword");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabaseName, "mydatabase");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DatabaseConnectionType, "Named pipes");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.SiteServicePluginOverride, "");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DaysToKeepLogs, "30");

			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.PrivateHashKey, ConfigurationManager.AppSettings["PrivateHashKey"] ?? string.Empty);
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.MaxCount, "50");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.MaxUserConnectionCount, "30");

			// Support for older names
			if (configurations.ContainsKey("StoreControllerUser"))
			{
				configurations.Add("SiteManagerUser", configurations["StoreControllerUser"]);
				dirty = true;
			}
			if (configurations.ContainsKey("StoreControllerPassword"))
			{
				configurations.Add("SiteManagerPassword", configurations["StoreControllerPassword"]);
				dirty = true;
			}

			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.SiteManagerUser, "SiteServiceUser");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.SiteManagerPassword, "1234");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.DataAreaID, "LSR");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.Port, "9101");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.EMailSendInterval, "0");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.EMailMaximumBatch, "25");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.EMailMaximumAttempts, "5");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.EMailTruncateQueue, "Each");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.IsCloud, "false");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.NetTcpTimeout, "60");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.Administration, "");
			dirty |= VerifyEntry(configurations, SiteServiceConfigurationConstants.AdministrationTimeout, "0");

			return dirty;
		}

		public virtual void WriteConfigurations(Dictionary<string, string> configurations, ISiteServiceSettings settings)
		{
			if (isCloudInstallation)
			{
				return;
			}
			settings.WriteEmptyLine();
			settings.WriteKey(SiteServiceConfigurationConstants.ExternalAddress, configurations[SiteServiceConfigurationConstants.ExternalAddress]);
			settings.WriteKey(SiteServiceConfigurationConstants.DatabaseServer, configurations[SiteServiceConfigurationConstants.DatabaseServer]);
			settings.WriteKey(SiteServiceConfigurationConstants.DatabaseWindowsAuthentication, configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication]);
			settings.WriteKey(SiteServiceConfigurationConstants.DatabaseUser, configurations[SiteServiceConfigurationConstants.DatabaseUser]);
			settings.WriteKey(SiteServiceConfigurationConstants.DatabasePassword, configurations[SiteServiceConfigurationConstants.DatabasePassword]);
			settings.WriteKey(SiteServiceConfigurationConstants.DatabaseName, configurations[SiteServiceConfigurationConstants.DatabaseName]);
			settings.WriteKey(SiteServiceConfigurationConstants.SiteServicePluginOverride, configurations[SiteServiceConfigurationConstants.SiteServicePluginOverride]);
			settings.WriteKey(SiteServiceConfigurationConstants.DaysToKeepLogs, configurations[SiteServiceConfigurationConstants.DaysToKeepLogs]);
			settings.WriteEmptyLine();

			settings.WriteComment("Security");
			settings.WriteKey(SiteServiceConfigurationConstants.PrivateHashKey, configurations[SiteServiceConfigurationConstants.PrivateHashKey] ?? string.Empty);
			settings.WriteKey(SiteServiceConfigurationConstants.MaxCount, configurations[SiteServiceConfigurationConstants.MaxCount]);
			settings.WriteKey(SiteServiceConfigurationConstants.MaxUserConnectionCount, configurations[SiteServiceConfigurationConstants.MaxUserConnectionCount]);
			settings.WriteEmptyLine();

			settings.WriteComment("DatabaseConnectionType can be one of the following: TCP/IP,Named pipes, Shared memory");
			settings.WriteEmptyLine();
			settings.WriteKey(SiteServiceConfigurationConstants.DatabaseConnectionType, configurations[SiteServiceConfigurationConstants.DatabaseConnectionType]);
			settings.WriteEmptyLine();

			settings.WriteComment("SiteManagerUser has to be created as a special server user on the Site Manager - normal user will not work");
			settings.WriteKey(SiteServiceConfigurationConstants.SiteManagerUser, configurations[SiteServiceConfigurationConstants.SiteManagerUser]);
			settings.WriteKey(SiteServiceConfigurationConstants.SiteManagerPassword, configurations[SiteServiceConfigurationConstants.SiteManagerPassword]);
			settings.WriteKey(SiteServiceConfigurationConstants.DataAreaID, configurations[SiteServiceConfigurationConstants.DataAreaID]);
			settings.WriteKey(SiteServiceConfigurationConstants.Port, configurations[SiteServiceConfigurationConstants.Port]);

			settings.WriteComment("E-mail settings");
			settings.WriteKey(SiteServiceConfigurationConstants.EMailSendInterval, configurations[SiteServiceConfigurationConstants.EMailSendInterval]);
			settings.WriteKey(SiteServiceConfigurationConstants.EMailMaximumBatch, configurations[SiteServiceConfigurationConstants.EMailMaximumBatch]);
			settings.WriteKey(SiteServiceConfigurationConstants.EMailMaximumAttempts, configurations[SiteServiceConfigurationConstants.EMailMaximumAttempts]);
			settings.WriteKey(SiteServiceConfigurationConstants.EMailTruncateQueue, configurations[SiteServiceConfigurationConstants.EMailTruncateQueue]);

			if (configurations.ContainsKey(SiteServiceConfigurationConstants.IsCloud))
			{
				settings.WriteComment("HBO Settings");
				settings.WriteKey(SiteServiceConfigurationConstants.IsCloud,
					configurations[SiteServiceConfigurationConstants.IsCloud]);
			}

			settings.WriteComment("SiteService NetTcp connection timeout (in seconds)");
			settings.WriteKey(SiteServiceConfigurationConstants.NetTcpTimeout, configurations[SiteServiceConfigurationConstants.NetTcpTimeout]);

			settings.WriteComment("SiteService administrative section");
			settings.WriteComment("** SiteService administrative password. This is configured at install time");
			settings.WriteKey(SiteServiceConfigurationConstants.Administration, 
							configurations.ContainsKey(SiteServiceConfigurationConstants.Administration)
								? configurations[SiteServiceConfigurationConstants.Administration]
								: string.Empty);
			settings.WriteComment("** SiteService administrative session timeout (in seconds). Default value is 0 (off).");
			settings.WriteKey(SiteServiceConfigurationConstants.AdministrationTimeout, 
				configurations.ContainsKey(SiteServiceConfigurationConstants.AdministrationTimeout)
					? configurations[SiteServiceConfigurationConstants.AdministrationTimeout]
					: "0");

			Utils.Log(this, "Settings have been created", LogLevel.Trace);

			UpdateEMailSchedule(configurations);
			UpdateClearLogSchedule(Convert.ToInt32(configurations[SiteServiceConfigurationConstants.DaysToKeepLogs]));
            UpdateOmniJournalSchedule();
			
			Utils.Log(this, "UpdateEmailSchedule done", LogLevel.Trace);
		}

		private static bool VerifyEntry(Dictionary<string, string> configurations, string entry, string defaultValue)
		{
			if (isCloudInstallation)
			{
				return false;
			}
			if (!configurations.ContainsKey(entry))
			{
				configurations.Add(entry, defaultValue);
				return true;
			}

			// Nothing added
			return false;
		}

		private ConnectionType ResolveConnectionTypeFromName(string connectionTypeName)
		{
			ConnectionType connectionType;

			if (connectionTypeName == "TCP/IP")
			{
				connectionType = ConnectionType.TCP_IP;
			}
			else if (connectionTypeName == "Named pipes")
			{
				connectionType = ConnectionType.NamedPipes;
			}
			else
			{
				connectionType = ConnectionType.SharedMemory;
			}

			return connectionType;
		}

		private void ResolveConfiguration(Dictionary<string, string> configurations)
		{
			ConnectionType connectionType = ResolveConnectionTypeFromName(configurations[SiteServiceConfigurationConstants.DatabaseConnectionType]);

			try
			{
				parameters = new Tuple<string, bool, string, SecureString, string, string, SecureString, Tuple<ConnectionType, ConnectionUsageType, string>>(
					configurations[SiteServiceConfigurationConstants.DatabaseServer],
					configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication].Equals("true", StringComparison.InvariantCultureIgnoreCase),
					configurations[SiteServiceConfigurationConstants.DatabaseUser],
					SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.DatabasePassword]),
					configurations[SiteServiceConfigurationConstants.DatabaseName],
					configurations[SiteServiceConfigurationConstants.SiteManagerUser],
					SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.SiteManagerPassword]),
					new Tuple<ConnectionType, ConnectionUsageType, string>(
						connectionType,
						ConnectionUsageType.UsageService,
						configurations[SiteServiceConfigurationConstants.DataAreaID]));
			}
			catch (Exception)
			{
				parameters = new Tuple<string, bool, string, SecureString, string, string, SecureString, Tuple<ConnectionType, ConnectionUsageType, string>>(
					configurations[SiteServiceConfigurationConstants.DatabaseServer],
					configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication].Equals("true", StringComparison.InvariantCultureIgnoreCase),
					configurations[SiteServiceConfigurationConstants.DatabaseUser],
					SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.DatabasePassword]),
					configurations[SiteServiceConfigurationConstants.DatabaseName],
					configurations["StoreControllerUser"],
					SecureStringHelper.FromString(configurations["StoreControllerPassword"]),
					new Tuple<ConnectionType, ConnectionUsageType, string>(
						connectionType,
						ConnectionUsageType.UsageService,
						configurations[SiteServiceConfigurationConstants.DataAreaID]));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="administrativePassword"></param>
		/// <param name="withTimestamp">If true, the administrative password format is expected to be <i>encrypted hashed password###encrypted timestamp</i>.</param>
		/// <param name="defaultPassword">Site Service administrative password as set at install time.</param>
		/// <param name="defaultTimeout">Site Service administrative session timeout.</param>
		/// <exception cref="FaultException">If <param name="administrativePassword"></param> is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="FaultException">If timestamp is older than <see cref="AdministrativeSessionTimeout">default 2 hours</see> returns <i>Administrative session expired</i></exception>
		private void ValidateAdministrativePassword(string administrativePassword, bool withTimestamp, string defaultPassword, string defaultTimeout)
		{
			if (string.IsNullOrEmpty(administrativePassword)
				||(withTimestamp && !administrativePassword.Contains(AdministrativePasswordDelimiter)))
			{
				throw new FaultException(new FaultReason(Properties.Resources.AdministrativePasswordIncorrect));
			}

			

			string encryptedPassword = string.Empty;
			if (withTimestamp)
			{
				string[] shards    = administrativePassword.Split(new string[] {AdministrativePasswordDelimiter}, StringSplitOptions.None);
				encryptedPassword  = shards[0];

				string timestamp = Cipher.Decrypt(shards[1], ConfigurationCryptoKey);
				long previousSeconds;
				if (!long.TryParse(timestamp, out previousSeconds) || previousSeconds == 0)
				{
					throw new FaultException(new FaultReason(Properties.Resources.AdministrativePasswordIncorrect));
				}

				long timeout = long.Parse(defaultTimeout);
				if (timeout > 0 && DateTimeOffset.UtcNow.ToUnixTimeSeconds() - previousSeconds > timeout)
				{
					throw new FaultException(new FaultReason(Properties.Resources.AdministrativeSessionExpired));
				}
			}
			else
			{
				encryptedPassword = administrativePassword;
			}

			string hash = Cipher.Decrypt(encryptedPassword, ConfigurationCryptoKey);

			if (string.Compare(hash, defaultPassword, StringComparison.InvariantCulture) != 0)
			{
				throw new FaultException(new FaultReason(Properties.Resources.AdministrativePasswordIncorrect));
			}
		}
	}
}
