using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.Services.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.ExcelFiles.Properties;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class RetailItemMapper : MapperBase
    {
        private const string ItemIDColumn = "ITEMID";
        private const string ItemDescriptionColumn = "DESCRIPTION";
        private const string VariantDescriptionColumn = "VARIANT DESCRIPTION";

        private static Dictionary<string, Guid> groupMasterIDs = null;
        private static Dictionary<string, Guid> departmentMasterIDs = null;
        private static Dictionary< Guid, string> groupIDs = null;
        private static Dictionary< Guid,string> departmentIDs = null;

        private static void GenerateHierarchyCache()
        {
            groupIDs = new Dictionary<Guid, string>();
            departmentIDs = new Dictionary<Guid, string>();
            groupMasterIDs = new Dictionary<string, Guid>();
            departmentMasterIDs = new Dictionary<string, Guid>();


            var retailGroups = Providers.RetailGroupData.GetDetailedList(PluginEntry.DataModel, RetailGroupSorting.RetailGroupName,
                false);
            foreach (var retailGroup in retailGroups)
            {
                groupMasterIDs.Add( (string)retailGroup.ID,(Guid)retailGroup.MasterID);
                groupIDs.Add( (Guid)retailGroup.MasterID, (string)retailGroup.ID);
            }

            var retaildepartments = Providers.RetailDepartmentData.GetDetailedList(PluginEntry.DataModel,
                RetailDepartment.SortEnum.RetailDepartment, false);

            foreach (var retailDepartment in retaildepartments)
            {
                departmentMasterIDs.Add( (string)retailDepartment.ID,(Guid)retailDepartment.MasterID);
                departmentIDs.Add( (Guid)retailDepartment.MasterID, (string)retailDepartment.ID);
            }
        }

        private static void SetItemRetailGroup(RetailItem item, RecordIdentifier retailGroupID)
        {

            if (!Providers.RetailGroupData.Exists(PluginEntry.DataModel, retailGroupID))
            {
                RetailGroup group = new RetailGroup();
                group.ID = retailGroupID;
                group.Text = (string)retailGroupID;

                Providers.RetailGroupData.Save(PluginEntry.DataModel, group);
                GenerateHierarchyCache();
            }
            item.RetailGroupMasterID = groupMasterIDs[(string)retailGroupID];
        }

        private static void SetItemHeaderID(RetailItem item, string headerItemID)
        {
            if (string.IsNullOrEmpty(headerItemID))
            {
                return;
            }

            if (!Providers.RetailItemData.Exists(PluginEntry.DataModel, headerItemID))
            {
                RetailItem parentItem = new RetailItem();
                parentItem.ID = headerItemID;
                parentItem.Text = item.Text;
                parentItem.ItemType = DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem;
                Providers.RetailItemData.Save(PluginEntry.DataModel, parentItem);

                item.HeaderItemID = parentItem.MasterID;
            }
            else
            {
                item.HeaderItemID = 
                    Providers.RetailItemData.GetMasterIDFromItemID(PluginEntry.DataModel, headerItemID);
            }
        }

        private static void SetRetailDepartment(RetailItem item, RecordIdentifier departmentID)
        {
            if ((string)departmentID != string.Empty && !Providers.RetailDepartmentData.Exists(PluginEntry.DataModel, departmentID))
            {
                RetailDepartment department = new RetailDepartment();
                department.ID = departmentID;
                department.Text = (string)departmentID;

                Providers.RetailDepartmentData.Save(PluginEntry.DataModel, department);
                GenerateHierarchyCache();
            }
            item.RetailDepartmentMasterID = departmentMasterIDs[(string)departmentID];
        }

        internal static RetailItem.KeyInSerialNumberEnum MapKeyInSerialNumber(string value)
        {
            switch (value)
            {
                case "Not mandatory":
                    return RetailItem.KeyInSerialNumberEnum.NotMandatory;

                case "Must key in serial number":
                    return RetailItem.KeyInSerialNumberEnum.MustKeyInSerialNumber;

                case "Never key in serial number":
                    return RetailItem.KeyInSerialNumberEnum.Never;

                default:
                    return RetailItem.KeyInSerialNumberEnum.Never;
            }
        }

        internal static string MapKeyInSerialNumber(RetailItem.KeyInSerialNumberEnum value)
        {
            switch (value)
            {
                case RetailItem.KeyInSerialNumberEnum.NotMandatory:
                    return "Not mandatory";

                case RetailItem.KeyInSerialNumberEnum.MustKeyInSerialNumber:
                    return "Must key in serial number";

                case RetailItem.KeyInSerialNumberEnum.Never:
                    return "Never key in serial number";

                default:
                    return "Never key in serial number";
            }
        }

        internal static string MapKeyInPrice(RetailItem.KeyInPriceEnum value)
        {
            switch (value)
            {
                case RetailItem.KeyInPriceEnum.NotMandatory:
                    return "Not mandatory";

                case RetailItem.KeyInPriceEnum.MustKeyInNewPrice:
                    return "Must key in new price";

                case RetailItem.KeyInPriceEnum.MustKeyInHigherEqualPrice:
                    return "Must key in higher/equal price";

                case RetailItem.KeyInPriceEnum.MustKeyInLowerEqualPrice:
                    return "Must key in lower/equal price";

                case RetailItem.KeyInPriceEnum.MustNotKeyInNewPrice:
                    return "Must not key in new price";

                default:
                    return "Not mandatory";
            }
        }

        internal static RetailItem.KeyInPriceEnum MapKeyInPrice(string excelValue)
        {
            switch (excelValue)
            {
                case "Not mandatory":
                    return RetailItem.KeyInPriceEnum.NotMandatory;

                case "Must key in new price":
                    return RetailItem.KeyInPriceEnum.MustKeyInNewPrice;

                case "Must key in higher/equal price":
                    return RetailItem.KeyInPriceEnum.MustKeyInHigherEqualPrice;

                case "Must key in lower/equal price":
                    return RetailItem.KeyInPriceEnum.MustKeyInLowerEqualPrice;

                case "Must not key in new price":
                    return RetailItem.KeyInPriceEnum.MustNotKeyInNewPrice;

                default:
                    return RetailItem.KeyInPriceEnum.NotMandatory;
            }
        }

        internal static RetailItem.KeyInQuantityEnum MapKeyInQuantity(string excelValue)
        {
            switch (excelValue)
            {
                case "Not mandatory":
                    return RetailItem.KeyInQuantityEnum.NotMandatory;

                case "Must key in quantity":
                    return RetailItem.KeyInQuantityEnum.MustKeyInQuantity;

                case "Must not key in quantity":
                    return RetailItem.KeyInQuantityEnum.MustNotKeyInQuantity;

                default:
                    return RetailItem.KeyInQuantityEnum.NotMandatory;
            }
        }

        internal static string MapKeyInQuantity(RetailItem.KeyInQuantityEnum value)
        {
            switch (value)
            {
                case RetailItem.KeyInQuantityEnum.MustKeyInQuantity:
                    return "Must key in quantity";

                case RetailItem.KeyInQuantityEnum.MustNotKeyInQuantity:
                    return "Must not key in quantity";

                default:
                    return "Not mandatory";
            }
        }

        internal static ItemTypeEnum MapItemType(string excelValue)
        {
            switch (excelValue)
            {
                case "Retail":
                    return ItemTypeEnum.Item;
                case "Service":
                    return ItemTypeEnum.Service;
                case "Header":
                    return ItemTypeEnum.MasterItem;
                case "Assembly":
                    return ItemTypeEnum.AssemblyItem;
                default:
                    return ItemTypeEnum.Item;
            }
        }

        internal static string MapItemType(ItemTypeEnum value)
        {
            switch (value)
            {
                case ItemTypeEnum.Item:
                default:
                    return "Retail";
                case ItemTypeEnum.Service:
                    return "Service";
                case ItemTypeEnum.MasterItem:
                    return "Header";
                case ItemTypeEnum.AssemblyItem:
                    return "Assembly";
            }
        }

        internal static string MapDimensionAttributesFromItem(RetailItem item)
        {
            if(item.ItemType == ItemTypeEnum.MasterItem)
            {
                var dimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, item.MasterID);
                
                if(dimensions != null && dimensions.Any())
                {
                    return string.Join(";", dimensions.Select(x => x.Text));
                }
            }
            else if(item.IsVariantItem)
            {
                var attributeRelations = Providers.DimensionAttributeData.GetRetailItemDimensionAttributeRelations(PluginEntry.DataModel, item.HeaderItemID, false);
                var attributes = attributeRelations.FirstOrDefault(x => x.Key.PrimaryID == item.MasterID);

                if(attributes.Value != null && attributes.Value.Any())
                {
                    return string.Join(";", attributes.Value.Select(x => x.Text));
                }
            }

            return "";
        }

        private static void CalculateProfitMargin(RetailItem item)
        {
            if (item.SalesPrice < item.PurchasePrice)
            {
                item.ProfitMargin = 0;
                item.Dirty = true;
            }
            else
            {
                if (item.SalesPrice != 0)
                {
                    item.ProfitMargin = ((item.SalesPrice - item.PurchasePrice) / item.SalesPrice) * 100;
                    item.Dirty = true;
                }

            }
        }

        private static void CalculatePrices(RetailItem item, bool pricesAreWithTax, decimal price)
        {
            decimal priceWithTax;
            decimal priceWithoutTax;

            if (pricesAreWithTax)
            {
                priceWithTax = price;

                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                priceWithoutTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceFromPriceWithTax(
                    PluginEntry.DataModel,
                    priceWithTax,
                    item.SalesTaxItemGroupID,
                    defaultStoreTaxGroup);
            }
            else
            {
                priceWithoutTax = price;

                DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

                priceWithTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceWithTax(
                    PluginEntry.DataModel,
                    priceWithoutTax,
                    item.SalesTaxItemGroupID,
                    defaultStoreTaxGroup,
                    false,
                    0.0m,
                    priceLimiter);
            }

            item.SalesPrice = priceWithoutTax;
            item.SalesPriceIncludingTax = priceWithTax;
        }

        private static void CreateVendor(RecordIdentifier vendorID)
        {
            Vendor vendor = new Vendor();
            vendor.ID = vendorID;
            vendor.Text = (string)vendorID;

            Providers.VendorData.Save(PluginEntry.DataModel, vendor);
        }

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems, MergeModeEnum mergeMode, bool calculateProfitMargins, char dimensionAttributeSeparator)
        {
            RetailItem item;
            BarCode barCode = null;
            RecordIdentifier oldBarCodeID;
            RecordIdentifier itemID;
            VendorItem vendorItem;
            bool barCodeDirty;
            bool vendorItemDirty;
            bool inserting;
            bool tamperedWithUnits;
            bool tamperedWithSalesTaxGroup;
            bool itemExisted;
            RecordIdentifier defaultStoreID;
            bool pricesAreWithTax = false;
            bool costOrSalesPriceChanged;
            bool addedRetailItem = false;
            GenerateHierarchyCache();
            defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

            if (defaultStoreID != null && defaultStoreID != "")
            {
                pricesAreWithTax = Providers.StoreData.GetPriceWithTaxForStore(PluginEntry.DataModel, defaultStoreID);
            }

            //Backwards compatibility
            string vendorPurchasePriceColumn = dt.Columns.Contains("VENDOR PRICE") ? "VENDOR PRICE" : "VENDOR PURCHASE PRICE";

            // Check that mandatory and semi mandatory columns exits
            CheckMandatoryColumns(dt, new string[] { "ITEMID", "DESCRIPTION", "INVENTORY UNIT", "SALES UNIT", "RETAIL GROUP", "SALES TAX GROUP", "SALES PRICE" });

            // Start importing
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
                if(itemID == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", "ITEMID"),
                        lineNumber, itemDescription));

                    continue;
                }

                inserting = false;
                tamperedWithUnits = false;
                tamperedWithSalesTaxGroup = false;
                barCodeDirty = false;
                oldBarCodeID = RecordIdentifier.Empty;
                itemExisted = false;
                vendorItem = null;
                vendorItemDirty = false;

                costOrSalesPriceChanged = false;

                try
                {
                    if (Providers.RetailItemData.Exists(PluginEntry.DataModel, itemID))
                    {
                        if (Providers.RetailItemData.IsDeleted(PluginEntry.DataModel, itemID))
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.ImportPreviousItemDeleted, lineNumber, itemDescription));
                            continue;
                        }

                        if (mergeMode == MergeModeEnum.InsertIfNotExists)
                        {
                            continue;
                        }

                        item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
                        item.Dirty = false;
                        itemExisted = true;
                    }
                    else
                    {
                        item = new RetailItem();
                        item.ID = itemID;
                        item.Dirty = true;
                        inserting = true;
                    }

                    DimensionMapper dimensionMapper = null;

                    if (mergeMode == MergeModeEnum.Merge && !inserting)
                    {
                        // We need to do merge, thus only change if something changed
                        if (!CheckMandatoryFields(dt, row, itemID, new string[] { "DESCRIPTION", "INVENTORY UNIT", "SALES UNIT", "RETAIL GROUP" }, true, importLogItems,
                            lineNumber, itemDescription))
                        {
                            continue;
                        }

                        if (!(string.IsNullOrEmpty(row["DESCRIPTION"].ToString())))
                        {
                            if (row.GetStringValue("DESCRIPTION") != item.Text)
                            {
                                item.Text = row.GetStringValue("DESCRIPTION");
                                item.Dirty = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["INVENTORY UNIT"].ToString())))
                        {
                            if (row.GetStringValue("INVENTORY UNIT") != item.InventoryUnitID)
                            {
                                item.InventoryUnitID = row.GetStringValue("INVENTORY UNIT");
                                item.Dirty = true;
                                tamperedWithUnits = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["SALES UNIT"].ToString())))
                        {
                            if (row.GetStringValue("SALES UNIT") != item.SalesUnitID)
                            {
                                item.SalesUnitID = row.GetStringValue("SALES UNIT");
                                item.Dirty = true;
                                tamperedWithUnits = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["SEARCH ALIAS"].ToString())))
                        {
                            if (row.GetStringValue("SEARCH ALIAS") != item.NameAlias)
                            {
                                item.NameAlias = row.GetStringValue("SEARCH ALIAS");
                                item.Dirty = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["NOTES"].ToString())))
                        {
                            if (row.GetStringValue("NOTES") != item.ExtendedDescription)
                            {
                                item.ExtendedDescription = row.GetStringValue("NOTES");
                                item.Dirty = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["RETAIL GROUP"].ToString())))
                        {
                            string groupID = groupIDs.ContainsKey((Guid) item.RetailGroupMasterID)
                                ? groupIDs[(Guid) item.RetailGroupMasterID]
                                : string.Empty;
                            if (row.GetStringValue("RETAIL GROUP") != groupID)
                            {
                                SetItemRetailGroup(item, row.GetStringValue("RETAIL GROUP"));
                                item.Dirty = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["SALES TAX GROUP"].ToString())))
                        {
                            if (row.GetStringValue("SALES TAX GROUP") != item.SalesTaxItemGroupID)
                            {
                                item.SalesTaxItemGroupID = row.GetStringValue("SALES TAX GROUP");
                                item.Dirty = true;
                                tamperedWithSalesTaxGroup = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["COST PRICE"].ToString())))
                        {
                            if (row.GetDecimalValue("COST PRICE") != item.PurchasePrice)
                            {
                                item.PurchasePrice = row.GetDecimalValue("COST PRICE");
                                item.Dirty = true;

                                costOrSalesPriceChanged = true;
                            }
                        }

                        if (!(string.IsNullOrEmpty(row["SALES PRICE"].ToString())))
                        {
                            if ((pricesAreWithTax && row.GetDecimalValue("SALES PRICE") != item.SalesPriceIncludingTax) ||
                                (!pricesAreWithTax && row.GetDecimalValue("SALES PRICE") != item.SalesPrice))
                            {
                                CalculatePrices(item, pricesAreWithTax, row.GetDecimalValue("SALES PRICE"));

                                item.Dirty = true;

                                costOrSalesPriceChanged = true;
                            }
                        }

                        if (costOrSalesPriceChanged && calculateProfitMargins)
                        {
                            CalculateProfitMargin(item);
                        }

                        if (dt.Columns.Contains("SCALE ITEM") && !(string.IsNullOrEmpty(row["SCALE ITEM"].ToString())))
                        {
                            if ((row.GetIntegerValue("SCALE ITEM") != 0) != item.ScaleItem)
                            {
                                item.ScaleItem = row.GetIntegerValue("SCALE ITEM") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("ZERO PRICE VALID") && !(string.IsNullOrEmpty(row["ZERO PRICE VALID"].ToString())))
                        {
                            if ((row.GetIntegerValue("ZERO PRICE VALID") != 0) != item.ZeroPriceValid)
                            {
                                item.ZeroPriceValid = row.GetIntegerValue("ZERO PRICE VALID") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("NO DISCOUNT ALLOWED") && !(string.IsNullOrEmpty(row["NO DISCOUNT ALLOWED"].ToString())))
                        {
                            if ((row.GetIntegerValue("NO DISCOUNT ALLOWED") != 0) != item.NoDiscountAllowed)
                            {
                                item.NoDiscountAllowed = row.GetIntegerValue("NO DISCOUNT ALLOWED") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("MUST SELECT UNIT") && !(string.IsNullOrEmpty(row["MUST SELECT UNIT"].ToString())))
                        {
                            if ((row.GetIntegerValue("MUST SELECT UNIT") != 0) != item.MustSelectUOM)
                            {
                                item.MustSelectUOM = row.GetIntegerValue("MUST SELECT UNIT") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("MUST KEY IN COMMENT") && !(string.IsNullOrEmpty(row["MUST KEY IN COMMENT"].ToString())))
                        {
                            if ((row.GetIntegerValue("MUST KEY IN COMMENT") != 0) != item.MustKeyInComment)
                            {
                                item.MustKeyInComment = row.GetIntegerValue("MUST KEY IN COMMENT") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("QTY BECOMES NEGATIVE") && !(string.IsNullOrEmpty(row["QTY BECOMES NEGATIVE"].ToString())))
                        {
                            if ((row.GetIntegerValue("QTY BECOMES NEGATIVE") != 0) != item.QuantityBecomesNegative)
                            {
                                item.QuantityBecomesNegative = row.GetIntegerValue("QTY BECOMES NEGATIVE") != 0;
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("KEY IN PRICE") && !(string.IsNullOrEmpty(row["KEY IN PRICE"].ToString())))
                        {
                            if (MapKeyInPrice(row.GetStringValue("KEY IN PRICE")) != item.KeyInPrice)
                            {
                                item.KeyInPrice = MapKeyInPrice(row.GetStringValue("KEY IN PRICE"));
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("KEY IN QUANTITY") && !(string.IsNullOrEmpty(row["KEY IN QUANTITY"].ToString())))
                        {
                            if (MapKeyInQuantity(row.GetStringValue("KEY IN QUANTITY")) != item.KeyInQuantity)
                            {
                                item.KeyInQuantity = MapKeyInQuantity(row.GetStringValue("KEY IN QUANTITY"));
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("KEY IN SERIAL NUMBER") && !(string.IsNullOrEmpty(row["KEY IN SERIAL NUMBER"].ToString())))
                        {
                            if (MapKeyInSerialNumber(row.GetStringValue("KEY IN SERIAL NUMBER")) != item.KeyInSerialNumber)
                            {
                                item.KeyInSerialNumber = MapKeyInSerialNumber(row.GetStringValue("KEY IN SERIAL NUMBER"));
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("TARE WEIGHT") && !(string.IsNullOrEmpty(row["TARE WEIGHT"].ToString())))
                        {
                            if (row.GetDecimalValue("TARE WEIGHT") != item.TareWeight)
                            {
                                item.TareWeight = row.GetIntegerValue("TARE WEIGHT");
                                item.Dirty = true;
                            }
                        }

                        if (dt.Columns.Contains("BARCODE") && !(string.IsNullOrEmpty(row["BARCODE"].ToString())))
                        {
                            string barCodeString = row.GetStringValue("BARCODE");
                            if (barCodeString != "")
                            {
                                // We need to fetch bar code on the item
                                barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, item.ID);
                                oldBarCodeID = barCode != null ? barCode.ItemBarCode : RecordIdentifier.Empty;

                                if (oldBarCodeID != barCodeString)
                                {
                                    barCodeDirty = true;
                                }

                                if (barCode == null)
                                {
                                    barCode = new BarCode();
                                }

                                barCode.ItemBarCode = barCodeString;
                                barCode.ShowForItem = true;
                                barCode.ItemID = item.ID;
                            }

                            if (dt.Columns.Contains("BARCODESETUP") && !(string.IsNullOrEmpty(row["BARCODESETUP"].ToString())) && barCode != null)
                            {
                                if (!RecordIdentifier.IsEmptyOrNull(barCode.BarCodeSetupID))
                                {
                                    if (row.GetStringValue("BARCODESETUP") != barCode.BarCodeSetupID)
                                    {
                                        barCodeDirty = true;
                                        barCode.BarCodeSetupID = row.GetStringValue("BARCODESETUP");
                                    }
                                }
                            }

                            if (dt.Columns.Contains("BARCODEUNIT") && !(string.IsNullOrEmpty(row["BARCODEUNIT"].ToString())) && barCode != null)
                            {
                                if (!RecordIdentifier.IsEmptyOrNull(barCode.UnitID))
                                {
                                    if (row.GetStringValue("BARCODEUNIT") != barCode.UnitID)
                                    {
                                        barCodeDirty = true;
                                        barCode.UnitID = row.GetStringValue("BARCODEUNIT");
                                    }
                                }
                            }
                        }

                        if (dt.Columns.Contains("VENDORID") && !(string.IsNullOrEmpty(row["VENDORID"].ToString())))
                        {
                            var vendorID = row.GetStringValue("VENDORID");
                            string vendoritemID = string.Empty;

                            if (dt.Columns.Contains("VENDOR ITEMID") && !(string.IsNullOrEmpty(row["VENDOR ITEMID"].ToString())))
                            {
                                vendoritemID = row.GetStringValue("VENDOR ITEMID");
                            }

                            if (!Providers.VendorData.Exists(PluginEntry.DataModel, vendorID))
                            {
                                CreateVendor(vendorID);
                            }

                            vendorItem = Providers.VendorItemData.GetVendorForItem(PluginEntry.DataModel, item.ID, vendorID);

                            if (vendorItem == null)
                            {
                                vendorItem = new VendorItem();
                                vendorItem.VendorID = vendorID;
                                vendorItem.VendorItemID = vendoritemID;
                                vendorItem.RetailItemID = item.ID;
                                vendorItem.UnitID = item.InventoryUnitID;
                                vendorItemDirty = true;
                            }
                            else
                            {
                                var vendorItemID = row.GetStringValue("VENDOR ITEMID");
                                if (vendorItem.VendorItemID != vendorItemID)
                                {
                                    vendorItem.VendorItemID = vendoritemID;
                                    vendorItemDirty = true;
                                }
                            }

                            if (!(string.IsNullOrEmpty(row[vendorPurchasePriceColumn].ToString())))
                            {
                                var rowVendorPrice = row.GetDecimalValue(vendorPurchasePriceColumn);

                                if (rowVendorPrice != vendorItem.DefaultPurchasePrice)
                                {
                                    vendorItem.DefaultPurchasePrice = rowVendorPrice;
                                    vendorItemDirty = true;
                                }

                                if (vendorItem.LastItemPrice == 0 && rowVendorPrice != vendorItem.LastItemPrice)
                                {
                                    vendorItem.LastItemPrice = rowVendorPrice;
                                    vendorItemDirty = true;
                                }
                            }
                            
                            item.DefaultVendorID = vendorID;
                            item.Dirty = true;
                        }

                        if (item.IsVariantItem && dt.Columns.Contains("VARIANT DESCRIPTION") && !(string.IsNullOrEmpty(row["VARIANT DESCRIPTION"].ToString())))
                        {
                            if (row.GetStringValue("VARIANT DESCRIPTION") != item.VariantName)
                            {
                                string newVariantDescription = row.GetStringValue("VARIANT DESCRIPTION");
                                if (String.IsNullOrEmpty(newVariantDescription))
                                {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.MandatoryFieldMissing.Replace("#1", "VARIANT DESCRIPTION"), lineNumber, itemDescription));
                                    continue;
                                }

                                item.VariantName = newVariantDescription;
                                item.Dirty = true;
                            }
                        }
                    }
                    else
                    {
                        // We just do dumb insert since we are either inserting new or in overide mode
                        if (!CheckMandatoryFields(dt, row, itemID, new string[] { "DESCRIPTION", "INVENTORY UNIT", "SALES UNIT", "RETAIL GROUP" }, inserting, importLogItems,
                                lineNumber, itemDescription))
                        {
                            continue;
                        }

                        if (!(string.IsNullOrEmpty(row["DESCRIPTION"].ToString())))
                        {
                            item.Text = row.GetStringValue("DESCRIPTION");
                            item.Dirty = true;
                        }

                        if (!(string.IsNullOrEmpty(row["SEARCH ALIAS"].ToString())))
                        {
                            item.NameAlias = row.GetStringValue("SEARCH ALIAS");
                            item.Dirty = true;
                        }
                        else
                        {
                            item.NameAlias = "";
                            item.Dirty = true;
                        }

                        if (!(string.IsNullOrEmpty(row["NOTES"].ToString())))
                        {
                            item.ExtendedDescription = row.GetStringValue("NOTES");
                            item.Dirty = true;
                        }
                        else
                        {
                            item.ExtendedDescription = "";
                            item.Dirty = true;
                        }


                        if (!(string.IsNullOrEmpty(row["INVENTORY UNIT"].ToString())))
                        {
                            item.InventoryUnitID = row.GetStringValue("INVENTORY UNIT");
                            item.Dirty = true;
                            tamperedWithUnits = true;
                        }

                        if (!(string.IsNullOrEmpty(row["SALES UNIT"].ToString())))
                        {
                            item.SalesUnitID = row.GetStringValue("SALES UNIT");
                            item.Dirty = true;
                            tamperedWithUnits = true;
                        }

                        SetItemRetailGroup(item, row.GetStringValue("RETAIL GROUP")); // mandatory columns been verified above to not be DBNull
                        
                        if (!(string.IsNullOrEmpty(row["SALES TAX GROUP"].ToString())))
                        {
                            item.SalesTaxItemGroupID = row.GetStringValue("SALES TAX GROUP");
                            item.Dirty = true;

                            tamperedWithSalesTaxGroup = true;
                        }

                        if (!(string.IsNullOrEmpty(row["COST PRICE"].ToString())))
                        {
                            item.PurchasePrice = row.GetDecimalValue("COST PRICE");
                            item.Dirty = true;

                            costOrSalesPriceChanged = true;
                        }
                        else
                        {
                            item.PurchasePrice = 0.0m;
                            item.Dirty = true;

                            costOrSalesPriceChanged = true;
                        }

                        if (!(string.IsNullOrEmpty(row["SALES PRICE"].ToString())))
                        {
                            CalculatePrices(item, pricesAreWithTax, row.GetDecimalValue("SALES PRICE"));
                            item.Dirty = true;
                            costOrSalesPriceChanged = true;
                        }

                        if (costOrSalesPriceChanged && calculateProfitMargins)
                        {
                            CalculateProfitMargin(item);
                        }
                         
                        if (row.Table.Columns.Contains("SCALE ITEM") && !(string.IsNullOrEmpty(row["SCALE ITEM"].ToString())))
                        {
                            item.ScaleItem = row.GetIntegerValue("SCALE ITEM") != 0;
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("ZERO PRICE VALID") && !(string.IsNullOrEmpty(row["ZERO PRICE VALID"].ToString())))
                        {
                            item.ZeroPriceValid = row.GetIntegerValue("ZERO PRICE VALID") != 0;
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("NO DISCOUNT ALLOWED") && !(string.IsNullOrEmpty(row["NO DISCOUNT ALLOWED"].ToString())))
                        {
                            item.NoDiscountAllowed = row.GetIntegerValue("NO DISCOUNT ALLOWED") != 0;
                            item.Dirty = true;
                        }


                         if (row.Table.Columns.Contains("MUST SELECT UNIT") && !(string.IsNullOrEmpty(row["MUST SELECT UNIT"].ToString())))
                        {
                            item.MustSelectUOM = row.GetIntegerValue("MUST SELECT UNIT") != 0;
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("MUST KEY IN COMMENT") && !(string.IsNullOrEmpty(row["MUST KEY IN COMMENT"].ToString())))
                        {
                            item.MustKeyInComment = row.GetIntegerValue("MUST KEY IN COMMENT") != 0;
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("QTY BECOMES NEGATIVE") && !(string.IsNullOrEmpty(row["QTY BECOMES NEGATIVE"].ToString())))
                        {
                            item.QuantityBecomesNegative = row.GetIntegerValue("QTY BECOMES NEGATIVE") != 0;
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("KEY IN PRICE") && !(string.IsNullOrEmpty(row["KEY IN PRICE"].ToString())))
                        {
                            item.KeyInPrice = MapKeyInPrice(row.GetStringValue("KEY IN PRICE"));
                            item.Dirty = true;
                        }

                         if (row.Table.Columns.Contains("KEY IN QUANTITY") && !(string.IsNullOrEmpty(row["KEY IN QUANTITY"].ToString())))
                        {
                            item.KeyInQuantity = MapKeyInQuantity(row.GetStringValue("KEY IN QUANTITY"));
                            item.Dirty = true;
                        }

                        if (row.Table.Columns.Contains("KEY IN SERIAL NUMBER") && !(string.IsNullOrEmpty(row["KEY IN SERIAL NUMBER"].ToString())))
                        {
                            item.KeyInSerialNumber = MapKeyInSerialNumber(row.GetStringValue("KEY IN SERIAL NUMBER"));
                            item.Dirty = true;
                        }

                        if (row.Table.Columns.Contains("TARE WEIGHT") && !(string.IsNullOrEmpty(row["TARE WEIGHT"].ToString())))
                        {
                            item.TareWeight = row.GetIntegerValue("TARE WEIGHT");
                            item.Dirty = true;
                        }

                        if (row.Table.Columns.Contains("BARCODE") && !(string.IsNullOrEmpty(row["BARCODE"].ToString())))
                        {

                            var barcodeVessel = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, item.ID);
                            if (barcodeVessel != null)
                            {
                                oldBarCodeID = barcodeVessel.ItemBarCode;
                            }
                            string barCodeString = row.GetStringValue("BARCODE");
                            if (barCodeString != "")
                            {
                                barCode = new BarCode();
                                barCode.ItemBarCode = barCodeString;
                                barCode.ItemID = item.ID;
                                barCode.ShowForItem = true;
                                barCodeDirty = true;
                            }

                            if (row.Table.Columns.Contains("BARCODESETUP") && !(string.IsNullOrEmpty(row["BARCODESETUP"].ToString())) && barCode != null)
                            {
                                barCodeDirty = true;
                                barCode.BarCodeSetupID = row.GetStringValue("BARCODESETUP");
                            }

                            if (row.Table.Columns.Contains("BARCODEUNIT") && !(string.IsNullOrEmpty(row["BARCODEUNIT"].ToString())) && barCode != null)
                            {
                                barCodeDirty = true;
                                barCode.UnitID = row.GetStringValue("BARCODEUNIT");
                            }
                        }

                        if (row.Table.Columns.Contains("VENDORID") && !(string.IsNullOrEmpty(row["VENDORID"].ToString())))
                        {
                            var vendorID = row.GetStringValue("VENDORID");

                            if (inserting)
                            {
                                vendorItem = new VendorItem();
                            }
                            else
                            {
                                vendorItem = Providers.VendorItemData.GetVendorForItem(PluginEntry.DataModel, item.ID, vendorID);
                            }

                            vendorItem.VendorID = vendorID;
                            vendorItem.VendorItemID = (row["VENDOR ITEMID"] != DBNull.Value) ? row.GetStringValue("VENDOR ITEMID") : "";
                            vendorItem.RetailItemID = item.ID;

                            if(inserting)
                            { 
                                vendorItem.UnitID = item.InventoryUnitID;
                            }
                            
                            if (!(string.IsNullOrEmpty(row[vendorPurchasePriceColumn].ToString())))
                            {
                                var rowVendorPrice = row.GetDecimalValue(vendorPurchasePriceColumn);

                                vendorItem.DefaultPurchasePrice = rowVendorPrice;
                                vendorItem.LastItemPrice = rowVendorPrice;
                            }

                            item.DefaultVendorID = vendorItem.VendorID;
                            vendorItemDirty = true;
                        }

                        if(inserting && row.Table.Columns.Contains("ITEM TYPE") && !(string.IsNullOrEmpty(row["ITEM TYPE"].ToString())))
                        {
                            item.ItemType = MapItemType(row.GetStringValue("ITEM TYPE"));
                            item.Dirty = true;
                        }

                        if (inserting && item.ItemType == ItemTypeEnum.Item && !(string.IsNullOrEmpty(row["VARIANT HEADER ID"].ToString())))
                        {
                            string newVariantHeaderItemID = row.GetStringValue("VARIANT HEADER ID");
                            
                            if (Providers.RetailItemData.Exists(PluginEntry.DataModel, newVariantHeaderItemID))
                            {
                                item.HeaderItemID = Providers.RetailItemData.GetMasterIDFromItemID(PluginEntry.DataModel, newVariantHeaderItemID);
                            }
                            else
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.HeaderItemNotFound, lineNumber, itemDescription));
                                continue;
                            }
                        }

                        if(row.Table.Columns.Contains("DIMENSIONS/ATTRIBUTES") && !(string.IsNullOrEmpty(row["DIMENSIONS/ATTRIBUTES"].ToString())))
                        {
                            if (item.ItemType == ItemTypeEnum.MasterItem)
                            {
                                dimensionMapper = DimensionMapper.PrepareDimensions(row.GetStringValue("DIMENSIONS/ATTRIBUTES"), item.MasterID, dimensionAttributeSeparator);
                            }
                            else if (inserting && item.IsVariantItem)
                            {
                                dimensionMapper = DimensionMapper.PrepareAttributes(row.GetStringValue("DIMENSIONS/ATTRIBUTES"), item.HeaderItemID, dimensionAttributeSeparator);
                            }
                            else
                            {
                                if(!inserting)
                                {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.DimensionsAndAttributesWhenUpdating, lineNumber, itemDescription));
                                }
                                else
                                {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.DimensionsAndAttributesMustBeDefinedForVariants, lineNumber, itemDescription));
                                }
                                continue;
                            }

                            if(dimensionMapper != null && !dimensionMapper.IsValid)
                            {
                                switch (dimensionMapper.Error)
                                {
                                    case DimensionMapper.ErrorCode.None:
                                    default:
                                        break;
                                    case DimensionMapper.ErrorCode.DimensionsAlreadyDefined:
                                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.DimensionsAlreadyDefined, lineNumber, itemDescription));
                                        break;
                                    case DimensionMapper.ErrorCode.AttributeCountMismatch:
                                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.AttributeCountMismatch, lineNumber, itemDescription));
                                        break;
                                    case DimensionMapper.ErrorCode.AttributeCombinationAlreadyExists:
                                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.AttributeCombinationAlreadyExists, lineNumber, itemDescription));
                                        break;
                                    case DimensionMapper.ErrorCode.AttributesCannotBeEmpty:
                                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.AttributesCannotBeEmpty, lineNumber, itemDescription));
                                        break;
                                }
                                continue;
                            }
                        }
                        else if(inserting && item.IsVariantItem)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Resources.MandatoryFieldMissing.Replace("#1", "DIMENSIONS/ATTRIBUTES"), lineNumber, itemDescription));
                            continue;
                        }

                        if (item.IsVariantItem)
                        {
                            if(dt.Columns.Contains("VARIANT DESCRIPTION") && !(string.IsNullOrEmpty(row["VARIANT DESCRIPTION"].ToString())) && row.GetStringValue("VARIANT DESCRIPTION") != item.VariantName)
                            {
                                item.VariantName = row.GetStringValue("VARIANT DESCRIPTION");
                                item.Dirty = true;
                            }

                            if(string.IsNullOrWhiteSpace(item.VariantName))
                            {
                                string attributesVariantName = dimensionMapper?.GetVariantNameFromAttributes();

                                if(!string.IsNullOrWhiteSpace(attributesVariantName) && item.VariantName != attributesVariantName)
                                {
                                    item.VariantName = attributesVariantName;
                                    item.Dirty = true;
                                }
                            }

                            if(string.IsNullOrWhiteSpace(item.VariantName))
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", "VARIANT DESCRIPTION"), lineNumber, itemDescription));
                                continue;
                            }
                        }
                    }

                    // Post processing ----------------------------------------------------------------------------------------------------------------------
                    if (tamperedWithUnits)
                    {
                        Unit salesUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.SalesUnitID);

                        if (salesUnit == null)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.SalesUnitNotFound, 
                                lineNumber, itemDescription));
                            continue;
                        }

                        //If the casing is not matched, use the one from DB
                        if (string.Compare(item.SalesUnitID.StringValue, salesUnit.ID.StringValue, false) != 0)
                        {
                            item.SalesUnitID = salesUnit.ID;
                        }
                        

                        Unit inventoryUnit = Providers.UnitData.Get(PluginEntry.DataModel, item.InventoryUnitID);

                        if (inventoryUnit == null)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.InventoryUnitNotFound,
                                lineNumber, itemDescription));
                            continue;
                        }

                        //If the casing is not matched, use the one from DB
                        if (string.Compare(item.InventoryUnitID.StringValue, inventoryUnit.ID.StringValue, false) != 0)
                        {
                            item.InventoryUnitID = inventoryUnit.ID;
                        }

                        if (item.InventoryUnitID != item.SalesUnitID)
                        {
                            if (!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel, RecordIdentifier.Empty, item.InventoryUnitID, item.SalesUnitID))
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.UnitConversionRuleFromInventoryToSalesDoesNotExist,
                                    lineNumber, itemDescription));
                                continue;
                            }
                        }
                    }

                    // See if we need to copy default values from the retail group
                    if (item.RetailGroupMasterID != RecordIdentifier.Empty)
                    {
                        RetailGroup retailGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, item.RetailGroupMasterID);

                        if (retailGroup != null)
                        {
                            if (item.RetailDepartmentMasterID == "" && retailGroup.RetailDepartmentMasterID != "")
                            {
                                item.RetailDepartmentMasterID = retailGroup.RetailDepartmentMasterID;
                                item.Dirty = true;
                            }

                            if (item.SalesTaxItemGroupID == "" && retailGroup.ItemSalesTaxGroupId != "")
                            {
                                item.SalesTaxItemGroupID = retailGroup.ItemSalesTaxGroupId;
                                item.Dirty = true;
                                tamperedWithSalesTaxGroup = true;
                            }
                        }
                    }

                    if (tamperedWithSalesTaxGroup)
                    {
                        if (item.SalesTaxItemGroupID != "")
                        {
                            if (!Providers.ItemSalesTaxGroupData.Exists(PluginEntry.DataModel, item.SalesTaxItemGroupID))
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.TaxGroupDoesNotExist,
                                    lineNumber, itemDescription));
                                continue;
                            }
                        }
                    }

                    if (barCodeDirty && barCode != null)
                    {
                        // Validate Barcode
                        if (!RecordIdentifier.IsEmptyOrNull(barCode.BarCodeSetupID))
                        {
                            // See if BarCode setup exists
                            if (!Providers.BarCodeSetupData.Exists(PluginEntry.DataModel, barCode.BarCodeSetupID))
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.BarCodeSetupDoesNotExist,
                                    lineNumber, itemDescription));
                                continue;
                            }

                            BarCodeSetup barCodeSetup = Providers.BarCodeSetupData.Get(PluginEntry.DataModel, barCode.BarCodeSetupID);

                            if (barCodeSetup.MaximumLength > 0)
                            {
                                if (((string)barCode.ItemBarCode).Length > barCodeSetup.MaximumLength && barCodeSetup.BarCodeMask.Length != barCodeSetup.BarCodeMask.Count(c => c == 'X'))
                                    {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.BarCodeIsLongerThanAllowedByBarCodeSetup,
                                        lineNumber, itemDescription));
                                    continue;
                                }
                            }

                            if (barCodeSetup.MinimumLength > 0)
                            {
                                if (((string)barCode.ItemBarCode).Length < barCodeSetup.MinimumLength && barCodeSetup.BarCodeMask.Length != barCodeSetup.BarCodeMask.Count(c => c == 'X'))
                                {
                                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.BarCodeIsShorterThanAllowedByBarCodeSetup,
                                        lineNumber, itemDescription));
                                    continue;
                                }
                            }
                        }

                        if (!RecordIdentifier.IsEmptyOrNull(barCode.UnitID))
                        {
                            if(!Providers.UnitConversionData.UnitConversionRuleExists(PluginEntry.DataModel,itemExisted ? item.ID : "",item.InventoryUnitID,barCode.UnitID))
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.UnitConversionRuleFromBarCodeToInventoryUnitDoesNotExist,
                                    lineNumber, itemDescription));
                                continue;
                            }
                        }
                    } // End of validating bar code

                    if (vendorItem != null && vendorItemDirty)
                    {
                        if (!Providers.VendorData.Exists(PluginEntry.DataModel, vendorItem.VendorID))
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.VendorDoesNotExits,
                                lineNumber, itemDescription));
                        }
                    }

                    // End of post processing ----------------------------------------------------------------------------------------------------------------------

                    bool wasDirty = item.Dirty;

                    // Saving
                    Providers.RetailItemData.Save(PluginEntry.DataModel, item);
                    dimensionMapper?.Save(item.MasterID);

                    if (barCodeDirty && barCode != null)
                    {
                        if (oldBarCodeID != RecordIdentifier.Empty && oldBarCodeID != "")
                        {
                            BarCode oldBarcode = Providers.BarCodeData.Get(PluginEntry.DataModel, oldBarCodeID);

                            if(oldBarcode != null)
                            {
                                barCode.ItemBarcodeID = oldBarcode.ItemBarcodeID;
                            }
                        }
                        
                        Providers.BarCodeData.Save(PluginEntry.DataModel, barCode);                       
                    } 

                    if (vendorItemDirty && vendorItem != null)
                    {
                        // Because of the complexity of the VendorItems then we cannot just blindly save here because VendorItemID
                        // is not really the records primary key so we need to poke around a bit.
                        VendorItem tmp = Providers.VendorItemData.Get(PluginEntry.DataModel, vendorItem.VendorID, vendorItem.VendorItemID);

                        if (tmp != null)
                        {
                            vendorItem.ID = tmp.ID;
                        }

                        Providers.VendorItemData.Save(PluginEntry.DataModel, vendorItem);

                        if (wasDirty && !RecordIdentifier.IsEmptyOrNull(item.DefaultVendorID))
                        {
                            Providers.RetailItemData.SetItemsDefaultVendor(PluginEntry.DataModel, item.MasterID, item.DefaultVendorID);
                        }
                    }

                    if (wasDirty || (barCodeDirty && barCode != null) || (vendorItemDirty && vendorItem != null))
                    {
                        if(inserting)
                        {
                            addedRetailItem = true;
                        }

                        importLogItems.Add(new ImportLogItem(inserting ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, itemID, "", 
                            lineNumber, itemDescription));
                    }
                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, ex.Message));
                    continue;
                }
            }

            if(addedRetailItem)
            {
                // Notify others that we added retail items. (This for example might be nice for the dashboard to know)
                // Here we opted to use old style plugin messaging instead of the new style, the old style is more suitable when its just a notification
                // and not a well defined operation that we want to run
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null,"RetailItemNotifications",null);
                
                if(plugin != null)
                {
                    plugin.Message(null, "NotifyRetailItem", new object[] { DataEntityChangeType.MultiAdd });
                }
            }
        }
    }
}
