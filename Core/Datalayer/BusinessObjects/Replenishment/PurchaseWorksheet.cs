using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class PurchaseWorksheet: DataEntity
    {
        public RecordIdentifier InventoryTemplateID { get; set; }
        public RecordIdentifier StoreId { get; set; }
        public int NumberOfLinesInWorksheet { get; set; }
        //public InventoryTemplateListItem InventoryTemplateListItem { get; set; }
    }
}
