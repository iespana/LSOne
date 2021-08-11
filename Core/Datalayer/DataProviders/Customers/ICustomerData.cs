using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Tools;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;

namespace LSOne.DataLayer.DataProviders.Customers
{
    public interface ICustomerData : IDataProvider<Customer>, ICompareListGetter<Customer>, ISequenceable
    {
        List<CustomerListItem> Search(IConnectionManager entry, SearchParameter[] parameters, int maxCount);

        /// <summary>
        /// Searches for customers that contain a given search text, and returns the results as a List of CustomerListItem
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in customer NAME, customer ACCOUNTNUM and customer INVOICEACCOUNT fields.</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sortBy">A enum defining the sort column</param>
        /// <param name="sortBackwards">Set to true if wanting to sort backwards, else false</param>
        /// <returns>A list of found customers</returns>
        List<CustomerListItem> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, 
            CustomerSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Searches for customers that contain a given search text, and returns the results as a List of CustomerListItem
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in customer NAME, customer ACCOUNTNUM and customer ADDRESS fields.</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sortBy">A enum defining the sort column</param>
        /// <param name="sortBackwards">Set to true if wanting to sort backwards, else false</param>
        /// <returns>A list of found customers</returns>
        List<CustomerListItem> SearchWithAddress(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith,
            CustomerSorting sortBy, bool sortBackwards);

        List<CustomerListItem> Search(IConnectionManager entry, string displayName, string firstName, string lastName, int rowFrom, int rowTo, bool beginsWith, CustomerSorting sortBy, bool sortBackwards);

        List<CustomerListItemAdvanced> AdvancedSearch(IConnectionManager entry, 
            int rowFrom, 
            int rowTo, 
            out int totalRecordsMatching, 
            CustomerSorting sortBy, 
            bool sortBackwards,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier salesTaxGroupID = null,
            RecordIdentifier priceGroupID = null,
            RecordIdentifier lineDiscountGroupID = null,
            RecordIdentifier totalDiscountGroupID = null,
            RecordIdentifier invoiceCustomerID = null,
            BlockedEnum? isBlocked = null,
            bool? showDeleted = null,
            TaxExemptEnum? taxExempt = null
            );

        /// <summary>
        /// Gets a customer by a card number
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="cardNumber">Number of the card</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns></returns>
        Customer GetByCardNumber(IConnectionManager entry, RecordIdentifier cardNumber,UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone);

        string GetCustomerName(IConnectionManager entry, RecordIdentifier id);
        Name GetCustomerLongName(IConnectionManager entry, RecordIdentifier id);
        List<CustomerListItem> GetLocallySavedCustomers(IConnectionManager entry);
        bool LocallySavedCustomersExist(IConnectionManager entry);

        /// <summary>
        /// Gets a customer by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to fetch</param>
        /// <param name="usageIntent">Specifies what extra data should be loaded</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>The requested customer or null if not found</returns>
        Customer Get(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone);

        List<Customer> GetAllCustomers(IConnectionManager entry, UsageIntentEnum usageIntent, bool getDeletedCustomers = true);
        Customer GetTemporaryInvoiceCustomer(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone);        
        List<CustomerListItem> GetList(IConnectionManager entry,string searchString, CustomerSorting sortBy, bool sortBackwards, bool beginsWith = true, RecordIdentifier excludeID = null);

		RecordIdentifier GetOmniCustomerID(IConnectionManager entry, string userName, string password);
        List<DataEntity> GetOmniCustomers(IConnectionManager entry); 
        bool ExistsOmniCustomer(IConnectionManager entry, RecordIdentifier customerID);
        bool ExistsOmniUser(IConnectionManager entry, string userName);
        void DeleteOmnicustomer(IConnectionManager entry, RecordIdentifier customerID);
        void SaveOmniCustomer(IConnectionManager entry, RecordIdentifier customerID, string userName, string password);

        void DeleteCustomer(IConnectionManager entry, RecordIdentifier id);

        void DeleteAllCustomers(IConnectionManager entry);

        /// <summary>
        /// Checks if customer has dependant entries preventing it from being deleted
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CustomerCanBeDeleted(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Checks if any customer is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any customer uses the tax group, else false</returns>
        bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID);        

        /// <summary>
        /// Updates blocking status for customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">Customer to be updated</param>
        /// <param name="blockingStatus">The blocking status</param>
        void SaveBlockStatus(IConnectionManager entry, RecordIdentifier customerID, BlockedEnum blockingStatus);

        /// <summary>
        /// Updates the tax exempt value for customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerIDs">A list of customer IDs to be updated</param>
        /// <param name="taxExempt">What the tax exempt status should be for all the customers</param>
        /// <param name="salesTaxGroupID">The sales tax group that should be set for all the customers</param>
        void SaveTaxInformation(IConnectionManager entry, List<RecordIdentifier> customerIDs, TaxExemptEnum taxExempt, RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Updates all customers that are marked as tax exempt to have the default tax exempt tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxExemptTaxGroupID">The tax group that should be set for all the tax exempt customers</param>
        void UpdateTaxExemptInformation(IConnectionManager entry, RecordIdentifier taxExemptTaxGroupID);

        void ClearCache();

        /// <summary>
        /// Resets the cache stored in the data provider.
        /// </summary>
        /// <param name="entry"></param>
        void RefreshCache(IConnectionManager entry);

        /// <summary>
        /// Saves the customer and the addresses assigned to the customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customer">The customer to be saved</param>
        /// <returns>The id of the customer</returns>
        RecordIdentifier SaveWithAddresses(IConnectionManager entry, Customer customer);

        /// <summary>
        /// Set the credit limit of a customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">The customer ID for which to update the credit limit</param>
        /// <param name="creditLimit">The credit limit to be set on the customer</param>
        void SetCustomerCreditLimit(IConnectionManager entry, RecordIdentifier customerID, decimal creditLimit);

        /// <summary>
        /// Get all information for a customer to be displayed in the customer panel of the POS
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="customerID">ID of the customer</param>
        /// <returns></returns>
        CustomerPanelInformation GetCustomerPanelInformation(IConnectionManager entry, RecordIdentifier customerID);
    }
}