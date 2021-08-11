﻿using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.ItemMaster.Dimensions
{
    /// <summary>
    /// A enum that defines sorting for the dimension groups
    /// </summary>
    public enum DimensionGroupSorting
    {
        /// <summary>
        /// Sort by group ID
        /// </summary>
        ID,
        /// <summary>
        /// Sort by group name
        /// </summary>
        Name,
        /// <summary>
        /// Sort by size active
        /// </summary>
        SizeActive,
        /// <summary>
        /// Sort by color active
        /// </summary>
        ColorActive,
        /// <summary>
        /// Sort by style active
        /// </summary>
        StyleActive,
        /// <summary>
        /// Sort by serial number
        /// </summary>
        SerialNumberActive
    };
}