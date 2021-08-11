using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class CurrencyService : ICurrencyService
    {
        #region ICurrency Members

        /// <summary>
        /// Returns the ration between the exchange rates of two currency codes.
        /// If the exchange rate of either one of the codes are zero, then the result will be zero. 
        /// </summary>
        /// <param name="currencyCode1">i.e. 'USD'</param>
        /// <param name="currencyCode2">i.e. 'DKK'</param>
        /// <returns>The currency exchange rate.</returns>
        public virtual decimal GetExchangeRateRatio(IConnectionManager entry, string currencyCode1, string currencyCode2)
        {
            decimal exchRate1 = ExchangeRate(entry, currencyCode1);
            decimal exchRate2 = ExchangeRate(entry, currencyCode2);
            if (exchRate2 > 0)
            {
                return (exchRate1 / exchRate2) * 100;
            }
            else
                return 0;
        }       

        /// <summary>
        /// Returns the latest currency for a given currency code.
        /// If the currency code is DKK, the default currency EUR and the exchange rate is 744.50 then 
        /// for 100 EUR you will get 744.50 DKK.
        /// Axapta stores exchange rate for 100 unit of each currency.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="currencyCode">The currency name,i.e. USD, DKK, EUR</param>
        /// <returns>The currency rate</returns>
        public virtual decimal ExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode)
        {
            return Providers.ExchangeRatesData.GetExchangeRate(entry, currencyCode, CacheType.CacheTypeTransactionLifeTime);
        }

        public virtual CurrencyInfo DetailedCurrencyInfo(IConnectionManager entry, string currencyCode)
        {
            CurrencyInfo currInfo = new CurrencyInfo();
            currInfo.PosCurrencyRate = ExchangeRate(entry, currencyCode);
            currInfo.CurrencyItems = Providers.CashDenominatorData.GetCashDenominators(entry, currencyCode, 1, false);          
            return currInfo;
        }


        public virtual decimal CurrencyToCurrencyNoRounding(IConnectionManager entry, RecordIdentifier fromCurrencyCode, RecordIdentifier toCurrencyCode, RecordIdentifier companyCurrency, decimal orgValue)
        {
            //if from and to currency is the same one return the original value
            if (((string) fromCurrencyCode).Trim() == ((string) toCurrencyCode).Trim())
            {
                return orgValue;
            }

            //If the value to be converted is 0 then just return 0
            if (orgValue == 0M)
            {
                return orgValue;
            }

            //If the fromCurrencyCode is the default HO currency then only one conversion is necessary                
            if (companyCurrency == fromCurrencyCode)
            {
                // Default HO Currency = USD
                // fromCurrencyCode = USD, toCurrencyCode = EUR
                // Convert USD to EUR

                decimal toExchRate = ExchangeRate(entry, toCurrencyCode);
                if (toExchRate != 0) // When 
                    return orgValue/toExchRate;
            }
                //If the toCurrencyCode is the default HO currency then only one conversion is necessary
            else if (companyCurrency == toCurrencyCode)
            {
                // Default HO Currency = USD
                // fromCurrencyCode = CAN, toCurrencyCode = USD
                // Convert CAN to USD

                decimal fromExchRate = ExchangeRate(entry, fromCurrencyCode);
                return orgValue*fromExchRate;
            }
                //if neither the to or from currency is the default HO currency then we need to 
                //convert the orgValue to the local currency and then to the toCurrencyCode
            else
            {
                // Default HO Currency = USD
                // fromCurrencyCode = CAN, toCurrencyCode = EUR
                // Convert CAN to USD then convert USD to EUR

                decimal fromExchRate = ExchangeRate(entry, fromCurrencyCode);
                decimal toExchRate = ExchangeRate(entry, toCurrencyCode);

                //if no exchange rates are found for one of the currency codes then there's not point in
                //doing the conversions as one will multiply with 0 and the other divide with 0 => return 0
                if (fromExchRate == 0M || toExchRate == 0M)
                {
                    return 0M;
                }

                //Convert the org value to the local currency with the fromCurrency value
                // i.e. Convert CAN to USD
                decimal localCurrency = (orgValue*fromExchRate);

                //Convert the local currency to the toCurrency value
                // i.e. Convert USD to EUR
                return localCurrency/toExchRate;
            }
            return 0M;
        }

        /// <summary>
        /// Converts a value from one currency to another.
        /// </summary>
        /// <param name="fromCurrencyCode">The currency which the orgValue is in</param>
        /// <param name="toCurrencyCode">The currency to which to convert the orgValue to</param>
        /// <param name="orgValue">The value to be converted</param>
        /// <returns>The value as it is after conversion in the toCurrencyCode rounded according to the toCurrencyCode rounding setup</returns>
        public virtual decimal CurrencyToCurrency(IConnectionManager entry, RecordIdentifier fromCurrencyCode, RecordIdentifier toCurrencyCode, RecordIdentifier companyCurrency, decimal orgValue)
        {
            IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

            return rounding.Round(entry, CurrencyToCurrencyNoRounding(entry, fromCurrencyCode, toCurrencyCode,companyCurrency, orgValue), toCurrencyCode, CacheType.CacheTypeTransactionLifeTime);
        }

        #endregion

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }
    }
}
