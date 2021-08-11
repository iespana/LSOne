using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// An abstract class that specific version sql params inherit from. Holds all the parameters which are the same in both installations
    /// </summary>
    public abstract class SQLParams
    {
        #region Public variables        

        /// <summary>
        /// The SQL Express server instance name created. Default is MSSQLSERVER
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// The file path to the SQL 2005 Express installation files.
        /// </summary>
        public string SetupFileLocation { get; set; }

        /// <summary>
        /// Tells the silent install if the SQL server being install should be automatically started on startup or if it should be manually started.
        /// Default value is true
        /// </summary>
        public bool AutostartSQLService { get; set; }

        /// <summary>
        /// Tells the silent install if the SQL browser being install should be automatically started on startup or if it should be manually started.
        /// Default value is true
        /// </summary>
        public bool AutostartSQLBrowserService { get; set; }

     
        /// <summary>
        /// The user running the SQL Express service. Empty string means LocalSystem will run the service
        /// </summary>
        public string SqlServiceAccountName { get; set; }

        /// <summary>
        /// If a user is specified to run the SQL Express service the password needs to be specified as well
        /// </summary>
        public string SqlServicePassword { get; set; }

        /// <summary>
        /// If true the SQL Express server is installed to have a Mixed Mode Authentication. If false only Windows Authentication will be allowed
        /// Default is true
        /// </summary>
        public bool SqlSecurityMode { get; set; }

        /// <summary>
        /// The password for the sa user. Has to be a strong password. Default is D0nt.F0rg3t
        /// </summary>
        public string SysadminPassword { get; set; }

        /// <summary>
        /// Changes the default settings for the SQL server collation. If SqlCollation is left empty then the default collation which
        /// corralates to the local regional settings on the computer will be chosen.
        /// </summary>
        public string SqlCollation { get; set; }        

        /// <summary>
        /// If true the SQL Server (and its components) and Analysis Services are configured to automatically send a report to Microsoft if a fatal error occurs in the SQL Server Database Engine, SQL Server Agent, or Analysis Services.
        /// Default value is true.
        /// </summary>
        public bool ErrorReporting { get; set; }

        #endregion    

        #region Constructor

        /// <summary>
        /// Default constructor - sets all parameters that are necessary in both SQL 2005/2008/2012 Express installations
        /// </summary>
        public SQLParams()
        {
            InstanceName = "MSSQLSERVER";
            AutostartSQLService = true;
            AutostartSQLBrowserService = true;
            SetupFileLocation = "";
            SqlSecurityMode = true;
            SysadminPassword = "D0nt.F0rg3t";            
            ErrorReporting = true;
            SqlServiceAccountName = "";
            SqlServicePassword = "";
            SqlCollation = "";            
            SetupFileLocation = "";
        }

        /// <summary>
        /// Sets all the default parameters in addition it sets the setup file location
        /// </summary>
        /// <param name="SetupFileLocation">The location of the setup file</param>
        public SQLParams(string SetupFileLocation)
            : this()
        {
            this.SetupFileLocation = SetupFileLocation;
        }

        #endregion

    }
}
