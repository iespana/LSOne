using System;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class RFIDScanner
    {
        static private bool deviceActive = false;

        public static bool NewTags 
        { 
            get { return Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).NewTags; }
            set { Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).NewTags = value; } 
        } //for polling use


        static public void Load()
        {
            try
            {
                if (!DLLEntry.Settings.HardwareProfile.RFIDScannerConnected || deviceActive)
                {
                    return;                  
                }
                Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).Load();
                deviceActive = true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.RFIDScanner", x);
                throw x;
            }
        }

        static public void Unload()
        {
            if (!deviceActive) { return; }
            Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).Unload();
            deviceActive = false;
        }

        static public void DisableForScan()
        {
            if (!deviceActive) { return; }
            Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).DisableForScan();
        }

        static public void ReEnableForScan()
        {
            if (!deviceActive) { return; }
            Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).ReEnableForScan();
        }

        static public void ConcludeRFIDs()
        {
            if (!deviceActive) { return; }
            Services.Interfaces.Services.RFIDService(DLLEntry.DataModel).ConcludeRFIDs(DLLEntry.DataModel);
        }
    }
}

