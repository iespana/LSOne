using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement.ListItems;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.UserManagement
{
    public interface IPermissionData : IDataProviderBase<PermissionsAssignmentResult>
    {
        List<PermissionsAssignmentResult> GetPermissionsForGroup(IConnectionManager entry, Guid groupGuid, string searchText);
        List<PermissionsAssignmentResult> GetPermissionsForUser(IConnectionManager entry, Guid userGuid, string searchText);
    }
}