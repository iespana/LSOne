using System;
using System.Configuration;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO.JSON;
using LSRetail.Forecourt;

namespace LSOne.Peripherals
{
    static public class ForecourtManager
    {
        static private bool deviceActive;
        private static IForecourtManager forecourtManager;

        static public void Load()
        {
            try
            {
                if (!DLLEntry.Settings.HardwareProfile.FCMActive || deviceActive)
                    return;

                ListenToForecourtManager();
                RunForecourtManager();

                Thread.Sleep(500);

                deviceActive = true;

            }
            catch (Exception x)
            {
                deviceActive = false;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.ForecourtManager.Load()", x);
                throw x;
            }
        }

        public static IForecourtManager ForecourtManagerClient => forecourtManager;

        private static void RunForecourtManager()
        {
            Store currentStore = Providers.StoreData.Get(DLLEntry.DataModel, DLLEntry.DataModel.CurrentStoreID);
            Currency currency = Providers.CurrencyData.Get(DLLEntry.DataModel, currentStore.Currency);
            HardwareProfile forecourtSettings = DLLEntry.Settings.HardwareProfile;
            IForecourtManagerProfile fuelingStationProfile = new ForecourtManagerProfile();
            fuelingStationProfile.ImplFileType = forecourtSettings.FCMImplFileType;
            fuelingStationProfile.ImplFileName = forecourtSettings.FCMImplFileName;
            fuelingStationProfile.CurrencySymbol = currency.CurrencyPrefix;
            fuelingStationProfile.Language = currentStore.LanguageCode;//"en-us";
            fuelingStationProfile.BaseUnit = forecourtSettings.FCMVolumeUnitDescription;
            fuelingStationProfile.MainStateAsText = true;
            fuelingStationProfile.UseDynamicsAX = false;
            fuelingStationProfile.Orientation = "TOP";
            fuelingStationProfile.DriveOffMinutes = 7;
            fuelingStationProfile.TransactionBuffer = 2;
            fuelingStationProfile.UnattendedFuelVolumeLimit = 3;
            fuelingStationProfile.ManagerFolder = "LSForecourtManager";
            fuelingStationProfile.PosPort = forecourtSettings.FCMPosPort;
            fuelingStationProfile.ControllerHostName = forecourtSettings.FCMControllerHostName;
            fuelingStationProfile.FuelDecimalPrecision = 3;
            fuelingStationProfile.TraceLogDays = 10;
            fuelingStationProfile.ScreenHeightPct = (int)forecourtSettings.FCMScreenHeightPercentage;
            fuelingStationProfile.ScreenExtHeightPct = (int)forecourtSettings.FCMScreenExtHeightPercentage;
            fuelingStationProfile.FuelingPointColumns = forecourtSettings.FCMFuellingPointColumns;
            fuelingStationProfile.LogLevel = forecourtSettings.FCMLogLevel;//1;
            fuelingStationProfile.CallingBlink = forecourtSettings.FCMCallingBlink; //false;
            fuelingStationProfile.CallingSound = forecourtSettings.FCMCallingSound; //false;
            fuelingStationProfile.SoundOnDriveOff = false;

            fuelingStationProfile.TerminalId = (string)DLLEntry.DataModel.CurrentTerminalID;
            fuelingStationProfile.Port = Convert.ToInt32(forecourtSettings.FCMPort);
            fuelingStationProfile.FileName = "C:\\Program Files (x86)\\LS Retail";

            IDispensersProfile dispensersProfile = new DispensersProfile();
            dispensersProfile.DispenserRows = 0;
            dispensersProfile.HoseColumns = 0;
            dispensersProfile.DispenserHoseMatrix = new int[dispensersProfile.DispenserRows, dispensersProfile.HoseColumns];
            dispensersProfile.addToPrepayAllowedDictionary(1, true);
            for (int i = 1; i <= dispensersProfile.DispenserRows; i++)
            {
                for (int x = 1; x <= dispensersProfile.HoseColumns; x++)
                {
                    dispensersProfile.DispenserHoseMatrix[i - 1, x - 1] = x;
                }
            }

            forecourtManager = new LSRetail.Forecourt.ForecourtManager();
            forecourtManager.RunForecourtManager(fuelingStationProfile, dispensersProfile);

            IFuellingPointTransaction ft = new FuellingPointTransaction();
            forecourtManager.FuellingPointTransactionClear(ft);
        }

        static void ListenToForecourtManager()
        {
            IStartThreads threads = new StartThreads();
            threads.StartPipeServer();

            threads.sellEvent += threads_sellEvent;
            threads.pumpTransferEvent += threads_operationEvent;
        }

        static void threads_operationEvent(object source, EventArgs e)
        {
            IStartThreads startThread = (IStartThreads)source;
        }

        static void threads_sellEvent(object source, EventArgs e)
        {
            IStartThreads startThread = (IStartThreads)source;

            IFuellingPointTransaction trans = new FuellingPointTransaction();
            XmlDocument transDoc = new XmlDocument();
            transDoc.LoadXml(startThread.message);

            trans.ToClass((XDocument.Load(new XmlNodeReader(transDoc)).Root));

            DLLEntry.Settings.POSApp.RunOperation(POSOperations.FuelItemSale, trans);
        }
    }
}
