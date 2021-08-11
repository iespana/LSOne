using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldDimensionGroupData : IDataProvider<OldDimensionGroup>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing ID and name for each dimension group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all dimension groups, ordered by name</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Returns true if the item has dimensions 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The unique ID of the item</param>
        bool ItemIsVariantItem(IConnectionManager entry, RecordIdentifier retailItemID);

        /// <summary>
        /// Fetches all dimension groups from the database.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A enum defining how to sort the result</param>
        /// <param name="backwardsSort">Set to true if wanting backwards sort, else false</param>
        /// <returns>List of all dimension groups</returns>
        List<OldDimensionGroup> GetGroups(IConnectionManager entry, Services.Datalayer.DataProviders.Dimensions.OldDimensionGroupSorting sortBy, bool backwardsSort);

        /// <summary>
        /// Deletes all dimension combinations for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The unique ID of the item</param>
        void DeleteDimensionCombinations(IConnectionManager entry, RecordIdentifier retailItemID);

        OldDimensionGroup Get(IConnectionManager entry, RecordIdentifier groupID);
    }
}