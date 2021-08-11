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
        /// Saves a single <see cref="Currency"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="currency">The currency to save</param>
        [OperationContract]
        void Save(Currency currency);

        /// <summary>
        /// Saves a list of <see cref="Currency"/> objects to the database. Currencies that do not exist will be created.
        /// </summary>
        /// <param name="currencies">The list of currencies to save</param>
        [OperationContract]
        SaveResult SaveList(List<Currency> currencies);

        /// <summary>
        /// Gets a single <see cref="Currency"/> from the databse for the given <paramref name="currencyId"/>
        /// </summary>
        /// <param name="currencyId">The ID of the currency to get. </param>
        /// <returns></returns>
        [OperationContract]
        Currency Get(RecordIdentifier currencyId);

        /// <summary>
        /// Deletes the currency with the given <paramref name="currencyId"/> from the database
        /// </summary>
        /// <param name="currencyId">The ID of the currency to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier currencyId);
    }
}
