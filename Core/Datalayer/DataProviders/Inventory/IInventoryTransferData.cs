using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransferData : IDataProvider<DataEntity>
    {
        /// <summary>
        /// Creates inventory transfer lines based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transferID">Transfer order id</param>
        /// <param name="storeID">Store id</param>
        /// <param name="filter">Filter container</param>
        /// <param name="transferType">The type of inventory transfer</param>
        /// <returns>Number of lines inserted</returns>
        int CreateStoreTransferLinesFromFilter(IConnectionManager entry, RecordIdentifier transferID, RecordIdentifier storeID, InventoryTemplateFilterContainer filter, StoreTransferTypeEnum transferType);
    }
}
