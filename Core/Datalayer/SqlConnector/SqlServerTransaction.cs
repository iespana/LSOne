using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security;

using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Caching;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.EventArguments;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.SqlConnector
{
	internal class SqlServerTransaction : IConnectionManagerTransaction
	{
		private bool isAdmin = false;
		private bool isCentralDatabase;
		private bool isCloud;
		IConnectionManager connectionManager;
		SqlTransaction transaction;

		public event ConnectionLostHandler ConnectionLost;

		public event ResolveEventHandler ResolveServiceAssembly;

		public SqlServerTransaction(IConnectionManager connectionManager, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			this.connectionManager = connectionManager;
			this.isAdmin = connectionManager.IsAdmin;
			this.isCloud = connectionManager.IsCloud;
			this.isCentralDatabase = connectionManager.IsCentralDatabase;
		    this.IgnoreColumnOptimizer = connectionManager.IgnoreColumnOptimizer;
			#pragma warning disable 0612, 0618 // Disable warning on NativeConnection, its valid here since we are within the component
			SqlConnection connection =  ((SqlConnection) Connection.NativeConnection);
			if (connection.State == ConnectionState.Closed)
			{
				connection.Open();
			}
			transaction = connection.BeginTransaction(isolationLevel);
			#pragma warning restore 0612, 0618

			connectionManager.ConnectionLost += OnConnectionLost;
			connectionManager.ResolveServiceAssembly += OnResolveServiceAssembly;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetConnectionEnvironment(bool isCloud, bool isCentralDatabase)
		{
			connectionManager.SetConnectionEnvironment(isCloud, isCentralDatabase);
		}

		public bool IsCloud { get { return isCloud; } }

	    public bool IgnoreColumnOptimizer { get; set; }
	    public bool DisableReplicationActionCreation { get { return connectionManager.DisableReplicationActionCreation; } set { connectionManager.DisableReplicationActionCreation = value; } }

		public bool IsCentralDatabase { get { return isCentralDatabase; } }

		private void OnConnectionLost(object sender, ConnectionLostEventArguments args)
		{
			if (ConnectionLost != null)
				ConnectionLost(sender, args);
		}

		private Assembly OnResolveServiceAssembly(object sender, ResolveEventArgs args)
		{
			if (ResolveServiceAssembly != null)
				return ResolveServiceAssembly(sender, args);

			return null;
		}

		#region IConnectionManagerTransaction Members

		public IErrorLog ErrorLogger
		{
			get
			{
				return connectionManager.ErrorLogger;
			}
			set
			{
				((SqlServerConnectionManager)connectionManager).ErrorLogger = value;
			}
		}

		public ServiceFactory ServiceFactory
		{
			get { return connectionManager.ServiceFactory; }
			set { connectionManager.ServiceFactory = value; }
		}

		//TO Be deleted DO NOT USE
		public IDbTransaction TempGetTransaction()
		{
			return transaction;
		}

		public void Commit()
		{
			try
			{
				if (transaction != null)
				{
					transaction.Commit();
				}
			}
			finally
			{
				transaction.Dispose();
				transaction = null;
			}
		}

		public void Rollback()
		{
			if (transaction != null)
			{
				try
				{
					transaction.Rollback();
				}
				finally
				{
					transaction.Dispose();
					transaction = null;
				}
			}
		}

		public RecordIdentifier CurrentTerminalID
		{
			get { return connectionManager.CurrentTerminalID; }
			set { connectionManager.CurrentTerminalID = value; }
		}

		public RecordIdentifier CurrentStaffID
		{
			get { return connectionManager.CurrentStaffID; }
			set { connectionManager.CurrentStaffID = value; }
		}

		public RecordIdentifier CurrentItemID
        {
			get { return connectionManager.CurrentItemID; }
			set { connectionManager.CurrentItemID = value;  }
        }

		public int PageSize { get; set; }

		public bool IsTouchClient
		{
			get
			{
				return connectionManager.IsTouchClient;
			}
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetTouchClient(bool value)
		{
			connectionManager.SetTouchClient(value);
		}

		public RecordIdentifier CurrentStoreID
		{
			get
			{
				return connectionManager.CurrentStoreID;
			}
			set
			{
				connectionManager.CurrentStoreID = value;
			}
		}

		public FolderItem ServiceBasePath
		{
			get { return connectionManager.ServiceBasePath; }
			set { connectionManager.ServiceBasePath = value; }
		}

		public bool IsHeadOffice
		{
			get { return connectionManager.IsHeadOffice; }
		}

		public IConnection Connection
		{
			get { return new SqlServerTransactionConnectionWrapper(connectionManager.Connection, transaction); }
		}

		public IUser CurrentUser
		{
			get { return connectionManager.CurrentUser; }
		}

		public bool HasPermission(string permissionUUID)
		{
			return connectionManager.HasPermission(permissionUUID);
		}

		public bool HasPermissionIgnoreOverrides(string permissionUUID)
		{
			return connectionManager.HasPermissionIgnoreOverrides(permissionUUID);
		}

		public bool HasPermission(string permissionUUID, string login, System.Security.SecureString password)
		{
			return connectionManager.HasPermission(permissionUUID, login, password);
		}

		public bool HasPermission(string permissionUUID, string tokenHash, out bool tokenIsUser)
		{
			return connectionManager.HasPermission(permissionUUID, tokenHash, out tokenIsUser);
		}

		public bool VerifyCredentials(string login, SecureString password, out bool isLockedOut)
		{
			return connectionManager.VerifyCredentials(login, password, out isLockedOut);
		}

		public bool VerifyCredentials(string login, string tokenHash, out bool isLockedOut, out bool tokenIsUser)
		{
			return connectionManager.VerifyCredentials(login,tokenHash, out isLockedOut, out tokenIsUser);
		}

		public void BeginPermissionOverride(Guid context, HashSet<string> permissions)
		{
			connectionManager.BeginPermissionOverride(context, permissions);
		}

		public void EndPermissionOverride(Guid context)
		{
			connectionManager.EndPermissionOverride(context);
		}

		/*public RecordIdentifier GenerateNumberFromSequence(ISequenceable sequenceProvider)
		{
			return DataProviders.NumberSequenceData.GenerateNumberFromSequence(this, sequenceProvider);
		}*/

		public ICache Cache
		{
			get 
			{
				ICache cache = connectionManager.Cache;
				((Cache)cache).DataModel = this;

				return cache;
			}
		}

		public IProfileSettings Settings
		{
			get { return connectionManager.Settings; }
		}

		public DecimalLimit GetDecimalSetting(DecimalSettingEnum type)
		{
			return SqlServerConnectionManager.GetDecimalSetting(this,type);
		}

		public DecimalLimit GetDecimalSetting(string id)
		{
			return SqlServerConnectionManager.GetDecimalSetting(this,id);
		}

		public bool IsSameAsCurrentPassword(System.Security.SecureString password)
		{
			return connectionManager.IsSameAsCurrentPassword(password);
		}

		public bool ChangePassword(System.Security.SecureString oldPassword, System.Security.SecureString newPassword)
		{
			return connectionManager.ChangePassword(oldPassword, newPassword);
		}

		public bool ChangePasswordForOtherUser(RecordIdentifier userID, SecureString newPassword, bool needPasswordChange)
		{
			return connectionManager.ChangePasswordForOtherUser(userID, newPassword, needPasswordChange);
		}

		public bool ChangePasswordHashForOtherUser(RecordIdentifier userID, string passwordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, bool generateActions = true)
		{
			return connectionManager.ChangePasswordHashForOtherUser(userID, passwordHash, needPasswordChange, expiresDate, lastChangeTime, generateActions);
		}

		/// <summary>
		/// Returns the login, hashed password and if the user is an Active Directory user for an active (non-deleted) user.
		/// </summary>
		public void GetUserPasswordInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate,
			out DateTime lastChangeTime)
		{
			connectionManager.GetUserPasswordInfo(userID, out passwordHash, out expiresDate, out lastChangeTime);
		}

		public void GetActiveUserInfo(RecordIdentifier userID, out string login, out string passwordHash, out bool isDomainUser)
		{
			connectionManager.GetActiveUserInfo(userID, out login, out passwordHash, out isDomainUser);
		}

		public IService Service(ServiceType serviceType)
		{
			return connectionManager.Service(serviceType);
		}

		public void SetService(ServiceType serviceType, IService service)
		{
			connectionManager.SetService(serviceType, service);
		}

		public bool ServiceIsLoaded(ServiceType serviceType)
		{
			return connectionManager.ServiceIsLoaded(serviceType);
		}

		public IConnectionManagerTransaction CreateTransaction()
		{
			return connectionManager.CreateTransaction(); 
		}

		public IConnectionManagerTransaction CreateTransaction(IsolationLevel isolationLevel)
		{
			return connectionManager.CreateTransaction(isolationLevel);
		}

		public string SiteServiceAddress
		{
			get { return connectionManager.SiteServiceAddress;}
			set { connectionManager.SiteServiceAddress = value;}
		}

		public UInt16 SiteServicePortNumber
		{
			get { return connectionManager.SiteServicePortNumber; }
			set { connectionManager.SiteServicePortNumber = value; }
		}

		public IConnectionManagerTemporary CreateTemporaryConnection()
		{
			return connectionManager.CreateTemporaryConnection();
		}

		public List<T> UnsecureExecuteReader<T, S>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ref S param, RefDataPopulatorWithEntry<T, S> populator, bool persist = false, List<string> persistStrings = null) where T : new()
		{
			return connectionManager.UnsecureExecuteReader<T,S>(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, cmd,ref param, populator, persist,persistStrings);
		}

		public List<T> UnsecureExecuteReader<T>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ConnectionManagerDataPopulator<T> dataPopulator) where T : new()
		{
			return connectionManager.UnsecureExecuteReader<T>(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, cmd, dataPopulator);
		}

		public LoginResult TestConnection(string server, bool windowsAuthentication,
										  string login, SecureString password,
										  string databaseName, ConnectionType connectionType, string dataAreaID)
		{
			return connectionManager.TestConnection(server, windowsAuthentication, login, password,
													databaseName, connectionType, dataAreaID);
		}


		public LoginResult ReLogin(string server, bool windowsAuthentication, string serverLogin,
								   SecureString serverPassword,
								   string databaseName, string login, ConnectionType connectionType,
								   ConnectionUsageType connectionUsage, string dataAreaID)
		{
			return connectionManager.ReLogin(server, windowsAuthentication, serverLogin,
											 serverPassword, databaseName, login,
											 connectionType, connectionUsage, dataAreaID);
		}

		public LoginResult Login(string server, bool windowsAuthentication, string serverLogin,
								 SecureString serverPassword,
								 string databaseName, string login, SecureString password, ConnectionType connectionType,
								 ConnectionUsageType connectionUsage, string dataAreaID)
		{
			return connectionManager.Login(server, windowsAuthentication, serverLogin, serverPassword, databaseName,
										   login, password, connectionType, connectionUsage,
										   dataAreaID);
		}


		public LoginResult TokenLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, string tokenHash, ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID, out string login, out string passwordHash)
		{
			return connectionManager.TokenLogin(server, windowsAuthentication, serverLogin, serverPassword, databaseName,
				tokenHash, connectionType, connectionUsage, dataAreaID, out login, out passwordHash);
		}

		public LoginResult SwitchUser(string login, SecureString password, string passwordHash = "")
		{
			return connectionManager.SwitchUser(login, password, passwordHash);
		}


		public LoginResult TokenSwitchUser(string tokenHash)
		{
			return TokenSwitchUser(tokenHash);
		}

		public void LogOff()
		{
			connectionManager.LogOff();
		}

		public void UnsecureNonQuary(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd)
		{
			connectionManager.UnsecureNonQuary(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, cmd);
		}

		public void RunDatabaseUpdateScript(string script)
		{
			connectionManager.RunDatabaseUpdateScript(script);
		}

		public void RunDatabaseUpdateScriptFromResource(Assembly assembly, string resourceName)
		{
			connectionManager.RunDatabaseUpdateScriptFromResource(assembly, resourceName);
		}

		public bool UserNeedsPasswordChange(RecordIdentifier userID)
		{
			return connectionManager.UserNeedsPasswordChange(userID);
		}

		public void LockUser(RecordIdentifier userID)
		{
			connectionManager.LockUser(userID);
		}

		public string GetPasswordHash(SecureString password)
		{
			return connectionManager.GetPasswordHash(password);
		}

		public bool OpenDatabaseConnection(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName,
			ConnectionType connectionType, string dataAreaID)
		{
			return connectionManager.OpenDatabaseConnection(server, windowsAuthentication, databaseName, serverPassword, databaseName, connectionType, databaseName);
		}

		#endregion


		public bool IsAdmin
		{
			get { return isAdmin; }
		}

		public LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, string login, SecureString password, ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{
			throw new NotImplementedException();
		}

		public LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, Guid login, ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{
			throw new NotImplementedException();
		}
	}
}
