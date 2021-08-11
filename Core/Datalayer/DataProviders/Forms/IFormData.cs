using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Forms
{
    public interface IFormData : IDataProvider<Form>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry, string sort);
        List<DataEntity> GetList(IConnectionManager entry, FormSystemType systemType, string sort);
        Form GetProfileForm(IConnectionManager entry, RecordIdentifier profileID, FormSystemType systemType, CacheType cacheType = CacheType.CacheTypeNone);
        List<Form> GetProfileForms(IConnectionManager entry, RecordIdentifier profileID, FormSorting sortBy, bool sortBackwards);
        List<Form> GetLists(IConnectionManager entry, FormSorting sortBy, bool sortBackwards);
        List<Form> GetFormsOfType(IConnectionManager entry, RecordIdentifier formTypeID, FormSorting sortBy, bool sortBackwards);
        List<Form> SearchForms(IConnectionManager entry, string description, bool descriptionBeginsWith, bool? isSystemForm, FormSorting sortBy, bool sortBackwards);
        Form Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
        List<Form> GetSystemProfileForms(IConnectionManager entry);
    }
}