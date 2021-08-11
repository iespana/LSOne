using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class DepositTransactionData : SqlServerDataProviderBase, IDepositTransactionData
    {
    }
}
