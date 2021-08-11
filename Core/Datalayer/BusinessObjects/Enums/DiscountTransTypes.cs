namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum DiscountTransTypes
    {
        /// <summary>
        /// 0
        /// </summary>
        Periodic = 0,
        /// <summary>
        /// 1
        /// </summary>
        Customer = 1,
        /// <summary>
        /// 2
        /// </summary>
        LineDisc = 2,
        /// <summary>
        /// 3
        /// </summary>
        TotalDisc = 3,
        LoyaltyDisc = 4
    }

    /// <summary>
    /// An enum that tells the system if the discount line was created by the POS or by a customization
    /// </summary>
    public enum DiscountOrigin
    {
        /// <summary>
        /// Default value for discounts created by the standard POS functionality.
        /// </summary>
        POS = 0,
        /// <summary>
        /// Set for discounts that require customized functionality written by partner
        /// </summary>
        Custom = 1
    }
}
