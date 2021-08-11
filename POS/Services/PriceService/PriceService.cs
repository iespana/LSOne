using System;
using System.Collections.Generic;
using System.Linq;

using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Price;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
	public partial class PriceService : IPriceService
	{
		public PriceService()
		{
		}

		public void Init(IConnectionManager entry)
		{
			ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

#pragma warning disable 0612, 0618
			DLLEntry.DataModel = entry;
			DLLEntry.Settings = settings;
#pragma warning restore 0612, 0618
		}

		public virtual IErrorLog ErrorLog
		{
			set
			{

			}
		}

		/// <summary>
		/// ***********************************************************************************
		/// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
		/// ***********************************************************************************
		/// </summary>
		public virtual void SetPrice(IConnectionManager entry, IRetailTransaction retailTransaction, CacheType cacheType)
		{
			if (retailTransaction.SaleItems.Count > 0)
			{
				SaleLineItem saleItem = (SaleLineItem)retailTransaction.SaleItems.Last.Value;

				if (!saleItem.PriceInBarcode)
				{
					if (SpecialPriceCase(saleItem)) return;

					decimal quantity = GetQuantity(retailTransaction, saleItem);
					TradeAgreementPriceInfo tradeAgreementPriceInfo = GetPrice(entry, 
											 saleItem, 
											 saleItem.Dimension.VariantNumber, 
											 retailTransaction.Customer.ID, 
											 retailTransaction.StoreId, 
											 retailTransaction.StoreCurrencyCode, 
											 saleItem.SalesOrderUnitOfMeasure, 
											 retailTransaction.Hospitality.ActiveHospitalitySalesType, 
											 saleItem.TaxIncludedInItemPrice, 
											 quantity, 
											 DateTime.Now, 
											 DateTime.Now, 
											 cacheType);

					//Update all item with the item id with this price
					// This applies to all identical items except those who have had their price overridden and items returned with a receipt.
					foreach (SaleLineItem saleLineItem in retailTransaction.SaleItems)
					{
						if (saleLineItem.ItemId == saleItem.ItemId
                            && !saleLineItem.NoPriceCalculation
							&& !saleLineItem.PriceOverridden
							&& !saleLineItem.ReceiptReturnItem
							&& saleLineItem.Dimension.ColorID.PrimaryID == saleItem.Dimension.ColorID.PrimaryID
							&& saleLineItem.Dimension.SizeID.PrimaryID == saleItem.Dimension.SizeID.PrimaryID
							&& saleLineItem.Dimension.StyleID.PrimaryID == saleItem.Dimension.StyleID.PrimaryID
							&& saleLineItem.SalesOrderUnitOfMeasure == saleItem.SalesOrderUnitOfMeasure
							&& !saleLineItem.Voided
							&& !saleLineItem.UnitOfMeasureChanged
							&& tradeAgreementPriceInfo.Price != null
                            && saleItem.ShouldCalculateAndDisplayAssemblyPrice())
						{
							SetTaxPrice(saleLineItem, tradeAgreementPriceInfo);
							saleLineItem.WasChanged = true;
						}                        
					}
				}
			}
		}

		/// <summary>
		/// ***********************************************************************************
		/// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
		/// ***********************************************************************************
		/// </summary>
		public virtual IRetailTransaction UpdateAllPrices(IConnectionManager entry, IRetailTransaction retailTransaction, bool restoreItemPrices, CacheType cacheType)
		{
			foreach (SaleLineItem saleItem in retailTransaction.SaleItems.Where(x => !x.ReceiptReturnItem && x.ShouldCalculateAndDisplayAssemblyPrice()))
			{
				if (saleItem is SaleLineItem)
				{
					retailTransaction = UpdatePrice(entry, retailTransaction, saleItem, restoreItemPrices, cacheType);
				}
			}

			return retailTransaction;
		}

		public virtual IRetailTransaction UpdatePrice(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem, bool restoreItemPrices, CacheType cacheType)
		{
			SaleLineItem saleItem = saleLineItem as SaleLineItem;

			if (saleItem == null) return retailTransaction;

			if (!saleItem.PriceInBarcode)
			{
				decimal quantity = GetQuantity(retailTransaction, saleItem);

				decimal priceBefore = GetTaxPrice(saleItem);
				TradeAgreementPriceInfo tradeAgreementPriceInfo = GetPrice(entry,
											saleItem,
											saleItem.Dimension.VariantNumber,
											retailTransaction.Customer.ID,
											retailTransaction.StoreId,
											retailTransaction.StoreCurrencyCode,
											saleItem.SalesOrderUnitOfMeasure,
											retailTransaction.Hospitality.ActiveHospitalitySalesType,
											saleItem.TaxIncludedInItemPrice,
											quantity,
											DateTime.Now,
											DateTime.Now,
											cacheType);
				//restores item prices if...
				//1.) A customer action has been involved AND a PRICE    has been overridden
				//2.) A unit-of-measure - change has been applied to the item
				//
				//If a customer has been added or cleared from a transaction, then "restoreItemPrices" is set to "true".

				if (!saleItem.NoPriceCalculation && restoreItemPrices && !saleItem.PriceOverridden && tradeAgreementPriceInfo.Price != null)
				{
					saleItem.TaxIncludedInItemPrice = retailTransaction.TaxIncludedInPrice;
					SetTaxPrice(saleItem, tradeAgreementPriceInfo);
				}
				else if(!saleItem.PriceOverridden)
				{
					SetTaxPrice(saleItem, tradeAgreementPriceInfo);
				}

				if (tradeAgreementPriceInfo.Price != priceBefore)
				{
					saleItem.WasChanged = true;
				}
			}

			return retailTransaction;
		}

		/// <summary>
		/// ***********************************************************************************
		/// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
		/// ***********************************************************************************
		/// </summary>
		public virtual TradeAgreementPriceInfo GetPrice(IConnectionManager entry,
								SaleLineItem item,
								RecordIdentifier variantID,
								RecordIdentifier customerID,
								RecordIdentifier storeID,
								RecordIdentifier currencyCodeID,
								RecordIdentifier unitID,
								RecordIdentifier salesTypeID,
								bool calculateWithTax,
								decimal quantity,
								DateTime fromDate,
								DateTime toDate,
								CacheType cacheType)
		{
			var retailItem = Providers.RetailItemData.Get(entry, item.ItemId, CacheType.CacheTypeTransactionLifeTime);

			if (retailItem == null)
			{
				return new TradeAgreementPriceInfo();
			}

			Currency companyCurrency = Providers.CurrencyData.GetCompanyCurrency(entry, CacheType.CacheTypeApplicationLifeTime);
			RecordIdentifier companyCurrencyID = companyCurrency == null ? currencyCodeID : companyCurrency.ID;

			RecordIdentifier baseSalesUOM = retailItem.SalesUnitID;

			if (unitID == RecordIdentifier.Empty)
			{
				unitID = baseSalesUOM;
			}

			decimal itemCardPrice = !item.IsAssembly ? retailItem.SalesPrice : item.ItemAssembly.Price;
			decimal itemCardPriceWithTax = !item.IsAssembly ? retailItem.SalesPriceIncludingTax : item.ItemAssembly.Price;
			decimal baseItemPrice;
			if (calculateWithTax)
			{
				baseItemPrice = itemCardPriceWithTax + retailItem.SalesMarkup;
			}
			else
			{
				baseItemPrice = itemCardPrice + retailItem.SalesMarkup;
			}
			return processPrice(entry, baseItemPrice, customerID, storeID, currencyCodeID, unitID, salesTypeID, companyCurrencyID, baseSalesUOM, item.ItemId, calculateWithTax, quantity, fromDate, toDate, cacheType);
		}

		/// <summary>
		/// ***********************************************************************************
		/// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
		/// ***********************************************************************************
		/// </summary>
		public virtual TradeAgreementPriceInfo GetPrice(IConnectionManager entry,
								RecordIdentifier itemID,
								RecordIdentifier variantID,
								RecordIdentifier customerID,
								RecordIdentifier storeID,
								RecordIdentifier currencyCodeID,
								RecordIdentifier unitID,
								RecordIdentifier salesTypeID,
								bool calculateWithTax,
								decimal quantity,
								DateTime fromDate,
								DateTime toDate,
								CacheType cacheType)
		{

			var item = Providers.RetailItemData.Get(entry, itemID, CacheType.CacheTypeTransactionLifeTime);

			if (item == null)
			{
				return new TradeAgreementPriceInfo();
			}

			Currency companyCurrency = Providers.CurrencyData.GetCompanyCurrency(entry, cacheType);
			var companyCurrencyID = companyCurrency == null ? currencyCodeID : companyCurrency.ID;

			var baseSalesUOM = item.SalesUnitID;

			if (unitID == RecordIdentifier.Empty)
			{
				unitID = item.SalesUnitID;
			}

			decimal itemCardPrice = item.SalesPrice;
			decimal itemCardPriceWithTax = item.SalesPriceIncludingTax;
			decimal baseItemPrice;
			if (calculateWithTax)
			{
				baseItemPrice = itemCardPriceWithTax + item.SalesMarkup;
			}
			else
			{
				baseItemPrice = itemCardPrice + item.SalesMarkup;
			}

			return processPrice(entry, baseItemPrice, customerID, storeID, currencyCodeID, unitID, salesTypeID, companyCurrencyID, baseSalesUOM, itemID, calculateWithTax, quantity, fromDate, toDate, cacheType);
		}

		public virtual TradeAgreementPriceInfo GetPrice(
								IConnectionManager entry, 
								RecordIdentifier itemID,
								RecordIdentifier variantID,
								RecordIdentifier customerID,
								RecordIdentifier storeID,
								RecordIdentifier currencyCodeID,
								RecordIdentifier unitID,
								RecordIdentifier salesTypeID,
								bool calculateWithTax,
								Decimal quantity,
								CacheType cacheType)
		{
			return GetPrice(entry, itemID, variantID, customerID, storeID, currencyCodeID, unitID, salesTypeID, calculateWithTax, quantity, DateTime.Now, DateTime.Now, cacheType);
		}

		private TradeAgreementPriceInfo processPrice(IConnectionManager entry,
									 decimal baseItemPrice,
									 RecordIdentifier customerID,
									 RecordIdentifier storeID,
									 RecordIdentifier currencyCodeID,
									 RecordIdentifier unitID,
									 RecordIdentifier salesTypeID,
									 RecordIdentifier companyCurrencyID,
									 RecordIdentifier salesUnitOfMeasure,
									 RecordIdentifier itemID,
									 bool calculateWithTax,
									 Decimal quantity,
									 DateTime fromDate,
									 DateTime toDate,
									 CacheType cacheType)
		{
			var conversionFactor = GetUnitAndCurrencyFactor(entry, itemID, companyCurrencyID, currencyCodeID, salesUnitOfMeasure, unitID, cacheType);

            baseItemPrice = baseItemPrice * conversionFactor;

			List<PromotionOfferLine> promotionLinesForItem = Providers.DiscountOfferLineData.GetValidAndEnabledPromotionsForItem(entry, itemID, storeID, customerID);

			TradeAgreementPriceInfo bestTradeAgreementPrice = FindBestTradeAgreementPrice(entry, 
																						  itemID, 
																						  salesUnitOfMeasure, 
																						  customerID, 
																						  storeID, 
																						  currencyCodeID, 
																						  companyCurrencyID, 
																						  unitID,  
																						  salesTypeID, 
																						  calculateWithTax, 
																						  quantity, 
																						  fromDate, 
																						  toDate, 
																						  cacheType);

			TradeAgreementPriceInfo bestPromotionPrice = FindBestPromotionPrice(promotionLinesForItem,
																				conversionFactor,
																				calculateWithTax,
																				entry, 
																				currencyCodeID,
																				cacheType);

			TradeAgreementPriceInfo basePrice = new TradeAgreementPriceInfo
			{
				Price = baseItemPrice
			};

            TradeAgreementPriceInfo bestPromotionDiscountPercentage = FindBestPromotionDiscounts(promotionLinesForItem);
			decimal promotionDiscountMultiplier = 1;

			if (!bestPromotionDiscountPercentage.IsEmpty)
			{
				promotionDiscountMultiplier = (decimal)bestPromotionDiscountPercentage.DiscountPercentage / 100;
			}

            TradeAgreementPriceInfo basePriceWithDiscount = new TradeAgreementPriceInfo
            {
                Price = baseItemPrice - baseItemPrice * promotionDiscountMultiplier,
                PriceType = bestPromotionDiscountPercentage.IsEmpty ? TradeAgreementPriceType.BasePrice : bestPromotionDiscountPercentage.PriceType,
                PriceID = bestPromotionDiscountPercentage.IsEmpty ? RecordIdentifier.Empty : bestPromotionDiscountPercentage.PriceID
            };

			TradeAgreementPriceInfo bestTradeAgreementPriceWithDiscount = bestTradeAgreementPrice.Clone();
			if (bestTradeAgreementPrice.Price.HasValue)
			{
                bestTradeAgreementPriceWithDiscount.Price -= (decimal)bestTradeAgreementPrice.Price * promotionDiscountMultiplier;
			}

			TradeAgreementPriceInfo tradeAgreementPriceInfo = new TradeAgreementPriceInfo();

			// Now we have all the prices needed. Time to return the correct one.
			if (bestTradeAgreementPrice.Price == null && bestPromotionPrice.Price == null && bestPromotionDiscountPercentage.DiscountPercentage == null) return basePrice;
			if (bestTradeAgreementPrice.Price == null && bestPromotionPrice.Price == null && bestPromotionDiscountPercentage.DiscountPercentage != null) return basePriceWithDiscount;
			if (bestTradeAgreementPrice.Price == null && bestPromotionPrice.Price != null && bestPromotionDiscountPercentage.DiscountPercentage == null) return BestTradeAgreement(basePrice, bestPromotionPrice);
			if (bestTradeAgreementPrice.Price != null && bestPromotionPrice.Price == null && bestPromotionDiscountPercentage.DiscountPercentage == null) return bestTradeAgreementPrice;
			if (bestTradeAgreementPrice.Price == null && bestPromotionPrice.Price != null && bestPromotionDiscountPercentage.DiscountPercentage != null) return BestTradeAgreement(basePriceWithDiscount, bestPromotionPrice);
			if (bestTradeAgreementPrice.Price != null && bestPromotionPrice.Price == null && bestPromotionDiscountPercentage.DiscountPercentage != null) return bestTradeAgreementPriceWithDiscount;
			if (bestTradeAgreementPrice.Price != null && bestPromotionPrice.Price != null && bestPromotionDiscountPercentage.DiscountPercentage == null) return BestTradeAgreement(bestPromotionPrice, bestTradeAgreementPrice);
			if (bestTradeAgreementPrice.Price != null && bestPromotionPrice.Price != null && bestPromotionDiscountPercentage.DiscountPercentage != null) return BestTradeAgreement(bestPromotionPrice, bestTradeAgreementPriceWithDiscount);

			return tradeAgreementPriceInfo;
		}

		private TradeAgreementPriceInfo BestTradeAgreement(TradeAgreementPriceInfo agreementOne,
			TradeAgreementPriceInfo agreementTwo)
		{
			if (agreementOne.Price > agreementTwo.Price)
			{
				return agreementTwo;
			}
			return agreementOne;
		}

		protected virtual TradeAgreementPriceInfo FindBestTradeAgreementPrice(
			IConnectionManager entry, 
			RecordIdentifier itemID, 
			RecordIdentifier baseUnitOfMeasureID,
			RecordIdentifier customerID, 
			RecordIdentifier storeID,
			RecordIdentifier currencyCode,
			RecordIdentifier companyCurrencyID,
			RecordIdentifier unitID,
			RecordIdentifier salesTypeID,
			bool calculateWithTax,
			decimal quantity,
			DateTime fromDate,
			DateTime toDate,
			CacheType cacheType)
		{
			ITradeAgreementData tradeAgreementData = DataProviderFactory.Instance.Get<ITradeAgreementData, TradeAgreementEntry>();
			// Get settings that can shut down trade agreement prices
			DiscountAndPriceActivation discountsActive = Providers.DiscountAndPriceActivationData.Get(entry, cacheType) ?? new DiscountAndPriceActivation();

			List<TradeAgreementEntry> listOfTradeAgreements = new List<TradeAgreementEntry>();

			Customer customer = null;
			if (customerID != null && !customerID.IsEmpty && customerID != "")
			{
				customer = DataProviderFactory.Instance.Get<ICustomerData, Customer>().Get(entry, customerID, UsageIntentEnum.Minimal, cacheType);
			}

			// Add TA that belong to the item. This deals with TA that are assigned to all items and between customers and items. This does not deal with any groups, we do that later
			List<TradeAgreementEntry> allTradeAgreementsForItem = tradeAgreementData.GetForItem(entry, itemID, TradeAgreementRelation.PriceSales, discountsActive.PriceCustomerItem, customer != null ? customer.ID : "");

			listOfTradeAgreements.AddRange(allTradeAgreementsForItem);

			if (discountsActive.PriceCustomerItem)
			{
				// Add TA that belong to the customer. Here we only deal with customers getting All items. We have already dealt with item-customer TA's and will deal with groups later
				var allTradeAgreementsForCustomer = tradeAgreementData.GetForCustomer(entry, customerID, TradeAgreementRelation.PriceSales, discountsActive.PriceAllCustomersItem);

				listOfTradeAgreements.AddRange(allTradeAgreementsForCustomer);
			}

			// Get taEntries for the stores price groups. We loop through the price groups based on their level to the store and if we find TA's in any of them we add them and stop searching.
			// We only want to add the taEntries that have the correct ItemRelation.
			List<StoreInPriceGroup> priceGroupsForStore = DataProviderFactory.Instance.Get<IPriceDiscountGroupData, PriceDiscountGroup>().GetPriceGroupsForStore(entry, storeID, cacheType).OrderBy(x => x.Level).ToList();
			foreach (StoreInPriceGroup storeInPriceGroup in priceGroupsForStore)
			{
				List<TradeAgreementEntry> allTradeAgreementForStorePriceGroup = tradeAgreementData.GetForGroup(entry, storeInPriceGroup.PriceGroupID, TradeAgreementRelation.PriceSales, itemID);
				if (allTradeAgreementForStorePriceGroup != null && allTradeAgreementForStorePriceGroup.Count > 0)
				{
					listOfTradeAgreements.AddRange(allTradeAgreementForStorePriceGroup);
					break;
				}
			}

			if (discountsActive.PriceCustomerGroupItem)
			{
				// Get taEntries for the customers price group. We only want to add the taEntries that have the correct ItemRelation
				if (customer != null && customer.PriceGroupID != RecordIdentifier.Empty)
				{
					List<TradeAgreementEntry> allTradeAgreementsForCustomerPriceGroup = tradeAgreementData.GetForGroup(entry, customer.PriceGroupID, TradeAgreementRelation.PriceSales, itemID);

					listOfTradeAgreements.AddRange(allTradeAgreementsForCustomerPriceGroup);
				}

				if (salesTypeID != null && salesTypeID != RecordIdentifier.Empty && salesTypeID != "")
				{
					SalesType salesType = DataProviderFactory.Instance.Get<ISalesTypeData, SalesType>().Get(entry, salesTypeID);
					if (salesType != null && salesType.PriceGroup != "")
					{
						List<TradeAgreementEntry> allTradeAgreementsForSalesTypeID = tradeAgreementData.GetForGroup(entry, salesType.PriceGroup, TradeAgreementRelation.PriceSales, itemID);

						listOfTradeAgreements.AddRange(allTradeAgreementsForSalesTypeID);
					}
				}
			}

			// Now we have all the trade agreements possible for our variables, but now we must filter them down because they might have
			// - wrong currency code (can only be the supplied currency code or company currency code)
			// - unit that needs to be converted (can only be supplied unit or item's base unit)
			// - invalid validation period
			// - not enough quantity to activate TA price
			// - incorrect variant (if a variant has been supplied)      

			List<TradeAgreementEntry> filteredTAList = (from ta in listOfTradeAgreements
													 where
													 (ta.FromDate.DateTime <= fromDate.Date || ta.FromDate == Date.Empty)
													 // Note that ToDate on the trade agreement is midnight at the start of the last day
													 // Add 24 hours to include the entire last day
													 && (ta.ToDate.DateTime.AddDays(1) > toDate.Date || ta.ToDate == Date.Empty)
													 && (
														 (ta.Currency == currencyCode && ta.UnitID == unitID) 
														 || 
														 (ta.Currency == companyCurrencyID && ta.UnitID == baseUnitOfMeasureID) 
														 )
													 && ta.QuantityAmount <= Math.Abs(quantity)
													 //TODO refactor for headeritem
													 //&& ( ta.VariantID == variantID || ta.InventDimID == "" )
													 //&& ta.it
													 orderby ta.Amount ascending, ta.ItemCode, ta.ItemRelation, ta.AccountCode, ta.AccountRelation, ta.Currency, ta.QuantityAmount
													 select ta).ToList();

			TradeAgreementPriceInfo tradeAgreementPriceInfo = new TradeAgreementPriceInfo();

			if (filteredTAList.Count > 0)
			{
				// Before we find the best TA we need to remove any TA that are behind a TA with findNext == false
				int firstFindNextFalseIndex = filteredTAList.FindIndex(x => !x.SearchAgain);
				if (firstFindNextFalseIndex != -1)
				{
					// Sublist of the firstFindNextFalseIndex + 1 TA items
					filteredTAList = filteredTAList.Take(firstFindNextFalseIndex + 1).ToList();
				}

				// Now we find the best TA entry
				TradeAgreementEntry bestTAEntry;

				// First we try to return a TA with the provided currency code and unit ID
				if (filteredTAList.Any(x => x.Currency == currencyCode && x.UnitID == unitID))
				{
					filteredTAList = filteredTAList.Where(x => x.Currency == currencyCode && x.UnitID == unitID).ToList();

					// If we have a TA with the provided variant, that takes precedence over a TA with an empty variant
					//TODO filter for headeritem
					//if (filteredTAList.Where(x => x.VariantID == variantID).Count() > 0)
					//{
					//    filteredTAList = filteredTAList.Where(x => x.VariantID == variantID).ToList();
					//}
					bestTAEntry = filteredTAList.First(x => x.Amount == filteredTAList.Min(y => y.Amount));

					tradeAgreementPriceInfo = new TradeAgreementPriceInfo()
					{
						PriceID = bestTAEntry.PriceID,
						PriceType = TradeAgreementPriceType.SalesPrice,
						Price = (calculateWithTax ? bestTAEntry.AmountIncludingTax : bestTAEntry.Amount) + bestTAEntry.Markup
					};

					return tradeAgreementPriceInfo;
				}

				// Does not contain a TA with the provided currency code and unit ID so now we must find the lowest price from the company currency and base sale UOM of the item
				// and convert it to the provided currency and unit

				// If we have a TA with the provided variant, that takes precedence over a TA with an empty variant
				// TODO filter for headeritem
				//if (filteredTAList.Where(x => x.VariantID == variantID).Count() > 0)
				//{
				//    filteredTAList = filteredTAList.Where(x => x.VariantID == variantID).ToList();
				//}

				bestTAEntry = filteredTAList.First(x => x.Amount == filteredTAList.Min(y => y.Amount));

				var conversionFactor = GetUnitAndCurrencyFactor(entry
																, itemID
																, companyCurrencyID
																, currencyCode
																, baseUnitOfMeasureID
																, unitID
																, cacheType);

				tradeAgreementPriceInfo = new TradeAgreementPriceInfo()
				{
					PriceID = bestTAEntry.PriceID,
					PriceType = TradeAgreementPriceType.SalesPrice,
					Price = ((calculateWithTax ? bestTAEntry.AmountIncludingTax : bestTAEntry.Amount) + bestTAEntry.Markup) * conversionFactor
				};
				return tradeAgreementPriceInfo;
			}

			return tradeAgreementPriceInfo;
		}

		protected virtual TradeAgreementPriceInfo FindBestPromotionPrice(
			List<PromotionOfferLine> promotions,
			decimal conversionFactor,
			bool calculateWithTax,
			IConnectionManager entry,
			RecordIdentifier currencyID,
			CacheType cacheType)
		{
			TradeAgreementPriceInfo tradeAgreementPriceInfo = new TradeAgreementPriceInfo();

			if (promotions == null || promotions.Count == 0)
			{
				return tradeAgreementPriceInfo;
			}

			Func<PromotionOfferLine, decimal> getPromoPrice = promo => calculateWithTax ? promo.OfferPriceIncludeTax : promo.OfferPrice;

			PromotionOfferLine minValidPromotion = promotions
														.Where(p => getPromoPrice(p) != 0)
														.OrderBy(p => getPromoPrice(p))
														.FirstOrDefault();

			if(minValidPromotion != null)
			{
				tradeAgreementPriceInfo.PriceID = minValidPromotion.OfferID;
				tradeAgreementPriceInfo.PriceType = TradeAgreementPriceType.Promotion;
                tradeAgreementPriceInfo.Price = getPromoPrice(minValidPromotion) * conversionFactor;
			}

			return tradeAgreementPriceInfo;
		}

        protected virtual TradeAgreementPriceInfo FindBestPromotionDiscounts(List<PromotionOfferLine> promotions)
		{
            TradeAgreementPriceInfo tradeAgreementPriceInfo = new TradeAgreementPriceInfo();

            if (promotions == null || promotions.Count == 0)
			{
				return tradeAgreementPriceInfo;
			}

			PromotionOfferLine maxPromotion = promotions
												.Where(p => p.DiscountPercent != 0)
												.OrderByDescending(p => p.DiscountPercent)
												.FirstOrDefault();

            if (maxPromotion != null)
            {
                tradeAgreementPriceInfo.PriceID = maxPromotion.OfferID;
                tradeAgreementPriceInfo.PriceType = TradeAgreementPriceType.Promotion;
                tradeAgreementPriceInfo.DiscountPercentage = maxPromotion.DiscountPercent;
            }

            return tradeAgreementPriceInfo;
        }

		protected virtual void FindConversionFactors(
			IConnectionManager entry,
			RecordIdentifier itemID,
			RecordIdentifier currencyIDTo,
			RecordIdentifier currencyIDFrom,
			RecordIdentifier unitIDTo,
			RecordIdentifier unitIDFrom,
			CacheType cacheType,
			out decimal unitConversionFactor,
			out decimal currencyConversionFactor
			)
		{
			if (currencyIDFrom != currencyIDTo)
			{
				currencyConversionFactor = DataProviderFactory.Instance.Get<IExchangeRatesData, ExchangeRate>()
					.ConversionRateBetweenCurrencies(entry, currencyIDFrom, currencyIDTo, cacheType);
			}
			else
			{
				currencyConversionFactor = 1;
			}

			if (unitIDFrom != unitIDTo)
			{
				unitConversionFactor = DataProviderFactory.Instance.Get<IUnitConversionData, UnitConversion>()
					.ConvertQtyBetweenUnits(entry, itemID, unitIDFrom, unitIDTo, 1); // The quantity is 1 because we only want the conversion factor
			}
			else
			{
				unitConversionFactor = 1;
			}
		}

		/// <summary>
		/// Checks if there is a special price case for the sale item. For example if the item is a fuel item etc.
		/// Note that this function can also change properties of the sale item 
		/// </summary>
		/// <param name="saleItem">The sale item to check for special case</param>
		protected virtual bool SpecialPriceCase(SaleLineItem saleItem)
		{
			// For fuel items the only valid price is the one that the POS receives from the pump controller.
			// For gift cards the only valid price is the one that the user typed in.
			// For sales orders the only valid price is the one that is already on the sales order
			// For sales invoices the only valid price is the one that is already on the sales invoice
			// So we do not get a price from the database.
			// For items returned with a receipt the only valid price is the one that is already on the item being returned.
			// For items that have NoPriceCalculation as true the valid price as been set and cannot be changed.
			// For income expense items the only valid price is the one that is already on the sales invoice
			if (saleItem.OriginatesFromForecourt 
				|| saleItem is GiftCertificateItem 
				|| saleItem is SalesOrderLineItem 
				|| saleItem is SalesInvoiceLineItem 
				|| saleItem is IncomeExpenseItem 
				|| saleItem.ReceiptReturnItem
				|| saleItem.PriceOverridden
				|| saleItem.NoPriceCalculation)
			{
				saleItem.WasChanged = true;
				return true;
			}
			return false;
		}

		protected virtual decimal GetTaxPrice(SaleLineItem saleLineItem)
		{
			if (saleLineItem.TaxIncludedInItemPrice)
				return saleLineItem.PriceWithTax;
			else
				return saleLineItem.Price;
		}

		protected virtual void SetTaxPrice(SaleLineItem saleLineItem, TradeAgreementPriceInfo tradeAgreementPriceInfo)
		{
			saleLineItem.PriceID = (string)tradeAgreementPriceInfo.PriceID;
			saleLineItem.PriceType = tradeAgreementPriceInfo.PriceType;

			if (saleLineItem.TaxIncludedInItemPrice)
			{
				if (tradeAgreementPriceInfo.Price != null)
				{
					saleLineItem.PriceWithTax = (decimal)tradeAgreementPriceInfo.Price;
					if (saleLineItem.IsAssembly)
					{
						saleLineItem.ItemAssembly.Price = (decimal)tradeAgreementPriceInfo.Price;
					}
				}
			}
			else
			{
				if (tradeAgreementPriceInfo.Price != null)
				{
					saleLineItem.Price = (decimal)tradeAgreementPriceInfo.Price;
				}
			}
		}

		/// <summary>
		/// ***********************************************************************************
		/// This method is a part of scale certification. Changes will affect and void the scale certification hash codes.
		/// ***********************************************************************************
		/// 
		/// Loops trough all sale items to find a quantity for a certain item.
		/// </summary>
		/// <param name="retailTransaction">The retail transaction.</param>
		/// <param name="saleItem">The item id a quantity is needed.</param>
		/// <returns></returns>
		protected virtual decimal GetQuantity(IRetailTransaction retailTransaction, SaleLineItem saleItem)
		{
			Decimal quantity = 0;
			foreach (SaleLineItem saleLineItem in retailTransaction.SaleItems)
			{
				if (saleLineItem.ItemId == saleItem.ItemId
					&& (string)saleLineItem.Dimension.ColorID == (string)saleItem.Dimension.ColorID
					&& (string)saleLineItem.Dimension.SizeID == (string)saleItem.Dimension.SizeID
					&& (string)saleLineItem.Dimension.StyleID == (string)saleItem.Dimension.StyleID
					&& saleLineItem.SalesOrderUnitOfMeasure == saleItem.SalesOrderUnitOfMeasure
					&& !saleLineItem.Voided)
				{
					quantity += saleLineItem.Quantity;
				}
			}
			if (quantity == 0) //If item is bought and return in the same quantity, a price is found as if one pcs. was bought, and not zero.
			{
				return 1;
			}
			else
			{
				return quantity;
			}

		}

		protected virtual decimal GetUnitAndCurrencyFactor(
			IConnectionManager entry
			, RecordIdentifier itemID
			, RecordIdentifier currencyIDTo
			, RecordIdentifier currencyIDFrom
			, RecordIdentifier unitIDTo
			, RecordIdentifier unitIDFrom
			, CacheType cacheType)
		{
			decimal conversionFactorBetweenUnits;
			decimal conversionFactorBetweenCurrencies;

			// Set the conversion factors
			FindConversionFactors(
				entry
				, itemID
				, currencyIDTo
				, currencyIDFrom
				, unitIDTo
				, unitIDFrom
				, cacheType
				, out conversionFactorBetweenUnits
				, out conversionFactorBetweenCurrencies);

			return conversionFactorBetweenUnits * conversionFactorBetweenCurrencies;
		}
	}
}

