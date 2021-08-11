using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IDiscountOfferLineDataOLD : IDataProvider<DiscountOfferLine>
    {
        /// <summary>
        /// Gets a single discount offer line for the given offer, relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="relationID">The relation ID, f.ex a retail item ID or a retail group ID</param>
        /// <param name="relationType">The type of relation</param>
        /// <returns></returns>
        DiscountOfferLine Get(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relationID, 
            DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets all periodic discount lines for a given offer for all discount types except mix and match
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="sortColumn">The number of the sort colum. 0 = PRODUCTTYPE, 1 = ID, 2 = LINEGROUP, 3 = STDPRICE, 4 = DEALPRICEORDISCPCT, 5 = OFFERPRICEINCLTAX, 6 = DISCAMOUNTINCLTAX</param>
        /// <param name="backwardsSort">True if sorting should be backwards</param>
        /// <param name="maxLines">Maximum number of lines to retrieve</param>
        /// <param name="totalLines">Total available lines</param>
        /// <returns>List of discount lines for a given offer</returns>
        List<DiscountOfferLine> GetLines(IConnectionManager entry, RecordIdentifier offerID, int sortColumn, bool backwardsSort, int maxLines, out int totalLines);

        /// <summary>
        /// Gets all discount offer lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        List<DiscountOfferLine> GetDiscountOfferLinesForItem(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Checks if an relation exists for a given periodic discount offer   
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The type of relation <see cref="DiscountOfferLine.DiscountOfferTypeEnum"/></param>
        /// <param name="discountType">The type of discount offer <see cref="DiscountOffer.PeriodicDiscountOfferTypeEnum"/></param>
        /// <returns></returns>
        bool RelationExists(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, 
            DiscountOfferLine.DiscountOfferTypeEnum relationType, DiscountOffer.PeriodicDiscountOfferTypeEnum discountType);

        /// <summary>
        /// Gets all discount offer lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        List<DiscountOfferLine> GetDiscountOfferLinesForRetailGroup(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Gets all discount offer lines for a given special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        List<DiscountOfferLine> GetDiscountOfferLinesForSpecialGroup(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Gets all discount offer lines for a given retail department
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The id of the department</param>
        List<DiscountOfferLine> GetDiscountOfferLinesForRetailDepartment(IConnectionManager entry, RecordIdentifier departmentID);

        /// <summary>
        /// Gets all multibuy lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        List<DiscountOfferLine> GetMultiBuyLinesForItem(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Gets all mix and match lines for a given item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the item</param>
        List<DiscountOfferLine> GetMixMatchLinesForItem(IConnectionManager entry, RecordIdentifier itemID);

        /// <summary>
        /// Gets all mix and match lines for a given retail group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The id of the group</param>
        List<DiscountOfferLine> GetMixMatchLinesForRetailGroup(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Gets a single mix and match discount offer line by a given id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGuid">The id of the line to fetch</param>
        /// <param name="mixAndMatchType">A enum stating what kind of mix and match offer we are dealing with</param>
        /// <returns>The mix and match discount offer line or null if not found</returns>
        DiscountOfferLine GetMixAndMatchLine(IConnectionManager entry, RecordIdentifier lineGuid, 
            DiscountOffer.MixAndMatchDiscountTypeEnum mixAndMatchType);

        /// <summary>
        /// Gets all periodic discount lines for a given mix and match offer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="mixAndMatchType">A enum stating what kind of mix and match offer we are dealing with</param>
        /// <returns>List of mix and match discount lines for a given offer</returns>
        List<DiscountOfferLine> GetMixAndMatchLines(IConnectionManager entry, RecordIdentifier offerID, 
            DiscountOffer.MixAndMatchDiscountTypeEnum mixAndMatchType);

        /// <summary>
        /// Gets a list of data entities conaining the IDs and names of the rtail items and groups belonging to the discount offer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="type">The type of discount offer line to get</param>
        /// <returns></returns>
        List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, 
            DiscountOfferLine.DiscountOfferTypeEnum type);

        /// <summary>
        /// Gets a list of data entities conaining the IDs and names of the rtail items and groups belonging to the discount offer for the given line group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="lineGroupID">The line group ID</param>
        /// <param name="type">The type of discount offer line to get</param>
        /// <returns></returns>
        List<DataEntity> GetSimpleLines(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier lineGroupID, 
            DiscountOfferLine.DiscountOfferTypeEnum type);

        /// <summary>
        /// Deletes a discount offer line by relation and relation type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The discount offer ID</param>
        /// <param name="relation">The relation ID</param>
        /// <param name="relationType">The relation type</param>
        void DeleteByRelation(IConnectionManager entry, RecordIdentifier offerID, RecordIdentifier relation, DiscountOfferLine.DiscountOfferTypeEnum relationType);

        /// <summary>
        /// Gets a specific DiscountofferLine
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineGuid">The discount line ID</param>
        /// <returns></returns>
        DiscountOfferLine Get(IConnectionManager entry, RecordIdentifier lineGuid);

        /// <summary>
        /// Deletes all discount lines for a variant.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="variant">The variant id</param>
        void DeleteDiscountLinesForVariant(IConnectionManager entry, RecordIdentifier variant);

        /// <summary>
        /// Gets the line count for a discount offer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>Discount line count</returns>
        int GetLineCount(IConnectionManager entry, RecordIdentifier offerID);

        /// <summary>
        /// Gets all variants on discaount for a specific offer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>A list of variant ids and the variant name</returns>
        List<DataEntity> GetVariants(IConnectionManager entry, RecordIdentifier offerID);
    }
}