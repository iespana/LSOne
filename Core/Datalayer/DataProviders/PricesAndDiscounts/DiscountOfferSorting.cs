using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    /// <summary>
    /// An enum that defines sorting for periodic discounts. Note that some fields only apply for certain discount types
    /// </summary>
    public enum DiscountOfferSorting
    {
        OfferNumber,
        Description,
        Priority,
        OfferType,
        Status,
        DiscountType,
        DiscountValidationPeriod,
        StartingDate,
        EndingDate,
        NumberOfItemsNeeded,
        DiscountPercentValue
    }
}