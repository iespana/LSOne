using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.BusinessObjects.Invoice;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Represents the error when the client (POS or Site Manager) is not in sync with the server. This means that either the Site Service host or the local machine is
    /// out of sync in regards to date/time.
    /// </summary>
    public class ClientTimeNotSynchronizedException : Exception
    {
        public ClientTimeNotSynchronizedException()
        {
        }

        public ClientTimeNotSynchronizedException(string message)
            : base(message)
        {
        }

        public ClientTimeNotSynchronizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public partial interface ISiteServiceService : IService
    {
        #region Connections

        [Obsolete("The other Connect function with SiteServiceProfile parameter should be used. Only in very rare cases can this function be used, and you need to make sure that the SiteServiceAddress/port is correct on the entry")]
        void Connect(IConnectionManager entry);

        void Connect(IConnectionManager entry, SiteServiceProfile siteServiceProfile);

        void Disconnect(IConnectionManager entry);

        void SetAddress(SiteServiceProfile siteServiceProfile);

        void SetAddress(string address, ushort port);

        string GetExceptionDisplayText(Exception e);

        #endregion

        #region Connection link verification
        /// <summary>
        /// Tests the connection to the Site Service.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="host">The hostname for the Site Service</param>
        /// <param name="port">The port to use for connecting to the Site Service</param>
        /// <returns>The result of the connection test</returns>
        ConnectionEnum TestConnection(IConnectionManager entry, string host, ushort port);        

        /// <summary>
        /// Tests the connection to the Site Service and show a message to the user if the connection was not succesful.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="host">The hostname for the Site Service</param>
        /// <param name="port">The port to use for connecting to the Site Service</param>
        /// <param name="additionalMessage">An optional message that is appended in a new line below the standard message shown if the connection was not succesful</param>
        /// <returns>The result of the connection test</returns>
        ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, string host, ushort port, string additionalMessage = "");

        /// <summary>
        /// Tests the connection to the Site Service and show a message to the user if the connection was not succesful.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="siteServiceProfile">A populated <see cref="SiteServiceProfile"/> containing the connection information for the Site Service</param>
        /// <param name="additionalMessage">An optional message that is appended in a new line below the standard message shown if the connection was not succesful</param>
        /// <returns>The result of the connection test</returns>
        ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string additionalMessage = "");

        /// <summary>
        /// Tests the connection to the Site Service.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="siteServiceProfile">A populated <see cref="SiteServiceProfile"/> containing the connection information for the Site Service</param>
        /// <returns>The result of the connection test</returns>
        ConnectionEnum TestConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile);

		/// <summary>
		/// Validates an administrative password by returning an encrypted UNIX timestamp.
		/// </summary>
		/// <param name="entry">The connection to the database</param>
        /// <param name="host">The hostname for the Site Service</param>
        /// <param name="port">The port to use for connecting to the Site Service</param>
		/// <param name="administrativePassword"></param>
		/// <returns></returns>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
        string AdministrativeLogin(IConnectionManager entry, string host, ushort port, string administrativePassword);
		
		/// <summary>
		/// Returns the Site Service configurations from config file.
		/// </summary>
		/// <param name="entry">The connection to the database</param>
        /// <param name="host">The hostname for the Site Service</param>
        /// <param name="port">The port to use for connecting to the Site Service</param>
		/// <param name="administrativePassword">Authorization password set at install time for retrieving the settings.</param>
		/// <returns></returns>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="System.ServiceModel.FaultException">If timestamp is older than AdministrativeSessionTimeout (default 2 hours) returns <i>Administrative session expired</i></exception>
        Dictionary<string, string> LoadConfiguration(IConnectionManager entry, string host, ushort port, string administrativePassword);
		
		/// <summary>
		/// Updates Site Service configuratiosn from config file.
		/// </summary>
		/// <param name="entry">The connection to the database</param>
        /// <param name="host">The hostname for the Site Service</param>
        /// <param name="port">The port to use for connecting to the Site Service</param>
		/// <param name="administrativePassword">Authorization password set at install time for saving the passed settings.</param>
		/// <param name="fileConfigurations">List of settings to be saved in Site Service config file.</param>
		/// <exception cref="System.ServiceModel.FaultException">If administrativePassword is null or empty string or the timestamp is an invalid number or zero or if the provided password does not match the Site Service one returns <i>Provided administrative password is incorrect</i></exception>
		/// <exception cref="System.ServiceModel.FaultException">If timestamp is older than AdministrativeSessionTimeout (default 2 hours) returns <i>Administrative session expired</i></exception>
        void SendConfiguration(IConnectionManager entry, string host, ushort port, string administrativePassword, Dictionary<string, string> fileConfigurations);
        Dictionary<string, string> LoadIFConfiguration(IConnectionManager entry, WebserviceConfiguration config);
        void SendIFConfiguration(IConnectionManager entry, WebserviceConfiguration config, Dictionary<string, string> configurations);
        ServiceConnectionStatus TestIntegrationFrameworkConnection(IConnectionManager entry, bool checkTcp, bool checkHttp, WebserviceConfiguration config);

        #endregion

        #region Customer Orders

        RecordIdentifier SaveCustomerOrderDetails(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrder customerOrder);

        RecordIdentifier SaveCustomerOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, IRetailTransaction retailTransaction);

        void SaveCustomerOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrder order);

        RecordIdentifier CreateReferenceNo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerOrderType orderType);

        List<CustomerOrder> CustomerOrderSearch(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria,
            bool closeConnection = true
            );

        void GenerateCustomerOrders(IConnectionManager entry, SiteServiceProfile siteServiceProfile);

        #endregion

        #region Gift cards

        GiftCard GetGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection);
        List<GiftCardLine> GetGiftCardLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection);

        List<GiftCard> SearchGiftCards(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GiftCardFilter filter, out int itemCount, bool closeConnection);

        RecordIdentifier AddNewGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GiftCard giftCard, bool closeConnection, string prefix, int? numberSequenceLowest = null);

        void DeleteGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftcardID, bool closeConnection);

        bool ActivateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, RecordIdentifier transactionID, RecordIdentifier receiptID, bool closeConnection);

        bool MarkGiftCertificateIssued(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection);

        bool DeactivateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, bool closeConnection);

        decimal AddToGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, decimal amount, bool closeConnection);

        GiftCardValidationEnum ValidateGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier giftCardID, bool closeConnection);

        GiftCardValidationEnum UseGiftCard(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier giftCardID, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection);

        GiftCardValidationEnum UpdateGiftCardPaymentReceipt(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier giftCardID, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier receiptID, bool closeConnection);

        #endregion

        #region Credit Vouchers

        CreditVoucher GetCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID, bool closeConnection);
        List<CreditVoucherLine> GetCreditVoucherLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID,bool closeConnection);

        List<CreditVoucher> SearchCreditVouchers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CreditVoucherFilter filter, out int itemCount, bool closeConnection);

        RecordIdentifier IssueCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CreditVoucher voucher, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection);

        CreditVoucherValidationEnum ValidateCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier creditVoucherID, bool closeConnection);

        CreditVoucherValidationEnum UseCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref decimal amount, RecordIdentifier creditVoucherID, RecordIdentifier transactionId, RecordIdentifier receiptId, bool closeConnection);

        void DeleteCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditvoucherID,bool closeConnection);

        decimal AddToCreditVoucher(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier creditVoucherID, decimal amount, bool closeConnection);

        #endregion

        #region Central suspension

        RecordIdentifier SuspendTransaction(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier suspendedTransactionId,
            RecordIdentifier transactionTypeID,
            string xmlTransaction,
            decimal balance,
            decimal balanceWithTax,
            List<SuspendedTransactionAnswer> answers,
            bool closeConnection);

        List<SuspendedTransaction> GetSuspendedTransactionList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            Date dateFrom,
            Date dateTo,
            SuspendedTransaction.SortEnum sortEnum,
            bool sortBackwards,
            bool closeConnection);

        List<SuspendedTransaction> GetSuspendedTransactionListForStore(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID,
            bool closeConnection);

        SuspendedTransaction GetSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspendedTransactionID, bool closeConnection);

        List<SuspendedTransaction> GetAllSuspendedTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection);

        string RecallSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection);

        bool DeleteSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection);

        int GetSuspendedTransCount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeId, RecordIdentifier terminalId, RecordIdentifier suspensionTransactionTypeID, RetrieveSuspendedTransactions whatToRetrieve, bool closeConnection);

        List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswersByType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspensionTypeID, bool closeConnection);

        #endregion

        #region Hospitality

        TableInfo SaveHospitalityTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TableInfo table, bool closeConnection);
        void SaveUnlockedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid transactionID, bool closeConnection);
        bool ExistsUnlockedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid transactionID, bool closeConnection);
        void ClearTerminalLocks(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string terminalID, bool closeConnection);
        TableInfo LoadSpecificTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TableInfo table, bool closeConnection);
        List<TableInfo> LoadHospitalityTableState(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DiningTableLayout tableLayout, bool closeConnection);

        #endregion

        #region Retail items
        
        /// <summary>
        /// Retrieves information about a specific retail item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        RetailItem GetRetailItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Retrieves information about a specific retail item even if deleted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        RetailItem GetRetailItemIncludeDeleted(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection);

        /// <summary>
        /// Saves information about a specific retail item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="retailItem">The item to be saved</param>
        void SaveRetailItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RetailItem retailItem, bool closeConnection);

        /// <summary>
        /// Saves the unit conversion
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="unitConversion">The conversion rule</param>
        void SaveUnitConversionRule(IConnectionManager entry, SiteServiceProfile siteServiceProfile, UnitConversion unitConversion, bool closeConnection);

        /// <summary>
        /// Updates the type on a specific item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="newType">The new type</param>
        void SaveItemType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, ItemTypeEnum newType, bool closeConnection);

        /// <summary>
        /// Get the purchase price for an item and store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="storeID">ID of the store for which to retrieve the cost. Empty ID will return an average cost of all stores</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        RetailItemCost GetRetailItemCost(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

        /// <summary>
        /// Get a list purchase prices for an item, for each store including an average for all stores
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<RetailItemCost> GetRetailItemCostList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount, bool closeConnection);

        /// <summary>
        /// Insert a list of retail item costs
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemCosts">List of item costs to insert</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void InsertRetailItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RetailItemCost> itemCosts, bool closeConnection);

        /// <summary>
        /// Move all item costs to an archive table except the last calculated cost for each store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        void ArchiveItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        #endregion

        #region  Customers

        /// <summary>
        /// Checks if a customer exists with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="customerID">The ID of the customer to look for</param>
        /// <param name="useCentralDatabase">If true the Site Service is used to look up the ID, otherwise the local database is used</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        bool CustomerExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool useCentralDatabase, bool closeConnection);

        void ValidateCustomerStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref CustomerStatusValidationEnum valid, ref string comment, string customerId, ref decimal amount, string currencyCode, bool localDB, bool closeConnection = true);

        List<CustomerListItem> GetCustomers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string searchString, bool beginsWith, CustomerSorting sortOrder, bool sortBackwards, bool useCentralDatabase, bool closeConnection);

        Customer GetCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool useCentralDatabase, bool closeConnection);

        /// <summary>
        /// Save customer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="customer">The customer to be saved</param>
        /// <param name="useCentralDatabase">If true the Site Service is used to look up the ID, otherwise the local database is used</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Returns the customer being saved. This is useful when another the integration service provides more data than the one used to create the customer</returns>
        Customer SaveCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Customer customer, bool useCentralDatabase, bool closeConnection);

        void DeleteCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Customer customer, bool useCentralDatabase, bool closeConnection);

        void CustomersDiscountedPurchasesStatus(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            string customerID,
            out decimal maxDiscountedPurchases,
            out decimal currentPeriodDiscountedPurchases,
            bool closeConnection);

        XElement GetCustomerTransactionXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool useCentralCustomer, bool closeConnection = true);

        /// <summary>
        /// Set the credit limit of a customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="customerID">The customer ID for which to update the credit limit</param>
        /// <param name="creditLimit">The credit limit to be set on the customer</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void SetCustomerCreditLimit(IConnectionManager entry, SiteServiceProfile siteServiceProfile,  RecordIdentifier customerID, decimal creditLimit, bool closeConnection);

        /// <summary>
        /// Get all information for a customer to be displayed in the customer panel of the POS
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="customerID">ID of the customer</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        CustomerPanelInformation GetCustomerPanelInformation(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool closeConnection);

        #endregion

        #region Staff functions

        void StaffLogOn(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, string password);

        void StaffLogOff(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal);

        bool ChangePasswordForUser(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID, string newPasswordHash, bool needsPasswordChange, DateTime expiresDate, DateTime lastChangeTime);

        bool UserNeedsToChangePassword(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID);

        void LockUser(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID);

        void GetUserPasswordChangeInfo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userID, out string passwordHash, out DateTime expiresDate, out DateTime lastChangeTime);

        #endregion

        #region Sales Orders / Invoices

        //Moved to ISiteServiceService.SalesInvoices and ISiteServiceService.SalesOrders

        #endregion

        #region Customer ledger

        void UpdateRemainingAmount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true);

        List<CustomerLedgerEntries> GetCustomerLedgerEntriesList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier customerId,
            out int totalRecords,
            bool useCentralCustomer, 
            CustomerLedgerFilter filter,
            bool closeConnection = true
            );

        decimal GetCustomerTotalSales(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true);

        decimal GetCustomerBalance(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true);

        bool UpdateCustomerLedgerAtEOD(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier statementID, bool useCentralCustomer, bool closeConnection = false);

        void SaveCustomerLedgerEntries(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerLedgerEntries custLedgerEntries, bool useCentralCustomer, bool closeConnection = true);

        void DeleteCustomerLedgerEntry(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier ledgerEntryNo,
            bool useCentralCustomer, bool closeConnection = true);

        void CustomerAccountCreditMemo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true);

        void CustomerAccountPayment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true);

        void PaymentIntoCustomerAccount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true); 


        #endregion

        #region Customer loyalty

        int GetLoyaltyMSRCardTransCount(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier cardNumber, bool closeConnection = true);

        int GetCustomerLoyaltyMSRCardTransCount(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier cardNumber,
                                                bool closeConnection = true);

        LoyaltyMSRCard GetLoyaltyMSRCard(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier cardNumber, bool closeConnection = true);

        void DeleteLoyaltyMSRCard(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier cardID, bool closeConnection = true);

        LoyaltySchemes GetLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier schemeID, bool closeConnection);
        List<LoyaltySchemes> GetLoyaltySchemes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);
        List<LoyaltyPoints> GetLoyaltySchemeRules(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier loyaltySchemeId, bool closeConnection);
        LoyaltyPoints GetLoyaltySchemeRule(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier loyaltySchemeRuleId, bool closeConnection);
        LoyaltySchemes SaveLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile, LoyaltySchemes scheme, bool closeConnection);

        LoyaltySchemes SaveLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile, LoyaltySchemes scheme, RecordIdentifier copyRulesFrom,
                               bool closeConnection);

        void SaveLoyaltySchemeRule(IConnectionManager entry, SiteServiceProfile siteServiceProfile, LoyaltyPoints schemeRule, bool closeConnection);
        

        LoyaltyMSRCardTrans GetLoyaltyMSRCardTrans(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier loyMsrCardTransID, bool closeConnection = true);

        decimal GetMaxLoyaltyMSRCardTransLnNum(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, string cardNumber, bool closeConnection = true);
        
        LoyaltyPointStatus GetLoyaltyPointsStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, LoyaltyPointStatus pointStatus,  bool closeConnection = true);

        void UpdateIssuedLoyaltyPoints(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref LoyaltyCustomer.ErrorCodes valid, ref string comment, RecordIdentifier transactionId, decimal lineNum, string cardNumber,
                                       DateTime transDate, decimal loyaltyPoints, RecordIdentifier receiptId, bool closeConnection = true);
        
        LoyaltyPointStatus UpdateUsedLoyaltyPoints(IConnectionManager entry, SiteServiceProfile siteServiceProfile, LoyaltyPointStatus pointStatus,
                                     RecordIdentifier transactionId, decimal lineNum, DateTime transDate,
                                     decimal loyaltyPoints, RecordIdentifier receiptId, bool voidLoyaltyTrans,
                                     bool closeConnection = true);

        void UpdateLoyaltyCardCustomerID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref LoyaltyCustomer.ErrorCodes errorCode, ref string comment, RecordIdentifier LoyaltyCardID, RecordIdentifier CustomerID, bool closeConnection = true);
        List<LoyaltyMSRCard> GetCustomerMSRCards(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<DataEntity> customers, List<DataEntity> schemes, RecordIdentifier cardID, bool? hasCustomer, double? status, LoyaltyMSRCardInequality statusInequality, int tenderType, int fromRow, int toRow, LoyaltyMSRCardSorting sortBy, bool backwards, bool closeConnection);
        List<LoyaltyMSRCardTrans> GetLoyaltyTrans(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            string storeFilter,
            string terminalFilter,
            string msrCardFilter,
            string schemeFilter,
            int typeFilter,
            int openFilter,
            int entryTypeFilter,
            string customerFilter,
            string receiptID,
            Date dateFrom,
            Date dateTo,
            Date expiredateFrom,
            Date expiredateTo,
            int rowFrom,
            int rowTo,
            bool backwards,
            bool closeConnection);

        LoyaltyPoints GetPointsExchangeRate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier schemeID, bool closeConnection);

        RecordIdentifier SaveLoyaltyMSRCard(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, LoyaltyMSRCard loyaltyCard, bool closeConnection = true);

        void UpdateIssuedLoyaltyPointsForCustomer(IConnectionManager dataModel, SiteServiceProfile siteServiceProfile, RecordIdentifier loyaltyCardId,
                                                  RecordIdentifier customerId, bool closeConnection);

        void DeleteLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier schemeID, bool closeConnection);
        void DeleteLoyaltySchemeRule(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier schemeRuleID, bool closeConnection);

        void UpdateCouponCustomerLink(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier couponID, RecordIdentifier customerID, bool closeConnection);

        bool LoyaltyCardExistsForLoyaltyScheme(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier loyaltySchemeID,bool closeConnection);
        #endregion

        #region Inventory operations

        //All inventory functions moved to partial files i.e. IStoreServerService.Inventory.GoodsReceiving.cs and etc.

        #endregion

        #region Properties
        RecordIdentifier TerminalID
        {
            get;
            set;
        }

        RecordIdentifier StaffID
        {
            get;
            set;
        }
        #endregion

        #region EMail
        bool IsEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool useCentralDatbase);

        EMailSetting GetEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool useCentralDatabase);

        void SaveEMailSetupForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, EMailSetting setting, bool useCentralDatabase);

        void QueueEMailEntry(IConnectionManager entry, SiteServiceProfile siteServiceProfile, EMailQueueEntry emailQueue, List<EMailQueueAttachment> attachments, bool useCentralDatabase);

        void SendQueuedEMailEntries(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int maximumEntries, int maximumAttempts);

        int GetEMailCount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool unsentOnly);

        List<EMailQueueEntry> GetEMails(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool unsentOnly, int index, int maxEntries, EMailSortEnum sort);

        EMailQueueEntry GetEMail(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int ID);

        void TruncateEMailQueue(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DateTime createdBefore);

        #endregion

        #region Central Returns

        List<ReceiptListItem> GetTransactionListForReceiptID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier receiptID, RecordIdentifier terminalID, RecordIdentifier storeID, bool useCentralReturns, bool closeConnection = true);

        XElement GetTransactionXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool useCentralReturns, bool closeConnection = true);

        void MarkItemsReturned(IConnectionManager entry, SiteServiceProfile siteServiceProfile, IRetailTransaction transaction, bool useCentralReturns, bool closeConnection = true);

        #endregion

        #region Tax Refund

        void SaveTaxRefund(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TaxRefund refund, bool closeConnection = true);

        TaxRefund GetTaxRefund(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection = true);

        #endregion

        #region Cloud functionality

        void RegisterClient(IConnectionManager entry, string userName, string password, string dbname);

        bool ClientRegistered();

        void SetHardwareProfile(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, RecordIdentifier storeID, HardwareProfile profile);

        bool MarkAsActivated(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID);

        void SetTerminalEFT(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier terminalID, RecordIdentifier storeID,
            string ipAddress, string eftStoreID, string eftTerminalID, string efTcustomField1, string efTcustomField2);

        DateTime GetServerUTCDate(IConnectionManager entry, SiteServiceProfile siteServiceProfile);

        List<ScriptInfo> GetAvailableDefaultData(IConnectionManager entry, SiteServiceProfile siteServiceProfile);

        Guid RunDemoData(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ScriptInfo DemoDataName);

        bool IsTaskActive(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Guid taskGuid);

        string SiteServiceKey { get;  }

        #endregion
        
        #region Report functionality
        bool ReportExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reportID, bool closeConnection);
        ReportResult ReportRun(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ReportManifest manifest, bool closeConnection);

        #endregion

        #region Settings

        /// <summary>
        /// Returns the ID of the default tax store configured at the central database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        RecordIdentifier GetDefaultTaxStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        #endregion

        #region Terminal

        /// <summary>
        /// Saves settings for the current terminal
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="terminal">Current terminal</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        void SaveTerminaldData(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Terminal terminal, bool closeConnection);

        #endregion

        #region TimeKeeper
        void KeepTime(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TimeKept timeToKeep);

        TimeKept GetLastTimeKept(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier userGuid);
        #endregion

        void NotifyPlugin(IConnectionManager entry, SiteServiceProfile siteServiceProfile, MessageEventArgs e);
    }
}
