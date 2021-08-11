using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    public class ReturnedItemReason
    {
        public int LineNum { get; set; }
        public ReasonCode ReasonCode { get; set; }
    }
}
