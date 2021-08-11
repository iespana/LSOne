using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IPosTransactionData : IDataProviderBase<PosTransaction>, ISequenceable
    {

        List<PosTransaction> GetTransactionsForIntegrationService(IConnectionManager entry,
            List<TypeOfTransaction> types,
            List<TransactionStatus> statuses,            
            List<RecordIdentifier> storeIDs,
            List<RecordIdentifier> terminalIDs,            
            DateTime? fromDate, 
            DateTime? toDate,
            bool includePreviouslyExported,
            List<RecordIdentifier> transactionIDs = null);

        List<IFRetailTransaction> GetIFTransactions(IConnectionManager entry, bool? onlyForDefaultCustomers = null, Date startDateTime = null, Date endDateTime = null, int? replicationFrom = null, string storeID = null, string terminalID = null, string transactionID = null, string customerID = null);

        List<IFRetailTransaction> GetIFTransactionsByType(IConnectionManager entry, TypeOfTransaction type, Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null);
        void GetTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, PosTransaction transaction, bool taxIncludedInPrice);
        RecordIdentifier GetTransactionID(IConnectionManager entry, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID);
        TypeOfTransaction GetTransactionType(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID);
        bool Exists(IConnectionManager entry, IPosTransaction transaction, bool ignoreTransactionType = true);
        void Save(IConnectionManager entry, ISettings settings, PosTransaction transaction);

        List<ReceiptListItem> GetGetTransactionsForReceiptID(IConnectionManager entry, RecordIdentifier receiptID, RecordIdentifier storeID, RecordIdentifier terminalID, bool includeCanceldRefunds);

        IFRetailTransaction GetSaleTransactionSumForDatePeriod(IConnectionManager entry, bool forDefaultCustomers, Date startDateTime = null, Date endDateTime = null, string storeID = null);
    }
}