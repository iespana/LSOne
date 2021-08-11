using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Terminals
{
    public interface ITerminalGroupConnectionData : IDataProviderBase<TerminalGroupConnection>
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<DataEntity> GetTerminalsForDropDown(IConnectionManager entry, RecordIdentifier groupId);
        List<TerminalGroupConnection> GetTerminalsList(IConnectionManager entry, RecordIdentifier groupId, 
            bool sortAscending = true , TerminalGroupConnection.SortEnum sortEnum = TerminalGroupConnection.SortEnum.ID);
        void DeleteGroup(IConnectionManager entry, RecordIdentifier terminalId);

        void Delete(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier groupId, RecordIdentifier storeId);
        void Save(IConnectionManager entry, TerminalGroupConnection terminalGroup);
    }
}