using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IPOSUserData : IDataProvider<POSUser>
    {
        /// <summary>
        /// Get all POS users
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>All POS users</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Get POS users based on the filter parameters
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">If specified, the method returns all POS users whose STAFFID matches the <paramref name="id"/> value</param>
        /// <param name="description">If specified, the method returns all POS users whose NAME matches the <paramref name="description"/> value</param>
        /// <param name="maxCount">If maxCount is positive and the number of found POS users is bigger than maxCount, then maxCount will limit the number of returned POS users</param>
        /// <returns>POS users based on the filter parameters</returns>
        List<DataEntity> Search(IConnectionManager entry, string id, string description, int maxCount);

        /// <summary>
        /// Get POS users based on the filter parameters
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">If specified, the method returns all POS users whose STAFFID matches the <paramref name="id"/> value</param>
        /// <param name="description">If specified, the method returns all POS users whose NAME matches the <paramref name="description"/> value</param>
        /// <returns>POS users based on the filter parameters</returns>
        List<POSUser> Search(IConnectionManager entry, RecordIdentifier id, string description);

        /// <summary>
        /// Get all POS users for the given store and usage intent
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="usageIntent">The usage intent</param>
        /// <param name="cacheType">The cache type. This parameter has no effect</param>
        /// <returns>All POS users for the given store and usage intent</returns>
        List<POSUser> GetList(IConnectionManager entry, RecordIdentifier storeID, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Get POS user by ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff ID</param>
        /// <param name="usageIntent">The usage intent</param>
        /// <param name="cacheType">The cache type</param>
        /// <returns>POS user</returns>
        POSUser Get(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Get POS user by ID with option to restrict the query to the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff ID</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="staffListLimitedToStore">If true, the restriction to the given store is applied</param>
        /// <param name="cacheType">The cache type</param>
        /// <returns>POS user</returns>
        POSUser GetPerStore(IConnectionManager entry, RecordIdentifier id, RecordIdentifier storeID, bool staffListLimitedToStore, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets the name and the name on the receipt of the staff based on the given staff ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The staff I</param>
        /// <param name="name">The staff name</param>
        /// <param name="nameOnReceipt">The staff name on the receipt</param>
        /// <returns>true if the operation is successful; false if the operation fails</returns>
        bool GetNameAndNameOnReceipt(IConnectionManager entry, RecordIdentifier id, ref string name, ref string nameOnReceipt);
    }
}