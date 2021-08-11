using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Peripherals;
using LSOne.Peripherals.OPOS;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using Customer = LSOne.DataLayer.BusinessObjects.Customers.Customer;

namespace LSOne.Services
{
    public partial class TransactionService
    {
        /// <summary>
        /// Creates a file name for the receipt attachment that is going to be emailed. If the transaction is a RetailTransaction then the receipt ID
        /// is used for the file name otherwise the file name is set to "TRID-" + transaction ID.
        /// </summary>
        /// <param name="transaction">The current transaction</param>
        /// <param name="formInfo">The form that is going to be emailed</param>
        /// <returns></returns>
        public virtual string CreateEmailAttachmentName(IPosTransaction transaction, FormInfo formInfo)
        {
            return string.IsNullOrEmpty(transaction.ReceiptId) ? "TRID-" + transaction.TransactionId + " - " + formInfo.FormDescription : transaction.ReceiptId + " - " + formInfo.FormDescription;
        }

        protected virtual string CreateEmailAttachmentName(IPosTransaction transaction, FormInfo formInfo, IReceiptInfo receipt, List<FormType> formTypes)
        {
            if (string.IsNullOrEmpty(receipt.DocumentName))
            {
                FormType formType = formTypes.FirstOrDefault(f => f.ID == receipt.FormType);
                formInfo.FormDescription = formType != null ? formType.Text : Conversion.ToStr(receipt.LineID);

                receipt.DocumentName = CreateEmailAttachmentName(transaction, formInfo);
            }

            return receipt.DocumentName;
        }

        public virtual void EmailReceipt(IConnectionManager entry, ISession session, IPosTransaction transaction, ReceiptEmailParameterEnum currentOption, OperationInfo operationInfo)
        {
            if (currentOption == ReceiptEmailParameterEnum.CurrentReceipt)
            {
                // We assume that since we are sending the current transaction all receipt email information has been populated
                EmailTransactionReceipts(entry, transaction);
                return;
            }

            if (currentOption == ReceiptEmailParameterEnum.LastReceipt)
            {
                Transaction lastRetailTransaction = Providers.TransactionData.GetLastRetailTransaction(entry);
                PosTransaction posTransaction = null;

                posTransaction = LoadTransaction(entry, lastRetailTransaction.TransactionID, lastRetailTransaction.StoreID, lastRetailTransaction.TerminalID);

                CreateAndSendEmailReceipt(entry, posTransaction);

                return;
            }

            if (currentOption == ReceiptEmailParameterEnum.SearchReceipt)
            {
                RecordIdentifier store = "";
                RecordIdentifier terminal = "";

                //Find the receipt ID either from the transaction ID or ask the user for it
                RecordIdentifier receiptID = RetrieveReceiptID(entry, RecordIdentifier.Empty, RecordIdentifier.Empty, ref store, ref terminal);

                //If no receipt ID is entered or found then the operation cannot continue
                if (receiptID == RecordIdentifier.Empty)
                {
                    return;
                }

                RetailTransaction transToReturn = new RetailTransaction("", "", true);

                //Retrieve and prepare the transaction for return. All error messages are displayed in this function
                transaction = RetrieveTransaction(entry, transToReturn, receiptID, store, terminal, true, true);

                //Then the transaction could not be retrieved and the operation cannot go on
                if (transaction == null)
                {
                    return;
                }

                CreateAndSendEmailReceipt(entry, transaction);
            }
        }

