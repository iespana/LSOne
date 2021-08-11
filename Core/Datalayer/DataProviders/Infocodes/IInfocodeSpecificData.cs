using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Infocodes
{
    public interface IInfocodeSpecificData : IDataProvider<InfocodeSpecific>
    {
        /// <summary>
        /// Gets all infocodeSpecific records for a specific usage category ordered by a specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="refRelationID">The ID of the RefRelation to get fo(RefRelation,RefRelation2,RefRelation3)</param>
        /// <param name="usageCategory">The usage category to filter by</param>
        /// <param name="refTable">The table that we are referencing. This determines what our infocode is attached to</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all infocodeSpecific for a specific usage category</returns>
        List<InfocodeSpecific> GetListForRefRelation(
            IConnectionManager entry, 
            RecordIdentifier refRelationID, 
            UsageCategoriesEnum usageCategory, 
            RefTableEnum refTable,
            InfocodeSpecificSorting sortBy,
            bool sortBackwards);

        InfocodeSpecific Get(IConnectionManager entry, RecordIdentifier infocodeSpecificID);
    }
}