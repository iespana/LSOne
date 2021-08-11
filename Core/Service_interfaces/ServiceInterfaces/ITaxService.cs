using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using System;

namespace LSOne.Services.Interfaces
{
    public interface ITaxService : IService
    {
        /// <summary>
        /// Calculates the tax for a specific item in the transaction.
        /// </summary>
        /// <param name="entry">Entry into the data framework</param>
        /// <param name="retailTransaction">The transaction to be calculated</param>
        /// <param name="saleLineItem">The sale line item to calculate tax for</param>
        void CalculateTax(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem);

        /// <summary>
        /// Calculates tax for all amounts and discounts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="saleLineItem">The sale line to be calculated</param>
        /// <param name="retailTransaction">The transaction to calculate for</param>
        void CalcAmountsTaxIncluded(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction);

        /// <summary>
        /// Calculate tax for all amounts and discounts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="saleLineItem">The sale line to be calculated</param>
        void CalcAmountsTaxExcluded(IConnectionManager entry, ISaleLineItem saleLineItem);

        /// <summary>
        /// Returns true if prices are calculated with tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="defaultStoreID">The default storeID</param>
        /// <returns></returns>
        bool PricesAreCalculatedWithTax(IConnectionManager entry, RecordIdentifier defaultStoreID);


        /// <summary>
        /// Calculates new item price for a price struct when a tax group has changed. 
        /// Note this only handles base price
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The item to be recalculated</param>
        /// <param name="calculatePriceWithTax">True if prices should be calculated with tax, else false</param>
        /// <param name="defaultStoresTaxGroupID">The default stores tax group ID</param>
        /// <returns>True if price actually changed, else false</returns>
        bool CalculateNewItemPrice(IConnectionManager entry, RetailItem item, bool calculatePriceWithTax, RecordIdentifier defaultStoresTaxGroupID);

        /// <summary>
        /// Updates trade agreement lines as well as promotion lines for given retail item when tax group has changed.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The retail item to update the lines for</param>
        /// <param name="calculatePriceWithTax">True if prices should be calculated with tax, else false</param>
        /// <param name="defaultStoresTaxGroupID">The default stores tax group ID</param>
        void UpdateTradeAgreementsAndPromotionsForItem(IConnectionManager entry, RetailItem item, bool calculatePriceWithTax, RecordIdentifier defaultStoresTaxGroupID);

        /// <summary>
        /// Simple tax check for an item. The function calculates the taxamount and sets the PriceInclTax variable
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineItem">The sale line to be calculated</param>
        /// <param name="rt">The retail transaction</param>
        void CalcTaxExcluded(IConnectionManager entry, ISaleLineItem lineItem, IRetailTransaction rt);


        /// <summary>
        /// Simple tax check for an item. The function calculates the taxamount and sets the Price variable
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineItem">The sale line to be calculated</param>
        /// <param name="rt">The retail transaction</param>
        void CalcTaxIncluded(IConnectionManager entry, ISaleLineItem lineItem, IRetailTransaction rt);


        /// <summary>
        /// Calculates price with tax based on the given price and the two sales tax groups
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="price">The original price</param>
        /// <param name="itemSalesTaxGroupID">The item sales tax group ID. TaxCodes that are in this group and in the group defined by salesTaxGroupID will be used to calculate the tax</param>
        /// <param name="salesTaxGroupID">The sales tax group ID. TaxCodes that are in this group and in the group defined by itemSalesTaxGroupID will be used to calculate the tax</param>
        /// <param name="hasLastKnownPriceWithTax">True if we supply lastknownPriceWithTax, else false</param>
        /// <param name="lastKnownPriceWithTax">Last known price with tax, this is for the system to be able to correct it self from rounding inaccurancy. The parameter may be null</param>
        /// <param name="priceWithTaxLimiter">The decimal limiter used to figure out if the last known price has changed enough, given that hasLastKnownPriceWithTax is true</param>
        /// <returns>Price with tax</returns>
        decimal CalculatePriceWithTax(
            IConnectionManager entry,
            decimal price,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID,
            bool hasLastKnownPriceWithTax,
            decimal lastKnownPriceWithTax,
            DecimalLimit priceWithTaxLimiter);

