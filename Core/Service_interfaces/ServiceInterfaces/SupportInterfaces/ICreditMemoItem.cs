namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ICreditMemoItem : ISaleLineItem
    {
        decimal Amount { get; set; }
        string CreditMemoNumber { get; set; }
    }
}
