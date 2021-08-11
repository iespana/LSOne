using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Peripherals;
using LSOne.Peripherals.OPOS;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Services.Interfaces.Constants;
using LSOne.Utilities.DataTypes;
using LSRetail.PrintingStationClient;
using LSOne.Controls.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;

namespace LSOne.Services
{
    public partial class PrintingService : IPrintingService
    {
        protected FormModulation formModulation;
        protected string spaceString = "   ";
        protected string storeCurrencyCode;

        protected const int tenderCol1 = 19;
        protected const int tenderCol2 = 4;
        protected const int tenderCol3 = 14;
        protected const int tenderCol4 = 4;
        protected const int tenderCol5 = 13;

        protected const string key = "IcelandPos1944";

        protected TransactionPrinting printing;

        protected List<FormType> formTypes;

        protected string printFileLocation;

        public PrintingService()
        {

        }

        public IErrorLog ErrorLog
        {
            set { }
        }

        public void Init(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            DLLEntry.Settings = settings;
#pragma warning restore 0612, 0618

            storeCurrencyCode = settings.Store.Currency;

            formModulation = new FormModulation(entry);
            printing = new TransactionPrinting();
            formTypes = new List<FormType>();

            printFileLocation = settings.HardwareProfile.WindowsPrinterConfiguration == null ? "" : settings.HardwareProfile.WindowsPrinterConfiguration.FolderLocation;
        }

        protected virtual string GetPromptText(FormSystemType formType)
        {
            switch (formType)
            {
                case FormSystemType.Receipt:
                    return Resources.PrintReceiptQuestion;

                case FormSystemType.CardReceiptForShop:
                    return Resources.PrintShopCardReceiptQuestion;

                case FormSystemType.CardReceiptForCust:
                    return Resources.CardReceiptForCust;

                case FormSystemType.CardReceiptForShopReturn:
                    return Resources.CardReceiptForShopReturn;

                case FormSystemType.CardReceiptForCustReturn:
                    return Resources.CardReceiptForCustReturn;

                case FormSystemType.CustAcntReceiptForShop:
                    return Resources.CustAcntReceiptForShop;

                case FormSystemType.CustAcntReceiptForCust:
                    return Resources.CustAcntReceiptForCust;

                case FormSystemType.CustAcntReceiptForShopReturn:
                    return Resources.CustAcntReceiptForShopReturn;

                case FormSystemType.CustAcntReceiptForCustReturn:
                    return Resources.CustAcntReceiptForCustReturn;

                default:
                    return Resources.CustomerAccountDeposit;

            }

        }



        // Checks if the application should ask whether to print out the respective form and then asks the question.
        // Returns true if it should not as the question.
        // Returns the users choice if it should ask the question.
        protected virtual bool ShouldWePrint(IConnectionManager entry, FormInfo formInfo, FormSystemType formType, bool copyReceipt)
        {
            bool retval = true;

            //If the formInfo was not found then return false
            if (formInfo == null)
            {
                return false;
            }

            // If this is a copy, then we always print everything without asking.
            if (copyReceipt)
            {
                return retval;
            }

            if (formInfo.PrintBehavior == PrintBehaviors.PromptUser)
            {
                DialogResult result = Interfaces.Services.DialogService(entry).ShowMessage(
                    GetPromptText(formType),
                    MessageBoxButtons.YesNo,
                    MessageDialogType.Attention); //Do you want to print a receipt?

                if (result == DialogResult.No)
                    retval = false;

            }
            else if (formInfo.PrintBehavior == PrintBehaviors.NeverPrint)
            {
                retval = false;
            }

            return retval;
        }

        // Checks if the application should ask whether to print out the respective form and then asks the question.
        // Returns true if it should not ask the question.
        // Returns the users choice if it should ask the question.
        public virtual bool ShowPrintPreview(IConnectionManager entry, string textToDisplay, bool allowPrint)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            textToDisplay = OPOSConstants.CleanOPOSFonts(textToDisplay);

