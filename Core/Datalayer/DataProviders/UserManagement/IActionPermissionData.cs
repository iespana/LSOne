using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IActionPermissionData : IDataProviderBase<ActionPermission>
    {
        List<DataEntity> GetList(IConnectionManager entry, string sort);
    }
}