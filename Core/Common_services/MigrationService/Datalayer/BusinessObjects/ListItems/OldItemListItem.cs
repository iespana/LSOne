using LSOne.DataLayer.BusinessObjects;

namespace LSOne.Services.Datalayer.BusinessObjects.ListItems
{
    /// <summary>
    /// A class that represents a minor subset of a retail item. This class is used in some queries that return a list of retail items.
    /// </summary>
    public class OldItemListItem : DataEntity
    {
        /// <summary>
        /// Default constructor for the ItemListItem
        /// </summary>
        public OldItemListItem()
            : base()
        {
            NameAlias = "";
            ItemType = OldRetailItem.ItemTypeEnum.NA;
            RetailGroupName = "";
            RetailDepartmentName = "";
            TaxGroupName = "";
            ValidationPeriod = "";
        }
        /// <summary>
        /// Total number of rows
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// The name alias of the retail item.
        /// </summary>
        public string NameAlias { get; set; } 

        /// <summary>
        /// Type of the retail item, N/A, Item, BOM, Service.
        /// </summary>
        public OldRetailItem.ItemTypeEnum ItemType { get; set; }

        /// <summary>
        /// The name of the retail division that this item belongs to
        /// </summary>
        public string RetailDivisionName { get; set; }

        /// <summary>
        /// 
        /// The name of the retail group that this item belongs to
        /// </summary>
        public string RetailGroupName { get; set; }

        /// <summary>
        /// The name of the retail department that this item belongs to
        /// </summary>
        public string RetailDepartmentName { get; set; }

        /// <summary>
        /// The name of the tax group that this item belongs to
        /// </summary>
        public string TaxGroupName { get; set; }

        public string ValidationPeriod { get; set; }

        /// <summary>
        /// Item price without modifications
        /// </summary>
        public decimal Price { get; set; }
    }
}
