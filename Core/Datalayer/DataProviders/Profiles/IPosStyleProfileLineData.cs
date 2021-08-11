using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IPosStyleProfileLineData : IDataProvider<PosStyleProfileLine>, ISequenceable
    {
        /// <summary>
        /// Returns a list of <see cref="PosStyleProfileLine"/> with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <param name="sort">Sorting string</param>
        /// <returns>A list of <see cref="PosStyleProfileLine"/> for the specific ID</returns>
        List<PosStyleProfileLine> GetProfileLines(IConnectionManager entry, RecordIdentifier id, string sort, 
            CacheType cache = CacheType.CacheTypeNone);

        bool ProfileLineExists(IConnectionManager entry, PosStyleProfileLine posStyleProfileLine, RecordIdentifier profileID, 
            CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a <see cref="PosStyleProfileLine"/> with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileLineID">The id of the profile line to check for</param>
        /// <param name="profileID">The id of the profile of the line</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A <see cref="PosStyleProfileLine"/></returns>
        PosStyleProfileLine GetProfileLine(IConnectionManager entry, RecordIdentifier profileLineID, RecordIdentifier profileID, 
            CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Deletes all profile lines for a a specific style profile
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="profileID">The profile ID to delete</param>
        void DeleteLines(IConnectionManager entry, RecordIdentifier profileID);

        List<PosStyleProfileLine> Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
    }
}