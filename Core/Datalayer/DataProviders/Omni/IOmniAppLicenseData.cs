using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Omni
{
    public interface IOmniAppLicenseData : IDataProviderBase<OmniLicense>
    {
        /// <summary>
        /// Retrives omni license info assinged to given terminal IDa and omni app ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID">The terminal you want to get omni license information for</param>
        /// <param name="appID">ID of the omni app</param>
        /// <returns>Omni license information for the given terminal and omni application</returns>
        OmniLicense Get(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier appID = null);

        /// <summary>
        /// Retrives all omni licenses for store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">ID of the store. This parameter should be null if you are on head office level</param>
        /// <returns>List of omni licenses</returns>
        List<OmniLicense> GetOmniLicenses(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Saves a single omni license into the databace
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="omniLicense">The omni license that is going to be save</param>
        void Save(IConnectionManager entry, OmniLicense omniLicense);

        /// <summary>
        /// Checks if a license key already exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="licenseKey">The license key you want to check if already exists in the databace</param>
        /// <returns>True if the key already exists in the omni license table, false otherwise</returns>
        bool LicenseKeyRecordExists(IConnectionManager entry, RecordIdentifier licenseKey);

        /// <summary>
        /// Deletes a single omni license from the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="licenseKey">The license key of the license you want to delete from the database</param>
        void DeleteLicense(IConnectionManager entry, RecordIdentifier licenseKey);
    }
}