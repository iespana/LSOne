using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IRetailDivisionData : IDataProvider<RetailDivision>, ICompareListGetter<RetailDivision>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing ID and name for each retail group, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">A sort enum that defines how the result should be sorted </param>
        /// <returns>A list of all retail groups, ordered by a chosen field</returns>
        List<DataEntity> GetList(IConnectionManager entry, RetailDivisionSorting sortEnum);

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all retail group, ordered by retail group name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of retail groups, ordered by retail group name</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        List<RetailDivision> GetDetailedList(IConnectionManager entry, RetailDivisionSorting sortEnum,
            bool backwardsSort);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        List<MasterIDEntity> GetMasterIDList(IConnectionManager entry);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="RetailDivision" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<RetailDivision> Search(IConnectionManager entry, string searchString,
            int rowFrom, int rowTo,
            bool beginsWith, RetailDivisionSorting sort);

        /// <summary>
        /// Searches for the given search text, and returns the results as a list of <see
        /// cref="MasterIDEntity" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The value to search for</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the
        /// text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        List<MasterIDEntity> SearchMasterID(IConnectionManager entry, string searchString,
            int rowFrom, int rowTo,
            bool beginsWith, RetailDivisionSorting sort);

        RetailDivision Get(IConnectionManager entry, RecordIdentifier divisionId);

        /// <summary>
        /// Undeletes the given division. This set the DELETED flag on the division to 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="divisionId">The ID of the retail division to undelete</param>
        void Undelete(IConnectionManager entry, RecordIdentifier divisionId);

        /// <summary>
        /// Returns true if the division is marked as deleted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="divisionId">The ID of the division to check</param>
        /// <returns>True if the division is marked as deleted, false otherwise</returns>
        bool IsDeleted(IConnectionManager entry, RecordIdentifier divisionId);
    }
}