namespace LSOne.DataLayer.BusinessObjects
{
    public class Payment : DataEntity
    {
        public bool IsForeignCurrency { get; set; }
        public int RoundingMethod { get; set; }
        public decimal RoundingValue { get; set; }
        public int PosOperation { get; set; }

        public Payment() : base() { }
        
        public Payment(string paymentID, string paymentName, int posOperation)
            : base(paymentID, paymentName)
        {
            IsForeignCurrency = (posOperation == 203);
        }
    }
}
