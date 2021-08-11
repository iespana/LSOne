using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ISuspendedTransactionData : IDataProvider<SuspendedTransaction>, ISequenceable
    {
        int GetCount(
            IConnectionManager entry,
            RecordIdentifier storeId,
            RecordIdentifier terminalId,
            RecordIdentifier suspendedTransactionType,
            RetrieveSuspendedTransactions whatToRetrieve);

        List<SuspendedTransaction> GetList(
            IConnectionManager entry,
            SuspendedTransaction.SortEnum sortEnum, 
            bool sortBackwards);

        /// <summary>
        /// Get a list of suspended transactions for a given suspension transaction type and a certain store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="suspensionTransactionTypeID">The ID of the suspension transaction type </param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="sortEnum">Enum which decides in what order the result set is returned</param>
        /// <param name="sortBackwards">Whether to reverse the sorting of the result set</param>
        /// <param name="dateFrom">The from date of the transaction. Use Date.Empty to ignore</param>
        /// <param name="dateTo">The to date of the transaction. Use Date.Empty to ignore</param>
        /// <param name="terminalId"> The Id of the terminal</param>
        /// <returns></returns>
        List<SuspendedTransaction> GetListForTypeAndStore(
            IConnectionManager entry,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID,
            Date dateFrom,
            Date dateTo,
            SuspendedTransaction.SortEnum sortEnum, 
            bool sortBackwards,
            RecordIdentifier terminalId = null);

        /// <summary>
        /// Gets a list of all suspended transactions for a certain store and terminal over a certain period.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store to search for</param>
        /// <param name="terminalID">The terminal to search for. If this is RecordIdentifier.Empty then results for all terminals is given</param>
        /// <param name="fromDate">We get suspended transactions that are older then this date but younger then the toDate</param>
        /// <param name="toDate">We get suspended transactiosn that are newer then this date but older then the fromDate</param>
        /// <returns>list of all suspended transactions for a certain store and terminal</returns>
        List<SuspendedTransaction> SuspendedTransactionsForStoreAndTerminal(
            IConnectionManager entry, 
            RecordIdentifier storeID, 
            RecordIdentifier terminalID,
            DateTime fromDate,
            DateTime toDate);

        SuspendedTransaction Get(
            IConnectionManager entry,
            RecordIdentifier transactionID);
    }
}