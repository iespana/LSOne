using LSOne.DataLayer.BusinessObjects.Enums;
using static LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItem;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IItemSale
    {
        ISaleLineItem Item { get; set; }
        KeyInSerialNumberEnum KeyInSerialNumber { get; set; }
        bool PreSaleInfocodes { get; set; }
        bool SaleInfocodes { get; set; }
        ItemSaleCancelledEnum ItemSaleCancelledReason { get; set; }
    }
}
