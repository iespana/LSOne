using System;
using POS.Devices;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSLineDisplay : OPOSBase
    {
        
        #region Member variables

        private OPOSLineDisplayClass posLineDisplay;

        private string deviceName;                                // The device name, e.g. DM-D106
        private int characterSet;
        private string lastDisplayLine1 = "";
        private string lastDisplayLine2 = "";
       
        #endregion

        #region Properties

        public string DeviceName
        {
            set { deviceName = value; }
        }

        public int CharacterSet
        {
            set { characterSet = value; }
        }

        // Returns the number of columns in a line
        public int Columns
        {
            get { return posLineDisplay.Columns; }
        }

        #endregion

        public override void Initialize()
        {
            Open();
            Claim();
            Enable();
            Configure();
            lastDisplayLine1 = "";
            lastDisplayLine2 = "";
        }

        public override void Finalise()
        {
            if (posLineDisplay != null)
            {
                posLineDisplay.ReleaseDevice();
                posLineDisplay.Close();
            }
        }

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            posLineDisplay = new OPOSLineDisplayClass();
            Configure();
            posLineDisplay.Open(deviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            posLineDisplay.ClaimDevice(10000);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        private void Enable()
        {
            if (posLineDisplay.State != (int) OPOS_Constants.OPOS_S_CLOSED)
            {
                posLineDisplay.DeviceEnabled = true;
            }
        }

        /// <summary>
        /// Configures the OPOS device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void Configure()
        {
            posLineDisplay.CharacterSet = this.characterSet;
            posLineDisplay.MapCharacterSet = DLLEntry.Settings.HardwareProfile.DisplayBinaryConversion;
        }


        private void CheckState()
        {
            switch (posLineDisplay.State)
            {
                   
                case (int)OPOS_Constants.OPOS_S_CLOSED:
                    throw new Exception(Properties.Resources.DisplayIsClosed);
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

            switch ((OPOS_Constants)posLineDisplay.ResultCode)
            {
                case OPOS_Constants.OPOS_SUCCESS:
                    //throw new Exception("The printer is closed...");
                    break;
                case OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Properties.Resources.DisplayIsClosed);
                case OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Properties.Resources.DisplayIsClosed);
                case OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Properties.Resources.DisplayIsClosed);
                case OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Properties.Resources.DisplayIsIllegallyClosed);
                case OPOS_Constants.OPOS_E_EXTENDED:
                    if (posLineDisplay.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Properties.Resources.DisplayIsClosed);
                    }
                    break;
                case OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Properties.Resources.DisplayTimedOut);
            }
        }

        /// <summary>
        /// Displays a text on the line display 
        /// </summary>
        /// <param name="textToDisplay"> The text to display on the line display</param>
        public void DisplayText(string textToDisplay)
        {
            textToDisplay = textToDisplay ?? "";

            if (textToDisplay.Trim() == lastDisplayLine1.Trim())
            {
                return;
            }

            if (DLLEntry.Settings.HardwareProfile.DisplayBinaryConversion)
            {
                posLineDisplay.BinaryConversion = 2; // OposBcDecimal

                string convertedTextToDisplay = OPOSCommon.ConvertToBCD(textToDisplay, this.characterSet);

                posLineDisplay.DisplayText(convertedTextToDisplay, (int) OPOSLineDisplayConstants.DISP_DT_NORMAL);
                posLineDisplay.BinaryConversion = 0; // OposBcNone
            }
            else
            {
                posLineDisplay.DisplayText(textToDisplay, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
            }

            lastDisplayLine1 = textToDisplay;
            lastDisplayLine2 = string.Empty;

        }

        public void DisplayTextAt(string textToDisplay, int row, int column)
        {
            textToDisplay = textToDisplay ?? "";
            if (row == 0 && lastDisplayLine1.Trim() == textToDisplay)
            {
                return;
            }

            if (row == 1 && lastDisplayLine2.Trim() == textToDisplay)
            {
                return;
            }

            if (DLLEntry.Settings.HardwareProfile.DisplayBinaryConversion)
            {
                posLineDisplay.BinaryConversion = 2; // OposBcDecimal

                string convertedTextToDisplay = OPOSCommon.ConvertToBCD(textToDisplay, this.characterSet);

                posLineDisplay.DisplayTextAt(row, column, convertedTextToDisplay, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
                posLineDisplay.BinaryConversion = 0; // OposBcNone
            }
            else
            {
                posLineDisplay.DisplayTextAt(row, column, textToDisplay, (int)OPOSLineDisplayConstants.DISP_DT_NORMAL);
            }
            

            lastDisplayLine1 = row == 0 ? textToDisplay : lastDisplayLine1;
            lastDisplayLine2 = row == 1 ? textToDisplay : lastDisplayLine2;
           
        }

        /// <summary>
        /// Clears all text from the linedisplay
        /// </summary>
        public void ClearText(bool clearLastDisplay = false)
        {
           
            posLineDisplay.ClearText();

            if (clearLastDisplay)
            {
                lastDisplayLine1 = "";
                lastDisplayLine2 = "";
            }
        }
    }
}
