namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Tab keys for item view. Used when switching records to properly choose the tab since some tabs may not be present for all item types
    /// </summary>
    public enum ItemTabKey
    {
        General,
        Prices,
        Discounts,
        POSSettings,
        FuelSettings,
        SpecialGroups,
        LinkedItems,
        AdditionalInformation,
        ItemLedger,
        Inventory,
        Vendors,
        Replenishment,
        Assemblies,
        VariantItems,
        Barcodes,
        Infocodes,
        CrossSelling,
        ItemModifiers,
        Hospitality,
        Images
    }
}
