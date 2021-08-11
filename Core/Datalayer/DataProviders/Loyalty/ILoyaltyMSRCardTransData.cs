using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    public interface ILoyaltyMSRCardTransData : IDataProviderBase<LoyaltyMSRCardTrans>
    {
        int GetListCount(IConnectionManager entry, RecordIdentifier cardNumber);
        int NumberOfCustomerTransactionsForCard(IConnectionManager entry, RecordIdentifier cardNumber);

        /// <summary>
        /// Gets the list of transactions.
        /// </summary>
        /// <param name="entry">The entry into the database</param> 
        /// <param name="storeFilter">Filter by store. Note that if this is null or empty then filter is disabled</param>
        /// <param name="terminalFilter">Filter by terminal. Note that if this is null or empty then filter is disabled</param>
        /// <param name="msrCardFilter">Fulter by MSRCard. Note that if this is null or empty then filter is disabled</param>
        /// <param name="schemeFilter">Filter by scheme. Note that if this is null or empty then filter is disabled</param>
        /// <param name="typeFilter">Filter by type. Note that if this is less than zero then filter is disabled</param>
        /// <param name="openFilter">Filter by status. Note that if this is less than zero then filter is disabled</param>
        /// <param name="entryTypeFilter">Filter by entry type. Note that if this is less than zero then filter is disabled</param>
        /// <param name="customerFilter">Filter by customer. Note that if this is null or empty then filter is disabled</param>
        /// <param name="receiptID">Filter by receiptID. Note that if this is null or empty then filter is disabled</param>
        /// <param name="dateFrom">Filter by date. Note that if this is empty then filter is disabled</param>
        /// <param name="dateTo">Filter by date. Note that if this is empty then filter is disabled</param>
        /// <param name="expiredateFrom">Filter by expire date. Note that if this is empty then filter is disabled</param>
        /// <param name="expiredateTo">Filter by expire date. Note that if this is empty then filter is disabled</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="backwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of instances of <see cref="LoyaltyMSRCardTrans"/></returns>
        List<LoyaltyMSRCardTrans> GetList(IConnectionManager entry,
            string storeFilter,
            string terminalFilter,
            string msrCardFilter,
            string schemeFilter,
            int typeFilter,
            int openFilter,
            int entryTypeFilter,
            string customerFilter,
            string receiptID,
            Date dateFrom,
            Date dateTo,
            Date expiredateFrom,
            Date expiredateTo,
            int rowFrom, int rowTo, bool backwards = false);

        List<LoyaltyMSRCardTrans> GetListForCard(IConnectionManager entry, RecordIdentifier cardId);
        decimal GetMaxLineNumber(IConnectionManager entry, RecordIdentifier cardNum);

        LoyaltyCustomer.ErrorCodes UpdateRemainingPoints(IConnectionManager entry, RecordIdentifier CardID, RecordIdentifier LoyaltyCustID, decimal usedPoints);
        LoyaltyCustomer.ErrorCodes? SetExpirePoints(IConnectionManager entry, RecordIdentifier CardID, RecordIdentifier loyaltyCustID, DateTime ToDate, RecordIdentifier UserId = null);
        LoyaltyMSRCard.TenderTypeEnum? GetLoyaltyCardType(IConnectionManager entry, RecordIdentifier cardNumber);
        void GetLoyaltyPointsStatus(IConnectionManager entry, RecordIdentifier customerId, LoyaltyPointStatus pointStatus);
        void UpdateIssuedLoyaltyPointsForCustomer(IConnectionManager entry, RecordIdentifier loyalityCardId, RecordIdentifier customerId);

        LoyaltyMSRCardTrans Get(IConnectionManager entry, RecordIdentifier loyMsrCardTransID);
        void Save(IConnectionManager entry, LoyaltyMSRCardTrans item);
    }
}