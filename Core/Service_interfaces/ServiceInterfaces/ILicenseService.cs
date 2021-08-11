using System;
using System.Security;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.IO;
using LSRetail.Licensing.Common;

namespace LSOne.Services.Interfaces
{
    public enum LicenseType
    {
        Perpetual = 0,
        Demo = 1,
        Subscription = 2,
        Partner = 3,
        Basic = 4,
        None = 10
    }

    public enum LicenseErrorDisplayType
    {
        NoError = 0,
        InvalidType = 1,
        Expired = 2,
        LicenseExpiresIn = 3,
        NoLicense = 4,
        GracePeriodExceeded = 5,
        CommunicationError = 6
    }

    public enum Product
    {
        POS = 1,
        NAVPOS = 2,
        SiteManager = 4
    }

    public interface ILicenseService : IService
    {
        Product ProductID { get; set; }
        LicenseType LicenseType { get; }

        LicenseErrorDisplayType ErrorDisplayType { get; }
        DateTime? LicenseExpiryDate { get; }
        string ErrorMessage { get; }
        string LicenseTypeName { get; }
        string Version { get; }
        string SiteServiceHost { get; }
        int SiteServicePort { get; }
        string Challenge(Guid guid);
        bool Initialize(FolderItem location, SQLServerLoginEntry loginEntry);

        bool Initialize(FolderItem location);
        List<string> GetDatabasesByHost(string host);
        bool ShowLicenseDialog();
        void SetLicense(ILicenseValidator validator);
        void PeriodicCheck(Form parent);
        bool GetSiteService(string login, string password, out string sshost, out int ssport);
        bool GetHBOLogin(string login, string password, out string dbServer, out int ssport, out string dbName, out string dbUser, out string dbPassword);
        void CheckDatabaseForLicense(string dataSource,
                                     bool windowsAuthentication,
                                     string sqlServerLogin,
                                     SecureString sqlServerPassword,
                                     string databaseName,
                                     ConnectionType connectionType,
                                     string dataAreaID,
                                     string licenseCode);

        void SaveIntegrationFrameworkLicense(IConnectionManager entry);

        DateTime ValidateIntegrationFrameworkLicense(IConnectionManager entry);

        /// <summary>
        /// Validate a product license
        /// </summary>
        /// <param name="encodedFile">The encoded license</param>
        /// <param name="isDeactivating"></param>
        /// <param name="loginEntry">The database connection</param>
        void Validate(string encodedFile = null, bool isDeactivating = false, SQLServerLoginEntry loginEntry = null);

        /// <summary>
        /// Validates a license for the given plugin and version information
        /// </summary>
        /// <param name="pluginID">The ID of the plugin</param>
        /// <param name="pluginHostVersionInfo">Contains info from the plugin about which versions of the Site Manager the plugin is compatible with</param>
        /// <returns>A tuple that contains a boolean flag indicating whether the plugin was valid for a license or not, message containing a description of the results</returns>
        (bool Validated, string ValidationMessage) ValidatePluginLicense(string pluginID, PluginHostVersion pluginHostVersionInfo);
    }
}
