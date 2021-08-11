using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface ICustomerPaymentTransaction : IPosTransaction
    {
        decimal Amount { get; set; }
        decimal Payment { get; set; }
        ICustomerDepositItem ICustomerDepositItem { get; set; }
        decimal TransSalePmtDiff { get; set; }
        decimal RoundingSalePmtDiff { get; set; }

        int NoOfDeposits { get; set; }
    }
}
