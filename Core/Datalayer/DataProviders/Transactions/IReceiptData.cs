using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface IReceiptData : IDataProviderBase<ReceiptListItem>
    {
        List<ReceiptListItem> Find(
            IConnectionManager entry,
            Date dateFrom, Date dateTo,
            string idOrReceiptNumber,
            bool idOrReceiptNumberBeginsWith,
            RecordIdentifier employeeID,
            RecordIdentifier storeID,
            RecordIdentifier terminalID,
            RecordIdentifier currency,
            decimal paidAmount,
            int recordFrom,
            int recordTo,
            string sort,
            out int totalReceiptsMatching);
    }
}