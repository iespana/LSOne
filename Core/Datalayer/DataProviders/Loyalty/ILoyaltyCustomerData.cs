using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltyCustomerData : IDataProviderBase<LoyaltyCustomer>
    {
        /// <summary>
        /// Gets the list of loyalty customers.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyCustomersOnly">return only customers with LOYALTYCUSTOMER = 1.</param>
        /// <returns>A list of <see cref="LoyaltyCustomer"/>.</returns>
        List<LoyaltyCustomer> GetList(IConnectionManager entry, bool loyaltyCustomersOnly = true);

        LoyaltyCustomer Get(IConnectionManager entry, RecordIdentifier loyalityCustomerID);
    }
}