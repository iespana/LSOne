using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IFunctionalityProfileData : IDataProvider<FunctionalityProfile>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<DataEntity> GetList(IConnectionManager entry, string sort);
        List<FunctionalityProfile> GetFunctionalityProfileList(IConnectionManager entry, string sort);
        FunctionalityProfile Get(IConnectionManager entry, RecordIdentifier id,
            CacheType cache = CacheType.CacheTypeNone);
        void SetDisplayItemIDInReturnDialog(IConnectionManager entry, FunctionalityProfile profile, bool DisplayItemIDInReturnDialog);
    }
}