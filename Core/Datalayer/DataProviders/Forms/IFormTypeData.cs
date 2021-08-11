using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Forms
{
    public interface IFormTypeData : IDataProvider<FormType>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all form types
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">Sorting column</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form types</returns>
        List<DataEntity> GetList(IConnectionManager entry, FormTypeSorting sort, bool sortBackwards);

        List<FormType> GetFormTypes(IConnectionManager entry, FormTypeSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets the formType with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="id">id of FormType to get</param>
        /// /// <param name="cache">Cachetype</param>
        /// <returns>A formType </returns>
        FormType Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets a list of form types that are not used by a profile
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="profileId">Profile id to filter form types</param>
        /// <returns>List of form types not used by the profile</returns>
        List<FormType> GetUnusedFormTypes(IConnectionManager entry, RecordIdentifier profileId);
    }
}