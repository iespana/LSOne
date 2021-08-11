using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    partial interface ISiteService
    {
        /// <summary>
        /// Returns the ID of the default tax store configured at the central database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        RecordIdentifier GetDefaultTaxStoreID(LogonInfo logonInfo);
    }
}
