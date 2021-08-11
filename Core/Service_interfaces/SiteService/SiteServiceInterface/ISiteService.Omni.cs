using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Retrives omni license info assinged to given terminal IDa and omni app ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="terminalID">The terminal you want to get omni license information for</param>
        /// <param name="appID">ID of the omni app</param>
        /// <returns>Omni license information for the given terminal and omni application</returns>
        [OperationContract]
        OmniLicense GetOmniLicense(LogonInfo logonInfo, RecordIdentifier terminalID, RecordIdentifier appID = null);

        /// <summary>
        /// Retrives all omni licenses for store
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID">ID of the store. This parameter should be null if you are on head office level</param>
        /// <returns>List of omni licenses</returns>
        [OperationContract]
        List<OmniLicense> GetOmniLicenses(LogonInfo logonInfo, RecordIdentifier storeID = null);

        /// <summary>
        /// Saves a single omni license into the databace
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="omniLicense">The omni license that is going to be save</param>
        [OperationContract]
        void SaveOmniLicenses(LogonInfo logonInfo, OmniLicense omniLicense);

        /// <summary>
        /// Checks if a license key already exists in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="licenseKey">The license key you want to check if already exists in the databace</param>
        /// <returns>True if the key already exists in the omni license table, false otherwise</returns>
        [OperationContract]
        bool OmniLicenseKeyRecordExists(LogonInfo logonInfo, RecordIdentifier licenseKey);

        /// <summary>
        /// Deletes a single omni license from the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="licenseKey">The license key of the license you want to delete from the database</param>
        [OperationContract]
        void DeleteOmniLicense(LogonInfo logonInfo, RecordIdentifier licenseKey);
    }
}
