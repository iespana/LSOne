using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IPrintingStationData : IDataProvider<PrintingStation>, ISequenceable
    {
        /// <summary>
        /// Gets all PrintingStation stations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PrintingStation objects containing all printing station</returns>
        List<PrintingStation> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all printing stations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all printing stations, ordered by a chosen field</returns>
        List<PrintingStation> GetList(IConnectionManager entry, PrintingStationSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a data entity version of the printing station with the give ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Id of the printing station to get data entity for</param>
        DataEntity GetDataEntity(IConnectionManager entry, RecordIdentifier id);

        PrintingStation Get(IConnectionManager entry, RecordIdentifier id);
    }
}