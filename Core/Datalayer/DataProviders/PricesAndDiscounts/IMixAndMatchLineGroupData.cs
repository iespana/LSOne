using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.PricesAndDiscounts
{
    public interface IMixAndMatchLineGroupData : IDataProvider<MixAndMatchLineGroup>, ISequenceable
    {
        /// <summary>
        /// Gets a list of mix of match group lines for a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>A list of mix and match line groups</returns>
        List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier offerID);

        /// <summary>
        /// Gets all mix and match groups for a a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="sortColumn">The number of the column to sort by. 0 = LINEGROUP, 1 = NOOFITEMSNEEDED</param>
        /// <param name="backwardsSort">True if the sort should be backwards</param>
        /// <returns>List of all mix and match groups found for a given offer.</returns>
        List<MixAndMatchLineGroup> GetGroups(IConnectionManager entry, RecordIdentifier offerID, int sortColumn, bool backwardsSort);

        /// <summary>
        /// Checks if a mix and match group may be deleted. This will return false if there is some line using the group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGroupID">ID of the line group, note this is double key, OFFERID, LINEGROUP</param>
        /// <returns>True if it is safe to delete, else false</returns>
        bool CanDelete(IConnectionManager entry, RecordIdentifier lineGroupID);

        MixAndMatchLineGroup Get(IConnectionManager entry, RecordIdentifier lineGroupID);
    }
}