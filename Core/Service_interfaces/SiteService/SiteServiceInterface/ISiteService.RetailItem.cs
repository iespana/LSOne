using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Retrieves information about a specific retail item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        [OperationContract]
        RetailItem GetRetailItem(LogonInfo logonInfo, RecordIdentifier itemID);


        /// <summary>
        /// Saves information about a specific retail item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItem">The item to be saved</param>
        [OperationContract]
        void SaveRetailItem(LogonInfo logonInfo, RetailItem retailItem);

        /// <summary>
        /// Gets information about a specific retail item even it is deleted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItem">The item to get< /param>
        [OperationContract]
        RetailItem GetRetailItemIncludeDeleted(LogonInfo logonInfo, RecordIdentifier itemID);

        /// <summary>
        /// Saves the unitconversion rule
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="unitConversion">the conversion</param>
        [OperationContract]
        void SaveUnitConversionRule(LogonInfo logonInfo, UnitConversion unitConversion);

        /// <summary>
        /// Updates the type on a specific item
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="newType">The new type</param>
        [OperationContract]
        void SaveItemType(LogonInfo logonInfo, RecordIdentifier itemID, ItemTypeEnum newType);

        /// <summary>
        /// Get the purchase price for an item and store
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="storeID">ID of the store for which to retrieve the cost. Empty ID will return an average cost of all stores</param>
        /// <returns></returns>
        [OperationContract]
        RetailItemCost GetRetailItemCost(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID);

        /// <summary>
        /// Get a list purchase prices for an item, for each store including an average for all stores
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <returns></returns>
        [OperationContract]
        List<RetailItemCost> GetRetailItemCostList(LogonInfo logonInfo, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount);

        /// <summary>
        /// Insert a list of retail item costs
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <param name="itemCosts">List of item costs to insert</param>
        [OperationContract]
        void InsertRetailItemCosts(LogonInfo logonInfo, List<RetailItemCost> itemCosts);

        /// <summary>
        /// Move all item costs to an archive table except the last calculated cost for each store
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <returns></returns>
        [OperationContract]
        void ArchiveItemCosts(LogonInfo logonInfo);
    }
}
