using System;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Defines the group depth when printing an item sales report
    /// </summary>
    public enum ItemSaleReportGroupEnum
    {
        /// <summary>
        /// Print item sales report based on items (default)
        /// </summary>
        RetailItem = 0,
        /// <summary>
        /// Print item sales report based on special groups
        /// </summary>
        SpecialGroup = 1,
        /// <summary>
        /// Print item sales report based on retail groups
        /// </summary>
        RetailGroup = 2,
        /// <summary>
        /// Print item sales report based on retail departments
        /// </summary>
        RetailDepartment = 3,
        /// <summary>
        /// Print item sales report based on retail divisions
        /// </summary>
        RetailDivision = 4
    }

    public static class ItemSaleReportGroupHelper
    {
        /// <summary>
        /// Gets the <see cref="ItemSaleReportGroupEnum">item type</see> localized description.
        /// </summary>
        public static string ItemSaleReportGroupToString(ItemSaleReportGroupEnum reportGroup)
        {
            switch (reportGroup)
            {
                case ItemSaleReportGroupEnum.RetailItem:
                    return Properties.Resources.Item;
                case ItemSaleReportGroupEnum.SpecialGroup:
                    return Properties.Resources.SpecialGroup;
                case ItemSaleReportGroupEnum.RetailGroup:
                    return Properties.Resources.RetailGroup;
                case ItemSaleReportGroupEnum.RetailDepartment:
                    return Properties.Resources.RetailDepartment;
                case ItemSaleReportGroupEnum.RetailDivision:
                    return Properties.Resources.RetailDivision;
                default:
                    return string.Empty;
            }
        }

        public static List<DataEntity> GetList()
        {
            Array enumValues = Enum.GetValues(typeof(ItemSaleReportGroupEnum));
            List<DataEntity> data = new List<DataEntity>();

            foreach(var v in enumValues)
            {
                data.Add(new DataEntity(((int)v).ToString(), ItemSaleReportGroupToString((ItemSaleReportGroupEnum)v)));
            }

            return data;
        }
    }
}
