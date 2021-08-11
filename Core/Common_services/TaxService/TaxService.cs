using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class TaxService : ITaxService
    {
        IErrorLog errorLog;

        public virtual IErrorLog ErrorLog
        {
            set
            {
                errorLog = value;
            }
        }

        public void Init(IConnectionManager entry)
        {

        }

        /// <summary>
        /// Calculates the tax for a specific item in the transaction.
        /// </summary>
        /// <param name="entry">Entry into the data framework</param>
        /// <param name="retailTransaction">The transaction to be calculated</param>
        /// <param name="saleLineItem">The sale line item to calculate tax for</param>
        public virtual void CalculateTax(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem)
        {
            // If this is a new item and the transaction has been tax exempted before the item was added to the transaction we
            // need to mark the new item as tax exempted
            if (retailTransaction.TaxExempt && !saleLineItem.TaxExempt)
            {
                saleLineItem.TaxExempt = true;
                saleLineItem.TaxExemptionCode = retailTransaction.TransactionTaxExemptionCode;
            }

            if (saleLineItem.TaxIncludedInItemPrice)
            {
                CalcTaxIncluded(entry, saleLineItem, retailTransaction);
            }
            else
            {
                CalcTaxExcluded(entry, saleLineItem, retailTransaction);
            }
        }

        public virtual void CalcTaxIncluded(IConnectionManager entry, ISaleLineItem lineItem, IRetailTransaction rt)
        {
            try
            {
                //Get the tax rate with or without customer                     
                GetTaxRate(entry, lineItem, lineItem.ItemId, rt.Customer.ID, rt.Hospitality.ActiveHospitalitySalesType, rt.UseTaxGroupFrom, rt.UseOverrideTaxGroup, rt.OverrideTaxGroup);
                POSOperations lastOperation = rt.OperationStack.Count > 0 ? rt.OperationStack[rt.OperationStack.Count - 1] : POSOperations.NoOperation;
                //Find the combines tax rate %
                foreach (TaxItem taxItem in lineItem.TaxLines)
                {
                    lineItem.TaxRatePct += taxItem.Percentage;
                }

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                decimal originalPriceWithTax = Interfaces.Services.CurrencyService(entry).CurrencyToCurrencyNoRounding(
                                                entry,
                                                settings.CompanyInfo.CurrencyCode,
                                                settings.Store.Currency,
                                                settings.CompanyInfo.CurrencyCode,
                                                lineItem.IsAssembly ? lineItem.ItemAssembly.Price : lineItem.OriginalPriceWithTax);

                //Calculate the price without tax using the item tax group      
                decimal price;
                decimal unitConversion = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(entry, lineItem.OrgUnitOfMeasure, lineItem.SalesOrderUnitOfMeasure, lineItem.ItemId);

                if (lineItem.TaxExempt
                    &&
                    (((lineItem.PriceWithTax > 0
                      && lineItem.OriginalPriceWithTax > 0)
                      && lineItem.PriceWithTax != lineItem.OriginalPriceWithTax)
                      || (lineItem.KeyInPrice == KeyInPrices.MustKeyInNewPrice)))
                {
                    // Check if we are coming from the price override operation, because then we need to use the price entered by the user
                    if (lineItem.PriceOverridden)
                    {
                        price = lineItem.PriceOverrideAmount / (1 + (lineItem.TaxRatePct / 100));
                    }
                    else
                    {
                        if (lineItem.PriceType == Interfaces.Enums.TradeAgreementPriceType.BasePrice)
                        {
                            price = (originalPriceWithTax * unitConversion) / (1 + (lineItem.TaxRatePct / 100));
                        }
                        else
                        {
                            price = lineItem.PriceWithTax / (1 + (lineItem.TaxRatePct / 100));
                        }
                    }
                }
                else
                {
                    if (lineItem.PriceOverridden && string.IsNullOrEmpty(rt.OrgTransactionId))
                    {
                        price = lineItem.PriceOverrideAmount / (1 + (lineItem.TaxRatePct / 100));
                    }
                    else
                    {
                        price = lineItem.PriceWithTax / (1 + (lineItem.TaxRatePct / 100));
                    }
                }

                lineItem.Price = price;

                decimal taxAmount = 0;

                IRoundingService rounding = Interfaces.Services.RoundingService(entry);

                foreach (TaxItem taxItem in lineItem.TaxLines)
                {
                    //Each tax amount rounded depending on rounding settings on the tax group
                    taxAmount += rounding.TaxRound(entry, lineItem.Price * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                }

                // This checks if we have already calculated a tax exempted price before
                if (lineItem.TaxExempt
                    &&
                    (((lineItem.PriceWithTax > 0
                       && lineItem.OriginalPriceWithTax > 0)
                      && lineItem.PriceWithTax != lineItem.OriginalPriceWithTax)
                     || (lineItem.KeyInPrice == KeyInPrices.MustKeyInNewPrice)))
                {
                    if (lineItem.PriceOverridden)
                    {
                        lineItem.Price = lineItem.PriceOverrideAmount - taxAmount;
                    }
                    else
                    {
                        if (lineItem.PriceType == Interfaces.Enums.TradeAgreementPriceType.BasePrice)
                        {
                            lineItem.Price = (originalPriceWithTax * unitConversion) - taxAmount;
                        }
                        else
                        {
                            lineItem.Price = lineItem.PriceWithTax - taxAmount;
                        }
                    }
                }
                else
                {
                    // Check if we are clearing tax exemption form an item 
                    if (lineItem.PriceOverridden && string.IsNullOrEmpty(rt.OrgTransactionId))
                    {
                        // Because of price override we need to restore the price w/ tax
                        lineItem.PriceWithTax = lineItem.Price + taxAmount;
                    }
                    else
                    {
                        lineItem.Price = lineItem.PriceWithTax - taxAmount;
                    }
                }

                // Tax exemption:
                // Only viable to actually perform the tax exemption here, since we need to know the price without tax when using price including tax calculations.
                if (lineItem.TaxExempt)
                {
                    lineItem.TaxLines.Clear();

                    TaxItem taxExemptedItem = new TaxItem();
                    taxExemptedItem.Percentage = 0;
                    taxExemptedItem.TaxCode = "";
                    taxExemptedItem.ItemTaxGroup = "";
                    taxExemptedItem.TaxRoundOff = 0;
                    taxExemptedItem.TaxRoundOffType = 0;
                    taxExemptedItem.TaxCodeDisplay = "";
                    taxExemptedItem.ItemTaxGroupDisplay = "";
                    taxExemptedItem.TaxExempt = true;
                    taxExemptedItem.TaxExemptionCode = lineItem.TaxExemptionCode;

                    lineItem.Add(taxExemptedItem);

                    lineItem.TaxRatePct = 0;
                    lineItem.PriceWithTax = lineItem.Price;
                }
            }
            catch (Exception x)
            {
                if (errorLog != null)
                {
                    errorLog.LogMessage(LogMessageType.Error, ToString(), x);
                }

                throw x;
            }
        }

        public virtual void CalcTaxExcluded(IConnectionManager entry, ISaleLineItem lineItem, IRetailTransaction rt)
        {
            try
            {
                GetTaxRate(entry, lineItem, lineItem.ItemId, rt.Customer.ID, rt.Hospitality.ActiveHospitalitySalesType, rt.UseTaxGroupFrom, rt.UseOverrideTaxGroup, rt.OverrideTaxGroup);

                // Tax exemption    
                if (lineItem.TaxExempt)
                {
                    lineItem.TaxLines.Clear();

                    TaxItem taxExemptedItem = new TaxItem();
                    taxExemptedItem.Percentage = 0;
                    taxExemptedItem.TaxCode = "";
                    taxExemptedItem.ItemTaxGroup = "";
                    taxExemptedItem.TaxRoundOff = 0;
                    taxExemptedItem.TaxRoundOffType = 0;
                    taxExemptedItem.TaxCodeDisplay = "";
                    taxExemptedItem.ItemTaxGroupDisplay = "";
                    taxExemptedItem.TaxExempt = true;
                    taxExemptedItem.TaxExemptionCode = lineItem.TaxExemptionCode;

                    lineItem.Add(taxExemptedItem);

                    lineItem.TaxRatePct = 0;
                }

                decimal taxAmount = 0;

                IRoundingService rounding = Interfaces.Services.RoundingService(entry);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                bool priceOverridenWithChangedTaxSetting = lineItem.PriceOverridden
                    && lineItem.OriginalTaxIncludedInItemPrice != lineItem.TaxIncludedInItemPrice
                    && !lineItem.OriginalTaxIncludedInItemPrice
                    && lineItem.TaxIncludedInItemPrice;
                
                bool calculatePriceFromPriceWithTax = lineItem.PriceOverridden && !lineItem.TaxIncludedInItemPrice && settings.Store.KeyedInPriceIncludesTax;
                bool hasTax = !(lineItem.TaxExempt || lineItem.TaxLines.Sum(x => x.Percentage) == 0);

                decimal price = priceOverridenWithChangedTaxSetting ? lineItem.PriceOverrideAmount : lineItem.Price;

                if(hasTax && ((lineItem.IsAssembly && !lineItem.TaxIncludedInItemPrice) || calculatePriceFromPriceWithTax))
                {
                    //If it's an assembly item, calculate price from price with tax instead since the price with tax is set on the assembly
                    lineItem.PriceWithTax = calculatePriceFromPriceWithTax ? lineItem.PriceOverrideAmount : lineItem.ItemAssembly.Price;

                    RecordIdentifier salesTaxGroup = GetSalesTaxGroup(entry, settings, lineItem, rt);
                    lineItem.Price = CalculatePriceFromPriceWithTax(entry, lineItem.PriceWithTax, lineItem.TaxGroupId, salesTaxGroup);

                    foreach (TaxItem taxItem in lineItem.TaxLines)
                    {
                        lineItem.TaxRatePct += taxItem.Percentage;
                    }
                }
                else
                {
                    foreach (TaxItem taxItem in lineItem.TaxLines)
                    {
                        taxAmount += rounding.TaxRound(entry, price * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                        lineItem.TaxRatePct += taxItem.Percentage;
                    }
                    lineItem.PriceWithTax = price + taxAmount;
                }
            }
            catch (Exception x)
            {
                if (errorLog != null)
                {
                    errorLog.LogMessage(LogMessageType.Error, this.ToString(), x);
                }
                throw x;
            }
        }

        protected virtual void GetTaxRate(IConnectionManager entry, ISaleLineItem lineItem, RecordIdentifier itemID, RecordIdentifier customerID, RecordIdentifier salesTypeID, UseTaxGroupFromEnum useTaxGroupFrom, bool useOverrideTaxGroup, RecordIdentifier overrideTaxGroup)
        {
            try
            {
                //Clear the tax code so that RetailTransaction.AddToTaxItems doesn't recalc the tax onto the item
                //if the item originally had a tax code (i.e. customer with no tax code added afterwards)                    
                lineItem.TaxRatePct = 0;

                lineItem.TaxGroupId = null;
                lineItem.SalesTaxGroupId = null;
                lineItem.TaxLines.Clear();

                List<TaxItem> taxRates = Providers.TaxItemData.GetTaxRate(entry, itemID, customerID, salesTypeID, useTaxGroupFrom, useOverrideTaxGroup, overrideTaxGroup, CacheType.CacheTypeTransactionLifeTime);

                if (taxRates.Count > 0)
                {
                    lineItem.TaxGroupId = (string)taxRates[0].ItemTaxGroup;
                    lineItem.SalesTaxGroupId = (string)taxRates[0].StoreTaxGroup;

                    foreach (TaxItem ti in taxRates)
                    {
                        ti.Amount = decimal.Zero; //Calculated field and due to caching this has to be set to zero if and when the tax rates are retrieved from the cache rather then data.
                        lineItem.Add(ti);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void CalcAmountsTaxExcluded(IConnectionManager entry, ISaleLineItem saleLineItem)
        {
            try
            {
                decimal taxAmount = 0;
                decimal grTaxAmount = 0;
                decimal srpTaxAmount = 0;
                decimal ldTaxAmount = 0;
                decimal pdTaxAmount = 0;
                decimal tdTaxAmount = 0;
                decimal puTaxAmount = 0;
                decimal naTaxAmount = 0;
                decimal lpTaxAmount = 0;

                IRoundingService rounding = Interfaces.Services.RoundingService(entry);

                foreach (TaxItem taxItem in saleLineItem.ITaxLines)
                {
                    //Each tax amount rounded depending on rounding settings on the tax group => TaxRound

                    //Calculate tax for each amount
                    taxAmount += rounding.TaxRound(entry, saleLineItem.Price * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    grTaxAmount += rounding.TaxRound(entry, saleLineItem.GrossAmount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    srpTaxAmount += rounding.TaxRound(entry, saleLineItem.StandardRetailPrice * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    ldTaxAmount += rounding.TaxRound(entry, saleLineItem.LineDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    pdTaxAmount += rounding.TaxRound(entry, saleLineItem.PeriodicDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    tdTaxAmount += rounding.TaxRound(entry, saleLineItem.TotalDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    puTaxAmount += rounding.TaxRound(entry, saleLineItem.PriceUnit * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    lpTaxAmount += rounding.TaxRound(entry, saleLineItem.LoyaltyDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);

                    //Get the tax amount from the price for this tax group and set the
                    //tax amount onto the tax group
                    taxItem.Amount = rounding.TaxRound(entry, saleLineItem.NetAmount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    naTaxAmount += taxItem.Amount;
                }
                saleLineItem.PriceWithTax = saleLineItem.Price + taxAmount;
                saleLineItem.GrossAmountWithTax = saleLineItem.GrossAmount + grTaxAmount;
                saleLineItem.StandardRetailPriceWithTax = saleLineItem.StandardRetailPrice + srpTaxAmount;
                saleLineItem.LineDiscountWithTax = saleLineItem.LineDiscount + ldTaxAmount;
                saleLineItem.PeriodicDiscountWithTax = saleLineItem.PeriodicDiscount + pdTaxAmount;
                saleLineItem.TotalDiscountWithTax = saleLineItem.TotalDiscount + tdTaxAmount;
                saleLineItem.PriceUnitWithTax = saleLineItem.PriceUnit + puTaxAmount;
                saleLineItem.NetAmountWithTax = saleLineItem.NetAmount + naTaxAmount;
                saleLineItem.LoyaltyDiscountWithTax = saleLineItem.LoyaltyDiscount + lpTaxAmount;
            }
            catch (Exception x)
            {
                if (errorLog != null)
                {
                    errorLog.LogMessage(LogMessageType.Error, this.ToString(), x);
                }

                throw x;
            }
        }

        protected virtual RecordIdentifier GetSalesTaxGroup(IConnectionManager entry, ISettings settings, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction)
        {
            switch (retailTransaction.UseTaxGroupFrom)
            {
                case UseTaxGroupFromEnum.Customer:
                    if (retailTransaction.Customer == null
                        || RecordIdentifier.IsEmptyOrNull(retailTransaction.Customer.ID)
                        || (retailTransaction.Customer.TaxExempt == TaxExemptEnum.No && RecordIdentifier.IsEmptyOrNull(retailTransaction.Customer.TaxGroup)))
                    {
                        return settings.Store.TaxGroup;
                    }
                    else
                    {
                        return retailTransaction.Customer.TaxGroup;
                    }

                case UseTaxGroupFromEnum.SalesType:
                    if (retailTransaction.Hospitality.ActiveHospitalitySalesType == null || retailTransaction.Hospitality.ActiveHospitalitySalesType == "" || retailTransaction.Hospitality.ActiveHospitalitySalesType == RecordIdentifier.Empty)
                    {
                        return settings.Store.TaxGroup;
                    }
                    else
                    {
                        SalesType st = Providers.SalesTypeData.Get(entry, retailTransaction.Hospitality.ActiveHospitalitySalesType);
                        return st.TaxGroupID;
                    }

                default:
                    return settings.Store.TaxGroup;
            }

        }

        public virtual void CalcAmountsTaxIncluded(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                RecordIdentifier salesTaxGroup = GetSalesTaxGroup(entry, settings, saleLineItem, retailTransaction);

                saleLineItem.Price = saleLineItem.PriceWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.PriceWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.Price = saleLineItem.PriceWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.GrossAmount = saleLineItem.GrossAmountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.GrossAmountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.GrossAmount = saleLineItem.GrossAmountWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.StandardRetailPrice = saleLineItem.StandardRetailPriceWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.StandardRetailPriceWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.StandardRetailPrice = saleLineItem.StandardRetailPriceWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.LineDiscount = saleLineItem.LineDiscountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.LineDiscountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.LineDiscount = saleLineItem.LineDiscountWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.PeriodicDiscount = saleLineItem.PeriodicDiscountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.PeriodicDiscountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.PeriodicDiscount = saleLineItem.PeriodicDiscountWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.TotalDiscount = saleLineItem.TotalDiscountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.TotalDiscountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.TotalDiscount = saleLineItem.TotalDiscountWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.LoyaltyDiscount = saleLineItem.LoyaltyDiscountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.LoyaltyDiscountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;

                saleLineItem.PriceUnit = saleLineItem.PriceUnitWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.PriceUnitWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.PriceUnit = saleLineItem.PriceUnitWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                saleLineItem.NetAmount = saleLineItem.NetAmountWithTax != decimal.Zero ? CalculatePriceFromPriceWithTax(entry, saleLineItem.NetAmountWithTax, saleLineItem.TaxGroupId, salesTaxGroup) : decimal.Zero;
                //saleLineItem.NetAmount = saleLineItem.NetAmountWithTax / (1 + (saleLineItem.TaxRatePct / 100));

                decimal taxAmount = 0;
                decimal grTaxAmount = 0;
                decimal srpTaxAmount = 0;
                decimal ldTaxAmount = 0;
                decimal pdTaxAmount = 0;
                decimal tdTaxAmount = 0;
                decimal puTaxAmount = 0;
                decimal naTaxAmount = 0;
                decimal lTaxAmount = 0;

                IRoundingService rounding = Interfaces.Services.RoundingService(entry);

                foreach (TaxItem taxItem in saleLineItem.ITaxLines)
                {
                    //Each tax amount rounded depending on rounding settings on the tax group => TaxRound

                    //Calculate tax for each amount
                    taxAmount += rounding.TaxRound(entry, saleLineItem.Price * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    grTaxAmount += rounding.TaxRound(entry, saleLineItem.GrossAmount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    srpTaxAmount += rounding.TaxRound(entry, saleLineItem.StandardRetailPrice * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    ldTaxAmount += rounding.TaxRound(entry, saleLineItem.LineDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    pdTaxAmount += rounding.TaxRound(entry, saleLineItem.PeriodicDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    tdTaxAmount += rounding.TaxRound(entry, saleLineItem.TotalDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    lTaxAmount += rounding.TaxRound(entry, saleLineItem.LoyaltyDiscount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    puTaxAmount += rounding.TaxRound(entry, saleLineItem.PriceUnit * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);

                    //Get the tax amount from the price for this tax group and set the
                    //tax amount onto the tax group
                    taxItem.Amount = rounding.TaxRound(entry, saleLineItem.NetAmount * (taxItem.Percentage / 100), taxItem.TaxRoundOff, taxItem.TaxRoundOffType);
                    naTaxAmount += taxItem.Amount;
                }

                saleLineItem.Price = saleLineItem.PriceWithTax - taxAmount;
                saleLineItem.GrossAmount = saleLineItem.GrossAmountWithTax - grTaxAmount;
                saleLineItem.StandardRetailPrice = saleLineItem.StandardRetailPriceWithTax - srpTaxAmount;
                saleLineItem.LineDiscount = saleLineItem.LineDiscountWithTax - ldTaxAmount;
                saleLineItem.PeriodicDiscount = saleLineItem.PeriodicDiscountWithTax - pdTaxAmount;
                saleLineItem.TotalDiscount = saleLineItem.TotalDiscountWithTax - tdTaxAmount;
                saleLineItem.LoyaltyDiscount = saleLineItem.LoyaltyDiscountWithTax - lTaxAmount;
                saleLineItem.PriceUnit = saleLineItem.PriceUnitWithTax - puTaxAmount;
                saleLineItem.NetAmount = saleLineItem.NetAmountWithTax - naTaxAmount;
            }
            catch (Exception x)
            {
                if (errorLog != null)
                {
                    errorLog.LogMessage(LogMessageType.Error, this.ToString(), x);
                }

                throw x;
            }
        }

        /// <summary>
        /// Returns common tax values between the itemSalesTaxGroup and the salesTaxGroup
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemSalesTaxGroupID"></param>
        /// <param name="salesTaxGroupID"></param>
        /// <returns></returns>
        public virtual List<TaxCode> GetCommonTaxCodes(
            IConnectionManager entry,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID)
        {
            List<TaxCodeInItemSalesTaxGroup> taxCodesInItemSalesTaxGroup = Providers.ItemSalesTaxGroupData.GetTaxCodesInItemSalesTaxGroup(entry, itemSalesTaxGroupID, 0, false, CacheType.CacheTypeApplicationLifeTime);
            List<TaxCodeInSalesTaxGroup> taxCodesInSalesTaxGroup = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroup(entry, salesTaxGroupID, 0, false, CacheType.CacheTypeApplicationLifeTime);

            List<RecordIdentifier> taxCodeIDsInBothGroup = (from t in taxCodesInItemSalesTaxGroup
                                                            where t.TaxValue != -1  // -1 means we got an taxcode that has expired
                                                            select t.TaxCode).Intersect(from t2 in taxCodesInSalesTaxGroup select t2.TaxCode).ToList();

            List<TaxCode> taxCodesInBothGroups = new List<TaxCode>();
            foreach (RecordIdentifier taxCodeID in taxCodeIDsInBothGroup)
            {
                TaxCode taxCode = Providers.TaxCodeData.Get(entry, taxCodeID, CacheType.CacheTypeApplicationLifeTime);
                taxCodesInBothGroups.Add(taxCode);
            }

            return taxCodesInBothGroups;
        }

        /// <summary>
        /// Returns common tax values between two salesTaxGroup
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="salesTaxGroupID1"></param>
        /// <param name="salesTaxGroupID2"></param>
        /// <returns></returns>
        protected virtual List<TaxCode> GetCommonTaxCodesBetweenSalesTaxGroups(
            IConnectionManager entry,
            RecordIdentifier salesTaxGroupID1,
            RecordIdentifier salesTaxGroupID2)
        {
            List<TaxCodeInSalesTaxGroup> taxCodesInSalesTaxGroup1 = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroup(entry, salesTaxGroupID1, 0, false);
            List<TaxCodeInSalesTaxGroup> taxCodesInSalesTaxGroup2 = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroup(entry, salesTaxGroupID2, 0, false);

            List<RecordIdentifier> taxCodeIDsInBothGroup = (from t in taxCodesInSalesTaxGroup1
                                                            select t.TaxCode).Intersect(from t2 in taxCodesInSalesTaxGroup2 select t2.TaxCode).ToList();

            List<TaxCode> taxCodesInBothGroups = new List<TaxCode>();
            foreach (RecordIdentifier taxCodeID in taxCodeIDsInBothGroup)
            {
                TaxCode taxCode = Providers.TaxCodeData.Get(entry, taxCodeID, CacheType.CacheTypeApplicationLifeTime);
                taxCodesInBothGroups.Add(taxCode);
            }

            return taxCodesInBothGroups;
        }

        protected virtual decimal CalculateTax(
            IConnectionManager entry,
            decimal price,
            List<TaxCode> taxCodes)
        {
            decimal taxAmount = 0.0M;

            IRoundingService rounding = Interfaces.Services.RoundingService(entry);

            foreach (TaxCode tax in taxCodes)
            {
                foreach (TaxCodeValue taxCodeLine in tax.TaxCodeLines)
                {
                    if (TaxCodeLineIsValid(taxCodeLine))
                    {
                        taxAmount += rounding.TaxRound(entry, price * (taxCodeLine.Value / 100), tax.TaxRoundOff, (int)tax.TaxRoundOffType);
                        break;
                    }
                }
            }

            return taxAmount;
        }

        protected virtual bool TaxCodeLineIsValid(TaxCodeValue taxCodeLine)
        {
            return taxCodeLine.FromDate.DateTime <= DateTime.Now.Date &&
                   (taxCodeLine.ToDate.DateTime >= DateTime.Now.Date || taxCodeLine.ToDate.IsEmpty);
        }

        public virtual decimal CalculateTax(
            IConnectionManager entry,
            decimal priceWithoutTax,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID)
        {
            List<TaxCode> taxCodes = GetCommonTaxCodes(entry, itemSalesTaxGroupID, salesTaxGroupID);

            return CalculateTax(entry, priceWithoutTax, taxCodes);
        }

        public virtual decimal CalculateTaxBetweenSalesTaxGroups(
            IConnectionManager entry,
            decimal price,
            RecordIdentifier salesTaxGroupID1,
            RecordIdentifier salesTaxGroupID2)
        {
            List<TaxCode> taxCodes = GetCommonTaxCodesBetweenSalesTaxGroups(entry, salesTaxGroupID1, salesTaxGroupID2);

            return CalculateTax(entry, price, taxCodes);
        }

        /// <summary>
        /// Calculates Price with tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="price">The price without tax</param>
        /// <param name="hasLastKnownPriceWithTax">True if we supply lastknownPriceWithTax, else false</param>
        /// <param name="lastKnownPriceWithTax">Last known price with tax, this is for the system to be able to correct it self from rounding inaccurancy</param>
        /// <param name="priceWithTaxLimiter">Limiter that is used for the price with tax</param>
        /// <param name="itemSalesTaxGroupID">Item sales tax group ID</param>
        /// <param name="salesTaxGroupID">The sales tax group ID</param>
        /// <returns>The calculated tax value</returns>
        public virtual decimal CalculatePriceWithTax(
            IConnectionManager entry,
            decimal price,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID,
            bool hasLastKnownPriceWithTax,
            decimal lastKnownPriceWithTax,
            DecimalLimit priceWithTaxLimiter)
        {
            decimal priceWithTax;

            priceWithTax = price + CalculateTax(entry, price, itemSalesTaxGroupID, salesTaxGroupID);

            if (hasLastKnownPriceWithTax)
            {
                decimal minVariation = (decimal)(1.0 / Math.Pow(10.0, priceWithTaxLimiter.Max));

                if ((Math.Abs(priceWithTax - lastKnownPriceWithTax) <= minVariation))
                {
                    priceWithTax = lastKnownPriceWithTax;
                }
            }

            return priceWithTax;
        }

        public virtual decimal CalculatePriceFromPriceWithTax(
            IConnectionManager entry,
            decimal priceWithTax,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID)
        {
            decimal totalPercent = 0.0M;
            decimal guessedPrice;
            DecimalLimit limit = entry.GetDecimalSetting(DecimalSettingEnum.Prices);

            List<TaxCode> taxCodes = GetCommonTaxCodes(entry, itemSalesTaxGroupID, salesTaxGroupID);

            foreach (TaxCode tax in taxCodes)
            {
                foreach (TaxCodeValue taxCodeLine in tax.TaxCodeLines)
                {
                    if (TaxCodeLineIsValid(taxCodeLine))
                    {
                        totalPercent += taxCodeLine.Value;
                    }
                }
            }

            guessedPrice = priceWithTax / (1M + totalPercent / 100);

            if (Math.Round(guessedPrice + CalculateTax(entry, guessedPrice, taxCodes), limit.Max) != Math.Round(priceWithTax, limit.Max))
            {
                decimal guessedPriceWithTax = guessedPrice + CalculateTax(entry, guessedPrice, taxCodes);

                decimal lowerGuess;
                decimal higherGuess;
                decimal changeFactor = 1.0M / (decimal)Math.Pow(10, limit.Max);

                SetUpperAndLowerGuessLimits(priceWithTax, guessedPrice, guessedPriceWithTax, changeFactor, out lowerGuess, out higherGuess);

                MakeSureBoundsSurroundCorrectValue(entry, priceWithTax, changeFactor, taxCodes, limit.Max, ref lowerGuess, ref higherGuess);

                decimal newGuess = (lowerGuess + higherGuess) / 2;
                guessedPrice = newGuess;

                while (Math.Round(newGuess + CalculateTax(entry, guessedPrice, taxCodes), limit.Max) != Math.Round(priceWithTax, limit.Max))
                {
                    guessedPriceWithTax = newGuess + CalculateTax(entry, guessedPrice, taxCodes);

                    if (guessedPriceWithTax > priceWithTax)
                    {
                        higherGuess = newGuess;
                    }
                    else
                    {
                        lowerGuess = newGuess;
                    }

                    newGuess = (lowerGuess + higherGuess) / 2;

                    // If this happens, we have reached an infinite loop and there is no correct solution
                    if (guessedPrice == newGuess)
                    {
                        return newGuess;
                    }

                    guessedPrice = newGuess;
                }
            }

            return guessedPrice;
        }

        public virtual decimal GetTaxAmountForPurchaseOrderLine(
            IConnectionManager entry,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier vendorID,
            RecordIdentifier storeID,
            decimal unitPrice,
            decimal discountAmount,
            decimal discountPercentage,
            TaxCalculationMethodEnum taxCalculationMethod)
        {
            RecordIdentifier vendorsSalesTaxGroupID = Providers.VendorData.GetVendorsSalesTaxGroupID(entry, vendorID);

            decimal taxAmount = 0;

            switch (taxCalculationMethod)
            {
                case TaxCalculationMethodEnum.NoTax:
                    break;
                case TaxCalculationMethodEnum.AddTax:
                    taxAmount = CalculateTax(
                        entry,
                        unitPrice,
                        itemSalesTaxGroupID,
                        vendorsSalesTaxGroupID);
                    break;
                case TaxCalculationMethodEnum.IncludeTax:
                    decimal priceWithoutTax = CalculatePriceFromPriceWithTax(
                        entry,
                        unitPrice,
                        itemSalesTaxGroupID,
                        vendorsSalesTaxGroupID);

                    taxAmount = unitPrice - priceWithoutTax;
                    break;
            }

            return taxAmount;
        }

        // Makes sure that lowerGuess produces a price with tax lower than priceWithTax and that higherGuess produces a price with tax higher than priceWithTax
        protected virtual void MakeSureBoundsSurroundCorrectValue(IConnectionManager entry, decimal priceWithTax, decimal changeFactor, List<TaxCode> taxCodes, int numberOfDecimals, ref decimal lowerGuess, ref decimal higherGuess)
        {
            // First we deal with lowerGuess
            while (Math.Round(lowerGuess + CalculateTax(entry, lowerGuess, taxCodes), numberOfDecimals) >= Math.Round(priceWithTax, numberOfDecimals))
            {
                lowerGuess -= changeFactor;
            }

            // Then we deal with higherGuess
            while (Math.Round(higherGuess + CalculateTax(entry, lowerGuess, taxCodes), numberOfDecimals) <= Math.Round(priceWithTax, numberOfDecimals))
            {
                higherGuess += changeFactor;
            }
        }

        protected virtual void SetUpperAndLowerGuessLimits(
            decimal priceWithTax,
            decimal guessedPrice,
            decimal guessedPriceWithTax,
            decimal changeFactor,
            out decimal lowerGuess,
            out decimal higherGuess)
        {
            if (guessedPriceWithTax < priceWithTax)
            {
                lowerGuess = guessedPrice;
                higherGuess = guessedPrice + changeFactor;
            }
            else
            {
                higherGuess = guessedPrice;
                lowerGuess = guessedPrice - changeFactor;
            }
        }

        public virtual decimal GetItemTax(IConnectionManager entry, RetailItem item)
        {
            if (item == null)
            {
                return 0;
            }

            RecordIdentifier defaulStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);

            return CalculateTax(
                entry,
                item.SalesPrice,
                item.SalesTaxItemGroupID,
                defaulStoreSalesTaxGroup);
        }

        public virtual List<DiscountOfferLineWithPrice> GetLinesWithPrices(IConnectionManager entry,
            List<DiscountOfferLine> lines)
        {
            RecordIdentifier defaulStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);
            DecimalLimit priceLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Prices);

            List<DiscountOfferLineWithPrice> reply = new List<DiscountOfferLineWithPrice>();

            RecordIdentifier taxItemGroupID = RecordIdentifier.Empty;
            List<TaxCode> taxCodes = null;
            foreach (DiscountOfferLine discountOfferLine in lines)
            {
                DiscountOfferLineWithPrice discountOfferLineWithPrice = new DiscountOfferLineWithPrice
                {
                    DiscountOfferLine = discountOfferLine
                };

                if (discountOfferLine.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
                {
                    if (taxItemGroupID != discountOfferLine.TaxItemGroupID)
                    {
                        taxItemGroupID = discountOfferLine.TaxItemGroupID;
                        taxCodes = GetCommonTaxCodes(entry, taxItemGroupID, defaulStoreSalesTaxGroup);
                    }

                    discountOfferLineWithPrice.OfferPrice =
                        Math.Round(
                            discountOfferLine.StandardPrice -
                            (discountOfferLine.StandardPrice * (discountOfferLine.DiscountPercent / 100.0M)),
                            priceLimiter.Max);

                    discountOfferLineWithPrice.TaxAmount =
                        CalculateTax(entry, discountOfferLineWithPrice.OfferPrice, taxCodes);

                    discountOfferLineWithPrice.OfferPriceWithTax =
                        Math.Round(discountOfferLineWithPrice.TaxAmount + discountOfferLineWithPrice.OfferPrice,
                            priceLimiter.Max);

                    discountOfferLineWithPrice.StandardPriceWithTax = discountOfferLine.StandardPrice +
                                                                      CalculateTax(entry,
                                                                          discountOfferLine.StandardPrice,
                                                                          taxCodes);

                    discountOfferLineWithPrice.DiscountAmountWithTax =
                        Math.Round(
                            discountOfferLineWithPrice.StandardPriceWithTax -
                            discountOfferLineWithPrice.OfferPriceWithTax,
                            priceLimiter.Max);
                }
                reply.Add(discountOfferLineWithPrice);
            }
            return reply;
        }
        public virtual decimal GetItemTaxForAmount(IConnectionManager entry, RecordIdentifier salesTaxItemGroupID, decimal amount)
        {
            if (RecordIdentifier.IsEmptyOrNull(salesTaxItemGroupID))
            {
                return 0;
            }

            RecordIdentifier defaulStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);

            return CalculateTax(
                entry,
                amount,
                salesTaxItemGroupID,
                defaulStoreSalesTaxGroup);
        }

        public virtual decimal GetItemPriceForItemPriceWithTax(IConnectionManager entry, RecordIdentifier salesTaxItemGroupID, decimal amountWithTax)
        {
            RecordIdentifier defaulStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);

            return CalculatePriceFromPriceWithTax(
                entry,
                amountWithTax,
                salesTaxItemGroupID,
                defaulStoreSalesTaxGroup);
        }

        public virtual void UpdatePrices(
            IConnectionManager entry,
            RecordIdentifier ID,
            UpdateItemTaxPricesEnum updateEnum,
            out int updatedItemsCount,
            out int updatedTradeAgreementsCount,
            out int updatedPromotionOfferLinesCount)
        {
            RecordIdentifier defaultStoresTaxGroupID;
            RecordIdentifier defaultStoreID;

            updatedTradeAgreementsCount = 0;
            updatedPromotionOfferLinesCount = 0;

            if (updateEnum != UpdateItemTaxPricesEnum.DefaultStoreTaxGroup)
            {
                defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);
                defaultStoreID = Providers.StoreData.GetDefaultStoreID(entry);
            }
            else
            {
                defaultStoreID = ID.PrimaryID;
                defaultStoresTaxGroupID = ID.SecondaryID;
            }

            bool calculatePriceWithTax = defaultStoreID == "" || Providers.StoreData.GetPriceWithTaxForStore(entry, defaultStoreID);

            updatedItemsCount = UpdateItemPrices(entry, defaultStoresTaxGroupID, calculatePriceWithTax, ID, updateEnum, ref updatedTradeAgreementsCount, ref updatedPromotionOfferLinesCount);
        }

        protected virtual int UpdateTradeAgreementPrice(
           IConnectionManager entry,
           RetailItem item,
           RecordIdentifier defaultStoresTaxGroupID,
           bool calculatePriceWithTax,
           RecordIdentifier ID,
           UpdateItemTaxPricesEnum updateEnum)
        {
            int updatedTAEntriesCount = 0;

            List<TradeAgreementEntry> tradeAgreementsForItemThatMightNeedToUpdate =
                Providers.TradeAgreementData.GetForItem(entry, item.MasterID, TradeAgreementRelation.PriceSales);

            foreach (TradeAgreementEntry taEntry in tradeAgreementsForItemThatMightNeedToUpdate)
            {
                decimal prevPriceWithoutTax = taEntry.Amount;
                decimal prevPriceWithTax = taEntry.AmountIncludingTax;

                if (updateEnum == UpdateItemTaxPricesEnum.ItemTaxGroup)
                {
                    item.SalesTaxItemGroupID = ID.SecondaryID;
                }

                bool pricesHaveChanged = false;

                if (calculatePriceWithTax)
                {
                    // Price with tax rules
                    taEntry.Amount =
                        CalculatePriceFromPriceWithTax(
                        entry,
                        taEntry.AmountIncludingTax,
                        item.SalesTaxItemGroupID,
                        defaultStoresTaxGroupID);

                    pricesHaveChanged = (taEntry.Amount != prevPriceWithoutTax);
                }
                else
                {
                    // Price without tax rules
                    taEntry.AmountIncludingTax =
                        CalculatePriceWithTax(
                        entry,
                        taEntry.Amount,
                        item.SalesTaxItemGroupID,
                        defaultStoresTaxGroupID,
                        false,
                        0,
                        null);

                    pricesHaveChanged = (taEntry.AmountIncludingTax != prevPriceWithTax);
                }

                if (pricesHaveChanged)
                {
                    Providers.TradeAgreementData.Save(entry, taEntry, Permission.ManageTradeAgreementPrices);
                    updatedTAEntriesCount++;
                }
            }

            return updatedTAEntriesCount;
        }

        protected int UpdateTradeAgreementPrices(
            IConnectionManager entry,
            List<RetailItem> items,
            RecordIdentifier defaultStoresTaxGroupID,
            bool calculatePriceWithTax,
            RecordIdentifier ID,
            UpdateItemTaxPricesEnum updateEnum)
        {
            int updatedTAEntriesCount = 0;

            foreach (RetailItem item in items)
            {
                updatedTAEntriesCount += UpdateTradeAgreementPrice(entry, item, defaultStoresTaxGroupID, calculatePriceWithTax, ID, updateEnum);
            }

            return updatedTAEntriesCount;
        }

        protected int UpdateItemPromotionPrice(
            IConnectionManager entry,
            RetailItem item,
            RecordIdentifier defaultStoresTaxGroupID,
            bool calculatePriceWithTax,
            RecordIdentifier ID,
            UpdateItemTaxPricesEnum updateEnum)
        {
            int updatedPromotionLinesCount = 0;

            List<PromotionOfferLine> promotionLinesForItem = Providers.DiscountOfferLineData.GetPromotionsForItem(
                entry,
                item.ID,
                RecordIdentifier.Empty,
                RecordIdentifier.Empty);

            foreach (PromotionOfferLine promotionLine in promotionLinesForItem)
            {
                // Don't want to change promotion discounts
                if (promotionLine.DiscountPercent > 0) continue;

                decimal prevPriceWithoutTax = promotionLine.OfferPrice;
                decimal prevPriceWithTax = promotionLine.OfferPriceIncludeTax;

                if (updateEnum == UpdateItemTaxPricesEnum.ItemTaxGroup)
                {
                    item.SalesTaxItemGroupID = ID.SecondaryID;
                }

                bool pricesHaveChanged = false;

                if (calculatePriceWithTax)
                {
                    // Price with tax rules
                    promotionLine.OfferPrice =
                        CalculatePriceFromPriceWithTax(
                        entry,
                        promotionLine.OfferPriceIncludeTax,
                        item.SalesTaxItemGroupID,
                        defaultStoresTaxGroupID);

                    pricesHaveChanged = (promotionLine.OfferPrice != prevPriceWithoutTax);
                }
                else
                {
                    // Price without tax rules
                    promotionLine.OfferPriceIncludeTax =
                        CalculatePriceWithTax(
                        entry,
                        promotionLine.OfferPrice,
                        item.SalesTaxItemGroupID,
                        defaultStoresTaxGroupID,
                        false,
                        0,
                        null);

                    pricesHaveChanged = (promotionLine.OfferPriceIncludeTax != prevPriceWithTax);
                }

                if (pricesHaveChanged)
                {
                    Providers.DiscountOfferLineData.Save(entry, promotionLine);
                    updatedPromotionLinesCount++;
                }
            }

            return updatedPromotionLinesCount;
        }

        protected int UpdateItemPromotionPrices(
            IConnectionManager entry,
            List<RetailItem> items,
            RecordIdentifier defaultStoresTaxGroupID,
            bool calculatePriceWithTax,
            RecordIdentifier ID,
            UpdateItemTaxPricesEnum updateEnum)
        {
            int updatedPromotionLinesCount = 0;

            foreach (RetailItem item in items)
            {
                updatedPromotionLinesCount += UpdateItemPromotionPrice(entry, item, defaultStoresTaxGroupID, calculatePriceWithTax, ID, updateEnum);
            }

            return updatedPromotionLinesCount;
        }

        /// <summary>
        /// Returns true if prices are calculated with tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="defaultStoreID">The default storeID</param>
        /// <returns></returns>
        public virtual bool PricesAreCalculatedWithTax(IConnectionManager entry, RecordIdentifier defaultStoreID)
        {
            return defaultStoreID == "" || Providers.StoreData.GetPriceWithTaxForStore(entry, defaultStoreID);
        }

        /// <summary>
        /// Calculates new item price for a price struct when a tax group has changed. 
        /// Note this only handles base price
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The item to be recalculated</param>
        /// <param name="calculatePriceWithTax">True if prices should be calculated with tax, else false</param>
        /// <param name="defaultStoresTaxGroupID">The default stores tax group ID</param>
        /// <returns>True if price actually changed, else false</returns>
        public virtual bool CalculateNewItemPrice(IConnectionManager entry, RetailItem item, bool calculatePriceWithTax, RecordIdentifier defaultStoresTaxGroupID)
        {
            decimal prevPriceWithoutTax = item.SalesPrice;
            decimal prevPriceWithTax = item.SalesPriceIncludingTax;

            if (calculatePriceWithTax)
            {
                // Price with tax rules
                item.SalesPrice =
                    CalculatePriceFromPriceWithTax(
                    entry,
                   item.SalesPriceIncludingTax,
                    item.SalesTaxItemGroupID,
                    defaultStoresTaxGroupID);

                return (item.SalesPrice != prevPriceWithoutTax);
            }
            else
            {
                // Price without tax rules
                item.SalesPriceIncludingTax =
                    CalculatePriceWithTax(
                    entry,
                    item.SalesPrice,
                    item.SalesTaxItemGroupID,
                    defaultStoresTaxGroupID,
                    false,
                    0,
                    null);

                return (item.SalesPriceIncludingTax != prevPriceWithTax);
            }
        }

        /// <summary>
        /// Updates trade agreement lines as well as promotion lines for given retail item when tax group has changed.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The retail item to update the lines for</param>
        /// <param name="calculatePriceWithTax">True if prices should be calculated with tax, else false</param>
        /// <param name="defaultStoresTaxGroupID">The default stores tax group ID</param>
        public virtual void UpdateTradeAgreementsAndPromotionsForItem(IConnectionManager entry, RetailItem item, bool calculatePriceWithTax, RecordIdentifier defaultStoresTaxGroupID)
        {
            List<RetailItem> items = new List<RetailItem>() { item };

            UpdateTradeAgreementPrices(entry, items, defaultStoresTaxGroupID, calculatePriceWithTax, RecordIdentifier.Empty, UpdateItemTaxPricesEnum.SpecificItemTaxGroup);
            UpdateItemPromotionPrices(entry, items, defaultStoresTaxGroupID, calculatePriceWithTax, RecordIdentifier.Empty, UpdateItemTaxPricesEnum.SpecificItemTaxGroup);
        }

        protected virtual int UpdateItemPrices(
            IConnectionManager entry,
            RecordIdentifier defaultStoresTaxGroupID,
            bool calculatePriceWithTax,
            RecordIdentifier ID,
            UpdateItemTaxPricesEnum updateEnum,
            ref int updatedTradeAgreementsCount,
            ref int updatedPromotionPricesCount)
        {
            int updatedItemsCount = 0;
            List<RetailItem> itemsThatMightNeedPriceUpdate = new List<RetailItem>();

            int startRow = 1;
            int fetchCount = 500;
            int fetchedRows = 0;
            int matchingRecords = 0;
            bool done = false;
            if (updateEnum == UpdateItemTaxPricesEnum.ItemTaxGroup)
            {
                List<DataEntity> taxCodesInDefaultStoreGroup = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroupList(entry, defaultStoresTaxGroupID, true);
                List<TaxCodeInItemSalesTaxGroup> taxCodesInItemTaxGroup = Providers.ItemSalesTaxGroupData.GetTaxCodesInItemSalesTaxGroup(entry, ID, TaxCodeInItemSalesTaxGroup.SortEnum.Description, false, CacheType.CacheTypeApplicationLifeTime);

                IEnumerable<DataEntity> commonTaxCodes = from x in taxCodesInDefaultStoreGroup
                                                         join y in taxCodesInItemTaxGroup on x.ID equals y.TaxCode
                                                         select x;
                if (commonTaxCodes.Count() == 0)
                {
                    updatedTradeAgreementsCount = 0;
                    updatedPromotionPricesCount = 0;
                    return 0;
                }
            }
            else if (updateEnum == UpdateItemTaxPricesEnum.TaxCode)

            {
                List<DataEntity> taxCodesInDefaultStoreGroup = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroupList(entry, defaultStoresTaxGroupID, true);
                if (taxCodesInDefaultStoreGroup.Where(x => x.ID == ID).Count() == 0)
                {
                    updatedTradeAgreementsCount = 0;
                    updatedPromotionPricesCount = 0;
                    return 0;
                }
            }

            while (!done)
            {
                switch (updateEnum)
                {
                    case UpdateItemTaxPricesEnum.TaxCode:
                        itemsThatMightNeedPriceUpdate = Providers.RetailItemData.GetItemsFromTaxCode(entry, ID, startRow, startRow + fetchCount - 1, out matchingRecords);
                        startRow += fetchCount;
                        fetchedRows += itemsThatMightNeedPriceUpdate.Count;
                        done = fetchedRows >= matchingRecords;
                        break;
                    case UpdateItemTaxPricesEnum.ItemTaxGroup:
                        itemsThatMightNeedPriceUpdate = Providers.RetailItemData.GetItemsFromTaxGroup(entry, ID, startRow, startRow + fetchCount - 1, out matchingRecords);
                        startRow += fetchCount;
                        fetchedRows += itemsThatMightNeedPriceUpdate.Count;
                        done = fetchedRows >= matchingRecords;
                        break;
                    case UpdateItemTaxPricesEnum.DefaultStoreTaxGroup:
                        itemsThatMightNeedPriceUpdate = Providers.RetailItemData.GetItemsWithTaxGroup(entry, startRow, startRow + fetchCount - 1, out matchingRecords);
                        startRow += fetchCount;
                        fetchedRows += itemsThatMightNeedPriceUpdate.Count;
                        done = fetchedRows >= matchingRecords;
                        break;
                    case UpdateItemTaxPricesEnum.SpecificItemTaxGroup:
                        RetailItem item = Providers.RetailItemData.Get(entry, ID.PrimaryID);
                        item.SalesTaxItemGroupID = ID.SecondaryID;
                        itemsThatMightNeedPriceUpdate.Add(item);
                        done = true;
                        break;
                    case UpdateItemTaxPricesEnum.AllItems:
                        itemsThatMightNeedPriceUpdate = Providers.RetailItemData.AdvancedSearchFull(entry, startRow, startRow + fetchCount - 1, SortEnum.None, out matchingRecords);
                        startRow += fetchCount;
                        fetchedRows += itemsThatMightNeedPriceUpdate.Count;
                        done = fetchedRows >= matchingRecords;
                        break;
                }

                foreach (RetailItem item in itemsThatMightNeedPriceUpdate)
                {
                    decimal prevPriceWithoutTax = item.SalesPrice;
                    decimal prevPriceWithTax = item.SalesPriceIncludingTax;

                    bool pricesHaveChanged = false;

                    if (calculatePriceWithTax)
                    {
                        // Price with tax rules
                        item.SalesPrice =
                            CalculatePriceFromPriceWithTax(
                            entry,
                            item.SalesPriceIncludingTax,
                            item.SalesTaxItemGroupID,
                            defaultStoresTaxGroupID);

                        pricesHaveChanged = (item.SalesPrice != prevPriceWithoutTax);
                    }
                    else
                    {
                        // Price without tax rules
                        item.SalesPriceIncludingTax =
                            CalculatePriceWithTax(
                            entry,
                            item.SalesPrice,
                            item.SalesTaxItemGroupID,
                            defaultStoresTaxGroupID,
                            false,
                            0,
                            null);

                        pricesHaveChanged = (item.SalesPriceIncludingTax != prevPriceWithTax);
                    }

                    if (pricesHaveChanged)
                    {
                        item.Dirty = true;
                        Providers.RetailItemData.Save(entry, item);
                    }

                    if (pricesHaveChanged || item.SalesPrice == decimal.Zero)
                    {
                        updatedItemsCount++;
                    }

                    updatedPromotionPricesCount += UpdateItemPromotionPrice(entry, item, defaultStoresTaxGroupID, calculatePriceWithTax, ID, updateEnum);
                    updatedTradeAgreementsCount += UpdateTradeAgreementPrice(entry, item, defaultStoresTaxGroupID, calculatePriceWithTax, ID, updateEnum);
                }
            }

            return updatedItemsCount;
        }
    }
}