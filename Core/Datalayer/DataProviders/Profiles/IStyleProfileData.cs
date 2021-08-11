using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IStyleProfileData : IDataProviderBase<StyleProfile>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all style profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all style profiles</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all style profiles sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <returns>A list of all style profiles sorted</returns>
        List<DataEntity> GetList(IConnectionManager entry, string sort);

        /// <summary>
        /// Gets the styleprofile with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="profile">profile which list gets saved under</param>
        /// /// <param name="styleLineList">List of PosStyleProfileLines to save</param>
        void Save(IConnectionManager entry, StyleProfile profile, List<PosStyleProfileLine> styleLineList = null);

        StyleProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
        void Delete(IConnectionManager entry, RecordIdentifier id);
    }
}