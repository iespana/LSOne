using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the vendor items
    /// </summary>
    public enum VendorItemSorting
    {
        /// <summary>
        /// Sort by ID
        /// </summary>
        ID,
        /// <summary>
        /// Sort by description
        /// </summary>
        Description,
        /// <summary>
        /// Sort by vendor item id
        /// </summary>
        VendorItemID,
        /// <summary>
        /// Sort by size description
        /// </summary>
        SizeDescription,
        /// <summary>
        /// Sort by color description
        /// </summary>
        ColorDescription,
        /// <summary>
        /// Sort by Style description
        /// </summary>
        StyleDescription,
        /// <summary>
        /// Sort by unit description
        /// </summary>
        UnitDescription,
        /// <summary>
        /// Sort by item price
        /// </summary>
        ItemPrice,
        /// <summary>
        /// Sort by last order date
        /// </summary>
        LastOrderDate,
        /// <summary>
        /// Sort by variant name
        /// </summary>
        VendorName,
        /// <summary>
        /// Sort by default purhcase price
        /// </summary>
        DefaultPurchasePrice,
        /// <summary>
        /// Sort by variant
        /// </summary>
        Variant
    };
}