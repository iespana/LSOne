using System;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.OPOS;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    static public class Keylock
    {
        static private OPOSKeylock posKeylock;
        static private bool deviceActive = false;

        public delegate void KeylockSupervisorPositionDelegate();
        static public event KeylockSupervisorPositionDelegate KeylockSupervisorPositionEvent;

        static public void Load()
        {
            try
            {
                if (DLLEntry.Settings.HardwareProfile.KeyLockDeviceType != HardwareProfile.DeviceTypes.OPOS || deviceActive)
                {
                    return;
                }

                deviceActive = true;

                posKeylock = new OPOSKeylock(DLLEntry.Settings.HardwareProfile.KeyLockDeviceName);
                posKeylock.KeylockSupervisorEvent += new OPOSKeylock.keylockSupervisorDelegate(posKeylock_KeylockSupervisorEvent);

                posKeylock.Initialize();
            }
            catch (Exception x)
            {
                posKeylock = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Keylock", x);
                throw x;
            }
        }

        static public void Unload()
        {
            if (!deviceActive) { return; }

            try
            {
                posKeylock.Finalise();
                deviceActive = false;
            }
            catch (Exception x)
            {
                posKeylock = null;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Keylock", x);
                throw x;
            }
        }

        static void posKeylock_KeylockSupervisorEvent()
        {
            KeylockSupervisorPositionEvent();
        }

        // Returns true if the key is in the supervisor position
        static public bool SupervisorPosition()
        {
            if (!deviceActive) { return false; }

            return posKeylock.SupervisorPosition();
        }

        // Returns true if the key is in the locked position
        static public bool LockedPosition()
        {
            if (!deviceActive) { return false; }

            return posKeylock.LockedPosition();
        }
    }
}
