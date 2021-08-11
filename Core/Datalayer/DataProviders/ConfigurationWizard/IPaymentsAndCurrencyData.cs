using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface IPaymentsAndCurrencyData : IDataProviderBase<PaymentsAndCurrency>
    {
        /// <summary>
        /// Get tender list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        List<PaymentsAndCurrency> GetTenderList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get currency list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        List<PaymentsAndCurrency> GetCurrencyList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">table name</param>
        /// <returns></returns>
        void Delete(IConnectionManager entry, RecordIdentifier id, string table);

        /// <summary>
        /// Save a given tender declaration into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="paymentsAndCurrencyList">PaymentsAndCurrency</param>
        void SaveTenderCurrency(IConnectionManager entry, List<PaymentsAndCurrency> paymentsAndCurrencyList);

        /// <summary>
        /// Get selected payment method list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">payment method ids</param>
        /// <returns>payment method list</returns>
        List<PaymentMethod> GetSelectedPaymentMethodList(IConnectionManager entry, List<RecordIdentifier> idList);

        /// <summary>
        /// Get selected store payment method list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeAndPaymentTypeID"></param>
        /// <returns>store payment method</returns>
        StorePaymentMethod GetSelectedStorePaymentMethodList(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID);

        /// <summary>
        /// Get selected currency list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCodeList">currencyId list</param>
        /// <returns>A list of selected currency</returns>
        List<Currency> GetSelectedCurrencyList(IConnectionManager entry, List<RecordIdentifier> currencyCodeList);
    }
}