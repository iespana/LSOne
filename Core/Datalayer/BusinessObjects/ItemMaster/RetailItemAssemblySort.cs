namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Sorting enum for assembly items
    /// </summary>
    public enum RetailItemAssemblySort
    {
        Description = 1,
        Store = 2,
        StartingDate = 3,
        Cost = 4,
        Price = 5,
        Margin = 6,
        DisplayWithComponents = 7,
        SendComponentsToKds = 8,
        CalculatePrice = 9
    }

    /// <summary>
    /// Sorting enum for assembly components
    /// </summary>
    public enum RetailItemAssemblyComponentSort
    {
        ItemID = 0,
        ItemName = 1,
        VariantName = 2,
        Quantity = 3,
        Unit = 4,
        CostPerUnit = 5,
        TotalCost = 6
    }
}
