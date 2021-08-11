using System;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.BusinessObjects.Interfaces;

namespace LSRetail.StoreController.BusinessObjects.Customers
{
    /// <summary>
    /// Data entity class for a Customer order lines
    /// </summary>
    public class CustomerOrderLine : DataEntity, ITaxAmount
    {
        public CustomerOrderLine()
        {
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CustomerOrderLineID);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier CustomerOrderLineID { get; set; }
        public RecordIdentifier CustomerOrderID { get; set; }
        public int LineNumber { get; set; }
        public RecordIdentifier ItemID { get; set; }
        public RecordIdentifier ItemName { get; set; }
        public RecordIdentifier VariantID { get; set; }
        public string ColorName { get; set; }
        public string StyleName { get; set; }
        public string SizeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FulfilledQuantity { get; set; }
        public string Comment { get; set; }
        public RecordIdentifier TransactionID { get; set; }
    }
}
 