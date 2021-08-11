using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Terminals
{
    public interface ITerminalGroupData : IDataProvider<TerminalGroup>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<TerminalGroup> GetListForTerminalGroup(IConnectionManager entry, bool sortAscending, TerminalGroup.SortEnum sortEnum);

        TerminalGroup Get(IConnectionManager entry, RecordIdentifier groupId);
    }
}