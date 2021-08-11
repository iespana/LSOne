using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public class CustomerOrderSettingsConstants
    {
        public const string ConstCustomerOrder = "24BF119B-B909-48B4-A494-022EB6B93FD7";
        public const string ConstQuote = "A7F9FBA4-03B6-431A-B318-188EC5735809";
    }

    public class CustomerOrderReasonConstants
    {
        public const string ConstReserveStockToOrderReasonID = "COR001";
        public const string ConstVoidStockFromOrderReasonID = "COR002";
        public const string ConstPickupFromOrderReasonID = "COR003";
    }

    [DataContract(Name = "CustomerOrderType")]
    public enum CustomerOrderType
    {
        [EnumMember]
        None,
        [EnumMember]
        CustomerOrder,
        [EnumMember]
        Quote
    }

    [DataContract(Name = "ConfigurationType")]
    public enum ConfigurationType
    {
        [EnumMember]
        None,
        [EnumMember]
        Source,
        [EnumMember]
        Delivery
    }

    [DataContract(Name = "CustomerOrderActions")]
    public enum CustomerOrderAction
    {
        [EnumMember]
        None,
        [EnumMember]
        SaveChanges,
        [EnumMember]
        SaveChangesAndPrintReceiptCopy,
        [EnumMember]
        AdditionalPayment,
        [EnumMember]
        FullPickup,
        [EnumMember]
        PartialPickUp,
        [EnumMember]
        CancelQuote,
        [EnumMember]
        Cancel,
        [EnumMember]
        PayDeposit,
        [EnumMember]
        Recall,
        [EnumMember]
        ContinueToPayment,
        [EnumMember]
        ReturnFullDeposit,
        [EnumMember]
        ReturnPartialDeposit
    }

    /// <summary>
    /// The status of the customer order depending on where in the process it is
    /// </summary>
    [DataContract(Name = "CustomerOrderStatus")]
    public enum CustomerOrderStatus
    {
        /// <summary>
        /// A customer order that still has items on it on order
        /// </summary>
        [EnumMember]
        Open = 4,
        /// <summary>
        /// All items on the customer order have been delivered and paid for
        /// </summary>
        [EnumMember]
        Closed = 8,
        /// <summary>
        /// The customer order has been cancelled
        /// </summary>
        [EnumMember]
        Cancelled = 16,
        /// <summary>
        /// New is the default status when a Customer order is created. This should only be used by the POS
        /// </summary>
        [EnumMember]
        New = 32,
        /// <summary>
        /// Once the customer order has been included in a printed picking list it gets this status
        /// </summary>
        [EnumMember]
        Printed = 64,
        /// <summary>
        /// A work status that the users can set if needed
        /// </summary>
        [EnumMember]
        Ready = 128,
        /// <summary>
        /// A work status that the users can set if needed
        /// </summary>
        [EnumMember]
        Delivered = 256,
        /// <summary>
        /// If the customer order is deleted in the Site Manager it gets this status
        /// </summary>
        [EnumMember]
        Deleted = 512

    }

    [DataContract(Name = "CustomerOrderSummaries")]
    public enum CustomerOrderSummaries
    {
        [EnumMember]
        All,
        [EnumMember]
        OnOrder,
        [EnumMember]
        ToPickUp
    }

    [DataContract(Name = "DepositsStatus")]
    public enum DepositsStatus
    {
        /// <summary>
        /// An original calculated deposit line
        /// </summary>
        [EnumMember]
        Normal,
        /// <summary>
        /// A deposit line that has been returned
        /// </summary>
        [EnumMember]
        Returned,
        /// <summary>
        /// A deposit line that was cancelled f.ex. when a line is voided before the deposit has been paid
        /// </summary>
        [EnumMember]
        Cancelled,
        /// <summary>
        /// A deposit amount that was originally on another item but moved to this due to voiding f.ex.
        /// </summary>
        [EnumMember]
        Distributed
    }

    [DataContract(Name = "PaymentStatus")]
    public enum PaymentStatus
    {
        /// <summary>
        /// An original calculated payment line
        /// </summary>
        [EnumMember]
        Normal,
        /// <summary>
        /// A payment line that has been returned
        /// </summary>
        [EnumMember]
        Returned,
        /// <summary>
        /// A payment line that was cancelled f.ex. when a line is voided before the payment has been paid
        /// </summary>
        [EnumMember]
        Cancelled
        
    }
}
