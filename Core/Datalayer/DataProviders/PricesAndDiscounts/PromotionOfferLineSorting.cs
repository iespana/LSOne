using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    /// <summary>
    /// A enum that defines sorting columns for promotion offer lines 
    /// </summary>
    public enum PromotionOfferLineSorting
    {
        OfferID,
        Name,
        StandardPriceInclTax,
        StandardPrice,
        DiscountPercentage,
        ItemId,
        Type
    };
}