using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.ExcelFiles.Exceptions;
using LSOne.ViewPlugins.ExcelFiles.Properties;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class StockCountingMapper : MapperBase
    {
        private const string StockCountingIDColumn = "Stock counting Id";
        private const string StoreIDColumn = "Store Id";
        private const string DescriptionColumn = "Description";
        private const string ItemIDColumn = "Item Id";
        private const string BarcodeColumn = "Barcode";
        private const string UnitIDColumn = "Unit Id";
        private const string CountedColumn = "Counted";
        private const string VariantIDColumn = "Variant description";

        internal static void ImportHeader(DataTable dt, List<ImportLogItem> importLogItems)
        {
            RecordIdentifier stockCountingID;
            RecordIdentifier storeID;
            string description;
            bool save = false;
            InventoryAdjustment stockCountingHeader = null;
            string[] mandatoryHeaderColumns = new string[]
            {
                    StockCountingIDColumn, StoreIDColumn
            };

            CheckMandatoryColumns(dt, mandatoryHeaderColumns);
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            foreach (DataRow row in dt.Rows)
            {
                stockCountingID = row.GetStringValue(StockCountingIDColumn);

                if (!CheckMandatoryFields(dt, row, stockCountingID, mandatoryHeaderColumns, true, importLogItems))
                {
                    continue;
                }

                try
                {
                    storeID = row.GetStringValue(StoreIDColumn);
                    description = row.GetStringValue(DescriptionColumn);

                    if (storeID == "" || !Providers.StoreData.Exists(PluginEntry.DataModel, storeID))
                    {
                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, stockCountingID, Properties.Resources.StoreDoesNotExist));
                        continue;
                    }

                    if (!service.InventoryAdjustmentExists(PluginEntry.DataModel, siteServiceProfile, stockCountingID, false))
                    {
                        save = true;

                        stockCountingHeader = new InventoryAdjustment();
                        stockCountingHeader.JournalType = InventoryJournalTypeEnum.Counting;
                        stockCountingHeader.ID = stockCountingID;
                        stockCountingHeader.StoreId = storeID;
                        stockCountingHeader.Text = description;
                    }
                    else
                    {
                        stockCountingHeader = service.GetInventoryAdjustment(PluginEntry.DataModel, siteServiceProfile,
                            stockCountingID, false);

                        if (row.GetStringValue(StoreIDColumn) != (string)stockCountingHeader.StoreId)
                        {
                            stockCountingHeader.StoreId = row.GetStringValue(StoreIDColumn);
                            save = true;
                        }
                        if (row.GetStringValue(DescriptionColumn) != stockCountingHeader.Text)
                        {
                            stockCountingHeader.Text = row.GetStringValue(DescriptionColumn);
                            save = true;
                        }
                    }

                    if (save)
                    {
                        service.SaveInventoryAdjustment(PluginEntry.DataModel, siteServiceProfile, stockCountingHeader, false);
                        importLogItems.Add(new ImportLogItem(save ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, stockCountingID, ""));
                    }
                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, stockCountingID, ex.Message));
                    continue;
                }
            }

            service.Disconnect(PluginEntry.DataModel);
        }

        internal static void ImportLine(DataTable dt, List<ImportLogItem> importLogItems, RecordIdentifier storeID)
        {
            string[] mandatoryHeaderColumns = new string[]
            {
                    StockCountingIDColumn, UnitIDColumn, CountedColumn
            };

            CheckMandatoryColumns(dt, mandatoryHeaderColumns);

            if(!dt.Columns.Contains(ItemIDColumn) && !dt.Columns.Contains(BarcodeColumn))
            {
                throw new ColumnMissingException(ItemIDColumn);
            }

            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            try
            {
                importLogItems.AddRange(service.ImportStockCountingFromExcel(PluginEntry.DataModel, siteServiceProfile, dt, true));
            }
            catch (Exception ex)
            {
                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, "Stock counting import", ex.Message));
            }

            service.Disconnect(PluginEntry.DataModel);
        }
    }
}
