using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.ViewPlugins.Inventory
{
    /// <summary>
    /// This class contaions logic that is used by Inventory Store Transfers
    /// </summary>
    public class StoreTransfersPermissionManager
    {
        private readonly IConnectionManager connectionManager;
        private readonly StoreTransferTypeEnum pageType;
        private readonly InventoryTransferType tabType;

        public StoreTransfersPermissionManager(IConnectionManager connectionManager, StoreTransferTypeEnum pageType, InventoryTransferType tabType)
        {
            this.connectionManager = connectionManager;
            this.pageType = pageType;
            this.tabType = tabType;
        }

        /// <summary>
        /// Returns true if the user has access to all stores for the specified filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool HasAccessToAllStores(string filter)
        {
            return
                connectionManager.IsHeadOffice ||
                connectionManager.HasPermission(Permission.ManageTransfersForAllStores) ||
                (pageType == StoreTransferTypeEnum.Request && tabType == InventoryTransferType.Outgoing && filter == "SendingStore") ||
                (pageType == StoreTransferTypeEnum.Request && tabType == InventoryTransferType.Incoming && filter == "ReceivingStore") ||
                (pageType == StoreTransferTypeEnum.Order && tabType == InventoryTransferType.Outgoing && filter == "ReceivingStore") ||
                (pageType == StoreTransferTypeEnum.Order && tabType == InventoryTransferType.Incoming && filter == "SendingStore");
        }
    }
}