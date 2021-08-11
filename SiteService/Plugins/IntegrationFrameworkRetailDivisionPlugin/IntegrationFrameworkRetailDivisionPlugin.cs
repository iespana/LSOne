﻿using LSOne.Services.Interfaces.Constants;
using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using LSRetail.SiteService.IntegrationFrameworkRetailDivisionInterface;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailDivisionPlugin
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkRetailDivisionPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkRetailDivisionService
    {
        internal static IntegrationFrameworkRetailDivisionPlugin Instance;

        public IntegrationFrameworkRetailDivisionPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkRetailDivisionPlugin(ITokenCache tokenCache)
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

            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkRetailDivisionService,
                typeof(IIntegrationFrameworkRetailDivisionService));

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
