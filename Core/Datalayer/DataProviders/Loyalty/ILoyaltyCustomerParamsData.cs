using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltyCustomerParamsData : IDataProviderBase<LoyaltyCustomerParams>
    {
        LoyaltyCustomerParams Get(IConnectionManager entry, RecordIdentifier Key = null);
        void Save(IConnectionManager entry, LoyaltyCustomerParams loyaltyCustomerParams);
    }
}