using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IOldSpecialGroupData : IDataProvider<DataEntity>, ISequenceable
    {
        /// <summary>
        /// Gets a list of of data entities containing IDs and names for all special groups, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwords">A enum that defines how the result should be sorted</param>
        /// <returns>A list of all special groups, ordered by a chosen field</returns>
        List<DataEntity> GetList(IConnectionManager entry, OldSpecialGroupSorting sortBy, bool sortBackwords);

        /// <summary>
        /// Gets a list of of data entities containing IDs and names for all special groups, ordered by special group names
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all special groups</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of retail items in a given special group. The list is ordered by retail item names, 
        /// and items numbered between recordFrom and recordTo will be returned
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">The ID of the special group</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <param name="sortBy">Defines the column to sort the result set by</param>
        /// <param name="sortedBackwards">Set to true if wanting to sort the result set backwards</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given special group meeting the above criteria</returns>
        List<DataEntity> ItemsInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId,
            int recordFrom, int recordTo, OldSpecialGroupItemSorting sortBy,
            bool sortedBackwards);

        /// <summary>
        /// Get a list of retail items not in a selected special group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedGroupId">The ID of the special group to exclude</param>
        /// <returns>A list of retail items meeting the above criteria</returns>
        List<OldItemInGroup> ItemsNotInSpecialGroup(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedGroupId);

        bool ItemInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId, RecordIdentifier itemId);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="DataEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith);

        /// <summary>
        /// Removes a retail item from a special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the item to remove</param>
        /// <param name="groupId">ID of the special group</param>
        void RemoveItemFromSpecialGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId);

        /// <summary>
        /// Adds a retail item to a special group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the item to add</param>
        /// <param name="groupId">ID of the special group</param>
        void AddItemToSpecialGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId);

        /// <summary>
        /// Gets a list of SpecialGroupItem entities, one for each special group. Each entity contains information about a special group, 
        /// an item with the given ID and whether the item is in the group or not.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item we want to see if is contained in the groups</param>
        /// <returns>A list of data entities that show information about all the special groups and whether the item with the given ID is contained in each group</returns>
        List<OldSpecialGroupItem> GetItemsGroupInformation(IConnectionManager entry, RecordIdentifier itemId);

        DataEntity Get(IConnectionManager entry, RecordIdentifier ID);
    }
}