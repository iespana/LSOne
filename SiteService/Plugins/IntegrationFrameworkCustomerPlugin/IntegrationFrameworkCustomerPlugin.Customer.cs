using System;
using System.Collections.Generic;
using System.Diagnostics;

using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkCustomer
{
	public partial class IntegrationFrameworkCustomerPlugin
	{
		public virtual void Save(Customer customer)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (customer.Deleted)
					{
						Providers.CustomerData.Delete(dataModel, customer.ID);
					}					
					else
					{
					    Providers.CustomerData.SaveWithAddresses(dataModel, customer);
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

		public virtual SaveResult SaveList(List<Customer> customers)
		{
            Action<IConnectionManager, Customer> save = (dataModel, customer) =>
            {
                if (customer.Deleted)
                {
                    Providers.CustomerData.Delete(dataModel, customer.ID);
                }
                else
                {
                    Providers.CustomerData.SaveWithAddresses(dataModel, customer);
                }
            };

            return SaveList(customers, Providers.CustomerData, save);
		}

		public virtual Customer Get(RecordIdentifier customerID)
		{
			IConnectionManager dataModel = GetConnectionManagerIF();
			try
			{
				return Providers.CustomerData.Get(dataModel, customerID, UsageIntentEnum.Normal);
			}

			finally
			{
				ReturnConnection(dataModel, out dataModel);
			}
		}      

		public virtual void Delete(RecordIdentifier customerID)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					Providers.CustomerData.Delete(dataModel, customerID);
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

		public virtual void SavePrice(IFSalesPrice salesPrice)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (!salesPrice.Validate())
					{
						throw new Exception("Sales price is not valid.");
					}

					TradeAgreementEntry tradeAgreement = new TradeAgreementEntry
					{
						ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode) salesPrice.PriceIsFor,
						AccountCode = (TradeAgreementEntryAccountCode) salesPrice.AppliesTo,
						ItemRelation = salesPrice.PriceIsForValue,
						AccountRelation = salesPrice.AppliesToValue,
						Currency = salesPrice.Currency,
						UnitID = salesPrice.UnitID,
						FromDate = salesPrice.FromDate,
						ToDate = salesPrice.ToDate,
						QuantityAmount = salesPrice.Quantity,
						Amount = salesPrice.Price,
						AmountIncludingTax = salesPrice.PriceWithTax,
						Markup = salesPrice.Markup,
						Relation = TradeAgreementEntry.TradeAgreementEntryRelation.PriceSales
					};

					//Get ID if it exists to allow update
					tradeAgreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(dataModel, tradeAgreement);
					Providers.TradeAgreementData.Save(dataModel, tradeAgreement, string.Empty);
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

		public virtual void SaveCustomerLineDiscount(IFDiscount discount)
		{
			try
			{

				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (!discount.Validate())
					{
						throw new Exception("Discount is not valid.");
					}

					TradeAgreementEntry tradeAgreement = new TradeAgreementEntry
					{
						ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode) discount.DiscountIsFor,
						AccountCode = (TradeAgreementEntryAccountCode) discount.AppliesTo,
						ItemRelation = discount.DiscountIsForValue,
						AccountRelation = discount.AppliesToValue,
						Currency = discount.Currency,
						UnitID = discount.UnitID,
						FromDate = discount.FromDate,
						ToDate = discount.ToDate,
						QuantityAmount = discount.Quantity,
						Amount = discount.Amount,
						Percent1 = discount.Percentage1,
						Percent2 = discount.Percentage1,
						Relation = TradeAgreementEntry.TradeAgreementEntryRelation.LineDiscSales
					};

					//Get ID if it exists to allow update
					tradeAgreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(dataModel, tradeAgreement);
					Providers.TradeAgreementData.Save(dataModel, tradeAgreement, string.Empty);
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

		public virtual void SaveCustomerTotalDiscount(IFDiscount discount)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (!discount.Validate())
					{
						throw new Exception("Discount is not valid.");
					}

					TradeAgreementEntry tradeAgreement = new TradeAgreementEntry
					{
						ItemCode = (TradeAgreementEntry.TradeAgreementEntryItemCode) discount.DiscountIsFor,
						AccountCode = (TradeAgreementEntryAccountCode) discount.AppliesTo,
						ItemRelation = discount.DiscountIsForValue,
						AccountRelation = discount.AppliesToValue,
						Currency = discount.Currency,
						UnitID = discount.UnitID,
						FromDate = discount.FromDate,
						ToDate = discount.ToDate,
						QuantityAmount = discount.Quantity,
						Amount = discount.Amount,
						Percent1 = discount.Percentage1,
						Percent2 = discount.Percentage1,
						Relation = TradeAgreementEntry.TradeAgreementEntryRelation.EndDiscSales
					};

					//Get ID if it exists to allow update
					tradeAgreement.ID = Providers.TradeAgreementData.GetTradeAgreementID(dataModel, tradeAgreement);
					Providers.TradeAgreementData.Save(dataModel, tradeAgreement, string.Empty);
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

		public virtual void SavePriceDiscountGroup(PriceDiscountGroup group)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					Providers.PriceDiscountGroupData.Save(dataModel, group);
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

		public virtual void AddItemsToLineDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> itemIDs)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (
						!Providers.PriceDiscountGroupData.Exists(dataModel, PriceDiscountModuleEnum.Item,
							PriceDiscGroupEnum.LineDiscountGroup, groupID))
					{
						PriceDiscountGroup newGroup = new PriceDiscountGroup
						{
							GroupID = groupID,
							Module = PriceDiscountModuleEnum.Item,
							Text = groupID.StringValue,
							Type = PriceDiscGroupEnum.LineDiscountGroup,
						};

						Providers.PriceDiscountGroupData.Save(dataModel, newGroup);
					}

					foreach (RecordIdentifier itemID in itemIDs)
					{
						Providers.ItemInPriceDiscountGroupData.AddItemToGroup(dataModel, itemID,
							PriceDiscGroupEnum.LineDiscountGroup, groupID.StringValue);
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

		public virtual void AddCustomersToLineDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> customerIDs)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (
						!Providers.PriceDiscountGroupData.Exists(dataModel, PriceDiscountModuleEnum.Customer,
							PriceDiscGroupEnum.LineDiscountGroup, groupID))
					{
						PriceDiscountGroup newGroup = new PriceDiscountGroup
						{
							GroupID = groupID,
							Module = PriceDiscountModuleEnum.Customer,
							Text = groupID.StringValue,
							Type = PriceDiscGroupEnum.LineDiscountGroup,
						};

						Providers.PriceDiscountGroupData.Save(dataModel, newGroup);
					}

					foreach (RecordIdentifier customerID in customerIDs)
					{
						Providers.PriceDiscountGroupData.AddCustomerToGroup(dataModel, customerID,
							PriceDiscGroupEnum.LineDiscountGroup, groupID.StringValue);
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

		public virtual void AddCustomersToTotalDiscountGroup(RecordIdentifier groupID, List<RecordIdentifier> customerIDs)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					if (
						!Providers.PriceDiscountGroupData.Exists(dataModel, PriceDiscountModuleEnum.Customer,
							PriceDiscGroupEnum.TotalDiscountGroup, groupID))
					{
						PriceDiscountGroup newGroup = new PriceDiscountGroup
						{
							GroupID = groupID,
							Module = PriceDiscountModuleEnum.Customer,
							Text = groupID.StringValue,
							Type = PriceDiscGroupEnum.TotalDiscountGroup,
						};

						Providers.PriceDiscountGroupData.Save(dataModel, newGroup);
					}

					foreach (RecordIdentifier customerID in customerIDs)
					{
						Providers.PriceDiscountGroupData.AddCustomerToGroup(dataModel, customerID,
							PriceDiscGroupEnum.TotalDiscountGroup, groupID.StringValue);
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
	}
}
