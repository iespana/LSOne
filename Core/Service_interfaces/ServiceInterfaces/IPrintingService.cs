using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;

namespace LSOne.Services.Interfaces
{
    public interface IPrintingService : IService
    {

        /// <summary>
        /// Prints the given tender line to the given printing station
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="printingStation">The printing station to print to</param>
        /// <param name="formType">The type of receipt to get the receipt string for</param>
        /// <param name="tenderLine">The tender line to print</param>
        /// <param name="posTransaction">The transaction to generate a receipt for</param>
        /// <returns>True if the tender line was printed on the printing station, false otherwise if something went wrong</returns>
        bool PrintTenderLineToPrintingStation(IConnectionManager entry, PrintingStation printingStation, FormSystemType formType, ITenderLineItem tenderLine,
            IPosTransaction posTransaction);

        /// <summary>
        /// Prints a receipt string to the given printing station
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="printingStation">The printing station to print to</param>
        /// <param name="formType">The type of receipt to get the receipt string for</param>
        /// <param name="posTransaction">The transaction to generate a receipt for</param>
        /// <returns>True if the receipt string was printed on the printing station, false otherwise if something went wrong</returns>
        bool PrintToPrintingStation(IConnectionManager entry, PrintingStation printingStation, FormSystemType formType, IPosTransaction posTransaction);

        /// <summary>
        /// Gets the OPOS receipt string for the given transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt to get the receipt string for</param>
        /// <param name="posTransaction">The transaction to generate a receipt for</param>
        /// <returns>An OPOS print string identical to the one that would be sent out to the printer if using the function <see cref="PrintReceipt(IConnectionManager, FormSystemType, IPosTransaction, bool)"/></returns>
        string GetOPOSPrintString(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction);        
            
        

        /// <summary>
        /// Prints a specific receipt to either OPOS or Windows printer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <returns>If true the printing was successful</returns>
        bool PrintReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt);

        /// <summary>
        /// Prints a specific receipt to either OPOS or Windows printer
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="formInfo">Information about the receipt to be printed.</param>
        /// <returns>If true the printing was successful</returns>
        bool PrintReceipt(IConnectionManager entry, FormSystemType formType, FormInfo formInfo, IPosTransaction posTransaction, bool copyReceipt);

