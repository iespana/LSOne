using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Currencies
{
    public class CashDenominator : DataEntity
    {
        public CashDenominator()
            : base()
        {}

        public enum Type
        {
            Coin,
            Bill,
            TotalAmount
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(Amount, new RecordIdentifier((int)CashType, CurrencyCode));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public Type CashType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string FormattedAmount { get; set; }
        public string Denomination { get; set; }
        
        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return CurrencyCode;

                    case 1:
                        return FormattedAmount;

                    case 2:
                        return CashType.ToString();
                    case 3:
                        return Denomination;

                    default:
                        return "";
                }


            }
        }
    }
}
