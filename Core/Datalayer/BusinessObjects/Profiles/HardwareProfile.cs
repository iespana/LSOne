using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;
using System.Runtime.Serialization;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    [DataContract]
    public class HardwareProfile : DataEntity
    {

        public enum DisplayType
        {
            /// <summary>
            /// The application logo
            /// </summary>
            Logo = 0,

            /// <summary>
            /// Display an image rotator.
            /// </summary>
            ImageRotator = 1,

            /// <summary>
            /// Display a web browser 
            /// </summary>
            WebPage = 2,

            /// <summary>
            /// Display an image rotator syncronized by the store server.
            /// </summary>
            SyncronizedImageRotator = 3
        }

        public enum PrinterHardwareTypes
        {
            /// <summary>
            /// None = 0
            /// </summary>
            None = 0,

            /// <summary>
            /// OPOS = 1
            /// </summary>
            OPOS = 1,

            /// <summary>
            /// Windows = 2
            /// </summary>
            Windows = 2,
            /// <summary>
            /// Printing station = 3
            /// </summary>
            PrintingStation = 3
        }

        public enum DeviceTypes
        {
            /// <summary>
            /// None = 0
            /// </summary>
            None = 0,

            /// <summary>
            /// OPOS = 1
            /// </summary>
            OPOS = 1,
            /// <summary>
            /// USB = 2
            /// </summary>
            USB = 2,
        }

        public enum CashChangerDeviceTypes
        {
            None = 0, // Device not connected
            SingleMode = 1, // Device connected to a single device
            DualMode = 2 // Device connected with two pos applications
        }

        public enum LineDisplayDeviceTypes
        {
            /// <summary>
            /// None = 0
            /// </summary>
            None = 0,

            /// <summary>
            /// OPOS = 1
            /// </summary>
            OPOS = 1,

            ///<summary> 
            /// Virtual display  = 2
            ///</summary>
            VirtualDisplay = 2
        }

        public enum DualDisplayScreens
        {
            Screen1 = 0,
            Screen2 = 1,
            Screen3 = 2,
            Default = 3
        }

        public HardwareProfile(RecordIdentifier id, string text)
            : this()
        {
            ID = id;
            Text = text;
        }

        public HardwareProfile()
        {
            DrawerDeviceType = DeviceTypes.None;
            DrawerDeviceName = "";
            DrawerDescription = "";
            DrawerOpenText = "";

            LineDisplayDeviceType = LineDisplayDeviceTypes.None;
            DisplayDeviceName = "";
            DisplayDeviceDescription = "";
            DisplayTotalText = "";
            DisplayBalanceText = "";
            DisplayClosedLine1 = "";
            DisplayClosedLine2 = "";
            DisplayCharacterSet = 850;
            DisplayBinaryConversion = false;

            MsrDeviceType = DeviceTypes.None;
            MsrDeviceName = "";
            MsrDeviceDescription = "";
            StartTrack1 = "";
            Separator1 = "";
            EndTrack1 = "";

            Printer = 0;
            PrinterDeviceName = "";
            PrinterDeviceDescription = "";
            PrintBinaryConversion = false;
            PrinterCharacterSet = 850;
            PrinterExtraLines = 0;

            ScannerConnected = false;
            ScannerDeviceName = "";
            ScannerDeviceDescription = "";

            ETaxConnected = false;
            ETaxPortName = "";

            RFIDScannerConnected = false;
            RFIDScannerDeviceName = "";
            RfIDScannerDeviceDescription = "";

            ScaleConnected = false;
            ScaleDeviceName = "";
            ScaleDeviceDescription = "";
            ScaleManualInputAllowed = false;

            KeyLockDeviceType = DeviceTypes.None;
            KeyLockDeviceName = "";
            KeyLockDeviceDescription = "";

            EftConnected = false;
            LsPayConnected = false;
            EftDescription = "";
            EftServerName = "";
            EftServerPort = 0;
            EftCompanyID = "";
            EftUserID = "";
            EftPassword = "";

            CctvConnected = false;
            CctvHostname = "";
            CctvPort = 0;
            CctvCamera = "";

            Forecourt = false;
            ForecourtPriceDecimalPointPosition = 0;
            ForecourtToneActive = false;
            ForecourtToneFrequency = 0;
            ForecourtToneLength = 0;
            ForecourtToneRepeats = 0;
            ForecourtSuspendAllowed = false;
            Hostname = "";
            EftBatchIncrementAtEOS = false;

            CashChangerConnected = false;
            CashChangerPortSettings = "";
            CashChangerInitSettings = "";

            DualDisplayConnected = false;
            DualDisplayDeviceName = "127.0.0.1";
            DualDisplayDescription = "";
            DualDisplayType = DisplayType.Logo;
            DualDisplayPort = 1250;
            DualDisplayReceiptPrecentage = 50.0M;
            DualdisplayImagePath = "";
            DualDisplayImageInterval = 1;
            DualDisplayBrowserUrl = "";

            FCMControllerHostName = "";
            FCMImplFileName = "";
            FCMImplFileType = "";
            FCMVolumeUnitID = "";
            FCMVolumeUnitDescription = "";
            FCMServer = "";
            FCMPort = "";
            FCMPosPort = "";

            FiscalPrinter = 0;
            FiscalPrinterConnection = "";
            FiscalPrinterDescription = "";

            KeyboardmappingID = RecordIdentifier.Empty;

            PharmacyActive = false;
            PharmacyHost = "";
            PharmacyPort = 0;

            UseKitchenDisplay = false;

            DallasKeyConnected = false;
            DallasMessagePrefix = "";
            DallasKeyRemovedMessage = "";
            DallasComPort = "";
            DallasBaudRate = 9600;
            DallasParity = Parity.None;
            DallasDataBits = 8;
            DallasStopBits = StopBits.One;
            
            FileType = ".bmp";
            WindowsPrinterConfigurationID = RecordIdentifier.Empty;

            StationPrintingHostID = "";

            ProfileIsUsed = false;
        }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string DrawerDeviceName { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string DisplayDeviceName { get; set; }

        /// <summary>
        /// This field contains the text which appears on the customer display, 
        /// along with the total amount, when the POS terminal displays the total amount of a transaction.
        /// </summary>
        [DataMember]
        public string DisplayTotalText { get; set; }

        /// <summary>
        /// This field contains the text that appears on the customer display, along with the balance amount. 
        /// </summary>
        [DataMember]
        public string DisplayBalanceText { get; set; }

        /// <summary>
        /// This field contains the text that appears in the first of the two lines displayed when the POS terminal is closed.
        /// </summary>
        [DataMember]
        public string DisplayClosedLine1 { get; set; }

        /// <summary>
        /// This field contains the text that appears in the second of the two lines displayed when the POS terminal is closed.
        /// </summary>
        [DataMember]
        public string DisplayClosedLine2 { get; set; }

        /// <summary>
        /// This field contains the character set used with the display. The default character set is '850'.
        /// </summary>
        [DataMember]
        public int DisplayCharacterSet { get; set; }

        /// <summary>
        /// If true the text is converted using a decimal/binary conversion before it's sent to the line display
        /// </summary>
        [DataMember]
        public bool DisplayBinaryConversion { get; set; }

        /// <summary>
        /// The type of device connected, None or OPOS.
        /// </summary>
        [DataMember]
        public DeviceTypes MsrDeviceType { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string MsrDeviceName { get; set; }

        /// <summary>
        /// The start characters on the MSR
        /// </summary>
        [DataMember]
        public string StartTrack1 { get; set; }

        /// <summary>
        /// The separator on the card track
        /// </summary>
        [DataMember]
        public string Separator1 { get; set; }

        /// <summary>
        /// The end characters on the MSR 
        /// </summary>
        [DataMember]
        public string EndTrack1 { get; set; }

        /// <summary>
        /// The type of device connected; None, OPOS, Windows, or VirtualPrinter.
        /// </summary>
        [DataMember]
        public PrinterHardwareTypes Printer { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string PrinterDeviceName { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string PrinterDeviceDescription { get; set; }

        /// <summary>
        /// A checkmark in this field indicates that the text printed out is converted to binary text, that is, each byte becomes two bytes.
        /// </summary>
        [DataMember]
        public bool PrintBinaryConversion { get; set; }

        /// <summary>
        /// This field contains the character set used with the printer. The default character set is '850'.
        /// </summary>
        [DataMember]
        public int PrinterCharacterSet { get; set; }

        /// <summary>
        /// Extra lines to print before cutting paper
        /// </summary>
        [DataMember]
        public int PrinterExtraLines { get; set; }

        /// <summary>
        /// The type of device connected, None or OPOS.
        /// </summary>
        [DataMember]
        public bool ScannerConnected { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string ScannerDeviceName { get; set; }

        /// <summary>
        /// The type of device connected, None or OPOS.
        /// </summary>
        [DataMember]
        public bool ETaxConnected { get; set; }

        /// <summary>
        /// A name of the port used by the ETax device.
        /// </summary>
        [DataMember]
        public string ETaxPortName { get; set; }

        /// <summary>
        /// The type of device connected, None or OPOS.
        /// </summary>
        [DataMember]
        public bool RFIDScannerConnected { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string RFIDScannerDeviceName { get; set; }

        /// <summary>
        /// The type of device connected, None or OPOS.
        /// </summary>
        [DataMember]
        public bool ScaleConnected { get; set; }

        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string ScaleDeviceName { get; set; }



        /// <summary>
        /// A name of the OPOS device.
        /// </summary>
        [DataMember]
        public string KeyLockDeviceName { get; set; }

        /// <summary>
        /// Using EFT services or not.
        /// </summary>
        [DataMember]
        public bool EftConnected { get; set; }

        /// <summary>
        /// Using LS Pay or not.
        /// </summary>
        [DataMember]
        public bool LsPayConnected { get; set; }

        /// <summary>
        /// A description of the EFT service provider.
        /// </summary>
        [DataMember]
        public string EftDescription { get; set; }

        /// <summary>
        /// The EFT server name or IP address.
        /// </summary>
        [DataMember]
        public string EftServerName { get; set; }

        /// <summary>
        /// The EFT server port
        /// </summary>
        [DataMember]
        public int EftServerPort { get; set; }

        /// <summary>
        /// The company ID, aka. Merchant ID.
        /// </summary>
        [DataMember]
        public string EftCompanyID { get; set; }

        /// <summary>
        /// The user ID, aka. Terminal ID.
        /// </summary>
        [DataMember]
        public string EftUserID { get; set; }

        /// <summary>
        /// The EFT server password.
        /// </summary>
        [DataMember]
        public string EftPassword { get; set; }

        /// <summary>
        /// The merchant account for EFT
        /// </summary>
        [DataMember]
        public string EftMerchantAccount { get; set; }

        /// <summary>
        /// The merchant key for EFT
        /// </summary>
        [DataMember]
        public string EftMerchantKey { get; set; }

        /// <summary>
        /// Any custom info required for EFT
        /// </summary>
        [DataMember]
        public string EftCustomField1 { get; set; }

        /// <summary>
        /// Any custom info required for EFT
        /// </summary>
        [DataMember]
        public string EftCustomField2 { get; set; }

        [DataMember]
        public bool CctvConnected { get; set; }

        [DataMember]
        public string CctvHostname { get; set; }

        [DataMember]
        public int CctvPort { get; set; }

        [DataMember]
        public string CctvCamera { get; set; }

        /// <summary>
        /// Is the forcourt module to be used?
        /// </summary>
        [DataMember]
        public bool Forecourt { get; set; }

        /// <summary>
        /// The position of the decimal point in the price
        /// </summary>
        [DataMember]
        public int ForecourtPriceDecimalPointPosition { get; set; }

        /// <summary>
        /// Should a tone be played when a pump is calling
        /// </summary>
        [DataMember]
        public bool ForecourtToneActive { get; set; }

        /// <summary>
        /// The frequency of the tone
        /// </summary>
        [DataMember]
        public int ForecourtToneFrequency { get; set; }

        /// <summary>
        /// The length of the tone in milliseconds
        /// </summary>
        [DataMember]
        public int ForecourtToneLength { get; set; }

        /// <summary>
        /// How often should a tone be repeated.
        /// </summary>
        [DataMember]
        public int ForecourtToneRepeats { get; set; }

        /// <summary>
        /// Syncronize time on the forecourt controller to the POS
        /// </summary>
        [DataMember]
        public bool ForecourtSuspendAllowed { get; set; }

        /// <summary>
        /// The IP or hostname address of the pump controller
        /// </summary>
        [DataMember]
        public string Hostname { get; set; }

        /// <summary>
        /// Is the EFT batch closed at end of sale.
        /// </summary>
        [DataMember]
        public bool EftBatchIncrementAtEOS { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string DrawerDescription { get; set; }

        /// <summary>
        /// The text to display when opening the drawer.
        /// </summary>
        [DataMember]
        public string DrawerOpenText { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string DisplayDeviceDescription { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string MsrDeviceDescription { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string ScannerDeviceDescription { get; set; }

        /// <summary>
        /// A checkmark in this field indicates the manual input of item weight is allowed.
        /// This is only applicable if the Scale field is set to 'OPOS'.
        /// </summary>
        [DataMember]
        public bool ScaleManualInputAllowed { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string ScaleDeviceDescription { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string KeyLockDeviceDescription { get; set; }

        /// <summary>
        /// A decription of the OPOS device.
        /// </summary>
        [DataMember]
        public string RfIDScannerDeviceDescription { get; set; }

        //TODO Remove from Site Manager, should use enum.
        [DataMember]
        public bool CashChangerConnected
        {
            get { return CashChangerDeviceType == CashChangerDeviceTypes.SingleMode; }
            set { CashChangerDeviceType = value ? CashChangerDeviceTypes.SingleMode : CashChangerDeviceTypes.None; }
        }

        [DataMember]
        public CashChangerDeviceTypes CashChangerDeviceType { get; set; }

        [DataMember]
        public string CashChangerPortSettings { get; set; }

        [DataMember]
        public string CashChangerInitSettings { get; set; }

        /// <summary>
        /// The URL of the web page to display in the web browser.
        /// </summary>
        [DataMember]
        public string DualDisplayBrowserUrl { get; set; }

        /// <summary>
        /// Do we employ a dual display monitor?
        /// </summary>
        [DataMember]
        public bool DualDisplayConnected { get; set; }

        /// <summary>
        /// The IP adress of the dual display server
        /// </summary>
        [DataMember]
        public string DualDisplayDeviceName { get; set; }

        [DataMember]
        public string DualDisplayDescription { get; set; }

        /// <summary>
        /// What kind of advertisement panel to display
        /// </summary>
        [DataMember]
        public DisplayType DualDisplayType { get; set; }

        /// <summary>
        /// The port the dual display service operates on
        /// </summary>
        [DataMember]
        public int DualDisplayPort { get; set; }

        /// <summary>
        /// The width of the receipt control with regards to the dual display screen
        /// </summary>
        [DataMember]
        public decimal DualDisplayReceiptPrecentage { get; set; }

        /// <summary>
        /// The path to the images
        /// </summary>
        [DataMember]
        public string DualdisplayImagePath { get; set; }

        /// <summary>
        /// How frequently to change images (in seconds).
        /// </summary>
        [DataMember]
        public int DualDisplayImageInterval { get; set; }

        /// <summary>
        /// The portion of the screen that will be used for the pos. Forecourt manager will use the rest
        /// </summary>
        [DataMember]
        public decimal FCMScreenHeightPercentage { get; set; }

        [DataMember]
        public decimal FCMScreenExtHeightPercentage { get; set; }

        [DataMember]
        public string FCMControllerHostName { get; set; }

        [DataMember]
        public string FCMImplFileName { get; set; }

        [DataMember]
        public string FCMImplFileType { get; set; }

        [DataMember]
        public RecordIdentifier FCMVolumeUnitID { get; set; }

        [DataMember]
        public string FCMVolumeUnitDescription { get; set; }

        /// <summary>
        /// The IP adress of the computer hosting the Forecourt Manager
        /// </summary>
        [DataMember]
        public string FCMServer { get; set; }

        /// <summary>
        /// The port the Forecourt Manager operates on
        /// </summary>
        [DataMember]
        public string FCMPort { get; set; }

        /// <summary>
        /// The port the POS operates on
        /// </summary>
        [DataMember]
        public string FCMPosPort { get; set; }

        [DataMember]
        public int FCMLogLevel { get; set; }

        [DataMember]
        public int FCMFuellingPointColumns { get; set; }

        [DataMember]
        public Boolean FCMCallingSound { get; set; }

        [DataMember]
        public Boolean FCMCallingBlink { get; set; }

        /// <summary>
        /// Do we connect to a Forecourt Manager?
        /// </summary>
        [DataMember]
        public Boolean FCMActive { get; set; }

        [DataMember]
        public int FiscalPrinter { get; set; }

        [DataMember]
        public string FiscalPrinterConnection { get; set; }

        [DataMember]
        public string FiscalPrinterDescription { get; set; }

        [DataMember]
        public string FiscalPrinterDeviceName { get; set; }

        // TODO Make sure that the database field has been altered to mach the validation
        [RecordIdentifierValidation(20)]
        [DataMember]
        public RecordIdentifier KeyboardmappingID { get; set; }

        /// <summary>
        /// Should the pharmacy service be active or not
        /// </summary>
        [DataMember]
        public bool PharmacyActive { get; set; }

        /// <summary>
        /// Host name or IP-address of the pharmacy service
        /// </summary>
        [StringLength(30)]
        [DataMember]
        public string PharmacyHost { get; set; }

        /// <summary>
        /// The port id of the pharmacy service
        /// </summary>
        [DataMember]
        public int PharmacyPort { get; set; }

        [DataMember]
        public bool UseKitchenDisplay { get; set; }

        [DataMember]
        public DualDisplayScreens DualDisplayScreen { get; set; }

        [DataMember]
        public DeviceTypes DrawerDeviceType { get; set;}

        [DataMember]
        public DeviceTypes KeyLockDeviceType
        {
            get; set; 
        }

        [DataMember]
        public LineDisplayDeviceTypes LineDisplayDeviceType { get; set; }

        [DataMember]
        public bool DallasKeyConnected { get; set; }

        [StringLength(50)]
        [DataMember]
        public string DallasMessagePrefix { get; set; }

        [StringLength(50)]
        [DataMember]
        public string DallasKeyRemovedMessage { get; set; }

        [StringLength(6)]
        [DataMember]
        public string DallasComPort { get; set; }

        [DataMember]
        public int DallasBaudRate { get; set; }

        [DataMember]
        public Parity DallasParity { get; set; }

        [DataMember]
        public int DallasDataBits { get; set; }

        [DataMember]
        public StopBits DallasStopBits { get; set; }

        /// <summary>
        /// The file type the WinPrinter uses when saving the printout as a file. The default value is .PNG and is not saved to the database
        /// </summary>
        [DataMember]
        public string FileType { get; set; }

        /// <summary>
        /// True if the profile is used by a store or terminal
        /// </summary>
        [DataMember]
        public bool ProfileIsUsed { get; set; }

        /// <summary>
        /// ID of the windows printer configuration
        /// </summary>
        [DataMember]
        public RecordIdentifier WindowsPrinterConfigurationID { get; set; }

        /// <summary>
        /// Windows printer configuration loaded at POS startup if WindowsPrinterConfigurationID is set
        /// </summary>
        public WindowsPrinterConfiguration WindowsPrinterConfiguration { get; set; }

        /// <summary>
        /// The ID of the station printing host to use when the POS has <see cref="Printer"/> set to <see cref="PrinterHardwareTypes.PrintingStation"/>
        /// </summary>
        [RecordIdentifierValidation(20)]
        [DataMember]
        public RecordIdentifier StationPrintingHostID { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "profileID":
                                ID = current.Value;
                                break;
                            case "name":
                                Text = current.Value;
                                break;
                            case "drawerDeviceName":
                                DrawerDeviceName = current.Value;
                                break;
                            case "drawerConnected":
                                DrawerDeviceType = current.Value != "false" ? DeviceTypes.OPOS : DeviceTypes.None;                                 
                                break;
                            case "displayDevice":
                                LineDisplayDeviceType = (LineDisplayDeviceTypes)Convert.ToInt32(current.Value);
                                break;
                            case "displayDeviceName":
                                DisplayDeviceName = current.Value;
                                break;
                            case "displayTotalText":
                                DisplayTotalText = current.Value;
                                break;
                            case "displayBalanceText":
                                DisplayBalanceText = current.Value;
                                break;
                            case "displayClosedLine1":
                                DisplayClosedLine1 = current.Value;
                                break;
                            case "displayClosedLine2":
                                DisplayClosedLine2 = current.Value;
                                break;
                            case "displayCharacterSet":
                                DisplayCharacterSet = Convert.ToInt32(current.Value);
                                break;
                            case "displayBinaryConversion":
                                DisplayBinaryConversion = current.Value != "false";
                                break;
                            case "msrDeviceType":
                                MsrDeviceType = (DeviceTypes)Convert.ToInt32(current.Value);
                                break;
                            case "msrDeviceName":
                                MsrDeviceName = current.Value;
                                break;
                            case "startTrack1":
                                StartTrack1 = current.Value;
                                break;
                            case "separator1":
                                Separator1 = current.Value;
                                break;
                            case "endTrack1":
                                EndTrack1 = current.Value;
                                break;
                            case "printer":
                                Printer = (PrinterHardwareTypes) Convert.ToInt32(current.Value);
                                break;
                            case "printerDeviceName":
                                PrinterDeviceName = current.Value;
                                break;
                            case "printerDeviceDescription":
                                PrinterDeviceDescription = current.Value;
                                break;
                            case "printBinaryConversion":
                                PrintBinaryConversion = current.Value != "false";
                                break;
                            case "printerCharacterSet":
                                PrinterCharacterSet = Convert.ToInt32(current.Value);
                                break;
                            case "printerExtraLines":
                                PrinterExtraLines = Convert.ToInt32(current.Value);
                                break;
                            case "scannerConnected":
                                ScannerConnected = current.Value != "false";
                                break;
                            case "scannerDeviceName":
                                ScannerDeviceName = current.Value;
                                break;
                            case "eTaxConnected":
                                ETaxConnected = current.Value != "false";
                                break;
                            case "eTaxPortName":
                                ETaxPortName = current.Value;
                                break;
                            case "rFIDScannerConnected":
                                RFIDScannerConnected = current.Value != "false";
                                break;
                            case "rFIDScannerDeviceName":
                                RFIDScannerDeviceName = current.Value;
                                break;
                            case "scaleConnected":
                                ScaleConnected = current.Value != "false";
                                break;
                            case "scaleDeviceName":
                                ScaleDeviceName = current.Value;
                                break;
                            case "keyLockConnected":
                                KeyLockDeviceType = current.Value != "false" ? DeviceTypes.OPOS : DeviceTypes.None;
                                break;
                            case "keyLockDeviceName":
                                KeyLockDeviceName = current.Value;
                                break;
                            case "eftConnected":
                                EftConnected = current.Value != "false";
                                break;
                            case "lsPayConnected":
                                LsPayConnected = current.Value != "false";
                                break;
                            case "eftDescription":
                                EftDescription = current.Value;
                                break;
                            case "eftServerName":
                                EftServerName = current.Value;
                                break;
                            case "eftServerPort":
                                EftServerPort = Convert.ToInt32(current.Value);
                                break;
                            case "eftCompanyID":
                                EftCompanyID = current.Value;
                                break;
                            case "eftUserID":
                                EftUserID = current.Value;
                                break;
                            case "eftPassword":
                                EftPassword = current.Value;
                                break;
                            case "cctvConnected":
                                CctvConnected = current.Value != "false";
                                break;
                            case "cctvHostname":
                                CctvHostname = current.Value;
                                break;
                            case "cctvPort":
                                CctvPort = Convert.ToInt32(current.Value);
                                break;
                            case "cctvCamera":
                                CctvCamera = current.Value;
                                break;
                            case "forecourt":
                                Forecourt = current.Value != "false";
                                break;
                            case "forecourtPriceDecimalPointPosition":
                                ForecourtPriceDecimalPointPosition = Convert.ToInt32(current.Value);
                                break;
                            case "forecourtToneActive":
                                ForecourtToneActive = current.Value != "false";
                                break;
                            case "forecourtToneFrequency":
                                ForecourtToneFrequency = Convert.ToInt32(current.Value);
                                break;
                            case "forecourtToneLength":
                                ForecourtToneLength = Convert.ToInt32(current.Value);
                                break;
                            case "forecourtToneRepeats":
                                ForecourtToneRepeats = Convert.ToInt32(current.Value);
                                break;
                            case "forecourtSuspendAllowed":
                                ForecourtSuspendAllowed = current.Value != "false";
                                break;
                            case "hostname":
                                Hostname = current.Value;
                                break;
                            case "eftBatchIncrementAtEOS":
                                EftBatchIncrementAtEOS = current.Value != "false";
                                break;
                            case "drawerDescription":
                                DrawerDescription = current.Value;
                                break;
                            case "drawerOpenText":
                                DrawerOpenText = current.Value;
                                break;
                            case "displayDeviceDescription":
                                DisplayDeviceDescription = current.Value;
                                break;
                            case "msrDeviceDescription":
                                MsrDeviceDescription = current.Value;
                                break;
                            case "scannerDeviceDescription":
                                ScannerDeviceDescription = current.Value;
                                break;
                            case "scaleManualInputAllowed":
                                ScaleManualInputAllowed = current.Value != "false";
                                break;
                            case "scaleDeviceDescription":
                                ScaleDeviceDescription = current.Value;
                                break;
                            case "keyLockDeviceDescription":
                                KeyLockDeviceDescription = current.Value;
                                break;
                            case "rfIDScannerDeviceDescription":
                                RfIDScannerDeviceDescription = current.Value;
                                break;
                            case "cashChangerConnected":
                                CashChangerConnected = current.Value != "false";
                                break;
                            case "cashChangerDeviceType":
                                CashChangerDeviceType = (CashChangerDeviceTypes) Convert.ToInt32(current.Value);
                                break;
                            case "cashChangerPortSettings":
                                CashChangerPortSettings = current.Value;
                                break;
                            case "cashChangerInitSettings":
                                CashChangerInitSettings = current.Value;
                                break;
                            case "dualDisplayBrowserUrl":
                                DualDisplayBrowserUrl = current.Value;
                                break;
                            case "dualDisplayConnected":
                                DualDisplayConnected = current.Value != "false";
                                break;
                            case "dualDisplayDeviceName":
                                DualDisplayDeviceName = current.Value;
                                break;
                            case "dualDisplayDescription":
                                DualDisplayDescription = current.Value;
                                break;
                            case "dualDisplayType":
                                DualDisplayType = (DisplayType) Convert.ToInt32(current.Value);
                                break;
                            case "dualDisplayPort":
                                DualDisplayPort = Convert.ToInt32(current.Value);
                                break;
                            case "dualDisplayReceiptPrecentage":
                                DualDisplayReceiptPrecentage = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "dualdisplayImagePath":
                                DualdisplayImagePath = current.Value;
                                break;
                            case "dualDisplayImageInterval":
                                DualDisplayImageInterval = Convert.ToInt32(current.Value);
                                break;
                            case "fCMScreenHeightPercentage":
                                FCMScreenHeightPercentage = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "fCMScreenExtHeightPercentage":
                                FCMScreenExtHeightPercentage = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "fCMControllerHostName":
                                FCMControllerHostName = current.Value;
                                break;
                            case "fCMImplFileName":
                                FCMImplFileName = current.Value;
                                break;
                            case "fCMImplFileType":
                                FCMImplFileType = current.Value;
                                break;
                            case "fCMVolumeUnitID":
                                FCMVolumeUnitID = current.Value;
                                break;
                            case "fCMVolumeUnitDescription":
                                FCMVolumeUnitDescription = current.Value;
                                break;
                            case "fCMServer":
                                FCMServer = current.Value;
                                break;
                            case "fCMPort":
                                FCMPort = current.Value;
                                break;
                            case "fCMPosPort":
                                FCMPosPort = current.Value;
                                break;
                            case "fCMLogLevel":
                                FCMLogLevel = Convert.ToInt32(current.Value);
                                break;
                            case "fCMFuellingPointColumns":
                                FCMFuellingPointColumns = Convert.ToInt32(current.Value);
                                break;
                            case "fCMCallingSound":
                                FCMCallingSound = current.Value != "false";
                                break;
                            case "fCMCallingBlink":
                                FCMCallingBlink = current.Value != "false";
                                break;
                            case "fCMActive":
                                FCMActive = current.Value != "false";
                                break;
                            case "fiscalPrinter":
                                FiscalPrinter = Convert.ToInt32(current.Value);
                                break;
                            case "fiscalPrinterConnection":
                                FiscalPrinterConnection = current.Value;
                                break;
                            case "fiscalPrinterDescription":
                                FiscalPrinterDescription = current.Value;
                                break;
                            case "keyboardmappingID":
                                KeyboardmappingID = current.Value;
                                break;
                            case "pharmacyActive":
                                PharmacyActive = current.Value != "false";
                                break;
                            case "pharmacyHost":
                                PharmacyHost = current.Value;
                                break;
                            case "pharmacyPort":
                                PharmacyPort = Convert.ToInt32(current.Value);
                                break;
                            case "useKitchenDisplay":
                                UseKitchenDisplay = current.Value != "false";
                                break;
                            case "dualDisplayScreen":
                                DualDisplayScreen = (DualDisplayScreens) Convert.ToInt32(current.Value);
                                break;
                            case "dallasKeyConnected":
                                DallasKeyConnected = current.Value != "false";
                                break;
                            case "dallasMessagePrefix":
                                DallasMessagePrefix = current.Value;
                                break;
                            case "dallasKeyRemovedMessage":
                                DallasKeyRemovedMessage = current.Value;
                                break;
                            case "dallasComPort":
                                DallasComPort = current.Value;
                                break;
                            case "dallasBaudRate":
                                DallasBaudRate = Convert.ToInt32(current.Value);
                                break;
                            case "dallasParity":
                                DallasParity = (Parity) Convert.ToInt32(current.Value);
                                break;
                            case "dallasDataBits":
                                DallasDataBits = Convert.ToInt32(current.Value);
                                break;
                            case "dallasStopBits":
                                DallasStopBits = (StopBits) Convert.ToInt32(current.Value);
                                break;
                            case "FileType":
                                FileType = current.Value;
                                break;
                            case "ProfileIsUsed":
                                ProfileIsUsed = current.Value != "false";
                                break;
                            case "StationPrintingHostID":
                                StationPrintingHostID = current.Value;
                                break;
                            case "WindowsPrinterConfigurationID":
                                WindowsPrinterConfigurationID = current.Value;
                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("hardwareProfile",
                new XElement("profileID", ID),
                new XElement("name", Text),
                new XElement("drawerDeviceName", DrawerDeviceName),
                new XElement("drawerConnected", DrawerDeviceType == DeviceTypes.OPOS),
                new XElement("displayDevice", (int)LineDisplayDeviceType),
                new XElement("displayDeviceName", DisplayDeviceName),
                new XElement("displayTotalText", DisplayTotalText),
                new XElement("displayBalanceText", DisplayBalanceText),
                new XElement("displayClosedLine1", DisplayClosedLine1),
                new XElement("displayClosedLine2", DisplayClosedLine2),
                new XElement("displayCharacterSet", DisplayCharacterSet),
                new XElement("displayBinaryConversion", DisplayBinaryConversion),
                new XElement("msrDeviceType", (int)MsrDeviceType),
                new XElement("msrDeviceName", MsrDeviceName),
                new XElement("startTrack1", StartTrack1),
                new XElement("separator1", Separator1),
                new XElement("endTrack1", EndTrack1),
                new XElement("printer", (int) Printer),
                new XElement("printerDeviceName", PrinterDeviceName),
                new XElement("printerDeviceDescription", PrinterDeviceDescription),
                new XElement("printBinaryConversion", PrintBinaryConversion),
                new XElement("printerCharacterSet", PrinterCharacterSet),
                new XElement("printerExtraLines", PrinterExtraLines),
                new XElement("scannerConnected", ScannerConnected),
                new XElement("scannerDeviceName", ScannerDeviceName),
                new XElement("eTaxConnected", ETaxConnected),
                new XElement("eTaxPortName", ETaxPortName),
                new XElement("rFIDScannerConnected", RFIDScannerConnected),
                new XElement("rFIDScannerDeviceName", RFIDScannerDeviceName),
                new XElement("scaleConnected", ScaleConnected),
                new XElement("scaleDeviceName", ScaleDeviceName),
                new XElement("KeyLockDeviceType", KeyLockDeviceType == DeviceTypes.OPOS),
                new XElement("keyLockDeviceName", KeyLockDeviceName),
                new XElement("eftConnected", EftConnected),
                new XElement("lsPayConnected", LsPayConnected),
                new XElement("eftDescription", EftDescription),
                new XElement("eftServerName", EftServerName),
                new XElement("eftServerPort", EftServerPort),
                new XElement("eftCompanyID", EftCompanyID),
                new XElement("eftUserID", EftUserID),
                new XElement("eftPassword", EftPassword),
                new XElement("cctvConnected", CctvConnected),
                new XElement("cctvHostname", CctvHostname),
                new XElement("cctvPort", CctvPort),
                new XElement("cctvCamera", CctvCamera),
                new XElement("forecourt", Forecourt),
                new XElement("forecourtPriceDecimalPointPosition", ForecourtPriceDecimalPointPosition),
                new XElement("forecourtToneActive", ForecourtToneActive),
                new XElement("forecourtToneFrequency", ForecourtToneFrequency),
                new XElement("forecourtToneLength", ForecourtToneLength),
                new XElement("forecourtToneRepeats", ForecourtToneRepeats),
                new XElement("forecourtSuspendAllowed", ForecourtSuspendAllowed),
                new XElement("hostname", Hostname),
                new XElement("eftBatchIncrementAtEOS", EftBatchIncrementAtEOS),
                new XElement("drawerDescription", DrawerDescription),
                new XElement("drawerOpenText", DrawerOpenText),
                new XElement("displayDeviceDescription", DisplayDeviceDescription),
                new XElement("msrDeviceDescription", MsrDeviceDescription),
                new XElement("scannerDeviceDescription", ScannerDeviceDescription),
                new XElement("scaleManualInputAllowed", ScaleManualInputAllowed),
                new XElement("scaleDeviceDescription", ScaleDeviceDescription),
                new XElement("keyLockDeviceDescription", KeyLockDeviceDescription),
                new XElement("rfIDScannerDeviceDescription", RfIDScannerDeviceDescription),
                new XElement("cashChangerConnected", CashChangerConnected),
                new XElement("cashChangerDeviceType", (int) CashChangerDeviceType),
                new XElement("cashChangerPortSettings", CashChangerPortSettings),
                new XElement("cashChangerInitSettings", CashChangerInitSettings),
                new XElement("dualDisplayBrowserUrl", DualDisplayBrowserUrl),
                new XElement("dualDisplayConnected", DualDisplayConnected),
                new XElement("dualDisplayDeviceName", DualDisplayDeviceName),
                new XElement("dualDisplayDescription", DualDisplayDescription),
                new XElement("dualDisplayType", (int) DualDisplayType),
                new XElement("dualDisplayPort", DualDisplayPort),
                new XElement("dualDisplayReceiptPrecentage", DualDisplayReceiptPrecentage.ToString(XmlCulture)),
                new XElement("dualdisplayImagePath", DualdisplayImagePath),
                new XElement("dualDisplayImageInterval", DualDisplayImageInterval),
                new XElement("fCMScreenHeightPercentage", FCMScreenHeightPercentage.ToString(XmlCulture)),
                new XElement("fCMScreenExtHeightPercentage", FCMScreenExtHeightPercentage.ToString(XmlCulture)),
                new XElement("fCMControllerHostName", FCMControllerHostName),
                new XElement("fCMImplFileName", FCMImplFileName),
                new XElement("fCMImplFileType", FCMImplFileType),
                new XElement("fCMVolumeUnitID", (string) FCMVolumeUnitID),
                new XElement("fCMVolumeUnitDescription", FCMVolumeUnitDescription),
                new XElement("fCMServer", FCMServer),
                new XElement("fCMPort", FCMPort),
                new XElement("fCMPosPort", FCMPosPort),
                new XElement("fCMLogLevel", FCMLogLevel),
                new XElement("fCMFuellingPointColumns", FCMFuellingPointColumns),
                new XElement("fCMCallingSound", FCMCallingSound),
                new XElement("fCMCallingBlink", FCMCallingBlink),
                new XElement("fCMActive", FCMActive),
                new XElement("fiscalPrinter", FiscalPrinter),
                new XElement("fiscalPrinterConnection", FiscalPrinterConnection),
                new XElement("fiscalPrinterDescription", FiscalPrinterDescription),
                new XElement("keyboardmappingID", (string) KeyboardmappingID),
                new XElement("pharmacyActive", PharmacyActive),
                new XElement("pharmacyHost", PharmacyHost),
                new XElement("pharmacyPort", PharmacyPort),
                new XElement("useKitchenDisplay", UseKitchenDisplay),
                new XElement("dualDisplayScreen", (int) DualDisplayScreen),
                new XElement("dallasKeyConnected", DallasKeyConnected),
                new XElement("dallasMessagePrefix", DallasMessagePrefix),
                new XElement("dallasKeyRemovedMessage", DallasKeyRemovedMessage),
                new XElement("dallasComPort", DallasComPort),
                new XElement("dallasBaudRate", DallasBaudRate),
                new XElement("dallasParity", (int) DallasParity),
                new XElement("dallasDataBits", DallasDataBits),
                new XElement("dallasStopBits", (int) DallasStopBits),
                new XElement("FileType", FileType),
                new XElement("ProfileIsUsed", ProfileIsUsed),
                new XElement("StationPrintingHostID", StationPrintingHostID),
                new XElement("WindowsPrinterConfigurationID", WindowsPrinterConfigurationID)
                );
            return xml;
        }

        public override object Clone()
        {
            var o = new HardwareProfile();
            Populate(o);
            return o;
        }

        protected void Populate(HardwareProfile o)
        {
            o.ID = (RecordIdentifier) ID.Clone();
            o.Text = Text;
            o.DrawerDeviceName = DrawerDeviceName;
            o.DrawerDeviceType = DrawerDeviceType;
            o.LineDisplayDeviceType = LineDisplayDeviceType;
            o.DisplayDeviceName = DisplayDeviceName;
            o.DisplayTotalText = DisplayTotalText;
            o.DisplayBalanceText = DisplayBalanceText;
            o.DisplayClosedLine1 = DisplayClosedLine1;
            o.DisplayClosedLine2 = DisplayClosedLine2;
            o.DisplayCharacterSet = DisplayCharacterSet;
            o.DisplayBinaryConversion = DisplayBinaryConversion;
            o.MsrDeviceType = MsrDeviceType;
            o.MsrDeviceName = MsrDeviceName;
            o.StartTrack1 = StartTrack1;
            o.Separator1 = Separator1;
            o.EndTrack1 = EndTrack1;
            o.Printer = Printer;
            o.PrinterDeviceName = PrinterDeviceName;
            o.PrinterDeviceDescription = PrinterDeviceDescription;
            o.PrintBinaryConversion = PrintBinaryConversion;
            o.PrinterCharacterSet = PrinterCharacterSet;
            o.PrinterExtraLines = PrinterExtraLines;
            o.ScannerConnected = ScannerConnected;
            o.ScannerDeviceName = ScannerDeviceName;
            o.ETaxConnected = ETaxConnected;
            o.ETaxPortName = ETaxPortName;
            o.RFIDScannerConnected = RFIDScannerConnected;
            o.RFIDScannerDeviceName = RFIDScannerDeviceName;
            o.ScaleConnected = ScaleConnected;
            o.ScaleDeviceName = ScaleDeviceName;
            o.KeyLockDeviceType = KeyLockDeviceType;
            o.KeyLockDeviceName = KeyLockDeviceName;
            o.EftConnected = EftConnected;
            o.LsPayConnected = LsPayConnected;
            o.EftDescription = EftDescription;
            o.EftServerName = EftServerName;
            o.EftServerPort = EftServerPort;
            o.EftCompanyID = EftCompanyID;
            o.EftUserID = EftUserID;
            o.EftPassword = EftPassword;
            o.CctvConnected = CctvConnected;
            o.CctvHostname = CctvHostname;
            o.CctvPort = CctvPort;
            o.CctvCamera = CctvCamera;
            o.Forecourt = Forecourt;
            o.ForecourtPriceDecimalPointPosition = ForecourtPriceDecimalPointPosition;
            o.ForecourtToneActive = ForecourtToneActive;
            o.ForecourtToneFrequency = ForecourtToneFrequency;
            o.ForecourtToneLength = ForecourtToneLength;
            o.ForecourtToneRepeats = ForecourtToneRepeats;
            o.ForecourtSuspendAllowed = ForecourtSuspendAllowed;
            o.Hostname = Hostname;
            o.EftBatchIncrementAtEOS = EftBatchIncrementAtEOS;
            o.DrawerDescription = DrawerDescription;
            o.DrawerOpenText = DrawerOpenText;
            o.DisplayDeviceDescription = DisplayDeviceDescription;
            o.MsrDeviceDescription = MsrDeviceDescription;
            o.ScannerDeviceDescription = ScannerDeviceDescription;
            o.ScaleManualInputAllowed = ScaleManualInputAllowed;
            o.ScaleDeviceDescription = ScaleDeviceDescription;
            o.KeyLockDeviceDescription = KeyLockDeviceDescription;
            o.RfIDScannerDeviceDescription = RfIDScannerDeviceDescription;
            o.CashChangerConnected = CashChangerConnected;
            o.CashChangerDeviceType = CashChangerDeviceType;
            o.CashChangerPortSettings = CashChangerPortSettings;
            o.CashChangerInitSettings = CashChangerInitSettings;
            o.DualDisplayBrowserUrl = DualDisplayBrowserUrl;
            o.DualDisplayConnected = DualDisplayConnected;
            o.DualDisplayDeviceName = DualDisplayDeviceName;
            o.DualDisplayDescription = DualDisplayDescription;
            o.DualDisplayType = DualDisplayType;
            o.DualDisplayPort = DualDisplayPort;
            o.DualDisplayReceiptPrecentage = DualDisplayReceiptPrecentage;
            o.DualdisplayImagePath = DualdisplayImagePath;
            o.DualDisplayImageInterval = DualDisplayImageInterval;
            o.FCMScreenHeightPercentage = FCMScreenHeightPercentage;
            o.FCMScreenExtHeightPercentage = FCMScreenExtHeightPercentage;
            o.FCMControllerHostName = FCMControllerHostName;
            o.FCMImplFileName = FCMImplFileName;
            o.FCMImplFileType = FCMImplFileType;
            o.FCMVolumeUnitID = (RecordIdentifier) FCMVolumeUnitID.Clone();
            o.FCMVolumeUnitDescription = FCMVolumeUnitDescription;
            o.FCMServer = FCMServer;
            o.FCMPort = FCMPort;
            o.FCMPosPort = FCMPosPort;
            o.FCMLogLevel = FCMLogLevel;
            o.FCMFuellingPointColumns = FCMFuellingPointColumns;
            o.FCMCallingSound = FCMCallingSound;
            o.FCMCallingBlink = FCMCallingBlink;
            o.FCMActive = FCMActive;
            o.FiscalPrinter = FiscalPrinter;
            o.FiscalPrinterConnection = FiscalPrinterConnection;
            o.FiscalPrinterDescription = FiscalPrinterDescription;
            o.KeyboardmappingID = (RecordIdentifier) KeyboardmappingID.Clone();
            o.PharmacyActive = PharmacyActive;
            o.PharmacyHost = PharmacyHost;
            o.PharmacyPort = PharmacyPort;
            o.UseKitchenDisplay = UseKitchenDisplay;
            o.DualDisplayScreen = DualDisplayScreen;
            o.DallasKeyConnected = DallasKeyConnected;
            o.DallasMessagePrefix = DallasMessagePrefix;
            o.DallasKeyRemovedMessage = DallasKeyRemovedMessage;
            o.DallasComPort = DallasComPort;
            o.DallasBaudRate = DallasBaudRate;
            o.DallasParity = DallasParity;
            o.DallasDataBits = DallasDataBits;
            o.DallasStopBits = DallasStopBits;
            o.FileType = FileType;
            o.ProfileIsUsed = ProfileIsUsed;
            o.WindowsPrinterConfigurationID = WindowsPrinterConfigurationID;
        }
    }
}
