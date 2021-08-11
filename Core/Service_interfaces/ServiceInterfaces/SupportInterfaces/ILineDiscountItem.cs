using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ILineDiscountItem : IDiscountItem
    {
        DiscountTypes LineDiscountType { get; set; }
    }
}
