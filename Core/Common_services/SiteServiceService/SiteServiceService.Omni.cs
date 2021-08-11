using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public OmniLicense GetOmniLicense(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, bool closeConnection, RecordIdentifier appID = null)
        {
            OmniLicense result = new OmniLicense();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOmniLicense(CreateLogonInfo(entry), terminalID, appID), closeConnection);

            return result;
        }

        public List<OmniLicense> GetOmniLicenses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection, RecordIdentifier storeID = null)
        {
            List<OmniLicense> result = new List<OmniLicense>();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOmniLicenses(CreateLogonInfo(entry), storeID), closeConnection);

            return result;
        }

        public void SaveOmniLicenses(IConnectionManager entry, SiteServiceProfile siteServiceProfile, OmniLicense omniLicense, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveOmniLicenses(CreateLogonInfo(entry), omniLicense), closeConnection);
        }

        public bool OmniLicenseKeyRecordExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier licenseKey, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.OmniLicenseKeyRecordExists(CreateLogonInfo(entry), licenseKey), closeConnection);

            return result;
        }

        public void DeleteOmniLicense(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier licenseKey, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteOmniLicense(CreateLogonInfo(entry), licenseKey), closeConnection);
        }
    }
}
