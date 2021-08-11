using System;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services
{
    public class TransactionPrinting
    {

        /// <summary>
        /// If anything needs to be printed specifically due to items then it is done here
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction being printed</param>
        /// <param name="copyReceipt">If true then this is a copy of the receipt</param>
        /// <param name="printPreview">If true then this is not being sent to a printer but displayed on the screen</param>
        protected virtual void PrintItems(IConnectionManager entry, IRetailTransaction transaction, bool copyReceipt, bool printPreview)
        {
            // Loop through the items and see if there is a gift certificate we need to print....
            foreach (IGiftCertificateItem saleItem in transaction.SaleItems.OfType<IGiftCertificateItem>().Where(saleItem => !saleItem.Voided))
            {
                Interfaces.Services.PrintingService(entry).PrintGiftCertificate(entry, FormSystemType.GiftCertificate, transaction, saleItem, copyReceipt);
            }
        }

        /// <summary>
        /// If anything needs to be printed specifically due to tenders then it is done here
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction being printed</param>
        /// <param name="copyReceipt">If true then this is a copy of the receipt</param>
        /// <param name="printPreview">If true then this is not being sent to a printer but displayed on the screen</param>
        protected  virtual void PrintTenders(IConnectionManager entry, IRetailTransaction transaction, bool copyReceipt, bool printPreview)
        {
            IPrintingService printingService = LSOne.Services.Interfaces.Services.PrintingService(entry);
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            foreach (TenderLineItem tenderLineItem in ((RetailTransaction)transaction).TenderLines)
            {
                if (!tenderLineItem.Voided && (!copyReceipt || settings.Store.TenderReceiptAreReprinted))
                {
                    if (tenderLineItem is LoyaltyTenderLineItem)
                    {
                        printingService.PrintLoyaltyReceipt(entry, FormSystemType.LoyaltyPaymentReceipt, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                    }
                    else if (tenderLineItem is CardTenderLineItem)
                    {
                        if (tenderLineItem.Amount >= 0)
                        {
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForShop, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForCust, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                        else
                        {
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForShopReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForCustReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                    }
                    else if (tenderLineItem is CustomerTenderLineItem)
                    {
                        if (tenderLineItem.Amount >= 0)
                        {
                            printingService.PrintCustomerReceipt(entry, FormSystemType.CustAcntReceiptForShop, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCustomerReceipt(entry, FormSystemType.CustAcntReceiptForCust, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                        else
                        {
                            printingService.PrintCustomerReceipt(entry, FormSystemType.CustAcntReceiptForShopReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCustomerReceipt(entry, FormSystemType.CustAcntReceiptForCustReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                    }
                    else if (tenderLineItem is CreditMemoTenderLineItem)
                    {
                        if (tenderLineItem.Amount < 0)
                        {
                            printingService.PrintCreditMemo(entry, FormSystemType.CreditMemo, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                    }
                    else if (tenderLineItem is CorporateCardTenderLineItem)
                    {
                        if (tenderLineItem.Amount >= 0)
                        {
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForShop, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForCust, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                        else
                        {
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForShopReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                            printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForCustReturn, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                    }
                    else if (tenderLineItem is GiftCertificateTenderLineItem)
                    {
                        //If the gift card is a change back tender line then we need to create a gift certificate item and print it out as a normal gift card
                        //not a gift card balance receipt
                        if (tenderLineItem.ChangeBack)
                        {
                            GiftCertificateItem giftCard = (GiftCertificateItem)Interfaces.Services.GiftCardService(entry).CreateGiftCardFromTenderLine(entry, transaction, (IGiftCertificateTenderLineItem)tenderLineItem);
                            printingService.PrintGiftCertificate(entry, FormSystemType.GiftCertificate, (PosTransaction)transaction, giftCard, copyReceipt);
                        }
                        else
                        {
                            printingService.PrintGiftCardBalanceReceipt(entry, FormSystemType.GiftCardBalance, (PosTransaction)transaction, tenderLineItem, copyReceipt);
                        }
                    }
                }
            }
        }

        protected virtual bool ContinueWithPrinting(ISettings settings, IPosTransaction transaction, bool printPreview)
        {
            //If no printer is configured but the transaction is configured to be emailed the "printing" can continue
            if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
            {
                if (transaction.ReceiptSettings == ReceiptSettingsEnum.Email || transaction.ReceiptSettings == ReceiptSettingsEnum.PrintAndEmail)
                {
                    return true;
                }
            }

            if ((settings.HardwareProfile.Printer != HardwareProfile.PrinterHardwareTypes.None) || (printPreview))
            {
                return true;
            }

            return false;
        }


        public virtual bool PrintTransaction(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt, bool printPreview)
        {
            try
            {
                bool printResult = true;

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                IPrintingService printingService = LSOne.Services.Interfaces.Services.PrintingService(entry);

                if (ContinueWithPrinting(settings, transaction, printPreview))
                {
                    if (transaction is DepositTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal || transaction.EntryStatus == TransactionStatus.Training)
                        {
                            // Printing forms for a normal sales transaction...
                            bool okToProceed = true;

                            // Print the receipt (if preview, then show it before printing, else print the receipt after all other forms. 
                            if (printPreview)
                            {
                                okToProceed = printingService.ShowPrintPreview(entry, FormSystemType.CustomerOrderDeposit, (PosTransaction)transaction);
                                
                                printResult = okToProceed;
                            }

                            if (okToProceed)
                            {
                                //If anything needs to be printed specifically due to items then it is done here
                                PrintItems(entry, (IRetailTransaction)transaction, copyReceipt, printPreview);

                                //If anything needs to be printed specifically due to payments on the transaction then it is done here
                                PrintTenders(entry, (IRetailTransaction)transaction, copyReceipt, printPreview);

                                printResult = printingService.PrintReceipt(entry, FormSystemType.CustomerOrderDeposit, (PosTransaction)transaction, copyReceipt);
                            }
                        }
                    }
                    else if (transaction is RetailTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal || transaction.EntryStatus == TransactionStatus.Training)
                        {
                            // Printing forms for a normal sales transaction...
                            bool okToProceed = true;

                            // Print the receipt (if preview, then show it before printing, else print the receipt after all other forms. 
                            if (printPreview)
                            {
                                okToProceed = printingService.ShowPrintPreview(entry, FormSystemType.Receipt, (PosTransaction)transaction);
                                printResult = okToProceed;
                            }

                            if (okToProceed)
                            {
                                //If anything needs to be printed specifically due to items then it is done here
                                PrintItems(entry, (IRetailTransaction) transaction, copyReceipt, printPreview);

                                //If anything needs to be printed specifically due to payments on the transaction then it is done here
                                PrintTenders(entry, (IRetailTransaction) transaction, copyReceipt, printPreview);

                                // And print the receipt if we should do so...
                                printResult = printingService.PrintReceipt(entry, FormSystemType.Receipt, (PosTransaction) transaction, copyReceipt);
                                if (settings.Store.ReturnsPrintedTwice && !copyReceipt && ((RetailTransaction) transaction).SaleItems.Any(l => !l.Voided && l.Quantity < 0))
                                {
                                    printingService.PrintReceipt(entry, FormSystemType.Receipt, (PosTransaction) transaction, true);
                                }
                            }

                        }
                        else if (transaction.EntryStatus == TransactionStatus.OnHold)
                        {
                            printingService.PrintSuspendedTransaction(entry, FormSystemType.SuspendedTransaction, (PosTransaction)transaction, copyReceipt);
                        }
                        else if (transaction.EntryStatus == TransactionStatus.Voided)
                        {
                            printingService.PrintVoidedTransaction(entry, FormSystemType.VoidedTransaction, (PosTransaction)transaction, copyReceipt);
                        }
                    }
                    else if (transaction is NoSaleTransaction)
                    {
                        if(transaction.OpenDrawer)
                        {
                            printingService.PrintOpenDrawer(entry, (PosTransaction)transaction);
                        }
                    }
                    else if (transaction is CustomerPaymentTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            // Printing the forms when paying into a customer account...

                            //----------------------

                            // Because the printing functionality assumes that it is always a RetailTransaction when we are paying
                            // with card, then we need to temporarily create a RetailTransaction and pass it into the function.

                            var tempRetailTrans1 = new RetailTransaction((string)entry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice)
                                {
                                    Training = ((PosTransaction) transaction).Training,                                    
                                    TerminalId = transaction.TerminalId,
                                    StoreId = transaction.StoreId,
                                    ReceiptId = ((PosTransaction) transaction).ReceiptId,
                                    TransactionId = ((PosTransaction) transaction).TransactionId
                                };
                            tempRetailTrans1.Cashier.ID = ((PosTransaction)transaction).Cashier.ID;
                            if (((CustomerPaymentTransaction)transaction).Customer != null)
                            {
                                tempRetailTrans1.Add(((CustomerPaymentTransaction)transaction).Customer);
                            }

                            var tempItem = new SaleLineItem(tempRetailTrans1)
                                {
                                    Description = ((CustomerPaymentTransaction) transaction).CustomerDepositItem.Description,
                                    Comment = ((CustomerPaymentTransaction) transaction).CustomerDepositItem.Comment,
                                    PriceWithTax = ((CustomerPaymentTransaction) transaction).CustomerDepositItem.Amount,
                                    Quantity = 1,
                                    Found = true
                                };
                            tempRetailTrans1.Add(tempItem);

                            foreach (TenderLineItem tenderLineItem in ((CustomerPaymentTransaction)transaction).TenderLines)
                            {
                                if (!tenderLineItem.Voided)
                                {
                                    tempRetailTrans1.Add(tenderLineItem);
                                }
                            }

                            Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, tempRetailTrans1);

                            // Print the receipt (if preview, then show it before printing, else print the receipt after all other forms. 
                            if (!printPreview || printingService.ShowPrintPreview(entry, FormSystemType.CustomerAccountDeposit, tempRetailTrans1))
                            {
                                // Print the receipt
                                printingService.PrintReceipt(entry, FormSystemType.CustomerAccountDeposit, tempRetailTrans1, copyReceipt);

                                foreach (TenderLineItem tenderLineItem in ((CustomerPaymentTransaction) transaction).TenderLines)
                                {
                                    if (!tenderLineItem.Voided && tenderLineItem is CardTenderLineItem && tenderLineItem.Amount >= 0)
                                    {
                                        // Because the printing functionality assumes that it is always a RetailTransaction when we are paying
                                        // with card, then we need to temporarily create a RetailTransaction and pass it into the function.

                                        RetailTransaction tempRetailTrans = new RetailTransaction((string) entry.CurrentStoreID, settings.Store.Currency, settings.TaxIncludedInPrice);
                                        tempRetailTrans.Training = ((PosTransaction) transaction).Training;
                                        tempRetailTrans.Cashier.ID = ((PosTransaction)transaction).Cashier.ID;
                                        tempRetailTrans.TerminalId = ((PosTransaction) transaction).TerminalId;
                                        tempRetailTrans.StoreId = ((PosTransaction) transaction).StoreId;

                                        printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForShop, tempRetailTrans, tenderLineItem, copyReceipt);
                                        printingService.PrintCardReceipt(entry, FormSystemType.CardReceiptForCust, tempRetailTrans, tenderLineItem, copyReceipt);
                                    }
                                }

                                //We need to move the receipts that were created on the temporary retail transaction over to the actual transaction
                                foreach (IReceiptInfo receipt in tempRetailTrans1.Receipts)
                                {
                                    transaction.AddReceipt(receipt.PrintString, receipt.FormType, receipt.DocumentName, receipt.DocumentLocation, receipt.FormWidth, receipt.IsEmailReceipt);
                                }
                            }
                        }
                    }
                    else if (transaction is FloatEntryTransaction)
                    {
                        printingService.PrintFloatEntryReceipt(entry, ((PosTransaction)transaction));
                    }
                    else if (transaction is RemoveTenderTransaction)
                    {
                        printingService.PrintRemoveTenderReceipt(entry, ((PosTransaction)transaction));
                    }
                    else if (transaction is TenderDeclarationTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            printingService.PrintTenderDeclaration(entry, ((PosTransaction)transaction));
                        }
                    }
                    else if (transaction is BankDropTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            printingService.PrintBankDrop(entry, ((PosTransaction)transaction));
                        }
                    }
                    else if (transaction is BankDropReversalTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            printingService.PrintBankDropReversal(entry, ((PosTransaction)transaction));
                        }
                    }
                    else if (transaction is SafeDropTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            printingService.PrintSafeDrop(entry, ((PosTransaction)transaction));
                        }
                    }
                    else if (transaction is SafeDropReversalTransaction)
                    {
                        if (transaction.EntryStatus == TransactionStatus.Normal)
                        {
                            printingService.PrintSafeDropReversal(entry, ((PosTransaction)transaction));
                        }
                    }
                }

                return printResult;

            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, x.Message, x);
                throw x;
            }
        }
    }
}
