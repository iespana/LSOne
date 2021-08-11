using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public class EmployeeSaleLine : DataEntity
    {
        public static DecimalLimit PriceLimiter { get; set; }
        public static DecimalLimit QuantityLimiter { get; internal set; }

        public string StaffId { get; set; }
        public string Name { get; set; }
        public string NameOnReceipt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetAmountWithTax { get; set; }
        public decimal WholeDiscountAmountWithTax { get; set; }
        public decimal Price { get; set; }
        public decimal CostAmount { get; set; }   
        public string Login { get; set; }

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
    }
}
