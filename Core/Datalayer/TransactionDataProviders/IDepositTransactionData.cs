using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.TransactionObjects;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IDepositTransactionData : IDataProviderBase<DepositTransaction>
    {
    }
}
