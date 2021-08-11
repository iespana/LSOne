using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        public virtual List<InventoryTemplateListItem> GetInventoryTemplateList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateListFilter filter, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateList(entry, siteServiceProfile, filter, closeConnection);

        }

        public virtual List<InventoryTemplateListItem> GetInventoryTemplateListForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplateListFilter filter,
            RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateListForStore(entry, siteServiceProfile, storeID, filter, closeConnection);
        }

        public virtual string GetInventoryTemplateFirstStoreName(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateFirstStoreName(entry, siteServiceProfile, templateId, closeConnection);
        }

        public virtual InventoryTemplate GetInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplate(entry, siteServiceProfile, templateID, closeConnection);

        }

        public virtual RecordIdentifier SaveInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryTemplate template,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveInventoryTemplate(entry, siteServiceProfile, template, closeConnection);

        }

        public virtual void SaveInventoryTemplateStoreConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTemplateStoreConnection template, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SaveInventoryTemplateStoreConnection(entry, siteServiceProfile, template, closeConnection);
        }

        public virtual void DeleteInventoryTemplateStoreConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryTemplateStoreConnection(entry, siteServiceProfile, storeID, closeConnection);
        }

        public virtual void DeleteInventoryTemplateTemplateConnection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryTemplateTemplateConnection(entry, siteServiceProfile, templateID, closeConnection);
        }

        public virtual List<InventoryTemplateStoreConnection> GetInventoryTemplateStoreConnectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateStoreConnectionList(entry, siteServiceProfile, templateID, closeConnection);

        }

        public virtual InventoryTemplateListItem GetInventoryTemplateListItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateListItem(entry, siteServiceProfile, templateID, closeConnection);
        }

        public virtual InventoryTemplateDeleteResult DeleteInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).DeleteInventoryTemplate(entry, siteServiceProfile, templateID, closeConnection);
        }

        public virtual List<InventoryTemplateSection> GetInventoryTemplateSectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateSectionList(entry, siteServiceProfile, templateID, closeConnection);
        }

        public virtual void InsertInventoryTemplateSection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTemplateSection templateSection, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).InsertInventoryTemplateSection(entry, siteServiceProfile, templateSection, closeConnection);
        }

        public virtual List<InventoryTemplateSectionSelection> GetInventoryTemplateSectionSelectionList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateSectionSelectionList(entry, siteServiceProfile, templateID, sectionID, closeConnection);
        }

        public virtual void InsertInventoryTemplateSectionSelection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTemplateSectionSelection templateSectionSelection, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).InsertInventoryTemplateSectionSelection(entry, siteServiceProfile, templateSectionSelection, closeConnection);
        }

        public virtual List<RecordIdentifier> GetInventoryTemplateSectionSelectionListForFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateSectionSelectionListForFilter(entry, siteServiceProfile, templateID, sectionID, closeConnection);
        }

        public virtual void DeleteInventoryTemplateSection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryTemplateSection(entry, siteServiceProfile, templateID, closeConnection);
        }

        public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection,
            InventoryTemplateFilterContainer filter)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateItems(
                entry,
                siteServiceProfile,
                closeConnection,
                filter);
        }

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
        public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateItems(entry, siteServiceProfile, templateId, storeID, getItemsWithNoVendor, closeConnection);
        }

        public virtual void DeleteInventoryTemplateSectionSelection(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnections)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryTemplateSectionSelection(
                 entry,
                 siteServiceProfile,
                 templateID,
                 sectionID,
                 closeConnections);
        }


        public virtual InventoryArea GetInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection, UsageIntentEnum usage = UsageIntentEnum.Normal)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryArea(entry, siteServiceProfile, areaID, closeConnection, usage);
        }

        public virtual List<InventoryArea> GetInventoryAreasList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryAreasList(entry, siteServiceProfile, closeConnection);
        }

        public virtual void DeleteInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier lineID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryAreaLine(entry, siteServiceProfile, lineID, closeConnection);
        }

        public virtual void DeleteInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteInventoryArea(entry, siteServiceProfile, areaID, closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryArea area, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveInventoryArea(entry, siteServiceProfile, area, closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAreaLine line, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveInventoryAreaLine(entry, siteServiceProfile, line, closeConnection);
        }

        public virtual InventoryAreaLine GetInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryAreaLine(entry, siteServiceProfile, masterID, closeConnection);
        }

        public virtual bool AreaInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaLineID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).AreaInUse(entry, siteServiceProfile, areaLineID, closeConnection);
        }

        public virtual List<InventoryAreaLine> GetInventoryAreaLinesByArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryAreaLinesByArea(entry, siteServiceProfile, areaID, closeConnection);
        }

        public virtual List<InventoryAreaLineListItem> GetInventoryAreaLinesListItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryAreaLinesListItems(entry, siteServiceProfile, closeConnection);
        }

        public virtual List<TemplateListItem> GetInventoryTemplatesByType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateEntryTypeEnum templateEntryType, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplatesByType(entry, siteServiceProfile, templateEntryType, closeConnection);
        }
    }
}
