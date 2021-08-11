using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects.Enums;
using System.Linq;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkPriceAndDiscount
{
    public partial class IntegrationFrameworkPriceAndDiscountPlugin
    {
        /// <inheritdoc />
        public void SaveTradeAgreement(TradeAgreementEntry agreement)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    if (RecordIdentifier.IsEmptyOrNull(agreement.UnitID))
                    {
                        agreement.UnitID = Providers.RetailItemData.GetSalesUnitID(dataModel, agreement.ItemRelation);
                    }

                    if (RecordIdentifier.IsEmptyOrNull(agreement.ID))
                    {
                        agreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(dataModel, agreement);
                    }
                    if (agreement.Amount == 0  && agreement.AmountIncludingTax != 0)
                    {
                        RetailItem item = Providers.RetailItemData.Get(dataModel, agreement.ItemRelation);
                        decimal priceWithoutTax = Services.Interfaces.Services.TaxService(dataModel).GetItemPriceForItemPriceWithTax(dataModel, item.SalesTaxItemGroupID, agreement.AmountIncludingTax);
                        agreement.Amount = priceWithoutTax;
                    }
                    else if (agreement.Amount != 0 && agreement.AmountIncludingTax == 0)
                    {
                        RetailItem item = Providers.RetailItemData.Get(dataModel, agreement.ItemRelation);
                        decimal taxAmount = Services.Interfaces.Services.TaxService(dataModel).GetItemTaxForAmount(dataModel, item.SalesTaxItemGroupID, agreement.Amount);

                        agreement.AmountIncludingTax = agreement.Amount + taxAmount;
                    }
                    Providers.TradeAgreementData.Save(dataModel, agreement, Permission.ManageTradeAgreementPrices);
                }

                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        /// <inheritdoc />
        public virtual SaveResult SaveTradeAgreementList(List<TradeAgreementEntry> agreements)
        {
            Action<List<TradeAgreementEntry>> prepareDataForSaveList = (initialAgreements) =>
            {
                IConnectionManager dataModel = GetConnectionManagerIF();

                RecordIdentifier storeTaxGroupID = Providers.StoreData.GetStoresSalesTaxGroupID(dataModel, dataModel.CurrentStoreID);

                List<RecordIdentifier> itemIDs = initialAgreements.Select(x => x.ItemRelation).ToList();

                Dictionary<string, string> taxCodesDictionary = Providers.RetailItemData.GetTaxCodesForItems(dataModel, itemIDs);

                string salesTaxItemGroupID;

                foreach(TradeAgreementEntry agreement in initialAgreements)
                {
                    if (RecordIdentifier.IsEmptyOrNull(agreement.UnitID))
                    {
                        agreement.UnitID = Providers.RetailItemData.GetSalesUnitID(dataModel, agreement.ItemRelation);

                        if (RecordIdentifier.IsEmptyOrNull(agreement.UnitID))
                        {
                            agreement.UnitID = "LS1 DEFAULT";
                        }
                    }

                    if (RecordIdentifier.IsEmptyOrNull(agreement.ID))
                    {
                        agreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(dataModel, agreement);
                    }

                    taxCodesDictionary.TryGetValue((string)agreement.ItemRelation, out salesTaxItemGroupID);

                    if (agreement.Amount == 0 && agreement.AmountIncludingTax != 0)
                    {
                        decimal priceWithoutTax = Services.Interfaces.Services.TaxService(dataModel).GetItemPriceForItemPriceWithTax(dataModel, salesTaxItemGroupID, agreement.AmountIncludingTax);
                        agreement.Amount = priceWithoutTax;
                    }
                    else if (agreement.Amount != 0 && agreement.AmountIncludingTax == 0)
                    {
                        bool useStoreTaxGroup = string.IsNullOrEmpty(salesTaxItemGroupID);
                        decimal taxAmount = Services.Interfaces.Services.TaxService(dataModel).GetItemTaxForAmount(dataModel, useStoreTaxGroup ? (string)storeTaxGroupID : salesTaxItemGroupID, agreement.Amount);
                        agreement.AmountIncludingTax = agreement.Amount + taxAmount;
                    }
                }
            };

            Action<IConnectionManager, TradeAgreementEntry> save = (dataModel, agreement) =>
            {
                Providers.TradeAgreementData.Save(dataModel, agreement, Permission.ManageTradeAgreementPrices);
            };

            Func<TradeAgreementEntry, TradeAgreementEntry, bool> filterById = (agreement1, agreement2) => agreement1.ID == agreement2.ID;
            Func<TradeAgreementEntry, TradeAgreementEntry, bool> filterByOldId = (agreement1, agreement2) => agreement1.OldID == agreement2.OldID;
            Func<TradeAgreementEntry, TradeAgreementEntry, bool> customFilterById = agreements.Any(x => x.ID != RecordIdentifier.Empty) ?
                filterById : filterByOldId;

            return SaveList(agreements, Providers.TradeAgreementData, save, prepareDataForSaveList, customFilterById);
        }

        /// <inheritdoc />
        public void SavePriceDiscountGroup(PriceDiscountGroup priceDiscountGroup)
        {
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    Providers.PriceDiscountGroupData.Save(dataModel, priceDiscountGroup);
                }

                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        /// <inheritdoc />
        public virtual SaveResult SavePriceDiscountGroupList(List<PriceDiscountGroup> priceDiscountGroup)
        {
            return SaveList(priceDiscountGroup, Providers.PriceDiscountGroupData, Providers.PriceDiscountGroupData.Save);
        }

        /// <inheritdoc />
        public PriceDiscountGroup GetPriceDiscountGroup(RecordIdentifier priceDiscountGroupID)
        {
            IConnectionManager dataModel = GetConnectionManagerIF();
            try
            {
                return Providers.PriceDiscountGroupData.Get(dataModel, priceDiscountGroupID);
            }

            finally
            {
                ReturnConnection(dataModel, out dataModel);
            }
        }

        /// <inheritdoc />
        public void DeletePriceDiscountGroupList(List<PriceDiscountGroup> priceDiscountGroup)
        {
            if (priceDiscountGroup == null || priceDiscountGroup.Count == 0)
            {
                return;
            }
            
            IConnectionManager dataModel = GetConnectionManagerIF();
            try
            {
                try
                {
                    foreach (var discountGroup in priceDiscountGroup)
                    {
                        DeletePriceDiscountGroup(discountGroup?.GroupID);
                    }
                }

                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }

        /// <inheritdoc />
        public void DeletePriceDiscountGroup(RecordIdentifier priceDiscountGroupID)
        {
            if (RecordIdentifier.IsEmptyOrNull(priceDiscountGroupID))
            {
                return;
            }
            
            try
            {
                IConnectionManager dataModel = GetConnectionManagerIF();
                try
                {
                    RecordIdentifier groupID = Providers.PriceDiscountGroupData.GetIDFromGroupID(dataModel, priceDiscountGroupID);
                    if (RecordIdentifier.IsEmptyOrNull(groupID))
                    {
                        return; //gracefully exit if there is no such price groups       
                    }
                    
                    PriceDiscountGroup priceDiscountGroup = new PriceDiscountGroup();
                    priceDiscountGroup.GroupID = priceDiscountGroupID;
                    priceDiscountGroup.Module = (PriceDiscountModuleEnum) (int) groupID[0];
                    priceDiscountGroup.Type = (PriceDiscGroupEnum) (int) groupID[1];
                    Providers.PriceDiscountGroupData.Delete(dataModel, priceDiscountGroup);
                }

                finally
                {
                    ReturnConnection(dataModel, out dataModel);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }
        }
    }
}
