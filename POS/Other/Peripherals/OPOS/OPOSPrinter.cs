using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public partial class OPOSPrinter : OPOSBase
    {
        #region Member variables
        private static bool wincorBmpAvailable;
        private static string wincorLogoFile;
        private OPOSPOSPrinterClass posPrinter; // An instance of the PosPrinter class.  The printer itself.
        private OPOSPOSPrinterConstants currentPrinterStation; // Receipt, Slip, Journal, etc...
        private string printerDeviceName; // The device name, e.g. TM-H6000II

        private int characterSet; // The characterset used for printing
        private bool asyncPrintingMode; // The printing mode, synchronous or asynchronous.

        private int linesLeftOnCurrentPage = 0;
        private int totalNumberOfLines = 40;
        private string[] headerLinesx;
        private string[] itemLinesx;
        private string[] footerLinesx;

        public delegate void printerMessageDelegate(string message);

        public event printerMessageDelegate PrinterMessageEvent;

        #endregion

        #region Constructor

        static OPOSPrinter()
        {
            wincorLogoFile = Path.Combine(Application.StartupPath, "Wincor_LOGO.bmp");
            wincorBmpAvailable = File.Exists(wincorLogoFile);
            if (wincorBmpAvailable) 
                return;

            wincorLogoFile = Path.Combine(Application.StartupPath, "LOGO.bmp");
            wincorBmpAvailable = File.Exists(wincorLogoFile);
            
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="printerDeviceName">Denotes the printer device name, e.g. "TM-H6000II".</param> 
        /// <param name="characterSet">The character set used for printing</param>
        public OPOSPrinter(string printerDeviceName, int characterSet)
        {
            // Get all text through the Translation function in the Printer class
            //
            // TextID's for OPOS.Printer are reserved at 14000 - 14019
            // In use now are ID's  
            this.printerDeviceName = printerDeviceName;
            this.characterSet = characterSet;
            this.asyncPrintingMode = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// To initialize the printer device
        /// </summary>
        public override void Initialize()
        {
            Open();
            Claim();
            Enable();
            Configure();
            PrintReceipt("\r", null);
            
        }

        public bool Test(bool breakOnError)
        {
            Open();
            Claim();
            Enable();
            Configure();
            int printResult =  PrintReceipt("", null, breakOnError);
            if (printResult != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clean-up of all the printer's resources.
        /// </summary>
        public override void Finalise()
        {
            if (posPrinter != null)
            {
                posPrinter.ReleaseDevice();
                posPrinter.Close();
            }
            CloseExistingMessageWindow();
        }

        public void SetBitmap(string fileName, int width)
        {
            posPrinter.SetBitmap(1, (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT, fileName, width, (int)OPOSPOSPrinterConstants.PTR_BM_CENTER);
        }

        public void SetBitmap(int id, string fileName, int width)
        {
            if (id < 2)
                return;     // dont overload default id

            posPrinter.SetBitmap(id, (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT, fileName, width, (int)OPOSPOSPrinterConstants.PTR_BM_CENTER);
        }

        public void SetLogo(int location, string imageData)
        {
            posPrinter.SetLogo(location, imageData);
        }

        public void ClearBitmap()
        {
            posPrinter.SetBitmap(1, (int) OPOSPOSPrinterConstants.PTR_S_RECEIPT, @"", 0, (int) OPOSPOSPrinterConstants.PTR_BM_CENTER);
        }

        public int PrintNormal(string textToPrint)
        {
            if (textToPrint.Contains("<B:"))
            {
                // Locate the receiptId
                int indxB = textToPrint.IndexOf("<B:", StringComparison.InvariantCultureIgnoreCase);
                string restOfString = textToPrint.Substring(indxB + 4, textToPrint.Length - indxB - 4);
                string receiptId = restOfString.Substring(0, restOfString.IndexOf(">", StringComparison.InvariantCultureIgnoreCase));

                string barcodeData = Services.Interfaces.Services.BarcodeService(DLLEntry.DataModel).GetReceiptBarCodeData(DLLEntry.DataModel, new BarcodeReceiptParseInfo((string)DLLEntry.DataModel.CurrentStoreID, (string)DLLEntry.DataModel.CurrentTerminalID, receiptId));
                PrintBarCode(barcodeData);
                return 0;
            }

            if (textToPrint.Contains("<L"))
            {
                // look for logo to print
                int indxL = textToPrint.IndexOf("<L", StringComparison.InvariantCultureIgnoreCase);
                if (textToPrint[indxL + 2] == '>')
                {
                    textToPrint = textToPrint.Replace("<L>", "\x1B|1B\x1B|bC");
                }
                else
                {
                    int indxLend = textToPrint.IndexOf(">", indxL, StringComparison.InvariantCultureIgnoreCase);
                    string Lvalue = textToPrint.Substring(indxL, indxLend - indxL + 1);
                    int id = int.Parse(Lvalue.Substring(2, indxLend - 2));
                    textToPrint = textToPrint.Replace(Lvalue, string.Format("\x1B|{0}B\x1B|bC", id));
                }
                return posPrinter.PrintNormal((int)currentPrinterStation, textToPrint);
            }

            return posPrinter.PrintNormal((int)currentPrinterStation, textToPrint);
        }

        /// <summary>
        /// Prints a receipt containing the text in the textToPrint parameter
        /// </summary>
        /// <param name="textToPrint"> The text to print on the receipt</param>
        /// <param name="barcodePrintInfo">A list of barcodes to be printed</param>
        /// <param name="breakOnError"></param>
        public int PrintReceipt(string textToPrint, List<BarcodePrintInfo> barcodePrintInfo, bool breakOnError = false)
        {
            if (barcodePrintInfo == null)
            {
                barcodePrintInfo = new List<BarcodePrintInfo>();
            }

            if (barcodePrintInfo.Count > 0)
            {
                if (textToPrint.Contains(barcodePrintInfo[0].BarcodeMarker))
                {
                    foreach (BarcodePrintInfo info in barcodePrintInfo)
                    {
                        //Locate the barcode to be printed
                        int indxB = textToPrint.IndexOf(info.BarcodeMarker.Trim(),
                            StringComparison.InvariantCultureIgnoreCase);

                        //Delete 
                        string restOfString = textToPrint.Substring(indxB + info.BarcodeMarkerLength,
                            textToPrint.Length - indxB - (info.BarcodeMarkerLength));
                        string toPrint = restOfString.Substring(0,
                            restOfString.IndexOf(BarcodePrintMarkers.BarcodeEndMarker,
                                StringComparison.InvariantCultureIgnoreCase));

                        // Delete the barcode marker from the printed string
                        textToPrint = textToPrint.Remove(indxB, toPrint.Length + info.BarcodeMarkerLength + 1);
                    }
                }
            }

            currentPrinterStation = OPOSPOSPrinterConstants.PTR_S_RECEIPT;

            // Special handling for Wincor embedded printers, which don't support storing bitmaps in printer memory
            // GillCapital (H&M) in Indonesia and Thailand
            if (wincorBmpAvailable && textToPrint.Contains("<L>"))
            {
                posPrinter.PrintBitmap((int) currentPrinterStation, wincorLogoFile, (int) OPOSPOSPrinterConstants.PTR_BM_ASIS, (int) OPOSPOSPrinterConstants.PTR_BM_CENTER);
            }

            if (textToPrint.Contains("<L"))
            {
                // look for logo to print
                int indxL = textToPrint.IndexOf("<L", StringComparison.InvariantCultureIgnoreCase);
                if (textToPrint[indxL + 2] == '>')
                {
                    textToPrint = textToPrint.Replace("<L>", "\x1B|1B\x1B|bC");
                }
                else
                {
                    int indxLend = textToPrint.IndexOf(">", indxL, StringComparison.InvariantCultureIgnoreCase);
                    string Lvalue = textToPrint.Substring(indxL, indxLend - indxL + 1);
                    int id = int.Parse(Lvalue.Substring(2, indxLend - 2));
                    textToPrint = textToPrint.Replace(Lvalue, string.Format("\x1B|{0}B\x1B|bC", id));
                }
            }
            string convertedTextToPrint;

            if (DLLEntry.Settings.HardwareProfile.PrintBinaryConversion)
            {
                convertedTextToPrint = OPOSCommon.ConvertToBCD(textToPrint + "\r\n\r\n\r\n", this.characterSet);
                //0 = OPOS_BC_NONE
                //1 = OPOS_BC_NIBBLE
                //2 = OPOS_BC_DECIMAL
                posPrinter.BinaryConversion = 2;
            }
            else
            {
                convertedTextToPrint = textToPrint;
            }

            CheckState();

            int printResult = posPrinter.PrintNormal((int) currentPrinterStation, convertedTextToPrint);
            while (printResult != 0)
            {
                DialogResult result;

                if ((OPOS_Constants) posPrinter.ResultCode == OPOS_Constants.OPOS_E_NOHARDWARE)
                {
                    result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.NoPrinterFound, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                }
                else if ((OPOS_Constants) posPrinter.ResultCode == OPOS_Constants.OPOS_E_DISABLED)
                {
                    result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrintingDeviceIsDisabled, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                }
                else if ((OPOS_Constants) posPrinter.ResultCode == OPOS_Constants.OPOS_E_EXTENDED)
                {
                    OPOSPOSPrinterConstants errorCode = (OPOSPOSPrinterConstants) posPrinter.ResultCodeExtended;

                    if (errorCode == OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrinterIsOpen, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else if (errorCode == OPOSPOSPrinterConstants.OPOS_EPTR_REC_EMPTY)
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrinterIsOutOfPaper, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }
                    else
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(((OPOSPOSPrinterConstants) posPrinter.ResultCodeExtended).ToString(), MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                    }
                }
                else if ((OPOS_Constants) posPrinter.ResultCode == OPOS_Constants.OPOS_E_FAILURE)
                {
                    result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CheckForPaperJam, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }

                else
                {
                    result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PleaseCheckThePrinter, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                }

                if (result == DialogResult.Cancel)
                {
                    return printResult;
                }

                posPrinter.ClearOutput();

                if (DLLEntry.Settings.HardwareProfile.PrintBinaryConversion)
                {
                    //0 = OPOS_BC_NONE
                    //1 = OPOS_BC_NIBBLE
                    //2 = OPOS_BC_DECIMAL
                    posPrinter.BinaryConversion = 2;
                }

                printResult = posPrinter.PrintNormal((int) currentPrinterStation, convertedTextToPrint);
                if (printResult != 0 && breakOnError)
                {
                    return printResult;
                }
            }

            //Reset the printer
            posPrinter.BinaryConversion = 0; //0 = OPOS_BC_NONE
            IBarcodeService barcodeService = Services.Interfaces.Services.BarcodeService(DLLEntry.DataModel);

            foreach (BarcodePrintInfo info in barcodePrintInfo.Where(w => !w.Printed))
            {
                string barcodeData = barcodeService.GetReceiptBarCodeData(DLLEntry.DataModel, new BarcodeReceiptParseInfo((string)DLLEntry.DataModel.CurrentStoreID, (string)DLLEntry.DataModel.CurrentTerminalID, info.BarcodeToPrint));
                PrintBarCode(barcodeData);
                info.Printed = true;
            }

            if (DLLEntry.Settings.HardwareProfile.PrinterExtraLines > 0)
            {
                posPrinter.PrintNormal((int) currentPrinterStation, string.Format("{0}|{1}lF", (char) 27, DLLEntry.Settings.HardwareProfile.PrinterExtraLines));
            }

            if (convertedTextToPrint.Length > 1)
            {
                posPrinter.CutPaper(100);
            }

            return printResult;
        }

        private void PrintBarCode(string barcodeData)
        {
            // Check if we should print the receipt id as a barcode on the receipt
            string barcodeMessage = null;
            int barCodeType = 0;
            Size size = Size.Empty;

            Services.Interfaces.Services.BarcodeService(DLLEntry.DataModel).ManageReceiptBarcode(DLLEntry.Settings.Store.BarcodeSymbology, ref barCodeType, ref barcodeData, ref barcodeMessage, ref size);
            
            int res = 0;

            if (barcodeMessage == null)
            {
                res = posPrinter.PrintBarCode((int) currentPrinterStation, barcodeData,
                    barCodeType, 80, posPrinter.RecLineWidth - 10,
                    (int) OPOSPOSPrinterConstants.PTR_BC_CENTER, (int) OPOSPOSPrinterConstants.PTR_BC_TEXT_BELOW);
            }
            else
            {
                posPrinter.PrintNormal((int) currentPrinterStation, barcodeMessage);
            }
#if DEBUG
            if (res != 0)
            {
                posPrinter.PrintNormal((int) currentPrinterStation, string.Format("Error {0} printing barcode of type {1}", res, barCodeType));
            }
#endif

            //line feed add a number between '|' and 'l' to set how many line feeds.
            posPrinter.PrintNormal((int)currentPrinterStation, new string(new[] { (char)27, '|', 'l', 'F' }));
        }

        /// <summary>
        /// Prints a slip containing the text in the textToPrint parameter
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <param name="footer"></param> 
        public void PrintSlip(string header, string details, string footer)
        {
            headerLinesx = GetStringArray(header);
            itemLinesx = GetStringArray(details);
            footerLinesx = GetStringArray(footer);

            currentPrinterStation = OPOSPOSPrinterConstants.PTR_S_SLIP;
            linesLeftOnCurrentPage = 0;
            posPrinter.BinaryConversion = 2;  // OposBcDecimal

            //useless if - statement. if binary conversion is NOT applied (that is, the field useBinaryConversion is set to FALSE), then the posPrinter.BinaryConversion
            //would be 0 leading to decimal numbers being printed out.
            //so I took out the check thus rolling back the previous changes (TOH, 04.10.2011)
            //if (LSRetailPosis.Settings.HardwareProfiles.Printer.BinaryConversion)
            //{
            //0 = OPOS_BC_NONE
            //1 = OPOS_BC_NIBBLE
            //2 = OPOS_BC_DECIMAL
            //}

            LoadNextSlipPaper(true);
            StartPrinting();
            posPrinter.BinaryConversion = 0; // OposBcNone
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            if (posPrinter == null)
            {
                posPrinter = new OPOSPOSPrinterClass();
                posPrinter.StatusUpdateEvent += posPrinter_StatusUpdateEvent;
                posPrinter.ErrorEvent += posPrinter_ErrorEvent;
                posPrinter.OutputCompleteEvent += posPrinter_OutputCompleteEvent;
            }
            else
            {
                posPrinter.Close();
            }
            int openresult = posPrinter.Open(printerDeviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            posPrinter.ClaimDevice(5000);
            CheckState();
            CheckResultCode();
        }


        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            if (posPrinter.State != (int) OPOS_Constants.OPOS_S_CLOSED)
            {
                posPrinter.DeviceEnabled = true;
            }
        }

        /// <summary>
        /// Configures the OPOS device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void Configure()
        {
            posPrinter.AsyncMode = asyncPrintingMode;
            posPrinter.CharacterSet = characterSet;
            posPrinter.RecLineChars = 56;
            posPrinter.SlpLineChars = 60;

            posPrinter.MapCharacterSet = DLLEntry.Settings.HardwareProfile.PrintBinaryConversion;

        }

        private void CloseExistingMessageWindow()
        {
            Services.Interfaces.Services.DialogService(DLLEntry.DataModel).CloseStatusDialog();
        }

        private void NewMessageWindow(string message)
        {
            CloseExistingMessageWindow();
            Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowStatusDialog(message, Properties.Resources.Printing, StatusDialogIcon.Message);
        }

        private string[] GetStringArray(string text)
        {
            string[] sep = { "\r\n" };
            if (text.EndsWith("\r\n"))
                text = text.Substring(0, text.Length - 2);
            return text.Split(sep, StringSplitOptions.None);
        }
        
        private void CheckState()
        {
            switch (posPrinter.State)
            {
                case (int)OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception(Properties.Resources.PrinterIsClosed);
                case (int)OPOS_Constants.OPOS_S_ERROR:
                    //MessageBox.Show("Villa: Villa");
                    break;               
                case (int)OPOS_Constants.OPOS_S_IDLE:
                    //MessageBox.Show("Idle");
                    break;
                case (int)OPOS_Constants.OPOS_S_BUSY:
                    break;
            }
        }

        private void CheckResultCode()
        {
            switch (posPrinter.ResultCode)
            {
                case (int)OPOS_Constants.OPOS_SUCCESS:
                    //throw new Exception("The printer is closed...");
                    break;
                case (int)OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.PrinterIsClosed);
                case (int)OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.PrinterIsClosed);
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.PrinterTimeout);
                case (int)OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.PrinterIsIllegallyClosed);
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                    if (posPrinter.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.PrinterIsClosed);
                    }

                    break;

                default:
                    throw new Exception("Unknown error code: " + posPrinter.ResultCode);
            }
        }

        private void StartPrinting()
        {
            try
            {
                PrintArray(itemLinesx);
                // if there is not space for the footer on the current page..
                if (linesLeftOnCurrentPage < footerLinesx.Length)
                {
                    //... then must be prompted for a new page
                    LoadNextSlipPaper(false);
                }
                PrintArray(footerLinesx);
                RemoveSlipPaper();
            }
            finally
            {
                CloseExistingMessageWindow();
            }
        }

        private void PrintArray(string[] array)
        {            
            foreach (string text in array)
            {
                if (linesLeftOnCurrentPage == 0)
                    LoadNextSlipPaper(false);

                posPrinter.PrintNormal((int)currentPrinterStation, OPOSCommon.ConvertToBCD(text + "\r\n", this.characterSet));
                linesLeftOnCurrentPage--;
            }
        }

        private void LoadNextSlipPaper(bool firstSlip)
        {
            if (firstSlip == false)
            {
                RemoveSlipPaper();
            }
            NewMessageWindow(Properties.Resources.InsertSlipIntoPrinter);
            posPrinter.BeginInsertion(60000);
            NewMessageWindow(Properties.Resources.PrintingInProgress);
            posPrinter.EndInsertion();
            linesLeftOnCurrentPage = totalNumberOfLines;
            PrintArray(headerLinesx);
        }

        private void RemoveSlipPaper()
        {
            NewMessageWindow(Properties.Resources.RemoveSlipFromPrinter); 
            posPrinter.BeginRemoval(10000);
            posPrinter.EndRemoval();
        }

        #endregion
        
        #region Events

        public void posPrinter_StatusUpdateEvent(int data)
        {
            switch (data)
            {

                case (int)OPOSPOSPrinterConstants.PTR_SUE_COVER_OPEN:
                    if (PrinterMessageEvent!= null)
                        PrinterMessageEvent(Properties.Resources.PrinterCoverIsOpen);
                    break;
                case (int)OPOSPOSPrinterConstants.PTR_SUE_COVER_OK:
                    if (PrinterMessageEvent != null)
                        PrinterMessageEvent(Properties.Resources.PrinterCoverIsOK);
                    break;
                case (int)OPOSPOSPrinterConstants.PTR_SUE_SLP_PAPEROK:
                    break;
            }
        }

        /// <summary>
        /// NOTE -- This event is only fired then the printer is running in sync. mode!!!!
        /// </summary>
        /// <param name="ResultCode"></param>
        /// <param name="ResultCodeExtended"></param>
        /// <param name="ErrorLocus"></param>
        /// <param name="pErrorResponse"></param>
        public void posPrinter_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
        {
            PrinterMessageEvent(Properties.Resources.PrinterFiredOffAnEvent);
        }
        
        public void posPrinter_OutputCompleteEvent(int outputId)
        {
            PrinterMessageEvent(Properties.Resources.PrinterCompletedOutput);
        }
        #endregion

    }
}
