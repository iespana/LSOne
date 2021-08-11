using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class SalesTotalAmounts : DataEntity
    {
        public static DecimalLimit PriceLimiter { get; set; }
        public DecimalLimit QuantityLimiter { get; internal set; }
        
        public decimal TotalQuantity { get; set; }
        public decimal TotalNetAmountWithTax { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalWholeDiscountAmountWithTax { get; set; }
        public decimal TotalCostAmount { get; set; }


        public string FormattedQuantity
        {
            get
            {
                if (QuantityLimiter != null)
                {
                    return TotalQuantity.FormatWithLimits(QuantityLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedPrice
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalPrice.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedNetAmountWithTax
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalNetAmountWithTax.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedWholeDiscountAmountWithTax
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalWholeDiscountAmountWithTax.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedCostAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalCostAmount.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }        
    }
}
