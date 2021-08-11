using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading;
using System.Xml;

using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.DatabaseUtil;
using LSOne.DataLayer.DatabaseUtil.Enums;
using LSOne.DataLayer.DatabaseUtil.Exceptions;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Plugins.SiteManager.DataLayer;
using LSOne.SiteService.Plugins.SiteManager.DataLayer.DataEntities;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;

using LSRetail.SiteService.SiteServiceInterface;

namespace LSOne.SiteService.Plugins.SiteManager
{
	internal class Credentials
	{
		public string Database { get; set; }
		public string PassCode { get; set; }
		public long Ticks { get; set; }
	}

	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
		InstanceContextMode = InstanceContextMode.Single)]
	public partial class SiteManagerPlugin : ISiteService, ISiteServicePlugin, IConfigurationWriterPlugin
	{
		private long lastTicks;
		private object tickLock;
		private Dictionary<Guid,string> activeTasks = new Dictionary<Guid, string>();
		private string createDatabasePassword = "12345";
		private Dictionary<string, int> existingDatabases;  

		private Dictionary<Guid, Credentials> CredentialsCache = new Dictionary<Guid, Credentials>();
		internal static SiteManagerPlugin Instance;

		private ConnectionPoolManager connectionPool;

		private static Tuple<
			string, // Database server
			bool, // Database windows authentication
			string, // Database user
			SecureString, // Database password
			string, // Database name
			string, // Site Manager user
			SecureString, // Site manager password
			Tuple<ConnectionType, ConnectionUsageType, string>> parameters;

		private static bool isCloudInstallation = false;
		private ServiceHost wcfHost;

		private Dictionary<string, string> configurations;

		public SiteManagerPlugin()
		{
			Instance = this;
			lastTicks = DateTime.UtcNow.Ticks;
			tickLock = new object();
			existingDatabases = new Dictionary<string, int>();
			connectionPool = new ConnectionPoolManager();
		}

		protected void ReturnConnection(IConnectionManager entry, out IConnectionManager externalEntry)
		{
			connectionPool.ReturnConnection(entry, Conversion.ToInt(configurations[SiteServiceConfigurationConstants.MaxCount]), Conversion.ToInt(configurations[SiteServiceConfigurationConstants.MaxUserConnectionCount]));
			externalEntry = null;
		}

		private string LoginDBConnection()
		{
			return DBConnection("LoginDB");
		}

		protected string DBConnection(string dbName)
		{
			string networkLibrary = string.Empty;

			switch (configurations[SiteServiceConfigurationConstants.DatabaseConnectionType])
			{
				case "TCP/IP":
					networkLibrary = "Network Library=DBMSSOCN;";
					break;
				case "Named pipes":
					networkLibrary = "Network Library=dbmslpcn;";
					break;
				case "Shared memory":
					networkLibrary = "Network Library=dbnmpntw;";
					break;
			}

			//we want it to fail if the DatabaseWindowsAuthentication setting is not a valid boolean string (like True, true, False, false)
			bool useWindowsAuth = bool.Parse(configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication]);
			if (useWindowsAuth)
			{
				return string.Format("Persist Security Info=False;{0}Initial Catalog={1};Packet Size=4096;Pooling=False;Connect Timeout=15;Data Source={2};Integrated Security = True;",
									networkLibrary,
									dbName,
									configurations[SiteServiceConfigurationConstants.DatabaseServer]);
			}
			else
			{
				return string.Format("Persist Security Info=False;{0}Initial Catalog={1};Packet Size=4096;Pooling=False;Connect Timeout=15;Data Source={2};User ID={3};Password={4};",
									networkLibrary,
									dbName,
									configurations[SiteServiceConfigurationConstants.DatabaseServer],
									configurations[SiteServiceConfigurationConstants.DatabaseUser],
									configurations[SiteServiceConfigurationConstants.DatabasePassword]);
			}
		}

		protected string VerifyCredentials(LogonInfo logonInfo)
		{
			Credentials credentials = CredentialsCache.ContainsKey(logonInfo.ClientID) 
											? CredentialsCache[logonInfo.ClientID] 
											: null;
			if (credentials == null)
			{
				string connectionString = LoginDBConnection();
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					using (SqlCommand credentialCommand = new SqlCommand())
					{
						credentialCommand.Connection = connection;
						credentialCommand.CommandText = @"SELECT 
															DBName,
															Passcode,
															Ticks 
															FROM 
																CredentialEntries 
															WHERE 
																ClientID = @ClientID";

						SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.UniqueIdentifier);
						clientIDParameter.Value = logonInfo.ClientID;
						credentialCommand.Parameters.Add(clientIDParameter);

						connection.Open();
						
						var credentialReader = credentialCommand.ExecuteReader();
						if (credentialReader.Read())
						{
							credentials = new Credentials();
							if (credentialReader["DBName"] != DBNull.Value)
							{
								credentials.Database = (string) credentialReader["DBName"];
							}
							if (credentialReader["Passcode"] != DBNull.Value)
							{
								string passCode = Cipher.Decrypt((string)credentialReader["Passcode"], configurations[SiteServiceConfigurationConstants.PrivateHashKey]);
								if (passCode.Contains("-"))
								{
									passCode = passCode.Split('-')[0];
								}
								else
								{
									passCode = (string) credentialReader["Passcode"];
								}
								credentials.PassCode = passCode;
							}
							if (credentialReader["Ticks"] != DBNull.Value)
							{
								credentials.Ticks = (Int64) credentialReader["Ticks"];
							}
						}
						credentialReader.Close();
						credentialReader.Dispose();
						connection.Close();
					}
				}
			}
			if (credentials == null)
			{
				return null;
			}

			CheckTicksValid(logonInfo.Ticks, credentials.Ticks);

			byte[] result;
			using (HMACSHA256 hmac = new HMACSHA256(configurations[SiteServiceConfigurationConstants.PrivateHashKey].GetBytes()))
			{
				result = hmac.ComputeHash((credentials.PassCode + logonInfo.Ticks).GetBytes());
			}

			if (System.Text.Encoding.UTF8.GetString(logonInfo.Hash) == System.Text.Encoding.UTF8.GetString(result))
			{
				CredentialsCache[logonInfo.ClientID] = credentials;
				return credentials.Database;
			}
			return null;
		}

		protected IConnectionManager GetConnectionManager(LogonInfo logonInfo)
		{
			try
			{
				IConnectionManager dataModel;
				if (isCloudInstallation)
				{
					string database = VerifyCredentials(logonInfo);

					if (database != null)
					{
						dataModel = connectionPool.GetConnection(parameters.Item1,
							parameters.Item2,
							parameters.Item3,
							parameters.Item4,
							database,
							(Guid) logonInfo.UserID,
							parameters.Rest.Item1,
							parameters.Rest.Item2,
							parameters.Rest.Item3);
					}
					else
					{
						throw new Exception("No credentials found");
					}
				}
				else
				{
					dataModel = connectionPool.GetConnection(
						parameters.Item1,
						parameters.Item2,
						parameters.Item3,
						parameters.Item4,
						parameters.Item5,
						(Guid) logonInfo.UserID,
						parameters.Rest.Item1,
						parameters.Rest.Item2,
						parameters.Rest.Item3);
				}
				if (dataModel != null)
				{
					if (logonInfo.UserID != RecordIdentifier.Empty)
					{
						dataModel.Connection.SetContext(logonInfo.UserID);
					}

					((ProfileSettings) dataModel.Settings).Populate(logonInfo.Settings, dataModel.Cache);
				}
				return dataModel;
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
				throw;
			}
		}

		protected IConnectionManager GetConnectionManager()
		{
			try
			{
				return connectionPool.GetConnection(
					parameters.Item1,
					parameters.Item2,
					parameters.Item3,
					parameters.Item4,
					parameters.Item5,
					parameters.Item6,
					parameters.Item7,
					parameters.Rest.Item1,
					parameters.Rest.Item2,
					parameters.Rest.Item3);
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
				throw;
			}
		}

		protected void ThrowChannelError(Exception e)
		{
			throw new FaultException("9" + e);
		}

		protected FaultException GetChannelError(Exception e)
		{
			return new FaultException("9" + e);
		}

		public virtual ConnectionEnum TestConnection(LogonInfo logonInfo)
		{
			IConnectionManager connection = GetConnectionManager(logonInfo);
			try
			{
				return connection != null
					? ConnectionEnum.Success
					: ConnectionEnum.DatabaseConnectionFailed;
			}
			finally
			{
				ReturnConnection(connection, out connection);
			}
		}

		public void Load(Dictionary<string, string> configurations)
		{
			this.configurations = configurations;

			Guid connectPoolChallenge = connectionPool.GetChallenge(configurations[SiteServiceConfigurationConstants.PrivateHashKey]);
			using (HMACSHA256 hmac = new HMACSHA256(configurations[SiteServiceConfigurationConstants.PrivateHashKey].GetBytes()))
			{
				byte[] result = hmac.ComputeHash(connectPoolChallenge.ToString().GetBytes());
				connectionPool.AdminAuthenticate(result);
			}

			SetIsCloud(configurations[SiteServiceConfigurationConstants.IsCloud]);

			string port = configurations["Port"];

			if (wcfHost != null)
			{
				wcfHost.Close();
			}

			ResolveConfiguration(configurations);

			UpdateAllDatabases();

			// By passing 'this' as the first parameter, we connect to this instance
			wcfHost = new ServiceHost(this, new Uri("net.tcp://" + Environment.MachineName + ":" + port));

			var debug = wcfHost.Description.Behaviors.Find<System.ServiceModel.Description.ServiceDebugBehavior>();
			if (debug == null)
			{
				wcfHost.Description.Behaviors.Add(new System.ServiceModel.Description.ServiceDebugBehavior
				{
					IncludeExceptionDetailInFaults = true
				});
			}
			else
			{
				// make sure setting is turned ON     
				if (!debug.IncludeExceptionDetailInFaults)
				{
					debug.IncludeExceptionDetailInFaults = true;
				}
			}

			int timeout;
			if (!int.TryParse(configurations[SiteServiceConfigurationConstants.NetTcpTimeout], out timeout))
			{
				timeout = 60;
			}

			var binding = new NetTcpBinding
			{
				MaxBufferSize = 55000000,
				MaxReceivedMessageSize = 55000000,
				SendTimeout = new TimeSpan(0, 0, timeout),
				ReaderQuotas = {MaxStringContentLength = 55000000},
				Security = {Mode = SecurityMode.None},
				ReliableSession = {InactivityTimeout = TimeSpan.MaxValue},
				ReceiveTimeout = TimeSpan.MaxValue
			};

			wcfHost.AddServiceEndpoint(
				typeof (ISiteService), // Contract 
				binding, // Binding 
				SiteServiceConstants.EndPointName); // Address 

			wcfHost.Open();

			UpdateEMailSchedule(configurations);
            UpdateClearLogSchedule(Convert.ToInt32(configurations[SiteServiceConfigurationConstants.DaysToKeepLogs]));
            UpdateOmniJournalSchedule();
        }

		/// <summary>
		/// Determines if this instance of SiteService is running in cloud (is an LSOne Express instance) based on the given setting.
		/// </summary>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="FormatException" />
		private static void SetIsCloud(string configuration)
		{
			isCloudInstallation = bool.Parse(configuration);
		}

		internal class MaintainDatabaseParmeters
		{
			public string DatabaseName;
			public Guid UserGuid;
			public string DatabaseUserName;
			public string DatabaseUserPassword;
			public string CloudPassword;
			public string ScriptName { get; set; }
			public bool IsExternalScript { get; set; }
			public IConnectionManager Entry { get; set; }
			public Guid TaskGuid { get; set; }
		}
	  
		void util_MessageCallbackHandler(string sender, string msg)
		{
			Utils.LogDatabaseUpdate(msg);
		}

		private void CreateDatabase(object data)
		{
			try
			{
				var parameters = (MaintainDatabaseParmeters)data;
				string databaseName = parameters.DatabaseName;
				Guid userGuid = parameters.UserGuid;
				DatabaseUtility dbUtil = new DatabaseUtility("LSR");

				try
				{
					Utils.LogDatabaseUpdate("Create database: " + databaseName);
					Utils.LogDatabaseUpdate("UserGuid: " + userGuid);
					dbUtil.SetDatabaseConnection(DBConnection(databaseName));
				}
				catch (DatabaseNotExistsException)
				{
				}

				if (dbUtil.ConnectionResult == UtilityResult.DatabaseNotFound)
				{
					dbUtil.MessageCallbackHandler += new MessageCallback(util_MessageCallbackHandler);
					dbUtil.EmbeddedSQLServerInstall = false;
					dbUtil.IsCloud = true;
					dbUtil.UpdateDatabase(SQLServerType.SQLServer, RunScripts.All, DatabaseType.All, userGuid,parameters.CloudPassword,parameters.DatabaseUserPassword,parameters.DatabaseUserName);

					// Install main and audit database
				}

				dbUtil.CloseConnection();
			}

			catch (Exception e)
			{
				Utils.LogDatabaseUpdate(e.ToString());
				Utils.LogException(this, e);
				throw;
			}
		}

		public Guid CreateDatabase(string databaseName, long ticks,
			byte[] authenticationHash,string databaseUserPassword, string databaseUserName, string cloudPassword)
		{
			Utils.Log(this, Utils.Starting, LogLevel.Trace);

			Guid reply = Guid.Empty;

			try
			{
				lock (tickLock)
				{
					CheckTicksValid(ticks, lastTicks);
				}

				byte[] result;
				using (
					HMACSHA256 hmac =
						new HMACSHA256(
							configurations[SiteServiceConfigurationConstants.PrivateHashKey]
							.GetBytes()))
				{
					result = hmac.ComputeHash((createDatabasePassword + ticks).GetBytes());
				}

				if (System.Text.Encoding.UTF8.GetString(authenticationHash) !=
					System.Text.Encoding.UTF8.GetString(result))
				{
					Utils.Log(this, "Bad hash", LogLevel.Trace);
					return Guid.Empty;
				}
				reply = Guid.NewGuid();
				var thread = new Thread(CreateDatabase);
				MaintainDatabaseParmeters parameter = new MaintainDatabaseParmeters
				{
					DatabaseName = databaseName,
					UserGuid = reply,
					DatabaseUserName = databaseUserName,
					DatabaseUserPassword = Cipher.Decrypt(databaseUserPassword, configurations[SiteServiceConfigurationConstants.PrivateHashKey]),
					CloudPassword = Cipher.Decrypt(cloudPassword, configurations[SiteServiceConfigurationConstants.PrivateHashKey])
				};
				thread.Start(parameter);
			}
			catch (Exception e)
			{
				Utils.LogException(this, e);
				//Do not throw an exception it will fault the service
			}

			Utils.Log(this, Utils.Done, LogLevel.Trace);

			return reply;
		}

		private void UpdateAllDatabases()
		{
			if (isCloudInstallation)
			{
				List<string> databaseNames;
				try
				{
					Utils.LogDatabaseUpdate("Fetching databases from license service to update...");
					IConnectionManager dataModel = ConnectionManagerFactory.CreateConnectionManager();
					dataModel.ServiceFactory = new ServiceFactory();
					databaseNames = Services.Interfaces.Services.LicenseService(dataModel).GetDatabasesByHost(configurations[SiteServiceConfigurationConstants.ExternalAddress]);                                   
				}
				catch (Exception e)
				{
					Utils.LogDatabaseUpdate("Could not get databases to update. Database update process aborted.");
					Utils.LogDatabaseUpdate(e.Message);
					return;
				}                

				if (databaseNames.Count == 0)
				{
					return;
				}
								
				DatabaseUtility dbUtil = new DatabaseUtility("LSR");
				LinkedList<ScriptInfo> scripts = dbUtil.GetSQLScriptInfo();
				List<ScriptInfo> orderedScripts = scripts.Where(info =>
					info.ScriptType == RunScripts.UpdateDatabase //Only scripts that are of type UpdateDatabase                 
					&& info.ScriptSubType == ScriptSubType.Normal)
				.OrderBy(info => info.Version.DbVersion) //Order by DbVersion, PartnerVersion
				.ThenBy(info => info.Version.PartnerVersion).ToList();

				string latestVersion = orderedScripts[orderedScripts.Count - 1].Version.ToString();

				Utils.LogDatabaseUpdate("Starting database update to version: " + latestVersion);

				foreach (var database in databaseNames)
				{
					try
					{                    
						Utils.LogDatabaseUpdate("Updating database: " + database);
						dbUtil.SetDatabaseConnection(DBConnection(database));

						if(dbUtil.ConnectionResult != UtilityResult.Validated)
						{
							string message = GetDBUtilConnectionError(dbUtil.ConnectionResult);
							Utils.LogDatabaseUpdate("ERROR updating database " + database + ": " + message);
						}
						else
						{
							dbUtil.UpdateDatabase(SQLServerType.SQLServer, RunScripts.UpdateDatabase | RunScripts.LogicScripts, DatabaseType.All);
						}

						dbUtil.CloseConnection();
					}
					catch (Exception e)
					{
						string errorMessage = string.Empty;
						string errorScriptText = string.Empty;

						if (e.InnerException != null && e.InnerException.InnerException != null)
						{
							errorMessage = e.InnerException.InnerException.Message;
						}

						errorScriptText = e.Message;

						Utils.LogDatabaseUpdate("ERROR updating database " + database);
						Utils.LogDatabaseUpdate(errorMessage);
						Utils.LogDatabaseUpdate(errorScriptText + "\r\n");
					}
				}

				Utils.LogDatabaseUpdate("Finished updating databases\r\n");                
			}
		}

		private void SetfieldValue(DBField field, XmlNode record)
		{
			if (field.DBType == SqlDbType.Image || field.DBType == SqlDbType.VarBinary || field.DBType == SqlDbType.Binary)
			{
				field.Value = Convert.FromBase64String(record.SelectSingleNode(field.Name).InnerText);
			}
			else if (field.DBType == SqlDbType.Decimal)
			{
				field.Value = XmlConvert.ToDecimal(record.SelectSingleNode(field.Name).InnerText);
			}
			else if (field.DBType == SqlDbType.TinyInt)
			{
				byte value = record.SelectSingleNode(field.Name).InnerText == "0" ? (byte)0 : (byte)1;

				field.Value = value;
			}
			else if (field.DBType == SqlDbType.Int)
			{
				field.Value = XmlConvert.ToInt32(record.SelectSingleNode(field.Name).InnerText);
			}
			else if (field.DBType == SqlDbType.DateTime)
			{
				field.Value = XmlConvert.ToDateTime(record.SelectSingleNode(field.Name).InnerText, XmlDateTimeSerializationMode.Utc);
			}
			else if (field.DBType == SqlDbType.Time)
			{
				field.Value = XmlConvert.ToTimeSpan(record.SelectSingleNode(field.Name).InnerText);
			}
			else if (field.DBType == SqlDbType.UniqueIdentifier)
			{
				field.Value = XmlConvert.ToGuid(record.SelectSingleNode(field.Name).InnerText);
			}
			else
			{
				field.Value = record.SelectSingleNode(field.Name).InnerText;
			}
		}

		private void RunXmlScript(ScriptInfo scriptInfo,  string databaseName, Guid userID)
		{
			IConnectionManager dataModel = null;
			try
			{
				Utils.Log(this, Utils.Starting, LogLevel.Trace);

				dataModel = connectionPool.GetConnection(parameters.Item1,
					parameters.Item2,
					parameters.Item3,
					parameters.Item4,
					databaseName,
					userID,
					parameters.Rest.Item1,
					parameters.Rest.Item2,
					parameters.Rest.Item3);

				if (dataModel != null)
				{
					XmlDocument document;
					XmlNode root;
					XmlNode record;
					XmlNode foundNode;
					List<DBField> primaryKeys = null;
					List<DBField> columns = null;
					string fieldName;
					Parameters param;
					bool hasPostedWarning = false;
					bool abortRecord = false;

					document = new XmlDocument();

					try
					{
						string scriptData = "";

						if (!scriptInfo.IsExternalScript)
						{
							scriptData = DatabaseUtility.GetSpecificScript(dataModel, scriptInfo);

							if (scriptData == null)
							{
								Utils.Log(this, "Did not find script " + scriptInfo.ScriptName, LogLevel.Trace);
								return;
							}

							Utils.Log(this, "Found script " + scriptInfo.ScriptName, LogLevel.Trace);
							document.LoadXml(scriptData);
						}
						else
						{
							if (!File.Exists(scriptInfo.ResourceName))
							{
								Utils.Log(this, "Did not find external script " + scriptInfo.ScriptName, LogLevel.Trace);
								return;
							}

							Utils.Log(this, "Found external script " + scriptInfo.ScriptName, LogLevel.Trace);
							document.Load(scriptInfo.ResourceName);
						}

						param = Providers.ParameterData.Get(dataModel);

						root = document.FirstChild.NextSibling;
						string lastTableName = null;

						foreach (XmlNode node in root.ChildNodes)
						{
							// We skip over the schema since we do not care for that thats why we call next sibling
							record = node.FirstChild.NextSibling;

							var dbProvider = DataProviderFactory.Instance.Get<IDBFieldData, DBField>();
							while (record != null)
							{
								//Application.DoEvents();

								if (lastTableName != record.Name)
								{
									// We need to get primary key schema so that we can see if the record exists,
									// we also need to get the full colum schema
									primaryKeys = dbProvider.GetPrimaryFieldsForTable(dataModel, record.Name);
									columns = dbProvider.GetAllFieldsForTable(dataModel, record.Name);

									//MessageDialog.Show(record.Name);
								}

								abortRecord = false;

								foreach (DBField field in primaryKeys)
								{
									fieldName = field.Name.ToUpperInvariant();

									foundNode = record.SelectSingleNode(field.Name);

									if (foundNode != null)
									{
										SetfieldValue(field, record);
									}
									else
									{
										field.Value = DBNull.Value;
									}

									if (fieldName == "DATAAREAID")
									{
										field.Value = dataModel.Connection.DataAreaId;
									}
									else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID")
											 && (field.Value == DBNull.Value || (string)field.Value != "")
											 && !scriptInfo.ScriptName.ToLower().Contains("demo data"))
									// && record.Name != "RBOSTORETABLE" && record.Name != "RBOTERMINALTABLE" && record.Name != "RBOSTORETENDERTYPETABLE" && record.Name != "RBOLOCATIONPRICEGROUP"
									{
										if (param.LocalStore == "")
										{
											if (!hasPostedWarning)
											{
												hasPostedWarning = true;
											}

											abortRecord = true;
											break;
										}

										field.Value = (string)param.LocalStore;
									}
								}

								if (!abortRecord && !dbProvider.Exists(dataModel, record.Name, primaryKeys))
								{
									// Then if it did not exist then we insert the record. We need to remember to replace dataareaID with currently set dataarea ID
									// also if we find store ID then we need to replace it with current store ID same goes for RESTAURANTID
									foreach (DBField field in columns)
									{
										fieldName = field.Name.ToUpperInvariant();

										foundNode = record.SelectSingleNode(field.Name);

										if (foundNode != null)
										{
											SetfieldValue(field, record);
										}
										else
										{
											field.Value = DBNull.Value;
										}

										if (fieldName == "DATAAREAID")
										{
											field.Value = dataModel.Connection.DataAreaId;
										}
										else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID")
												 && (field.Value == DBNull.Value || (string)field.Value != "")
												 && !scriptInfo.ScriptName.ToLower().Contains("demo data"))
										{
											if (param.LocalStore == "")
											{
												if (!hasPostedWarning)
												{
													hasPostedWarning = true;
												}

												abortRecord = true;
												break;
											}

											field.Value = (string)param.LocalStore;
										}
									}

									if (!abortRecord)
									{
										dbProvider.Insert(dataModel, record.Name, columns);
									}
								}

								record = record.NextSibling;
							}
						}
					}
					catch (Exception x)
					{
						Utils.Log(this, $"Error running with datamodel {databaseName}, user {userID}", LogLevel.Error);
						Utils.LogException(this, x);
					}
				}
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex);
				throw ex;
			}
			finally
			{
				ReturnConnection(dataModel, out dataModel);
				Utils.Log(this, Utils.Done, LogLevel.Trace);
			}
		}

		private void RunXmlScript(ScriptInfo scriptInfo, IConnectionManager entry)
		{
			if (entry != null)
			{
				XmlDocument document;
				XmlNode root;
				XmlNode record;
				XmlNode foundNode;
				List<DBField> primaryKeys = null;
				List<DBField> columns = null;
				string fieldName;
				Parameters param;
				bool hasPostedWarning = false;
				bool abortRecord = false;

				document = new XmlDocument();

				try
				{
					string scriptData = "";

					if(!scriptInfo.IsExternalScript)
					{
						scriptData = DatabaseUtility.GetSpecificScript(entry, scriptInfo);

						if (scriptData == null)
						{
							Utils.Log(this, "Did not find script " + scriptInfo.ScriptName, LogLevel.Trace);
							return;
						}

						Utils.Log(this, "Found script " + scriptInfo.ScriptName, LogLevel.Trace);
						document.LoadXml(scriptData);
					}
					else
					{
						if(!File.Exists(scriptInfo.ResourceName))
						{
							Utils.Log(this, "Did not find external script " + scriptInfo.ScriptName, LogLevel.Trace);
							return;
						}

						Utils.Log(this, "Found external script " + scriptInfo.ScriptName, LogLevel.Trace);
						document.Load(scriptInfo.ResourceName);
					}

					param = Providers.ParameterData.Get(entry);

					root = document.FirstChild.NextSibling;
					string lastTableName = null;

					foreach (XmlNode node in root.ChildNodes)
					{
						// We skip over the schema since we do not care for that thats why we call next sibling
						record = node.FirstChild.NextSibling;

						var dbProvider = DataProviderFactory.Instance.Get<IDBFieldData, DBField>();
						while (record != null)
						{
							//Application.DoEvents();

							if (lastTableName != record.Name)
							{
								// We need to get primary key schema so that we can see if the record exists,
								// we also need to get the full colum schema
								primaryKeys = dbProvider.GetPrimaryFieldsForTable(entry, record.Name);
								columns = dbProvider.GetAllFieldsForTable(entry, record.Name);

								//MessageDialog.Show(record.Name);
							}

							abortRecord = false;

							foreach (DBField field in primaryKeys)
							{
								fieldName = field.Name.ToUpperInvariant();

								foundNode = record.SelectSingleNode(field.Name);

								if (foundNode != null)
								{
									SetfieldValue(field, record);
								}
								else
								{
									field.Value = DBNull.Value;
								}

								if (fieldName == "DATAAREAID")
								{
									field.Value = entry.Connection.DataAreaId;
								}
								else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID")
											&& (field.Value == DBNull.Value || (string)field.Value != "")
											&& !scriptInfo.ScriptName.Contains("Demo data.xml"))
								// && record.Name != "RBOSTORETABLE" && record.Name != "RBOTERMINALTABLE" && record.Name != "RBOSTORETENDERTYPETABLE" && record.Name != "RBOLOCATIONPRICEGROUP"
								{
									if (param.LocalStore == "")
									{
										if (!hasPostedWarning)
										{
											hasPostedWarning = true;
										}

										abortRecord = true;
										break;
									}

									field.Value = (string)param.LocalStore;
								}
							}

							if (!abortRecord && !dbProvider.Exists(entry, record.Name, primaryKeys))
							{
								// Then if it did not exist then we insert the record. We need to remember to replace dataareaID with currently set dataarea ID
								// also if we find store ID then we need to replace it with current store ID same goes for RESTAURANTID
								foreach (DBField field in columns)
								{
									fieldName = field.Name.ToUpperInvariant();

									foundNode = record.SelectSingleNode(field.Name);

									if (foundNode != null)
									{
										SetfieldValue(field, record);
									}
									else
									{
										field.Value = DBNull.Value;
									}

									if (fieldName == "DATAAREAID")
									{
										field.Value = entry.Connection.DataAreaId;
									}
									else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID")
												&& (field.Value == DBNull.Value || (string)field.Value != "")
												&& !scriptInfo.ScriptName.Contains("Demo data.xml"))
									{
										if (param.LocalStore == "")
										{
											if (!hasPostedWarning)
											{
												hasPostedWarning = true;
											}

											abortRecord = true;
											break;
										}

										field.Value = (string)param.LocalStore;
									}
								}

								if (!abortRecord)
								{
									dbProvider.Insert(entry, record.Name, columns);
								}
							}

							record = record.NextSibling;
						}
					}
				}
				catch (Exception x)
				{
					Utils.Log(this, $"Error running with datamodel {entry.Connection.DatabaseName}, user {entry.CurrentUser}" , LogLevel.Error);
					Utils.LogException(this, x);
				}
			}
		}

		private void InsertDemoData(object data)
		{
			try
			{
				DataProviderFactory.Instance.Register<IDBFieldData, DBFieldData, DBField>();
				var parameters = (MaintainDatabaseParmeters)data;

				string scriptName = parameters.ScriptName;
				List<ScriptInfo> demoDatalist = DatabaseUtility.GetSQLScriptInfo(RunScripts.DefaultData);

				if(parameters.IsExternalScript)
				{
					demoDatalist.AddRange(DatabaseUtility.GetExternalSQLScriptInfo());
				}

				if (demoDatalist.Count == 0)
				{
					Utils.Log(this, "No Scripts found", LogLevel.Trace);
					return;
				}
				ScriptInfo script = demoDatalist.Find(x => x.ScriptName == scriptName && x.IsExternalScript == parameters.IsExternalScript);
				if (script == null)
				{
					Utils.Log(this, "Script "+ scriptName +" not found", LogLevel.Trace);
					
					return;
				}

				Guid taskGuid = parameters.TaskGuid;
				if (parameters.Entry == null)
				{
					string databaseName = parameters.DatabaseName;
					Guid userGuid = parameters.UserGuid;
					if (taskGuid != Guid.Empty)
					{
						lock (activeTasks)
						{
							activeTasks.Add(taskGuid, string.Empty);
						}
					}
					RunXmlScript(script, databaseName, userGuid);
					if (taskGuid != Guid.Empty)
					{
						lock (activeTasks)
						{
							activeTasks.Remove(taskGuid);
						}
					}
				}
				else
				{
					if (taskGuid != Guid.Empty)
					{
						lock (activeTasks)
						{
							activeTasks.Add(taskGuid, string.Empty);
						}
					}
					RunXmlScript(script, parameters.Entry);
					if (taskGuid != Guid.Empty)
					{
						lock (activeTasks)
						{
							activeTasks.Remove(taskGuid);
						}
					}
				}
			}
			catch (Exception e)
			{
				Utils.LogDatabaseUpdate(e.ToString());
				Utils.LogException(this, e);
				throw;
			}
		}

		public Guid RunDemoData(ScriptInfo demoDataType, LogonInfo logonInfo)
		{
			Guid reply = Guid.Empty;

			Utils.Log(this, Utils.Starting, LogLevel.Trace);

			try
			{
				IConnectionManager dataModel = GetConnectionManager(logonInfo);

				try
				{
					Utils.Log(this, Utils.Starting, LogLevel.Trace);

					var thread = new Thread(InsertDemoData);
					reply = Guid.NewGuid();
					MaintainDatabaseParmeters parameter = new MaintainDatabaseParmeters
					{
						ScriptName = demoDataType.ScriptName,
						Entry = dataModel,
						TaskGuid = reply,
						IsExternalScript = demoDataType.IsExternalScript
					};
					thread.Start(parameter);
				}
				catch (Exception e)
				{
					Utils.LogException(this, e);
					//Do not throw an exception it will fault the service
				}
				finally
				{
					ReturnConnection(dataModel, out dataModel);
					Utils.Log(this, Utils.Done, LogLevel.Trace);
				}
			}
			catch (Exception e)
			{
				Utils.LogException(this, e);
				//Do not throw an exception it will fault the service
			}

			Utils.Log(this, Utils.Done, LogLevel.Trace);

			return reply;
		}
	  
		public Guid RunDemoDataUnsecureRemoveWhenNotNeeded(ScriptInfo demoDataType, long ticks, byte[] authenticationHash, string databaseName, Guid userGuid)
		{
			Guid reply = Guid.Empty;

			Utils.Log(this, Utils.Starting, LogLevel.Trace);

			try
			{
				lock (tickLock)
				{
					if (ticks < lastTicks)
					{
						Utils.Log(this, "Invalid tick", LogLevel.Trace);
						throw new Exception("Check client date/time settings and retry.");
					}
				}

				byte[] result;
				using (
					HMACSHA256 hmac =
						new HMACSHA256(
							configurations[SiteServiceConfigurationConstants.PrivateHashKey]
							.GetBytes()))
				{
					result = hmac.ComputeHash((createDatabasePassword + ticks).GetBytes());
				}

				if (System.Text.Encoding.UTF8.GetString(authenticationHash) !=
					System.Text.Encoding.UTF8.GetString(result))
				{
					Utils.Log(this, "Bad hash", LogLevel.Trace);
					return Guid.Empty;
				}
				reply = Guid.NewGuid();
				var thread = new Thread(InsertDemoData);
				MaintainDatabaseParmeters parameter = new MaintainDatabaseParmeters
				{
					DatabaseName = databaseName,
					UserGuid = userGuid,
					ScriptName = demoDataType.ScriptName,
					TaskGuid = reply,
					IsExternalScript = demoDataType.IsExternalScript
				};
				thread.Start(parameter);
			}
			catch (Exception e)
			{
				Utils.LogException(this, e);
				//Do not throw an exception it will fault the service
			}

			Utils.Log(this, Utils.Done, LogLevel.Trace);

			return reply;
		}

		public bool IsTaskActive(Guid taskGuid)
		{
			lock (activeTasks)
			{
				return activeTasks.ContainsKey(taskGuid);
			}
		}

		public List<ScriptInfo> GetDemoDataTypes()
		{
			List<ScriptInfo> reply = new List<ScriptInfo>();
			reply.AddRange(DatabaseUtility.GetSQLScriptInfo(RunScripts.DefaultData));
			reply.AddRange(DatabaseUtility.GetExternalSQLScriptInfo());
			return reply;
		}

		private string GetDBUtilConnectionError(UtilityResult result)
		{
			switch (result)
			{
				case UtilityResult.NotValidated:
					return "Connection could not be validated";
				case UtilityResult.SQLServerNotFound:
					return "The SQL server defined in the configuration file could not be found";
				case UtilityResult.DatabaseNotFound:
					return "The database could not be found";                                    
				case UtilityResult.LogonInformationNotValid:
					return "The logon information in the configuration file is not valid for this database";                    
				default:
					return "";
			}            
		}

		public void Unload()
		{
			Instance = null;

            StopScheduler(emailScheduler);
            StopScheduler(clearLogsScheduler);

			if (wcfHost != null)
			{
				wcfHost.Close();
				wcfHost = null;
			}

			if (connectionPool != null)
			{
				connectionPool.Clear();
			}
		}

		public bool Exclude
		{
			get
			{
				return false;
			}
		}

		public FolderItem CustomConfigFile
		{
			get
			{
				return null;
			}
		}

		public string ConfigurationName
		{
			get { return "base"; }
		}

		public string ConfigurationKey
		{
			get { return "base"; }
		}

		public DateTime GetServerUTCDate()
		{
			return DateTime.UtcNow;
		}

		private void CheckTicksValid(long ticks, long referenceTicks)
		{
			long serverTicks = DateTime.UtcNow.Ticks;
			long ticksInAnHour = 36000000000;

			if ((ticks > serverTicks + ticksInAnHour) || (ticks < referenceTicks))
			{
				throw new ServerTimeException(serverTicks, ticks);
			}
		}

		public Guid RegisterClient(string userName, byte[] hash, string dbname, Guid clientID, string passCode, long tick)
		{
			string connectionString = LoginDBConnection();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				Utils.Log(this, "Connected", LogLevel.Debug);

				bool exists = false;
				using (SqlCommand checkExists = new SqlCommand())
				{
					checkExists.Connection = connection;
					checkExists.CommandText = "select 1 from CredentialEntries where ClientID = @ClientID";

					SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.UniqueIdentifier);
					clientIDParameter.Value = clientID;
					checkExists.Parameters.Add(clientIDParameter);

					var checkReader = checkExists.ExecuteReader();
					if (checkReader.HasRows)
					{
						exists = true;
					}

					checkReader.Close();
					checkReader.Dispose();
					checkReader = null;
				}

				if (exists)
				{
					using (SqlCommand updateCredentialEntryCommand = new SqlCommand())
					{
						updateCredentialEntryCommand.Connection = connection;
						updateCredentialEntryCommand.CommandText = @"update CredentialEntries set
																				PassCode = @PassCode ,
																				Ticks = @Ticks
																				where ClientID = @ClientID";

						SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.UniqueIdentifier);
						clientIDParameter.Value = clientID;
						updateCredentialEntryCommand.Parameters.Add(clientIDParameter);

						SqlParameter passCodeParameter = new SqlParameter("Passcode", SqlDbType.NVarChar);
						passCodeParameter.Value = passCode;
						updateCredentialEntryCommand.Parameters.Add(passCodeParameter);

						SqlParameter ticParameter = new SqlParameter("Ticks", SqlDbType.BigInt);
						ticParameter.Value = tick;
						updateCredentialEntryCommand.Parameters.Add(ticParameter);

						updateCredentialEntryCommand.ExecuteNonQuery();
					}
				}
				else
				{
					using (SqlCommand insertCredentialEntryCommand = new SqlCommand())
					{
						insertCredentialEntryCommand.Connection = connection;
						insertCredentialEntryCommand.CommandText =
							@"insert into CredentialEntries values(
																	@UserName,
																	@ClientID,
																	@PassCode,
																	@Ticks,
																	@DBName)";

						SqlParameter userNameParameter = new SqlParameter("UserName", SqlDbType.NVarChar);
						userNameParameter.Value = userName;
						insertCredentialEntryCommand.Parameters.Add(userNameParameter);

						SqlParameter clientIDParameter = new SqlParameter("ClientID", SqlDbType.UniqueIdentifier);
						clientIDParameter.Value = clientID;
						insertCredentialEntryCommand.Parameters.Add(clientIDParameter);

						SqlParameter passCodeParameter = new SqlParameter("Passcode", SqlDbType.NVarChar);
						passCodeParameter.Value = passCode;
						insertCredentialEntryCommand.Parameters.Add(passCodeParameter);

						SqlParameter ticParameter = new SqlParameter("Ticks", SqlDbType.BigInt);
						ticParameter.Value = tick;
						insertCredentialEntryCommand.Parameters.Add(ticParameter);

						SqlParameter dbNameParameter = new SqlParameter("DBName", SqlDbType.NVarChar);
						dbNameParameter.Value = dbname;
						insertCredentialEntryCommand.Parameters.Add(dbNameParameter);

						insertCredentialEntryCommand.ExecuteNonQuery();
					}
				}
			}

			return Guid.NewGuid();
		}

		public bool CheckDatabaseAvailability(string databaseName)
		{
			if (existingDatabases.ContainsKey(databaseName))
			{
				existingDatabases[databaseName]++;
				return false;
			}

			DatabaseUtility util = new DatabaseUtility("LSR");
			try
			{
				util.SetDatabaseConnection(DBConnection(databaseName));
			}
			catch (DatabaseNotExistsException)
			{
				return true;
			}

			if (util.ConnectionResult == UtilityResult.DatabaseNotFound)
			{
			   return true;
			}
			if (util.ConnectionResult == UtilityResult.Validated)
			{
				existingDatabases.Add(databaseName, 1);
				return false;
			}
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entry"></param>
		/// <param name="func"></param>
		/// <param name="logString">Should be set as "MethodBase.GetCurrentMethod().Name"</param>
		/// <returns></returns>
		protected T DoWork<T>(IConnectionManager entry, Func<T> func, string logString)
		{
			try
			{
				Utils.Log(this, Utils.Starting, LogLevel.Trace, logString);
				return func();
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex, logString);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);
				Utils.Log(this, Utils.Done, LogLevel.Trace, logString);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="func"></param>
		/// <param name="logString">Should be set as "MethodBase.GetCurrentMethod().Name"</param>
		protected void DoWork(IConnectionManager entry, Action func, string logString)
		{
			try
			{
				Utils.Log(this, Utils.Starting, LogLevel.Trace, logString);
				func();
			}
			catch (Exception ex)
			{
				Utils.LogException(this, ex, logString);
				throw GetChannelError(ex);
			}
			finally
			{
				ReturnConnection(entry, out entry);
				Utils.Log(this, Utils.Done, LogLevel.Trace, logString);
			}
		}

		public void OnNotifyPlugin(object sender, MessageEventArgs e)
		{
			
		}

		public virtual void NotifyPlugin(MessageEventArgs e)
		{
			MessageHandler.SendNotifyPlugin(this, e);
		}        
    }
}