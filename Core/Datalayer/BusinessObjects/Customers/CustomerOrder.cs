using System;
using LSRetail.Utilities.DataTypes;

namespace LSRetail.StoreController.BusinessObjects.Customers
{
    public enum CustomerOrderFulfillmentTypeEnum
    {
        Ship = 0,
        InStorePickup = 1
    }

    public enum CustomerOrderStatusEnum
    {
        Unfulfilled,
        PartiallyFulfilled,
        Fulfilled,
        Cancelled
    }

    public enum CustomerOrderPaymentTypeEnum
    {
        NotPaid,
        PartiallyPaid,
        FullyPaid
    }

    /// <summary>
    /// Data entity class for a Customer orders
    /// </summary>
    public class CustomerOrder : DataEntity
    {
        public CustomerOrder()
        {

            CustomerID = "";
            Created = DateTime.Now;
            LastModified = Created;
            FulfilledBy = "";
            FulfilledAtStore = "";
            FulfilledAtTerminal = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return CustomerOrderID;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier CustomerOrderID { get; set; }
        public RecordIdentifier CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public CustomerOrderFulfillmentTypeEnum FulfillmentType { get; set; }
        public CustomerOrderStatusEnum Status { get; set; }
        public RecordIdentifier FulfilledBy { get; set; }
        public RecordIdentifier FulfilledAtStore { get; set; }
        public RecordIdentifier FulfilledAtTerminal { get; set; }
        public string Comment { get; set; }
        public CustomerOrderPaymentTypeEnum PaymentType { get; set; }
        public decimal AmountPaid { get; set; }
        public int TenderType { get; set; }
    }
}
 