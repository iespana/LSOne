using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IRetailGroupsData : IDataProviderBase<RetailGroups>
    {
        /// <summary>
        /// Get RetailGroup List from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>List of retail group</returns>
        List<RetailGroups> GetRetailGroupList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Save RetailGroup list into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupsList">RetailGroups List</param>
        void SaveGroups(IConnectionManager entry, List<RetailGroups> retailGroupsList);

        /// <summary>
        /// Gets a retail group with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The ID of the retail group to get</param>
        /// <returns>A retail group with a given ID, or null if not found</returns>
        RetailGroup GetSelectedRetailGroup(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// New Save method for RetailItemModule to save retail item module into database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="module">RetailItemModule</param>
        void SmartSave(IConnectionManager entry, RetailItemOld.RetailItemModule module);

        /// <summary>
        /// Generates new RetailGroupID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>new RetailGroupId</returns>
        RecordIdentifier GenerateRetailGroupID(IConnectionManager entry);

        void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATERETAILGROUPS");
    }
}