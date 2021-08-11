using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CustomerOrder;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class CustomerOrderService
    {
        /// <summary>
        /// When concluding the transaction this function is called to make sure the customer order information is correct and updated
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction being concluded</param>
        /// <returns>Returns the transaction with updated information</returns>
        public virtual IPosTransaction ConcludeTransaction(IConnectionManager entry, IPosTransaction posTransaction)
        {
            string logStr = "Started; ";
            try
            {

                //If the transaction isn't a retail transaction then there is no need to go forward
                if (!(posTransaction is RetailTransaction))
                {
                    return posTransaction;
                }

                logStr += "1; ";
                RetailTransaction retailTransaction = posTransaction as RetailTransaction;

                //If the transaction doesn't have a customer order on it then no need to go forward
                if (retailTransaction.CustomerOrder.Empty())
                {
                    return posTransaction;
                }

                retailTransaction.SaleItems.ToList().ForEach(x => x.NoPriceCalculation = true);

                CustomerOrderAction action = retailTransaction.CustomerOrder.CurrentAction;
                CustomerOrderStatus status = retailTransaction.CustomerOrder.Status;

                //If a customer order was being converted from one type to another then this property was set to know what the original type was
                //But now we are concluding the transaction and don't want this information on the customer order any more
                retailTransaction.CustomerOrder.ConvertedFrom = CustomerOrderType.None;

                logStr += "2; ";

                if (status == CustomerOrderStatus.New && retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) > 0)
                {
                    /****************************************************************

                 We are here because the user created a customer order AND is picking up items at the same time
                 The items that are being picked up have to have a Retail transaction with items for pick up
                 The customer then does ONE payment which needs to be represented both on the deposit transaction AND on the retail transaction 
                 So what we do is:

                 1. Cancel the deposit on the items being picked up - as there is no deposit actually being paid but rather the entire amount
                    a. Update the deposit information to be the correct deposit amount - without the payment for the picked up item

                 2. Create the deposit transaction 
                     a. Create a deposit tender line (positive) to make that transaction balance out
                     b. Make sure that the deposit transaction has a link to the Customer order through the Customer Order ID (GUID)
                     c. Save the deposit transaction using TransactionService.ConcludeTransaction.
                         This will print out and save the transaction as it normally would

                 3. Create a retail transaction with the items for pick up
                     a. The actual payment tender line created in payment operation is on this transaction
                     b. A deposit tender line (negative and marked as a Change Line) is created and added to the transaction (as the first tender line)
                         to make the transaction balance out
                     c. The items that are not being picked up are removed from the transaction
                     d. Mark the transaction with a link to the Customer order through the Customer Order ID (GUID)
                     e. Send this transaction onwards through the core POS (it is not specifically saved here) where the 
                         TransactionService will continue with the printing and saving

                 4. Update the Customer order to represent the changes done in steps 1 and 2 and save it again.

                 ****************************************************************/

                    logStr += "3; ";

                    //Cancel the deposit on the items being picked up - as there is no deposit actually being paid but rather the entire amount
                    foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.ToPickUp > 0))
                    {
                        item.Order.SetAllDepositsStatus(DepositsStatus.Cancelled);
                    }

                    //Update the deposit information to be the correct deposit amount - without the payment for the picked up item
                    retailTransaction.CustomerOrder.DepositToBePaid = CalculationHelper.GetTotalAlreadyPaidDeposit(entry, CustomerOrderSummaries.OnOrder, retailTransaction);

                    /***************************************************************

                            DEPOSIT TRANSACTION

                ****************************************************************/

                    logStr += "4; ";

                    retailTransaction.CustomerOrder.Status = CustomerOrderStatus.Open;
                    //Save the customer order as it is now - we have to do that to get a unique ID for the Customer Order
                    // DO NOT RESERVE THE STOCK AT THIS POINT!!
                    SaveCustomerOrder(entry, retailTransaction, false);

                    //Save a copy of the transaction as it was before breaking it up
                    RetailTransaction copyOfRetailTransaction = (RetailTransaction) retailTransaction.Clone();

                    DepositTransaction depositTransaction = PrepareDepositTransaction(entry, retailTransaction);

                    //Create a tender line that has the correct amount for the deposit on the deposit transaction
                    PrepareDepositTenderLine(entry, depositTransaction, retailTransaction);

                    if (depositTransaction.Payment != decimal.Zero)
                    {
                        //Conclude the deposit transaction, print it and save it to the database
                        Interfaces.Services.TransactionService(entry).ConcludeTransaction(entry, depositTransaction);
                    }
                    else
                    {
                        //If no deposit is to be paid/saved then return the transaction ID used for the deposit transaction
                        Interfaces.Services.TransactionService(entry).ReturnSequencedNumbers(entry, depositTransaction, false, true);
                    }

                    //Update the deposit payment information on the copy of the transaction to maintain the correct status
                    UpdateDepositInformation(entry, copyOfRetailTransaction, CustomerOrderSummaries.All);

                    /***************************************************************

                                RETAIL TRANSACTION

                ****************************************************************/

                    logStr += "5; ";

                    //Create the retail transaction for the items being picked up
                    CreatePickUpTransaction(entry, retailTransaction);

                    //Mark the items as picked up on the copy of the transaction to maintain the correct status
                    MarkItemsAsReceived(entry, copyOfRetailTransaction, true);

                    //UpdateStockInformation(entry, copyOfRetailTransaction);

                    //Save the changes done to the customer order by using the copy transaction
                    SaveCustomerOrder(entry, copyOfRetailTransaction, CustomerOrderStatus.Open);

                    PrintCustomerOrderInformation(entry, copyOfRetailTransaction, false);

                    logStr += "6; ";
                    //Return the "pick up" transaction to be saved and printed by the POS
                    return retailTransaction;

                }

                if (action == CustomerOrderAction.PayDeposit)
                {
                    /****************************************************************
                    
                    We are here because the user created or updated a customer order but NO items are being picked up                    
                    The customer then does ONE payment which is represented in a deposit transaction
                    So what we do is:

                    1. Update deposit information on the customer order
                    2. Save the udpated customer order to centrally
                    3. Create the deposit transaction                         
                        a. Make sure that the deposit transaction has a link to the Customer order through the Customer Order ID (GUID)                        
                        b. Send the deposit transaction onwards through the core POS (it is not specifically saved here) where the 
                           TransactionService will continue with the printing and saving                    

                    ****************************************************************/
                    logStr += "7; ";
                    //Update the deposit information and save the customer order as it is before saving the deposit information
                    if (UpdateDepositInformation(entry, retailTransaction, CustomerOrderSummaries.All))
                    {
                        if (SaveCustomerOrder(entry, retailTransaction, CustomerOrderStatus.Open) != RecordIdentifier.Empty)
                        {
                            logStr += "8; ";
                            PrintCustomerOrderInformation(entry, retailTransaction, false);
                            return PrepareDepositTransaction(entry, retailTransaction);
                        }
                    }
                }
                else if (action == CustomerOrderAction.FullPickup ||
                         action == CustomerOrderAction.PartialPickUp)
                {

                    /****************************************************************
                    
                    We are here because the user recalled a customer order and is either doing a partial or full pickup
                    The customer then does ONE payment which is represented in a retail transaction
                    So what we do is:

                    1. Mark the items being picked up as received
                    2. Save the udpated customer order to centrally
                    3. Create a retail transaction
                        a. Items that are not being picked up are removed from the retail transaction
                        b. The payment that was made by the customer is already on the current retail transaction
                        c. Create a deposit tender line that is marked as a Redeemed payment line so that the transaction is fully balanced out                      
                        d. Send the retail transaction onwards through the core POS where the 
                           TransactionService will continue with the printing and saving                    

                    ****************************************************************/
                    logStr += "9; ";
                    if (UpdateDepositInformation(entry, retailTransaction, CustomerOrderSummaries.All))
                    {
                        logStr += "10; ";
                        MarkItemsAsReceived(entry, retailTransaction, false);

                        PrintCustomerOrderInformation(entry, retailTransaction, false);

                        //Save the customer order as Closed and then create a RetailTransaction from the items that are being picked up
                        CustomerOrderStatus orderStatus = action == CustomerOrderAction.FullPickup ? CustomerOrderStatus.Closed : CustomerOrderStatus.Open;

                        //If the action is Partial pickup then we need to make sure that all the items have already been picked up or are being picked up
                        //If so then the order should be set to Closed otherwise it should remain Open
                        if (action == CustomerOrderAction.PartialPickUp)
                        {
                            logStr += "11; ";
                            int fullyRecieved = retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.FullyReceived);
                            int forDelivery = retailTransaction.SaleItems.Count(c => !c.Voided && (c.Order.Ordered == c.Order.ToPickUp));

                            if (retailTransaction.SaleItems.Count(c => !c.Voided) == (fullyRecieved + forDelivery))
                            {
                                orderStatus = CustomerOrderStatus.Closed;
                            }
                        }

                        logStr += "12; ";

                        retailTransaction.CustomerOrder.Status = orderStatus;
                        if (SaveCustomerOrder(entry, retailTransaction, action == CustomerOrderAction.FullPickup) != RecordIdentifier.Empty)
                        {
                            return CreateRetailTransaction(entry, retailTransaction);
                        }
                    }
                }
                else if (action == CustomerOrderAction.ReturnFullDeposit ||
                         action == CustomerOrderAction.ReturnPartialDeposit)
                {
                    /****************************************************************
                    
                    We are here because the user voided items on a customer order and the cashier is either returning the full deposit or partial deposit
                    
                    So what we do is:

                    1. Add a deposit tenderline (negative) to the customer order
                    2. Save the udpated customer order centrally
                    2. Create the deposit transaction                         
                        a. Make sure that the deposit transaction has a link to the Customer order through the Customer Order ID (GUID)                        
                        b. Send the deposit transaction onwards through the core POS (it is not specifically saved here) where the 
                           TransactionService will continue with the printing and saving            

                    ****************************************************************/
                    logStr += "13; ";
                    if (UpdateDepositInformation(entry, retailTransaction, CustomerOrderSummaries.All))
                    {
                        //If all the items on the transaction are voided then close the customer order
                        status = CustomerOrderStatus.Open;
                        if (retailTransaction.SaleItems.Count(c => !c.Voided) == 0)
                        {
                            logStr += "14; ";
                            status = CustomerOrderStatus.Closed;
                        }

                        //The deposit has been returned to the customer so this value needs to be cleared
                        retailTransaction.CustomerOrder.DepositToBeReturned = decimal.Zero;
                        PrintCustomerOrderInformation(entry, retailTransaction, false);

                        logStr += "15; ";

                        if (SaveCustomerOrder(entry, retailTransaction, status) != RecordIdentifier.Empty)
                        {
                            logStr += "16; ";
                            DepositTransaction depositTransaction = PrepareDepositTransaction(entry, retailTransaction);

                            depositTransaction.SaleItems.Clear();
                            
                            depositTransaction.GrossAmount = decimal.Zero;
                            depositTransaction.GrossAmountWithTax = decimal.Zero;
                            depositTransaction.NetAmountWithTax = decimal.Zero;
                            depositTransaction.TaxAmount = decimal.Zero;

                            CreateReturnDepositTenderLine(entry, retailTransaction, depositTransaction);
                            
                            logStr += "17; ";
                            return depositTransaction;
                        }
                    }
                }

                else if (action == CustomerOrderAction.AdditionalPayment)
                {
                    /****************************************************************
                    
                    We are here because the user added an additional payment to the customer order
                    
                    So what we do is:

                    1. Add a deposit tenderline (negative) to the customer order
                    2. Save the udpated customer order centrally
                    2. Create the deposit transaction                         
                        a. Make sure that the deposit transaction has a link to the Customer order through the Customer Order ID (GUID)                        
                        b. Send the deposit transaction onwards through the core POS (it is not specifically saved here) where the 
                           TransactionService will continue with the printing and saving            

                    ****************************************************************/
                    logStr += "18; ";

                    ITenderLineItem lastTender = retailTransaction.TenderLines.LastOrDefault(l => !l.ChangeBack);
                    if (lastTender == null)
                    {
                        return retailTransaction;
                    }

                    //Clear out the additional payment information and Update the customer order 
                    decimal additionalPayment = retailTransaction.CustomerOrder.AdditionalPayment;

                    //Save the additional payment onto the customer order object
                    retailTransaction.CustomerOrder.AdditionalPaymentLines.Add(new PaymentItem(additionalPayment, true, PaymentStatus.Normal));

                    retailTransaction.CustomerOrder.AdditionalPayment = decimal.Zero;
                    retailTransaction.CustomerOrder.HasAdditionalPayment = false;

                    PrintCustomerOrderInformation(entry, retailTransaction, false);

                    logStr += "19; ";

                    if (SaveCustomerOrder(entry, retailTransaction, status) != RecordIdentifier.Empty)
                    {
                        logStr += "20; ";
                        DepositTransaction depositTransaction = PrepareDepositTransaction(entry, retailTransaction);

                        depositTransaction.SaleItems.Clear();

                        //Keep a copy of the original tender lines before clearing them out
                        List<ITenderLineItem> originalTenderLineItems = retailTransaction.TenderLines.Select(tenderLine => (ITenderLineItem) tenderLine.Clone()).ToList();
                        depositTransaction.TenderLines.Clear();

                        depositTransaction.GrossAmount = decimal.Zero;
                        depositTransaction.GrossAmountWithTax = decimal.Zero;
                        depositTransaction.TaxAmount = decimal.Zero;
                        depositTransaction.Payment = decimal.Zero;

                        depositTransaction.NetAmountWithTax = additionalPayment;

                        ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                        
                        StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, new RecordIdentifier(settings.Store.ID, lastTender.TenderTypeId));
                        DepositTenderLineItem depositTenderLine = CreateTenderLine(entry, settings, payment, false, lastTender.Amount);

                        depositTransaction.Add(depositTenderLine);

                        //If the user paid with a higher amount then the required payment then a cash back tender line
                        //is on the transaction. We need to find it and add it after the deposit tender line so that the
                        //deposit transaction will add up
                        ITenderLineItem currentCashBack = originalTenderLineItems.LastOrDefault();
                        if (currentCashBack != null && currentCashBack.ChangeBack)
                        {
                            depositTransaction.Add(currentCashBack);
                        }

                        logStr += "21; ";
                        return depositTransaction;
                    }

                }

                return posTransaction;
            }
            finally
            {
                logStr += "  Done";
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, logStr, "CustomerOrderService.ConcludeTransaction");
            }
        }

        /// <summary>
        /// Creates a deposit transaction 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <returns>A new deposit transaction</returns>
        protected virtual DepositTransaction PrepareDepositTransaction(IConnectionManager entry, RetailTransaction retailTransaction)
        {
            DepositTransaction newTrans = new DepositTransaction(retailTransaction.StoreId, retailTransaction.StoreCurrencyCode, retailTransaction.TaxIncludedInPrice);
            DepositTransaction depositTransaction = newTrans.Clone(retailTransaction);

            Interfaces.Services.TransactionService(entry).ReturnSequencedNumbers(entry, retailTransaction, false, true);

            depositTransaction.TransactionId = "";

            Interfaces.Services.TransactionService(entry).LoadTransactionStatus(entry, depositTransaction);
            depositTransaction.BeginDateTime = DateTime.Now;

            return depositTransaction;
        }

        /// <summary>
        /// Creates the transaction for the items that are being picked up
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        protected virtual void CreatePickUpTransaction(IConnectionManager entry, RetailTransaction retailTransaction)
        {
            //First get the deposit total for the items that are being not being picked up
            decimal depositForPickup = retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.Ordered > 0 && w.Order.ToPickUp == 0).Sum(s => s.Order.TotalDepositAmount());

            //Remove the items not being picked up
            List<ISaleLineItem> itemsToRemove = retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.ToPickUp == 0).ToList();

            foreach (ISaleLineItem toRemove in itemsToRemove)
            {
                retailTransaction.Remove(toRemove, false);
            }

            //Update the date and time of the transaction and items
            retailTransaction.BeginDateTime = DateTime.Now;
            foreach (ISaleLineItem item in retailTransaction.SaleItems)
            {
                item.BeginDateTime = DateTime.Now;
            }

            if (depositForPickup != decimal.Zero)
            {
                TenderLineItem tenderLine = CreateDepositTenderLine(entry, depositForPickup > decimal.Zero ? depositForPickup*-1 : depositForPickup);

                retailTransaction.Add(tenderLine);
            }

            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, retailTransaction);

        }

        /// <summary>
        /// Calculates the deposit that is being paid and creates a deposit tender line and deposit transaction
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="depositTransaction"></param>
        /// <param name="retailTransaction"></param>
        protected virtual void PrepareDepositTenderLine(IConnectionManager entry, DepositTransaction depositTransaction, RetailTransaction retailTransaction)
        {
         
            //Get what the deposit was for the items NOT being picked up   
            decimal deposit = retailTransaction.SaleItems.Where(w => !w.Voided && w.Order.Ordered > 0 && w.Order.ToPickUp == 0).Sum(s => s.Order.TotalDepositAmount());

            TenderLineItem tenderLine = CreateDepositTenderLine(entry, deposit);

            //Clear out all the other tender lines so they are not saved
            depositTransaction.TenderLines.Clear();
            depositTransaction.Payment = decimal.Zero;

            //Add the new payment line to the deposit transaction
            depositTransaction.Add(tenderLine);

            depositTransaction.NetAmountWithTax = tenderLine.Amount;
            depositTransaction.NetAmount = tenderLine.Amount;
            depositTransaction.Payment = tenderLine.Amount;
            depositTransaction.AmountToAccount = tenderLine.Amount;

        }

        #region Deposit and Redeemed tenderlines

        /// <summary>
        /// Creates the deposit tender line for when the customer pays the deposit
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="depositAmount">The amount that is to be paid</param>
        /// <returns>The deposit tender line</returns>
        protected virtual TenderLineItem CreateDepositTenderLine(IConnectionManager entry, decimal depositAmount)
        {
            StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.DepositTender);

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            TenderLineItem tenderLine = CreateTenderLine(entry, settings, payment, false, depositAmount);

            return tenderLine;
        }

        /// <summary>
        /// Create a tender line for the redeemed deposit that has already been paid for
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        protected virtual void CreateRedeemedTenderLine(IConnectionManager entry, RetailTransaction retailTransaction)
        {
            decimal redeemedDeposit = CalculationHelper.GetTotalAlreadyPaidDeposit(entry, CustomerOrderSummaries.ToPickUp, retailTransaction) +
                                      CalculationHelper.GetTotalAlreadyPaidAdditionalPayments(entry, retailTransaction);

            if (redeemedDeposit == decimal.Zero)
            {
                return;
            }

            StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.DepositTender);
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            DepositTenderLineItem tenderLine = CreateTenderLine(entry, settings, payment, true, redeemedDeposit);

            retailTransaction.Add(tenderLine);
        }

        /// <summary>
        /// Creates a tender line for when the deposit is being returned
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="depositTransaction">The deposit transaction</param>
        protected virtual void CreateReturnDepositTenderLine(IConnectionManager entry, RetailTransaction retailTransaction, DepositTransaction depositTransaction)
        {
            decimal otherTenderLines = retailTransaction.TenderLines.Where(w => !w.PaidDeposit && !w.Voided).Sum(s => s.Amount);
            decimal depositToBeReturned = retailTransaction.CustomerOrder.DepositToBeReturned > decimal.Zero ? retailTransaction.CustomerOrder.DepositToBeReturned * -1 : retailTransaction.CustomerOrder.DepositToBeReturned;

            decimal orginalDeposit = depositToBeReturned - otherTenderLines;

            if (orginalDeposit == decimal.Zero)
            {
                return;
            }

            StorePaymentMethod payment = Providers.StorePaymentMethodData.Get(entry, entry.CurrentStoreID, PaymentMethodDefaultFunctionEnum.DepositTender);
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            DepositTenderLineItem tenderLine = CreateTenderLine(entry, settings, payment, true, orginalDeposit);

            //Clear out the tender lines that should not be on the deposit transaction and keep the ones that are relevant to this deposit transaction
            List<ITenderLineItem> listToKeep = depositTransaction.TenderLines.Where(w => !w.Voided && !w.PaidDeposit).ToList();
            depositTransaction.TenderLines.Clear();
            depositTransaction.Payment = decimal.Zero;

            foreach (ITenderLineItem item in listToKeep)
            {
                depositTransaction.Add(item);
            }

            depositTransaction.Add(tenderLine);

            depositTransaction.NetAmount = depositTransaction.NetAmountWithTax;
            depositTransaction.AmountToAccount = depositTransaction.Payment;
        }

        /// <summary>
        /// Creates a deposit tender line item with the information necessary
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="settings">The settings of the current POS session</param>
        /// <param name="payment">The payment type that is being used for the payment</param>
        /// <param name="redeemedDeposit">If true the deposit is being returned</param>
        /// <param name="depositAmount">The amount of the deposit</param>
        /// <returns></returns>
        protected virtual DepositTenderLineItem CreateTenderLine(IConnectionManager entry, ISettings settings, StorePaymentMethod payment, bool redeemedDeposit, decimal depositAmount)
        {
            DepositTenderLineItem tenderLine = new DepositTenderLineItem();
            tenderLine.RedeemedDeposit = redeemedDeposit;
            tenderLine.TenderTypeId = (string)payment.ID.SecondaryID;
            tenderLine.Description = redeemedDeposit ? Properties.Resources.RedeemedDeposit : (string)payment.Text;
            tenderLine.OpenDrawer = !redeemedDeposit && payment.OpenDrawer;
            tenderLine.ChangeBack = !redeemedDeposit && depositAmount < decimal.Zero;
            
            tenderLine.Amount = depositAmount;
            tenderLine.CompanyCurrencyAmount = Services.Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                entry,
                settings.Store.Currency,
                settings.CompanyInfo.CurrencyCode,
                settings.CompanyInfo.CurrencyCode,
                depositAmount);
            tenderLine.ExchrateMST = Services.Interfaces.Services.CurrencyService(entry).ExchangeRate(entry, settings.Store.Currency) * 100;

            return tenderLine;
        }

        #endregion

        /// <summary>
        /// Marks all the items that are to be picked up with information about the pickup
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="reservationDone">If true then the stock reservation has already been created</param>
        protected virtual void MarkItemsAsReceived(IConnectionManager entry, RetailTransaction retailTransaction, bool reservationDone)
        {
            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => w.Order.ToPickUp > 0))
            {
                item.Order.Received += item.Order.ToPickUp;
                item.Order.DateFullyReceived = Date.Now;
                item.Order.ReservationDone = reservationDone;
            }
        }

        /// <summary>
        /// When the customer is picking up items then a retail transaction is created with the items that are being picked up
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="retailTransaction"></param>
        /// <returns></returns>
        protected virtual RetailTransaction CreateRetailTransaction(IConnectionManager entry, RetailTransaction retailTransaction)
        {
            RetailTransaction sale = new RetailTransaction("", "", true);
            sale.ToClass(retailTransaction.ToXML());
            sale.BeginDateTime = DateTime.Now;

            //Create the redeemed tender line so that the final balance of the sale adds up and information for the X/Z report 
            CreateRedeemedTenderLine(entry, sale);

            List<ISaleLineItem> itemsToRemove = new List<ISaleLineItem>();
            foreach (ISaleLineItem item in sale.SaleItems)
            {
                item.BeginDateTime = DateTime.Now;
                //If the item has been voided it should only be saved on the last transaction created from the customer order
                if (item.Voided && sale.CustomerOrder.CurrentAction != CustomerOrderAction.FullPickup)
                {
                    itemsToRemove.Add(item);
                }
                //If there is nothing to be picked up on this item then remove it
                else if (item.Order.ToPickUp == 0)
                {
                    itemsToRemove.Add(item);
                }
                //If the pickup is not the same as the quantity then change the quantity on the item and make sure that the price is not calculated again
                else if (item.Order.ToPickUp < item.Quantity)
                {
                    item.Quantity = item.Order.ToPickUp;
                    item.NoPriceCalculation = true;
                }
            }

            foreach (ISaleLineItem toRemove in itemsToRemove)
            {
                sale.Remove(toRemove, false);
            }

            List<ITenderLineItem> tendersToRemove = new List<ITenderLineItem>();
            foreach (ITenderLineItem item in sale.TenderLines)
            {
                item.BeginDateTime = DateTime.Now;
                //If the tenderline has been voided it should only be saved on the last transactoin created from the customer order
                if (item.Voided && sale.CustomerOrder.CurrentAction != CustomerOrderAction.FullPickup)
                {
                    tendersToRemove.Add(item);
                }
                //If the tenderline was paid on a previous deposit/transaction then it already has been saved and needs to be removed
                else if (item.PaidDeposit)
                {
                    tendersToRemove.Add(item);
                }
            }

            foreach (ITenderLineItem toRemove in tendersToRemove)
            {
                sale.Remove(toRemove, false);
            }

            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, sale);

            return sale;
        }
    }
}
