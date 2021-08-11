using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IDiscountOfferDataOLD : IDataProviderBase<DiscountOffer>, ISequenceable
    {
        /// <summary>
        /// Gets name of a discount offer from a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">ID of the discount offer</param>
        /// <returns>The discount offer name or empty string if not found</returns>
        string GetDiscountName(IConnectionManager entry,RecordIdentifier offerID);

        DiscountOffer Get(IConnectionManager entry, RecordIdentifier offerID,DiscountOffer.PeriodicDiscountOfferTypeEnum type);

        /// <summary>
        /// Returns a sorted list of all periodic discounts (Discount offer, Mix and Match, Multibuy, Promotines)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludePromotions">Controls wether promotions are excluded from the results</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetPeriodicDiscounts(IConnectionManager entry, bool excludePromotions, DiscountOfferSorting sortBy, 
            bool sortBackwards);

        /// <summary>
        /// Returns a sorted list of all periodic discounts (Discount offer, Mix and Match, Multibuy, Promotines)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetManuallyTriggeredPeriodicDiscounts(IConnectionManager entry, DiscountOfferSorting sortBy, 
            bool sortBackwards);

        /// <summary>
        /// Returns a sorted list of all periodic dicsounts for a given line discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineDiscountGroupID">The ID of the line discount group</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetOffersForLineDiscountGroup(IConnectionManager entry, RecordIdentifier lineDiscountGroupID, 
            DiscountOfferSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Returns a sorted list of all promotions for a given line discount group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineDiscountGroupID">The ID of the line discount group</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetPromotionsForLineDiscountGroup(IConnectionManager entry, RecordIdentifier lineDiscountGroupID, 
            DiscountOfferSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Returns a sorted list of all periodic discounts for a given customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">Controls wether promotions are excluded from the results</param>
        /// <param name="customerID">The ID of the customer</param>
        /// <returns></returns>
        List<DiscountOffer> GetForCustomer(IConnectionManager entry, DiscountOfferFilter filter, RecordIdentifier customerID);

        /// <summary>
        /// Gets a sorted list of all Discount offers, Mix and Match and Multiby periodic discounts.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetAllOffers(IConnectionManager entry, DiscountOfferSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets the next valid priority value that can be used for a periodic discount. 
        /// By default new priority values increment in steps of 10.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        int GetNextPriority(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all priority values currently in use ordered in ascending order.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<int> GetPrioritiesInUse(IConnectionManager entry);

        /// <summary>
        /// Gets a list of periodic discounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">The periodic discount type to get</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<DiscountOffer> GetOffers(IConnectionManager entry, DiscountOffer.PeriodicDiscountOfferTypeEnum type, 
            DiscountOfferSorting sortBy, bool backwardsSort);

        List<DiscountOffer> GetOffers(IConnectionManager entry, DiscountOffer.PeriodicDiscountOfferTypeEnum type, 
            int sortColumn, bool backwardsSort);
        DiscountOffer GetOfferFromLine(IConnectionManager entry, RecordIdentifier offerId);

        /// <summary>
        /// Gets a list of all discount offers for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        List<DiscountOffer> GetOffersFromRelation(IConnectionManager entry, RecordIdentifier relationID,
            DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets a list of all promotion offers for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        List<DiscountOffer> GetPromotionsFromRelation(IConnectionManager entry, RecordIdentifier relationID, DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets a list of all offers and promotions for the given relation i.e all offers for a give retail group or retail item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="relationID">The relation ID, f.ex a retial item ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        List<DiscountOffer> GetOffersAndPromotionsFromRelation(IConnectionManager entry, RecordIdentifier relationID, DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets number of discounts expiring over the next 7 days (includes current day and the next 7 days), so if today is monday, then it includes today and next monday
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Number of discounts expiring in the next 7 days or today</returns>
        int GetNumberOfDiscountsExpiringOverTheNext7Days(IConnectionManager entry);

        /// <summary>
        /// Gets number of active discounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Number of active discounts</returns>
        int GetNumberOfActiveDiscounts(IConnectionManager entry);

        /// <summary>
        /// Returns true if there are discounts configured, else false
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool DiscountsAreConfigured(IConnectionManager entry);

        void Delete(IConnectionManager entry, RecordIdentifier offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum type);
        void UpdateStatus(IConnectionManager entry, RecordIdentifier offerID,bool enabled);

        void Save(IConnectionManager entry, DiscountOffer offer);
    }
}