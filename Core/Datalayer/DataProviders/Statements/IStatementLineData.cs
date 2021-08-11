using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Statements
{
    public interface IStatementLineData : IDataProviderBase<StatementLine>, ISequenceable
    {
        void DeleteLines(IConnectionManager entry, RecordIdentifier statementInfoID);
        StatementLine Get(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier lineNumber);
        List<StatementLine> GetStatementLines(IConnectionManager entry, RecordIdentifier statementID);
        void Save(IConnectionManager entry, StatementLine statementLine);
    }
}