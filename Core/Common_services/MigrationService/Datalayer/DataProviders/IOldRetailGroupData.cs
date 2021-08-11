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
    public interface IOldRetailGroupData : IDataProvider<RetailGroup>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing ID and name for each retail group, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted </param>
        /// <returns>A list of all retail groups, ordered by a chosen field</returns>
        List<DataEntity> GetList(IConnectionManager entry, RetailGroupSorting sortEnum);

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of retail groups, ordered by a given sort column index and a reversed ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted</param>
        /// <param name="backwardsSort">Whether to reverse the result set or not</param>
        /// <returns>A list of retail groups meeting the above criteria</returns>
        List<RetailGroup> GetDetailedList(IConnectionManager entry, RetailGroupSorting sortEnum, bool backwardsSort);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailGroup" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="retailDepartmentID">The unique ID of the department to search
        /// for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<RetailGroup> Search(IConnectionManager entry, string searchString,
            RecordIdentifier retailDepartmentID, int rowFrom, int rowTo,
            bool beginsWith, RetailGroupSorting sort);

        List<RetailGroup> AdvancedSearch(IConnectionManager entry,
            int rowFrom, int rowTo, string sort,
            out int totalRecordsMatching,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier sizeGroupID = null,
            RecordIdentifier colorGroupID = null,
            RecordIdentifier styleGroupID = null,
            string validationPeriod = null);

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <param name="recordFrom">The result number of the first item to be retrieved</param>
        /// <param name="recordTo">The result number of the last item to be retrieved</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        List<DataEntity> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId, int recordFrom,
            int recordTo);

        /// <summary>
        /// Gets a list of data entities containing ID and name of a retail items in a given retail group. The list is
        /// ordered by retail item names and items numbered between recordFrom and recordTo will be returned.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="groupId">Id of the retail group</param>
        /// <returns>A list of data entities containing IDs and names of retail items in a given retail group meeting the above criteria</returns>
        List<RetailItem> ItemsInGroup(IConnectionManager entry, RecordIdentifier groupId);

        /// <summary>
        /// Get a list of retail items not in a selected retail group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedGroupId">ID of the retail group which the items are not supposed to be in</param>
        /// <returns>A list of items meeting the above criteria</returns>
        List<ItemInGroup> ItemsNotInRetailGroup(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedGroupId);

        /// <summary>
        /// Removes a retail item with a given Id from the retail group it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to be removed</param>
        void RemoveItemFromRetailGroup(IConnectionManager entry, RecordIdentifier itemId);

        /// <summary>
        /// Adds a retail item with a given Id to a retail group with a given id.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The ID of the item to add</param>
        /// <param name="groupId">The ID of the retail group to add an item to</param>
        void AddItemToRetailGroup(IConnectionManager entry, RecordIdentifier itemId, string groupId);

        /// <summary>
        /// Returns a list of retail groups that are within a specific retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailDepartmentID">The unique ID of the retail deparment</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        List<RetailGroup> GetRetailGroupsInRetailDepartment(IConnectionManager entry, RecordIdentifier retailDepartmentID, RetailGroupSorting sortEnum, bool sortBackwards);

        /// <summary>
        /// Returns a list of retail groups that are not included in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="excludedRetailDepartmentID">The unique ID of the retail
        /// department</param>
        /// <param name="searchText">Full or partial name of a retail group to be
        /// found</param>
        /// <param name="sortEnum">Controls which columns will be sorted by in the SQL
        /// statement</param>
        /// <param name="sortBackwards">If true then the ordering will be descending
        /// otherwise it will be ascending</param>
        List<RetailGroup> GetRetailGroupsNotInRetailDepartment(
            IConnectionManager entry, 
            RecordIdentifier excludedRetailDepartmentID,
            string searchText,
            RetailGroupSorting sortEnum, 
            bool sortBackwards);

        /// <summary>
        /// Adds a specific retail group to the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        void AddRetailGroupToRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID);

        /// <summary>
        /// Clears the retail department value of the specific retail group 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        void RemoveRetailGroupFromRetailDepartment(IConnectionManager entry, RecordIdentifier retailGroupID);

        /// <summary>
        /// Returns true if the retail group is in the retail department
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailGroupID">The unique ID of the retail group</param>
        /// <param name="retailDepartmentID">The unique ID of the retail department</param>
        bool RetailGroupInDepartment(IConnectionManager entry, RecordIdentifier retailGroupID, RecordIdentifier retailDepartmentID);

        RetailGroup Get(IConnectionManager entry, RecordIdentifier ID);
    }
}