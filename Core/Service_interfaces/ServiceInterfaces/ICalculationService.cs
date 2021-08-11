using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.Calculation;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ICalculationService : IService
    {
        /// <summary>
        /// Calculates all amounts on the transaction
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="posTransaction">The transaction to be calculated</param>
        /// <param name="currencyCode">POS Should in most cases pass null here then current store currency is used, while on Site Manager a value needs to be passed here</param>
        void CalculateTotals(IConnectionManager entry, IPosTransaction posTransaction, RecordIdentifier currencyCode = null);

        /// <summary>
        /// Calculates all amounts on the saleline item such as discount amounts, net amount and etc.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="saleLineItem">The sale line item that is to be calculated</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <param name="currencyCode">POS Should in most cases pass null here then current store currency is used, while on Site Manager a value needs to be passed here</param>
        void CalculateLine(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction, RecordIdentifier currencyCode = null);

        /// <summary>
        /// Calculates all amounts on the saleline item such as discount amounts, net amount and etc.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="saleLineItem">The sale line item that is to be calculated</param>
        /// <param name="retailTransaction">The retail transaction</param>
        /// <param name="compareDiscounts">If true then discounts are compared and the best one found for the customer</param>
        /// <param name="currencyCode">POS Should in most cases pass null here then current store currency is used, while on Site Manager a value needs to be passed here</param>
        void CalculateLine(IConnectionManager entry, ISaleLineItem saleLineItem, IRetailTransaction retailTransaction, bool compareDiscounts, RecordIdentifier currencyCode = null);

        /// <summary>
        /// Calculates the periodic discount percent on a sale line
        /// </summary>
        /// <param name="saleLineItem">The sale line item to be calculated</param>
        /// <param name="retailTransaction">The retail transaction</param>
        void CalculatePeriodicDiscountPercent(ISaleLineItem saleLineItem, IRetailTransaction retailTransaction);

        /// <summary>
        /// Calculates the change-back amounts for the given transaction
        /// </summary>
        /// <param name="entry">Entry into the database</param>        
        /// <param name="transaction">The transaction to calculate the change-back. Transaction of type <see cref="IRetailTransaction"/> and <see cref="ICustomerPaymentTransaction"/> are supported</param>
        /// <param name="lastTenderLineItem">The last tender line of <paramref name="transaction"/> that should be used to calculate the change back from</param>
        ChangeBackAmountsInfo CalculateChangeBackAmounts(IConnectionManager entry, IPosTransaction transaction, ITenderLineItem lastTenderLineItem);
    }
}
