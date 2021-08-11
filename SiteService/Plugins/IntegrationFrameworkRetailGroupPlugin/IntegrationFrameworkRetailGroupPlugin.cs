using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Helpers;
using LSOne.Utilities.IO;
using LSRetail.SiteService.IntegrationFrameworkRetailGroupInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailGroup
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkRetailGroupPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkRetailGroupService
    {
      
        internal static IntegrationFrameworkRetailGroupPlugin Instance;

        public IntegrationFrameworkRetailGroupPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkRetailGroupPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkRetailGroupService,
                typeof (IIntegrationFrameworkRetailGroupService));
              
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