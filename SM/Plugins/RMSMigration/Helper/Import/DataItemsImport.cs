using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.DataLayer.SqlDataProviders.Replenishment;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Helper.Import
{
    public class DataItemsImport : IImportManager
    {
        public event ReportProgressHandler ReportProgress;
        public event SetProgressSizeHandler SetProgressSize;
        public ILookupManager LookupManager { get; set; }

        private string RMSConnectionString { get; set; }

        private List<RMSItem> RMSItems = new List<RMSItem>();

        public List<ImportLogItem> Import(ILookupManager lookupManager, string rmsConnectionString)
        {
            LookupManager = lookupManager;
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

                List<RMSRetailDepartment> retailDepartments = entry.Connection.ExecuteReader(Constants.GET_ALL_RETAILDEPARTMENTS).ToDataTable().ToList<RMSRetailDepartment>();
                List<RMSRetailGroup> retailGroups = entry.Connection.ExecuteReader(Constants.GET_ALL_RETAILGROUPS).ToDataTable().ToList<RMSRetailGroup>();
                List<RMSItem> matrixItems = entry.Connection.ExecuteReader(Constants.GET_ALL_MATRIX_ITEMS).ToDataTable().ToList();
                List<RMSItem> standardItems = entry.Connection.ExecuteReader(Constants.GET_ALL_STANDARD_ITEMS).ToDataTable().ToList();
                List<RMSItem> variantItems = entry.Connection.ExecuteReader(Constants.GET_ALL_VARIANT_ITEMS).ToDataTable().ToList();
                List<RMSItemSalePrice> itemSalePrices = entry.Connection.ExecuteReader(Constants.GET_ALL_ITEM_SALE_PRICE).ToDataTable().ToList<RMSItemSalePrice>();

                int allDataItemsCount = retailDepartments.Count + retailGroups.Count +
                                        matrixItems.Count() + standardItems.Count() +
                                        variantItems.Count() + GetAttributeCount(variantItems, logItems) + itemSalePrices.Count;

                SetProgressSize(allDataItemsCount);
                ImportRetailDepartments(retailDepartments, logItems);
                ImportRetailGroups(retailGroups, logItems);
                ImportMatrixItems(matrixItems, logItems);
                ImportStandardItems(standardItems, logItems);
                ImportVariants(variantItems, logItems);
                RMSItems.ForEach(r => ImportAdditionalEntitiesForItem(r));
                ImportItemSalePrices(itemSalePrices, logItems);
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = ex.Message });
                return logItems;
            }
            return logItems;
        }

        private void ImportRetailDepartments(List<RMSRetailDepartment> retailDepartments, List<ImportLogItem> logItems)
        {
            retailDepartments.ForEach(rd =>
            {
                try
                {
                    Providers.RetailDepartmentData.Save(PluginEntry.DataModel, rd);

                    if (!LookupManager.RetailDepartmentLookup.ContainsKey(rd.RMS_ID))
                    {
                        LookupManager.RetailDepartmentLookup.Add(rd.RMS_ID, new Tuple<RecordIdentifier, RecordIdentifier>(rd.ID, rd.MasterID));
                    }
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingDepartment, rd.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }
        private void ImportRetailGroups(List<RMSRetailGroup> retailGroups, List<ImportLogItem> logItems)
        {
            retailGroups.ForEach(rg =>
            {
                try
                {
                    if (LookupManager.RetailDepartmentLookup != null && LookupManager.RetailDepartmentLookup.ContainsKey(rg.RMS_Department_ID))
                    {
                        rg.RetailDepartmentID = LookupManager.RetailDepartmentLookup[rg.RMS_Department_ID].Item1;
                        rg.RetailDepartmentMasterID = LookupManager.RetailDepartmentLookup[rg.RMS_Department_ID].Item2;
                    }

                    Providers.RetailGroupData.Save(PluginEntry.DataModel, rg);

                    if (!LookupManager.RetailGroupLookup.ContainsKey(rg.RMS_ID))
                    {
                        LookupManager.RetailGroupLookup.Add(rg.RMS_ID, new Tuple<RecordIdentifier, RecordIdentifier>(rg.ID, rg.MasterID));
                    }
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingRetailGroup, rg.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }

        private void ImportMatrixItems(List<RMSItem> matrixItems, List<ImportLogItem> logItems)
        {
            matrixItems.ForEach(mi =>
            {
                try
                {
                    mi.ID = mi.ItemNumber;
                    mi.ItemType = DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem;
                    if (mi.RMS_CustomerID.HasValue && LookupManager.CustomerLookup.ContainsKey(mi.RMS_CustomerID.Value))
                    {
                        mi.DefaultVendorID = LookupManager.CustomerLookup[mi.RMS_CustomerID.Value];
                    }
                    if (LookupManager.RetailGroupLookup.ContainsKey(mi.RMS_RetailGroupID))
                    {
                        mi.RetailGroupMasterID = LookupManager.RetailGroupLookup[mi.RMS_RetailGroupID].Item2;
                    }
                    if (LookupManager.RetailDepartmentLookup.ContainsKey(mi.RMS_DepartmentID))
                    {
                        mi.RetailDepartmentMasterID = LookupManager.RetailDepartmentLookup[mi.RMS_DepartmentID].Item2;
                    }
                    if (mi.RMS_SalesTaxItemGroupID.HasValue && LookupManager.SaleGroupTaxLookup.ContainsKey(mi.RMS_SalesTaxItemGroupID.Value))
                    {
                        mi.SalesTaxItemGroupID = LookupManager.SaleGroupTaxLookup[mi.RMS_SalesTaxItemGroupID.Value];
                    }

                    mi.SalesUnitID = LookupManager.DefaultUnitOfMeasureID;
                    mi.PurchaseUnitID = LookupManager.DefaultUnitOfMeasureID;
                    mi.InventoryUnitID = LookupManager.DefaultUnitOfMeasureID;

                    Providers.RetailItemData.Save(PluginEntry.DataModel, mi);

                    mi.AddBarcode(LookupManager.SetupBarcodeID);

                    if (!LookupManager.MatrixItemLookup.ContainsKey(mi.RMS_ID))
                    {
                        LookupManager.MatrixItemLookup.Add(mi.RMS_ID, mi.MasterID);
                    }

                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingMatrixItem, mi.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }

        private void ImportStandardItems(List<RMSItem> standardItems, List<ImportLogItem> logItems)
        {
            standardItems.ForEach(ri =>
            {
                try
                {
                    ri.ID = ri.ItemNumber;
                    ri.ItemType = DataLayer.BusinessObjects.Enums.ItemTypeEnum.Item;
                    if (ri.RMS_CustomerID.HasValue && LookupManager.CustomerLookup.ContainsKey(ri.RMS_CustomerID.Value))
                    {
                        ri.DefaultVendorID = LookupManager.CustomerLookup[ri.RMS_CustomerID.Value];
                    }
                    if (LookupManager.RetailGroupLookup.ContainsKey(ri.RMS_RetailGroupID))
                    {
                        ri.RetailGroupMasterID = LookupManager.RetailGroupLookup[ri.RMS_RetailGroupID].Item2;
                    }
                    if (LookupManager.RetailDepartmentLookup.ContainsKey(ri.RMS_DepartmentID))
                    {
                        ri.RetailDepartmentMasterID = LookupManager.RetailDepartmentLookup[ri.RMS_DepartmentID].Item2;
                    }
                    if (ri.RMS_SalesTaxItemGroupID.HasValue && LookupManager.SaleGroupTaxLookup.ContainsKey(ri.RMS_SalesTaxItemGroupID.Value))
                    {
                        ri.SalesTaxItemGroupID = LookupManager.SaleGroupTaxLookup[ri.RMS_SalesTaxItemGroupID.Value];
                    }

                    ri.CalculatePrice();
                    ri.SetUnitOfMeasure(LookupManager);

                    Providers.RetailItemData.Save(PluginEntry.DataModel, ri);

                    ri.AddBarcode(LookupManager.SetupBarcodeID);

                    if (!LookupManager.StandardItemsLookup.ContainsKey(ri.RMS_ID))
                    {
                        LookupManager.StandardItemsLookup.Add(ri.RMS_ID, new Tuple<RecordIdentifier, RecordIdentifier>(ri.ID, ri.SalesUnitID));
                    }
                    ImportPicture(ri.ID, ri.PictureName);
                    RMSItems.Add(ri);
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingStandardItem, ri.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }

        private void ImportVariants(List<RMSItem> variantItems, List<ImportLogItem> logItems)
        {
            ImportDimensions(variantItems, logItems);
            variantItems.ForEach(vi =>
            {
                try
                {
                    vi.ID = vi.VariantCode;
                    vi.ItemType = DataLayer.BusinessObjects.Enums.ItemTypeEnum.Item;
                    if (vi.RMS_CustomerID.HasValue && LookupManager.CustomerLookup.ContainsKey(vi.RMS_CustomerID.Value))
                    {
                        vi.DefaultVendorID = LookupManager.CustomerLookup[vi.RMS_CustomerID.Value];
                    }
                    if (LookupManager.RetailGroupLookup.ContainsKey(vi.RMS_RetailGroupID))
                    {
                        vi.RetailGroupMasterID = LookupManager.RetailGroupLookup[vi.RMS_RetailGroupID].Item2;
                    }
                    if (LookupManager.RetailDepartmentLookup.ContainsKey(vi.RMS_DepartmentID))
                    {
                        vi.RetailDepartmentMasterID = LookupManager.RetailDepartmentLookup[vi.RMS_DepartmentID].Item2;
                    }
                    if (vi.RMS_SalesTaxItemGroupID.HasValue && LookupManager.SaleGroupTaxLookup.ContainsKey(vi.RMS_SalesTaxItemGroupID.Value))
                    {
                        vi.SalesTaxItemGroupID = LookupManager.SaleGroupTaxLookup[vi.RMS_SalesTaxItemGroupID.Value];
                    }

                    vi.CalculatePrice();
                    vi.SetUnitOfMeasure(LookupManager);

                    if (LookupManager.MatrixItemLookup.ContainsKey(vi.RMS_MasterItemID))
                    {
                        vi.HeaderItemID = LookupManager.MatrixItemLookup[vi.RMS_MasterItemID];
                    }
                    Providers.RetailItemData.Save(PluginEntry.DataModel, vi);

                    vi.AddBarcode(LookupManager.SetupBarcodeID);

                    if (!LookupManager.VariantItemsLookup.ContainsKey(vi.RMS_ID))
                    {
                        LookupManager.VariantItemsLookup.Add(vi.RMS_ID, new Tuple<RecordIdentifier, RecordIdentifier>(vi.ID, vi.SalesUnitID));
                    }

                    if (!string.IsNullOrEmpty(vi.DimensionAttribute1) && !string.IsNullOrEmpty(vi.Dimension1))
                    {
                        Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, vi.MasterID, Dimension1[string.Format("{0}{1}{2}", vi.HeaderItemID, vi.Dimension1, vi.DimensionAttribute1)]);
                    }
                    if (!string.IsNullOrEmpty(vi.DimensionAttribute2) && !string.IsNullOrEmpty(vi.Dimension2))
                    {
                        Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, vi.MasterID, Dimension2[string.Format("{0}{1}{2}", vi.HeaderItemID, vi.Dimension2, vi.DimensionAttribute2)]);
                    }
                    if (!string.IsNullOrEmpty(vi.DimensionAttribute3) && !string.IsNullOrEmpty(vi.Dimension3))
                    {
                        Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, vi.MasterID, Dimension3[string.Format("{0}{1}{2}", vi.HeaderItemID, vi.Dimension3, vi.DimensionAttribute3)]);
                    }
                    ImportPicture(vi.ID, vi.PictureName);
                    RMSItems.Add(vi);
                    ReportProgress?.Invoke();
                }
                catch (Exception ex)
                {
                    logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingVariantItems, vi.RMS_ID) + " " + ex.Message });
                    ReportProgress?.Invoke();
                }
            });
        }

        private int GetAttributeCount(List<RMSItem> variantItems, List<ImportLogItem> logItems)
        {
            int count = 0;
            try
            {
                variantItems.GroupBy(el => el.RMS_MasterItemID).ToList().ForEach(gr =>
                {
                    RecordIdentifier masterItemID = RecordIdentifier.Empty;
                    if (LookupManager.MatrixItemLookup.ContainsKey(gr.Key))
                    {
                        masterItemID = LookupManager.MatrixItemLookup[gr.Key];
                    }
                    string dimension1Name = gr.FirstOrDefault().Dimension1;
                    List<string> dimension1Attributes = gr.Select(el => el.DimensionAttribute1).Distinct().ToList();

                    string dimension2Name = gr.FirstOrDefault().Dimension2;
                    List<string> dimension2Attributes = gr.Select(el => el.DimensionAttribute2).Distinct().ToList();

                    string dimension3Name = gr.FirstOrDefault().Dimension3;
                    List<string> dimension3Attributes = gr.Select(el => el.DimensionAttribute3).Distinct().ToList();

                    count += 3 + dimension1Attributes.Count + dimension2Attributes.Count + dimension3Attributes.Count;
                });
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingDimensions) + " " + ex.Message });
                return 0;
            }
            return count;
        }

        private void ImportDimensions(List<RMSItem> variantItems, List<ImportLogItem> logItems)
        {
            try
            {
                variantItems.GroupBy(el => el.RMS_MasterItemID).ToList().ForEach(gr =>
                {
                    dimensionSequense = 10;
                    RecordIdentifier masterItemID = RecordIdentifier.Empty;
                    if (LookupManager.MatrixItemLookup.ContainsKey(gr.Key))
                    {
                        masterItemID = LookupManager.MatrixItemLookup[gr.Key];
                    }
                    string dimension1Name = gr.FirstOrDefault().Dimension1;
                    if (!string.IsNullOrEmpty(dimension1Name))
                    {
                        List<string> dimension1Attributes = gr.Select(el => el.DimensionAttribute1).Distinct().ToList();
                        AddDimension(masterItemID, dimension1Name, dimension1Attributes, Dimension1);
                    }

                    string dimension2Name = gr.FirstOrDefault().Dimension2;
                    if (!string.IsNullOrEmpty(dimension2Name))
                    {
                        List<string> dimension2Attributes = gr.Select(el => el.DimensionAttribute2).Distinct().ToList();
                        AddDimension(masterItemID, dimension2Name, dimension2Attributes, Dimension2);
                    }

                    string dimension3Name = gr.FirstOrDefault().Dimension3;
                    if (!string.IsNullOrEmpty(dimension3Name))
                    {
                        List<string> dimension3Attributes = gr.Select(el => el.DimensionAttribute3).Distinct().ToList();
                        AddDimension(masterItemID, dimension3Name, dimension3Attributes, Dimension3);
                    }
                });
            }
            catch (Exception ex)
            {
                logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingDimensions) + " " + ex.Message });
            }
        }


        public void ImportPicture(RecordIdentifier itemID, string imageName)
        {
            try
            {
                if (string.IsNullOrEmpty(LookupManager.BaseImagePath) || string.IsNullOrEmpty(imageName))
                {
                    return;
                }

                string fullImagePath = Path.Combine(LookupManager.BaseImagePath, imageName);
                if (!File.Exists(fullImagePath))
                {
                    return;
                }

                Image img = Image.FromFile(fullImagePath);
                Providers.RetailItemData.SaveImage(PluginEntry.DataModel, new ItemImage { ID = itemID, Image = img, ImageIndex = 0 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Dictionary<string, RecordIdentifier> Dimension1 = new Dictionary<string, RecordIdentifier>();
        private Dictionary<string, RecordIdentifier> Dimension2 = new Dictionary<string, RecordIdentifier>();
        private Dictionary<string, RecordIdentifier> Dimension3 = new Dictionary<string, RecordIdentifier>();
        private int dimensionSequense = 0;

        private void AddDimension(RecordIdentifier masterItemID, string dimensionName, List<string> attributes, Dictionary<string, RecordIdentifier> dimLookup)
        {
            int attributeSequense;

            RetailItemDimension dimension = new RetailItemDimension();
            dimension.RetailItemMasterID = masterItemID;
            dimension.Text = dimensionName;
            dimension.Sequence = dimensionSequense;

            Providers.RetailItemDimensionData.Save(PluginEntry.DataModel, dimension);
            ReportProgress?.Invoke();

            dimensionSequense += 10;
            attributeSequense = 10;

            foreach (var attribute in attributes)
            {
                DimensionAttribute attr = new DimensionAttribute();
                attr.DimensionID = dimension.ID;
                attr.Text = attribute;
                attr.Code = attribute.Length >= 20 ? attribute.Substring(0, 20) : attribute;
                attr.Sequence = attributeSequense;

                Providers.DimensionAttributeData.Save(PluginEntry.DataModel, attr);
                ReportProgress?.Invoke();

                attributeSequense += 10;

                dimLookup.Add(string.Format("{0}{1}{2}", masterItemID, dimensionName, attribute), attr.ID);
            }
        }

        private void ImportItemSalePrices(List<RMSItemSalePrice> itemSalePrices, List<ImportLogItem> logItems)
        {
            string companyCurrency = Providers.CompanyInfoData.CompanyCurrencyCode(PluginEntry.DataModel);

            itemSalePrices.ForEach(isp =>
             {
                 try
                 {
                     isp.ItemCode = DataLayer.BusinessObjects.PricesAndDiscounts.TradeAgreementEntry.TradeAgreementEntryItemCode.Table;
                     isp.AccountCode = DataLayer.BusinessObjects.PricesAndDiscounts.TradeAgreementEntryAccountCode.All;
                     isp.AccountRelation = "";
                     isp.Currency = companyCurrency;
                     isp.SearchAgain = true;
                     isp.Relation = DataLayer.BusinessObjects.PricesAndDiscounts.TradeAgreementEntry.TradeAgreementEntryRelation.PriceSales;
                     isp.FromDate = isp.RMS_StartDate == DateTime.MinValue ? Date.Empty : new Date(isp.RMS_StartDate);
                     isp.ToDate = isp.RMS_EndDate == DateTime.MinValue ? Date.Empty : new Date(isp.RMS_EndDate);

                     Tuple<RecordIdentifier, RecordIdentifier> lookup = null;
                     if (LookupManager.StandardItemsLookup.ContainsKey(isp.RMS_ItemID))
                     {
                         lookup = LookupManager.StandardItemsLookup[isp.RMS_ItemID];
                     }
                     else if (LookupManager.VariantItemsLookup.ContainsKey(isp.RMS_ItemID))
                     {
                         lookup = LookupManager.VariantItemsLookup[isp.RMS_ItemID];
                     }

                     if (lookup != null)
                     {
                         isp.ItemRelation = lookup.Item1;
                         isp.UnitID = lookup.Item2;
                     }

                     Providers.TradeAgreementData.Save(PluginEntry.DataModel, isp, Permission.ManageDiscounts);

                     ReportProgress?.Invoke();
                 }
                 catch (Exception ex)
                 {
                     logItems.Add(new ImportLogItem() { ErrorMessage = string.Format(Properties.Resources.ErrorMigratingItemSalePrice, isp.RMS_ID) + " " + ex.Message });
                     ReportProgress?.Invoke();
                 }
             });
        }

        public Tuple<RecordIdentifier, RecordIdentifier> GetItemByRMSID(int rmsID)
        {
            if (LookupManager.StandardItemsLookup.ContainsKey(rmsID))
            {
                return LookupManager.StandardItemsLookup[rmsID];

            }
            if (LookupManager.VariantItemsLookup.ContainsKey(rmsID))
            {
                return LookupManager.VariantItemsLookup[rmsID];
            }
            else return null;
        }

        public void ImportAdditionalEntitiesForItem(RMSItem item)
        {
            try
            {
                if (item.TagAlongItem != 0)
                {
                    Tuple<RecordIdentifier, RecordIdentifier> lsOneItem = GetItemByRMSID(item.TagAlongItem);

                    if (lsOneItem != null)
                    {
                        LinkedItem li = new LinkedItem();
                        li.LinkedItemID = lsOneItem.Item1;
                        li.LinkedItemQuantity = item.TagAlongQuantity;
                        li.OriginalItemID = item.ID;
                        li.LinkedItemUnitID = lsOneItem.Item2;
                        li.Blocked = false;
                        Providers.LinkedItemData.Save(PluginEntry.DataModel, li);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                ItemReplenishmentSetting itemReplenishment = new ItemReplenishmentSetting();
                itemReplenishment.ItemId = item.ID;
                itemReplenishment.ReplenishmentMethod = ReplenishmentMethodEnum.StockLevel;
                itemReplenishment.ReorderPoint = item.ReorderPoint;
                itemReplenishment.MaximumInventory = item.RestockLevel;
                Providers.ItemReplenishmentSettingData.Save(PluginEntry.DataModel, itemReplenishment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
