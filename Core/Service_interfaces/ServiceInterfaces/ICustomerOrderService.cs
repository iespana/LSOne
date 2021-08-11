using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.Services.Interfaces
{
    public interface ICustomerOrderService : IService
    {
        /// <summary>
        /// Checks that all configurations on the current store that are necessary for the customer orders functionality is set 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderType">The <see cref="CustomerOrderType"/> that is being checked </param>
        /// <returns>False if configurations don't exists or are not set otherwise true</returns>
        bool ConfigurationsExistForStore(IConnectionManager entry, CustomerOrderType orderType);
        bool CanBeCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType);

        /// <summary>
        /// Creates a new transaction and adds the sale lines and tender lines. 
        /// If the sale lines have an empty Order variable then it is filled out with default values. 
        /// The transaction is then saved as a customer order with status Closed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The ID of the customer that should be added to the transaction</param>
        /// <param name="saleLines">The item lines to be added to the new customer order</param>
        /// <param name="tenderLines">The tender lines to be added to the new customer order. All tender lines will be marked as paid</param>
        /// <param name="orderItem">Information about the customer order itself that should be added to the transaction</param>
        void CreateCustomerOrder(IConnectionManager entry, RecordIdentifier customerID, LinkedList<ISaleLineItem> saleLines, List<ITenderLineItem> tenderLines, CustomerOrderItem orderItem);

        /// <summary>
        /// Creates a new customer order and attaches is to the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of order is this <see cref="CustomerOrderType"/></param>
        /// <param name="updateStock">If true then saving the customer order will affect the stock numbers of the item</param>
        void CreateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType, bool updateStock);

        /// <summary>
        /// Creates a new customer order and attaches is to the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of order is this <see cref="CustomerOrderType"/></param>
        /// <param name="updateStock">If true then saving the customer order will affect the stock numbers of the item</param>
        /// <param name="action">Defines what action should be taken when the customer order has been created</param>
        void CreateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType, CustomerOrderAction action, bool updateStock);

        /// <summary>
        /// Displays a dialog that has a list of customer orders that can be recalled and managed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of order is this <see cref="CustomerOrderType"/></param>
        /// <returns>The customer order that was selected to be recalled</returns>
        IRetailTransaction RecallCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType);

        void CalculateMinDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType);

        void DistributeMinDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType);

        /// <summary>
        /// After payment has been done and before the customer order is saved this function updates all deposit variables on both the transaction and each item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        /// <param name="summaries">If All then all deposit information is updated otherwise only for items on order or to pick up</param>
        /// <returns></returns>
        bool UpdateDepositInformation(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderSummaries summaries);

        /// <summary>
        /// After each POS operation and before "Customer order actions" are displayed to the user, information about any more deposits that could possibly be necessary are updated
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        /// <returns></returns>
        void UpdatePaymentInformation(IConnectionManager entry, IRetailTransaction retailTransaction);


        /// <summary>
        /// Sets the status of the customer order and then saves it centrally
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        /// <param name="status">The new status for the customer order</param>
        /// <returns>the unique ID of the Customer order - a GUID</returns>
        RecordIdentifier SaveCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderStatus status);
        
        /// <summary>
        /// Saves the customer order centrally. If the customer order has status New then it is changed to Open before being saved
        /// Current action is also set to None before saving.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        /// <returns>the unique ID of the Customer order - a GUID</returns>
        /// <param name="updateStock"></param>
        RecordIdentifier SaveCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, bool updateStock = true);

        /// <summary>
        /// Saves the customer order centrally. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="order">The order object that is to be saved</param>
        void SaveCustomerOrder(IConnectionManager entry, CustomerOrder order);

        /// <summary>
        /// Saves the customer order centrally without saving the order XML. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="order">The order object that is to be saved</param>
        void SaveCustomerOrderDetails(IConnectionManager entry, CustomerOrder order);

        /// <summary>
        /// Calculates the amount that needs to be paid including items that are being picked and any deposits necessary
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        /// <param name="includeLastTenderLine"></param>
        /// <returns></returns>
        //decimal CalculateAmountToBePaid(IConnectionManager entry, IRetailTransaction retailTransaction);
        decimal CalculateAmountToBePaid(IConnectionManager entry, IRetailTransaction retailTransaction, bool includeLastTenderLine);

        /// <summary>
        /// Calculates the amount that needs to be paid including items that are being picked and any deposits necessary
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param> 
        /// <param name="includeLastTenderLine"></param>
        /// <returns></returns>
        //decimal CalculateAmountToBeTendered(IConnectionManager entry, IRetailTransaction retailTransaction);
        decimal CalculateAmountToBeTendered(IConnectionManager entry, IRetailTransaction retailTransaction, bool includeLastTenderLine);

        /// <summary>
        /// Updates the customer order. The action parameter determines how the customer order is udpated, saved or printed.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="action">Defines what action should be taken when the customer order has been created</param>
        void UpdateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderAction action);

        /// <summary>
        /// Marks all remaining items that have not been fully or partially picked up for pickup
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        void MarkAllRemainingItemsForPickup(IConnectionManager entry, IRetailTransaction retailTransaction);

        CustomerOrderAction CustomerOrderActions(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// After the payment has been finalized the transaction needs to be concluded. This creates a DepositTransaction, new RetailTransaction when needed
        /// and saves the Customer order to the HO through the Site Service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transctions</param>
        /// <returns></returns>
        IPosTransaction ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction);

        List<CustomerOrder> Search(IConnectionManager entry,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria,
            bool closeConnection = true
            );

        bool ItemCanBeVoided(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item);

        void VoidItemDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item);

        void ConfirmDepositAmountToReturn(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// This function is called when operation Void transaction is clicked in the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transctions</param>
        void VoidCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction);

        void PrintCustomerOrderInformation(IConnectionManager entry, IRetailTransaction retailTransaction, bool copyReceipt);

        void PrintCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, FormSystemType formType, bool copyReceipt);

        void GenerateCustomerOrders(IConnectionManager entry);

        void UpdateStockInformation(IConnectionManager entry, IRetailTransaction retailTransaction);
    }
}
