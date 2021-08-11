using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Linq;

namespace LSOne.Services
{
    public partial class TenderService
    {      

        public string ErrorText { get; set; }

        public bool IsTenderAllowed(IConnectionManager dataModel, IPosTransaction transaction, RecordIdentifier storeCurrencyID, StorePaymentMethod tenderInfo, decimal paidAmount, 
                                    bool manuallyEnteredAmount, decimal balance, decimal transactionTotal, decimal payment = 0, decimal restrictedAmount = 0)
        {
            ErrorText = "";
            var rounding = (IRoundingService)dataModel.Service(ServiceType.RoundingService);

            //if the paid amount is less than zero then check if the tender allow negative payment amounts
            if (!tenderInfo.AllowNegativePaymentAmounts && paidAmount < decimal.Zero)
            {
                //Payment type #1 cannot be used when returning items
                ErrorText = Properties.Resources.PaymentTypeCannotReturningItems.Replace("#1", tenderInfo.Text);
                return false;
            }

            transactionTotal = rounding.RoundAmount(dataModel, transactionTotal, dataModel.CurrentStoreID, tenderInfo.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime);
            //If the total balance of the transaction is larger than the current balance then check if the tender is allowed
            //to be a part of a split payment
            if (paidAmount != 0 && !tenderInfo.PaymentTypeCanBePartOfSplitPayment && (transactionTotal > paidAmount))
            {
                ErrorText = Properties.Resources.PaymentTypeCannotSplitPayment.Replace("#1", tenderInfo.Text);
                return false;
            }

            // Is the paid amount higher than the max specified amount level
            if ((tenderInfo.MaximumAmountAllowed != 0) && ((paidAmount > tenderInfo.MaximumAmountAllowed) || ((payment + paidAmount) > tenderInfo.MaximumAmountAllowed)))
            {
                // 1383 = The maximum amount entered is:     
                ErrorText = Properties.Resources.MaximumPaymentAllowed.Replace("#1", tenderInfo.Text);

                ErrorText += " " + rounding.RoundString(dataModel, tenderInfo.MaximumAmountAllowed, storeCurrencyID, true, CacheType.CacheTypeTransactionLifeTime);
                return false;
            }

            // Is the paid amount higher than the max specified amount level
            if ((manuallyEnteredAmount) && (tenderInfo.MaximumAmountEntered != 0) && (paidAmount > tenderInfo.MaximumAmountEntered))
            {
                ErrorText = Properties.Resources.MinimumAmountEnteredIs + " " +
                            rounding.RoundString(dataModel, tenderInfo.MaximumAmountEntered, storeCurrencyID, false,
                                CacheType.CacheTypeTransactionLifeTime);
                return false;
            }

            // Is the absolut value of the paid amount lower than the min specified amount level
            if ((tenderInfo.MinimumAmountAllowed != 0) && (Math.Abs(paidAmount) < tenderInfo.MinimumAmountAllowed) && (balance != 0))
            {
                // 1387 = The minimum amount entered is:
                // 1388 =                
                ErrorText = Properties.Resources.MinimumPaymentAllowed.Replace("#1", tenderInfo.Text);
                ErrorText += " " + rounding.RoundString(dataModel, tenderInfo.MinimumAmountAllowed, storeCurrencyID, true, CacheType.CacheTypeTransactionLifeTime);
                return false;
            }

            // Is the paid amount lower than the min specified amount level
            if ((manuallyEnteredAmount) && (tenderInfo.MinimumAmountEntered != 0) && ((Math.Abs(paidAmount) < tenderInfo.MinimumAmountEntered)) && (balance != 0))
            {
                ErrorText = Properties.Resources.MinimumAmountEnteredIs + " " +
                            rounding.RoundString(dataModel, tenderInfo.MaximumAmountAllowed, storeCurrencyID, false,
                                CacheType.CacheTypeTransactionLifeTime);
                return false;
            }

            // Is paid more than the balance amount
            if (Math.Abs(balance) < Math.Abs(paidAmount))
            {
                var retailTransaction = transaction as RetailTransaction;

                bool hasPaymentsWithoutLimitations =
                        transaction.ITenderLines
                        .Select(x => Providers.StorePaymentMethodData.Get(dataModel, new RecordIdentifier(dataModel.CurrentStoreID, new RecordIdentifier(x.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime))
                        .Any(x => !x.PaymentLimitations.Any());

                if (!tenderInfo.AllowOverTender ||
                    (retailTransaction != null &&
                    !retailTransaction.IsReturnTransaction && 
                    tenderInfo.PaymentLimitations.Any() &&
                    retailTransaction.CustomerOrder.Empty() &&
                    (hasPaymentsWithoutLimitations || Math.Abs(restrictedAmount) < Math.Abs(paidAmount))))
                {
                    if (rounding.RoundAmount(dataModel, balance, dataModel.CurrentStoreID, tenderInfo.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime) < paidAmount)
                    {
                        ErrorText = Properties.Resources.NoChangeAllowed;
                        return false;
                    }
                }

                if (Math.Abs(tenderInfo.MaximumOverTenderAmount) > 0)
                {
                    if (tenderInfo.MaximumOverTenderAmount < (paidAmount - balance))
                    {
                        ErrorText = Properties.Resources.ChangeAmountCannotExceed + " " +
                                    rounding.RoundString(dataModel, tenderInfo.MaximumOverTenderAmount, storeCurrencyID, true,
                                        CacheType.CacheTypeTransactionLifeTime);
                        return false;
                    }
                }
            }
            else if (Math.Abs(balance) > Math.Abs(paidAmount))     // Is it possible to pay lower than the total amount?           
            {
                if (!tenderInfo.AllowUnderTender)
                {
                    if (rounding.RoundAmount(dataModel, balance, dataModel.CurrentStoreID, tenderInfo.ID.SecondaryID, CacheType.CacheTypeTransactionLifeTime) > paidAmount)
                    {
                        ErrorText = Properties.Resources.PaymentFinalizeTransaction;
                        return false;
                    }
                }

                if (Math.Abs(tenderInfo.UnderTenderAmount) > 0)
                {
                    if ((balance - paidAmount) > tenderInfo.UnderTenderAmount)
                    {
                        ErrorText = Properties.Resources.ChangeAmountCannotExceed + " " +
                                    rounding.RoundString(dataModel, tenderInfo.UnderTenderAmount, storeCurrencyID, true,
                                        CacheType.CacheTypeTransactionLifeTime);
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
