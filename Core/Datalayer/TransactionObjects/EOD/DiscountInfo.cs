using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    public class DiscountInfo
    {
        public decimal CombinedDiscountIncludingTax { get; set; }
        public decimal TotalDiscountIncludingTax { get; set; }
        public decimal LineDiscountIncludingTax { get; set; }
        public decimal PeriodicDiscountIncludingTax { get; set; }

        public decimal CombinedDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal LineDiscount { get; set; }
        public decimal PeriodicDiscount { get; set; }

        public XZDisplayAmounts SalesReturns { get; set; }

        public DiscountInfo()
        {
            CombinedDiscountIncludingTax = decimal.Zero;
            TotalDiscountIncludingTax = decimal.Zero;
            LineDiscountIncludingTax = decimal.Zero;
            PeriodicDiscountIncludingTax = decimal.Zero;

            CombinedDiscount = decimal.Zero;
            TotalDiscount = decimal.Zero;
            LineDiscount = decimal.Zero;
            PeriodicDiscount = decimal.Zero;
        }

        public bool Empty()
        {
            return CombinedDiscount == decimal.Zero &&
                   CombinedDiscountIncludingTax == decimal.Zero &&
                   TotalDiscount == decimal.Zero &&
                   TotalDiscountIncludingTax == decimal.Zero &&
                   LineDiscount == decimal.Zero &&
                   LineDiscountIncludingTax == decimal.Zero &&
                   PeriodicDiscount == decimal.Zero &&
                   PeriodicDiscountIncludingTax == decimal.Zero;
        }
    }
}
