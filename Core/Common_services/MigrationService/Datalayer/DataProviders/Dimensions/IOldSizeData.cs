using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldSizeData : IDataProviderBase<OldItemWithDescription>, ISequenceable
    {
        /// <summary>
        /// Returns a list of groups that are available
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of available groups</returns>
        List<DataEntity> GetGroupList(IConnectionManager entry);

        /// <summary>
        /// Returns a data entity for the requested group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">ID of the group to get</param>
        /// <returns></returns>
        DataEntity GetGroup(IConnectionManager entry, RecordIdentifier groupID);

        /// <summary>
        /// Gets a list of sizes that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available sizes</returns>
        List<DataEntity> GetSizeList(IConnectionManager entry);

        /// <summary>
        /// Returns true if the size is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sizeID">The size ID to search for</param>
        /// <returns>Returns true if the size is being used in a dimension combination</returns>
        bool SizeIsInUse(IConnectionManager entry, RecordIdentifier sizeID);

        /// <summary>
        /// Get information about a specific size
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sizeID">The size ID</param>
        /// <returns>Returns information about the size through the <see cref="ItemWithDescription"/> class</returns>
        OldItemWithDescription GetSize(IConnectionManager entry, RecordIdentifier sizeID);

        /// <summary>
        /// Returns a list of sizes included in a size group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The size group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the sizes</returns>
        List<OldStyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort);

        /// <summary>
        /// Returns information about a specific size within a size group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The size group ID</param>
        /// <param name="sizeID">The size ID</param>
        /// <returns>Information about a size returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        OldStyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier sizeID);

        /// <summary>
        /// Gets the available sizes from a specific size group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="sizeGroup">The size group</param>
        /// <param name="includeSize">If set then the result is limited to groups that include his size ID</param>
        /// <returns>A list of available sizes for a specific size group</returns>
        List<DataEntity> GetAvailableSizes(IConnectionManager entry, RecordIdentifier sizeGroup, RecordIdentifier includeSize);

        /// <summary>
        /// Returns true if the size group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the size group ID is found</returns>
        bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves a given size item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The size item to save</param>
        /// <param name="oldID">If empty then a new size item will be created otherwise the
        /// size will be updated</param>
        void SaveGroupLine(IConnectionManager entry, OldStyleGroupLineItem item, RecordIdentifier oldID);

        /// <summary>
        /// Returns true if the size group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroupID">The unique ID of size group to be checked</param>
        bool SizeGroupExists(IConnectionManager entry, RecordIdentifier sizeGroupID);

        /// <summary>
        /// Returns true if the size exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeID">The unique ID of the size to be checked</param>
        /// <returns>
        /// Returns true if the size exists
        /// </returns>
        bool SizeExists(IConnectionManager entry, RecordIdentifier sizeID);

        /// <summary>
        /// Deletes a given size 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeID">The unique ID of the size</param>
        void DeleteSize(IConnectionManager entry, RecordIdentifier sizeID);

        /// <summary>
        /// Deletes a given size group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroupID">The unique ID of a size group</param>
        void DeleteSizeGroup(IConnectionManager entry, RecordIdentifier sizeGroupID);

        /// <summary>
        /// Deletes a size from a size group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the size to be deleted</param>
        void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves the given size to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="size">The size to be saved</param>
        void SaveSize(IConnectionManager entry, OldItemWithDescription size);

        /// <summary>
        /// Saves a given size group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sizeGroup">The size group to be saved</param>
        void SaveSizeGroup(IConnectionManager entry, DataEntity sizeGroup);
    }
}