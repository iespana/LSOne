using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    /// <summary>
    /// A enum that defines sorting for the vendors
    /// </summary>
    public enum VendorSorting
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
        /// Sort by phone
        /// </summary>
        Phone,
        /// <summary>
        /// Sort by addresss
        /// </summary>
        Address
    };
}