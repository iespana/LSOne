using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Container class used for sending inventory tranfers request and it's lines
    /// between applications
    /// </summary>
    public class InventoryTransferRequestContainer : DataEntity
    {
        public InventoryTransferRequest InventoryTransferRequest { get; set; }
        public List<InventoryTransferRequestLine> InventoryTransferRequestLines { get; set; }
    }
}
 