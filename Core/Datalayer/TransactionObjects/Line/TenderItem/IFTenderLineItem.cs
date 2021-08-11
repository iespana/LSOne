using System;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.Line.IFTenderItem
{
    [Serializable]
    public class IFTenderLineItem
    {
        public string TenderType { get; set; }
        public string Currency { get; set; }
        public decimal AmountTendered { get; set; }
        public decimal AmountCurrency { get; set; }
        public decimal Quantity { get; set; }
        public decimal LoyaltyPoints { get; set; }
    }
}
