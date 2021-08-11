using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSRetail.SiteService.KDSLSOneWebServiceInterface;
using LSRetail.SiteService.SiteServiceInterface;
using System;
using System.Collections.Generic;
using System.Security;
using System.ServiceModel;


namespace LSOne.SiteService.Plugins.KDSLSOneWebServicePlugin
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, AddressFilterMode =AddressFilterMode.Any)]
    public partial class KDSLSOneWebServicePlugin : IKDSLSOneWebService, IConfigurationWriterPlugin
    {
        protected ServiceHostBase WcfHost;
        protected int httpPort;

        public bool Exclude => false;
        public string ConfigurationKey => "KDSLSOneWebService";
        public FolderItem CustomConfigFile => ConfigurationFile.ConfigPath.Child("KDSLSOneWebService.config");
        public string ConfigurationName => "KDSLSOneWebService";

        protected ConnectionPoolManager ConnectionPool;
        protected Dictionary<string, string> Configurations;
        protected static Tuple<
         string, // Database server
         bool, // Database windows authentication
         string, // Database user
         SecureString, // Database password
         string, // Database name
         string, // Site Manager user
         SecureString, // Site manager password
         Tuple<ConnectionType, ConnectionUsageType, string>> Parameters;

        public KDSLSOneWebServicePlugin()
        {
            initPool();
        }

        protected void initPool()
        {
            ConnectionPool = new ConnectionPoolManager();
        }

        protected IConnectionManager GetConnectionManager()
        {
            string database;
            IConnectionManager dataModel;

            try
            {
                database = Parameters.Item5;

                dataModel = ConnectionPool.GetConnection(
                    Parameters.Item1, //server
                    Parameters.Item2, //windows authentication
                    Parameters.Item3, //server login
                    Parameters.Item4, //server password
                    Parameters.Item5, //database name
                    new Guid(), //login
                    Parameters.Rest.Item1, //connection type
                    ConnectionUsageType.UsageNormalClient,
                    Parameters.Rest.Item3);

                if (dataModel == null)
                {
                    throw new Exception("Authentication Failed. Incorrect credentials");
                }

                dataModel.IgnoreColumnOptimizer = false;

                return dataModel;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw;
            }
        }

        protected void ReturnConnection(IConnectionManager entry, out IConnectionManager externalEntry)
        {
            ConnectionPool.ReturnConnection(entry, Conversion.ToInt(Configurations[SiteServiceConfigurationConstants.MaxCount]), Conversion.ToInt(Configurations[SiteServiceConfigurationConstants.MaxUserConnectionCount]));
            externalEntry = null;
        }

        protected virtual void ResolveConfiguration(Dictionary<string, string> configurations)
        {
            ConnectionType connectionType = ResolveConnectionTypeFromName(configurations[SiteServiceConfigurationConstants.DatabaseConnectionType]);

            try
            {
                Parameters = new Tuple<string, bool, string, SecureString, string, string, SecureString, Tuple<ConnectionType, ConnectionUsageType, string>>(
                    configurations[SiteServiceConfigurationConstants.DatabaseServer],
                    configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                    configurations[SiteServiceConfigurationConstants.DatabaseUser],
                    SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.DatabasePassword]),
                    configurations[SiteServiceConfigurationConstants.DatabaseName],
                    configurations[SiteServiceConfigurationConstants.SiteManagerUser],
                    SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.SiteManagerPassword]),
                    new Tuple<ConnectionType, ConnectionUsageType, string>(
                        connectionType,
                        ConnectionUsageType.UsageService,
                        configurations[SiteServiceConfigurationConstants.DataAreaID]));
            }
            catch (Exception)
            {
                Parameters = new Tuple<string, bool, string, SecureString, string, string, SecureString, Tuple<ConnectionType, ConnectionUsageType, string>>(
                    configurations[SiteServiceConfigurationConstants.DatabaseServer],
                    configurations[SiteServiceConfigurationConstants.DatabaseWindowsAuthentication].Equals("true", StringComparison.InvariantCultureIgnoreCase),
                    configurations[SiteServiceConfigurationConstants.DatabaseUser],
                    SecureStringHelper.FromString(configurations[SiteServiceConfigurationConstants.DatabasePassword]),
                    configurations[SiteServiceConfigurationConstants.DatabaseName],
                    configurations["StoreControllerUser"],
                    SecureStringHelper.FromString(configurations["StoreControllerPassword"]),
                    new Tuple<ConnectionType, ConnectionUsageType, string>(
                        connectionType,
                        ConnectionUsageType.UsageService,
                        configurations[SiteServiceConfigurationConstants.DataAreaID]));
            }
        }

        protected ConnectionType ResolveConnectionTypeFromName(string connectionTypeName)
        {
            ConnectionType connectionType;

            if (connectionTypeName == "TCP/IP")
            {
                connectionType = ConnectionType.TCP_IP;
            }
            else if (connectionTypeName == "Named pipes")
            {
                connectionType = ConnectionType.NamedPipes;
            }
            else
            {
                connectionType = ConnectionType.SharedMemory;
            }

            return connectionType;
        }

        protected void ThrowChannelError(Exception e)
        {
            throw new FaultException("9" + e);
        }

        public void Load(Dictionary<string, string> configurations)
        {
            Configurations = configurations;

            if (WcfHost != null)
            {
                WcfHost.Close();
            }

            ResolveConfiguration(configurations);

            httpPort = Convert.ToInt32(configurations[KDSLSOneWebServiceConstants.KDSHttpPort]);            

            WcfHost = KDSWcfServiceBuilder.CreateKDSHost(this, configurations, KDSLSOneWebServiceConstants.EndpointName, typeof(IKDSLSOneWebService), Environment.MachineName);

            WcfHost.Open();
        }

        public void OnNotifyPlugin(object sender, LSOne.DataLayer.BusinessObjects.IntegrationFramework.MessageEventArgs e)
        {
        }        

        public void ReloadConfigurations(Dictionary<string, string> configurations)
        {
            if (WcfHost != null)
            {
                Unload();
            }
            Load(configurations);
        }

        public void Unload()
        {
            if (WcfHost != null)
            {
                /// Per official documentation: 
                /// - an object in the Faulted state is not closed and may be holding resources. 
                /// - The Abort method should be used to close an object that has faulted. 
                /// - If Close is called on an object in the Faulted state, 
                ///a CommunicationObjectFaultedException is thrown because the object cannot be gracefully closed.
                if (WcfHost.State == CommunicationState.Faulted)
                {
                    WcfHost.Abort();
                }
                else
                {
                    WcfHost.Close();
                }

                WcfHost = null;
            }
        }

        public bool VerifyConfigurations(Dictionary<string, string> configurations)
        {
            bool dirty = false;

            dirty |= VerifyEntry(configurations, KDSLSOneWebServiceConstants.KDSHttpPort, "9110");
            dirty |= VerifyEntry(configurations, KDSLSOneWebServiceConstants.KDSCertificateStoreLocation, "LocalMachine");
            dirty |= VerifyEntry(configurations, KDSLSOneWebServiceConstants.KDSCertificateStoreName, "My");
            dirty |= VerifyEntry(configurations, KDSLSOneWebServiceConstants.KDSCertificateThumbnail, "");

            return dirty;
        }

        protected static bool VerifyEntry(Dictionary<string, string> configurations, string entry, string defaultValue)
        {            
            if (!configurations.ContainsKey(entry))
            {
                configurations.Add(entry, defaultValue);
                return true;
            }

            // Nothing added
            return false;
        }

        public void WriteConfigurations(Dictionary<string, string> configurations, ISiteServiceSettings settings)
        {
            settings.WriteComment("KDS LS One Web Service configuration");
            settings.WriteKey(KDSLSOneWebServiceConstants.KDSHttpPort, configurations[KDSLSOneWebServiceConstants.KDSHttpPort]);
            settings.WriteKey(KDSLSOneWebServiceConstants.KDSCertificateStoreLocation, configurations[KDSLSOneWebServiceConstants.KDSCertificateStoreLocation]);
            settings.WriteKey(KDSLSOneWebServiceConstants.KDSCertificateStoreName, configurations[KDSLSOneWebServiceConstants.KDSCertificateStoreName]);
            settings.WriteKey(KDSLSOneWebServiceConstants.KDSCertificateThumbnail, configurations[KDSLSOneWebServiceConstants.KDSCertificateThumbnail]);
        }        
    }
}