using System.Diagnostics;
using System.IO;
using System.Text;
using LSOne.DataLayer.DatabaseUtil.Exceptions;
using LSOne.DataLayer.DatabaseUtil.Properties;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// 
    /// </summary>
    public class InstallSQL2012Express : InstallSQLExpress
    {
        /// <summary>
        /// When using the InstallSQL2008Expr the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the class. 
        /// This is done instead of having a user interface with progress information.
        /// </summary>
        public event InstallingSQL2008MessageCallback MessageCallbackHandler;

        /// <summary>
        /// The parameters required to install the SQL 2008 Express server quietly. Default parameters can be changed before installation.
        /// </summary>
        public SQL2012Params SQLParameters { get; set; }

        #region Constructor

        /// <summary>
        /// Initialises the class and sets the parameters
        /// </summary>
        /// <param name="SQLParameters">The SQL 2008 parameters that should be used when installing the SQL Server</param>
        public InstallSQL2012Express(SQL2012Params SQLParameters)
        {
            this.SQLParameters = SQLParameters;
        }        

        #endregion

        #region Overridden functions

        /// <summary>
        /// Sends a message through the message callback function
        /// </summary>
        /// <param name="msg">The message to send</param>
        public override void SendMessage(string msg)
        {
            if (MessageCallbackHandler != null)
            {
                MessageCallbackHandler(Resources.SenderInstallSQL2008, msg);
            }
        }

        /// <summary>
        /// After the SQL Server has been installed this will return the instance name of the new SQL Server
        /// </summary>
        /// <returns></returns>
        public override string SqlServerName()
        {
            return @"LOCALHOST\" + SQLParameters.InstanceName;
        }
        
        #endregion

        #region Public functions

        /// <summary>
        /// Creates the command prompt string that will be generated from the current SQL Parameters
        /// </summary>
        /// <returns></returns>
        public override string GetCommandPromptString()
        {
            return SQLParameters.SetupFileLocation + " /QS " + BuildCommandLine();
        }

        /// <summary>
        /// Installs SQL 2008 SQLExpress. If the installation files (SetupFileLocation) don't exist a DatabaseUtilityException is thrown.
        /// </summary>
        public void InstallExpress()
        {
            if (!File.Exists(SQLParameters.SetupFileLocation))
            {
                throw new DatabaseUtilityException(Resources.SetupFilesDoNotExistInPathGiven + ": " + SQLParameters.SetupFileLocation);
            }

            //In both cases, we run Setup because we have the file.
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = SQLParameters.SetupFileLocation;
            myProcess.StartInfo.Arguments = "/QS " + BuildCommandLine();
            SendMessage(myProcess.StartInfo.Arguments);

            /*
             *  When installing through the command prompt, SQL Server supports full quiet mode by using the /Q parameter, 
             *  or Quiet Simple mode by using the /QS parameter. The /QS switch only shows progress, does not accept any input, 
             *  and displays no error messages if encountered. The /QS parameter is only supported when /Action=install is specified.
             * 
             * /
            /*      /qn -- Specifies that setup run with no user interface.
                    /qb -- Specifies that setup show only the basic 
                    user interface. Only dialog boxes displaying progress information are 
                    displayed. Other dialog boxes, such as the dialog box that asks users if 
                    they want to restart at the end of the setup process, are not displayed.
            */
            myProcess.StartInfo.UseShellExecute = false;

            myProcess.Start();
            myProcess.WaitForExit();
        }

        #endregion

        #region Private Functions

        private string BuildCommandLine()
        {

            /*
             * For further information on all parameters see: http://msdn.microsoft.com/en-us/library/ms144259.aspx            
             */

            StringBuilder strCommandLine = new StringBuilder();

            //Specifies a Setup work flow, like INSTALL, UNINSTALL, or UPGRADE. This is a required parameter. 
            strCommandLine.Append(" /ACTION=Install");

            //Specify a default or named instance. MSSQLSERVER is the default instance for non-Express editions and SQLExpress for Express editions. 
            //This parameter is required when installing the SQL Server Database Engine (SQL), Analysis Services (AS), or Reporting Services (RS). 
            strCommandLine.Append(" /INSTANCENAME=").Append(SQLParameters.InstanceName.ToUpper());

            //Specify the Instance ID for the SQL Server features you have specified. SQL Server directory structure, 
            //registry structure, and service names will reflect the instance ID of the SQL Server instance. 
            if (SQLParameters.InstanceID != SQLParameters.InstanceName)
            {
                strCommandLine.Append(" /INSTANCEID=\"").Append(SQLParameters.InstanceID).Append("\"");
            }

            strCommandLine.Append(" /IACCEPTSQLSERVERLICENSETERMS"); 

            //Specifies features to install, uninstall, or upgrade. The list of top-level features include SQL, AS, RS, IS, and Tools. 
            //The SQL feature will install the database engine, replication, and full-text. 
            //The Tools feature will install Management Tools, Books online, Business Intelligence Development Studio, and other shared components. 
            strCommandLine.Append(" /FEATURES=SQLEngine");

            strCommandLine.Append(SQLParameters.ErrorReporting ? " /ERRORREPORTING=1 " : " /ERRORREPORTING=0 ");

            //Startup type for the SQL Server service. 
            strCommandLine.Append(SQLParameters.AutostartSQLService ? " /SQLSVCSTARTUPTYPE=\"Automatic\"" : " /SQLSVCSTARTUPTYPE=\"Manual\"");

            if (SQLParameters.SqlCollation != "")
            {
                strCommandLine.Append(" /SQLCOLLATION=\"").Append(SQLParameters.SqlCollation).Append("\"");
            }

            if (SQLParameters.SqlSysAdminAccounts != "")
            {
                strCommandLine.Append(" /SQLSYSADMINACCOUNTS=\"").Append(SQLParameters.SqlSysAdminAccounts).Append("\"");
            }
            else
            {
                strCommandLine.Append(" /SQLSYSADMINACCOUNTS=\"BUILTIN\\Administrators\"");
            }

            // Account for SQL Server service: Domain\User or system account. 
            if (SQLParameters.SqlServiceAccountName != "")
            {
                strCommandLine.Append(" /SQLSVCACCOUNT=\"").Append(SQLParameters.SqlServiceAccountName).Append("\"");
            }
            else
            {
                strCommandLine.Append(" /SQLSVCACCOUNT=\"").Append(Properties.Resources.NetworkService).Append("\"");
            }

            if (SQLParameters.SqlServicePassword != "")
            {
                strCommandLine.Append(" /SQLSVCPASSWORD=\"").Append(SQLParameters.SqlServicePassword).Append("\"");
            }           

            //The default is Windows Authentication. Use "SQL" for Mixed Mode Authentication. 
            if (SQLParameters.SqlSecurityMode)
            {
                strCommandLine.Append(" /SECURITYMODE=\"SQL\"");
            }

            if (SQLParameters.AddCurrentUserAsSQLAdmin)
            {
                strCommandLine.Append(" /ADDCURRENTUSERASSQLADMIN=\"True\"");
            }

            if (SQLParameters.TcpEnabled)
            {
                strCommandLine.Append(" /TCPENABLED=\"1\"");
            }

            if (SQLParameters.NamedPipesEnabled)
            {
                strCommandLine.Append(" /NPENABLED=\"1\"");
            }

            //To use the SQLBROWSERAUTOSTART parameter, specify 1 to start automatically or 0 to start manually.             
            strCommandLine.Append(SQLParameters.AutostartSQLBrowserService ? " /BROWSERSVCSTARTUPTYPE=\"Automatic\"" : " /BROWSERSVCSTARTUPTYPE=\"Manual\"");

            //The user to run the SQL Browser service
            if (SQLParameters.SqlBrowserAccountName != "")
            {
                strCommandLine.Append(" /BROWSERSVRACCOUNT=\"").Append(SQLParameters.SqlBrowserAccountName).Append("\"");
            }

            //The psw for the user who is to run the SQL Browser service
            if (SQLParameters.SqlBrowserPassword != "")
            {
                strCommandLine.Append(" /BROWSERSVRPASSWORD=\"").Append(SQLParameters.SqlBrowserPassword).Append("\"");
            }

            if (SQLParameters.SqlServerAgentAccountName == "")
            {
                strCommandLine.Append(" /AGTSVCACCOUNT=\"").Append(Properties.Resources.NetworkService).Append("\"");
            }
            else
            {
                strCommandLine.Append(" /AGTSVCACCOUNT=\"").Append(SQLParameters.SqlServerAgentAccountName).Append("\"");
            }

            if (SQLParameters.SqlServerAgentPassword != "")
            {
                strCommandLine.Append(" /AGTSVCPASSWORD=\"").Append(SQLParameters.SqlServerAgentPassword).Append("\"");
            }

            if (SQLParameters.SysadminPassword != "")
            {
                strCommandLine.Append(" /SAPWD=\"").Append(SQLParameters.SysadminPassword).Append("\"");
            }

            /*if (!SQLParameters.x64BitComputer)
            {
                strCommandLine.Append(" /X86=\"True\"");
            }*/

            return strCommandLine.ToString();
        }

        #endregion
    }    
}
