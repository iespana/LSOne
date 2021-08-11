using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IStationPrintingHostData : IDataProvider<StationPrintingHost>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all Printing Station hosts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of StationPrintingHost objects containing all Printing Station host records</returns>
        List<StationPrintingHost> GetList(IConnectionManager entry);

        StationPrintingHost Get(IConnectionManager entry, RecordIdentifier stprintHostID);
    }
}