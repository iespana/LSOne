using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Units
{
    public interface IUnitConversionData : IDataProvider<UnitConversion>, ICompareListGetter<UnitConversion>
    {
        /// <summary>
        /// Gets all general unit conversion objects. General means they are not tied to an item. 
        /// The objects are ordered by the FromUnit field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortColumn">The index (measured by the UnitConverionView columns) to sort by</param>
        /// <param name="backwardsSort">Whether to sort the result backwards or not</param>
        /// <returns>A list of unit conversion objects meeting the above criteria</returns>
        List<UnitConversion> GetUnitConversions(IConnectionManager entry, int sortColumn, bool backwardsSort);

        /// <summary>
        /// Gets all the unit conversion objects that convert an item with a given ID. 
        /// The objects are ordered by the FromUnit field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the item to get unit conversion objects for</param>
        /// <param name="sortColumn">The index (measured by the UnitConverionView columns) to sort by</param>
        /// <param name="backwardsSort">Whether to sort the result backwards or not</param>
        /// <returns>All the unit conversion objects that convert an item with a given ID</returns>
        List<UnitConversion> GetUnitConversions(IConnectionManager entry, RecordIdentifier itemID, int sortColumn, bool backwardsSort);

        /// <summary>
        /// Gets the coversion rules that apply to an item, this includes the global rules. 
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <param name="itemID">ID of the item that the rules apply to.</param>
        /// <param name="unitID">The unit that is currently being used to measure the quatitiy of the item.</param>
        /// <returns></returns>
        List<UnitConversion> GetAllConversionsForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID);

        List<UnitConversion> GetConversionsForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID);

        /// <summary>
        /// Deletes all unit conversion rules from database by performing a TRUNCATE on UNITCONVERT table.
        /// </summary>
        /// <param name="entry"></param>
        void DeleteAll(IConnectionManager entry);
        
        /// <summary>
        /// Whether a unit conversion rule exists between two units for a specific item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item to check for. Passing an empty string here checks whether a general conversion
        /// rule exists between the two units</param>
        /// <param name="fromUnitID">The ID of the unit to convert from</param>
        /// <param name="toUnitID">The ID of the unit to convert to</param>
        /// <returns>Whether a unit conversion rule exists between two units for a specific item.</returns>
        bool UnitConversionRuleExists(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier fromUnitID, 
            RecordIdentifier toUnitID);

        /// <summary>
        /// Gets a list of DataEntities containing unitID and unit descriptions for units that are convertable to a unit with a 
        /// given unit ID and for a given retail item ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item who's unit conversions we are dealing with</param>
        /// <param name="unitID">The unit id of the unit that the returned unit data entities are convertable to</param>
        /// <returns></returns>
        List<Unit> GetConvertableTo(IConnectionManager entry, RecordIdentifier itemID,
            RecordIdentifier unitID);

        /// <summary>
        /// Returns the factor for a unit. It is able to fallback to another rule if it exists in the other direction.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitFrom">The unit that is to be changed</param>
        /// <param name="unitTo">The unit that is to be changed to</param>
        /// <param name="itemID">The units that this factor applies to</param>
        /// <returns></returns>
        decimal GetUnitOfMeasureConversionFactor(IConnectionManager entry, RecordIdentifier unitFrom, RecordIdentifier unitTo, 
            RecordIdentifier itemID);

        /// <summary>
        /// Used when creating conversion rules so you cannot for example create a unit conversion rule to 
        /// prevent that you can make rule that says 1 cm = 3 cm. 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the item</param>
        /// <param name="unitID">The unit to convert to</param>
        /// <returns></returns>
        List<DataEntity> GetConvertableToWithoutCurrentUnit(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID);

        /// <summary>
        /// Converts some quantity of an item from one unit to another using unit conversion rules. If a unit conversion 
        /// rule does not exist,
        /// this function returns 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The items who's quantity we are changing</param>
        /// <param name="fromUnitID">The unit ID to convert from</param>
        /// <param name="toUnitID">The unit ID to convert to</param>
        /// <param name="originalQty">The original quantity of the item</param>
        /// <returns>The new quantity or 0 if a unit conversion rule does not exist</returns>
        decimal ConvertQtyBetweenUnits(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier fromUnitID,
            RecordIdentifier toUnitID,
            decimal originalQty);

        /// <summary>
        /// Creates an inverted rule for a given rule.
        /// </summary>
        /// <param name="rule">Orginal rule.</param>
        /// <returns></returns>
        UnitConversion ReverseRule(UnitConversion rule);

        UnitConversion Get(IConnectionManager entry, RecordIdentifier ID);
    }
}