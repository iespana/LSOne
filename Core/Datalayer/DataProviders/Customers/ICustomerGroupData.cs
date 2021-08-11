using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Customers
{
    public interface ICustomerGroupData : IDataProviderBase<CustomerGroup>, ISequenceable
    {
        /// <summary>
        /// Returns a list of all Customer groups 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of groups</returns>
        List<CustomerGroup> GetList(IConnectionManager entry);

        /// <summary>
        /// Returns the Customer group with the given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerGroupID">ID of the group to get</param>
        /// <param name="cacheType">Type of cache to be used</param>
        /// <returns></returns>
        CustomerGroup Get(IConnectionManager entry, RecordIdentifier customerGroupID, CacheType cacheType);

        /// <summary>
        /// Checks if a customer group by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to check for</param>
        /// <returns>True if the customer exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier id);
        
        /// <summary>
        /// Deletes a customer group with a given ID. All customers that were in that group are also removed from it
        /// </summary>
        /// <remarks>Requires the 'Edit customer group' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the group to delete</param>
        void Delete(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Returns true if a customer group has customers assigned to it
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The customer group ID to check for</param>
        /// <returns></returns>
        bool GroupHasCustomers(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Returns a list of groups the customer belongs to
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">The customer ID to search for</param>
        /// <returns>A list of groups</returns>
        List<CustomerGroup> GetGroupsForCustomer(IConnectionManager entry, RecordIdentifier customerID);

        /// <summary>
        /// Returns the default group of the given customer. Null if customer has no default group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">Id of the customer</param>
        CustomerGroup GetDefaultCustomerGroup(IConnectionManager entry, RecordIdentifier customerID);

        /// <summary>
        /// Saves a customer group to the database
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="group">The group to be saved</param>
        void Save(IConnectionManager entry, CustomerGroup group);

        /// <summary>
        /// Removes a customer from a specific customer group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="customerID">The customer to be removed</param>
        /// <param name="groupID">The group the customer is in</param>
        void DeleteCustomerFromGroup(IConnectionManager entry, RecordIdentifier customerID, RecordIdentifier groupID);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="CustomerGroup" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<CustomerGroup> Search(IConnectionManager entry, string searchString,
                                               int rowFrom, int rowTo,
                                               bool beginsWith, CustomerGroupSorting sort);
    }
}