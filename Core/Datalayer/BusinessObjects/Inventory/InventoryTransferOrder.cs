using System;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Represents the current type of a transfer order/request based on it's current status
    /// </summary>
    public enum InventoryTransferType
    {
        /// <summary>
        /// The transfer order/request is either new, sent or fetched by the receiving store
        /// </summary>
        Outgoing,
        /// <summary>
        /// The transfer order is fetched by the receiving store and needs to be received
        /// </summary>
        Incoming,
        /// <summary>
        /// NOT USED! This was probably used to display both Sending and ToReceive orders and requests
        /// </summary>
        SendingAndReceiving,
        /// <summary>
        /// The transfer order is either fully received or rejected
        /// </summary>
        Finished
    }

    public enum InventoryTransferOrderSortEnum
    {
        Id,
        CreatedDate,
        ReceivedDate,
        SentDate,
        SendingStore,
        ReceivingStore,
        Received,
        Sent,
        Fetched,
        InventoryTransferCreated,
        Description,
        SentQuantity,
        ReceivedQuantity,
        ItemLines,
        ExpectedDelivery
    }

    /// <summary>
    /// The action performed on a transfer order that triggered an update in inventory calculation
    /// </summary>
    public enum TransferOrderUpdateInventoryAction
    {
        /// <summary>
        /// The transfer order has been received. Inventory must be calculated for all items.
        /// </summary>
        Receive = 0,
        /// <summary>
        /// The transfer order has been sent. Inventory must be calculated for all items.
        /// </summary>
        Send = 1,
        /// <summary>
        /// The transfer order has been rejected. Inventory calculation must be reverted for items marked as sent.
        /// </summary>
        Reject = 2
    }

    /// <summary>
    /// Data entity class for an inventory transfer 
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(DateTime))]
    [KnownType(typeof(RecordIdentifier))]
    public class InventoryTransferOrder : DataEntity
    {
        public InventoryTransferOrder()
        {
            Sent = false;
            Received = false;
            FetchedByReceivingStore = false;
            Rejected = false;
            InventoryTransferRequestId = "";
            ReceivingDate = Date.Empty.DateTime;
            SentDate = Date.Empty.DateTime;
            ExpectedDelivery = Date.Empty.DateTime;
            TemplateID = "";
        }

        [DataMember]
        public RecordIdentifier InventoryTransferRequestId { get; set; }
        public bool CreatedFromTransferRequest
        {
            get { return (string) InventoryTransferRequestId != ""; }
        }
        [DataMember]
        public RecordIdentifier SendingStoreId { get; set; }
        [DataMember]
        public string SendingStoreName { get; set; }
        [DataMember]
        public RecordIdentifier ReceivingStoreId { get; set; }
        [DataMember]
        public string ReceivingStoreName { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public DateTime ReceivingDate { get; set; }
        [DataMember]
        public DateTime SentDate { get; set; }

        /// <summary>
        /// The receiving store has received the transfer and confirmed it
        /// </summary>
        [DataMember]
        public bool Received { get; set; }

        /// <summary>
        /// The transfer has been sent by the sending store
        /// </summary>
        [DataMember]
        public bool Sent { get; set; }

        /// <summary>
        /// The transfer has been fetched by the receiving store
        /// </summary>
        [DataMember]
        public bool FetchedByReceivingStore { get; set; }

        /// <summary>
        /// The transfer has been rejected by the receiving store
        /// </summary>
        [DataMember]
        public bool Rejected { get; set; }

        /// <summary>
        /// The Id of the store that created the order. This is an empty string if created by Head office
        /// </summary>
        [DataMember]
        public RecordIdentifier CreatedBy { get; set; }

        [DataMember]
        public DateTime ExpectedDelivery { get; set; }

        public bool CreatedByHeadOffice
        {
            get { return (string)CreatedBy == ""; }
        }

        [DataMember]
        public decimal NoOfItems { get; set; }

        /// <summary>
        /// True if the purchase order was created from the mobile inventory app.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        [DataMember]
        public bool CreatedFromOmni { get; set; }

        /// <summary>
        /// The ID of the template used to create the purchase order
        /// </summary>
        [DataMember]
        public RecordIdentifier TemplateID { get; set; }

        /// <summary>
        /// Current processing status of the purchase order
        /// </summary>
        public InventoryProcessingStatus ProcessingStatus { get; set; }

        public static bool operator ==(InventoryTransferOrder a, InventoryTransferOrder b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.InventoryTransferRequestId == b.InventoryTransferRequestId
                && a.SendingStoreId == b.SendingStoreId
                && a.ReceivingStoreId == b.ReceivingStoreId
                && a.CreationDate == b.CreationDate
                && a.ReceivingDate == b.ReceivingDate
                && a.SentDate == b.SentDate
                && a.Received == b.Received
                && a.Sent == b.Sent
                && a.FetchedByReceivingStore == b.FetchedByReceivingStore
                && a.ExpectedDelivery == b.ExpectedDelivery
                && a.Rejected == b.Rejected
                && a.CreatedFromOmni == b.CreatedFromOmni
                && a.TemplateID == b.TemplateID;
        }

        public static bool operator !=(InventoryTransferOrder a, InventoryTransferOrder b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is InventoryTransferOrder)
            {
                return this == (InventoryTransferOrder)obj;
            }
            return false;

        }

        public override int GetHashCode()
        {
            return ((string)ID).GetHashCode();
        }

    }
}
 