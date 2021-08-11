using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// All parameters available to use when installing SQL 2008 Express server. The parameters that have to be used during the install all have default values.
    /// </summary>
    public class SQL2008Params : SQLParams
    {

        #region Properties

        /// <summary>
        /// Use this parameter to provision logins to be members of the sysadmin role. Default value is Builtin\Administrators
        /// </summary>
        public string SqlSysAdminAccounts { get; set; }

        /// <summary>
        /// Adds the current user to the SQL Server sysadmin fixed server role. Default value is true.
        /// </summary>
        public bool AddCurrentUserAsSQLAdmin { get; set; }

        /// <summary>
        /// Set to true to enable the TCP/IP protocol. Default value is false
        /// </summary>
        public bool TcpEnabled { get; set; }

        /// <summary>
        /// Set to true to enabled Named Pipes protocol. Default value is false; 
        /// </summary>
        public bool NamedPipesEnabled { get; set; }

        /// <summary>
        /// Sets the account for the SQL Server Agent service. Default value is Network Service
        /// </summary>
        public string SqlServerAgentAccountName { get; set; }

        /// <summary>
        /// Sets the password for the SQL Server Agent service user. Default value is empty
        /// </summary>
        public string SqlServerAgentPassword { get; set; }

        /// <summary>
        /// Specify the Instance ID for the SQL Server features you have specified. SQL Server directory structure, 
        /// registry structure, and service names will reflect the instance ID of the SQL Server instance. 
        /// Default value is SQlExpress
        /// </summary>
        public string InstanceID { get; set; }

        /// <summary>
        /// Set to true if the computer is a 64-bit system. Default value is false
        /// </summary>
        public bool x64BitComputer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor - all default values necessary for SQL 2008 Express server to install are set
        /// </summary>
        public SQL2008Params()
            : base()
        {
            SqlSysAdminAccounts = "";
            AddCurrentUserAsSQLAdmin = true;
            TcpEnabled = false;
            NamedPipesEnabled = false;
            SqlServerAgentAccountName = "";
            SqlServerAgentPassword = "";
            InstanceID = "MSSQLSERVER";
            x64BitComputer = false;
        }

        /// <summary>
        /// A constructor that sets the setup file location parameter in addition to all default values necessary for SQL 2008 Express server to install are set
        /// </summary>
        public SQL2008Params(string SetupFileLocation)
            : base(SetupFileLocation)
        {
            SqlSysAdminAccounts = "";
            AddCurrentUserAsSQLAdmin = true;
            TcpEnabled = false;
            NamedPipesEnabled = false;
            SqlServerAgentAccountName = "";
            SqlServerAgentPassword = "";
            InstanceID = "MSSQLSERVER";
            x64BitComputer = false;
        }

        #endregion

    }
}
