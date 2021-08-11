using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CustomerOrder;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.POS.Processes;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services
{
    /// <summary>
    /// A service that has all the functionality for the customer orders and quotes within the POS
    /// </summary>
    public partial class CustomerOrderService : ICustomerOrderService
    {
        private readonly Stack<(ISaleLineItem Item, decimal OrderToPickup)> itemsMarkedForPickup = new Stack<(ISaleLineItem, decimal)>();

        /// <summary>
        /// An logging interface that can be used to log errors
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes the Customer order service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }

        /// <summary>
        /// Checks if the necessary payment type configurations exist for the store (Deposit tender)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderType">The type of customer order</param>
        /// <returns></returns>
        public virtual bool ConfigurationsExistForStore(IConnectionManager entry, CustomerOrderType orderType)
        {
            StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.DepositTender);
            if (payment == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.DepositTenderNotConfiguredOnStore, MessageBoxButtons.OK, MessageDialogType.Attention);
                return false;
            }

            CustomerOrderSettings orderSettings = Providers.CustomerOrderSettingsData.Get(entry, orderType);

            // If no configurations exist for the customer order/quote then a customer order/quote cannot be created
            if (orderSettings.Empty())
            {
                string strType = orderType == CustomerOrderType.CustomerOrder ? Properties.Resources.CustomerOrderLowCase : Properties.Resources.QuoteLowCase;
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoCustomerOrderSettingsHaveBeenSet.Replace("#1", strType));
                return false;
            }

            return true;
        }

        /// <summary>
        /// When the Customer order and Quote operations are clicked in the POS this function is called to decide if the transaction can be a customer order or a Quote
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">The type of customer order</param>
        /// <returns>If true then the transaction can become a customer order or quote</returns>
        public virtual bool CanBeCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            /*******************************************************************************************************

                If for some reason the transaction cannot be a customer order then display a msg and return false
            
            *******************************************************************************************************/

            if (orderType == CustomerOrderType.Quote && retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerOrderCannotBeChangedToAQuote);
                return false;
            }

            if (orderType == CustomerOrderType.CustomerOrder && retailTransaction.CustomerOrder.OrderType == CustomerOrderType.Quote)
            {
                string msg = Properties.Resources.YouAreConvertingAQuoteToACustomerOrder + Environment.NewLine + Properties.Resources.DoYouWantToContinue;
                if (Interfaces.Services.DialogService(entry).ShowMessage(msg, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.No)
                {
                    return false;
                }
            }

            //If the customer order is New and all items are for pickup then this isn't a customer order
            if (retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New && retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) == retailTransaction.SaleItems.Count(c => !c.Voided))
            {
                string type = orderType == CustomerOrderType.CustomerOrder ? Properties.Resources.CustomerOrderLowCase : Properties.Resources.QuoteLowCase;
                if (Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ThisCustomerOrderHasNoItemsOnOrder.Replace("#1", type) + " " + Properties.Resources.DoYouWantToChangeToNormalSale, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                {
                    retailTransaction.CustomerOrder.Clear();
                    foreach (ISaleLineItem item in retailTransaction.SaleItems)
                    {
                        item.Order.Clear();
                    }

                    //Update the receipt control to reflect the changed sale
                    ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                    ((POSApp)settings.POSApp).SetTransaction(retailTransaction);
                }
                return false;
            }

            //If there are items that have been returned on the sale then this cannot be a customer order
            if (retailTransaction.SaleIsReturnSale || retailTransaction.SaleItems.Count(c => !c.Voided && c.Quantity < decimal.Zero) > 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(orderType == CustomerOrderType.CustomerOrder ? 
                                                                     Properties.Resources.ReturnedItemsCannotBeIncludedInACustomerOrder : 
                                                                     Properties.Resources.ReturnedItemsCannotBeIncludedInAQuote);
                return false;
            }

            if (!AllItemsAreAllowed(entry, retailTransaction, orderType))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if all the items on the customer order are allowed to be a part of it
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">The type of customer order</param>
        /// <returns>True if all the items on the transaction are allowed to be on a customer order</returns>
        protected bool AllItemsAreAllowed(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            if (retailTransaction.SaleItems.Any(a => !a.Voided && a is GiftCertificateItem))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(orderType == CustomerOrderType.CustomerOrder ?
                                                                     Properties.Resources.GiftCertificatesAreNotAllowedOnCustomerOrder :
                                                                     Properties.Resources.GiftCertificatesAreNotAllowedOnQuotes);
                return false;
            }

            if (retailTransaction.SaleItems.Any(a => !a.Voided && a is IncomeExpenseItem))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(orderType == CustomerOrderType.CustomerOrder ?
                                                                     Properties.Resources.IncomeAccountNotAllowedOnCustomerOrder :
                                                                     Properties.Resources.IncomeAccountNotAllowedOnQuote);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a customer order, sets all the properties needed and saves the information through the site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customerID">The customer that was selected for the customer order/quote</param>
        /// <param name="saleLines">The items on the sale</param>
        /// <param name="tenderLines">The tender lines on the sale</param>
        /// <param name="order">Information about the order that is being created</param>
        public virtual void CreateCustomerOrder(IConnectionManager entry, RecordIdentifier customerID, LinkedList<ISaleLineItem> saleLines, List<ITenderLineItem> tenderLines, CustomerOrderItem order)
        {
            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            RetailTransaction transaction = new RetailTransaction((string)entry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice);
            Interfaces.Services.TransactionService(entry).LoadTransactionStatus(entry, transaction);

            if (customerID != RecordIdentifier.Empty)
            {
                Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal);
                transaction.Add(customer);
            }

            transaction.CustomerOrder = (CustomerOrderItem)order.Clone();

            if (order.OrderType != CustomerOrderType.Quote)
            {
                foreach (ISaleLineItem item in saleLines)
                {
                    if (item.Order.Empty())
                    {
                        item.Order.ToPickUp = item.Quantity;
                        item.Order.Ordered = item.Quantity;
                        item.Order.Received = decimal.Zero;
                        item.Order.FullyReceived = false;
                    }

                    transaction.Add(item);
                }
            }

            if (tenderLines != null)
            {
                foreach (ITenderLineItem item in tenderLines)
                {
                    item.PaidDeposit = true;
                    transaction.Add(item);
                }
            }

            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, transaction, null);
            SaveCustomerOrder(entry, transaction, transaction.CustomerOrder.Status);

            Interfaces.Services.TransactionService(entry).ReturnSequencedNumbers(entry, transaction, false, true);
        }

        /// <summary>
        /// Displays a dialog to let the user enter information about the customer order / quote and then saves the information entered for the cstomer order
        /// If no deposit is to be paid a receipt is printed and the quote/customer order is cleared from the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of customer order is being created/viewed; Quote or Customer order</param>
        /// <param name="action">What is the action that was selected (if the dialog is being displayed after an action was selected)</param>
        /// <param name="updateStock">If true then the Reserve stock check box on the dialog is checked otherwise it is unchecked</param>
        public virtual void CreateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType, CustomerOrderAction action, bool updateStock)
        {
            //If a Quote is being turned into a customer order we need to update the Customer order information on the retail transaction
            if (!retailTransaction.CustomerOrder.Empty() && retailTransaction.CustomerOrder.OrderType == CustomerOrderType.Quote && orderType == CustomerOrderType.CustomerOrder)
            {
                retailTransaction.CustomerOrder.OrderType = CustomerOrderType.CustomerOrder;
                retailTransaction.CustomerOrder.UpdateStock = updateStock;
                retailTransaction.CustomerOrder.ConvertedFrom = CustomerOrderType.Quote;
            }

            bool newCustomerOrder = retailTransaction.CustomerOrder.Empty();
            if (newCustomerOrder)
            {
                //The reserve stock option is only available for a new customer order
                //after that it cannot be changed so only new customer orders should set this property
                retailTransaction.CustomerOrder.UpdateStock = updateStock;
            }
            
            CalculateMinDeposit(entry, retailTransaction, orderType);

            CustomerOrderDetails dlg = new CustomerOrderDetails(entry, retailTransaction, orderType, action);
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                UpdatePaymentInformation(entry, retailTransaction);

                if (dlg.SaveChanges)
                {
                    SaveCustomerOrder(entry, retailTransaction);

                    //If the action is partial pickup then the transaction cannot be cancelled because ConcludeTransaction needs to be done
                    if (retailTransaction.CustomerOrder.DepositToBePaid == decimal.Zero && !retailTransaction.CustomerOrder.HasAdditionalPayment && action != CustomerOrderAction.PartialPickUp)
                    {
                        PrintCustomerOrderInformation(entry, retailTransaction, false);
                        retailTransaction.EntryStatus = TransactionStatus.Cancelled;
                    }

                    if (retailTransaction.CustomerOrder.HasAdditionalPayment)
                    {
                        UpdateItemInformation(entry, retailTransaction);
                    }
                }

                POSFormsManager.ShowPOSStatusPanelText(newCustomerOrder ? Properties.Resources.CustomerOrderCreated.Replace("#1", (string)retailTransaction.CustomerOrder.Reference) : Properties.Resources.CustomerOrderUpdated.Replace("#1", (string)retailTransaction.CustomerOrder.Reference));
            }
            else
            {
                retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.Cancel;
                POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.CustomerOrderCancelled + " " + Properties.Resources.AllChangesHaveBeenReverted);
            }
        }

        /// <summary>
        /// Displays a dialog to let the user enter information about the customer order / quote and then saves the information entered for the cstomer order
        /// If no deposit is to be paid a receipt is printed and the quote/customer order is cleared from the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of customer order is being created/viewed; Quote or Customer order</param>
        /// <param name="updateStock">If true then the Reserve stock check box on the dialog is checked otherwise it is unchecked</param>
        public virtual void CreateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType, bool updateStock)
        {
            CreateCustomerOrder(entry, retailTransaction, orderType, CustomerOrderAction.None, updateStock);
        }

        /// <summary>
        /// Displays a dialog to recall either a customer order or a quote
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of customer order is being created/viewed; Quote or Customer order</param>
        /// <returns>The selected customer order / quote</returns>
        public virtual IRetailTransaction RecallCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            try
            {
                Interfaces.Services.DialogService(entry).UpdateStatusDialog("");

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                RecallCustomerOrder dlg = new RecallCustomerOrder(entry, retailTransaction, orderType);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.SelectedTransaction;
                }

                //Cancel the current transaction so that the POS will reset itself.
                retailTransaction.EntryStatus = TransactionStatus.Cancelled;
                return retailTransaction;
            }
            finally
            {
                Interfaces.Services.DialogService(entry).CloseStatusDialog();
            }
        }

        /// <summary>
        /// Updates deposit and payment infromation depending on the status of the customer order and the action being taken
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void UpdatePaymentInformation(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //If there are no items to be paid
            if (retailTransaction.SaleItems.Count(c => c.Order.NewDepositsToBePaid()) > 0 || retailTransaction.CustomerOrder.DepositOverriden)
            {
                CalculateMinDeposit(entry, retailTransaction, retailTransaction.CustomerOrder.OrderType);
            }
            
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PayDeposit)
            {
                decimal totalItemDeposits = CalculationHelper.GetTotalDeposit(entry, CustomerOrderSummaries.ToPickUp, retailTransaction) +
                                            CalculationHelper.GetTotalDeposit(entry, CustomerOrderSummaries.OnOrder, retailTransaction);
                retailTransaction.CustomerOrder.DepositToBePaid = totalItemDeposits;
            }
            
            else if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PartialPickUp)
            {
                retailTransaction.CustomerOrder.DepositToBePaid = CalculatePartialPickupRemainingPayment(entry, retailTransaction, true);
            }
        }

        /// <summary>
        /// Calculates the remaining total of the customer order after items have been picked up and paid for
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="includeLastTenderLine">If the customer is over tendering we need to ignore the last payment line</param>
        /// <returns></returns>
        protected virtual decimal CalculatePartialPickupRemainingPayment(IConnectionManager entry, IRetailTransaction retailTransaction, bool includeLastTenderLine)
        {
            if (retailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.PartialPickUp)
            {
                return decimal.Zero;
            }

            //Get all the deposits (if any) that have not yet been paid for items that are not going to be picked up
            decimal totalItemDeposits = CalculationHelper.GetTotalDeposit(entry, CustomerOrderSummaries.OnOrder, retailTransaction);

            //Get the total to be paid for the items that are to be picked up
            decimal totalToBePaid = CalculationHelper.GetTotalToBePaid(entry, CustomerOrderSummaries.ToPickUp, retailTransaction, false);

            decimal previousTenders = retailTransaction.TenderLines.Where(w => !w.PaidDeposit && !w.Voided).Sum(s => s.Amount);

            //If the customer order is new then no deposits should be considered
            if (retailTransaction.CustomerOrder.Status != CustomerOrderStatus.New)
            {
                totalToBePaid -= CalculationHelper.GetTotalAlreadyPaidDeposit(entry, CustomerOrderSummaries.ToPickUp, retailTransaction);
            }

            totalToBePaid += totalItemDeposits;

            //If this is over tendering - then return the amount to be paid minus partial payments
            if (includeLastTenderLine)
            {
                ITenderLineItem lastTenderLineItem = retailTransaction.TenderLines.LastOrDefault(w => !w.PaidDeposit && !w.Voided);
                if (lastTenderLineItem != null)
                {
                    previousTenders -= lastTenderLineItem.Amount;
                }
            }

            totalToBePaid -= previousTenders;

            var rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);
            return rounding.Round(entry, totalToBePaid, entry.CurrentStoreID);
        }

        /// <summary>
        /// Updates a set of deposit lines as paid. The set is controlled by the <see cref="CustomerOrderSummaries"/> parameter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="summaries">Which set of items should have their deposits updated</param>
        /// <returns></returns>
        public virtual bool UpdateDepositInformation(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderSummaries summaries)
        {
            if (summaries == CustomerOrderSummaries.All)
            {
                foreach (ISaleLineItem saleLine in retailTransaction.SaleItems.Where(w => !w.Voided))
                {
                    saleLine.Order.SetAllDepositsAsPaid();
                }

                return true;
            }
            else if (summaries == CustomerOrderSummaries.OnOrder)
            {
                foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided && !w.Order.FullyReceived))
                {
                    if (item.Order.ToPickUp == 0)
                    {
                        item.Order.SetAllDepositsAsPaid();
                    }
                }

                return true;
            }
            else if (summaries == CustomerOrderSummaries.ToPickUp)
            {
                foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided && !w.Order.FullyReceived && w.Order.ToPickUp > 0))
                {
                    if (item.Order.ToPickUp == item.Order.Ordered)
                    {
                        item.Order.SetAllDepositsAsPaid();
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the customer order status and then saves the customer order through the Site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="status">The status to be set on the customer order</param>
        /// <returns></returns>
        public virtual RecordIdentifier SaveCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderStatus status)
        {
            retailTransaction.CustomerOrder.Status = status;
            return SaveCustomerOrder(entry, retailTransaction);
        }

        /// <summary>
        /// Saves the customer order through the site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="order">The order that is being saved</param>
        public virtual void SaveCustomerOrder(IConnectionManager entry, CustomerOrder order)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            Interfaces.Services.SiteServiceService(entry).SaveCustomerOrder(entry, settings.SiteServiceProfile, order);
        }

        /// <summary>
        /// Saves the customer order through the site service without saving the order XML
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="order">The order that is being saved</param>
        public virtual void SaveCustomerOrderDetails(IConnectionManager entry, CustomerOrder order)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            Interfaces.Services.SiteServiceService(entry).SaveCustomerOrderDetails(entry, settings.SiteServiceProfile, order);
        }

        /// <summary>
        /// Saves the customer order and updates the stock by creating a stock reservation 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="updateStock">If true then a stock reservation is created</param>
        /// <returns>The unique ID of the customer order</returns>
        public virtual RecordIdentifier SaveCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, bool updateStock = true)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            CustomerOrderStatus orgStatus = retailTransaction.CustomerOrder.Status;

            //A customer order should never be saved with status New - only Open
            retailTransaction.CustomerOrder.Status = retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New ? CustomerOrderStatus.Open : retailTransaction.CustomerOrder.Status;

            if (retailTransaction.SaleItems.Count(c => !c.Voided) == 0)
            {
                retailTransaction.CustomerOrder.Status = CustomerOrderStatus.Cancelled;
            }

            else if (retailTransaction.SaleItems.Count(c => !c.Voided && !c.Order.FullyReceived) == 0)
            {
                retailTransaction.CustomerOrder.Status = retailTransaction.CustomerOrder.Status == CustomerOrderStatus.Open ? CustomerOrderStatus.Closed : retailTransaction.CustomerOrder.Status;
            }
            
            retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.None;

            UpdateItemInformation(entry, retailTransaction);

            RecordIdentifier customerOrderID;

            if (updateStock && (!string.IsNullOrEmpty(retailTransaction.CustomerOrder.ID.StringValue)))
            {
                UpdateStockInformation(entry, retailTransaction);
            }
            else if (updateStock && string.IsNullOrEmpty(retailTransaction.CustomerOrder.ID.StringValue) && orgStatus == CustomerOrderStatus.New)
            {
                //This is a new customer order and it has to be saved first so that we get the Customer order ID that is essential 
                //for the updating of the stock information
                customerOrderID = Interfaces.Services.SiteServiceService(entry).SaveCustomerOrder(entry, settings.SiteServiceProfile, retailTransaction);
                retailTransaction.CustomerOrder.ID = customerOrderID;

                UpdateStockInformation(entry, retailTransaction);
            }

            //The customer order then needs to be saved (again) so that stock reservation information is saved to the central database with the transaction
            customerOrderID = Interfaces.Services.SiteServiceService(entry).SaveCustomerOrder(entry, settings.SiteServiceProfile, retailTransaction);
            retailTransaction.CustomerOrder.ID = customerOrderID;

            //Set the status back to be New if that was the original status
            if (orgStatus == CustomerOrderStatus.New && retailTransaction.CustomerOrder.Status != CustomerOrderStatus.Cancelled)
            {
                retailTransaction.CustomerOrder.Status = CustomerOrderStatus.New;
            }

            return customerOrderID;
        }

        /// <summary>
        /// Updates exclude actions on the items
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        protected virtual void UpdateItemInformation(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            //All items on the transaction will be excluded from the operations here below.
            foreach (ISaleLineItem item in retailTransaction.SaleItems)
            {
                item.ExcludedActions = ExcludedActions.ChangeUnitOfMeasure
                                       | ExcludedActions.ClearQuantity
                                       | ExcludedActions.SetQuantity
                                       | ExcludedActions.PriceOverride
                                       | ExcludedActions.ChangeHospitalityMenuType;

                if (retailTransaction.CustomerOrder.HasAdditionalPayment)
                {
                    item.ExcludedActions |= ExcludedActions.VoidItem;
                }
            }
        }

        /// <summary>
        /// Distributes the minimum deposit on all the items equally
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">The type of customer order</param>
        public virtual void DistributeMinDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            //This can only be done on a NEW customer order
            if (retailTransaction.CustomerOrder.Status != CustomerOrderStatus.New)
            {
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal totalDeposit = decimal.Zero;

            if (retailTransaction.CustomerOrder.MinimumDeposit <= 0M)
            {
                return;
            }

            decimal depositPercentage = (retailTransaction.CustomerOrder.MinimumDeposit * 100 / retailTransaction.NetAmountWithTax) / 100;

            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
            {
                item.Order.Deposits.Clear();

                decimal deposit = Interfaces.Services.RoundingService(entry).Round(entry, item.NetAmountWithTax * depositPercentage, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                item.Order.SetDeposit(deposit);
                
                totalDeposit += deposit;
            }

            if (totalDeposit != retailTransaction.CustomerOrder.MinimumDeposit)
            {
                decimal difference = retailTransaction.CustomerOrder.MinimumDeposit - totalDeposit;
                ISaleLineItem item = retailTransaction.SaleItems.FirstOrDefault(w => !w.Voided);
                if (item != null)
                {
                    IDepositItem deposit = item.Order.Deposits.LastOrDefault();
                    if (deposit != null)
                    {
                        deposit.Deposit += difference;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the amount of the minimum deposit based on the settings. If the customer order does not accept deposits then no calculations are done
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">the type of customer order that is being created</param>
        public virtual void CalculateMinDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            CustomerOrderSettings orderSettings = Providers.CustomerOrderSettingsData.Get(entry, orderType);

            if (!orderSettings.AcceptsDeposits)
            {
                return;
            }

            if (retailTransaction.CustomerOrder.DepositOverriden)
            {
                DistributeMinDeposit(entry, retailTransaction, orderType);
                retailTransaction.CustomerOrder.DepositOverriden = false;
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal totalDeposit = decimal.Zero;

            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
            {
                CalculationHelper.CalculateDeposit(entry, item, orderType, retailTransaction);

                //Add up the total min deposit
                totalDeposit += item.Order.TotalDepositAmount();
            }

            //The total calculated deposit added to the Customer order item on the transaction
            retailTransaction.CustomerOrder.MinimumDeposit = totalDeposit;
        }

        /// <summary>
        /// Displays the action dialog when the user clicks a payment operations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns>The action that was selected</returns>
        public virtual CustomerOrderAction CustomerOrderActions(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (!CanBeCustomerOrder(entry, retailTransaction, retailTransaction.CustomerOrder.OrderType))
            {
                return CustomerOrderAction.None;
            }

            CustomerOrderAction orgAction = retailTransaction.CustomerOrder.CurrentAction;
            
            CustomerOrderActions dlg = new CustomerOrderActions(entry, retailTransaction);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                switch (dlg.SelectedAction)
                {
                    case CustomerOrderAction.ReturnPartialDeposit:
                        ConfirmDepositAmountToReturn(entry, retailTransaction);
                        break;
                    case CustomerOrderAction.SaveChanges:
                        SaveCustomerOrder(entry, retailTransaction, CustomerOrderStatus.Open);
                        break;
                    case CustomerOrderAction.SaveChangesAndPrintReceiptCopy:
                        SaveCustomerOrder(entry, retailTransaction, CustomerOrderStatus.Open);
                        PrintCustomerOrderInformation(entry, retailTransaction, true);
                        break;
                    case CustomerOrderAction.PartialPickUp:
                        UpdateCustomerOrder(entry, retailTransaction, dlg.SelectedAction);
                        if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.Cancel)
                        {
                            dlg.SelectedAction = CustomerOrderAction.Cancel;
                        }
                        else if (retailTransaction.CustomerOrder.HasAdditionalPayment)
                        {
                            dlg.SelectedAction = CustomerOrderAction.AdditionalPayment;
                        }
                        break;
                    case CustomerOrderAction.FullPickup:
                        MarkAllRemainingItemsForPickup(entry, retailTransaction);
                        break;
                    case CustomerOrderAction.CancelQuote:
                        SaveCustomerOrder(entry, retailTransaction, CustomerOrderStatus.Cancelled);
                        break;
                    case CustomerOrderAction.ContinueToPayment:
                        if (retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New)
                        {
                            dlg.SelectedAction = CustomerOrderAction.PartialPickUp;
                            break;
                        }
                        dlg.SelectedAction = orgAction;
                        break;
                    //Nothing needs to be done - just continue with the action
                    case CustomerOrderAction.AdditionalPayment:
                        break;
                }
                retailTransaction.CustomerOrder.CurrentAction = dlg.SelectedAction;
            }

            return retailTransaction.CustomerOrder.CurrentAction;
        }

        /// <summary>
        /// Updates information on a customer order through the site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="action">The action that was selected when the user clicked the payment operation</param>
        public virtual void UpdateCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderAction action)
        {
            CreateCustomerOrder(entry, retailTransaction, retailTransaction.CustomerOrder.OrderType, action, retailTransaction.CustomerOrder.UpdateStock);
        }

        /// <summary>
        /// Mark all items that remain on order for pickup
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void MarkAllRemainingItemsForPickup(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => w.Order.Received < w.Quantity))
            {
                item.Order.ToPickUp = item.Quantity - item.Order.Received;
            }
        }

        /// <summary>
        /// Calculates the amount to be paid by the customer when the customer order is being finalized. This can both be a deposit, a returned deposit, or a pickup
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="includeLastTenderLine">If this is over tendering - then the last tender line on the transaction should be ignored from calculations</param>
        /// <returns></returns>
        public virtual decimal CalculateAmountToBePaid(IConnectionManager entry, IRetailTransaction retailTransaction, bool includeLastTenderLine)
        {
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.AdditionalPayment)
            {
                return retailTransaction.CustomerOrder.AdditionalPayment;
            }

            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnFullDeposit || 
                retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnPartialDeposit)
            {
                decimal otherTenderLines = retailTransaction.TenderLines.Where(w => !w.PaidDeposit && !w.Voided).Sum(s => s.Amount);
                decimal depositToBeReturned = retailTransaction.CustomerOrder.DepositToBeReturned > decimal.Zero ? retailTransaction.CustomerOrder.DepositToBeReturned*-1 : retailTransaction.CustomerOrder.DepositToBeReturned;

                return depositToBeReturned - otherTenderLines;
            }

            //No items to pick up - then only the Deposit to be paid should be returned
            if (retailTransaction.SaleItems.Count(c => c.Order.ToPickUp > 0) == 0)
            {
                return retailTransaction.CustomerOrder.DepositToBePaid;
            }
            
            //All the items are being picked up, so the rest of the balance should be paid
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.FullPickup)
            {
                return retailTransaction.TransSalePmtDiff;
            }

            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PartialPickUp)
            {
                return CalculatePartialPickupRemainingPayment(entry, retailTransaction, includeLastTenderLine);
            }

            return retailTransaction.TransSalePmtDiff;
        }

        /// <summary>
        /// Calculates the amount to be paid on the customer order, taking into account deposits paid and partial payments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="includeLastTenderLine">If the customer is over tendering then the last tender line has to be excluded from calculations</param>
        /// <returns>The amount to be paid</returns>
        public virtual decimal CalculateAmountToBeTendered(IConnectionManager entry, IRetailTransaction retailTransaction, bool includeLastTenderLine)
        {
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnFullDeposit ||
                retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.ReturnPartialDeposit)
            {
                return retailTransaction.CustomerOrder.DepositToBeReturned > decimal.Zero ? retailTransaction.CustomerOrder.DepositToBeReturned * -1 : retailTransaction.CustomerOrder.DepositToBeReturned;
            }

            //No items to pick up - then only the Deposit to be paid should be returned
            if (retailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.AdditionalPayment && retailTransaction.SaleItems.Count(c => c.Order.ToPickUp > 0) == 0)
            {
                return retailTransaction.CustomerOrder.DepositToBePaid;
            }

            //All the items are being picked up, so the rest of the balance should be paid
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.FullPickup)
            {
                decimal previouslyPaid = retailTransaction.TenderLines.Where(w => !w.Voided && w.PaidDeposit).Sum(s => s.Amount);
                return retailTransaction.NetAmountWithTax - previouslyPaid;
            }

            //Some of the items are being picked up
            if (retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.PartialPickUp || 
                retailTransaction.CustomerOrder.CurrentAction == CustomerOrderAction.AdditionalPayment)
            {
                //Get all the deposits (if any) that have not yet been paid for items that are not going to be picked up
                decimal totalItemDeposits = CalculationHelper.GetTotalDeposit(entry, CustomerOrderSummaries.OnOrder, retailTransaction);

                //Get the total to be paid for the items that are to be picked up
                decimal totalToBePaid = CalculationHelper.GetTotalToBePaid(entry, CustomerOrderSummaries.ToPickUp, retailTransaction, false);

                //If the customer order is new then no previous deposits should be considered
                if (retailTransaction.CustomerOrder.Status != CustomerOrderStatus.New)
                {
                    totalToBePaid -= CalculationHelper.GetTotalAlreadyPaidDeposit(entry, CustomerOrderSummaries.ToPickUp, retailTransaction);
                }

                totalToBePaid += totalItemDeposits;

                //If the customer order has additional payments to be paid then add them to the total
                totalToBePaid += retailTransaction.CustomerOrder.HasAdditionalPayment ? retailTransaction.CustomerOrder.AdditionalPayment : decimal.Zero;

                return totalToBePaid;
            }

            if (retailTransaction.GrossAmountWithTax < retailTransaction.Payment)
            {
                return retailTransaction.GrossAmountWithTax;
            }

            return retailTransaction.TransSalePmtDiff;
        }


        /// <summary>
        /// Searches for customer order using the criteria set in the <see cref="CustomerOrderSearch"/> object
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="totalRecordsMatching">The total records matching the search</param>
        /// <param name="numberOfRecordsToReturn">The total number of records returned</param>
        /// <param name="searchCriteria">The search criteria for the search</param>
        /// <param name="closeConnection">If true the connection to the Site service is closed after the search</param>
        /// <returns>A list of customer orders that fit the search criteria</returns>
        public virtual List<CustomerOrder> Search(IConnectionManager entry,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria,
            bool closeConnection = true
            )
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            return Interfaces.Services.SiteServiceService(entry).CustomerOrderSearch(entry, settings.SiteServiceProfile, out totalRecordsMatching,
                numberOfRecordsToReturn, searchCriteria, closeConnection);
        }

        /// <summary>
        /// A function called by the Void transaction operation within the POS to update the customer order accordingly
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void VoidCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            if (retailTransaction.CustomerOrder.HasAdditionalPayment)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerOrderWithAdditionalPaymentsCannotBeVoided);
                return;
            }

            //Checks to see if the transaction can be voided are done in the Void transaction operation they do not need to be done here
            if (Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AreYouSureCancelCustomerOrder.Replace("#1", retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder ? Properties.Resources.CustomerOrderLowCase : Properties.Resources.QuoteLowCase), MessageBoxButtons.YesNo, MessageDialogType.Generic) == DialogResult.Yes)
            {
                //To make sure that the UpdateStockInformation (updating reservations) works we need to set all items as ReservationDone = false
                foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided))
                {
                    item.Order.ReservationDone = false;
                }
                SaveCustomerOrder(entry, retailTransaction, CustomerOrderStatus.Cancelled);
            }
        }

        #region Voiding items

        /// <summary>
        /// A function called from the Void item operation within the POS to decide if the item can be voided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="item">The item to be voided</param>
        /// <returns>True if the item can be voided</returns>
        public virtual bool ItemCanBeVoided(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item)
        {
            if (item.Order.Empty())
            {
                return true;
            }

            //If the item is already voided then it cannot be unvoided, the item needs to be added again to the order
            if (item.Voided)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AnItemOnCustomerOrderCannotBeUnvoidedPleaseAddItAgain, MessageBoxButtons.OK, MessageDialogType.Generic);
                return false;
            }

            //If the item has linked items then they need to be voided first
            if (item.IsReferencedByLinkItems)
            {
                if (retailTransaction.SaleItems.Count(c => (c.IsLinkedItem || c.IsInfoCodeItem) && c.LinkedToLineId == item.LineId && c.AssemblyParentLineID != item.LineId && !c.Voided) > 0)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ItemHasLinkedItemsCannotBeVoided, MessageBoxButtons.OK, MessageDialogType.Generic);
                    return false;
                }
            }

            if (item.Order.FullyReceived)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ThisItemHasBeenFullyReceivedAndCannotBeVoided, MessageBoxButtons.OK, MessageDialogType.Generic);
                return false;
            }

            if (item.Order.Received > 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PartsOfThisItemHasBeenReceivedAndCannotBeVoided, MessageBoxButtons.OK, MessageDialogType.Generic);
                return false;
            }

            if (item.Order.ToPickUp > 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ThisItemHasBeenSelectedForPickupCannotBeVoided, MessageBoxButtons.OK, MessageDialogType.Generic);
                return false;
            }

            return true;
        }

        /// <summary>
        /// After the item has already been voided the deposit needs to be marked returnable. If other items are on the customer order the deposit
        /// is distributed to the other items otherwise the user is told that a payment button needs to be clicked to return the deposits
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="item">The item to be voided</param>
        public virtual void VoidItemDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, ISaleLineItem item)
        {
            /***************************************************************

                THE ITEM HAS ALREADY BEEN VOIDED HERE BY THE VOID ITEM OPERATION

            ***************************************************************/

            decimal voidedDeposit = decimal.Zero;

            foreach (IDepositItem deposit in item.Order.Deposits.Where(w => w.Status == DepositsStatus.Normal || w.Status == DepositsStatus.Distributed))
            {
                //If deposit has already been paid then mark it to be returned
                if (deposit.DepositPaid)
                {
                    voidedDeposit += deposit.Deposit;
                    deposit.Status = DepositsStatus.Returned;
                }
                else
                {
                    deposit.Clear();
                }
            }

            //Add any additional payments that might have been paid for
            if (retailTransaction.CustomerOrder.AdditionalPaymentLines != null && retailTransaction.CustomerOrder.AdditionalPaymentLines.Any())
            {
                voidedDeposit += CalculationHelper.GetTotalAlreadyPaidAdditionalPayments(entry, retailTransaction);
            }

            if (voidedDeposit == decimal.Zero)
            {
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            string amt = Interfaces.Services.RoundingService(entry).RoundString(entry, voidedDeposit, settings.Store.Currency, true, CacheType.CacheTypeApplicationLifeTime);

            if (retailTransaction.SaleItems.Count(c => !c.Voided && !c.Order.FullyReceived) == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ClickAPaymentButtonToGetDeposit.Replace("#1", amt));
            }
            else if (voidedDeposit <= retailTransaction.TransSalePmtDiff)
            {
                //Display a msg telling the user that the deposit is going to be distributed
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.DepositWillBeDistributedToOtherItems.Replace("#1", amt));
            }
            else
            {
                //Display a msg telling the user that the deposit is going to be used to pay up all the other items
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.DepositWillBeUsedToPayUpOtherItems.Replace("#1", amt));
            }

            //Distribute the deposit to the other items
            DistributeVoidedDeposit(entry, retailTransaction, voidedDeposit);

            // If the last action is not partial pick up, then reset the action so that the POS core won't try to do anything else
            // In case the last action is partial pickup we want the current action to remain the same,
            // otherwise we won't be able to finalize the customer order by paying it after voiding an item containing a paid deposit
            // The ContinueToPayment action rely on this action. If this action is reset to None, then it is impossible to continue to payment (see method CustomeOrderActions())
            if (retailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.PartialPickUp)
            {
                retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.None;
            }

            //The operation Void Item will then take over and actually void the item
        }

        /// <summary>
        /// The user needs to confirm how much of the deposit is to be kept when the customer order is being voided
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void ConfirmDepositAmountToReturn(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            decimal depositToKeep = decimal.Zero;
            
            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (Interfaces.Services.DialogService(entry).NumpadInput(settings, ref depositToKeep, Properties.Resources.DepositToKeep, Properties.Resources.Amount, true, DecimalSettingEnum.Prices) == DialogResult.Cancel)
            {
                depositToKeep = decimal.Zero;
            }

            retailTransaction.CustomerOrder.DepositToBeReturned += depositToKeep;
        }

        /// <summary>
        /// Distributes the deposit on the voided items onto the other items in the customer order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="voidedDeposit">The deposit that is being voided</param>
        protected virtual void DistributeVoidedDeposit(IConnectionManager entry, IRetailTransaction retailTransaction, decimal voidedDeposit)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            decimal totalBalance = retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.DepositAlreadyPaid() != decimal.Zero && !w.Order.FullyReceived).Sum(s => s.NetAmountWithTax);
            decimal totalDeposit = decimal.Zero;

            //If there is at least one item that has not been voided
            if (totalBalance != decimal.Zero)
            {
                decimal depositPercent = voidedDeposit/totalBalance;

                depositPercent = depositPercent > 1M ? 1M : depositPercent; //This cannot be higher than 100%

                foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.DepositAlreadyPaid() != decimal.Zero && !w.Order.FullyReceived))
                {
                    decimal itemNetAmount = item.NetAmountWithTax;
                    decimal deposit = Interfaces.Services.RoundingService(entry).Round(entry, itemNetAmount*depositPercent, settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);

                    decimal depositAlreadyPaid = item.Order.DepositAlreadyPaid();

                    //If the deposit is higher than the net amount of the item
                    if (deposit > (itemNetAmount - depositAlreadyPaid))
                    {
                        deposit = itemNetAmount - depositAlreadyPaid;
                    }

                    item.Order.Deposits.Add(new DepositItem(deposit, true, DepositsStatus.Distributed));
                    totalDeposit += deposit;
                }

                if (totalDeposit < voidedDeposit)
                {
                    decimal difference = voidedDeposit - totalDeposit;
                    foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.DepositAlreadyPaid() != decimal.Zero && !w.Order.FullyReceived))
                    {
                        item.Order.Deposits.Add(new DepositItem(difference, true, DepositsStatus.Distributed));
                        totalDeposit += difference;
                        break;
                    }
                }
            }

            if (voidedDeposit - totalDeposit > decimal.Zero)
            {
                retailTransaction.CustomerOrder.DepositToBeReturned = voidedDeposit - totalDeposit;
            }
        }

        #endregion

        #region Reserve stock

        /// <summary>
        /// Checks if any of the stock reservation reason codes are missing and create them through the site service
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="reasonCodes">The list of reason codes to be checked</param>
        /// <param name="closeConnection">If true the connection to the site service is closed after the update</param>
        protected virtual void UpdateReasonCodes(IConnectionManager entry, List<ReasonCode> reasonCodes, bool closeConnection)
        {
            bool updateHO = false;
            ReasonCode toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID && f.Text == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID);
            if (toUpdate != null)
            {
                toUpdate.Text = Properties.Resources.ReasonCodeReserveStockForOrder;
                updateHO = true;
            }

            toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID && f.Text == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID);
            if (toUpdate != null)
            {
                toUpdate.Text = Properties.Resources.ReasonCodePickUpFromOrder;
                updateHO = true;
            }

            toUpdate = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID && f.Text == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID);
            if (toUpdate != null)
            {
                toUpdate.Text = Properties.Resources.ReasonCodeVoidStockFromOrder;
                updateHO = true;
            }

            if (updateHO)
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                Interfaces.Services.SiteServiceService(entry).UpdateReasonCodes(entry, settings.SiteServiceProfile, reasonCodes, InventoryJournalTypeEnum.Reservation, closeConnection);
            }
        }

        /// <summary>
        /// Retrieves the existing reason codes through the site service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed after the retrieving of the list is done</param>
        /// <returns></returns>
        protected virtual List<ReasonCode> GetReasonCodes(IConnectionManager entry, bool closeConnection)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            List<ReasonCode> reasonCodes = Interfaces.Services.SiteServiceService(entry).GetReasonCodes(entry, settings.SiteServiceProfile, InventoryJournalTypeEnum.Reservation, closeConnection);

            UpdateReasonCodes(entry, reasonCodes, closeConnection);

            return reasonCodes;
        }

        /// <summary>
        /// Updates the stock reservation information for the customer order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        public virtual void UpdateStockInformation(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            string logStr = "Started; ";

            try
            {
                try
                {
                    if (retailTransaction is DepositTransaction)
                    {
                        logStr += "deposit trans";
                        return;
                    }

                    if (retailTransaction.CustomerOrder.Empty())
                    {
                        logStr += "empty order";
                        return;
                    }

                    //If the customer order is configured to not update stock then do nothing
                    //this configuration can only be changed when the customer order is created
                    if (!retailTransaction.CustomerOrder.UpdateStock)
                    {
                        logStr += "update stock = false";
                        return;
                    }

                    logStr += "1;";

                    //items that are voided but have reservation quantity that needs to be cleared
                    bool voided = retailTransaction.SaleItems.Count(c => c.Voided && c.Order.ReservationQty > 0) > 0;
                    //items that have not had the stock reserved
                    bool notReserved = retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ReservationQty == 0) > 0;
                    //items that are being picked up
                    bool toPickup = retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) > 0;
                    //any items on the transaction that haven't been already reserved in this session
                    //ReservationDone is set to false when the order is recalled
                    bool anyOrdersLeft = retailTransaction.SaleItems.Count(c => !c.Order.ReservationDone) > 0;

                    if (voided || notReserved || toPickup || anyOrdersLeft)
                    {
                        logStr += "2;";
                        ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                        InventoryAdjustment journal = Interfaces.Services.SiteServiceService(entry).GetInventoryAdjustment(entry, settings.SiteServiceProfile, retailTransaction.CustomerOrder.ID, false);

                        //If a reservation journal is not found for the customer order and the status is cancelled (voided) then 
                        //there is no need to update anything
                        if (journal == null && retailTransaction.CustomerOrder.Status == CustomerOrderStatus.Cancelled)
                        {
                            return;
                        }

                        List<ReasonCode> reasonCodes = GetReasonCodes(entry, false);

                        DataEntity voidingReasoncode = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstVoidStockFromOrderReasonID) ?? new DataEntity();
                        DataEntity pickupReasoncode = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstPickupFromOrderReasonID) ?? new DataEntity();
                        DataEntity reserveReasoncode = reasonCodes.FirstOrDefault(f => f.ID == CustomerOrderReasonConstants.ConstReserveStockToOrderReasonID) ?? new DataEntity();

                        if (journal == null)
                        {
                            logStr += "3;";
                            journal = new InventoryAdjustment();
                            journal.MasterID = (Guid) retailTransaction.CustomerOrder.ID;
                            journal.Text = (string) retailTransaction.CustomerOrder.Reference;
                            journal.JournalType = InventoryJournalTypeEnum.Reservation;
                            journal.CreatedDateTime = DateTime.Now;

                            RecordIdentifier deliveryLocation = retailTransaction.CustomerOrder.DeliveryLocation ?? RecordIdentifier.Empty;
                            deliveryLocation = string.IsNullOrWhiteSpace(deliveryLocation.StringValue) ? entry.CurrentStoreID : deliveryLocation;

                            journal.StoreId = deliveryLocation;

                            journal = Interfaces.Services.SiteServiceService(entry).SaveInventoryAdjustment(entry, settings.SiteServiceProfile, journal, false);
                        }
                        else if (journal.StoreId != retailTransaction.CustomerOrder.DeliveryLocation)
                        {
                            logStr += "4;";
                            journal.StoreId = retailTransaction.CustomerOrder.DeliveryLocation;
                            Interfaces.Services.SiteServiceService(entry).MoveInventoryAdjustment(entry, settings.SiteServiceProfile, journal, false);
                            logStr += "5;";
                        }
                        
                        foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Order.FullyReceived && !w.Order.ReservationDone))
                        {
                            logStr += "6;";

                            decimal reserveStock = decimal.Zero;
                            InventoryJournalTransaction reservation = new InventoryJournalTransaction();
                            
                            if ((retailTransaction.CustomerOrder.Status == CustomerOrderStatus.Cancelled || item.Voided) && item.Order.ReservationQty != decimal.Zero)
                            {
                                logStr += "7;";
                                //When removing the stock from reservation the quantity must be positive
                                reserveStock = item.Quantity < decimal.Zero ? item.Quantity*-1 : item.Quantity;
                                reservation.ReasonId = voidingReasoncode.ID;
                            }
                            else if (!item.Voided && (item.Order.ReservationQty != item.Quantity))
                            {
                                logStr += "8;";
                                //When adding the stock to reservation the quantity must be negative
                                reserveStock = (item.Quantity - item.Order.ReservationQty) <= decimal.Zero ? decimal.Zero : (item.Quantity - item.Order.ReservationQty);
                                reserveStock *= -1;
                                reservation.ReasonId = reserveReasoncode.ID;
                            }
                            else if (!item.Voided && (item.Order.ReservationQty == item.Order.ToPickUp))
                            {
                                logStr += "9;";
                                //The items are being picked up so they will be part of the normal stock now
                                //and should be removed from the reservation so the quantity must be positive
                                reserveStock = item.Order.ToPickUp;
                                reservation.ReasonId = pickupReasoncode.ID;
                            }

                            reservation.JournalId = journal.ID;
                            reservation.LineNum = RecordIdentifier.Empty;
                            reservation.TransDate = DateTime.Now;
                            reservation.ItemId = item.ItemId;
                            reservation.UnitID = item.SalesOrderUnitOfMeasure;
                            reservation.Adjustment = reserveStock;
                            reservation.StaffID = entry.CurrentStaffID;

                            reservation.CostPrice = item.CostPrice;

                            if (reservation.Adjustment != decimal.Zero)
                            {
                                logStr += "10;";
                                reservation.InventoryUnitID = Providers.RetailItemData.GetItemUnitID(entry, item.ItemId, RetailItem.UnitTypeEnum.Inventory);

                                reservation.AdjustmentInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry,
                                    item.ItemId,
                                    item.SalesOrderUnitOfMeasure,
                                    reservation.InventoryUnitID,
                                    reservation.Adjustment);

                                Interfaces.Services.SiteServiceService(entry).ReserveStockTransaction(entry, settings.SiteServiceProfile, reservation, journal.StoreId, InventoryTypeEnum.Reservation, false);

                                item.Order.ReservationQty = item.Voided ? decimal.Zero : reserveStock*-1;
                                item.Order.ReservationDone = true;
                                item.Order.DateReserved = new Date(DateTime.Now);
                                item.Order.JournalID = journal.ID;

                                logStr += "11;";
                            }
                        }
                    }
                }
                finally
                {
                    logStr += " Done ";
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, logStr, "CustomerOrderService.UpdateStockInformation");
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        #endregion
    }
}