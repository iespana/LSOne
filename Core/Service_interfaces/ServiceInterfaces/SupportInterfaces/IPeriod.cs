using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IPeriod
    {
        void Add(string periodId, bool valid);
        void Clear();
        PeriodStatus IsValid(string periodId);
    }
}
