using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkCurrency
{
    public partial class IntegrationFrameworkCurrencyPlugin
    {
        public virtual void SaveExchangeRate(ExchangeRate exchangeRate)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    bool rateExists = Providers.ExchangeRatesData.Exists(dataModel, exchangeRate.ID);
                    Providers.ExchangeRatesData.Save(dataModel, exchangeRate, rateExists ? (DateTime)exchangeRate.FromDate : new DateTime());
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        public virtual SaveResult SaveExchangeRateList(List<ExchangeRate> exchangeRates)
        {
            Action<IConnectionManager, ExchangeRate> save = (dataModel, exchangeRate) =>
            {
                bool rateExists = Providers.ExchangeRatesData.Exists(dataModel, exchangeRate.ID);
                Providers.ExchangeRatesData.Save(dataModel, exchangeRate, rateExists ? (DateTime)exchangeRate.FromDate : new DateTime());
            };

            return SaveList(exchangeRates, Providers.ExchangeRatesData, save);
        }

        public virtual ExchangeRate GetExchangeRate(RecordIdentifier exchangeRateId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.ExchangeRatesData.Get(dataModel, exchangeRateId);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
                return null;
            }
        }

        public virtual void DeleteExchangeRate(RecordIdentifier exchangeRateId)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.ExchangeRatesData.Delete(dataModel, exchangeRateId);
                }
                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }
    }
}
