using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using System;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.RetailItems.ViewPages.LogicHandlers
{
    internal class ItemBaseSalesPriceLogic : IMultiEditTabLogicExtension
    {
        RecordIdentifier defaultStoreTaxGroupForMultiEdit;
        RecordIdentifier defaultStoresTaxGroupID;
        RecordIdentifier defaultStoreID;
        bool? calculatePricesWithTax;

        RecordIdentifier oldTaxGroup;

        public static ITabLogic CreateInstance(TabControl.Tab tab)
        {
            return new ItemBaseSalesPriceLogic();
        }

        public void CalculatePriceWithTax(RetailItemMultiEdit item, RecordIdentifier salesTaxGroupID)
        {
            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            if (defaultStoreTaxGroupForMultiEdit == null)
            {
                defaultStoreTaxGroupForMultiEdit = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);
            }

            item.SalesPriceIncludingTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceWithTax(
                PluginEntry.DataModel,
                item.SalesPrice,
                salesTaxGroupID,
                defaultStoreTaxGroupForMultiEdit,
                true,
                item.SalesPriceIncludingTax,
                priceLimiter);
        }

        public void MultiEditPostSave(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            RetailItem item = (RetailItem)dataEntity;

            // See if price recalculation is triggered because of changed taxgroup, we want to do this before we evaluate price changes
            // since in price changes the user might make explicit choice to put the sales price with tax to 10 $ which would mean the tax calculation will
            // happen backwards in such cases but in normal cases it would calculate it on top of the price without tax.
            if ((item as RetailItemMultiEdit).MustRecalculatePrices)
            {
                if (defaultStoresTaxGroupID == null)
                {
                    defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(threadedConnection);
                }

                if(calculatePricesWithTax == null)
                {
                    if (defaultStoreID == null)
                    {
                        defaultStoreID = Providers.StoreData.GetDefaultStoreID(threadedConnection);
                    }

                    calculatePricesWithTax = Services.Interfaces.Services.TaxService(threadedConnection).PricesAreCalculatedWithTax(threadedConnection, defaultStoreID);
                }


                item.ID = primaryRecordID.SecondaryID;
                item.MasterID = primaryRecordID.PrimaryID;
                Services.Interfaces.Services.TaxService(threadedConnection).UpdateTradeAgreementsAndPromotionsForItem(threadedConnection, item, (bool)calculatePricesWithTax, defaultStoresTaxGroupID);
                item.ID = RecordIdentifier.Empty;
                item.MasterID = null;
            }
        }

        public void MultiEditPreSave(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            RetailItem item = (RetailItem)dataEntity;

            // See if price recalculation is triggered because of changed taxgroup, we want to do this before we evaluate price changes
            // since in price changes the user might make explicit choice to put the sales price with tax to 10 $ which would mean the tax calculation will
            // happen backwards in such cases but in normal cases it would calculate it on top of the price without tax.
            if ((item as RetailItemMultiEdit).MustRecalculatePrices)
            {
                if (((item as RetailItemMultiEdit).FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPrice) != RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)
                {
                    if ((item as RetailItemMultiEdit).OldPrices == null)
                    {
                        (item as RetailItemMultiEdit).OldPrices = Providers.RetailItemData.GetItemPrice(threadedConnection, primaryRecordID.PrimaryID);
                    }

                    item.SalesPrice = (item as RetailItemMultiEdit).OldPrices.SalesPrice;
                }

                if (((item as RetailItemMultiEdit).FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax) != RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)
                {
                    if ((item as RetailItemMultiEdit).OldPrices == null)
                    {
                        (item as RetailItemMultiEdit).OldPrices = Providers.RetailItemData.GetItemPrice(threadedConnection, primaryRecordID.PrimaryID);

                    }

                    item.SalesPriceIncludingTax = (item as RetailItemMultiEdit).OldPrices.SalesPriceIncludingTax;
                }

                if (defaultStoresTaxGroupID == null)
                {
                    defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(threadedConnection);
                }
                
                if (calculatePricesWithTax == null)
                {
                    if (defaultStoreID == null)
                    {
                        defaultStoreID = Providers.StoreData.GetDefaultStoreID(threadedConnection);
                    }
                    
                    calculatePricesWithTax = Services.Interfaces.Services.TaxService(threadedConnection).PricesAreCalculatedWithTax(threadedConnection, defaultStoreID);
                }

                if (Services.Interfaces.Services.TaxService(threadedConnection).CalculateNewItemPrice(threadedConnection, item, (bool)calculatePricesWithTax, defaultStoresTaxGroupID))
                {
                    (item as RetailItemMultiEdit).FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPrice;
                    (item as RetailItemMultiEdit).FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax;

                    if ((item as RetailItemMultiEdit).OldPrices == null)
                    {
                        (item as RetailItemMultiEdit).OldPrices = Providers.RetailItemData.GetItemPrice(threadedConnection, primaryRecordID.PrimaryID);
                    }

                    item.ProfitMargin = CalculateProfitMargin(item.SalesPrice, (((item as RetailItemMultiEdit).FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice) == RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice) ? item.PurchasePrice : (item as RetailItemMultiEdit).OldPrices.PurchasePrice);

                    (item as RetailItemMultiEdit).FieldSelection |= RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin;

                    Providers.RetailItemMultiEditData.UpdatePriceFields(item as RetailItemMultiEdit);
                }
            }
        }

        public void PreSave(IConnectionManager entry, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            RetailItem item = (RetailItem)dataEntity;

            if(oldTaxGroup != item.SalesTaxItemGroupID)
            {
                // Tax group has changed so we need to recalculate prices

                if (defaultStoresTaxGroupID == null)
                {
                    defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);
                }

                if (calculatePricesWithTax == null)
                {
                    if (defaultStoreID == null)
                    {
                        defaultStoreID = Providers.StoreData.GetDefaultStoreID(entry);
                    }

                    calculatePricesWithTax = Services.Interfaces.Services.TaxService(entry).PricesAreCalculatedWithTax(entry, defaultStoreID);
                }

                Services.Interfaces.Services.TaxService(entry).CalculateNewItemPrice(entry, item, (bool)calculatePricesWithTax, defaultStoresTaxGroupID);
                Services.Interfaces.Services.TaxService(entry).UpdateTradeAgreementsAndPromotionsForItem(entry, item, (bool)calculatePricesWithTax, defaultStoresTaxGroupID);
                item.ProfitMargin = CalculateProfitMargin(item.SalesPrice, item.PurchasePrice);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RetailItemTaxGroupWasChangedPricesRecalculated", item.ID, null);

            }

            oldTaxGroup = item.SalesTaxItemGroupID;
        }

        public decimal CalculateProfitMargin(decimal salesPrice, decimal costPrice)
        {
            if (salesPrice == 0) return 0;
            return ((salesPrice - costPrice) / salesPrice) * 100;
        }

        public void PostLoad(IConnectionManager entry, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            oldTaxGroup = (dataEntity as RetailItem).SalesTaxItemGroupID;
        }
    }
}
