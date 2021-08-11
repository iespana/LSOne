using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ICustomerDiscountItem : ILineDiscountItem
    {
        string CustomerDiscountGroup { get; set; }
        CustomerDiscountTypes CustomerDiscountType { get; set; }
        string ItemDiscountGroup { get; set; }
    }
}
