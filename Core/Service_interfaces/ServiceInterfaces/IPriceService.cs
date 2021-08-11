using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using LSOne.Services.Interfaces.SupportClasses.Price;

namespace LSOne.Services.Interfaces
{
    public interface IPriceService : IService
    {
        /// <summary>
        /// Gets and sets the price for the last item and any other instances of the same item on the current sale. 
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="cacheType">What type of caching should be done during this operation</param>
        void SetPrice(IConnectionManager entry, IRetailTransaction retailTransaction, CacheType cacheType);

        /// <summary>
        /// Updates all prices for all items on the transaction
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="cacheType">What type of caching should be done during this operation</param>
        /// <param name="restoreItemPrices">If false then the prices will not be restored for specific items f.ex. when a price has already been overwritten and a customer is then added to the transaction</param>        
        /// <returns></returns>
        IRetailTransaction UpdateAllPrices(IConnectionManager entry, IRetailTransaction retailTransaction, bool restoreItemPrices, CacheType cacheType);

        /// <summary>
        /// Updates the price of a single item on the transaction
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        /// <param name="saleLineItem">Item for which to update the price</param>
        /// <param name="cacheType">What type of caching should be done during this operation</param>
        /// <param name="restoreItemPrices">If false then the prices will not be restored for specific items f.ex. when a price has already been overwritten and a customer is then added to the transaction</param>        
        /// <returns></returns>
        IRetailTransaction UpdatePrice(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem, bool restoreItemPrices, CacheType cacheType);

        /// <summary>
        /// Retrieves the TradeAgreementPriceInfo for a specific item. This function checks the all the prices and promotions and decides what is the best trade agreement depending on currency, dates and other configurations
        /// </summary>
        /// <param name="entry">The entry to the database</param>        
        /// <param name="cacheType">What type of caching should be done during this operation</param>
        /// <param name="itemID">The </param>
        /// <param name="variantID"></param>
        /// <param name="customerID"></param>
        /// <param name="storeID"></param>
        /// <param name="currencyCode"></param>
        /// <param name="unitID"></param>
        /// <param name="salesTypeID"></param>
        /// <param name="calculateWithTax"></param>
        /// <param name="quantity"></param>        
        /// <returns></returns>
        TradeAgreementPriceInfo GetPrice(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier variantID,
            RecordIdentifier customerID,
            RecordIdentifier storeID,
            RecordIdentifier currencyCode,
            RecordIdentifier unitID,
            RecordIdentifier salesTypeID,
            bool calculateWithTax,
            Decimal quantity,
            CacheType cacheType);

        TradeAgreementPriceInfo GetPrice(
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
            DateTime fromDate,
            DateTime toDate,
            CacheType cacheType);

        // <summary>Called from the class BusinessLogic\ItemSystem.cs: 
        // <code>LSRetailPosis.ApplicationServices.IPrice.GetPrice((RetailTransaction)posTransaction);</code>
        // For the case, that the barcode does not contain the price:
        // 1. The total quantity of the occurrences of this item needs to be calculated:
        // <code>private decimal GetQuantity(RetailTransaction retailTransaction, string itemId)</code>
        // 2. The implementation checks whether we are handling fuel. In that case the only valid price is the one that the POS receives from the pump controller; such that we do not retrieve any price from the database.
        // 3. The customer price for the item is calculated. It has to be distinguished between the retail customer and the registered customer.
        // In any case, the active retail price is retrieved and the lowest price found is applied.
        // The sequence of checking: item card(?), trade agreements, promotionprice. If no price has been found (price == 0), then the basic price is chosen.
        // 
        // <code>private decimal GetRetailPrice(string priceGroup, string storeCurrencyCode, bool taxIncludedInPrice, decimal quantity, string itemId)</code> 
        // In case of dealing with a registered customer, the function 
        // <code> price = FindPriceAgreement(retailTransaction.Customer.CustomerId, retailTransaction.Customer.PriceGroup, retailTransaction.StoreCurrencyCode, quantity, saleItem.ItemID)</code> +
        // is called. (Note: the function is currently also called when not dealing with a customer.)
        // If the promotion price is lower then the lowest price found in trade agreements then it should be used - otherwise use the trade agreement price.
        // </summary>
        // <param name="retailTransaction"></param>
        // <returns>A new (updated) copy of the retail transaction (not using a reference value).</returns>
        // RetailTransaction GetPrice(IConnectionManager entry, RetailTransaction retailTransaction);

        // <summary>
        // To be used when a customer is specified or updated. 
        // A certain customer can receive special discounts, tax etc., such that
        // if a customer is selected, then all checks and maybe an price update has to be done. 
        // The lowest price from the following price policy options becomes the 'active' price:
        // - Item Card
        // - Trade Agreements 
        // - Promotion
        // - Key in new Price
        // All these aspects have to be checked.
        // </summary>
        // <param name="retailTransaction">The current retail transaction.</param>
        // <param name="restoreItemPrices">If true then prices that have been overridden will be returned back to original prices</param>
        // <returns>A new (updated) copy of the retail transaction (not using a reference value).</returns>
        //RetailTransaction UpdateAllPrices(IConnectionManager entry, RetailTransaction retailTransaction, bool restoreItemPrices);

        // <summary>
        // Checks if the item is a part of a promotion and if it is returns that price. If it's not then the function returns the basic item price        
        // </summary>
        // <param name="itemId">The ID of the item to be checked </param>
        // <returns>The price of the item.</returns>
        //decimal GetItemPrice(IConnectionManager entry, RecordIdentifier itemId);

    }
}
