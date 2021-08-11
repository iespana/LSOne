using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security;

using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
	public delegate void SiteServiceAddressProc(ref string address, ref UInt16 portNumber);
	public delegate void ConnectionManagerDataPopulator<T>(IDataReader dr, T item);
	public delegate void RefDataPopulatorWithEntry<T, S>(IConnectionManager entry, IDataReader dr, T item, ref S param);
	public delegate void RefDataPopulatorWithEntryAndExtraParameter<T, S>(IConnectionManager entry, IDataReader dr, T item, ref S param, object param2);

	public delegate void ConnectionLostHandler(object sender, EventArguments.ConnectionLostEventArguments args);

	public interface IConnectionManager
	{
		event ConnectionLostHandler ConnectionLost;

		event ResolveEventHandler ResolveServiceAssembly;

		RecordIdentifier CurrentStoreID
		{
			get;
			set;
		}

		RecordIdentifier CurrentTerminalID
		{
			get;
			set;
		}

		RecordIdentifier CurrentStaffID
		{
			get;
			set;
		}

		RecordIdentifier CurrentItemID
        {
			get;
			set;
        }

		int PageSize
		{
			get; set;
		}

	    bool IgnoreColumnOptimizer { get; set; }

        bool DisableReplicationActionCreation { get; set; }
		bool IsTouchClient
		{
			get;
		}

		FolderItem ServiceBasePath { get; set; }

		IConnection Connection
		{
			get;
		}

		bool IsAdmin { get; }

	    /// <summary>
	    /// Executes a stored procedure in unsecure way, as in before a valid Site Manager user is actually logged in. Note that the word _Unsecure will be automatically embedded at back of the procedure name
	    /// to make sure that only procedures that were intended for such execution can be executed.
	    /// </summary>
	    /// <typeparam name="T">Type of dataentity to populate</typeparam>
	    /// <typeparam name="S">Type of ... </typeparam>
	    /// <param name="server">Name of the SQL server</param>
	    /// <param name="windowsAuthentication">Specifies if Windows authentication should be used</param>
	    /// <param name="login">Login into the SQL server if windows authentication is not used</param>
	    /// <param name="password">Password into the sql server if windows authentication is not used</param>
	    /// <param name="databaseName">Name of the database</param>
	    /// <param name="connectionType">The type of the connection, Named pipes, TCP/IP, etc</param>
	    /// <param name="dataAreaID">The dataarea id</param>
	    /// <param name="cmd">The command to be executed. (Note that the keyword _Unsecure will be embedded to the back of the name of the command</param>
	    /// <param name="param">By ref parameter that is passed to the populator</param>
	    /// <param name="populator">Populator that will handle populating the dataentities</param>
	    /// <param name="persist">If true then each string in <paramref name="persistStrings"/> is executed against the database</param>
	    /// <param name="persistStrings">A list of SQL statements that should be executed on the database. Only executed if <paramref name="persist"/> is true. Should be used to keep procedures or specific system data up to date</param>
	    /// <returns>List of data entities</returns>
	    List<T> UnsecureExecuteReader<T, S>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ref S param, RefDataPopulatorWithEntry<T, S> populator, bool persist = false, List<string> persistStrings = null) where T : new();

		/// <summary>
		/// Executes a stored procedure in unsecure way, as in before a valid Site Manager user is actually logged in. Note that the word _Unsecure will be automatically embedded at back of the procedure name
		/// to make sure that only procedures that were intended for such execution can be executed.
		/// </summary>
		/// <typeparam name="T">Type of dataentity to populate</typeparam>
		/// <param name="server">Name of the SQL server</param>
		/// <param name="windowsAuthentication">Specifies if Windows authentication should be used</param>
		/// <param name="login">Login into the SQL server if windows authentication is not used</param>
		/// <param name="password">Password into the sql server if windows authentication is not used</param>
		/// <param name="databaseName">Name of the database</param>
		/// <param name="connectionType">The type of the connection, Named pipes, TCP/IP, etc</param>
		/// <param name="dataAreaID">The dataarea id</param>
		/// <param name="cmd">The command to be executed. (Note that the keyword _Unsecure will be embedded to the back of the name of the command</param>
		/// <param name="populator">Populator that will handle populating the dataentities</param>
		/// <returns>List of data entities</returns>
		List<T> UnsecureExecuteReader<T>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ConnectionManagerDataPopulator<T> populator) where T : new();

		/// <summary>
		/// Executes a stored procedure in unsecure way, as in before a valid Site Manager user is actually logged in. Note that the word _Unsecure will be automatically embedded at back of the procedure name
		/// to make sure that only procedures that were intended for such execution can be executed.
		/// </summary>
		/// <param name="server">Name of the SQL server</param>
		/// <param name="windowsAuthentication">Specifies if Windows authentication should be used</param>
		/// <param name="login">Login into the SQL server if windows authentication is not used</param>
		/// <param name="password">Password into the sql server if windows authentication is not used</param>
		/// <param name="databaseName">Name of the database</param>
		/// <param name="connectionType">The type of the connection, Named pipes, TCP/IP, etc</param>
		/// <param name="dataAreaID">The dataarea id</param>
		/// <param name="cmd">The command to be executed. (Note that the keyword _Unsecure will be embedded to the back of the name of the command</param>
		void UnsecureNonQuary(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd);

		IUser CurrentUser
		{
			get;
		}

		bool IsHeadOffice { get; }

		/// <summary>
		/// Should return true if connection manager is connecting to central database that can see all stores and all terminals
		/// </summary>
		bool IsCentralDatabase { get; }

		/// <summary>
		/// Should return true if we are in cloud setup. This does not mean that connection manager is directly connecting to the cloud,
		/// like for POS it has local database.
		/// </summary>
		bool IsCloud { get; }

		/// <summary>
		/// Checks if the currently logged in user has a given permission.
		/// Note that permission checks using this variationo of the function comes at zero database cost.
		/// </summary>
		/// <param name="permissionUUID">The permission to check for</param>
		/// <returns>True if the user had the permission, else false</returns>
		bool HasPermission(string permissionUUID);

		/// <summary>
		/// Checks if the currently logged in user has a given permission - Ignoring overrides.
		/// Note that permission checks using this variation of the function comes at zero database cost.
		/// </summary>
		/// <param name="permissionUUID"></param>
		/// <returns></returns>
		bool HasPermissionIgnoreOverrides(string permissionUUID);

		/// <summary>
		/// Checks if a given user with a given password has a given permission.
		/// </summary>
		/// <param name="permissionUUID">The permission to check for</param>
		/// <param name="login">The user name</param>
		/// <param name="password">The password as a SecureString</param>
		/// <returns>True if the user had the permission, else false</returns>
		bool HasPermission(string permissionUUID, string login, SecureString password);

		/// <summary>
		/// Checks if a given user resolved with a token login has a given permission.
		/// </summary>
		/// <param name="permissionUUID">The permission to check for</param>
		/// <param name="tokenHash">The token hash to check for</param>
		/// <param name="tokenIsUser">Returns true if the given token was actually a user and not a valid token</param>
		/// <returns>True if the user had the permission, else false</returns>
		bool HasPermission(string permissionUUID, string tokenHash, out bool tokenIsUser);

		/// <summary>
		/// Verifies a login. This is used for example if client has been temporarily locked and needs to verify the user.
		/// </summary>
		/// <param name="login">The user login</param>
		/// <param name="password">The password as SecureString</param>
		/// <param name="isLockedOut">Returns true if the user has been locked out, else false</param>
		/// <returns>True if the user credentials checked out, else false.</returns>
		bool VerifyCredentials(string login, SecureString password, out bool isLockedOut);

		/// <summary>
		/// Verifies a login. This is used for example if client has been temporarily locked and needs to verify the user.
		/// </summary>
		/// <param name="login">The user login, pass empty string if wanting to use the login name from the token, else only password from the token is used</param>
		/// <param name="tokenHash">Token hash representing the user login</param>
		/// <param name="isLockedOut">Returns true if the user has been locked out, else false</param>
		/// <param name="tokenIsUser">Returns true if the given token was actually a user and not a valid token</param>
		/// <returns>True if the user credentials checked out, else false.</returns>
		bool VerifyCredentials(string login, string tokenHash, out bool isLockedOut, out bool tokenIsUser);

		/// <summary>
		/// Used to temporarly add the permissions that are in the permissions set. Override is stopped with
		/// EndPermissionOverride when the override is no longer valid.
		/// </summary>
		/// <param name="context">A unique identifer for the context this override applies to.</param>
		/// <param name="permissions">The permissions that are to be overridden.</param>
		void BeginPermissionOverride(Guid context, HashSet<string> permissions);
			   
		/// <summary>
		/// Ends the temporary permission override for the given context.
		/// </summary>
		/// <param name="context">A unique identifer for the context this override applies to.</param>
		void EndPermissionOverride(Guid context);

		//RecordIdentifier GenerateNumberFromSequence(ISequenceable sequenceProvider);
		
		ICache Cache
		{
			get;
		}

		IProfileSettings Settings
		{
			get;
		}

		DecimalLimit GetDecimalSetting(DecimalSettingEnum type);

		DecimalLimit GetDecimalSetting(string id);

		bool IsSameAsCurrentPassword(SecureString password);

		bool ChangePassword(SecureString oldPassword, SecureString newPassword);
		bool ChangePasswordForOtherUser(RecordIdentifier userID, SecureString newPassword, bool needPasswordChange);

		/// <summary>
		/// Changes the password for the given user by directly inserting the password hash
		/// </summary>
		/// <param name="userID">ID of the user</param>
		/// <param name="passwordHash">The hash of the new password</param>
		/// <param name="needPasswordChange">Set flag on the user that indecates if he needs to change his password</param>
		/// <param name="expiresDate">Sets the expire date</param>
		/// <param name="lastChangeTime">Sets the last change time</param>
		/// <param name="generateActions">Indicates wether replication actions should be generated</param>
		/// <returns></returns>
		bool ChangePasswordHashForOtherUser(RecordIdentifier userID, string passwordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, bool generateActions = true);

		/// <summary>
		/// Gets the password for info the given user
		/// </summary>
		/// <param name="userID">ID of the user</param>
		/// <param name="passwordHash">The hash of the new password</param>
		/// <param name="expiresDate">The date when the password expires</param>
		/// <param name="lastChangeTime">The last time the password was changed</param>
		/// <returns></returns>
		void GetUserPasswordInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime);

		/// <summary>
		/// Returns the login, hashed password and if the user is an Active Directory user for an active (non-deleted) user.
		/// </summary>
		void GetActiveUserInfo(RecordIdentifier userID, out string login, out string passwordHash, out bool isDomainUser);

		IService Service(ServiceType serviceType);
		void SetService(ServiceType serviceType, IService service);

		bool ServiceIsLoaded(ServiceType serviceType);

		IConnectionManagerTransaction CreateTransaction();
        IConnectionManagerTransaction CreateTransaction(IsolationLevel isolationLevel);
		
		string SiteServiceAddress { get; set; }
		UInt16 SiteServicePortNumber { get; set; }
		IErrorLog ErrorLogger { get; set; }

		/// <summary>
		/// Gets or sets the instance of <see cref="ServiceFactory"/> to use when loading services
		/// </summary>
		ServiceFactory ServiceFactory { get; set; }

		IConnectionManagerTemporary CreateTemporaryConnection();


		LoginResult ReLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
							string databaseName, string login, ConnectionType connectionType,
							ConnectionUsageType connectionUsage, string dataAreaID);

		LoginResult Login(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
						  string databaseName, string login, SecureString password, ConnectionType connectionType,
						  ConnectionUsageType connectionUsage, string dataAreaID);

		LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
			SecureString serverPassword, string databaseName, string login,SecureString password,
			ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID);

		LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
	SecureString serverPassword, string databaseName, Guid login, 
	ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID);
		
		LoginResult TokenLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword,
						  string databaseName, string tokenHash, ConnectionType connectionType,
						  ConnectionUsageType connectionUsage, string dataAreaID, out string login, out string passwordHash);

		LoginResult SwitchUser(string login, SecureString password, string passwordHash = "");

		LoginResult TokenSwitchUser(string tokenHash);

		void LogOff();

		LoginResult TestConnection(string server, bool windowsAuthentication,
								   string login, SecureString password,
								   string databaseName, ConnectionType connectionType, string dataAreaID);

		void RunDatabaseUpdateScript(string script);
		void RunDatabaseUpdateScriptFromResource(Assembly assembly, string resourceName);

		/// <summary>
		/// Indicates wether the given user needs to change password
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		/// <returns></returns>
		bool UserNeedsPasswordChange(RecordIdentifier userID);

		/// <summary>
		/// Locks the given user
		/// </summary>
		/// <param name="userID">The ID of the user</param>
		void LockUser(RecordIdentifier userID);

		/// <summary>
		/// Returns a hash of the given password
		/// </summary>
		/// <param name="password">The password string to hash</param>
		/// <returns></returns>
		string GetPasswordHash(SecureString password);

		/// <summary>
		/// Explicitly opens a connection to the database
		/// </summary>
		/// <param name="server">Name of the SQL server</param>
		/// <param name="windowsAuthentication">Specifies if Windows authentication should be used</param>
		/// <param name="serverLogin">Login into the SQL server if windows authentication is not used</param>
		/// <param name="serverPassword">Password in to the SQL server if windows authentication is not used</param>
		/// <param name="databaseName">Name of the database</param>
		/// <param name="connectionType">The type of the connection, Named pipes, TCP/IP, etc</param>
		/// <param name="dataAreaID">The dataarea id</param>
		/// <returns></returns>
		bool OpenDatabaseConnection(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, ConnectionType connectionType, string dataAreaID);

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		void SetConnectionEnvironment(bool isCloud, bool isCentralDatabase);


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		void SetTouchClient(bool value);


    }
}
