using System.Collections.Generic;
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
    public partial class SiteServiceService
    {
        public virtual List<InventoryTemplateListItem> GetInventoryTemplateList(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, InventoryTemplateListFilter filter, bool closeConnection)
        {
            List<InventoryTemplateListItem> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateList(CreateLogonInfo(entry), filter), closeConnection);

            return result;
        }

        public virtual List<InventoryTemplateListItem> GetInventoryTemplateListForStore(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, InventoryTemplateListFilter filter, bool closeConnection)
        {
            List<InventoryTemplateListItem> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateListForStore(CreateLogonInfo(entry), storeID, filter), closeConnection);

            return result;
        }

        public virtual string GetInventoryTemplateFirstStoreName(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateId, bool closeConnection)
        {
            string result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateFirstStoreName(CreateLogonInfo(entry), templateId),
                closeConnection);

            return result;
        }

        public virtual InventoryTemplate GetInventoryTemplate(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            InventoryTemplate result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplate(CreateLogonInfo(entry), templateID), closeConnection);

            return result;
        }

        public virtual RecordIdentifier SaveInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryTemplate template,
            bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryTemplate(CreateLogonInfo(entry), template),
                closeConnection);
            return result;
        }

        public virtual void SaveInventoryTemplateStoreConnection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTemplateStoreConnection template, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.SaveInventoryTemplateStoreConnection(CreateLogonInfo(entry), template), closeConnection);
        }

        public virtual void DeleteInventoryTemplateStoreConnection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryTemplateStoreConnection(CreateLogonInfo(entry), storeID), closeConnection);
        }

        public virtual void DeleteInventoryTemplateTemplateConnection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryTemplateTemplateConnection(CreateLogonInfo(entry), templateID),
                closeConnection);
        }

        public virtual List<InventoryTemplateStoreConnection> GetInventoryTemplateStoreConnectionList(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            List<InventoryTemplateStoreConnection> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateStoreConnectionList(CreateLogonInfo(entry), templateID),
                closeConnection);

            return result;
        }

        public virtual InventoryTemplateListItem GetInventoryTemplateListItem(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            InventoryTemplateListItem result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateListItem(CreateLogonInfo(entry), templateID), closeConnection);

            return result;
        }

        public virtual InventoryTemplateDeleteResult DeleteInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            InventoryTemplateDeleteResult result = InventoryTemplateDeleteResult.None;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.DeleteInventoryTemplate(CreateLogonInfo(entry), templateID), closeConnection);

            return result;
        }

        public virtual List<InventoryTemplateSection> GetInventoryTemplateSectionList(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            List<InventoryTemplateSection> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplateSectionList(CreateLogonInfo(entry), templateID),
                closeConnection);

            return result;
        }

        public virtual void InsertInventoryTemplateSection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTemplateSection templateSection, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.InsertInventoryTemplateSection(CreateLogonInfo(entry), templateSection), closeConnection);
        }

        public virtual List<InventoryTemplateSectionSelection> GetInventoryTemplateSectionSelectionList(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnection)
        {
            List<InventoryTemplateSectionSelection> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result =
                        server.GetInventoryTemplateSectionSelectionList(CreateLogonInfo(entry), templateID, sectionID),
                closeConnection);

            return result;
        }

        public virtual void InsertInventoryTemplateSectionSelection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            InventoryTemplateSectionSelection templateSectionSelection, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.InsertInventoryTemplateSectionSelection(CreateLogonInfo(entry), templateSectionSelection),
                closeConnection);
        }

        public virtual List<RecordIdentifier> GetInventoryTemplateSectionSelectionListForFilter(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnection)
        {
            List<RecordIdentifier> result = null;

            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result =
                        server.GetInventoryTemplateSectionSelectionListForFilter(CreateLogonInfo(entry), templateID,
                            sectionID), closeConnection);

            return result;
        }

        public virtual void DeleteInventoryTemplateSection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryTemplateSection(CreateLogonInfo(entry), templateID), closeConnection);

        }

        public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection,
            InventoryTemplateFilterContainer filter)
        {
            List<InventoryTemplateFilterListItem> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTemplateItems(CreateLogonInfo(entry), filter), closeConnection);

            return result;
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
            List<InventoryTemplateFilterListItem> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTemplateItems(CreateLogonInfo(entry), templateId, storeID, getItemsWithNoVendor), closeConnection);

            return result;
        }

        public virtual void DeleteInventoryTemplateSectionSelection(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier templateID, RecordIdentifier sectionID, bool closeConnections)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryTemplateSectionSelection(CreateLogonInfo(entry), templateID, sectionID),
                closeConnections);

        }

        public virtual InventoryArea GetInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection, UsageIntentEnum usage = UsageIntentEnum.Normal)
        {
            InventoryArea result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryArea(CreateLogonInfo(entry), areaID, usage), closeConnection);
            return result;
        }

        public virtual List<InventoryArea> GetInventoryAreasList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<InventoryArea> result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryAreasList(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual void DeleteInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier lineID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryAreaLine(CreateLogonInfo(entry), lineID), closeConnection);
        }

        public virtual void DeleteInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,
                () => server.DeleteInventoryArea(CreateLogonInfo(entry), areaID), closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryArea area, bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.SaveInventoryArea(CreateLogonInfo(entry), area), closeConnection);
            return result;
        }

        public virtual RecordIdentifier SaveInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAreaLine line, bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.SaveInventoryAreaLine(CreateLogonInfo(entry), line), closeConnection);
            return result;
        }

        public virtual InventoryAreaLine GetInventoryAreaLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection)
        {
            InventoryAreaLine result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryAreaLine(CreateLogonInfo(entry), masterID), closeConnection);
            return result;
        }

        public virtual bool AreaInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaLineID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.AreaInUse(CreateLogonInfo(entry), areaLineID), closeConnection);
            return result;
        }

        public virtual List<InventoryAreaLine> GetInventoryAreaLinesByArea(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier areaID, bool closeConnection)
        {
            List<InventoryAreaLine> result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryAreaLinesByArea(CreateLogonInfo(entry), areaID), closeConnection);
            return result;
        }

        public virtual List<InventoryAreaLineListItem> GetInventoryAreaLinesListItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<InventoryAreaLineListItem> result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryAreaLinesListItems(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual List<TemplateListItem> GetInventoryTemplatesByType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateEntryTypeEnum templateEntryType, bool closeConnection)
        {
            List<TemplateListItem> result = null;
            DoRemoteWork(entry, siteServiceProfile,
                () => result = server.GetInventoryTemplatesByType(CreateLogonInfo(entry), templateEntryType), closeConnection);
            return result;
        }
    }
}
