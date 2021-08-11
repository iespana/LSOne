using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.Administration.DataLayer
{
    internal interface IDBFieldData : IDataProviderBase<DBField>
    {
        List<DBField> GetAllFieldsForTable(IConnectionManager entry, string tableName);
        List<DBField> GetPrimaryFieldsForTable(IConnectionManager entry, string tableName);
        void Insert(IConnectionManager entry, string tableName, List<DBField> fields);
        bool Exists(IConnectionManager entry, string tableName, List<DBField> fields);

        void ClearTable(IConnectionManager entry, string tableName);
    }
}