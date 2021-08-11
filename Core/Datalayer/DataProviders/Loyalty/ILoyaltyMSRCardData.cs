using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltyMSRCardData : IDataProvider<LoyaltyMSRCard>, ISequenceable
    {
        /// <summary>
        /// Gets the list of MSR cards.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customers"></param>
        /// <param name="schemas"></param>
        /// <param name="cardID">CardID</param>
        /// <param name="hasCustomers">Should customers exlusivly included or excluded</param>
        /// <param name="tenderType">Tender Type</param>
        /// <param name="status"></param>
        /// <param name="statusInequality"></param>
        /// <param name="rowFrom">from row </param>
        /// <param name="rowTo">to row</param>
        /// <param name="sortBy">sort descending</param>
        /// <param name="backwards">sort descending</param>
        /// <returns>List of instances of <see cref="LoyaltyMSRCard"/></returns>
        List<LoyaltyMSRCard> GetList(IConnectionManager entry, 
            List<DataEntity> customers, 
            List<DataEntity> schemas, 
            RecordIdentifier cardID, 
            bool? hasCustomers,
            int tenderType, 
            double? status, 
            LoyaltyMSRCardInequality statusInequality, 
            int rowFrom, 
            int rowTo, 
            LoyaltyMSRCardSorting sortBy, 
            bool backwards);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="LoyaltyMSRCard" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="sortBy">sort descending</param>
        /// <param name="backwards">sort descending</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        List<LoyaltyMSRCard> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, 
            LoyaltyMSRCardSorting sortBy, bool backwards);

        LoyaltyMSRCard Get(IConnectionManager entry, RecordIdentifier loyaltyCardNumber);

        LoyaltyMSRCard GetCustomerMobileCard(IConnectionManager entry, RecordIdentifier customerID);

        bool ExistsForLoyaltyScheme(IConnectionManager entry, RecordIdentifier loyaltySchemeID);
    }
}