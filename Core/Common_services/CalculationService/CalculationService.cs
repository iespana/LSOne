using System;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.Services.Interfaces.SupportClasses.Calculation;
using LSOne.POS.Engine.OperationHandlers.Helpers;

namespace LSOne.Services
{
    /// <summary>
    /// The Calculation service takes care of calculating each line of the transaction as well as the balance of the entire transaction. The sum totals of various
    /// amounts is also calculated such as the tax amount
    /// </summary>    
    public partial class CalculationService : ICalculationService
    {
        /// <summary>
        /// A summary of which discounts are available on each sales line
        /// </summary>
        protected struct LineDiscountInfo
        {
            /// <summary>
            /// If true then the line has a periodic discount line
            /// </summary>
            public bool HasPeriodicDiscount;
            /// <summary>
            /// If true then the line has a mix and match discount line
            /// </summary>
            public bool HasMixAndMatchDiscount;
            /// <summary>
            /// If true then the line has a customer line discount line
            /// </summary>
            public bool HasCustomerDiscount;
            /// <summary>
            /// If true then the line has a customer total discount line
            /// </summary>
            public bool HasCustomerTotalDiscount;
        }

        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set
            {
            }
        }

        /// <summary>
        /// Initializes the calculation service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        #region Constructor
        /// <summary>
        /// Sets a SqlConnection variable and the current DataAreaId using LSRetailPosis.Settings.ApplicationSettings
        /// </summary>
        public CalculationService()
        {
        }

        #endregion

        #region ICalculation Members

