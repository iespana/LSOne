using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Peripherals;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using Customer = LSOne.DataLayer.BusinessObjects.Customers.Customer;
using LSOne.Controls.Dialogs;

namespace LSOne.Services
{
    public partial class TransactionService : ITransactionService
    {
        private BackgroundWorker custdisplayWorker;
        private object threadLock = new object();

        public IErrorLog ErrorLog
        {
            set { throw new NotImplementedException(); }
        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public void CalculatePriceTaxDiscount(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings == null)
                {
                    return;
                }
                // Get the price
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                System.Diagnostics.Stopwatch intervalWatch = new System.Diagnostics.Stopwatch();
                intervalWatch.Start();
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - Start ----  " + stopwatch.ElapsedMilliseconds.ToString(), ToString(), (int)stopwatch.ElapsedMilliseconds);

                ((RetailTransaction)transaction).StoreCurrencyCode = settings.Store.Currency;
                if (((RetailTransaction)transaction).Customer.PriceGroupID == null || ((string)((RetailTransaction)transaction).Customer.PriceGroupID).Trim() == "")
                {
                    ((RetailTransaction)transaction).Customer.PriceGroupID = settings.StorePriceGroup;
                }
                ((RetailTransaction)transaction).LineDiscCalculationType = settings.DiscountCalculation.DiscountsToApply;
                Interfaces.Services.PriceService(entry).SetPrice(entry, (IRetailTransaction)transaction, CacheType.CacheTypeTransactionLifeTime);

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - GetPriceInterval ----  " + intervalWatch.ElapsedMilliseconds.ToString(), ToString(), (int)stopwatch.ElapsedMilliseconds);
                intervalWatch.Start();
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - GetPrice ----  " + stopwatch.ElapsedMilliseconds.ToString(), ToString(), (int)stopwatch.ElapsedMilliseconds);

                // Calculate the tax
                ITaxService tax = Interfaces.Services.TaxService(entry);

                //Get the last item added to calculate for tax  
                foreach (SaleLineItem saleLine in ((RetailTransaction)transaction).SaleItems.Where(w => w.WasChanged && w.ShouldCalculateAndDisplayAssemblyPrice()))
                {
                    tax.CalculateTax(entry, ((RetailTransaction)transaction), saleLine);
                }

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - CalculateTaxInterval ----  " + intervalWatch.ElapsedMilliseconds.ToString(), this.ToString(), (int)stopwatch.ElapsedMilliseconds);
                intervalWatch.Start();
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - CalculateTax ----  " + stopwatch.ElapsedMilliseconds.ToString(), this.ToString(), (int)stopwatch.ElapsedMilliseconds);

                // Calculate the discount 
                Interfaces.Services.DiscountService(entry).CalculateDiscount(entry, (RetailTransaction)transaction);

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - CalculateDiscountInterval ----  " + intervalWatch.ElapsedMilliseconds.ToString(), this.ToString(), (int)stopwatch.ElapsedMilliseconds);
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "---- " + ((RetailTransaction)transaction).SaleItems.Count.ToString() + " Time measurements - CalculatePriceTaxDiscount - CalculateDiscount ----  " + stopwatch.ElapsedMilliseconds.ToString(), this.ToString(), (int)stopwatch.ElapsedMilliseconds);

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, ToString(), x);
                throw;
            }
        }

        public void RecalculatePriceTaxDiscount(IConnectionManager entry, IPosTransaction transaction, bool restoreItemPrices, bool calculateDiscountsNow = false)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings == null)
                {
                    return;
                }
                // Get the price
                ((RetailTransaction)transaction).StoreCurrencyCode = settings.Store.Currency;
                Interfaces.Services.PriceService(entry).UpdateAllPrices(entry, (IRetailTransaction)transaction, restoreItemPrices, CacheType.CacheTypeTransactionLifeTime);

                ITaxService tax = (ITaxService)entry.Service(ServiceType.TaxService);

                // All prices are being recalculated so all items need to have the tax recalculated
                // ----------------------------------------------------------------------------------------------------
                LinkedListNode<ISaleLineItem> saleItemNode = ((RetailTransaction)transaction).SaleItems.First;
                while (saleItemNode != null)
                {
                    if(!saleItemNode.Value.ReceiptReturnItem && saleItemNode.Value.ShouldCalculateAndDisplayAssemblyPrice())
                    {
                        tax.CalculateTax(entry, ((RetailTransaction)transaction), saleItemNode.Value);
                    }

                    saleItemNode = saleItemNode.Next;
                }

                // Calculate the discount 
                Interfaces.Services.DiscountService(entry).CalculateDiscount(entry, (RetailTransaction)transaction, calculateDiscountsNow);

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        public void AddCustomerToTransaction(IConnectionManager entry, ISession session, IPosTransaction transaction, RecordIdentifier customerID, bool processInfocodes)
        {
            Customer customer = Providers.CustomerData.Get(entry, customerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
            if (customer == null)
            {
                return;
            }

            if (transaction is RetailTransaction)
            {
                if (((RetailTransaction)transaction).Add(customer))
                {
                    ((RetailTransaction)transaction).AddInvoicedCustomer(Providers.CustomerData.Get(entry, customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
                }

                if (processInfocodes)
                {
                    // Get infocode information if needed.
                    Interfaces.Services.InfocodesService(entry).ProcessInfocode(entry, session, (PosTransaction) transaction, 0, 0, (string) ((RetailTransaction) transaction).Customer.ID, "", "", InfoCodeLineItem.TableRefId.Customer, "", null, InfoCodeLineItem.InfocodeType.Header, true);
                    Interfaces.Services.InfocodesService(entry).ProcessLinkedInfocodes(entry, session, (PosTransaction) transaction, InfoCodeLineItem.TableRefId.Customer, InfoCodeLineItem.InfocodeType.Header);
                }
                // Trigger new price,tax and discount for the customer
                RecalculatePriceTaxDiscount(entry, transaction, true);
                Interfaces.Services.CalculationService(entry).CalculateTotals(entry, (PosTransaction)transaction);

            }
            else if (transaction is CustomerPaymentTransaction)
            {
                ((CustomerPaymentTransaction)transaction).Add(customer);

                if (processInfocodes && ((CustomerPaymentTransaction)transaction).Customer.ID != RecordIdentifier.Empty) // See if the customer exists
                {
                    //Get infocode information if needed.
                    Interfaces.Services.InfocodesService(entry).ProcessInfocode(entry, session, (PosTransaction)transaction, 0, 0, (string)((CustomerPaymentTransaction)transaction).Customer.ID, "", "", InfoCodeLineItem.TableRefId.Customer, "", null, InfoCodeLineItem.InfocodeType.Header, true);
                    Interfaces.Services.InfocodesService(entry).ProcessLinkedInfocodes(entry, session, (PosTransaction)transaction, InfoCodeLineItem.TableRefId.Customer, InfoCodeLineItem.InfocodeType.Header);
                }
            }
        }

        public void ClearCustomerFromTransaction(IConnectionManager entry, ref IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (settings == null)
            {
                return;
            }

            settings.POSApp.RunOperation(POSOperations.CustomerClear, null, ref transaction);
        }        

        public void ConcludeTransaction(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            IConnectionManagerTemporary saveTransactionEntry = null;

            try
            {                
                try
                {
                    // Check for cash-back
                    if (transaction.EntryStatus == TransactionStatus.Normal)
                    {                        
                        if (transaction is IRetailTransaction)
                        {
                            IRetailTransaction currentRetailTransaction = (IRetailTransaction)transaction;

                            if (currentRetailTransaction.TransSalePmtDiff == 0 && currentRetailTransaction.SaleItems.Count > 0 && currentRetailTransaction.LastRunOperationIsValidPayment
                                && (currentRetailTransaction.CustomerOrder.Empty() || currentRetailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.PayDeposit))
                            {
                                // if the payment is a negative payment then display the change back dialog                           
                                if ((currentRetailTransaction.Payment < decimal.Zero || CustomerOrderDepositPayback(currentRetailTransaction)) &&
                                    currentRetailTransaction.LastRunOperation != POSOperations.IssueCreditMemo &&
                                    currentRetailTransaction.LastRunOperation != POSOperations.PayCard &&
                                    currentRetailTransaction.LastRunOperation != POSOperations.AuthorizeCard)
                                {
                                    int last = currentRetailTransaction.TenderLines.Count() - 1;
                                    if (!(currentRetailTransaction.TenderLines[last] is CustomerTenderLineItem) &&!settings.SuppressUI)
                                    {
                                        using (ChangeBackDialog dialog = new ChangeBackDialog((PosTransaction)currentRetailTransaction))
                                        {
                                            decimal amount = currentRetailTransaction.TenderLines[last].ForeignCurrencyAmount != 0 ? currentRetailTransaction.TenderLines[last].ForeignCurrencyAmount : currentRetailTransaction.TenderLines[last].Amount;
                                            dialog.Amount = amount;
                                            dialog.AmountTendered = 0;
                                            dialog.Changeback = amount * -1;
                                            dialog.PaymentName = currentRetailTransaction.TenderLines[last].Description;
                                            dialog.OpenDrawer = currentRetailTransaction.TenderLines[last].OpenDrawer;
                                            dialog.UsePaymentInMoneyBack = currentRetailTransaction.LastRunOperation == POSOperations.PayLoyalty;
                                            dialog.ShowDialog();
                                        }
                                    }
                                }
                            }
                            else if (!currentRetailTransaction.CustomerOrder.Empty() && currentRetailTransaction.LastRunOperationIsValidPayment)
                            {
                                CustomerOrderAction action = currentRetailTransaction.CustomerOrder.CurrentAction;
                                ICustomerOrderService customerOrderService = Services.Interfaces.Services.CustomerOrderService(entry);

                                if (action == CustomerOrderAction.ReturnFullDeposit ||
                                    action == CustomerOrderAction.ReturnPartialDeposit ||
                                    action == CustomerOrderAction.PayDeposit ||
                                    action == CustomerOrderAction.AdditionalPayment ||
                                    action == CustomerOrderAction.PartialPickUp ||
                                    action == CustomerOrderAction.FullPickup)
                                {
                                    /******************************************************

                                        In these actions the POS needs to display a cash back dialog in some instances

                                        1) Returning deposits
                                        2) When the cashier is taking a deposit payments and the customer is paying more than the deposit is
                                            => this means that the customer want's to get money back from the cashier
                                            => the TransSalePmtDiff variable cannot be used here as the balance of the customer order is still larger than the payment                                      

                                    *******************************************************/

                                    bool doCashBack = true;
                                    bool doSimpleCashBack = false;

                                    //Retrieve the amount that is actually to be paid                                    
                                    decimal amountToBePaid = customerOrderService.CalculateAmountToBePaid(entry, currentRetailTransaction, false);

                                    //Retrieve the total of the tender amounts that are being paid at this moment
                                    decimal currentPayment = currentRetailTransaction.TenderLines.Where(w => !w.Voided && !w.PaidDeposit).Sum(s => s.Amount);

                                    //Don't conclude if the customer still needs to pay
                                    if (action == CustomerOrderAction.PayDeposit || action == CustomerOrderAction.AdditionalPayment)
                                    {
                                        //If we pickup items while paying deposit (when minumim deposit is 100%)
                                        if (action == CustomerOrderAction.PayDeposit && currentRetailTransaction.SaleItems.Any(c => c.Order.ToPickUp > 0))
                                        {
                                            amountToBePaid += currentPayment;
                                        }

                                        doCashBack = currentPayment > amountToBePaid;                                        
                                    }
                                    //If the amount the total of previous payments and current payments are less then zero then continue to cash back otherwise go to Concluding the transaction
                                    else if ((action == CustomerOrderAction.ReturnFullDeposit || action == CustomerOrderAction.ReturnPartialDeposit) &&
                                             currentRetailTransaction.Payment + amountToBePaid >= decimal.Zero)
                                    {
                                        doCashBack = false;
                                    }
                                    else if ((action == CustomerOrderAction.ReturnFullDeposit || action == CustomerOrderAction.ReturnPartialDeposit) &&
                                             currentRetailTransaction.Payment + amountToBePaid < decimal.Zero)
                                    {
                                        doCashBack = false;
                                        doSimpleCashBack = true;
                                    }
                                    else if ((action == CustomerOrderAction.PartialPickUp || action == CustomerOrderAction.FullPickup) && amountToBePaid >= decimal.Zero)
                                    {
                                        doCashBack = false;                                        
                                    }

                                    if (doCashBack)
                                    {
                                        settings.POSApp.RunOperation(POSOperations.ChangeBack, new OperationInfo { SkipResultCallback = true }, null, ref transaction);
                                    }

                                    if (doSimpleCashBack)
                                    {
                                        int last = currentRetailTransaction.TenderLines.Count() - 1;
                                        if (!(currentRetailTransaction.TenderLines[last] is CustomerTenderLineItem) && !settings.SuppressUI)
                                        {
                                            using (ChangeBackDialog dialog = new ChangeBackDialog((PosTransaction)currentRetailTransaction))
                                            {
                                                dialog.Amount = currentRetailTransaction.TenderLines[last].Amount;
                                                dialog.AmountTendered = 0;
                                                dialog.Changeback = currentRetailTransaction.TenderLines[last].Amount * -1;
                                                dialog.PaymentName = currentRetailTransaction.TenderLines[last].Description;
                                                dialog.OpenDrawer = currentRetailTransaction.TenderLines[last].OpenDrawer;
                                                dialog.UsePaymentInMoneyBack = currentRetailTransaction.LastRunOperation == POSOperations.PayLoyalty;
                                                dialog.ShowDialog();
                                            }
                                        }
                                    }
                                }                                
                            }
                            else if (currentRetailTransaction.TransSalePmtDiff < 0 && currentRetailTransaction.LastRunOperationIsValidPayment)
                            {
                                bool doCashback = false;
                                if (currentRetailTransaction.CustomerOrder.Empty() && currentRetailTransaction.NetAmountWithTax >= decimal.Zero)
                                {
                                    doCashback = true;
                                }
                                else if (!currentRetailTransaction.CustomerOrder.Empty() && currentRetailTransaction.CustomerOrder.DepositToBeReturned == decimal.Zero)
                                {
                                    doCashback = true;
                                }
                                else if (!currentRetailTransaction.CustomerOrder.Empty() && currentRetailTransaction.CustomerOrder.DepositToBeReturned != decimal.Zero)
                                {
                                    if (currentRetailTransaction.CustomerOrder.DepositToBeReturned == currentRetailTransaction.Payment)
                                    {
                                        doCashback = true;
                                    }
                                }

                                if (doCashback)
                                {
                                    // Overtendered -> Show the change back dialog and then conclude the transaction
                                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "ConcludeTransaction - balance negative, last operation payment - no action taken", this.ToString());
                                    settings.POSApp.RunOperation(POSOperations.ChangeBack, null, new OperationInfo { SkipResultCallback = true }, ref transaction);
                                }
                            }

                            // Save the state after running a cash-back
                            TransactionProviders.SerializedTransactionData.SaveSerializedTransaction(entry, (PosTransaction)transaction);
                        }
                        else if (transaction is CustomerPaymentTransaction)
                        {
                            // The possible combinations at the end of an Customer Payment transaction operation are as follows.....

                            // 1)   Undertendered:
                            //      Continue with the transaction
                            //
                            // 2)   Correctly tendered:
                            //      If the transaction is correctly tendered (TransSalePmtDiff = 0), there are actually items in the transaction
                            //      and the last line added to the transaction was a tender line then we can conclude the transaction.
                            //
                            // 3)   Overtendered:
                            //      If the transaction is overtendered (TransSalePmtDiff < 0) and the last line added to the transaction
                            //      was a tender line, then we can run the change back operation and subsequently conclude the transaction.

                            CustomerPaymentTransaction currentTransaction = (CustomerPaymentTransaction)transaction;

                            if (currentTransaction.CustomerDepositItem != null && currentTransaction.TransSalePmtDiff < 0 && currentTransaction.LastRunOperationIsValidPayment)
                            {
                                // Overtendered -> Show the change back dialog and then conclude the transaction          
                                settings.POSApp.RunOperation(POSOperations.ChangeBack, null, new OperationInfo { SkipResultCallback = true }, ref transaction);
                            }

                            // Save the state after running a cash-back
                            TransactionProviders.SerializedTransactionData.SaveSerializedTransaction(entry, (PosTransaction)transaction);
                        }
                    }

                    try
                    {
                        IFiscalService fiscService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                        if (fiscService != null && fiscService.IsActive())
                        {
                            if (fiscService.TransactionCompleted(entry, (PosTransaction)transaction) == false)
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }

                    //Create a temporary connection so that we have an isolated SQL connection when saving data into the transaction tables.
                    //This is done so that a trigger/service/background task cannot interfere with the saving process because we are sharing the same
                    //connection manager with all components in the application.                                         
                    saveTransactionEntry = entry.CreateTemporaryConnection();

                    string statusMsg = transaction is RetailTransaction ? Resources.PostingSaleInformation : Resources.FinalisingTransaction;

                    ShowStatusDialog(entry, settings, transaction, statusMsg);
                                        
                    if (transaction.EntryStatus == TransactionStatus.Normal)
                    {
                        if (transaction is RetailTransaction && !((RetailTransaction)transaction).CustomerOrder.Empty())
                        {
                            transaction = (PosTransaction)Interfaces.Services.CustomerOrderService(saveTransactionEntry).ConcludeTransaction(saveTransactionEntry, (RetailTransaction)transaction);
                        }

                        Interfaces.Services.ApplicationService(entry).GenerateReceiptID(entry, settings, transaction);

                        SetReceiptOptions(entry, settings, transaction);
                        AddLoyaltyPoints(entry, transaction);

                        ShowStatusDialog(entry, settings, transaction, Resources.UpdatingSerialNumberInformation);

                        UseSerialNumbers(entry, transaction);
                        UpdatePharmacyReceiptStatus(entry, transaction);

                        ShowStatusDialog(entry, settings, transaction, Resources.UpdatingCreditMemoAndGiftCardInformation);
                        
                        UpdateCreditMemoStatus(entry, transaction);
                        UpdateGiftCardStatus(entry, transaction);

                        ShowStatusDialog(entry, settings, transaction, Resources.UpdatingLoyaltyInformation);
                        UpdateLoyaltyPoints(entry, transaction);

                        ShowStatusDialog(entry, settings, transaction, Resources.UpdatingCustomerLedger);
                        
                        UpdateCustomerLedger(entry, transaction);
                        UpdateCoupons(entry, transaction);

                        CreateParkedInventoryJournal(entry, settings, transaction);

                        RFIDScanner.ConcludeRFIDs();

                        ShowStatusDialog(entry, settings, transaction, statusMsg);
                        
                        SaveTransaction(saveTransactionEntry, transaction);
                    }
                    if (transaction.EntryStatus == TransactionStatus.Voided)
                    {
                        // Voided transaction...
                        SaveTransaction(saveTransactionEntry, transaction);
                    }
                    else if (transaction.EntryStatus == TransactionStatus.Concluded)
                    {
                        if (transaction is LogOnOffTransaction || transaction.OpenDrawer)
                        {
                            SaveTransaction(saveTransactionEntry, transaction);
                        }
                    }
                    
                    //reseting the connection on the cache in case the temporary connection was used to access the cache when saving the transaction
                    entry.Cache.DataModel = entry;

                    IFiscalService fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                    if (fiscalService != null && fiscalService.IsActive())
                    {
                        ShowStatusDialog(entry, settings, transaction, Resources.SendingFiscalInformation);
                        // Saving a text representation of the transaction to database as a fiscal log
                        // ==========================================================================
                        fiscalService.SaveReceiptCopy(entry, transaction, false);
                        if (transaction.ReceiptId != "")
                        {
                            fiscalService.HashTransaction(entry, transaction); //hash transaction needs to be before PrintTransaction to print also the hash
                        }
                        // ==========================================================================
                    }
                    ShowStatusDialog(entry, settings, transaction, statusMsg);

                    ShowStatusDialog(entry, settings, transaction, Resources.Printing);
                    Interfaces.Services.PrintingService(entry).PrintTransaction(entry, transaction, false, false);
                    SaveReceipts(saveTransactionEntry, transaction);

                    if (transaction.ReceiptSettings != ReceiptSettingsEnum.Ignore)
                    {
                        ShowStatusDialog(entry, settings, transaction, Resources.EmailingReceipts);
                    }

                    EmailTransactionReceipts(entry, transaction);

                    ShowStatusDialog(entry, settings, transaction, statusMsg);

                    // Opening the drawer when appropriate
                    if (transaction.EntryStatus == TransactionStatus.Normal && transaction.ITenderLines != null)
                    {
                        foreach (ITenderLineItem tenderLineItem in transaction.ITenderLines)
                        {
                            if (tenderLineItem.OpenDrawer)
                            {
                                OpenCashDrawer(entry, transaction, transaction.OpenDrawer);
                                break;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(settings.FunctionalityProfile.DDSchedulerLocation) &&
                        !string.IsNullOrEmpty(settings.FunctionalityProfile.PostTransactionDDJob))
                    {
                        ShowStatusDialog(entry, settings, transaction, Resources.SendingToHeadOffice);
                        Interfaces.Services.DDService(entry).RunPostTransactionJob(transaction);
                    }
                }
                catch (Exception x)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                    throw;
                }
            }
            finally
            {
                CloseStatusDialog(entry, settings);
                saveTransactionEntry?.Close();
            }
        }

        private void CreateParkedInventoryJournal(IConnectionManager entry, ISettings settings, IPosTransaction transaction)
        {
            try
            {
                if(transaction is RetailTransaction && !settings.TrainingMode && ((RetailTransaction)transaction).SaleItems.Any(x => x.ReasonCode != null && x.ReasonCode.Action == ReasonActionEnum.ParkedInventory))
                {
                    ShowStatusDialog(entry, settings, transaction, Resources.CreatingParkedInventoryJournal);

                    InventoryAdjustment journal = new InventoryAdjustment
                    {
                        CreatedDateTime = DateTime.Now,
                        JournalType = InventoryJournalTypeEnum.Parked,
                        MasterID = Guid.NewGuid(),
                        Posted = InventoryJournalStatus.Active,
                        PostedDateTime = Date.Empty,
                        StoreId = transaction.StoreId,
                        Text = transaction.ReceiptId,
                        DeletePostedLines = 0,
                        ID = RecordIdentifier.Empty
                    };

                    journal = Interfaces.Services.SiteServiceService(entry).SaveInventoryAdjustment(entry, settings.SiteServiceProfile, journal, false);

                    foreach (ISaleLineItem item in ((RetailTransaction)transaction).SaleItems.Where(x => x.ReasonCode != null && x.ReasonCode.Action == ReasonActionEnum.ParkedInventory))
                    {
                        InventoryJournalTransaction line = new InventoryJournalTransaction
                        {
                            JournalId = journal.ID,
                            Posted = true,
                            PostedDateTime = DateTime.Now,
                            UnitID = item.SalesOrderUnitOfMeasure,
                            TransDate = DateTime.Now,
                            ItemId = item.ItemId,
                            LineNum = RecordIdentifier.Empty,
                            ReasonId = item.ReasonCode.ID,
                            Counted = 0,
                            MasterID = Guid.NewGuid(),
                            ParentMasterID = RecordIdentifier.Empty,
                            CostPrice = item.CostPrice,
                            Adjustment = item.Quantity,
                            StaffID = transaction.Cashier.ID
                        };

                        line.InventoryUnitID = Providers.RetailItemData.GetItemUnitID(entry, line.ItemId, RetailItem.UnitTypeEnum.Inventory);
                        line.InventOnHandInInventoryUnits = Interfaces.Services.SiteServiceService(entry).GetInventoryOnHand(entry, settings.SiteServiceProfile, line.ItemId, journal.StoreId, false);
                        line.AdjustmentInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry,
                                                                                                                line.ItemId,
                                                                                                                line.UnitID,
                                                                                                                line.InventoryUnitID,
                                                                                                                line.Adjustment);
                        Interfaces.Services.SiteServiceService(entry).SaveJournalTransaction(entry, settings.SiteServiceProfile, line, false);
                    }

                    ((RetailTransaction)transaction).JournalID = journal.ID.StringValue;
                }
            }
            catch(Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        public void LoadTransactionStatus(IConnectionManager entry, IPosTransaction transaction, bool rebuildingTransaction = false, bool generateTransactionID = false)
        {
            if (transaction == null)
            {
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            if (settings == null)
            {
                return;
            }
            if (entry.CurrentTerminalID == "")
            {
                ((PosTransaction)transaction).Training = false; // We are probably in the Store Controller

                Store store = Providers.StoreData.Get(entry, transaction.StoreId);
                ((PosTransaction)transaction).CalculateDiscountFrom = store.CalculateDiscountsFrom;
            }
            else
            {
                ((PosTransaction)transaction).Training = settings.TrainingMode;
                ((PosTransaction)transaction).Cashier.ID = (string)settings.POSUser.ID;
                ((PosTransaction)transaction).Cashier.Name = entry.Settings.NameFormatter.Format(settings.POSUser.Name);
                ((PosTransaction)transaction).Cashier.NameOnReceipt = settings.POSUser.NameOnReceipt;
                ((PosTransaction)transaction).Cashier.Login = settings.POSUser.Login;
                ((PosTransaction)transaction).TerminalId = (string)entry.CurrentTerminalID;
                ((PosTransaction)transaction).StoreId = (string)entry.CurrentStoreID;
                ((PosTransaction)transaction).StoreName = settings.Store.Text;
                ((PosTransaction)transaction).StoreAddress = entry.Settings.LocalizationContext.FormatSingleLine(settings.Store.Address, entry.Cache);
                ((PosTransaction)transaction).StatementMethod = StatementGroupingMethod.POSTerminal;
                ((PosTransaction)transaction).CreatedOnTerminalId = (string)entry.CurrentTerminalID;
                ((PosTransaction)transaction).TransactionIdNumberSequence = settings.Terminal.TransactionIDNumberSequence;
                ((PosTransaction)transaction).StoreTaxGroup = settings.Store.TaxGroup;
                ((PosTransaction)transaction).KeyedInPriceContainsTax = settings.Store.KeyedInPriceIncludesTax;
                ((PosTransaction)transaction).CalculateDiscountFrom = settings.Store.CalculateDiscountsFrom;
                ((PosTransaction)transaction).DisplayAmountsIncludingTax = settings.Store.DisplayAmountsWithTax;
                ((PosTransaction)transaction).CalcCustomerDiscounts = settings.DiscountCalculation.CalculateCustomerDiscounts;
                ((PosTransaction)transaction).CalcPeriodicDiscounts = settings.DiscountCalculation.CalculatePeriodicDiscounts;
                ((PosTransaction)transaction).BusinessDay = settings.BusinessDay.Date + transaction.BeginDateTime.TimeOfDay;
                ((PosTransaction)transaction).BusinessSystemDay = settings.BusinessSystemDay == default(DateTime) ? DateTime.Now : settings.BusinessSystemDay;
            }

            IPosTransactionData posTransactionData = TransactionProviders.PosTransactionData;

            // There are two cases when we might need a new transaction id
            //     * The transaction id is for some reason empty (can happen in isolated instances)
            //     * The transaction has an older id from before the number sequence was added to the POS. 
            //       This could be an older suspended transaction or a recalled transaction after a crash or shutdown. In this
            //       case we must check if the transaction id already exists and in that case assign a new one.
            if (generateTransactionID && (!rebuildingTransaction && (string.IsNullOrEmpty(transaction.TransactionId) || posTransactionData.SequenceExists(entry, transaction.TransactionId))))
            {
                transaction.TransactionId = (string)DataProviderFactory.Instance.GenerateNumber(entry, posTransactionData);
            }

            if (!rebuildingTransaction && (transaction is RetailTransaction || transaction is CustomerPaymentTransaction))
            {
                if (transaction is RetailTransaction)
                {
                    ((RetailTransaction)transaction).ReceiptIdNumberSequence = (string)settings.Terminal.ReceiptIDNumberSequence;

                    if (settings.FunctionalityProfile != null && settings.FunctionalityProfile.IsHospitality)
                    {
                        ((RetailTransaction)transaction).Hospitality.ActiveHospitalitySalesType = Interfaces.Services.HospitalityService(entry).GetActiveHospSalesType(entry);
                        ((RetailTransaction)transaction).UseTaxGroupFrom = Interfaces.Services.HospitalityService(entry).GetHospitalityTaxGroupFrom(entry);
                    }
                    else
                    {
                        ((RetailTransaction)transaction).Hospitality.ActiveHospitalitySalesType = RecordIdentifier.Empty;
                        ((RetailTransaction)transaction).UseTaxGroupFrom = settings.Store.UseTaxGroupFrom;
                    }
                }
                else
                {
                    ((CustomerPaymentTransaction)transaction).ReceiptIdNumberSequence = (string)settings.Terminal.ReceiptIDNumberSequence;
                }
            }

            /*******************************************************************************************************************

                        RECEIPT NUMBER IS CREATED JUST BEFORE THE TRANSACTION IS SAVED AND PAYMENTS HAVE BEEN MADE
                        IT SHOULD NOT BE CREATED HERE

            ********************************************************************************************************************/
        }

        private bool UserCanReturnItems(IConnectionManager entry, LinkedList<int> returnedItems, RetailTransaction transToReturn, IPosTransaction posTransaction)
        {
            ISaleLineItem returnedItem;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            // Start with this baseline amount
            decimal totalReturnAmount = ((RetailTransaction)posTransaction).NetAmountWithTax;

            foreach (int lineNum in returnedItems)
            {
                returnedItem = transToReturn.GetItem(lineNum);

                decimal amount = settings.Store.StorePriceSetting == Store.StorePriceSettingsEnum.PricesIncludeTax ? returnedItem.NetAmountWithTax : returnedItem.NetAmount;

                // Checking if only part of the original items were selected
                // and if so, then the MaxLineReturnAmount check is done

                if (settings.UserProfile.MaxLineReturnAmount > 0)
                {
                    // And the amount is less than some specified amount (returned amount)
                    if (Math.Abs(amount) > settings.UserProfile.MaxLineReturnAmount)
                    {
                        try
                        {
                            Interfaces.Services.DialogService(entry).ShowMessage(Resources.YouSelectedItemWithHigherThanAllowed, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                            return false;
                        }
                        catch
                        {
                            //Do nothing
                        }
                    }
                }
                totalReturnAmount += amount;
            }

            if (settings.UserProfile.MaxTotalReturnAmount > 0)
            {
                if (Math.Abs(totalReturnAmount) > settings.UserProfile.MaxTotalReturnAmount)
                {
                    try
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Resources.NotAllowedToReturnTransaction, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        return false;
                    }
                    catch
                    {
                        //Do nothing
                    }
                }
            }

            return true;
        }

        // Displaying a message on the line display.
        void custdisplaySaleLineWorker_DoWork(object sender, DoWorkEventArgs e)
        {
#pragma warning disable 0612, 0618
            try
            {
                LineDisplay.DisplayItem((RetailTransaction)((object[])e.Argument)[0], (SaleLineItem)((object[])e.Argument)[1]);
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }
#pragma warning restore 0612, 0618
        }

        // Displaying a message on the line display.
        void custdisplayBalanceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
#pragma warning disable 0612, 0618
            try
            {
                IRoundingService rounding = (IRoundingService)DLLEntry.DataModel.Service(ServiceType.RoundingService);

                LineDisplay.DisplayBalance(rounding.RoundString(
                    DLLEntry.DataModel,
                    (decimal)(((object[])e.Argument)[0]),
                    DLLEntry.Settings.Store.Currency,
                    true,
                    CacheType.CacheTypeTransactionLifeTime));
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }
#pragma warning restore 0612, 0618
        }

        private void RoundAmounts(IConnectionManager entry, IPosTransaction transaction)
        {
            if (transaction is RetailTransaction)
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings == null)
                {
                    return;
                }

                if (!settings.Store.UseTaxRounding)
                {
                    return;
                }
                IRoundingService rounding = (IRoundingService)entry.Service(ServiceType.RoundingService);

                ((RetailTransaction)transaction).LineDiscount = rounding.Round(entry, ((RetailTransaction)transaction).LineDiscount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).LineDiscountWithTax = rounding.Round(entry, ((RetailTransaction)transaction).LineDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).PeriodicDiscountAmount = rounding.Round(entry, ((RetailTransaction)transaction).PeriodicDiscountAmount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).PeriodicDiscountWithTax = rounding.Round(entry, ((RetailTransaction)transaction).PeriodicDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).TotalDiscount = rounding.Round(entry, ((RetailTransaction)transaction).TotalDiscount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).TotalDiscountWithTax = rounding.Round(entry, ((RetailTransaction)transaction).TotalDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).NetAmount = rounding.Round(entry, ((RetailTransaction)transaction).NetAmount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

                ((RetailTransaction)transaction).NetAmountWithTax = rounding.Round(entry, ((RetailTransaction)transaction).NetAmountWithTax, settings.Store.Currency, true, CacheType.CacheTypeTransactionLifeTime);
                ((RetailTransaction)transaction).RoundingDifference = rounding.Round(entry, ((RetailTransaction)transaction).RoundingDifference, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);

                ((RetailTransaction)transaction).Payment = rounding.Round(entry, ((RetailTransaction)transaction).Payment, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);

                foreach (SaleLineItem saleLineItem in ((RetailTransaction)transaction).SaleItems)
                {
                    if (!(saleLineItem is DiscountVoucherItem) && !(saleLineItem is FuelSalesLineItem))
                    {
                        saleLineItem.TaxAmount = rounding.Round(entry, saleLineItem.TaxAmount, settings.Store.Currency, false, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.Price = rounding.Round(entry, saleLineItem.Price, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.PriceWithTax = rounding.Round(entry, saleLineItem.PriceWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.NetAmount = rounding.Round(entry, saleLineItem.NetAmount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.GrossAmount = rounding.Round(entry, saleLineItem.GrossAmount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.NetAmountWithTax = rounding.Round(entry, saleLineItem.NetAmountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.GrossAmountWithTax = rounding.Round(entry, saleLineItem.GrossAmountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.LineDiscount = rounding.Round(entry, saleLineItem.LineDiscount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.LineDiscountWithTax = rounding.Round(entry, saleLineItem.LineDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.PeriodicDiscount = rounding.Round(entry, saleLineItem.PeriodicDiscount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.PeriodicDiscountWithTax = rounding.Round(entry, saleLineItem.PeriodicDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.TotalDiscount = rounding.Round(entry, saleLineItem.TotalDiscount, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                        saleLineItem.TotalDiscountWithTax = rounding.Round(entry, saleLineItem.TotalDiscountWithTax, settings.Store.Currency, CacheType.CacheTypeTransactionLifeTime);
                    }
                }
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "RoundAmounts() END at " +
                    DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString(), this.ToString());
            }
        }

        private void SaveTransaction(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                // Since the amounts in the transaction need to be rounded when read from the db it is rounded as late as possible
                RoundAmounts(entry, transaction);

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings == null)
                {
                    return;
                }

                transaction.OriginalNumberOfTransactionLines = CalculatePartnerTotalNumberOfTransactionLines(entry, transaction) + transaction.CalculateTotalNumberOfTransactionLines();

                // No two threads can save a transaction at the same time since then they could possibly get the same transaction ID
                lock (threadLock)
                {
                    TransactionProviders.PosTransactionData.Save(entry, settings, (PosTransaction) transaction);
                }

                //If the partner object exists then allow it to save itself
                if (transaction is RetailTransaction retailTransaction)
                {
                    if (retailTransaction.PartnerObject != null)
                    {
                        retailTransaction.PartnerObject.Save(retailTransaction);
                    }

                    if (settings.HardwareProfile.EftConnected)
                    {   
                        foreach (var tenderLine in retailTransaction.TenderLines)
                        {
                            if (tenderLine is ICardTenderLineItem cardTenderLine)
                            {
                                cardTenderLine.EFTInfo.EFTExtraInfo?.Insert(entry, retailTransaction);
                            }
                        }

                        retailTransaction.EFTTransactionExtraInfo?.Insert(entry, retailTransaction);
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void SaveReceipts(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            ShowStatusDialog(entry, settings, transaction, Resources.SavingReceipt);

            foreach (ReceiptInfo receipt in transaction.Receipts)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, $"SaveReceipts() START for line {receipt.LineID}", this.ToString());

                int retryCount = 5;
                int currentRetry = 0;

                for (;;)
                {
                    try
                    {
                        TransactionProviders.ReceiptTransactionData.Insert(entry, receipt, transaction);

                        break;
                    }
                    catch(Exception ex)
                    {
                        entry.ErrorLogger.LogMessage(LogMessageType.Trace, $"SaveReceipts() failed for line {receipt.LineID} at attempt {currentRetry + 1}, with error {ex}", this.ToString());

                        currentRetry++;

                        if (currentRetry > retryCount || !IsTransient(ex))
                        {
                            throw;
                        }
                    }
                }
            }

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "SaveReceipts() END", this.ToString());
        }

        private bool IsTransient(Exception ex)
        {
            var databaseException = ex as DatabaseException;
            if (databaseException != null)
            {
                var sqlException = ex.InnerException as SqlException;

                if (sqlException.Number == -2)  // Sql Server time out
                {
                    return true;
                }
            }

            return false;
        }

        private void OpenCashDrawer(IConnectionManager entry, IPosTransaction transaction, bool open)
        {
            try
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "OpenCashDrawer()", this.ToString());

                // Loop through the tender lines and open the cash drawer if applicable, 
                // displaying the open drawer text on the line display.
                if (transaction is RetailTransaction || transaction is CustomerPaymentTransaction)
                {
                    // If the drawer has been opened during the transaction we do not do it again.
                    if (!open)
                    {
                        bool openDrawer = transaction.ITenderLines.Any(tenderItem => !tenderItem.Voided && tenderItem.OpenDrawer);

                        if (openDrawer)
                        {
                            CashDrawer.OpenDrawer();
                        }
                    }
                    else
                    {
                        entry.ErrorLogger.LogMessage(LogMessageType.Trace, "CashDrawer not opened - has already been opened during the transaction. OpenCashDrawer()", this.ToString());
                    }
                }
                // For any other type of transaction - Opening the drawer does not apply                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        /// <summary>
        /// Check if there is a loyalty record on the transaction and if there is then we 
        /// call the ILoyalty.AddLoyaltyPoints() operation to add the calculated loyalty points to the transaction.
        /// </summary>
        private void AddLoyaltyPoints(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                if (transaction is RetailTransaction)
                {
                    //If there is no loyalty item or there is a loyalty discount item on the transaction then the loyalty points should not be calculated
                    if (!((RetailTransaction)transaction).LoyaltyItem.Empty && ((RetailTransaction)transaction).LoyaltyItem.Relation != LoyaltyPointsRelation.Discount)
                    {
                        Interfaces.Services.LoyaltyService(entry).AddLoyaltyPoints(entry, (RetailTransaction)transaction);
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }
        }

        private void UseSerialNumbers(IConnectionManager entry, IPosTransaction transaction)
        {
            Interfaces.Services.ItemService(entry).UseSerialNumbers(entry, transaction);
        }

        private void UpdatePharmacyReceiptStatus(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings == null)
                {
                    return;
                }
                if (settings.HardwareProfile.PharmacyActive)
                {
                    if (transaction is RetailTransaction)
                    {
                        Interfaces.Services.PharmacyService(entry).PayPrescription(entry, (RetailTransaction)transaction);
                    }
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void UpdateLoyaltyPoints(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                if (transaction is RetailTransaction)
                {
                    ILoyaltyService loyaltyService = Interfaces.Services.LoyaltyService(entry);

                    // Sending confirmation to the site service about earned points
                    if (!((RetailTransaction)transaction).LoyaltyItem.Empty)
                    {
                        if (((RetailTransaction)transaction).LoyaltyItem.CalculatedPoints > decimal.Zero)
                        {
                            loyaltyService.UpdateIssuedLoyaltyPoints(entry, (RetailTransaction)transaction, (LoyaltyItem)((RetailTransaction)transaction).LoyaltyItem);
                        }
                        else if (((RetailTransaction)transaction).LoyaltyItem.CalculatedPoints < decimal.Zero)
                        {
                            Interfaces.Services.LoyaltyService(entry).UpdateUsedLoyaltyPoints(entry, (RetailTransaction)transaction, 1, ((RetailTransaction)transaction).LoyaltyItem.CalculatedPoints, false);
                        }
                    }

                    // Sending confirmation to the site service about used points
                    foreach (TenderLineItem tenderItem in ((RetailTransaction)transaction).TenderLines.Where(w => w is LoyaltyTenderLineItem && w.Voided == false))
                    {
                        if (((LoyaltyTenderLineItem)tenderItem).Points < 0)
                        {
                            loyaltyService.UpdateUsedLoyaltyPoints(entry, (RetailTransaction)transaction, (LoyaltyTenderLineItem)tenderItem);
                        }
                        else if (((LoyaltyTenderLineItem)tenderItem).Points > 0)
                        {
                            loyaltyService.UpdateIssuedLoyaltyPoints(entry, (RetailTransaction)transaction, (LoyaltyItem)((RetailTransaction)transaction).LoyaltyItem, tenderItem.LineId, ((LoyaltyTenderLineItem)tenderItem).CardNumber, ((LoyaltyTenderLineItem)tenderItem).Points);
                        }
                    }
                }
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "UpdateLoyaltyPoints() END at" +
                    DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString(), this.ToString());
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void UpdateCustomerLedger(IConnectionManager entry, IPosTransaction transaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (transaction is CustomerPaymentTransaction)
            {
                if (transaction.EntryStatus == TransactionStatus.Normal)
                {
                    Interfaces.Services.CustomerService(entry).PaymentIntoCustomerAccount(entry, (CustomerPaymentTransaction)transaction, entry.CurrentStoreID, entry.CurrentTerminalID);
                }
            }
            else if (transaction is RetailTransaction && transaction.EntryStatus == TransactionStatus.Normal)
            {
                CustomerTenderLineItem tenderItem = (CustomerTenderLineItem)((RetailTransaction)transaction).TenderLines.FirstOrDefault(p => p is CustomerTenderLineItem);

                try
                {
                    if (tenderItem != null && !tenderItem.Voided)
                    {
                        decimal amountCur = tenderItem.Amount;
                        IRetailTransaction rTransaction = (IRetailTransaction)transaction;

                        //convert from the store-currency to the company-currency...
                        decimal amountCom = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                            entry,
                            settings.Store.Currency,
                            settings.CompanyInfo.CurrencyCode,
                            settings.CompanyInfo.CurrencyCode,
                            amountCur);

                        decimal dis = rTransaction.TotalDiscountWithTax + rTransaction.LineDiscountWithTax + rTransaction.PeriodicDiscountWithTax;
                        decimal amountDiscCom = Interfaces.Services.CurrencyService(entry).CurrencyToCurrency(
                            entry,
                            settings.Store.Currency,
                            settings.CompanyInfo.CurrencyCode,
                            settings.CompanyInfo.CurrencyCode,
                            dis);

                        if (amountCur > 0)
                        {
                            Interfaces.Services.CustomerService(entry).CustomerAccountPayment(entry,
                                                                                                           rTransaction.Customer.ID,
                                                                                                           rTransaction.ReceiptId,
                                                                                                           rTransaction.StoreCurrencyCode,
                                                                                                           amountCur,
                                                                                                           amountCom,
                                                                                                           dis,
                                                                                                           amountDiscCom,
                                                                                                           entry.CurrentStoreID,
                                                                                                           entry.CurrentTerminalID,
                                                                                                           rTransaction.TransactionId);
                        }
                        else
                        {
                            Interfaces.Services.CustomerService(entry).CustomerAccountCreditMemo(entry,
                                                                                                              rTransaction.Customer.ID,
                                                                                                              rTransaction.ReceiptId,
                                                                                                              rTransaction.StoreCurrencyCode,
                                                                                                              amountCur,
                                                                                                              amountCom,
                                                                                                              dis,
                                                                                                              amountDiscCom,
                                                                                                              entry.CurrentStoreID,
                                                                                                              entry.CurrentTerminalID,
                                                                                                              rTransaction.TransactionId);
                        }
                    }

                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "UpdateCustomerLedger() END at " +
                        DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString(), this.ToString());
                }
                catch (Exception x)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                    throw;
                }
            }
        }

        private void UpdateCreditMemoStatus(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                if (transaction is RetailTransaction)
                {
                    foreach (TenderLineItem tenderItem in ((RetailTransaction)transaction).TenderLines.Where(w => w is CreditMemoTenderLineItem && !w.Voided))
                    {                        
                        if (tenderItem.Amount > 0)
                        {
                            // The customer is paying with a credit memo, so we need to mark it as used.
                            Interfaces.Services.CreditMemoService(entry).UpdateCreditMemo(
                                entry,
                                ((CreditMemoTenderLineItem)tenderItem).SerialNumber,
                                tenderItem.Amount,
                                transaction);
                        }
                        else if (tenderItem.Amount < 0)
                        {
                            // We are issueing a credit memo to the customer.
                            Interfaces.Services.CreditMemoService(entry).IssueCreditMemo(entry, (CreditMemoTenderLineItem)tenderItem, transaction);
                        }
                    }
                }
                if (transaction is CustomerPaymentTransaction)
                {
                    foreach (TenderLineItem tenderItem in ((CustomerPaymentTransaction)transaction).TenderLines.Where(w => w is CreditMemoTenderLineItem && !w.Voided))
                    {
                        if (tenderItem.Amount > 0)
                        {
                            // The customer is paying with a credit memo, so we need to mark it as used.
                            Interfaces.Services.CreditMemoService(entry).UpdateCreditMemo(
                                entry,
                                ((CreditMemoTenderLineItem)tenderItem).SerialNumber,
                                tenderItem.Amount,
                                transaction);
                        }
                        else if (tenderItem.Amount < 0)
                        {
                            // We are issueing a credit memo to the customer.
                            Interfaces.Services.CreditMemoService(entry).IssueCreditMemo(entry, (CreditMemoTenderLineItem)tenderItem, transaction);
                        }

                    }
                }
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "UpdateCreditMemoStatus() END at " +
                   DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString(), this.ToString());
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void UpdateGiftCardStatus(IConnectionManager entry, IPosTransaction transaction)
        {
            try
            {
                if (transaction is RetailTransaction)
                {
                    RetailTransaction retailTransaction = (RetailTransaction)transaction;
                    foreach (ISaleLineItem saleItem in retailTransaction.SaleItems.Where(w => w.ItemClassType == SalesTransaction.ItemClassTypeEnum.GiftCertificateItem && w.Voided == false))
                    {                        
                        Interfaces.Services.GiftCardService(entry).GiftCardPaid(entry, (GiftCertificateItem)saleItem, transaction.ReceiptId);
                    }

                    foreach (GiftCertificateTenderLineItem tender in retailTransaction.TenderLines.Where(w => w is GiftCertificateTenderLineItem))
                    {                     
                        Interfaces.Services.GiftCardService(entry).UpdateGiftCardPaymentReceipt(entry, tender, retailTransaction);
                    }
                }

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "UpdateGiftCardStatus() END at " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString(), this.ToString());
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private void UpdateCoupons(IConnectionManager entry, IPosTransaction transaction)
        {
            //Any updates that are needed to do regarding Coupons before the transaction is concluded, saved and printed
            //should be done here
            Interfaces.Services.CouponService(entry).UpdateCoupons(entry, transaction);
        }

        public void ReturnSequencedNumbers(IConnectionManager entry, IPosTransaction posTransaction, bool returnReceiptID, bool returnTransactionID)
        {
            if (posTransaction == null)
            {
                return;
            }

            // Return the sequenced numbers
            if (returnReceiptID && Interfaces.Services.ApplicationService(entry).ReceiptSequenceProvider != null && !String.IsNullOrEmpty(posTransaction.ReceiptId))
            {
                RecordIdentifier receiptSequenceID;

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (!String.IsNullOrEmpty((string)settings.Terminal.ReceiptIDNumberSequence))
                {
                    receiptSequenceID = (string)settings.Terminal.ReceiptIDNumberSequence;
                }
                else
                {
                    receiptSequenceID = Interfaces.Services.ApplicationService(entry).ReceiptSequenceProvider.SequenceID;
                }

                Providers.NumberSequenceData.ReturnNumberToSequence(entry, receiptSequenceID, posTransaction.ReceiptId);
                posTransaction.ReceiptId = "";
            }

            if (returnTransactionID)
            {
                Providers.NumberSequenceData.ReturnNumberToSequence(entry, TransactionProviders.PosTransactionData.SequenceID, posTransaction.TransactionId);
                posTransaction.TransactionId = "";
            }
        }

        public string CheckTenderStatusInDrawer(IConnectionManager entry, IPosTransaction posTransaction)
        {
            if (posTransaction is RetailTransaction
                 || posTransaction is SafeDropTransaction
                 || posTransaction is BankDropTransaction
                 || posTransaction is RemoveTenderTransaction)
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                // We don't want to calculate the expected amounts if no tender types have any limitation
                List<StorePaymentMethod> storePaymentMethods = Providers.StorePaymentMethodData.GetRecords(entry, entry.CurrentStoreID, false, CacheType.CacheTypeApplicationLifeTime);

                if (!storePaymentMethods.Any(p => p.MaximumAmountInPOSEnabled))
                {
                    return "";
                }

                // Show how much cash is in the drawer
                DateTime lastTenderDeclarationTime = Providers.PaymentTransactionData.GetNextCountingDate(
                    entry,
                    (int)TypeOfTransaction.TenderDeclaration,
                    entry.CurrentTerminalID,
                    entry.CurrentStoreID);

                List<PaymentTransaction> expectedAmounts = Providers.PaymentTransactionData.GetRequiredDropAmounts(
                    entry,
                    lastTenderDeclarationTime,
                    entry.CurrentTerminalID,
                    entry.CurrentStoreID);

                Func<string, string> getTenderExceededMessage = tenderTypeId =>
                {
                    string tenderName = storePaymentMethods.FirstOrDefault(x => x.PaymentTypeID.StringValue == tenderTypeId)?.Text ?? string.Empty;
                    return Resources.TenderRemovalRequired.Replace("#1", tenderName);
                };

                StringBuilder tendersExceeded = new StringBuilder();
                if (posTransaction is RetailTransaction)
                {
                    List<string> checkedTenders = new List<string>();
                    foreach (ITenderLineItem tenderLine in posTransaction.ITenderLines)
                    {
                        if (!checkedTenders.Contains(tenderLine.TenderTypeId))
                        {
                            if (VerifyTenderTypeDrawerStatus(entry, expectedAmounts, tenderLine.TenderTypeId))
                            {
                                tendersExceeded.AppendLine(getTenderExceededMessage(tenderLine.TenderTypeId));
                            }
                            checkedTenders.Add(tenderLine.TenderTypeId);
                        }
                    }
                }
                else
                {
                    var tenderTypeId = (string)Providers.StorePaymentMethodData.GetCashTender(entry, settings.Store.ID);

                    if (VerifyTenderTypeDrawerStatus(entry, expectedAmounts, tenderTypeId))
                    {
                        tendersExceeded.AppendLine(getTenderExceededMessage(tenderTypeId));
                    }
                }

                return tendersExceeded.ToString();
            }

            return string.Empty;
        }

        private bool VerifyTenderTypeDrawerStatus(IConnectionManager entry, List<PaymentTransaction> expectedAmounts, string tenderTypeId)
        {
            RecordIdentifier storeAndPayment = entry.CurrentStoreID;
            storeAndPayment.SecondaryID = tenderTypeId;
            StorePaymentMethod paymentMethod = Providers.StorePaymentMethodData.Get(entry, storeAndPayment, CacheType.CacheTypeTransactionLifeTime);
            if (paymentMethod != null && paymentMethod.MaximumAmountInPOSEnabled)
            {
                decimal amountInDrawer = 0;
                bool containsTender = expectedAmounts.Any(x => x.TenderType == tenderTypeId);
                if (containsTender)
                {
                    PaymentTransaction firstOrDefault = expectedAmounts.FirstOrDefault(x => x.TenderType == tenderTypeId);
                    if (firstOrDefault != null)
                    {
                        amountInDrawer = firstOrDefault.AmountOfCurrency;
                    }
                }
                if (amountInDrawer > paymentMethod.MaximumAmountInPOS)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool SiteServiceConnectionIsNeededandAlive(IConnectionManager entry, IPosTransaction transaction, ISettings settings)
        {
            bool canBeIgnored = false;
            bool allowVoiding = true;
            string additionalMessage = "";

            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Checking if Site service connection is needed", ToString());

            if (!TransactionCanSendEmail(transaction))
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "This type of transaction needs a site service connection", ToString());
                return true;
            }

            bool testConnection = transaction is CustomerPaymentTransaction;

            if (!testConnection && !((RetailTransaction)transaction).CustomerOrder.Empty())
            {
                testConnection = true;
                additionalMessage = Resources.CustomerOrderCannotBeProcessedWithoutASiteServiceconnection;
            }

            if (!testConnection && !((RetailTransaction) transaction).LoyaltyItem.Empty)
            {
                testConnection = Interfaces.Services.LoyaltyService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.LoyaltyInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction) transaction).SaleItems.Any(a => !a.Voided && !string.IsNullOrEmpty(a.SerialId)))
            {
                testConnection = true;
                allowVoiding = ((RetailTransaction) transaction).SaleItems.Any(a => !a.Voided && !string.IsNullOrEmpty(a.SerialId) && a.ReceiptReturnItem);
                additionalMessage = Resources.SerialNumberInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && (((RetailTransaction) transaction).SaleItems.Any(c => !c.Voided && c.ReceiptReturnItem)))
            {                
                testConnection = settings.SiteServiceProfile.CentralizedReturns;
                additionalMessage = Resources.ReturnInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if(!testConnection && (((RetailTransaction)transaction).SaleItems.Any(x => !x.Voided && x.ReasonCode != null && x.ReasonCode.Action == ReasonActionEnum.ParkedInventory)))
            {
                testConnection = true;
                additionalMessage = Resources.ReturnInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction) transaction).SaleItems.Any(a => !a.Voided && a is GiftCertificateItem))
            {
                testConnection = Interfaces.Services.LoyaltyService(entry).SiteServiceIsNeeded(entry, transaction);
                allowVoiding = false;
                additionalMessage = Resources.GiftCardInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction) transaction).TenderLines.Any(a => !a.Voided && a is GiftCertificateTenderLineItem))
            {
                testConnection = Interfaces.Services.LoyaltyService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.GiftCardPaymentInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction)transaction).TenderLines.Any(a => !a.Voided && a is LoyaltyTenderLineItem))
            {
                testConnection = Interfaces.Services.LoyaltyService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.LoyaltyPaymentInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction)transaction).TenderLines.Any(a => !a.Voided && a is CustomerTenderLineItem))
            {
                testConnection = Interfaces.Services.CustomerService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.CustomerPaymentInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction)transaction).TenderLines.Any(a => !a.Voided && a is CreditMemoTenderLineItem))
            {
                testConnection = Interfaces.Services.CreditMemoService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.CreditMemoInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction)transaction).SaleItems.Any(a => !a.Voided && a is PharmacySalesLineItem))
            {
                testConnection = Interfaces.Services.PharmacyService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.PrescriptionInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction) transaction).SaleItems.Any(a => !a.Voided && a is SalesOrderLineItem))
            {
                testConnection = Interfaces.Services.SalesOrderService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.SalesOrderInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && ((RetailTransaction)transaction).SaleItems.Any(a => !a.Voided && a is SalesInvoiceLineItem))
            {
                testConnection = Interfaces.Services.SalesInvoiceService(entry).SiteServiceIsNeeded(entry, transaction);
                additionalMessage = Resources.SalesInvoiceInformationCannotBeUpdatedWithoutASiteServiceConnection;
            }

            if (!testConnection && TransactionCanSendEmail(transaction) && settings.SiteServiceProfile.SendReceiptEmails != ReceiptEmailOptionsEnum.Never)
            {
                if (settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.OnRequest)
                {
                    transaction.ReceiptSettings = ReceiptSettingsEnum.Ignore;
                }
                else if (settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.OnlyToCustomers && ((RetailTransaction) transaction).Customer.ID == RecordIdentifier.Empty)
                {
                    transaction.ReceiptSettings = ReceiptSettingsEnum.Ignore;
                }
                else
                {
                    testConnection = true;
                    canBeIgnored = true;
                }
            }

            if (testConnection)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Site service connection is needed and should be tested", ToString());
                //If a customer payment transaction is being voided then we need to by-pass the site service check
                if (allowVoiding && transaction.EntryStatus == TransactionStatus.Voided)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "Customer payment transaction is being voided - Site service not needed", ToString());
                    return true;
                }

                if (canBeIgnored)
                {                    
                    if (Interfaces.Services.SiteServiceService(entry).TestConnection(entry, settings.SiteServiceProfile) != ConnectionEnum.Success)
                    {
                        entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Site service connection availble - but is not necessarily needed, can be ignored", ToString());
                        transaction.ReceiptSettings = ReceiptSettingsEnum.Ignore;
                        return true;
                    }
                }
                if (Interfaces.Services.SiteServiceService(entry).TestConnectionWithFeedback(entry, settings.SiteServiceProfile, additionalMessage) != ConnectionEnum.Success)
                {
                    entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Site service connection is found, POS cannot continue concluding transaction", ToString());                    
                    return false;
                }
            }

            //If none of the conditions are met or the test connection was successful the conclution of the transaction can continue
            entry.ErrorLogger.LogMessage(LogMessageType.Trace, "No Site service connection is needed, POS can continue concluding transaction", ToString());
            return true;
        }

        public ICloneTransactions CreateCloneTransactions()
        {
            return new CloneTransactions();
        }

        protected void ShowStatusDialog(IConnectionManager entry, ISettings settings, IPosTransaction transaction, string message)
        {
            if (settings.SuppressUI)
            {
                return;
            }

            if (transaction is RetailTransaction || transaction is CustomerPaymentTransaction)
            {
                Interfaces.Services.DialogService(entry).UpdateStatusDialog(message);
            }
        }

        protected void CloseStatusDialog(IConnectionManager entry, ISettings settings)
        {
            if (settings.SuppressUI)
            {
                return;
            }

            Interfaces.Services.DialogService(entry).CloseStatusDialog();
        }

        public virtual bool OnPriceOverride(IConnectionManager entry, IPosTransaction transaction, OperationInfo operationInfo)
        {
            if (!(transaction is RetailTransaction))
            {
                return false;
            }

            // Check if there are items to override.
            if (((RetailTransaction)transaction).SaleItems.Count == 0)
            {
                return false;
            }

            if (operationInfo == null)
            {
                return false;
            }            

            //Get the item to be overridden.
            ISaleLineItem saleLineItem = (SaleLineItem)((RetailTransaction)transaction).GetItem(operationInfo.ItemLineId);

            //If the item is a sales order item
            if (saleLineItem is SalesOrderLineItem)
            {
                return Interfaces.Services.SalesOrderService(entry).PriceOverride(entry, transaction, operationInfo);
            }

            return false;
        }

        private bool CustomerOrderDepositPayback(IRetailTransaction retailTransaction)
        {
            if (retailTransaction.CustomerOrder.Empty())
            {
                return false;
            }

            //If the action is not to return deposits then return false
            if (retailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.ReturnPartialDeposit && retailTransaction.CustomerOrder.CurrentAction != CustomerOrderAction.ReturnFullDeposit)
            {
                return false;
            }

            if (retailTransaction.CustomerOrder.DepositToBeReturned > decimal.Zero)
            {
                return true;
            }

            return false;
        }
    }
}