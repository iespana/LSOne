using LSOne.SiteService.Plugins.IntegrationFrameworkBaseImplementation;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSRetail.SiteService.IntegrationFrameworkPriceAndDiscountInterface;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkPriceAndDiscount
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public partial class IntegrationFrameworkPriceAndDiscountPlugin : IntegrationFrameworkImplementation, IIntegrationFrameworkPriceAndDiscountService
    {
        internal static IntegrationFrameworkPriceAndDiscountPlugin Instance;

        public IntegrationFrameworkPriceAndDiscountPlugin() : this(TokenCache.Instance)
        {

        }

        public IntegrationFrameworkPriceAndDiscountPlugin(ITokenCache tokenCache)
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
            base.Load(configurations, IntegrationFrameworkConstants.IntegrationFrameworkPriceAndDiscountService, typeof(IIntegrationFrameworkPriceAndDiscountService));
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