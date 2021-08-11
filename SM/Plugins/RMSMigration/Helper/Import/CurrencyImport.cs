using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class CurrencyImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }
        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            lookupManager.CurrencyLookup = new Dictionary<int, RecordIdentifier>();
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                List<RMSCurrency> currencies = entry.Connection.ExecuteReader(Constants.GET_ALL_CURRENCIES).ToDataTable().ToList<RMSCurrency>();
                SetProgressSize(currencies.Count());
                currencies.ForEach(c =>
                {
                    try
                    {
                        Providers.CurrencyData.Save(PluginEntry.DataModel, c);
                        DateTime fromDate = DateTime.Now.Date;
                        ExchangeRate exchangeRate = Providers.ExchangeRatesData.Get(PluginEntry.DataModel, new RecordIdentifier(c.ID, fromDate));
                        decimal lsOneExchangeRate = c.ExchangeRate * 100;
                        Providers.ExchangeRatesData.Save(PluginEntry.DataModel,
                            new ExchangeRate()
                            {
                                CurrencyCode = c.ID,
                                ExchangeRateValue = lsOneExchangeRate,
                                POSExchangeRateValue = lsOneExchangeRate,
                                FromDate = DateTime.Now.Date
                            }, exchangeRate == null ? new DateTime(0001, 01, 01) : fromDate);
                        if (!lookupManager.CurrencyLookup.ContainsKey(c.RMS_ID))
                        {
                            lookupManager.CurrencyLookup.Add(c.RMS_ID, c.ID);
                        }
                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingCurrency, c.RMS_ID) + " " + ex.Message });
                        ReportProgress?.Invoke();
                    }
                });
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = ex.Message });
                return logItems;
            }
            return logItems;
        }
    }
}
