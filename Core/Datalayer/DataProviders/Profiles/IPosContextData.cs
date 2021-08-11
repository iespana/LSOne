using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IPosContextData : IDataProvider<PosContext>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all Contexts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all Contexts</returns>
        List<PosContext> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all Contexts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">How to sort list</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>A list of all Contexts sorted in a particular order</returns>
        List<PosContext> GetList(IConnectionManager entry, string sort, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a list of all Contexts associated with id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Get contexts associated with this id. If empty then all contexts are returned</param>
        /// <param name="sort">How to sort the list</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>A list of all Contexts associated with a particular id</returns>
        List<PosContext> GetList(IConnectionManager entry, RecordIdentifier id, string sort = "", 
            CacheType cache = CacheType.CacheTypeNone);

        PosContext Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
    }
}