        /// <summary>
        /// Calculates price without tax based on price with tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="priceWithTax">The price with tax</param>
        /// <param name="itemSalesTaxGroupID">The item sales tax group ID. TaxCodes that are in this group and in the group defined by salesTaxGroupID will be used to calculate the tax</param>
        /// <param name="salesTaxGroupID">The sales tax group ID. TaxCodes that are in this group and in the group defined by itemSalesTaxGroupID will be used to calculate the tax</param>
        /// <returns>Price without tax</returns>
        decimal CalculatePriceFromPriceWithTax(
            IConnectionManager entry,
            decimal priceWithTax,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Calculates the tax on an item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="priceWithoutTax">The items price without tax</param>
        /// <param name="itemSalesTaxGroupID">The item sales tax group ID. TaxCodes that are in this group and in the group defined by salesTaxGroupID will be used to calculate the tax</param>
        /// <param name="salesTaxGroupID">The sales tax group ID. TaxCodes that are in this group and in the group defined by itemSalesTaxGroupID will be used to calculate the tax</param>
        /// <returns></returns>
        decimal CalculateTax(
            IConnectionManager entry,
            decimal priceWithoutTax,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID);

        decimal CalculateTaxBetweenSalesTaxGroups(
            IConnectionManager entry,
            decimal price,
            RecordIdentifier salesTaxGroupID1,
            RecordIdentifier salesTaxGroupID2);

        /// <summary>
        /// Gets an items tax based on its sales price. It is assumed the item is sold on the default store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The item</param>
        /// <returns>Items tax based on its sales price</returns>
        decimal GetItemTax(IConnectionManager entry, RetailItem item);

        /// <summary>
        /// Gets the tax for an item given a price and that it is sold on the default store 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="salesTaxItemGroupID">The item</param>
        /// <param name="amount">The amount to calculate taxes on</param>
        /// <returns>The tax for an item given a price and that it is sold on the default store </returns>
        decimal GetItemTaxForAmount(IConnectionManager entry, RecordIdentifier salesTaxItemGroupID, decimal amount);

        /// <summary>
        /// Gets an items price given its price with tax. It is assumed the item is sold on the default store
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="salesTaxItemGroupID">The tax group</param>
        /// <param name="amountWithTax">The amount with tax to calculate amount without tax</param>
        /// <returns>Items price given its price with tax</returns>
        decimal GetItemPriceForItemPriceWithTax(IConnectionManager entry, RecordIdentifier salesTaxItemGroupID, decimal amountWithTax);

        /// <summary>
        /// Updates prices in the system. Updates include item prices, trade agreement prices, promotion prices. It depends on the 
        /// default store setting wether prices with or without tax are kept and the other updated.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="ID">ID related to the change, controled by the parameter updateEnum. The following list describes what each enum means for the ID
        /// TaxCode: ID = new RecordIdentifier(taxCodeID);
        /// ItemTaxGroup: ID = new RecordIdentifier(itemTaxGroupID);
        /// DefaultStoreTaxGroup: ID = new RecordIdentifier(defaultStoreID, defaultStoresTaxGroupID);
        /// SpecificItemTaxGroup:ID = new RecordIdentifier(itemID, itemSalesTaxGroupID);
        /// AllItems: ID not used;
        /// </param>
        /// <param name="updateEnum">Indicates the type of change happening. This controls what is stored in the ID parameter</param>
        /// <param name="updatedItemsCount">Count of items that were updated</param>
        /// <param name="updatedTradeAgreementsCount">Count of trade agreements that were updated</param>
        /// <param name="updatedPromotionOfferLinesCount">Count of promotion offer lines that were updated</param>
        void UpdatePrices(
           IConnectionManager entry,
           RecordIdentifier ID,
           UpdateItemTaxPricesEnum updateEnum,
           out int updatedItemsCount,
           out int updatedTradeAgreementsCount,
           out int updatedPromotionOfferLinesCount);

            /// <summary>
            /// Get common tax codes between an item sales tax group and a sales tax group
            /// </summary>
            /// <param name="entry">Entry into the database</param>
            /// <param name="itemSalesTaxGroupID">ID of the item sales tax group</param>
            /// <param name="salesTaxGroupID">ID of the sales tax group</param>
            List<TaxCode> GetCommonTaxCodes(
            IConnectionManager entry,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Gets the tax amount for a purchse order line
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemSalesTaxGroupID">Sales tax group id of the item</param>
        /// <param name="vendorID">ID of the vendor connected to the purchase order</param>
        /// <param name="storeID">ID of the store connected to the purchase order</param>
        /// <param name="unitPrice">Unit price of the item</param>
        /// <param name="discountAmount">Discount amount of the item</param>
        /// <param name="discountPercentage">Discount percentage of the item</param>
        /// <param name="taxCalculationMethod">The tax calculation method to use</param>
        /// <returns></returns>
        decimal GetTaxAmountForPurchaseOrderLine(
            IConnectionManager entry,
            RecordIdentifier itemSalesTaxGroupID,
            RecordIdentifier vendorID,
            RecordIdentifier storeID,
            decimal unitPrice,
            decimal discountAmount,
            decimal discountPercentage,
            TaxCalculationMethodEnum taxCalculationMethod);

        /// <summary>
        /// Looks up extended information for each item in supplied list and adds to the reply.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lines"></param>
        /// <returns></returns>
        List<DiscountOfferLineWithPrice> GetLinesWithPrices(IConnectionManager entry, List<DiscountOfferLine> lines);

        /// <summary>
        /// Clears the tax exemption properties of the transaction and recalculates the prices
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        void ClearTaxExemption(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Asks the user for a tax exemption code and then sets the tax exemption properties of the transaction and recalculates the prices
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        void SetTaxExemption(IConnectionManager entry, IRetailTransaction retailTransaction);


    }
}
