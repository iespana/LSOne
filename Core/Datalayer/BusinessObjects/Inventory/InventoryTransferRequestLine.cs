using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public enum InventoryTransferRequestLineSortEnum
    {
        ItemID,
        ItemName,
        VariantName,
        Barcode,
        RequestedQuantity,
        Sent
    }

    /// <summary>
    /// Data entity class for an inventory transfer request line
    /// </summary>
    public class InventoryTransferRequestLine : DataEntity
    {
        public static bool operator ==(InventoryTransferRequestLine a, InventoryTransferRequestLine b)
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
                && a.ItemId == b.ItemId
                && a.UnitId == b.UnitId
                && a.QuantityRequested == b.QuantityRequested
                && a.Sent == b.Sent
                && a.Barcode == b.Barcode;
        }

        public static bool operator !=(InventoryTransferRequestLine a, InventoryTransferRequestLine b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is InventoryTransferRequestLine)
            {
                return this == (InventoryTransferRequestLine)obj;
            }
            return false;

        }

        public override int GetHashCode()
        {
            return ((Guid)ID).GetHashCode();
        }

        public RecordIdentifier InventoryTransferRequestId { get; set; }
        public RecordIdentifier ItemId { get; set; }
        public string ItemName { get; set; }
        public string VariantName { get; set; }
        public RecordIdentifier UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal QuantityRequested { get; set; }
        public bool Sent { get; set; }
        public string Barcode { get; set; }
    }
}
 