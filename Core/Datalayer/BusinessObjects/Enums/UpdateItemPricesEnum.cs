namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Types of changes that affect updating of items price with and without tax
    /// </summary>
    public enum UpdateItemTaxPricesEnum
    {
        TaxCode,
        ItemTaxGroup,
        DefaultStoreTaxGroup,
        SpecificItemTaxGroup,
        AllItems
    }
}
