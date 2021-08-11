using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IUserProfileData : IDataProvider<UserProfile>, ISequenceable
    {
        UserProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
        List<UserProfile> GetList(IConnectionManager entry);
        List<UserProfile> GetListAdvanced(IConnectionManager entry, UserProfileFilter filter);
    }
}
