using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// A class that will return the SQL Server name. Used by functions such as EnumerateSQLInstances
    /// </summary>
    public class SQLExpressInfo
    {

        #region Properties
                
        /// <summary>
        /// The SQL Server instance name i.e. SQLEXPRESS
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// The SQL Server name
        /// </summary>
        public string ServerName { get; set; }
       

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SQLExpressInfo()
        {
            InstanceName = "";
            ServerName = "";        
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Returns the SQL Server name in a concatenated string if necessary
        /// </summary>
        /// <returns>Returns the SQL Server name in a concatenated string if necessary</returns>
        public override string ToString()
        {
            if (InstanceName == "" && ServerName != "")
            {
                return ServerName;
            }
            else if (ServerName == "" && InstanceName != "")
            {
                return InstanceName;
            }
            else
            {
                return ServerName + @"\" + InstanceName;
            }
        }

        #endregion

    }
}
