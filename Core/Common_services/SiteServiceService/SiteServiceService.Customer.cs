using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Xml.Linq;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        //Customers
        public void ValidateCustomerStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref bool retVal, ref string comment, string customerId, string amount, string currencyCode)
        {

        }

        public virtual bool CustomerExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool useCentralDatabase, bool closeConnection)
        {
            bool result = false;

            if (useCentralDatabase)
            {
                DoRemoteWork(entry, siteServiceProfile, () => result = server.CustomerExists(customerID, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                return Providers.CustomerData.Exists(entry, customerID);
            }

            return result;
        }

        public virtual List<CustomerListItem> GetCustomers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string searchString, bool beginsWith, CustomerSorting sortOrder, bool sortBackwards, bool useCentralDatabase, bool closeConnection)
        {
            List<CustomerListItem> result = null;

            if (useCentralDatabase)
            {
                DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomers(searchString, beginsWith, sortOrder, sortBackwards, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                return Providers.CustomerData.GetList(entry, searchString, sortOrder, sortBackwards, beginsWith);
            }

            return result;
        }

        public virtual Customer GetCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool useCentralDatabase, bool closeConnection)
        {
            Customer result = null;

            if (useCentralDatabase)
            {
                DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomer(customerID, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                return Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal);
            }

            return result;
        }

        public virtual Customer SaveCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Customer customer, bool useCentralDatabase, bool closeConnection)
        {
            if (useCentralDatabase)
            {
                return DoRemoteWork(entry, siteServiceProfile, () => server.SaveCustomer(customer, CreateLogonInfo(entry)), closeConnection);
            }

            var overWritePermissions = new HashSet<string>
                {
                    Permission.CustomerEdit
                };

            var contextGuid = Guid.NewGuid();

            try
            {
                entry.BeginPermissionOverride(contextGuid, overWritePermissions);

                Providers.CustomerData.SaveWithAddresses(entry, customer);

                return customer;
            }
            finally
            {
                entry.EndPermissionOverride(contextGuid);
            }
        }

        public virtual void DeleteCustomer(IConnectionManager entry, SiteServiceProfile siteServiceProfile, Customer customer, bool useCentralDatabase, bool closeConnection)
        {
            if (useCentralDatabase)
            {
                DoRemoteWork(entry, siteServiceProfile, () => server.DeleteCustomer(customer, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                var overWritePermissions = new HashSet<string>
                {
                    Permission.CustomerEdit
                };
                var contextGuid = Guid.NewGuid();
                try
                {
                    entry.BeginPermissionOverride(contextGuid, overWritePermissions);
                    Providers.CustomerData.Delete(entry, customer.ID);
                }
                finally
                {
                    entry.EndPermissionOverride(contextGuid);
                }
            }
        }

        public virtual void SetCustomerCreditLimit(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, decimal creditLimit, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SetCustomerCreditLimit(customerID, creditLimit, CreateLogonInfo(entry)), closeConnection);
        }

        public virtual void CustomersDiscountedPurchasesStatus(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            string customerID,
            out decimal maxDiscountedPurchases,
            out decimal currentPeriodDiscountedPurchases,
            bool closeConnection)
        {
            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                server.CustomersDiscountedPurchasesStatus(customerID, CreateLogonInfo(entry), out maxDiscountedPurchases, out currentPeriodDiscountedPurchases);

                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception ex)
            {
                isClosed = true;
                Disconnect(entry);
                throw ex;
            }
        }

        public virtual CustomerPanelInformation GetCustomerPanelInformation(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerID, bool closeConnection)
        {
            CustomerPanelInformation result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomerPanelInformation(CreateLogonInfo(entry), customerID), closeConnection);
            return result;
        }

        public virtual XElement GetCustomerTransactionXML(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, bool useCentralCustomer, bool closeConnection = true)
        {
            XElement xml = null;
            try
            {
                ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

                if (!isClosed)
                {
                    Disconnect(entry);
                }

                if (isClosed)
                {
                    if (useCentralCustomer)
                    {
                        Connect(entry, siteServiceProfile);
                    }
                }

                xml = useCentralCustomer ?
                        server.GetCustomerTransactionXML(CreateLogonInfo(entry), transactionID, storeID, terminalID, settings.Store.Currency, settings.TaxIncludedInPrice) :
                        GetLocalCustomerTransactionXML(entry, transactionID, storeID, terminalID, settings.Store.Currency, settings.TaxIncludedInPrice);

                if (closeConnection)
                {
                    Disconnect(entry);
                }

                return xml;

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        private XElement GetLocalCustomerTransactionXML(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID, string currency, bool taxIncludedInPrice)
        {
            try
            {
                TypeOfTransaction transactionType = TransactionProviders.PosTransactionData.GetTransactionType(entry, transactionID, storeID, terminalID);
                PosTransaction transaction = new RetailTransaction((string)storeID, currency, taxIncludedInPrice);
                transaction.TerminalId = (string)terminalID;

                if (transactionType == TypeOfTransaction.Sales)
                {
                    TransactionProviders.PosTransactionData.GetTransaction(entry, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);
                }
                else if (transactionType == TypeOfTransaction.Deposit)
                {
                    TransactionProviders.PosTransactionData.GetTransaction(entry, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);

                    DepositTransaction newTrans = new DepositTransaction(transaction.StoreId, transaction.StoreCurrencyCode, ((RetailTransaction)transaction).TaxIncludedInPrice);
                    transaction = newTrans.Clone(((RetailTransaction)transaction));
                }
                else if (transactionType == TypeOfTransaction.Payment)
                {
                    transaction = new CustomerPaymentTransaction(transaction.StoreCurrencyCode);
                    TransactionProviders.PosTransactionData.GetTransaction(entry, transactionID,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             taxIncludedInPrice);
                }

                return transaction.ToXML();
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }
    }
}
