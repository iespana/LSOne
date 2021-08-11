using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.SalesOrder
{
    public class SalesOrder : DataEntity
    {
        public SalesOrder()
            : base()
        {
            Created = new DateTime(1900, 1, 1);
            Total = 0;
            Prepaid = 0;
            Prepayment = 0;
            Balance = 0;
            CustomerId = 0;
            Currency = RecordIdentifier.Empty;
        }
        public DateTime Created { get; set; }
        public decimal Prepayment { get; set; }
        public decimal Prepaid { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public RecordIdentifier CustomerId { get; set; }
        public RecordIdentifier Currency { get; set; }
    }
}
