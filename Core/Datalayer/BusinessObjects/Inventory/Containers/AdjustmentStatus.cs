using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    public struct AdjustmentStatus
    {
        public InventoryProcessingStatus ProcessingStatus;
        public InventoryJournalStatus InventoryStatus;
    }
}
