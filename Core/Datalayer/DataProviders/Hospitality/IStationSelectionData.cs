using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IStationSelectionData : IDataProvider<StationSelection>
    {
        /// <summary>
        /// Gets a list for a given station
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationID">Station id</param>
        /// <returns>A list of StationSelection objects containing given station</returns>
        List<StationSelection> GetListForStation(IConnectionManager entry, RecordIdentifier stationID);

        /// <summary>
        /// Gets all station selections
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of StationSelection objects conaining all station selections</returns>
        List<StationSelection> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all station selections for the given hospitality type (restaurantID, salesTypeID)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The restaurant ID of the hospitality type</param>
        /// <param name="salesTypeID">The sales type ID of the hospitality type</param>
        /// <returns></returns>
        List<StationSelection> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID);

        StationSelection Get(IConnectionManager entry, RecordIdentifier stationSelectionID);
    }
}