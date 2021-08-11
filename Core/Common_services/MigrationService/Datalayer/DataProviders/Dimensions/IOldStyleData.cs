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
    public interface IOldStyleData : IDataProviderBase<OldItemWithDescription>, ISequenceable
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
        /// Gets a list of styles that are available
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <returns>A list of available styles</returns>
        List<DataEntity> GetStyleList(IConnectionManager entry);

        /// <summary>
        /// Returns true if the style is being used in a dimension combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="styleID">The style ID to search for</param>
        /// <returns>Returns true if the style is being used in a dimension combination</returns>
        bool StyleIsInUse(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Get information about a specific style
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="styleID">The style ID</param>
        /// <returns>Returns information about the style through the <see cref="ItemWithDescription"/> class</returns>
        OldItemWithDescription GetStyle(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Returns a list of styles included in a style group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The style group ID.</param>
        /// <param name="sort">How to sort the result</param>
        /// <returns>A list of <see cref="StyleGroupLineItem"/> with information about the styles</returns>
        List<OldStyleGroupLineItem> GetGroupLines(IConnectionManager entry, RecordIdentifier groupID, string sort);

        /// <summary>
        /// Returns information about a specific style within a style group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupID">The style group ID</param>
        /// <param name="styleID">The style ID</param>
        /// <returns>Information about a style returned in an instance of <see cref="StyleGroupLineItem"/> </returns>
        OldStyleGroupLineItem GetGroupLine(IConnectionManager entry, RecordIdentifier groupID, RecordIdentifier styleID);

        /// <summary>
        /// Gets the available styles from a specific style group
        /// </summary>
        /// <param name="entry">Entry to the database</param>
        /// <param name="styleGroup">The style group</param>
        /// <param name="includeStyle">If set then the result is limited to groups that include his style ID</param>
        /// <returns>A list of available styles for a specific style group</returns>
        List<DataEntity> GetAvailableStyles(IConnectionManager entry, RecordIdentifier styleGroup, RecordIdentifier includeStyle);

        /// <summary>
        /// Returns true if the style group ID is included in a combination
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupLineID">The group ID.</param>
        /// <returns>Returns true if the style group ID is found</returns>
        bool GroupLineExists(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves a given style item to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The style item to save</param>
        /// <param name="oldID">If empty then a new style item will be created otherwise the
        /// style will be updated</param>
        void SaveGroupLine(IConnectionManager entry, OldStyleGroupLineItem item, RecordIdentifier oldID);

        /// <summary>
        /// Returns true if the style group already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroupID">The unique ID of style group to be checked</param>
        bool StyleGroupExists(IConnectionManager entry, RecordIdentifier styleGroupID);

        /// <summary>
        /// Returns true if the style exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The unique ID of the style to be checked</param>
        /// <returns>
        /// Returns true if the style exists
        /// </returns>
        bool StyleExists(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Deletes a given style 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The unique ID of the style</param>
        void DeleteStyle(IConnectionManager entry, RecordIdentifier styleID);

        /// <summary>
        /// Deletes a given style group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroupID">The unique ID of a style group</param>
        void DeleteStyleGroup(IConnectionManager entry, RecordIdentifier styleGroupID);

        /// <summary>
        /// Deletes a style from a style group
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="groupLineID">The unique ID of the style to be deleted</param>
        void DeleteGroupLine(IConnectionManager entry, RecordIdentifier groupLineID);

        /// <summary>
        /// Saves the given style to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="style">The style to be saved</param>
        void SaveStyle(IConnectionManager entry, OldItemWithDescription style);

        /// <summary>
        /// Saves a given style group to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleGroup">The style group to be saved</param>
        void SaveStyleGroup(IConnectionManager entry, DataEntity styleGroup);
    }
}