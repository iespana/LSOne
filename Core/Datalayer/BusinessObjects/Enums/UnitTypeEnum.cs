namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Time unit enum used f.ex. in Loyalty schemes
    /// </summary>
    public enum UnitTypeEnum
    {
        /// <summary>
        /// add days to date 
        /// </summary>
        SalesUnit = 0,
        /// <summary>
        /// add weeks to date
        /// </summary>
        InventoryUnit = 1,
        /// <summary>
        /// add months to date
        /// </summary>
        PurchaseUnit = 2,
    }
}
