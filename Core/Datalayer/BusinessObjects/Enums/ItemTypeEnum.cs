using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Item types that can be created on the Site Manager to be sold on the LS POS. Currently the Site Manager only supports Item
    /// </summary>
    public enum ItemTypeEnum
    {
        /// <summary>
        /// A normal retail item
        /// </summary>
        Item = 0,
        /// <summary>
        /// An item that is a bill of material
        /// </summary>
        BOM = 1, // Bill of materials
        /// <summary>
        /// An item that represents a service
        /// </summary>
        Service = 2,
        /// <summary>
        /// A item that is master item for variant items
        /// </summary>
        MasterItem = 3,
        /// <summary>
        /// An retail item that can be assembled from components
        /// </summary>
        AssemblyItem = 4
    };

    /// <summary>
    /// Helper class for operations on <see cref="ItemTypeEnum">item type</see> enumeration.
    /// </summary>
    public static class ItemTypeHelper
    {
        /// <summary>
        /// Gets the <see cref="ItemTypeEnum">item type</see> localized description.
        /// </summary>
        public static string ItemTypeToString(ItemTypeEnum itemType)
        {
            switch (itemType)
            {
                case ItemTypeEnum.Item:
                    return Properties.Resources.RetailItem;
                case ItemTypeEnum.MasterItem:
                    return Properties.Resources.MasterItem;
                case ItemTypeEnum.AssemblyItem:
                    return Properties.Resources.AssemblyItem;
                case ItemTypeEnum.Service:
                    return Properties.Resources.ServiceItem;
                default:
                    return string.Empty;
            }
        }

        public static ItemTypeEnum StringToItemType(string value)
        {
            if (value == Properties.Resources.RetailItem)
                return ItemTypeEnum.Item;
            if (value == Properties.Resources.MasterItem)
                return ItemTypeEnum.MasterItem;
            if (value == Properties.Resources.AssemblyItem)
                return ItemTypeEnum.AssemblyItem;
            if (value == Properties.Resources.ServiceItem)
                return ItemTypeEnum.Service;

            return ItemTypeEnum.Item;
        }
    }
}
