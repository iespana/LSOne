using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IStorePaymentLimitationData : IDataProviderBase<StorePaymentLimitation>
    {
        /// <summary>
        /// Returns a list of payment limitations for a specific store and tender type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID </param>
        /// <param name="tenderTypeID">The tender type ID</param>
        /// <param name="cacheType">Optional parameter to specify what type of cache to use</param>
        /// <returns></returns>
        List<StorePaymentLimitation> GetListForStoreTender(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier tenderTypeID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// REturns a list of all payment limitations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<StorePaymentLimitation> GetList(IConnectionManager entry);

        /// <summary>
        /// Saves payment limitations selected for a store and tender type. All the columns are a part of the primary key so there can only be an insert into this table
        /// If the record already exists nothing is saved.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="paymentLimitation">The payment limitation for tender type and store that is to be saved</param>
        void Save(IConnectionManager entry, StorePaymentLimitation paymentLimitation);

        /// <summary>
        /// Deletes a specific limitation attached to a store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID </param>
        /// <param name="tenderTypeID">The tender type ID</param>
        /// <param name="limitationMasterID"></param>
        void Delete(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier tenderTypeID, RecordIdentifier limitationMasterID);
    }
}
