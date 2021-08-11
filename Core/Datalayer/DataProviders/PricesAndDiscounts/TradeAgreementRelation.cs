using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    /// <summary>
    /// The type of price and/or discount of a trade agreement entry
    /// </summary>
    public enum TradeAgreementRelation
    {
        /// <summary>
        /// Tradeagreement entry is a sales price
        /// </summary>
        PriceSales = 4,
        /// <summary>
        /// Tradeagreement entry is a line discount
        /// </summary>
        LineDiscSales = 5,
        /// <summary>
        /// Tradeagreement entry is a multiline discount
        /// </summary>
        MultiLineDiscSales = 6,
        /// <summary>
        /// Trade agreement entry is total discount
        /// </summary>
        TotalDiscount
    }
}