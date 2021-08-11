using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        /// <summary>
        /// Retrives omni license info assinged to given terminal IDa and omni app ID
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="terminalID">The terminal you want to get omni license information for</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="appID">ID of the omni app</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Omni license information for the given terminal and omni application</returns>
        OmniLicense GetOmniLicense(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, bool closeConnection, RecordIdentifier appID = null);

        /// <summary>
        /// Retrives all omni licenses for store
        /// </summary>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="storeID">ID of the store. This parameter should be null if you are on head office level</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of omni licenses</returns>
        List<OmniLicense> GetOmniLicenses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection, RecordIdentifier storeID = null);

        /// <summary>
        /// Saves a single omni license into the databace
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="omniLicense">The omni license that is going to be save</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void SaveOmniLicenses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, OmniLicense omniLicense, bool closeConnection);

        /// <summary>
        /// Checks if a license key already exists in the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="licenseKey">The license key you want to check if already exists in the databace</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the key already exists in the omni license table, false otherwise</returns>
        bool OmniLicenseKeyRecordExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier licenseKey, bool closeConnection);

        /// <summary>
        /// Deletes a single omni license from the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="licenseKey">The license key of the license you want to delete from the database</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteOmniLicense(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier licenseKey, bool closeConnection);
    }
}
