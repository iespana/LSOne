using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the purchase orders
    /// </summary>
    public enum PurchaseOrderSorting
    {
        /// <summary>
        /// Sort by purchase order ID
        /// </summary>
        PurchaseOrderID,
        /// <summary>
        /// Sort by VendorDescription
        /// </summary>
        VendorDescription,
        /// <summary>
        /// Sort by Status
        /// </summary>
        Status,
        /// <summary>
        /// Sort by DeliveryDate
        /// </summary>
        DeliveryDate,
        /// <summary>
        /// Sort by store
        /// </summary>
        StoreName
    };
}