            ReceiptPreviewDialog preview = new ReceiptPreviewDialog(entry, textToDisplay);
            if (preview.ShowDialog() == DialogResult.OK && allowPrint)
            {
                if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Resources.NoPrinterConfigured, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    return false;
                }
                return true;
            }
            return false;
        }

        public virtual FormInfo GetInfoForForm(FormSystemType systemType, bool copyReceipt, RecordIdentifier formProfileID, bool displayNoPrintform)
        {
            return formModulation.GetInfoForForm(systemType, copyReceipt, displayNoPrintform, formProfileID);
        }

        public virtual FormInfo GetTransformedTransaction(IConnectionManager entry, FormSystemType systemType, IPosTransaction transaction,
            FormInfo formInfo, bool copyReceipt)
        {
            if (formInfo == null || string.IsNullOrEmpty(formInfo.FormDescription))
            {
                formInfo = formModulation.GetInfoForForm(systemType, copyReceipt);
            }

            formModulation.GetTransformedTransaction(entry, systemType, transaction, formInfo);
            return formInfo;
        }

        public virtual bool ShowPrintPreview(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction)
        {
            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);

            if (!ShouldWePrint(entry, formInfo, formType, true))
            {
                return false;
            }

            formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            string textForPreview = formInfo.Header;
            textForPreview += formInfo.Details;
            textForPreview += formInfo.Footer;

            return ShowPrintPreview(entry, textForPreview, true);
        }

        protected virtual bool DoPrinting(
            IConnectionManager entry,
            FormSystemType formType,
            IPosTransaction posTransaction,
            bool copyReceipt,
            Action Windowsprinting,
            Action SlipPrinting,
            Action ReceiptPrinting,
            string logString)
        {
            //Get the receipt form and where and how to print
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt, false, false);
            return DoPrinting(entry, formType, posTransaction, formInfo, copyReceipt, Windowsprinting, SlipPrinting, ReceiptPrinting, logString);
        }

        protected virtual bool DoPrinting(
            IConnectionManager entry,
            FormSystemType formType,
            IPosTransaction posTransaction,
            FormInfo formInfo,
            bool copyReceipt,
            Action Windowsprinting,
            Action SlipPrinting,
            Action ReceiptPrinting,
            string logString)
        {
            try
            {
                //Check configurations and decide if the printing should happen
                if (ShouldWePrint(entry, formInfo, formType, copyReceipt) == false)
                    return true;

                //Print
                if (formInfo.UseWindowsPrinter)
                {
                    Windowsprinting();
                }
                else if (formInfo.PrintAsSlip)
                {
                    SlipPrinting();
                }
                else 
                {
                    ReceiptPrinting();
                }

                return true;
            }
            catch (POSException ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, logString);
                Interfaces.Services.DialogService(entry).ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Trace, logString);
                POSFormsManager.ShowPOSErrorDialog(new POSException(1003, ex));
            }

            return false;
        }

        protected virtual WindowsPrinterConfiguration GetWindowsPrinterConfiguration(IConnectionManager entry, FormInfo formInfo)
        {
            if(formInfo.UseWindowsPrinter && formInfo.WindowsPrinterConfiguration != null)
            {
                return formInfo.WindowsPrinterConfiguration;
            }

            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            HardwareProfile hardwareProfile = Providers.HardwareProfileData.Get(entry, settings.HardwareProfile.ID, CacheType.CacheTypeApplicationLifeTime);
            return Providers.WindowsPrinterConfigurationData.Get(entry, hardwareProfile.WindowsPrinterConfigurationID, CacheType.CacheTypeApplicationLifeTime);
        }

        public bool PrintTenderLineToPrintingStation(IConnectionManager entry, PrintingStation printingStation, FormSystemType formType, ITenderLineItem tenderLine, IPosTransaction posTransaction)
        {
            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);
            if (formInfo == null)
            {
                return false;
            }

            if (tenderLine is ICardTenderLineItem)
            {
                string receiptText = formModulation.GetTransformedCardTender(formType, ((ICardTenderLineItem) tenderLine).EFTInfo, (RetailTransaction) posTransaction,
                    (CardTenderLineItem) tenderLine);

                return PrintReceiptOnPrintingStation(entry, receiptText, printingStation, formInfo.FormWidth);
            }

            return true;
        }

        public bool PrintToPrintingStation(IConnectionManager entry, PrintingStation printingStation, FormSystemType formType, IPosTransaction posTransaction)
        {            
            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);
            if (formInfo == null)
            {
                return false;
            }                        

            string receiptString = formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);

            return PrintReceiptOnPrintingStation(entry, receiptString, printingStation, formInfo.FormWidth);
        }

        public bool PrintReceiptOnPrintingStation(IConnectionManager entry, string receipt, PrintingStation printingStation, int formWidth = 56)
        {
            bool result = true;
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            PrintingStationCli printingStationClient = new PrintingStationCli(printingStation.StationPrintingHostAddress, printingStation.StationPrintingHostPort);

            string receiptString = OPOSConstants.CleanAlignments(receipt);

            List<BarcodePrintInfo> barcodePrintInfo = formModulation.BarcodePrintInfoList;

            if (barcodePrintInfo.Count > 0)
            {
                if (receiptString.Contains(barcodePrintInfo[0].BarcodeMarker))
                {
                    foreach (BarcodePrintInfo info in barcodePrintInfo)
                    {
                        //Locate the barcode to be printed
                        int indxB = receiptString.IndexOf(info.BarcodeMarker.Trim(),
                            StringComparison.InvariantCultureIgnoreCase);

                        //Delete 
                        string restOfString = receiptString.Substring(indxB + info.BarcodeMarkerLength,
                            receiptString.Length - indxB - (info.BarcodeMarkerLength));
                        string toPrint = restOfString.Substring(0,
                            restOfString.IndexOf(BarcodePrintMarkers.BarcodeEndMarker,
                                StringComparison.InvariantCultureIgnoreCase));

                        // Delete the barcode marker from the printed string
                        receiptString = receiptString.Remove(indxB, toPrint.Length + info.BarcodeMarkerLength + 1);
                    }
                }
            }

            if (receiptString.Contains("<L"))
            {
                // look for logo to print
                int indxL = receiptString.IndexOf("<L", StringComparison.InvariantCultureIgnoreCase);
                if (receiptString[indxL + 2] == '>')
                {
                    receiptString = receiptString.Replace("<L>", "\x1B|1B\x1B|bC");
                }
                else
                {
                    int indxLend = receiptString.IndexOf(">", indxL, StringComparison.InvariantCultureIgnoreCase);
                    string Lvalue = receiptString.Substring(indxL, indxLend - indxL + 1);
                    int id = int.Parse(Lvalue.Substring(2, indxLend - 2));
                    receiptString = receiptString.Replace(Lvalue, string.Format("\x1B|{0}B\x1B|bC", id));
                }
            }
            
            result = printingStationClient.StationPrintEx2(printingStation.PrinterDeviceName, receiptString, false);

            IBarcodeService barcodeService = Interfaces.Services.BarcodeService(entry);

            foreach (BarcodePrintInfo info in barcodePrintInfo.Where(w => !w.Printed))
            {
                string barcodeData = barcodeService.GetReceiptBarCodeData(entry, new BarcodeReceiptParseInfo((string)entry.CurrentStoreID, (string)entry.CurrentTerminalID, info.BarcodeToPrint));

                string barcodeMessage = null;
                int barCodeType = 0;
                Size size = Size.Empty;

                barcodeService.ManageReceiptBarcode(settings.Store.BarcodeSymbology, ref barCodeType, ref barcodeData, ref barcodeMessage, ref size);

                if (barcodeMessage == null)
                {
                    result = result && printingStationClient.PrintBarcode(
                        printingStation.PrinterDeviceName,
                        2,
                        barcodeData.Trim(),
                        formWidth - 10,
                        80,
                        110,
                        -13
                        );
                }
                else
                {
                    receiptString += barcodeMessage;
                }

                //line feed add a number between '|' and 'l' to set how many line feeds.
                result = result && printingStationClient.StationPrintEx2(printingStation.PrinterDeviceName, $"{(char)27}|{1}lF", false);

                info.Printed = true;
            }

            if (settings.HardwareProfile.PrinterExtraLines > 0)
            {

               result = result && printingStationClient.StationPrintEx2(printingStation.PrinterDeviceName, $"{(char)27}|{settings.HardwareProfile.PrinterExtraLines}lF", false);
            }

            result = result && printingStationClient.CutPaper(printingStation.PrinterDeviceName, 100);

            return result;
        }

        public string GetOPOSPrintString(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);
            if (formInfo == null)
            {
                return "";
            }

            string receiptString = formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            
            return receiptString;
        }

        // Print the standard slip, returns false if printing should be aborted altogether       
        public virtual bool PrintReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt)
        {
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt);
            if (formInfo == null)
            {
                return false;
            }

            formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            string fileName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, formInfo);

            if (posTransaction != null)
            {
                //Add the receipt to the transaction
                posTransaction.AddReceipt(formInfo.Header + formInfo.Details + formInfo.Footer, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, formInfo.FormWidth, false);

                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                CreateEmailReceipt(entry, settings, formType, posTransaction, copyReceipt);

                if (posTransaction.ReceiptSettings == ReceiptSettingsEnum.Email || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return true;
                }
            }

            bool originalReceiptPrinted = DoPrinting(entry, formType, posTransaction, copyReceipt,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, formType, posTransaction, copyReceipt, formModulation.BarcodePrintInfoList),
                                formInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                formInfo.Header,
                                formInfo.Details,
                                formInfo.Footer),
                    () => Printer.PrintReceipt(
                                entry,
                                formInfo.Header + formInfo.Details + formInfo.Footer, 
                                formModulation.BarcodePrintInfoList,
                                formInfo.FormWidth),
                    MethodBase.GetCurrentMethod().Name);

            if (!copyReceipt && originalReceiptPrinted && formInfo.NumberOfCopiesToPrint > 1)
            {
                FormInfo copyFormInfo = formModulation.GetInfoForForm(formType, true, false);
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, copyFormInfo);

                for (int i = 0; i < formInfo.NumberOfCopiesToPrint - 1; i++)
                {
                    if (posTransaction != null)
                    {
                        posTransaction.AddReceipt(copyFormInfo.Header + copyFormInfo.Details + copyFormInfo.Footer, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, copyFormInfo.FormWidth, false);
                    }

                    DoPrinting(entry, formType, posTransaction, true,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, formType, posTransaction, true, formModulation.BarcodePrintInfoList),
                                copyFormInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                copyFormInfo.Header,
                                copyFormInfo.Details,
                                copyFormInfo.Footer),
                    () => Printer.PrintReceipt(
                                entry,
                                copyFormInfo.Header + copyFormInfo.Details + copyFormInfo.Footer,
                                formModulation.BarcodePrintInfoList,
                                copyFormInfo.FormWidth),
                    MethodBase.GetCurrentMethod().Name);
                }
            }

            return originalReceiptPrinted;
        }

        public virtual bool PrintReceipt(IConnectionManager entry, FormSystemType formType, FormInfo formInfo, IPosTransaction posTransaction, bool copyReceipt)
        {
            string receiptText = formInfo.Header + formInfo.Details + formInfo.Footer;
            string fileName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, formInfo);

            if (posTransaction != null)
            {
                //Add the receipt to the transaction
                posTransaction.AddReceipt(receiptText, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, formInfo.FormWidth, false);

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                CreateEmailReceipt(entry, settings, formType, posTransaction, copyReceipt);

                if (posTransaction.ReceiptSettings == ReceiptSettingsEnum.Email || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return true;
                }
            }

            bool originalReceiptPrinted = DoPrinting(entry, formType, posTransaction, formInfo, copyReceipt,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, receiptText, posTransaction, formModulation.BarcodePrintInfoList, formInfo.FormWidth),
                                formInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                receiptText),
                    () => Printer.PrintReceipt(
                                entry,
                                receiptText, formModulation.BarcodePrintInfoList),
                    MethodBase.GetCurrentMethod().Name);

            if (!copyReceipt && originalReceiptPrinted && formInfo.NumberOfCopiesToPrint > 1)
            {
                FormInfo copyFormInfo = formModulation.GetInfoForForm(formType, true, false);
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, copyFormInfo);
                receiptText = copyFormInfo.Header + copyFormInfo.Details + copyFormInfo.Footer;

                for (int i = 0; i < formInfo.NumberOfCopiesToPrint - 1; i++)
                {
                    if (posTransaction != null)
                    {
                        posTransaction.AddReceipt(receiptText, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, copyFormInfo.FormWidth, false);
                    }

                    DoPrinting(entry, formType, posTransaction, copyFormInfo, true,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, receiptText, posTransaction, formModulation.BarcodePrintInfoList, copyFormInfo.FormWidth),
                                formInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                receiptText),
                    () => Printer.PrintReceipt(
                                entry,
                                receiptText, formModulation.BarcodePrintInfoList),
                    MethodBase.GetCurrentMethod().Name);
                }
            }

            return originalReceiptPrinted;
        }

        private void CreateEmailReceipt(IConnectionManager entry, ISettings settings, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt)
        {
            if(settings.SiteServiceProfile.SendReceiptEmails == ReceiptEmailOptionsEnum.Never)
            {
                return;
            }

            bool isCashManagementReceipt = formType == FormSystemType.BankDrop || formType == FormSystemType.BankDropReversal || formType == FormSystemType.SafeDrop || formType == FormSystemType.SafeDropReversal || formType == FormSystemType.TenderDeclaration;

            //Create and save separate receipt based on email profile
            FormInfo emailFormInfo = formModulation.GetInfoForForm(formType, copyReceipt, !isCashManagementReceipt, true);

            if (emailFormInfo != null)
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, emailFormInfo, null, true);
                string emailAttachmentName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, emailFormInfo);
                posTransaction.AddReceipt(emailFormInfo.Header + emailFormInfo.Details + emailFormInfo.Footer, GetFormTypeID(entry, formType).ID, emailAttachmentName, printFileLocation, emailFormInfo.FormWidth, true);

                if(!copyReceipt && emailFormInfo.NumberOfCopiesToPrint > 1)
                {
                    FormInfo emailCopyFormInfo = formModulation.GetInfoForForm(formType, true, !isCashManagementReceipt, true);
                    formModulation.GetTransformedTransaction(entry, formType, posTransaction, emailCopyFormInfo, null, true);

                    for(int i = 0; i < emailFormInfo.NumberOfCopiesToPrint - 1; i++)
                    {
                        posTransaction.AddReceipt(emailCopyFormInfo.Header + emailCopyFormInfo.Details + emailCopyFormInfo.Footer, GetFormTypeID(entry, formType).ID, emailAttachmentName, printFileLocation, emailCopyFormInfo.FormWidth, true);
                    }
                }
            }
        }

        /// <summary>
        /// Prints a receipt using a tender line f.ex. for Customer account payment
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="formType">The form to be printed</param>
        /// <param name="posTransaction">The transaction where the tender line is</param>
        /// <param name="tenderLine">The tenderline to be printed</param>
        /// <param name="copyReceipt">Is the receipt a copy</param>
        protected virtual void PrintTenderReceipt(IConnectionManager entry, FormSystemType formType,
            IPosTransaction posTransaction, ITenderLineItem tenderLine, bool copyReceipt)
        {

            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt, false, false);
            string fileName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, formInfo);

            string receiptText = formModulation.GetTransformedTender(formType, tenderLine, (IRetailTransaction) posTransaction);

            if (posTransaction != null)
            {
                //Add the receipt to the transaction
                posTransaction.AddReceipt(receiptText, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, formInfo.FormWidth, false);

                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if(settings.SiteServiceProfile.SendReceiptEmails != ReceiptEmailOptionsEnum.Never)
                {
                    //Create and save separate receipt based on email profile
                    FormInfo emailFormInfo = formModulation.GetInfoForForm(formType, copyReceipt, true);

                    if (emailFormInfo != null)
                    {
                        formModulation.GetTransformedTender(formType, tenderLine, (IRetailTransaction)posTransaction, true);
                        string emailAttachmentName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, emailFormInfo);
                        posTransaction.AddReceipt(emailFormInfo.Header + emailFormInfo.Details + emailFormInfo.Footer, GetFormTypeID(entry, formType).ID, emailAttachmentName, printFileLocation, emailFormInfo.FormWidth, true);

                        if (!copyReceipt && emailFormInfo.NumberOfCopiesToPrint > 1)
                        {
                            FormInfo emailCopyFormInfo = formModulation.GetInfoForForm(formType, true, true);
                            formModulation.GetTransformedTender(formType, tenderLine, (IRetailTransaction)posTransaction, true);

                            for (int i = 0; i < emailFormInfo.NumberOfCopiesToPrint - 1; i++)
                            {
                                posTransaction.AddReceipt(emailCopyFormInfo.Header + emailCopyFormInfo.Details + emailCopyFormInfo.Footer, GetFormTypeID(entry, formType).ID, emailAttachmentName, printFileLocation, emailCopyFormInfo.FormWidth, true);
                            }
                        }
                    }
                }

                if (posTransaction.ReceiptSettings == ReceiptSettingsEnum.Email || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return;
                }
            }

            bool originalReceiptPrinted = DoPrinting(entry, formType, posTransaction, copyReceipt,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, receiptText, posTransaction, formModulation.BarcodePrintInfoList, formInfo.FormWidth),
                                formInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                receiptText),
                    () => Printer.PrintReceipt(
                                entry,
                                receiptText, formModulation.BarcodePrintInfoList),
                    MethodBase.GetCurrentMethod().Name);

            if(!copyReceipt && originalReceiptPrinted && formInfo.NumberOfCopiesToPrint > 1)
            {
                FormInfo copyFormInfo = formModulation.GetInfoForForm(formType, true, false, false);
                string copyReceiptText = formModulation.GetTransformedTender(formType, tenderLine, (IRetailTransaction)posTransaction);

                for(int i = 0; i < formInfo.NumberOfCopiesToPrint - 1; i++)
                {
                    if (posTransaction != null)
                    {
                        posTransaction.AddReceipt(copyReceiptText, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, copyFormInfo.FormWidth, false);
                    }

                    DoPrinting(entry, formType, posTransaction, true,
                    () => Printer.WinPrinterPrinting(
                                entry,
                                GetWindowsPrinterConfiguration(entry, formInfo),
                                CreateWinPrintReceipt(entry, copyReceiptText, posTransaction, formModulation.BarcodePrintInfoList, copyFormInfo.FormWidth),
                                copyFormInfo,
                                fileName),
                    () => Printer.PrintSlip(
                                entry,
                                copyReceiptText),
                    () => Printer.PrintReceipt(
                                entry,
                                copyReceiptText, formModulation.BarcodePrintInfoList),
                    MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        public virtual void PrintLoyaltyReceipt(IConnectionManager entry, FormSystemType formType,
            IPosTransaction posTransaction, ITenderLineItem tenderLine, bool copyReceipt)
        {
            PrintTenderReceipt(entry, formType, posTransaction, tenderLine, copyReceipt);
        }


        // Print card slips
        public virtual void PrintCardReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction,
            ITenderLineItem tenderLineItem, bool copyReceipt)
        {
            
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt);

            if (ShouldWePrint(entry, formInfo, formType, copyReceipt) == false)
            {
                return;
            }

            //Check is the POS should print out the EFT or if the EFT device did it for us
            IRetailTransaction retailTransaction = posTransaction as IRetailTransaction;
            if (retailTransaction != null && retailTransaction.TenderLines != null)
            {
                foreach (ITenderLineItem tenderLine in retailTransaction.TenderLines.Where(w => w is ICardTenderLineItem))
                {
                    ICardTenderLineItem cardLineItem = tenderLine as ICardTenderLineItem;
                    if (cardLineItem != null && !PrintEFTReceipt(entry, cardLineItem.EFTInfo, formType))
                    {
                        return;
                    }
                }
            }

            string receiptText = formModulation.GetTransformedCardTender(formType, ((ICardTenderLineItem) tenderLineItem).EFTInfo, (RetailTransaction) posTransaction, (CardTenderLineItem) tenderLineItem);
            string fileName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, formInfo);

            if (posTransaction != null)
            {
                //Add the receipt to the transaction
                posTransaction.AddReceipt(receiptText, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, formInfo.FormWidth, false);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (posTransaction.ReceiptSettings == ReceiptSettingsEnum.Email || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return;
                }
            }

            DoPrinting(entry, formType, posTransaction, copyReceipt,
                () => Printer.WinPrinterPrinting(
                            entry,
                            GetWindowsPrinterConfiguration(entry, formInfo),
                            CreateWinPrintReceipt(entry, receiptText, posTransaction, formModulation.BarcodePrintInfoList, formInfo.FormWidth),
                            formInfo,
                            posTransaction.ReceiptId + " - " + formType),
                () => Printer.PrintSlip(
                            entry,
                            receiptText),
                () => Printer.PrintReceipt(
                            entry,
                            receiptText, formModulation.BarcodePrintInfoList),
                MethodBase.GetCurrentMethod().Name);
        }

        // Print card slips
        public virtual void PrintCardReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction,
            IEFTInfo eftInfo, bool copyReceipt, ICardTenderLineItem tenderLineItem)
        {
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt);

            if (ShouldWePrint(entry, formInfo, formType, copyReceipt) == false)
            {
                return;
            }

            //Check if the POS should be printing out the receipt or not
            if (!PrintEFTReceipt(entry, eftInfo, formType))
            {
                return;
            }

            string receiptText = formModulation.GetTransformedCardTender(formType, tenderLineItem.EFTInfo, (RetailTransaction) posTransaction, tenderLineItem);
            string fileName = Interfaces.Services.TransactionService(entry).CreateEmailAttachmentName(posTransaction, formInfo);

            if (posTransaction != null)
            {
                //Add the receipt to the transaction
                posTransaction.AddReceipt(formInfo.Header + formInfo.Details + formInfo.Footer, GetFormTypeID(entry, formType).ID, fileName, printFileLocation, formInfo.FormWidth, false);
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (posTransaction.ReceiptSettings == ReceiptSettingsEnum.Email || settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return;
                }
            }
            
            DoPrinting(entry, formType, posTransaction, copyReceipt,
                () => Printer.WinPrinterPrinting(
                            entry,
                            GetWindowsPrinterConfiguration(entry, formInfo),
                            CreateWinPrintReceipt(entry, receiptText, posTransaction, formModulation.BarcodePrintInfoList, formInfo.FormWidth),
                            formInfo,
                            posTransaction.ReceiptId + " - " + formType),
                () => Printer.PrintSlip(
                            entry,
                            receiptText),
                () => Printer.PrintReceipt(
                            entry,
                            receiptText, formModulation.BarcodePrintInfoList),
                MethodBase.GetCurrentMethod().Name);
        }

        // Print customer account slips
        public virtual void PrintCustomerReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt)
        {
            PrintTenderReceipt(entry, formType, posTransaction, tenderLineItem, copyReceipt);
        }
        
        public virtual void PrintGiftCardBalanceReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, bool copyReceipt)
        {
            PrintTenderReceipt(entry, formType, posTransaction, tenderLineItem, copyReceipt);
        }

        public virtual void PrintFloatEntryReceipt(IConnectionManager entry, IPosTransaction posTransaction)
        {
            FormSystemType formType = FormSystemType.FloatEntry;
            
            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);
            if (formInfo == null)
            {
                return;
            }
            formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);

            PrintReceipt(entry, formType, formInfo, posTransaction, false);
        }

        public virtual void PrintRemoveTenderReceipt(IConnectionManager entry, IPosTransaction posTransaction)
        {
            FormSystemType formType = FormSystemType.RemoveTender;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false);
            if (formInfo == null)
            {
                return;
            }
            formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);

            PrintReceipt(entry, formType, formInfo, posTransaction, false);
        }

        public virtual void PrintCreditMemo(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction,
            ITenderLineItem tenderLineItem, bool copyReceipt)
        {
            PrintTenderReceipt(entry, formType, posTransaction, tenderLineItem, copyReceipt);
        }

        public virtual bool PrintInvoice(IConnectionManager entry, IPosTransaction posTransaction, bool copyInvoice,
            bool printPreview)
        {
            try
            {
                FormSystemType formType = posTransaction is DepositTransaction ? FormSystemType.CustomerOrderDeposit : FormSystemType.Invoice;
                return PrintReceipt(entry, formType, posTransaction, copyInvoice);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw ex;
            }
        }

        public virtual void PrintTenderDeclaration(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            
            FormSystemType formType = FormSystemType.TenderDeclaration;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreateCodedTenderDeclarationForm(entry, posTransaction, settings);
            }
            else
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            }


            PrintReceipt(entry, formType, formInfo, posTransaction, false);
        }

        public virtual void PrintBankDropReversal(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            FormSystemType formType = FormSystemType.BankDropReversal;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreateCodedBankDropReversalForm(entry, posTransaction, settings);
            }
            else
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            }

            PrintReceipt(entry, formType, formInfo, posTransaction, false); 

        }

        public virtual void PrintBankDrop(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            FormSystemType formType = FormSystemType.BankDrop;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreateCodedBankDropForm(entry, posTransaction, settings);
            }
            else
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            }

            PrintReceipt(entry, formType, formInfo, posTransaction, false);

        }

        /// <summary>
        /// Print a slip containing fiscal information
        /// e.g. For the Swedish fiscalization should contain the following info: - terminal ID, - serial number of the eTax
        /// e.g. For the Default fiscalization will contain the following info: - terminal ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The pos transaction</param>
        public virtual void PrintFiscalInfo(IConnectionManager entry, IPosTransaction posTransaction)
        {
            const string fiscalInfoFormTypeId = "0D9866C4-99A2-46D0-A667-4A54D282866B";
            DataLayer.BusinessObjects.Forms.Form form = Providers.FormData.GetFormsOfType(entry, 
                new RecordIdentifier(fiscalInfoFormTypeId), FormSorting.Description, false).FirstOrDefault();

            if (form != null)
            {
                var formType = FormSystemType.FiscalInfoSlip;
                FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, form);
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo, form);
                PrintReceipt(entry, formType, formInfo, posTransaction, false);
            }
            else
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "Unable to print fiscal info slip due to missing form for Fiscal info slip.");
                Interfaces.Services.DialogService(entry).ShowMessage(Resources.UnableToPrintFiscalInfoSlip, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
            }
        }

        public virtual void PrintSafeDropReversal(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            FormSystemType formType = FormSystemType.SafeDropReversal;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreateSaveDropReversalForm(entry, posTransaction, settings);
            }
            else
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            }

            PrintReceipt(entry, formType, formInfo, posTransaction, false);
        }

        public virtual void PrintSafeDrop(IConnectionManager entry, IPosTransaction posTransaction)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            FormSystemType formType = FormSystemType.SafeDrop;

            FormInfo formInfo = formModulation.GetInfoForForm(formType, false, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreatedCodedSafeDropForm(entry, posTransaction, settings);
            }
            else
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            }


            if (formInfo != null)
            {
                PrintReceipt(entry, formType, formInfo, posTransaction, false);
            }

        }

        #region Formatting copied from ReportLogic

        protected virtual string GetCorrectStringSize(string tempString, int size, PadDirection padDir)
        {
            if (tempString.Length > size)
            {
                if (padDir == PadDirection.Left)
                    tempString = tempString.Substring(tempString.Length - size, size);
                else
                    tempString = tempString.Substring(0, size);
            }
            return tempString;
        }

        protected virtual string FormatString(String tempString, int padSize, PadDirection padDir)
        {
            tempString = GetCorrectStringSize(tempString, padSize, padDir);
            if (padDir == PadDirection.Right)
                return tempString.PadRight(padSize);
            return tempString.PadLeft(padSize);
        }

        protected virtual string FormatString(String tempString, int padSize)
        {
            return FormatString(tempString, padSize, PadDirection.Right);
        }

        protected virtual string FormatStringLEFT(String tempString, int padSize)
        {
            return FormatString(tempString, padSize, PadDirection.Left);
        }

        #endregion

        public virtual void PrintGiftCertificate(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, IGiftCertificateItem giftCertificateItem, bool copyReceipt)
        {
            if (giftCertificateItem == null)
            {
                return;
            }

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt, false, false);

            if (formInfo == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                formInfo = CreateCodedGiftCertificateForm(entry, posTransaction, settings, giftCertificateItem, copyReceipt);
                PrintReceipt(entry, formType, formInfo, posTransaction, copyReceipt);
                return;
            }

            formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
            
            GiftCertificateTenderLineItem tenderLineItem = new GiftCertificateTenderLineItem();
            tenderLineItem.SerialNumber = giftCertificateItem.SerialNumber;
            tenderLineItem.Amount = giftCertificateItem.Amount;

            PrintTenderReceipt(entry, formType, posTransaction, tenderLineItem, copyReceipt);
            
        }

        public virtual void PrintGiftReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction transaction)
        {

            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if (settings.HardwareProfile.Printer == HardwareProfile.PrinterHardwareTypes.None)
                {
                    return;
                }

                if (!(transaction is RetailTransaction))
                {
                    return;
                }

                if (transaction.EntryStatus != TransactionStatus.Normal && transaction.EntryStatus != TransactionStatus.Training)
                {
                    return;
                }


                PrintReceipt(entry, formType, transaction, false);
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, x.Message, x);
                throw x;
            }
        }

        public virtual void PrintSuspendedTransaction(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt)
        {
            FormSystemType formTypePrefix = FormSystemType.SuspendedTransactionPrefix;

            FormInfo formInfoHeader = formModulation.GetInfoForForm(formTypePrefix, copyReceipt, false, false);
            FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt, false, false);

            if (!ShouldWePrint(entry, formInfo, formType, copyReceipt))
            {
                return;
            }

            StringBuilder reportLayout = new StringBuilder();

            // See if we have an override for the prefix
            if (formInfoHeader == null)
            {
                formModulation.BarcodePrintInfoList.Clear();
                reportLayout = CreateCodedSuspendTransactionPrintPrefix(entry, (RetailTransaction) posTransaction);
            }
            else
            {
                if (ShouldWePrint(entry, formInfoHeader, formTypePrefix, copyReceipt))
                {
                    formModulation.GetTransformedTransaction(entry, formTypePrefix, posTransaction, formInfoHeader);
                    formInfoHeader.Header = reportLayout + formInfoHeader.Header;

                    formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
                    formInfo.Header = formInfoHeader.Header + formInfoHeader.Details + formInfoHeader.Footer + formInfo.Header;

                    PrintReceipt(entry, formType, formInfoHeader, posTransaction, false);
                }
            }

            if (formInfo != null)
            {
                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);
                formInfo.Header = reportLayout + formInfo.Header;
                PrintReceipt(entry, formType, formInfo, posTransaction, false);
            }
            else
            {
                formModulation.BarcodePrintInfoList.Clear();
                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                formInfo = CreateFormInfo(entry, settings);
                formInfo.Header = reportLayout.ToString();

                PrintReceipt(entry, formType, formInfo, posTransaction, false);
            }
        }

        public virtual void PrintVoidedTransaction(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction,
            bool copyReceipt)
        {
            PrintReceipt(entry, formType, posTransaction, copyReceipt);
        }

        public virtual string Validate(IConnectionManager entry, string validation)
        {
            return HMAC_SHA1.GetValue(validation, key);
        }

        public virtual bool PrintTransaction(IConnectionManager entry, IPosTransaction transaction, bool copyReceipt, bool printPreview)
        {
            return printing.PrintTransaction(entry, transaction, copyReceipt, printPreview);
        }

        protected virtual bool PrintEFTReceipt(IConnectionManager entry, IEFTInfo eftInfo, FormSystemType formType)
        {

            if (PrintPartnerReceipt(entry, eftInfo, formType))
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "Receipt from EFT device printed", "PrintingService.cs");
                return false;
            }

            if (eftInfo != null && eftInfo.ReceiptPrinting != EFTReceiptPrinting.All)
            {
                bool print = true;

                // Check that we should print this receipt
                if (formType == FormSystemType.CardReceiptForCust ||
                    formType == FormSystemType.CardReceiptForCustReturn)
                {
                    print = eftInfo.ShouldPrint(EFTReceiptPrinting.CardHoldersReceipt);
                }
                else if (formType == FormSystemType.CardReceiptForShop ||
                    formType == FormSystemType.CardReceiptForShopReturn)
                {
                    print = eftInfo.ShouldPrint(EFTReceiptPrinting.StoreReceipt);
                }

                if (!print)
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual FormType GetFormTypeID(IConnectionManager entry, FormSystemType formType)
        {
            if (formTypes.Count == 0)
            {
                formTypes = Providers.FormTypeData.GetFormTypes(entry, FormTypeSorting.Type, false);
            }

            FormType type = formTypes.FirstOrDefault(f => f.SystemType == (int)formType);
            if (type != null)
            {
                return type;
            }

            return new FormType();
        }

        public virtual void PrintOpenDrawer(IConnectionManager entry, IPosTransaction transaction)
        {
            PrintReceipt(entry, FormSystemType.OpenDrawer, transaction, false);
        }
    }
}
