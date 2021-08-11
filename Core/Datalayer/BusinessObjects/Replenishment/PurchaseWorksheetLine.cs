using System;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public enum PurchaseWorksheetLineSortEnum
    {
        Barcode,
        ItemId,
        Description,
        VariantName,
        UnitName,
        VendorName,
        OrderingQuantity,
        SuggestedQuantity,
        ReorderPoint,
        MaximumInventory
    }

    public class PurchaseWorksheetLine : DataEntity
    {
        public PurchaseWorksheetLine()
        {
            ID = Guid.NewGuid();
            Item = new InventoryTemplateFilterListItem();
            Unit = new DataEntity();
            InventoryUnit = new DataEntity();
            Vendor = new DataEntity();
            PurchaseWorksheetID = "";
            VariantName = "";
        }

        public RecordIdentifier PurchaseWorksheetID { get; set; }
        public string BarCodeNumber { get; set; }
        public InventoryTemplateFilterListItem Item { get; set; }
        public decimal Quantity { get; set; }

        /// <summary>
        /// Current unit selected for this line
        /// </summary>
        public DataEntity Unit { get; set; }

        /// <summary>
        /// Inventory unit of the item of this line
        /// </summary>
        public DataEntity InventoryUnit { get; set; }
        public DataEntity Vendor { get; set; }

        /// <summary>
        /// Suggested quantity for replenishment based on reorder point, maximum inventory and effective inventory. This value is calculated in inventory unit.
        /// </summary>
        public decimal SuggestedQuantity { get; set; }
        public decimal ReorderPoint { get; set; }
        public decimal MaximumInventory { get; set; }

        /// <summary>
        /// Current inventory on hand including unposted quantities from purchase orders and transfer orders
        /// </summary>
        public decimal EffectiveInventory { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string VariantName { get; set; }
        public bool Deleted { get; set; }
        public bool ManuallyEdited { get; set; }
        public bool Dirty { get; set; }

        /// <summary>
        /// Returns the <see cref="ItemTypeEnum"/> of the item.
        /// </summary>
        public ItemTypeEnum ItemType { get; set; }

        public int TotalNumberOfRows { get; set; }

        /// <summary>
        /// Returns true if current item cannot be used in inventory operations.
        /// </summary>
        public bool IsInventoryExcluded()
        {
            return Deleted || ItemType == ItemTypeEnum.Service;
        }
    }
}
