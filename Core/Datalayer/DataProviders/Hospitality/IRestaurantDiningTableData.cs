using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IRestaurantDiningTableData : IDataProvider<RestaurantDiningTable>
    {
        /// <summary>
        /// Returns all restaurant dining tables
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RestaurantDiningTable objects containig all restaurant dining tables</returns>
        List<RestaurantDiningTable> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all restaurant dining table for a given hospitality type and dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant of the hospitality type</param>
        /// <param name="sequence">The sequence of the hospitality type</param>
        /// <param name="salesType">The sales type of the hospitality type</param>
        /// <param name="diningTableLayoutID">The dining table layout id</param>
        /// <param name="cacheType"></param>
        /// <returns>All restaurant dining tables for the given id's</returns>
        List<RestaurantDiningTable> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType, RecordIdentifier diningTableLayoutID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns the lowest table number for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The lowest restaurant dining table number for the given layout ID</returns>
        int GetStartingTableNumber(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Returns the highest restaurant dining table number for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The highest restaurant dining table number for the given layout ID</returns>
        int GetEndingTableNumber(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Gets the current number of restaurant dining tables assigned to a given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The current number fo restaurant dining tables for the given dining table layout</returns>
        int GetNumberOfTables(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Checks wether any restaurant dining tables exists within the given range for a specific dining table layout.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The id of the dining table layout (restaurantId, sequence, salestype, diningTableLayoutId)</param>
        /// <param name="rangeFrom">The start of the range</param>
        /// <param name="rangeTo">The end of the range</param>
        /// <returns>True if any records exist within the given range, false otherwise</returns>
        bool DineInTableRangeExists(IConnectionManager entry, RecordIdentifier diningTableLayoutID, int rangeFrom, int rangeTo);

        /// <summary>
        /// Deletes all restaurant dining tables for a given dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        void DeleteForDiningTableLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Deletes all restaurant dining tables belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType);

        RestaurantDiningTable Get(IConnectionManager entry, RecordIdentifier restaurantDiningTableID);
    }
}