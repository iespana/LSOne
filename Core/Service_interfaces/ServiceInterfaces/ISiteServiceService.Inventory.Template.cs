using System.Collections.Generic;

using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
	public partial interface ISiteServiceService
	{
		List<InventoryTemplateListItem> GetInventoryTemplateList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateListFilter filter, bool closeConnection);

		List<InventoryTemplateListItem> GetInventoryTemplateListForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, InventoryTemplateListFilter filter, bool closeConnection);

		string GetInventoryTemplateFirstStoreName(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId, bool closeConnection);

		InventoryTemplate GetInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateID, bool closeConnection);

		RecordIdentifier SaveInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplate template, bool closeConnection);

		void SaveInventoryTemplateStoreConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateStoreConnection template, bool closeConnection);

		void DeleteInventoryTemplateStoreConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, bool closeConnection);

		void DeleteInventoryTemplateTemplateConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateID, bool closeConnection);

		List<InventoryTemplateStoreConnection> GetInventoryTemplateStoreConnectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
			RecordIdentifier templateId, bool closeConnection);

		InventoryTemplateListItem GetInventoryTemplateListItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId,bool closeConnection);

		InventoryTemplateDeleteResult DeleteInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateID, bool closeConnection);
		List<InventoryTemplateSection> GetInventoryTemplateSectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId, bool closeConnection);

		void InsertInventoryTemplateSection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateSection templateSection, bool closeConnection);

		List<InventoryTemplateSectionSelection> GetInventoryTemplateSectionSelectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId,
			RecordIdentifier sectionID, bool closeConnection);

		void InsertInventoryTemplateSectionSelection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateSectionSelection templateSectionSelection, bool closeConnection);

		List<RecordIdentifier> GetInventoryTemplateSectionSelectionListForFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnection);
		
		void DeleteInventoryTemplateSection(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateID, bool closeConnection);

		/// <summary>
		/// Gets filter list of inventory template items
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="closeConnections"></param>
		/// <param name="filter">Container with IDs to filter</param>
		/// <returns></returns>
		List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(
			IConnectionManager entry,
			SiteServiceProfile siteServiceProfile,
			bool closeConnections,
			InventoryTemplateFilterContainer filter);

        /// <summary>
        /// Returns all items that match the item filter for given inventory template.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="templateId">Id of the <see cref="InventoryTemplate"/></param>
        /// <param name="storeID">Store ID used in case of filtering by inventory on hand</param>
        /// <param name="getItemsWithNoVendor">If true, items that have no vendor will also be included</param>
        /// <param name="closeConnection"></param>
        /// <returns></returns>
        List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor, bool closeConnection);

        void DeleteInventoryTemplateSectionSelection(IConnectionManager entry,
			SiteServiceProfile siteServiceProfile, 
			RecordIdentifier templateID, 
			RecordIdentifier sectionID,
			bool closeConnections);


		InventoryArea GetInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection, UsageIntentEnum usage = UsageIntentEnum.Normal);

		List<InventoryArea> GetInventoryAreasList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

		void DeleteInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier lineID, bool closeConnection);

		void DeleteInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection);

		RecordIdentifier SaveInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryArea area, bool closeConnection);

		RecordIdentifier SaveInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAreaLine line, bool closeConnection);

		InventoryAreaLine GetInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection);

		bool AreaInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaLineID, bool closeConnection);

		List<InventoryAreaLine> GetInventoryAreaLinesByArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection);

		List<InventoryAreaLineListItem> GetInventoryAreaLinesListItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

		List<TemplateListItem> GetInventoryTemplatesByType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateEntryTypeEnum templateEntryType, bool closeConnection);
    }
}
