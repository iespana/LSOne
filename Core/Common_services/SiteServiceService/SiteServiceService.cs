using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Configurations;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Properties;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

using LSRetail.SiteService.IntegrationFrameworkBaseInterface;
using LSRetail.SiteService.SiteServiceInterface;
using LSRetail.SiteService.SiteServiceInterface.Enums;

namespace LSOne.Services
{
	public partial class SiteServiceService : ISiteServiceService
	{
		private ISiteService server;
		private UInt16 serverport;
		private string serveraddress;
		private NetTcpBinding binding;
		private ChannelFactory<ISiteService> storeServerFactory;
		private bool isClosed;
		private SiteServiceConfig siteServiceConfiguration;
		private object threadLock = new object();
		private SecureString httpsCertificateThumbnail;

		public SiteServiceConfig SiteServiceConfiguration
		{
			get
			{
				if (siteServiceConfiguration == null)
				{
					siteServiceConfiguration = SiteServiceConfig.Read();
				} 
			   return siteServiceConfiguration;
			}
			set { siteServiceConfiguration = value; }
		}

		public string SiteServiceKey
		{
			get
			{
				//When Test connection is being done then this value is null and the functions that use 
				//it then throw errors and none can use the Site Service
				if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PrivateHashKey"]))
				{
					return "";
				}

				//Configured for each service
				return ConfigurationManager.AppSettings["PrivateHashKey"];
			}
		}

		public virtual IErrorLog ErrorLog
		{
			set { }
		}

		public void Init(IConnectionManager entry)
		{

		}

		#region Connection management

		public SiteServiceService()
		{
			serveraddress = null;
			isClosed = true;

			StaffID = ""; // For SC this is empty, while POS can set this property through its assessors.
			TerminalID = ""; // For SC this is empty, while POS can set this property through its assessors.         

            binding = new NetTcpBinding
            {
				Security = { Mode = SecurityMode.None },
				ReliableSession = { InactivityTimeout = TimeSpan.MaxValue },
				ReceiveTimeout = TimeSpan.MaxValue,
            };
		}

		public virtual void SetAddress(SiteServiceProfile siteServiceProfile)
		{
            #pragma warning disable 0612, 0618
			SetAddress(siteServiceProfile.SiteServiceAddress, (UInt16)siteServiceProfile.SiteServicePortNumber);
            #pragma warning restore 0612, 0618

            var maxMessageSize = siteServiceProfile.MaxMessageSize;

            binding.MaxBufferSize = maxMessageSize;
            binding.MaxReceivedMessageSize = maxMessageSize;
            binding.MaxBufferPoolSize = maxMessageSize;
            binding.ReaderQuotas.MaxStringContentLength = maxMessageSize;

            var timeout = siteServiceProfile.Timeout;

            binding.SendTimeout = timeout > 0 ? new TimeSpan(0, 0, timeout) : TimeSpan.MaxValue;
        }

		[Obsolete("The other SetAddress function with SiteServiceProfile parameter should be used. Only in very rare cases can this function be used, and you need to make sure that the address/port is correct")]
		public virtual void SetAddress(string address, UInt16 port)
		{
			this.serveraddress = address;
			this.serverport = port;
		}

		[Obsolete("The other Connect function with SiteServiceProfile parameter should be used. Only in very rare cases can this function be used, and you need to make sure that the address/port is correct")]
		private void Connect(IConnectionManager entry, string address, UInt16 port)
		{
			if (string.IsNullOrEmpty(address))
			{
				throw new Exception(Resources.AddressToConnectToIsNotSet);
			}
		   
			if (isClosed == false)
			{
				Disconnect(entry);
			}

			try
			{
				storeServerFactory = new ChannelFactory<ISiteService>(binding, new EndpointAddress("net.tcp://" + address + ":" + port + "/" + SiteServiceConstants.EndPointName));
				
				if (entry != null)
				{
					entry.ErrorLogger.LogMessageToFile(LogMessageType.Trace, "Address:" + address, this.ToString());
					entry.ErrorLogger.LogMessageToFile(LogMessageType.Trace, "Port:" + port, this.ToString());
				}

				//Printer.PrintReceipt("Address: " + address + ", Port: " + port);

				#if DEBUG
				Debug.WriteLine("Address:" + address);
				Debug.WriteLine("Port:" + port);
				#endif

				server = storeServerFactory.CreateChannel();
				isClosed = false;
				storeServerFactory.Closed += Channel_Closed;
			}
			catch (Exception x)
			{
				throw new Exception(Resources.CouldNotConnectToSiteService, x);
			}
		}

