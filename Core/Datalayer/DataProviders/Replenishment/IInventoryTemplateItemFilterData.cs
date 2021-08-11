using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IInventoryTemplateItemFilterData : IDataProviderBase<InventoryTemplateFilterListItem>
    {
        List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, InventoryTemplateFilterContainer filter);

        /// <summary>
		/// Returns all items that match the item filter for given inventory template.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="templateId">Id of the <see cref="BusinessObjects.Replenishment.InventoryTemplate"/></param>
        /// <param name="storeID">Store ID used in case of filtering by inventory on hand</param>
        /// <param name="getItemsWithNoVendor">If true, items that have no vendor will also be included</param>
		/// <returns></returns>
		List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor);
    }
}