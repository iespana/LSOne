using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Ledger
{
    public interface ICustomerLedgerEntriesData : IDataProvider<CustomerLedgerEntries>, ISequenceable
    {
        List<CustomerLedgerEntries> GetList(IConnectionManager entry, RecordIdentifier customerId, out int totalRecords, CustomerLedgerFilter filter);

        int GetMaxEntryNo(IConnectionManager entry);
        bool UpdateRemainingAmount(IConnectionManager entry, RecordIdentifier CustID);

        /// <summary>
        /// Gets customer total sales
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Total sales</returns>
        decimal GetCustomerTotalSales(IConnectionManager entry, RecordIdentifier customerId);

        /// <summary>
        /// Gets customer balance from local database
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="customerId">ID of the customer</param>
        /// <returns>Customer balance</returns>
        decimal GetCustomerBalance(IConnectionManager entry, RecordIdentifier customerId);

        int RecreateCustomerLedger(IConnectionManager entry, RecordIdentifier customerID, RecordIdentifier statementID);
    }
}