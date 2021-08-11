namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum CalculateCustomerDiscountEnums
    {
        /// <summary>
        /// Each time an item is added to the transaction the customer discounts are calculated
        /// </summary>
        AlwaysCalculate = 0,
        /// <summary>
        /// The customer discounts are calculated on payment, as well when the operation DisplayTotal is run
        /// </summary>
        CalculateOnTotal = 1        
    }

    public enum CalculatePeriodicDiscountEnums
    {
        /// <summary>
        /// Each time an item is added to the transaction the periodic discounts are calculated
        /// </summary>
        AlwaysCalculate = 0,
        /// <summary>
        /// The periodic discounts are calculated on payment, as well when the operation DisplayTotal is run
        /// </summary>
        CalculateOnTotal = 1        
    }

    public enum ClearPeriodicDiscountCacheEnum
    {
        /// <summary>
        /// Before each sale is started the periodic discount cache is cleared and restarted
        /// </summary>
        BeforeEachSale = 0,

        /// <summary>
        /// The periodic discount cache is cleared after specific number of minutes
        /// </summary>
        ClearAfter = 1
    }
}
