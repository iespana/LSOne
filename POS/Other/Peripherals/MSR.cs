using System;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.OPOS;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class MSR
    {
        static internal OPOSMSR posMSR;
        static private bool deviceActive = false;

        public delegate void MSRMessageDelegate(string track2);
        static public event MSRMessageDelegate MSRMessageEvent;
        
        static public void Load()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.MsrDeviceType  != HardwareProfile.DeviceTypes.OPOS|| deviceActive)
                {
                    return;
                }

                deviceActive = true;

                posMSR = new OPOSMSR(DLLEntry.Settings.HardwareProfile.MsrDeviceName);
                posMSR.MSRMessageEvent += new OPOSMSR.msrMessageDelegate(posMSR_MSRMessageEvent);

                posMSR.Initialize();

            }
            catch (Exception x)
            {
                posMSR = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.MSR", x);
                throw x;
            }
        }


        static public void Unload()
        {

            if (!deviceActive) { return; }

            try
            {
                posMSR.Finalise();
                deviceActive = false;
            }
            catch (Exception x)
            {
                posMSR = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.MSR", x);
                throw x;
            }

        }


        static void posMSR_MSRMessageEvent(string track2)
        {
            MSRMessageEvent(track2);
        }

        static public void EnableForSwipe()
        {
            if (!deviceActive) { return; }

            posMSR.EnableForSwipe();
        }

        static public void DisableForSwipe()
        {
            if (!deviceActive) { return; }

            posMSR.DisableForSwipe();
        }

        

        static public string CardNo()
        {
            return posMSR.CardNo;
        }

        static public string ExpDate()
        {
            return posMSR.ExpDate;
        }

    }


}
