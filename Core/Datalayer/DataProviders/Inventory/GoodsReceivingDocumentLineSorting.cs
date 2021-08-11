using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the goods receiving document lines
    /// </summary>
    public enum GoodsReceivingDocumentLineSorting
    {
        /// <summary>
        /// Sort by receive date
        /// </summary>
        ReceiveDate,
        /// <summary>
        /// Sort by receive quantity
        /// </summary>
        ReceiveQuantity,
        /// <summary>
        /// Sort by posted
        /// </summary>
        Posted,
        /// <summary>
        /// Sort by item name
        /// </summary>
        ItemName,
        /// <summary>
        /// Sort by variant
        /// </summary>
        Variant,
        /// <summary>
        /// Sort by ordered quantity
        /// </summary>
        OrderedQuantity,
        /// <summary>
        /// Sort by store name
        /// </summary>
        StoreName,
        /// <summary>
        /// Sort by itemID
        /// </summary>
        ItemID
    };
}