using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSOne.Utilities.IO;
using LSRetail.SiteService.IntegrationFrameworkRetailDepartmentInterface;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailDepartment
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkRetailDepartmentPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkRetailDepartmentService
    {

        internal static IntegrationFrameworkRetailDepartmentPlugin Instance;

        public IntegrationFrameworkRetailDepartmentPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkRetailDepartmentPlugin(ITokenCache tokenCache)
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

            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkRetailDepartmentService,
                typeof (IIntegrationFrameworkRetailDepartmentService));

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