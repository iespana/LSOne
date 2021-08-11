using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Container class used for sending inventory tranfers and it's lines
    /// between applications
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(DataEntity))]
    [KnownType(typeof(InventoryTransferOrder))]
    [KnownType(typeof(InventoryTransferOrderLine))]
    [KnownType(typeof(List<InventoryTransferOrderLine>))]
    public class InventoryTransferContainer : DataEntity
    {
        [DataMember]
        public InventoryTransferOrder InventoryTransferOrder { get; set; }
        [DataMember]
        public List<InventoryTransferOrderLine> InventoryTransferLines { get; set; }
    }
}
 