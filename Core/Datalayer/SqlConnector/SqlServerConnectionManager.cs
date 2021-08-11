using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security;

using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Caching;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.EventArguments;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector
{
	public class SqlServerConnectionManager : ConnectionManagerBase
	{
		private SqlServerConnection connection;
		internal bool profileWasUpdated;
		
		internal string siteServiceAddress;
		internal UInt16 siteServicePort;
		private ConnectionUsageType connectionUsage;

		

		// There should only be one constructor and it should be private
		protected SqlServerConnectionManager()
		{
			profileWasUpdated = false;
			connection = null;
			siteServiceAddress = null;
			Settings = new ProfileSettings();
			
		}

		// This method is called by the ConnectionManagerFactory via Reflection, so needs to be excluded from obfuscation
		[Obfuscation(Exclude = true)]
		private static IConnectionManager Create()
		{
			return new SqlServerConnectionManager();
		}

		protected SqlServerConnectionManager(SqlServerConnectionManager connectionManager)
			: base(connectionManager)
		{
			Settings = connectionManager.Settings;
		}

		public override LoginResult TestConnection(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID)
		{
			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, DisableReplicationActionCreation);
		 
				connection.Open();
				connection.Close();
			}
			catch (SqlException e)
			{
				return (e.Number == 18456) ? LoginResult.UserAuthenticationFailed : LoginResult.UnknownServerError;
			}

			return LoginResult.Success;
		}

		public override List<T> UnsecureExecuteReader<T,S>(string server,
			bool windowsAuthentication, 
			string login, 
			SecureString password, 
			string databaseName, 
			ConnectionType connectionType, 
			string dataAreaID, 
			IDbCommand cmd, 
			ref S param,
			RefDataPopulatorWithEntry<T,S> populator,
			bool persist = false,
			List<string> persistStrings = null)
		{
			T item;
			List<T> result = new List<T>();
			bool connectionOpened = false;
			IDataReader dr = null;

			if (cmd.CommandType != System.Data.CommandType.StoredProcedure)
			{
				throw new NotSupportedException("Only stored procedure is supported");
			}
			if (persist)
			{
				foreach (string persistString in persistStrings)
				{
					using (var cmd2 = new SqlCommand(persistString))
					{
						cmd2.CommandType = CommandType.Text;

						connection.ExecuteNonQuery(cmd2, true, CommandType.Text);

					}
				}
			   
			}
			cmd.CommandText = cmd.CommandText + "_Unsecure";

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, DisableReplicationActionCreation);

				try
				{
					connection.Open();
				}
				catch
				{
					//Retry with 2 minutes timeout
					CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName,
						connectionType, dataAreaID, true);
					connection.Open();
				}

				connectionOpened = true;
				
				try
				{
					dr = connection.ExecuteReader(cmd);

					while (dr.Read())
					{
						item = new T();

						populator(this,dr, item, ref param);

						result.Add(item);
					}
				}
				finally
				{
					if (dr != null)
					{
						dr.Close();
						dr.Dispose();
					}
				}
			}
			catch(SqlException)
			{
				throw;
			}
			finally
			{
				if (connectionOpened)
				{
					connection.Close();
				}
			}

			return result;
		}

		public override List<T> UnsecureExecuteReader<T>(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd, ConnectionManagerDataPopulator<T> dataPopulator)
		{
			T item;
			List<T> result = new List<T>();
			bool connectionOpened = false;
			IDataReader dr = null;

			if (cmd.CommandType != System.Data.CommandType.StoredProcedure)
			{
				throw new NotSupportedException("Only stored procedure is supported");
			}

			cmd.CommandText = cmd.CommandText + "_Unsecure";

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, DisableReplicationActionCreation);

				try
				{
					connection.Open();
				}
				catch
				{
					//Retry with 2 minutes timeout
					CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName,
						connectionType, dataAreaID, true);
					connection.Open();
				}

				connectionOpened = true;

				try
				{
					dr = connection.ExecuteReader(cmd);

					while (dr.Read())
					{
						item = new T();

						dataPopulator( dr, item);

						result.Add(item);
					}
				}
				finally
				{
					if (dr != null)
					{
						dr.Close();
						dr.Dispose();
					}
				}
			}
			catch (SqlException)
			{
				throw;
			}
			finally
			{
				if (connectionOpened)
				{
					connection.Close();
				}
			}

			return result;
		}

		public override void UnsecureNonQuary(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, IDbCommand cmd)
		{
			
			bool connectionOpened = false;

			if (cmd.CommandType != System.Data.CommandType.StoredProcedure)
			{
				throw new NotSupportedException("Only stored procedure is supported");
			}

			cmd.CommandText = cmd.CommandText + "_Unsecure";

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID);
				connection.Open();

				connectionOpened = true;
				connection.ExecuteNonQuery(cmd, true);

			}
			finally
			{
				if (connectionOpened)
				{
					connection.Close();
				}
			}
		}

		public override bool UserNeedsPasswordChange(RecordIdentifier userID)
		{
			SqlParameter needsPasswordChange;
			bool result;

			using (SqlCommand cmd = new SqlCommand("spSECURITY_UserNeedsPasswordChange_1_0"))
			{
				SqlServerParameters.MakeParam(cmd, "userID", (Guid) userID, SqlDbType.UniqueIdentifier);

				needsPasswordChange = SqlServerParameters.MakeParam(cmd, "NeedPasswordChange", false, SqlDbType.Bit, ParameterDirection.Output);

				connection.ExecuteNonQuery(cmd, true);

				result = IsNull(needsPasswordChange) ? false : (bool)needsPasswordChange.Value;

				return result;
			}
		}

		private static bool IsNull(IDataParameter value)
		{
			return value == null || value.Value == null || value.Value == DBNull.Value;
		}

		public override bool ChangePasswordHashForOtherUser(RecordIdentifier userID, string passwordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, bool generateActions = true)
		{
			return UserData.ChangePasswordHashForOtherUser(this, userID, passwordHash, needPasswordChange, expiresDate, lastChangeTime, generateActions);
		}

		public override void GetUserPasswordInfo(RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime)
		{
			UserData.GetUserPasswordInfo(this, userID, out passwordHash, out expiresDate, out lastChangeTime);
		}

		/// <summary>
		/// Returns the login, hashed password and if the user is an Active Directory user for an active (non-deleted) user.
		/// </summary>
		public override void GetActiveUserInfo(RecordIdentifier userID, out string login, out string passwordHash, out bool isDomainUser)
		{
			UserData.GetActiveUserInfo(this, userID, out login, out passwordHash, out isDomainUser);
		}

		public override void LockUser(RecordIdentifier userID)
		{

			using (SqlCommand cmd = new SqlCommand("spSECURITY_LockUser_1_0"))
			{
				SqlServerParameters.MakeParam(cmd, "userID", (Guid)userID, SqlDbType.UniqueIdentifier);
				
				connection.ExecuteNonQuery(cmd, false);
			}
		}

		void connection_ConnectionLost(object sender,ConnectionLostEventArguments args)
		{
			OnConnectionLost(sender, args);
		}


		public override LoginResult ReLogin(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, string login, ConnectionType connectionType, ConnectionUsageType connectionUsage,string dataAreaID)
		{
			if (connection == null)
			{
				return LoginResult.UserAuthenticationFailed;
			}

			return Login(server, windowsAuthentication, serverLogin, serverPassword, databaseName, login, PasswordHash,Password, connectionType, connectionUsage, dataAreaID);
		}

		public override LoginResult TokenLogin(string server, 
											   bool windowsAuthentication, 
											   string serverLogin, 
											   SecureString serverPassword, 
											   string databaseName, 
											   string tokenHash, 
											   ConnectionType connectionType, 
											   ConnectionUsageType connectionUsage, 
											   string dataAreaID,
											   out string login,
											   out string passwordHash)
		{
			SqlCommand cmd;
			SqlParameter tokenIsUserParam;
			SqlParameter validTokenParam;
			SqlParameter loginParam;
			SqlParameter passwordHashParam;

			login = "";
			passwordHash = "";

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, serverLogin, serverPassword, databaseName, connectionType, dataAreaID);
			}
			catch (SqlException)
			{
				return LoginResult.UnknownServerError;
			}

			try
			{
				cmd = new SqlCommand("spSECURITY_LoginFromTokenLogin_1_0");


				SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "TokenLoginHash", tokenHash);

				tokenIsUserParam = SqlServerParameters.MakeParam(cmd, "tokenIsUser", false, SqlDbType.Bit, ParameterDirection.Output);
				validTokenParam = SqlServerParameters.MakeParam(cmd, "validToken", false, SqlDbType.Bit, ParameterDirection.Output);

				loginParam = SqlServerParameters.MakeParam(cmd, "login", false, SqlDbType.NVarChar, ParameterDirection.Output, 32);
				passwordHashParam = SqlServerParameters.MakeParam(cmd, "passwordHash", false, SqlDbType.NVarChar, ParameterDirection.Output, 40);

				connection.ExecuteNonQuery(cmd, true);

				connection.Close();
				connection = null;

				login = loginParam.Value is string ?(string)loginParam.Value : "";
				passwordHash = loginParam.Value is string ? (string)passwordHashParam.Value : "";

				if ((bool)tokenIsUserParam.Value)
				{
					return LoginResult.TokenIsUser;
				}
				else
				{
					if ((bool)validTokenParam.Value)
					{
						return Login(server, windowsAuthentication, serverLogin, serverPassword, databaseName, (string)loginParam.Value, (string)passwordHashParam.Value,null, connectionType, connectionUsage, dataAreaID);
					}
					else
					{
						return LoginResult.TokenNotFound;
					}
				}
			}
			catch (Exception)
			{
				if (connection != null)
				{
					connection.Close();
					connection = null;
				}
			}
			return LoginResult.TokenNotFound;
		}



		private LoginResult Login(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, string login, string passwordHash, SecureString password, ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{
			User user = null;
			SqlServerUserProfile userProfile;
			SqlServerUserProfile newUserProfile = null;
			IDataReader dr = null;
			SqlParameter successPrm;
			SqlParameter isADUserPrm;
			SqlParameter userGuidPrm;
			SqlParameter needPwChange;
			SqlParameter expiresDays;
			SqlParameter isLockedOut;
			SqlParameter loginDisabled;
			SqlParameter isServerUser = null;
			SqlParameter domain;
			SqlParameter adOffLinePassed;
			SqlParameter returnedPermissions;
			SqlParameter staffID;
            SqlCommand cmd;
			string profileHash;
			bool success;
			LoginResult result;

			profileWasUpdated = false;

			result = LoginResult.UserAuthenticationFailed;
			loginDisabled = new SqlParameter("LoginDisabled", SqlDbType.Bit, 1);
			loginDisabled.Value = false;
            staffID = new SqlParameter("staffID", SqlDbType.NVarChar, 40);
            staffID.Value = "";
            userProfile = SqlServerUserProfile.LoadProfile(login);
			profileHash = userProfile.HashCode;
			this.connectionUsage = connectionUsage;

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, serverLogin, serverPassword, databaseName, connectionType, dataAreaID);
			}
			catch (SqlException)
			{
				return LoginResult.UnknownServerError;
			}

			try
			{
                cmd = new SqlCommand();
                LoginProcedureVersion version = GetLoginProcedureVersion(cmd);
                cmd.Dispose();
                cmd = new SqlCommand(LoginProcedureVersionHelper.ToString(version));

				SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "Login", login);
				SqlServerParameters.MakeParam(cmd, "PasswordHash", passwordHash);
				SqlServerParameters.MakeParam(cmd, "LocalProfileHash", profileHash);

				successPrm = SqlServerParameters.MakeParam(cmd, "Success", false, SqlDbType.Bit, ParameterDirection.Output);
				isADUserPrm = SqlServerParameters.MakeParam(cmd, "IsActiveDirectoryUser", false, SqlDbType.Bit, ParameterDirection.Output);
				userGuidPrm = SqlServerParameters.MakeParam(cmd, "UserGUID", Guid.Empty, SqlDbType.UniqueIdentifier, ParameterDirection.Output);
				needPwChange = SqlServerParameters.MakeParam(cmd, "NeedPasswordChange", false, SqlDbType.Bit, ParameterDirection.Output);
				expiresDays = SqlServerParameters.MakeParam(cmd, "PasswordExpiresDays", 0, SqlDbType.Int, ParameterDirection.Output);
				isLockedOut = SqlServerParameters.MakeParam(cmd, "IsLockedOut", false, SqlDbType.Bit, ParameterDirection.Output);
				returnedPermissions = SqlServerParameters.MakeParam(cmd, "ReturnedPermissions", false, SqlDbType.Bit, ParameterDirection.Output);
				domain = SqlServerParameters.MakeParam(cmd, "Domain", false, SqlDbType.NVarChar, ParameterDirection.Output, 50);
				adOffLinePassed = SqlServerParameters.MakeParam(cmd, "adOffLinePassed", false, SqlDbType.Bit, ParameterDirection.Output);
				isServerUser = SqlServerParameters.MakeParam(cmd, "isServerUser", false, SqlDbType.Bit, ParameterDirection.Output);

                if (version >= LoginProcedureVersion.Version1_2)
				{
					loginDisabled = SqlServerParameters.MakeParam(cmd, "LoginDisabled", false, SqlDbType.Bit, ParameterDirection.Output);
				}

                if (version >= LoginProcedureVersion.Version1_3)
                {
                    staffID = SqlServerParameters.MakeParam(cmd, "staffID", "", SqlDbType.NVarChar, ParameterDirection.Output, 40);
                }

                dr = connection.ExecuteReader(cmd);

				// Silly SQL Server rules require us to read the DataReader before we actually attempt
				// to read the output parameters.
				if (dr != null)
				{
					// The database may have decided to send us a new profile which we must accept
					// We will check once we have access to the output parameters if it was actually a user profile
					newUserProfile = new SqlServerUserProfile((SqlDataReader)dr);

					// Here we need to remember that the user might not be valid since it
					// could be a active directory user that we cannot validate at this point
					// so we cannot save the profile at this point.

					dr.Close();
					dr = null;
				}

				// We have to check for this because we may have gotten a bogus result set above. 
				if (!(bool)returnedPermissions.Value)
				{
					newUserProfile = null;
				}

				success = (bool)successPrm.Value;

				if(success)
				{
					if (password != null && (bool)isADUserPrm.Value)
					{
						// We need to validate against Active directory
						success = ActiveDirectory.AuthenticateUser(login, password, (string)domain.Value);

						if (success)
						{
							// Send the latest valid active directory login hash to the Server
							cmd = new SqlCommand("spSECURITY_SetDomainUserPasswordHash_1_0");

							SqlServerParameters.MakeParam(cmd, "UserGUID", (Guid)userGuidPrm.Value, SqlDbType.UniqueIdentifier);
							SqlServerParameters.MakeParam(cmd, "dataareaID", connection.DataAreaId);
							SqlServerParameters.MakeParam(cmd, "newPasswordHash", passwordHash);

							// We say here that we are only reading since this is not supposed to be added to the replication
							connection.ExecuteNonQuery(cmd, true);
						}
						else if ((bool)adOffLinePassed.Value == true)
						{
							// Off line active directory login
							success = true;
						}
					}

					user = new User(
							login,
							(Guid)userGuidPrm.Value,
							(string)staffID.Value,
                            (bool)isADUserPrm.Value,
							newUserProfile != null && newUserProfile.Settings.Count > 0 ? newUserProfile : userProfile,
							(bool)needPwChange.Value,
							(int)expiresDays.Value,
							(bool)isLockedOut.Value);

					SetUserID((Guid)userGuidPrm.Value, version >= LoginProcedureVersion.Version1_2 ? (bool)isServerUser.Value : false);

					if (connectionUsage != ConnectionUsageType.UsageService && connection.IsServerUser ||
						connectionUsage == ConnectionUsageType.UsageService && !connection.IsServerUser)
					{
						result = LoginResult.UserDoesNotMatchConnectionIntent;
						return result;
					}

					// If we got a new user profile from the server then we need to let the server
					// know about the new Hash code, and we need to save the user profile
					if (newUserProfile != null)
					{
						profileWasUpdated = true;

						cmd = new SqlCommand("spSECURITY_SetLocalProfileHash_1_0");

						SqlServerParameters.MakeParam(cmd, "Login", login);
						SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
						SqlServerParameters.MakeParam(cmd, "LocalProfileHash", newUserProfile.HashCode);

						connection.ExecuteNonQuery(cmd, false);

						newUserProfile.Save(login);

						user.Profile = newUserProfile;
					}

					PasswordHash = passwordHash;
					Password = password;

					((ProfileSettings)Settings).Populate(user.Profile.Settings, Cache);

					if (User.WriteAudit)
					{
						UserData.AddToLoginLog(this, user.UserName, user.ID, "Login");
					}

					CurrentUser = user;

					result = LoginResult.Success;
				}
				else
				{
					result = LoginFailed(login, (bool)isLockedOut.Value, (bool)loginDisabled.Value, result);

                    //Set current user to properly check for password change on HO
                    CurrentUser = new User(
                            login,
                            (Guid)userGuidPrm.Value,
                            (string)staffID.Value,
                            (bool)isADUserPrm.Value,
                            newUserProfile != null && newUserProfile.Settings.Count > 0 ? newUserProfile : userProfile,
                            (bool)needPwChange.Value,
                            (int)expiresDays.Value,
                            (bool)isLockedOut.Value);
                }
			}
			catch (Exception ex)
			{
				if (ex is SqlException)
				{
					if (((SqlException)ex).Number == 53)
					{
						result = LoginResult.CouldNotConnectToDatabase;
					}
					else
					{
						result = LoginResult.UnknownServerError;
					}
				}
				else
				{
					result = LoginResult.UnknownServerError;
				}
			}
			finally
			{
				if (dr != null)
				{
					dr.Close();
				}

				if (result != LoginResult.Success)
				{
					try
					{
						connection.Close();
					}
					catch (Exception)
					{
						//TODO: we should at least log it as a Warn
					}

					connection = null;
				}
			}			

			return result;
		}

		
		public override LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
			SecureString serverPassword, string databaseName, string login, SecureString password,
			ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{

			Guid userGuid = Guid.Empty;
			SqlServerUserProfile userProfile = SqlServerUserProfile.LoadProfile("admin");
			RecordIdentifier staffID = "";
			LoginResult result;
			profileWasUpdated = false;
			bool isServerUser = false;

			result = LoginResult.UserAuthenticationFailed;

			this.connectionUsage = connectionUsage;

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, serverLogin, serverPassword, databaseName,
					connectionType, dataAreaID);

				LoginProcedureVersion version;
				using (var cmd = new SqlCommand())
				{
					version = GetLoginProcedureVersion(cmd);
				}

				using (var cmd = new SqlCommand(" select GUID, isServerUser, STAFFID from vSECURITY_AllUsers_1_0 where login = @login"))
				{
					cmd.CommandType = CommandType.Text;
					SqlServerParameters.MakeParam(cmd, "login", login);

					using (var reader = connection.ExecuteReader(cmd, CommandType.Text))
					{
						if (reader.Read())
						{
							userGuid = reader["GUID"] == DBNull.Value ? Guid.Empty : (Guid)reader["GUID"];
							isServerUser = reader["isServerUser"] != DBNull.Value && (bool)reader["isServerUser"];
							staffID = reader["STAFFID"] == DBNull.Value ? "" : (string)reader["STAFFID"];
						}
					}
				}

				if (userGuid == Guid.Empty)
				{
					throw new Exception("UserContext not found");
				}

				SetUserID(userGuid, version >= LoginProcedureVersion.Version1_2 && isServerUser);

				result = LoginResult.Success;
			}
			catch (SqlException)
			{
				return LoginResult.UnknownServerError;
			}
			isAdmin = true;
			CurrentUser = new User(login, userGuid, staffID, false, userProfile, false, 0);
			return result;
		}

		public override LoginResult AdminLogin(string server, bool windowsAuthentication, string serverLogin,
		  SecureString serverPassword, string databaseName, Guid login, 
		  ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{
			SqlServerUserProfile userProfile = SqlServerUserProfile.LoadProfile("admin");
			string userName = "";
            RecordIdentifier staffID = "";
			bool isServerUser = false;
			LoginResult result;
			profileWasUpdated = false;

			result = LoginResult.UserAuthenticationFailed;

			this.connectionUsage = connectionUsage;

			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, serverLogin, serverPassword, databaseName,
					connectionType, dataAreaID);
				try
				{
					connection.Open();
					using (var cmd = connection.CreateCommand("select 1"))
					{
						var scalar = connection.ExecuteScalar(cmd);
					}

					LoginProcedureVersion version;
					using (var cmd = new SqlCommand())
					{
						version = GetLoginProcedureVersion(cmd);
					}

					using (var cmd = new SqlCommand(" select Login, isServerUser, STAFFID from vSECURITY_AllUsers_1_0 where GUID = @guid"))
					{
						cmd.CommandType = CommandType.Text;
						SqlServerParameters.MakeParam(cmd, "guid", login);

						using (var reader = connection.ExecuteReader(cmd, CommandType.Text))
						{
							if (reader.Read())
							{
								userName = reader["Login"] == DBNull.Value ? "" : (string)reader["Login"];
								isServerUser = reader["isServerUser"] != DBNull.Value && (bool)reader["isServerUser"];
								staffID = reader["STAFFID"] == DBNull.Value ? "" : (string)reader["STAFFID"];
							}
						}
					}

					if (userName == "")
					{
						userName = "admin";
                        staffID = "admin";
					}

					if (login != Guid.Empty)
					{
						SetUserID(login, version >= LoginProcedureVersion.Version1_2 && isServerUser);
					}
					result = LoginResult.Success;
				}
				catch (Exception)
				{
					return LoginResult.CouldNotConnectToDatabase;
				}
				finally
				{
					connection.Close();
				}
			}
			catch (SqlException)
			{
				return LoginResult.UnknownServerError;
			}
			isAdmin = true;
			CurrentUser = new User(userName, login, staffID, false, userProfile, false, 0);
			return result;
		}

		protected void SetUserID(Guid userID, bool isServerUser)
		{
			connection.UserID = userID;
			connection.IsServerUser = isServerUser;
		}

		protected virtual void CreateSqlServerConnection(string server, bool windowsAuthentication, string login, SecureString password, string databaseName, ConnectionType connectionType, string dataAreaID, bool retry = false)
		{
			connection = new SqlServerConnection(server, windowsAuthentication, login, password, databaseName, connectionType, dataAreaID, DisableReplicationActionCreation, retry);
			ConnectLostHandler();
		}

		protected virtual void CreateSqlServerConnection(string connectionString, string dataAreaID)
		{
			connection = new SqlServerConnection(connectionString, dataAreaID, DisableReplicationActionCreation);
			ConnectLostHandler();
		}

		protected void ConnectLostHandler()
		{
			connection.ConnectionLost += new ConnectionLostHandler(connection_ConnectionLost);
		}

		protected ConnectionUsageType ConnectionUsage
		{
			get
			{
				return connectionUsage;
			}
			set
			{
				connectionUsage = value;
			}
		}

		public override LoginResult SwitchUser(string login, SecureString password, string passwordHash = "")
		{
			User user = null;
			SqlServerUserProfile userProfile;
			SqlServerUserProfile newUserProfile = null;
			IDataReader dr = null;
			SqlParameter successPrm;
			SqlParameter isADUserPrm;
			SqlParameter userGuidPrm;
			SqlParameter needPwChange;
			SqlParameter expiresDays;
			SqlParameter isLockedOut;
			SqlParameter loginDisabled;
			SqlParameter isServerUser = null;
			SqlParameter domain;
			SqlParameter adOffLinePassed;
			SqlParameter returnedPermissions;
			SqlParameter staffID;
            SqlCommand cmd;
			string profileHash;
			bool success;
			LoginResult result;

			profileWasUpdated = false;

			result = LoginResult.UserAuthenticationFailed;
			loginDisabled = new SqlParameter("LoginDisabled", SqlDbType.Bit, 1);
			loginDisabled.Value = false;
            staffID = new SqlParameter("staffID", SqlDbType.NVarChar, 40);
            staffID.Value = "";
            userProfile = SqlServerUserProfile.LoadProfile(login);
			profileHash = userProfile.HashCode;
			if (passwordHash == "")
			{
				passwordHash = HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66");
			}

			try
			{
                cmd = new SqlCommand();
                LoginProcedureVersion version = GetLoginProcedureVersion(cmd);
                cmd.Dispose();
                cmd = new SqlCommand(LoginProcedureVersionHelper.ToString(version));

                SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "Login", login);
				SqlServerParameters.MakeParam(cmd, "PasswordHash", passwordHash);
				SqlServerParameters.MakeParam(cmd, "LocalProfileHash", profileHash);

				successPrm = SqlServerParameters.MakeParam(cmd, "Success", false, SqlDbType.Bit, ParameterDirection.Output);
				isADUserPrm = SqlServerParameters.MakeParam(cmd, "IsActiveDirectoryUser", false, SqlDbType.Bit, ParameterDirection.Output);
				userGuidPrm = SqlServerParameters.MakeParam(cmd, "UserGUID", Guid.Empty, SqlDbType.UniqueIdentifier, ParameterDirection.Output);
				needPwChange = SqlServerParameters.MakeParam(cmd, "NeedPasswordChange", false, SqlDbType.Bit, ParameterDirection.Output);
				expiresDays = SqlServerParameters.MakeParam(cmd, "PasswordExpiresDays", 0, SqlDbType.Int, ParameterDirection.Output);
				isLockedOut = SqlServerParameters.MakeParam(cmd, "IsLockedOut", false, SqlDbType.Bit, ParameterDirection.Output);
				returnedPermissions = SqlServerParameters.MakeParam(cmd, "ReturnedPermissions", false, SqlDbType.Bit, ParameterDirection.Output);
				domain = SqlServerParameters.MakeParam(cmd, "Domain", false, SqlDbType.NVarChar, ParameterDirection.Output, 50);
				adOffLinePassed = SqlServerParameters.MakeParam(cmd, "adOffLinePassed", false, SqlDbType.Bit, ParameterDirection.Output);
				isServerUser = SqlServerParameters.MakeParam(cmd, "isServerUser", false, SqlDbType.Bit, ParameterDirection.Output);

                if (version >= LoginProcedureVersion.Version1_2)
				{
					loginDisabled = SqlServerParameters.MakeParam(cmd, "LoginDisabled", false, SqlDbType.Bit, ParameterDirection.Output);
				}

                if (version >= LoginProcedureVersion.Version1_3)
                {
                    staffID = SqlServerParameters.MakeParam(cmd, "staffID", "", SqlDbType.NVarChar, ParameterDirection.Output, 40);
                }

                dr = connection.ExecuteReader(cmd);

				// Silly SQL Server rules require us to read the DataReader before we actually attempt
				// to read the output parameters.
				if (dr != null)
				{
					// The database may have decided to send us a new profile which we must accept
					// We will check once we have access to the output parameters if it was actually a user profile
					newUserProfile = new SqlServerUserProfile((SqlDataReader)dr);

					// Here we need to remember that the user might not be valid since it
					// could be a active directory user that we cannot validate at this point
					// so we cannot save the profile at this point.

					dr.Close();
					dr = null;
				}

				// We have to check for this because we may have gotten a bogus result set above. 
				if (!(bool)returnedPermissions.Value)
				{
					newUserProfile = null;
				}

				success = (bool)successPrm.Value;


				if (success && password != null && (bool)isADUserPrm.Value)
				{
					// We need to validate against Active directory

					success = ActiveDirectory.AuthenticateUser(login, password, (string)domain.Value);

					if (success)
					{
						// Send the latest valid active directory login hash to the Server
						cmd = new SqlCommand("spSECURITY_SetDomainUserPasswordHash_1_0");

						SqlServerParameters.MakeParam(cmd, "UserGUID", (Guid)userGuidPrm.Value, SqlDbType.UniqueIdentifier);
						SqlServerParameters.MakeParam(cmd, "dataareaID", connection.DataAreaId);
						SqlServerParameters.MakeParam(cmd, "newPasswordHash", passwordHash);

						// We say here that we are only reading since this is not supposed to be added to the replication
						connection.ExecuteNonQuery(cmd, true);
					}
					else if ((bool)adOffLinePassed.Value)
					{
						// Off line active directory login
						success = true;
					}
				}

				if (success)
				{
					if (connectionUsage != ConnectionUsageType.UsageService && connection.IsServerUser ||
						connectionUsage == ConnectionUsageType.UsageService && !connection.IsServerUser)
					{
						result = LoginResult.UserDoesNotMatchConnectionIntent;
					}
					else
					{
						user = new User(
							login,
							(Guid)userGuidPrm.Value,
							(string)staffID.Value,
                            (bool)isADUserPrm.Value,
							newUserProfile ?? userProfile,
							(bool)needPwChange.Value,
							(int)expiresDays.Value);

						connection.UserID = (Guid)userGuidPrm.Value;
						connection.IsServerUser = version >= LoginProcedureVersion.Version1_2 && (bool)isServerUser.Value;

						// If we got a new user profile from the server then we need to let the server
						// know about the new Hash code, and we need to save the user profile
						if (newUserProfile != null)
						{
							profileWasUpdated = true;

							cmd = new SqlCommand("spSECURITY_SetLocalProfileHash_1_0");

							SqlServerParameters.MakeParam(cmd, "Login", login);
							SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
							SqlServerParameters.MakeParam(cmd, "LocalProfileHash", newUserProfile.HashCode);

							connection.ExecuteNonQuery(cmd, false);

							newUserProfile.Save(login);

							user.Profile = newUserProfile;
						}

						result = LoginResult.Success;

						PasswordHash = passwordHash;
						Password = password;

						if (User.WriteAudit)
						{
							UserData.AddToLoginLog(this, user.UserName, user.ID, "Login");
						}
					}
				}
				else
				{
					result = LoginFailed(login, (bool)isLockedOut.Value, (bool)loginDisabled.Value, result);
				}
			}
			catch (Exception ex)
			{
				if (ex is SqlException)
				{
					if (((SqlException)ex).Number == 53)
					{
						return LoginResult.CouldNotConnectToDatabase;
					}
					else
					{
						return LoginResult.UnknownServerError;
					}
				}
				else
				{
					return LoginResult.UnknownServerError;
				}
			}
			finally
			{
				if (dr != null)
				{
					dr.Close();
				}

			}

			if (result == LoginResult.Success)
			{
				((ProfileSettings)Settings).Populate(user.Profile.Settings, Cache);

				CurrentUser = user;
			}

			return result;
		}

		public override LoginResult TokenSwitchUser(string tokenHash)
		{
			SqlCommand cmd;
			SqlParameter tokenIsUserParam;
			SqlParameter validTokenParam;
			SqlParameter loginParam;
			SqlParameter passwordHashParam;

			try
			{
				cmd = new SqlCommand("spSECURITY_LoginFromTokenLogin_1_0");


				SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "TokenLoginHash", tokenHash);

				tokenIsUserParam = SqlServerParameters.MakeParam(cmd, "tokenIsUser", false, SqlDbType.Bit, ParameterDirection.Output);
				validTokenParam = SqlServerParameters.MakeParam(cmd, "validToken", false, SqlDbType.Bit, ParameterDirection.Output);

				loginParam = SqlServerParameters.MakeParam(cmd, "login", false, SqlDbType.NVarChar, ParameterDirection.Output, 32);
				passwordHashParam = SqlServerParameters.MakeParam(cmd, "passwordHash", false, SqlDbType.NVarChar, ParameterDirection.Output, 40);

				connection.ExecuteNonQuery(cmd, true);

				if ((bool)tokenIsUserParam.Value)
				{
					return LoginResult.TokenIsUser;
				}
				else
				{
					if ((bool)validTokenParam.Value)
					{
						return SwitchUser((string)loginParam.Value, null, (string)passwordHashParam.Value);
					}
					else
					{
						return LoginResult.TokenNotFound;
					}
				}
			}
			catch (Exception)
			{
				if (connection != null)
				{
					connection.Close();
					connection = null;
				}
			}

			return LoginResult.TokenNotFound;
		}

		public override LoginResult Login(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, string login, SecureString password, ConnectionType connectionType, ConnectionUsageType connectionUsage, string dataAreaID)
		{       
			return Login(server, windowsAuthentication, serverLogin, serverPassword, databaseName, login, HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66"),password, connectionType, connectionUsage, dataAreaID);
		}

		public override void LogOff()
		{
			if (CurrentUser == null||isAdmin)
				return;

			if (!CurrentUser.SessionClosed)
			{
				if (User.WriteAudit)
				{
					DataProviders.UserData.AddToLoginLog(this, CurrentUser.UserName, CurrentUser.ID, "Logout");
				}

				CurrentUser.SessionClosed = true;

				connection.Close();
				connection = null;
			}
		}

		private static LoginResult LoginFailed(string userName, bool isLockedOut, bool loginDisabled, LoginResult result)
		{
			/* Here we have no normal means to determine if auditing is on or off
			 * because Audit settings come with the User profile and without successful
			 * login then we have no user profile for this reason we have no other choice than
			 * to look this value up in the database*/

			//writeAudit = (SystemAdministration.GetSystemSetting(new Guid(SettingsConstants.WriteAuditing)) == "1");

			if (loginDisabled)
			{
				result = LoginResult.LoginDisabled;

			}
			else if (isLockedOut)
			{
				result = LoginResult.UserLockedOut;
			}

			return result;
		}

        private LoginProcedureVersion GetLoginProcedureVersion(SqlCommand cmd)
        {
            cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_3]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) THEN 2
                                               WHEN EXISTS (SELECT 1 FROM dbo.sysobjects where id = object_id(N'[dbo].[spSECURITY_Login_1_2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) THEN 1
                                               ELSE 0 END AS RESULT";
            cmd.CommandType = CommandType.Text;

            LoginProcedureVersion version = (LoginProcedureVersion)(int)connection.ExecuteScalar(cmd);

            return version;
        }

		public bool ProfileWasUpdated
		{
			get
			{
				return profileWasUpdated;
			}
		}

		protected override bool DoChangePassword(SecureString oldPassword, SecureString newPassword)
		{
			return UserData.ChangePassword(this, oldPassword, newPassword);
		}

		protected override bool DoChangePasswordForOtherUser(RecordIdentifier userID, SecureString newPassword,
			bool needPasswordChange)
		{
			return UserData.ChangePasswordForOtherUser(this, userID, newPassword, needPasswordChange);
		}

		#region IConnectionManager Members

		public override IConnection Connection
		{
			get { return connection; }
			protected set { connection = value as SqlServerConnection; }
		}

		public override bool HasPermission(string permissionUUID)
		{
			if (connection != null)
			{
				if (CurrentUser is User && (CurrentUser as User).HasPermission(permissionUUID))
				{
					return true;
				}

				return base.HasPermission(permissionUUID);
			}
			return false;
		}

		public override bool HasPermissionIgnoreOverrides(string permissionUUID)
		{
			if (connection != null)
			{
				if ((isAdmin) || (CurrentUser is User && (CurrentUser as User).HasPermission(permissionUUID)))
				{
					return true;
				}
			}
			return false;
		}

		public override bool HasPermission(string permissionUUID, string login, SecureString password)
		{
			if (isAdmin)
			{
				return true;
			}
			string passwordHash = HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66");
			using (SqlCommand cmd = new SqlCommand("spSECURITY_DoesUserHavePermission_1_0"))
			{
				SqlServerParameters.MakeParam(cmd, "Login", login);
				SqlServerParameters.MakeParam(cmd, "PasswordHash", passwordHash);
				SqlServerParameters.MakeParam(cmd, "dataareaID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "permissionGUID", permissionUUID);
				SqlDataReader reader = (SqlDataReader) connection.ExecuteReader(cmd);
				try
				{
					return reader.HasRows;
				}
				finally
				{
					reader.Close();
					reader.Dispose();
				}
			}
		}

		public override bool HasPermission(string permissionUUID, string tokenHash, out bool tokenIsUser)
		{
	  
			SqlParameter tokenIsUserParam;
			SqlParameter validTokenParam;
			SqlParameter loginParam;
			SqlParameter passwordHashParam;

			tokenIsUser = false;
			if (isAdmin)
			{
				return true;
			}
			using(SqlCommand cmd = new SqlCommand("spSECURITY_LoginFromTokenLogin_1_0"))
			{

				SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "TokenLoginHash", tokenHash);

				tokenIsUserParam = SqlServerParameters.MakeParam(cmd, "tokenIsUser", false, SqlDbType.Bit, ParameterDirection.Output);
				validTokenParam = SqlServerParameters.MakeParam(cmd, "validToken", false, SqlDbType.Bit, ParameterDirection.Output);

				loginParam = SqlServerParameters.MakeParam(cmd, "login", false, SqlDbType.NVarChar, ParameterDirection.Output, 32);
				passwordHashParam = SqlServerParameters.MakeParam(cmd, "passwordHash", false, SqlDbType.NVarChar, ParameterDirection.Output, 40);

				connection.ExecuteNonQuery(cmd, true);

				if ((bool)tokenIsUserParam.Value)
				{
					tokenIsUser = true;
					return false;
				}

				if ((bool)validTokenParam.Value)
				{
					using (SqlCommand cmd2 = new SqlCommand("spSECURITY_DoesUserHavePermission_1_0"))
					{
						SqlServerParameters.MakeParam(cmd2, "Login", (string)loginParam.Value);
						SqlServerParameters.MakeParam(cmd2, "PasswordHash", (string)passwordHashParam.Value);
						SqlServerParameters.MakeParam(cmd2, "dataareaID", connection.DataAreaId);
						SqlServerParameters.MakeParam(cmd2, "permissionGUID", permissionUUID);
						SqlDataReader reader = (SqlDataReader)connection.ExecuteReader(cmd2);
						try
						{
							return reader.HasRows;
						}
						finally
						{
							reader.Close();
							reader.Dispose();
						}
					}
				}
			}

			return false;
		}

		public override bool VerifyCredentials(string login, string tokenHash, out bool isLockedOut, out bool tokenIsUser)
		{
			SqlParameter tokenIsUserParam;
			SqlParameter validTokenParam;
			SqlParameter loginParam;
			SqlParameter passwordHashParam;

			tokenIsUser = false;

			using (SqlCommand cmd = new SqlCommand("spSECURITY_LoginFromTokenLogin_1_0"))
			{

				SqlServerParameters.MakeParam(cmd, "DATAAREAID", connection.DataAreaId);
				SqlServerParameters.MakeParam(cmd, "TokenLoginHash", tokenHash);

				tokenIsUserParam = SqlServerParameters.MakeParam(cmd, "tokenIsUser", false, SqlDbType.Bit, ParameterDirection.Output);
				validTokenParam = SqlServerParameters.MakeParam(cmd, "validToken", false, SqlDbType.Bit, ParameterDirection.Output);

				loginParam = SqlServerParameters.MakeParam(cmd, "login", false, SqlDbType.NVarChar, ParameterDirection.Output, 32);
				passwordHashParam = SqlServerParameters.MakeParam(cmd, "passwordHash", false, SqlDbType.NVarChar, ParameterDirection.Output, 40);

				connection.ExecuteNonQuery(cmd, true);

				if (loginParam.Value is DBNull || (login != "" && login != (string)loginParam.Value))
				{
					isLockedOut = false;
					return false;
				}

				if ((bool)tokenIsUserParam.Value)
				{
					tokenIsUser = true;
					isLockedOut = false;
					return false;
				}

				if (!(bool)validTokenParam.Value)
				{
					isLockedOut = false;
					return false;
				}
			}

			return VerifyCredentials(login != "" ? login : (string)loginParam.Value, (string)passwordHashParam.Value, out isLockedOut);
		}

		public override bool VerifyCredentials(string login, SecureString password, out bool isLockedOut)
		{
			return VerifyCredentials(login, HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66"), out isLockedOut);
		}

		private bool VerifyCredentials(string login, string passwordHash, out bool isLockedOut)
		{
			SqlParameter isValidParam;
			SqlParameter isLockedOutParam;

			using (SqlCommand cmd = new SqlCommand("spSECURITY_VerifyCredentials_1_0"))
			{
				SqlServerParameters.MakeParam(cmd, "Login", login);
				SqlServerParameters.MakeParam(cmd, "PasswordHash", passwordHash);
				SqlServerParameters.MakeParam(cmd, "DataAreaID", connection.DataAreaId);

				isValidParam = SqlServerParameters.MakeParam(cmd, "IsValid", false, SqlDbType.Bit, ParameterDirection.Output);
				isLockedOutParam = SqlServerParameters.MakeParam(cmd, "IsLockedOut", false, SqlDbType.Bit, ParameterDirection.Output);

				connection.ExecuteNonQuery(cmd, true);

				isLockedOut = (bool)isLockedOutParam.Value;

				return (bool)isValidParam.Value;
			}
		}

		protected override Cache CreateCache()
		{
			var cache = new Cache {DataModel = this};
			cache.SetCacheProvider(new SqlServerCacheProvider());

			return cache;
		}
		

		public override IConnectionManagerTransaction CreateTransaction()
		{
			return new SqlServerTransaction(this);
		}

		public override IConnectionManagerTransaction CreateTransaction(IsolationLevel isolationLevel)
		{
			return new SqlServerTransaction(this, isolationLevel);
		}

		public override IConnectionManagerTemporary CreateTemporaryConnection()
		{
			return new ConnectionManagerTemporary(this, DisableReplicationActionCreation);
		}
	   
		public override string SiteServiceAddress
		{
			get
			{
				if (siteServiceAddress == null || siteServiceAddress == string.Empty)
				{
					try
					{
						SystemData.GetStoreServerHost(this, ref siteServiceAddress, ref siteServicePort);
					}
					catch (Exception)
					{
						return string.Empty;
					}
				}
				return siteServiceAddress;
			}
			set
			{
				siteServiceAddress = value;
			}
		}

		public override UInt16 SiteServicePortNumber
		{
			get
			{
				try
				{
					if (siteServiceAddress == null || siteServiceAddress == string.Empty)
					{
						SystemData.GetStoreServerHost(this, ref siteServiceAddress, ref siteServicePort);
					}
					if (siteServicePort == 0)
					{
						siteServicePort = 9101;
					}
				}
				catch (Exception)
				{
					return 9101;
				}

				return siteServicePort;
			}
			set
			{
				siteServicePort = value;
			}
		}
		
		#endregion

		public override bool OpenDatabaseConnection(string server, bool windowsAuthentication, string serverLogin, SecureString serverPassword, string databaseName, ConnectionType connectionType, string dataAreaID)
		{
			try
			{
				CreateSqlServerConnection(server, windowsAuthentication, serverLogin, serverPassword, databaseName, connectionType, dataAreaID);
				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}
	}
}
