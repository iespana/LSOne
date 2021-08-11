using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.DataTypes;
using LSRetailPosis;

namespace LSOne.Services
{
    public partial class TransactionService
    {

        private RecordIdentifier RetrieveReceiptID(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier receiptID, ref RecordIdentifier store, ref RecordIdentifier terminal)
        {
            if (transactionID == null || string.IsNullOrEmpty((string)transactionID))
            {
                // If we don't have the receipt id at this point we need to query for it
                if (string.IsNullOrEmpty((string)receiptID))
                {
                    receiptID = GetReceiptID(entry, receiptID);
                    if (receiptID == null || string.IsNullOrEmpty((string)receiptID))
                    {
                        return RecordIdentifier.Empty;
                    }

                    // Check to see if the receipt entered is a barcode with embedded store/terminal information
                    BarcodeReceiptParseInfo parseInfo = Interfaces.Services.BarcodeService(entry).ParseBarcodeReceipt(entry, (string)receiptID);

                    if (!string.IsNullOrEmpty(parseInfo.StoreID))
                    {
                        store = parseInfo.StoreID;
                    }

                    if (!string.IsNullOrEmpty(parseInfo.TerminalID))
                    {
                        terminal = parseInfo.TerminalID;
                    }

                    return parseInfo.ReceiptID;
                }

                return receiptID;
            }

            return RecordIdentifier.Empty;
        }

        private bool ReceiptAlreadyReturned(IConnectionManager entry , IPosTransaction transaction , RecordIdentifier receiptID )
        {
            if (((IRetailTransaction)transaction).OrgReceiptId != "")
            {
                if (((RetailTransaction)transaction).SaleItems.Any(saleLineItem => saleLineItem.Voided == false && saleLineItem.ReturnReceiptId == receiptID))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.ReceiptAlreadyBeenReturned.Replace("#1", (string)receiptID));
                    return true;
                }
            }

