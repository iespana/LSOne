using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Utilities.IO;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSRetail.SiteService.IntegrationFrameworkStatementsInterface;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkStatementsPlugin
{
  
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkStatementsPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkStatementsService
    {
        internal static IntegrationFrameworkStatementsPlugin Instance;

        public IntegrationFrameworkStatementsPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkStatementsPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkStatementsService, typeof(IIntegrationFrameworkStatementsService));
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
