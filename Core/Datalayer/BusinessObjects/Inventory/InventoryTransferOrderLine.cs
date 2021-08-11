using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public enum InventoryTransferOrderLineSortEnum
    {
        ItemName,
        SentQuantity,
        ReceivedQuantity,
        RequestedQuantity,
        Sent,
        ItemID,
        VariantName,
        Barcode
    }

    /// <summary>
    /// Data entity class for an inventory transfer line 
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class InventoryTransferOrderLine : DataEntity
    {
        public InventoryTransferOrderLine()
        {
            Sent = false;
            PictureID = RecordIdentifier.Empty;
            OmniLineID = "";
            OmniTransactionID = "";
            InventoryTransferRequestLineID = RecordIdentifier.Empty;
        }

        [DataMember]
        public RecordIdentifier InventoryTransferId { get; set; }
        [DataMember]
        public RecordIdentifier ItemId { get; set; }
        [DataMember]
        public RecordIdentifier InventoryTransferRequestLineID { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string VariantName { get; set; }
        [DataMember]
        public RecordIdentifier UnitId { get; set; }
        [DataMember]
        public string UnitName { get; set; }
        [DataMember]
        public bool Sent { get; set; }
        [DataMember]
        public decimal QuantitySent { get; set; }
        [DataMember]
        public decimal QuantityReceived { get; set; }
        [DataMember]
        public decimal QuantityRequested { get; set; }
        [DataMember]
        public string Barcode { get; set; }

        /// <summary>
        /// The ID of the image associated with this line
        /// </summary>       
        [DataMember]
        public RecordIdentifier PictureID { get; set; }

        /// <summary>
        /// The ID of the line that was assigned to it by the inventory app.         
        /// </summary>
        [DataMember]
        public string OmniLineID { get; set; }

        /// <summary>
        /// The ID of the transaction in the inventory app that this line was created on
        /// </summary>
        [DataMember]
        public string OmniTransactionID { get; set; }

        /// <summary>
        /// Last purchase price of the item from the sending store
        /// </summary>
        [DataMember]
        public decimal CostPrice { get; set; }

        public static bool operator ==(InventoryTransferOrderLine a, InventoryTransferOrderLine b)
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
            return a.InventoryTransferId == b.InventoryTransferId
                && a.ItemId == b.ItemId
                && a.InventoryTransferRequestLineID == b.InventoryTransferRequestLineID
                && a.UnitId == b.UnitId
                && a.QuantitySent == b.QuantitySent
                && a.QuantityReceived == b.QuantityReceived
                && a.QuantityRequested == b.QuantityRequested
                && a.Sent == b.Sent
                && a.Barcode == b.Barcode;
        }

        public static bool operator !=(InventoryTransferOrderLine a, InventoryTransferOrderLine b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is InventoryTransferOrderLine)
            {
                return this == (InventoryTransferOrderLine)obj;
            }
            return false;

        }

        public override int GetHashCode()
        {
            return ((Guid) ID).GetHashCode();
        }
    }
}
 