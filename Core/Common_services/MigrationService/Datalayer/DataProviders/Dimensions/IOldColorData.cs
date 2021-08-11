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
    public interface IOldColorData : IDataProviderBase<OldItemWithDescription>, ISequenceable
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
        /// Gets a list of colors that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available colors</returns>
        List<DataEntity> GetColorList(IConnectionManager entry);

        /// <summary>
        /// Returns true if the color is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="colorID">The color ID to search for</param>
        /// <returns>Returns true if the color is being used in a dimension combination</returns>
        bool ColorIsInUse(IConnectionManager entry, RecordIdentifier colorID);

        /// <summary>
        /// Get information about a specific color
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="colorID">The color ID</param>
        /// <returns>Returns information about the color through the <see cref="ItemWithDescription"/> class</returns>
        OldItemWithDescription GetColor(IConnectionManager entry, RecordIdentifier colorID);

        /// <summary>
        /// Returns a list of colors included in a color group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The color group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the colors</returns>
        List<OldStyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort);

        /// <summary>
        /// Returns information about a specific color within a color group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The color group ID</param>
        /// <param name="colorID">The color ID</param>
        /// <returns>Information about a color returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        OldStyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier colorID);

        /// <summary>
        /// Gets the available colors from a specific color group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="colorGroup">The color group</param>
        /// <param name="includeColor">If set then the result is limited to groups that include his color ID</param>
        /// <returns>A list of available colors for a specific color group</returns>
        List<DataEntity> GetAvailableColors(IConnectionManager entry, RecordIdentifier colorGroup,RecordIdentifier includeColor);

        /// <summary>
        /// Returns true if the color group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the color group ID is found</returns>
        bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves a given color item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The color item to save</param>
        /// <param name="oldID">If empty then a new color item will be created otherwise the
        /// color will be updated</param>
        void SaveGroupLine(IConnectionManager entry, OldStyleGroupLineItem item, RecordIdentifier oldID);

        /// <summary>
        /// Returns true if the color group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroupID">The unique ID of color group to be checked</param>
        bool ColorGroupExists(IConnectionManager entry, RecordIdentifier colorGroupID);

        /// <summary>
        /// Returns true if the color exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The unique ID of the color to be checked</param>
        /// <returns>
        /// Returns true if the color exists
        /// </returns>
        bool ColorExists(IConnectionManager entry, RecordIdentifier colorID);

        /// <summary>
        /// Deletes a given color 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The unique ID of the color</param>
        void DeleteColor(IConnectionManager entry, RecordIdentifier colorID);

        /// <summary>
        /// Deletes a given color group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroupID">The unique ID of a color group</param>
        void DeleteColorGroup(IConnectionManager entry, RecordIdentifier colorGroupID);

        /// <summary>
        /// Deletes a color from a color group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the color to be deleted</param>
        void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves the given color to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="color">The color to be saved</param>
        void SaveColor(IConnectionManager entry, OldItemWithDescription color);

        /// <summary>
        /// Saves a given color group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorGroup">The color group to be saved</param>
        void SaveColorGroup(IConnectionManager entry, DataEntity colorGroup);
    }
}