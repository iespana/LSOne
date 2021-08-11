using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Exeptions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector;

namespace LSOne.DataLayer.SqlDataProviders.UserManagement
{
    public class UserData : SqlServerDataProviderBase, IUserData
    {
        public virtual User Get(IConnectionManager entry, Guid userID)
        {
            // No permission needed to read a user
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select u.GUID,
                                           u.Login, 
                                           u.IsDomainUser,
                                           u.IsServerUser, 
                                           u.FirstName, 
                                           u.MiddleName, 
                                           u.LastName, 
                                           u.NamePrefix,
                                           u.NameSuffix, 
                                           u.Disabled, 
                                           ISNULL(s.STAFFID,'') as STAFFID,
                                           ISNULL(u.EMAIL,'') AS EMAIL
                                   FROM vSECURITY_AllUsers_1_0 u
                                   LEFT OUTER JOIN RBOSTAFFTABLE s ON u.DATAAREAID = s.DATAAREAID AND u.STAFFID = s.STAFFID
                                   WHERE u.DATAAREAID = @dataAreaID AND  u.GUID = @userID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "userID", userID, SqlDbType.UniqueIdentifier);

                return Get<User>(entry, cmd, userID, PopulateUser, CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Checks if the user login is valid
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="userLogin">The user login to validate</param>
        /// <returns></returns>
        public virtual bool IsUserLoginValid(IConnectionManager entry, RecordIdentifier userLogin)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                cmd.CommandText = "SELECT LOGIN FROM USERS WHERE LOGIN = @id and DELETED = 0";
                MakeParam(cmd, "id", userLogin.DBValue, userLogin.DBType);

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

        public virtual RecordIdentifier CreateStaffMemberForPOS(IConnectionManager entry, RecordIdentifier staffID, Name name, RecordIdentifier userProfileID)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityCreateNewUsers);

            var statement = new SqlServerStatement("RBOSTAFFTABLE") {StatementType = StatementType.Insert};
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("STAFFID", (string)staffID);
            statement.AddField("NAME", entry.Settings.NameFormatter.Format(name));
            statement.AddField("PASSWORD", "");
            statement.AddField("CHANGEPASSWORD", 1, SqlDbType.TinyInt);
            statement.AddField("USERPROFILE", (string)userProfileID);

            entry.Connection.ExecuteStatement(statement);
         
            return staffID;
        }

        public virtual bool Exists(IConnectionManager entry, string login)
        {
            return RecordExists<User>(entry, "vSECURITY_AllUsers_1_0", "LOGIN", login);
        }

        public virtual bool GuidExists(IConnectionManager entry, Guid guid)
        {
            return RecordExists<User>(entry, "vSECURITY_AllUsers_1_0", "GUID", guid);
        }

        private static void PopulateUser(IDataReader dr, User user)
        {
            user.Guid = (Guid)dr["GUID"];
            user.Login = (string)dr["Login"];
            user.IsDomainUser = (bool)dr["IsDomainUser"];
            user.IsServerUser = (bool)dr["IsServerUser"];
            user.Disabled = ((int)dr["Disabled"] == 1);
            user.StaffID = (string)dr["STAFFID"];
            user.Email = (string)dr["EMAIL"];

            var name = user.Name;

            name.First = (string)dr["FirstName"];
            name.Middle = (string)dr["MiddleName"];
            name.Last = (string)dr["LastName"];
            name.Prefix = (string)dr["NamePrefix"];
            name.Suffix = (string)dr["NameSuffix"];
        }


        private static void PopulateDetailedUser(IDataReader dr, User user)
        {
            user.Guid = (Guid)dr["GUID"];
            user.Login = (string)dr["Login"];
            user.IsDomainUser = (bool)dr["IsDomainUser"];
            user.IsServerUser = (bool)dr["IsServerUser"];
            user.Disabled = ((int)dr["Disabled"] == 1);
            user.StaffID = (string)dr["STAFFID"];
            user.Email = (string)dr["EMAIL"];

            var name = user.Name;

            name.First = (string)dr["FirstName"];
            name.Middle = (string)dr["MiddleName"];
            name.Last = (string)dr["LastName"];
            name.Prefix = (string)dr["NamePrefix"];
            name.Suffix = (string)dr["NameSuffix"];

            user.NameOnReceipt = dr["NAMEONRECEIPT"] == DBNull.Value ? "" : (string)dr["NAMEONRECEIPT"];
            user.Culture = dr["OPERATORCULTURE"] == DBNull.Value ? "" : (string)dr["OPERATORCULTURE"];
            user.LayoutName = dr["LAYOUTNAME"] == DBNull.Value ? "" : (string)dr["LAYOUTNAME"];
            user.GroupName = dr["GROUPNAME"] == DBNull.Value ? "" : (string)dr["GROUPNAME"];
            user.UserProfileName = (string)dr["USERPROFILEDESCRIPTION"];
            user.UserProfileID = (string)dr["USERPROFILEID"];
        }
        public virtual List<User> AllUsers(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // No permission needed to search users
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT GUID, 
                                           Login, 
                                           IsDomainUser,
                                           IsServerUser, 
                                           FirstName, 
                                           MiddleName, 
                                           LastName,
                                           NamePrefix,
                                           NameSuffix,
                                           Disabled,
                                           ISNULL(STAFFID,'') AS STAFFID, 
                                           ISNULL(EMAIL,'') AS EMAIL 
                                    FROM vSECURITY_AllUsers_1_0 
                                    WHERE DATAAREAID = @dataAreaID";

                if(entry.Settings.NameFormat == NameFormat.FirstNameFirst)
                {
                    cmd.CommandText += " ORDER BY FirstName, MiddleName, LastName";
                }
                else
                {
                    cmd.CommandText += " ORDER BY LastName, FirstName, MiddleName";
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<User>(entry, cmd, CommandType.Text, PopulateUser);
            }
        }

