using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IFormProfileLineData : IDataProviderBase<FormProfileLine>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all form profile names
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all form profiles</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all form profile Lines sorted
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="sort">How to sort list</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form profile lines sorted</returns>
        List<DataEntity> GetList(IConnectionManager entry, FormProfileLineSorting sort, bool sortBackwards);

        List<FormProfileLine> GetLines(IConnectionManager entry, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of formProfileLines with the given profileID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the profile to return the lines for</param>
        /// <param name="sortBackwards"></param>
        /// <param name="cache">CacheType</param>
        /// <param name="sort">Sort string</param>
        /// <returns>A list of formProfileLines with the given ProfileID</returns>
        List<FormProfileLine> GetProfileLines(IConnectionManager entry, RecordIdentifier id, FormProfileLineSorting sort,
            bool sortBackwards, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of all user defined profile lines from a specific profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileId">The id of the profile/param> </param>
        /// <param name="cache">CacheType</param>
        /// <returns></returns>
        List<FormProfileLine> GetUserDefinedFormProfileLines(IConnectionManager entry, RecordIdentifier profileId, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of formProfileLines with the given typeID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the type to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of formProfileLines with the given TypeID</returns>
        List<FormProfileLine> GetLinesByTypeId(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of FormProfileLine with the given formLayoutID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the form layout to return the lines for</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A list of formProfileLines with the given FormLayoutID</returns>
        List<FormProfileLine> GetLinesByFormLayoutId(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a FormProfileLine with the given primaryID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileId">The id of the profile/param> </param>
        /// <param name="formTypeId">The id of the form type</param>
        /// <param name="cache">CacheType</param>
        /// <returns>A FormProfileLine with the given PrimaryID</returns>
        FormProfileLine GetFormProfileLine(IConnectionManager entry, RecordIdentifier profileId, RecordIdentifier formTypeId, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Deletes a pos FormProfileLine with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profileID">The ID of the profile</param>
        /// <param name="formTypeID">The ID of the form type</param>
        void DeleteProfileLine(IConnectionManager entry, RecordIdentifier profileID, RecordIdentifier formTypeID);

        bool IDExists(IConnectionManager entry, RecordIdentifier profileID, RecordIdentifier formTypeID);

        void Save(IConnectionManager entry, FormProfileLine item);

        List<FormProfileLine> Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
    }
}