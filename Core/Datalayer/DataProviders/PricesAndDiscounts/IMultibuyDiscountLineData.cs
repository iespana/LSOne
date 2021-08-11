using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IMultibuyDiscountLineData : IDataProvider<MultibuyDiscountLine>
    {
        MultibuyDiscountLine GetMultibuyOfferForQuantity(IConnectionManager entry, RecordIdentifier offerID, decimal quantity );

        /// <summary>
        /// Gets all configurations for a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">ID of the Offer</param>
        /// <returns>List of MultibuyDiscountLine entities</returns>
        List<MultibuyDiscountLine> GetAllForOffer(IConnectionManager entry, RecordIdentifier offerID);
    }
}