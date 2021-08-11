using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class ItemSaleLine : DataEntity
    {        
        public static DecimalLimit PriceLimiter { get; set; }
        public DecimalLimit QuantityLimiter { get; internal set; }

        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string VariantID { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string StyleName { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmountWithTax { get; set; }
        public decimal Price { get; set; }
        public decimal WholeDiscountAmountWithTax { get; set; }

        public decimal CostAmount { get; set; }        
        public decimal UnitPrice { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal UnitQuantity { get; set; }
        

        public string FormattedQuantity
        {
            get
            {
                if (QuantityLimiter != null)
                {
                    return Quantity.FormatWithLimits(QuantityLimiter);
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
                    return Price.FormatWithLimits(PriceLimiter);
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
                    return NetAmountWithTax.FormatWithLimits(PriceLimiter);
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
                    return WholeDiscountAmountWithTax.FormatWithLimits(PriceLimiter);
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
                    return CostAmount.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedUnitPrice
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return UnitPrice.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedPriceUnit
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return PriceUnit.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public string FormattedUnitQuantity
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return UnitQuantity.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }
    
    }
}
