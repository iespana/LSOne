using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface IDiscountVoucherItem : ISaleLineItem
    {
        int SourceLineNum { get; set; }        

    }
}
