using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {

        public virtual void ValidateCustomerStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ref CustomerStatusValidationEnum valid, ref string comment, string customerId, ref decimal amount,
            string currencyCode, bool useCentralCustomer, bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                Customer customer;
                decimal custBalance;
                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (!useCentralCustomer)
                {
                    custBalance = Providers.CustomerLedgerEntriesData.GetCustomerBalance(entry, customerId);
                    customer = Providers.CustomerData.Get(entry, customerId, UsageIntentEnum.Normal);
                }
                else
                {
                    custBalance = server.GetCustomerBalance(customerId, CreateLogonInfo(entry));
                    customer = server.GetCustomer(customerId, CreateLogonInfo(entry));
                }

                var currencyExchangeRate = Providers.ExchangeRatesData.GetExchangeRate(entry, currencyCode, CacheType.CacheTypeTransactionLifeTime);

                decimal amountInLocalCurrency = amount * currencyExchangeRate;

                if (0 < amountInLocalCurrency && (0 > customer.MaxCredit + custBalance - rounding.Round(entry, amountInLocalCurrency, settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime)))
                {                    

                    decimal maxCreditCurrency = customer.MaxCredit / currencyExchangeRate;
                    decimal custBalanceCurrency = custBalance / currencyExchangeRate;
                    decimal maxChargedAmount = rounding.Round(entry, custBalanceCurrency + maxCreditCurrency, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);

                    if (maxChargedAmount > 0)
                    {
                        comment = Resources.CustomerCreditLimitExceeded.Replace("#1", rounding.RoundString(entry, custBalanceCurrency + maxCreditCurrency,
                                             settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime));

                        amount = custBalanceCurrency + maxCreditCurrency;
                        valid = CustomerStatusValidationEnum.MaxChargedAmountExceeded;
                    }
                    else
                    {
                        comment = Resources.CustomerCreditLimitReached.Replace("#1", rounding.RoundString(entry, maxCreditCurrency,
                                             settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime));

                        valid = CustomerStatusValidationEnum.Invalid;
                    }
                }
                else
                {
                    valid = CustomerStatusValidationEnum.Valid;
                }

                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch 
            {
                isClosed = true;
                Disconnect(entry);
                comment = Resources.CouldNotPayThroughCustomerAccount;
            }
        }

        public virtual List<CustomerLedgerEntries> GetCustomerLedgerEntriesList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier customerId,
            out int totalRecords,
            bool useCentralCustomer,
            CustomerLedgerFilter filter,
            bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                List<CustomerLedgerEntries> retVal;

                if (!useCentralCustomer)
                {
                    retVal = Providers.CustomerLedgerEntriesData.GetList(entry, customerId, out totalRecords, filter);
                }
                else
                {
                    retVal = server.GetCustomerLedgerEntriesList(customerId, out totalRecords, CreateLogonInfo(entry), filter);
                }

                if (useCentralCustomer && closeConnection)
                {
                    Disconnect(entry);
                }

                return retVal;

            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual decimal GetCustomerTotalSales(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true)
        {
            decimal result = 0;

            if (useCentralCustomer)
            {
                DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomerTotalSales(customerId, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                result = Providers.CustomerLedgerEntriesData.GetCustomerTotalSales(entry, customerId);
            }

            return result;
        }

        public virtual decimal GetCustomerBalance(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true)
        {
            decimal result = 0;

            if(useCentralCustomer)
            {
                DoRemoteWork(entry, siteServiceProfile, () => result = server.GetCustomerBalance(customerId, CreateLogonInfo(entry)), closeConnection);
            }
            else
            {
                result = Providers.CustomerLedgerEntriesData.GetCustomerBalance(entry, customerId);
            }

            return result;
        }

        public virtual bool UpdateCustomerLedgerAtEOD(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier statementID, bool useCentralCustomer, bool closeConnection = false)
        {
            int valid;
            const string comment = "";

            try
            {
                //if (!localDB && isClosed)
                //{
                //    Connect(entry, entry.SiteServiceAddress, entry.SiteServicePortNumber);
                //}
                //if (localDB)
                //{
                valid = Providers.CustomerLedgerEntriesData.RecreateCustomerLedger(entry, "", statementID);
                //}
                //else
                //{
                //server.UpdateCustomerLedgerAtEOD(ref valid, ref comment, statementID, null);
                //}

                if (valid != 0)
                {
                    if (valid == 3)
                        throw new Exception(Resources.ErrInsertUpdateError);
                    throw new Exception(comment);
                }

                if (useCentralCustomer && closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }

            return (valid == 0);
        }

        public virtual void SaveCustomerLedgerEntries(IConnectionManager entry, SiteServiceProfile siteServiceProfile, CustomerLedgerEntries custLedgerEntries, bool useCentralCustomer, bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.Save(entry, custLedgerEntries);
                }
                else
                {
                    server.SaveCustomerLedgerEntries(custLedgerEntries, CreateLogonInfo(entry));
                }

                if (useCentralCustomer && closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public void DeleteCustomerLedgerEntry(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier ledgerEntryNo, bool useCentralCustomer, bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.Delete(entry, ledgerEntryNo);
                }
                else
                {
                    server.DeleteCustomerLedgerEntry(ledgerEntryNo, CreateLogonInfo(entry));
                }

                if (useCentralCustomer && closeConnection)
                {
                    Disconnect(entry);
                }
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
        }

        public virtual void UpdateRemainingAmount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, bool useCentralCustomer, bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(entry, customerId);
                }
                else
                {
                    server.UpdateRemainingAmount(customerId, CreateLogonInfo(entry));
                }
            }

            catch (Exception)
            {
                Disconnect(entry);
                throw;
            }

            if (closeConnection)
                Disconnect(entry);
        }

        public virtual void CustomerAccountCreditMemo(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true)
        {

            if (amount < 0)
                amount = amount * -1;

            if (currencyAmount < 0)
                currencyAmount = currencyAmount * -1;

            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                var rec = new CustomerLedgerEntries();
                CustomerLedgerEntries recD = null;

                //rec.EntryNo = CustomerLedgerEtriesData.GetMaxEntryNo(entry) + 1; //ENTRYNO – MAX(ENTRYNO) + 1
                //Auto //DATAAREAID - LSR
                rec.PostingDate = DateTime.Now; //POSTINGDATE - NOW
                rec.Customer = customerId; //CUSTOMER – parameter CustomerID
                rec.EntryType = CustomerLedgerEntries.TypeEnum.CreditMemo; //TYPE – Credit Memo
                rec.DocumentNo = receiptId; //DOCUMENTNO - parameter ReceiptID
                rec.Currency = currency; //CURRENCY – parameter Currency
                rec.CurrencyAmount = currencyAmount; //CURRENCYAMOUNT – parameter currency amount
                rec.Amount = amount; //AMOUNT – parameter amount
                rec.RemainingAmount = amount; //REMAININGAMOUNT - parameter amount
                rec.StoreId = storeId;  //STOREID – parameter StoreID
                rec.TerminalId = terminalId;  //TERMINALID – parameter TerminalID
                rec.TransactionId = transactionId; //TRANSACTIONID – parameter TransactionID
                rec.ReceiptId = receiptId; //RECEIPTID – parameter ReceiptID
                rec.Status = CustomerLedgerEntries.StatusEnum.Open; //STATUS - Open
                rec.UserId = new Guid(entry.CurrentUser.ID.ToString());//UserID; //USERID – parameter UserID     

                if (amountDis < 0)
                {
                    recD = new CustomerLedgerEntries
                    {
                        PostingDate = DateTime.Now,
                        Customer = customerId,
                        EntryType = CustomerLedgerEntries.TypeEnum.Discount,
                        DocumentNo = receiptId,
                        Currency = currency,
                        CurrencyAmount = currencyAmountDis,
                        Amount = amountDis,
                        RemainingAmount = 0,
                        StoreId = storeId,
                        TerminalId = terminalId,
                        TransactionId = transactionId,
                        ReceiptId = receiptId,
                        Status = CustomerLedgerEntries.StatusEnum.Closed,
                        UserId = new Guid(entry.CurrentUser.ID.ToString())
                    };
                }

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.Save(entry, rec);

                    if (amountDis < 0 && recD != null)
                    {
                        Providers.CustomerLedgerEntriesData.Save(entry, recD);
                    }

                    Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(entry, customerId);
                }
                else
                {
                    server.SaveCustomerLedgerEntries(rec, CreateLogonInfo(entry));

                    if (amountDis < 0 && recD != null)
                    {
                        server.SaveCustomerLedgerEntries(recD, CreateLogonInfo(entry));
                    }

                    server.UpdateRemainingAmount(customerId, CreateLogonInfo(entry));
                }
            }

            catch (Exception)
            {
                Disconnect(entry);
                throw;
            }

            if (closeConnection)
                Disconnect(entry);
        }

        public virtual void CustomerAccountPayment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, decimal currencyAmountDis, decimal amountDis, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true)
        {

            if (amount > 0)
                amount = amount * -1;

            if (currencyAmount > 0)
                currencyAmount = currencyAmount * -1;

            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                CustomerLedgerEntries recD = null;

                //rec.EntryNo = CustomerLedgerEtriesData.GetMaxEntryNo(entry) + 1; //ENTRYNO – MAX(ENTRYNO) + 1
                //Auto //DATAAREAID - LSR
                var rec = new CustomerLedgerEntries
                {
                    PostingDate = DateTime.Now,
                    Customer = customerId,
                    EntryType = CustomerLedgerEntries.TypeEnum.Invoice,
                    DocumentNo = receiptId,
                    Currency = currency,
                    CurrencyAmount = currencyAmount,
                    Amount = amount,
                    RemainingAmount = amount,
                    StoreId = storeId,
                    TerminalId = terminalId,
                    TransactionId = transactionId,
                    ReceiptId = receiptId,
                    Status = CustomerLedgerEntries.StatusEnum.Open,
                    UserId = new Guid(entry.CurrentUser.ID.ToString())
                };

                if (amountDis > 0)
                {
                    recD = new CustomerLedgerEntries
                    {
                        PostingDate = DateTime.Now,
                        Customer = customerId,
                        EntryType = CustomerLedgerEntries.TypeEnum.Discount,
                        DocumentNo = receiptId,
                        Currency = currency,
                        CurrencyAmount = currencyAmountDis,
                        Amount = amountDis,
                        RemainingAmount = 0,
                        StoreId = storeId,
                        TerminalId = terminalId,
                        TransactionId = transactionId,
                        ReceiptId = receiptId,
                        Status = CustomerLedgerEntries.StatusEnum.Closed,
                        UserId = new Guid(entry.CurrentUser.ID.ToString())
                    };
                }

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.Save(entry, rec);

                    if (amountDis > 0 && recD != null)
                    {
                        Providers.CustomerLedgerEntriesData.Save(entry, recD);
                    }

                    Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(entry, customerId);
                }
                else
                {
                    server.SaveCustomerLedgerEntries(rec, CreateLogonInfo(entry));

                    if (amountDis > 0 && recD != null)
                    {
                        server.SaveCustomerLedgerEntries(recD, CreateLogonInfo(entry));
                    }

                    server.UpdateRemainingAmount(customerId, CreateLogonInfo(entry));

                }
            }

            catch (Exception)
            {
                Disconnect(entry);
                throw;
            }

            if (closeConnection)
                Disconnect(entry);
        }

        public virtual void PaymentIntoCustomerAccount(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier customerId, RecordIdentifier receiptId,
            string currency, decimal currencyAmount, decimal amount, RecordIdentifier storeId,
            RecordIdentifier terminalId, RecordIdentifier transactionId, bool useCentralCustomer, bool closeConnection = true)
        {
            try
            {
                if (useCentralCustomer && isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                var rec = new CustomerLedgerEntries
                {
                    PostingDate = DateTime.Now,
                    Customer = customerId,
                    EntryType = CustomerLedgerEntries.TypeEnum.Payment,
                    DocumentNo = receiptId,
                    Currency = currency,
                    CurrencyAmount = currencyAmount,
                    Amount = amount,
                    RemainingAmount = amount,
                    StoreId = storeId,
                    TerminalId = terminalId,
                    TransactionId = transactionId,
                    ReceiptId = receiptId,
                    Status = CustomerLedgerEntries.StatusEnum.Open,
                    UserId = new Guid(entry.CurrentUser.ID.ToString())
                };

                //rec.EntryNo = CustomerLedgerEtriesData.GetMaxEntryNo(entry) + 1; //ENTRYNO – MAX(ENTRYNO) + 1
                //Auto //DATAAREAID - LSR

                if (!useCentralCustomer)
                {
                    Providers.CustomerLedgerEntriesData.Save(entry, rec);
                    Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(entry, customerId);
                }
                else
                {
                    server.SaveCustomerLedgerEntries(rec, CreateLogonInfo(entry));
                    server.UpdateRemainingAmount(customerId, CreateLogonInfo(entry));
                }
            }

            catch (Exception)
            {
                Disconnect(entry);
                throw;
            }

            if (closeConnection)
                Disconnect(entry);
        }
    }
}
