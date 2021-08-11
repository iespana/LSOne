using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Invoice
{
    public class Invoice : DataEntity
    {
        public Invoice()
            : base()
        {
            Created = new DateTime(1900,1,1);
            Total = 0;
            Paid = 0;
            Balance = 0;
            Currency = RecordIdentifier.Empty;
        }
        public DateTime Created { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public decimal Paid { get; set; }
        public RecordIdentifier Currency { get; set; }
    }
}
