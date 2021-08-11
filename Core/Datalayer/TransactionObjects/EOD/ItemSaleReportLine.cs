using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    public class ItemSaleReportLine : DataEntity
    {
        public ItemSaleReportLine()
        {
            ItemDescription = "";
            Quantity = 0;
            Amount = 0;
            Unit = "";
            VariantName = "";
        }

        public string ItemDescription;
        public decimal Quantity;
        public decimal Amount;
        public string Unit;
        public string VariantName;

    }
}
