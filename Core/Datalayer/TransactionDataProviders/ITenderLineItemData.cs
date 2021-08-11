using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface ITenderLineItemData : IDataProviderBase<TenderLineItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">TransactionID, TerminalID, StoreID</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        List<TenderLineItem> Get(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction);

        List<TenderLineItem> HMGet(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction);

    }
}