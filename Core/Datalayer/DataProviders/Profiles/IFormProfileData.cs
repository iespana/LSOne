using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IFormProfileData : IDataProviderBase<FormProfile>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all form profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all form profiles</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all form profiles sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form profiles sorted</returns>
        List<FormProfile> GetList(IConnectionManager entry, FormProfileSorting sort, bool sortBackwards);

        /// <summary>
        /// Gets the styleprofile with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="id">id of styleprofile to get</param>
        /// /// <param name="cache">Cachetype</param>
        /// <returns>A Styleprofile </returns>
        FormProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Saves the formprofile
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="profile">profile which list gets saved under</param>
        /// /// <param name="profileLineList">List of FormProfileLines to save</param>
        void Save(IConnectionManager entry, FormProfile profile, List<FormProfileLine> profileLineList = null);

        void Delete(IConnectionManager entry, RecordIdentifier id);
    }
}