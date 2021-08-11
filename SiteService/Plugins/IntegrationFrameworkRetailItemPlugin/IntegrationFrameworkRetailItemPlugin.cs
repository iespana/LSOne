using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSOne.Utilities.IO;
using LSRetail.SiteService.IntegrationFrameworkRetailItemInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailItem
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkRetailItemPlugin : IntegrationFrameworkImplementation,
        IIntegrationFrameworkRetailItemService
    {

        internal static IntegrationFrameworkRetailItemPlugin Instance;

        public IntegrationFrameworkRetailItemPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkRetailItemPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkRetailItemService,
                typeof (IIntegrationFrameworkRetailItemService));
        }

        public override void Unload()
        {
            Instance = null;
            base.Unload();

        }

        public override bool Exclude
        {
            get { return false; }
        }
    }
}