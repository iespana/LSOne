using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkCurrencyInterface
{
    public partial interface IIntegrationFrameworkCurrencyService
    {
        /// <summary>
        /// Saves a single <see cref="ExchangeRate"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="exchangeRate">The exchange rate to save</param>
        [OperationContract]
        void SaveExchangeRate(ExchangeRate exchangeRate);

        /// <summary>
        /// Saves a list of <see cref="ExchangeRate"/> objects to the database. Exchange rates that do not exist will be created.
        /// </summary>
        /// <param name="exchangeRates">The list of exchange rates to save</param>
        [OperationContract]
        SaveResult SaveExchangeRateList(List<ExchangeRate> exchangeRates);

        /// <summary>
        /// Gets a single <see cref="ExchangeRate"/> from the databse for the given <paramref name="exchangeRateId"/>
        /// </summary>
        /// <param name="exchangeRateId">The ID of the exchange rate to get. </param>
        /// <returns></returns>
        [OperationContract]
        ExchangeRate GetExchangeRate(RecordIdentifier exchangeRateId);

        /// <summary>
        /// Deletes the exchange rate with the given <paramref name="exchangeRateId"/> from the database
        /// </summary>
        /// <param name="exchangeRateId">The ID of the exchange rate to delete</param>
        [OperationContract]
        void DeleteExchangeRate(RecordIdentifier exchangeRateId);
    }
}
