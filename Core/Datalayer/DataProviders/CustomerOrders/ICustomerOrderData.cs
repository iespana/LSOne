using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.CustomerOrders
{
    public interface ICustomerOrderData : IDataProvider<CustomerOrder>, ISequenceable
    {
        /// <summary>
        /// Generates a reference number for a customer order. If no number sequence is selected in the settings object then
        /// a default number sequence is used
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderSettings">The settings for the customer order or quote.</param>
        /// <returns></returns>
        RecordIdentifier GenerateReference(IConnectionManager entry, CustomerOrderSettings orderSettings);

        /// <summary>
        /// Returns a specific customer order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ID">The unique ID of the customer order to retrieve</param>
        /// <returns>A customer order object</returns>
        CustomerOrder Get(IConnectionManager entry, RecordIdentifier ID);

        /// <summary>
        /// Returns a list of all existing customer orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of customer orders</returns>
        List<CustomerOrder> GetList(IConnectionManager entry);

        /// <summary>
        /// Searches for customer orders that match the search parameters set in the <see cref="CustomerOrderSearch"/> object
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="totalRecordsMatching">How many records match the search criteria</param>
        /// <param name="numberOfRecordsToReturn">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search by</param>
        /// <returns></returns>
        List<CustomerOrder> AdvancedSearch(IConnectionManager entry,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria);

        /// <summary>
        /// Saves the customer order to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The customer order to be saved</param>
        /// <param name="excludeOrderXML">If true then the orderXML value is not saved</param>
        void Save(IConnectionManager entry, CustomerOrder item, bool excludeOrderXML);
    }
}
