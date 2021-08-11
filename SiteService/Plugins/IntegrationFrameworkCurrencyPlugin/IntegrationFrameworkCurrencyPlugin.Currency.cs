using System;
using System.Collections.Generic;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkCurrency
{
    public partial class IntegrationFrameworkCurrencyPlugin
    {
        public virtual void Save(Currency currency)
        {
            try
            { 
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {                  
                    Providers.CurrencyData.Save(dataModel, currency);
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

        public virtual SaveResult SaveList(List<Currency> currencies)
        {
            return SaveList(currencies, Providers.CurrencyData);
        }

        public virtual Currency Get(RecordIdentifier currencyID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    return Providers.CurrencyData.Get(dataModel, currencyID);
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

        public virtual void Delete(RecordIdentifier currencyID)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.CurrencyData.Delete(dataModel, currencyID);
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
