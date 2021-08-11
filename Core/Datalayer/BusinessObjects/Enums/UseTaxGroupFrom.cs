namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Tells the POS from where it should retrieve the tax group for tax calculations
    /// </summary>
    public enum UseTaxGroupFromEnum
    {
        /// <summary>
        /// When customer is added to the transaction the tax group set on the customer is used
        /// </summary>
        Customer = 0,
        /// <summary>
        /// The tax group on the store will always be used.
        /// </summary>
        Store = 1,
        /// <summary>
        /// The tax group on the current sales type will be used
        /// </summary>
        SalesType = 2
    }
}
