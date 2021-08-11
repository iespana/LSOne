using System;
using System.Collections.Generic;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkTransactionPlugin
{
    public partial class IntegrationFrameworkTransactionPlugin
    {
        public virtual IFRetailTransaction GetSaleTransaction(RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactions(dataModel, false, null, null, null,
                            (string) storeID, (string) terminalID, (string) transactionID, null);
                    return transactions[0];
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<IFRetailTransaction> GetSaleTransactionListForDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactions(dataModel, null, startDateTime,
                            endDateTime, null, storeID, null, null, null);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<IFRetailTransaction> GetSaleTransactionListForCustomerAndDatePeriodExcludingDefaultCustomers(RecordIdentifier customerID, Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactions(dataModel, true, startDateTime,
                            endDateTime, null, storeID, null, null, (string) customerID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<IFRetailTransaction> GetSaleTransactionListForDefaultCustomersAndDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactions(dataModel, false, startDateTime,
                            endDateTime, null, storeID, null, null, null);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual IFRetailTransaction GetSaleTransactionSumForDatePeriod(Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return TransactionProviders.PosTransactionData.GetSaleTransactionSumForDatePeriod(dataModel, false,
                        startDateTime, endDateTime, storeID);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual IFRetailTransaction GetSaleTransactionSumForDateAndDefaultCustomers(Date startDateTime = null, Date endDateTime = null, string storeID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return TransactionProviders.PosTransactionData.GetSaleTransactionSumForDatePeriod(dataModel, false,
                        startDateTime, endDateTime, storeID);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual List<IFRetailTransaction> GetSaleTransactionsFromReplicationCount(int replicationFrom)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactions(dataModel, null, null, null,
                            replicationFrom, null, null);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetRemoveTenderTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.RemoveTender, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetFloatEntryTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.FloatEntry, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetTenderDeclarationTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.TenderDeclaration, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetOpenDrawerTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.OpenDrawer, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetEndOfDayTransactions(Date startDateTime = null,
            Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.EndOfDay, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            } 
        }

        public List<IFRetailTransaction> GetEndOfShiftTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            IConnectionManager dataModel = GetConnectionManagerIF();

            try
            {
                List<IFRetailTransaction> transactions = TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel, TypeOfTransaction.EndOfShift, startDateTime, endDateTime, storeID, statementID);
                return transactions;
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
            }
        }

        public List<IFRetailTransaction> GetBankDropTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.BankDrop, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetBankDropReversalTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.BankDropReversal, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetSafeDropTransactions(Date startDateTime = null, Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.SafeDrop, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public List<IFRetailTransaction> GetSafeDropReversalTransactions(Date startDateTime = null,
            Date endDateTime = null, string storeID = null, string statementID = null)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                try
                {
                    List<IFRetailTransaction> transactions =
                        TransactionProviders.PosTransactionData.GetIFTransactionsByType(dataModel,
                            TypeOfTransaction.SafeDropReversal, startDateTime, endDateTime, storeID, statementID);
                    return transactions;
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }
    }
}
