using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService : IService
    {
        /// <summary>
        /// Disconnects the connection to the Site service
        /// </summary>
        /// <param name="entry"></param>
        void Disconnect(IConnectionManager entry);

        /// <summary>
        /// Tests the connection to the Site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="host">The IP address where the Site service is running</param>
        /// <param name="port">The port the Site Service is configured to listen to</param>
        /// <returns>The <see cref="ConnectionEnum"/> reports how the connection test went</returns>
        ConnectionEnum TestConnection(IConnectionManager entry, string host, UInt16 port);

        /// <summary>
        /// Tests the connection to the Site service and displays a msg with the result of the test
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="host">The IP address where the Site service is running</param>
        /// <param name="port">The port the Site Service is configured to listen to</param>
        /// <returns>The <see cref="ConnectionEnum"/> reports how the connection test went</returns>
        ConnectionEnum TestConnectionWithFeedback(IConnectionManager entry, string host, UInt16 port);

        /// <summary>
        /// Gets the amount of available inventory
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item to check </param>
        /// <param name="storeID">The store to find available inventory for</param>
        /// <returns></returns>
        decimal GetInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

        /// <summary>
        /// Get the status of inventory for item and store sorted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item </param>
        /// <param name="storeID">The store</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">The sort method</param>
        /// <param name="backwardsSort">The sort direction</param>
        /// <returns></returns>
        List<InventoryStatus> GetInventoryListForItemAndStore(
        IConnectionManager entry, SiteServiceProfile siteServiceProfile,
        RecordIdentifier itemID,
        RecordIdentifier storeID,
        RecordIdentifier regionID,
        InventorySorting sort,
        bool backwardsSort, bool closeConnection);

        /// <summary>
        /// Returns the inventory unit ID for a specific item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemId">The unique ID for the item to be checked</param>
        /// <returns></returns>
        RecordIdentifier GetInventoryUnitId(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, bool closeConnection);

        void UpdateInventoryUnit(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, decimal conversionFactor, bool closeConnection);

        /// <summary>
        /// Gets itemledgerentries for the given searchparameters
        /// </summary>  
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemSearch">The search manifest</param>
        /// <returns>The Item Ledger</returns>
        List<ItemLedger> GetItemLedgerList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ItemLedgerSearchParameters itemSearch, bool closeConnection);

        /// <summary>
        /// Get the status of inventory for item and store sorted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item </param>
        /// <param name="storeID">The store</param>
        /// <param name="regionID">The region's ID. Note that if this is RecordIdentifier.Empty then results for all regions will be returned. If the storeID param is not empty, this will be ignored.</param>
        /// <param name="sort">The sort method</param>
        /// <param name="backwardsSort">The sort direction</param>
        /// <returns></returns>
        List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(
        IConnectionManager entry, SiteServiceProfile siteServiceProfile,
        RecordIdentifier itemID,
        RecordIdentifier storeID,
        RecordIdentifier regionID,
        InventorySorting sort,
        bool backwardsSort, bool closeConnection);

        /// <summary>
        /// Gets the amount of available inventory
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="itemID">The item to check </param>
        /// <param name="storeID">The store to find available inventory for</param>
        /// <returns></returns>
        decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

    }
}
