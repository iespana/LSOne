using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IDataDirectorTransactionJobData : IDataProvider<DataDirectorTransactionJob>
    {
        List<DataDirectorTransactionJob> GetPendingJobs(IConnectionManager entry);
    }
}