        public virtual void CreateAndSendEmailReceipt(IConnectionManager entry, IPosTransaction transaction)
        {
            bool doPrinting = true;

            IFiscalService fiscalService = (IFiscalService)entry.Service(ServiceType.FiscalService);
            if (fiscalService != null && fiscalService.IsActive())
            {
                doPrinting = fiscalService.PrintReceiptCopy(entry, transaction, ReprintTypeEnum.Email);
            }

            if (doPrinting)
            {
                // Get the email address from the customer.
                string email = "";

                if(transaction is RetailTransaction)
                {
                    RetailTransaction retailTransaction = (RetailTransaction)transaction;

                    if(retailTransaction.Customer != null)
                    {
                        email = retailTransaction.Customer.ReceiptEmailAddress;
                    }
                }

                if (transaction is CustomerPaymentTransaction)
                {
                    CustomerPaymentTransaction customerPaymentTransaction = (CustomerPaymentTransaction)transaction;

                    if (customerPaymentTransaction.Customer != null)
                    {
                        email = customerPaymentTransaction.Customer.ReceiptEmailAddress;
                    }
                }

                if (transaction is DepositTransaction)
                {
                    DepositTransaction depositTransaction = (DepositTransaction)transaction;

                    if (depositTransaction.Customer != null)
                    {
                        email = depositTransaction.Customer.ReceiptEmailAddress;
                    }
                }

                if (Interfaces.Services.DialogService(entry).EmailAddressInput(ref email) == DialogResult.OK)
                {
                    transaction.ReceiptEmailAddress = email;
                    transaction.ReceiptSettings = ReceiptSettingsEnum.Email;

                    EmailTransactionReceipts(entry, transaction);

                    SaveReceiptCopyInformation(entry, transaction, ReprintTypeEnum.Email);
                }
            }
        }

        public virtual bool TransactionCanSendEmail(IPosTransaction transaction)
        {
            if (transaction is DepositTransaction)
            {
                return true;
            }

            if (transaction is RetailTransaction)
            {
                return true;
            }

            if (transaction is CustomerPaymentTransaction)
            {
                return true;
            }

            return false;
        }

