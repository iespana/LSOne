using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using LSOne.DataLayer.DatabaseUtil.Enums;
using LSOne.DataLayer.DatabaseUtil.Exceptions;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DatabaseUtil.EmbeddedInstall;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using System.Xml;

namespace LSOne.DataLayer.DatabaseUtil
{

    /// <summary>
    /// This class is the main entry point into the DatabaseUtility. All functionality can be called and used through this class.
    /// </summary>
    public class DatabaseUtility
    {
        private bool cancelRun = false;
        /// <summary>
        /// Indicates if creation of database should be from Cloud context
        /// </summary>
        public bool IsCloud { get; set; }
        /// <summary>
        /// When using the Database Utility the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the DB Utility. This is done instead of having a user interface with progress information.
        /// </summary>
        /// 
        public event MessageCallback MessageCallbackHandler;

        /// <summary>
        /// Provides information on where in the database create/update process we currently are. Occurs each time a script is being executed.
        /// </summary>
        public event UpdateDatabaseProgressCallback UpdateDatabaseProgressHandler;

        /// <summary>
        /// Event to trigger on counter
        /// </summary>
        public event PerformTrigger updateTrigger;
        private Dictionary<int, bool> triggerTargets = new Dictionary<int, bool>();
        private const string Database = "DATABASE";
        private const string Demodata = "DEMODATA";
        private const string DefaultDataPath = "\\LS Retail\\Default Data";
        private int databaseMinVer = 0;
        private int auditDatabaseMinVer = 0;

        #region Properties

        /// <summary>
        /// The SqlConnection that will be used to create and/or update the database
        /// </summary>
        public SqlConnection Connection { get; private set; }

        /// <summary>
        /// If true then a SQL Server will be installed. If the SQLConnection is null or the SQLServer in the connection string
        /// cannot be found then the constructor will set the variable to true. Otherwise the default value is false
        /// </summary>
        public bool EmbeddedSQLServerInstall { get; set; }

        /// <summary>
        /// The result of setting the connection through either constructors or SetDatabaseConnection functions
        /// </summary>
        public UtilityResult ConnectionResult { get; private set; }

        /// <summary>
        /// The version to check for when looking for the "Current version"
        /// </summary>
        public string CurrentVersionType { get; set; }

        /// <summary>
        /// The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown
        /// </summary>
        private string DataAreaId { get; set; }

        private Assembly ScriptAssembly { get; set; }

        #endregion

        #region Private variables

        private string masterConnectionStr = "";
        private string orgDatabase = "";
        private string auditDatabase = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for DatabaseUtility class
        /// </summary>
        /// <param name="dataAreaId">The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown</param>
        public DatabaseUtility(string dataAreaId)
        {
            ConnectionResult = UtilityResult.NotValidated;
            EmbeddedSQLServerInstall = false;
            this.DataAreaId = dataAreaId;
            this.ScriptAssembly = null;
            CurrentVersionType = "";

            if (dataAreaId == "" || dataAreaId.Length > 4)
            {
                throw new DataAreaIdNotValidException(Properties.Resources.DataAreaIdIsNotValid);
            }
        }

        /// <summary>
        /// Constructor that sets the database connection. The state of the connection after testing can be viewed in property ConnectionResult
        /// </summary>
        /// <param name="dataAreaId">The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown</param>
        /// <param name="connection">The SQL connection to be used to update and/or create a database</param>
        public DatabaseUtility(string dataAreaId, SqlConnection connection) : this(dataAreaId)
        {
            SetDatabaseConnection(connection);
        }

        /// <summary>
        /// Constructor that sets the database connection from a connection string. The state of the connection after testing can be viewed in property ConnectionResult
        /// </summary>
        /// <param name="dataAreaId">The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown</param>
        /// <param name="connectionStr"></param>
        public DatabaseUtility(string dataAreaId, string connectionStr) : this(dataAreaId)
        {
            SetDatabaseConnection(connectionStr);
        }

        /// <summary>
        /// Constructor that sets the database connection from SQL server and database name. Windows Authentication is by default true. The state of the connection after testing can be viewed in property ConnectionResult
        /// </summary>
        /// <param name="dataAreaId">The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown</param>
        /// <param name="sqlServer">The SQL server</param>
        /// <param name="databaseName">The database to connect to</param>
        /// <param name="connectionType">The type of connection to use</param>
        public DatabaseUtility(string dataAreaId, string sqlServer, string databaseName, ConnectionType connectionType)
            : this(dataAreaId)
        {
            SetDatabaseConnection(sqlServer, databaseName, true, "", "", connectionType);
        }

        /// <summary>
        /// Constructor that sets the database connection from SQL server, database name and windows Authentication or username or password. The state of the connection can be viewed in property ConnectionResult
        /// </summary>
        /// <param name="dataAreaId">The default Data Area Id (company id) on all tables. If DataAreaId is not valid then a DataAreaIdNotValidException is thrown</param>
        /// <param name="sqlServer">The SQL server</param>
        /// <param name="databaseName">The database to connect to</param>
        /// <param name="windowsAuthentication">If true Windows Authentication is used to connect to the SQL server</param>
        /// <param name="userName">If using SQL Server authentication then this user name is used to connect to the SQL server</param>
        /// <param name="password">If using SQL Server authentication then this password is used to connect to the SQL server</param>
        /// <param name="connectionType">The type of connection to use</param>
        public DatabaseUtility(string dataAreaId, string sqlServer, string databaseName, bool windowsAuthentication, string userName, string password, ConnectionType connectionType)
            : this(dataAreaId)
        {
            SetDatabaseConnection(sqlServer, databaseName, windowsAuthentication, userName, password, connectionType);
        }
        #endregion        

        #region SetDatabaseConnection
        /// <summary>
        /// Sets the database connection for the Database Utility class using a SqlConnection. The state of the connection can be viewed in property ConnectionResult        
        /// </summary>
        /// <param name="connection">The SQL connection</param>
        /// <param name="connectionIsValid">Pass true here to guaranty that the connection is valid</param>
        public void SetDatabaseConnection(SqlConnection connection, bool connectionIsValid = false)
        {
            try
            {
                this.Connection = connection;

                if (!connectionIsValid)
                {
                    SQLConnectionIsValid(connection);
                }
                else
                {
                    ConnectionResult = UtilityResult.Validated;
                    orgDatabase = this.Connection.Database;
                    masterConnectionStr = CreateConnectionString(
                        RetrieveServerName(connection.ConnectionString), "master",
                        RetrieveWindowsAuthentication(connection.ConnectionString),
                        RetrieveUserName(connection.ConnectionString),
                        RetrievePassword(connection.ConnectionString),
                        RetrieveConnectionType(connection.ConnectionString));
                }

                SendMessage(Properties.Resources.DatabaseConnectionSet);
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.FailedToSetTheDatabaseConnection, ex);
            }
        }

