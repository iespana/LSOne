using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltySchemesData : IDataProvider<LoyaltySchemes>, ISequenceable
    {
        /// <summary>
        /// Gets the list of Loyalty schemes.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of instances of <see cref="LoyaltySchemes"/></returns>
        List<LoyaltySchemes> GetList(IConnectionManager entry);

        LoyaltySchemes Get(IConnectionManager entry, RecordIdentifier schemesID);

    }
}