using System;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    public class TenderdeclarationLine : DataEntity
    {
        public TenderdeclarationLine() : base() 
        {
            Denominator = new CashDenominator();
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CountedDateTime, new RecordIdentifier(PaymentTypeID, new RecordIdentifier(Denominator.CurrencyCode, Denominator.Amount)));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }


        public RecordIdentifier CountedDateTime { get; set; }
        public RecordIdentifier PaymentTypeID { get; set; }
        public string PaymentTypeText { get; set; }
        public int Quantity { get; set; }
        public bool IsLocalCurrency { get; set; }
        public string FormattedTotalAmount { get; set; }
        public CashDenominator Denominator { get; set; }
    }
}
