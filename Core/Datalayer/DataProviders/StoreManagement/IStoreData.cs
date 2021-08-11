using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.StoreManagement.Validity;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IStoreData : IDataProvider<Store>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all stores
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all stores</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all stores except one specific one
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="excludeStoreID">The store to be excluded</param>
        /// <returns>A list of all store except one that was specified</returns>
        List<DataEntity> GetList(IConnectionManager entry,RecordIdentifier excludeStoreID);        

        /// <summary>
        /// Gets one specific store as simple data entity, that is ID and description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store to fetch</param>
        /// <returns>The requested store or null if not found</returns>
        DataEntity GetStoreEntity(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Gets all available store for a given terminal, including the current selected store, excluding stores already having the same terminal ID
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="currentTerminal"></param>
        /// <param name="currentStore"></param>
        /// <returns></returns>
        List<DataEntity> GetListForTerminal(IConnectionManager entry, RecordIdentifier currentTerminal, RecordIdentifier currentStore);

        /// <summary>
        /// Gets all stores. The amount of information populated depends on the <see cref="UsageIntentEnum"/> parameter given.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <returns></returns>
        List<Store> GetStores(IConnectionManager entry, UsageIntentEnum usageIntent = UsageIntentEnum.Normal);

        /// <summary>
        /// Search for a list of stores with minimal information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">Search filter</param>
        /// <returns></returns>
        List<StoreListItem> Search(IConnectionManager entry, StoreListSearchFilter filter);

        /// <summary>
        /// Search for a list of stores with extended information
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">Search filter</param>
        /// <returns></returns>
        List<StoreListItemExtended> SearchExtended(IConnectionManager entry, StoreListSearchFilterExtended filter);

        bool WarnOnStatementPostingIfSuspendedTransExists(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Checks if any store is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any store uses the tax group, else false</returns>
        bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID);

        RecordIdentifier GetDefaultStoreID(IConnectionManager entry);
        RecordIdentifier GetStoresSalesTaxGroupID(IConnectionManager entry, RecordIdentifier storeID);
        RecordIdentifier GetDefaultStoreSalesTaxGroup(IConnectionManager entry);
        List<Payment> GetCountableStorePayments(IConnectionManager entry, string storeID);

        /// <summary>
        /// Returns a list of all payment types for the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <returns>A list of all payment types for the given store ID</returns>
        List<Payment> GetStorePayments(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Gets the store payment type that the given card number belongs to
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="cardNumber">The first 6 numbers of the card number</param>
        /// <returns>The store payment type that this card type belongs to</returns>
        Payment GetStorePaymentFromCardNumber(IConnectionManager entry, RecordIdentifier storeID, string cardNumber);

        string GetStoreCurrencyCode(IConnectionManager entry, RecordIdentifier storeID);
        Payment GetStorePayment(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier paymentID, CacheType cacheType = CacheType.CacheTypeNone);
        Payment GetStoreCashPayment(IConnectionManager entry, RecordIdentifier storeID, CacheType cacheType = CacheType.CacheTypeNone);
        Store Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal, bool includeReportFormatting = false);
        void PopulateStoreNormal(IConnectionManager entry, IDataReader dr,Store store, object param);
        void PopulateStoreMinimal(IConnectionManager entry, IDataReader dr, Store store, object param);

        /// <summary>
        /// Returns if store has a price setting where prices should be fetched with or without tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>True and store has price with tax, false and store has price without tax</returns>
        bool GetPriceWithTaxForStore(IConnectionManager entry, RecordIdentifier storeID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns if store has a price setting where prices should be fetched with or without tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="store">The store</param>
        /// <returns>True and store has price with tax, false and store has price without tax</returns>
        bool GetPriceWithTaxForStore(IConnectionManager entry, Store store);

        /// <summary>
        /// Checks if all stores are valid, returning list of all store IDs and their validity report.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>List of all store IDs and their validity report</returns>
        List<StoreValidity> CheckStoreValidity(IConnectionManager entry);

        /// <summary>
        /// Loads all available stores within the given segment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rowFrom">starting row</param>
        /// <param name="rowTo">the end row</param>
        /// <param name="totalRecordsMatching">how many rows are there</param>
        /// <returns></returns>
        List<Store> LoadStores(
            IConnectionManager entry,
            int rowFrom,
            int rowTo,
            out int totalRecordsMatching);

        /// <summary>
        /// Returns true if the KDS profile is used in one or more stores
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="kdsProfileId">ID of the KDS manager profile</param>
        /// <returns>true if the KDS profile is used in one or more stores</returns>
        bool KdsProfileInUse(IConnectionManager entry, RecordIdentifier kdsProfileId);
    }
}