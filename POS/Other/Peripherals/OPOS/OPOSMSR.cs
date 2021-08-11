using System;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSMSR : OPOSBase
    {
        

        #region Member variables

        private OPOSMSRClass posMSR;

        private string deviceName;                                // The device name, e.g. TM-H6000II

        public delegate void msrMessageDelegate(string track2);
        public event msrMessageDelegate MSRMessageEvent;

        #endregion

        #region Properties
        
        public string DeviceName
        {
            set { deviceName = value; }
        }

        public string CardNo
        {
            get { return posMSR.AccountNumber; }
        }

        public string ExpDate
        {
            get{return posMSR.ExpirationDate;}
        }


        #endregion


        public OPOSMSR(string deviceName)
        {
            this.deviceName = deviceName;
            
        }


        public override void Initialize()
        {
            try
            {
                Open();
                Claim();
                Enable();
                Configure();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public override void Finalise()
        {
            try
            {
                if (posMSR!= null)
                {
                    posMSR.ReleaseDevice();
                    posMSR.Close();
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }


        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            try
            {
                posMSR = new OPOSMSRClass();

                posMSR.DataEvent += new _IOPOSMSREvents_DataEventEventHandler(posMSR_DataEvent);

                posMSR.Open(deviceName);

                CheckState();
                CheckResultCode();

            }
            catch (Exception x)
            {
                throw x;
            }

        }

        


        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            try
            {
                posMSR.ClaimDevice(10000);
                
                CheckState();
                CheckResultCode();
               
            }
            catch (Exception x)
            {
                throw x;
            }
        }


        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            try
            {
                if (posMSR.State != (int)OPOS_Constants.OPOS_S_CLOSED)
                {
                    posMSR.DeviceEnabled = true;
                }
            }
            catch (Exception x)
            {
                throw x;
            }

        }


        /// <summary>
        /// Configures the OPOS device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void Configure()
        {
            try
            {
                posMSR.AutoDisable = true;
                posMSR.DecodeData = true;

                // The reader starts in a disabled state
                posMSR.DeviceEnabled = false;
                posMSR.DataEventEnabled = false;
                
            }
            catch (Exception x)
            {
                throw x;
            }
        }


        private void CheckState()
        {
            switch (posMSR.State)
            {
                   
                case (int)OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception(Properties.Resources.MSRIsClosed);
                case (int)OPOS_Constants.OPOS_S_ERROR:
                    //MessageBox.Show("Villa: Villa");
                    break;
                case (int)OPOS_Constants.OPOS_S_IDLE:
                    //MessageBox.Show("Idle");
                    break;

                default:
                    break;
            }

        }

        private void CheckResultCode()
        {

            switch (posMSR.ResultCode)
            {

                case (int)OPOS_Constants.OPOS_SUCCESS:
                    //throw new Exception("The MSR is closed...");
                    break;
                case (int)OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.MSRIsClosed);
                case (int)OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.MSRIsClosed);
                case (int)OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Properties.Resources.MSRIsClosed);
                case (int)OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.MSRIllegallyClosed);
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                    if (posMSR.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.MSRIsClosed);
                    }
                    break;
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.MSRTimedOut);
                default:
                    break;
            }
        }



        void posMSR_DataEvent(int Status)
        {
            MSRMessageEvent(posMSR.Track2Data);
        }


        // After each swipe the MSR disables itself.  When the magnetic data has been processed
        // we need to call this function to reenable the MSR.
        public void EnableForSwipe()
        {
            posMSR.DataEventEnabled = true;
            posMSR.DeviceEnabled = true;
        }

        public void DisableForSwipe()
        {
            posMSR.DeviceEnabled = false;
            posMSR.DataEventEnabled = false;

        }





    }
}
