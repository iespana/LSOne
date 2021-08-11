using LSOne.DataLayer.BusinessObjects.Tax;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class SalesTaxImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }
        public ILookupManager LookupManager { get; set; }

        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            LookupManager = lookupManager;
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                List<RMSTax> rmsTax = entry.Connection.ExecuteReader(Constants.GET_ALL_SALES_TAX).ToDataTable().ToList<RMSTax>();
                List<RMSItemTax> rmsGroupTax = entry.Connection.ExecuteReader(Constants.GET_ALL_GROUP_TAX).ToDataTable().ToList<RMSItemTax>();

                int allDataItemsCount = rmsTax.Count() + rmsGroupTax.Count();

                SetProgressSize(allDataItemsCount);

                ImportTax(rmsTax, logItems);
                ImportGruopTax(rmsGroupTax, logItems);
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = ex.Message });
                return logItems;
            }
            return logItems;
        }

        private void ImportTax(List<RMSTax> rmsTax, List<ImportLogItem> logItems)
        {
            rmsTax.ForEach(t =>
            {
                try
                {
                    Providers.TaxCodeData.Save(PluginEntry.DataModel, t);

                    TaxCodeValue taxCodeLine = new TaxCodeValue();
                    taxCodeLine.TaxCode = t.ID;
                    taxCodeLine.FromDate = new Date(DateTime.Today.AddDays(-1));
                    taxCodeLine.Value = (decimal)t.Percentage;

                    Providers.TaxCodeValueData.Save(PluginEntry.DataModel, taxCodeLine);

                    if (!LookupManager.SaleTaxLookup.ContainsKey(t.RMS_ID))
                    {
                        LookupManager.SaleTaxLookup.Add(t.RMS_ID, t.ID);
                    }
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingTax, t.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }

        private void ImportGruopTax(List<RMSItemTax> rmsGroupTax, List<ImportLogItem> logItems)
        {
            rmsGroupTax.ForEach(t =>
            {
                try
                {
                    Providers.ItemSalesTaxGroupData.Save(PluginEntry.DataModel, t);

                    for (int i = 0; i <= 10; i++)
                    {
                        PropertyInfo propertyInfo = t.GetType().GetProperty(string.Format("Tax{0}", i));
                        if (propertyInfo == null)
                        {
                            continue;
                        }
                        int id = (int)propertyInfo.GetValue(t, null);
                        if (LookupManager.SaleTaxLookup.ContainsKey(id))
                        {
                            TaxCodeInItemSalesTaxGroup line = new TaxCodeInItemSalesTaxGroup { TaxItemGroup = t.ID, TaxCode = LookupManager.SaleTaxLookup[id] };
                            Providers.ItemSalesTaxGroupData.AddTaxCodeToItemSalesTaxGroup(PluginEntry.DataModel, line);
                        }
                    }

                    if (!LookupManager.SaleGroupTaxLookup.ContainsKey(t.RMS_ID))
                    {
                        LookupManager.SaleGroupTaxLookup.Add(t.RMS_ID, t.ID);
                    }
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingTaxGroup, t.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }
    }
}
