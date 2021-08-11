using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Security;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class HardwareProfileData : SqlServerDataProviderBase, IHardwareProfileData
    {
        private string BaseSQL =
            @"select PROFILEID, ISNULL(NAME,'') as NAME,
            ISNULL(DRAWER,0) as DRAWER,ISNULL(DRAWERDEVICENAME,'') as DRAWERDEVICENAME,
            ISNULL(DRAWERDESCRIPTION,'') as DRAWERDESCRIPTION,ISNULL(DRAWEROPENTEXT,'') as DRAWEROPENTEXT,
            ISNULL(DISPLAYDEVICE,0) as DISPLAYDEVICE,ISNULL(DISPLAYDEVICENAME,'') as DISPLAYDEVICENAME,
            ISNULL(DISPLAYDESCRIPTION,'') as DISPLAYDESCRIPTION,ISNULL(DISPLAYTOTALTEXT, '') as DISPLAYTOTALTEXT,ISNULL(DISPLAYBALANCETEXT,'') as DISPLAYBALANCETEXT,
            ISNULL(DISPLAYCLOSEDLINE1,'') as DISPLAYCLOSEDLINE1,ISNULL(DISPLAYCLOSEDLINE2,'') as DISPLAYCLOSEDLINE2,ISNULL(DISPLAYCHARACTERSET,0) as DISPLAYCHARACTERSET, ISNULL(DISPLAYBINARYCONVERSION, 0) AS DISPLAYBINARYCONVERSION,
            ISNULL(MSR,0) as MSR,ISNULL(MSRDEVICENAME,'') as MSRDEVICENAME,ISNULL(MSRDESCRIPTION,'') as MSRDESCRIPTION,
            ISNULL(STARTTRACK1,'') as STARTTRACK1,ISNULL(SEPARATOR1,'') as SEPARATOR1,ISNULL(ENDTRACK1,'') as ENDTRACK1,
            ISNULL(PRINTER,0) as PRINTER,ISNULL(PRINTERDEVICENAME,'') as PRINTERDEVICENAME,
            ISNULL(PRINTERDESCRIPTION,'') as PRINTERDESCRIPTION,ISNULL(PRINTBINARYCONVERSION,0) as PRINTBINARYCONVERSION,ISNULL(PRINTERCHARACTERSET,0) as PRINTERCHARACTERSET,ISNULL(PRINTEREXTRALINES,0) AS PRINTEREXTRALINES,
            ISNULL(SCANNER,0) as SCANNER,ISNULL(SCANNERDEVICENAME,'') as SCANNERDEVICENAME,ISNULL(SCANNERDESCRIPTION,'') as SCANNERDESCRIPTION,
            ISNULL(ETAX,0) as ETAX,ISNULL(ETAXPORTNAME,'') as ETAXPORTNAME,
            ISNULL(SCALE,0) as SCALE,ISNULL(SCALEDEVICENAME,'') as SCALEDEVICENAME,
            ISNULL(SCALEDESCRIPTION,'') as SCALEDESCRIPTION,ISNULL(MANUALINPUTALLOWED,0) as MANUALINPUTALLOWED,
            ISNULL(KEYLOCK,0) as KEYLOCK,ISNULL(KEYLOCKDEVICENAME,'') as KEYLOCKDEVICENAME,ISNULL(KEYLOCKDESCRIPTION,'') as KEYLOCKDESCRIPTION,
            ISNULL(EFT,0) as EFT,LSPAY,ISNULL(EFTSERVERNAME,'') as EFTSERVERNAME,ISNULL(EFTDESCRIPTION,'') as EFTDESCRIPTION,
            ISNULL(EFTSERVERPORT,'0') as EFTSERVERPORT,ISNULL(EFTCOMPANYID,'') as EFTCOMPANYID,
            ISNULL(EFTUSERID,'') as EFTUSERID,ISNULL(EFTPASSWORD,'') as EFTPASSWORD,ISNULL(EFTBATCHINCREMENTATEOS,0) as EFTBATCHINCREMENTATEOS,
            ISNULL(EFTMERCHANTACCOUNT,'') AS EFTMERCHANTACCOUNT,ISNULL(EFTMERCHANTKEY,'') AS EFTMERCHANTKEY,ISNULL(EFTCUSTOMFIELD1,'') AS EFTCUSTOMFIELD1,ISNULL(EFTCUSTOMFIELD2,'') AS EFTCUSTOMFIELD2,
            ISNULL(CCTV,0) as CCTV,ISNULL(CCTVCAMERA,'') as CCTVCAMERA,ISNULL(CCTVHOSTNAME,'') as CCTVHOSTNAME,ISNULL(CCTVPORT,0) as CCTVPORT,
            ISNULL(FORECOURT,0) as FORECOURT,ISNULL(HOSTNAME,'') as HOSTNAME,
            ISNULL(RFIDSCANNERTYPE,0) as RFIDSCANNERTYPE,ISNULL(RFIDDEVICENAME,'') as RFIDDEVICENAME,ISNULL(RFIDDESCRIPTION,'') as RFIDDESCRIPTION,
            ISNULL(CASHCHANGER,0) as CASHCHANGER,ISNULL(CASHCHANGERPORTSETTINGS,'') as CASHCHANGERPORTSETTINGS, ISNULL(CASHCHANGERINITSETTINGS,'') as CASHCHANGERINITSETTINGS,
            ISNULL(DUALDISPLAY,0) as DUALDISPLAY,ISNULL(DUALDISPLAYDEVICENAME,'') as DUALDISPLAYDEVICENAME,
            ISNULL(DUALDISPLAYDESCRIPTION,'') as DUALDISPLAYDESCRIPTION,ISNULL(DUALDISPLAYTYPE,0) as DUALDISPLAYTYPE,
            ISNULL(DUALDISPLAYPORT,'0') as DUALDISPLAYPORT,ISNULL(DUALDISPLAYRECEIPTPERCENTAGE,0) as DUALDISPLAYRECEIPTPERCENTAGE,
            ISNULL(DUALDISPLAYIMAGEPATH,'') as DUALDISPLAYIMAGEPATH,ISNULL(DUALDISPLAYIMAGEINTERVAL,0) as DUALDISPLAYIMAGEINTERVAL,
            ISNULL(DUALDISPLAYBROWSERURL,'') as DUALDISPLAYBROWSERURL, 
            ISNULL(FORECOURTMANAGERScreenHeightPercentage,0) as FCMSCREENHEIGHTPERCENTAGE, 
            ISNULL(FORECOURTMANAGERControllerHostName,'') as FCMCONTROLLERHOSTNAME, 
            ISNULL(FORECOURTMANAGERLogLevel,0) as FCMLOGLEVEL, 
            ISNULL(FORECOURTMANAGERImplFileName,'') as FCMIMPLFILENAME, 
            ISNULL(FORECOURTMANAGERImplFileType,'') as FCMIMPLFILETYPE, 
            ISNULL(FORECOURTMANAGERScreenExtHeightPercentage,0) as FCMSCREENEXTHEIGHTPERCENTAGE, 
            ISNULL(FORECOURTMANAGERVolumeUnit,'') as FCMVOLUMEUNIT, 
            ISNULL(FORECOURTMANAGERCallingSound,0) as FCMCALLINGSOUND, 
            ISNULL(FORECOURTMANAGERCallingBlink,0) as FCMCALLINGBLINK, 
            ISNULL(FORECOURTMANAGERFuellingPointColumns,0) as FCMFUELLINGPOINTCOLUMNS, 
            ISNULL(FORECOURTMANAGER,0) as FCMACTIVE, 
            ISNULL(FORECOURTMANAGERHOSTNAME,'') as FCMSERVER, 
            ISNULL(FORECOURTMANAGERPORT,'') as FCMPORT, 
            ISNULL(FORECOURTMANAGERPOSPORT,0) as FCMPOSPORT, 
            ISNULL(FISCALPRINTER,0) as FISCALPRINTER, 
            ISNULL(FISCALPRINTERCONNECTION,'') as FISCALPRINTERCONNECTION, 
            ISNULL(FISCALPRINTERDESCRIPTION,'') as FISCALPRINTERDESCRIPTION, 
            ISNULL(u.TXT,'') as FCMVOLUMEUNITDESCRIPTION, 
            ISNULL(KEYBOARDMAPPINGID,'') as KEYBOARDMAPPINGID, 
            ISNULL(FORECOURTONEACTIVE,0) as FORECOURTONEACTIVE, 
            ISNULL(FORECOURTONEFREQUENCY,0) as FORECOURTONEFREQUENCY, 
            ISNULL(FORECOURTONELENGTH,0) as FORECOURTONELENGTH, 
            ISNULL(FORECOURTONEREPEATS,0) as FORECOURTONEREPEATS, 
            ISNULL(FORECOURTSUSPENDALLOWED,0) as FORECOURTSUSPENDALLOWED, 
            ISNULL(PHARMACY,0) as PHARMACY, 
            ISNULL(PHARMACYHOST,'') as PHARMACYHOST, 
            ISNULL(PHARMACYPORT,0) as PHARMACYPORT, 
            ISNULL(FORECOURTPRICEDECPOINTPOSITION,0) as FORECOURTPRICEDECPOINTPOSITION, 
            ISNULL(USEKITCHENDISPLAY,0) as USEKITCHENDISPLAY,
            ISNULL(DUALDISPLAYSCREENNUMBER, 3) AS DUALDISPLAYSCREENNUMBER, 
            ISNULL(DALLASKEYCONNECTED, 0) AS DALLASKEYCONNECTED, 
            ISNULL(DALLASMESSAGEPREFIX, '') AS DALLASMESSAGEPREFIX, 
            ISNULL(DALLASKEYREMOVEDMESSAGE, '') AS DALLASKEYREMOVEDMESSAGE, 
            ISNULL(DALLASCOMPORT, '') AS DALLASCOMPORT, 
            ISNULL(DALLASBAUDRATE, 9600) AS DALLASBAUDRATE, 
            ISNULL(DALLASPARITY, 0) AS DALLASPARITY, 
            ISNULL(DALLASSTOPBITS, 1) AS DALLASSTOPBITS, 
            ISNULL(DALLASDATABITS, 8) AS DALLASDATABITS, 
	        CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE t WHERE t.HARDWAREPROFILE = p.PROFILEID)
	        	THEN 1
	        	ELSE 0
	        END AS BIT) AS PROFILEISUSED,
            ISNULL(STATIONPRINTINGHOSTID, '') as STATIONPRINTINGHOSTID,       
            ISNULL(WINDOWSPRINTERCONFIGURATIONID, '') as WINDOWSPRINTERCONFIGURATIONID       
            from POSHARDWAREPROFILE p 
            Left outer Join UNIT u on u.UNITID = p.FORECOURTMANAGERVolumeUnit and u.DATAAREAID = p.DATAAREAID ";

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, "NAME");
        }
        public virtual List<HardwareProfile> GetHardwareProfileList(IConnectionManager entry, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                @"SELECT PROFILEID, 
                         ISNULL(NAME,'') as NAME,
	                     CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE t WHERE t.HARDWAREPROFILE = php.PROFILEID)
	                     	THEN 1
	                     	ELSE 0
	                     END AS BIT) AS PROFILEISUSED
                       FROM POSHARDWAREPROFILE  php
                       WHERE DATAAREAID = @dataAreaId
                       ORDER BY " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<HardwareProfile>(entry, cmd, CommandType.Text, PopulateHardwareProfile);
            }
        }

        private static void PopulateHardwareProfile(IDataReader dr, HardwareProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
        }

        public virtual string GetName(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT ISNULL(NAME,'') AS NAME FROM POSHARDWAREPROFILE WHERE DATAAREAID = @dataAreaId AND PROFILEID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", ID);

                object returnValue = entry.Connection.ExecuteScalar(cmd);
                return returnValue as string ?? "";
            }
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSHARDWAREPROFILE", "NAME", "ProfileID", sort);
        }

        public virtual List<HardwareProfile> GetFullProfileList(IConnectionManager entry)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSQL +
                    " where p.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                
                return Execute<HardwareProfile>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        private static void PopulateProfile(IDataReader dr, HardwareProfile profile)
        {
            object unused = null;
            PopulateTempProfile(null, dr, profile, ref unused);

            profile.ID = (string) dr["PROFILEID"];
            profile.Text = (string) dr["NAME"];

            profile.DrawerDeviceType =  ((int) dr["DRAWER"] != 0)?HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.DrawerDeviceName = (string) dr["DRAWERDEVICENAME"];
            profile.DrawerDescription = (string) dr["DRAWERDESCRIPTION"];
            profile.DrawerOpenText = (string) dr["DRAWEROPENTEXT"];

            profile.LineDisplayDeviceType = (HardwareProfile.LineDisplayDeviceTypes) (int) dr["DISPLAYDEVICE"];
            profile.DisplayDeviceName = (string) dr["DISPLAYDEVICENAME"];
            profile.DisplayDeviceDescription = (string) dr["DISPLAYDESCRIPTION"];
            profile.DisplayTotalText = (string) dr["DISPLAYTOTALTEXT"];
            profile.DisplayBalanceText = (string) dr["DISPLAYBALANCETEXT"];
            profile.DisplayClosedLine1 = (string) dr["DISPLAYCLOSEDLINE1"];
            profile.DisplayClosedLine2 = (string) dr["DISPLAYCLOSEDLINE2"];
            profile.DisplayCharacterSet = (int) dr["DISPLAYCHARACTERSET"];
            profile.DisplayBinaryConversion = ((byte) dr["DISPLAYBINARYCONVERSION"] != 0);

            profile.MsrDeviceType = (HardwareProfile.DeviceTypes)(int)dr["MSR"];
            profile.MsrDeviceName = (string) dr["MSRDEVICENAME"];
            profile.MsrDeviceDescription = (string) dr["MSRDESCRIPTION"];
            profile.StartTrack1 = (string) dr["STARTTRACK1"];
            profile.Separator1 = (string) dr["SEPARATOR1"];
            profile.EndTrack1 = (string) dr["ENDTRACK1"];

            //Virtual printer is not available anymore so if the data is set to a Virtual printer
            //the new default is NONE
            profile.Printer = (HardwareProfile.PrinterHardwareTypes)((int)dr["PRINTER"]);

            profile.PrinterDeviceName = (string) dr["PRINTERDEVICENAME"];
            profile.PrinterDeviceDescription = (string) dr["PRINTERDESCRIPTION"];
            profile.PrintBinaryConversion = ((byte) dr["PRINTBINARYCONVERSION"] != 0);
            profile.PrinterCharacterSet = (int) dr["PRINTERCHARACTERSET"];
            profile.PrinterExtraLines = AsInt(dr["PRINTEREXTRALINES"]);

            profile.ScannerConnected = ((int) dr["SCANNER"] != 0);
            profile.ScannerDeviceName = (string) dr["SCANNERDEVICENAME"];
            profile.ScannerDeviceDescription = (string) dr["SCANNERDESCRIPTION"];

            profile.ETaxConnected = ((int)dr["ETAX"] != 0);
            profile.ETaxPortName = (string)dr["ETAXPORTNAME"];

            profile.ScaleConnected = ((int) dr["SCALE"] != 0);
            profile.ScaleDeviceName = (string) dr["SCALEDEVICENAME"];
            profile.ScaleDeviceDescription = (string) dr["SCALEDESCRIPTION"];
            profile.ScaleManualInputAllowed = ((byte) dr["MANUALINPUTALLOWED"] != 0);

            profile.KeyLockDeviceType = ((int) dr["KEYLOCK"] != 0) ?HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None;
            profile.KeyLockDeviceName = (string) dr["KEYLOCKDEVICENAME"];
            profile.KeyLockDeviceDescription = (string) dr["KEYLOCKDESCRIPTION"];

            profile.EftConnected = ((int)dr["EFT"] != 0);
            profile.LsPayConnected = (bool)dr["LSPAY"];
            profile.EftServerName = (string) dr["EFTSERVERNAME"];
            profile.EftDescription = (string) dr["EFTDESCRIPTION"];

            profile.FCMActive = ((byte) dr["FCMACTIVE"] != 0);
            profile.FCMCallingBlink = ((byte) dr["FCMCALLINGBLINK"] != 0);
            profile.FCMCallingSound = ((byte) dr["FCMCALLINGSOUND"] != 0);
            profile.FCMServer = (string) dr["FCMSERVER"];
            profile.FCMPort = (string) dr["FCMPORT"];
            profile.FCMPosPort = (string) dr["FCMPOSPORT"];
            profile.FCMControllerHostName = (string) dr["FCMCONTROLLERHOSTNAME"];
            profile.FCMImplFileName = (string) dr["FCMIMPLFILENAME"];
            profile.FCMImplFileType = (string) dr["FCMIMPLFILETYPE"];
            profile.FCMFuellingPointColumns = (int) dr["FCMFUELLINGPOINTCOLUMNS"];
            profile.FCMLogLevel = (int) dr["FCMLOGLEVEL"];
            profile.FCMScreenExtHeightPercentage = (decimal) dr["FCMSCREENEXTHEIGHTPERCENTAGE"];
            profile.FCMScreenHeightPercentage = (decimal) dr["FCMSCREENHEIGHTPERCENTAGE"];
            profile.FCMVolumeUnitID = (string) dr["FCMVOLUMEUNIT"];
            profile.FCMVolumeUnitDescription = (string) dr["FCMVOLUMEUNITDESCRIPTION"];

            try
            {
                profile.EftServerPort = Convert.ToInt32((string) dr["EFTSERVERPORT"]);
            }
            catch (Exception)
            {
                profile.EftServerPort = 0;
            }

            profile.EftCompanyID = (string) dr["EFTCOMPANYID"];
            profile.EftUserID = (string) dr["EFTUSERID"];
            profile.EftPassword = (string) dr["EFTPASSWORD"];
            profile.EftMerchantAccount = (string)dr["EFTMERCHANTACCOUNT"];
            profile.EftMerchantKey = (string)dr["EFTMERCHANTKEY"];
            profile.EftCustomField1 = (string)dr["EFTCUSTOMFIELD1"];
            profile.EftCustomField2 = (string)dr["EFTCUSTOMFIELD2"];
            profile.EftBatchIncrementAtEOS = ((byte) dr["EFTBATCHINCREMENTATEOS"] != 0);

            profile.CctvConnected = ((byte) dr["CCTV"] != 0);
            profile.CctvCamera = (string) dr["CCTVCAMERA"];
            profile.CctvHostname = (string) dr["CCTVHOSTNAME"];
            profile.CctvPort = (int) dr["CCTVPORT"];

            profile.Forecourt = ((int) dr["FORECOURT"] != 0);
            profile.Hostname = (string) dr["HOSTNAME"];
            profile.ForecourtPriceDecimalPointPosition = (int) dr["FORECOURTPRICEDECPOINTPOSITION"];
            profile.ForecourtToneActive = ((byte) dr["FORECOURTONEACTIVE"] != 0);
            profile.ForecourtToneFrequency = (int) dr["FORECOURTONEFREQUENCY"];
            profile.ForecourtToneLength = (int) dr["FORECOURTONELENGTH"];
            profile.ForecourtToneRepeats = (int) dr["FORECOURTONEREPEATS"];
            profile.ForecourtSuspendAllowed = ((byte) dr["FORECOURTSUSPENDALLOWED"] != 0);

            profile.PharmacyActive = ((byte) dr["PHARMACY"] != 0);
            profile.PharmacyHost = (string) dr["PHARMACYHOST"];
            profile.PharmacyPort = (int) dr["PHARMACYPORT"];

            profile.RFIDScannerConnected = ((int) dr["RFIDSCANNERTYPE"] != 0);
            profile.RFIDScannerDeviceName = (string) dr["RFIDDEVICENAME"];
            profile.RfIDScannerDeviceDescription = (string) dr["RFIDDESCRIPTION"];

            profile.CashChangerDeviceType = (HardwareProfile.CashChangerDeviceTypes) dr["CASHCHANGER"];
            profile.CashChangerConnected = ((int) profile.CashChangerDeviceType != 0);
            profile.CashChangerInitSettings = (string) dr["CASHCHANGERINITSETTINGS"];
            profile.CashChangerPortSettings = (string) dr["CASHCHANGERPORTSETTINGS"];

            profile.DualDisplayConnected = ((byte) dr["DUALDISPLAY"] != 0);
            profile.DualDisplayDeviceName = (string) dr["DUALDISPLAYDEVICENAME"];
            profile.DualDisplayDescription = (string) dr["DUALDISPLAYDESCRIPTION"];
            profile.DualDisplayType = (HardwareProfile.DisplayType) dr["DUALDISPLAYTYPE"];

            try
            {
                profile.DualDisplayPort = Convert.ToInt32((string) dr["DUALDISPLAYPORT"]);
            }
            catch (Exception)
            {
                profile.DualDisplayPort = 0;
            }

            profile.DualDisplayReceiptPrecentage = (decimal) dr["DUALDISPLAYRECEIPTPERCENTAGE"];
            profile.DualdisplayImagePath = (string) dr["DUALDISPLAYIMAGEPATH"];
            profile.DualDisplayImageInterval = (int) dr["DUALDISPLAYIMAGEINTERVAL"];
            profile.DualDisplayBrowserUrl = (string) dr["DUALDISPLAYBROWSERURL"];

            profile.FiscalPrinter = (int) dr["FISCALPRINTER"];
            profile.FiscalPrinterConnection = (string)dr["FISCALPRINTERCONNECTION"];
            profile.FiscalPrinterDescription = (string) dr["FISCALPRINTERDESCRIPTION"];

            profile.KeyboardmappingID = (string) dr["KEYBOARDMAPPINGID"];
            profile.UseKitchenDisplay = ((byte) dr["USEKITCHENDISPLAY"] != 0);
            profile.DualDisplayScreen = (HardwareProfile.DualDisplayScreens) (int) dr["DUALDISPLAYSCREENNUMBER"];

            profile.DallasKeyConnected = ((byte) dr["DALLASKEYCONNECTED"] != 0);
            profile.DallasMessagePrefix = (string) dr["DALLASMESSAGEPREFIX"];
            profile.DallasKeyRemovedMessage = (string) dr["DALLASKEYREMOVEDMESSAGE"];
            profile.DallasComPort = (string) dr["DALLASCOMPORT"];
            profile.DallasBaudRate = (int) dr["DALLASBAUDRATE"];
            profile.DallasParity = (Parity) (int) dr["DALLASPARITY"];
            profile.DallasStopBits = (StopBits) (int) dr["DALLASSTOPBITS"];
            profile.DallasDataBits = (int) dr["DALLASDATABITS"];
            
            profile.WindowsPrinterConfigurationID = (string) dr["WINDOWSPRINTERCONFIGURATIONID"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
            profile.StationPrintingHostID = (string) dr["STATIONPRINTINGHOSTID"];
        }

        private static void PopulateTempProfile(IConnectionManager entry, IDataReader dr, HardwareProfile profile, ref object parameter)
        {
            profile.MsrDeviceType = (HardwareProfile.DeviceTypes)(int)dr["MSR"];
            profile.MsrDeviceName = (string)dr["MSRDEVICENAME"];
            profile.MsrDeviceDescription = (string)dr["MSRDESCRIPTION"];
            profile.StartTrack1 = (string)dr["STARTTRACK1"];
            profile.Separator1 = (string)dr["SEPARATOR1"];
            profile.EndTrack1 = (string)dr["ENDTRACK1"];

            profile.ScannerConnected = ((int)dr["SCANNER"] != 0);
            profile.ScannerDeviceName = (string)dr["SCANNERDEVICENAME"];
            profile.ScannerDeviceDescription = (string)dr["SCANNERDESCRIPTION"];

            profile.RFIDScannerConnected = ((int)dr["RFIDSCANNERTYPE"] != 0);
            profile.RFIDScannerDeviceName = (string)dr["RFIDDEVICENAME"];
            profile.RfIDScannerDeviceDescription = (string)dr["RFIDDESCRIPTION"];

            profile.DallasKeyConnected = ((byte)dr["DALLASKEYCONNECTED"] != 0);
            profile.DallasMessagePrefix = (string)dr["DALLASMESSAGEPREFIX"];
            profile.DallasKeyRemovedMessage = (string)dr["DALLASKEYREMOVEDMESSAGE"];
            profile.DallasComPort = (string)dr["DALLASCOMPORT"];
            profile.DallasBaudRate = (int)dr["DALLASBAUDRATE"];
            profile.DallasParity = (Parity)(int)dr["DALLASPARITY"];
            profile.DallasStopBits = (StopBits)(int)dr["DALLASSTOPBITS"];
            profile.DallasDataBits = (int)dr["DALLASDATABITS"];

            profile.ScaleConnected = ((int)dr["SCALE"] != 0);
        }

        public virtual HardwareProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSQL +
                    " where p.DATAAREAID = @dataAreaId and p.PROFILEID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);
                return Get<HardwareProfile>(entry, cmd, id, PopulateProfile, cacheType, UsageIntentEnum.Normal);
            }
        }

        public HardwareProfile GetTempProfileForTokenLogin(IConnectionManager entry,
                                                                  string dataSource,
                                                                  bool windowsAuthentication,
                                                                  string sqlServerLogin,
                                                                  SecureString sqlServerPassword,
                                                                  string databaseName,
                                                                  ConnectionType connectionType,
                                                                  string dataAreaID,
                                                                  RecordIdentifier storeID,
                                                                  RecordIdentifier terminalID)
        {
            using (IDbCommand cmd = new SqlCommand("spPOSGetTempHardwareProfile_1_0"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "terminalID", (string)terminalID);
                MakeParam(cmd, "dataareaID", dataAreaID);

                object parameter = new object();

                List<HardwareProfile> result = entry.UnsecureExecuteReader<HardwareProfile, object>(
                    dataSource,
                    windowsAuthentication,
                    sqlServerLogin,
                    sqlServerPassword,
                    databaseName,
                    connectionType,
                    dataAreaID,
                    cmd,
                    ref parameter,
                    PopulateTempProfile);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<HardwareProfile>(entry, "POSHARDWAREPROFILE", "PROFILEID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<HardwareProfile>(entry, "POSHARDWAREPROFILE", "PROFILEID", id, BusinessObjects.Permission.HardwareProfileEdit);
        }

        public virtual void Save(IConnectionManager entry, HardwareProfile profile)
        {
            var statement = new SqlServerStatement("POSHARDWAREPROFILE");

            //ValidateSecurity(entry, BusinessObjects.Permission.HardwareProfileEdit);

            ValidateSecurity(entry);

            if (!(entry.HasPermission(Permission.HardwareProfileEdit) || entry.HasPermission(Permission.ManagePosHardwareProfile)))
            {
                throw new PermissionException(Permission.HardwareProfileEdit + " or " + Permission.ManagePosHardwareProfile);
            }

            profile.Validate();

            bool isNew = false;
            if (profile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                profile.ID = DataProviderFactory.Instance.GenerateNumber<IHardwareProfileData, HardwareProfile>(entry); 
            }

            if (isNew || !Exists(entry, profile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (string)profile.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (string)profile.ID);
            }

            statement.AddField("NAME", profile.Text);

            statement.AddField("DRAWER", profile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS ? 1 : 0, SqlDbType.Int);
            statement.AddField("DRAWERDEVICENAME", profile.DrawerDeviceName);
            statement.AddField("DRAWERDESCRIPTION", profile.DrawerDescription);
            statement.AddField("DRAWEROPENTEXT", profile.DrawerOpenText);

            statement.AddField("DISPLAYDEVICE", (int) profile.LineDisplayDeviceType, SqlDbType.Int);
            statement.AddField("DISPLAYDEVICENAME", profile.DisplayDeviceName);
            statement.AddField("DISPLAYDESCRIPTION", profile.DisplayDeviceDescription);
            statement.AddField("DISPLAYTOTALTEXT", profile.DisplayTotalText);
            statement.AddField("DISPLAYBALANCETEXT", profile.DisplayBalanceText);
            statement.AddField("DISPLAYCLOSEDLINE1", profile.DisplayClosedLine1);
            statement.AddField("DISPLAYCLOSEDLINE2", profile.DisplayClosedLine2);
            statement.AddField("DISPLAYCHARACTERSET", profile.DisplayCharacterSet, SqlDbType.Int);
            statement.AddField("DISPLAYBINARYCONVERSION", profile.DisplayBinaryConversion ? 1 : 0, SqlDbType.Int);

            statement.AddField("MSR", (int)profile.MsrDeviceType, SqlDbType.Int);
            statement.AddField("MSRDEVICENAME", profile.MsrDeviceName);
            statement.AddField("MSRDESCRIPTION", profile.MsrDeviceDescription);
            statement.AddField("STARTTRACK1", profile.StartTrack1);
            statement.AddField("SEPARATOR1", profile.Separator1);
            statement.AddField("ENDTRACK1", profile.EndTrack1);

            statement.AddField("PRINTER", profile.Printer, SqlDbType.Int);
            statement.AddField("PRINTERDEVICENAME", profile.PrinterDeviceName);
            statement.AddField("PRINTERDESCRIPTION", profile.PrinterDeviceDescription);
            statement.AddField("PRINTBINARYCONVERSION", profile.PrintBinaryConversion ? 1 : 0, SqlDbType.Int);
            statement.AddField("PRINTERCHARACTERSET", profile.PrinterCharacterSet, SqlDbType.Int);
            statement.AddField("PRINTEREXTRALINES", profile.PrinterExtraLines, SqlDbType.Int);

            statement.AddField("SCANNER", profile.ScannerConnected ? 1 : 0, SqlDbType.Int);
            statement.AddField("SCANNERDEVICENAME", profile.ScannerDeviceName);
            statement.AddField("SCANNERDESCRIPTION", profile.ScannerDeviceDescription);

            statement.AddField("ETAX", profile.ETaxConnected ? 1 : 0, SqlDbType.Int);
            statement.AddField("ETAXPORTNAME", profile.ETaxPortName);

            statement.AddField("SCALE", profile.ScaleConnected ? 1 : 0, SqlDbType.Int);
            statement.AddField("SCALEDEVICENAME", profile.ScaleDeviceName);
            statement.AddField("SCALEDESCRIPTION", profile.ScaleDeviceDescription);
            statement.AddField("MANUALINPUTALLOWED", profile.ScaleManualInputAllowed ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("KEYLOCK",  profile.KeyLockDeviceType == HardwareProfile.DeviceTypes.OPOS ? 1 : 0, SqlDbType.Int);
            statement.AddField("KEYLOCKDEVICENAME", profile.KeyLockDeviceName);
            statement.AddField("KEYLOCKDESCRIPTION", profile.KeyLockDeviceDescription);

            statement.AddField("EFT", profile.EftConnected ? 1 : 0, SqlDbType.Int);
            statement.AddField("LSPAY", profile.LsPayConnected ? 1 : 0, SqlDbType.Bit);
            statement.AddField("EFTSERVERNAME", profile.EftServerName);
            statement.AddField("EFTDESCRIPTION", profile.EftDescription);
            statement.AddField("EFTSERVERPORT", profile.EftServerPort.ToString());
            statement.AddField("EFTCOMPANYID", profile.EftCompanyID);
            statement.AddField("EFTUSERID", profile.EftUserID);
            statement.AddField("EFTPASSWORD", profile.EftPassword);
            statement.AddField("EFTMERCHANTACCOUNT", profile.EftMerchantAccount);
            statement.AddField("EFTMERCHANTKEY", profile.EftMerchantKey);
            statement.AddField("EFTCUSTOMFIELD1", profile.EftCustomField1);
            statement.AddField("EFTCUSTOMFIELD2", profile.EftCustomField2);
            statement.AddField("EFTBATCHINCREMENTATEOS", profile.EftBatchIncrementAtEOS ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("CCTV", profile.CctvConnected ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CCTVCAMERA", profile.CctvCamera);
            statement.AddField("CCTVHOSTNAME", profile.CctvHostname);
            statement.AddField("CCTVPORT", profile.CctvPort, SqlDbType.Int);

            statement.AddField("FORECOURT", profile.Forecourt ? 1 : 0, SqlDbType.Int);
            statement.AddField("HOSTNAME", profile.Hostname);

            statement.AddField("RFIDSCANNERTYPE", profile.RFIDScannerConnected ? 1 : 0, SqlDbType.Int);
            statement.AddField("RFIDDEVICENAME", profile.RFIDScannerDeviceName);
            statement.AddField("RFIDDESCRIPTION", profile.RfIDScannerDeviceDescription);

            statement.AddField("CASHCHANGER", (int)profile.CashChangerDeviceType, SqlDbType.Int);
            statement.AddField("CASHCHANGERPORTSETTINGS", profile.CashChangerPortSettings);
            statement.AddField("CASHCHANGERINITSETTINGS", profile.CashChangerInitSettings);

            statement.AddField("DUALDISPLAY", profile.DualDisplayConnected ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DUALDISPLAYDEVICENAME", profile.DualDisplayDeviceName);
            statement.AddField("DUALDISPLAYDESCRIPTION", profile.DualDisplayDescription);
            statement.AddField("DUALDISPLAYTYPE", (int)profile.DualDisplayType,SqlDbType.Int);
            statement.AddField("DUALDISPLAYPORT", profile.DualDisplayPort.ToString());
            statement.AddField("DUALDISPLAYRECEIPTPERCENTAGE", profile.DualDisplayReceiptPrecentage, SqlDbType.Decimal);
            statement.AddField("DUALDISPLAYIMAGEPATH", profile.DualdisplayImagePath);
            statement.AddField("DUALDISPLAYIMAGEINTERVAL", profile.DualDisplayImageInterval,SqlDbType.Int);
            statement.AddField("DUALDISPLAYBROWSERURL", profile.DualDisplayBrowserUrl);

            statement.AddField("FORECOURTMANAGERSCREENHEIGHTPERCENTAGE", profile.FCMScreenHeightPercentage, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERSCREENEXTHEIGHTPERCENTAGE", profile.FCMScreenExtHeightPercentage, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERCONTROLLERHOSTNAME", profile.FCMControllerHostName);
            statement.AddField("FORECOURTMANAGERLOGLEVEL", profile.FCMLogLevel, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERFUELLINGPOINTCOLUMNS", profile.FCMFuellingPointColumns, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERIMPLFILENAME", profile.FCMImplFileName);
            statement.AddField("FORECOURTMANAGERIMPLFILETYPE", profile.FCMImplFileType);
            statement.AddField("FORECOURTMANAGERVOLUMEUNIT", (string)profile.FCMVolumeUnitID);
            statement.AddField("FORECOURTMANAGERCALLINGSOUND", profile.FCMCallingSound ? 1 : 0, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERCALLINGBLINK", profile.FCMCallingBlink ? 1 : 0, SqlDbType.Int);

            statement.AddField("FORECOURTMANAGER", profile.FCMActive ? 1 : 0, SqlDbType.Int);
            statement.AddField("FORECOURTMANAGERHOSTNAME", profile.FCMServer);
            statement.AddField("FORECOURTMANAGERPORT", profile.FCMPort);
            statement.AddField("FORECOURTMANAGERPOSPORT", profile.FCMPosPort);

            statement.AddField("FISCALPRINTER", profile.FiscalPrinter, SqlDbType.Int);
            statement.AddField("FISCALPRINTERCONNECTION", profile.FiscalPrinterConnection);
            statement.AddField("FISCALPRINTERDESCRIPTION", profile.FiscalPrinterDescription);

            statement.AddField("USEKITCHENDISPLAY", profile.UseKitchenDisplay ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DUALDISPLAYSCREENNUMBER", (int)profile.DualDisplayScreen, SqlDbType.Int);


            statement.AddField("DALLASKEYCONNECTED", profile.DallasKeyConnected ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DALLASMESSAGEPREFIX", profile.DallasMessagePrefix);
            statement.AddField("DALLASKEYREMOVEDMESSAGE", profile.DallasKeyRemovedMessage);
            statement.AddField("DALLASCOMPORT", profile.DallasComPort);
            statement.AddField("DALLASBAUDRATE", profile.DallasBaudRate, SqlDbType.Int);
            statement.AddField("DALLASPARITY", (int)profile.DallasParity, SqlDbType.Int);
            statement.AddField("DALLASSTOPBITS", (int) profile.DallasStopBits, SqlDbType.Int);
            statement.AddField("DALLASDATABITS", profile.DallasDataBits, SqlDbType.Int);

            statement.AddField("STATIONPRINTINGHOSTID", (string)profile.StationPrintingHostID);
            statement.AddField("WINDOWSPRINTERCONFIGURATIONID", (string)profile.WindowsPrinterConfigurationID);

            statement.AddField("PHARMACY", profile.PharmacyActive ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PHARMACYHOST", profile.PharmacyHost);
            statement.AddField("PHARMACYPORT", profile.PharmacyPort, SqlDbType.Int);

            Save(entry, profile, statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "HARDWAREPROFILE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSHARDWAREPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
