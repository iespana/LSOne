using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkCustomerInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace ="LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkCustomerService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Saves a single <see cref="Customer"/> to the database. If it does not exists it will be created.
        /// </summary>        
        /// <param name="customer">The customer to save</param>
        [OperationContract]
        void Save(Customer customer);

        /// <summary>
        /// Saves a list of <see cref="Customer"/> objects to the database. Customers that do not exist will be created.
        /// </summary>        
        /// <param name="customers">The list of customers to save</param>     
        [OperationContract]
        SaveResult SaveList(List<Customer> customers);

        /// <summary>
        /// Gets a single <see cref="Customer"/> from the database for the given <paramref name="customerID"/>
        /// </summary>        
        /// <param name="customerID">The ID of the customer to get. </param>
        /// <returns></returns>        
        [OperationContract]
        Customer Get(RecordIdentifier customerID);

        /// <summary>
        /// Deletes the customer with the given <paramref name="customerID"/> from the database
        /// </summary>        
        /// <param name="customerID">The ID of the item to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier customerID);

        /// <summary>
        /// Save a sales price for a specific item, customer, item group, customer groups or all
        /// </summary>        
        /// <param name="salesPrice">Sale price information to save</param>
        [OperationContract]
        void SavePrice(IFSalesPrice salesPrice);

        /// <summary>
        /// Save a line discount for a specific item, customer, item group, customer group or all
        /// </summary>
        /// <param name="discount">Discount information to save</param>
        [OperationContract]
        void SaveCustomerLineDiscount(IFDiscount discount);

        /// <summary>
        /// Save a total discount for a customer, customer group or all
        /// </summary>        
        /// <param name="discount">Discount information to save</param>
        [OperationContract]
        void SaveCustomerTotalDiscount(IFDiscount discount);

        /// <summary>
        /// Save a price discount group
        /// </summary>        
        /// <param name="group">The group info to save</param>
        [OperationContract]
        void SavePriceDiscountGroup(PriceDiscountGroup group);

        /// <summary>
        /// Add items to a price discount group
        /// </summary>        
        /// <param name="groupID">The group ID to which to assign the items</param>
        /// <param name="itemIDs">Te item IDs to assign</param>
        [OperationContract]
        void AddItemsToLineDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> itemIDs);

        /// <summary>
        /// Assign customers to a line discount group
        /// </summary>        
        /// <param name="groupID">The group ID to which to assign the customers</param>
        /// <param name="customerIDs">Customer IDs to assign</param>
        [OperationContract]
        void AddCustomersToLineDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> customerIDs);

        /// <summary>
        /// Assign customers to a total discount group
        /// </summary>        
        /// <param name="groupID">The group ID to which to assign the customers</param>
        /// <param name="customerIDs">Customer IDs to assign</param>
        [OperationContract]
        void AddCustomersToTotalDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> customerIDs);
    }
}
