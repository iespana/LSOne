using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Helpers;
using LSOne.Utilities.IO;
using LSRetail.SiteService.IntegrationFrameworkCurrencyInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkCurrency
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkCurrencyPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkCurrencyService
    {
      
        internal static IntegrationFrameworkCurrencyPlugin Instance;

        public IntegrationFrameworkCurrencyPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkCurrencyPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkCurrencyService,
                typeof (IIntegrationFrameworkCurrencyService));
              
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