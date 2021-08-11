using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual SuspendedTransaction GetSuspendedTransaction(LogonInfo logonInfo, RecordIdentifier suspendedTransactionID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(suspendedTransactionID)}: {suspendedTransactionID}", LogLevel.Trace);
                return Providers.SuspendedTransactionData.Get(dataModel, suspendedTransactionID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }

        public virtual List<SuspendedTransaction> GetAllSuspendedTransactions(LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, "GetAllSuspendedTransactions", LogLevel.Trace);
                return Providers.SuspendedTransactionData.GetList(dataModel, SuspendedTransaction.SortEnum.TransactionID, false);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }

        public virtual RecordIdentifier SuspendTransaction(
            RecordIdentifier suspendedTransactionId,
            LogonInfo logonInfo,
            RecordIdentifier transactionTypeID,
            string xmlTransaction,
            decimal balance,
            decimal balanceWithTax,
            List<SuspendedTransactionAnswer> answers)
        {
            var overWritePermissions = new HashSet<string>
            {
                Permission.ManageCentralSuspensions
            };
            var contextGuid = Guid.NewGuid();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(suspendedTransactionId)}: {suspendedTransactionId}, {nameof(transactionTypeID)}: {transactionTypeID}", LogLevel.Trace);

                var suspendedTransactionType = Providers.SuspendedTransactionTypeData.Get(dataModel, transactionTypeID);

                var transactionToSuspend = new SuspendedTransaction
                {
                    ID = suspendedTransactionId,
                    AllowStatementPosting = suspendedTransactionType.EndofDayCode,
                    Text = suspendedTransactionType.Text,
                    StaffID = logonInfo.StaffID,
                    StoreID = logonInfo.storeId,
                    TerminalID = logonInfo.terminalId,
                    SuspensionTypeID = suspendedTransactionType.ID,
                    TransactionDate = DateTime.Now,
                    TransactionXML = xmlTransaction,
                    Balance = balance,
                    BalanceWithTax = balanceWithTax
                };

                Providers.SuspendedTransactionData.Save(dataModel, transactionToSuspend);

                Utils.Log(this, "Suspended transaction saved", LogLevel.Trace);

                //If answers were entered then save them
                if (answers != null)
                {
                    foreach (var answer in answers)
                    {
                        answer.TransactionID = transactionToSuspend.ID;
                        Providers.SuspendedTransactionAnswerData.Save(dataModel, answer);
                    }
                    Utils.Log(this, "Suspended answers saved", LogLevel.Trace);
                }

                return transactionToSuspend.ID;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return RecordIdentifier.Empty;
        }

        public virtual int GetSuspendedTransCount(RecordIdentifier storeId, RecordIdentifier terminalId,
            RecordIdentifier suspensionTransactionTypeID, RetrieveSuspendedTransactions whatToRetreive,
            LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeId)}: {storeId}, {nameof(terminalId)}: {terminalId}, {nameof(suspensionTransactionTypeID)}: {suspensionTransactionTypeID}", LogLevel.Trace);

                return Providers.SuspendedTransactionData.GetCount(dataModel, storeId, terminalId, suspensionTransactionTypeID, whatToRetreive);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return 0;
        }

        public virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswers(RecordIdentifier transactionID,
            LogonInfo logonInfo)
        {
            var overWritePermissions = new HashSet<string>
            {
                Permission.ManageCentralSuspensions
            };
            var contextGuid = Guid.NewGuid();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}", LogLevel.Trace);

                dataModel.BeginPermissionOverride(contextGuid, overWritePermissions);
                return Providers.SuspendedTransactionAnswerData.GetList(dataModel, transactionID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }

        public virtual List<SuspendedTransaction> GetSuspendedTransactionList(
            LogonInfo logonInfo,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            Date dateFrom,
            Date dateTo,
            SuspendedTransaction.SortEnum sortEnum,
            bool sortBackwards)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}, {nameof(suspensionTransactionTypeID)}: {suspensionTransactionTypeID}", LogLevel.Trace);
                return Providers.SuspendedTransactionData.GetListForTypeAndStore(
                    dataModel,
                    suspensionTransactionTypeID,
                    storeID,
                    dateFrom,
                    dateTo,
                    sortEnum,
                    sortBackwards,
                    terminalID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
            return null;
        }

        public virtual List<SuspendedTransaction> GetSuspendedTransactionListForStore(
            LogonInfo logonInfo,
            RecordIdentifier suspensionTransactionTypeID,
            RecordIdentifier storeID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}, {nameof(suspensionTransactionTypeID)}: {suspensionTransactionTypeID}", LogLevel.Trace);
                return Providers.SuspendedTransactionData.GetListForTypeAndStore(
                    dataModel,
                    suspensionTransactionTypeID,
                    storeID,
                    Date.Empty,
                    Date.Empty,
                    SuspendedTransaction.SortEnum.TransactionDate,
                    true);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);

            }

            return null;
        }

        public string RecallSuspendedTransaction(RecordIdentifier transactionID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}", LogLevel.Trace);
                var suspendedTransaction = Providers.SuspendedTransactionData.Get(dataModel, transactionID);

                return suspendedTransaction.TransactionXML;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;

        }

        public virtual bool DeleteSuspendedTransaction(RecordIdentifier transactionID, LogonInfo logonInfo)
        {
            var overWritePermissions = new HashSet<string>
            {
                Permission.ManageCentralSuspensions
            };
            var contextGuid = Guid.NewGuid();

            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}", LogLevel.Trace);
                List<SuspendedTransactionAnswer> answers = Providers.SuspendedTransactionAnswerData.GetList(dataModel, transactionID);
                Providers.SuspendedTransactionData.Delete(dataModel, transactionID);

                foreach (SuspendedTransactionAnswer answer in answers)
                {
                    Providers.SuspendedTransactionAnswerData.Delete(dataModel, answer.ID);
                }

                Utils.Log(this, "Answers deleted", LogLevel.Trace); 

                return true;
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                return false;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual List<SuspendedTransactionAnswer> GetSuspendedTransactionAnswersByType(RecordIdentifier suspensionTypeID,
            LogonInfo logonInfo)
        {
            var overWritePermissions = new HashSet<string>
            {
                Permission.ManageCentralSuspensions
            };
            var contextGuid = Guid.NewGuid();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(suspensionTypeID)}: {suspensionTypeID}", LogLevel.Trace);
                return Providers.SuspendedTransactionAnswerData.GetListForSuspensionType(dataModel, suspensionTypeID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);

                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }

            return null;
        }
    }
}