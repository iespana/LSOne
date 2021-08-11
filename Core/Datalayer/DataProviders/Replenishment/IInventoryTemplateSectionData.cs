using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IInventoryTemplateSectionData : IDataProviderBase<InventoryTemplateSection>
    {
        List<InventoryTemplateSection> GetList(IConnectionManager entry, RecordIdentifier templateId);

        /// <summary>
        /// Inserts a given tempate section to the database.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="templateSection"></param>
        /// <returns></returns>
        void Insert(IConnectionManager entry, InventoryTemplateSection templateSection);

        void Delete(IConnectionManager entry, RecordIdentifier templateId);
    }
}