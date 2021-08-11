using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Security;

using LSOne.DataLayer.GenericConnector.Caching;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.GenericConnector
{
	public abstract class ConnectionManagerBase : IConnectionManager
	{
		private static object _synclock = new object();

		private IProfileSettings settings;
		private RecordIdentifier currentStoreID;
		private RecordIdentifier currentTerminalID;
		private RecordIdentifier currentStaffID;
		private RecordIdentifier currentItemID;
		private Dictionary<Guid, HashSet<string>> permissionOverrideContexts;
		private IErrorLog errorLogger;
		private User currentUser;
		private Cache cache;
		private bool isCentralDatabase;
		private bool isCloud;
		protected bool isTouchClient;
		protected bool isAdmin ;
		public event ConnectionLostHandler ConnectionLost;

		protected Dictionary<string, string> serviceOverrides;
		private Dictionary<ServiceType, bool> hasServiceCache;

		private ServiceFactory serviceFactory;
#pragma warning disable CS0067
		public event ResolveEventHandler ResolveServiceAssembly;
#pragma warning restore CS0067
		protected ConnectionManagerBase()
		{
			isTouchClient = false;
			hasServiceCache = new Dictionary<ServiceType, bool>();
			cache = null;
			
			currentStoreID = 0; // 0 means not defined, RecordIdentifier.Empty means Head office
			currentTerminalID = RecordIdentifier.Empty;
			currentStaffID = RecordIdentifier.Empty;
			currentItemID = RecordIdentifier.Empty;
			Services = null;
			currentUser = null;

			PasswordHash = "";
			Password = null;
			isAdmin = false;
			isCloud = false;
			isCentralDatabase = false;

			serviceOverrides = new Dictionary<string, string>();

		    IgnoreColumnOptimizer = true;
		}

        protected internal ConnectionManagerBase(ConnectionManagerBase connectionManager)
		{
			cache = connectionManager.cache;
			PasswordHash = connectionManager.PasswordHash;
			Password = connectionManager.Password;
			settings = connectionManager.settings;
			isAdmin = false;
			isCloud = connectionManager.isCloud;
			isCentralDatabase = connectionManager.isCentralDatabase;

			serviceOverrides = new Dictionary<string, string>();
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetConnectionEnvironment(bool isCloud, bool isCentralDatabase)
		{
			this.isCentralDatabase = isCentralDatabase;
			this.isCloud = isCloud;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public void SetTouchClient(bool value)
		{
			this.isTouchClient = value;
		}

		public bool IsTouchClient
		{
			get
			{
				return isTouchClient;
			}
		}

	    public bool IgnoreColumnOptimizer { get; set; }
	    public bool DisableReplicationActionCreation { get; set; }

		public void SetServiceOverride(ServiceType serviceType, string overrideFullName)
		{
			serviceFactory.SetServiceOverride(serviceType, overrideFullName);
		}

		/// <summary>
		/// Removes the override for the given <see cref="ServiceType"/> and unloads the current service of that type. The next time the service is called it will then load the default implementation.
		/// </summary>
		/// <param name="serviceType">The type of service to remove the override for</param>
		public void RemoveServiceOverride(ServiceType serviceType)
        {
			serviceFactory.RemoveServiceOverride(serviceType);
        }

		/// <summary>
		/// Should return true if connection manager is connecting to central database that can see all stores and all terminals
		/// </summary>
		public bool IsCentralDatabase 
		{ 
			get { return isCentralDatabase; }
			protected set { isCentralDatabase = value; }
		}

		/// <summary>
		/// Should return true if we are in cloud setup. This does not mean that connection manager is directly connecting to the cloud,
		/// like for POS it has local database.
		/// </summary>
		public bool IsCloud 
		{ 
			get { return isCloud; }
			protected set { isCloud = value; }
		}

	  

		protected abstract Cache CreateCache();

		protected internal IDictionary<ServiceType, IService> Services { get; set; }

		public ICache Cache
		{
			get
			{
				lock (_synclock)
				{
					if (cache == null)
					{
						cache = CreateCache();
					}
					return cache;
				}
			}
		}

		public IProfileSettings Settings
		{
			get { return settings; }
			protected set { settings = value; }
		}

		// Note this may NOT be exposed to other components  
		protected internal string PasswordHash { get; set; }
		protected internal SecureString Password { get; set; }

		public RecordIdentifier CurrentTerminalID
		{
			get { return currentTerminalID; }
			set { currentTerminalID = (value == "" ? RecordIdentifier.Empty : value); }
		}

		public RecordIdentifier CurrentStaffID
		{
			get { return currentStaffID; }
			set { currentStaffID = (value == "" ? RecordIdentifier.Empty : value); }
		}

		public RecordIdentifier CurrentStoreID
		{
			get { return currentStoreID; }
			set { currentStoreID = (value == "" ? RecordIdentifier.Empty : value); }
		}

		public RecordIdentifier CurrentItemID
        {
			get { return currentItemID;  }
			set { currentItemID = (value == "" ? RecordIdentifier.Empty : value);  }
        }

		/// <summary>
		/// The default page size for Data scrolls, paging and limited search result
		/// </summary>
		public int PageSize { get; set; }

		public FolderItem ServiceBasePath
		{
			get { return serviceFactory.ServiceBasePath; }
			set { serviceFactory.ServiceBasePath = value; }
		}

		public bool IsAdmin
		{
			get { return isAdmin; }
		}

	   
		public bool IsHeadOffice
		{
			get
			{
				return CurrentStoreID == null
						|| CurrentStoreID == RecordIdentifier.Empty
						|| CurrentStoreID == "";
			}
		}

		public IErrorLog ErrorLogger
		{
			get
			{
				if (errorLogger == null)
				{
					return new EmptyErrorLogger();
				}
				return errorLogger;
			}
			set
			{
				errorLogger = value;
			}
		}

		public ServiceFactory ServiceFactory
		{
			get { return serviceFactory; }
			set { serviceFactory = value; }
		}

		public void BeginPermissionOverride(Guid context, HashSet<string> permissions)
		{
			if (permissionOverrideContexts == null)
			{
				permissionOverrideContexts = new Dictionary<Guid, HashSet<string>>();
			}
			permissionOverrideContexts.Add(context, permissions);
		}

		public void EndPermissionOverride(Guid context)
		{
			if (permissionOverrideContexts != null && permissionOverrideContexts.ContainsKey(context))
			{
				permissionOverrideContexts.Remove(context);
			}
		}

		public virtual bool HasPermission(string permissionUUID)
		{
			if (isAdmin)
			{
				return true;
			}
			// Check override dictionary 
			if (permissionOverrideContexts != null && permissionOverrideContexts.Count > 0)
			{
				foreach (HashSet<string> current in permissionOverrideContexts.Values)
				{
					if (current.Contains(permissionUUID))
					{
						return true;
					}
				}
			}
			return false;
		}

		public abstract bool HasPermissionIgnoreOverrides(string permissionUUID);

		public abstract bool HasPermission(string permissionUUID, string login, SecureString password);

		public abstract bool HasPermission(string permissionUUID, string tokenHash, out bool tokenIsUser);

		public abstract bool VerifyCredentials(string login, SecureString password, out bool isLockedOut);

		public abstract bool VerifyCredentials(string login, string tokenHash, out bool isLockedOut, out bool tokenIsUser);

		//public abstract RecordIdentifier GenerateNumberFromSequence(ISequenceable sequenceProvider);

		public bool ServiceIsLoaded(ServiceType serviceType)
		{
			return serviceFactory.ServiceIsLoaded(serviceType);
		}

		public IUser CurrentUser
		{
			get { return currentUser; }
			internal protected set { currentUser = value as User; }
		}

		protected virtual void OnConnectionLost(object sender, EventArguments.ConnectionLostEventArguments args)
		{
			if (ConnectionLost != null)
			{
				ConnectionLost(this, args);
			}
		}

		public static DecimalLimit GetDecimalSetting(IConnectionManager entry, DecimalSettingEnum type)
		{
			if (type == DecimalSettingEnum.Prices)
			{
				return GetDecimalSetting(entry, "Prices");
			}
			if (type == DecimalSettingEnum.Quantity)
			{
				return GetDecimalSetting(entry, "Quantity");
			}
			if (type == DecimalSettingEnum.Tax)
			{
				return GetDecimalSetting(entry, "Tax");
			}
			if (type == DecimalSettingEnum.DiscountPercent)
			{
				return GetDecimalSetting(entry, "Disc pct");
			}
			return new DecimalLimit(0, 2);
		}

		public DecimalLimit GetDecimalSetting(DecimalSettingEnum type)
		{
			return GetDecimalSetting((IConnectionManager)this, type);
		}

		public static DecimalLimit GetDecimalSetting(IConnectionManager entry, string id)
		{
			var decimals = entry.Cache.GetSystemDecimals();

			return decimals.ContainsKey(id) ? decimals[id] : new DecimalLimit(0, 2);
		}

		public DecimalLimit GetDecimalSetting(string id)
		{
			return GetDecimalSetting((IConnectionManager)this, id);
		}

		public bool IsSameAsCurrentPassword(SecureString password)
		{
			return (HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66") == PasswordHash);
		}

		protected abstract bool DoChangePassword(SecureString oldPassword, SecureString newPassword);

		public bool ChangePassword(SecureString oldPassword, SecureString newPassword)
		{
			bool result = DoChangePassword(oldPassword, newPassword);

			if (result)
			{
				PasswordHash = HMAC_SHA1.GetValue(SecureStringHelper.ToString(newPassword), "df5da100-a9ba-11de-8a39-0800200c9a66");
				Password = newPassword;
			}

			return result;
		}

		protected abstract bool DoChangePasswordForOtherUser(RecordIdentifier userID, SecureString newPassword,
			bool needPasswordChange);
		public bool ChangePasswordForOtherUser(RecordIdentifier userID, SecureString newPassword, bool needPasswordChange)
		{
			bool result = DoChangePasswordForOtherUser(userID, newPassword, needPasswordChange);

			if (result && userID == CurrentUser.ID)
			{
				PasswordHash = HMAC_SHA1.GetValue(SecureStringHelper.ToString(newPassword), "df5da100-a9ba-11de-8a39-0800200c9a66");
				Password = newPassword;
				((User)CurrentUser).ForcePasswordChange = needPasswordChange;
			}

			return result;
		}

		public abstract IConnection Connection { get; protected set; }

		public abstract IConnectionManagerTransaction CreateTransaction();
        public abstract IConnectionManagerTransaction CreateTransaction(IsolationLevel isolationLevel);

		public abstract IConnectionManagerTemporary CreateTemporaryConnection();

		public abstract List<T> UnsecureExecuteReader<T, S>(string server,
			bool windowsAuthentication,
			string login,
			SecureString password,
			string databaseName,
			ConnectionType connectionType,
			string dataAreaID,
			IDbCommand cmd,
			ref S param,
			RefDataPopulatorWithEntry<T, S> populator,
			bool persist = false, 
			List<string> persiststrings = null) where T : new();

		public abstract List<T> UnsecureExecuteReader<T>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ConnectionManagerDataPopulator<T> dataPopulator) where T : new();

		public abstract LoginResult TestConnection(string server, bool windowsAuthentication,
								   string login, SecureString password,
								   string databaseName, ConnectionType connectionType, string dataAreaID);


		public abstract LoginResult ReLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
							string databaseName, string login, ConnectionType connectionType,
							ConnectionUsageType connectionUsage, string dataAreaID);

		public abstract LoginResult Login(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
						  string databaseName, string login, SecureString password, ConnectionType connectionType,
						  ConnectionUsageType connectionUsage, string dataAreaID);

		
		public abstract LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
			SecureString serverPassword, string databaseName, string login, SecureString password,
			ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID);

		public abstract LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
			SecureString serverPassword, string databaseName, Guid login, 
			ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID);
		
		public abstract LoginResult SwitchUser(string login, SecureString password, string passwordHash = "");

		public abstract LoginResult TokenSwitchUser(string tokenHash);

		public abstract LoginResult TokenLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
						  string databaseName, string tokenHash, ConnectionType connectionType,
						  ConnectionUsageType connectionUsage, string dataAreaID, out string login, out string passwordHash);

		public abstract void LogOff();

		public abstract string SiteServiceAddress { get; set; }
		public abstract UInt16 SiteServicePortNumber { get; set; }

		public abstract void UnsecureNonQuary(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd);

		public void RunDatabaseUpdateScript(string script)
		{
			var parts = script.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

			string sqlScript = "";
			foreach (var part in parts)
			{
				var scriptLine = part.Trim('\r', '\t');
				if (scriptLine.Trim().ToUpperInvariant() == "GO")
				{
					if (sqlScript.Length > 0)
					{
						using (var cmd = Connection.CreateCommand(sqlScript))
							Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
					}
					sqlScript = "";
				}
				else
				{
					if (scriptLine.Trim().Length > 0)
						sqlScript += scriptLine + "\n";
				}
			}
		}

		public void RunDatabaseUpdateScriptFromResource(Assembly assembly, string resourceName)
		{
			using (var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
			{
				RunDatabaseUpdateScript( textStreamReader.ReadToEnd());
			}
		}

		public abstract bool UserNeedsPasswordChange(RecordIdentifier userID);

		public abstract bool ChangePasswordHashForOtherUser(RecordIdentifier userID, string passwordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, bool generateActions = true);

		public abstract void GetUserPasswordInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime);

		/// <summary>
		/// Returns the login, hashed password and if the user is an Active Directory user for an active (non-deleted) user.
		/// </summary>
		public abstract void GetActiveUserInfo(RecordIdentifier userID, out string login, out string passwordHash, out bool isDomainUser);

		/// <summary>
		/// Locks the given user
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		public abstract void LockUser(RecordIdentifier userID);

		public string GetPasswordHash(SecureString password)
		{
			return HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66");
		}

		public abstract bool OpenDatabaseConnection(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
			string databaseName, ConnectionType connectionType, string dataAreaID);


		public virtual string GetServiceShortName(ServiceType serviceType)
		{
			string svcName = Enum.GetName(typeof(ServiceType), serviceType) + "@";
			return svcName.Replace("Service@", "");
		}

		public virtual List<KeyValuePair<ServiceType,string>> GetOverloadServiesWithMask(string endsWithMask, ServiceType? exclude)
		{
			KeyValuePair<ServiceType, string> item;
			List<string> overloadList;
			List<KeyValuePair<ServiceType,string>> result = new List<KeyValuePair<ServiceType,string>>();

			ServiceType[] values = (ServiceType[])Enum.GetValues(typeof(ServiceType));

			for (int i = 0; i < values.Length; i++)
			{
				if (exclude == null || exclude != values[i])
				{
					overloadList = GetServiceOverloadNames(values[i]);

					foreach (string overload in overloadList)
					{
						if (overload.Right(endsWithMask.Length).Equals(endsWithMask, StringComparison.InvariantCultureIgnoreCase))
						{
							item = new KeyValuePair<ServiceType, string>(values[i], overload);
							result.Add(item);
							break;
						}
					}
				}
			}

			return result;
		}

		public virtual List<string> GetServiceOverloadNames(ServiceType serviceType)
		{
			string serviceName;
			List<string> result = new List<string>();
			FolderItem servicesDirectory = ServiceBasePath;

			string servicePartName = "." + GetServiceShortName(serviceType) + ".";  // We use this one to find mask within services like *.EFT.*
			string serviceFileName = "LSOne.Services." + servicePartName + ".dll";

			List<FolderItem> serviceItems = servicesDirectory.FilteredChildren("*.dll");

			foreach(FolderItem serviceItem in serviceItems)
			{
				serviceName = serviceItem.Name;

				if (serviceName.Contains(servicePartName) && serviceItem.FullName != serviceFileName)
				{
					if (serviceName != "LSOne.Services.EFT.Common")
					{
						result.Add(serviceName);
					}
				}
			}

			return result;

		}

		public void UnloadService(ServiceType serviceType)
		{
			serviceFactory.UnloadService(serviceType);
		}

		public virtual void SetService(ServiceType serviceType, IService service)
		{
			serviceFactory.SetService(serviceType, service);
		}

		public virtual bool HasService(ServiceType serviceType)
		{
			return serviceFactory.HasService(serviceType);
		}

		public virtual IService Service(ServiceType serviceType)
		{
			return serviceFactory.Service(serviceType, this);
		}
		
	}
}
