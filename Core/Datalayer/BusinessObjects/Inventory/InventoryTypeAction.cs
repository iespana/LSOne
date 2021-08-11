using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [DataContract]
    public class InventoryTypeAction
    {
        public InventoryEnum? InventoryType;
        public InventoryActionEnum? Action;
        public StoreTransferTypeEnum? StoreTransferType;

        public InventoryTypeAction()
        {
            InventoryType = null;
            Action = null;
            StoreTransferType = null;
        }
    }
}
