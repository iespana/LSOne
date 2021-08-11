using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual OmniLicense GetOmniLicense(LogonInfo logonInfo, RecordIdentifier terminalID, RecordIdentifier appID = null)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(terminalID)}: {terminalID}, {nameof(appID)}: {appID}");

                return Providers.OmniAppLicenseData.Get(dataModel, terminalID, appID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<OmniLicense> GetOmniLicenses(LogonInfo logonInfo, RecordIdentifier storeID = null)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                return Providers.OmniAppLicenseData.GetOmniLicenses(dataModel, storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void SaveOmniLicenses(LogonInfo logonInfo, OmniLicense omniLicense)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.OmniAppLicenseData.Save(dataModel, omniLicense);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);

            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual bool OmniLicenseKeyRecordExists(LogonInfo logonInfo, RecordIdentifier licenseKey)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(licenseKey)}: {licenseKey}");

                return Providers.OmniAppLicenseData.LicenseKeyRecordExists(dataModel, licenseKey);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void DeleteOmniLicense(LogonInfo logonInfo, RecordIdentifier licenseKey)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(licenseKey)}: {licenseKey}");

                Providers.OmniAppLicenseData.DeleteLicense(dataModel, licenseKey);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}