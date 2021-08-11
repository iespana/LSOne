using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    public enum OldItemLedgerType
    {
        Sale, VoidedLine, VoidedSale, Purchase, Adjustment
    }

    public class OldItemLedger : DataEntity
    {
        public OldItemLedger()
        {
            ItemID = "";
            TransactionID = "";
            RecieptNumber = "";
            StoreName = "";
            StoreID = "";
            TerminalName = "";
            TerminalID = "";
            Customer = "";
        }

        public RecordIdentifier ItemID { get; set; }
        public DateTime Time { get; set; }
        public RecordIdentifier TransactionID { get; set; }
        public RecordIdentifier RecieptNumber { get; set; }
        public string StoreName { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string TerminalName { get; set; }
        public RecordIdentifier TerminalID { get; set; }
        public decimal Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal NetDiscount { get; set; }
        public RecordIdentifier Customer { get; set; }
        public OldItemLedgerType LedgerType { get; set; }
    }
}
