using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class PromotionOfferLine : DiscountOfferLine
    {
        public enum PromotionLineStatuses
        {
            Valid,
            Invalid,
            Both
        }

        public PromotionOfferLine()
            : base()
        {
            DiscountAmount = 0.0M;
            OfferPrice = 0.0M;
            OfferPriceIncludeTax = 0.0M;
            DiscountamountIncludeTax = 0.0M;
        }

        public decimal DiscountAmount { get; set; }
        public decimal DiscountamountIncludeTax { get; set; }
        public decimal OfferPrice { get; set; }
        public decimal OfferPriceIncludeTax { get; set; }

        public bool InActiveDiscount { get; set; }

        public RecordIdentifier ValidationPeriodID { get; set; }

        public bool ItemIsVariant;

    }
}
