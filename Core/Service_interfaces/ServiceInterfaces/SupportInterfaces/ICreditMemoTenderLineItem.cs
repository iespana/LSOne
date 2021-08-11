using System;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ICreditMemoTenderLineItem : ITenderLineItem
    {
        DateTime AppliedDate { get; set; }
        DateTime IssuedDate { get; set; }
        string IssuedToExtraInfo { get; set; }
        string IssuedToName { get; set; }
        string IssuingStoreId { get; set; }
        string IssuingTerminalId { get; set; }
        string SerialNumber { get; set; }
    }
}
