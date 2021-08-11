using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Settings;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    // See documentation for each method on the interface

    [Serializable]
    public partial class RoundingService : IRoundingService
    {
        private const string Key = "IcelandPos1944";

        public RoundingService()
        {
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {
            }
        }

        public void Init(IConnectionManager entry)
        {
        }

        public virtual decimal Round(IConnectionManager entry, decimal value, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return RoundToUnit(value, CurrencyUnit(entry, currencyCode, cacheType), Method(entry, currencyCode, cacheType));
        }

        public virtual decimal Round(IConnectionManager entry, decimal value, RecordIdentifier currencyCode, bool useSalesRounding, CacheType cacheType = CacheType.CacheTypeNone)
        {
            //If false then no change - use the "old" Round (decimal, bool) function
            if (!useSalesRounding)
            {
                return Round(entry, value, currencyCode, cacheType);
            }
            
            return RoundToUnit(
                value, 
                CurrencyUnit(entry, currencyCode, useSalesRounding, cacheType), 
                Method(entry, currencyCode, useSalesRounding, cacheType));
        }

        public virtual string RoundString(IConnectionManager entry, decimal value, RecordIdentifier currencyCode, bool useCurrencySymbol, CacheType cacheType = CacheType.CacheTypeNone)
        {
            decimal currencyUnit = CurrencyUnit(entry, currencyCode, cacheType);
            string numberFormat = NumberFormat(currencyUnit);
            decimal roundedValue = RoundToUnit(value, currencyUnit, Method(entry, currencyCode,cacheType));

            bool negativeValue = (roundedValue < 0);
            roundedValue = Math.Abs(roundedValue);
            
            string roundedValueString = roundedValue.ToString(numberFormat);
            string roundedValueWithSuffix = AddPrefixSuffix(entry, roundedValueString, useCurrencySymbol, currencyCode,cacheType);
            return negativeValue ? "-" + roundedValueWithSuffix : roundedValueWithSuffix;
        }

        public virtual decimal Round(IConnectionManager entry, decimal value, int numberOfDecimals, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            decimal unit = 1.0M / (decimal)Math.Pow(10, numberOfDecimals);
            return RoundToUnit(value, unit, Method(entry, currencyCode, cacheType));
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public virtual string RoundString(IConnectionManager entry, decimal value, int numberOfDecimals, bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            decimal unit = 1.0M / (decimal)Math.Pow(10, numberOfDecimals);
            decimal roundedValue = RoundToUnit(value, unit, Method(entry, currencyCode,cacheType));
            string formatString = "N" + numberOfDecimals;

            bool negativeValue = (roundedValue < 0);
            roundedValue = Math.Abs(roundedValue);
                        
            string roundedValueString = roundedValue.ToString(formatString);
            string roundedValueWithSuffix = AddPrefixSuffix(entry, roundedValueString, useCurrencySymbol, currencyCode, cacheType);
            return negativeValue ? "- " + roundedValueWithSuffix : roundedValueWithSuffix;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public virtual string RoundForDisplay(IConnectionManager entry, decimal value, bool useCurrencySymbol, bool useSalesRounding, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            //If false then no change - use the "old" Round(decimal, bool) function
            if (!useSalesRounding)
            {
                return RoundString(entry, value,currencyCode, useCurrencySymbol,cacheType);
            }
            
            decimal currencyUnit = CurrencyUnit(entry, currencyCode, useSalesRounding, cacheType);
            string numberFormat = NumberFormat(currencyUnit);
                    
            decimal roundedValue = RoundToUnit(value, currencyUnit, Method(entry, currencyCode, useSalesRounding,cacheType));

            bool negativeValue = (roundedValue < 0);
            roundedValue = Math.Abs(roundedValue);
            
            string roundedValueString = roundedValue.ToString(numberFormat);

            string roundedValueWithSuffix = AddPrefixSuffix(entry, roundedValueString, useCurrencySymbol, currencyCode, cacheType);
            return negativeValue ? "- " + roundedValueWithSuffix : roundedValueWithSuffix;
        }

        private static string NumberFormat(decimal currencyUnit)
        {
            if (Math.Round(currencyUnit) > 0)
            {
                return "N0";
            }

            string result = "N";
            for (int i = 1; i < 9; i++)
            {
                decimal factor = (decimal)Math.Pow(10, i);
                decimal multipl = currencyUnit * factor;
                if (multipl == Math.Round(multipl))
                {
                    result += i.ToString();
                    break;
                }
            }
            return result;
        }

        public virtual string RoundAmountViewer(IConnectionManager entry, decimal value)
        {
            return value.ToString(NumberFormat(value));
        }

        public virtual string RoundAmountViewer(IConnectionManager entry, decimal value, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (currencyCode == "")
            {
                return RoundAmountViewer(entry,value);
            }
            return value.ToString(NumberFormat(CurrencyUnit(entry, currencyCode,cacheType)));
        }

        public virtual decimal RoundAmount(IConnectionManager entry, decimal value, decimal roundingValue, TenderRoundMethod roundMethod, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return RoundToUnit(value, roundingValue, roundMethod);
        }

        public virtual decimal RoundAmount(IConnectionManager entry, decimal value, RecordIdentifier storeId, RecordIdentifier tenderTypeId, CacheType cacheType = CacheType.CacheTypeNone)
        {
            int roundingMethod = 0;
            decimal roundingValue = 0.01M;
            GetPaymentRoundInfo(entry, storeId, tenderTypeId, ref roundingMethod, ref roundingValue,cacheType);
            return RoundToUnit(value, roundingValue, (TenderRoundMethod)roundingMethod);
        }

        public virtual decimal RoundAmount(IConnectionManager entry, decimal value, RecordIdentifier storeId, CacheType cacheType = CacheType.CacheTypeNone)
        {
            int roundingMethod = 0;
            decimal roundingValue = 0.01M;
            GetPaymentRoundInfo(entry, storeId, ref roundingMethod, ref roundingValue, cacheType);
            return RoundToUnit(value, roundingValue, (TenderRoundMethod)roundingMethod);
        }

        public virtual string RoundAmount(IConnectionManager entry, decimal value, decimal roundingValue, TenderRoundMethod roundMethod, bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            decimal currencyUnit = CurrencyUnit(entry, currencyCode, cacheType);
            string numberFormat = NumberFormat(currencyUnit);
            decimal roundedValue = RoundAmount(entry, value, roundingValue, roundMethod, cacheType);

            bool negativeValue = (roundedValue < 0);
            roundedValue = Math.Abs(roundedValue);

            string roundedValueString = roundedValue.ToString(numberFormat);
            string roundedValueWithSuffix = AddPrefixSuffix(entry, roundedValueString, useCurrencySymbol, currencyCode, cacheType);
            return negativeValue ? "- " + roundedValueWithSuffix : roundedValueWithSuffix;
        }

        public virtual string RoundAmount(IConnectionManager entry, decimal value, RecordIdentifier storeId, RecordIdentifier tenderTypeId, bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            decimal currencyUnit = CurrencyUnit(entry, currencyCode,cacheType);
            string numberFormat = NumberFormat(currencyUnit);
            decimal roundedValue = RoundAmount(entry, value, storeId, tenderTypeId, cacheType);

            bool negativeValue = (roundedValue < 0);
            roundedValue = Math.Abs(roundedValue);
           
            string roundedValueString = roundedValue.ToString(numberFormat);
            string roundedValueWithSuffix = AddPrefixSuffix(entry, roundedValueString, useCurrencySymbol, currencyCode, cacheType);
            return negativeValue ? "- " + roundedValueWithSuffix : roundedValueWithSuffix;
        }

        public virtual bool IsLessThanSmallestCoin(IConnectionManager entry, decimal roundingDifference, RecordIdentifier storeId, RecordIdentifier tenderTypeId, CacheType cacheType = CacheType.CacheTypeNone)
        {
            int roundingMethod = 0;
            decimal roundingValue = 0.01M;
            GetPaymentRoundInfo(entry, storeId, tenderTypeId, ref roundingMethod, ref roundingValue, cacheType);
            return roundingDifference < roundingValue;
        }

        public virtual bool IsRoundedEqualToSmallestCoin(IConnectionManager entry, decimal roundingDifference, RecordIdentifier storeId, RecordIdentifier tenderTypeId, CacheType cacheType = CacheType.CacheTypeNone)
        {
            int roundingMethod = 0;
            decimal roundingValue = 0.01M;
            decimal roundedDifference = RoundAmount(entry, roundingDifference, storeId, tenderTypeId, cacheType);
            GetPaymentRoundInfo(entry, storeId, tenderTypeId, ref roundingMethod, ref roundingValue, cacheType);
            return roundedDifference == 0 || roundedDifference == roundingValue;
        }

        public virtual string RoundForReceipt(IConnectionManager entry,decimal value, int numberOfDecimals)
        {
            decimal unit = 1.0M / (decimal)Math.Pow(10, numberOfDecimals);
            decimal roundedValue = RoundToUnit(value, unit, RoundMethod.RoundNearest);
            string numberFormat = NumberFormat(unit);
            return roundedValue.ToString(numberFormat);
        }

        private string AddPrefixSuffix(IConnectionManager entry, string value, bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            string result = value;

            if (useCurrencySymbol)
            {
                Currency currency = Providers.CurrencyData.Get(entry, currencyCode, cacheType);

                if (currency != null)
                {
                    string prefix = currency.CurrencyPrefix;
                    string suffix = currency.CurrencySuffix;
                    
                    if (prefix.Length > 0)
                    {
                        result = prefix + " " + result;
                    }

                    if (suffix.Length > 0)
                    {
                        result = result + " " + suffix;
                    }
                }
            }

            return result;
        }

        private static Decimal CurrencyUnit(IConnectionManager entry, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return CurrencyUnit(entry, currencyCode, false, cacheType);
        }

        private static Decimal CurrencyUnit(IConnectionManager entry, RecordIdentifier currencyCode, bool useSalesRounding, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Currency currency = Providers.CurrencyData.Get(entry, currencyCode, cacheType);
            
            if (currency != null)
            {
                return useSalesRounding ? currency.RoundOffSales : currency.RoundOffAmount;
            }
            
            int defaultNumberOfDecimals = entry.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
            return (decimal)(1 / Math.Pow(10, defaultNumberOfDecimals));
        }

        protected virtual RoundMethod Method(IConnectionManager entry, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Currency currency = Providers.CurrencyData.Get(entry, currencyCode, cacheType);
            if (currency != null)
            {
                return RoundMethodFromInt(currency.RoundOffTypeAmount);
            }
            return RoundMethod.RoundNearest; //Default if nothing is found
        }

        protected virtual RoundMethod Method(IConnectionManager entry, RecordIdentifier currencyCode, bool useSalesRounding, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (!useSalesRounding)
            {
                return Method(entry, currencyCode, cacheType);
            }

            Currency currency = Providers.CurrencyData.Get(entry, currencyCode, cacheType);

            if (currency != null)
            {
                return RoundMethodFromInt(currency.RoundOffTypeSales);
            }
            return RoundMethod.RoundNearest;
        }

        protected virtual RoundMethod RoundMethodFromInt(int methodNumber)
        {
            switch (methodNumber)
            {
                case 1:
                    return RoundMethod.RoundDown;
                case 2:
                    return RoundMethod.RoundUp;
                case 0:
                default:
                    return RoundMethod.RoundNearest;
            }
        }

        /// <summary>
        /// Returns roundmethod that will be used to round the tax value
        /// </summary>
        /// <param name="taxRoundOffType">The tax code</param>
        /// <returns>The round method that will be used.</returns>
        private static RoundMethod TaxMethod(int taxRoundOffType)
        {
            // Standard Dynamics AX doesn't look at the RoundMethod when rounding tax groups
            // RoundNearest is only ever used.
            // If using another system than Dynamics AX for statements then this configuration 
            // should be used

            switch (taxRoundOffType)
            {
                case 1:
                    return RoundMethod.RoundDown;
                case 2:
                    return RoundMethod.RoundUp;
                case 0:
                default:
                    return RoundMethod.RoundNearest; //Default if nothing is found     
            }
        }

        public virtual decimal TaxRound(IConnectionManager entry, decimal value, decimal taxRoundOff, int taxRoundOffType)
        {            
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (settings != null && settings.Store.UseTaxRounding == false)
            {
                return value;
            }

            if (value == 0M)
            {
                return 0M;
            }

            if (taxRoundOff == 0M)
            {
                taxRoundOff = 0.01M;
            }
                        
            return RoundToUnit(value, taxRoundOff, TaxMethod(taxRoundOffType));

        }

        public virtual string RoundQuantity(IConnectionManager entry, decimal value, RecordIdentifier unitId, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return RoundQuantity(entry, value, unitId, false, currencyCode,cacheType);
        }

        public virtual string RoundQuantity(IConnectionManager entry, decimal value, RecordIdentifier unitId, bool splitItem, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone)
        {
            return RoundQuantity(entry, value, unitId, splitItem, currencyCode, false, cacheType);        
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public virtual string RoundQuantity(IConnectionManager entry, decimal value, RecordIdentifier unitId, bool splitItem, RecordIdentifier currencyCode, bool showUOM, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Unit unit = Providers.UnitData.Get(entry, unitId, cacheType);
            string unitPostFix = "";

            if (unit != null)
                unitPostFix = showUOM ? " " + unit.Text : "";           
           
            //DecimalLimit limiter = new DecimalLimit();
            if (!splitItem)
            {

                if (unit == null)
                    return value.ToString("N" + entry.GetDecimalSetting(DecimalSettingEnum.Quantity).Min) + unitPostFix;

                for (int i = unit.MaximumDecimals; i >= unit.MinimumDecimals; i--)
                {
                    string outputString = RoundForReceipt(entry, value, i);
                    if (outputString.Length > 0 && (outputString.EndsWith("0") == false))
                        return outputString + unitPostFix;
                }

                return value.ToString("N" + unit.MinimumDecimals) + unitPostFix;
            }

            DecimalSetting decimalData = Providers.DecimalSettingsData.Get(entry, "Split Qty");

            for (int i = decimalData.Max; i >= decimalData.Min; i--)
            {
                string outputString = value.ToString("N" + i);
                if (outputString.Length > 0 && (outputString.EndsWith("0") == false))
                    return outputString + unitPostFix;
            }

            // if all fails, normal rounding will be used.
            return RoundString(entry, value, currencyCode, false, cacheType) + unitPostFix;
        }

        private int GetNumberOfDecimals(decimal round)
        {
            return BitConverter.GetBytes(decimal.GetBits(round)[3])[2];
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public virtual decimal RoundToUnit(decimal value, decimal unit, RoundMethod roundMethod)
        {
            try
            {
                if (unit != 0)
                {
                    decimal decimalValue = value / unit;

                    Int64 intValue = (Int64)(value / unit);

                    decimal difference = Math.Abs(decimalValue) - Math.Abs(intValue);

                    // is rounding required?
                    if (difference > 0)
                    {
                        switch (roundMethod)
                        {
                            case RoundMethod.RoundNearest:
                                decimal roundResult = Math.Round(Math.Round((value / unit), 0, MidpointRounding.AwayFromZero) * unit,
                                    GetNumberOfDecimals(unit), MidpointRounding.AwayFromZero);
                                return roundResult;
                            case RoundMethod.RoundDown:
                                if (value > 0M)
                                    return Math.Round(Math.Round((value/unit) - 0.5M, 0)*unit, GetNumberOfDecimals(unit));
                                return Math.Round(Math.Round((value/unit) + 0.5M, 0)*unit, GetNumberOfDecimals(unit));
                            case RoundMethod.RoundUp:
                                if (value > 0M)
                                    return Math.Round(Math.Round((value/unit) + 0.5M, 0)*unit, GetNumberOfDecimals(unit));
                                return Math.Round(Math.Round((value/unit) - 0.5M, 0)*unit, GetNumberOfDecimals(unit));
                        }
                    }
                }
                return value;
            }
            catch (OverflowException ex)
            {
                throw new Exception(Properties.Resources.MaxAmountReached,ex);
            }
        }

        /// <summary>
        /// Rounds values to nearest currency unit using the tender type rounding settings, i.e 16,45 kr. rounded up if the smallest coin is 10 kr will give 20 kr.
        /// or if the smallest coin is 24 aurar(0,25 kr.) then if rounded up it will give 16,50 kr.
        /// </summary>
        /// <param name="value">The currency value or value to be rounded.</param>
        /// <param name="unit">The smallest unit to be rounded to.</param>
        /// <param name="roundMethod">The method of rounding, None, Nearest, up or down</param>
        /// <returns>Returns a value rounded to the nearest unit.</returns>
        protected virtual decimal RoundToUnit(decimal value, decimal unit, TenderRoundMethod roundMethod)
        {
            if (roundMethod == TenderRoundMethod.None)
            {
                return value;
            }

            /* 
             * TenderRoundMethod = 0 - None, 1 - Nearest, 2 - Up, 3 - Down
             * RoundMethod = 0 - Nearest, 1 - Down, 2 - Up
             * 
             * When rounding payment numbers then the TenderRoundingMethod should be but when
             * rounding tax or currency numbers then the RoundMethod should be used.
             * 
             * If TenderRoundMethod is set to NONE then the value is returned without any rounding
             * otherwise we need to cast the TenderRoundingMethod to RoundingMethod and then call the original RoundToUnit function
             * 
             */
            var currencyRound = RoundMethod.RoundNearest;
            switch (roundMethod)
            {
                case TenderRoundMethod.RoundNearest:
                    currencyRound = RoundMethod.RoundNearest;
                    break;
                case TenderRoundMethod.RoundUp:
                    currencyRound = RoundMethod.RoundUp;
                    break;
                case TenderRoundMethod.RoundDown:
                    currencyRound = RoundMethod.RoundDown;
                    break;
            }
            return RoundToUnit(value, unit, currencyRound);
        }

        private static void GetPaymentRoundInfo(IConnectionManager entry, RecordIdentifier storeId, ref int roundingMethod, ref decimal roundingValue, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Payment payment = Providers.StoreData.GetStoreCashPayment(entry, storeId, cacheType);

            if (payment != null)
            {
                // We shift the rounding method up by 1, since when setting up payment rounding we do not have the option of selecting
                // "none", which is the first setting defined in the rounding method enum
                roundingMethod = payment.RoundingMethod + 1;
                roundingValue = payment.RoundingValue;
            }
            else
            {
                // We set default values since no record was found
                roundingMethod = 0;
                int defaultNumberOfDecimals = entry.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
                roundingValue = (decimal)(1 / Math.Pow(10, defaultNumberOfDecimals));
            }   
        }

        private static void GetPaymentRoundInfo(IConnectionManager entry, RecordIdentifier storeId, RecordIdentifier tenderTypeId, ref int roundingMethod, ref decimal roundingValue, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Payment payment = Providers.StoreData.GetStorePayment(entry, storeId, tenderTypeId, cacheType);

            if (payment != null)
            {
                // We shift the rounding method up by 1, since when setting up payment rounding we do not have the option of selecting
                // "none", which is the first setting defined in the rounding method enum
                roundingMethod = payment.RoundingMethod + 1;
                roundingValue = payment.RoundingValue;
            }
            else
            {
                // We set default values since no record was found
                roundingMethod = 0;
                int defaultNumberOfDecimals = entry.GetDecimalSetting(DecimalSettingEnum.Prices).Max;
                roundingValue = (decimal)(1 / Math.Pow(10, defaultNumberOfDecimals));
            }   
        }

        public virtual string RoundPrecision(IConnectionManager connection, decimal value, string priceDecimalPlaces, RecordIdentifier currencyCode,CacheType cacheType = CacheType.CacheTypeNone)
        {
            try
            {
                if (Conversion.ToStr(priceDecimalPlaces) != "")
                {
                    string outputString = "";
                    DecimalSetting decimals = new DecimalSetting(priceDecimalPlaces);

                    for (int i = decimals.Max; i >= decimals.Min; i--)
                    {
                        outputString = RoundForReceipt(connection, value, i);
                        if (outputString.Length > 0 && (outputString.EndsWith("0") == false))
                            return outputString;
                    }

                    //// The minimum length of this is x:x = 3 letters, and it must contain the letter ':'
                    if (priceDecimalPlaces.Length >= 3 && priceDecimalPlaces.Contains(":"))
                    {
                        string[] minAndMax = priceDecimalPlaces.Split(':');
                        if (minAndMax.Length >= 2)
                        {
                            int min = Convert.ToInt16(minAndMax[0]);
                            int max = Convert.ToInt16(minAndMax[1]);
                            for (int i = max; i >= min; i--)
                            {
                                outputString = RoundForReceipt(connection, value, i);
                                if (outputString.Length > 0 && (outputString.EndsWith("0") == false))
                                    return outputString;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            // if all fails, normal rounding will be used.
            return RoundString(connection, value, currencyCode,false, cacheType);
        }

        public virtual string Validate(string validation)
        {
            return HMAC_SHA1.GetValue(validation, Key);
        }
    }
}
