using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.OPOS;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSRetail.NAV.Opos;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSRetail.PrintingStationClient;
using LSRetailPosis;
using POS.Devices;
using LSOne.Services.Interfaces;
using LSOne.Controls.SupportClasses;
using LSOne.Utilities.DataTypes;

namespace LSOne.Peripherals
{
    static public partial class Printer
    {
        internal static string StoreLogoFile;
        internal static int StoreLogoWidth = 500;
        static internal OPOSPrinter posPrinter;
        static private bool oposDeviceActive = false;
        static StreamReader streamToPrint;
        internal static PrintingStationCli printingStationClient;
        static private WindowsPrinterConfiguration windowsPrinterConfiguration;

        public static bool IsLoaded => oposDeviceActive && posPrinter != null;

        static public void Load(IConnectionManager entry)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {
                switch (settings.HardwareProfile.Printer)
                {
                    case HardwareProfile.PrinterHardwareTypes.OPOS:
                        {
                            oposDeviceActive = true;
                            posPrinter = new OPOSPrinter(settings.HardwareProfile.PrinterDeviceName, settings.HardwareProfile.PrinterCharacterSet);
                            posPrinter.PrinterMessageEvent += ShowPrinterMessage;
                            posPrinter.Initialize();

                            LoadLogos(settings, entry);

                            if (File.Exists(StoreLogoFile))
                            {
                                posPrinter.SetBitmap(StoreLogoFile, StoreLogoWidth);
                            }
                        }
                        break;
                    case HardwareProfile.PrinterHardwareTypes.PrintingStation:
                        StationPrintingHost stationPrintingHost = Providers.StationPrintingHostData.Get(entry, settings.HardwareProfile.StationPrintingHostID);
                        printingStationClient = new PrintingStationCli(stationPrintingHost.Address, stationPrintingHost.Port);
                        break;
                    case HardwareProfile.PrinterHardwareTypes.Windows:
                    case HardwareProfile.PrinterHardwareTypes.None:
                        LoadLogos(settings, entry);
                        break;
                }
            }
            catch (Exception x)
            {
                posPrinter = null;
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                throw;
            }
        }

        /// <summary>
        /// Loads the logo of a store into the printer
        /// </summary>
        /// <param name="settings">Current settings</param>
        /// <param name="entry">Database connection</param>
        /// <param name="forceReload">Forces the logo to be loaded again even if it was already loaded</param>
        static public void LoadLogos(ISettings settings, IConnectionManager entry, bool forceReload = false)
        {
            FolderItem f = FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData).Child("LS Retail").Child("LS POS");
            string oldLogoLocation = f.AbsolutePath + "\\LSPOSNETLOGO.bmp";

            //If the POS is still using the original LSPOSNETLOGO file then move it to Program Data folder
            if (File.Exists(GetAppPath() + @"LSPOSNETLOGO.bmp"))
            {
                File.Move(GetAppPath() + @"LSPOSNETLOGO.bmp", oldLogoLocation);
                StoreLogoFile = oldLogoLocation;
            }

