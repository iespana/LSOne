using System;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class InventoryTransferFilterExtended : InventoryTransferFilter
    {
        public InventoryTransferType TransferFilterType { get; set; }
        public decimal? ItemsFrom { get; set; }
        public decimal? ItemsTo { get; set; }
        public decimal? RequestedQuantityFrom { get; set; }
        public decimal? RequestedQuantityTo { get; set; }
        public decimal? ReceivedQuantityFrom { get; set; }
        public decimal? ReceivedQuantityTo { get; set; }
        public decimal? SentQuantityFrom { get; set; }
        public decimal? SentQuantityTo { get; set; }
        public DateTime? ExpectedFrom { get; set; }
        public DateTime? ExpectedTo { get; set; }
        public DateTime? SentFrom { get; set; }
        public DateTime? SentTo { get; set; }
        public bool? Sent { get; set; }
        public string Barcode { get; set; }
        public bool BarcodeBeginsWith { get; set; }
        public int RowFrom { get; set; }
        public int RowTo { get; set; }
        public InventoryTransferOrderLineSortEnum TransferOrderSortBy { get; set; }
        public InventoryTransferRequestLineSortEnum TransferRequestSortBy { get; set; }

        public InventoryTransferFilterExtended() : base()
        {
            ItemsFrom = null;
            ItemsTo = null;
            RequestedQuantityFrom = null;
            RequestedQuantityTo = null;
            ReceivedQuantityFrom = null;
            ReceivedQuantityTo = null;
            SentQuantityFrom = null;
            SentQuantityTo = null;
            ExpectedFrom = null;
            ExpectedTo = null;
            SentFrom = null;
            SentTo = null;
            Sent = null;
            BarcodeBeginsWith = true;
            Barcode = null;
            TransferOrderSortBy = InventoryTransferOrderLineSortEnum.ItemID;
            TransferRequestSortBy = InventoryTransferRequestLineSortEnum.ItemID;
        }
    }
}
