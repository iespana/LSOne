using System;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSCashDrawer : OPOSBase
    {
        #region Member variables

        private OPOSCashDrawerClass posCashDrawer;

        private string deviceName;                                // The device name, e.g. TM-H6000II

        public delegate void cashdrawerMessageDelegate(string message);
        public event cashdrawerMessageDelegate CashDrawerMessageEvent;

        #endregion

        #region Properties
        
        public string DeviceName
        {
            set { deviceName = value; }
        }

        #endregion

        public OPOSCashDrawer(string deviceName)
        {
            this.deviceName = deviceName;
        }

        public override void Initialize()
        {
            Open();
            Claim();
            Enable();
            Configure();
        }

        public override void Finalise()
        {
            if (posCashDrawer != null)
            {
                posCashDrawer.ReleaseDevice();
                posCashDrawer.Close();
            }
        }

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            posCashDrawer = new OPOSCashDrawerClass();

            posCashDrawer.StatusUpdateEvent += posCashDrawer_StatusUpdateEvent;

            posCashDrawer.Open(deviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            posCashDrawer.ClaimDevice(10000);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            if (posCashDrawer.State != (int) OPOS_Constants.OPOS_S_CLOSED)
            {
                posCashDrawer.DeviceEnabled = true;
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
            switch (posCashDrawer.State)
            {
                   
                case (int)OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception(Properties.Resources.CashDrawerIsClosed);
                case (int)OPOS_Constants.OPOS_S_ERROR:
                    //MessageBox.Show("Villa: Villa");
                    break;
                case (int)OPOS_Constants.OPOS_S_IDLE:
                    //MessageBox.Show("Idle");
                    break;
            }
        }

        private void CheckResultCode()
        {
            switch (posCashDrawer.ResultCode)
            {
                  
                case (int)OPOS_Constants.OPOS_SUCCESS:
                    break;
                case (int)OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.CashDrawerIsClosed);
                case (int)OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.CashDrawerIsClosed);
                case (int)OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Properties.Resources.CashDrawerIsClosed);
                case (int)OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.CashDrawerIsIllegallyClosed);
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                    if (posCashDrawer.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.CashDrawerIsClosed);
                    }
                    break;
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.CashDrawerTimedOut);
            }
        }

        public bool OpenDrawer()
        {
            return posCashDrawer.OpenDrawer() == (int)OPOS_Constants.OPOS_SUCCESS;
        }

        public bool DrawerOpen()
        {
            return posCashDrawer.DrawerOpened;
        }

        public bool CapStatus()
        {
            return posCashDrawer.CapStatus;
        }
        
        void posCashDrawer_StatusUpdateEvent(int Data)
        {
            CashDrawerMessageEvent(Data.ToString());
        }
    }
}
