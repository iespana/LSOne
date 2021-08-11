using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSRetail.SiteService.IntegrationFrameworkTransactionInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTransactionPlugin
{
  
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkTransactionPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkTransactionService
    {
        internal static IntegrationFrameworkTransactionPlugin Instance;

        public IntegrationFrameworkTransactionPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkTransactionPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkTransactionService, typeof(IIntegrationFrameworkTransactionService));
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
