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
    public interface IReceiptTransactionData : IDataProviderBase<ReceiptInfo>
    {
        void Insert(IConnectionManager entry, ReceiptInfo receiptInfo, IPosTransaction transaction);

        List<ReceiptInfo> GetReceiptInfo(IConnectionManager entry, RecordIdentifier transactionID, IPosTransaction transaction);
    }
}
