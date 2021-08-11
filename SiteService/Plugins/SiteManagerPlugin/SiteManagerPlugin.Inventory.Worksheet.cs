using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Gets the list with the inventory worksheets
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheet> GetInventoryWorksheetList(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseWorksheetData.GetList(entry);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the inventory worksheet for the given inventory template ID and store ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTemplateID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public virtual PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(LogonInfo logonInfo, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTemplateID)}: {inventoryTemplateID}, {nameof(storeID)}: {storeID}");

                return Providers.PurchaseWorksheetData.GetWorksheetFromTemplateIDAndStoreID(entry, inventoryTemplateID, storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the list with the inventory worksheets for the given inventory template ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="inventoryTemplateID"></param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheet> GetInventoryWorksheetListByInventoryTemplate(LogonInfo logonInfo, RecordIdentifier inventoryTemplateID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(inventoryTemplateID)}: {inventoryTemplateID}");

                return Providers.PurchaseWorksheetData.GetListByInventoryTemplate(entry,inventoryTemplateID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns true if an inventory worksheet exists for the given store; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public virtual bool InventoryWorksheetExistsForStore(LogonInfo logonInfo, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");

                return Providers.PurchaseWorksheetData.ExistsForStore(entry,storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the purchase worksheet with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        /// <returns></returns>
        public virtual PurchaseWorksheet GetPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier worksheetId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}");

                return Providers.PurchaseWorksheetData.Get(entry, worksheetId);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the given worksheet
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public virtual RecordIdentifier SaveInventoryWorksheet(LogonInfo logonInfo, PurchaseWorksheet worksheet)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseWorksheetData.Save(entry,worksheet);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes the given worksheet
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        public virtual void DeleteInventoryWorksheet(LogonInfo logonInfo, RecordIdentifier worksheetId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}");

                Providers.PurchaseWorksheetData.Delete(entry,worksheetId);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list with worksheet lines matching the given criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        /// <param name="includeDeletedItems"></param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineListSimple(LogonInfo logonInfo, RecordIdentifier worksheetId, bool includeDeletedItems)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}, {nameof(includeDeletedItems)}: {includeDeletedItems}");

                return Providers.PurchaseWorksheetLineData.GetList(entry,worksheetId,includeDeletedItems);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list with worksheet lines matching the given criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        /// <param name="includeDeletedItems"></param>
        /// <param name="sortEnum"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineList(LogonInfo logonInfo, RecordIdentifier worksheetId, bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum, bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}, {nameof(includeDeletedItems)}: {includeDeletedItems}, {nameof(sortEnum)}: {sortEnum}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.PurchaseWorksheetLineData.GetList(entry,worksheetId,includeDeletedItems,sortEnum,sortBackwards);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes the lines of the given inventory worksheet
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID"></param>
        public virtual void DeleteInventoryWorksheetLineForPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseWorksheetID)}: {purchaseWorksheetID}");

                Providers.PurchaseWorksheetLineData.DeleteForPurchaseWorksheet(entry, purchaseWorksheetID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the inventory worksheet line with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetLineId"></param>
        /// <returns></returns>
        public virtual PurchaseWorksheetLine GetPurchaseWorksheetLine(LogonInfo logonInfo, RecordIdentifier worksheetLineId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetLineId)}: {worksheetLineId}");

                return Providers.PurchaseWorksheetLineData.Get(entry, worksheetLineId);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the given inventory worksheet line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetLine"></param>
        /// <returns></returns>
        public virtual RecordIdentifier SaveInventoryWorksheetLine(LogonInfo logonInfo, PurchaseWorksheetLine worksheetLine)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseWorksheetLineData.Save(entry, worksheetLine);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Get the inventory on hand for an item in inventory unit, including unposted purchase orders and store transfers
        /// </summary>
        /// <param name="entry">The login information for the database</param>
        /// <param name="itemID">ID of the item for which to get the inventory</param>
        /// <param name="storeID">ID of the store for which to get the inventory</param>
        /// <returns></returns>
        public virtual decimal GetEffectiveInventoryForItem(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}");

                return Providers.PurchaseWorksheetLineData.GetEffectiveInventoryForItem(entry, itemID, storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Post a purchase worksheet
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to post</param>
        /// <returns>A container with operation result and number of created purchase orders</returns>
        public virtual PostPurchaseWorksheetContainer PostPurchaseWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            IConnectionManagerTransaction transaction = null;

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseWorksheetID)}: {purchaseWorksheetID}");

                PurchaseWorksheet purchaseWorksheet = Providers.PurchaseWorksheetData.Get(entry, purchaseWorksheetID);
                List<PurchaseWorksheetLine> items = Providers.PurchaseWorksheetLineData.GetList(entry, purchaseWorksheetID, false);

                items.RemoveAll(x => x.IsInventoryExcluded());

                if(items.Count == 0)
                {
                    Utils.Log(this, $"Cannot post purchase worksheet {purchaseWorksheetID} because it contains no items.");
                    return new PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult.NoItems);
                }

                if (items.Any(line => string.IsNullOrEmpty(line.Vendor.ID.StringValue)))
                {
                    Utils.Log(this, $"Cannot post purchase worksheet {purchaseWorksheetID} because it contains items with no vendor.");
                    return new PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult.NonVendorItems);
                }

                InventoryTemplate template = Providers.InventoryTemplateData.Get(entry, purchaseWorksheet.InventoryTemplateID);

                if(template == null)
                {
                    Utils.Log(this, $"Cannot post purchase worksheet {purchaseWorksheetID} because the template was not found.");
                    return new PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult.TemplateNotFound);
                }


                IEnumerable<RecordIdentifier> vendorsInWorksheetIds = items.Select(item => item.Vendor.ID).Distinct();
                List<RecordIdentifier> createdPurchaseOrderIDs = new List<RecordIdentifier>();

                transaction = entry.CreateTransaction();

                if (transaction.ServiceFactory == null)
                {
                    Utils.Log(this, "Service factory is null. Creating service factory.", LogLevel.Trace);
                    transaction.ServiceFactory = new LSOne.DataLayer.GenericConnector.ServiceFactory();
                }

                ITaxService taxService = (ITaxService)transaction.Service(ServiceType.TaxService);

                Utils.Log(this, $"Posting purchase worksheet {purchaseWorksheetID}...");

                foreach (RecordIdentifier vendorId in vendorsInWorksheetIds)
                {
                    IEnumerable<PurchaseWorksheetLine> itemsForVendor = items.Where(item => item.Vendor.ID == vendorId);
                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                    purchaseOrder.Description = template.Text;
                    purchaseOrder.VendorID = (string)vendorId;
                    purchaseOrder.StoreID = purchaseWorksheet.StoreId;
                    purchaseOrder.OrderingDate = Date.Now;
                    purchaseOrder.PurchaseOrderID = (string)DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderData, PurchaseOrder>(transaction);

                    Providers.PurchaseOrderData.Save(transaction, purchaseOrder);

                    createdPurchaseOrderIDs.Add(purchaseOrder.PurchaseOrderID);

                    Vendor vendor = Providers.VendorData.Get(transaction, purchaseOrder.VendorID);

                    foreach (PurchaseWorksheetLine item in itemsForVendor)
                    {
                        if (item.Quantity <= 0) continue;

                        PurchaseOrderLine purchaseOrderItem = new PurchaseOrderLine();
                        RecordIdentifier purchaseOrderUnitId = item.Unit.ID;

                        decimal unitPrice = Providers.VendorItemData.GetDefaultPurchasePrice(transaction, item.Item.ID, vendorId, purchaseOrderUnitId);
                        decimal taxAmount = 0;

                        if (vendor.TaxCalculationMethod != TaxCalculationMethodEnum.NoTax)
                        {
                            purchaseOrderItem.TaxCalculationMethod = vendor.TaxCalculationMethod;
                            RecordIdentifier itemSalesTaxGroupID = Providers.RetailItemData.GetItemsItemSalesTaxGroupID(transaction, item.Item.ID);

                            taxAmount = taxService.GetTaxAmountForPurchaseOrderLine(transaction, itemSalesTaxGroupID, purchaseOrder.VendorID,
                                        purchaseOrder.StoreID, unitPrice, 0, 0, vendor.TaxCalculationMethod);
                        }

                        purchaseOrderItem.UnitPrice = unitPrice;
                        purchaseOrderItem.TaxAmount = taxAmount;
                        purchaseOrderItem.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
                        purchaseOrderItem.LineNumber = (string)DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderLineData, PurchaseOrderLine>(transaction);
                        purchaseOrderItem.ItemID = (string)item.Item.ID;
                        purchaseOrderItem.UnitID = (string)purchaseOrderUnitId;

                        decimal quantityInPurchaseOrderUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(transaction, item.Item.ID, item.Unit.ID, purchaseOrderUnitId, item.Quantity);

                        purchaseOrderItem.Quantity = quantityInPurchaseOrderUnit;

                        Providers.PurchaseOrderLineData.Save(transaction, purchaseOrderItem);

                        List<VendorItem> vendorsForItem = Providers.VendorItemData.GetVendorsForItem(transaction, item.Item.ID, VendorItemSorting.ID, false);

                        VendorItem vendorItem = vendorsForItem.Find(x => x.VendorID == vendor.ID && x.UnitID == item.Unit.ID);

                        if (vendorItem == null || vendorItem.UnitID != item.Unit.ID)
                        {
                            vendorItem = new VendorItem();
                            vendorItem.RetailItemID = item.Item.ID;
                            vendorItem.UnitID = item.Unit.ID;
                            vendorItem.VendorID = vendor.ID;
                            Providers.VendorItemData.Save(transaction, vendorItem);
                        }

                        RetailItem retailItem = Providers.RetailItemData.Get(transaction, item.Item.ID);

                        if (retailItem.DefaultVendorID == "")
                        {
                            retailItem.DefaultVendorID = vendorItem.VendorID;
                            retailItem.Dirty = true;
                            Providers.RetailItemData.Save(transaction, retailItem);
                        }
                    }

                    CreateGoodsReceivingForPurchaseOrderTemplate(transaction, template, purchaseOrder.ID);
                }

                Utils.Log(this, $"Purchase worksheet {purchaseWorksheetID} posted.");
                Utils.Log(this, $"Deleting purchase worksheet lines...");
                Providers.PurchaseWorksheetLineData.DeleteForPurchaseWorksheet(transaction, purchaseWorksheetID);

                transaction.Commit();

                return new PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult.Success, createdPurchaseOrderIDs);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                if(transaction != null)
                {
                    transaction.Rollback();
                }

                return new PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult.Error);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Checks if there is any item in a purchase worksheet that is exluded from inventory operations (ex. Service items)
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet to check</param>
        /// <returns>True if there is an inventory excluded item</returns>
        public virtual bool PurchaseWorksheetHasInventoryExcludedItems(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseWorksheetID)}: {purchaseWorksheetID}");
                return Providers.PurchaseWorksheetLineData.PurchaseWorksheetHasInventoryExcludedItems(entry, purchaseWorksheetID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Creates purchase worksheet lines based on a filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">Purchase worksheet ID</param>
        /// <returns>Number of lines inserted</returns>
        public virtual int CreatePurchaseWorksheetLinesFromFilter(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Creating purchase worksheet lines");

                PurchaseWorksheet pw = Providers.PurchaseWorksheetData.Get(entry, purchaseWorksheetID);

                if (pw == null
                    || RecordIdentifier.IsEmptyOrNull(pw.InventoryTemplateID)
                    || !Providers.InventoryTemplateData.Exists(entry, pw.InventoryTemplateID))
                {
                    Utils.Log(this, $"Purchase worksheet or template {pw?.InventoryTemplateID?.StringValue} not found.");
                    return 0;
                }

                List<InventoryTemplateSectionSelection> filters = Providers.InventoryTemplateSectionSelectionData.GetList(entry, pw.InventoryTemplateID);
                InventoryTemplateFilterContainer filter = new InventoryTemplateFilterContainer(filters);

                if(!filter.HasFilterCriteria())
                {
                    return 0;
                }

                int result = Providers.PurchaseWorksheetData.CreatePurchaseWorksheetLinesFromFilter(entry, purchaseWorksheetID, filter);
                return result;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Refresh the lines in a purchase worksheet by recalculating suggested quantities and adding missing items from the filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetID">ID of the purchase worksheet</param>
        /// <returns></returns>
        public virtual void RefreshPurchaseWorksheetLines(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Refreshing purchase worksheet lines");

                PurchaseWorksheet pw = Providers.PurchaseWorksheetData.Get(entry, purchaseWorksheetID);

                if (pw == null
                    || RecordIdentifier.IsEmptyOrNull(pw.InventoryTemplateID)
                    || !Providers.InventoryTemplateData.Exists(entry, pw.InventoryTemplateID))
                {
                    Utils.Log(this, $"Purchase worksheet or template {pw?.InventoryTemplateID?.StringValue} not found.");
                    return;
                }

                Providers.PurchaseWorksheetData.RefreshPurchaseWorksheetLines(entry, purchaseWorksheetID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Calculate suggested quantity for replenishment for an item in a specific store
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">ID of the item</param>
        /// <param name="storeID">ID of the store</param>
        /// <param name="unitID">ID of the unit in which to convert the result</param>
        /// <returns></returns>
        public virtual decimal CalculateSuggestedQuantity(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Calculating suggested quantity");

                decimal effectiveInventory = Providers.PurchaseWorksheetLineData.GetEffectiveInventoryForItem(entry, itemID, storeID);
                ItemReplenishmentSetting itemReplenishmentSetting = Providers.ItemReplenishmentSettingData.GetItemSettingForStore(entry, itemID, storeID)
                                                                 ?? Providers.ItemReplenishmentSettingData.GetForItem(entry, itemID);

                if(itemReplenishmentSetting == null)
                {
                    return 0;
                }

                decimal suggestedQuantity = Providers.PurchaseWorksheetLineData.CalculateSuggestedQuantity(entry, effectiveInventory, itemReplenishmentSetting);
                return Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, itemID, Providers.InventoryData.GetInventoryUnitId(entry, itemID), unitID, suggestedQuantity);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}