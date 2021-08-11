using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSScanner : OPOSBase
    {
        #region Member variables

        private OPOSScannerClass posScanner;
        //private IOPOSScanner posScanner;

        private string deviceName; // The device name, e.g. TM-H6000II

        public delegate void scannerMessageDelegate(ScanInfo scanInfo);

        public event scannerMessageDelegate ScannerMessageEvent;

        private Timer scannerMessageTimer; // Used to raise the ScannerMessageEvent. This is needed so the scanner is available for use in the Barcode service

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

        public OPOSScanner(string deviceName)
        {
            this.deviceName = deviceName;

            scannerMessageTimer = new Timer();
            scannerMessageTimer.Interval = 1;
            scannerMessageTimer.Tick += ScannerMessageTimerOnTick;
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
            if (posScanner != null)
            {
                posScanner.ReleaseDevice();
                posScanner.Close();
            }
        }

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            posScanner = new OPOSScannerClass();

            posScanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(posScanner_DataEvent);

            posScanner.Open(deviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            posScanner.ClaimDevice(10000);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            if (posScanner.State != (int) OPOS_Constants.OPOS_S_CLOSED)
            {
                posScanner.DeviceEnabled = true;
            }
        }

        /// <summary>
        /// Configures the OPOS device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void Configure()
        {
            posScanner.AutoDisable = true;
            posScanner.DecodeData = true;
            posScanner.DataEventEnabled = true;
            if (posScanner.DeviceEnabled == false)
            {
                posScanner.DeviceEnabled = true;
            }
        }

        private void CheckState()
        {
            switch (posScanner.State)
            {
                case (int) OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception("The scanner is closed...");
                case (int) OPOS_Constants.OPOS_S_ERROR:
                    break;
                case (int) OPOS_Constants.OPOS_S_IDLE:
                    break;
            }
        }

        private void CheckResultCode()
        {
            switch (posScanner.ResultCode)
            {

                case (int) OPOS_Constants.OPOS_SUCCESS:
                    //throw new Exception("The scanner is closed...");
                    break;
                case (int) OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.ScannerIsClosesd);
                case (int) OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.ScannerIsClosesd);
                case (int) OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Properties.Resources.ScannerIsClosesd);
                case (int) OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.ScannerIsIllegallyClosed);
                case (int) OPOS_Constants.OPOS_E_EXTENDED:
                    if (posScanner.ResultCodeExtended == (int) OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.ScannerIsClosesd);
                    }

                    break;
                case (int) OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.ScannedTimedOut);
                default:
                    break;
            }
        }

        public void posScanner_DataEvent(int Status)
        {
            ScanInfo scanInfo = new ScanInfo(posScanner.ScanDataLabel);
            scanInfo.ScanData = posScanner.ScanData;
            scanInfo.ScanDataType = posScanner.ScanDataType;
            scanInfo.EntryType = BarCode.BarcodeEntryType.SingleScanned;            

            // The reason for the timer is that if you want to be able to receive scanner events from a dialog that is shown from the Barcode service,
            // we must return from the posScanner_DataEvent handler to make the barcode scanner available for use. The timer will run as soon as possible but
            // this ensures that whatever custom dialogs or functionality that is run as a direct result of a barcode scan can still listen for scanner events.
            scannerMessageTimer.Tag = scanInfo;
            scannerMessageTimer.Start();
        }

        public void DisableForScan()
        {
            posScanner.DeviceEnabled = false;
            posScanner.DataEventEnabled = false;
        }

        // After each scan the scanner disables itself.  When the scanned data has been processed
        // we need to call this function to reenable the scanner.
        public void ReEnableForScan()
        {
            posScanner.DataEventEnabled = true;
            posScanner.DeviceEnabled = true;
        }

        private void ScannerMessageTimerOnTick(object sender, EventArgs eventArgs)
        {
            scannerMessageTimer.Stop();
            ScannerMessageEvent((ScanInfo)scannerMessageTimer.Tag);
        }
    }
}
