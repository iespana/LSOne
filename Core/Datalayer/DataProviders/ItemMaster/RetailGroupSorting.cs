using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    /// <summary>
    /// A enum that defines sorting for the Retail groups
    /// </summary>
    public enum RetailGroupSorting
    {
        /// <summary>
        /// Sort by group ID
        /// </summary>
        RetailGroupId,
        /// <summary>
        /// Sort by group name
        /// </summary>
        RetailGroupName,
        /// <summary>
        /// Sort by department name
        /// </summary>
        RetailDepartmentName
    };
}