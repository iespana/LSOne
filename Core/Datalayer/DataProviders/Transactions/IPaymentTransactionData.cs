using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface IPaymentTransactionData : IDataProviderBase<PaymentTransaction>
    {
        List<PaymentTransaction> GetAll(IConnectionManager entry, RecordIdentifier transactionId, 
            CacheType cacheType = CacheType.CacheTypeNone);
        List<PaymentTransaction> GetForStatement(IConnectionManager entry, RecordIdentifier statementId, 
            RecordIdentifier storeId, string currency, string tenderType);
        DateTime GetNextCountingDate(IConnectionManager entry, RecordIdentifier transactionType, RecordIdentifier terminalID, 
            RecordIdentifier storeID);
        List<PaymentTransaction> GetRequiredDropAmounts(IConnectionManager entry, DateTime startDate, RecordIdentifier terminalID, 
            RecordIdentifier storeID);

        /// <summary>
        /// Gets all payment lines included in the given statement by store, terminal, currency and tender type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="statementId">The ID of the statement</param>
        /// <param name="storeId">The ID of the store</param>
        /// <param name="terminalId">The ID of the terminal</param>
        /// <param name="currency">The currency used for payment</param>
        /// <param name="tenderType">The payment type</param>
        /// <returns></returns>
        List<PaymentTransaction> GetForStatementAndTerminal(IConnectionManager entry, RecordIdentifier statementId, RecordIdentifier storeId, RecordIdentifier terminalId, string currency, string tenderType);

        /// <summary>
        /// Get all payment lines for a given store over a given period
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeId">ID of the store</param>
        /// <param name="from">Period start point</param>
        /// <param name="to">Period end point</param>
        /// <param name="dateFilterType">Which column to use for period</param>
        List<PaymentTransaction> GetForStoreAndPeriod(IConnectionManager entry, string storeId, DateTime from, DateTime to, DateFilterTypeEnum dateFilterType);
    }
}