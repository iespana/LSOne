using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using LSOne.DataLayer.DatabaseUtil.Exceptions;
using LSOne.DataLayer.DatabaseUtil.Properties;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// A class that installs SQL 2005 Express server. It builds the command line from parameters defined in SQL2005Params
    /// </summary>
    public class InstallSQL2005Expr : InstallSQLExpress
    {
        /// <summary>
        /// When using the InstallSQL2005Expr the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the class. 
        /// This is done instead of having a user interface with progress information.
        /// </summary>
        public event InstallingSQL2005MessageCallback MessageCallbackHandler;

        /// <summary>
        /// The parameters required to install the SQL 2005 Express server quietly. Default parameters can be changed before installation.
        /// </summary>
        public SQL2005Params SQLParameters { get; set; }

        #region Constructor

        /// <summary>
        /// Initialises the class and sets the parameters
        /// </summary>
        /// <param name="SQLParameters">The SQL 2005 parameters that should be used when installing the SQL Server</param>
        public InstallSQL2005Expr(SQL2005Params SQLParameters) : base()
        {
            this.SQLParameters = SQLParameters;
        }        

        #endregion

        #region Overridden Functions
       
        /// <summary>
        /// Sends a message through the message callback function
        /// </summary>
        /// <param name="msg">The message to send</param>
        public override void SendMessage(string msg)
        {
            if (MessageCallbackHandler != null)
            {
                MessageCallbackHandler(Resources.SenderInstallSQL2005, msg);
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

        /// <summary>
        /// Creates the command prompt string that will be generated from the current SQL Parameters
        /// </summary>
        /// <returns></returns>
        public override string GetCommandPromptString()
        {
            return SQLParameters.SetupFileLocation + "/qb " + BuildCommandLine();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Installs SQL 2005 SQLExpress. If the installation files (SetupFileLocation) don't exist a DatabaseUtilityException is thrown.
        /// </summary>
        public void InstallExpress()
        {
            try
            {
                if (!File.Exists(SQLParameters.SetupFileLocation))
                {
                    throw new DatabaseUtilityException(Resources.SetupFilesDoNotExistInPathGiven);
                }
                                
                //In both cases, we run Setup because we have the file.
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = SQLParameters.SetupFileLocation;
                myProcess.StartInfo.Arguments = "/qb " + BuildCommandLine();
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
            catch (DatabaseUtilityException dbEx)
            {
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Private functions

        private string BuildCommandLine()
        {
            StringBuilder strCommandLine = new StringBuilder();            

            //ADDLOCAL specifies which components to install. If ADDLOCAL is not specified, Setup fails.
            //To install all components specify 'ADDLOCAL=All' from the command prompt.
            if (!string.IsNullOrEmpty(SQLParameters.AddLocal))
            {
                strCommandLine.Append(" ADDLOCAL=\"").Append(SQLParameters.AddLocal).Append("\"");
            }

            //To use the AUTOSTART parameter, specify 1 to start automatically or 0 to start manually. 
            //If you decide to start SQL Server Agent automatically, then SQL is also started automatically
            if (SQLParameters.AutostartSQLService)
            {
                strCommandLine.Append(" SQLAUTOSTART=1");
            }
            else
            {
                strCommandLine.Append(" SQLAUTOSTART=0");
            }

            //To use the SQLBROWSERAUTOSTART parameter, specify 1 to start automatically or 0 to start manually.             
            if (SQLParameters.AutostartSQLBrowserService)
            {
                strCommandLine.Append(" SQLBROWSERAUTOSTART=1");
            }
            else
            {
                strCommandLine.Append(" SQLBROWSERAUTOSTART=0");
            }

            //The user to run the SQL Browser service
            if (!string.IsNullOrEmpty(SQLParameters.SqlBrowserAccountName))
            {
                strCommandLine.Append(" SQLBROWSERACCOUNT=\"").Append(SQLParameters.SqlBrowserAccountName).Append("\"");
            }

            //The psw for the user who is to run the SQL Browser service
            if (!string.IsNullOrEmpty(SQLParameters.SqlBrowserPassword))
            {
                strCommandLine.Append(" SQLBROWSERPASSWORD=\"").Append(SQLParameters.SqlBrowserPassword).Append("\"");
            }

            if (!string.IsNullOrEmpty(SQLParameters.SqlServiceAccountName))
            {
                strCommandLine.Append(" SQLACCOUNT=\"").Append(SQLParameters.SqlServiceAccountName).Append("\"");
            }

            if (!string.IsNullOrEmpty(SQLParameters.SqlServicePassword))
            {
                strCommandLine.Append(" SQLPASSWORD=\"").Append(SQLParameters.SqlServicePassword).Append("\"");
            }

            if (SQLParameters.SqlSecurityMode == true)
            {
                strCommandLine.Append(" SECURITYMODE=SQL");
            }

            if (!string.IsNullOrEmpty(SQLParameters.SysadminPassword))
            {
                strCommandLine.Append(" SAPWD=\"").Append(SQLParameters.SysadminPassword).Append("\"");
            }

            if (!string.IsNullOrEmpty(SQLParameters.SqlCollation))
            {
                strCommandLine.Append(" SQLCOLLATION=\"").Append(SQLParameters.SqlCollation).Append("\"");
            }

            strCommandLine.Append(" DISABLENETWORKPROTOCOLS=" + SQLParameters.DisableNetworkProtocols.ToString());

            if (SQLParameters.ErrorReporting == true)
            {
                strCommandLine.Append(" ERRORREPORTING=1");
            }
            else
            {
                strCommandLine.Append(" ERRORREPORTING=0");
            }

            return strCommandLine.ToString();
        }

        #endregion

    }
}
