using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IDiscountAndPriceActivationData : IDataProviderBase<DiscountAndPriceActivation>
    {
        DiscountAndPriceActivation Get(IConnectionManager entry, CacheType cacheType = CacheType.CacheTypeNone);
        bool Exists(IConnectionManager entry);
        void Save(IConnectionManager entry, DiscountAndPriceActivation discountAndPriceActivation);
    }
}