        /// <summary>
        /// Creates a receipt in PDF format
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="receiptString">The receipt after it has been transformed to an OPOS string</param>
        /// <param name="barcodePrintInfo">Information for the barcode printing</param>
        /// <param name="formWidth">The width of the print form</param>
        /// <returns>The receipt in a format the WinPrinter understands</returns>
        List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, string receiptString, IPosTransaction posTransaction, List<BarcodePrintInfo> barcodePrintInfo, int formWidth);

        /// <summary>
        /// Creates a receipt in PDF format
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="barcodePrintInfo">Information for the barcode printing</param>
        /// <returns>The receipt in a format the WinPrinter understands</returns>
        List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt, List<BarcodePrintInfo> barcodePrintInfo);

        /// <summary>
        /// Creates a receipt in PDF format
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="formInfo">The receipt after it has been transformed to an OPOS string</param>
        /// <param name="barcodePrintInfo">Information for the barcode printing</param>
        /// <returns>The receipt in a format the WinPrinter understands</returns>
        List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, FormInfo formInfo, IPosTransaction posTransaction, List<BarcodePrintInfo> barcodePrintInfo);

        /// <summary>
        /// Prints a credit card receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="tenderLineItem">The card tender line item that has the information to be printed</param>
        void PrintCardReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt);

        /// <summary>
        /// Prints a credit card receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="tenderLineItem">The card tender line item that has the information to be printed</param>
        /// <param name="eftInfo">Specific information about the card payment</param>
        void PrintCardReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, IEFTInfo eftInfo, bool copyReceipt, ICardTenderLineItem tenderLineItem);

        /// <summary>
        /// Prints a customer account receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="tenderLineItem">The tender line that has the information to be printed</param>
        void PrintCustomerReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt);

        /// <summary>
        /// Prints a float entry receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The float entry transaction</param>
        void PrintFloatEntryReceipt(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Prints a remove tender receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The remove tender transaction</param>
        void PrintRemoveTenderReceipt(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Prints a when customer is paying with a credit memo
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        /// <param name="tenderLineItem">The tender line that has the information to be printed</param>
        void PrintCreditMemo(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt);

        ///// <summary>
        ///// Prints the current balance on a credit memo
        ///// </summary>
        ///// <param name="entry">The entry into the database</param>
        ///// <param name="formType">The type of receipt that should be printed</param>
        ///// <param name="balance">The credit memo balance</param>
        ///// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        //void PrintCreditMemoBalance(IConnectionManager entry, FormSystemType formType, decimal balance, bool copyReceipt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        /// <param name="copyInvoice"></param>
        /// <param name="printPreview"></param>
        /// <returns></returns>
        bool PrintInvoice(IConnectionManager entry, IPosTransaction posTransaction, bool copyInvoice, bool printPreview);

        /// <summary>
        /// show preview 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        bool ShowPrintPreview(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction);

        /// <summary>
        /// Displays a print preview for any text
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="textToDisplay">The text to display</param>
        /// <param name="allowPrint">Is the user allowed to print the result</param>
        /// <returns>Was the text printed or not</returns>
        bool ShowPrintPreview(IConnectionManager entry, string textToDisplay, bool allowPrint);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PrintTenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PrintBankDrop(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// Print a slip containing fiscal information
        /// e.g. For the Swedish fiscalization should contain the following info: - terminal ID, - serial number of the eTax
        /// e.g. For the Default fiscalization will contain the following info: - terminal ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The pos transaction</param>
        void PrintFiscalInfo(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PrintSafeDrop(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        /// <param name="giftCertificateItem"></param>
        /// <param name="copyReceipt"></param>
        void PrintGiftCertificate(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, IGiftCertificateItem giftCertificateItem, bool copyReceipt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        void PrintGiftReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction );


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="posTransaction"></param>
        void PrintSafeDropReversal(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction"></param>
        void PrintBankDropReversal(IConnectionManager entry, IPosTransaction posTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        /// <param name="copyReceipt"></param>
        void PrintSuspendedTransaction(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        /// <param name="copyReceipt"></param>
        void PrintVoidedTransaction(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="validation"></param>
        /// <returns></returns>
        string Validate(IConnectionManager entry, string validation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="transaction"></param>
        /// <param name="copyReceipt"></param>
        /// <param name="printPreview"></param>
        bool PrintTransaction(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt, bool printPreview);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="formType"></param>
        /// <param name="posTransaction"></param>
        /// <param name="tenderLine"></param>
        /// <param name="copyReceipt"></param>
        void PrintLoyaltyReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLine, bool copyReceipt);


        /// <summary>
        /// Returns form information about a specific receipt form
        /// </summary>
        /// <param name="systemType">What form type is being requested</param>
        /// <param name="copyReceipt">Is this a copy of the receipt</param>
        /// <param name="formProfileID">The form profile that should be used to get the information about the form</param>
        /// <param name="displayNoPrintform">If false then no message is displayed if the form does not exist</param>
        /// <returns></returns>
        FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, RecordIdentifier formProfileID, bool displayNoPrintform);

        /// <summary>
        /// Returns the complete form information with the text ready for printing
        /// </summary>
        /// <param name="systemType">The form to be printed</param>
        /// <param name="transaction">The transaction to be printed</param>
        /// <param name="formInfo"></param>
        /// <param name="copyReceipt"></param>
        /// <param name="entry">The entry into the database</param>
        FormInfo GetTransformedTransaction(IConnectionManager entry, FormSystemType systemType, IPosTransaction transaction, FormInfo formInfo, bool copyReceipt);

        ///// <summary>
        ///// Returns the complete form information with the text ready for printing
        ///// </summary>
        ///// <param name="systemType">The form to be printed</param>
        ///// <param name="transaction">The transaction to be printed</param>
        ///// <param name="formInfo"></param>
        ///// <param name="copyReceipt"></param>
        ///// <param name="formProfileID">The form profile that should be used to get the receipt layout.</param>
        ///// <param name="entry">The entry into the database</param>
        //FormInfo GetTransformedTransaction(IConnectionManager entry, FormSystemType systemType, IPosTransaction transaction, FormInfo formInfo, bool copyReceipt, RecordIdentifier formProfileID);

        /// <summary>
        /// Prints a custom receipt needs tpo be configured by a partner
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="transaction"></param>
        /// <param name="customText"></param>
        /// <param name="operationInfo"></param>
        void PrintCustomReceipt(IConnectionManager entry, Transaction transaction, string customText, OperationInfo operationInfo);

        /// <summary>
        /// Prints a slip when open drawer operation is concluded without any sale
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The transaction to be printed</param>
        void PrintOpenDrawer(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Prints a gift card balance receipt
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The type of receipt that should be printed</param>
        /// <param name="posTransaction">The transaction to be printed</param>
        /// <param name="tenderLineItem">The tender line that has the information to be printed</param>
        /// <param name="copyReceipt">If true then this is a copy of a previously existing receipt</param>
        void PrintGiftCardBalanceReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt);

        /// <summary>
        /// Print a string on the printing station
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="receipt">String to print</param>
        /// <param name="printingStation">Printing station on which to print</param>
        /// <param name="formWidth">Form width. Default value is 56.</param>
        /// <returns>True if the printing was succesful</returns>
        bool PrintReceiptOnPrintingStation(IConnectionManager entry, string receipt, PrintingStation printingStation, int formWidth = 56);
    }
}
