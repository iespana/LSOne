using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface IRoundingService : IService
    {
        /// <summary>
        /// Rounds values to nearest currency unit, i.e 16,45 kr. rounded up if the smallest coin is 10 kr will give 20 kr.
        /// or if the smallest coin is 24 aurar(0,25 kr.) then if rounded up it will give 16,50 kr.
        /// </summary>
        /// <param name="value">The currency value or value to be rounded.</param>
        /// <param name="unit">The smallest unit to be rounded to.</param>
        /// <param name="roundMethod">The method of rounding, Nearest,up and down</param>
        /// <returns>Returns a value rounded to the nearest unit.</returns>
        decimal RoundToUnit(decimal value, decimal unit, RoundMethod roundMethod);

        /// <summary>
        /// Round the value with a certain number of decimals.
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="numberOfDecimals">Number of decimals to round to</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        decimal Round(IConnectionManager connection, decimal value, int numberOfDecimals, RecordIdentifier currencyCode, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Standard round to minimal coin/value in defined currency
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        decimal Round(IConnectionManager connection, decimal value, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Standard round to sales rounding
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="useSalesRounding">If true the Currency.RoundOffTypeSales is used otherwise Currency.RoundOffTypeAmount is used</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        decimal Round(IConnectionManager connection, decimal value, RecordIdentifier currencyCode, bool useSalesRounding, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Rounds the value either using the default store currency or a specific currency code
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="numberOfDecimals">Number of decimals to round to</param>
        /// <param name="useCurrencySymbol">Set as true if a currency symbol should be added to the string</param>
        /// <param name="currencyCode">Used if the useStoreCurrency parameter is false to control the rounding</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        string RoundString(IConnectionManager connection, decimal value, int numberOfDecimals, bool useCurrencySymbol, RecordIdentifier currencyCode, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Standard round to minimal coin/value in defined currency
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="currencyCode">Used if the useStoreCurrency parameter is false to control the rounding</param>
        /// <param name="useCurrencySymbol">Set as true if a currency symbol should be added to the string</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        string RoundString(IConnectionManager connection, decimal value, RecordIdentifier currencyCode, bool useCurrencySymbol, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Rounding for display: If no decimal places then sales rounding is used otherwise the value rounding is used
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="useCurrencySymbol">Set as true if a currency symbol should be added to the string</param>
        /// <param name="useSalesRounding">If true the Currency.RoundOffTypeSales is used otherwise Currency.RoundOffTypeAmount is used</param>
        /// <param name="currencyCode">Used if the useStoreCurrency parameter is false to control the rounding</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value as string</returns>
        string RoundForDisplay(IConnectionManager connection, decimal value, bool useCurrencySymbol, bool useSalesRounding, RecordIdentifier currencyCode, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a decimal converted as a string with as many decimal numbers as there are in the value
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">Value to be converted to string</param>
        /// <returns>Value as string with the same number of decimals</returns>
        string RoundAmountViewer(IConnectionManager connection, decimal value);

        /// <summary>
        /// Returns a decimal converted as a string with as many decimal numbers as there are in the currencyUnit value
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">Amount to be converted to string</param>
        /// <param name="currencyCode">Returned string should have as many decimals as the value rounding value for the currency</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Amount as string with as many decimals as there are in the currencyCode value</returns>
        string RoundAmountViewer(IConnectionManager connection, decimal value, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);

        //string DecimalToStringTrimTrailingZeros(decimal value, string decimalSep);

        /// <summary>
        /// Rounds the tender value according to the tender type and store
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="storeId">The store id</param>
        /// <param name="tenderTypeId">The tender type id</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        decimal RoundAmount(IConnectionManager connection, decimal value, RecordIdentifier storeId, RecordIdentifier tenderTypeId, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Rounds the tender value according to store 
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="storeId">The store id</param>
        /// <param name="tenderTypeId">The tender type id</param>
        /// <param name="useCurrencySymbol">Set as true if a currency symbol should be added to the string</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        string RoundAmount(IConnectionManager connection, decimal value, RecordIdentifier storeId, RecordIdentifier tenderTypeId, 
            bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Rounds an amount using the tender type settings except the rounding method is fixed i.e. the tender rounding method is not used
        /// </summary>
        /// <param name="entry">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="roundingValue">The number of decimals the amount should be rounded to i.e. 0,01 or 0,1 or 1,0</param>
        /// <param name="useCurrencySymbol">Set as true if a currency symbol should be added to the string</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <param name="roundMethod">The rounding method that should be used regardless of what the tender settings are</param>
        /// <returns>The rounded value</returns>
        string RoundAmount(IConnectionManager entry, decimal value, decimal roundingValue, TenderRoundMethod roundMethod, bool useCurrencySymbol, RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Rounds the tender value according to the tender type and store
        /// </summary>
        /// <param name="entry">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="roundingValue">The number of decimals the amount should be rounded to i.e. 0,01 or 0,1 or 1,0</param>
        /// <param name="roundMethod">The rounding method that should be used regardless of what the tender settings are</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value</returns>
        decimal RoundAmount(IConnectionManager entry, decimal value, decimal roundingValue, TenderRoundMethod roundMethod, CacheType cacheType = CacheType.CacheTypeNone);


        /// <summary>
        /// Finds if the rounding difference is smaller than the smallest used coin.
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="roundingDifference">The rounding difference between item and payments</param>
        /// <param name="storeId">The store id</param>
        /// <param name="tenderTypeId">The tender type id</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Returns true if the rounding difference is smaller than the rounding value</returns>
        bool IsLessThanSmallestCoin(IConnectionManager connection, decimal roundingDifference, RecordIdentifier storeId, 
            RecordIdentifier tenderTypeId, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Finds if the rounded rounding difference is equal to the smallest used coin.
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="roundingDifference">The rounding difference between item and payments</param>
        /// <param name="storeId">The store id</param>
        /// <param name="tenderTypeId">The tender type id</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Returns true if the rounding difference is smaller than the rounding value</returns>
        bool IsRoundedEqualToSmallestCoin(IConnectionManager connection, decimal roundingDifference, RecordIdentifier storeId,
            RecordIdentifier tenderTypeId, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Standard round of the tax value according to the tax code setup
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to round</param>
        /// <param name="taxRoundOff">The roundoff value</param>
        /// <param name="taxRoundOffType">The round off type i.e. round nearest, down, up</param>
        /// <returns>The rounded value</returns>
        decimal TaxRound(IConnectionManager connection, decimal value, decimal taxRoundOff, int taxRoundOffType);

        /// <summary>
        /// Returns the quantity rounded to the correct value of decimals, corresponding to the settings in the Unit table.
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to be rounded</param>
        /// <param name="unitId">The unit the quantity should be rounded to</param>
        /// <param name="currencyCode">Returned string should have as many decimals as the value rounding value for the currency</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Rounded quanity</returns>
        string RoundQuantity(IConnectionManager connection, decimal value, RecordIdentifier unitId, RecordIdentifier currencyCode, 
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns the quantity rounded to the correct value of decimals, corresponding to the settings in the Unit table or the Decimals settings
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to be rounded</param>
        /// <param name="unitId">The unit the quantity should be rounded to. Not valid when SplitItem is true</param>
        /// <param name="splitItem">Is the item a split item? If so then the Split Qty DecimalSettings are used</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Rounded quanity</returns>
        string RoundQuantity(IConnectionManager connection, decimal value, RecordIdentifier unitId, bool splitItem, 
            RecordIdentifier currencyCode, CacheType cacheType = CacheType.CacheTypeNone);


        /// <summary>
        /// Returns the quantity rounded to the correct value of decimals, corresponding to the settings in the Unit table or the Decimals settings
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to be rounded</param>
        /// <param name="unitId">The unit the quantity should be rounded to. Not valid when SplitItem is true</param>
        /// <param name="splitItem">Is the item a split item? If so then the Split Qty DecimalSettings are used</param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="showUOM">Whether or not to show the unit ID as a postfix</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>Rounded quanity</returns>
        string RoundQuantity(IConnectionManager connection, decimal value, RecordIdentifier unitId, bool splitItem,
            RecordIdentifier currencyCode, bool showUOM, CacheType cacheType = CacheType.CacheTypeNone);
        
        /// <summary>
        /// returns formated string for receipt displaying.
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">value to round</param>
        /// <param name="numberOfDecimals">number of decemal digits</param>
        /// <returns></returns>
        string RoundForReceipt(IConnectionManager connection, decimal value, int numberOfDecimals);

        /// <summary>
        /// A value rounded to the specified precision
        /// </summary>
        /// <param name="connection">entry into the data model</param>
        /// <param name="value">The value to be rounded</param>
        /// <param name="priceDecimalPlaces"></param>
        /// <param name="currencyCode">The currency of the value</param>
        /// <param name="cacheType">The type of caching to be used, default is CacheType.CacheTypeNone</param>
        /// <returns>The rounded value as a string</returns>
        string RoundPrecision(IConnectionManager connection, decimal value, string priceDecimalPlaces, RecordIdentifier currencyCode,
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Get a hash key for the specified string
        /// </summary>
        /// <param name="validation">String to get a hash for</param>
        /// <returns>A hash for the string</returns>
        string Validate(string validation);
        
    }
}
