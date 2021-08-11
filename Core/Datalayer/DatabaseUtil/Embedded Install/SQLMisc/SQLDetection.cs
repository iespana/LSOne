using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data;
using LSOne.DataLayer.DatabaseUtil.Exceptions;
using Microsoft.Win32;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// 
    /// Detects any and all SQL Server instances on the network
    /// 
    /// </summary>
    public class SQLDetection
    {
        /// <summary>
        /// Enumerates all SQL Server instances on the network and returns them in a LinkedList
        /// </summary>
        /// <returns></returns>
        public List<SQLExpressInfo> EnumerateSQLInstances()
        {
            return EnumerateSQLInstances("", "");            
        }

        private List<SQLExpressInfo> EnumerateSQLInstances(string serverName, string instanceName)
        {
            string expression = "";
            if (serverName == "" && instanceName == "")
            {
                expression = "";
            }
            else if (serverName != "" && instanceName == "")
            {
                expression = "ServerName = '" + serverName + "' AND InstanceName = ''";
            }
            else if (serverName != "" && instanceName != "")
            {
                expression = "ServerName = '" + serverName + "' AND InstanceName = '" + instanceName + "'";
            }

            var tblSqlServers = SqlDataSourceEnumerator.Instance.GetDataSources();
            var rows = tblSqlServers.Select(expression, "ServerName", DataViewRowState.CurrentRows);

            var sqlExprInfoList = new List<SQLExpressInfo>();
            foreach (var row in rows)
            {
                var exprInfo = new SQLExpressInfo
                    {
                        InstanceName = (string) row["InstanceName"],
                        ServerName = (string) row["ServerName"]
                    };
                sqlExprInfoList.Add(exprInfo);
            }

            return sqlExprInfoList;
        }

        /// <summary>
        /// Returns true if a server name and instance name can be found. Does NOT work for LOCALHOST
        /// </summary>
        /// <param name="serverName">Server name to search for</param>
        /// <param name="instanceName">Instance name to search for</param>
        /// <returns>Returns true if SQL Server is found</returns>
        public bool SQLInstanceExists(string serverName, string instanceName)
        {
            var sqlExprInfoList = EnumerateSQLInstances(serverName, instanceName);
            return (sqlExprInfoList.Count == 1);            
        }

        private static bool SQLServerInstalled(string subKey)
        {
            bool found32BitVersion;

            // Since the POS is running as x86, the call to OpenSubKey will be directed to the 32 bit version of the registry even if you are running on 
            // a 64 bit machine. This is equivalent to looking at the Wow64Node keys in the registry on a 64 bit machine.
            using (var sqlServerKey = Registry.LocalMachine.OpenSubKey(subKey, false))
            {
                found32BitVersion = sqlServerKey != null;
            }

            if (found32BitVersion)
                return true;

            // Look for the 64 bit version
            bool found64BitVersion;
            using (var sqlServerKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).
                OpenSubKey(subKey, false))
            {
                found64BitVersion = sqlServerKey != null;
            }

            return found64BitVersion;
        }

        /// <summary>
        /// Searches for Microsoft SQL Server 2008 R2 for both 32 and 64 bit machines
        /// </summary>
        /// <returns></returns>
        public bool SQLServer2008R2Installed()
        {
            return SQLServerInstalled("Software\\Microsoft\\Microsoft SQL Server\\100\\10.50\\");
        }

        /// <summary>
        /// Searches for Microsoft SQL Server 2012 for both 32 and 64 bit machines
        /// </summary>
        /// <returns></returns>
        public bool SQLServer2012Installed()
        {
            return SQLServerInstalled("Software\\Microsoft\\Microsoft SQL Server\\110\\11.0\\");
        }

        /// <summary>
        /// Searches for Microsoft SQL Server 2014 for both 32 and 64 bit machines
        /// </summary>
        /// <returns></returns>
        public bool SQLServer2014Installed()
        {
            return SQLServerInstalled("Software\\Microsoft\\Microsoft SQL Server\\120\\12.0\\");
        }

        /// <summary>
        /// Searches for Microsoft SQL Server 2016 for both 32 and 64 bit machines
        /// </summary>
        /// <returns></returns>
        public bool SQLServer2016Installed()
        {
            return SQLServerInstalled("Software\\Microsoft\\Microsoft SQL Server\\130\\13.0\\");
        }

        /// <summary>
        /// Searches for Microsoft SQL Server 2017 for both 32 and 64 bit machines
        /// </summary>
        /// <returns></returns>
        public bool SQLServer2017Installed()
        {
            return SQLServerInstalled("Software\\Microsoft\\Microsoft SQL Server\\140\\14.0\\");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlExpress"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseUtilityException"></exception>
        public bool IsLocalHostInstalled(bool sqlExpress)
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Microsoft SQL Server\\", false))
                {
                    if (key == null) 
                        return false;
                    var strNames = key.GetSubKeyNames();

                    //If we cannot find a SQL Server registry key, we don't have SQL Server Express installed
                    if (strNames.Length == 0) return false;

                    foreach (string s in strNames)
                    {
                        if (!s.StartsWith("MSSQL")) 
                            continue;

                        //Check to see if the edition is "Express Edition"
                        using (var keyEdition = key.OpenSubKey(s + "\\Setup\\", false))
                        {
                            if (keyEdition == null) 
                                continue;

                            if (sqlExpress && (string)keyEdition.GetValue("Edition") == "Express Edition")
                            {
                                //If there is at least one instance of SQL Server Express installed, return true
                                return true;
                            }
                                
                            if (!sqlExpress && (string)keyEdition.GetValue("Edition") == "Standard Edition")
                            {
                                //If there is at least one instance of SQL Server installed, return true
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new DatabaseUtilityException(Properties.Resources.ErrorAccessingRegistryKeys, ex);
            }
        }
    }
}