            return false;
        }

        private RetailTransaction RetrieveTransaction(IConnectionManager entry, RetailTransaction transToReturn, RecordIdentifier receiptID,
            RecordIdentifier store, RecordIdentifier terminal, bool useCentralReturns, bool cancelTaxFree)
        {
            
            ISiteServiceService service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));
            List<ReceiptListItem> list = null;
            try
            {
                Action action = () =>
                {
                    list = service.GetTransactionListForReceiptID(entry, settings.SiteServiceProfile, receiptID, terminal, store, useCentralReturns, false);
                };
                Exception ex;
                Interfaces.Services.DialogService(entry).ShowSpinnerDialog(action, Resources.GettingTransaction, Resources.GettingTransaction, out ex);

                if (ex != null)
                {
                    throw ex;
                }
            }
            catch (Exception e)
            {
                if (e.GetType().Name == "EndpointNotFoundException")
                {                    
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.CouldNotConnectToSiteService, MessageDialogType.ErrorWarning);
                    return null;
                }
                else if (e is ClientTimeNotSynchronizedException)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(e.Message, MessageDialogType.ErrorWarning);
                    return null;
                }

                throw;
            }



            ReceiptListItem selectedSale = new ReceiptListItem();

            if (list == null || list.Count == 0)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NoTransactionsFoundForReceipt);
                return null;
            }

            if (list != null && list.Count == 1)
            {
                selectedSale = list[0];
            }

            if (list != null && list.Count > 1)
            {
                if (store != "" && terminal != "")
                {
                    selectedSale = list.FirstOrDefault(f => f.StoreID == store && f.TerminalID == terminal);
                }

                if (selectedSale == null || selectedSale.Empty())
                {
                    ReceiptListDialog dlg = new ReceiptListDialog(entry, list);
                    DialogResult result = dlg.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return null;
                    }

                    if (result == DialogResult.OK)
                    {
                        selectedSale = list.FirstOrDefault(f => f.StoreID == dlg.SelectedStoreID && f.TerminalID == dlg.SelectedTerminalID);
                        if (selectedSale == null)
                        {
                            return null;
                        }
                    }
                }
            }

            if (cancelTaxFree && selectedSale.TaxRefundID != Guid.Empty)
            {
                ITaxFreeService taxFreeService = Interfaces.Services.TaxFreeService(entry);
                if (taxFreeService != null)
                {
                    if (!taxFreeService.CancelTaxRefund(entry, selectedSale.TaxRefundID))
                    {
                        return null;
                    }
                }
            }
            XElement transactionXML = null;

            Action getTransactionAction = () =>
            {
                transactionXML = service.GetTransactionXML(entry, settings.SiteServiceProfile, selectedSale.ID, selectedSale.StoreID, selectedSale.TerminalID, useCentralReturns);
            };

            Exception exception;
            Interfaces.Services.DialogService(entry).ShowSpinnerDialog(getTransactionAction, Resources.GettingTransaction, Resources.GettingTransaction, out exception);
            if (exception != null)
            {
                throw exception;
            }
            if (transactionXML == null)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.SaleCouldNotBeRetrievedOrFound);
                return null;
            }

            transToReturn.ToClass(transactionXML);

            return transToReturn;
        }

        private RetailTransaction RetrieveAndPrepareTransaction(IConnectionManager entry, IPosTransaction transaction, ref RetailTransaction transToReturn, RecordIdentifier receiptID, 
            RecordIdentifier store, RecordIdentifier terminal, bool useCentralReturns, bool cancelTaxFree, POSOperations operation)
        {

            transToReturn = RetrieveTransaction(entry, transToReturn, receiptID, store, terminal, useCentralReturns, cancelTaxFree);

            if (transToReturn == null)
            {
                return null;
            }

            // Here we need to go through the items in the transaction and remove those that cannot be selected for return.
            // E.g. gift cards, credit vouchers, voided items, sales orders & invoices, returned items, unrecognized items, etc....
            RemoveNonReturnableEntities(ref transToReturn);

            Interfaces.Services.CalculationService(entry).CalculateTotals(entry, transToReturn);

            // Check if there are any items left in the transaction to return...
            if (transToReturn.SaleItems.Count == 0)
            {
                // This transaction does not include any items that can be returned.
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TransactionWithoutReturnableItems, MessageBoxButtons.OK, MessageDialogType.Attention);
                return null;
            }

            PreTriggerResults results = new PreTriggerResults();

            PosTransaction posTransaction = (PosTransaction)transaction;

            if (!transToReturn.LoyaltyItem.Empty)
            {
                ((RetailTransaction)posTransaction).LoyaltyItem = (LoyaltyItem)transToReturn.LoyaltyItem.Clone();
                ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPointsAmount = decimal.Zero;
                ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPoints = decimal.Zero;

                if (operation == POSOperations.ReturnTransaction)
                {
                    if (transToReturn.TenderLines.Count(c => c.TypeOfTender == TenderTypeEnum.LoyaltyTender) > 0)
                    {
                        foreach (ITenderLineItem item in transToReturn.TenderLines.Where(c => c.TypeOfTender == TenderTypeEnum.LoyaltyTender))
                        {
                            LoyaltyTenderLineItem clone = (LoyaltyTenderLineItem) item.Clone();
                            clone.Amount *= -1;
                            clone.Points *= -1;
                            ((RetailTransaction) posTransaction).Add(clone);
                        }
                    }
                }
            }

            ICloneTransactions cloning = Interfaces.Services.TransactionService(entry).CreateCloneTransactions();
            cloning.CloneEFTExtraInfo(entry, transToReturn);
            posTransaction.IOriginalTenderLines = CollectionHelper.Clone<ITenderLineItem, List<ITenderLineItem>>(transToReturn.TenderLines);
            cloning.CloneEFTExtraInfo(entry, posTransaction);

            if (operation == POSOperations.ReturnTransaction)
            {

                Interfaces.Services.TenderRestrictionService(entry).ClearTenderRestriction(entry, (RetailTransaction)posTransaction);

                ApplicationTriggers.ITransactionTriggers.PreReturnTransaction(entry, results, transToReturn, posTransaction);
                if (!TriggerHelpers.ProcessPreTriggerResults(entry, results))
                {
                    return null;
                }
            }

            return (RetailTransaction)posTransaction;
        }

        public void ReturnTransaction(IConnectionManager entry, ISession session, IPosTransaction transaction, RecordIdentifier receiptID, RecordIdentifier transactionID, bool useCentralReturns, bool showReasonCodesSelectList, string defaultReasonCodeID)
        {
            RecordIdentifier store = "";
            RecordIdentifier terminal = "";

            //Find the receipt ID either from the transaction ID or ask the user for it
            receiptID = RetrieveReceiptID(entry, transactionID, receiptID, ref store, ref terminal);

            //If no receipt ID is entered or found then the operation cannot continue
            if (receiptID == RecordIdentifier.Empty)
            {
                return;
            }

            //Check to see if the receipt has already been returned
            if (ReceiptAlreadyReturned(entry, transaction, receiptID))
            {
                return;
            }

            RetailTransaction transToReturn = new RetailTransaction("", "", true);
            
            //Retrieve and prepare the transaction for return. All error messages are displayed in this function
            transaction = RetrieveAndPrepareTransaction(entry, transaction, ref transToReturn, receiptID, store, terminal, useCentralReturns, true, POSOperations.ReturnTransaction);

            //Then the transaction could not be retrieved and the operation cannot go on
            if (transaction == null)
            {
                return;
            }

            ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

            // View the return dialog
            if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                LinkedList<ReturnedItemReason> returnedItems = new LinkedList<ReturnedItemReason>();
                DialogResult result = Services.Interfaces.Services.DialogService(entry).ShowReturnItemsDialog(entry, transToReturn, ReturnTransactionDialogBehaviourEnum.ReturnTransaction, showReasonCodesSelectList, defaultReasonCodeID, ref returnedItems);

                if (result == DialogResult.OK)
                {
                    if (!UserCanReturnItems(entry, new LinkedList<int>(returnedItems.Select(x => x.LineNum)), transToReturn, transaction))
                    {
                        ((RetailTransaction)transaction).TenderLines.Clear();
                        ((RetailTransaction)transaction).LoyaltyItem.Clear();
                        return;
                    }

                    // The user selected to return items. Check if we can actually return those items
                    InsertReturnedItemsIntoTransaction(entry, returnedItems, transToReturn, transaction);
                }

                if (result == DialogResult.Cancel)
                {
                    ((RetailTransaction)transaction).TenderLines.Clear();
                    ((RetailTransaction)transaction).LoyaltyItem.Clear();
                    return;
                }
            }

            //Are there any items that we actually return and do not become sale items?
            bool canShowReturnTransInfocode = transToReturn.SaleItems.Any(x => !x.QtyBecomesNegative && x.ReturnedQty > 0);

            if (((string)settings.FunctionalityProfile.InfocodeReturnTransaction).Trim() != "" && canShowReturnTransInfocode)
            {
                Services.Interfaces.Services.InfocodesService(entry).ProcessInfocode(
                    entry,
                    session,
                    (PosTransaction)transaction,
                    0,
                    0,
                    (string)settings.FunctionalityProfile.ID,
                    ((string)settings.FunctionalityProfile.InfocodeReturnTransaction).Trim(),
                    "",
                    InfoCodeLineItem.TableRefId.FunctionalityProfile,
                    "",
                    null,
                    InfoCodeLineItem.InfocodeType.Header,
                    true);

                Services.Interfaces.Services.InfocodesService(entry).ProcessLinkedInfocodes(entry, session,
                                                              (PosTransaction)transaction,
                                                              InfoCodeLineItem.TableRefId.FunctionalityProfile,
                                                              InfoCodeLineItem.InfocodeType.Header);
            }

            // Notify the display about the operation
            custdisplayWorker = new BackgroundWorker();
            object[] args = new object[2];
            if (((RetailTransaction)transaction).SaleItems.Count == 1)
            {
                custdisplayWorker.DoWork += custdisplaySaleLineWorker_DoWork;
                args[0] = transaction;
                args[1] = ((RetailTransaction)transaction).SaleItems.First.Value;
                custdisplayWorker.RunWorkerAsync(args);
            }
            else if (((RetailTransaction)transaction).SaleItems.Count > 1)
            {
                Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, (PosTransaction)transaction);
                args[0] = ((RetailTransaction)transaction).TransSalePmtDiff;

                custdisplayWorker.DoWork += custdisplayBalanceWorker_DoWork;
                custdisplayWorker.RunWorkerAsync(args);
            }

            //DO NOT REMOVE - IMPORTANT
            ((RetailTransaction)transaction).SaleIsReturnSale = true;
        }

        private void RemoveNonReturnableEntities(ref RetailTransaction transToReturn)
        {
            // Here we need to go through the items in the transaction and remove those that cannot be selected for return.
            // E.g. gift cards, voided items, returned items, unrecognized items, etc....

            // We do not need to worry about sales order,invoices & credit memos since they are not stored in the item table and 
            // therefore not present.

            // Here we need to check if we recognize the item in question.
            // It might be an item only available in selective stores and not in this one, for example.
            // So if we do not recognize the item in the local POS database we cannot allow it to be returned in this store and 
            // therefore we need to remove it from the item list that can be selected for return.
            // This involves checking if the item description is null or "" since the item description is not received from AX
            // but queried from the local database.

            LinkedListNode<ISaleLineItem> itemNode = transToReturn.SaleItems.First;

            while (itemNode != null)
            {
                if (!CanReturnSaleLineItem(itemNode.Value))
                {
                    LinkedListNode<ISaleLineItem> tempNode = itemNode.Next;
                    transToReturn.SaleItems.Remove(itemNode);

                    // If this has been removed but it had a DV attached to it, then this should also be removed
                    if (tempNode != null)
                    {

                        if (tempNode.Value.ItemClassType == SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem)
                        {
                            transToReturn.SaleItems.Remove(tempNode);
                            tempNode = itemNode.Next;
                        }
                    }

                    itemNode = tempNode;
                }
                else
                {
                    // Adjusting the quantity allowed to sell
                    itemNode.Value.Quantity -= itemNode.Value.ReturnedQty;
                    // The formula is taken from QuantityHandler\SetValue(); note that we are in the Quantity > 0 scenario, which simplifies the formula
                    itemNode.Value.UnitQuantity = (itemNode.Value.Quantity * itemNode.Value.UnitQuantityFactor) + itemNode.Value.MarkupAmount;

                    // Storing the returnable amount in this sale item
                    // If this is > 0, then (since it is only set here) it is used as a flag that indicates that we are returning items.
                    itemNode.Value.ReturnableAmount = itemNode.Value.Quantity;

                    if ((itemNode.Next != null) && (itemNode.Next.GetType() == typeof(DiscountVoucherItem)))
                    {
                        itemNode.Value.NetAmountWithTax += (itemNode.Next.Value.NetAmountWithTax);
                    }

                    itemNode = itemNode.Next;
                }
            }
        }

        public virtual bool CanReturnSaleLineItem(ISaleLineItem item)
        {
            if (item.Voided // We cannot return items that were voided in the original transaction
                || ((item.Quantity < 0) && item.QtyBecomesNegative == false && (item.ItemClassType != SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem)) // We cannot return items that were returned in the original transaction or have negative qty for any other reason  
                || item.GetType() == typeof(GiftCertificateItem) // We cannot return gift cards, which are stored neither with an item id nor a barcode.
                || item.GetType() == typeof(IncomeExpenseItem) // We cannot return income or expense transactions.
                || item.ItemClassType == SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem // We cannot return fuel items
                || !item.Found // We cannot return items that are not recognized on this particular till
                || !item.Returnable
                || (item.ItemClassType != SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem &&
                    ((!item.QtyBecomesNegative && item.Quantity - item.ReturnedQty <= 0) ||
                    (item.QtyBecomesNegative && item.Quantity < 0 && (item.Quantity * -1) - (item.ReturnedQty * -1) <= 0)))) 
            {
                return false;
            }

            return true;
        }

        private void InsertReturnedItemsIntoTransaction(IConnectionManager entry, LinkedList<ReturnedItemReason> returnedItems, RetailTransaction transToReturn, IPosTransaction posTransaction)
        {
            ISaleLineItem returnedItem;

            foreach (ReturnedItemReason item in returnedItems)
            {
                returnedItem = transToReturn.GetItem(item.LineNum);

                if (returnedItem.IsLinkedItem && returnedItems.ToList().Find(line => line.LineNum == returnedItem.LinkedToLineId) == null)
                {
                    returnedItem.IsLinkedItem = false;
                    returnedItem.LinkedToLineId = -1;
                }

                // Transfer the lineId from the returned transaction to the proper property in the new transaction.
                returnedItem.ReturnLineId = returnedItem.LineId;
                // Transfer the transactionId from the returned transaction to the proper property in the new transaction.
                returnedItem.ReturnReceiptId = returnedItem.Transaction.ReceiptId;
                returnedItem.ReturnTransId = returnedItem.Transaction.TransactionId;
                returnedItem.ReturnStoreId = returnedItem.Transaction.StoreId;
                returnedItem.ReturnTerminalId = returnedItem.Transaction.TerminalId;
                returnedItem.ReceiptReturnItem = true;
                returnedItem.BeginDateTime = DateTime.Now;
                returnedItem.ReasonCode = item.ReasonCode;
                returnedItem.ReturnedQty = returnedItem.Quantity;
                returnedItem.Quantity *= -1;            //this is now the quantity that is allowed to be returned
                returnedItem.QuantityDiscounted *= -1;
                returnedItem.UnitQuantity *= -1;
                if (((RetailTransaction)posTransaction).LoyaltyItem.Relation == LoyaltyPointsRelation.Discount && returnedItem.DiscountLines.Any(a => a.DiscountType == DiscountTransTypes.LoyaltyDisc))
                {
                    if (transToReturn.LoyaltyItem.CalculatedPoints < decimal.Zero)
                    {
                        returnedItem.LoyaltyPoints.CalculatedPoints += (returnedItem.LoyaltyDiscountWithTax / (transToReturn.GrossAmountWithTax - transToReturn.NetAmountWithTax)) * (transToReturn.LoyaltyItem.CalculatedPoints * -1);
                    }
                    ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPointsAmount += transToReturn.CalculateDiscountFrom == Store.CalculateDiscountsFromEnum.PriceWithTax ? returnedItem.LoyaltyDiscountWithTax : returnedItem.LoyaltyDiscount;
                    returnedItem.DiscountLines.First().Amount = decimal.Zero;
                    returnedItem.DiscountLines.First().AmountWithTax = decimal.Zero;
                    ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPoints += returnedItem.LoyaltyPoints.CalculatedPoints;
                }

                ((RetailTransaction)posTransaction).Add((ISaleLineItem)returnedItem.Clone());
            }

            if (((RetailTransaction)posTransaction).LoyaltyItem.Relation == LoyaltyPointsRelation.Discount)
            {
                ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPoints = LSOne.Services.Interfaces.Services.RoundingService(entry).RoundAmount(entry, ((RetailTransaction)posTransaction).LoyaltyItem.CalculatedPoints, 1M, TenderRoundMethod.RoundDown, CacheType.CacheTypeApplicationLifeTime);
            }

            if (transToReturn.Customer.ID != RecordIdentifier.Empty && transToReturn.Customer.ID != "" && ((RetailTransaction)posTransaction).Add(transToReturn.Customer, true))
            {
                ((RetailTransaction)posTransaction).AddInvoicedCustomer(Providers.CustomerData.Get(entry, transToReturn.Customer.AccountNumber, UsageIntentEnum.Minimal, CacheType.CacheTypeTransactionLifeTime));
            }

            ((RetailTransaction)posTransaction).OrgReceiptId = transToReturn.ReceiptId;
            ((RetailTransaction)posTransaction).OrgStore = transToReturn.StoreId;
            ((RetailTransaction)posTransaction).OrgTerminal = transToReturn.TerminalId;
            ((RetailTransaction)posTransaction).OrgTransactionId = transToReturn.TransactionId;


            ITaxService tax = Interfaces.Services.TaxService(entry);

            // Calculate taxes for all the item to be returned
            // ----------------------------------------------------------------------------------------------------
            LinkedListNode<ISaleLineItem> saleItemNode = ((RetailTransaction)posTransaction).SaleItems.First;
            while (saleItemNode != null)
            {
                if (!saleItemNode.Value.TaxExempt)
                {
                    tax.CalculateTax(entry, ((RetailTransaction)posTransaction), saleItemNode.Value);
                }

                saleItemNode = saleItemNode.Next;
            }
            // ----------------------------------------------------------------------------------------------------
            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, (PosTransaction)posTransaction);
        }

        private RecordIdentifier GetReceiptID(IConnectionManager entry, RecordIdentifier receiptID)
        {
            // If we don't have the receipt id at this point we need to query for it....
            if (string.IsNullOrEmpty((string)receiptID))
            {
                string keyboardInput = "";
                if (Interfaces.Services.DialogService(entry).KeyboardInput(ref keyboardInput, Resources.EnterAReceiptId, Resources.ReceiptID, 61, InputTypeEnum.Normal) == DialogResult.OK)
                {
                    return keyboardInput;
                }
                return null;
            }

            return receiptID;
        }

    }
}
