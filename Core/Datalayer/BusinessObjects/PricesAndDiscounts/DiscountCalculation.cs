using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    public class DiscountCalculation : DataEntity
    {
        public DiscountCalculation()
        {
            DiscountsToApply = LineDiscCalculationTypes.Line;
            CalculatePeriodicDiscounts = CalculatePeriodicDiscountEnums.AlwaysCalculate;
            CalculateCustomerDiscounts = CalculateCustomerDiscountEnums.AlwaysCalculate;
            ClearPeriodicDiscountCache = ClearPeriodicDiscountCacheEnum.BeforeEachSale;
            ClearPeriodicDiscountAfterMinutes = 30;
        }

        /// <summary>
        /// Determines how the linediscount is found/calculated.
        /// </summary>
        public LineDiscCalculationTypes DiscountsToApply { get; set; }
        /// <summary>
        /// Controls if the customer discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        public CalculateCustomerDiscountEnums CalculateCustomerDiscounts { get; set; }
        /// <summary>
        /// Controls if the periodic discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        public CalculatePeriodicDiscountEnums CalculatePeriodicDiscounts { get; set; }

        /// <summary>
        /// Controls when the periodic discount cache is cleared from the POS
        /// </summary>
        public ClearPeriodicDiscountCacheEnum ClearPeriodicDiscountCache { get; set; }

        /// <summary>
        /// The number of minutes the periodic discount cache should be kept before it is cleared. Default value is 30 minutes
        /// </summary>
        public int ClearPeriodicDiscountAfterMinutes { get; set; }
    }
}
