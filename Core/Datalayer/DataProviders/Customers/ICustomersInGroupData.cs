using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Customers
{
    public interface ICustomersInGroupData : IDataProvider<CustomerInGroup>
    {
        /// <summary>
        /// Returns a list of customers that belongs to a specific customer gorup
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerGroupID">The customer group ID to check</param>
        /// <param name="recordFrom">If not null then only specific rows are returned for paging purposes</param>
        /// <param name="recordTo">If not null then only specific rows are returned for paging purposes</param>
        /// <returns>A list of <see cref="CustomerInGroup"/></returns>
        List<CustomerInGroup> GetCustomersInGroupList(IConnectionManager entry,
                                                            RecordIdentifier customerGroupID,
                                                            int? recordFrom,
                                                            int? recordTo);

        /// <summary>
        /// Returns a list of groups that a customer is a part of
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">The customer ID to look for</param>
        /// <returns>A list of groups</returns>
        List<CustomerInGroup> GetGroupsForCustomerList(IConnectionManager entry,
                                                            RecordIdentifier customerID);

        /// <summary>
        /// Clears the default value of a customer group for a customer
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="inGroup">The information about the group and customer that should be changed</param>
        void ClearDefaultValueForCustomer(IConnectionManager entry, CustomerInGroup inGroup);

        /// <summary>
        /// Sets a specific group as default for a customer
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="inGroup">Information about the group and customer that should be changed</param>
        void SetGroupAsDefault(IConnectionManager entry, CustomerInGroup inGroup);
    }
}
