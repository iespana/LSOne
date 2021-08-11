using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        /// <summary>
        /// Returns the ID of the default tax store configured at the central database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual RecordIdentifier GetDefaultTaxStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            RecordIdentifier result = RecordIdentifier.Empty;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetDefaultTaxStoreID(CreateLogonInfo(entry)), closeConnection);

            return result;
        }
    }
}
