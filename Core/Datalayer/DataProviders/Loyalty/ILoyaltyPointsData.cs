using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltyPointsData : IDataProvider<LoyaltyPoints>
    {
        /// <summary>
        /// Gets a list of rules for a given scheme.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeID">The scheme ID.</param>
        /// <returns>A list of instances of <see cref="LoyaltyPoints"/></returns>
        List<LoyaltyPoints> GetList(IConnectionManager entry, RecordIdentifier schemeID);

        /// <summary>
        /// returns exchange rate - how much money for one loyalty point
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeID">The scheme ID</param>
        /// <returns></returns>
        LoyaltyPoints GetPointsExchangeRate(IConnectionManager entry, RecordIdentifier schemeID);

        void CopyRules(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo);

        LoyaltyPoints Get(IConnectionManager entry, RecordIdentifier loyaltyPointsLineID);
    }
}