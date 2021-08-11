using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {

        [OperationContract]
        List<InventoryTemplateListItem> GetInventoryTemplateList(LogonInfo logonInfo, InventoryTemplateListFilter filter);

        [OperationContract]
        List<InventoryTemplateListItem> GetInventoryTemplateListForStore(LogonInfo logonInfo, RecordIdentifier storeID, InventoryTemplateListFilter filter);

        [OperationContract]
        string GetInventoryTemplateFirstStoreName(LogonInfo logonInfo, RecordIdentifier templateId);

        [OperationContract]
        InventoryTemplate GetInventoryTemplate(LogonInfo logonInfo, RecordIdentifier templateID);

        [OperationContract]
        RecordIdentifier SaveInventoryTemplate(LogonInfo logonInfo, InventoryTemplate template);

        [OperationContract]
        void SaveInventoryTemplateStoreConnection(LogonInfo logonInfo, InventoryTemplateStoreConnection template);

        [OperationContract]
        void DeleteInventoryTemplateStoreConnection(LogonInfo logonInfo, RecordIdentifier storeID);

        [OperationContract]
        void DeleteInventoryTemplateTemplateConnection(LogonInfo logonInfo, RecordIdentifier templateID);

        [OperationContract]
        List<InventoryTemplateStoreConnection> GetInventoryTemplateStoreConnectionList(LogonInfo logonInfo,
            RecordIdentifier templateId);

        [OperationContract]
        InventoryTemplateListItem GetInventoryTemplateListItem(LogonInfo logonInfo, RecordIdentifier templateId);

        [OperationContract]
        InventoryTemplateDeleteResult DeleteInventoryTemplate(LogonInfo logonInfo, RecordIdentifier templateID);

        [OperationContract]
        List<InventoryTemplateSection> GetInventoryTemplateSectionList(LogonInfo logonInfo, RecordIdentifier templateId);

        [OperationContract]
        void InsertInventoryTemplateSection(LogonInfo logonInfo, InventoryTemplateSection templateSection);

        [OperationContract]
        List<InventoryTemplateSectionSelection> GetInventoryTemplateSectionSelectionList(LogonInfo logonInfo, RecordIdentifier templateId,
            RecordIdentifier sectionID);

        [OperationContract]
        void InsertInventoryTemplateSectionSelection(LogonInfo logonInfo, InventoryTemplateSectionSelection templateSectionSelection);

        [OperationContract]
        List<RecordIdentifier> GetInventoryTemplateSectionSelectionListForFilter(LogonInfo logonInfo, RecordIdentifier templateID, RecordIdentifier sectionID);

        [OperationContract]
        void DeleteInventoryTemplateSection(LogonInfo logonInfo, RecordIdentifier templateID);

        /// <summary>
		/// Gets filter list of inventory template items
		/// </summary>
		/// <param name="logonInfo">Log-on information</param>
		/// <param name="preview">If true, it will return only the first 50 items</param>
		/// <param name="filter">Container with IDs to filter</param>
		/// <returns></returns>
        [OperationContract]
        List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(LogonInfo logonInfo, InventoryTemplateFilterContainer filter);

        /// <summary>
        /// Returns all items that match the item filter for given inventory template.
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="templateId">Id of the <see cref="InventoryTemplate"/></param>
        /// <param name="storeID">Store ID used in case of filtering by inventory on hand</param>
        /// <param name="getItemsWithNoVendor">If true, items that have no vendor will also be included</param>
        /// <returns></returns>
        [OperationContract(Name = "GetInventoryTemplateItemsByTemplateId")]
        List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(LogonInfo logonInfo, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor);

        [OperationContract]
        void DeleteInventoryTemplateSectionSelection(LogonInfo logonInfo, RecordIdentifier templateID, RecordIdentifier sectionID);

        [OperationContract]
        InventoryArea GetInventoryArea(LogonInfo logonInfo, RecordIdentifier areaID, UsageIntentEnum usage = UsageIntentEnum.Normal);

        [OperationContract]
        List<InventoryArea> GetInventoryAreasList(LogonInfo logonInfo);

        [OperationContract]
        void DeleteInventoryAreaLine(LogonInfo logonInfo, RecordIdentifier lineID);

        [OperationContract]
        void DeleteInventoryArea(LogonInfo logonInfo, RecordIdentifier areaID);

        [OperationContract]
        RecordIdentifier SaveInventoryArea(LogonInfo logonInfo, InventoryArea area);

        [OperationContract]
        RecordIdentifier SaveInventoryAreaLine(LogonInfo logonInfo, InventoryAreaLine line);

        [OperationContract]
        InventoryAreaLine GetInventoryAreaLine(LogonInfo logonInfo, RecordIdentifier masterID);

        [OperationContract]
        bool AreaInUse(LogonInfo logonInfo, RecordIdentifier areaLineID);

        [OperationContract]
        List<InventoryAreaLine> GetInventoryAreaLinesByArea(LogonInfo logonInfo, RecordIdentifier areaID);

        [OperationContract]
        List<InventoryAreaLineListItem> GetInventoryAreaLinesListItems(LogonInfo logonInfo);

        [OperationContract]
        List<TemplateListItem> GetInventoryTemplatesByType(LogonInfo logonInfo, TemplateEntryTypeEnum templateEntryType);
    }
}
