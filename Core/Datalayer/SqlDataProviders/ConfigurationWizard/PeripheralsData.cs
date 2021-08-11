using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for peripherals and hardware profile.
    /// </summary>
    public class PeripheralsData : SqlServerDataProviderBase, IPeripheralsData
    {
        /// <summary>
        /// Get peripheral list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>peripheral list</returns>
        public virtual Peripherals GetPeripheralList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, DATAAREAID,
                                    ISNULL(DRAWER, 0) as DRAWER, 
                                    ISNULL(LINEDISPLAY, 0) as LINEDISPLAY, 
                                    ISNULL(DUALDISPLAY, 0) as DUALDISPLAY, 
                                    ISNULL(CARDREADER, 0) as CARDREADER, 
                                    ISNULL(PRINTER, 0) as PRINTER, 
                                    ISNULL(BARCODE, 0) as BARCODE, 
                                    ISNULL(SCALE, 0) as SCALE, 
                                    ISNULL(EFT, 0) as EFT, 
                                    ISNULL(RFID, 0) as RFID, 
                                    ISNULL(CASHCHANGER, 0) as CASHCHANGER, 
                                    ISNULL(FISCALPRINTER, 0) as FISCALPRINTER, 
                                    ISNULL(KEYLOCK, 0) as KEYLOCK 
                                    ISNULL(ETAX, 0) as ETAX 
                                    FROM WIZARDTEMPLATEPERIPHERALS 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                var result = Execute<Peripherals>(entry, cmd, CommandType.Text, null, PopulatePeripheralItems);

                return (result.Count > 0) ? result[0] : null;
            }
        }

        private static void PopulatePeripheralItems(IConnectionManager entry, IDataReader dr, Peripherals periperals, object obj)
        {
            periperals.BarcodeReader = Convert.ToByte(dr["Barcode"]);
            periperals.CardReader = Convert.ToByte(dr["CardReader"]);
            periperals.CashChanger = Convert.ToByte(dr["CashChanger"]);
            periperals.CreditCardService = Convert.ToByte(dr["EFT"]);
            periperals.Drawer = Convert.ToByte(dr["Drawer"]);
            periperals.DualDisplay = Convert.ToByte(dr["DualDisplay"]);
            periperals.FiscalPrinter = Convert.ToByte(dr["FiscalPrinter"]);
            periperals.Key = Convert.ToByte(dr["KEYLOCK"]);
            periperals.LineDisplay = Convert.ToByte(dr["LineDisplay"]);
            periperals.Printer = Convert.ToByte(dr["Printer"]);
            periperals.RFIDReader = Convert.ToByte(dr["RFID"]);
            periperals.Scale = Convert.ToByte(dr["Scale"]);
            periperals.ETax = Convert.ToByte(dr["ETAX"]);
        }

        /// <summary>
        /// Method to provide command text to SavePeripherals method.
        /// </summary>
        /// <param name="entry">for DataAreaId</param>
        /// <param name="peripherals">Peripherals list</param>
        /// <returns>Command text</returns>
        public virtual void Save(IConnectionManager entry, Peripherals peripherals)
        {
            var statement = new SqlServerStatement("WIZARDTEMPLATEPERIPHERALS");

            ValidateSecurity(entry, BusinessObjects.Permission.ConfigurationWizardEdit);

            peripherals.Validate();

            if (!Exists(entry, peripherals.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)peripherals.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)peripherals.ID);
            }

            statement.AddField("BARCODE", peripherals.BarcodeReader, SqlDbType.TinyInt);
            statement.AddField("CARDREADER", peripherals.CardReader, SqlDbType.TinyInt);
            statement.AddField("EFT", peripherals.CreditCardService, SqlDbType.TinyInt);
            statement.AddField("CASHCHANGER", peripherals.CashChanger, SqlDbType.TinyInt);
            statement.AddField("DRAWER", peripherals.Drawer, SqlDbType.TinyInt);
            statement.AddField("DUALDISPLAY", peripherals.DualDisplay, SqlDbType.TinyInt);
            statement.AddField("FISCALPRINTER", peripherals.FiscalPrinter, SqlDbType.TinyInt);
            statement.AddField("KEYLOCK", peripherals.Key, SqlDbType.TinyInt);
            statement.AddField("LINEDISPLAY", peripherals.LineDisplay, SqlDbType.TinyInt);
            statement.AddField("PRINTER", peripherals.Printer, SqlDbType.TinyInt);
            statement.AddField("RFID", peripherals.RFIDReader, SqlDbType.TinyInt);
            statement.AddField("SCALE", peripherals.Scale, SqlDbType.TinyInt);
            statement.AddField("ETAX", peripherals.ETax, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        /// <returns>void</returns>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATEPERIPHERALS")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.HardwareProfileEdit);
        }

        /// <summary>
        /// Returns true if the id already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">Id of Template</param>
        /// <returns>boolean result</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "WIZARDTEMPLATEPERIPHERALS", "ID", id);
        }

        /// <summary>
        /// Get selected hardware profile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">profileId</param>
        /// <param name="cacheType">cacheType</param>
        /// <returns>selected hardware profile</returns>
        public virtual HardwareProfile GetSelectedProfile(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                       "select PROFILEID, ISNULL(NAME,'') as NAME," +
                       "ISNULL(DRAWER,0) as DRAWER,ISNULL(DRAWERDEVICENAME,'') as DRAWERDEVICENAME," +
                       "ISNULL(DRAWERDESCRIPTION,'') as DRAWERDESCRIPTION,ISNULL(DRAWEROPENTEXT,'') as DRAWEROPENTEXT," +
                       "ISNULL(DISPLAYDEVICE,0) as DISPLAYDEVICE,ISNULL(DISPLAYDEVICENAME,'') as DISPLAYDEVICENAME," +
                       "ISNULL(DISPLAYDESCRIPTION,'') as DISPLAYDESCRIPTION,ISNULL(DISPLAYTOTALTEXT, '') as DISPLAYTOTALTEXT,ISNULL(DISPLAYBALANCETEXT,'') as DISPLAYBALANCETEXT," +
                       "ISNULL(DISPLAYCLOSEDLINE1,'') as DISPLAYCLOSEDLINE1,ISNULL(DISPLAYCLOSEDLINE2,'') as DISPLAYCLOSEDLINE2,ISNULL(DISPLAYCHARACTERSET,0) as DISPLAYCHARACTERSET, ISNULL(DISPLAYBINARYCONVERSION, 0) AS DISPLAYBINARYCONVERSION," +
                       "ISNULL(MSR,0) as MSR,ISNULL(MSRDEVICENAME,'') as MSRDEVICENAME,ISNULL(MSRDESCRIPTION,'') as MSRDESCRIPTION," +
                       "ISNULL(STARTTRACK1,'') as STARTTRACK1,ISNULL(SEPARATOR1,'') as SEPARATOR1,ISNULL(ENDTRACK1,'') as ENDTRACK1," +
                       "ISNULL(PRINTER,0) as PRINTER,ISNULL(PRINTERDEVICENAME,'') as PRINTERDEVICENAME," +
                       "ISNULL(PRINTERDESCRIPTION,'') as PRINTERDESCRIPTION,ISNULL(PRINTBINARYCONVERSION,0) as PRINTBINARYCONVERSION,ISNULL(PRINTERCHARACTERSET,0) as PRINTERCHARACTERSET," +
                       "ISNULL(SCANNER,0) as SCANNER,ISNULL(SCANNERDEVICENAME,'') as SCANNERDEVICENAME,ISNULL(SCANNERDESCRIPTION,'') as SCANNERDESCRIPTION," +
                       "ISNULL(ETAX,0) as ETAX,ISNULL(ETAXPORTNAME,'') as ETAXPORTNAME," +
                       "ISNULL(SCALE,0) as SCALE,ISNULL(SCALEDEVICENAME,'') as SCALEDEVICENAME," +
                       "ISNULL(SCALEDESCRIPTION,'') as SCALEDESCRIPTION,ISNULL(MANUALINPUTALLOWED,0) as MANUALINPUTALLOWED," +
                       "ISNULL(KEYLOCK,0) as KEYLOCK,ISNULL(KEYLOCKDEVICENAME,'') as KEYLOCKDEVICENAME,ISNULL(KEYLOCKDESCRIPTION,'') as KEYLOCKDESCRIPTION," +
                       "ISNULL(EFT,0) as EFT,ISNULL(EFTSERVERNAME,'') as EFTSERVERNAME,ISNULL(EFTDESCRIPTION,'') as EFTDESCRIPTION," +
                       "ISNULL(EFTSERVERPORT,'0') as EFTSERVERPORT,ISNULL(EFTCOMPANYID,'') as EFTCOMPANYID," +
                       "ISNULL(EFTUSERID,'') as EFTUSERID,ISNULL(EFTPASSWORD,'') as EFTPASSWORD,ISNULL(EFTBATCHINCREMENTATEOS,0) as EFTBATCHINCREMENTATEOS," +
                       "ISNULL(EFTMERCHANTACCOUNT,'') AS EFTMERCHANTACCOUNT,ISNULL(EFTMERCHANTKEY,'') AS EFTMERCHANTKEY,ISNULL(EFTCUSTOMFIELD1,'') AS EFTCUSTOMFIELD1,ISNULL(EFTCUSTOMFIELD2,'') AS EFTCUSTOMFIELD2," +
                       "ISNULL(CCTV,0) as CCTV,ISNULL(CCTVCAMERA,'') as CCTVCAMERA,ISNULL(CCTVHOSTNAME,'') as CCTVHOSTNAME,ISNULL(CCTVPORT,0) as CCTVPORT," +
                       "ISNULL(FORECOURT,0) as FORECOURT,ISNULL(HOSTNAME,'') as HOSTNAME," +
                       "ISNULL(RFIDSCANNERTYPE,0) as RFIDSCANNERTYPE,ISNULL(RFIDDEVICENAME,'') as RFIDDEVICENAME,ISNULL(RFIDDESCRIPTION,'') as RFIDDESCRIPTION," +
                       "ISNULL(CASHCHANGER,0) as CASHCHANGER,ISNULL(CASHCHANGERPORTSETTINGS,'') as CASHCHANGERPORTSETTINGS, ISNULL(CASHCHANGERINITSETTINGS,'') as CASHCHANGERINITSETTINGS," +
                       "ISNULL(DUALDISPLAY,0) as DUALDISPLAY,ISNULL(DUALDISPLAYDEVICENAME,'') as DUALDISPLAYDEVICENAME," +
                       "ISNULL(DUALDISPLAYDESCRIPTION,'') as DUALDISPLAYDESCRIPTION,ISNULL(DUALDISPLAYTYPE,0) as DUALDISPLAYTYPE," +
                       "ISNULL(DUALDISPLAYPORT,'0') as DUALDISPLAYPORT,ISNULL(DUALDISPLAYRECEIPTPERCENTAGE,0) as DUALDISPLAYRECEIPTPERCENTAGE," +
                       "ISNULL(DUALDISPLAYIMAGEPATH,'') as DUALDISPLAYIMAGEPATH,ISNULL(DUALDISPLAYIMAGEINTERVAL,0) as DUALDISPLAYIMAGEINTERVAL," +
                       "ISNULL(DUALDISPLAYBROWSERURL,'') as DUALDISPLAYBROWSERURL, " +
                       "ISNULL(FORECOURTMANAGERScreenHeightPercentage,0) as FCMSCREENHEIGHTPERCENTAGE, " +
                       "ISNULL(FORECOURTMANAGERControllerHostName,'') as FCMCONTROLLERHOSTNAME, " +
                       "ISNULL(FORECOURTMANAGERLogLevel,0) as FCMLOGLEVEL, " +
                       "ISNULL(FORECOURTMANAGERImplFileName,'') as FCMIMPLFILENAME, " +
                       "ISNULL(FORECOURTMANAGERImplFileType,'') as FCMIMPLFILETYPE, " +
                       "ISNULL(FORECOURTMANAGERScreenExtHeightPercentage,0) as FCMSCREENEXTHEIGHTPERCENTAGE, " +
                       "ISNULL(FORECOURTMANAGERVolumeUnit,'') as FCMVOLUMEUNIT, " +
                       "ISNULL(FORECOURTMANAGERCallingSound,0) as FCMCALLINGSOUND, " +
                       "ISNULL(FORECOURTMANAGERCallingBlink,0) as FCMCALLINGBLINK, " +
                       "ISNULL(FORECOURTMANAGERFuellingPointColumns,0) as FCMFUELLINGPOINTCOLUMNS, " +
                       "ISNULL(FORECOURTMANAGER,0) as FCMACTIVE, " +
                       "ISNULL(FORECOURTMANAGERHOSTNAME,'') as FCMSERVER, " +
                       "ISNULL(FORECOURTMANAGERPORT,'') as FCMPORT, " +
                       "ISNULL(FORECOURTMANAGERPOSPORT,0) as FCMPOSPORT, " +
                       "ISNULL(FISCALPRINTER,0) as FISCALPRINTER, " + 
                       "ISNULL(FISCALPRINTERCONNECTION,'') as FISCALPRINTERCONNECTION, " +
                       "ISNULL(FISCALPRINTERDESCRIPTION,'') as FISCALPRINTERDESCRIPTION, " +
                       "ISNULL(u.TXT,'') as FCMVOLUMEUNITDESCRIPTION, " +
                       "ISNULL(KEYBOARDMAPPINGID,'') as KEYBOARDMAPPINGID, " +
                       "ISNULL(FORECOURTONEACTIVE,0) as FORECOURTONEACTIVE, " +
                       "ISNULL(FORECOURTONEFREQUENCY,0) as FORECOURTONEFREQUENCY, " +
                       "ISNULL(FORECOURTONELENGTH,0) as FORECOURTONELENGTH, " +
                       "ISNULL(FORECOURTONEREPEATS,0) as FORECOURTONEREPEATS, " +
                       "ISNULL(FORECOURTSUSPENDALLOWED,0) as FORECOURTSUSPENDALLOWED, " +
                       "ISNULL(PHARMACY,0) as PHARMACY, " +
                       "ISNULL(PHARMACYHOST,'') as PHARMACYHOST, " +
                       "ISNULL(PHARMACYPORT,0) as PHARMACYPORT, " +
                       "ISNULL(FORECOURTPRICEDECPOINTPOSITION,0) as FORECOURTPRICEDECPOINTPOSITION, " +
                       "ISNULL(USEKITCHENDISPLAY,0) as USEKITCHENDISPLAY," +
                       "ISNULL(DUALDISPLAYSCREENNUMBER, 3) AS DUALDISPLAYSCREENNUMBER " +
                       "from POSHARDWAREPROFILE p " +
                       "Left outer Join UNIT u on u.UNITID = p.FORECOURTMANAGERVolumeUnit and u.DATAAREAID = p.DATAAREAID " +
                       "where p.DATAAREAID = @DATAAREAID and p.PROFILEID = @ID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);
                return Get<HardwareProfile>(entry, cmd, id, PopulateProfile, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Data populator for PopulateProfile function
        /// </summary>
        /// <param name="dr">data reader object</param>
        /// <param name="profile">object of HardwareProfile entity</param>
        private static void PopulateProfile(IDataReader dr, HardwareProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];

            profile.DrawerDeviceType = ((int)dr["DRAWER"] != 0) ? HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.DrawerDeviceName = (string)dr["DRAWERDEVICENAME"];
            profile.DrawerDescription = (string)dr["DRAWERDESCRIPTION"];
            profile.DrawerOpenText = (string)dr["DRAWEROPENTEXT"];

            profile.LineDisplayDeviceType = (HardwareProfile.LineDisplayDeviceTypes) (int)dr["DISPLAYDEVICE"];
            profile.DisplayDeviceName = (string)dr["DISPLAYDEVICENAME"];
            profile.DisplayDeviceDescription = (string)dr["DISPLAYDESCRIPTION"];
            profile.DisplayTotalText = (string)dr["DISPLAYTOTALTEXT"];
            profile.DisplayBalanceText = (string)dr["DISPLAYBALANCETEXT"];
            profile.DisplayClosedLine1 = (string)dr["DISPLAYCLOSEDLINE1"];
            profile.DisplayClosedLine2 = (string)dr["DISPLAYCLOSEDLINE2"];
            profile.DisplayCharacterSet = (int)dr["DISPLAYCHARACTERSET"];
            profile.DisplayBinaryConversion = ((byte) dr["DISPLAYBINARYCONVERSION"] != 0);

            profile.MsrDeviceType = (HardwareProfile.DeviceTypes)(int)dr["MSR"]; ;
            profile.MsrDeviceName = (string)dr["MSRDEVICENAME"];
            profile.MsrDeviceDescription = (string)dr["MSRDESCRIPTION"];
            profile.StartTrack1 = (string)dr["STARTTRACK1"];
            profile.Separator1 = (string)dr["SEPARATOR1"];
            profile.EndTrack1 = (string)dr["ENDTRACK1"];

            profile.Printer = (HardwareProfile.PrinterHardwareTypes)((int)dr["PRINTER"]);
            profile.PrinterDeviceName = (string)dr["PRINTERDEVICENAME"];
            profile.PrinterDeviceDescription = (string)dr["PRINTERDESCRIPTION"];
            profile.PrintBinaryConversion = ((byte)dr["PRINTBINARYCONVERSION"] != 0);
            profile.PrinterCharacterSet = (int)dr["PRINTERCHARACTERSET"];

            profile.ScannerConnected = ((int)dr["SCANNER"] != 0);
            profile.ScannerDeviceName = (string)dr["SCANNERDEVICENAME"];
            profile.ScannerDeviceDescription = (string)dr["SCANNERDESCRIPTION"];

            profile.ETaxConnected = ((int)dr["ETAX"] != 0);
            profile.ETaxPortName = (string)dr["ETAXPORTNAME"];

            profile.ScaleConnected = ((int)dr["SCALE"] != 0);
            profile.ScaleDeviceName = (string)dr["SCALEDEVICENAME"];
            profile.ScaleDeviceDescription = (string)dr["SCALEDESCRIPTION"];
            profile.ScaleManualInputAllowed = ((byte)dr["MANUALINPUTALLOWED"] != 0);

            profile.KeyLockDeviceType = ((int)dr["KEYLOCK"] != 0)?HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.KeyLockDeviceName = (string)dr["KEYLOCKDEVICENAME"];
            profile.KeyLockDeviceDescription = (string)dr["KEYLOCKDESCRIPTION"];

            profile.EftConnected = ((int)dr["EFT"] != 0);
            profile.EftServerName = (string)dr["EFTSERVERNAME"];
            profile.EftDescription = (string)dr["EFTDESCRIPTION"];

            profile.FCMActive = ((byte)dr["FCMACTIVE"] != 0);
            profile.FCMCallingBlink = ((byte)dr["FCMCALLINGBLINK"] != 0);
            profile.FCMCallingSound = ((byte)dr["FCMCALLINGSOUND"] != 0);
            profile.FCMServer = (string)dr["FCMSERVER"];
            profile.FCMPort = (string)dr["FCMPORT"];
            profile.FCMPosPort = (string)dr["FCMPOSPORT"];
            profile.FCMControllerHostName = (string)dr["FCMCONTROLLERHOSTNAME"];
            profile.FCMImplFileName = (string)dr["FCMIMPLFILENAME"];
            profile.FCMImplFileType = (string)dr["FCMIMPLFILETYPE"];
            profile.FCMFuellingPointColumns = (int)dr["FCMFUELLINGPOINTCOLUMNS"];
            profile.FCMLogLevel = (int)dr["FCMLOGLEVEL"];
            profile.FCMScreenExtHeightPercentage = (decimal)dr["FCMSCREENEXTHEIGHTPERCENTAGE"];
            profile.FCMScreenHeightPercentage = (decimal)dr["FCMSCREENHEIGHTPERCENTAGE"];
            profile.FCMVolumeUnitID = (string)dr["FCMVOLUMEUNIT"];
            profile.FCMVolumeUnitDescription = (string)dr["FCMVOLUMEUNITDESCRIPTION"];

            try
            {
                profile.EftServerPort = Convert.ToInt32((string)dr["EFTSERVERPORT"]);
            }
            catch (Exception)
            {
                profile.EftServerPort = 0;
            }

            profile.EftCompanyID = (string)dr["EFTCOMPANYID"];
            profile.EftUserID = (string)dr["EFTUSERID"];
            profile.EftPassword = (string)dr["EFTPASSWORD"];
            profile.EftMerchantAccount = (string)dr["EFTMERCHANTACCOUNT"];
            profile.EftMerchantKey = (string)dr["EFTMERCHANTKEY"];
            profile.EftCustomField1 = (string)dr["EFTCUSTOMFIELD1"];
            profile.EftCustomField2 = (string)dr["EFTCUSTOMFIELD2"];
            profile.EftBatchIncrementAtEOS = ((byte)dr["EFTBATCHINCREMENTATEOS"] != 0);

            profile.CctvConnected = ((byte)dr["CCTV"] != 0);
            profile.CctvCamera = (string)dr["CCTVCAMERA"];
            profile.CctvHostname = (string)dr["CCTVHOSTNAME"];
            profile.CctvPort = (int)dr["CCTVPORT"];

            profile.Forecourt = ((int)dr["FORECOURT"] != 0);
            profile.Hostname = (string)dr["HOSTNAME"];
            profile.ForecourtPriceDecimalPointPosition = (int)dr["FORECOURTPRICEDECPOINTPOSITION"];
            profile.ForecourtToneActive = ((byte)dr["FORECOURTONEACTIVE"] != 0);
            profile.ForecourtToneFrequency = (int)dr["FORECOURTONEFREQUENCY"];
            profile.ForecourtToneLength = (int)dr["FORECOURTONELENGTH"];
            profile.ForecourtToneRepeats = (int)dr["FORECOURTONEREPEATS"];
            profile.ForecourtSuspendAllowed = ((byte)dr["FORECOURTSUSPENDALLOWED"] != 0);

            profile.PharmacyActive = ((byte)dr["PHARMACY"] != 0);
            profile.PharmacyHost = (string)dr["PHARMACYHOST"];
            profile.PharmacyPort = (int)dr["PHARMACYPORT"];

            profile.RFIDScannerConnected = ((int)dr["RFIDSCANNERTYPE"] != 0);
            profile.RFIDScannerDeviceName = (string)dr["RFIDDEVICENAME"];
            profile.RfIDScannerDeviceDescription = (string)dr["RFIDDESCRIPTION"];

            profile.CashChangerDeviceType = (HardwareProfile.CashChangerDeviceTypes)dr["CASHCHANGER"];
            profile.CashChangerConnected = ((int)profile.CashChangerDeviceType != 0);
            profile.CashChangerInitSettings = (string)dr["CASHCHANGERINITSETTINGS"];
            profile.CashChangerPortSettings = (string)dr["CASHCHANGERPORTSETTINGS"];

            profile.DualDisplayConnected = ((byte)dr["DUALDISPLAY"] != 0);
            profile.DualDisplayDeviceName = (string)dr["DUALDISPLAYDEVICENAME"];
            profile.DualDisplayDescription = (string)dr["DUALDISPLAYDESCRIPTION"];
            profile.DualDisplayType = (HardwareProfile.DisplayType)dr["DUALDISPLAYTYPE"];

            try
            {
                profile.DualDisplayPort = Convert.ToInt32((string)dr["DUALDISPLAYPORT"]);
            }
            catch (Exception)
            {
                profile.DualDisplayPort = 0;
            }

            profile.DualDisplayReceiptPrecentage = (decimal)dr["DUALDISPLAYRECEIPTPERCENTAGE"];
            profile.DualdisplayImagePath = (string)dr["DUALDISPLAYIMAGEPATH"];
            profile.DualDisplayImageInterval = (int)dr["DUALDISPLAYIMAGEINTERVAL"];
            profile.DualDisplayBrowserUrl = (string)dr["DUALDISPLAYBROWSERURL"];

            profile.FiscalPrinter = (int)dr["FISCALPRINTER"];
            profile.FiscalPrinterConnection = (string)dr["FISCALPRINTERCONNECTION"];
            profile.FiscalPrinterDescription = (string)dr["FISCALPRINTERDESCRIPTION"];

            profile.KeyboardmappingID = (string)dr["KEYBOARDMAPPINGID"];
            profile.UseKitchenDisplay = ((byte)dr["USEKITCHENDISPLAY"] != 0);
            profile.DualDisplayScreen = (HardwareProfile.DualDisplayScreens)(int)dr["DUALDISPLAYSCREENNUMBER"];
        }

    }
}
