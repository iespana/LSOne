namespace LSOne.Services.SplitBill
{
    public class OriginalItemInfo
    {
        public decimal ItemAmt { get; set; }
        public decimal ItemQty { get; set; }
        public int ItemCounter { get; set; }

        public OriginalItemInfo()
        {
            ItemAmt = 0M;
            ItemQty = 0M;
            ItemCounter = 0;
        }
    }
}