            // Loading a bitmap for the printer
            if (forceReload || string.IsNullOrEmpty(StoreLogoFile))
            {
                // Save the logo to a file
                try
                {
                    string filePath = f.AbsolutePath + "\\LSONESTORELOGO1.bmp";
                    
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    if (!RecordIdentifier.IsEmptyOrNull(settings.Store.PictureID))
                    {
                        Providers.ImageData.Get(entry, settings.Store.PictureID).Picture.Save(filePath, ImageFormat.Bmp);

                        StoreLogoFile = filePath;
                        StoreLogoWidth = settings.Store.LogoSize == StoreLogoSizeType.Normal
                                            ? 250
                                            : 500;
                    }
                }
                catch { }
            }
        }

        static public void Unload(IConnectionManager entry)
        {
            if (!oposDeviceActive || posPrinter == null) { return; }

            try
            {
                posPrinter.Finalise();
                oposDeviceActive = false;
            }
            catch (Exception x)
            {
                posPrinter = null;
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                throw;
            }
        }

        static public void LoadCustomLogo(int id, string logofile, int width)
        {
            if (posPrinter != null && File.Exists(logofile))
                posPrinter.SetBitmap(id, logofile, width);
        }

        static public int PrintReceipt(IConnectionManager entry, string text, List<BarcodePrintInfo> barcodePrintInfo, int formWidth = 56)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {

                switch (settings.HardwareProfile.Printer)
                {
                    case HardwareProfile.PrinterHardwareTypes.OPOS:
                        if ((!oposDeviceActive) || (posPrinter == null))
                        {
                            return 0;
                        }
                        text = OPOSConstants.CleanAlignments(text);
                        return posPrinter.PrintReceipt(text, barcodePrintInfo);

                    case HardwareProfile.PrinterHardwareTypes.Windows:
                        WindowsPrinting(entry, text, settings.HardwareProfile.WindowsPrinterConfiguration);
                        break;

                    case HardwareProfile.PrinterHardwareTypes.PrintingStation:
                        PrintToStationPrinter(entry, text, barcodePrintInfo, formWidth);
                        break;
                }

                return 0;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                throw;
            }
        }

        private static void PrintToStationPrinter(IConnectionManager entry, string text, List<BarcodePrintInfo> barcodePrintInfo, int formWidth)
        {
            bool printJobSuccessful = false;
            bool retryingPrintJob = false;

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            
            string receiptString = OPOSConstants.CleanAlignments(text);            

            if(barcodePrintInfo == null)
            {
                barcodePrintInfo = new List<BarcodePrintInfo>();
            }
            
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

            string textToPrint = receiptString;
            if (settings.HardwareProfile.PrintBinaryConversion)
            {
                textToPrint = OPOSCommon.ConvertToBCD(textToPrint + "\r\n\r\n\r\n", settings.HardwareProfile.PrinterCharacterSet);
                //0 = OPOS_BC_NONE
                //1 = OPOS_BC_NIBBLE
                //2 = OPOS_BC_DECIMAL                
            }

            while (!printJobSuccessful)
            {

                bool printingStationJobStarted = false;

                while (!printingStationJobStarted)
                {
                    printingStationJobStarted = printingStationClient.StartPrintJob(settings.HardwareProfile.PrinterDeviceName,
                        (int) OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                        (int) OPOSPOSPrinterConstants.PTR_TP_TRANSACTION);

                    if (!printingStationJobStarted)
                    {
                        string message = Properties.Resources.CouldNotStartPrintJob + "\r\n\r\n" + printingStationClient.GetLastPrintMessage();

                        DialogResult dlgResult = Services.Interfaces.Services.DialogService(entry)
                            .ShowMessage(message, Properties.Resources.PrintingStationError, MessageBoxButtons.RetryCancel);

                        if (dlgResult == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }

                

                if (retryingPrintJob)
                {
                    printingStationClient.CutPaper(settings.HardwareProfile.PrinterDeviceName, 100);
                }

                printingStationClient.PrintNormal(
                    settings.HardwareProfile.PrinterDeviceName,
                    (int) OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                    settings.HardwareProfile.PrintBinaryConversion ? 2 : 0,
                    textToPrint);

                IBarcodeService barcodeService = Services.Interfaces.Services.BarcodeService(entry);

                foreach (BarcodePrintInfo info in barcodePrintInfo)
                {
                    string barcodeData = barcodeService.GetReceiptBarCodeData(entry, new BarcodeReceiptParseInfo ((string) entry.CurrentStoreID, (string) entry.CurrentTerminalID, info.BarcodeToPrint));

                    string barcodeMessage = null;
                    int barCodeType = 0;
                    Size size = Size.Empty;

                    barcodeService.ManageReceiptBarcode(settings.Store.BarcodeSymbology, ref barCodeType, ref barcodeData, ref barcodeMessage, ref size);

                    if (barcodeMessage == null)
                    {
                        printingStationClient.PrintBarcode(
                            settings.HardwareProfile.PrinterDeviceName,
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
                        printingStationClient.PrintNormal(
                            settings.HardwareProfile.PrinterDeviceName,
                            (int) OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                            settings.HardwareProfile.PrintBinaryConversion ? 2 : 0,
                            barcodeMessage);
                    }

                    //line feed add a number between '|' and 'l' to set how many line feeds.
                    //printResult = printResult && printingStationClient.StationPrintEx2(settings.HardwareProfile.PrinterDeviceName, $"{(char) 27}|{1}lF", false);
                    printingStationClient.PrintNormal(
                        settings.HardwareProfile.PrinterDeviceName,
                        (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                        settings.HardwareProfile.PrintBinaryConversion ? 2 : 0,
                        $"{(char) 27}|{1}lF");

                    info.Printed = true;
                }

                if (settings.HardwareProfile.PrinterExtraLines > 0)
                {                    
                    printingStationClient.PrintNormal(
                        settings.HardwareProfile.PrinterDeviceName,
                        (int)OPOSPOSPrinterConstants.PTR_S_RECEIPT,
                        settings.HardwareProfile.PrintBinaryConversion ? 2 : 0,
                        $"{(char) 27}|{settings.HardwareProfile.PrinterExtraLines}lF");
                }

                printJobSuccessful = printingStationClient.EndPrintJob(settings.HardwareProfile.PrinterDeviceName, true);

                // Check if any errors occurred while printing
                if (!printJobSuccessful)
                {
                    string lastMessage = printingStationClient.GetLastPrintMessage();

                    // Check weter the message contains a valid OPOS error code that we can parse
                    OPOS_Constants resultCode;
                    OPOSPOSPrinterConstants resultCodeExtended;
                    DialogResult result;

                    int codeStartIndex = lastMessage.IndexOf("Code:") + 5;
                    int codeLength = lastMessage.IndexOf("Ext:") - codeStartIndex - 1;

                    int extStartIndex = lastMessage.IndexOf("Ext:") + 4;
                    int extLength = lastMessage.IndexOf("Msg:") - extStartIndex - 1;

                    resultCode = (OPOS_Constants) Convert.ToInt32(lastMessage.Substring(codeStartIndex, codeLength));
                    resultCodeExtended = (OPOSPOSPrinterConstants) Convert.ToInt32(lastMessage.Substring(extStartIndex, extLength));

                    if (resultCode == OPOS_Constants.OPOS_E_NOHARDWARE)
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.NoPrinterFound, Properties.Resources.PrintingStation, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                    }
                    else if (resultCode == OPOS_Constants.OPOS_E_DISABLED)
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrintingDeviceIsDisabled, Properties.Resources.PrintingStation, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                    }
                    else if (resultCode == OPOS_Constants.OPOS_E_EXTENDED)
                    {
                        OPOSPOSPrinterConstants errorCode = resultCodeExtended;

                        if (errorCode == OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                        {
                            result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrinterIsOpen, Properties.Resources.PrintingStation, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                        else if (errorCode == OPOSPOSPrinterConstants.OPOS_EPTR_REC_EMPTY)
                        {
                            result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PrinterIsOutOfPaper, Properties.Resources.PrintingStation, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        }
                        else
                        {
                            result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage((resultCodeExtended).ToString(), Properties.Resources.PrintingStation, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                        }
                    }
                    else if (resultCode == OPOS_Constants.OPOS_E_FAILURE)
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.CheckForPaperJam, Properties.Resources.PrintingStation, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                    }

                    else
                    {
                        result = Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.PleaseCheckPrinterOrPrintingStation, Properties.Resources.PrintingStation, MessageBoxButtons.RetryCancel, MessageDialogType.ErrorWarning);
                    }

                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    retryingPrintJob = true;
                }
            }
        }

        static public int PrintNormal(IConnectionManager entry, string text)
        {
            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            try
            {
                switch (settings.HardwareProfile.Printer)
                {
                    case HardwareProfile.PrinterHardwareTypes.OPOS:
                        if ((!oposDeviceActive) || (posPrinter == null))
                        {
                            return 0;
                        }
                        return posPrinter.PrintNormal(text);

                    case HardwareProfile.PrinterHardwareTypes.Windows:
                        //WindowsPrinting(entry,text, settings.HardwareProfile.PrinterDeviceName);
                        break;
                }
                return 0;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                throw;
            }
        }

        static private string GetAppPath()
        {
            return Application.StartupPath + '\\';
        }

        // The PrintPage event is raised for each page to be printed.
        private static void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage;
            float yPos;
            float leftMargin = windowsPrinterConfiguration.LeftMargin;
            float topMargin = windowsPrinterConfiguration.TopMargin;
            string line = null;

            Console.WriteLine("printed");
            Graphics g = ev.Graphics;

            g.Clear(Color.White);

            using (Font printFont = new Font(windowsPrinterConfiguration.FontName, (float)windowsPrinterConfiguration.FontSize, FontStyle.Bold))
            {
                // Calculate the number of lines per page.
                linesPerPage = ev.MarginBounds.Height/printFont.GetHeight(g);

                // Print each line of the file.
                int extra = 0;
                for (int count = 0; count < linesPerPage && ((line = streamToPrint.ReadLine()) != null); count++)
                {
                    yPos = extra + topMargin + (count*printFont.GetHeight(g));

                    if (line.IndexOf("<B:") >= 0)
                    {
                        // Locate the receiptId
                        int indxB = line.IndexOf("<B:");
                        string restOfString = line.Substring(indxB + 4, line.Length - indxB - 4);
                        var receiptId = restOfString.Substring(0, restOfString.IndexOf(">"));

                        extra += PrintBarcode(g, receiptId, leftMargin, yPos);
                    }
                    else
                    {
                        g.DrawString(line, printFont, Brushes.Black, leftMargin, yPos);
                    }
                }
            }
            ev.HasMorePages = line != null;
        }

        private static int Count(string text, string textToBeCounted)
        {
            string[] split = text.Split(new string[] { textToBeCounted }, StringSplitOptions.None);
            if (split.Length != 0)
                return (split.Length - 1);
            return 0;
        }

        static public void PrintSlip(IConnectionManager entry, string textToPrint)
        {
            PrintSlip(entry, textToPrint, "", "");
        }

        static public void PrintSlip(IConnectionManager entry, string header, string details, string footer)
        {
            if (!oposDeviceActive) { return; }

            try
            {
                posPrinter.PrintSlip(header, details, footer);
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                throw;
            }
        }

        private static void ShowPrinterMessage(string message)
        {
            try
            {
                // We need to check for null here since accessing the one from the static might not always be valid, expesially not on server side.
                if (DLLEntry.DataModel != null)
                {
                    ISettings settings = (ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                    ((Control) settings.POSApp.POSMainWindow).Invoke(ApplicationFramework.PosShowStatusBarInfoDelegate, new object[] {message, null, TaskbarSection.Message});
                }
            }
            catch (Exception x)
            {
                // We need to check for null here since accessing the one from the static might not always be valid, expesially not on server side.
                if (DLLEntry.DataModel != null)
                {
                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Printer", x);
                }
                throw;
            }
        }

        public static void WinPrinterPrinting(IConnectionManager entry, WindowsPrinterConfiguration printerConfiguration, List<FormLine> formLines, FormInfo formInfo, string documentName, bool printToFile = false)
        {
            if (formLines.Count == 0)
            {
                return;
            }

            if(printerConfiguration == null)
            {
                return;
            }

            ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            int station = 2; //the receipt part of the printer

            WinPrinter printer = new WinPrinter("LS One printer");
            printer.Initialize("Copyright: LS Retail ehf.");
            printer.DeviceName = formInfo.WindowsPrinterName;
            printer.DocumentName = documentName;
            printer.OpenDevice();

            bool logoExists = false;
            LogoSize logoSize = LogoSize.Normal;

            printer.StartPrintJob(station, 0);
            printer.RecLineChars = formInfo.FormWidth;
            printer.PrintDesignRectangles = printerConfiguration.PrintDesignBoxes;
            printer.PrintDesignText = false;
            printer.PrintLeftMarginMM = printerConfiguration.LeftMargin;
            printer.PrintRightMarginMM = printerConfiguration.RightMargin;
            printer.PrintBottomMarginMM = printerConfiguration.BottomMargin;
            printer.PrintTopMarginMM = printerConfiguration.TopMargin;
            printer.PaperWidthMM = (printer.RecLineChars*2) + printer.PrintLeftMarginMM + printer.PrintRightMarginMM;

            Font regularFont = new Font(printerConfiguration.FontName, (float)printerConfiguration.FontSize, FontStyle.Regular, GraphicsUnit.Millimeter);
            Font wideHighFont = new Font(printerConfiguration.FontName, (float)printerConfiguration.WideHighFontSize, FontStyle.Regular, GraphicsUnit.Millimeter);
            Font designFont = new Font("Courier New", 3.0f, FontStyle.Regular, GraphicsUnit.Millimeter); //This is not applicable to LS One
            printer.SetReceiptFonts(regularFont, wideHighFont, designFont);

            if (File.Exists(StoreLogoFile))
            {
                printer.SetBitmap(1, station, StoreLogoFile, LogoAlign.Left);
                logoSize = settings.Store.LogoSize == StoreLogoSizeType.Double ? LogoSize.DoubleWideAndHigh : LogoSize.Normal;
                logoExists = true;
            }

            IBarcodeService barcodeService = Services.Interfaces.Services.BarcodeService(entry);

            foreach (FormLine line in formLines)
            {
                if (logoExists && line.PrintLineType == FormLineTypeEnum.Logo)
                {
                    printer.LogoSize = logoSize;
                    printer.LogoAlign = LogoAlign.Left;
                    printer.PrintBitmap(station, 1);
                }

                if (line.PrintLineType == FormLineTypeEnum.Barcode)
                {
                    FormLineBarcode barcodeLine = line as FormLineBarcode;
                    printer.PrintNormal(station, "");
                    printer.PrintNormal(station, "");
                    
                    string barcodeMessage = "";
                    Size barcodeSize = Size.Empty;

                    if (!ManageReceiptBarcode(ref barcodeLine))
                    {
                        barcodeService.ManageReceiptBarcode(barcodeLine.TypeOfBarcode, ref barcodeLine.BarcodeSymbologyType, ref barcodeLine.Line, ref barcodeMessage, ref barcodeSize);
                    }

                    if (!string.IsNullOrEmpty(barcodeMessage))
                    {
                        printer.PrintNormal(station, barcodeMessage);
                    }
                    else
                    {
                        printer.PrintBarcode(station, barcodeLine.Line, barcodeLine.BarcodeSize.Width, barcodeLine.BarcodeSize.Height, barcodeLine.BarcodeSymbologyType, -13);
                    }
                    printer.PrintNormal(station, "");
                    printer.PrintNormal(station, "");
                }

                if (line.PrintLineType == FormLineTypeEnum.Text)
                {
                    if (line.TextAlignment == HorizontalAlign.Center)
                    {
                        printer.PrintNormal(station, line.Line, GetPrinterFontType(line.FontType), CreateAlignedDesignText(line, printer.RecLineChars));
                    }
                    else
                    {
                        printer.PrintNormal(station, line.Line, GetPrinterFontType(line.FontType));
                    }
                }
            }

            if (printToFile)
            {
                printer.EndPrintJob(true, printerConfiguration.FolderLocation, documentName, settings.HardwareProfile.FileType);
            }
            else
            {
                printer.EndPrintJob(true);
            }
            printer.CloseDevice();
        }

        private static string CreateAlignedDesignText(FormLine line, int numberOfCharacters)
        {
            int designLength = numberOfCharacters;
            if (line.TextAlignment == HorizontalAlign.Center)
            {
                if (line.FontType == FormLineFontTypeEnum.Wide || line.FontType == FormLineFontTypeEnum.WideHigh)
                {
                    designLength = designLength/2;
                }

                return "C" + new string('#', designLength-1);
            }

            return new string('#', designLength);
        }

        private static FontType GetPrinterFontType(FormLineFontTypeEnum fontType)
        {
            switch (fontType)
            {
                case FormLineFontTypeEnum.Normal:
                    return FontType.Normal;
                case FormLineFontTypeEnum.Bold:
                    return FontType.Bold;
                case FormLineFontTypeEnum.Wide:
                    return FontType.Wide;
                case FormLineFontTypeEnum.WideHigh:
                    return FontType.WideHigh;
                case FormLineFontTypeEnum.High:
                    return FontType.High;
                case FormLineFontTypeEnum.Italic:
                    return FontType.Italic;
                default:
                    return FontType.Normal;
            }
        }

        public static void WindowsPrinting(IConnectionManager entry, string textToPrint, WindowsPrinterConfiguration printerConfiguration)
        {
            if (printerConfiguration == null) return;

            //Assing to private variable to be used by the print page event
            windowsPrinterConfiguration = printerConfiguration;
            string tempReceiptFile = SaveReceiptToFile(textToPrint);

            try
            {
                streamToPrint = new StreamReader(tempReceiptFile);
                PrintDocument pd = new PrintDocument();
                
                pd.PrinterSettings.PrinterName = printerConfiguration.PrinterDeviceName;
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.BeginPrint += PdOnBeginPrint;
                pd.QueryPageSettings += PdOnQueryPageSettings;

                pd.Print();
            }
            catch (InvalidPrinterException)
            {
                Services.Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PrinterNotAccessible.Replace("#1", printerConfiguration.PrinterDeviceName), MessageBoxButtons.OK, MessageDialogType.Generic);
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.WindowsPrinting", x);
                throw;
            }
            finally
            {
                streamToPrint.Close();
                File.Delete(tempReceiptFile);
            }
        }

        private static void PdOnQueryPageSettings(object sender, QueryPageSettingsEventArgs queryPageSettingsEventArgs)
        {
        }

        private static void PdOnBeginPrint(object sender, PrintEventArgs printEventArgs)
        {
        }


        private static string SaveReceiptToFile(string textToPrint)
        {
            textToPrint = OPOSConstants.CleanOPOSFonts(textToPrint);

            FolderItem f = FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData).Child("LS Retail");
            f = f.Child("LS POS"); //should be there since there also the POS.exe.config is written to
            string filePath = f.AbsolutePath + "\\TmpReceipt.txt";

            var fs = new FileStream(filePath, FileMode.Create);
            using (var sw = new StreamWriter(fs))
                sw.WriteLine(textToPrint);

            return filePath;
        }

        public static void PortPrinting(string textToPrint, string printerName)
        {
            Port port = new Port();
            port.WriteToPort(textToPrint, printerName);
        }
    }
}
