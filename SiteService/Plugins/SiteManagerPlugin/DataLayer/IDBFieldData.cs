using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.SiteService.Plugins.SiteManager.DataLayer.DataEntities;

namespace LSOne.SiteService.Plugins.SiteManager.DataLayer
{
    internal interface IDBFieldData : IDataProviderBase<DBField>
    {
        List<DBField> GetAllFieldsForTable(IConnectionManager entry, string tableName);
        List<DBField> GetPrimaryFieldsForTable(IConnectionManager entry, string tableName);
        void Insert(IConnectionManager entry, string tableName, List<DBField> fields);
        bool Exists(IConnectionManager entry, string tableName, List<DBField> fields);
    }
}