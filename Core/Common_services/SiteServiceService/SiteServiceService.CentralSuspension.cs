
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        public virtual RecordIdentifier SuspendTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspendedTransactionId, RecordIdentifier transactionTypeID, string xmlTransaction, decimal balance, decimal balanceWithTax, List<SuspendedTransactionAnswer> answers, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SuspendTransaction(suspendedTransactionId, CreateLogonInfo(entry), transactionTypeID, xmlTransaction, balance, balanceWithTax, answers), closeConnection);

            return result;
        }

        public virtual List<SuspendedTransaction> GetSuspendedTransactionList(IConnectionManager entry,
                                                                      SiteServiceProfile siteServiceProfile,
                                                                      RecordIdentifier suspensionTransactionTypeID,
                                                                      RecordIdentifier storeID,
                                                                      RecordIdentifier terminalID,
                                                                      Date dateFrom,
                                                                      Date dateTo,
                                                                      SuspendedTransaction.SortEnum sortEnum,
                                                                      bool sortBackwards,
                                                                      bool closeConnection)
        {
            List<SuspendedTransaction> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransactionList(CreateLogonInfo(entry), suspensionTransactionTypeID, storeID, terminalID, dateFrom, dateTo, sortEnum, sortBackwards), closeConnection);

            return result;
        }

        public virtual List<SuspendedTransaction> GetSuspendedTransactionListForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspensionTransactionTypeID, RecordIdentifier storeID, bool closeConnection)
        {
            List<SuspendedTransaction> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransactionListForStore(CreateLogonInfo(entry), suspensionTransactionTypeID, storeID), closeConnection);

            return result;
        }

        public virtual List<SuspendedTransaction> GetAllSuspendedTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<SuspendedTransaction> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetAllSuspendedTransactions(CreateLogonInfo(entry)), closeConnection);

            return result;
        }
        
        public SuspendedTransaction GetSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspendedTransactionID,
            bool closeConnection)
        {
            SuspendedTransaction result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransaction(CreateLogonInfo(entry), suspendedTransactionID), closeConnection);

            return result;
        }

        public virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection)
        {
            List<SuspendedTransactionAnswer> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransactionAnswers(transactionID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual string RecallSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection)
        {
            string result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.RecallSuspendedTransaction(transactionID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual bool DeleteSuspendedTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteSuspendedTransaction(transactionID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual int GetSuspendedTransCount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeId, RecordIdentifier terminalId, RecordIdentifier suspensionTransactionTypeID, RetrieveSuspendedTransactions whatToRetrieve, bool closeConnection)
        {
            int result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransCount(storeId, terminalId, suspensionTransactionTypeID, whatToRetrieve, CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswersByType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier suspensionTypeID, bool closeConnection)
        {
            List<SuspendedTransactionAnswer> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSuspendedTransactionAnswersByType(suspensionTypeID, CreateLogonInfo(entry)), closeConnection);

            return result;
        }



    }
}
