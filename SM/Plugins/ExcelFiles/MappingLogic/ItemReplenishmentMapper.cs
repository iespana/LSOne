using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class ItemReplenishmentMapper : MapperBase
    {
        private const string ItemIDColumn = "ITEMID";
        private const string ItemDescriptionColumn = "ITEM DESCRIPTION";
        private const string VariantDescriptionColumn = "VARIANT DESCRIPTION";
        private const string StoreIDColumn = "STOREID";
        private const string ReorderPointColumn = "REORDER POINT";
        private const string MaximumInventoryColumn = "MAXIMUM INVENTORY";
        private const string PurchaseOrderMultipleColumn = "PURCHASE ORDER MULTIPLE";
        private const string PurchaseOrderMultipleRoundingColumn = "PURCHASE ORDER MULTIPLE ROUNDING (Nearest, Down, Up)";
        private const string BlockedColumn = "BLOCKED (0, 1)";
        private const string BlockingDateColumn = "BLOCKING DATE";

        internal static string MapPurchaseOrderMultipleRounding(PurchaseOrderMultipleRoundingEnum value)
        {
            switch (value)
            {
                case PurchaseOrderMultipleRoundingEnum.Down:
                    return "Down";

                case PurchaseOrderMultipleRoundingEnum.Up:
                    return "Up";

                default:
                    return "Nearest";
            }
        }

        internal static PurchaseOrderMultipleRoundingEnum MapPurchaseOrderMultipleRounding(string value)
        {
            switch (value)
            {
                case "Down":
                    return PurchaseOrderMultipleRoundingEnum.Down;

                case "Up":
                    return PurchaseOrderMultipleRoundingEnum.Up;

                case "Nearest":
                    return PurchaseOrderMultipleRoundingEnum.Nearest;

                default:
                    throw new Exception(Properties.Resources.InvalidValueInColumn.Replace("#1", PurchaseOrderMultipleRoundingColumn));
            }
        }

        internal static int MapPurchaseOrderReplenishmentBlocking(BlockedForReplenishmentEnum value)
        {
            switch (value)
            {
                case BlockedForReplenishmentEnum.BlockedForReplenishment:
                    return 1;


                default:
                    return 0;
            }
        }

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems)
        {
            ItemReplenishmentSetting setting;
            RecordIdentifier itemID;
            RecordIdentifier storeID;
            bool inserting;
            bool changed;
            string[] mandatoryColumns = new string[] 
            { 
                    ItemIDColumn, ReorderPointColumn, MaximumInventoryColumn, PurchaseOrderMultipleColumn, 
                    PurchaseOrderMultipleRoundingColumn, BlockedColumn
            };
            bool hasBlockingDate = false;

            CheckMandatoryColumns(dt, mandatoryColumns);

            //start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                string itemDescription = row.GetStringValue(ItemDescriptionColumn);
                string variantDescription = row.GetStringValue(VariantDescriptionColumn);
                if (!string.IsNullOrEmpty(variantDescription))
                {
                    itemDescription += " " + variantDescription;
                }
                itemID = row.GetStringValue(ItemIDColumn);

                if (itemID == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", ItemIDColumn),
                        lineNumber, itemDescription));
                    continue;
                }

                if (!CheckMandatoryFields(dt, row, itemID, mandatoryColumns, true, importLogItems, lineNumber, itemDescription))
                {
                    continue;
                }

                try
                {
                    if (!Providers.RetailItemData.Exists(PluginEntry.DataModel, itemID))
                    {
                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.RetailItemDoesNotExist,
                            lineNumber, itemDescription));
                        continue;
                    }
                    else
                    {
                        

                        // figure out if the record exists or if its a new one.
                        storeID = row.GetStringValue(StoreIDColumn);

                        if (storeID == "")
                        {
                            setting = Providers.ItemReplenishmentSettingData.GetForItem(PluginEntry.DataModel, itemID, false);
                        }
                        else
                        {
                            setting = Providers.ItemReplenishmentSettingData.GetItemSettingForStore(PluginEntry.DataModel, itemID,storeID);
                        }

                        if (setting == null)
                        {
                            // We are creating a new replenishment record.
                            inserting = true;

                            setting = new ItemReplenishmentSetting();
                            setting.ItemId = itemID;
                            setting.StoreId = storeID;

                            if (storeID != "")
                            {
                                // We need to check if the store actually exists
                                if (!Providers.StoreData.Exists(PluginEntry.DataModel, storeID))
                                {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.StoreDoesNotExist,
                                        lineNumber, itemDescription));
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            inserting = false;
                        }

                        changed = false;

                        if (setting.ReorderPoint != row.GetDecimalValue(ReorderPointColumn))
                        {
                            setting.ReorderPoint = row.GetDecimalValue(ReorderPointColumn);
                            changed = true;
                        }

                        if(setting.MaximumInventory != row.GetDecimalValue(MaximumInventoryColumn))
                        {
                            setting.MaximumInventory = row.GetDecimalValue(MaximumInventoryColumn);
                            changed = true;
                        }

                        if(setting.PurchaseOrderMultiple != row.GetIntegerValue(PurchaseOrderMultipleColumn))
                        {
                            setting.PurchaseOrderMultiple = row.GetIntegerValue(PurchaseOrderMultipleColumn);
                            changed = true;
                        }

                        var newPurchaseOrderMultipleRoundingValue = MapPurchaseOrderMultipleRounding(row.GetStringValue(PurchaseOrderMultipleRoundingColumn));

                        if (setting.PurchaseOrderMultipleRounding != newPurchaseOrderMultipleRoundingValue)
                        {
                            setting.PurchaseOrderMultipleRounding = newPurchaseOrderMultipleRoundingValue;
                            changed = true;
                        }

                        var newBlockedForReplenishmentValue = (row.GetIntegerValue(BlockedColumn) == 1 ? BlockedForReplenishmentEnum.BlockedForReplenishment : BlockedForReplenishmentEnum.NotBlocked);

                        if (setting.BlockedForReplenishment != newBlockedForReplenishmentValue)
                        {
                            setting.BlockedForReplenishment = newBlockedForReplenishmentValue;
                            changed = true;
                        }

                        if (row[BlockingDateColumn] != DBNull.Value)
                        {
                            DateTime date;
                            if (DateTime.TryParse(row[BlockingDateColumn].ToString(), out date))
                            {

                                hasBlockingDate = true;
                                if (setting.BlockingDate != date)
                                {
                                    setting.BlockingDate = date;
                                    changed = true;
                                }
                            }
                        }

                        if (setting.BlockedForReplenishment == BlockedForReplenishmentEnum.BlockedForReplenishment && !hasBlockingDate)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.WhenBlockedIsActiveThenBlockingDateMustBeSet,
                                lineNumber, itemDescription));
                            continue;
                        }

                        if (changed || inserting)
                        {
                            Providers.ItemReplenishmentSettingData.Save(PluginEntry.DataModel, setting);

                            importLogItems.Add(new ImportLogItem(inserting ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, itemID, storeID == "" ? (string)itemID : (string)itemID + " - " + (string)storeID));
                        }
  
                    }
                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, ex.Message));
                    continue;
                }

                

            }
        }
        
    }
}
