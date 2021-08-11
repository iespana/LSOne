using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionDataProviders
{
    /// <summary>
    /// Saves data to the RBOTRANSACTIONLOGTRANS table
    /// </summary>
    public interface ILogTransactionData : IDataProviderBase<LogTransaction>
    {
        /// <summary>
        /// Inserts a line into the RBOTRANSACTIONLOGTRANS table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="logLine">The log line to insert into the table</param>
        /// <param name="transaction">The <see cref="ILogTransaction"/> instance this line belongs to</param>
        void Insert(IConnectionManager entry, LogLineItem logLine, ILogTransaction transaction);
    }
}
