using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Statements
{
    public interface IStatementInfoData : IDataProviderBase<StatementInfo>, ISequenceable
    {
        void Delete(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID);
        void ClearCalculatedUnpostedStatement(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID);
        List<StatementInfo> GetStatements(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum, 
            bool sortBackwards, StatementTypeEnum statementType);
        List<StatementInfo> GetAllStatements(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum, 
            bool sortBackwards);
        List<StatementInfo> GetStatementsForPeriod(IConnectionManager entry, RecordIdentifier storeID, StatementInfoSorting sortEnum,
            bool sortBackwards, StatementTypeEnum statementType, DateTime startTime, DateTime endTime);
        List<StatementInfo> GetUnprocessedERPStatementsFromAllStores(IConnectionManager entry, StatementTypeEnum statementType);

        /// <summary>
        /// Updates the calculated and calculatedTime properties of a StatementInfo object
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="statementInfo">The StatementInfo object to update</param>
        void UpdateCalculatedInfo(IConnectionManager entry, StatementInfo statementInfo);

        void UpdatePostedInfo(IConnectionManager entry, StatementInfo statementInfo);
        List<StatementTransaction> GetTransactionsWithoutStatementIDs(IConnectionManager entry, DateTime startTime, 
            DateTime endTime, RecordIdentifier storeID);

        /// <summary>
        /// Marks transactions that belong to the given statement
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="startTime">Transactions from startTime to endTime are considered</param>
        /// <param name="endTime">Transactions from startTime to endTime are considered</param>
        /// <param name="storeID">Transactions from this store are considered</param>
        /// <param name="statementID">The transactions are marked with this statement ID</param>
        void MarkStatementTransactions(IConnectionManager entry, DateTime startTime, DateTime endTime, RecordIdentifier storeID, 
            RecordIdentifier statementID);

        void ERPProcessStatement(IConnectionManager entry, RecordIdentifier statementID);
        void Save(IConnectionManager entry, StatementInfo statementInfo);
        StatementInfo Get(IConnectionManager entry, RecordIdentifier statementID);

        /// <summary>
        /// Gets list of posted statement headers
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">StoreID to filter by</param>
        /// <param name="startTime">Start time to filer by</param>
        /// <param name="endTime">End time to filter by</param>
        /// <returns>List of posted statement headers by the given filtering criteria</returns>
        List<StatementHeader> GetPostedStatementHeaders(IConnectionManager entry, RecordIdentifier storeID, DateTime startTime, DateTime endTime);

        List<StatementCountInfo> GetPostedStatementCount(IConnectionManager entry, DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets a list of posted statements created after a date, optionally filtered by a store.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="date">Start time to filter by</param>
        /// <param name="storeID">Store ID to filter by</param>
        /// <returns>List of posted statements</returns>
        List<StatementInfo> GetPostedStatementsAfterDate(IConnectionManager entry, DateTime date, RecordIdentifier storeID = null);

        /// <summary>
        /// Get the last statement created for a store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">Store ID</param>
        /// <returns>Statement</returns>
        StatementInfo GetLastStatement(IConnectionManager entry, RecordIdentifier storeID);
    }
}