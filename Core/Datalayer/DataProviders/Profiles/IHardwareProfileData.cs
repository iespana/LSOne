using System.Collections.Generic;
using System.Security;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface IHardwareProfileData : IDataProvider<HardwareProfile>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        string GetName(IConnectionManager entry, RecordIdentifier ID);
        List<DataEntity> GetList(IConnectionManager entry, string sort);
        List<HardwareProfile> GetHardwareProfileList(IConnectionManager entry, string sort);
        List<HardwareProfile> GetFullProfileList(IConnectionManager entry);

        HardwareProfile GetTempProfileForTokenLogin(IConnectionManager entry,
            string dataSource,
            bool windowsAuthentication,
            string sqlServerLogin,
            SecureString sqlServerPassword,
            string databaseName,
            ConnectionType connectionType,
            string dataAreaID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID);

        HardwareProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);
    }
}