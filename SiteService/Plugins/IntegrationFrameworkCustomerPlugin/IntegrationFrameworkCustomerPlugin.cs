using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSRetail.SiteService.IntegrationFrameworkCustomerInterface;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkCustomer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkCustomerPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkCustomerService
    {
        internal static IntegrationFrameworkCustomerPlugin Instance;

        public IntegrationFrameworkCustomerPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkCustomerPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkCustomerService, typeof(IIntegrationFrameworkCustomerService));

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