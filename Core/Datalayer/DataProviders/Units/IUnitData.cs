using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Units
{
    public interface IUnitData : IDataProviderBase<Unit>, ICompareListGetter<Unit>, ISequenceable
    {
        List<Unit> GetAllUnits(IConnectionManager entry);

        /// <summary>
        /// Gets all units, sorted by a specified column
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of units, meeting the above criteria</returns>
        List<Unit> GetUnits(IConnectionManager entry, UnitSorting sortEnum, bool backwardsSort);

        /// <summary>
        /// Gets the inventory unit for a specific item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the unit. Insert RecordIdentifier.Empty to get all units in the system</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of units, meeting the above criteria</returns>
        List<Unit> GetUnitForItem(IConnectionManager entry, RecordIdentifier itemID, UnitSorting sortEnum, bool backwardsSort, UnitTypeEnum unitType);

        /// <summary>
        /// Gets all units that a specific item is convertable to or from
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the unit. Insert RecordIdentifier.Empty to get all units in the system</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of units, meeting the above criteria</returns>
        List<Unit> GetUnitsForItem(IConnectionManager entry, RecordIdentifier itemID, UnitSorting sortEnum, bool backwardsSort);

        /// <summary>
        /// Gets a list of data entities containing ids and descriptions of units 
        /// The data entities are ordered by the units description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing ids and descriptions of units</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets the decimal limiter for a given unit ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unitID">The ID of the unit</param>
        /// <param name="cacheType">Type of cache</param>
        /// <returns>The decimal limiter for the given unit or a default limiter if the unit is not found</returns>
        DecimalLimit GetNumberLimitForUnit(IConnectionManager entry, RecordIdentifier unitID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns true if the quantity is allowed for the specific unit.
        /// E.g. it is illegal to sell 4,6 bottles of coke but allright for metric goods.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="qty">Quantity that we are checking if is ok</param>
        /// <param name="unitID">ID of the unit object we are checking against</param>
        /// <returns>If the given quantity is allowed for the given unit</returns>
        bool IsQuantityAllowed(IConnectionManager entry, decimal qty, RecordIdentifier unitID);

        string GetUnitDescription(IConnectionManager entry, RecordIdentifier unitId);

        /// <summary>
        /// Deletes a unit with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitID">The ID of the unit to delete</param>
        bool Delete(IConnectionManager entry, RecordIdentifier unitID);

        /// <summary>
        /// Returns true if a unit with the given description exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="description">The unit description to look for</param>
        /// <returns></returns>
        bool UnitDescriptionExists(IConnectionManager entry, string description);

        /// <summary>
        /// Returns the unit ID given the description
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="description">The unit description to look for</param>
        /// <returns></returns>
        RecordIdentifier GetIdFromDescription(IConnectionManager entry, string description);

        Unit Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);
        bool Exists(IConnectionManager entry, RecordIdentifier unitID);
        void Save(IConnectionManager entry, Unit unit);
    }
}