        /// <summary>
        /// Calculates the balance of the transaction. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="currencyCode">The currency the transaction is in</param>
        public virtual void CalculateTotals(IConnectionManager entry, IPosTransaction posTransaction, RecordIdentifier currencyCode = null)
        {
            try
            {
                if (posTransaction is IRetailTransaction)
                {
                    CalculateRetailTransaction(entry, (IRetailTransaction)posTransaction, currencyCode);
                }
                else if (posTransaction is ICustomerPaymentTransaction)
                {
                    CalculateCustomerPaymentTransaction(entry, (ICustomerPaymentTransaction)posTransaction);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        #region Customer Payment Transaction
        /// <summary>
        /// Calculates the balance of the customer payment transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerPaymentTransaction">The customer payment transaction to be calculated</param>
        protected virtual void CalculateCustomerPaymentTransaction(IConnectionManager entry, ICustomerPaymentTransaction customerPaymentTransaction)
        {
            try
            {
                if (customerPaymentTransaction.ICustomerDepositItem == null)
                {
                    customerPaymentTransaction.Amount = 0;
                    customerPaymentTransaction.NoOfDeposits = 0;
                }
                else
                {
                    customerPaymentTransaction.Amount = customerPaymentTransaction.ICustomerDepositItem.Amount;
                    customerPaymentTransaction.NoOfDeposits = 1;
                }

                //Calc totals for the payments
                customerPaymentTransaction.Payment = 0;
                string lastTenderID = null;
                foreach (ITenderLineItem tenderLineItem in customerPaymentTransaction.ITenderLines.Where(tenderLineItem => tenderLineItem.Voided == false))
                {
                    customerPaymentTransaction.Payment += tenderLineItem.Amount;
                    lastTenderID = tenderLineItem.TenderTypeId;
                }

                var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                RecordIdentifier storeCurrency = settings == null ? customerPaymentTransaction.StoreCurrencyCode : settings.Store.Currency;
                RecordIdentifier storeID = settings == null ? customerPaymentTransaction.StoreId : settings.Store.ID;

                IRoundingService roundingService = Interfaces.Services.RoundingService(entry);
                customerPaymentTransaction.TransSalePmtDiff = customerPaymentTransaction.Amount - customerPaymentTransaction.Payment;

                // If there is a payment in the transaction
                if (lastTenderID != null)
                {
                    if (roundingService.IsLessThanSmallestCoin(entry, Math.Abs(customerPaymentTransaction.TransSalePmtDiff), storeID, lastTenderID) == true)
                    {
                        customerPaymentTransaction.RoundingSalePmtDiff = customerPaymentTransaction.TransSalePmtDiff;
                        customerPaymentTransaction.TransSalePmtDiff = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        #endregion

        #region Retail Transaction

        /// <summary>
        /// Calculates the balance of the retail transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="currencyCode">The currency in which the transaction is</param>
        protected virtual void CalculateRetailTransaction(IConnectionManager entry, IRetailTransaction retailTransaction, RecordIdentifier currencyCode)
        {
            try
            {
                retailTransaction.ClearTotalAmounts();
                retailTransaction.NoOfItems = 0;

                #region Add total discounts if needed

                if (retailTransaction.CalculateTotalDiscount)
                {
                    if (retailTransaction.TotalManualPctDiscount == 0 && retailTransaction.TotalManualDiscountAmount == 0)
                    {
                        retailTransaction.ClearTotalDiscountLines();
                    }

                    if (retailTransaction.TotalManualPctDiscount != 0)
                    {
                        retailTransaction.AddTotalDiscPctLines();
                    }               
                }

                #endregion

                //Calc totals for sale items.
                foreach (ISaleLineItem saleLineItem in retailTransaction.SaleItems)
                {
                    if (saleLineItem.ShouldCalculateAndDisplayAssemblyPrice())
                    {
                        if (saleLineItem.Voided == false)
                        {
                            CalculateLine(entry, saleLineItem, retailTransaction, currencyCode);

                            #region Scanned/Keyed statistics.

                            Int64 qtyCounted = 0;
                            if (saleLineItem is IFuelSalesLineItem || saleLineItem.ScaleItem)
                            {
                                qtyCounted = 1;
                            }
                            else
                            {
                                qtyCounted = (Int64)((saleLineItem.Quantity) / 1);
                                if (qtyCounted == 0) { qtyCounted = 1; }
                            }
                            qtyCounted = Math.Abs(qtyCounted);
                            if (saleLineItem.EntryType == BarCode.BarcodeEntryType.ManuallyEntered) { retailTransaction.LineItemsKeyedCount += qtyCounted; }
                            if (saleLineItem.EntryType == BarCode.BarcodeEntryType.Selected) { retailTransaction.LineItemsKeyedCount += qtyCounted; }
                            if (saleLineItem.EntryType == BarCode.BarcodeEntryType.SingleScanned) { retailTransaction.LineItemsSingleScannedCount += qtyCounted; }
                            if (saleLineItem.EntryType == BarCode.BarcodeEntryType.MultiScanned) { retailTransaction.LineItemsMultiScannedCount += qtyCounted; }
                            retailTransaction.NoOfItems += (decimal)qtyCounted;
                            if (retailTransaction.NoOfItems != 0)
                            {
                                retailTransaction.LineItemsSingleScannedPercent = (decimal)(retailTransaction.LineItemsSingleScannedCount / (retailTransaction.NoOfItems)) * 100;
                                retailTransaction.LineItemsMultiScannedPercent = (decimal)(retailTransaction.LineItemsMultiScannedCount / (retailTransaction.NoOfItems)) * 100;
                                retailTransaction.LineItemsKeyedPercent = (decimal)(retailTransaction.LineItemsKeyedCount / (retailTransaction.NoOfItems)) * 100;
                            }

                            #endregion

                            retailTransaction.UpdateTotalAmounts(saleLineItem);
                        }
                        else
                        {
                            saleLineItem.PeriodicDiscountOfferId = "";
                            saleLineItem.PeriodicDiscountOfferName = "";
                            CalculateLine(entry, saleLineItem, retailTransaction, currencyCode);
                        }
                    }
                    
                    saleLineItem.WasChanged = false;
                }

                bool forceRecalculateTotal = false;
                if (BanCompoundDiscounts)
                {
                    bool totalDiscFound = false;
                    bool anyDiscountFound = false;
                    // Check to see if we need to clear the total discount amount and pct
                    foreach (ISaleLineItem saleLineItem in retailTransaction.SaleItems)
                    {
                        foreach (var discountItem in saleLineItem.DiscountLines)
                        {
                            if (discountItem is ITotalDiscountItem)
                            {
                                totalDiscFound = true;
                            }
                            else
                            {
                                anyDiscountFound = true;
                            }
                        }
                    }

                    if (anyDiscountFound)
                    {
                        // Clear total amount discount, cannot support that with other discounts
                        if (retailTransaction.TotalManualDiscountAmount != 0m)
                        {
                            forceRecalculateTotal = true;
                        }
                    }
                    if (!totalDiscFound)
                    {
                        retailTransaction.SetTotalDiscPercent(0m);
                    }
                }

                if (retailTransaction.CalculateTotalDiscount)
                {
                    if (forceRecalculateTotal || retailTransaction.TotalManualDiscountAmount != 0)
                    {
                        retailTransaction.ClearTotalAmounts();

                        decimal totalDiscountPercentage = retailTransaction.AddTotalDiscAmountLines(BanCompoundDiscounts);
                        AdjustTotalDiscountRoundingDifference(entry, retailTransaction, currencyCode, totalDiscountPercentage);

                        foreach (ISaleLineItem saleLineItem in retailTransaction.SaleItems.Where(item => !item.Voided))
                        {
                            CalculateLine(entry, saleLineItem, retailTransaction, currencyCode);

                            if (!saleLineItem.Voided)
                            {
                                retailTransaction.UpdateTotalAmounts(saleLineItem);
                            }
                        }
                    }
                }

                #region Payments

                retailTransaction.Payment = 0;

                //Calc totals for the payments
                string lastTenderID = null;
                foreach (ITenderLineItem tenderLineItem in retailTransaction.TenderLines)
                {
                    if (tenderLineItem.Voided == false)
                    {
                        retailTransaction.Payment += tenderLineItem.Amount;
                        lastTenderID = tenderLineItem.TenderTypeId;
                    }
                }

                IRoundingService rounding = Interfaces.Services.RoundingService(entry);

                // If we are comming from Site Manager then we get currency code and we should not be using cache when in the Site Manager
                CacheType cacheType = (currencyCode == null) ? CacheType.CacheTypeTransactionLifeTime : CacheType.CacheTypeNone;

                retailTransaction.TransSalePmtDiff = retailTransaction.NetAmountWithTax + retailTransaction.MarkupItem.Amount - retailTransaction.Payment;
                retailTransaction.RoundingDifference = rounding.Round(entry, retailTransaction.NetAmountWithTax, retailTransaction.StoreCurrencyCode, true, cacheType) - retailTransaction.NetAmountWithTax;

                // If there is a payment in the transaction
                if (lastTenderID != null)
                {
                    if (rounding.IsLessThanSmallestCoin(entry, Math.Abs(retailTransaction.TransSalePmtDiff), retailTransaction.StoreId, lastTenderID, cacheType))
                    {
                        if (rounding.IsRoundedEqualToSmallestCoin(entry, Math.Abs(retailTransaction.TransSalePmtDiff), retailTransaction.StoreId, lastTenderID, cacheType))
                        {
                            retailTransaction.SetNetAmountWithTax(rounding.RoundAmount(entry, retailTransaction.NetAmountWithTax, retailTransaction.StoreId, lastTenderID, cacheType), false);
                            retailTransaction.IsNetAmountWithTaxRounded = true;
                            retailTransaction.TransSalePmtDiff = retailTransaction.NetAmountWithTax + retailTransaction.MarkupItem.Amount - retailTransaction.Payment;
                            retailTransaction.RoundingDifference = 0;
                        }
                        else
                        {
                            retailTransaction.RoundingSalePmtDiff = retailTransaction.TransSalePmtDiff;
                            retailTransaction.TransSalePmtDiff = 0;
                            retailTransaction.SetNetAmountWithTax(rounding.Round(entry, retailTransaction.NetAmountWithTax, retailTransaction.StoreCurrencyCode, true, cacheType), false);
                            retailTransaction.IsNetAmountWithTaxRounded = true;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        private void AdjustTotalDiscountRoundingDifference(IConnectionManager entry, IRetailTransaction retailTransaction, RecordIdentifier currencyCode, decimal discountPercentage)
        {
            if (discountPercentage == 0) { return; }

            IRoundingService rounding = Interfaces.Services.RoundingService(entry);
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // If called from the POS, get the currency from the store and cache currency data during transaction lifetime
            CacheType cacheType = (currencyCode == null) ? CacheType.CacheTypeTransactionLifeTime : CacheType.CacheTypeNone;
            currencyCode = currencyCode ?? settings.Store.Currency;

            // All lines that the total discount was applied to
            var discountSaleItems = retailTransaction.SaleItems.Where(saleItem =>
                saleItem.Voided == false &&
                saleItem.IncludedInTotalDiscount &&
                saleItem.NoDiscountAllowed == false &&
                Math.Abs(saleItem.Quantity) > 0
            );
            
            if (!discountSaleItems.Any()) { return; }

            // Sum up discount from all lines after rounding
            decimal sumOfRoundedDiscountAmounts = 0;
            foreach (ISaleLineItem saleItem in discountSaleItems)
            {
                sumOfRoundedDiscountAmounts += rounding.Round(
                    entry,  
                    discountPercentage * GetAmountBeforeTotalDiscount(retailTransaction, saleItem) / 100, 
                    currencyCode, 
                    cacheType);
            }

            // If the amounts to not match, adjust the discount of the first item that the discount was applied to
            decimal roundedDiscountAmount = rounding.Round(entry, retailTransaction.TotalManualDiscountAmount, currencyCode, cacheType);
            decimal roundingDiff = (roundedDiscountAmount - Math.Abs(sumOfRoundedDiscountAmounts)) * Math.Sign(sumOfRoundedDiscountAmounts);
            if (roundingDiff != 0)
            {
                ISaleLineItem firstLineInDiscount = discountSaleItems.FirstOrDefault();

                decimal amountBeforeDiscount = GetAmountBeforeTotalDiscount(retailTransaction, firstLineInDiscount);
                decimal adjustedDiscountedAmount = (discountPercentage * amountBeforeDiscount / 100) + roundingDiff;

                firstLineInDiscount.ClearDiscountLines(typeof(TotalDiscountItem));
                firstLineInDiscount.Add(new TotalDiscountItem { 
                    Percentage = 100 * adjustedDiscountedAmount / amountBeforeDiscount 
                });
            }

            decimal GetAmountBeforeTotalDiscount(IRetailTransaction transaction, ISaleLineItem saleItem)
            {
                decimal amountBeforeTotalDiscount = 0;

                switch (transaction.CalculateDiscountFrom)
                {
                    case Store.CalculateDiscountsFromEnum.PriceWithTax:
                        amountBeforeTotalDiscount = saleItem.GrossAmountWithTax - saleItem.LineDiscountWithTax - saleItem.PeriodicDiscountWithTax - saleItem.LoyaltyDiscountWithTax;
                        break;

                    case Store.CalculateDiscountsFromEnum.Price:
                        amountBeforeTotalDiscount = saleItem.GrossAmount - saleItem.LineDiscount - saleItem.PeriodicDiscount - saleItem.LoyaltyDiscount;
                        break;
                }

                return amountBeforeTotalDiscount;
            }
        }

        #endregion

        #region CalculateLine

        /// <summary>
        /// Calculates all amounts on the saleline item such as discount amounts, net amount and etc.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The sale line item that is to be calculated</param>
        /// <param name="retailTransaction">The transaction the sale line being calculated belongs to</param>
        /// <param name="currencyCode">The currency code. If passing null then Store currency will be used. POS should pass null while Site Manager should pass a valid code</param>
        public virtual void CalculateLine(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction, RecordIdentifier currencyCode = null)
        {
            try
            {
                CalculateLine(entry, saleLineItem, retailTransaction, true, currencyCode);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Calculates all amounts on the saleline item such as discount amounts, net amount and etc.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">The sale line item that is to be calculated</param>
        /// <param name="retailTransaction">The transaction the sale line being calculated belongs to</param>
        /// <param name="currencyCode">The currency code. If passing null then Store currency will be used. POS should pass null while Site Manager should pass a valid code</param>
        /// <param name="compareDiscounts">If true then all discounts on the transaction are compared and the best ones are calculated according to the comparison rules</param>
        public virtual void CalculateLine(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction, bool compareDiscounts, RecordIdentifier currencyCode = null)
        {
            try
            {
                CacheType cacheType;

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (currencyCode == null)
                {
                    // We are comming from the POS
                    cacheType = CacheType.CacheTypeTransactionLifeTime;
                    currencyCode = settings.Store.Currency;
                }
                else
                {
                    // We are comming from Site Manager
                    cacheType = CacheType.CacheTypeNone;
                }

                if (!saleLineItem.Blocked && saleLineItem.Found && saleLineItem.DateToActivateItem.DateTime <= saleLineItem.Transaction.BeginDateTime && saleLineItem.ShouldCalculateAndDisplayAssemblyPrice())
                {
                    saleLineItem.LineDiscount = 0;
                    saleLineItem.LineDiscountWithTax = 0;
                    saleLineItem.LinePctDiscount = 0;
                    saleLineItem.PeriodicDiscount = 0;
                    saleLineItem.PeriodicDiscountWithTax = 0;
                    saleLineItem.PeriodicDiscount = 0;
                    saleLineItem.LoyaltyDiscountWithTax = 0;
                    saleLineItem.LoyaltyDiscount = 0;
                    saleLineItem.LoyaltyPctDiscount = 0;
                    saleLineItem.TotalDiscount = 0;
                    saleLineItem.TotalDiscountWithTax = 0;
                    saleLineItem.TotalPctDiscount = 0;

                    decimal lineAmount = 0;            //The total line discount amount that is given as a discount excluding the percentage discount.
                    decimal linePercentage = 0;        //The total line discount percentage discount that is given as as discount excluding the discount amount given.
                    decimal totalAmount = 0;           //The total discount amount that is given as a discount excluding the percentage discount.
                    decimal totalPercentage = 0;       //The total discount percentage discount that is given as as discount excluding the discount amount given.
                    decimal periodicAmount = 0;        //The total periodic disc. amount given as a discount excluding the percentage discount.
                    decimal periodicPercentage = 0;    //The total periodic disc. percentage discount that is given as discount excluding the discount amount given. 
                    decimal loyaltyAmount = 0;
                    decimal loyaltyPercentage = 0;
                    decimal calculatedQty = 0;         //The calculated quantity for fuel items                    

                    //Determine if the item is a fuel item or not
                    bool isFuelItem = (saleLineItem is IFuelSalesLineItem) || (saleLineItem.OriginatesFromForecourt);

                    if (saleLineItem.Voided)
                    {
                        saleLineItem.DiscountLines.Clear();
                    }

                    if (compareDiscounts)
                    {
                        ComparingDiscounts(entry, saleLineItem, retailTransaction,currencyCode);
                    }

                    //Calculate the total discount for the sales item.
                    bool seenPeriodicDiscount = false;
                    bool seenCustomerDiscount = false;
                    bool seenLineDiscount = false;
                    var removeItems = new List<IDiscountItem>();
                    if (BanCompoundDiscounts && saleLineItem.PriceOverridden)
                    {
                        saleLineItem.DiscountLines.Clear();
                    }

                    // Start by checking for periodic, customer and line discounts
                    foreach (IDiscountItem discountItem in saleLineItem.DiscountLines)
                    {
                        if (discountItem is ICustomerDiscountItem)
                        {
                            seenCustomerDiscount = true;
                        }
                        else if (discountItem is IPeriodicDiscountItem)
                        {
                            seenPeriodicDiscount = true;
                        }
                        else if (discountItem is ILineDiscountItem)
                        {
                            seenLineDiscount = true;
                        }
                    }

                    foreach (IDiscountItem discountLineItem in saleLineItem.DiscountLines)
                    {
                        #region Customer Discount Groups
                        if (discountLineItem is ICustomerDiscountItem)
                        {
                            seenCustomerDiscount = true;

                            if (BanCompoundDiscounts && (seenLineDiscount))
                            {
                                removeItems.Add(discountLineItem);
                                continue;
                            }

                            var custDiscLineItem = (ICustomerDiscountItem)discountLineItem;
                            if (custDiscLineItem.CustomerDiscountType == CustomerDiscountTypes.TotalDiscount)
                            {
                                totalAmount += custDiscLineItem.Amount;
                                totalPercentage += custDiscLineItem.Percentage;
                            }
                            else
                            {
                                //If the sale item has either Line Discount or Multi line discount then just use the discount values as they are
                                if (saleLineItem.LineMultiLineDiscOnItem != LineMultilineDiscountOnItem.Both )
                                {
                                    lineAmount += custDiscLineItem.Amount;
                                    linePercentage += custDiscLineItem.Percentage;
                                }
                                //if the item has both line discount and multiline discount then use LineDiscCalculationType to decide
                                //how the discount should be calculated
                                else if (saleLineItem.LineMultiLineDiscOnItem == LineMultilineDiscountOnItem.Both)
                                {
                                    switch (((IRetailTransaction)saleLineItem.Transaction).LineDiscCalculationType)
                                    {
                                        case LineDiscCalculationTypes.Line:
                                            {
                                                if (custDiscLineItem.CustomerDiscountType == CustomerDiscountTypes.LineDiscount)
                                                {
                                                    lineAmount += custDiscLineItem.Amount;
                                                    linePercentage += custDiscLineItem.Percentage;
                                                }
                                            }
                                            break;
                                        case LineDiscCalculationTypes.MultiLine:
                                            {
                                                if (custDiscLineItem.CustomerDiscountType == CustomerDiscountTypes.MultiLineDiscount)
                                                {
                                                    lineAmount += custDiscLineItem.Amount;
                                                    linePercentage += custDiscLineItem.Percentage;
                                                }
                                            }
                                            break;
                                        case LineDiscCalculationTypes.MaxLineMultiLine:
                                            {
                                                if (custDiscLineItem.Amount > lineAmount)
                                                {
                                                    lineAmount += custDiscLineItem.Amount;
                                                }
                                                if (custDiscLineItem.Percentage > linePercentage)
                                                {
                                                    linePercentage += custDiscLineItem.Percentage;
                                                }
                                            }
                                            break;
                                        case LineDiscCalculationTypes.MinLineMultiLine:
                                            {
                                                if (((custDiscLineItem.Amount < lineAmount) && (lineAmount == 0 && custDiscLineItem.Amount > 0)))
                                                {
                                                    lineAmount += custDiscLineItem.Amount;
                                                }
                                                if (((custDiscLineItem.Percentage < linePercentage) || (linePercentage == 0 && custDiscLineItem.Percentage > 0)))
                                                {
                                                    linePercentage += discountLineItem.Percentage;
                                                }
                                            }
                                            break;
                                        case LineDiscCalculationTypes.LinePlusMultiLine:
                                            {
                                                lineAmount += custDiscLineItem.Amount;
                                                linePercentage += custDiscLineItem.Percentage;
                                            }
                                            break;

                                        case LineDiscCalculationTypes.LineMultiplyMultiLine:
                                            {
                                                if (lineAmount == 0 && custDiscLineItem.Amount > 0)
                                                {
                                                    lineAmount += custDiscLineItem.Amount;
                                                }
                                                else if (lineAmount > 0 && custDiscLineItem.Amount != 0)
                                                {
                                                    lineAmount *= custDiscLineItem.Amount;
                                                }

                                                if (linePercentage == 0 && custDiscLineItem.Percentage > 0)
                                                {
                                                    linePercentage = custDiscLineItem.Percentage;
                                                }
                                                else if (linePercentage > 0 && custDiscLineItem.Percentage != 0)
                                                {
                                                    linePercentage *= custDiscLineItem.Percentage;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Periodic and "Normal" discounts
                        else if (discountLineItem is IPeriodicDiscountItem)
                        {
                            seenPeriodicDiscount = true; 
                            //even if discounted items are returned, discount amount must still be a positive number
                            //the only scenario where the discount amount is negative is when item price is lower than the discount
                            periodicAmount = saleLineItem.Quantity < 0 
                                                ? Math.Abs(discountLineItem.Amount)
                                                :discountLineItem.Amount;
                            periodicPercentage = discountLineItem.Percentage;
                        }
                        else if (discountLineItem is ILineDiscountItem)
                        {
                            if (BanCompoundDiscounts && seenPeriodicDiscount)
                            {
                                removeItems.Add(discountLineItem);
                            }
                            else
                            {
                                seenLineDiscount = true;                                
                                lineAmount += discountLineItem.Amount;
                                linePercentage += discountLineItem.Percentage;
                            }
                        }
                        else if (discountLineItem is ILoyaltyDiscountItem)
                        {
                            if (BanCompoundDiscounts && (seenPeriodicDiscount || seenCustomerDiscount))
                            {
                                removeItems.Add(discountLineItem);
                            }
                            else
                            {
                                loyaltyAmount += discountLineItem.Amount;
                                loyaltyPercentage += discountLineItem.Percentage;
                            }
                        }
                        else if (discountLineItem is ITotalDiscountItem)
                        {
                            if (BanCompoundDiscounts && (seenPeriodicDiscount || seenCustomerDiscount || seenLineDiscount))
                            {
                                removeItems.Add(discountLineItem);
                            }
                            else
                            {
                                //TotalDiscountItem totalDiscountItem = (TotalDiscountItem)discountLineItem;
                                totalAmount += discountLineItem.Amount;
                                totalPercentage += discountLineItem.Percentage;
                            }
                        }

                        #endregion
                    }

                    if (BanCompoundDiscounts && removeItems.Count > 0)
                    {
                        foreach (var discountItem in removeItems)
                            saleLineItem.DiscountLines.Remove(discountItem);
                    }

                    // Calculating the total due amount is prohibited when dealing with fuel items.
                    // The only valid amount is the one that the POS receives from the forecourt manager
                    // We still have to calculate the total amount excluding taxes as we do not receive 
                    // that information from the forecourt manager

                    // Updating the unit prices with the unit quantity
                    //saleLineItem.UnitQuantity = saleLineItem.Quantity * saleLineItem.UnitQuantityFactor;

                    saleLineItem.GrossAmount = saleLineItem.Price * saleLineItem.Quantity;
                    if (isFuelItem)
                    {
                        //If a fuel item is sold on the POS but not through the Forecourt manager we do not have
                        //the gross amount and need to calculate it
                        if (saleLineItem.OriginatesFromForecourt == false)
                        {
                            saleLineItem.GrossAmountWithTax = saleLineItem.PriceWithTax * saleLineItem.Quantity;
                        }

                        //If the fuel item was sold through the Forecourt manager and is being returned 
                        //then we need to reverse the gross amount - if it hasn't been reversed already 
                        //(CalculateLine can be called a few times for the same item)
                        if (saleLineItem.OriginatesFromForecourt == true && saleLineItem.Quantity < 0 && saleLineItem.GrossAmountWithTax > 0)
                        {
                            saleLineItem.GrossAmountWithTax *= -1;
                        }

                        //For fuel items the quantity cannot be used or changed when calculated the discounts
                        //so a new value needs to be created.
                        calculatedQty = saleLineItem.GrossAmountWithTax / saleLineItem.PriceWithTax;
                    }
                    else if (saleLineItem.IsAssembly && !saleLineItem.PriceOverridden)
                    {
                        saleLineItem.GrossAmountWithTax = (saleLineItem.TaxExempt ? saleLineItem.PriceWithTax : saleLineItem.ItemAssembly.Price) * saleLineItem.Quantity;

                        calculatedQty = saleLineItem.Quantity;
                    }
                    else
                    {
                        saleLineItem.GrossAmountWithTax = saleLineItem.PriceWithTax * saleLineItem.Quantity;
                        //CalculatedQty needs to be used in discount calculation due to fuel items
                        //for "normal" items this will always be the quantity of the item
                        calculatedQty = saleLineItem.Quantity;
                    }

                    #region Discount and Tax Calculations

                    IRoundingService rounding = Interfaces.Services.RoundingService(entry);

#if DEBUG
                    Debug.WriteLine("Item: " + Conversion.ToStr(saleLineItem.Description));
                    Debug.WriteLine("--------------------------------------------------------------");
#endif


                    #region Calculate Discount From - Price including Tax

                    //No need to check here if the item has tax included or not because of the CalculateDiscountFrom setting that has been added to the store
                    if ((isFuelItem == true) || saleLineItem.Transaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax)
                    {
                        if (saleLineItem.GrossAmountWithTax != 0)
                        {

                            // Adjusting the quantity discounted 
                            int qtyFactor = saleLineItem.Quantity < 0 ? -1 : 1;

                            decimal tmplineDiscount = (saleLineItem.PriceWithTax - lineAmount) * calculatedQty * (100 - linePercentage) / 100;
                            saleLineItem.LinePctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmountWithTax)) * 100 *qtyFactor;
                            if (saleLineItem.LinePctDiscount > 100) { saleLineItem.LinePctDiscount = 100; }
                           
                            tmplineDiscount = (saleLineItem.PriceWithTax - periodicAmount) * calculatedQty * (100 - periodicPercentage) / 100;
                            saleLineItem.PeriodicPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmountWithTax)) * 100 * qtyFactor;
                            if (saleLineItem.PeriodicPctDiscount > 100) { saleLineItem.PeriodicPctDiscount = 100; }

                            tmplineDiscount = (saleLineItem.PriceWithTax - totalAmount) * calculatedQty * (100 - totalPercentage) / 100;
                            saleLineItem.TotalPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmountWithTax)) * 100 * qtyFactor;
                            if (saleLineItem.TotalPctDiscount > 100) { saleLineItem.TotalPctDiscount = 100; }

                            tmplineDiscount = (saleLineItem.PriceWithTax - loyaltyAmount) * calculatedQty * (100 - loyaltyPercentage) / 100;
                            saleLineItem.LoyaltyPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmountWithTax)) * 100 * qtyFactor;
                            if (saleLineItem.LoyaltyPctDiscount > 100) { saleLineItem.LoyaltyPctDiscount = 100; }
                        }

                        //Calculating the Periodic Discount from Gross amount                    
                        if (Math.Abs(saleLineItem.QuantityDiscounted) > 0)
                        {
                            // Adjusting the quantity discounted 
                            int qtyFactor = saleLineItem.Quantity < 0 ? -1 : 1;

                            //When items are being returned using ReturnItem operation then QuantityDiscounted does not become a negative number as it should
                            if (saleLineItem.Quantity < decimal.Zero && saleLineItem.QuantityDiscounted > Decimal.Zero)
                            {
                                saleLineItem.QuantityDiscounted *= -1;
                            }

                            //With periodic discounts not all of the qty in one line needs to be included in the discount
                            saleLineItem.PeriodicDiscountWithTax =
                                saleLineItem.GrossAmountWithTax -
                                (
                                //Figure out what the price is of the qty that is included in the discount (QuantityDiscounted)                                    
                                    (saleLineItem.PriceWithTax * (saleLineItem.QuantityDiscounted * qtyFactor)) * ((100 - saleLineItem.PeriodicPctDiscount) / 100)
                                    +
                                //Adding the full price for the items not included in the discount (Quantity - QuantityDiscounted)
                                    (saleLineItem.PriceWithTax * (calculatedQty - (saleLineItem.QuantityDiscounted * qtyFactor)))
                                );

                            saleLineItem.PeriodicDiscountWithTax = rounding.Round(entry, saleLineItem.PeriodicDiscountWithTax, currencyCode, cacheType);

#if DEBUG
                            Debug.WriteLine("SaleIsReturnSale: " + Conversion.ToStr(retailTransaction.SaleIsReturnSale));
                            Debug.WriteLine("QtyFactor: " + Conversion.ToStr(qtyFactor));
                            Debug.WriteLine("PeriodicDiscountWithTax: " + Conversion.ToStr(saleLineItem.PeriodicDiscountWithTax));
                            Debug.WriteLine("GrossAmountWithTax: " + Conversion.ToStr(saleLineItem.GrossAmountWithTax));
                            Debug.WriteLine("Quantity: " + Conversion.ToStr(saleLineItem.Quantity));
                            Debug.WriteLine("QuantityDiscounted: " + Conversion.ToStr(saleLineItem.QuantityDiscounted));
#endif
                        }

                        // Calculate the current Netamount w. tax by subtracting the periodic discount
                        saleLineItem.NetAmountWithTax = saleLineItem.GrossAmountWithTax - saleLineItem.PeriodicDiscountWithTax;

#if DEBUG
                        Debug.WriteLine("NetAmountWithTax: " + Conversion.ToStr(saleLineItem.NetAmountWithTax));

                        Debug.WriteLine("--------------------------------------------------------------");
#endif

                        // Calculating the Line Discount from Gross amount
                        saleLineItem.LineDiscountWithTax = (Math.Abs(saleLineItem.LinePctDiscount) / 100) * saleLineItem.NetAmountWithTax;
                        saleLineItem.LineDiscountWithTaxExact = saleLineItem.LineDiscountWithTax;
                        saleLineItem.LineDiscountWithTax = rounding.Round(entry, saleLineItem.LineDiscountWithTax, currencyCode, cacheType);

                        // Subtracting the linediscount from GrossAmount to set the value of NetAmount
                        saleLineItem.NetAmountWithTax = saleLineItem.NetAmountWithTax - saleLineItem.LineDiscountWithTaxExact;

                        // Calculating the Total Discount from what is now the value of NetAmount
                        saleLineItem.TotalDiscountWithTax = (Math.Abs(saleLineItem.TotalPctDiscount) / 100) * saleLineItem.NetAmountWithTax;
                        saleLineItem.TotalDiscountWithTaxExact = saleLineItem.TotalDiscountWithTax;
                        saleLineItem.TotalDiscountWithTax = rounding.Round(entry, saleLineItem.TotalDiscountWithTax, currencyCode, cacheType);

                        // Calculating again the instance of NetAmounts after the total discounts has been applied
                        saleLineItem.NetAmountWithTax = saleLineItem.NetAmountWithTax - saleLineItem.TotalDiscountWithTaxExact;                        

                        if (Math.Abs(saleLineItem.LoyaltyPctDiscount) > decimal.Zero)
                        {
                            saleLineItem.LoyaltyDiscountWithTax = (Math.Abs(saleLineItem.LoyaltyPctDiscount) / 100) * saleLineItem.NetAmountWithTax;
                            saleLineItem.LoyaltyDiscountWithTax = rounding.Round(entry, saleLineItem.LoyaltyDiscountWithTax, currencyCode, cacheType);

                            saleLineItem.LoyaltyPoints.CalculatedPointsAmount = saleLineItem.LoyaltyDiscountWithTax;

                            if (retailTransaction.LoyaltyItem.CalculatedPointsAmount != decimal.Zero)
                            {
                                saleLineItem.LoyaltyPoints.CalculatedPoints = saleLineItem.LoyaltyDiscountWithTax / retailTransaction.LoyaltyItem.CalculatedPointsAmount * retailTransaction.LoyaltyItem.CalculatedPoints;
                            }
                            saleLineItem.LoyaltyPoints.Relation = LoyaltyPointsRelation.Discount;

                            // Calculating again the instance of NetAmounts after the total discounts has been applied
                            saleLineItem.NetAmountWithTax = saleLineItem.NetAmountWithTax - saleLineItem.LoyaltyDiscountWithTax;
                        }
                        else
                        {
                            saleLineItem.LoyaltyDiscountWithTax = decimal.Zero;
                            saleLineItem.LoyaltyDiscount = decimal.Zero;
                        }

                        ITaxService tax = Interfaces.Services.TaxService(entry);
                        tax.CalcAmountsTaxIncluded(entry, saleLineItem, retailTransaction);

                        saleLineItem.TotalDiscountExact = saleLineItem.TotalDiscount;
                        saleLineItem.NetAmountWithTaxExact = saleLineItem.NetAmountWithTax;
                        saleLineItem.NetAmountExact = saleLineItem.NetAmount;

                        // Rounding the net amount to be used for tax calculation  
                        // CANNOT BE DONE FOR FUEL ITEMS!!!
                        if (!isFuelItem)
                        {
                            saleLineItem.NetAmountWithTax = rounding.Round(entry, saleLineItem.NetAmountWithTax, currencyCode, cacheType);
                        }

                        // Adding together all the net amounts for amounts that are not discount vouchers
                        if (!(this is IDiscountVoucherItem))
                        {
                            saleLineItem.NetAmountWithTaxWithoutDiscountVoucher = saleLineItem.NetAmountWithTax;
                        }

                        // CANNOT BE DONE FOR FUEL ITEMS!!!
                        if (!isFuelItem)
                        {
                            saleLineItem.NetAmount = rounding.Round(entry, saleLineItem.NetAmount, currencyCode, cacheType);
                            saleLineItem.GrossAmount = rounding.Round(entry, saleLineItem.GrossAmount, currencyCode, cacheType);
                            saleLineItem.GrossAmountWithTax = rounding.Round(entry, saleLineItem.GrossAmountWithTax, currencyCode, cacheType);
                        }
                    }

                    #endregion

                    #region Calculate Discount From - Price
                    else
                    {
                        if (saleLineItem.GrossAmount != 0)
                        {
                            decimal tmplineDiscount = (saleLineItem.Price - lineAmount) * calculatedQty * (100 - linePercentage) / 100;
                            saleLineItem.LinePctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmount)) * 100;
                            if (saleLineItem.LinePctDiscount > 100) { saleLineItem.LinePctDiscount = 100; }

                            tmplineDiscount = (saleLineItem.Price - periodicAmount) * calculatedQty * (100 - periodicPercentage) / 100;
                            saleLineItem.PeriodicPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmount)) * 100;
                            if (saleLineItem.PeriodicPctDiscount > 100) { saleLineItem.PeriodicPctDiscount = 100; }

                            tmplineDiscount = (saleLineItem.Price - totalAmount) * calculatedQty * (100 - totalPercentage) / 100;
                            saleLineItem.TotalPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmount)) * 100;
                            if (saleLineItem.TotalPctDiscount > 100) { saleLineItem.TotalPctDiscount = 100; }

                            tmplineDiscount = (saleLineItem.Price - loyaltyAmount) * calculatedQty * (100 - loyaltyPercentage) / 100;
                            saleLineItem.LoyaltyPctDiscount = (1 - (tmplineDiscount / saleLineItem.GrossAmount)) * 100;
                            if (saleLineItem.LoyaltyPctDiscount > 100) { saleLineItem.LoyaltyPctDiscount = 100; }
                        }


                        // Calculating the Periodic Discount                                    
                        if (Math.Abs(saleLineItem.QuantityDiscounted) > 0)
                        {
                            // Adjusting the quantity discounted 
                            int qtyFactor = saleLineItem.Quantity < 0 ? -1 : 1;
                            if (saleLineItem.Quantity < decimal.Zero && saleLineItem.QuantityDiscounted > Decimal.Zero)
                            {
                                saleLineItem.QuantityDiscounted *= -1;
                            }

                            //With periodic discounts not all of the qty in one line needs to be included in the discount
                            saleLineItem.PeriodicDiscount =
                                saleLineItem.GrossAmount -
                                (
                                //Figure out what the price is of the qty that is included in the discount (QuantityDiscounted)                                    
                                    (saleLineItem.Price * (saleLineItem.QuantityDiscounted * qtyFactor)) * ((100 - saleLineItem.PeriodicPctDiscount) / 100)
                                    +
                                //Adding the full price for the items not included in the discount (Quantity - QuantityDiscounted)
                                    (saleLineItem.Price * (calculatedQty - (saleLineItem.QuantityDiscounted * qtyFactor)))
                                );
                            saleLineItem.PeriodicDiscount = rounding.Round(entry, saleLineItem.PeriodicDiscount, currencyCode, cacheType) * qtyFactor;
                        }

                        // Calculating the instance of NetAmounts after the periodic discount has been applied
                        saleLineItem.NetAmount = saleLineItem.GrossAmount - saleLineItem.PeriodicDiscount;

                        // Calculating the Line Discount                        
                        saleLineItem.LineDiscount = (Math.Abs(saleLineItem.LinePctDiscount) / 100) * saleLineItem.NetAmount;
                        saleLineItem.LineDiscount = rounding.Round(entry, saleLineItem.LineDiscount, currencyCode, cacheType);

                        // Calculating the instance of NetAmounts after the line discount has been applied
                        saleLineItem.NetAmount = saleLineItem.NetAmount - saleLineItem.LineDiscount;

                        // Calculating the Total Discount                    
                        saleLineItem.TotalDiscount = (Math.Abs(saleLineItem.TotalPctDiscount) / 100) * (saleLineItem.NetAmount);
                        saleLineItem.TotalDiscountExact = saleLineItem.TotalDiscount;
                        saleLineItem.TotalDiscount = rounding.Round(entry, saleLineItem.TotalDiscount, currencyCode, cacheType);

                        // Calculating again the instance of NetAmounts after the total discounts has been applied
                        saleLineItem.NetAmount = saleLineItem.NetAmount - saleLineItem.TotalDiscountExact;

                        if (Math.Abs(saleLineItem.LoyaltyPctDiscount) > decimal.Zero)
                        {
                            saleLineItem.LoyaltyDiscount = (Math.Abs(saleLineItem.LoyaltyPctDiscount) / 100) * saleLineItem.NetAmount;
                            saleLineItem.LoyaltyDiscount = rounding.Round(entry, saleLineItem.LoyaltyDiscount, currencyCode, cacheType);

                            saleLineItem.LoyaltyPoints.CalculatedPointsAmount = saleLineItem.LoyaltyDiscount;

                            if (retailTransaction.LoyaltyItem.CalculatedPointsAmount != decimal.Zero)
                            {
                                saleLineItem.LoyaltyPoints.CalculatedPoints = saleLineItem.LoyaltyDiscount / retailTransaction.LoyaltyItem.CalculatedPointsAmount * retailTransaction.LoyaltyItem.CalculatedPoints;
                            }
                            saleLineItem.LoyaltyPoints.Relation = LoyaltyPointsRelation.Discount;

                            // Calculating again the instance of NetAmounts after the total discounts has been applied
                            saleLineItem.NetAmount = saleLineItem.NetAmount - saleLineItem.LoyaltyDiscount;
                        }
                        else
                        {
                            saleLineItem.LoyaltyDiscountWithTax = decimal.Zero;
                            saleLineItem.LoyaltyDiscount = decimal.Zero;
                        }

                        ITaxService tax = Interfaces.Services.TaxService(entry);
                        tax.CalcAmountsTaxExcluded(entry, saleLineItem);

                        saleLineItem.TotalDiscountWithTaxExact = saleLineItem.TotalDiscountWithTax;
                        saleLineItem.NetAmountExact = saleLineItem.NetAmount;
                        saleLineItem.NetAmountWithTaxExact = saleLineItem.NetAmountWithTax;

                        //If tax rounding is not being used then this value should not be rounded either
                        if (settings.Store.UseTaxRounding)
                        {
                            saleLineItem.NetAmount = rounding.Round(entry, saleLineItem.NetAmount, currencyCode, cacheType);
                            saleLineItem.NetAmountWithTax = rounding.Round(entry, saleLineItem.NetAmountWithTax, currencyCode, cacheType);
                            saleLineItem.GrossAmount = rounding.Round(entry, saleLineItem.GrossAmount, currencyCode, cacheType);
                            saleLineItem.GrossAmountWithTax = rounding.Round(entry, saleLineItem.GrossAmountWithTax, currencyCode, cacheType);
                        }
                    }

                    #endregion

                    //If any of the discount types is 100% then the net amount should be 0, no rounding amount should remain
                    //see ONE-9457 for a test case
                    if (saleLineItem.LinePctDiscount == 100 || saleLineItem.LoyaltyPctDiscount == 100 || saleLineItem.PeriodicPctDiscount == 100 || saleLineItem.TotalPctDiscount == 100)
                    {
                        saleLineItem.NetAmount = decimal.Zero;
                        saleLineItem.NetAmountWithTax = decimal.Zero;
                        saleLineItem.NetAmountExact = decimal.Zero;
                        saleLineItem.NetAmountWithTaxExact = decimal.Zero;
                    }

                    decimal totalTaxAmount = 0M;
                    foreach (TaxItem taxItem in saleLineItem.TaxLines)
                    {
                        //Only total tax amount needs to be calculated - as CalculateTaxExluced/Included 
                        //have already set the tax amount on individual items
                        totalTaxAmount += taxItem.Amount;
                    }

                    //Update the tax amount on the sale item with the current total tax amount   
                    saleLineItem.TaxAmount = totalTaxAmount;

                    if (settings.Store.UseTaxRounding)
                    {
                        //When price is including tax a rounding difference can be created with the tax group rounding
                        //NetAmount will take the rounding difference as the totalTaxAmount nor the PriceWithTax can be changed
                        decimal taxDiff = saleLineItem.NetAmountWithTax - saleLineItem.NetAmount - totalTaxAmount;
                        if (taxDiff != 0M)
                        {
                            saleLineItem.NetAmount += taxDiff;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Compares the discounts on each of the sale line items:
        /// -- If the sale line has both a customer and a periodic discount (other then Mix and Match) then 
        ///     the discounts are compared and the better one is chosen and the other one taken of the sale line
        /// -- If the sale line has a customer discount and a mix and match discount then the mix and match discount is always
        ///     chosen because there is no way to know the total discount of the mix and match because it consists of 2 or more sale lines.
        ///     So we assume that the mix and match is always better.
        /// </summary>
        /// <param name="saleLineItem">The sale line with the discounts that are going to be compared</param>
        /// <param name="entry">Entry into the database</param>
        /// <param name="retailTransaction">The current transaction being calculated</param>
        /// <param name="currencyCode">The currency code. If passing null then Store currency will be used. POS should pass null while Site Manager should pass a valid code</param>
        protected virtual void ComparingDiscounts(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction, RecordIdentifier currencyCode = null)
        {
            DiscountParameters discountParameters = Providers.DiscountParametersData.Get(entry, CacheType.CacheTypeApplicationLifeTime);

            LineDiscountInfo combos = GetLineDiscountInfo(saleLineItem);

            if (BanCompoundDiscounts && combos.HasPeriodicDiscount)
            {
                combos.HasCustomerDiscount = false;
                combos.HasCustomerTotalDiscount = false;
                saleLineItem.ClearCustomerDiscountLines(false);
                saleLineItem.ClearCustomerDiscountLines(true);
            }

            //If Mix & Match and Customer Total Discount were found
            //then they will override any other discounts that are found on the sale line
            //no need to compare prices or discounts.
            if (combos.HasMixAndMatchDiscount && combos.HasCustomerTotalDiscount)
            {
                saleLineItem.ClearCustomerDiscountLines(false);
                combos.HasCustomerTotalDiscount = false;
                combos.HasCustomerDiscount = false;
            }

            //If Mix & Match and Customer discount both found on the sale item 
            //then the Mix & Match will override any customer discount 
            if (combos.HasMixAndMatchDiscount && combos.HasCustomerDiscount)
            {
                saleLineItem.ClearCustomerDiscountLines(false);
                combos.HasCustomerDiscount = false;
            }

            //If a Customer Discount is found and either a Multibuy or a Discount offer
            //the best price is found from either discount and the better one chosen.
            if (combos.HasPeriodicDiscount && combos.HasCustomerDiscount)
            {
                ISaleLineItem custSaleLineItem = (ISaleLineItem)saleLineItem.Clone();
                ISaleLineItem periodicSaleLineItem = (ISaleLineItem)saleLineItem.Clone();

                switch (discountParameters.PeriodicLine)
                {
                    case PeriodicLineEnum.CustomerLine:
                        saleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                        combos.HasPeriodicDiscount = false;
                        break;
                    case PeriodicLineEnum.Periodic:
                        saleLineItem.ClearCustomerDiscountLines(false);
                        combos.HasCustomerDiscount = false;
                        break;
                    case PeriodicLineEnum.Max:
                        custSaleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                        periodicSaleLineItem.ClearCustomerDiscountLines(false);

                        CalculateLine(entry, custSaleLineItem, retailTransaction, false);
                        CalculateLine(entry, periodicSaleLineItem, retailTransaction, false);

                        if (custSaleLineItem.NetAmountWithTax >= periodicSaleLineItem.NetAmountWithTax)
                        {
                            saleLineItem.ClearCustomerDiscountLines(false);
                            combos.HasCustomerDiscount = false;
                        }
                        else
                        {
                            saleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                            combos.HasPeriodicDiscount = false;
                        }
                        break;
                    case PeriodicLineEnum.Aggregate:
                    case PeriodicLineEnum.Accumulative:
                        break;
                }
            }
            if (combos.HasPeriodicDiscount && combos.HasCustomerTotalDiscount)
            {
                ISaleLineItem custTotalSaleLineItem = (ISaleLineItem)saleLineItem.Clone();
                ISaleLineItem periodicSaleLineItem = (ISaleLineItem)saleLineItem.Clone();

                switch (discountParameters.PeriodicTotal)
                {
                    case PeriodicTotalEnum.CustomerTotal:
                        saleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                        break;
                    case PeriodicTotalEnum.Periodic:
                        saleLineItem.ClearCustomerDiscountLines(true);
                        combos.HasCustomerTotalDiscount = false;
                        break;
                    case PeriodicTotalEnum.Max:
                        custTotalSaleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                        periodicSaleLineItem.ClearCustomerDiscountLines(true);

                        CalculateLine(entry, custTotalSaleLineItem, retailTransaction, false);
                        CalculateLine(entry, periodicSaleLineItem, retailTransaction, false);

                        if (custTotalSaleLineItem.NetAmountWithTax >= periodicSaleLineItem.NetAmountWithTax)
                        {
                            saleLineItem.ClearCustomerDiscountLines(true);
                            combos.HasCustomerTotalDiscount = false;
                        }
                        else
                        {
                            saleLineItem.ClearDiscountLines(typeof(IPeriodicDiscountItem));
                        }
                        break;
                    case PeriodicTotalEnum.Aggregate:
                    case PeriodicTotalEnum.Accumulative:
                        break;
                }
            }
            if (combos.HasCustomerTotalDiscount && combos.HasCustomerDiscount)
            {
               ISaleLineItem custSaleLineItem = (ISaleLineItem)saleLineItem.Clone();
               ISaleLineItem custTotalSaleLineItem = (ISaleLineItem)saleLineItem.Clone();

                switch (discountParameters.LineTotal)
                {
                    case CustomerLineTotalEnum.CustomerTotal:
                        saleLineItem.ClearCustomerDiscountLines(false);
                        break;
                    case CustomerLineTotalEnum.CustomerLine:
                        saleLineItem.ClearCustomerDiscountLines(true);
                        break;
                    case CustomerLineTotalEnum.Max:
                        custSaleLineItem.ClearCustomerDiscountLines(true);
                        custTotalSaleLineItem.ClearCustomerDiscountLines(false);

                        CalculateLine(entry, custSaleLineItem, retailTransaction, false);
                        CalculateLine(entry, custTotalSaleLineItem, retailTransaction, false);

                        if (custSaleLineItem.NetAmountWithTax >= custTotalSaleLineItem.NetAmountWithTax)
                        {
                            saleLineItem.ClearCustomerDiscountLines(true);
                        }
                        else
                        {
                            saleLineItem.ClearCustomerDiscountLines(false);
                        }
                        break;
                    case CustomerLineTotalEnum.Aggregate:
                    case CustomerLineTotalEnum.Accumulative:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saleLineItem"></param>
        /// <param name="retailTransaction"></param>
        public virtual void CalculatePeriodicDiscountPercent(ISaleLineItem saleLineItem, IRetailTransaction retailTransaction)
        {
            bool isFuelItem = saleLineItem is IFuelSalesLineItem || saleLineItem.OriginatesFromForecourt;
            IPeriodicDiscountItem periodicDiscount = (IPeriodicDiscountItem)saleLineItem.DiscountLines.FirstOrDefault(x => x is IPeriodicDiscountItem);

            decimal periodicAmount = 0;
            decimal periodicPercentage = 0;

            if(periodicDiscount != null)
            {
                periodicAmount = Math.Abs(periodicDiscount.Amount);
                periodicPercentage = periodicDiscount.Percentage;
            }

            if (isFuelItem || retailTransaction.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax)
            {
                if (saleLineItem.GrossAmountWithTax != 0)
                {
                    int qtyFactor = saleLineItem.Quantity < 0 ? -1 : 1;

                    decimal lineDiscount = (saleLineItem.PriceWithTax - periodicAmount) * saleLineItem.Quantity * (100 - periodicPercentage) / 100;
                    saleLineItem.PeriodicPctDiscount = (1 - (lineDiscount / saleLineItem.GrossAmountWithTax)) * 100 * qtyFactor;
                    if (saleLineItem.PeriodicPctDiscount > 100) { saleLineItem.PeriodicPctDiscount = 100; }
                }
            }
            else
            {
                if (saleLineItem.GrossAmount != 0)
                {
                    decimal lineDiscount = (saleLineItem.Price - periodicAmount) * saleLineItem.Quantity * (100 - periodicPercentage) / 100;
                    saleLineItem.PeriodicPctDiscount = (1 - (lineDiscount / saleLineItem.GrossAmount)) * 100;
                    if (saleLineItem.PeriodicPctDiscount > 100) { saleLineItem.PeriodicPctDiscount = 100; }
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Goes through all the discount lines and figure out what type of discount lines are available.
        /// </summary>
        /// <param name="saleLineItem">The sale line that is being checked</param>
        /// <returns></returns>
        protected virtual LineDiscountInfo GetLineDiscountInfo(ISaleLineItem saleLineItem)
        {
            LineDiscountInfo discountInfo = new LineDiscountInfo();
            //Go through all the discount lines and figure out what type of discount lines are available.
            foreach (IDiscountItem discountLine in saleLineItem.DiscountLines)
            {
                if (discountLine is IPeriodicDiscountItem)
                {
                    var periodicDiscLineItem = (IPeriodicDiscountItem)discountLine;
                    if (periodicDiscLineItem.PeriodicDiscountType != PeriodicDiscOfferType.MixAndMatch)
                    {
                        discountInfo.HasPeriodicDiscount = true;
                    }
                    else
                    {
                        discountInfo.HasMixAndMatchDiscount = true;
                    }
                }
                else if (discountLine is ICustomerDiscountItem)
                {
                    var custDiscItem = (ICustomerDiscountItem)discountLine;
                    if (custDiscItem.CustomerDiscountType != CustomerDiscountTypes.TotalDiscount)
                    {
                        discountInfo.HasCustomerDiscount = true;
                    }
                    else
                    {
                        discountInfo.HasCustomerTotalDiscount = true;
                    }
                }
            }

            return discountInfo;
        }

        /// <inheritdoc/>
        public virtual ChangeBackAmountsInfo CalculateChangeBackAmounts(IConnectionManager entry, IPosTransaction transaction, ITenderLineItem lastTenderLineItem)
        {
            ChangeBackAmountsInfo changeBackAmountsInfo = new ChangeBackAmountsInfo();

            if (!(transaction is IRetailTransaction) && !(transaction is ICustomerPaymentTransaction))
            {
                return changeBackAmountsInfo;
            }                               

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Starting CalculateChangeBackAmounts", ToString());

            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);
            
            if(transaction is ICustomerPaymentTransaction)
            {
                changeBackAmountsInfo.CalculatedAmount = ((ICustomerPaymentTransaction)transaction).Amount;
            }
            else
            {
                if (((IRetailTransaction)transaction).CustomerOrder.Empty())
                {
                    changeBackAmountsInfo.CalculatedAmount = ((IRetailTransaction)transaction).NetAmountWithTax;
                }
                else
                {
                    changeBackAmountsInfo.CalculatedAmount = Interfaces.Services.CustomerOrderService(entry).CalculateAmountToBeTendered(entry, (IRetailTransaction)transaction, true);
                }            
            }            

            changeBackAmountsInfo.CalculatedRoundedAmount = rounding.Round(entry,
                                                    changeBackAmountsInfo.CalculatedAmount,
                                                    transaction.StoreCurrencyCode,
                                                    true,
                                                    CacheType.CacheTypeTransactionLifeTime);

            changeBackAmountsInfo.CalculatedRoundedAmountForPayment = rounding.RoundAmount(entry, changeBackAmountsInfo.CalculatedRoundedAmount, entry.CurrentStoreID, lastTenderLineItem.TenderTypeId, CacheType.CacheTypeTransactionLifeTime);

            changeBackAmountsInfo.TenderedAmount = GetTotalTransactionPaymentAmount(transaction);            

            //CustomerPaymentTransaction doesn't have a markup item
            decimal markup = decimal.Zero;
            
            if (transaction is IRetailTransaction)
            {
                markup = ((IRetailTransaction)transaction).MarkupItem.Amount;
            }

            StorePaymentMethod tenderInfo = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(entry.CurrentStoreID, new RecordIdentifier(lastTenderLineItem.TenderTypeId)), CacheType.CacheTypeApplicationLifeTime);

            decimal notPayableAmountDueToLimitations = transaction is IRetailTransaction ? PaymentLimitation.GetNotPayableAmountDueToLimitations(entry, (IRetailTransaction)transaction, tenderInfo) : decimal.Zero;

            changeBackAmountsInfo.CalculatedChangeBack = changeBackAmountsInfo.CalculatedAmount + markup - changeBackAmountsInfo.TenderedAmount- notPayableAmountDueToLimitations;
            changeBackAmountsInfo.CalculatedChangeBack = rounding.RoundAmount(entry, changeBackAmountsInfo.CalculatedChangeBack, entry.CurrentStoreID, lastTenderLineItem.ChangeTenderID);

            changeBackAmountsInfo.CalculatedRoundingDifference = changeBackAmountsInfo.CalculatedRoundedAmount + (changeBackAmountsInfo.CalculatedChangeBack * -1) - changeBackAmountsInfo.TenderedAmount - notPayableAmountDueToLimitations;

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "CalculateChangeBackAmounts finished", ToString());

            return changeBackAmountsInfo;
        }

        /// <summary>
        /// Returns the total amount already paid on a transaction
        /// </summary>
        /// <param name="posTransaction">The current transaction in the POS</param>
        /// <returns></returns>
        protected decimal GetTotalTransactionPaymentAmount(IPosTransaction posTransaction)
        {            
            if (posTransaction is ICustomerPaymentTransaction)
            {
                return ((ICustomerPaymentTransaction)posTransaction).Payment;
            }
            else if (posTransaction is IRetailTransaction)
            {
                IRetailTransaction retailTransaction = (IRetailTransaction)posTransaction;

                if (retailTransaction.CustomerOrder.Empty())
                {
                    return retailTransaction.Payment;
                }

                decimal previouslyPaid = retailTransaction.TenderLines.Where(w => !w.Voided && w.PaidDeposit).Sum(s => s.Amount);
                decimal totalTenderLines = retailTransaction.TenderLines.Where(w => !w.Voided).Sum(s => s.Amount);
                decimal currentlyBeingPaid = retailTransaction.TenderLines.Where(w => !w.Voided && !w.PaidDeposit).Sum(s => s.Amount);

                if (!retailTransaction.CustomerOrder.Empty())
                {
                    if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnFullDeposit ||
                        retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnPartialDeposit)
                    {
                        return decimal.Zero;
                    }

                    if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PayDeposit)
                    {
                        return currentlyBeingPaid > retailTransaction.CustomerOrder.DepositToBePaid ? currentlyBeingPaid : decimal.Zero;
                    }

                    if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.AdditionalPayment)
                    {
                        return currentlyBeingPaid > retailTransaction.CustomerOrder.AdditionalPayment ? currentlyBeingPaid : decimal.Zero;
                    }
                }

                if (previouslyPaid != totalTenderLines)
                {
                    return retailTransaction.TenderLines.Where(w => !w.Voided && !w.PaidDeposit).Sum(s => s.Amount);
                }
                else
                {
                    return retailTransaction.Payment;
                }
            }

            return decimal.Zero;
        }
    }
}