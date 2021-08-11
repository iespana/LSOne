using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    partial interface ISiteService
    {
        /// <summary>
        /// Saves settings for the current terminal
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="terminal">Current terminal</param>
        /// <returns></returns>
        [OperationContract]
        void SaveTerminaldData(LogonInfo logonInfo, Terminal terminal);
    }
}
