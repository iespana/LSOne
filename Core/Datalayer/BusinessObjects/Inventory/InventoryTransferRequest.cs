using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
/// <summary>
    /// Data entity class for an inventory transfer request
    /// </summary>
    public class InventoryTransferRequest : DataEntity
    {
        public InventoryTransferRequest()
        {
            CreationDate = DateTime.Now;
            SentDate = DateTime.Now;
            InventoryTransferOrderId = "";
            ExpectedDelivery = Date.Empty.DateTime;
            TemplateID = "";
        }

        public RecordIdentifier InventoryTransferOrderId { get; set; }
        public RecordIdentifier SendingStoreId { get; set; }
        public string SendingStoreName { get; set; }
        public RecordIdentifier ReceivingStoreId { get; set; }
        public string ReceivingStoreName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SentDate { get; set; }
        public bool Sent { get; set;}
        public bool FetchedByReceivingStore { get; set; }
        public bool InventoryTransferOrderCreated { get; set; }
        public RecordIdentifier CreatedBy { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public bool Rejected { get; set; }
        public decimal NoOfItems { get; set; }

        /// <summary>
        /// True if the purchase order was created from the mobile inventory app.
        /// </summary>
        public bool CreatedFromOmni { get; set; }

        /// <summary>
        /// The ID of the template used to create the purchase order
        /// </summary>
        public RecordIdentifier TemplateID { get; set; }

        /// <summary>
        /// The Id of the store that created the request. This is an empty string if created by Head office
        /// </summary>
        public bool CreatedByHeadOffice
        {
            get { return (string) CreatedBy == ""; }
        }

        public static bool operator ==(InventoryTransferRequest a, InventoryTransferRequest b)
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
            return a.SendingStoreId == b.SendingStoreId
                && a.ReceivingStoreId == b.ReceivingStoreId
                && a.CreationDate == b.CreationDate
                && a.SentDate == b.SentDate
                && a.Sent == b.Sent
                && a.FetchedByReceivingStore == b.FetchedByReceivingStore
                && a.InventoryTransferOrderCreated == b.InventoryTransferOrderCreated
                && a.ExpectedDelivery == b.ExpectedDelivery
                && a.Rejected == b.Rejected
                && a.CreatedFromOmni == b.CreatedFromOmni
                && a.TemplateID == b.TemplateID;
        }

        public static bool operator !=(InventoryTransferRequest a, InventoryTransferRequest b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is InventoryTransferRequest)
            {
                return this == (InventoryTransferRequest)obj;
            }
            return false;

        }

        public override int GetHashCode()
        {
            return ((string)ID).GetHashCode();
        } 
    }
}

 