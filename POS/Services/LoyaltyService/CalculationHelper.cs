using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.Services
{
    public  class CalculationHelper
    {
        private decimal tenderPointsMultiplier;

        public IRetailTransaction Transaction { get; set; }
        public bool TenderRuleFound { get; set; }
        public decimal MinimumPaymentForTender { get; set; }
        public UseDialogEnum DialogUsage { get; set; }

        public decimal TenderPointsMultiplier
        {
            get { return tenderPointsMultiplier < decimal.Zero ? tenderPointsMultiplier*-1 : tenderPointsMultiplier; }
            set { tenderPointsMultiplier = value; }
        }

        public CalculationHelper(IRetailTransaction retailTransaction, UseDialogEnum dialogUsage)
        {
            Transaction = retailTransaction;
            DialogUsage = dialogUsage;
            InitializeProperties();
        }

        public CalculationHelper()
        {
            InitializeProperties();
        }

        public LoyaltyItem GetPointsforLoyaltyDiscount(IConnectionManager entry, RecordIdentifier tenderTypeID, decimal discountAmount)
        {
            TenderRuleFound = false;

            // Getting all possible loyalty possibilities for the found scheme id
            List<LoyaltyPoints> loyaltyPoints = Providers.LoyaltyPointsData.GetList(entry, Transaction.LoyaltyItem.SchemeID);

            LoyaltyItem item = GetPointsForTender(entry, tenderTypeID, discountAmount, loyaltyPoints);

            return item;
        }

        public LoyaltyItem GetPointsForAllLoyaltyTenderLines(IConnectionManager entry, ref decimal totalNumberOfPoints, RecordIdentifier tenderTypeID, decimal amount)
        {
            TenderRuleFound = false;

            // Getting all possible loyalty possibilities for the found scheme id
            List<LoyaltyPoints> loyaltyPoints = Providers.LoyaltyPointsData.GetList(entry, Transaction.LoyaltyItem.SchemeID);

            totalNumberOfPoints = 0;

            // To be sure that there will be queried for the whole loyalty amount in this transaction
            // we have to loop through all the LoyaltyTenders and do the calculations for them again (if any exist)
            foreach (TenderLineItem tenderline in Transaction.TenderLines.Where(w => w.LoyaltyPoints.RuleID != RecordIdentifier.Empty && w.TenderTypeId == tenderTypeID))
            {
                tenderline.LoyaltyPoints = GetPointsForTender(entry, tenderline.TenderTypeId, tenderline.Amount, loyaltyPoints);
                totalNumberOfPoints += tenderline.LoyaltyPoints.CalculatedPoints;
            }

            // now we add the points needed to pay current tender
            LoyaltyItem calculatedPoints = GetPointsForTender(entry, tenderTypeID, amount, loyaltyPoints);

            totalNumberOfPoints += calculatedPoints.CalculatedPoints;

            return calculatedPoints;
        }

        public LoyaltyItem GetPointsForTender(IConnectionManager entry, RecordIdentifier tenderTypeID, decimal amount, List<LoyaltyPoints> loyaltyPointsList)
        {
            if (loyaltyPointsList == null || (loyaltyPointsList != null && !loyaltyPointsList.Any()))
            {
                loyaltyPointsList = Providers.LoyaltyPointsData.GetList(entry, Transaction.LoyaltyItem.SchemeID);
            }

            TenderRuleFound = false;
            LoyaltyItem calculatedPoints = new LoyaltyItem();

            foreach (LoyaltyPoints points in loyaltyPointsList.Where(w => w.Type == LoyaltyPointTypeBase.Tender && w.SchemeRelation == tenderTypeID))
            {
                Date nowDate = Date.Now;
                Date emptyDate = Date.Empty;

                if ((points.StartingDate.DateTime.Date <= nowDate.DateTime.Date) && (nowDate.DateTime.Date <= points.EndingDate.DateTime.Date)
                        || ((points.StartingDate.DateTime.Date <= nowDate.DateTime.Date) && (points.EndingDate.DateTime.Date == emptyDate.DateTime.Date))
                        || ((points.StartingDate.DateTime.Date == emptyDate.DateTime.Date) && (points.EndingDate.DateTime.Date == emptyDate.DateTime.Date))
                    )
                {
                    //In previous implementations the user was allowed to enter both negative and positive points values in rules setup
                    //Current SM implementation doesn't allow this so now when calculating tender points for payment the points should always be negative.
                    points.Points = points.Points > decimal.Zero ? points.Points * -1 : points.Points;

                    MinimumPaymentForTender = points.QtyAmountLimit;

                    if ((Math.Abs(amount) >= points.QtyAmountLimit) && (points.BaseCalculationOn == CalculationTypeBase.Amounts) && (points.QtyAmountLimit > 0))
                    {
                        TenderRuleFound = true;
                        tenderPointsMultiplier = points.QtyAmountLimit * points.Points;
                        SetLoyaltyPoints(entry, calculatedPoints, amount / points.QtyAmountLimit, points, 1M);
                    }
                }
            }
            return calculatedPoints;
        }

        public void SetLoyaltyPoints(IConnectionManager entry, LoyaltyItem loyaltyItem, decimal multiplier, LoyaltyPoints points, decimal quantity)
        {
            loyaltyItem.CalculatedPoints = Interfaces.Services.RoundingService(entry).RoundAmount(entry, multiplier * points.Points, 1M, GetRoundMethod(), CacheType.CacheTypeApplicationLifeTime);
            loyaltyItem.RuleID = points.RuleID;
            loyaltyItem.AggregatedItemQuantity = quantity;
        }

        public TenderRoundMethod GetRoundMethod()
        {
            return (DialogUsage == UseDialogEnum.PointsDiscount || DialogUsage == UseDialogEnum.PointsPayment) ? TenderRoundMethod.RoundUp : TenderRoundMethod.RoundDown;
        }

        /// <summary>
        /// Gets the loyalty info.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyItem">The loyalty item.</param>
        public LoyaltyMSRCard GetLoyaltyInfo(IConnectionManager entry, ILoyaltyItem loyaltyItem)
        {
            LoyaltyMSRCard card = Providers.LoyaltyMSRCardData.Get(entry, loyaltyItem.CardNumber);

            if (card == null || card.CustomerID == RecordIdentifier.Empty)
            {
                try
                {
                    ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                    card = Interfaces.Services.SiteServiceService(entry).GetLoyaltyMSRCard(entry, settings.SiteServiceProfile, loyaltyItem.CardNumber);
                }
                catch (Exception ex)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(ex is ClientTimeNotSynchronizedException ? ex.Message : Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return null;
                }
                
                if (card == null)
                {
                    return null;
                }
            }

            //Retrieve the loyalty scheme and loyalty customer ID from the card
            loyaltyItem.SchemeID = card.SchemeID;
            loyaltyItem.CustomerID = card.CustomerID;

            //Get information from the loyalty scheme necessary for calculation of loyalty points
            LoyaltySchemes schemes = Providers.LoyaltySchemesData.Get(entry, loyaltyItem.SchemeID);
            if (schemes != null)
            {
                loyaltyItem.ExpireUnit = schemes.ExpirationTimeUnit;
                loyaltyItem.ExpireValue = schemes.ExpireTimeValue;
                loyaltyItem.UsePointsLimit = schemes.UseLimit;
            }
            else
            {
                loyaltyItem.ExpireUnit = 0;
                loyaltyItem.ExpireUnit = TimeUnitEnum.None;
                loyaltyItem.UsePointsLimit = card.UsePointsLimit;
            }
            
            return card;
        }

        public void AddCustomerToTransaction(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //If the loyalty customer is already on the transaction there is no need to continue
            if (retailTransaction.LoyaltyItem.CustomerID != RecordIdentifier.Empty && retailTransaction.LoyaltyItem.CustomerID == retailTransaction.Customer.ID)
            {
                return;
            }

            Customer customer = Providers.CustomerData.Get(entry, retailTransaction.LoyaltyItem.CustomerID, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime);

            if (customer != null)
            {
                if (retailTransaction.Add(customer))
                {
                    retailTransaction.AddInvoicedCustomer(Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }

                Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, retailTransaction, true, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, retailTransaction);
            }
        }

        public decimal GetTotalAmtUsedForDiscount(IRetailTransaction retailTransaction, bool includeLoyaltyDiscount)
        {
            decimal totalAmt = decimal.Zero;
            foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(w => w.Voided == false))
            {
                if (saleItem.GrossAmountWithTax > decimal.Zero && saleItem.GrossAmount > decimal.Zero && saleItem.NoDiscountAllowed == false && saleItem.Quantity != 0 && !saleItem.DiscountLines.Any(f => includeLoyaltyDiscount ? f.DiscountType != DiscountTransTypes.LoyaltyDisc : f != null))
                {
                    if (DialogUsage != UseDialogEnum.PointsPayment)
                    {
                        switch (retailTransaction.CalculateDiscountFrom)
                        {
                            case Store.CalculateDiscountsFromEnum.PriceWithTax:
                                totalAmt += saleItem.NetAmountWithTax + saleItem.LoyaltyDiscountWithTax;
                                break;
                            case Store.CalculateDiscountsFromEnum.Price:
                                totalAmt += saleItem.NetAmount + saleItem.LoyaltyDiscount;
                                break;
                        }
                    }
                    else
                    {
                        totalAmt += saleItem.NetAmountWithTax + saleItem.LoyaltyDiscountWithTax;
                    }
                    
                }
                else
                {
                    saleItem.DiscountUnsuccessfullyApplied = true;
                }
            }

            return totalAmt;
        }

        private void InitializeProperties()
        {
            TenderRuleFound = false;
            TenderPointsMultiplier = decimal.Zero;
            MinimumPaymentForTender = decimal.Zero;
        }
    }
}