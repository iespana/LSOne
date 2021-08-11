using System.Linq;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    /// <summary>
    /// A class that has various calculation functions that are used by both the service and dialogs within it
    /// </summary>
    public static class CalculationHelper
    {

        /// <summary>
        /// Calculates the deposit that needs to be paid
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">For which items is the total to be calculated. All returns the total regardless of ordered/fully received or pickup</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns></returns>
        public static decimal GetTotalDeposit(IConnectionManager entry, CustomerOrderSummaries type, IRetailTransaction retailTransaction)
        {
            decimal total = decimal.Zero;

            //Gets the total deposit paid for all items excluding voided items
            if (type == CustomerOrderSummaries.All)
            {
                total = retailTransaction.SaleItems.Where(w => !w.Voided).Sum(item => item.Order.DepositToBePaid());
            }
            //Get the total deposit paid for all items on order and have not been fully recieved. If the number on order is less than the quantity of the item then 
            //it is calculated how much of the deposit is related to the number on order
            else if (type == CustomerOrderSummaries.OnOrder)
            {
                total += retailTransaction.SaleItems.                    
                    Where(w => !w.Voided && !w.Order.FullyReceived).
                    Where(item => item.Order.ToPickUp == 0).
                    Sum(item => item.Order.DepositToBePaid());
            }

            //Get the total deposit paid for all items to be picked up and have not been fully recieved. If the number for pick up is less than the quantity of the item then 
            //it is calculated how much of the deposit is related to the number of item to pick up
            else if (type == CustomerOrderSummaries.ToPickUp)
            {
                total += retailTransaction.SaleItems.                    
                    Where(w => !w.Voided && !w.Order.FullyReceived && w.Order.ToPickUp > 0).
                    Where(item => item.Order.ToPickUp == item.Order.Ordered).
                    Sum(item => item.Order.DepositToBePaid());
            }

            return total;
        }

        public static decimal GetTotalAlreadyPaidAdditionalPayments(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (retailTransaction.CustomerOrder.AdditionalPaymentLines == null)
            {
                return decimal.Zero;
            }

            return retailTransaction.CustomerOrder.AdditionalPaymentLines.Where(w => w.AmountPaid).Sum(s => s.Amount);
        }

        /// <summary>
        /// Retrieves the total of already paid deposits for a subset of or all items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">What type of summary is this see <see cref="CustomerOrderSummaries"/></param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns>The total amount of already paid deposits</returns>
        public static decimal GetTotalAlreadyPaidDeposit(IConnectionManager entry, CustomerOrderSummaries type, IRetailTransaction retailTransaction)
        {
            decimal total = decimal.Zero;

            //Gets the total deposit paid for all items excluding voided items
            if (type == CustomerOrderSummaries.All)
            {
                total = retailTransaction.SaleItems.Where(w => !w.Voided).Sum(item => item.Order.DepositAlreadyPaid());
            }
            //Get the total deposit paid for all items on order and have not been fully recieved. If the number on order is less than the quantity of the item then 
            //it is calculated how much of the deposit is related to the number on order
            else if (type == CustomerOrderSummaries.OnOrder)
            {
                total += retailTransaction.SaleItems.
                    Where(w => !w.Voided && !w.Order.FullyReceived).
                    Where(item => item.Order.ToPickUp == 0).
                    Sum(item => item.Order.DepositAlreadyPaid());
            }

            //Get the total deposit paid for all items to be picked up and have not been fully recieved. If the number for pick up is less than the quantity of the item then 
            //it is calculated how much of the deposit is related to the number of item to pick up
            else if (type == CustomerOrderSummaries.ToPickUp)
            {
                total += retailTransaction.SaleItems.
                    Where(w => !w.Voided && !w.Order.FullyReceived && w.Order.ToPickUp > 0).
                    Where(item => item.Order.ToPickUp == item.Order.Ordered).
                    Sum(item => item.Order.DepositAlreadyPaid());
            }

            return total;
        }

        /// <summary>
        /// Return the total amount to be paid once the deposit has been detracted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">For which items is the total to be calculated. All returns the total regardless of ordered/fully received or pickup</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="includeDeposit">If true then include the deposit to be paid in the calculations</param>
        /// <returns></returns>
        public static decimal GetTotalToBePaid(IConnectionManager entry, CustomerOrderSummaries type, IRetailTransaction retailTransaction, bool includeDeposit)
        {
            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal total = decimal.Zero;

            //Returns the total to be paid for all items on the order except voided items regardless of order status
            if (type == CustomerOrderSummaries.All)
            {
                total = retailTransaction.SaleItems.Where(w => !w.Voided).Sum(item => item.NetAmountWithTax);
            }

            //Get the total to be paid for all items on order and have not been fully recieved. If the number on order is less than the quantity of the item then 
            //it is calculated how much of the total is related to the number on order
            else
            {
                if (type == CustomerOrderSummaries.OnOrder)
                {
                    total += retailTransaction.SaleItems.
                        Where(w => !w.Voided && !w.Order.FullyReceived).
                        Where(item => item.Order.ToPickUp == 0).
                        Sum(item => item.NetAmountWithTax);
                }

                //Get the total to be paid for all items that are to be picked up and have not been fully recieved. If the number to pick up is less than the quantity of the item then 
                //it is calculated how much of the deposit is related to the number of items for pickup
                else if (type == CustomerOrderSummaries.ToPickUp)
                {
                    var currentPaymentHasLimitations = retailTransaction.SaleItems.Any(x => x.PaymentIndex == Constants.PaymentIndexToBeUpdated);

                    total += retailTransaction.SaleItems.
                        Where(w => !w.Voided && !w.Order.FullyReceived && w.Order.ToPickUp > 0).
                        Where(item => item.Order.ToPickUp == item.Order.Ordered).
                        Where(x => currentPaymentHasLimitations && x.PaymentIndex == Constants.PaymentIndexToBeUpdated || !currentPaymentHasLimitations).
                        Sum(item => item.NetAmountWithTax);
                }
            }

            if (includeDeposit)
            {
                //Decrease the number paid with the already paid deposit
                decimal deposit = GetTotalDeposit(entry, type, retailTransaction);
                total -= deposit;
            }

            return total;
        }

        /// <summary>
        /// Returns the total amount of all items that have been fully received. FullyReceived flag is only set when the customer order is recalled
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns></returns>
        public static decimal GetTotalFullyReceived(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            return retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.FullyReceived).Sum(item => item.NetAmountWithTax);

        }

        /// <summary>
        /// Calculates what the deposit for a specific item on the customer order should be according to configurations and updates the item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The item that is to be calculated</param>
        /// <param name="orderType">What type of customer order. Used to get the settings</param>
        public static void CalculateDeposit(IConnectionManager entry, ISaleLineItem item, CustomerOrderType orderType, IRetailTransaction retailTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            CustomerOrderSettings orderSettings = Providers.CustomerOrderSettingsData.Get(entry, orderType);

            decimal depositPercentage = retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New ?
                orderSettings.MinimumDeposits / 100
                :
                (retailTransaction.CustomerOrder.MinimumDeposit * 100 / retailTransaction.NetAmountWithTax) / 100;

            //If the deposit has already been calculated then don't calculate it again
            if (item.Order.TotalDepositAmount() == decimal.Zero)
            {
                //Update the ordered value on the item - the starting value is always the same as the quantity
                item.Order.Ordered = item.Quantity;
                //Calculate the minimum deposit on the item and round it
                decimal deposit = Interfaces.Services.RoundingService(entry).Round(entry, item.NetAmountWithTax * depositPercentage, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
                item.Order.SetDeposit(deposit);
            }
        }

        /// <summary>
        /// Returns the total deposit for the customer order that can be returned
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns>The total deposit to be returned</returns>
        public static decimal GetTotalDepositToBeReturned(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            return retailTransaction.CustomerOrder.DepositToBeReturned;
        }

    }
}
