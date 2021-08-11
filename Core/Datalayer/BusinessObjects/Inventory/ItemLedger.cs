using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public enum ItemLedgerSearchOptions
    {
        All,
        Sales,
        Inventory
    }
  
    public enum ItemLedgerType
    {
        Sale, VoidedLine, VoidedSale, Purchase, Adjustment, TransferIn, TransferOut, StockCount, PostedStatement, StockReservation, ParkedInventory
    }

    public class ItemLedger : DataEntity
    {
        public ItemLedger()
        {
            ItemID = "";
            TransactionID = "";
            Reference = "";
            StoreName = "";
            StoreID = "";
            TerminalName = "";
            TerminalID = "";
            Customer = "";
            ReasonCode = "";
        }

        public RecordIdentifier ItemID { get; set; }
        public DateTime Time { get; set; }
        public RecordIdentifier TransactionID { get; set; }
        public RecordIdentifier Reference { get; set; }
        public string StoreName { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string TerminalName { get; set; }
        public RecordIdentifier TerminalID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Adjustment { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal NetDiscount { get; set; }
        public RecordIdentifier Customer { get; set; }
        public ItemLedgerType LedgerType { get; set; }
        public string Operator { get; set; }
        public string UnitName { get; set; }
        public bool Compatible { get; set; }
        public string ReasonCode { get; set; }
    }
}
