using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class PurchaseOrderImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        private string RMSConnectionString { get; set; }

        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            List<ImportLogItem> logItems = new List<ImportLogItem>();
            RMSConnectionString = rmsConnectionString;
            try
            {
                MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
                LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
                if (result != LoginResult.Success)
                {
                    return new List<ImportLogItem>();
                }

                List<RMSPurchaseOrderHeader> purchaseOrderHeaders = entry.Connection.ExecuteReader(Constants.GET_ALL_PURCHASE_ORDERS).ToDataTable().ToList<RMSPurchaseOrderHeader>();
                List<RMSPurchaseOrderLine> purchaseOrderLines = entry.Connection.ExecuteReader(Constants.GET_ALL_PURCHASE_ORDER_LINES).ToDataTable().ToList<RMSPurchaseOrderLine>();

                SetProgressSize(purchaseOrderHeaders.Count() + purchaseOrderLines.Count());

                RecordIdentifier companyCurrency = (RecordIdentifier)Providers.CompanyInfoData.CompanyCurrencyCode(PluginEntry.DataModel);

                purchaseOrderHeaders.ForEach(po =>
                {
                    try
                    {
                        if (lookupManager.StoreLookup.ContainsKey(po.RMS_StoreID))
                        {
                            po.StoreID = lookupManager.StoreLookup[po.RMS_StoreID];
                        }
                        if (lookupManager.VendorLookup.ContainsKey(po.RMS_VendorID))
                        {
                            po.VendorID = lookupManager.VendorLookup[po.RMS_VendorID].ToString();
                        }
                        po.OrderingDate = new Date(po.OrderDate);
                        po.CurrencyCode = companyCurrency;
                        po.DefaultDiscountPercentage = 0;
                        po.DefaultDiscountAmount = 0;
                        po.Orderer = (Guid)PluginEntry.DataModel.CurrentUser.ID;
                        po.PurchaseStatus = PurchaseStatusEnum.Open;

                        Providers.PurchaseOrderData.SaveAndReturnPurchaseOrder(PluginEntry.DataModel, po);
                        if (!lookupManager.PurchaseOrderHeaderLookup.ContainsKey(po.RMS_ID))
                        {
                            lookupManager.PurchaseOrderHeaderLookup.Add(po.RMS_ID, po.ID);
                        }
                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingPurchaseOrderHeader, po.RMS_ID) + " " + ex.Message });
                        ReportProgress?.Invoke();
                    }
                });
                List<SalesTaxGroup> storeSaleTaxGroups = Providers.SalesTaxGroupData.GetSalesTaxGroups(PluginEntry.DataModel, SalesTaxGroup.SortEnum.ID, false);
                Dictionary<decimal, RecordIdentifier> taxGroupLookup = new Dictionary<decimal, RecordIdentifier>();

                storeSaleTaxGroups.ForEach(sg =>
                {
                    List<TaxCodeInSalesTaxGroup> taxCodes = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroup(PluginEntry.DataModel, sg.ID, TaxCodeInSalesTaxGroup.SortEnum.SalesTaxCode, false);
                    taxCodes.ForEach(tc =>
                    {
                        if (!taxGroupLookup.ContainsKey(tc.TaxValue))
                        {
                            taxGroupLookup.Add(tc.TaxValue, tc.SalesTaxGroup);
                        }
                    });
                });

                purchaseOrderLines.ForEach(purchaseOrderLine =>
                {
                    try
                    {
                        RecordIdentifier vendorID = lookupManager.VendorLookup.ContainsKey(purchaseOrderLine.RMS_VendorID) ? lookupManager.VendorLookup[purchaseOrderLine.RMS_VendorID] : RecordIdentifier.Empty;

                        if (!vendorID.IsEmpty)
                        {
                            Vendor v = Providers.VendorData.Get(PluginEntry.DataModel, vendorID);
                            if (!taxGroupLookup.ContainsKey(purchaseOrderLine.VATCode))
                            {
                                SalesTaxGroup gr = new SalesTaxGroup() { Text = purchaseOrderLine.VATCode.ToString() + "%" };
                                Providers.SalesTaxGroupData.Save(PluginEntry.DataModel, gr);

                                TaxCode t = new TaxCode();
                                t.Text = purchaseOrderLine.VATCode.ToString() + "%";
                                Providers.TaxCodeData.Save(PluginEntry.DataModel, t);

                                TaxCodeValue taxCodeLine = new TaxCodeValue();
                                taxCodeLine.TaxCode = t.ID;
                                taxCodeLine.FromDate = new Date(DateTime.Now);
                                taxCodeLine.Value = purchaseOrderLine.VATCode;
                                Providers.TaxCodeValueData.Save(PluginEntry.DataModel, taxCodeLine);

                                TaxCodeInSalesTaxGroup linkItem = new TaxCodeInSalesTaxGroup() { SalesTaxGroup = gr.ID, TaxCode = t.ID };
                                Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(PluginEntry.DataModel, linkItem);
                                taxGroupLookup.Add(purchaseOrderLine.VATCode, gr.ID);

                            }
                            v.TaxGroup = taxGroupLookup[purchaseOrderLine.VATCode];
                            Providers.VendorData.Save(PluginEntry.DataModel, v);
                        }

                        if (lookupManager.PurchaseOrderHeaderLookup.ContainsKey(purchaseOrderLine.RMS_PurchaseOrderHeaderID))
                        {
                            purchaseOrderLine.PurchaseOrderID = lookupManager.PurchaseOrderHeaderLookup[purchaseOrderLine.RMS_PurchaseOrderHeaderID];
                        }
                        if (lookupManager.StandardItemsLookup.ContainsKey(purchaseOrderLine.RMS_ItemID))
                        {
                            purchaseOrderLine.ItemID = lookupManager.StandardItemsLookup[purchaseOrderLine.RMS_ItemID].Item1.ToString();
                        }
                        if (lookupManager.VariantItemsLookup.ContainsKey(purchaseOrderLine.RMS_ItemID))
                        {
                            purchaseOrderLine.ItemID = lookupManager.VariantItemsLookup[purchaseOrderLine.RMS_ItemID].Item1.ToString();
                        }
                        if (lookupManager.UnitOfMeasure.ContainsKey(purchaseOrderLine.UnitOfMeasure))
                        {
                            purchaseOrderLine.UnitID = lookupManager.UnitOfMeasure[purchaseOrderLine.UnitOfMeasure].ToString();
                        }
                        else
                        {
                            purchaseOrderLine.UnitID = lookupManager.DefaultUnitOfMeasureID.ToString();
                        }

                        string key = string.Format("{0}-{1}", vendorID, purchaseOrderLine.ItemID);
                        if (lookupManager.VendorItemLookup != null && lookupManager.VendorItemLookup.ContainsKey(key))
                        {
                            purchaseOrderLine.VendorItemID = lookupManager.VendorItemLookup[key];
                        }

                        purchaseOrderLine.TaxCalculationMethod = TaxCalculationMethodEnum.IncludeTax;
                        if (purchaseOrderLine.TaxCalculationMethod != TaxCalculationMethodEnum.NoTax)
                        {
                            purchaseOrderLine.UnitPrice = purchaseOrderLine.GetDiscountedPrice();
                        }

                        ITaxService taxService = (ITaxService)PluginEntry.DataModel.Service(ServiceType.TaxService);

                        RecordIdentifier salesTaxItemGroupID = lookupManager.SaleGroupTaxLookup.ContainsKey(purchaseOrderLine.RMS_SalesTaxItemGroupID) ? lookupManager.SaleGroupTaxLookup[purchaseOrderLine.RMS_SalesTaxItemGroupID] : RecordIdentifier.Empty;
                        RecordIdentifier storeID = lookupManager.StoreLookup.ContainsKey(purchaseOrderLine.RMS_StoreID) ? lookupManager.StoreLookup[purchaseOrderLine.RMS_StoreID] : RecordIdentifier.Empty;

                        purchaseOrderLine.TaxAmount = taxService.GetTaxAmountForPurchaseOrderLine(
                            PluginEntry.DataModel,
                            salesTaxItemGroupID,
                            vendorID,
                            storeID,
                            purchaseOrderLine.UnitPrice,
                            purchaseOrderLine.DiscountAmount,
                            purchaseOrderLine.DiscountPercentage,
                            purchaseOrderLine.TaxCalculationMethod
                            );

                        Providers.PurchaseOrderLineData.Save(PluginEntry.DataModel, purchaseOrderLine);
                        ReportProgress?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingPurchaseOrderLine, purchaseOrderLine.RMS_ID) + " " + ex.Message });
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
