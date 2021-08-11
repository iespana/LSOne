using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class LogTransactionData : SqlServerDataProviderBase, ILogTransactionData
    {
        public virtual void Insert(IConnectionManager entry, LogLineItem logLine, ILogTransaction transaction)
        {
            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONLOGTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)logLine.LineId, SqlDbType.Decimal);
            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("LOGTEXT", logLine.LogText);

            statement.AddField("REPLICATED", (byte)0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
