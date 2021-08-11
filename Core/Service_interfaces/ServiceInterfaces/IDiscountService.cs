using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface IDiscountService : IService
    {
        /// <summary>
        /// The implementation has to take respect to every possible configuration, i.e. periodic discounts, 
        /// quantity discounts, check for special offers, customer discounts, multiline discounts. 
        /// From these factors, the total discount has to be calculated, the retailTransaction parameter has to 
        /// be updated and then the instance is returned.
        /// Since an item can belong to more than one discounts (i.e. Mix-and-Match), all discounts attached to an 
        /// item have to be removed everytime a SaleLine is added and it's discount re-evaluated in order to grant
        /// the best possible offer to the customers.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The retailTransaction parameter has to be updated</param>
        /// <param name="CalculateNow">Forces the discounts to be calculated no matter what how the Calculate Discount configurations are set</param>
        /// <returns>A RetailTransaction instance.</returns>
        IRetailTransaction CalculateDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, bool CalculateNow = false);

        /// <summary>
        /// Entry point for further manipulation of the retail transaction. 
        /// In particular, it is possible to set the amount of the discount granted on the total amount using the property
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rt"></param>
        /// <param name="amountValue"></param>
        void AddTotalDiscountAmount(IConnectionManager entry, IRetailTransaction rt, decimal amountValue);

        /// <summary>
        /// Confirms that a total discount amount can be applied to the sale
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rt">The current sale</param>
        /// <param name="amountValue">The total amount value entered</param>
        /// <param name="maxAmountValue">The maximum amount allowed for the current user</param>
        bool AuthorizeTotalDiscountAmount(IConnectionManager entry, IRetailTransaction rt, decimal amountValue, decimal maxAmountValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rt"></param>        
        /// <param name="percentValue"></param>
        void AddTotalDiscountPercent(IConnectionManager entry, IRetailTransaction rt, decimal percentValue);

        /// <summary>
        /// Confirms that a total discount percentage can be applied to the sale
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="rt">The current sale</param>
        /// <param name="percentValue">The total % value entered</param>
        /// <param name="maxPercentValue">The maximum % allowed for the current user</param>
        bool AuthorizeTotalDiscountPercent(IConnectionManager entry, IRetailTransaction rt, decimal percentValue, decimal maxPercentValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        void AddLineDiscountAmount(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        /// <param name="maximumDiscountAmt"></param>
        bool AuthorizeLineDiscountAmount(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem, decimal maximumDiscountAmt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        void AddLineDiscountPercent(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineItem"></param>
        /// <param name="discountItem"></param>
        /// <param name="maximumDiscountPct"></param>
        bool AuthorizeLineDiscountPercent(IConnectionManager entry, ISaleLineItem lineItem, ILineDiscountItem discountItem, decimal maximumDiscountPct);

        /// <summary>
        /// Reloads the discount settings.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        void ResetDiscountService(IConnectionManager entry);

        /// <summary>
        /// Triggers a periodic discount with the given offerID if it's applicable to the given saleItem. Prompts user to choose a periodic discount for 
        /// the given item if offerID is empty.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current retail transaction in the POS</param>
        /// <param name="offerID">If not empty then this offer should be triggered otherwise a list of discounts should be displayed</param>
        void ManuallyTriggerPeriodicDiscount(IConnectionManager entry, IRetailTransaction retailTransaction, string offerID);

        /// <summary>
        /// Clears the periodic discount the user chooses from a list of manually triggered discounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current retail transaction in the POS</param>
        void ClearManuallyTriggeredDiscount(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Clears all discounts manual, triggered and automatically calculated discounts from the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current retail transaction in the POS</param>
        void ClearAllDiscounts(IConnectionManager entry, IRetailTransaction retailTransaction);

        /// <summary>
        /// Gets maximum discounted purchases and the current period's discounted purchases of a customer. The period and max discounted purchase are
        /// controlled by the customers Customer group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">ID of the customer</param>
        /// <param name="maxDiscountedPurchases">The maximum amount that a customer can buy with customer discounts</param>
        /// <param name="currentPeriodDiscountedPurchases">The current period's amount that the customer has purchased with discounts</param>
        void CustomersDiscountedPurchasesStatus(
            IConnectionManager entry,
            string customerID,
            out decimal maxDiscountedPurchases,
            out decimal currentPeriodDiscountedPurchases);
    }
}
