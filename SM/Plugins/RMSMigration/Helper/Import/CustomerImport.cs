using LSOne.DataLayer.BusinessObjects.Customers;
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
    public class CustomerImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }
        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            lookupManager.CustomerLookup = new Dictionary<int, RecordIdentifier>();
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                List<RMSCustomer> customers = entry.Connection.ExecuteReader(Constants.GET_ALL_CUSTOMERS).ToDataTable().ToList<RMSCustomer>();
                SetProgressSize(customers.Count());
                customers.ForEach(c =>
                {
                    try
                    {
                        c.Text = c.Text.Trim();
                        c.FirstName = c.FirstName.Trim();
                        c.LastName = c.LastName.Trim();
                        c.Prefix = c.Prefix.Trim();
                        c.Contact = c.Contact.Trim();
                        c.MiddleName = c.MiddleName.Trim();

                        CustomerAddress address = new CustomerAddress
                        {
                            Address = new Address()
                            {
                                Address1 = c.Address,
                                AddressType = Address.AddressTypes.Shipping,
                                City = c.City,
                                Zip = c.ZIP,
                                State = c.State,
                                Country = c.Country,
                            },
                            ContactName = c.Contact,
                            IsDefault = true
                        };
                        c.Addresses.Add(address);
                        Providers.CustomerData.Save(PluginEntry.DataModel, c);

                        address.CustomerID = c.ID.StringValue;
                        Providers.CustomerAddressData.Save(PluginEntry.DataModel, address);

                        if (!lookupManager.CustomerLookup.ContainsKey(c.RMS_ID))
                        {
                            lookupManager.CustomerLookup.Add(c.RMS_ID, c.ID);
                        }
                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingCustomer, c.RMS_ID) + " " + ex.Message });
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
