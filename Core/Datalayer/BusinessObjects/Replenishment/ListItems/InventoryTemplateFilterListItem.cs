using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Replenishment.ListItems
{
    public class InventoryTemplateFilterListItem : DataEntity
    {
        public string RetailGroupName { get; set; }
        public string RetailDepartmentName { get; set; }
        public string InventoryUnitDescription { get; set; }
        public string InventoryUnitId { get; set; }
        public string PurchaseUnitDescription { get; set; }
        public string PurchaseUnitId { get; set; }
        public string SalesUnitDescription { get; set; }
        public string SalesUnitId { get; set; }
        public RecordIdentifier VendorId { get; set; }
        public string VendorDescription { get; set; }
        public string DefaultItemBarcode { get; set; }
        public bool HasSetting { get; set; }
        public string VariantName { get; set; }
        
        /// <summary>
        /// The header item id, if this is a variant item.
        /// </summary>
        public RecordIdentifier HeaderItemId { get; set; }
        
        /// <summary>
        /// The vendor item id assigned to this item.s
        /// </summary>
        public RecordIdentifier VendorItemId { get; set; }

        public int TotalNumberOfRecords { get; set; }

        public InventoryTemplateFilterListItem(RecordIdentifier id, string text) :
            base(id, text)
        {

        }

        public InventoryTemplateFilterListItem() :
            base()
        {

        }
    }
}
