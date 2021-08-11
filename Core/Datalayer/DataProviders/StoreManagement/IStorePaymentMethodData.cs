using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IStorePaymentMethodData : IDataProviderBase<StorePaymentMethod>
    {
        List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Gets the list of valid return payment types. This excludes Pay customer account(202), Pay gift card(214), and Pay currency(203).
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store that the payments types are assigned to</param>
        /// <returns>Valid return payments</returns>
        List<DataEntity> GetReturnPaymentList(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Creates a store payment method for a given store with the information from the supplied payment method. If a store payment method already exists 
        /// for the given store it will be overwritten with the information from <paramref name="paymentMethod"/>  
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="paymentMethod">The payment method that will be copied to the store</param>
        /// <param name="storeID">The ID of the store to create the payment method for</param>
        void CopyPaymentMethodToStore(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier storeID);

        /// <summary>
        /// Gets unused tender types for a store.
        /// If the RecordIdentifier includes a TenderTypeID as secondary identifier then the result will include
        /// that ID.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="storeAndPaymentTypeID"></param>
        /// <returns></returns>
        List<DataEntity> GetUnusedList(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID);

        /// <summary>
        /// Gets a specific payment type for a given store or all payment types for a given store.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeAndPaymentTypeID">Has to contain store, buy payment type is optional, if payment type is empty then all payment methods for the given store are returned</param>
        /// <param name="useSecondary">If true then the payment type ID from <paramref name="storeAndPaymentTypeID"/> is used</param>
        /// <param name="cacheType">Optional parameter to specify what type of cache to use</param>
        /// <returns></returns>
        List<StorePaymentMethod> GetRecords(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID, bool useSecondary, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Gets all store payment methods that have the given operation set as the default function. The value of <paramref name="operationID"/> should be int value which matches the desired <see cref="POSOperations"/>
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store to get the payment methods for</param>
        /// <param name="operationID">The <see cref="POSOperations"/> to get the payment method for</param>
        /// <returns></returns>
        List<StorePaymentMethod> GetTenderForOperation(IConnectionManager entry, RecordIdentifier storeID,
            RecordIdentifier operationID);

        /// <summary>
        /// Checks if a store payment method already exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="paymentMethod">The payment method to check for</param>
        /// <param name="originalTenderTypeID">The ID of the base payment metyhod. This is used to look up the store payment method</param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier originalTenderTypeID);

        /// <summary>
        /// Saves a single store payment method into the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="paymentMethod">The store payment method to save</param>
        /// <param name="originalTenderTypeID">The ID of the base payment metyhod. This is saved to the store payment method</param>
        void Save(IConnectionManager entry, StorePaymentMethod paymentMethod,RecordIdentifier originalTenderTypeID);

        /// <summary>
        /// Gets the first tendertypeID for the given posOperation number.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="posOperation">Pos operation number</param>
        /// <param name="storeID">The store that the tendertype belongs to</param>
        /// <returns>TendertypeID</returns>
        RecordIdentifier GetTenderType(IConnectionManager entry, RecordIdentifier posOperation, RecordIdentifier storeID);

        /// <summary>
        /// Returns the change tender ID for a payment method on a store with a specific a default function (<see cref="PaymentMethodDefaultFunctionEnum"/>).
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store that the tendertype belongs to</param>
        /// <param name="paymentMethod">The default function of the tender type that is to be found</param>
        /// <returns></returns>
        RecordIdentifier GetChangeTenderForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum paymentMethod);

        /// <summary>
        /// Returns the tender ID for a payment method on a store with a specific a default function (<see cref="PaymentMethodDefaultFunctionEnum"/>).
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store that the tendertype belongs to</param>
        /// <param name="paymentMethod">The default function of the tender type that is to be found</param>
        /// <returns></returns>
        RecordIdentifier GetTenderForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum paymentMethod);

        /// <summary>
        /// Gets the tender ID for the payment method on a store that is being used as the main cash payment type.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store that the tender type belongs to</param>
        /// <returns>The ID of the tender, or <value>-1</value> if no store payment type is found</returns>
        RecordIdentifier GetCashTender(IConnectionManager entry, RecordIdentifier storeID);

        /// <summary>
        /// Indicates whether a store payment type shold be counted or not when performing a tender declaration 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="tenderTypeID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        bool CountingRequiredForTender(IConnectionManager entry, RecordIdentifier tenderTypeID, RecordIdentifier storeID);

        /// <summary>
        /// Returns a <see cref="StorePaymentMethod"/> object for a specific store and default function
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store the payment belongs to</param>
        /// <param name="defaultFunction">The default function of the store</param>
        /// <returns></returns>
        StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum defaultFunction);

        /// <summary>
        /// Returns all information about a specific payment method using <see cref="StorePaymentMethod"/> object 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ID">The payment method ID</param>
        /// <param name="cacheType">The type of cache to use</param>
        /// <returns></returns>
        StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a <see cref="StorePaymentMethod"/> for a specific store and payment limitation
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store the peyment belongs to</param>
        /// <param name="limitationMasterID">The master ID of the payment limitation to check for</param>
        /// <param name="cacheType">The type of cache to use</param>
        /// <returns></returns>
        StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier limitationMasterID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Deletes a single store payment type from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeAndTenderIdentifier">The ID of the store and tender to delete (store, tender type ID)</param>
        void Delete(IConnectionManager entry, RecordIdentifier storeAndTenderIdentifier);

        /// <summary>
        /// Return true if any of the change tenders (both normal change and above minimum change) have the operation sent as parameter
        /// Example: If it is needed to check if any of the change back tenders are gifti cards, loyalty payment and etc.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="operation">The ID of the payment operation to be checked</param>
        /// <param name="storeID">The current store that is being checked</param>
        /// <returns></returns>
        bool AnyChangeTenderOfType(IConnectionManager entry, POSOperations operation, RecordIdentifier storeID);

        /// <summary>
        /// Returns true if the associated store payment type only allows refund with the same payment type
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The current store that is being checked</param>
        /// <param name="limitationMasterID">The limitation master ID that is being checked</param>
        /// <returns></returns>
        bool GetForceRefundToThisPaymentType(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier limitationMasterID);
    }
}