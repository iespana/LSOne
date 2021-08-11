using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using System;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// A very small subset of a retail item. Used in stock counting operations for better performance
    /// </summary>
    public class InventoryRetailItem : DataEntity
    {
        public InventoryRetailItem()
        {
            MasterID = Guid.Empty;
            HeaderItemID = Guid.Empty;
            InventoryUnitID = RecordIdentifier.Empty;
            VariantName = "";
            InventoryOnHand = 0;
            Barcode = "";
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID { get; set; }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier HeaderItemID { get; set; }

        public RecordIdentifier InventoryUnitID { get; set; }

        public string VariantName { get; set; }

        public string Barcode { get; set; }

        public decimal InventoryOnHand { get; set; }

        public bool Deleted { get; set; }

        public ItemTypeEnum ItemType { get; set; }
    }
}
