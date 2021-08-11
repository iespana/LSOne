using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Customers
{
    public interface ICustomerAddressData : IDataProvider<CustomerAddress>
    {
        /// <summary>
        /// Gets all address for a particular customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="accountNum">Customer id</param>
        /// <returns>List of addresses for the customer</returns>
        List<CustomerAddress> GetListForCustomer(IConnectionManager entry, RecordIdentifier accountNum);

        /// <summary>
        /// Gets a specific address for a customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="accountNum">ID of the customer to fetch</param>
        /// <param name="addressType">Address to retrieve</param>
        /// <returns>The requested customer address</returns>
        CustomerAddress Get(IConnectionManager entry, RecordIdentifier accountNum, Address.AddressTypes addressType);

        /// <summary>
        /// Deletes the specified customer
        /// </summary>
        /// <remarks>Edit customer permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerAddress">Address</param>
        void Delete(IConnectionManager entry, CustomerAddress customerAddress);

        /// <summary>
        /// Deletes a customer address by a given ID
        /// </summary>
        /// <remarks>Edit customer permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="accountNum">ID of the customer whose address should be deleted</param>
        /// <param name="addressType">Address to delete</param>
        void Delete(IConnectionManager entry, RecordIdentifier accountNum, RecordIdentifier id);

        /// <summary>
        /// Deletes all addresses for a given customer
        /// </summary>
        /// <remarks>Edit customer permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="accountNum">ID of the customer whose addresses should be deleted</param>
        void DeleteAll(IConnectionManager entry, RecordIdentifier accountNum);

        /// <summary>
        /// Checks if a customer by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="accountNum">ID of the customer to check for</param>
        /// <param name="addressType">Address to delete</param>
        /// <returns>True if the customer exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier accountNum, RecordIdentifier id);


        bool HasDefaultAddress(IConnectionManager entry, RecordIdentifier customerID, Address.AddressTypes addressType);

        /// <summary>
        /// Checks if a customer with the given ID has any addresses
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">The account number of the customer to check for</param>
        /// <returns>True if the customer has one or more addresses, false otherwise</returns>
        bool HasAddress(IConnectionManager entry, RecordIdentifier customerID);
        void SetAddressAsDefault(IConnectionManager entry, RecordIdentifier customerID, CustomerAddress customerAddress);
    }
}