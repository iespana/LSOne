using System;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.Peripherals.OPOS;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class Scanner
    {
        static internal OPOSScanner posScanner;
        static private bool deviceActive = false;

        public delegate void ScannerMessageDelegate(ScanInfo scanInfo);
        static public event ScannerMessageDelegate ScannerMessageEvent;

        public static void DisableAllScannerEvents()
        {
            if (ScannerMessageEvent != null)
            {
                foreach (Delegate d in ScannerMessageEvent.GetInvocationList())
                {
                    ScannerMessageEvent -= (ScannerMessageDelegate)d;
                }
            }
        }

        static public void Load()
        {
            try
            {
                if (!DLLEntry.Settings.HardwareProfile.ScannerConnected || deviceActive)
                {
                    return;
                }
                posScanner = new OPOSScanner(DLLEntry.Settings.HardwareProfile.ScannerDeviceName);
                posScanner.ScannerMessageEvent += posScanner_ScannerMessageEvent;

                posScanner.Initialize();
                deviceActive = true;

                // Make sure the scanner is not active without having an event handler defined.
                DisableForScan();
            }
            catch (Exception x)
            {
                posScanner = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Scanner", x);
                throw x;
            }
        }

        static public void Unload()
        {
            if (!deviceActive) { return; }

            try
            {
                posScanner.Finalise();
                deviceActive = false;
            }
            catch (Exception x)
            {
                posScanner = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Scanner", x);
                throw x;
            }
        }

        static public void posScanner_ScannerMessageEvent(ScanInfo scanInfo)
        {
            ScannerMessageEvent(scanInfo);
        }

        static public void DisableForScan()
        {
            if (!deviceActive) { return; }

            posScanner.DisableForScan();
        }

        static public void ReEnableForScan()
        {
            if (!deviceActive) { return; }

            posScanner.ReEnableForScan();
        }
    }
}

