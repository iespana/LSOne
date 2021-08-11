using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IInventoryTemplateStoreConnectionData : IDataProvider<InventoryTemplateStoreConnection>
    {
        List<InventoryTemplateStoreConnection> GetList(IConnectionManager entry,
            RecordIdentifier templateId);

        void DeleteForStore(IConnectionManager entry, RecordIdentifier storeID);
        void DeleteForTemplate(IConnectionManager entry, RecordIdentifier templateID);
    }
}