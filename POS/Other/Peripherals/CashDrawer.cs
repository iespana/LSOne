using System;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.OPOS;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class CashDrawer
    {
        static internal OPOSCashDrawer posCashDrawer;
        static private bool deviceActive = false;
        static string OPEN_DRAWER_SEQUENCE = "p\0dd";

        public delegate void cashdrawerMessageDelegateX(string message);
        static public event cashdrawerMessageDelegateX CashDrawerMessageEventX;


        static public void Load()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.DrawerDeviceType != HardwareProfile.DeviceTypes.OPOS || deviceActive)
                {
                    return;
                }
                posCashDrawer = new OPOSCashDrawer(DLLEntry.Settings.HardwareProfile.DrawerDeviceName);
                posCashDrawer.CashDrawerMessageEvent += new OPOSCashDrawer.cashdrawerMessageDelegate(posCashDrawer_CashDrawerMessageEvent);
                posCashDrawer.Initialize();
                deviceActive = true;
            }
            catch (Exception x)
            {
                posCashDrawer = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.CashDrawer", x);
                throw x;
            }
        }

        static void posCashDrawer_CashDrawerMessageEvent(string message)
        {
            if (CashDrawerMessageEventX == null)
                return;            

            CashDrawerMessageEventX(message);
        }



        static public void Unload()
        {

            if (!deviceActive) { return; }
            if (posCashDrawer == null) { return; }

            try
            {
                posCashDrawer.Finalise();
                deviceActive = false;
            }
            catch (Exception x)
            {
                posCashDrawer = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.CashDrawer", x);
                throw x;
            }

        }

        static public bool OpenDrawer()
        {
            if (!deviceActive) { return false; }
            if (posCashDrawer == null) { return false; }

            LineDisplay.DisplayText(DLLEntry.Settings.HardwareProfile.DrawerOpenText);

            if (DLLEntry.Settings.HardwareProfile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                return posCashDrawer.OpenDrawer();
            }
            else
            {
                Port port = new Port();
                port.WriteToPort(OPEN_DRAWER_SEQUENCE, DLLEntry.Settings.HardwareProfile.DrawerDeviceName);                
            }

            return true;
        }



        static public bool DrawerOpen()
        {
            if (posCashDrawer == null) { return false; }

            if (DLLEntry.Settings.HardwareProfile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                return posCashDrawer.DrawerOpen();
            }
            else
            {
                return false;
            }

        }

        // Returns true if the drawer device is capable of reporting back whether it's closed or open.
        static public bool CapStatus()
        {
            if (posCashDrawer == null) { return false; }

            if (DLLEntry.Settings.HardwareProfile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS)
            {
                return posCashDrawer.CapStatus();
            }
            else
            {
                return false;
            }

        }



    }


}
