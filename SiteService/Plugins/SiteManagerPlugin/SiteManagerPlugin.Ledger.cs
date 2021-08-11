using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual List<CustomerLedgerEntries> GetCustomerLedgerEntriesList(
            RecordIdentifier customerId,
            out int totalRecords,
            LogonInfo logonInfo,
            CustomerLedgerFilter filter)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                return Providers.CustomerLedgerEntriesData.GetList(dataModel, customerId, out totalRecords, filter);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                totalRecords = 0;
                return null;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual decimal GetCustomerTotalSales(RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                return Providers.CustomerLedgerEntriesData.GetCustomerTotalSales(dataModel, customerId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual decimal GetCustomerBalance(RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                return Providers.CustomerLedgerEntriesData.GetCustomerBalance(dataModel, customerId);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
                return 0;
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void ValidateCustomerStatus(ref int valid, ref string comment, RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                decimal custBalance = Providers.CustomerLedgerEntriesData.GetCustomerBalance(dataModel, customerId);

                var cust = Providers.CustomerData.Get(dataModel, customerId, UsageIntentEnum.Normal);

                if (Math.Abs(custBalance) < cust.MaxCredit)
                {
                    valid = 0;
                }
                else
                {
                    valid = 1;
                    comment = "Customer credit limit " + cust.MaxCredit + cust.Currency + " was exceeded. Customer can charge only " + (custBalance + cust.MaxCredit) + cust.Currency;
                }
            }
            catch (Exception e)
            {
                comment = e.Message;
                valid = -1;
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void UpdateCustomerLedgerAtEOD(ref int valid, ref string comment, RecordIdentifier statementID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(statementID)}: {statementID}");

                if (logonInfo != null && logonInfo.UserID != RecordIdentifier.Empty)
                {
                    dataModel.Connection.SetContext(logonInfo.UserID);
                }

                valid = Providers.CustomerLedgerEntriesData.RecreateCustomerLedger(dataModel, "", statementID);

                if (logonInfo != null && logonInfo.UserID != RecordIdentifier.Empty)
                {
                    dataModel.Connection.RestoreContext();
                }

                if (valid == 3)
                {
                    comment = "Insert error";
                }
            }
            catch (Exception e)
            {
                comment = e.Message;
                valid = -1;
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void SaveCustomerLedgerEntries(CustomerLedgerEntries custLedgerEtries, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.CustomerLedgerEntriesData.Save(dataModel, custLedgerEtries);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void DeleteCustomerLedgerEntry(RecordIdentifier ledgerEntryNo, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(ledgerEntryNo)}: {ledgerEntryNo}");

                var contextGuid = Guid.NewGuid();
                try
                {
                    var overWritePermissions = new HashSet<string>
                    {
                        Permission.VoidPayment
                    };

                    dataModel.BeginPermissionOverride(contextGuid, overWritePermissions);

                    Providers.CustomerLedgerEntriesData.Delete(dataModel, ledgerEntryNo);
                }
                finally
                {
                    dataModel.EndPermissionOverride(contextGuid);
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual bool UpdateRemainingAmount(RecordIdentifier customerId, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(customerId)}: {customerId}");

                return Providers.CustomerLedgerEntriesData.UpdateRemainingAmount(dataModel, customerId);
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

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void CustomerAccountCreditMemo(ref int valid, ref string comment, RecordIdentifier CustomerID, RecordIdentifier ReceiptID,
            string Currency, decimal CurrencyAmount, decimal Amount, RecordIdentifier StoreId,
            RecordIdentifier TerminalId, RecordIdentifier TransactionId, Guid UserID, LogonInfo logonInf)
        {
            throw new NotImplementedException();
        }

        public virtual void CustomerAccountPayment(ref int valid, ref string comment, RecordIdentifier CustomerID, RecordIdentifier ReceiptID,
            string Currency, decimal CurrencyAmount, decimal Amount, RecordIdentifier StoreId,
            RecordIdentifier TerminalId, RecordIdentifier TransactionId, Guid UserID, LogonInfo logonInf)
        {
            throw new NotImplementedException();
        }


        public virtual void PaymentIntoCustomerAccount(ref int valid, ref string comment, RecordIdentifier CustomerID, RecordIdentifier ReceiptID,
            string Currency, decimal CurrencyAmount, decimal Amount, RecordIdentifier StoreId,
            RecordIdentifier TerminalId, RecordIdentifier TransactionId, Guid UserID, LogonInfo logonInf)
        {
            throw new NotImplementedException();
        }
    }
}