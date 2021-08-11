using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public enum PeriodicDiscOfferType
    {
        Multibuy = 0,
        MixAndMatch = 1,
        Offer = 2,
        Promotion = 3,
        None = 4
    }

    public interface IPeriodicDiscountItem : ILineDiscountItem
    {
        PeriodicDiscOfferType PeriodicDiscountType { get; set; }
    }
}
