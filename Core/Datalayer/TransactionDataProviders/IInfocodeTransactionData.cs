using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IInfocodeTransactionData : IDataProviderBase<InfoCodeLineItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">LineID, TransactionID, Type, TerminalID, StoreID</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        List<InfoCodeLineItem> Get(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction);

        void Insert(IConnectionManager entry, InfoCodeLineItem infocodeItem, PosTransaction transaction, int saleLineId);
    }
}