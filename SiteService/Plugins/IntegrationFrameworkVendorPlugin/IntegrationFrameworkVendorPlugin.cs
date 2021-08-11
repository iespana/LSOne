using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSRetail.SiteService.IntegrationFrameworkVendorInterface;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkVendorPlugin
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkVendorPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkVendorService
    {
        internal static IntegrationFrameworkVendorPlugin Instance;

        public IntegrationFrameworkVendorPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkVendorPlugin(ITokenCache tokenCache)
        {
            Instance = this;
            LastTicks = DateTime.UtcNow.Ticks;
            TickLock = new object();
            ExistingDatabases = new Dictionary<string, int>();
            AccessTokenCache = tokenCache;
            initPool();
        }

        public override void Load(Dictionary<string, string> configurations)
        {
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkVendorService, typeof(IIntegrationFrameworkVendorService));

        }

        public override void Unload()
        {
            Instance = null;
            base.Unload();
        }

        public override bool Exclude
        {
            get
            {
                return false;
            }
        }
    }
}