        public virtual List<User> GetUsersInGroup(IConnectionManager entry, Guid groupGuid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // No permission needed to search users
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT GUID, 
                                           Login, 
                                           IsDomainUser,
                                           IsServerUser, 
                                           FirstName, 
                                           MiddleName, 
                                           LastName, 
                                           NamePrefix,
                                           NameSuffix,
                                           Disabled,
                                           ISNULL(STAFFID,'') AS STAFFID, 
                                           ISNULL(EMAIL,'') AS EMAIL 
                                    FROM vSECURITY_UsersInGroup_1_0 
                                    WHERE DATAAREAID = @dataAreaID AND UserGroupGuid = @userGroupGuid";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "userGroupGuid", groupGuid);

                return Execute<User>(entry, cmd, CommandType.Text, PopulateUser);
            }
        }

        public virtual int GetUserCount(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(GUID) FROM vSECURITY_AllUsers_1_0 WHERE DATAAREAID = @dataAreaID ";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual int GetLockedOutUserCount(IConnectionManager entry, int lockoutThreshold)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(GUID) FROM vSECURITY_AllUsers_1_0 WHERE LockOutCounter >= @threshold AND LockOutCounter <> 1000 AND  DATAAREAID = @dataAreaID ";

                MakeParam(cmd, "threshold", lockoutThreshold, SqlDbType.Int);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        private static void PopulateLoginUser(IConnectionManager entry, IDataReader dr, User user, ref NameFormat nameFormat)
        {
            user.Guid = (Guid)dr["GUID"];
            user.Login = (string)dr["Login"];
            user.IsServerUser = false;
            user.Disabled = false;
            user.StaffID = (string)dr["STAFFID"];

            var name = user.Name;

            name.First = (string)dr["FirstName"];
            name.Middle = (string)dr["MiddleName"];
            name.Last = (string)dr["LastName"];
            name.Prefix = (string)dr["NamePrefix"];
            name.Suffix = (string)dr["NameSuffix"];

            nameFormat = ((string)dr["NameConvention"] == "1" ? NameFormat.FirstNameFirst : NameFormat.LastNameFirst);
        }


        public bool SetSiteServiceConnectionInfoUnsecure(IConnectionManager entry, RecordIdentifier storeID, 
            string serverName, bool windowsAuthentication, string login, SecureString password, 
            string databaseName, ConnectionType connectionType, string dataAreaID, 
            out string siteServiceAddress,
            out ushort siteServicePortNumber)
        {
            /*************************************************
             * 
             * IF THIS STORED PROC IS UPDATED THEN A FALLBACK FOR A PREVIOUS VERSION NEEDS TO BE CREATED
             * THE POS NEEDS TO BE ABLE TO LOGON WITHOUT THE CHANGED CODE BEING PRESENT AS THE DATABASE UPDATE
             * HAPPENS AFTER THIS CODE IS RUN
             * 
             * ***********************************************/
            using (IDbCommand cmd = new SqlCommand("spPOSGetSiteServiceAddress_1_0"))
                //No connection exists on this point to create the command.
            {
                siteServiceAddress = "";
                siteServicePortNumber = 0;

                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter storeIDparam = SqlServerParameters.MakeParam(cmd, "storeID", storeID);
                SqlParameter serviceHostParam = SqlServerParameters.MakeParam(cmd, "serviceHost", "", SqlDbType.NVarChar,
                    ParameterDirection.Output, 50);
                SqlParameter servicePortParam = SqlServerParameters.MakeParam(cmd, "servicePort", "", SqlDbType.NVarChar,
                    ParameterDirection.Output, 10);

                try
                {
                    entry.UnsecureNonQuary(serverName,
                        windowsAuthentication,
                        login,
                        password,
                        databaseName,
                        connectionType,
                        dataAreaID,
                        cmd);
                }
                catch (DatabaseException)
                {
                    return false;
                }

                // If no site service profile is present on the store we will get a DBNull value from the stored procedure call
                if (serviceHostParam.Value == DBNull.Value || servicePortParam.Value == DBNull.Value)
                {
                    return false;
                }

                siteServiceAddress = (string) serviceHostParam.Value;
                siteServicePortNumber = Convert.ToUInt16((string) servicePortParam.Value);
                return true;
            }

        }

        public List<User> GetLoginUsersUnsecure(
            IConnectionManager entry,
            string dataSource,
            bool windowsAuthentication,
            string sqlServerLogin,
            SecureString sqlServerPassword,
            string databaseName,
            ConnectionType connectionType,
            string dataAreaID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            RecordIdentifier licenseCode,
            string version,
            out NameFormat nameFormat,
            out string storeLanguage,
            out string storeKeyboardCode,
            out string storeKeyboardLayoutName,
            out RecordIdentifier licensePassword,
            out DateTime licenseExpireDate)
        {
            nameFormat = NameFormat.FirstNameFirst;
            storeLanguage = "en-EN";

            bool storeExists;
            bool terminalExists;
            bool allowTrainingMode;
            var result = new List<User>();

            /*************************************************
             * 
             * IF THIS STORED PROC IS UPDATED THEN A FALLBACK FOR A PREVIOUS VERSION NEEDS TO BE CREATED
             * THE POS NEEDS TO BE ABLE TO LOGON WITHOUT THE CHANGED CODE BEING PRESENT AS THE DATABASE UPDATE
             * HAPPENS AFTER THIS CODE IS RUN
             * 
             * ***********************************************/

            using (IDbCommand cmd = new SqlCommand("spPOSGetLoginType_1_1")) //No connection exists on this point to create the command.
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd,"storeID",(string)storeID);
                MakeParam(cmd,"terminalID",(string)terminalID);
                MakeParam(cmd,"dataareaID",dataAreaID);
                MakeParam(cmd,"licenseCode", (string)licenseCode);
                MakeParam(cmd, "version", version);
                var cultureParam = MakeParam(cmd, "cultureName", "", SqlDbType.NVarChar, ParameterDirection.Output, 20);
                var keyboardCodeParam = MakeParam(cmd, "keyboardCode","", SqlDbType.NVarChar, ParameterDirection.Output, 20);
                var layoutNameParam = MakeParam(cmd,"layoutName","",SqlDbType.NVarChar, ParameterDirection.Output, 50);
                var allowTrainingModeParam = MakeParam(cmd, "allowTrainingMode", 0, SqlDbType.Bit, ParameterDirection.Output);
                var licensePasswordParam = MakeParam(cmd, "licensePassword", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);
                var licenseExpireDateParam = MakeParam(cmd, "licenseExpireDate", new DateTime(), SqlDbType.DateTime, ParameterDirection.Output);
                var storeExistsParam = MakeParam(cmd, "storeExists", 0, SqlDbType.Bit, ParameterDirection.Output);
                var terminalExistsParam = MakeParam(cmd, "terminalExists", 0, SqlDbType.Bit, ParameterDirection.Output);

                try
                {
                    result = entry.UnsecureExecuteReader<User, NameFormat>(
                                dataSource,
                                windowsAuthentication,
                                sqlServerLogin,
                                sqlServerPassword,
                                databaseName,
                                connectionType,
                                dataAreaID,
                                cmd,
                                ref nameFormat,
                                PopulateLoginUser);

                    storeExists = (bool)storeExistsParam.Value;
                    terminalExists = (bool)terminalExistsParam.Value;
                    allowTrainingMode = (bool)allowTrainingModeParam.Value;
                }
                catch (DatabaseException)
                {
                    GetLoginUsersUnsecureFallback(entry, 
                                                  dataSource, 
                                                  windowsAuthentication, 
                                                  sqlServerLogin, 
                                                  sqlServerPassword, 
                                                  databaseName, 
                                                  connectionType, 
                                                  dataAreaID, 
                                                  storeID, 
                                                  terminalID, 
                                                  licenseCode, 
                                                  version, 
                                                  out nameFormat, 
                                                  out storeLanguage, 
                                                  out storeKeyboardCode, 
                                                  out storeKeyboardLayoutName, 
                                                  out allowTrainingMode,
                                                  out licensePassword, 
                                                  out licenseExpireDate);

                    cultureParam.Value = storeLanguage;
                    keyboardCodeParam.Value = storeKeyboardCode;
                    layoutNameParam.Value = storeKeyboardLayoutName;
                    licensePasswordParam.Value = (string)licensePassword;
                    allowTrainingModeParam.Value = allowTrainingMode;
                    licenseExpireDateParam.Value = licenseExpireDate;

                    storeExists = true;
                    terminalExists = true;
                }

                if (!storeExists)
                {
                    throw new StoreMissingException();
                }

                if (!terminalExists)
                {
                    throw new TerminalMissingException();
                }

                if (cultureParam.Value is DBNull)
                {
                    throw new FunctionalityProfileMissingExeption();
                }


                storeLanguage = (string)cultureParam.Value;
                storeKeyboardCode = (string)keyboardCodeParam.Value;
                storeKeyboardLayoutName = (string)layoutNameParam.Value;
                licensePassword = (string)licensePasswordParam.Value == " " ? "" : (string)licensePasswordParam.Value;
                licenseExpireDate = (DateTime)licenseExpireDateParam.Value;
                allowTrainingMode = false;

                return result;
            }
        }

        private List<User> GetLoginUsersUnsecureFallback(
                    IConnectionManager entry,
                    string dataSource,
                    bool windowsAuthentication,
                    string sqlServerLogin,
                    SecureString sqlServerPassword,
                    string databaseName,
                    ConnectionType connectionType,
                    string dataAreaID,
                    RecordIdentifier storeID,
                    RecordIdentifier terminalID,
                    RecordIdentifier licenseCode,
                    string version,
                    out NameFormat nameFormat,
                    out string storeLanguage,
                    out string storeKeyboardCode,
                    out string storeKeyboardLayoutName,
                    out bool allowTrainingMode,
                    out RecordIdentifier licensePassword,
                    out DateTime licenseExpireDate)
        {
            nameFormat = NameFormat.FirstNameFirst;
            storeLanguage = "en-EN";

            using (IDbCommand cmd = new SqlCommand("spPOSGetLoginType_1_0")) //No connection exists on this point to create the command.
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "terminalID", (string)terminalID);
                MakeParam(cmd, "dataareaID", dataAreaID);
                MakeParam(cmd, "licenseCode", (string)licenseCode);
                MakeParam(cmd, "version", version);
                var cultureParam = MakeParam(cmd, "cultureName", "", SqlDbType.NVarChar, ParameterDirection.Output, 20);
                var keyboardCodeParam = MakeParam(cmd, "keyboardCode", "", SqlDbType.NVarChar, ParameterDirection.Output, 20);
                var layoutNameParam = MakeParam(cmd, "layoutName", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);
                var allowTrainingModeParam = MakeParam(cmd, "allowTrainingMode", 0, SqlDbType.Bit, ParameterDirection.Output);
                var licensePasswordParam = MakeParam(cmd, "licensePassword", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);
                var licenseExpireDateParam = MakeParam(cmd, "licenseExpireDate", new DateTime(), SqlDbType.DateTime, ParameterDirection.Output);
                
                var result = entry.UnsecureExecuteReader<User, NameFormat>(
                            dataSource,
                            windowsAuthentication,
                            sqlServerLogin,
                            sqlServerPassword,
                            databaseName,
                            connectionType,
                            dataAreaID,
                            cmd,
                            ref nameFormat,
                            PopulateLoginUser);

                if (cultureParam.Value is DBNull)
                {
                    throw new FunctionalityProfileMissingExeption();
                }

                storeLanguage = (string)cultureParam.Value;
                storeKeyboardCode = (string)keyboardCodeParam.Value;
                storeKeyboardLayoutName = (string)layoutNameParam.Value;
                licensePassword = (string)licensePasswordParam.Value == " " ? "" : (string)licensePasswordParam.Value;
                licenseExpireDate = (DateTime)licenseExpireDateParam.Value;
                allowTrainingMode = false;

                return result;
            }
        }

        public virtual List<User> Search(IConnectionManager entry, UserSearchFilter filter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.Add(new TableColumn
                {
                    ColumnName = "GUID",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "LOGIN",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISDOMAINUSER",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISSERVERUSER",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "FIRSTNAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "MIDDLENAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "LASTNAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMEPREFIX",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMESUFFIX",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "DISABLED",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(AU.STAFFID, '')",
                    ColumnAlias = "STAFFID"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMEONRECEIPT",
                    TableAlias = "RS"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "OPERATORCULTURE",
                    TableAlias = "RS"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "DESCRIPTION",
                    TableAlias = "UP",
                    ColumnAlias = "USERPROFILEDESCRIPTION",
                    IsNull = true,
                    NullValue = "''"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "PROFILEID",
                    TableAlias = "UP",
                    ColumnAlias = "USERPROFILEID",
                    IsNull = true,
                    NullValue = "''"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(PL.NAME,'')  ",
                    ColumnAlias = "LAYOUTNAME"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAME",
                    TableAlias = "UG",
                    ColumnAlias = "GROUPNAME"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(AU.EMAIL,'')",
                    ColumnAlias = "EMAIL"
                });

                List<Join> joins = new List<Join>();

                if (!string.IsNullOrEmpty(filter.Login))
                {
                    var loginSearchString = PreProcessSearchText(filter.Login, true, filter.LoginBeginsWith);
                    joins.Add(new Join
                    {
                        Condition = "  RS.STAFFID = AU.STAFFID  AND AU.LOGIN LIKE @loginSearchString",
                        JoinType = "INNER",
                        Table = "RBOSTAFFTABLE",
                        TableAlias = "RS"
                    });
                    MakeParam(cmd, "loginSearchString", loginSearchString);
                }
                else
                {
                    joins.Add(new Join
                    {
                        Condition = "  RS.STAFFID = AU.STAFFID",
                        JoinType = "INNER",
                        Table = "RBOSTAFFTABLE",
                        TableAlias = "RS"
                    });
                }
                joins.Add(new Join
                {
                    Condition = " AU.GUID = UIG.USERGUID",
                    JoinType = "LEFT",
                    Table = "USERSINGROUP",
                    TableAlias = "UIG"
                });
                joins.Add(new Join
                {
                    Condition = " UG.GUID = UIG.USERGROUPGUID",
                    JoinType = "LEFT",
                    Table = "USERGROUPS",
                    TableAlias = "UG"
                });

                if (!RecordIdentifier.IsEmptyOrNull(filter.UserGroup))
                {
                    joins.Add(new Join
                    {
                        Condition = " AU.GUID = UIG2.USERGUID AND UIG2.USERGROUPGUID = @userGroup",
                        JoinType = "INNER",
                        Table = "USERSINGROUP",
                        TableAlias = "UIG2"
                    });
                    MakeParam(cmd, "userGroup",(Guid)filter.UserGroup, SqlDbType.UniqueIdentifier);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.UserProfile))
                {
                    joins.Add(new Join
                    {
                        Condition = " UP.PROFILEID = RS.USERPROFILE AND RS.USERPROFILE = @userProfile",
                        JoinType = "INNER",
                        Table = "POSUSERPROFILE",
                        TableAlias = "UP"
                    });
                    MakeParam(cmd, "userProfile", (string)filter.UserProfile, SqlDbType.NVarChar);
                }
                else
                {
                    joins.Add(new Join
                    {
                        Condition = " UP.PROFILEID = RS.USERPROFILE",
                        JoinType = "LEFT",
                        Table = "POSUSERPROFILE",
                        TableAlias = "UP"
                    });
                }

                joins.Add(new Join
                {
                    Condition = " PL.LAYOUTID = UP.LAYOUTID",
                    JoinType = "LEFT",
                    Table = "POSISTILLLAYOUT",
                    TableAlias = "PL"
                });

                List<Condition> conditions = new List<Condition>();

                if (!string.IsNullOrEmpty(filter.Username))
                {
                    var userNameSearchString = PreProcessSearchText(filter.Username, true, filter.UsernameBeginsWith);
                    conditions.Add(new Condition
                    {
                        ConditionValue = "(au.FirstName like @userNameSearchString or au.MiddleName like @userNameSearchString or au.LastName like @userNameSearchString)",
                        Operator = "AND"
                    });
                    MakeParam(cmd, "userNameSearchString", userNameSearchString);
                }

                if (!string.IsNullOrEmpty(filter.NameOnReceipt))
                {
                    var nameOnReceiptSearchString = PreProcessSearchText(filter.NameOnReceipt, true, filter.NameOnReceiptBeginsWith);
                    conditions.Add(new Condition
                    {
                        ConditionValue = "rs.NAMEONRECEIPT like @nameOnReceiptSearchString",
                        Operator = "AND"
                    });
                    MakeParam(cmd, "nameOnReceiptSearchString", nameOnReceiptSearchString);
                }
                if (filter.EnabledSet)
                {
                    if (filter.Enabled)
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = "au.Disabled = 0 ",
                            Operator = "AND"
                        });
                    }
                    else
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = "au.Disabled = 1 ",
                            Operator = "AND"
                        });
                    }
                }

                string sortText;
                if (entry.Settings.NameFormat == NameFormat.FirstNameFirst)
                {
                    sortText = " order by FirstName, MiddleName, LastName";
                }
                else
                {
                    sortText = " order by LastName, FirstName, MiddleName";
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("vSECURITY_AllUsers_1_0", "AU", filter.MaxCount),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    sortText);

                return Execute<User>(entry, cmd, CommandType.Text, PopulateDetailedUser);
            }
        }

        public virtual List<User> GetUsersByUserProfile(IConnectionManager entry, RecordIdentifier userProfileID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.Add(new TableColumn
                {
                    ColumnName = "GUID",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "LOGIN",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISDOMAINUSER",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISSERVERUSER",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "FIRSTNAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "MIDDLENAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "LASTNAME",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMEPREFIX",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMESUFFIX",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "DISABLED",
                    TableAlias = "AU"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(AU.STAFFID, '')",
                    ColumnAlias = "STAFFID"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAMEONRECEIPT",
                    TableAlias = "RS"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "OPERATORCULTURE",
                    TableAlias = "RS"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "DESCRIPTION",
                    TableAlias = "UP",
                    ColumnAlias = "USERPROFILEDESCRIPTION",
                    IsNull = true,
                    NullValue = "''"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "PROFILEID",
                    TableAlias = "UP",
                    ColumnAlias = "USERPROFILEID",
                    IsNull = true,
                    NullValue = "''"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(PL.NAME,'')  ",
                    ColumnAlias = "LAYOUTNAME"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "NAME",
                    TableAlias = "UG",
                    ColumnAlias = "GROUPNAME"
                });
                columns.Add(new TableColumn
                {
                    ColumnName = "ISNULL(AU.EMAIL,'')",
                    ColumnAlias = "EMAIL"
                });

                List<Join> joins = new List<Join>();
                joins.Add(new Join
                {
                    Condition = "  RS.STAFFID = AU.STAFFID",
                    JoinType = "INNER",
                    Table = "RBOSTAFFTABLE",
                    TableAlias = "RS"
                });
                joins.Add(new Join
                {
                    Condition = " AU.GUID = UIG.USERGUID",
                    JoinType = "LEFT",
                    Table = "USERSINGROUP",
                    TableAlias = "UIG"
                });
                joins.Add(new Join
                {
                    Condition = " UG.GUID = UIG.USERGROUPGUID",
                    JoinType = "LEFT",
                    Table = "USERGROUPS",
                    TableAlias = "UG"
                });
                joins.Add(new Join
                {
                    Condition = " UP.PROFILEID = RS.USERPROFILE",
                    JoinType = "LEFT",
                    Table = "POSUSERPROFILE",
                    TableAlias = "UP"
                });
                joins.Add(new Join
                {
                    Condition = " PL.LAYOUTID = UP.LAYOUTID",
                    JoinType = "LEFT",
                    Table = "POSISTILLLAYOUT",
                    TableAlias = "PL"
                });

                Condition condition = new Condition { Operator = "AND", ConditionValue = "RS.USERPROFILE = @userProfile" };
                MakeParam(cmd, "userProfile", (string)userProfileID, SqlDbType.NVarChar);

                string sortText;
                if (entry.Settings.NameFormat == NameFormat.FirstNameFirst)
                {
                    sortText = " order by FirstName, MiddleName, LastName";
                }
                else
                {
                    sortText = " order by LastName, FirstName, MiddleName";
                }

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("vSECURITY_AllUsers_1_0", "AU"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(condition),
                    sortText);

                return Execute<User>(entry, cmd, CommandType.Text, PopulateDetailedUser);
            }
        }

        public virtual List<User> FindUsers(IConnectionManager entry, string firstName, string middleName, string lastName, string suffix, string loginName, int maxCount)
        {
            ValidateSecurity(entry);
            bool firstWhere = true;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "GUID, Login, IsDomainUser,IsServerUser,IsServerUser, FirstName, MiddleName, LastName, NamePrefix,NameSuffix,Disabled,ISNULL(STAFFID,'') as STAFFID, ISNULL(EMAIL,'') AS EMAIL " +
                                  "from vSECURITY_AllUsers_1_0 where DATAAREAID = @dataAreaID ";
                // No permission needed to search users

                if (firstName.Length > 0)
                {
                    cmd.CommandText += (firstWhere ? " and (" : "") +  " FirstName Like @firstName";
                    MakeParam(cmd, "firstName", PreProcessSearchText(firstName,false,false) + "%");
                    firstWhere = false;
                }

                if (middleName.Length > 0)
                {
                    cmd.CommandText += (firstWhere ? " and (" : " and") + " MiddleName Like @middleName";
                    MakeParam(cmd, "middleName", PreProcessSearchText(middleName, false, false) + "%");
                    firstWhere = false;
                }

                if (lastName.Length > 0)
                {
                    cmd.CommandText += (firstWhere ? " and (" : " and") + " LastName Like @lastName";
                    MakeParam(cmd, "lastName", PreProcessSearchText(lastName, false, false) + "%");
                    firstWhere = false;
                }

                if (suffix.Length > 0)
                {
                    cmd.CommandText += (firstWhere ? " and (" : " and") + " NameSuffix Like @nameSuffix";
                    MakeParam(cmd, "nameSuffix", PreProcessSearchText(suffix, false, false) + "%");
                    firstWhere = false;
                }

                if (loginName.Length > 0)
                {
                    cmd.CommandText += (firstWhere ? " and (" : " or") + " Login Like '" + PreProcessSearchText(loginName, false, false) + "%'";
                    firstWhere = false;
                }

                if (!firstWhere)
                {
                    cmd.CommandText += ")";
                }
 
                if (maxCount > 0)
                {
                    cmd.CommandText = "Select Distinct Top " + maxCount.ToString() + " " + cmd.CommandText;
                }
                else
                {
                    cmd.CommandText = "Select Distinct " + cmd.CommandText;
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<User>(entry, cmd, CommandType.Text, PopulateUser);
            }
        }

        public virtual List<User> FindUsersFromNameOrLogin(IConnectionManager entry, string text, int maxCount)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"GUID, 
                                    Login, 
                                    IsDomainUser,
                                    IsServerUser,
                                    IsServerUser, 
                                    FirstName, 
                                    MiddleName, 
                                    LastName, 
                                    NamePrefix,
                                    NameSuffix,
                                    Disabled,ISNULL(STAFFID,'') as STAFFID,
                                    ISNULL(EMAIL,'') AS EMAIL
                            FROM vSECURITY_AllUsers_1_0 WHERE DATAAREAID = @dataAreaID ";

                if (text.Length > 0)
                {
                    cmd.CommandText += " AND FirstName LIKE @text OR LastName LIKE @text OR Login LIKE @text";
                    MakeParam(cmd, "text", PreProcessSearchText(text, false, false) + "%");
                }

                if (maxCount > 0)
                {
                    cmd.CommandText = "SELECT DISTINCT TOP " + maxCount.ToString() + " " + cmd.CommandText;
                }
                else
                {
                    cmd.CommandText = "SELECT DISTINCT " + cmd.CommandText;
                }

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<User>(entry, cmd, CommandType.Text, PopulateUser);
            }
        }

        public virtual User FindUserFromStaffID(IConnectionManager entry, string staffID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                // No permission needed to search users
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT GUID, 
                                           Login, 
                                           IsDomainUser,
                                           IsServerUser,
                                           IsServerUser, 
                                           FirstName, 
                                           MiddleName, 
                                           LastName, 
                                           NamePrefix,
                                           NameSuffix,
                                           Disabled,
                                           ISNULL(STAFFID,'') AS STAFFID, 
                                           ISNULL(EMAIL,'') AS EMAIL
                                    FROM vSECURITY_AllUsers_1_0 WHERE STAFFID = @staffID";

                MakeParam(cmd, "staffID", staffID);

                var users = Execute<User>(entry, cmd, CommandType.Text, PopulateUser);
                return users.Count > 0 ? users[0] : null;
            }
        }

        public virtual void SetPermission(IConnectionManager entry, Guid userGuid, Guid permissionGuid, UserGrantMode mode)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityGrantPermissions);

            try
            {
                if (!entry.HasPermission(BusinessObjects.Permission.SecurityGrantHigherPermissions))
                {
                    // We need to check if the permission that we are setting is higher than our own
                    if (!entry.HasPermission(GetPermissionCode(entry, permissionGuid)))
                    {
                        // This permission is above our rights
                        throw new PermissionException(BusinessObjects.Permission.SecurityGrantHigherPermissions);
                    }
                }

                DeleteUserPermission(entry, userGuid, permissionGuid);

                if(mode != UserGrantMode.Inherit)
                {
                    var statement = new SqlServerStatement("USERPERMISSION", StatementType.Insert);
                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    statement.AddKey("GUID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
                    statement.AddField("UserGUID", userGuid, SqlDbType.UniqueIdentifier);
                    statement.AddField("PermissionGUID", permissionGuid, SqlDbType.UniqueIdentifier);
                    statement.AddField("Grant", mode == UserGrantMode.Grant ? 1 : 0, SqlDbType.Bit);
                    entry.Connection.ExecuteStatement(statement);
                }

                SqlConnector.DataProviders.UserData.InvalidateUserProfile(entry, userGuid);
            }
            catch (Exception){ }
        }

        public virtual void DeleteUserPermission(IConnectionManager entry, Guid userGuid, Guid permissionGuid)
        {
            var statement = new SqlServerStatement("USERPERMISSION", StatementType.Delete);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("UserGUID", userGuid, SqlDbType.UniqueIdentifier);
            statement.AddCondition("PermissionGUID", permissionGuid, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual string GetPermissionCode(IConnectionManager entry, Guid permissionGuid)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT PermissionCode, CodeIsEncrypted FROM PERMISSIONS
                                    WHERE GUID = @PermissionGUID AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PermissionGUID", permissionGuid);

                IDataReader dr = null;
                string permissionCode = string.Empty;
                bool isEncrypted = false;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        permissionCode = (string)dr["PermissionCode"];
                        isEncrypted = (bool)dr["CodeIsEncrypted"];
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }

                if (isEncrypted)
                {
                    //TODO: Decrypt the PermissionCode before we return the value
                    return permissionCode;
                }

                return permissionCode;
            }
        }

        public virtual void SetEnabled(IConnectionManager entry, Guid userGuid, bool value)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityEnableDisableUser);

            var statement = new SqlServerStatement("USERS", StatementType.Update);

            statement.AddCondition("GUID", userGuid, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("LockOutCounter", value ? 0 : 1000, SqlDbType.Int);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Delete(IConnectionManager entry, Guid userGuid)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityDeleteUser);

            var statement = new SqlServerStatement("USERS", StatementType.Update);

            statement.AddCondition("GUID", userGuid, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("Deleted", true, SqlDbType.Bit);
            entry.Connection.ExecuteStatement(statement);
        }

        public (Guid UserID, RecordIdentifier StaffID) New(
            IConnectionManager entry,
            string login,
            string password,
            Name name,
            List<Guid> securityGroups,
            bool isActiveDirectoryUser,
            bool isServerUser,
            string email)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityCreateNewUsers);

            Guid userID = Guid.NewGuid();
            RecordIdentifier staffID = DataProviderFactory.Instance.GenerateNumber<IUserData, User>(entry);

            if (!GuidExists(entry, userID))
            {
                Setting setting = SystemData.GetSystemSetting(entry, Guid.Parse("7CB84D26-B28B-4086-8DCF-646F68CEF956"));

                var statement = new SqlServerStatement("USERS", StatementType.Insert);

                statement.AddKey("GUID", userID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                statement.AddField("Login", login);
                statement.AddField("PasswordHash", HMAC_SHA1.GetValue(password, "df5da100-a9ba-11de-8a39-0800200c9a66"));
                statement.AddField("NeedPasswordChange", !isActiveDirectoryUser, SqlDbType.Bit);
                statement.AddField("ExpiresDate", DateTime.Now.AddDays(double.Parse(setting.Value)), SqlDbType.DateTime);
                statement.AddField("IsDomainUser", isActiveDirectoryUser, SqlDbType.Bit);
                statement.AddField("IsServerUser", isServerUser, SqlDbType.Bit);
                statement.AddField("LastChangeTime", DateTime.Now, SqlDbType.DateTime);
                statement.AddField("LocalProfileHash", "");
                statement.AddField("FirstName", name.First);
                statement.AddField("MiddleName", name.Middle);
                statement.AddField("LastName", name.Last);
                statement.AddField("NamePrefix", name.Prefix);
                statement.AddField("NameSuffix", name.Suffix);
                statement.AddField("STAFFID", (string)staffID);
                statement.AddField("EMAIL", email);
                entry.Connection.ExecuteStatement(statement);

                //Add user to permission groups
                foreach(var groupGuid in securityGroups)
                {
                    var pStatement = new SqlServerStatement("USERSINGROUP", StatementType.Insert);

                    pStatement.AddKey("GUID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
                    pStatement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                    pStatement.AddField("UserGUID", userID, SqlDbType.UniqueIdentifier);
                    pStatement.AddField("UserGroupGUID", groupGuid, SqlDbType.UniqueIdentifier);
                    entry.Connection.ExecuteStatement(pStatement);
                    SqlConnector.DataProviders.UserData.InvalidateUserProfile(entry, userID);
                }
            }

            return (UserID: userID, StaffID: staffID);
        }

        public virtual void Save(IConnectionManager entry, User user)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SecurityEditUser);

            var statement = new SqlServerStatement("USERS", StatementType.Update);

            statement.AddCondition("GUID", user.Guid, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddField("FirstName", user.Name.First);
            statement.AddField("MiddleName", user.Name.Middle);
            statement.AddField("LastName", user.Name.Last);
            statement.AddField("NamePrefix", user.Name.Prefix);
            statement.AddField("NameSuffix", user.Name.Suffix);
            statement.AddField("STAFFID", (string)user.StaffID);
            statement.AddField("EMAIL", user.Email);
            Save(entry, user, statement);
        }

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier staffID)
        {
            return RecordExists<User>(entry, "RBOSTAFFTABLE", "STAFFID", staffID);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOSTAFFTABLE", "STAFFID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public RecordIdentifier SequenceID => "STAFFID";
    }
}