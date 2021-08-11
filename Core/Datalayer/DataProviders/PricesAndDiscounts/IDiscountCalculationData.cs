using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IDiscountCalculationData : IDataProviderBase<DiscountCalculation>
    {
        DiscountCalculation Get(IConnectionManager entry);
        bool Exists(IConnectionManager entry);
        void Save(IConnectionManager entry, DiscountCalculation discountCalculation);
    }
}