using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Implemented by Currency.cs.
    /// </summary>
    public interface ICurrencyService : IService
    {
        /// <summary>
        /// Returns the ration between the exchange rates of two currency codes.
        /// If the exchange rate of either one of the codes are zero, then the result will be zero. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode1">i.e. 'USD'</param>
        /// <param name="currencyCode2">i.e. 'DKK'</param>
        /// <returns>The currency exchange rate.</returns>
        decimal GetExchangeRateRatio(IConnectionManager entry, string currencyCode1, string currencyCode2);

        /// <summary>
        /// Returns the youngest currency exchange rate found for a given currency code.
        /// If the currency code is DKK, the default currency EUR and the exchange rate is 744.50 then 
        /// for 100 EUR you will get 744.50 DKK.
        /// Axapta stores exchange rate for 100 unit of each currency.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">i.e. 'DKK'</param>
        /// <returns>The currency exchange rate.</returns>
        decimal ExchangeRate(IConnectionManager entry, RecordIdentifier currencyCode);

        /// <summary>
        /// The CurrencyInfo instance returned contains the amounts and whether each is a coin or a bill for the currency code provided.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCode">i.e. 'DKK'</param>
        /// <returns>The CurrencyInfo object.</returns>
        CurrencyInfo DetailedCurrencyInfo(IConnectionManager entry, string currencyCode);

        /// <summary>
        /// Converts a value from one currency to another.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="fromCurrencyCode">The currency which the orgValue is in</param>
        /// <param name="toCurrencyCode">The currency to which to convert the orgValue to</param>
        /// <param name="companyCurrency">The company currency</param>
        /// <param name="orgValue">The value to be converted</param>
        /// <returns>The value as it is after conversion in the toCurrencyCode rounded according to the toCurrencyCode rounding setup</returns>
        decimal CurrencyToCurrency(IConnectionManager entry, RecordIdentifier fromCurrencyCode, RecordIdentifier toCurrencyCode, RecordIdentifier companyCurrency, decimal orgValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="fromCurrencyCode"></param>
        /// <param name="toCurrencyCode"></param>
        /// <param name="companyCurrency">The company currency</param>
        /// <param name="orgValue"></param>
        /// <returns></returns>
        decimal CurrencyToCurrencyNoRounding(IConnectionManager entry, RecordIdentifier fromCurrencyCode, RecordIdentifier toCurrencyCode, RecordIdentifier companyCurrency,decimal orgValue);

    }
}
