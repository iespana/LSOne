using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        /// <summary>
        /// Retrieves information about a specific retail item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        public virtual RetailItem GetRetailItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection)
        {
            RetailItem result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetRetailItem(CreateLogonInfo(entry), itemID), closeConnection);

            return result;
        }

        /// <summary>
        /// Retrieves information about a specific retail item even it is deleted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        public virtual RetailItem GetRetailItemIncludeDeleted(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, bool closeConnection)
        {
            RetailItem result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetRetailItemIncludeDeleted(CreateLogonInfo(entry), itemID), closeConnection);

            return result;
        }

        /// <summary>
        /// Saves information about a specific retail item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="retailItem">The item to be saved</param>
        public virtual void SaveRetailItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RetailItem retailItem, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveRetailItem(CreateLogonInfo(entry), retailItem), closeConnection);
        }

        /// <summary>
        /// Saves the unit conversion
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="unitConversion">The conversion rule</param>
        public void SaveUnitConversionRule(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            UnitConversion unitConversion, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveUnitConversionRule(CreateLogonInfo(entry), unitConversion), closeConnection);
        }

        /// <summary>
        /// Updates the type on a specific item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="newType">The new type</param>
        public void SaveItemType(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, ItemTypeEnum newType, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveItemType(CreateLogonInfo(entry), itemID, newType), closeConnection);
        }

        /// <summary>
        /// Get the purchase price for an item and store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="storeID">ID of the store for which to retrieve the cost. Empty ID will return an average cost of all stores</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public RetailItemCost GetRetailItemCost(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            RetailItemCost result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetRetailItemCost(CreateLogonInfo(entry), itemID, storeID), closeConnection);
            return result;
        }

        /// <summary>
        /// Get a list purchase prices for an item, for each store including an average for all stores
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public List<RetailItemCost> GetRetailItemCostList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RetailItemCostFilter filter,out int totalCount, bool closeConnection)
        {
            List<RetailItemCost> result = null;
            int totalItemCount = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetRetailItemCostList(CreateLogonInfo(entry), itemID, filter, out totalItemCount), closeConnection);
            totalCount = totalItemCount;
            return result;
        }

        ///<inheritdoc cref="ISiteServiceService"/>
        public void InsertRetailItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RetailItemCost> itemCosts, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.InsertRetailItemCosts(CreateLogonInfo(entry), itemCosts), closeConnection);
        }

        /// <summary>
        /// Move all item costs to an archive table except the last calculated cost for each store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public void ArchiveItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.ArchiveItemCosts(CreateLogonInfo(entry)), closeConnection);
        }
    }
}
