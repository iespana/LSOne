using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IPromotionOfferLineDataOLD : IDataProvider<PromotionOfferLine>
    {
        /// <summary>
        /// Gets a single promotion line for the given offer, relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="relationID">The relation ID, f.ex a retail item ID or retail group ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        PromotionOfferLine Get(IConnectionManager entry,
            RecordIdentifier offerID,
            RecordIdentifier relationID,
            DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets all promotion discount lines for a given offer
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="offerID"></param>
        /// <param name="sortBy">The sorting column</param>
        /// <param name="backwardsSort">"True" if sorted backwards</param>
        /// <returns>A list of discount offer lines</returns>
        List<PromotionOfferLine> GetLines(IConnectionManager entry, RecordIdentifier offerID, PromotionOfferLineSorting sortBy, 
            bool backwardsSort);

        /// <summary>
        /// Gets promotion discount lines for a given offer and line type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="type">The type of promotion offer line to get</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<PromotionOfferLine> GetLines(IConnectionManager entry, 
            RecordIdentifier offerID, 
            DiscountOfferLine.DiscountOfferTypeEnum type,
            PromotionOfferLineSorting sortBy,
            bool backwardsSort);

        /// <summary>
        /// Gets a list of data entities containing the IDs and names of the retail items and groups belonging to the promotion
        /// offer lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="type">The type of promotion offer line to get</param>
        /// <returns></returns>
        List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID,
            DiscountOfferLine.DiscountOfferTypeEnum type);

        List<PromotionOfferLine> GetPromotionsForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier itemVariantID,
            RecordIdentifier storeID,
            RecordIdentifier customerID,
             RecordIdentifier groupID,
            RecordIdentifier departmentID,
            bool checkPriceGroup = true,
            bool checkCustomerConnection = true);

        /// <summary>
        /// Gets all promotion discount lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        List<PromotionOfferLine> GetPromotionsForRetailGroup(
            IConnectionManager entry,
            RecordIdentifier groupID);

        /// <summary>
        /// Gets all promotion discount lines for a given special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        List<PromotionOfferLine> GetPromotionsForSpecialGroup(
            IConnectionManager entry,
            RecordIdentifier groupID);

        /// <summary>
        /// Gets all promotion discount lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The id of the group</param>
        /// <returns>A list of discount offer lines</returns>
        List<PromotionOfferLine> GetPromotionsForRetailDepartment(
            IConnectionManager entry,
            RecordIdentifier departmentID);

        /// <summary>
        /// Returns a list of valid promotions that the selected item is in.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the item to check for</param>
        /// <param name="itemVariantID">The variant ID of the item. Set to RecordIdentifier.Empty if item has no variant</param>
        /// <param name="storeID">The ID of the store we are working in. Used to check for valid price group settings. Set to 
        /// RecordIdentifier.Empty if no store context is available</param>
        /// <param name="customerID">The ID of the customer we are using. Used to check for valid price group settings. Set to RecordIdentifier.Empty if no customer context is available</param>
        /// <param name="groupID"></param>
        /// <param name="departmentID"></param>
        List<PromotionOfferLine> GetValidAndEnabledPromotionsForItem(
            IConnectionManager entry, 
            RecordIdentifier itemID, 
            RecordIdentifier itemVariantID, 
            RecordIdentifier storeID, 
            RecordIdentifier customerID,
             RecordIdentifier groupID,
            RecordIdentifier departmentID);

        /// <summary>
        /// Deletes a discount line by relation and relation type. For example by retail item id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The promotion offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The relation type</param>
        void DeleteByRelation(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType);

        PromotionOfferLine Get(IConnectionManager entry, RecordIdentifier lineID);
    }
}