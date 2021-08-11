using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the purchase order lines
    /// </summary>
    public enum PurchaseOrderLineSorting
    {
        /// <summary>
        /// Sort by PurchaseOrderID
        /// </summary>
        PurchaseOrderID,
        /// <summary>
        /// Sort by LineNumber
        /// </summary>
        LineNumber,
        /// <summary>
        /// Sort by ItemID
        /// </summary>
        ItemID,
        /// <summary>
        /// Sort by VendorItemID
        /// </summary>
        VendorItemID,
        /// <summary>
        /// Sort by VariantID
        /// </summary>
        VariantID,
        /// <summary>
        /// Sort by UnitID
        /// </summary>
        UnitID,
        /// <summary>
        /// Sort by Quantity
        /// </summary>
        Quantity,
        /// <summary>
        /// Sort by Price
        /// </summary>
        Price,
        /// <summary>
        /// Sort by ItemName
        /// </summary>
        ItemName,
        /// <summary>
        /// Sort by discount amount
        /// </summary>
        DiscountAmount,
        /// <summary>
        /// Sort by discount percentage
        /// </summary>
        DiscountPercentage,
        /// <summary>
        /// Sort by tax amount
        /// </summary>
        TaxAmount,
        /// <summary>
        /// No sorting set
        /// </summary>
        None

    };
}