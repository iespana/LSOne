using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface ITradeAgreementData : IDataProviderBase<TradeAgreementEntry>, ICompareListGetter<TradeAgreementEntry>, ISequenceable
    {
        /// <summary>
        /// Gets a trade agreement entry with the a give ID and a given type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of trade agreement entry</param>
        /// <param name="relation">The type of trade agreement</param>
        TradeAgreementEntry Get(IConnectionManager entry, RecordIdentifier agreementID,
            TradeAgreementRelation relation);

        /// <summary>
        /// Gets a trade agreement entry with the a give ID and a total discount type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of trade agreement entry</param>
        /// <returns>A trade agreement entry with the a give ID and a total discount type</returns>
        TradeAgreementEntry GetTotalDiscount(IConnectionManager entry, RecordIdentifier agreementID);
        

        /// <summary>
        /// Gets all trade agreement entries for an item that have a specific trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="accountRelation">Search for specific account relation</param>
        /// <param name="priceCustomerItem">Include or exclude the specific account relation</param>
        /// <returns>A list of trade agreement entries for an item and a trade agreement type</returns>
        ///
        List<TradeAgreementEntry> GetForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            TradeAgreementRelation relation,
            bool priceCustomerItem,
            RecordIdentifier accountRelation);

        /// <summary>
        /// Gets all trade agreement entries for an item that have a specific trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>A list of trade agreement entries for an item and a trade agreement type</returns>
        ///
        List<TradeAgreementEntry> GetForItem(
            IConnectionManager entry,
            RecordIdentifier itemID,
            TradeAgreementRelation relation);

        /// <summary>
        /// Gets all trade agreement entries for an item for all trade agreement type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="lineDiscountGroupID">The unique ID of the line discount group this item belongs to</param>
        /// <param name="multilineDiscountGroupID">The unique ID of the multiline discount group this item belongs to</param>
        /// <returns>A list of trade agreement entries for an item</returns>
        List<TradeAgreementEntry> GetForItemAndItemGroups(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier lineDiscountGroupID,
            RecordIdentifier multilineDiscountGroupID);

        ///// <summary>
        ///// Gets all trade agreement entries for a list of items that have a specific trade agreement type
        ///// </summary>
        ///// <param name="entry">The entry into the database</param>
        ///// <param name="itemIDs">Unique IDs of the items</param>
        ///// <param name="relation">The type of trade agreement entries to get</param>
        ///// <returns>A list of trade agreement entries for a list of items and a trade agreement type</returns>
        //List<TradeAgreementEntry> GetForItems(
        //    IConnectionManager entry,
        //    List<RecordIdentifier> itemIDs,
        //    TradeAgreementRelation relation);

        /// <summary>
        /// Gets all trade agreements for a given item discount group that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemDiscountGroupID">The unique ID of the item discount
        /// group</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>All trade agreements for a given item discount group that have a specific
        /// trade agreement type</returns>
        List<TradeAgreementEntry> GetForItemDiscountGroup(
            IConnectionManager entry,
            RecordIdentifier itemDiscountGroupID,
            TradeAgreementRelation relation);

        /// <summary>
        /// Gets all trade agreements for a given customer that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="allCustomers">Explicitly include or exclude agreements that apply to all customers</param>
        /// <returns>All trade agreements for a given customer that have a specific trade agreement type</returns>
        List<TradeAgreementEntry> GetForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID,
            TradeAgreementRelation relation,
            bool? allCustomers = null);

        /// <summary>
        /// Gets all trade agreements for a given customer that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="groupID">The unique ID of the group the customer is in</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <returns>All trade agreements for a given customer and customer group that customer is in that have a specific trade agreement type</returns>
        List<TradeAgreementEntry> GetForCustomerAndGroup(
            IConnectionManager entry,
            RecordIdentifier customerID,
            RecordIdentifier groupID,
            TradeAgreementRelation relation);

        /// <summary>
        /// Gets all total discount trade agreements for a given customer 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <returns>All total discount trade agreements for a given customer </returns>
        List<TradeAgreementEntry> GetTotalDiscForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID);

        /// <summary>
        /// Gets all total discount trade agreements for a given customer 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The unique ID of the customer</param>
        /// <param name="groupID">The unique ID of the group the customer is in</param>
        /// <returns>All total discount trade agreements for a given customer </returns>
        List<TradeAgreementEntry> GetTotalDiscForCustomer(
            IConnectionManager entry,
            RecordIdentifier customerID,
            RecordIdentifier groupID);

        /// <summary>
        /// Gets all trade agreements for a given customer group that have a specific
        /// trade agreement type 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupID">The unique ID of customer group</param>
        /// <param name="relation">The type of trade agreement entries to get</param>
        /// <param name="itemID">Optional if the aggrements retuned should be limited to a single item</param>
        /// <returns>All trade agreements for a given customer group that have a specific
        /// trade agreement type </returns>
        List<TradeAgreementEntry> GetForGroup(
            IConnectionManager entry,
            RecordIdentifier groupID,
            TradeAgreementRelation relation,
            RecordIdentifier itemID = null);

        ///// <summary>
        ///// Gets all total discount trade agreements for a given customer group 
        ///// </summary>
        ///// <param name="entry">The entry into the database</param>
        ///// <param name="groupID">The unique ID of the customer group</param>
        ///// <returns>All total discount trade agreements for a given customer group</returns>
        //List<TradeAgreementEntry> GetTotalDiscForGroup(
        //    IConnectionManager entry,
        //    RecordIdentifier groupID);

        //List<TradeAgreementEntry> GetTradeAggreementsForSale(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier customerID, RecordIdentifier salesType, RecordIdentifier storeID);

        /// <summary>
        /// Returns true if a trade agreement entry exists with the given oldeAgreementID ID (excludedNewAgreementID ID is excluded). Used when updating a trade agreement 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="oldAgreementID">The old trade agreement ID, which had 10 primary keys (see OldID in TradeAgreementEntry class)</param>
        /// <param name="excludedNewAgreementID">The unique ID of the trade agreement to exlucde</param>
        /// <returns>True if a trade agreement entry exists with the given oldeAgreementID ID (excludedNewAgreementID ID is excluded)</returns>
        bool DataContentExists(IConnectionManager entry, RecordIdentifier oldAgreementID, RecordIdentifier excludedNewAgreementID);

        /// <summary>
        /// Returns true if a trade agreement entry exists with the given oldeAgreementID ID. Used when creating a new trade agreement
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="oldAgreementID">The old trade agreement ID, which had 10 primary keys (see OldID in TradeAgreementEntry class)</param>
        /// <returns>Returns true if a trade agreement entry exists with the given oldeAgreementID ID</returns>
        bool DataContentExists(IConnectionManager entry, RecordIdentifier oldAgreementID);

        /// <summary>
        /// Deletes a tradeagreement record based on the main ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreementID">The unique ID of the trade agreement</param>
        /// <param name="permission">Permission string use to validate user permission. 
        /// Use BusinessObjects.Permisson.ManageTradeAgreementPrices for price trade agreements or
        /// BusinessObjects.Permisson.ManageDiscounts for discount trade agreements</param>
        void Delete(IConnectionManager entry, RecordIdentifier agreementID, string permission);

        /// <summary>
        /// Deletes a tradeagreement record based on all the logical keys, not based on the main ID.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreement">TradeAgreementEntry data entity, the price fields or main ID do not need to be populated</param>
        void Delete(IConnectionManager entry, TradeAgreementEntry agreement);

        /// <summary>
        /// Saves the given class to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="agreement">The trade agreement to save</param>
        /// <param name="permission">Permission string use to validate user permission. 
        /// Use BusinessObjects.Permisson.ManageTradeAgreementPrices for price trade agreements or
        /// BusinessObjects.Permisson.ManageDiscounts for discount trade agreements</param>
        void Save(IConnectionManager entry, TradeAgreementEntry agreement, string permission);

        /// <summary>
        /// Gets tradeagreement id for logical keys of a tradeagreement ID. This is used for example in multiediting
        /// to know if we need to update or add a record.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="agreement"></param>
        /// <returns></returns>
        RecordIdentifier GetTradeAgreementID(IConnectionManager entry, TradeAgreementEntry agreement);
    }
}