		[Obsolete("The other Connect function with SiteServiceProfile parameter should be used. Only in very rare cases can this function be used, and you need to make sure that the SiteServiceAddress/port is correct on the entry")]
		public virtual void Connect(IConnectionManager entry)
		{
			SetAddress(entry.SiteServiceAddress, entry.SiteServicePortNumber);

			#pragma warning disable 0612, 0618
			Connect(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber);
			#pragma warning restore 0612, 0618

		}

		public virtual void Connect(IConnectionManager entry, SiteServiceProfile siteServiceProfile)
		{
			SetAddress(siteServiceProfile);
			if (string.IsNullOrEmpty(serveraddress))
			{
				serveraddress = entry.SiteServiceAddress;
			}
			if (serverport == 0)
			{
				serverport = entry.SiteServicePortNumber;
			}
			#pragma warning disable 0612, 0618
			Connect(entry, serveraddress, serverport);
			#pragma warning restore 0612, 0618

		}

		void Channel_Closed(object sender, EventArgs e)
		{
			isClosed = true;
			server = null;
		}

		public virtual void Disconnect(IConnectionManager entry)
		{
			try
			{
				if (storeServerFactory != null)
				{
					storeServerFactory.Close();
				}
				storeServerFactory = null;

				#if DEBUG
				Debug.WriteLine("Disconnect");
				#endif

				if (entry != null)
				{
					entry.ErrorLogger.LogMessageToFile(LogMessageType.Trace, "Disconnect", this.ToString());
				}
			}
			catch
			{
				// Do nothing here
			}
		}

		public virtual ConnectionEnum TestConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile)
		{
			return TestConnection(entry, siteServiceProfile.SiteServiceAddress, (UInt16)siteServiceProfile.SiteServicePortNumber);
		}

		public virtual ConnectionEnum TestConnection(IConnectionManager entry, string host, UInt16 port)
		{
			ConnectionEnum result;
			try
			{
				if (!isClosed)
				{
					Disconnect(entry);
				}


#pragma warning disable 0612, 0618
				Connect(entry, host, port);
#pragma warning restore 0612, 0618

				result = server.TestConnection(CreateLogonInfo(entry));
				Disconnect(entry);
			}
			catch(Exception exception)
			{
				if (!isClosed)
				{
					Disconnect(entry);
				}

				return exception is ClientTimeNotSynchronizedException ? ConnectionEnum.ClientTimeNotSynchronized : ConnectionEnum.ConnectionFailed;
			}

			return result; 
		}

