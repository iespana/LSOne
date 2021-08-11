using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class InventoryAdjustmentFilter
    {
        public InventoryJournalTypeEnum JournalType { get; set; }
        public InventoryAdjustmentSorting Sort { get; set; }
        public bool SortBackwards { get; set; }
        public int PagingSize { get; set; }
        public InventoryProcessingStatus? ProcessingStatus { get; set; }
        public InventoryJournalStatus? Status { get; set; }
        public List<string> IdOrDescription { get; set; }
        public bool IdOrDescriptionBeginsWith { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public Date CreationDateFrom { get; set; }
        public Date CreationDateTo { get; set; }
        public Date PostedDateFrom { get; set; }
        public Date PostedDateTo { get; set; }

        public InventoryAdjustmentFilter()
        {
            ProcessingStatus = null;
            Status = null;
            IdOrDescription = new List<string>();
            IdOrDescriptionBeginsWith = true;
            CreationDateFrom = null;
            CreationDateTo = null;
            PostedDateFrom = null;
            PostedDateTo = null;
        }
    }
}
