using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for goods receving documents
    /// </summary>
    public enum GoodsReceivingDocumentSorting
    {
        /// <summary>
        /// Sort by goods receving documents IDs
        /// </summary>
        GoodsReceivingID,
        /// <summary>
        /// Sort by goods receiving documents statuses
        /// </summary>
        Status,
        /// <summary>
        /// Sort by vendor names
        /// </summary>
        VendorName
    };
}