using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of Peripherals page
    /// </summary>
    public class Peripherals : DataEntity
    {
        public Peripherals()
        {
            ID = RecordIdentifier.Empty;
            BarcodeReader = byte.MinValue;
            CardReader = byte.MinValue;
            CashChanger = byte.MinValue;
            CreditCardService = byte.MinValue;
            Drawer = byte.MinValue;
            DualDisplay = byte.MinValue;
            FiscalPrinter = byte.MinValue;
            Key = byte.MinValue;
            LineDisplay = byte.MinValue;
            Printer = byte.MinValue;
            RFIDReader = byte.MinValue;
            Scale = byte.MinValue;
            HardwareProfile = new HardwareProfile();
        }
                
        public byte BarcodeReader { get; set; }
        public byte CardReader { get; set; }
        public byte CashChanger { get; set; }
        public byte CreditCardService { get; set; }
        public byte Drawer { get; set; }
        public byte DualDisplay { get; set; }
        public byte FiscalPrinter { get; set; }
        public byte Key { get; set; }
        public byte LineDisplay { get; set; }
        public byte Printer { get; set; }
        public byte RFIDReader { get; set; }
        public byte Scale { get; set; }
        public byte ETax { get; set; }
        public HardwareProfile HardwareProfile { get; set; }
        
        /// <summary>
        /// Sets all variables in the Peripherals class with the values in the xml
        /// </summary>
        /// <param name="xPeripherals">The xml element with the peripherals setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xPeripherals, IErrorLog errorLogger = null)
        {
            if (xPeripherals.HasElements)
            {
                if (xPeripherals.Name == "PosPeripherals")
                {
                    var peripheralVariables = xPeripherals.Elements();
                    foreach (var peripheralElem in peripheralVariables)
                    {
                        //No printer -> no peripheral setting -> no need to go any further
                        if (peripheralElem.Name.ToString() == "printer" && peripheralElem.Value == "")
                        {
                            return;
                        }
                        if (!peripheralElem.IsEmpty)
                        {
                            try
                            {
                                switch (peripheralElem.Name.ToString())
                                {
                                    case "barcodeReader":
                                        BarcodeReader = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "cardReader":
                                        CardReader = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "cashChanger":
                                        CashChanger = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "creditCardService":
                                        CreditCardService = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "drawer":
                                        Drawer = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "dualDisplay":
                                        DualDisplay = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "fiscalPrinter":
                                        FiscalPrinter = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "key":
                                        Key = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "lineDisplay":
                                        LineDisplay = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "printer":
                                        Printer = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "rFIDReader":
                                        RFIDReader = Convert.ToByte(peripheralElem.Value);
                                        break;
                                    case "scale":
                                        Scale = Convert.ToByte(peripheralElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, peripheralElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
                if (xPeripherals.Name == "hardwareProfile")
                {
                    var profileVariables = xPeripherals.Elements();
                    foreach (var profileElem in profileVariables)
                    {
                        //No profile id -> no peripheral setting -> no need to go any further
                        if (profileElem.Name.ToString() == "profileID" && profileElem.Value == "")
                        {
                            return;
                        }

                        if (!profileElem.IsEmpty)
                        {
                            try
                            {
                                switch (profileElem.Name.ToString())
                                {
                                    case "profileID":
                                        HardwareProfile.ID = profileElem.Value;
                                        break;
                                    case "name":
                                        HardwareProfile.Text = profileElem.Value;
                                        break;
                                    case "drawerDeviceName":
                                        HardwareProfile.DrawerDeviceName = profileElem.Value;
                                        break;
                                    case "drawerConnected":
                                        HardwareProfile.DrawerDeviceType = (HardwareProfile.DeviceTypes) Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "displayDevice":
                                        HardwareProfile.LineDisplayDeviceType = (HardwareProfile.LineDisplayDeviceTypes) Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "displayDeviceName":
                                        HardwareProfile.DisplayDeviceName = profileElem.Value;
                                        break;
                                    case "displayTotalText":
                                        HardwareProfile.DisplayTotalText = profileElem.Value;
                                        break;
                                    case "displayBalanceText":
                                        HardwareProfile.DisplayBalanceText = profileElem.Value;
                                        break;
                                    case "dualDisplay":
                                        HardwareProfile.DisplayBalanceText = profileElem.Value;
                                        break;
                                    case "displayClosedLine1":
                                        HardwareProfile.DisplayClosedLine1 = profileElem.Value;
                                        break;
                                    case "displayClosedLine2":
                                        HardwareProfile.DisplayClosedLine2 = profileElem.Value;
                                        break;
                                    case "displayCharacterSet":
                                        HardwareProfile.DisplayCharacterSet = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "displayBinaryConversion":
                                        HardwareProfile.DisplayBinaryConversion = profileElem.Value != "false";
                                        break;
                                    case "msrConnected":
                                        HardwareProfile.MsrDeviceType = (HardwareProfile.DeviceTypes)Convert.ToInt32(profileElem.Value);                                            
                                        break;
                                    case "msrDeviceName":
                                        HardwareProfile.MsrDeviceName = profileElem.Value;
                                        break;
                                    case "startTrack1":
                                        HardwareProfile.StartTrack1 = profileElem.Value;
                                        break;
                                    case "separator1":
                                        HardwareProfile.Separator1 = profileElem.Value;
                                        break;
                                    case "endTrack1":
                                        HardwareProfile.EndTrack1 = profileElem.Value;
                                        break;
                                    case "printer":
                                        HardwareProfile.Printer = (HardwareProfile.PrinterHardwareTypes)Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "printerDeviceName":
                                        HardwareProfile.PrinterDeviceName = profileElem.Value;
                                        break;
                                    case "printerDeviceDescription":
                                        HardwareProfile.PrinterDeviceDescription = profileElem.Value;
                                        break;
                                    case "printBinaryConversion":
                                        HardwareProfile.PrintBinaryConversion = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "printerCharacterSet":
                                        HardwareProfile.PrinterCharacterSet = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "scannerConnected":
                                        HardwareProfile.ScannerConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "scannerDeviceName":
                                        HardwareProfile.ScannerDeviceName = profileElem.Value;
                                        break;
                                    case "eTaxConnected":
                                        HardwareProfile.ETaxConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "eTaxPortName":
                                        HardwareProfile.ETaxPortName = profileElem.Value;
                                        break;
                                    case "rFIDScannerConnected":
                                        HardwareProfile.RFIDScannerConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "rFIDScannerDeviceName":
                                        HardwareProfile.RFIDScannerDeviceName = profileElem.Value;
                                        break;
                                    case "scaleConnected":
                                        HardwareProfile.ScaleConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "scaleDeviceName":
                                        HardwareProfile.ScaleDeviceName = profileElem.Value;
                                        break;
                                    case "keyLockConnected":
                                        HardwareProfile.KeyLockDeviceType = (HardwareProfile.DeviceTypes ) Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "keyLockDeviceName":
                                        HardwareProfile.KeyLockDeviceName = profileElem.Value;
                                        break;
                                    case "eftConnected":
                                        HardwareProfile.EftConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "eftDescription":
                                        HardwareProfile.EftDescription = profileElem.Value;
                                        break;
                                    case "eftServerName":
                                        HardwareProfile.EftServerName = profileElem.Value;
                                        break;
                                    case "eftServerPort":
                                        HardwareProfile.EftServerPort = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "eftCompanyID":
                                        HardwareProfile.EftCompanyID = profileElem.Value;
                                        break;
                                    case "eftUserID":
                                        HardwareProfile.EftUserID = profileElem.Value;
                                        break;
                                    case "eftPassword":
                                        HardwareProfile.EftPassword = profileElem.Value;
                                        break;
                                    case "cctvConnected":
                                        HardwareProfile.CctvConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "cctvHostname":
                                        HardwareProfile.CctvHostname = profileElem.Value;
                                        break;
                                    case "cctvPort":
                                        HardwareProfile.CctvPort = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "cctvCamera":
                                        HardwareProfile.CctvCamera = profileElem.Value;
                                        break;
                                    case "forecourt":
                                        HardwareProfile.Forecourt = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "forecourtPriceDecimalPointPosition":
                                        HardwareProfile.ForecourtPriceDecimalPointPosition = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "forecourtToneActive":
                                        HardwareProfile.ForecourtToneActive = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "forecourtToneFrequency":
                                        HardwareProfile.ForecourtToneFrequency = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "forecourtToneLength":
                                        HardwareProfile.ForecourtToneLength = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "forecourtToneRepeats":
                                        HardwareProfile.ForecourtToneRepeats = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "forecourtSuspendAllowed":
                                        HardwareProfile.ForecourtSuspendAllowed = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "hostname":
                                        HardwareProfile.Hostname = profileElem.Value;
                                        break;
                                    case "eftBatchIncrementAtEOS":
                                        HardwareProfile.EftBatchIncrementAtEOS = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "drawerDescription":
                                        HardwareProfile.DrawerDescription = profileElem.Value;
                                        break;
                                    case "drawerOpenText":
                                        HardwareProfile.DrawerOpenText = profileElem.Value;
                                        break;
                                    case "displayDeviceDescription":
                                        HardwareProfile.DisplayDeviceDescription = profileElem.Value;
                                        break;
                                    case "msrDeviceDescription":
                                        HardwareProfile.MsrDeviceDescription = profileElem.Value;
                                        break;
                                    case "scannerDeviceDescription":
                                        HardwareProfile.ScannerDeviceDescription = profileElem.Value;
                                        break;
                                    case "scaleManualInputAllowed":
                                        HardwareProfile.ScaleManualInputAllowed = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "scaleDeviceDescription":
                                        HardwareProfile.ScaleDeviceDescription = profileElem.Value;
                                        break;
                                    case "keyLockDeviceDescription":
                                        HardwareProfile.KeyLockDeviceDescription = profileElem.Value;
                                        break;
                                    case "rfIDScannerDeviceDescription":
                                        HardwareProfile.RfIDScannerDeviceDescription = profileElem.Value;
                                        break;
                                    case "cashChangerConnected":
                                        HardwareProfile.CashChangerConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "cashChangerDeviceType":
                                        HardwareProfile.CashChangerDeviceType = (HardwareProfile.CashChangerDeviceTypes)Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "cashChangerPortSettings":
                                        HardwareProfile.CashChangerPortSettings = profileElem.Value;
                                        break;
                                    case "cashChangerInitSettings":
                                        HardwareProfile.CashChangerInitSettings = profileElem.Value;
                                        break;
                                    case "dualDisplayBrowserUrl":
                                        HardwareProfile.DualDisplayBrowserUrl = profileElem.Value;
                                        break;
                                    case "dualDisplayConnected":
                                        HardwareProfile.DualDisplayConnected = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "dualDisplayDeviceName":
                                        HardwareProfile.DualDisplayDeviceName = profileElem.Value;
                                        break;
                                    case "dualDisplayDescription":
                                        HardwareProfile.DualDisplayDescription = profileElem.Value;
                                        break;
                                    case "dualDisplayType":
                                        HardwareProfile.DualDisplayType = (HardwareProfile.DisplayType)Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "dualDisplayPort":
                                        HardwareProfile.DualDisplayPort = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "dualDisplayReceiptPrecentage":
                                        HardwareProfile.DualDisplayReceiptPrecentage = Convert.ToDecimal(profileElem.Value);
                                        break;
                                    case "dualdisplayImagePath":
                                        HardwareProfile.DualdisplayImagePath = profileElem.Value;
                                        break;
                                    case "dualDisplayImageInterval":
                                        HardwareProfile.DualDisplayImageInterval = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "fCMScreenHeightPercentage":
                                        HardwareProfile.FCMScreenHeightPercentage = Convert.ToDecimal(profileElem.Value);
                                        break;
                                    case "fCMScreenExtHeightPercentage":
                                        HardwareProfile.FCMScreenExtHeightPercentage = Convert.ToDecimal(profileElem.Value);
                                        break;
                                    case "fCMControllerHostName":
                                        HardwareProfile.FCMControllerHostName = profileElem.Value;
                                        break;
                                    case "fCMImplFileName":
                                        HardwareProfile.FCMImplFileName = profileElem.Value;
                                        break;
                                    case "fCMImplFileType":
                                        HardwareProfile.FCMImplFileType = profileElem.Value;
                                        break;
                                    case "fCMVolumeUnitID":
                                        HardwareProfile.FCMVolumeUnitID = profileElem.Value;
                                        break;
                                    case "fCMVolumeUnitDescription":
                                        HardwareProfile.FCMVolumeUnitDescription = profileElem.Value;
                                        break;
                                    case "fCMServer":
                                        HardwareProfile.FCMServer = profileElem.Value;
                                        break;
                                    case "fCMPort":
                                        HardwareProfile.FCMPort = profileElem.Value;
                                        break;
                                    case "fCMPosPort":
                                        HardwareProfile.FCMPosPort = profileElem.Value;
                                        break;
                                    case "fCMLogLevel":
                                        HardwareProfile.FCMLogLevel = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "fCMFuellingPointColumns":
                                        HardwareProfile.FCMFuellingPointColumns = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "fCMCallingSound":
                                        HardwareProfile.FCMCallingSound = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "fCMCallingBlink":
                                        HardwareProfile.FCMCallingBlink = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "fCMActive":
                                        HardwareProfile.FCMActive = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "fiscalPrinter":
                                        HardwareProfile.FiscalPrinter = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "FiscalPrinterConnection":
                                        HardwareProfile.FiscalPrinterConnection = profileElem.Value;
                                        break;
                                    case "fiscalPrinterDescription":
                                        HardwareProfile.FiscalPrinterDescription = profileElem.Value;
                                        break;
                                    case "keyboardmappingID":
                                        HardwareProfile.KeyboardmappingID = profileElem.Value;
                                        break;
                                    case "pharmacyActive":
                                        HardwareProfile.PharmacyActive = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "pharmacyHost":
                                        HardwareProfile.PharmacyHost = profileElem.Value;
                                        break;
                                    case "pharmacyPort":
                                        HardwareProfile.PharmacyPort = Convert.ToInt32(profileElem.Value);
                                        break;
                                    case "useKitchenDisplay":
                                        HardwareProfile.UseKitchenDisplay = Convert.ToBoolean(profileElem.Value);
                                        break;
                                    case "dualDisplayScreen":
                                        HardwareProfile.DualDisplayScreen = (HardwareProfile.DualDisplayScreens)Convert.ToInt32(profileElem.Value);
                                        break;
                                                   
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, profileElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the Peripherals class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
            XElement peripherals = null;
            if (ID.StringValue == string.Empty)
            {
                peripherals = new XElement("PosPeripherals",
                    new XElement("barcodeReader", BarcodeReader),
                    new XElement("cardReader", CardReader),
                    new XElement("cashChanger", CashChanger),
                    new XElement("creditCardService", CreditCardService),
                    new XElement("drawer", Drawer),
                    new XElement("dualDisplay", DualDisplay),
                    new XElement("fiscalPrinter", FiscalPrinter),
                    new XElement("key", Key),
                    new XElement("lineDisplay", LineDisplay),
                    new XElement("printer", Printer),
                    new XElement("rFIDReader", RFIDReader),
                    new XElement("scale", Scale)
                );
            }
            if (HardwareProfile.ID != RecordIdentifier.Empty)
            {
                peripherals = new XElement("hardwareProfile",
                    new XElement("profileID", HardwareProfile.ID),
                    new XElement("name", HardwareProfile.Text),
                    new XElement("drawerDeviceName", HardwareProfile.DrawerDeviceName),
                    new XElement("drawerConnected",  HardwareProfile. DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS),
                    new XElement("displayDevice", (int) HardwareProfile.LineDisplayDeviceType),
                    new XElement("displayDeviceName", HardwareProfile. DisplayDeviceName),
                    new XElement("displayTotalText", HardwareProfile. DisplayTotalText),
                    new XElement("displayBalanceText", HardwareProfile. DisplayBalanceText),
                    new XElement("displayClosedLine1", HardwareProfile. DisplayClosedLine1),
                    new XElement("displayClosedLine2", HardwareProfile. DisplayClosedLine2),
                    new XElement("displayCharacterSet", HardwareProfile. DisplayCharacterSet),
                    new XElement("displayBinaryConversion", HardwareProfile.DisplayBinaryConversion),
                    new XElement("MsrDeviceType", HardwareProfile.MsrDeviceType),
                    new XElement("msrDeviceName", HardwareProfile. MsrDeviceName),
                    new XElement("startTrack1", HardwareProfile. StartTrack1),
                    new XElement("separator1", HardwareProfile. Separator1),
                    new XElement("endTrack1", HardwareProfile. EndTrack1),
                    new XElement("printer", (int)HardwareProfile.Printer),
                    new XElement("printerDeviceName", HardwareProfile. PrinterDeviceName),
                    new XElement("printerDeviceDescription", HardwareProfile. PrinterDeviceDescription),
                    new XElement("printBinaryConversion", HardwareProfile. PrintBinaryConversion),
                    new XElement("printerCharacterSet", HardwareProfile. PrinterCharacterSet),
                    new XElement("scannerConnected", HardwareProfile. ScannerConnected),
                    new XElement("scannerDeviceName", HardwareProfile. ScannerDeviceName),
                    new XElement("eTaxConnected", HardwareProfile.ETaxConnected),
                    new XElement("eTaxPortName", HardwareProfile.ETaxPortName),
                    new XElement("rFIDScannerConnected", HardwareProfile. RFIDScannerConnected),
                    new XElement("rFIDScannerDeviceName", HardwareProfile. RFIDScannerDeviceName),
                    new XElement("scaleConnected", HardwareProfile. ScaleConnected),
                    new XElement("scaleDeviceName", HardwareProfile. ScaleDeviceName),
                    new XElement("keyLockConnected", HardwareProfile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS),
                    new XElement("keyLockDeviceName", HardwareProfile. KeyLockDeviceName),
                    new XElement("eftConnected", HardwareProfile. EftConnected),
                    new XElement("eftDescription", HardwareProfile. EftDescription),
                    new XElement("eftServerName", HardwareProfile. EftServerName),
                    new XElement("eftServerPort", HardwareProfile. EftServerPort),
                    new XElement("eftCompanyID", HardwareProfile. EftCompanyID),
                    new XElement("eftUserID", HardwareProfile. EftUserID),
                    new XElement("eftPassword", HardwareProfile. EftPassword),
                    new XElement("cctvConnected", HardwareProfile. CctvConnected),
                    new XElement("cctvHostname", HardwareProfile. CctvHostname),
                    new XElement("cctvPort", HardwareProfile. CctvPort),
                    new XElement("cctvCamera", HardwareProfile. CctvCamera),
                    new XElement("forecourt", HardwareProfile. Forecourt),
                    new XElement("forecourtPriceDecimalPointPosition", HardwareProfile. ForecourtPriceDecimalPointPosition),
                    new XElement("forecourtToneActive", HardwareProfile. ForecourtToneActive),
                    new XElement("forecourtToneFrequency", HardwareProfile. ForecourtToneFrequency),
                    new XElement("forecourtToneLength", HardwareProfile. ForecourtToneLength),
                    new XElement("forecourtToneRepeats", HardwareProfile. ForecourtToneRepeats),
                    new XElement("forecourtSuspendAllowed", HardwareProfile. ForecourtSuspendAllowed),
                    new XElement("hostname", HardwareProfile. Hostname),
                    new XElement("eftBatchIncrementAtEOS", HardwareProfile. EftBatchIncrementAtEOS),
                    new XElement("drawerDescription", HardwareProfile. DrawerDescription),
                    new XElement("drawerOpenText", HardwareProfile. DrawerOpenText),
                    new XElement("displayDeviceDescription", HardwareProfile. DisplayDeviceDescription),
                    new XElement("msrDeviceDescription", HardwareProfile. MsrDeviceDescription),
                    new XElement("scannerDeviceDescription", HardwareProfile. ScannerDeviceDescription),
                    new XElement("scaleManualInputAllowed", HardwareProfile. ScaleManualInputAllowed),
                    new XElement("scaleDeviceDescription", HardwareProfile. ScaleDeviceDescription),
                    new XElement("keyLockDeviceDescription", HardwareProfile. KeyLockDeviceDescription),
                    new XElement("rfIDScannerDeviceDescription", HardwareProfile. RfIDScannerDeviceDescription),
                    new XElement("cashChangerConnected", HardwareProfile. CashChangerConnected),
                    new XElement("cashChangerDeviceType", (int)HardwareProfile.CashChangerDeviceType),
                    new XElement("cashChangerPortSettings", HardwareProfile. CashChangerPortSettings),
                    new XElement("cashChangerInitSettings", HardwareProfile. CashChangerInitSettings),
                    new XElement("dualDisplayBrowserUrl", HardwareProfile. DualDisplayBrowserUrl),
                    new XElement("dualDisplayConnected", HardwareProfile. DualDisplayConnected),
                    new XElement("dualDisplayDeviceName", HardwareProfile. DualDisplayDeviceName),
                    new XElement("dualDisplayDescription", HardwareProfile. DualDisplayDescription),
                    new XElement("dualDisplayType", (int)HardwareProfile.DualDisplayType),
                    new XElement("dualDisplayPort", HardwareProfile. DualDisplayPort),
                    new XElement("dualDisplayReceiptPrecentage", HardwareProfile. DualDisplayReceiptPrecentage),
                    new XElement("dualdisplayImagePath", HardwareProfile. DualdisplayImagePath),
                    new XElement("dualDisplayImageInterval", HardwareProfile. DualDisplayImageInterval),
                    new XElement("fCMScreenHeightPercentage", HardwareProfile. FCMScreenHeightPercentage),
                    new XElement("fCMScreenExtHeightPercentage", HardwareProfile. FCMScreenExtHeightPercentage),
                    new XElement("fCMControllerHostName", HardwareProfile. FCMControllerHostName),
                    new XElement("fCMImplFileName", HardwareProfile. FCMImplFileName),
                    new XElement("fCMImplFileType", HardwareProfile. FCMImplFileType),
                    new XElement("fCMVolumeUnitID", (string)HardwareProfile.FCMVolumeUnitID),
                    new XElement("fCMVolumeUnitDescription", HardwareProfile. FCMVolumeUnitDescription),
                    new XElement("fCMServer", HardwareProfile. FCMServer),
                    new XElement("fCMPort", HardwareProfile. FCMPort),
                    new XElement("fCMPosPort", HardwareProfile. FCMPosPort),
                    new XElement("fCMLogLevel", HardwareProfile. FCMLogLevel),
                    new XElement("fCMFuellingPointColumns", HardwareProfile. FCMFuellingPointColumns),
                    new XElement("fCMCallingSound", HardwareProfile. FCMCallingSound),
                    new XElement("fCMCallingBlink", HardwareProfile. FCMCallingBlink),
                    new XElement("fCMActive", HardwareProfile. FCMActive),
                    new XElement("fiscalPrinter", HardwareProfile.FiscalPrinter),
                    new XElement("fiscalPrinterConnection", HardwareProfile.FiscalPrinterConnection),
                    new XElement("fiscalPrinterDescription", HardwareProfile. FiscalPrinterDescription),
                    new XElement("keyboardmappingID", (string)HardwareProfile.KeyboardmappingID),
                    new XElement("pharmacyActive", HardwareProfile. PharmacyActive),
                    new XElement("pharmacyHost", HardwareProfile. PharmacyHost),
                    new XElement("pharmacyPort", HardwareProfile. PharmacyPort),
                    new XElement("useKitchenDisplay", HardwareProfile. UseKitchenDisplay),
                    new XElement("dualDisplayScreen", (int)HardwareProfile.DualDisplayScreen),
                    new XElement("drawerDeviceType", (int)HardwareProfile.DrawerDeviceType),
                    new XElement("keyLockDeviceType", (int)HardwareProfile.KeyLockDeviceType),
                    new XElement("lineDisplayDeviceType", (int)HardwareProfile.LineDisplayDeviceType)
                    );
            }

            return peripherals;
        }
    }
}
