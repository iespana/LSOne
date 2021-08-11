using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Implemented by Customer.cs. Used for customer control: customer database search, customer changes, adding or deleting a customer - instance.
    /// </summary>
    public interface ICustomerService : IService
    {
        

        /// <summary>
        /// Enter the customer id and add the customer to the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        IRetailTransaction EnterCustomerId(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Search for the customer and add him to the retail transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The retail tranaction</param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <param name="initialSearch">Initial text to search for at the beggining of the operation</param>
        /// <returns>The retail tranaction</returns>
        IPosTransaction Search(IConnectionManager entry, IPosTransaction posTransaction, bool returnNewCustomer, string initialSearch = "");

        /// <summary>
        /// Input a customer Id and return a posTransaction containing that customer if it exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session</param>
        /// <param name="posTransaction">The retail tranaction</param>
        /// <param name="customerID">The customer id to be retrieved</param>
        /// <returns>The retail tranaction</returns>
        IPosTransaction Get(IConnectionManager entry, ISession session, RecordIdentifier customerID, IPosTransaction posTransaction);

        /// <summary>
        /// Search for the customer and return the customer's information if one is selected otherwise the function returns null
        /// </summary>        
        /// <param name="entry">The entry into the database</param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <param name="transaction">Current transaction</param>
        /// <param name="initialSearch">Initial text to search for at the beggining of the operation</param>
        /// <returns>Customer's information</returns>
        Customer Search(IConnectionManager entry, bool returnNewCustomer, IPosTransaction transaction, string initialSearch = "");

        /// <summary>
        /// Search for a customer and return the customer ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="inputRequired">If true then the user has to select a customer.</param>
        /// <param name="customerName"></param>
        /// <param name="returnNewCustomer">If true then when the user creates a new customer through the Search dialog the new customer is returned directly. 
        /// If false then the customer search dialog will use the name of the customer to search again in the list</param>
        /// <param name="transaction">Current transaction</param>
        /// <returns>Customer ID as a string</returns>
        string Search(IConnectionManager entry, bool inputRequired, ref Name customerName, bool returnNewCustomer, IPosTransaction transaction);

        /// <summary>
        /// Sets the customer balance of the customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        IRetailTransaction Balance(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Adds a customer with a given ID to the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">ID of the customer to be added</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="displayErrorDialogs">True if you want error dialogs to pop up, else false (for server side runs then you want false)</param>
        /// <returns>A enum result code that hints if there were errors.</returns>
        AddCustomerResultEnum AddCustomerToTransaction(IConnectionManager entry, RecordIdentifier customerID, IPosTransaction posTransaction, bool displayErrorDialogs);

        /// <summary>
        /// Sets the customer status of the customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retail tranaction</param>
        /// <returns>The retail tranaction</returns>
        IRetailTransaction Status(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Register information about a new customer into the database. Information about the customer is provided with a dialog
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customer"></param>
        /// <returns>Returns true if operations is successful</returns>
        bool AddNewWithDialog(IConnectionManager entry, ref Customer customer);

        /// <summary>
        /// Update a customer in the database. Information about the customer is provided with a dialog
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customer"></param>
        /// <param name="transaction">Current transaction</param>
        /// <returns>Returns true if operations is successful</returns>
        bool EditWithDialog(IConnectionManager entry, Customer customer, IPosTransaction transaction);

        /// <summary>
        /// Update a customer in the database. Information about the customer is provided with a dialog
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID"></param>
        /// <param name="transaction">Current transaction</param>
        /// <returns></returns>
        bool EditWithDialog(IConnectionManager entry, RecordIdentifier customerID, IPosTransaction transaction);

        /// <summary>
        /// Updates the customer information in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerId">The customer id</param>
        /// <returns>Returns true if operations is successful</returns>
        bool Update(IConnectionManager entry, string customerId);

        /// <summary>
        /// Delete the customer from the database if the customer holds no customer transactions.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerId">The customer id</param>
        /// <returns>Returns true if operations is successful</returns>
        bool Delete(IConnectionManager entry, string customerId);                       

        /// <summary>
        /// Authentication of a charging to a customer account.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="valid"></param>
        /// <param name="comment"></param>
        /// <param name="manualAuthenticationCode"></param>
        /// <param name="customerId"></param>
        /// <param name="amount"></param>
        /// <param name="retailTransaction"></param>
        void AuthorizeCustomerAccountPayment(IConnectionManager entry, ref CustomerStatusValidationEnum valid, ref string comment, ref string manualAuthenticationCode, string customerId, ref decimal amount, IRetailTransaction retailTransaction);

        /// <summary>
        /// CustomerAccountCreditMemo
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerId"></param>
        /// <param name="receiptId"></param>
        /// <param name="currency"></param>
        /// <param name="currencyAmount"></param>
        /// <param name="amount"></param>
        /// <param name="currencyAmountDis"></param>
        /// <param name="amountDis"></param>
        /// <param name="storeId"></param>
        /// <param name="terminalId"></param>
        /// <param name="transactionId"></param>
        void CustomerAccountCreditMemo(IConnectionManager entry, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId);

        /// <summary>
        /// CustomerAccountPayment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerId"></param>
        /// <param name="receiptId"></param>
        /// <param name="currency"></param>
        /// <param name="currencyAmount"></param>
        /// <param name="amount"></param>
        /// <param name="amountDis"></param>
        /// <param name="storeId"></param>
        /// <param name="terminalId"></param>
        /// <param name="transactionId"></param>
        /// <param name="currencyAmountDis"></param>
        void CustomerAccountPayment(IConnectionManager entry, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId);

        /// <summary>
        /// Payment into customer account
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="transaction"></param>
        /// <param name="storeId"></param>
        /// <param name="terminalId"></param>
        void PaymentIntoCustomerAccount(IConnectionManager entry, ICustomerPaymentTransaction transaction, RecordIdentifier storeId, RecordIdentifier terminalId);

        /// <summary>
        /// Make a deposit to customer account
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="session"></param>
        /// <param name="transaction">Current <see cref="ICustomerPaymentTransaction"/> transaction</param>
        /// <param name="initialAmount">Initial deposit amount</param>
        void CustomerAccountDeposit(IConnectionManager entry, ISession session, ref IPosTransaction transaction, decimal initialAmount = 0);

        /// <summary>
        /// Determines if the given customer has gone over his discounted purchase limit when me buys the given transaction. 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerId">Id of the customer</param>
        /// <param name="retailTransaction">The retail transaction currently being processed</param>
        /// <param name="maxDiscountedPurchases">The maximum amount the customer is allowed to buy</param>
        /// <param name="currentDiscountedPurchases">The current amount the customer is allowed to buy</param>
        /// <returns></returns>
        bool CustomerHasGoneOverDiscountedPurchaseLimit(
            IConnectionManager entry,
            RecordIdentifier customerId,
            IRetailTransaction retailTransaction,
            out decimal maxDiscountedPurchases,
            out decimal currentDiscountedPurchases);        

        /// <summary>
        /// Returns true if the Site service is needed to conclude a transaction that includes customer payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        /// <returns></returns>
        bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Called from the operation Customer transaction report which is marked as a system operation
        /// This report is not implemented in the standard functionality but this operation can be changed to 
        /// be a POS Operation and the function implemented as a part of a customization.
        /// The only limitation the POS core sets on this operation is that the user has to have the CustomerTransactionReport permission
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        void CustomerTransactionReport(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Called from the operation Customer balance report which is marked as a system operation
        /// This report is not implemented in the standard functionality but this operation can be changed to 
        /// be a POS Operation and the function implemented as a part of a customization.
        /// The only limitation the POS core sets on this operation is that the user has to have the CustomerTransactionsReport permission
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        void CustomerBalanceReport(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Called from the operation Customer transactions which is marked as a system operation
        /// This report is not implemented in the standard functionality but this operation can be changed to 
        /// be a POS Operation and the function implemented as a part of a customization.
        /// The only limitation the POS core sets on this operation is that the user has to have the CustomerTransactions permission
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The current transaction</param>
        void Transactions(IConnectionManager entry, IPosTransaction transaction);
    }
}
