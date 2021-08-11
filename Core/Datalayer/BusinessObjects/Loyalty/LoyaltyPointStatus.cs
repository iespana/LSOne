using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
    public class LoyaltyPointStatus : DataEntity
    {
        public LoyaltyPointStatus()
        {
            Clear();
        }

        public decimal Points { get; set; }
        public LoyaltyCustomer.ErrorCodes ResultCode { get; set; }
        public LoyaltyMSRCard.TenderTypeEnum? LoyaltyTenderType { get; set; }
        public RecordIdentifier CardNumber { get; set; }
        public RecordIdentifier CustomerID { get; set; }
        public string Comment { get; set; }
        public decimal CurrentValue { get; set; }

        public bool Empty
        {
            get
            {
                CardNumber = CardNumber == "" ? RecordIdentifier.Empty : CardNumber;
                return CardNumber == RecordIdentifier.Empty && LoyaltyTenderType == null && Points == decimal.Zero && ResultCode == LoyaltyCustomer.ErrorCodes.NoErrors;
            }
        }

        public void Clear()
        {
            Points = decimal.Zero;
            ResultCode = LoyaltyCustomer.ErrorCodes.NoConnectionTried;
            LoyaltyTenderType = null;
            CardNumber = RecordIdentifier.Empty;
            Comment = "";
            CurrentValue = decimal.Zero;
            CustomerID = RecordIdentifier.Empty;
        }
    }
}
