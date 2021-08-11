using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IReprintTransactionData : IDataProviderBase<ReprintInfo>
    {
        void Insert(IConnectionManager entry, ReprintInfo reprintInfo, IPosTransaction transaction);

        List<ReprintInfo> GetReprintInfo(IConnectionManager entry, RecordIdentifier transactionID, IRetailTransaction transaction);
    }
}