        /// <summary>
        /// Sets the database connection for the Database Utility class using a connection string. The state of the connection can be viewed in property ConnectionResult        
        /// </summary>
        /// <param name="connectionStr">Connection string to use to create the SQL connection</param>
        public void SetDatabaseConnection(string connectionStr)
        {
            try
            {
                SetSQLConnection(connectionStr);
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.FailedToSetTheDatabaseConnection, ex);
            }
        }

        /// <summary>
        /// Creates a connection string from the parameters and then sets the database connection for the Database Utility class using the connection string.
        /// The state of the connection can be viewed in property ConnectionResult        
        /// </summary>
        /// <param name="sqlServer">The SQL server</param>
        /// <param name="databaseName">The database to connect to</param>
        /// <param name="windowsAuthentication">If true Windows Authentication is used to connect to the SQL server</param>
        /// <param name="userName">If using SQL Server authentication then this user name is used to connect to the SQL server</param>
        /// <param name="password">If using SQL Server authentication then this password is used to connect to the SQL server</param>
        /// <param name="connectionType">The type of connection to use</param>
        public void SetDatabaseConnection(string sqlServer, string databaseName, bool windowsAuthentication, string userName, string password, ConnectionType connectionType)
        {
            try
            {
                masterConnectionStr = CreateConnectionString(sqlServer, "master", windowsAuthentication, userName, password, connectionType);
                SetSQLConnection(CreateConnectionString(sqlServer, databaseName, windowsAuthentication, userName, password, connectionType));
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.FailedToSetTheDatabaseConnection, ex);
            }
        }

        private void ValidateConnection()
        {
            //If the connection is null - throw the exception right away
            if (Connection == null)
            {
                throw new ConnectionException(Properties.Resources.ConnectionNotValid);
            }

            //If the connection has not been validated 
            if (ConnectionResult == UtilityResult.NotValidated)
            {
                SQLConnectionIsValid(Connection);
            }

            //SQLConnectionIsValid updates ConnectionResult - if the result is anything other than Validated throw an exception
            if (ConnectionResult != UtilityResult.Validated)
            {
                throw new ConnectionException(Properties.Resources.ConnectionNotValid);
            }
        }

        #endregion

        #region Check Connection string

        private void SetSQLConnection(string connectionString)
        {
            try
            {
                this.Connection = new SqlConnection(connectionString);
                SQLConnectionIsValid(this.Connection);

                SendMessage(Properties.Resources.DatabaseConnectionSet);
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.FailedToSetTheDatabaseConnection, ex);
            }
        }

        #endregion        

        #region Message Callback Handler

        /// <summary>
        /// Uses the MessageCallbackHandler delegate to send messages to whoever is subscribing
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        private void SendMessage(string msg)
        {
            SendMessage(msg, MessageCallbackHandler);
        }

        private static void SendMessage(string msg, MessageCallback callBack)
        {
            if (callBack != null)
            {
                callBack("Migration Service", msg);
            }
        }

        void sql2008Inst_MessageCallbackHandler(string sender, string msg)
        {
            SendMessage(msg);
        }

        #endregion

        #region triggerTargets
        /// <summary>
        /// Target counter to invoke migration
        /// </summary>
        /// <param name="target"></param>
        public void RegisterTriggerTarget(int target)
        {
            triggerTargets[target] = true;
        }

        /// <summary>
        /// Removes a registered trigger target
        /// </summary>
        /// <param name="target"></param>
        public void RemoveTriggerTarget(int target)
        {
            triggerTargets[target] = false;
        }
        #endregion

        #region Create and update database

        /// <summary>
        /// Returns true if the database needs an upgrade.
        /// </summary>
        /// <param name="databasesToCheck">Should the POS database (normal) be checked and/or the Audit database?</param>
        /// <param name="dataBaseCounter">Counter for comparisoni purposes</param>
        /// <param name="maxDatabaseCounter">Maximum version in utility</param>
        /// <param name="skipConnectionValidation">Set to true if you know you have a database connection that is working and good, this avoids database close</param>
        /// <returns></returns>
        public bool DatabaseNeedsUpgrade(DatabaseType databasesToCheck, out string dataBaseCounter, out string maxDatabaseCounter, bool skipConnectionValidation = false)
        {
            bool needsUpgrade = false;
            GetMinVersion(skipConnectionValidation);
            string currentDatabase = Connection.Database;
            if ((databasesToCheck & DatabaseType.Normal) == DatabaseType.Normal)
            {
                dataBaseCounter = GetCurrentVersion(VersionType.Database, DatabaseType.Normal, skipConnectionValidation);
                needsUpgrade = DatabaseNeedsUpgrade(dataBaseCounter, ScriptSubType.Normal, out maxDatabaseCounter);
            }
            else
            {
                dataBaseCounter = string.Empty;
                maxDatabaseCounter = string.Empty;
            }

            if (!needsUpgrade && (databasesToCheck & DatabaseType.Audit) == DatabaseType.Audit)
            {
                var dbVersion = GetCurrentVersion(VersionType.Database, DatabaseType.Audit, skipConnectionValidation);
                needsUpgrade = DatabaseNeedsUpgrade(dbVersion, ScriptSubType.Audit);
            }

            if (currentDatabase != Connection.Database)
            {
                ExecuteCommand("USE [" + currentDatabase + "]");
            }

            return needsUpgrade;
        }
        /// <summary>
        /// Returns true if the database needs an upgrade.
        /// </summary>
        /// <param name="databasesToCheck">Should the POS database (normal) be checked and/or the Audit database?</param>
        /// <param name="skipConnectionValidation">Set to true if you know you have a database connection that is working and good, this avoids database close</param>
        public bool DatabaseNeedsUpgrade(DatabaseType databasesToCheck, bool skipConnectionValidation = false)
        {
            string counter;
            string maxCounter;
            return DatabaseNeedsUpgrade(databasesToCheck, out counter, out maxCounter, skipConnectionValidation);
        }

        private bool DatabaseNeedsUpgrade(string dbVersion, ScriptSubType scriptSubType)
        {
            string maxVersion;
            return DatabaseNeedsUpgrade(dbVersion, scriptSubType, out maxVersion);
        }

        private bool DatabaseNeedsUpgrade(string dbVersion, ScriptSubType scriptSubType, out string maxVersion)
        {
            var currVersion = new DatabaseVersion();
            currVersion.ParseVersion(dbVersion);
            var scriptInfo = GetSQLScriptInfo();
            var scriptsToRun = scriptInfo.Where
                (info =>
                    info.ScriptType == RunScripts.UpdateDatabase //Only scripts that are of type UpdateDatabase
                    && info.Version.DbVersion >= currVersion.DbVersion
                    //Only scripts that have the same or higher DbVersion than the current version
                    && info.ScriptSubType == scriptSubType)
                .OrderByDescending(info => info.Version.DbVersion) //Order by DbVersion, Partner version
                .ThenBy(info => info.Version.PartnerVersion);

            foreach (var info in scriptsToRun)
            {
                if (ScriptCanBeRun(currVersion, info.Version, scriptSubType))
                {
                    maxVersion = info.Version.ToString();
                    return true;
                }
            }
            maxVersion = string.Empty;
            return false;
        }

        /// <summary>
        /// Installs the SQL server if needed, creates a new Database if needed,
        /// updates an existing database if needed, updates Stored procedures if needed.
        /// If any of the components failed a DatabaseUtilityException or RunScriptExceptions are thrown 
        /// </summary>
        /// <param name="sqlServerType">Is the database being updated on SQL CE or SQL Server</param>
        /// <param name="scriptsToRun">Tells the functions if all or specific scripts should be run</param>
        /// <param name="dbTypeToUpdate">Should the function update only "normal" database, only audit database or both</param>        
        /// <param name="userGuid">The user GUID for the cloud admin user</param>
        /// <param name="cloudPassword">The password for the cloud user</param>
        /// <param name="sqlPassword"></param>
        /// <param name="sqlUserName"></param>
        /// <param name="cancellationToken">Used to notify this operation that it should be cancelled. Will throw <see cref="OperationCanceledException"/> if the the operation is canceled.</param>
        /// //TODO DO NOT USE admin and 12345
        public void UpdateDatabase(
            SQLServerType sqlServerType,
            RunScripts scriptsToRun,
            DatabaseType dbTypeToUpdate,
            Guid userGuid = new Guid(),
            string cloudPassword = "abcdefg__123456789",
            string sqlPassword = "abcdefg__123456789",
            string sqlUserName = "",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (ConnectionResult == UtilityResult.NotValidated)
            {
                throw new ConnectionException(Properties.Resources.ConnectionNotValid);
            }
            UpdateDatabase(SQLServerType.SQLServer, scriptsToRun, SQLInstall.SQL2008Expr, null, dbTypeToUpdate, null,
                cloudPassword, userGuid, sqlPassword, sqlUserName, cancellationToken);
        }
        /// <summary>
        /// Lowast updatescript counter available
        /// </summary>
        /// <param name="skipConnectionValidation"></param>
        /// <returns></returns>
        public int GetMinVersion(bool skipConnectionValidation = false)
        {
            if (!skipConnectionValidation)
            {
                ValidateConnection();
            }

            DatabaseType databaseType = DatabaseType.Normal;
            //Set the database connection to use the Audit database
            if ((databaseType & DatabaseType.Audit) == DatabaseType.Audit && !Connection.Database.ToUpperInvariant().Contains("_AUDIT"))
            {
                try
                {
                    if (auditDatabase == "" && Connection.Database.ToUpperInvariant() != "MASTER")
                    {
                        ExecuteCommand("USE [" + orgDatabase + "_Audit]");
                    }
                    else if (auditDatabase != "")
                    {
                        ExecuteCommand("USE [" + auditDatabase + "]");
                    }
                }
                catch (RunScriptException)
                {
                    return 0;
                }
            }
            else if ((databaseType & DatabaseType.Normal) == DatabaseType.Normal && Connection.Database.ToUpperInvariant() != orgDatabase)
            {
                try
                {
                    ExecuteCommand("USE [" + orgDatabase + "]");
                }
                catch (RunScriptException)
                {
                    return 0;
                }
            }
            int version;
            string qry = "SELECT COALESCE(TEXT, '0') DB_MIN_VER FROM POSISINFO WHERE ID = 'DB_MIN_VER'";
            var db = GetTable(qry);

            if (db.Rows.Count > 0)
            {
                version = int.Parse(db.Rows[0]["DB_MIN_VER"].ToString());
            }
            else
            {


                //No DBVERSION in the database
                var ins = "INSERT INTO POSISINFO (ID, TEXT) VALUES ('DB_MIN_VER', '0')";
                ExecuteCommand(ins);

                //Set the connection back to use the original database
                if ((databaseType & DatabaseType.Audit) == DatabaseType.Audit &&
                    Connection.Database.ToUpperInvariant().Contains("_AUDIT"))
                {
                    ExecuteCommand("USE [" + Connection.Database + "]");
                }
                return 0;
            }
            if ((databaseType & DatabaseType.Audit) == DatabaseType.Audit && !Connection.Database.ToUpperInvariant().Contains("_AUDIT"))
            {
                auditDatabaseMinVer = version;

            }
            else if ((databaseType & DatabaseType.Normal) == DatabaseType.Normal && Connection.Database.ToUpperInvariant() == orgDatabase.ToUpperInvariant())
            {
                databaseMinVer = version;
            }
            return version;
        }

        /// <summary>
        /// Installs the SQL server if needed, creates a new Database if needed,
        /// updates an existing database if needed, updates Stored procedures if needed.
        /// If any of the components failed a DatabaseUtilityException or RunScriptExceptions are thrown 
        /// </summary>
        /// <param name="sqlServerType">Is the database being updated on SQL CE or SQL Server</param>
        /// <param name="scriptsToRun">Tells the functions if all or specific scripts should be run</param>        
        /// <param name="sqlnstallType">Which SQL server to install if none exists; 2005 or 2008</param>
        /// <param name="sqlParameters">What command prompt parameters should be used if SQL Server is to be installed </param>
        /// <param name="dbTypeToUpdate">Should the function update only "normal" database, only audit database or both</param>
        /// <param name="scriptInfo"></param>
        /// <param name="cloudPassword">The password for the cloud user</param>
        /// <param name="userGuid"></param>
        /// <param name="sqlPassword"></param>
        /// <param name="sqlUserName"></param>
        /// <param name="cancellationToken">Used to notify this operation that it should be cancelled. Will throw <see cref="OperationCanceledException"/> if the the operation is canceled.</param>
        public void UpdateDatabase(
            SQLServerType sqlServerType,
            RunScripts scriptsToRun,
            SQLInstall sqlnstallType,
            SQLParams sqlParameters,
            DatabaseType dbTypeToUpdate,
            LinkedList<ScriptInfo> scriptInfo,
            string cloudPassword,
            Guid userGuid,
            string sqlPassword = "",
            string sqlUserName = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ConnectionResult == UtilityResult.NotValidated)
            {
                throw new ConnectionException(Properties.Resources.ConnectionNotValid);
            }
            cancelRun = false;
            //SqlServerType - will not be used untill Mobile functionality is added into the Database Utility
            auditDatabase = "";
            const string adtSuffix = "_Audit";

            UpdateDatabaseProgressInfo updateDatabaseProgressInfo = new UpdateDatabaseProgressInfo();

            cancellationToken.ThrowIfCancellationRequested();            

            if (EmbeddedSQLServerInstall)
            {
                #region SQL Server Installation

                if (sqlParameters == null)
                {
                    SendMessage(Properties.Resources.SQLInstallParametersHaveNotBeenSet);
                    return;
                }

                InstallSQLServer(sqlnstallType, sqlParameters);

                //If the SQL Server instance doesn't exists
                SQLConnectionIsValid(this.Connection);
                if (ConnectionResult == UtilityResult.SQLServerNotFound)
                {
                    throw new SQLServerNotFoundException(Properties.Resources.SQLServerInstanceDoesNotExist);
                }

                #endregion
            }

            if (scriptInfo == null)
            {
                scriptInfo = GetSQLScriptInfo();
            }

            string databaseName = Connection.Database;
            bool runAdminScript = false;
            
            //Count the total number of scripts that we need to run for database creation            
            // Create database
            if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
            {
                if (!DatabaseExists((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal, databaseName))
                {
                    if ((scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
                    {                        
                        updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.CreateDatabase && info.ScriptSubType == ScriptSubType.Normal);
                    }
                }
            }            

            // Create audit
            if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
            {
                if (!DatabaseExists((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit, databaseName))
                {
                    if ((scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
                    {
                        updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.CreateDatabase && info.ScriptSubType == ScriptSubType.Audit);
                    }
                }
            }            

            #region Create Database            

            if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
            {
                if (CreateDatabase(databaseName, scriptsToRun, scriptInfo, ScriptSubType.Normal, DatabaseType.Normal, cancellationToken, updateDatabaseProgressInfo))
                {
                    runAdminScript = true;
                }
                else
                {
                    ExecuteCommand("USE [" + orgDatabase + "]");
                }
            }

            if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
            {
                auditDatabase = orgDatabase + adtSuffix;
                //Necessary for code that validates that the connection is pointing to the right database 
                CreateDatabase(databaseName, scriptsToRun, scriptInfo, ScriptSubType.Audit, DatabaseType.Audit, cancellationToken, updateDatabaseProgressInfo);

                //Set the connection back to to the original database and set auditDatabase to an empty string
                ExecuteCommand("USE [" + orgDatabase + "]");
                auditDatabase = "";
            }


            if ((scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase &&
                (dbTypeToUpdate & DatabaseType.All) == DatabaseType.All && IsCloud &&
                databaseName.ToUpperInvariant() != "LSONEPOS")
            {
                // Add entry to login DB               
                //TODO: get the cloud user and cloud password from somewhere

                // Create the SQL cloud db user
                if (sqlUserName == "")
                {
                    sqlUserName = "DBU_" + Connection.Database;
                }
                if (!string.IsNullOrEmpty(sqlPassword))
                {
                    string createUserSQL =
                        @"CREATE LOGIN <sqlUser> WITH PASSWORD = '<sqlPassword>'
                      GO
                      
                      USE <database>                      
                      CREATE USER <sqlUser> FOR LOGIN <sqlUser>                      
                      ALTER ROLE db_owner ADD MEMBER <sqlUser>
                      
                      use <database_audit>
                      CREATE USER <sqlUser> FOR LOGIN <sqlUser>
                      ALTER ROLE db_owner ADD MEMBER <sqlUser>";

                    createUserSQL = createUserSQL.Replace("<sqlUser>", sqlUserName);
                    createUserSQL = createUserSQL.Replace("<sqlPassword>", sqlPassword);
                    createUserSQL = createUserSQL.Replace("<database>", databaseName);
                    createUserSQL = createUserSQL.Replace("<database_audit>", databaseName + "_Audit");

                    if (Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string[] sqlSplit = createUserSQL.Split(new[] { "GO" }, StringSplitOptions.None);

                        cmd.Connection = Connection;

                        foreach (string sqlPart in sqlSplit)
                        {
                            cmd.CommandText = sqlPart;
                            cmd.ExecuteNonQuery();
                        }

                        //AddLoginDBEntry(databaseName, cloudPassword, databaseName, sqlUserName, sqlPassword.ToString());
                    }
                }
            }

            #endregion
            GetMinVersion();

            #region Script counters
            // Reset counters and count total number of update scripts
            updateDatabaseProgressInfo.TotalScripts = updateDatabaseProgressInfo.CurrentScript = 0;

            // Update database, normal and audit
            if ((scriptsToRun & RunScripts.UpdateDatabase) == RunScripts.UpdateDatabase)
            {
                string dbVersion = "";

                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    
                    //Depending on which database we're updating (normal or audit) we need to check the current version
                    if (CurrentVersionType != "")
                    {
                        dbVersion = GetCurrentVersion(CurrentVersionType, DatabaseType.Normal);
                    }
                    else
                    {
                        dbVersion = GetCurrentVersion(VersionType.Database, DatabaseType.Normal);
                    }

                    var currVersion = new DatabaseVersion();
                    currVersion.ParseVersion(dbVersion);

                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count
                                                                (info =>
                                                                    info.ScriptType == RunScripts.UpdateDatabase //Only scripts that are of type UpdateDatabase
                                                                    && info.Version.DbVersion >= currVersion.DbVersion
                                                                    //Only scripts that have the same or higher DB version than the current version                        
                                                                    && info.ScriptSubType == ScriptSubType.Normal
                                                                    && ScriptCanBeRun(currVersion, info.Version, ScriptSubType.Normal));                                                                                                                                
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    dbVersion = GetCurrentVersion(VersionType.Database, DatabaseType.Audit);

                    var currVersion = new DatabaseVersion();
                    currVersion.ParseVersion(dbVersion);

                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count
                                                                (info =>
                                                                    info.ScriptType == RunScripts.UpdateDatabase //Only scripts that are of type UpdateDatabase
                                                                    && info.Version.DbVersion >= currVersion.DbVersion
                                                                    //Only scripts that have the same or higher DB version than the current version                        
                                                                    && info.ScriptSubType == ScriptSubType.Audit
                                                                    && ScriptCanBeRun(currVersion, info.Version, ScriptSubType.Audit));
                }
            }

            // Logic scripts
            if ((scriptsToRun & RunScripts.LogicScripts) == RunScripts.LogicScripts)
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.LogicScripts && info.ScriptSubType == ScriptSubType.Normal);
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.LogicScripts && info.ScriptSubType == ScriptSubType.Audit);
                }
            }

            // Cloud scripts
            if (((scriptsToRun & RunScripts.CloudScripts) == RunScripts.CloudScripts) && IsCloud && (dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
            {
                updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.CloudScripts && info.ScriptSubType == ScriptSubType.Normal);

                if (databaseName != "LSONEPOS")
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.CloudScripts && info.ScriptSubType == ScriptSubType.HBOOnly);
                }
            }
            
            // System scripts
            if (runAdminScript && (scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.SystemData && info.ScriptSubType == ScriptSubType.Normal);
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.SystemData && info.ScriptSubType == ScriptSubType.Audit);
                }
            }

            // User scripts
            if ((scriptsToRun & RunScripts.Users) == RunScripts.Users &&
                (!IsCloud ||
                IsCloud && databaseName == "LSONEPOS")

                )
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.Users && info.ScriptSubType == ScriptSubType.Normal);
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.Users && info.ScriptSubType == ScriptSubType.Audit);
                }
            }

            // Admin scripts
            if (runAdminScript)
            {                
                updateDatabaseProgressInfo.TotalScripts += scriptInfo.Count(info => info.ScriptType == RunScripts.AdminScript && info.ScriptSubType == ScriptSubType.Normal);
            }

            #endregion

            #region Update database            
            //Run update scripts - everytime the database is updated the stored procedures need to be updated as well
            if ((scriptsToRun & RunScripts.UpdateDatabase) == RunScripts.UpdateDatabase)
            {
                bool update = false;
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    UpdateTables(scriptInfo, databaseName, ScriptSubType.Normal, scriptsToRun, cancellationToken, updateDatabaseProgressInfo);
                    update = true;
                    if (cancelRun)
                    {
                        return;
                    }
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    auditDatabase = orgDatabase + adtSuffix;
                    //Necessary for code that validates that the connection is pointing to the right database 
                    UpdateTables(scriptInfo, databaseName, ScriptSubType.Audit, scriptsToRun, cancellationToken, updateDatabaseProgressInfo);
                    update = true;

                    //Set the connection back to to the original database and set auditDatabase to an empty string
                    ExecuteCommand("USE [" + orgDatabase + "]");
                    auditDatabase = "";
                }

                if (update && (scriptsToRun & RunScripts.LogicScripts) != RunScripts.LogicScripts)
                {
                    scriptsToRun |= RunScripts.LogicScripts;
                }


            }

            #endregion

            #region Logic scripts

            //Run Logic script - stored procedures
            if ((scriptsToRun & RunScripts.LogicScripts) == RunScripts.LogicScripts)
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    UpdateLogicScripts(scriptInfo, databaseName, ScriptSubType.Normal, updateDatabaseProgressInfo);

                    // Recompile dependant objects, for all tables, Stored procedures, functions, views etc etc
                    SendMessage(Properties.Resources.RecompilingObjects);
                    ExecuteCommand("exec spEXECsp_RECOMPILE");
                }



                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    auditDatabase = orgDatabase + adtSuffix;
                    //Necessary for code that validates that the connection is pointing to the right database 
                    UpdateLogicScripts(scriptInfo, databaseName, ScriptSubType.Audit, updateDatabaseProgressInfo);

                    //Set the connection back to to the original database and set auditDatabase to an empty string
                    ExecuteCommand("USE [" + orgDatabase + "]");
                    auditDatabase = "";
                }
            }

            #endregion
            #region Cloud scripts

            //Run Cloud Scripts
            if (((scriptsToRun & RunScripts.CloudScripts) == RunScripts.CloudScripts) && IsCloud)
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    UpdateCloudScripts(scriptInfo, databaseName, ScriptSubType.Normal, userGuid, cloudPassword, updateDatabaseProgressInfo);

                }

            }

            #endregion

            #region System Data scripts

            //Run System Data scripts
            if (runAdminScript && (scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    UpdateSystemDataScripts(scriptInfo, databaseName, ScriptSubType.Normal, updateDatabaseProgressInfo);

                    // Recompile dependant objects, for all tables, Stored procedures, functions, views etc etc
                    SendMessage(Properties.Resources.RecompilingObjects);
                    ExecuteCommand("exec spEXECsp_RECOMPILE");
                }
                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    auditDatabase = orgDatabase + adtSuffix;
                    //Necessary for code that validates that the connection is pointing to the right database 
                    UpdateSystemDataScripts(scriptInfo, databaseName, ScriptSubType.Audit, updateDatabaseProgressInfo);

                    //Set the connection back to to the original database and set auditDatabase to an empty string
                    ExecuteCommand("USE [" + orgDatabase + "]");
                    auditDatabase = "";
                }
            }

            #endregion

            if ((scriptsToRun & RunScripts.Users) == RunScripts.Users &&
                (!IsCloud ||
                IsCloud && databaseName == "LSONEPOS")

                )
            {
                if ((dbTypeToUpdate & DatabaseType.Normal) == DatabaseType.Normal)
                {
                    UpdateUsers(scriptInfo, databaseName, ScriptSubType.Normal, updateDatabaseProgressInfo);
                }

                if ((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit)
                {
                    UpdateUsers(scriptInfo, databaseName, ScriptSubType.Audit, updateDatabaseProgressInfo);
                }
            }

            if (runAdminScript)
            {
                RunAdminScripts(databaseName, scriptInfo, updateDatabaseProgressInfo);
            }
            //Make sure we are returning the connection pointing to the Normal database
            ExecuteCommand("USE [" + orgDatabase + "]");


        }

        /// <summary>
        /// Adds an entry to the LoginDB database for the given user credentials
        /// </summary>
        /// <param name="userName">The cloud user name</param>
        /// <param name="password">The cloud user passowrd</param>
        /// <param name="dbName">The name of the database the user has created</param>
        /// <param name="dbUser">The SQL server user name for the database</param>
        /// <param name="dbPassword">The SQL server user password for the database</param>
        public void AddLoginDBEntry(string userName, string password, string dbName, string dbUser, string dbPassword)
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            using (var cmd = new SqlCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText =
                    @"insert into LoginDB.dbo.Credentials(UserName, Password, DBName, DBUser, DBPassword)
                      values(@userName, @password, @dbName, @dbUser, @dbPassword)";

                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@passWord", password);
                cmd.Parameters.AddWithValue("@dbName", dbName);
                cmd.Parameters.AddWithValue("@dbUser", dbUser);
                cmd.Parameters.AddWithValue("@dbPassword", dbPassword);

                cmd.ExecuteNonQuery();
            }
        }


        private bool CreateDatabase(
            string databaseName, 
            RunScripts scriptsToRun, 
            LinkedList<ScriptInfo> scriptInfo,
            ScriptSubType scriptSubType,
            DatabaseType dbTypeToUpdate,
            CancellationToken cancellationToken,
            UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            if (!DatabaseExists((dbTypeToUpdate & DatabaseType.Audit) == DatabaseType.Audit, databaseName))
            {
                if ((scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
                {
                    CreateNewDatabase(databaseName, scriptInfo, scriptSubType, cancellationToken, updateDatabaseProgressInfo);

                    //At this point the ConnectionResult is DatabaseNotFound - but once it's been created it needs to be changed to Validated
                    ConnectionResult = UtilityResult.Validated;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Runs a specific script using the information in ScriptInfo
        /// </summary>
        /// <param name="scriptToRun">Information about the script to be run</param>
        public void RunSpecificScript(ScriptInfo scriptToRun)
        {
            ValidateConnection();
            if (scriptToRun.ResourceName == "")
            {
                throw new DatabaseUtilityException(Properties.Resources.NoResourceNameProvided);
            }
            RunScript(scriptToRun.ResourceName, true, Connection.Database);
        }       

        /// <summary>
        /// Runs a specific script on a Site Manager connection manager using the information in ScriptInfo
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="scriptToRun">Information about the script to be run</param>
        /// <param name="messageCallbackHandler">Pass a callback hander to get status callbacks sent back, else null if not wanting to get messages</param>
        public static void RunSpecificScript(IConnectionManager entry, ScriptInfo scriptToRun, MessageCallback messageCallbackHandler)
        {
            if (scriptToRun.ResourceName == "")
            {
                throw new DatabaseUtilityException(Properties.Resources.NoResourceNameProvided);
            }

#pragma warning disable 0612, 0618 // Disable warning on NativeConnection, we can use the NativeConnection here since we are only using it to pull the database name from it
            RunScript(entry, scriptToRun.ResourceName, messageCallbackHandler, ((SqlConnection)entry.Connection.NativeConnection).Database);
#pragma warning restore 0612, 0618
        }

        /// <summary>
        /// Fetches a specific script and returns it as a string
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="scriptToGet">Itformation about the script to be fetched</param>
        public static string GetSpecificScript(IConnectionManager entry, ScriptInfo scriptToGet)
        {
            if (scriptToGet.ResourceName == "")
            {
                throw new DatabaseUtilityException(Properties.Resources.NoResourceNameProvided);
            }

            var scriptAssembly = Assembly.GetExecutingAssembly();
            using (var textStreamReader = new StreamReader(scriptAssembly.GetManifestResourceStream(scriptToGet.ResourceName)))
            {
                string script = "";

                if (textStreamReader.Peek() >= 0)
                {
                    script = textStreamReader.ReadToEnd();
                }

                return script;
            }
        }

        #endregion

        #region Import and update demo data

        /// <summary>
        /// Imports demo data into the database in the current connection.
        /// All scripts that are in ImportData -> Base Demo Data are run.
        /// </summary>
        public void ImportDemoData()
        {
            ValidateConnection();
            SendMessage(Properties.Resources.LoadingDemoDataIntoTables);

            var scriptInfo = GetSQLScriptInfo();
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.DemoData).OrderBy(info => info.ScriptName))
            {
                RunScript(info.ResourceName, true, orgDatabase, false, "", "", true);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }

            SendMessage(Properties.Resources.DemoDataSuccessfullyLoaded);
        }

        #endregion

        #region Get Resources to run as SQL Scripts

        /// <summary>
        /// Adds a script assembly
        /// </summary>
        /// <param name="scriptAssemblyParam">The assembly to be added</param>
        public void AddScriptAssembly(Assembly scriptAssemblyParam)
        {
            this.ScriptAssembly = scriptAssemblyParam;
        }

        /// <summary>
        /// Retrieves all the SQL script available of a given type or types to be run by the Database Utility. No instance of the class is needed to run this variation.
        /// </summary>
        /// <param name="scriptType">The type or types of scripts to be retrieved. More than one types can be bitwise or-ed together</param>
        /// <returns></returns>
        public static List<ScriptInfo> GetSQLScriptInfo(RunScripts scriptType)
        {
            var scriptAssembly = Assembly.GetExecutingAssembly();

            var scriptInfo = new List<ScriptInfo>();
            var resNames = scriptAssembly.GetManifestResourceNames();
            foreach (string str in resNames)
            {
                var info = new ScriptInfo();
                info.ParseResourceName(str);

                if ((info.ScriptType & scriptType) != 0)
                {
                    scriptInfo.Add(info);
                }
            }
            return scriptInfo;
        }

        /// <summary>
        /// Retrieve all external data packages available for import
        /// </summary>
        /// <returns></returns>
        public static List<ScriptInfo> GetExternalSQLScriptInfo()
        {
            List<ScriptInfo> scriptInfos = new List<ScriptInfo>();

            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + DefaultDataPath;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] files = Directory.GetFiles(path, "*.xml");

                foreach (string file in files)
                {
                    scriptInfos.Add(new ScriptInfo
                    {
                        ScriptType = RunScripts.DefaultData,
                        IsLogicScript = false,
                        ScriptSubType = ScriptSubType.Normal,
                        ResourceName = file,
                        ScriptName = Path.GetFileNameWithoutExtension(file),
                        IsExternalScript = true
                    });
                }
            }
            catch(Exception)
            {
            }

            return scriptInfos;
        }

        /// <summary>
        /// Retrieves all the SQL script available to be run by the Database Utility
        /// </summary>
        /// <returns>A list of all SQL Scripts</returns>
        public LinkedList<ScriptInfo> GetSQLScriptInfo()
        {
            if (this.ScriptAssembly == null)
            {
                ScriptAssembly = Assembly.GetExecutingAssembly();
            }

            var scriptInfo = new LinkedList<ScriptInfo>();
            var resNames = ScriptAssembly.GetManifestResourceNames();
            foreach (string str in resNames)
            {
                var info = new ScriptInfo();
                info.ParseResourceName(str);
                scriptInfo.AddLast(info);
            }

            return scriptInfo;
        }

        #endregion

        #region Run Scripts

        private void CreateNewDatabase(string databaseName, LinkedList<ScriptInfo> scriptInfo, ScriptSubType subType, CancellationToken cancellationToken, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.ScriptToCreateTablesStarted);

            //Create the tables
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.CreateDatabase && info.ScriptSubType == subType).
                OrderBy(info => info.ScriptName))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    if (subType == ScriptSubType.Audit)
                    {
                        // The normal database exists at this point                        
                        DropDatabase(databaseName);

                        // We might be catching the token just before the first create script was run
                        if (DatabaseExists(true))
                        {                            
                            DropDatabase(databaseName + "_Audit");
                        }
                    }
                    else
                    {
                        if (DatabaseExists(false))
                        {                            
                            DropDatabase(databaseName);
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }                

                UpdateDatabaseProgressHandler?.Invoke(subType == ScriptSubType.Audit ? Properties.Resources.CreatingAuditDatabase : Properties.Resources.CreatingDatabase, 
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);
                
                RunScript(info.ResourceName, true, databaseName, false, "", "", true);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
        }

        private void UpdateLogicScripts(LinkedList<ScriptInfo> scriptInfo, string databaseName, ScriptSubType subType, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.ScriptToAddStoredProceduresStarted);
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.LogicScripts && info.ScriptSubType == subType).
                OrderBy(info => info.ScriptName))
            {
                string databaseType = subType == ScriptSubType.Normal ? "Normal" : "Audit";
                UpdateDatabaseProgressHandler?.Invoke(subType == ScriptSubType.Audit ? Properties.Resources.RunningAuditLogicScripts : Properties.Resources.RunningLogicScripts,
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);
                
                RunScript(info.ResourceName, false, databaseName);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
        }

        private void UpdateSystemDataScripts(LinkedList<ScriptInfo> scriptInfo, string databaseName, ScriptSubType subType, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.ScriptToAddStoredProceduresStarted);
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.SystemData && info.ScriptSubType == subType).
                OrderBy(info => info.ScriptName))
            {                
                UpdateDatabaseProgressHandler?.Invoke(Properties.Resources.RunningSystemDataScripts, 
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);

                RunScript(info.ResourceName, false, databaseName);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
        }

        private void UpdateCloudScripts(LinkedList<ScriptInfo> scriptInfo, string databaseName, ScriptSubType subType, Guid userGuid, string cloudPassword, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.ScriptToAddCloudData);
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.CloudScripts && info.ScriptSubType == subType).
                OrderBy(info => info.ScriptName))
            {
                UpdateDatabaseProgressHandler?.Invoke(Properties.Resources.UpdatingCloudDatabase,
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);

                RunScript(info.ResourceName, false, databaseName);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
            if (databaseName != "LSONEPOS")
            {
                foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.CloudScripts && info.ScriptSubType == ScriptSubType.HBOOnly).
                    OrderBy(info => info.ScriptName))
                {
                    UpdateDatabaseProgressHandler?.Invoke(Properties.Resources.UpdatingCloudHBODatabase, 
                                                          String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                          updateDatabaseProgressInfo.TotalScripts, 
                                                          ++updateDatabaseProgressInfo.CurrentScript);

                    RunScript(info.ResourceName, false, databaseName, false, "", "", false, userGuid, cloudPassword);

                    SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
                }
            }
        }

        private void UpdateUsers(LinkedList<ScriptInfo> scriptInfo, string databaseName, ScriptSubType subType, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.UserScriptsStarted);
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.Users && info.ScriptSubType == subType).
                OrderBy(info => info.ScriptName))
            {
                
                UpdateDatabaseProgressHandler?.Invoke(subType == ScriptSubType.Audit ? Properties.Resources.UpdatingAuditUsers : Properties.Resources.UpdatingUsers, 
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);

                RunScript(info.ResourceName, false, databaseName);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
        }

        private void RunAdminScripts(string databaseName, LinkedList<ScriptInfo> scriptInfo, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.RunningAdminScript);
            foreach (var info in scriptInfo.Where(info => info.ScriptType == RunScripts.AdminScript && info.ScriptSubType == ScriptSubType.Normal).
                OrderBy(info => info.ScriptName))
            {                
                UpdateDatabaseProgressHandler?.Invoke(Properties.Resources.CreatingAdminUsers, 
                                                      String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                      updateDatabaseProgressInfo.TotalScripts, 
                                                      ++updateDatabaseProgressInfo.CurrentScript);

                RunScript(info.ResourceName, false, databaseName);
                SendMessage(Properties.Resources.SQLScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
            }
        }

        private void UpdateTables(LinkedList<ScriptInfo> scriptInfo, string databaseName, ScriptSubType subType, RunScripts scriptsToRun, CancellationToken cancellationToken, UpdateDatabaseProgressInfo updateDatabaseProgressInfo)
        {
            SendMessage(Properties.Resources.DatabaseNeedsToBeUpgradedStartingUpgrade);

            string dbVersion = "";

            //Depending on which database we're updating (normal or audit) we need to check the current version
            if (CurrentVersionType != "")
            {
                dbVersion = GetCurrentVersion(CurrentVersionType, DatabaseType.Normal);
            }
            else if (subType == ScriptSubType.Audit)
            {
                dbVersion = GetCurrentVersion(VersionType.Database, DatabaseType.Audit);
            }
            else if (subType == ScriptSubType.Normal)
            {
                dbVersion = GetCurrentVersion(VersionType.Database, DatabaseType.Normal);
            }

            var currVersion = new DatabaseVersion();
            currVersion.ParseVersion(dbVersion);
            var scripts = scriptInfo.Where
                (info =>
                    info.ScriptType == RunScripts.UpdateDatabase //Only scripts that are of type UpdateDatabase
                    && info.Version.DbVersion >= currVersion.DbVersion
                    //Only scripts that have the same or higher DB version than the current version                        
                    && info.ScriptSubType == subType)
                .OrderBy(info => info.Version.DbVersion) //Order by DbVersion, PartnerVersion
                .ThenBy(info => info.Version.PartnerVersion);

            //Run the Update scripts that apply
            foreach (var info in scripts)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    // Only drop the database if we are explicitly calling DbUtil with the intention of creating the database.
                    if ((scriptsToRun & RunScripts.CreateDatabase) == RunScripts.CreateDatabase)
                    {                        
                        DropDatabase(databaseName);
                        DropDatabase(databaseName + "_Audit");
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (ScriptCanBeRun(currVersion, info.Version, subType))
                {
                    
                    UpdateDatabaseProgressHandler?.Invoke(subType == ScriptSubType.Audit ? Properties.Resources.UpdatingAuditDatabase : Properties.Resources.UpdatingDatabase,
                                                          String.Format(Properties.Resources.RunningScript, info.ScriptName), 
                                                          updateDatabaseProgressInfo.TotalScripts, 
                                                          ++updateDatabaseProgressInfo.CurrentScript);

                    RunScript(info.ResourceName, false, databaseName);
                    currVersion = UpdateDBCurrentVersion(info.Version, Database);
                    SetLastUpdatedTime();
                    SendMessage(Properties.Resources.UpdateScriptFinishedSuccessfully.Replace("#1", info.ScriptName));
                }
                if (cancelRun)
                {
                    return;
                }
            }
            SendMessage(Properties.Resources.AllUpgradesFinishedSuccessfully);
        }

        private bool ScriptCanBeRun(DatabaseVersion currentVersion, DatabaseVersion scriptVersion, ScriptSubType scriptSubType)
        {
            bool reply = scriptVersion.DbVersion == currentVersion.DbVersion && scriptVersion.PartnerVersion > currentVersion.PartnerVersion;
            int minVersion = 0;
            if (scriptSubType == ScriptSubType.Audit)
            {
                minVersion = auditDatabaseMinVer;
            }
            else if (scriptSubType == ScriptSubType.Normal)
            {
                minVersion = databaseMinVer;
            }
            if (scriptVersion.DbVersion > currentVersion.DbVersion && scriptVersion.DbVersion >= minVersion)
            {
                reply = true;
            }

            if (reply && triggerTargets.Keys.Contains(scriptVersion.DbVersion) && triggerTargets[scriptVersion.DbVersion] && updateTrigger != null && scriptSubType == ScriptSubType.Normal)
            {
                cancelRun = true;
                updateTrigger(Properties.Resources.SenderDatabaseUtil, scriptVersion.DbVersion);
                SetMinVersion(scriptVersion.DbVersion);

            }
            return reply;
        }

        /// <summary>
        /// Updates the current database version of the specified type
        /// </summary>
        /// <param name="updateVersion">Current database version</param>
        /// <param name="typeOfVersion">The type of version to update</param>
        /// <returns></returns>
        public DatabaseVersion UpdateDBCurrentVersion(DatabaseVersion updateVersion, string typeOfVersion)
        {
            string qry = "";
            try
            {
                qry = " UPDATE POSISINFO " +
                      " SET TEXT = '" + updateVersion + "' " +
                      " WHERE ID = '" + typeOfVersion + "' ";

                ExecuteCommand(qry);

                return updateVersion;
            }
            catch (RunScriptException ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorWhenUpdatingDatabaseVersion.Replace("#1", qry), ex);
            }
        }

        private void SetMinVersion(int version)
        {
            string qry = "";
            try
            {
                qry = " UPDATE POSISINFO " +
                      " SET TEXT = '" + version + "' " +
                      " WHERE ID = 'DB_MIN_VER' ";

                ExecuteCommand(qry);

            }
            catch (RunScriptException ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorWhenUpdatingDatabaseVersion.Replace("#1", qry), ex);
            }
        }

        private void SetLastUpdatedTime()
        {
            string qry = "";

            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            try
            {
                string checkIfExists = @"SELECT TEXT FROM POSISINFO WHERE ID = 'UPDATED_ON'";
                var checkCmd = new SqlCommand(checkIfExists);
                checkCmd.Connection = Connection;
                checkCmd.CommandType = CommandType.Text;
                object text = checkCmd.ExecuteScalar();
                using (var cmd = new SqlCommand())
                {
                    if (string.IsNullOrEmpty((string)text))
                    {
                        qry = "INSERT INTO POSISINFO (ID, TEXT) VALUES ('UPDATED_ON', '" + DateTime.Now + "')";
                    }
                    else
                    {
                        qry = " UPDATE POSISINFO SET TEXT = '" + DateTime.Now + "' WHERE ID = 'UPDATED_ON' ";
                    }

                    cmd.Connection = Connection;
                    cmd.CommandText = qry;
                    cmd.ExecuteNonQuery();
                }
            }

            catch (RunScriptException ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorWhenUpdatingDatabaseVersion.Replace("#1", qry), ex);
            }
        }

        /// <summary>
        /// Returns the current version of the default database (NB does NOT check the audit database)
        /// </summary>
        /// <param name="typeOfVersion">The version to check for in the database</param>
        /// <param name="databaseType">Which database to check</param>
        /// <param name="skipConnectionValidation">Set to true if you know you have a database connection that is working and good, this avoids database close</param>
        /// <returns>Returns the current DB version - default if nothing is found is 00000-00</returns>        
        public string GetCurrentVersion(string typeOfVersion, DatabaseType databaseType, bool skipConnectionValidation = false)
        {
            if (!skipConnectionValidation)
            {
                ValidateConnection();
            }

            const string defaultVersion = "00000-00";
            //Set the database connection to use the Audit database
            if ((databaseType & DatabaseType.Audit) == DatabaseType.Audit && !Connection.Database.ToUpperInvariant().Contains("_AUDIT"))
            {
                try
                {
                    if (auditDatabase == "" && Connection.Database.ToUpperInvariant() != "MASTER")
                    {
                        ExecuteCommand("USE [" + orgDatabase + "_Audit]");
                    }
                    else if (auditDatabase != "")
                    {
                        ExecuteCommand("USE [" + auditDatabase + "]");
                    }
                }
                catch (RunScriptException)
                {
                    return defaultVersion;
                }
            }
            else if ((databaseType & DatabaseType.Normal) == DatabaseType.Normal && Connection.Database.ToUpperInvariant() != orgDatabase)
            {
                try
                {
                    ExecuteCommand("USE [" + orgDatabase + "]");
                }
                catch (RunScriptException)
                {
                    return defaultVersion;
                }
            }

            string qry = "SELECT COALESCE(TEXT, '00000-00') DBVERSION FROM POSISINFO WHERE ID = '" + typeOfVersion + "'";
            var db = GetTable(qry);
            if (db.Rows.Count > 0)
            {
                return db.Rows[0]["DBVERSION"].ToString();
            }

            //No DBVERSION in the database
            var ins = "INSERT INTO POSISINFO (ID, TEXT) VALUES ('" + typeOfVersion + "', '00000-00')";
            ExecuteCommand(ins);

            //Set the connection back to use the original database
            if ((databaseType & DatabaseType.Audit) == DatabaseType.Audit && Connection.Database.ToUpperInvariant().Contains("_AUDIT"))
            {
                ExecuteCommand("USE [" + Connection.Database + "]");
            }
            return defaultVersion;
        }



        /// <summary>
        /// Returns the current version of the default database (NB does NOT check the audit database)
        /// </summary>
        /// <param name="versionType">Which version to check</param>
        /// <param name="skipConnectionValidation">If true then connection validation is skipped</param>
        /// <returns>Returns the current DB version - default if nothing is found is 00000-00</returns>
        public string GetCurrentVersion(VersionType versionType, bool skipConnectionValidation = false)
        {
            return GetCurrentVersion(versionType, DatabaseType.Normal, skipConnectionValidation);
        }

        /// <summary>
        /// Returns the current DB version - default if nothing is found is 00000-00
        /// </summary>
        /// <param name="databaseType">Which database to check for updates; default database or the audit database</param>
        /// <param name="versionType">Which version to check</param>
        /// <param name="skipConnectionValidation">Set to true if you know you have a database connection that is working and good, this avoids database close</param>
        /// <returns>The current version of the database</returns>
        public string GetCurrentVersion(VersionType versionType, DatabaseType databaseType, bool skipConnectionValidation = false)
        {
            string typeOfVersion = "";
            if (versionType == VersionType.Database)
            {
                typeOfVersion = Database;
            }
            else if (versionType == VersionType.DemoData)
            {
                typeOfVersion = Demodata;
            }
            return GetCurrentVersion(typeOfVersion, databaseType, skipConnectionValidation);
        }

        private void RunScript(string resourceName, bool sendUpdateMsg, string databaseName)
        {
            RunScript(resourceName, sendUpdateMsg, databaseName, false, "", "", false);
        }

        private static void RunScript(IConnectionManager entry, string resourceName, MessageCallback messageCallbackHandler, string databaseName)
        {
            RunScript(entry, resourceName, messageCallbackHandler, databaseName, false, "", "", false);
        }

        private void RunScript(string resourceName, bool sendUpdateMsg, string databaseName, bool demoDataUpdate,
            string storeId, string terminalId, bool useMasterConnection)
        {
            string text = "";
            try
            {
                //Templates don't need to be run
                if (resourceName.ToUpperInvariant().Contains("TEMPLATE.SQL"))
                    return;

                string useDatabase = ValidateDatabase(useMasterConnection);

                if (useDatabase != "")
                {
                    ExecuteCommand(useDatabase);
                }

                if (ScriptAssembly == null)
                {
                    ScriptAssembly = Assembly.GetExecutingAssembly();
                }

                using (var textStreamReader = new StreamReader(ScriptAssembly.GetManifestResourceStream(resourceName)))
                {
                    while (textStreamReader.Peek() >= 0)
                    {
                        string textLine = textStreamReader.ReadLine();
                        if (textLine.Trim().ToUpperInvariant() == "GO")
                        {
                            if (sendUpdateMsg)
                            {
                                SendUpdateMessages(text, MessageCallbackHandler);
                            }

                            if (demoDataUpdate)
                            {
                                text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                            }

                            text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + DataAreaId + "'");
                            if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                            {
                                text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                            }
                            ExecuteCommand(text);
                            text = "";
                        }
                        else
                        {
                            text += textLine + "\n";
                        }
                    }

                    //If there is no GO in the SQL statement then run the entire statement
                    if (text.Trim() != "" && text.Trim().ToUpperInvariant() != "GO")
                    {
                        if (demoDataUpdate)
                        {
                            text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                        }

                        text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + DataAreaId + "'");
                        if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                        {
                            text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                        }
                        if (text.Trim().Length > 0)
                            ExecuteCommand(text);
                    }
                }
            }
            catch (Exception x)
            {
                throw new RunScriptException(resourceName + ": " + x.Message, Connection, x);
            }
        }



        private void RunScript(string resourceName, bool sendUpdateMsg, string databaseName, bool demoDataUpdate,
            string storeId, string terminalId, bool useMasterConnection, Guid userGuid, string cloudPassword)
        {
            string text = "";
            try
            {
                //Templates don't need to be run
                if (resourceName.ToUpperInvariant().Contains("TEMPLATE.SQL"))
                    return;

                string useDatabase = ValidateDatabase(useMasterConnection);

                if (useDatabase != "")
                {
                    ExecuteCommand(useDatabase);
                }

                if (ScriptAssembly == null)
                {
                    ScriptAssembly = Assembly.GetExecutingAssembly();
                }

                using (var textStreamReader = new StreamReader(ScriptAssembly.GetManifestResourceStream(resourceName)))
                {
                    while (textStreamReader.Peek() >= 0)
                    {
                        string textLine = textStreamReader.ReadLine();
                        if (textLine.Trim().ToUpperInvariant() == "GO")
                        {
                            if (sendUpdateMsg)
                            {
                                SendUpdateMessages(text, MessageCallbackHandler);
                            }

                            if (demoDataUpdate)
                            {
                                text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                            }

                            text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + DataAreaId + "'").Replace("ADMINUSERGUID", userGuid.ToString()).Replace("'CLOUDPASSWORD'", "'" + cloudPassword + "'");
                            if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                            {
                                text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                            }
                            ExecuteCommand(text);
                            text = "";
                        }
                        else
                        {
                            text += textLine + "\n";
                        }
                    }

                    //If there is no GO in the SQL statement then run the entire statement
                    if (text.Trim() != "" && text.Trim().ToUpperInvariant() != "GO")
                    {
                        if (demoDataUpdate)
                        {
                            text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                        }

                        text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + DataAreaId + "'").Replace("ADMINUSERGUID", userGuid.ToString()).Replace("'CLOUDPASSWORD'", "'" + cloudPassword + "'");
                        if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                        {
                            text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                        }
                        if (text.Trim().Length > 0)
                            ExecuteCommand(text);
                    }
                }
            }
            catch (Exception x)
            {
                throw new RunScriptException(resourceName + ": " + x.Message, Connection, x);
            }
        }

        private static void RunScript(IConnectionManager entry, string resourceName, MessageCallback messageCallbackHandler,
            string databaseName, bool demoDataUpdate, string storeId, string terminalId, bool useMasterConnection)
        {
            string text = "";

            try
            {
                //Templates don't need to be run
                if (resourceName.ToUpperInvariant().Contains("TEMPLATE.SQL"))
                    return;

#pragma warning disable 618 // Disable warning on NativeConnection, we can use the NativeConnection here since we are only using it to pull the database name from it
                if (((SqlConnection)entry.Connection.NativeConnection).Database.ToUpperInvariant() != databaseName.ToUpperInvariant())
                {
                    using (var cmd = entry.Connection.CreateCommand("USE [" + databaseName.ToUpperInvariant() + "]\r\n"))
                    {
                        entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                    }
                }
#pragma warning restore 618

                var scriptAssembly = Assembly.GetExecutingAssembly();
                using (var textStreamReader = new StreamReader(scriptAssembly.GetManifestResourceStream(resourceName)))
                {
                    while (textStreamReader.Peek() >= 0)
                    {
                        string textLine = textStreamReader.ReadLine();
                        if (textLine.Trim().ToUpperInvariant() == "GO")
                        {
                            if (messageCallbackHandler != null)
                            {
                                SendUpdateMessages(text, messageCallbackHandler);
                            }

                            if (demoDataUpdate)
                            {
                                text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                            }

                            text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + entry.Connection.DataAreaId + "'");
                            if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                            {
                                text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                            }

                            using (var cmd = entry.Connection.CreateCommand(text))
                            {
                                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                            }

                            text = "";
                        }
                        else
                        {
                            text += textLine + "\n";
                        }
                    }

                    //If there is no GO in the SQL statement then run the entire statement
                    if (text.Trim() != "" && text.Trim().ToUpperInvariant() != "GO")
                    {
                        if (demoDataUpdate)
                        {
                            text = text.Replace("'DEFAULTSTORE'", "'" + storeId + "'").Replace("'DEFAULTERMINAL'", "'" + terminalId + "'");
                        }

                        text = text.Replace("LSPOSNET", databaseName).Replace("'LSR'", "'" + entry.Connection.DataAreaId + "'");
                        if (!string.IsNullOrEmpty(Properties.Resources.CloudServer))
                        {
                            text = text.Replace("<PARTNERCLOUDSERVER>", Properties.Resources.CloudServer);
                        }

                        using (var cmd = entry.Connection.CreateCommand(text))
                        {
                            entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                        }
                    }
                }
            }
            catch (Exception x)
            {
#pragma warning disable 618 // Disable warning on NativeConnection
                throw new RunScriptException(resourceName + ": " + x.Message, (SqlConnection)entry.Connection.NativeConnection, x);
#pragma warning restore 618
            }
        }

        private static void SendUpdateMessages(string text, MessageCallback callBack)
        {
            try
            {
                string msg = "";
                if (text.ToUpperInvariant().Contains("CREATE TABLE"))
                {
                    int startpos = text.ToUpperInvariant().IndexOf("CREATE TABLE") + 13;
                    string tmpStr = text.Substring(startpos, text.Length - startpos - 1);
                    if (tmpStr.ToUpperInvariant().IndexOf("[DBO].") == 0)
                    {
                        startpos = tmpStr.ToUpperInvariant().IndexOf("[DBO].") + 6;
                        tmpStr = tmpStr.Substring(startpos, tmpStr.Length - startpos);
                    }
                    msg = Properties.Resources.TableCreatedSuccessfully.Replace("#1", tmpStr.Substring(0, tmpStr.IndexOf("(")));
                }

                else if (text.ToUpperInvariant().Contains("CREATE PROCEDURE"))
                {
                    int startpos = text.ToUpperInvariant().IndexOf("CREATE PROCEDURE") + 17;
                    string tmpStr = text.Substring(startpos, text.Length - startpos - 1);
                    if (tmpStr.ToUpperInvariant().IndexOf("[DBO].") == 0)
                    {
                        startpos = tmpStr.ToUpperInvariant().IndexOf("[DBO].") + 6;
                        tmpStr = tmpStr.Substring(startpos, tmpStr.Length - startpos);
                        if (tmpStr.ToUpperInvariant().IndexOf("]") > 0)
                        {
                            tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("]") + 1);
                        }
                        if (tmpStr.ToUpperInvariant().IndexOf("(") > 0)
                        {
                            tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("("));
                        }
                        if (tmpStr.ToUpperInvariant().IndexOf("@") > 0)
                        {
                            tmpStr = tmpStr.Substring(0, tmpStr.IndexOf("@"));
                        }
                    }
                    msg = Properties.Resources.StoredProcedureCreatedSuccessfully.Replace("#1", tmpStr);
                }

                if (msg != "")
                {
                    SendMessage(msg, callBack);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region SQL Server Installations

        /// <summary>
        /// Checks if the operating system is a 64 bit system or 32
        /// </summary>
        /// <returns>Returns true if the operating system is 64 bit</returns>
        public bool IsOS64Bit()
        {
            var det64Bit = new Detecting64bit();
            return det64Bit.IsOS64Bit();
        }

        /// <summary>
        /// Returns a list of all SQL Instances available on the machine
        /// </summary>
        /// <returns>A list of all SQL Instances</returns>
        public List<SQLExpressInfo> ReturnSQLInstances()
        {
            try
            {
                var expreDetection = new SQLDetection();
                return expreDetection.EnumerateSQLInstances();
            }
            catch (DatabaseUtilityException dbEx)
            {
                throw new DatabaseUtilityException("", dbEx);
            }
        }

        /// <summary>
        /// Returns true if a specific SQL server and instance name exist. WILL NOT WORK FOR LOCALHOST SERVER NAME!
        /// </summary>
        /// <param name="serverName">The SQL Server name to check</param>
        /// <param name="instanceName">The SQL Server Instance name to check</param>
        /// <returns></returns>
        public bool SQLInstanceExists(string serverName, string instanceName)
        {
            try
            {
                var expreDetection = new SQLDetection();
                return expreDetection.SQLInstanceExists(serverName, instanceName);
            }
            catch (DatabaseUtilityException dbEx)
            {
                throw new DatabaseUtilityException("", dbEx);
            }
        }

        /// <summary>
        /// Installs a SQL Server 2005 Express, SQL Server 2008 Express or SQL CE
        /// </summary>
        /// <param name="sqlInstallType">What type of SQL server should be installed</param>
        /// <param name="sqlParameters">The command prompt parameters to be used when installing the SQL server. If value is null then default parameters will be used</param>
        public void InstallSQLServer(SQLInstall sqlInstallType, SQLParams sqlParameters)
        {
            try
            {
                switch (sqlInstallType)
                {
                    //case SQLInstall.SQL2005Expr:
                    //    if (sqlParameters == null) { sqlParameters = new SQL2005Params(); }

                    //    var sql2005Inst = new InstallSQL2005Expr((SQL2005Params)sqlParameters);                                               
                    //    sql2005Inst.InstallExpress();
                    //    sql2005Inst.SqlServerName();
                    //    break;

                    //case SQLInstall.SQL2008Expr:
                    //    if (sqlParameters == null) { sqlParameters = new SQL2008Params(); }
                    //    if (!(sqlParameters as SQL2008Params).x64BitComputer) { (sqlParameters as SQL2008Params).x64BitComputer = IsOS64Bit(); }

                    //    var sql2008Inst = new InstallSQL2008Expr((SQL2008Params)sqlParameters);
                    //    sql2008Inst.MessageCallbackHandler += sql2008Inst_MessageCallbackHandler;                            

                    //    sql2008Inst.InstallExpress();
                    //    sql2008Inst.SqlServerName();
                    //    break;
                    case SQLInstall.SQL2017Expr:
                        if (sqlParameters == null) { sqlParameters = new SQL2017Params(); }
                        if (!(sqlParameters as SQL2017Params).x64BitComputer) { (sqlParameters as SQL2017Params).x64BitComputer = IsOS64Bit(); }

                        var sql2017Inst = new InstallSQL2017Express((SQL2017Params)sqlParameters);
                        sql2017Inst.MessageCallbackHandler += sql2008Inst_MessageCallbackHandler;

                        sql2017Inst.InstallExpress();
                        sql2017Inst.SqlServerName();
                        break;

                    case SQLInstall.SQLCompactEdition:
                        throw new DatabaseUtilityException(Properties.Resources.SQLInstallTypeNotImplemented);
                }
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
        }

        #endregion

        #region SQL Server information

        /// <summary>
        /// Returns a list of Databases in a specific SQL server
        /// </summary>
        /// <param name="sqlServer">The SQL Server to check</param>
        /// <param name="windowsAuthentication">Should the connection be done using Windows Authentication?</param>
        /// <param name="userName">User name to use to connect to the SQL Server</param>
        /// <param name="password">Password to use to connect to the SQL Server</param>
        /// <param name="connectionType">The type of connection to use</param>
        /// <returns></returns>
        public LinkedList<DatabaseInfo> ReturnDatabases(string sqlServer, bool windowsAuthentication, string userName, string password, ConnectionType connectionType)
        {
            try
            {
                var dbInfoList = new LinkedList<DatabaseInfo>();
                masterConnectionStr = CreateConnectionString(sqlServer, "Master", windowsAuthentication, userName, password, connectionType);

                var db = GetTable("EXEC SP_HELPDB", true);

                var allDBs =
                    from DataRow row in db.Rows
                    select row;

                foreach (var row in allDBs)
                {
                    var dbInfo = new DatabaseInfo
                    {
                        Name = row["name"].ToString(),
                        Size = row["db_size"].ToString(),
                        Owner = row["owner"].ToString(),
                        Created = row["created"].ToString(),
                        Status = row["status"].ToString(),
                        Compatability = row["compatibility_level"].ToString()
                    };
                    dbInfoList.AddLast(dbInfo);
                }

                return dbInfoList;
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorGettingDatabaseInformation, ex);
            }
        }

        /// <summary>
        /// Returns a list of all tables in a database - NOT YET IMPLEMENTED
        /// </summary>
        /// <returns></returns>
        public LinkedList<TableInfo> ReturnTableInformation()
        {
            try
            {
                throw new DatabaseUtilityException("Not yet implemented");
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorClosingTheDatabaseConnection, ex);
            }
        }

        #endregion

        #region Misc SQL Connection functions

        /// <summary>
        /// Closes the SQL connection
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (Connection == null)
                    return;

                this.Connection.Close();
                SendMessage(Properties.Resources.ConnectionIsClosed);
            }
            catch (DatabaseUtilityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorClosingTheDatabaseConnection, ex);
            }
        }

        /// <summary>
        /// Creates a connection string from the parameters
        /// </summary>
        /// <param name="sqlServer">The SQL server</param>
        /// <param name="databaseName">The database to connect to</param>
        /// <param name="windowsAuthentication">If true Windows Authentication is used to connect to the SQL server</param>
        /// <param name="userName">If using SQL Server authentication then this user name is used to connect to the SQL server</param>
        /// <param name="password">If using SQL Server authentication then this password is used to connect to the SQL server</param>
        /// <param name="connectionType">The type of connection to use</param>
        public string CreateConnectionString(string sqlServer, string databaseName, bool windowsAuthentication, string userName, string password, ConnectionType connectionType)
        {
            if (sqlServer == "" || databaseName == "")
            {
                throw new DatabaseUtilityException(Properties.Resources.ConnectionStringCannotBeCreated);
            }

            string connectionString = @"Data Source=" + sqlServer + ";Initial Catalog=" + databaseName;
            if (windowsAuthentication)
                connectionString += ";Integrated Security=True;";
            else
                connectionString += ";Integrated Security=False;UID=" + userName + ";PWD=" + password + ";";

            switch (connectionType)
            {
                case ConnectionType.TCP_IP:
                    connectionString += "Network Library=DBMSSOCN;";
                    break;
                case ConnectionType.SharedMemory:
                    connectionString += "Network Library=DBMSLPCN;";
                    break;
                case ConnectionType.NamedPipes:
                    connectionString += "Network Library=DBNMPNTW;";
                    break;
            }

            return connectionString;
        }

        /// <summary>
        /// Checks if the SqlConnection is valid. The results can be seen in property ConnectionResult
        /// </summary>
        /// <param name="connection">The SqlConnection to be checked</param>
        public void SQLConnectionIsValid(SqlConnection connection)
        {
            if (connection == null)
            {
                EmbeddedSQLServerInstall = true;
                ConnectionResult = UtilityResult.SQLServerNotFound;
                return;
            }

            orgDatabase = this.Connection.Database;
            masterConnectionStr = CreateConnectionString(
                RetrieveServerName(connection.ConnectionString), "master",
                RetrieveWindowsAuthentication(connection.ConnectionString),
                RetrieveUserName(connection.ConnectionString),
                RetrievePassword(connection.ConnectionString),
                RetrieveConnectionType(connection.ConnectionString));

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
                ConnectionResult = UtilityResult.Validated;
            }
            catch (SqlException ex)
            {
                string message = Properties.Resources.ConnectionNotValid;
                if (ex.Number == 4060)
                {
                    ConnectionResult = UtilityResult.DatabaseNotFound;
                    return;
                }
                if (ex.Number == 18456)
                {
                    message = Properties.Resources.FailedToConnectToSQLServer + "\n" + Properties.Resources.VerifyUserAndPassword;
                }
                throw new DatabaseUtilityException(message, ex);
            }

            /*
            #region Check Server Name
            string InstanceName = "";
            string ServerName = RetrieveServerName(connection.ConnectionString);
            if (ServerName.Contains(@"\"))
            {
                int start = ServerName.IndexOf(@"\");
                InstanceName = ServerName.Substring(start + 1, ServerName.Length - start - 1);
                ServerName = ServerName.Substring(0, start);                        
            }

            SQLDetection sqlDet = new SQLDetection();

            //If the server name is LOCALHOST then we need to check the registry to make sure
            //that the LOCALHOST is SQLExpress.
            if (ServerName.ToUpperInvariant().Contains("LOCALHOST"))
            {
                ServerName.ToUpperInvariant().Replace("LOCALHOST", Environment.MachineName);

                if (!sqlDet.SQLInstanceExists(ServerName, InstanceName))
                {
                    EmbeddedSQLServerInstall = true;
                    ConnectionResult = UtilityResult.SQLServerNotFound;
                    return;
                }                       
            }
            else if (!sqlDet.SQLInstanceExists(ServerName, InstanceName))
            {
                EmbeddedSQLServerInstall = true;
                ConnectionResult = UtilityResult.SQLServerNotFound;
                return;
            }
            #endregion

            #region Check Database
            if (!DatabaseExists(false))
            {
                ConnectionResult = UtilityResult.DatabaseNotFound;
                return;
            }               
            #endregion

            ConnectionResult = UtilityResult.Validated;
            */
        }

        /// <summary>
        /// Returns true if the SQL Server in the connection string exists. Will not work for SQLExpress instances
        /// </summary>
        /// <param name="connection">The connection to check</param>
        /// <returns>Returns true if the instance exists</returns>
        private bool SQLServerInstanceExists(SqlConnection connection)
        {
            return SQLServerInstanceExists(connection.ConnectionString);
        }

        /// <summary>
        /// Returns true if the SQL Server instance in the connection string exists
        /// </summary>        
        /// <param name="connStr">The connection string to check</param>
        /// <returns>Returns true if the instance exists</returns>
        private bool SQLServerInstanceExists(string connStr)
        {
            bool result = false;
            try
            {
                string serverName = RetrieveServerName(connStr);

                if (serverName.Contains(@"\"))
                {
                    serverName = serverName.Substring(0, serverName.IndexOf(@"\"));
                }

                var ping = new System.Net.NetworkInformation.Ping();
                var reply = ping.Send(serverName, 2000);

                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// Returns true if the database exists in the current SQL Connection
        /// </summary>
        /// <param name="dbName">The name of the database to check for</param>
        /// <param name="isAuditDatabase">Does the functionality check for the Normal or Audit database?</param>
        /// <returns></returns>
        private bool DatabaseExists(bool isAuditDatabase, string dbName = null)
        {            
            if (dbName == null)
            {
                dbName = Connection.Database;
            }

            if (isAuditDatabase && !dbName.Contains("_Audit"))
            {
                dbName += "_Audit";
            }

            string queryString = "SELECT COUNT(*) FROM master..sysdatabases WHERE NAME = '" + dbName + "'";
            DataTable dbExists = GetTable(queryString, true);
            if (dbExists.Rows.Count > 0)
            {
                if (Convert.ToInt32(dbExists.Rows[0][0]) == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retrieves the user name from the connection string
        /// </summary>
        /// <param name="connectionString">Connection string for SQL Server</param>
        /// <returns>Returns the user name</returns>
        private static string RetrieveUserName(string connectionString)
        {
            string searchString = "";
            if (connectionString.ToUpperInvariant().Contains("USER ID"))
                searchString = "USER ID";

            else if (connectionString.ToUpperInvariant().Contains("UID"))
                searchString = "UID";

            else if (connectionString.ToUpperInvariant().Contains("USER"))
                searchString = "USER";

            if (searchString != string.Empty)
            {
                string tempConnStr = connectionString.Substring(connectionString.ToUpperInvariant().IndexOf(searchString));

                int startpos = tempConnStr.IndexOf('=') + 1;
                int endpos = tempConnStr.IndexOf(';');
                if (endpos < 0)
                    endpos = tempConnStr.Length;
                int length = endpos - startpos;
                if (length < 1)
                    return "";
                return tempConnStr.Substring(startpos, length);
            }
            return "";
        }

        /// <summary>
        /// Retrieves the password from the connection string
        /// </summary>
        /// <param name="connectionString">Connection string for the SQL Server</param>
        /// <returns>Returns the password</returns>
        private static string RetrievePassword(string connectionString)
        {
            string searchString = "";
            if (connectionString.ToUpperInvariant().Contains("PASSWORD"))
                searchString = "PASSWORD";

            if (connectionString.ToUpperInvariant().Contains("PWD"))
                searchString = "PWD";

            if (searchString != string.Empty)
            {
                string tempConnStr = connectionString.Substring(connectionString.ToUpperInvariant().IndexOf(searchString));

                int startpos = tempConnStr.IndexOf('=') + 1;
                int endpos = tempConnStr.IndexOf(';');
                if (endpos < 0)
                    endpos = tempConnStr.Length;
                int length = endpos - startpos;
                if (length < 1)
                    return "";
                return tempConnStr.Substring(startpos, length);
            }
            return "";
        }

        /// <summary>
        /// Retrieves the Network Library from the connection string
        /// </summary>
        /// <param name="connectionString">Connection string for SQL Server</param>
        /// <returns>Returns the Network Library</returns>
        private static ConnectionType RetrieveConnectionType(string connectionString)
        {
            string searchString = "";
            ConnectionType connectionType = ConnectionType.NamedPipes;

            if (connectionString.ToUpperInvariant().Contains("NETWORK LIBRARY"))
                searchString = "NETWORK LIBRARY";

            if (searchString != string.Empty)
            {
                string tempConnStr = connectionString.Substring(connectionString.ToUpperInvariant().IndexOf(searchString));

                int startpos = tempConnStr.IndexOf('=') + 1;
                int endpos = tempConnStr.IndexOf(';');
                if (endpos < 0)
                    endpos = tempConnStr.Length;
                int length = endpos - startpos;
                if (length < 1)
                    return connectionType;
                string connectionTypeString = tempConnStr.Substring(startpos, length);

                if (connectionTypeString.ToUpperInvariant() == "DBMSSOCN")
                {
                    connectionType = ConnectionType.TCP_IP;
                }
                else if (connectionTypeString.ToUpperInvariant() == "DBMSLPCN")
                {
                    connectionType = ConnectionType.SharedMemory;
                }
            }
            return connectionType;
        }

        /// <summary>
        /// Returns true if Windows Authentication is used to connect to the SQL Server
        /// </summary>
        /// <param name="connectionString">Connection string for the SQL SErver</param>
        /// <returns>Returns true if Windows Authentication is used to connect to the SQL Server</returns>
        private static bool RetrieveWindowsAuthentication(string connectionString)
        {
            return connectionString.IndexOf("Integrated Security=True") > 0;
        }

        /// <summary>
        /// Retrieves the server name from the connection string
        /// </summary>
        /// <param name="connStr">Connection string for the SQL server</param>
        /// <returns>Returns the server name</returns>
        private static string RetrieveServerName(string connStr)
        {
            int startpos = connStr.IndexOf('=') + 1;
            int endpos = connStr.IndexOf(';');
            int length = endpos - startpos;

            if (length < 1)
                return connStr;

            return connStr.Substring(startpos, length);
        }

        #endregion

        #region SQL Commands        
        /// <summary>
        /// Runs a SQL statement and returns the results in a DataTable        
        /// </summary>
        /// <param name="sql">The SQL statement</param>
        /// <param name="useMasterConnection">If true then a sql connection to the master database is used for the sql statement</param>
        /// <returns>A DataTable containing the result data</returns>
        private System.Data.DataTable GetTable(string sql, bool useMasterConnection)
        {
            try
            {
                var table = new DataTable();

                if (useMasterConnection)
                {
                    SetMasterConnection();
                }

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                var command = new SqlCommand(sql, Connection);

                var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);

                return table;
            }
            catch (Exception x)
            {
                throw new DatabaseUtilityException(Properties.Resources.DatabaseUtilGetTableError.Replace("#1", x.Message).Replace("#2", sql), x);
            }
        }

        private void SetMasterConnection()
        {
            try
            {
                if (Connection != null && Connection.State != ConnectionState.Open)
                    Connection.Close();

                Connection = new SqlConnection(masterConnectionStr);

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
            }
            catch (Exception x)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorSettingMasterConnection, x);
            }
        }

        /// <summary>
        /// Runs a SQL statement and returns the results in a DataTable        
        /// </summary>
        /// <param name="sql">The SQL statement</param>
        /// <returns>A DataTable containing the result data</returns>
        private DataTable GetTable(string sql)
        {
            return GetTable(sql, false);
        }

        private void ExecuteCommand(string sqlQuery)
        {
            ExecuteCommand(sqlQuery, false);
        }

        private void ExecuteCommand(string sqlQuery, bool useMasterConnection)
        {
            try
            {
                if (useMasterConnection)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        Connection.Close();
                    }
                    Connection.ConnectionString = masterConnectionStr;
                }

                var command = new SqlCommand(sqlQuery, Connection);

                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }
                command.CommandTimeout = 600;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new RunScriptException(Properties.Resources.ErrorExecutingQuery.Replace("#1", sqlQuery), Connection, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string ValidateDatabase(bool useMaster)
        {
            if (useMaster)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.ConnectionString = masterConnectionStr;
                return "";
            }

            //auditDatabase is only filled out when the UpdateAuditDatabase is running
            if (auditDatabase != "" && Connection.Database.ToUpperInvariant() != auditDatabase.ToUpperInvariant())
            {
                return "USE [" + auditDatabase.ToUpperInvariant() + "]\r\n";
            }
            if (auditDatabase == "" && Connection.Database.ToUpperInvariant() != orgDatabase.ToUpperInvariant())
            {
                return "USE [" + orgDatabase.ToUpperInvariant() + "]\r\n";
            }

            return "";
        }

        /// <summary>
        /// Drops the given database
        /// </summary>
        /// <param name="databaseName">The database to drop</param>
        public void DropDatabase(string databaseName)
        {            
            ExecuteCommand($"ALTER DATABASE {databaseName} SET  SINGLE_USER WITH ROLLBACK IMMEDIATE", true);
            ExecuteCommand($"DROP DATABASE {databaseName}", true);
        }

        #endregion
    }
}
