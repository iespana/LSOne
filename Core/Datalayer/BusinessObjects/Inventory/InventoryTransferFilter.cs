using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class InventoryTransferFilter
    {
        public RecordIdentifier StoreID { get; set; }
        public string DescriptionOrID { get; set; }
        public bool DescriptionOrIDBeginsWith { get; set; }
        public TransferOrderStatusEnum? Status { get; set;}
        public RecordIdentifier SendingStoreID { get; set; }
        public RecordIdentifier ReceivingStoreID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public InventoryTransferOrderSortEnum SortBy { get; set; }
        public bool SortDescending { get; set; }

        public InventoryTransferFilter()
        {
            DescriptionOrID = null;
            DescriptionOrIDBeginsWith = true;
            Status = null;
            StoreID = null;
            SendingStoreID = null;
            ReceivingStoreID = null;
            FromDate = null;
            ToDate = null;
            SortBy = InventoryTransferOrderSortEnum.Id;
            SortDescending = false;
        }
    }
}