        protected virtual void EmailTransactionReceipts(IConnectionManager entry, IPosTransaction transaction)
        {
            if (!TransactionCanSendEmail(transaction))
            {
                return;
            }

            if (transaction.ReceiptSettings == ReceiptSettingsEnum.Ignore)
            {
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if(settings.SiteServiceProfile.EmailWindowsPrinterConfiguration == null)
            {
                return;
            }

            if (transaction.ReceiptSettings != ReceiptSettingsEnum.Printed)
            {
                if (string.IsNullOrEmpty(transaction.ReceiptEmailAddress))
                {
                    if (!settings.SuppressUI)
                    {
                        Interfaces.Services.DialogService(entry)
                            .ShowMessage(Properties.Resources.NoEmailAddressReceiptsCannotBeSent, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    return;
                }

                FormProfile storeFormProfile = Providers.FormProfileData.Get(entry, settings.Store.FormProfileID, CacheType.CacheTypeApplicationLifeTime);

                FormProfile emailFormProfile = Providers.FormProfileData.Get(entry, settings.Store.EmailFormProfileID, CacheType.CacheTypeApplicationLifeTime);
                List<FormProfileLine> emailFormProfileLines = Providers.FormProfileLineData.Get(entry, emailFormProfile.ID, CacheType.CacheTypeApplicationLifeTime);

                List<FormType> formTypes = Providers.FormTypeData.GetFormTypes(entry, FormTypeSorting.Type, false);

                //Set the predefined texts that will be used if no printforms exist
                string emailSubject = Properties.Resources.ThankYouForShoppingAt.Replace("{0}", settings.Store.Text);
                string emailText = Properties.Resources.AttachedIsYourReceipt + "\n\n" + emailSubject;

                if (storeFormProfile != null)
                {
                    FormInfo receiptSubject = Interfaces.Services.PrintingService(entry).GetInfoForForm(FormSystemType.ReceiptEmailSubject, false, storeFormProfile.ID, false);
                    if (receiptSubject != null)
                    {
                        receiptSubject = Interfaces.Services.PrintingService(entry).GetTransformedTransaction(entry, FormSystemType.ReceiptEmailSubject, transaction, receiptSubject, false);
                        if (!FormIsEmpty(receiptSubject))
                        {
                            emailSubject = receiptSubject.Header.Trim() + receiptSubject.Details.Trim() + receiptSubject.Footer.Trim();
                            emailSubject = OPOSConstants.CleanOPOSFonts(emailSubject);
                        }
                    }
                    FormInfo receiptText = Interfaces.Services.PrintingService(entry).GetInfoForForm(FormSystemType.ReceiptEmailBody, false, storeFormProfile.ID, false);
                    if (receiptText != null)
                    {
                        receiptText = Interfaces.Services.PrintingService(entry).GetTransformedTransaction(entry, FormSystemType.ReceiptEmailBody, transaction, receiptText, false);
                        if (!FormIsEmpty(receiptText))
                        {
                            emailText = receiptText.Header + receiptText.Details + receiptText.Footer;
                            emailText = OPOSConstants.CleanOPOSFonts(emailText);
                        }
                    }
                }

                EMailQueueEntry email = new EMailQueueEntry();
                email.BodyIsHTML = true;
                email.StoreID = transaction.StoreId;
                email.To = transaction.ReceiptEmailAddress;
                email.Subject = emailSubject;
                email.Body = emailText;


                FormInfo formInfo = new FormInfo();

                formInfo.WindowsPrinterName = settings.SiteServiceProfile.EmailWindowsPrinterConfiguration.PrinterDeviceName;
                formInfo.FormWidth = 56;

                List<EMailQueueAttachment> attachments = new List<EMailQueueAttachment>();

                foreach (IReceiptInfo receipt in transaction.Receipts.Where(x => x.IsEmailReceipt))
                {
                    //If the form profile exists in the Email receipt profile then it can be included as an attachment
                    FormProfileLine profileLine = emailFormProfileLines.FirstOrDefault(f => f.ReceiptTypeID == receipt.FormType);
                    if (profileLine != null)
                    {
                        formInfo.FormWidth = receipt.FormWidth;
                        receipt.DocumentName = CreateEmailAttachmentName(transaction, formInfo, receipt, formTypes);

                        List<BarcodePrintInfo> barcodePrintInfo = FindBarcodesToPrint(receipt.PrintString);

                        List<FormLine> WinPrintReceipt = Interfaces.Services.PrintingService(entry).CreateWinPrintReceipt(entry, receipt.PrintString, transaction, barcodePrintInfo, receipt.FormWidth);
                        Printer.WinPrinterPrinting(entry, settings.SiteServiceProfile.EmailWindowsPrinterConfiguration, WinPrintReceipt, formInfo, receipt.DocumentName, true);
                        string locationAndFile = settings.SiteServiceProfile.EmailWindowsPrinterConfiguration.FolderLocation + receipt.DocumentName + settings.HardwareProfile.FileType;


                        if (WaitForFileToBeCreated(locationAndFile))
                        {
                            EMailQueueAttachment attach = new EMailQueueAttachment(locationAndFile);
                            attachments.Add(attach);
                        }
                    }
                }

                Interfaces.Services.SiteServiceService(entry).QueueEMailEntry(entry, settings.SiteServiceProfile, email, attachments, true);
            }
        }

        protected virtual List<BarcodePrintInfo> FindBarcodesToPrint(string receiptText)
        {
            List<BarcodePrintInfo> barcodePrintInfo = new List<BarcodePrintInfo>();
            if (!BarcodePrintMarkers.BarcodeMarkersInText(receiptText))
            {
                return new List<BarcodePrintInfo>();
            }

            foreach (string marker in BarcodePrintMarkers.AllBarcodeMarkers)
            {
                while (receiptText.Contains(marker))
                {
                    BarcodePrintInfo info = new BarcodePrintInfo(marker, "");

                    //Locate the barcode to be printed
                    int indxB = receiptText.IndexOf(info.BarcodeMarker.Trim(), StringComparison.InvariantCultureIgnoreCase);

                    //Delete 
                    string restOfString = receiptText.Substring(indxB + info.BarcodeMarkerLength, receiptText.Length - indxB - (info.BarcodeMarkerLength));
                    string toPrint = restOfString.Substring(0, restOfString.IndexOf(BarcodePrintMarkers.BarcodeEndMarker, StringComparison.InvariantCultureIgnoreCase));

                    // Delete the barcode marker from the printed string
                    receiptText = receiptText.Remove(indxB, toPrint.Length + info.BarcodeMarkerLength + 1);

                    info.BarcodeToPrint = toPrint;
                    barcodePrintInfo.Add(info);
                }
            }

            return barcodePrintInfo;
        }

        protected virtual bool FormIsEmpty(FormInfo formInfo)
        {
            return formInfo == null || string.IsNullOrEmpty(formInfo.Header + formInfo.Details + formInfo.Footer);
        }

        protected virtual bool WaitForFileToBeCreated(string fileLocation)
        {
            int counter = 0;
            while (true)
            {
                counter++;
                try
                {
                    if (counter == 10)
                    {
                        return false;
                    }

                    FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    #if DEBUG
                    Debug.WriteLine(fileLocation);
                    Debug.WriteLine("File not found: " + counter);
                    #endif

                    System.Threading.Thread.Sleep(1000);
                }
            }

        }

        public void SetReceiptOptions(IConnectionManager entry, ISettings settings, IPosTransaction transaction)
        {

            if (!TransactionCanSendEmail(transaction))
            {
                return;
            }

            if (transaction.ReceiptSettings == ReceiptSettingsEnum.Ignore)
            {
                return;
            }

            if (settings.SiteServiceProfile.SendReceiptEmails != ReceiptEmailOptionsEnum.Never
                && Interfaces.Services.SiteServiceService(entry).IsEMailSetupForStore(entry, settings.SiteServiceProfile, transaction.StoreId, true))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.EmailHasNotBeenConfiguredNoEmailsWillBeSent + "\n" + Properties.Resources.EmailsCanOnlyBeSentAfterCloseAndRestart, MessageBoxButtons.OK, MessageDialogType.Attention);
                settings.SiteServiceProfile.SendReceiptEmails = ReceiptEmailOptionsEnum.Never;
            }


            // If a customer is on the transaction we need to check the receipt settings for the customer and act accordingly.
            // If an email should be sent we need to display a form with the registered email address and allow for the entry of a new one.
            //
            // If a customer is not on the transaction we need to check general settings of the system whether we should prompt for the 
            // receipt to be sent as an email, printed or both.

            Customer customer = transaction is RetailTransaction ? ((RetailTransaction) transaction).Customer : ((CustomerPaymentTransaction) transaction).Customer;

            if (customer.ID == RecordIdentifier.Empty)
            {
                // No customer is on the transaction so we check on the terminal setting whether we should prompt for en emailed receipt.
                if (settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.Always && !settings.SuppressUI)
                {

                    // Get the email address from the customer.
                    string email = "";

                    if (Interfaces.Services.DialogService(entry).EmailAddressInput(ref email) == DialogResult.OK)
                    {
                        transaction.ReceiptEmailAddress = email;
                        transaction.ReceiptSettings = ReceiptSettingsEnum.PrintAndEmail;
                    }
                    else
                    {
                        // The user pressed cancel to the email address prompt so we just print out the receipt and ignore all email settings.
                        transaction.ReceiptSettings = ReceiptSettingsEnum.Printed;
                    }
                }
            }
            else if (settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.Always
                     || settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.OnlyToCustomers)
            {
                // A customer is registered on the transaction
                string email = customer.ReceiptEmailAddress;
                if (Interfaces.Services.DialogService(entry).EmailAddressInput(ref email) == DialogResult.OK)
                {
                    transaction.ReceiptEmailAddress = email;

                    //If the customer is configured to only print then set the transaction now to Print and email otherwise use the configurations
                    //as they are on the customer
                    if (customer.ReceiptSettings == ReceiptSettingsEnum.Printed)
                    {
                        transaction.ReceiptSettings = ReceiptSettingsEnum.PrintAndEmail;
                    }
                    else
                    {
                        transaction.ReceiptSettings = customer.ReceiptSettings;
                    }
                }
            }
        }

    }
}
