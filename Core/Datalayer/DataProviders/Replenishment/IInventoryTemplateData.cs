using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IInventoryTemplateData : IDataProvider<InventoryTemplate>, ISequenceable
    {
        string GetFirstStoreName(IConnectionManager entry, RecordIdentifier templateId);
        InventoryTemplateListItem GetListItem(IConnectionManager entry, RecordIdentifier templateId);
        List<InventoryTemplateListItem> GetList(IConnectionManager entry, InventoryTemplateListFilter filter);
        List<InventoryTemplateListItem> GetListForStore(IConnectionManager entry, RecordIdentifier storeId, InventoryTemplateListFilter filter);
        List<InventoryTemplate> GetTemplatesOnAllStores(IConnectionManager entry);
        InventoryTemplate Get(IConnectionManager entry, RecordIdentifier templateId);
        List<TemplateListItem> GetInventoryTemplatesByType(IConnectionManager entry, TemplateEntryTypeEnum templateEntryType);
    }
}