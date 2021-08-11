using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders.EFT
{
    public interface IEFTInfoData : IDataProvider<EFTInfo>
    {
        /// <summary>
        /// Get all single eft transaction lines for a transaction
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="id">Transaction id + terminal id + store id</param>
        /// <param name="cache"></param>
        /// <returns></returns>
        List<EFTInfo> GetAll(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone);

        EFTInfo Get(IConnectionManager entry, RecordIdentifier ID, CacheType cache = CacheType.CacheTypeNone);
    }
}