using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the purchase order misc charges
    /// </summary>
    public enum PurchaseOrderMiscChargesSorting
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
        /// Sort by type
        /// </summary>
        Type,
        /// <summary>
        /// Sort by reason
        /// </summary>
        Reason,
        /// <summary>
        /// Sort by amount
        /// </summary>
        Amount,
        /// <summary>
        /// Sort by tax amount
        /// </summary>
        TaxAmount,
        /// <summary>
        /// Sort by tax group
        /// </summary>
        TaxGroup
    };
}