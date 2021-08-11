using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    /// <summary>
    /// An enum that defines sorting for pos menus
    /// </summary>
    public enum PosMenuHeaderSorting
    {
        /// <summary>
        /// Sort by menu id
        /// </summary>
        MenuID,
        /// <summary>
        /// Sort by menu description
        /// </summary>
        MenuDescription,
        /// <summary>
        /// Sort by style name
        /// </summary>
        StyleName,
        /// <summary>
        /// Sort by Import date and time
        /// </summary>
        ImportDateTime
    }
}