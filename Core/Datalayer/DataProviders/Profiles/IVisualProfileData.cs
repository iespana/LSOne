using System.Collections.Generic;
using System.Security;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IVisualProfileData : IDataProvider<VisualProfile>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<DataEntity> GetList(IConnectionManager entry, string sort);
        List<VisualProfile> GetVisualProfileList(IConnectionManager entry, string sort);
        VisualProfile GetTerminalProfile(IConnectionManager entry, RecordIdentifier id, RecordIdentifier storeId, CacheType cache = CacheType.CacheTypeNone);
        VisualProfile GetVisualProfileByTerminalUnsecure(IConnectionManager entry,
                                                                  string dataSource,
                                                                  bool windowsAuthentication,
                                                                  string sqlServerLogin,
                                                                  SecureString sqlServerPassword,
                                                                  string databaseName,
                                                                  ConnectionType connectionType,
                                                                  string dataAreaID,
                                                                  RecordIdentifier storeID,
                                                                  RecordIdentifier terminalID);
        VisualProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);
    }
}