using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class VendorImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;

        private string RMSConnectionString { get; set; }
        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            RecordIdentifier companyCurrency = (RecordIdentifier)Providers.CompanyInfoData.CompanyCurrencyCode(PluginEntry.DataModel);
            lookupManager.VendorLookup = new Dictionary<int, RecordIdentifier>();
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                List<RMSVendor> vendors = entry.Connection.ExecuteReader(Constants.GET_ALL_VENDORS).ToDataTable().ToList<RMSVendor>();
                List<RMSVendorItem> vendorItems = entry.Connection.ExecuteReader(Constants.GET_ALL_VENDOR_ITEMS).ToDataTable().ToList<RMSVendorItem>();

                SetProgressSize(vendors.Count() + vendorItems.Count());
                vendors.ForEach(v =>
                {
                    try
                    {
                        v.AddressFormat = Address.AddressFormatEnum.GenericWithState;
                        v.ZipCode = v.ZIP;

                        if (lookupManager.CurrencyLookup != null && lookupManager.CurrencyLookup.ContainsKey(v.RMS_CurrencyID))
                        {
                            v.CurrencyID = lookupManager.CurrencyLookup[v.RMS_CurrencyID];
                        }
                        else
                        {
                            v.CurrencyID = companyCurrency;
                        }
                        Providers.VendorData.Save(PluginEntry.DataModel, v);
                        if (!lookupManager.VendorLookup.ContainsKey(v.RMS_ID))
                        {
                            lookupManager.VendorLookup.Add(v.RMS_ID, v.ID);
                        }

                        if (!string.IsNullOrEmpty(v.DefaultContact))
                        {
                            Contact vContact = new Contact();
                            vContact.OwnerType = DataLayer.BusinessObjects.Enums.ContactRelationTypeEnum.Vendor;
                            vContact.Name = new Name(string.Empty, v.DefaultContact, string.Empty, string.Empty, string.Empty);
                            vContact.OwnerID = v.ID;

                            Providers.ContactData.Save(PluginEntry.DataModel, vContact);
                            Providers.VendorData.SetDefaultContact(PluginEntry.DataModel, v.ID, vContact.ID);

                        }
                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingVendor, v.RMS_ID) + " " + ex.Message });
                        ReportProgress?.Invoke();
                    }
                });

                vendorItems.ForEach(vi =>
                {
                    try
                    {
                        if (lookupManager.VendorLookup != null && lookupManager.VendorLookup.ContainsKey(vi.RMS_VendorID))
                        {
                            vi.VendorID = lookupManager.VendorLookup[vi.RMS_VendorID];
                        }

                        if (lookupManager.StandardItemsLookup.ContainsKey(vi.RMS_ItemID))
                        {
                            vi.RetailItemID = lookupManager.StandardItemsLookup[vi.RMS_ItemID].Item1;
                            vi.UnitID = lookupManager.StandardItemsLookup[vi.RMS_ItemID].Item2;
                        }
                        if (lookupManager.VariantItemsLookup.ContainsKey(vi.RMS_ItemID))
                        {
                            vi.RetailItemID = lookupManager.VariantItemsLookup[vi.RMS_ItemID].Item1;
                            vi.UnitID = lookupManager.VariantItemsLookup[vi.RMS_ItemID].Item2;
                        }

                        Providers.VendorItemData.Save(PluginEntry.DataModel, vi);
                        string key = string.Format("{0}-{1}", vi.VendorID, vi.RetailItemID);
                        if (lookupManager.VendorItemLookup != null && !lookupManager.VendorItemLookup.ContainsKey(key))
                        {
                            lookupManager.VendorItemLookup.Add(key, vi.VendorItemID.ToString());
                        }

                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingVendorItem, vi.RMS_ID) + " " + ex.Message });
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
