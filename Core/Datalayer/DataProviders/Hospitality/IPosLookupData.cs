using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IPosLookupData : IDataProvider<PosLookup>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all pos lookups
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all pos lookup recrods</returns>
        List<PosLookup> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all pos lookups ordered by the specific field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of PosLookup objects conataining all pos lookup records, ordered by the chosen field</returns>
        List<PosLookup> GetList(IConnectionManager entry, PosLookupSorting sortBy, bool sortBackwards);

        PosLookup Get(IConnectionManager entry, RecordIdentifier posLookupID);
    }
}