using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Wrapper for the store transfers use in the POS when adding or editing store transfers
    /// </summary>
    public class StoreTransferWrapper
    {
        public InventoryTransferOrder TransferOrder { get; set; }
        public InventoryTransferRequest TransferRequest { get; set; }
        public StoreTransferTypeEnum TransferType { get; }
        public InventoryTransferType TransferDirection { get; }

        public bool IsNewTransfer => TransferOrder == null && TransferRequest == null;
        public RecordIdentifier ID => IsNewTransfer ? RecordIdentifier.Empty : TransferType == StoreTransferTypeEnum.Order ? TransferOrder.ID : TransferRequest.ID;
        public string Description => IsNewTransfer ? "" : TransferType == StoreTransferTypeEnum.Order ? TransferOrder.Text : TransferRequest.Text;
        public DateTime ExpectedDelivery => IsNewTransfer ? Date.Empty.DateTime : TransferType == StoreTransferTypeEnum.Order ? TransferOrder.ExpectedDelivery : TransferRequest.ExpectedDelivery;

        public StoreTransferWrapper(StoreTransferTypeEnum transferType, InventoryTransferType transferDirection)
        {
            TransferType = transferType;
            TransferDirection = transferDirection;
        }

        public StoreTransferWrapper(InventoryTransferOrder order, InventoryTransferType transferDirection)
        {
            TransferOrder = order;
            TransferType = StoreTransferTypeEnum.Order;
            TransferDirection = transferDirection;
        }

        public StoreTransferWrapper(InventoryTransferRequest request, InventoryTransferType transferDirection)
        {
            TransferRequest = request;
            TransferType = StoreTransferTypeEnum.Request;
            TransferDirection = transferDirection;
        }
    }
}