		public virtual ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string additionalMessage = "")
		{
			return TestConnectionWithFeedback(entry, siteServiceProfile.SiteServiceAddress, (UInt16) siteServiceProfile.SiteServicePortNumber, additionalMessage);
		}

		public virtual ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, string host, UInt16 port, string additionalMessage = "")
		{
			var dialog = (IDialogService) entry.Service(ServiceType.DialogService);
			ConnectionEnum state = ConnectionEnum.Success;

			// Show a spinner so that the UI doesn't appear frozen to the user while the test is performed
			if(dialog != null)
            {
				Exception e;
				dialog.ShowSpinnerDialog(() => state = TestConnection(entry, host, port), Properties.Resources.SiteService, Properties.Resources.CheckingSiteServiceConnection, out e);

				if(e != null)
                {
					throw e;
                }
            }
			else
            {
				state = TestConnection(entry, host, port);
            }
			
			
			if (state != ConnectionEnum.Success)
			{
				if (dialog != null)
				{
					string message;

					if(state == ConnectionEnum.ClientTimeNotSynchronized)
					{
						message = Properties.Resources.ClientTimeNotSynchronizedMessage;
					}
					else
					{
						message = Resources.CouldNotConnectToSiteService + Environment.NewLine + string.Format("({0}:{1})", host, port);
					}

					dialog.ShowMessage(message +
						Environment.NewLine + additionalMessage,
						Properties.Resources.ConnectionFailure,
						MessageBoxButtons.OK, MessageDialogType.Attention);
				}
			}
			return state;
		}

		/// <summary>
		/// Validates an administrative password by returning an encrypted UNIX timestamp.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="administrativePassword"></param>
		/// <returns></returns>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		public virtual string AdministrativeLogin(IConnectionManager entry, 
												string host, ushort port,
												string administrativePassword)
		{
			try
			{
				if (!isClosed)
				{
					Disconnect(entry);
					isClosed = true;
				}

#pragma warning disable 0612, 0618
				Connect(entry, host, port);
#pragma warning restore 0612, 0618
				
				return server.AdministrativeLogin(administrativePassword);
			}
			catch (Exception)
			{
				isClosed = true;
				throw;
			}
			finally
			{
				Disconnect(entry);
			}
		}

		/// <summary>
		/// Returns the Site Service configurations from config file.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="administrativePassword">Authorization password set at install time for retrieving the settings.</param>
		/// <returns></returns>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="System.ServiceModel.FaultException">If timestamp is older than AdministrativeSessionTimeout (default 2 hours) returns <i>Administrative session expired</i></exception>
		public virtual Dictionary<string, string> LoadConfiguration(IConnectionManager entry, string host, ushort port, string administrativePassword)
		{
			try
			{
				if (!isClosed)
				{
					Disconnect(entry);
					isClosed = true;
				}

#pragma warning disable 0612, 0618
				Connect(entry, host, port);
#pragma warning restore 0612, 0618

				return server.LoadConfiguration(administrativePassword);
			}
			catch (Exception)
			{
				isClosed = true;
				throw;
			}
			finally
			{
				Disconnect(entry);
			}
		}

		/// <summary>
		/// Updates Site Service configuratiosn from config file.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="administrativePassword">Authorization password set at install time for saving the passed settings.</param>
		/// <param name="fileConfigurations">List of settings to be saved in Site Service config file.</param>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="System.ServiceModel.FaultException">If timestamp is older than AdministrativeSessionTimeout (default 2 hours) returns <i>Administrative session expired</i></exception>
		public virtual void SendConfiguration(IConnectionManager entry, string host, ushort port, string administrativePassword, Dictionary<string, string> fileConfigurations)
		{
			try
			{
				if (!isClosed)
				{
					Disconnect(entry);
					isClosed = true;
				}


#pragma warning disable 0612, 0618
				Connect(entry, host, port);
#pragma warning restore 0612, 0618

				server.SendConfiguration(administrativePassword, fileConfigurations);
				Disconnect(entry);
			}
			catch (Exception)
			{
				isClosed = true;
				Disconnect(entry);
				throw;
			}
		}

		#endregion

		#region properties
		
		public virtual RecordIdentifier TerminalID { get; set; }

		public virtual RecordIdentifier StaffID { get; set; }

		#endregion

		private byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private LogonInfo CreateLogonInfo(IConnectionManager entry)
		{
			byte[] result;
			long tick = DateTime.UtcNow.Ticks;
			using (HMACSHA256 hmac = new HMACSHA256(GetBytes(SiteServiceKey)))
			{
				result = hmac.ComputeHash(GetBytes(SiteServiceConfiguration.SiteServiceSettings.PassCode + tick));

			}
			DateTime serverUTCDate = server.GetServerUTCDate();

			if (serverUTCDate > DateTime.UtcNow.AddMinutes(2))
			{
				throw new ClientTimeNotSynchronizedException(Resources.ClientTimeNotSynchronized);
			}

			var logonInfo = new LogonInfo
				{
					UserID = entry.CurrentUser.ID,
					storeId = (string) entry.CurrentStoreID,
					StaffID = entry.CurrentStaffID == RecordIdentifier.Empty ? "" : (string) entry.CurrentStaffID,
					terminalId = entry.CurrentTerminalID == RecordIdentifier.Empty ? "" : (string) entry.CurrentTerminalID,
					Settings = ((User)entry.CurrentUser).Profile.Settings,
					ClientID =  SiteServiceConfiguration.SiteServiceSettings.ClientID,
					Hash = result,
					Ticks = tick
				};

			return logonInfo;
		}

        /// <summary>
        /// Create a LogonInfo with the specified SiteService channel
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceServer">SiteService channel connection</param>
        /// <returns></returns>
        private LogonInfo CreateLogonInfo(IConnectionManager entry, ISiteService siteServiceServer)
        {
            byte[] result;
            long tick = DateTime.UtcNow.Ticks;
            using (HMACSHA256 hmac = new HMACSHA256(GetBytes(SiteServiceKey)))
            {
                result = hmac.ComputeHash(GetBytes(SiteServiceConfiguration.SiteServiceSettings.PassCode + tick));

            }
            DateTime serverUTCDate = siteServiceServer.GetServerUTCDate();

            if (serverUTCDate > DateTime.UtcNow.AddMinutes(2))
            {
                throw new Exception(Resources.ClientTimeNotSynchronized);
            }

            var logonInfo = new LogonInfo
            {
                UserID = entry.CurrentUser.ID,
                storeId = (string)entry.CurrentStoreID,
                StaffID = entry.CurrentStaffID == RecordIdentifier.Empty ? "" : (string)entry.CurrentStaffID,
                terminalId = entry.CurrentTerminalID == RecordIdentifier.Empty ? "" : (string)entry.CurrentTerminalID,
                Settings = ((User)entry.CurrentUser).Profile.Settings,
                ClientID = SiteServiceConfiguration.SiteServiceSettings.ClientID,
                Hash = result,
                Ticks = tick
            };

            return logonInfo;
        }

        public virtual void GetTransaction(IConnectionManager entry, ref bool retVal, ref string comment, ref bool uniqueReceiptId, ref DataTable transHeader, ref DataTable transItems, ref DataTable transPayments, string receiptId) 
		{
		
		}
		
		public virtual string GetExceptionDisplayText(Exception e)
		{
			if (e is FaultException)
			{
				string[] parts = e.Message.Split(':');

				int code = -1;
				if (parts.Length > 0)
					int.TryParse(parts[0], out code);

				switch (code)
				{
					case 1:
						return Resources.FailureInStoreServerConnection + Resources.UserAuthenticationFailed;

					case 2:
						return Resources.FailureInStoreServerConnection + Resources.UserLockedOut;

					case 3:
						return Resources.FailureInStoreServerConnection + Resources.UnknownServerError;

					case 4:
						return Resources.FailureInStoreServerConnection + Resources.CouldNotConnectToDatabase;
						
					case 5:
						return Resources.FailureInStoreServerConnection + Resources.UserNotServerUser;

					default:
						return Resources.FailureInStoreServerConnection + e.Message;
				}
			}
			return Resources.FailureInStoreServerConnection + e.Message;
		}

		private void DoRemoteWork(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Action func, bool closeConnection)
		{
			lock (threadLock)
			{
				try
				{
					if (isClosed)
					{
						Connect(entry, siteServiceProfile);
					}

					func();

					if (closeConnection)
					{
						Disconnect(entry);
					}
				}
				catch (Exception)
				{
					isClosed = true;
					Disconnect(entry);
					throw;
				}
			}
		}

		private T DoRemoteWork<T>(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Func<T> func, bool closeConnection)
		{
			lock (threadLock)
			{
				try
				{
					if (isClosed)
					{
						Connect(entry, siteServiceProfile);
					}

					return func();
				}
				catch (Exception)
				{
					isClosed = true;
					Disconnect(entry);
					throw;
				}
				finally
				{
					if (closeConnection)
					{
						Disconnect(entry);
					}
				}
			}
		}

        public virtual void RegisterClient(IConnectionManager entry, string userName, string password, string dbname)
		{
			try
			{
				if (!isClosed)
				{
					Disconnect(entry);
				}

				const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
				string passCode = "";
				Random random = new Random();
				for (int i = 0; i < 8; i++)
				{
					passCode += characters[random.Next(characters.Length)];
				}

				Guid clientID = Guid.NewGuid();

#pragma warning disable 0612, 0618
				Connect(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber);
#pragma warning restore 0612, 0618

				DateTime serverUTCDate = server.GetServerUTCDate();

				if (serverUTCDate > DateTime.UtcNow.AddMinutes(2) || serverUTCDate < DateTime.UtcNow.AddMinutes(-2))
				{
					throw new ServerTimeException(serverUTCDate.Ticks, DateTime.UtcNow.Ticks);
				}

				long ticks = DateTime.UtcNow.Ticks;

				byte[] hash;
				using (
					HMACSHA256 hmac = new HMACSHA256(GetBytes(SiteServiceKey))
					)
				{
					hash = hmac.ComputeHash(GetBytes(password + ticks));

				}
				string passCodeHash = Cipher.Encrypt(passCode + "-" + ticks, SiteServiceKey);
				SiteServiceConfiguration.SiteServiceSettings.UserGuid  = server.RegisterClient(userName, hash, dbname, clientID, passCodeHash, ticks);
				if (SiteServiceConfiguration.SiteServiceSettings.UserGuid == Guid.Empty)
				{
					throw new Exception("Not authenticated");
				}
				SiteServiceConfiguration.SiteServiceSettings.PassCode = passCode;
				SiteServiceConfiguration.SiteServiceSettings.ClientID = clientID;
				SiteServiceConfiguration.SiteServiceSettings.Login = userName;
				SiteServiceConfiguration.Save();
				Disconnect(entry);
			}
			catch (Exception)
			{
				if (!isClosed)
				{
					Disconnect(entry);
				}

				throw ;
			}

			return; 
		}

		public virtual DateTime GetServerUTCDate(IConnectionManager entry, SiteServiceProfile siteServiceProfile)
		{
			Connect(entry, siteServiceProfile);
			return server.GetServerUTCDate();
		}

		public virtual List<ScriptInfo> GetAvailableDefaultData(IConnectionManager entry, SiteServiceProfile siteServiceProfile)
		{
			Connect(entry, siteServiceProfile);
			return server.GetDemoDataTypes();
		}

		public virtual Guid RunDemoData(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ScriptInfo DemoDataName)
		{
			Connect(entry, siteServiceProfile);
			return server.RunDemoData(DemoDataName, CreateLogonInfo(entry));
		}

		public virtual bool IsTaskActive(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid taskGuid)
		{
			Connect(entry, siteServiceProfile);
			return server.IsTaskActive(taskGuid);
		}

		public virtual bool ClientRegistered()
		{
			return SiteServiceConfig.ConfigExists();
		}

		public virtual bool MarkAsActivated(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID)
		{

#pragma warning disable 0612, 0618
			Connect(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber);
#pragma warning restore 0612, 0618

			var result =  server.MarkAsActivated(terminalID, storeID, CreateLogonInfo(entry));
			if (result != ActivationResultEnum.Success)
			{
				return false;
			}
			return true;
		}

		public virtual void SetHardwareProfile(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, RecordIdentifier storeID, DataLayer.BusinessObjects.Profiles.HardwareProfile profile)
		{
			Connect(entry, siteServiceProfile);
			server.SetHardwareProfile(terminalID,storeID,profile,CreateLogonInfo(entry));
		}

		public virtual void SetTerminalEFT(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, RecordIdentifier storeID,
			string ipAddress, string eftStoreID, string eftTerminalID, string efTcustomField1, string efTcustomField2)
		{
			Connect(entry, siteServiceProfile);
			server.SetEFTForTerminal(terminalID, storeID,ipAddress, eftStoreID, eftTerminalID, efTcustomField1, efTcustomField2, CreateLogonInfo(entry));
		}

		public virtual void NotifyPlugin(IConnectionManager entry, SiteServiceProfile siteServiceProfile, MessageEventArgs e)
		{
			DoRemoteWork(entry, siteServiceProfile, () => server.NotifyPlugin(e), true);
		}

		#region Integration Framework

		public virtual ServiceConnectionStatus TestIntegrationFrameworkConnection(IConnectionManager entry, bool checkTcp, bool checkHttp, WebserviceConfiguration config)
		{
			if (string.IsNullOrEmpty(config.Host))
			{
				throw new Exception(Resources.AddressToConnectToIsNotSet);
			}

			if (!checkTcp && !checkHttp)
			{
				throw new Exception(Resources.IFConnectionTypeNotSet);
			}

			ServiceConnectionStatus status = new ServiceConnectionStatus();

			if (checkTcp)
			{
				try
				{
					ChannelFactory<IIntegrationFrameworkBaseService> tcpServerFactory = 
						new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFNetTcpBinding(config), new EndpointAddress("net.tcp://" + config.Host + ":" + config.TcpPort + "/IntegrationFrameworkBaseService"));
					IIntegrationFrameworkBaseService ifBaseService = tcpServerFactory.CreateChannel();

					status.NetTcpConnectionSuccesfull = ifBaseService.IsAlive();

					tcpServerFactory.Close();
					tcpServerFactory = null;
					ifBaseService = null;
				}
				catch (Exception)
				{
					status.NetTcpConnectionSuccesfull = false;
				}
			}

			if (checkHttp)
			{
				try
				{
					ChannelFactory<IIntegrationFrameworkBaseService> httpServerFactory = 
						new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFHttpBinding(config), new EndpointAddress((config.UseHttps ? "https://" : "http://") + config.Host + ":" + config.HttpPort + "/IntegrationFrameworkBaseService"));
					IIntegrationFrameworkBaseService ifBaseService = httpServerFactory.CreateChannel();

					status.HttpConnectionSuccesfull = ifBaseService.IsAlive();

					httpServerFactory.Close();
					httpServerFactory = null;
					ifBaseService = null;
				}
				catch (Exception)
				{
					status.HttpConnectionSuccesfull = false;
				}
			}

			return status;
		}

		public virtual Dictionary<string, string> LoadIFConfiguration(IConnectionManager entry, WebserviceConfiguration config)
		{
			ChannelFactory<IIntegrationFrameworkBaseService> serverFactory = null;
			IIntegrationFrameworkBaseService ifBaseService = null;
			Dictionary<string, string> configurations = null;
			bool configurationsFound = false;

			try
			{
				serverFactory = new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFNetTcpBinding(config), new EndpointAddress("net.tcp://" + config.Host + ":" + config.TcpPort + "/IntegrationFrameworkBaseService"));
				ifBaseService = serverFactory.CreateChannel();
				configurations = ifBaseService.LoadConfiguration();
				configurationsFound = true;
			}
			catch (Exception) { }

			if(!configurationsFound)
			{
				try
				{
					serverFactory = new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFHttpBinding(config), new EndpointAddress((config.UseHttps ? "https://" : "http://") + config.Host + ":" + config.HttpPort + "/IntegrationFrameworkBaseService"));
					ifBaseService = serverFactory.CreateChannel();
					configurations = ifBaseService.LoadConfiguration();
					configurationsFound = true;
				}
				catch (Exception) { }
			}

			if(serverFactory != null)
			{
				serverFactory.Close();
				serverFactory = null;
			}

			ifBaseService = null;

			if(!configurationsFound)
			{
				throw new Exception(Properties.Resources.LoadIFConfigurationError);
			}

			return configurations;
		}

		public virtual void SendIFConfiguration(IConnectionManager entry, WebserviceConfiguration config, Dictionary<string, string> configurations)
		{
			ChannelFactory<IIntegrationFrameworkBaseService> serverFactory = null;
			IIntegrationFrameworkBaseService ifBaseService = null;
			bool configurationsSent = false;

			try
			{
				serverFactory = new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFNetTcpBinding(config), new EndpointAddress("net.tcp://" + config.Host + ":" + config.TcpPort + "/IntegrationFrameworkBaseService"));
				ifBaseService = serverFactory.CreateChannel();
				ifBaseService.SendConfiguration(configurations);
				configurationsSent = true;
			}
			catch (Exception)
			{

			}

			if (!configurationsSent)
			{
				try
				{
					serverFactory = new ChannelFactory<IIntegrationFrameworkBaseService>(GetIFHttpBinding(config), new EndpointAddress((config.UseHttps ? "https://" : "http://") + config.Host + ":" + config.HttpPort + "/IntegrationFrameworkBaseService"));
					ifBaseService = serverFactory.CreateChannel();
					ifBaseService.SendConfiguration(configurations);
					configurationsSent = true;
				}
				catch (Exception)
				{

				}
			}

			if (serverFactory != null)
			{
				serverFactory.Close();
				serverFactory = null;
			}

			ifBaseService = null;

			if(!configurationsSent)
			{
				throw new Exception(Properties.Resources.ConnectionFailure);
			}
		}

		private NetTcpBinding GetIFNetTcpBinding(WebserviceConfiguration config)
		{
			return new NetTcpBinding
			{
				MaxBufferSize = 55000000,
				MaxReceivedMessageSize = 55000000,
				SendTimeout = new TimeSpan(0, 0, 60),
				ReaderQuotas = { MaxStringContentLength = 55000000 },
				Security = { Mode = SecurityMode.None },
				ReliableSession = { InactivityTimeout = TimeSpan.MaxValue },
				ReceiveTimeout = TimeSpan.MaxValue,
				PortSharingEnabled = config.UseHttps
			};
		}

		private WSHttpBinding GetIFHttpBinding(WebserviceConfiguration config)
		{
			WSHttpBinding httpBinding = new WSHttpBinding
			{
				MaxReceivedMessageSize = 55000000,
				ReaderQuotas = { MaxStringContentLength = 55000000 },
				ReceiveTimeout = TimeSpan.MaxValue,
				SendTimeout = new TimeSpan(0, 0, 60),
				ReliableSession = { InactivityTimeout = TimeSpan.MaxValue }
			};
			httpBinding.Security.Mode = config.UseHttps ? SecurityMode.Transport : SecurityMode.None;
			httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			if (config.UseHttps)
			{
				httpsCertificateThumbnail = config.SSLThumbnail;
#pragma warning disable SCS0004 // Certificate Validation has been disabled
#pragma warning disable SG0004 // Certificate Validation has been disabled
				ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
#pragma warning restore SG0004 // Certificate Validation has been disabled
#pragma warning restore SCS0004 // Certificate Validation has been disabled
			}

			return httpBinding;
		}

		private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;   // already determined to be valid
			}

			if(certificate.GetCertHashString() == SecureStringHelper.ToString(httpsCertificateThumbnail).ToUpper())
			{
				return true;
			}

			return false;
		}        

        #endregion Integration Framework
    }
}