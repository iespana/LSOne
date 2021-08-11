using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ISalesTransactionData : IDataProviderBase<SalesTransaction>
    {
        List<SalesTransaction> GetRetailTransactionItems(IConnectionManager entry, RecordIdentifier transactionId, 
            RecordIdentifier storeId, RecordIdentifier terminalId);

        List<SalesTransaction> GetList(IConnectionManager entry, RecordIdentifier itemID,
            int rowFrom, int rowTo,
            string storeID, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all sales transaction lines for a customer where the customer got a customer discount.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerId">ID of the customer</param>
        /// <param name="startDate">Starting point of the period to fetch</param>
        /// <param name="endDate">Ending point of the period to fetch</param>
        List<SalesTransaction> GetDiscountedItemsForCustomer(
            IConnectionManager entry,
            string customerId,
            DateTime startDate,
            DateTime endDate);
    }
}