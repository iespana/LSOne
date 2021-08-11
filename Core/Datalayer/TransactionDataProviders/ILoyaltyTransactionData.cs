using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ILoyaltyTransactionData : IDataProviderBase<LoyaltyItem>
    {
        /// <summary>
        /// Inserts a points line for a sale line loyalty points 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        /// <param name="saleLineItem">The item currently being saved</param>
        void Insert(IConnectionManager entry, PosTransaction transaction, ISaleLineItem saleLineItem);

        /// <summary>
        /// Inserts a points line for a tender line loyalty points 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        /// <param name="tenderLineItem">The tender line currently being saved</param>
        void Insert(IConnectionManager entry, PosTransaction transaction, ITenderLineItem tenderLineItem);

        /// <summary>
        /// Inserts a summary points line for the entire transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction currently being saved</param>
        void Insert(IConnectionManager entry, PosTransaction transaction);

        LoyaltyItem GetTransactionLoyaltyItem(IConnectionManager entry,RecordIdentifier transactionID, RetailTransaction transaction);
        List<LoyaltyItem> GetList(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId);
        LoyaltyItem Get(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeId, RecordIdentifier terminalId, decimal lineNum,  LoyaltyPointsRelation relation);
    }
}