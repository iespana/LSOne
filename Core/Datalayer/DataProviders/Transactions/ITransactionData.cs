using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ITransactionData : IDataProviderBase<Transaction>, ISequenceable
    {
        TypeOfTransaction GetTransactionType(IConnectionManager entry, RecordIdentifier transactionId,
            RecordIdentifier storeID, RecordIdentifier terminalID);

        /// <summary>
        /// Gets a list of transactions optimized for Journals, i.e. only the relavant fields are loaded into the Transaction Business Object. 
        /// </summary>
        /// <param name="entry">Connections parameters for the database</param>
        /// <param name="numberOfTransactions">The maximum number of transactions returned</param>
        /// <param name="terminalId">The terminal where those transactions were conducted.</param>
        /// <param name="fromDate">Search constraint, limits the results to; from that date(inclusive)</param>
        /// <param name="toDate">Search constraint, limits the results to; to that date(inclusive)</param>
        /// <param name="receipt">Search constraint, the set should only return the transaction with that receipt number</param>
        /// <param name="fromTransaction">Search constraint, returns transactions lower than the constrain.</param>
        /// <param name="storeId">The store the transactions were concluded in</param>
        /// <param name="type">Search constraint, the set should only return the transactions of this type</param>
        /// <param name="staff">Search constraint, the set should only return the transactions created by this staff</param>
        /// <param name="minAmount">Search constraint, the set should only return the transactions with gross amount greater than <paramref name="minAmount"/></param>
        /// <param name="maxAmount">Search constraint, the set should only return the transactions with gross amount greater than <paramref name="maxAmount"/></param>
        /// <returns>List of transactions optimized for journal</returns>
        List<Transaction> GetJournalTransactions(IConnectionManager entry, int numberOfTransactions, RecordIdentifier terminalId = null, 
            DateTime? fromDate = null, DateTime ?toDate = null, RecordIdentifier receipt = null, RecordIdentifier fromTransaction = null, RecordIdentifier storeId = null,
            TypeOfTransaction? type = null, string staff = null, decimal? minAmount = null, decimal? maxAmount = null);

        List<Transaction> GetSalesTransactionsForStatementID(IConnectionManager entry, RecordIdentifier statementID);
        List<Transaction> GetTransactionsForStatementID(IConnectionManager entry, RecordIdentifier statementID);
        Transaction GetRetailTransHeader(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, 
            RecordIdentifier terminalId);
        List<Transaction> GetTransactionsFromType(IConnectionManager entry, RecordIdentifier storeId, TypeOfTransaction transactionType, 
            bool sortBackwards);
        void PerformEOD(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, DateTime timeOfEOD);
        void MarkTransactionAsInventoryUpdated(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeID, 
            RecordIdentifier terminalID);

        /// <summary>
        /// Checks if an unconcluded transaction exists in POSISTRANSACTION table. If true then the POS was exited abnormally i.e a crash
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        bool UnconcludedTransactionExists(IConnectionManager entry);

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="zReportID">The unique ID of a Z report</param>
        /// <returns>
        /// If true then the Z report ID already exists in the transaction tables
        /// </returns>
        bool ZReportExists(IConnectionManager entry, RecordIdentifier zReportID);

        List<Transaction> GetSalesTransactions(IConnectionManager entry, RecordIdentifier storeId,
            RecordIdentifier terminalId, DateTime? periodStart = null, DateTime? periodEnd = null);

        //List<Transaction> GetSaleTransactionsFromReplicationCount(IConnectionManager entry, int replicationFrom);

        int GetNumberOfItemsForTransaction(IConnectionManager entry, RecordIdentifier id, bool excludeVoidedAndReturns);
        
        bool TransactionHasBeenReturnedOrContainsReturns(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier terminalID);

        /// <summary>
        /// Checks if an assembly item has been sold, Returns true if it has been sold, false if not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="assemblyID">ID of the assembly item you want to check for</param>
        /// <returns></returns>
        bool AssemblyItemIsActive(IConnectionManager entry, RecordIdentifier assemblyID); 

        Transaction GetLastRetailTransaction(IConnectionManager entry);

        Transaction GetLastTransaction(IConnectionManager entry);

        DateTime? UnpostedTransactionExists(IConnectionManager entry);
    }
}