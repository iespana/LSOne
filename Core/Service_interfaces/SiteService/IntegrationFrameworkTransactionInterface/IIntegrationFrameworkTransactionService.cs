using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSRetail.SiteService.IntegrationFrameworkTransactionInterface
{
    //TODO: SessionMode should be required. Changed to Allowed for http binding
    [ServiceContract(SessionMode = SessionMode.Allowed, Namespace = "LSRetail.IntegrationFramework")]
    public partial interface IIntegrationFrameworkTransactionService
    {
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Gets a single <see cref="RetailTransaction"/> from the database for the given <paramref name="transactionID"/>
        /// </summary>
        /// <param name="transactionID">The ID of the transaction to get. </param>
        /// <param name="storeID">The store the transaction was concluded in</param>
        /// <param name="terminalID">The terminal the transaction was concluded on</param>
        /// <returns></returns>
        [OperationContract]
        IFRetailTransaction GetSaleTransaction(RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database for a specific date period <paramref name="startDateTime"/> and <paramref name="endDateTime"/> and optionally a specific <paramref name="storeID"/>.
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetSaleTransactionListForDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database for the given <paramref name="customerID"/>, <paramref name="startDateTime"/> and <paramref name="endDateTime"/> and <paramref name="storeID"/>, excluding default customers. <paramref name="storeID"/> is optional.
        /// </summary>
        /// <param name="customerID">ID of the customer selected for the transactions</param>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetSaleTransactionListForCustomerAndDatePeriodExcludingDefaultCustomers(RecordIdentifier customerID, Date startDateTime = null, Date endDateTime = null, string storeID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database for the given <paramref name="startDateTime"/> and <paramref name="endDateTime"/> and <paramref name="storeID"/>. <paramref name="storeID"/> is optional.
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetSaleTransactionListForDefaultCustomersAndDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null);


        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database for the given <paramref name="startDateTime"/> and <paramref name="endDateTime"/> and <paramref name="storeID"/> and sums in to one <see cref="RetailTransaction"/> object. <paramref name="storeID"/> is optional.
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <returns></returns>
        [OperationContract]
        IFRetailTransaction GetSaleTransactionSumForDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database for <paramref name="startDateTime"/> and <paramref name="endDateTime"/> and <paramref name="storeID"/> and sums in to one <see cref="RetailTransaction"/> object. <paramref name="storeID"/> is optional.
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <returns></returns>
        [OperationContract]
        IFRetailTransaction GetSaleTransactionSumForDateAndDefaultCustomers(Date startDateTime = null, Date endDateTime = null, string storeID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database from a specific replication count
        /// </summary>
        /// <param name="replicationFrom"></param>
        [OperationContract]
        List<IFRetailTransaction> GetSaleTransactionsFromReplicationCount(int replicationFrom);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type remove tender
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetRemoveTenderTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type float entry
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetFloatEntryTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type tender declaration
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetTenderDeclarationTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type open drawer
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetOpenDrawerTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type end of day
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetEndOfDayTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type remove tender
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetEndOfShiftTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type bank drop
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetBankDropTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type bank drop reversal
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetBankDropReversalTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type safe drop
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetSafeDropTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);

        /// <summary>
        /// Retrieves a list of <see cref="RetailTransaction"/> from the database of type safe drop reversal
        /// </summary>
        /// <param name="startDateTime">Sets the starting date and time for the search criteria</param>
        /// <param name="endDateTime">Sets the end date and time for the search criteria</param>
        /// <param name="storeID">The store the transactions were concluded in</param>
        /// <param name="statementID">The statement id of the transaction</param>
        /// <returns></returns>
        [OperationContract]
        List<IFRetailTransaction> GetSafeDropReversalTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);
    }
}
