using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class TransactionService
    {
        public void PrintGiftReceipt(IConnectionManager entry, IPosTransaction transaction, RecordIdentifier receiptID, RecordIdentifier transactionID)
        {
            RecordIdentifier store = "";
            RecordIdentifier terminal = "";

            receiptID = RetrieveReceiptID(entry, transactionID, receiptID, ref store, ref terminal);

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
            transaction = RetrieveAndPrepareTransaction(entry, transaction, ref transToReturn, receiptID, store, terminal, false, false, POSOperations.ReprintReceipt);

            //Then the transaction could not be retrieved and the operation cannot go on
            if (transaction == null)
            {
                return;
            }

            ISettings settings = ((ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication));

            // View the return dialog
            if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
            {
                // Touch screen hardware...
                using (GiftReceiptDialog dialog = new GiftReceiptDialog(entry, transToReturn))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (!UserCanReturnItems(entry, dialog.LineNumbersToPrint, transToReturn, transaction))
                        {
                            ((RetailTransaction)transaction).TenderLines.Clear();
                            ((RetailTransaction)transaction).LoyaltyItem.Clear();
                            return;
                        }

                        List<ISaleLineItem> receiptList = new List<ISaleLineItem>();
                        ISaleLineItem returnedItem;
                        foreach (int lineNum in dialog.LineNumbersToPrint)
                        {

                            returnedItem = transToReturn.GetItem(lineNum);
                            receiptList.Add(returnedItem);

                        }
                        transToReturn.SaleItems.Clear();
                        foreach (ISaleLineItem saleLineItem in receiptList)
                        {
                            transToReturn.Add(saleLineItem);
                        }
                        Interfaces.Services.PrintingService(entry).PrintGiftReceipt(entry, FormSystemType.GiftReceipt, transToReturn);
                        // The user selected to return items. Check if we can actually return those items
                        //InsertReturnedItemsIntoTransaction(dialog.ReturnedLineNumbers, transToReturn, transaction);
                    }
                    if (dialog.DialogResult == DialogResult.Cancel)
                    {
                        ((RetailTransaction)transaction).TenderLines.Clear();
                        ((RetailTransaction)transaction).LoyaltyItem.Clear();
                        return;
                    }
                }
            }           

        }

    }
}
