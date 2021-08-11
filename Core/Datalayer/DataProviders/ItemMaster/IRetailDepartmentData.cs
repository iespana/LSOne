using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IRetailDepartmentData : IDataProvider<RetailDepartment>, ICompareListGetter<RetailDepartment>, ISequenceable
    {
        /// <summary>
        /// Get a list of all retail department, sordered by a given sort column index and a revers ordering based on a parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum indicating the column to order the data by</param>
        /// <param name="backwardsSort">Whether to reverse the result set or not</param>
        /// <returns>A list of retail departments meeting the above criteria</returns>
        List<RetailDepartment> GetDetailedList(IConnectionManager entry, RetailDepartment.SortEnum sortEnum, bool backwardsSort);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all retail departments, sorted by a given field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">The field to sort by</param>
        /// <returns>A list of data entities containing IDs and names of all retail departments</returns>
        List<DataEntity> GetList(IConnectionManager entry, RetailDepartment.SortEnum sort);

        /// <summary>
        /// Gets a list of data entities containing IDs and names of all retail departments, ordered by retail departments name.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing IDs and names of all retail departments</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="DataEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="divisionId">Id of the division, if any</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<DataEntity> Search(IConnectionManager entry, string searchString, 
            RecordIdentifier divisionId,
            int rowFrom, int rowTo, bool beginsWith, RetailDepartment.SortEnum sort);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="divisionId">Id of the division, if any</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<MasterIDEntity> SearchWithMasterID(IConnectionManager entry, string searchString,
                                                RecordIdentifier divisionId,
                                                int rowFrom, int rowTo, bool beginsWith, RetailDepartment.SortEnum sort);

        /// <summary>
        /// Adds or removes a retail department with a given Id from the retail division it is currently in.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentID">The ID of the department to be removed</param>
        /// <param name="divisionId">The ID of the division - null if department should be removed from division</param>
        void AddOrRemoveRetailDepartmentFromRetailDivision(IConnectionManager entry, RecordIdentifier departmentId, RecordIdentifier divisionId);

        /// <summary>
        /// Adds or removes multiple departments from the retail division.
        /// </summary>
        /// <remarks>Requires the 'Edit retail items' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="departmentIDs">The IDs of the departments to be removed</param>
        /// <param name="divisionId">The ID of the division - null if department should be removed from division</param>
        void AddOrRemoveRetailDepartmentsFromRetailDivision(IConnectionManager entry, List<RecordIdentifier> departmentIDs, RecordIdentifier divisionId);

        /// <summary>
        /// Get a list of retail items not in a selected retail group, that contain a given search text.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
        /// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
        /// <param name="excludedDivisionId">ID of the retail group which the items are not supposed to be in</param>
        /// <returns>A list of items meeting the above criteria</returns>
        List<RetailDepartment> DepartmentsNotInRetailDivision(
            IConnectionManager entry,
            string searchText,
            int numberOfRecords,
            RecordIdentifier excludedDivisionId);

        List<RetailDepartment> DepartmentsInDivision(IConnectionManager entry, RecordIdentifier divisionID);

        RetailDepartment Get(IConnectionManager entry, RecordIdentifier ID);

        Guid GetMasterID(IConnectionManager entry, RecordIdentifier ID);

        //string GetReadableID(IConnectionManager entry, RecordIdentifier ID);

        List<MasterIDEntity> GetMasterIDList(IConnectionManager entry);

        /// <summary>
        /// Undeletes the department. This set the DELETED flag on the department to 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="departmentId">The ID of the retail department to undelete</param>
        void Undelete(IConnectionManager entry, RecordIdentifier departmentId);

        /// <summary>
        /// Returns true if the department is marked as deleted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="departmentId">The ID of the department to check</param>
        /// <returns>True if the department is marked as deleted, false otherwise</returns>
        bool IsDeleted(IConnectionManager entry, RecordIdentifier departmentId);
    }
}