using LSOne.Services.Interfaces.SupportInterfaces;
using static LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects
{
    public class ItemSale : IItemSale
    {
        public ISaleLineItem Item { get; set; }
        public KeyInSerialNumberEnum KeyInSerialNumber { get; set; }
        public bool PreSaleInfocodes { get; set; }
        public bool SaleInfocodes { get; set; }
        public ItemSaleCancelledEnum ItemSaleCancelledReason { get; set; }
    }
}
