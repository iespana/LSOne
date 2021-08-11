using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System.Collections.Generic;

namespace LSOne.Services
{
    public partial class CashManagementService : ICashManagementService
    {
        public IErrorLog ErrorLog { set; private get; }

        public void Init(IConnectionManager entry)
        {
            
        }

        /// <summary>
        /// Get the amount that was declared at the start of day
        /// </summary>
        /// <param name="entry">Database connection</param>
        public virtual decimal GetDeclaredStartOfDayAmount(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            List<StorePaymentMethod> tenderTypes = Providers.StorePaymentMethodData.GetRecords(entry, entry.CurrentStoreID, true);
            StorePaymentMethod tender = tenderTypes.Find(t => (int)t.PosOperation == (int)POSOperations.PayCash && t.CountingRequired && !t.AllowFloat && t.DefaultFunction == PaymentMethodDefaultFunctionEnum.Normal);

            decimal amount = tender != null ? TransactionProviders.StartOfDayData.GetStartOfDayAmount(entry, entry.CurrentStoreID, entry.CurrentTerminalID, tender.ID.SecondaryID) : 0M;
            amount = (settings.Store.StartAmount != 0 && amount > settings.Store.StartAmount) ? settings.Store.StartAmount : amount;

            return amount;
        }

        /// <summary>
        /// Get the last tender declaration amount
        /// </summary>
        /// <param name="entry">Database connection</param>
        public virtual decimal GetLastTenderDeclarationAmount(IConnectionManager entry)
        {
            RecordIdentifier tenderID = Providers.StorePaymentMethodData.GetChangeTenderForFunction(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.FloatTender) 
                                     ?? Providers.StorePaymentMethodData.GetCashTender(entry, entry.CurrentStoreID);

            return TransactionProviders.StartOfDayData.GetFloatsFromLastTenderDeclaration(entry, entry.CurrentStoreID, entry.CurrentTerminalID, tenderID);
        }

        /// <summary>
        /// Perform a tender declaration of any type
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <param name="tenderDeclarationType">Type of tender declaration</param>
        /// <param name="previousAmount">Amount that was previously declared in a tender declaration</param>
        /// <param name="amount">Amount that is declared in the currenct tender declaration</param>
        /// <param name="description">Description of the tender declaration</param>
        public virtual void TenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction, TenderDeclarationType tenderDeclarationType, decimal previousAmount, decimal amount, string description)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            decimal storeExchangeRate = Interfaces.Services.CurrencyService(entry).ExchangeRate(entry, settings.Store.Currency) * 100;
            decimal amountInExchangeRate = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(entry, settings.Store.Currency, settings.CompanyInfo.CurrencyCode, settings.CompanyInfo.CurrencyCode, amount);

            switch (tenderDeclarationType)
            {
                case TenderDeclarationType.DeclareStartAmount:
                case TenderDeclarationType.Float:
                    FloatEntryTransaction floatEntryTransaction = posTransaction as FloatEntryTransaction;

                    if(floatEntryTransaction == null)
                    {
                        posTransaction.EntryStatus = TransactionStatus.Cancelled;
                        return;
                    }

                    floatEntryTransaction.PreviouslyTendered = previousAmount;
                    floatEntryTransaction.Amount = amount;
                    floatEntryTransaction.AddedAmount = amount;
                    floatEntryTransaction.Description = description;
                    floatEntryTransaction.ExchrateMST = storeExchangeRate;
                    floatEntryTransaction.AmountMST = amountInExchangeRate;
                    floatEntryTransaction.StoreCurrencyCode = settings.Store.Currency;
                    break;
                case TenderDeclarationType.TenderRemoval:
                    RemoveTenderTransaction removeTenderTransaction = posTransaction as RemoveTenderTransaction;

                    if (removeTenderTransaction == null)
                    {
                        posTransaction.EntryStatus = TransactionStatus.Cancelled;
                        return;
                    }

                    removeTenderTransaction.Amount = amount;
                    removeTenderTransaction.Description = description;
                    removeTenderTransaction.ExchrateMST = storeExchangeRate;
                    removeTenderTransaction.AmountMST = amountInExchangeRate;
                    removeTenderTransaction.StoreCurrencyCode = settings.Store.Currency;
                    break;
            }
        }
    }
}
