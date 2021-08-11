using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.SiteService.Utilities;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual bool CustomerExists(RecordIdentifier customerID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerID)}: {customerID}", LogLevel.Trace);
                return Providers.CustomerData.Exists(dataModel, customerID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return false;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual List<CustomerListItem> GetCustomers(string searchString, bool beginsWith, CustomerSorting sortOrder, bool sortBackwards, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);
                return Providers.CustomerData.GetList(dataModel, searchString, sortOrder, sortBackwards, beginsWith);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual Customer GetCustomer(RecordIdentifier customerID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerID)}: {customerID}", LogLevel.Trace);
                return Providers.CustomerData.Get(dataModel, customerID, UsageIntentEnum.Normal);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        /// <summary>
        /// Save customer
        /// </summary>
        /// <param name="customer">The customer to be saved</param>
        /// <param name="logonInfo">Login credentials</param>
        /// <returns>Returns the customer being saved. This is useful when another the integration service provides more data than the one used to create the customer</returns>
        public virtual Customer SaveCustomer(Customer customer, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);
                Providers.CustomerData.SaveWithAddresses(dataModel, customer);
                return customer;
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

            return customer;
        }

        public virtual void DeleteCustomer(Customer customer, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customer)}.ID: {customer.ID}", LogLevel.Trace);
                Providers.CustomerData.Delete(dataModel, customer.ID);
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
        }

        public virtual void SetCustomerCreditLimit(RecordIdentifier customerID, decimal creditLimit, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerID)}: {customerID}", LogLevel.Trace);
                Providers.CustomerData.SetCustomerCreditLimit(dataModel, customerID, creditLimit);
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
        }

        public virtual void CustomersDiscountedPurchasesStatus(
            string customerID,
            LogonInfo logonInfo,
            out decimal maxDiscountedPurchases,
            out decimal currentPeriodDiscountedPurchases)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerID)}: {customerID}", LogLevel.Trace);
                if (dataModel.ServiceFactory == null)
                {
                    Utils.Log(this, "Service factory = null", LogLevel.Trace);
                    dataModel.ServiceFactory = new LSOne.DataLayer.GenericConnector.ServiceFactory();
                }

                var discountService = (IDiscountService)dataModel.Service(ServiceType.DiscountService);
                discountService.CustomersDiscountedPurchasesStatus(
                    dataModel,
                    (string)customerID,
                    out maxDiscountedPurchases,
                    out currentPeriodDiscountedPurchases);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                maxDiscountedPurchases = 0;
                currentPeriodDiscountedPurchases = 0;
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual CustomerPanelInformation GetCustomerPanelInformation(LogonInfo logonInfo, RecordIdentifier customerID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting, LogLevel.Trace);
                return Providers.CustomerData.GetCustomerPanelInformation(dataModel, customerID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done, LogLevel.Trace);
            }
        }

        public virtual XElement GetCustomerTransactionXML(LogonInfo logonInfo, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier storeCurrency, bool taxIncludedInPrice)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}, {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}", LogLevel.Trace);

                TypeOfTransaction transactionType = TransactionProviders.PosTransactionData.GetTransactionType(dataModel, transactionID, storeID, terminalID);
                PosTransaction transaction = new RetailTransaction((string)storeID, (string)storeCurrency, taxIncludedInPrice);
                transaction.TerminalId = (string)terminalID;

                if (transactionType == TypeOfTransaction.Sales)
                {
                    Utils.Log(this, "Transaction is sales transaction", LogLevel.Trace);
                    TransactionProviders.PosTransactionData.GetTransaction(dataModel, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);
                }
                else if (transactionType == TypeOfTransaction.Deposit)
                {
                    Utils.Log(this, "Transaction is deposit transaction", LogLevel.Trace);
                    TransactionProviders.PosTransactionData.GetTransaction(dataModel, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);

                    DepositTransaction newTrans = new DepositTransaction(transaction.StoreId, transaction.StoreCurrencyCode, ((RetailTransaction)transaction).TaxIncludedInPrice);
                    transaction = newTrans.Clone(((RetailTransaction)transaction));
                }
                else if (transactionType == TypeOfTransaction.Payment)
                {
                    Utils.Log(this, "Transaction is payment transaction", LogLevel.Trace);
                    if (dataModel.ServiceFactory == null)
                    {
                        Utils.Log(this, "Service factory = null", LogLevel.Trace);
                        dataModel.ServiceFactory = new LSOne.DataLayer.GenericConnector.ServiceFactory();
                    }

                    transaction = new CustomerPaymentTransaction(transaction.StoreCurrencyCode);
                    TransactionProviders.PosTransactionData.GetTransaction(dataModel, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);
                }

                return transaction.ToXML();
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