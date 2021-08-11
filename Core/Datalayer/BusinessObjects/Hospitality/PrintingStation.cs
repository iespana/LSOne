using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class PrintingStation : DataEntity
    {
        public PrintingStation()
        {
            Text = "";
            WindowsPrinter = "";
            OutputLines = OutputLinesEnum.All;
            CheckPrinting = CheckPrintingEnum.No;
            POSExternalPrinterID = "";
            Printing = PrintingEnum.AllStations;
            StationFilter = "";
            StationCheckMinutes = 0;
            CompressBOMReceipt = CompressBOMReceiptEnum.No;
            ExcludeFromCompression = "";
            StationType = StationTypeEnum.WindowsPrinter;
            PrintingPriority = 0;
            EndTurnsRedAfterMin = 0;
            StationPrintingHostID = RecordIdentifier.Empty;
            PrinterDeviceName = "";
            WindowsPrinterConfigurationID = RecordIdentifier.Empty;

            OPOSFontSizeV = 1;
            OPOSFontSizeH = 2;
            OPOSMaxChars = 28;
        }
        
        public string WindowsPrinter { get; set; }
        public OutputLinesEnum OutputLines { get; set; }
        public CheckPrintingEnum CheckPrinting { get; set; }
        public RecordIdentifier POSExternalPrinterID { get; set; }
        public PrintingEnum Printing { get; set; }
        public string StationFilter { get; set; }
        public int StationCheckMinutes { get; set; }
        public CompressBOMReceiptEnum CompressBOMReceipt { get; set; }
        public string ExcludeFromCompression { get; set; }
        public StationTypeEnum StationType { get; set; }
        public int PrintingPriority { get; set; }
        public int EndTurnsRedAfterMin { get; set; }
        public RecordIdentifier StationPrintingHostID { get; set; }
        public string StationPrintingHostAddress { get; set; }
        public int StationPrintingHostPort { get; set; }

        public string PrinterDeviceName { get; set; }

        /// <summary>
        /// ID of the windows printer configuration
        /// </summary>
        public RecordIdentifier WindowsPrinterConfigurationID { get; set; }

        /// <summary>
        /// The vertical font size for the OPOS printer. Can be either 1 or 2
        /// </summary>
        public int OPOSFontSizeV { get; set; }

        /// <summary>
        /// The horizontal font size for the OPOS printer. Can be either 1 or 2
        /// </summary>
        public int OPOSFontSizeH { get; set; }

        /// <summary>
        /// The number of characters the receipt can have horizontally. For OPOSFontSize 1 it can be 55, for OPOSFontSize 2 it should be around 28
        /// </summary>
        public int OPOSMaxChars { get; set; }

        #region Enums
        public enum OutputLinesEnum
        {
            All = 0,
            OnlyNewLines = 1
        }

        public enum CheckPrintingEnum
        {
            No = 0,
            OCXSpoolerCheck = 1
        }

        public enum PrintingEnum
        {
            ThisStation = 0,
            AllStations = 1,
            SelectedStations = 2
        }

        public enum CompressBOMReceiptEnum
        {
            Yes = 0,
            No = 1
        }

        public enum StationTypeEnum
        {
            WindowsPrinter = 0,            
            OPOSPrinter = 1,
            HardwareProfilePrinter = 2
        }
        #endregion
    }
}
