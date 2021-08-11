using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IPermissionGroupData : IDataProviderBase<DataEntity>
    {
        DataEntity GetByPermission(IConnectionManager entry, RecordIdentifier permissionGuid);
    }
}