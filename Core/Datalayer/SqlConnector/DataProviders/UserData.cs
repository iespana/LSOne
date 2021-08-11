using System;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector.DataProviders
{
	internal class UserData : SqlServerDataProviderBase
	{
		internal static void ClearUserSetting(IConnectionManager entry, RecordIdentifier userID, Guid settingID)
		{
			// The security in this method is that a user can only set or clear his own settings flags.

			ValidateSecurity(entry);

			if (userID == entry.CurrentUser.ID && UserSettingExists(entry, userID, settingID))
			{
				var statement = new SqlServerStatement("USERSETTINGS", StatementType.Delete);

				statement.AddCondition("SettingsGUID", settingID, SqlDbType.UniqueIdentifier);
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("UserGUID", (Guid)userID, SqlDbType.UniqueIdentifier);

				entry.Connection.ExecuteStatement(statement);

				InvalidateUserProfile(entry, (Guid)userID);
			}
		}

		internal static void SetUserSetting(IConnectionManager entry, RecordIdentifier userID, Guid settingID, SettingType settingType, string value, string longValue)
		{
			// The security in this method is that a user can only set or clear his own settings flags.

			ValidateSecurity(entry);

			if (userID == entry.CurrentUser.ID)
			{
				var statement = new SqlServerStatement("USERSETTINGS");

				if(UserSettingExists(entry, userID, settingID))
				{
					statement.StatementType = StatementType.Update;
					statement.AddField("Value", value);

					if(longValue == null)
					{
						statement.AddField("LONGTEXT", DBNull.Value, SqlDbType.NVarChar);
					}
					else
					{
						statement.AddField("LONGTEXT", longValue);
					}

					statement.AddCondition("SettingsGUID", settingID, SqlDbType.UniqueIdentifier);
					statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
					statement.AddCondition("UserGUID", (Guid)userID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					statement.StatementType = StatementType.Insert;
					statement.AddKey("GUID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
					statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
					statement.AddField("Value", value);

					if (longValue == null)
					{
						statement.AddField("LONGTEXT", DBNull.Value, SqlDbType.NVarChar);
					}
					else
					{
						statement.AddField("LONGTEXT", longValue);
					}

					statement.AddField("SettingsGUID", settingID, SqlDbType.UniqueIdentifier);
					statement.AddField("UserGUID", (Guid)userID, SqlDbType.UniqueIdentifier);
				}

				entry.Connection.ExecuteStatement(statement);

				if(settingType.Type == Guid.Parse("C79AE480-7EE1-11DB-9FE1-0800200C9A66"))
				{
					InvalidateUserProfile(entry, (Guid)userID);
				}
			}
		}

		internal static void SetUserSetting(IConnectionManager entry, Setting setting, RecordIdentifier settingID)
		{
			ValidateSecurity(entry);
			SetUserSetting(entry, (Guid)entry.CurrentUser.ID, (Guid)settingID, setting.SettingType, setting.UserSetting, setting.LongUserSetting);
		}

		internal static bool UserSettingExists(IConnectionManager entry, RecordIdentifier userID, Guid settingID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT 1 FROM USERSETTINGS WHERE SettingsGUID = @settingGuid AND UserGUID = @userGuid AND DATAAREAID = @dataareaID";

				MakeParam(cmd, "userGuid", (Guid)userID);
				MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "settingGuid", settingID);

				IDataReader dr = null;
				try
				{
					dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
					return dr.Read();
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
		}

		internal static bool UserPasswordExists(IConnectionManager entry, RecordIdentifier userID, SecureString password, bool checkPassword)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT 1 FROM vSECURITY_AllUsers_1_0 WHERE GUID = @UserGUID AND DATAAREAID = @dataareaID AND IsDomainUser = 0 " + (checkPassword ? "AND PasswordHash = @passHash" : "");

				MakeParam(cmd, "UserGUID", (Guid)userID);
				MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);

				if (checkPassword)
				{
					MakeParam(cmd, "passHash", HMAC_SHA1.GetValue(SecureStringHelper.ToString(password), "df5da100-a9ba-11de-8a39-0800200c9a66"));
				}

				IDataReader dr = null;
				try
				{
					dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
					return dr.Read();
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
		}

		internal static Setting GetSetting(IConnectionManager entry, RecordIdentifier userID, Guid settingID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand("spMAINT_GetSettingPair_1_0"))
			{
				MakeParam(cmd, "userGuid", (Guid) userID);
				MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "settingGuid", settingID);

				var userSettingExistsParam = MakeParam(cmd, "userSettingExists", false, SqlDbType.Bit, ParameterDirection.Output);
				var userValueParam = MakeParam(cmd, "userValue", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);
				var systemValueParam = MakeParam(cmd, "systemValue", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);
				var settingTypeParam = MakeParam(cmd, "typeGuid", "", SqlDbType.UniqueIdentifier, ParameterDirection.Output);
				var userValueLongParam = MakeParam(cmd, "userValueLong", "", SqlDbType.NVarChar, ParameterDirection.Output, -1);

				entry.Connection.ExecuteNonQuery(cmd, true);

				Setting setting = new Setting(
					IsNull(userSettingExistsParam) ? false : (bool) userSettingExistsParam.Value,
					IsNull(userValueParam) ? string.Empty : (string) userValueParam.Value,
					IsNull(systemValueParam) ? string.Empty : (string)systemValueParam.Value,
					IsNull(settingTypeParam) ? SettingType.UIFieldVisisbility : SettingType.Resolve((Guid)settingTypeParam.Value));
				if (!IsNull(userValueLongParam))
				{
					setting.LongUserSetting = (string) userValueLongParam.Value;
				}
				return setting;
			}
		}

		private static bool IsNull(IDataParameter value)
		{
			return value == null || value.Value == null || value.Value == DBNull.Value;
		}

		public static bool ChangePassword(IConnectionManager entry, SecureString oldPassword, SecureString newPassword)
		{
			if (entry.CurrentUser.ActiveDirectoryUser)
			{
				// We cannot change password for active directory user
				return false;
			}

			if(UserPasswordExists(entry, entry.CurrentUser.ID, oldPassword, true))
			{
				Setting setting = SystemData.GetSystemSetting(entry, Guid.Parse("7CB84D26-B28B-4086-8DCF-646F68CEF956"));

				var statement = new SqlServerStatement("USERS", StatementType.Update);

				statement.AddField("PasswordHash", HMAC_SHA1.GetValue(SecureStringHelper.ToString(newPassword), "df5da100-a9ba-11de-8a39-0800200c9a66"));
				statement.AddField("NeedPasswordChange", 0, SqlDbType.Bit);
				statement.AddField("ExpiresDate", DateTime.Now.AddDays(double.Parse(setting.Value)), SqlDbType.DateTime);
				statement.AddField("LastChangeTime", DateTime.Now, SqlDbType.DateTime);
				statement.AddCondition("GUID", (Guid)entry.CurrentUser.ID, SqlDbType.UniqueIdentifier);
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				entry.Connection.ExecuteStatement(statement);
				((User)entry.CurrentUser).ForcePasswordChange = false;
				return true;
			}

			return false;
		}

		public static bool ChangePasswordForOtherUser(IConnectionManager entry, RecordIdentifier userID, SecureString newPassword, bool needPasswordChange)
		{
			ValidateSecurity(entry, BusinessObjects.Permission.SecurityResetPassword);

			if (UserPasswordExists(entry, userID, new SecureString(), false))
			{
				Setting setting = SystemData.GetSystemSetting(entry, Guid.Parse("7CB84D26-B28B-4086-8DCF-646F68CEF956"));

				var statement = new SqlServerStatement("USERS", StatementType.Update);

				statement.AddField("PasswordHash", HMAC_SHA1.GetValue(SecureStringHelper.ToString(newPassword), "df5da100-a9ba-11de-8a39-0800200c9a66"));
				statement.AddField("NeedPasswordChange", needPasswordChange, SqlDbType.Bit);
				statement.AddField("ExpiresDate", DateTime.Now.AddDays(double.Parse(setting.Value)), SqlDbType.DateTime);
				statement.AddField("LastChangeTime", DateTime.Now, SqlDbType.DateTime);
				statement.AddCondition("GUID", (Guid)userID, SqlDbType.UniqueIdentifier);
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				entry.Connection.ExecuteStatement(statement);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Changes the password for the given user by directly inserting the password hash
		/// </summary>
		/// <remarks>This function does not check for user security since it is not intended to be run in a user security context. I.e this should be run byt the site service
		/// or by code that does not need user permission to function</remarks>
		/// <param name="entry">The entry into the database</param>
		/// <param name="userID">ID of the user</param>
		/// <param name="passwordHash">The hash of the new password</param>
		/// <param name="needPasswordChange">Set flag on the user that indecates if he needs to change his password</param>
		/// <param name="expiresDate">Sets the expire date</param>
		/// <param name="lastChangeTime">Sets the last change time</param>
		/// <param name="generateActions">Indicates wether replication actions are generated</param>
		/// <returns></returns>
		internal static bool ChangePasswordHashForOtherUser(IConnectionManager entry, RecordIdentifier userID, string passwordHash, bool needPasswordChange, DateTime expiresDate, DateTime lastChangeTime, bool generateActions)
		{
			if (UserPasswordExists(entry, userID, new SecureString(), false))
			{
				Setting setting = SystemData.GetSystemSetting(entry, Guid.Parse("7CB84D26-B28B-4086-8DCF-646F68CEF956"));

				var statement = new SqlServerStatement("USERS", StatementType.Update);

				statement.AddField("PasswordHash", passwordHash);
				statement.AddField("NeedPasswordChange", needPasswordChange, SqlDbType.Bit);
				statement.AddField("ExpiresDate", expiresDate, SqlDbType.DateTime);
				statement.AddField("LastChangeTime", lastChangeTime, SqlDbType.DateTime);
				statement.AddCondition("GUID", (Guid)userID, SqlDbType.UniqueIdentifier);
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				entry.Connection.ExecuteStatement(statement);
				return true;
			}

			return false;
		}

		internal static void AddToLoginLog(IConnectionManager entry, string login, RecordIdentifier userGuid, string function)
		{
			using (var cmd = entry.Connection.CreateCommand("spAuditing_AddLoginLog_1_0"))
			{
				MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "Login", login);
				MakeParam(cmd, "AuditUserGUID", (Guid) userGuid);
				MakeParam(cmd, "AuditFunction", function);

				entry.Connection.ExecuteNonQuery(cmd, true);
				// true here since this has no effect on replication
			}
		}

		internal static void GetUserPasswordInfo(IConnectionManager entry, RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT PasswordHash, ExpiresDate, LastChangeTime
									FROM USERS
									WHERE GUID = @userID";

				MakeParam(cmd, "userID", (Guid)userID, SqlDbType.UniqueIdentifier);

				IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

				dr.Read();

				passwordHash = (string)dr["PasswordHash"];
				expiresDate = AsDateTime(dr["ExpiresDate"]);
				lastChangeTime = AsDateTime(dr["LastChangeTime"]);

				dr.Close();
				dr.Dispose();
			}
		}

		/// <summary>
		/// Returns the login, hashed password and if the user is an Active Directory user for an active (non-deleted) user.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="userID"></param>
		/// <param name="login">The user login or an empty string if the user does not exist or was deleted</param>
		/// <param name="passwordHash">The hashed user password or an empty string if the user does not exist or was deleted</param>
		/// <param name="isDomainUser"></param>
		internal static void GetActiveUserInfo(IConnectionManager entry, RecordIdentifier userID, out string login, out string passwordHash, out bool isDomainUser)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT Login, PasswordHash, Deleted, isDomainUser
									FROM USERS
									WHERE GUID = @userID AND Deleted = 0";

				MakeParam(cmd, "userID", (Guid)userID, SqlDbType.UniqueIdentifier);

				IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
				if (dr.Read())
				{
					login = AsString(dr["Login"]);
					passwordHash = AsString(dr["PasswordHash"]);
					isDomainUser = AsBool(dr["isDomainUser"]);
				}
				else
				{
					login = string.Empty;
					passwordHash = string.Empty;
					isDomainUser = false;
				}

				dr.Close();
				dr.Dispose();
			}
		}

		internal static void InvalidateUserProfile(IConnectionManager entry, Guid userGuid)
		{
			var statement = new SqlServerStatement("USERS", StatementType.Update);

			statement.AddCondition("GUID", userGuid, SqlDbType.UniqueIdentifier);
			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddField("LocalProfileHash", "");
			statement.AddField("LastChangeTime", DateTime.Now, SqlDbType.DateTime);
			entry.Connection.ExecuteStatement(statement);
		}
	}
}
