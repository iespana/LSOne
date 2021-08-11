using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class TransactionService
    {
        public virtual void ReprintReceipt(IConnectionManager entry, ISession session, IPosTransaction transaction, OperationInfo operationInfo, ReprintReceiptEnum currentReprintType, string customText)
        {
            if (currentReprintType == ReprintReceiptEnum.CustomReceipt)
            {
                Transaction lastTransaction = Providers.TransactionData.GetLastTransaction(entry);
                OperationInfo opInfo = new OperationInfo
                {
                    Parameter = customText,
                    ItemLineId = operationInfo.ItemLineId,
                    NumpadQuantity = operationInfo.NumpadQuantity,
                    NumpadValue = operationInfo.NumpadValue,
                    ReturnItems = operationInfo.ReturnItems,
                    TenderLineId = operationInfo.TenderLineId
                };

                Interfaces.Services.PrintingService(entry).PrintCustomReceipt(entry, lastTransaction, null, opInfo);

                return;
            }

            if (currentReprintType == ReprintReceiptEnum.GiftReceipt)
            {
                Transaction lastRetailTransaction = Providers.TransactionData.GetLastRetailTransaction(entry);

                PosTransaction tempTrans = LoadTransaction(entry, lastRetailTransaction.TransactionID);
                string receiptID = lastRetailTransaction.ReceiptID.ToString();
                if (string.IsNullOrEmpty(receiptID))
                {
                    // Check if the Receipt id was typed in the numpad...
                    if (string.IsNullOrEmpty(operationInfo.NumpadValue))
                    {
                        receiptID = operationInfo.NumpadValue;
                    }
                }
                PrintGiftReceipt(entry, tempTrans, receiptID, null);

                return;
            }

            if (currentReprintType == ReprintReceiptEnum.TaxFreeReceipt)
            {
                if (!Providers.TaxFreeConfigData.HasEntries(entry))
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TaxFreeHasNotBeenConfigured);
                    return;
                }
                Transaction lastRetailTransaction = Providers.TransactionData.GetLastRetailTransaction(entry);

                PosTransaction tempTrans = LoadTransaction(entry, lastRetailTransaction.TransactionID);

                if (tempTrans != null)
                {
                    ITaxFreeService service = (ITaxFreeService)entry.Service(ServiceType.TaxFreeService);
                    service.CaptureSale(entry, tempTrans);
                }

                return;
            }

            if (currentReprintType == ReprintReceiptEnum.CopyLastReceipt)
            {

                Transaction lastRetailTransaction = Providers.TransactionData.GetLastRetailTransaction(entry);                

                PosTransaction posTransaction = LoadTransaction(entry, lastRetailTransaction.TransactionID, lastRetailTransaction.StoreID, lastRetailTransaction.TerminalID);                

                IFiscalService fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                if (fiscalService != null && fiscalService.IsActive())
                {
                    //If the fiscal service doesn't allow the receipt copy to be printed then return 
                    //The fiscal service should have already explained to the user why the copy cannot be printed
                    if (!fiscalService.PrintReceiptCopy(entry, posTransaction, ReprintTypeEnum.ReceiptCopy))                    
                    {
                        return;
                    }
                }                

                //If the printing is successful then save the information about the printed copy
                if (PrintReceipt(entry, posTransaction))
                {
                    SaveReceiptCopyInformation(entry, posTransaction, ReprintTypeEnum.ReceiptCopy);
                }
            }
        }

        private void SaveReceiptCopyInformation(IConnectionManager entry, IPosTransaction transaction, ReprintTypeEnum reprintType)
        {
            //Save down information of the reprint 
            ReprintInfo reprintInfo = new ReprintInfo();
            if (!(transaction is RetailTransaction))
            {
                transaction = LoadTransaction(entry, transaction.TransactionId, transaction.StoreId, transaction.TerminalId, true);
            }
            reprintInfo.LineID = ((RetailTransaction)transaction).Reprints.Count + 1;

            reprintInfo.Staff = entry.CurrentStaffID;
            reprintInfo.Store = entry.CurrentStoreID;
            reprintInfo.Terminal = entry.CurrentTerminalID;
            reprintInfo.ReprintDate = Date.Now;
            reprintInfo.ReprintType = reprintType;

            TransactionProviders.ReprintTransactionData.Insert(entry, reprintInfo, transaction);
        }

        private bool PrintReceipt(IConnectionManager entry, PosTransaction posTransaction)
        {
            bool printedOK = false;
            try
            {
                if (posTransaction != null)
                {
                    printedOK = Interfaces.Services.PrintingService(entry).PrintTransaction(entry, posTransaction, true, true);
                    if (printedOK)
                    {
                        IFiscalService fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
                        if (fiscalService != null && fiscalService.IsActive())
                        {
                            fiscalService.SaveReceiptCopy(entry, posTransaction, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }

            return printedOK;
        }

        private PosTransaction LoadTransaction(IConnectionManager entry, RecordIdentifier transactionId)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                RetailTransaction transToReturn = new RetailTransaction("", "", true);
                TransactionProviders.PosTransactionData.GetTransaction(entry, transactionId, entry.CurrentStoreID, entry.CurrentTerminalID, transToReturn, settings.TaxIncludedInPrice);
                return transToReturn;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }

        private PosTransaction LoadTransaction(IConnectionManager entry, RecordIdentifier transactionId, RecordIdentifier storeID, RecordIdentifier terminalID, bool asRetail = false)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                TypeOfTransaction transactionType = TransactionProviders.PosTransactionData.GetTransactionType(entry, transactionId, storeID, terminalID);
                PosTransaction transaction = new RetailTransaction((string)storeID,
                                                                          settings.Store.Currency,
                                                                          settings.TaxIncludedInPrice);



                transaction.TerminalId = (string)terminalID;

                if (transactionType == TypeOfTransaction.Sales || asRetail)
                {
                    TransactionProviders.PosTransactionData.GetTransaction(entry, transactionId,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             settings.TaxIncludedInPrice);

                }
                else if (transactionType == TypeOfTransaction.Payment)
                {
                    transaction = new CustomerPaymentTransaction(settings.Store.Currency);
                    TransactionProviders.PosTransactionData.GetTransaction(entry, transactionId,
                                                             storeID,
                                                             terminalID,
                                                             transaction,
                                                             settings.TaxIncludedInPrice);

                }
                return transaction;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw;
            }
        }
    }
}
