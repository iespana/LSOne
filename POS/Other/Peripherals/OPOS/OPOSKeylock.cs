using System;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSKeylock : OPOSBase
    {
        #region Member variables

        private OPOSKeylockClass posKeylock;

        private string deviceName; // The device name, e.g. TM-H6000II

        public delegate void keylockSupervisorDelegate();

        public event keylockSupervisorDelegate KeylockSupervisorEvent;

        #endregion

        #region Properties

        public string DeviceName
        {
            set
            {
                deviceName = value;
            }
        }

        #endregion

        public OPOSKeylock(string deviceName)
        {
            this.deviceName = deviceName;
        }

        public override void Initialize()
        {
            // The keylock control cannot be claimed.
            Open();
            Enable();
            Configure();
        }

        public override void Finalise()
        {
            if (posKeylock != null)
            {
                posKeylock.ReleaseDevice();
                posKeylock.Close();
            }
        }

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            posKeylock = new OPOSKeylockClass();

            posKeylock.StatusUpdateEvent += new _IOPOSKeylockEvents_StatusUpdateEventEventHandler(posKeylock_StatusUpdateEvent);

            posKeylock.Open(deviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            if (posKeylock.State != (int) OPOS_Constants.OPOS_S_CLOSED)
            {
                posKeylock.DeviceEnabled = true;
            }
        }

        /// <summary>
        /// Configures the OPOS device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void Configure()
        {
        }

        private void CheckState()
        {
            switch (posKeylock.State)
            {

                case (int) OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception(Properties.Resources.KeylockIsClosed);
                case (int) OPOS_Constants.OPOS_S_ERROR:
                    break;
                case (int) OPOS_Constants.OPOS_S_IDLE:
                    break;
            }
        }

        private void CheckResultCode()
        {
            switch (posKeylock.ResultCode)
            {

                case (int) OPOS_Constants.OPOS_SUCCESS:
                    break;
                case (int) OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.KeylockIsClosed);
                case (int) OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.KeylockIsClosed);
                case (int) OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Properties.Resources.KeylockIsClosed);
                case (int) OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.KeylockIsIllegallyClosed);
                case (int) OPOS_Constants.OPOS_E_EXTENDED:
                    if (posKeylock.ResultCodeExtended == (int) OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.KeylockIsClosed);
                    }
                    break;
                case (int) OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.KeylockTimedOut);
            }
        }

        private void posKeylock_StatusUpdateEvent(int Data)
        {
            if (Data == (int) OPOSKeylockConstants.LOCK_KP_SUPR)
            {
                KeylockSupervisorEvent();
            }
        }

        public bool SupervisorPosition()
        {
            return posKeylock.KeyPosition == (int) OPOSKeylockConstants.LOCK_KP_SUPR;
        }

        public bool LockedPosition()
        {
            return posKeylock.KeyPosition == (int) OPOSKeylockConstants.LOCK_KP_LOCK;
        }
    }
}
