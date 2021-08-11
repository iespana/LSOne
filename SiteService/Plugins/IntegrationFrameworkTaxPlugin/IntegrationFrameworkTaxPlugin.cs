using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSRetail.SiteService.IntegrationFrameworkTaxInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTax
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkTaxPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkTaxService
    {
      
        internal static IntegrationFrameworkTaxPlugin Instance;

        public IntegrationFrameworkTaxPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkTaxPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkTaxService,
                typeof (IIntegrationFrameworkTaxService));
              
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

        private RecordIdentifier DefaultStoreTaxGroupID { get { return "IFTG0001"; } }
        private string DefaultStoreTaxGroupName { get { return "Default Tax"; } }
    }
}