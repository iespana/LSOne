using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        /// <summary>
        /// Saves settings for the current terminal
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminal">Current terminal</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual void SaveTerminaldData(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Terminal terminal, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveTerminaldData(CreateLogonInfo(entry), terminal), closeConnection);
        }
    }
}
