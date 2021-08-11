using System;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Properties;
using POS.Devices;
using LSOne.Utilities.IO.JSON;
using LSOne.Utilities.ErrorHandling;
using System.Threading;

namespace LSOne.Services.OPOS
{
    /// <summary>
    /// Lower-level class to interact with the OPOS scale.
    /// </summary>
    /// <remarks>
    /// Scales are always used in synchronous mode because the OPOS object does not expose if the scale supports async or not.
    /// </remarks>
    public class OPOSScale
    {
        #region Member variables

        private OPOSScaleClass posScale;        

        /// <summary>
        /// The device name, e.g. TM-H6000II
        /// </summary>
        private string deviceName;

        public delegate void scaleDataMessageDelegate(int weight);
        public event scaleDataMessageDelegate ScaleDataMessageEvent;

        public delegate void scaleErrorMessageDelegate(string message);
        public event scaleErrorMessageDelegate ScaleErrorMessageEvent;

        #endregion

        #region Properties

        public string DeviceName
        {
            set { deviceName = value; }
        }


        #endregion

        public OPOSScale(string deviceName)
        {
            this.deviceName = deviceName;
        }

        public void Initialize()
        {
            // Open can only be called once, else some scales will fail to operate.
            Open();
            Claim();
            PreConfigure();
            Enable(); 
            PostConfigure();
            DLLEntry.DataModel.ErrorLogger.LogMessageToFile(LogMessageType.Trace, $"Scale initialized => {this.ToJsonString()}", "OPOSScale.Initialize");
        }
        
        public void Finalise()
        {
            if (posScale != null)
            {
                posScale.ReleaseDevice();
                posScale.Close();
            }
        }

        /// <summary>
        /// Searches for the correct OPOS device and opens up a connection to it.
        /// </summary>
        private void Open()
        {
            posScale = new OPOSScaleClass();

            posScale.DataEvent += ScaleDataEvent;
            posScale.ErrorEvent += posScale_ErrorEvent;
            posScale.StatusUpdateEvent += ScaleStatusUpdateEvent;

            posScale.Open(deviceName);

            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Claims the OPOS device.
        /// </summary>
        private void Claim()
        {
            posScale.ClaimDevice(10000);
            
            CheckState();
            CheckResultCode();
        }

        /// <summary>
        /// Enables the OPOS device.
        /// </summary>
        public void Enable()
        {
            if (posScale.State != (int)OPOS_Constants.OPOS_S_CLOSED)
            {
                posScale.DeviceEnabled = true;
            }
        }

        /// <summary>
        /// Disables the OPOS device.
        /// </summary>
        public void Disable()
        {
            if (posScale.State != (int)OPOS_Constants.OPOS_S_CLOSED)
            {
                posScale.DeviceEnabled = false;
            }
        }


        /// <summary>
        /// Configures the OPOS device before enabling the device.
        /// All configuration pre Open-Claim-Enable is performed here.
        /// </summary>
        /// <remarks>
        /// From UnifiedPOS v1.14 documentation:
        /// <list type="bullet">
        /// <item><b>PowerNotify</b> may only be set while the device is disabled; that is, while <b>DeviceEnabled</b> is false.</item>
        /// </list>
        /// </remarks>
        private void PreConfigure()
        {
            posScale.PowerNotify = (int)OPOS_Constants.OPOS_PN_ENABLED;
            posScale.StatusNotify = 1;
        }

        /// <summary>
        /// Configures the OPOS device after enabling the device.
        /// All configuration post Open-Claim-Enable is performed here.
        /// </summary>
        private void PostConfigure()
        {
            posScale.AsyncMode = false;
            posScale.AutoDisable = true;
            posScale.DataEventEnabled = true;
        }

        private void CheckState()
        {
            switch (posScale.State)
            {
                case (int)OPOS_Constants.OPOS_S_CLOSED:
                    //throw new Exception("The scale is closed...");
                    break;
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
            switch (posScale.ResultCode)
            {
                case (int)OPOS_Constants.OPOS_SUCCESS:
                    break;
                case (int)OPOS_Constants.OPOS_E_CLOSED:
                    throw new Exception(Resources.ScaleIsClosed);
                case (int)OPOS_Constants.OPOS_E_CLAIMED:
                    throw new Exception(Resources.ScaleIsClosed);
                case (int)OPOS_Constants.OPOS_E_NOTCLAIMED:
                    throw new Exception(Resources.ScaleIsClosed);
                case (int)OPOS_Constants.OPOS_E_ILLEGAL:
                    throw new Exception(Resources.ScaleIllegallyClosed);
                case (int)OPOS_Constants.OPOS_E_NOSERVICE:
                    throw new Exception(Resources.ScaleNoService);
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                    if (posScale.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                        throw new Exception(Resources.ScaleIsClosed);
                    }
                    break;
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    throw new Exception(Resources.ScaleTimedOut);
                case (int)OPOS_Constants.OPOS_E_DISABLED:
                    throw new Exception(Resources.ScaleIsDisabled);
            }
        }


        private void posScale_ErrorEvent(int resultCode, int resultCodeExtended, int errorLocus, ref int pErrorResponse)
        {
            switch (resultCode)
            {
                case (int)OPOS_Constants.OPOS_SUCCESS:
                    break;
                case (int)OPOS_Constants.OPOS_E_CLOSED:
                    ScaleErrorMessageEvent(Resources.ScaleIsClosed);
                    break;
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                    if (posScale.ResultCodeExtended == (int)OPOSPOSPrinterConstants.OPOS_EPTR_COVER_OPEN)
                    {
                    }

                    if (posScale.ResultCodeExtended == (int)OPOSScaleConstants.OPOS_ESCAL_UNDER_ZERO
                       || posScale.ResultCodeExtended == (int)OPOSScaleConstants.OPOS_ESCAL_OVERWEIGHT
                       || posScale.ResultCodeExtended == (int)OPOSScaleConstants.OPOS_ESCAL_SAME_WEIGHT)
                    {
                        ScaleErrorMessageEvent(Resources.ScaleInvalidWeight);
                    }

                    break;
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    ScaleErrorMessageEvent(Resources.NoInputDetectedCheckScale);
                    break;
                case (int)OPOS_Constants.OPOS_E_NOHARDWARE:
                    ScaleIsDisabled(0);

                    break;
            }
        }

        private void ScaleStatusUpdateEvent(int status)
        {
            //DLLEntry.DataModel.ErrorLogger.LogMessageToFile(LogMessageType.Trace, $"[tid{Thread.CurrentThread.ManagedThreadId}][{DateTime.Now.ToString("O")}] ScaleStatusUpdateEvent was raised with status {status} and Scale is {ToJsonString()}", "OPOSScale.ScaleStatusUpdateEvent");
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        private void ScaleDataEvent(int status)
        {
            ScaleDataMessageEvent(status);
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        public int ScaleUnit()
        {
            return posScale.WeightUnit;
        }

        public int TareWeight()
        {
            return posScale.TareWeight;
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>                
        public bool ReadFromScaleEx(out int weight, int timeout, string weightUnit, string description, decimal unitPrice, int tareWeight, out decimal salesPrice)
        {
            posScale.DeviceEnabled = true;
            posScale.DataEventEnabled = true;
            posScale.AutoDisable = true;

            weight = 0;
            salesPrice = 0;
            int readResult; //OPOS result from DisplayText and readWeight operations
            bool result;

            if (posScale.CapDisplayText)
            {
                readResult = posScale.DisplayText(description.Substring(0, posScale.MaxDisplayTextChars));
                result = HandleResult(readResult, tareWeight);
                if (!result)
                {                    
                    return false;
                }
            }

            if (posScale.CapPriceCalculating)
            {
                posScale.UnitPrice = unitPrice;
            }

            if (posScale.CapTareWeight)
            {
                posScale.TareWeight = tareWeight;
            }


            if (timeout == 0)
            {
                timeout = 10000; 
            }
            else if (timeout < 0)
            {
                timeout = -1; //Infinite
            }

            readResult = posScale.ReadWeight(out weight, timeout);
            result = HandleResult(readResult, tareWeight);
            if (!result)
            {                
                return false;
            }

            if (posScale.CapPriceCalculating)
            {
                salesPrice = posScale.SalesPrice;
            }

            return true;
        }

        public void ScaleIsDisabled(int tareWeight)
        {
            DialogResult result;
            
            if (DLLEntry.Settings.HardwareProfile.ScaleManualInputAllowed)
            {
                IDialogService dialogService = ((IDialogService) DLLEntry.DataModel.Service(ServiceType.DialogService));
                result = dialogService.ShowMessage(
                Resources.ScaleDisabledEnterManually,
                MessageBoxButtons.OKCancel, MessageDialogType.ErrorWarning);
                if (result == DialogResult.OK)
                {
                    decimal input = decimal.Zero;

                    dialogService.NumpadInput(DLLEntry.Settings, ref input, Resources.EnterWeight, Resources.Weight, true, DecimalSettingEnum.Quantity);

                    var weight = input - (tareWeight > 0 ? tareWeight / 1000m : 0);

                    //We multiply with 1000 so that the weight that is entered is the actual weight that is displayed & calculated
                    ScaleDataEvent(Convert.ToInt32(weight * 1000));
                }
                else
                {
                    ScaleErrorMessageEvent(Resources.ReconnectScale);
                    
                }

            }
            else
            {
                ScaleErrorMessageEvent(Resources.ScaleIsDisabledReconnect);
            }

        }
        public bool HandleResult(int result, int tareWeight)
        {
            bool isError = true;
            
            switch (result)
            {
                case (int)OPOS_Constants.OPOS_SUCCESS:
                    isError = false;
                    
                    break;
                case (int)OPOS_Constants.OPOS_E_ILLEGAL:
                    ScaleErrorMessageEvent(Resources.ScaleIllegallyClosed);
                    
                    break;
                case (int)OPOS_Constants.OPOS_E_NOSERVICE:
                    ScaleErrorMessageEvent(Resources.ScaleNoService);
                    
                    break;
                case (int)OPOS_Constants.OPOS_E_TIMEOUT:
                    //some scales like Mettler-Toledo report timeout when scale is disconnected in the middle of the session
                    //but read timeout is not an edge-case like disconnecting the scale while doing a sale
                    //ScaleErrorMessageEvent(Resources.ScaleIsDisabledReconnect); 
                    
                    break;
                case (int)OPOS_Constants.OPOS_E_EXTENDED:
                case (int)OPOSScaleConstants.OPOS_ESCAL_UNDER_ZERO:
                case (int)OPOSScaleConstants.OPOS_ESCAL_OVERWEIGHT:
                case (int)OPOSScaleConstants.OPOS_ESCAL_SAME_WEIGHT:
                    //some scales like Mettler-Toledo are returning an extended error code when read times out
                    //so it should be treated as a timeout
                    //ScaleErrorMessageEvent(Resources.ScaleInvalidWeight);

                    break;
                case (int)OPOS_Constants.OPOS_E_NOHARDWARE: 
                case (int)OPOS_Constants.OPOS_E_FAILURE:
                case (int)OPOS_Constants.OPOS_E_DISABLED:
                    ScaleIsDisabled(tareWeight);
                    isError = false;

                    break;
            }

            return !isError;
        }

        /// <summary>
        /// Serializes the OPOS object properties to json.
        /// Useful for logging and debugging purposes.
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            var logObj = new
            {
                hwName = deviceName,
                scale = new
                {
                    posScale?.OpenResult,
                    posScale?.CheckHealthText,
                    posScale?.Claimed,
                    posScale?.DeviceEnabled,
                    posScale?.FreezeEvents,
                    posScale?.ResultCode,
                    posScale?.ResultCodeExtended,
                    posScale?.State,
                    posScale?.ControlObjectDescription,
                    posScale?.ControlObjectVersion,
                    posScale?.ServiceObjectDescription,
                    posScale?.ServiceObjectVersion,
                    posScale?.DeviceDescription,
                    posScale?.DeviceName,
                    posScale?.MaximumWeight,
                    posScale?.WeightUnit,
                    posScale?.WeightUnits,
                    posScale?.BinaryConversion,
                    posScale?.CapDisplay,
                    posScale?.AutoDisable,
                    posScale?.CapPowerReporting,
                    posScale?.DataCount,
                    posScale?.DataEventEnabled,
                    posScale?.PowerNotify,
                    posScale?.PowerState,
                    posScale?.AsyncMode,
                    posScale?.CapDisplayText,
                    posScale?.CapPriceCalculating,
                    posScale?.CapTareWeight,
                    posScale?.CapZeroScale,
                    posScale?.MaxDisplayTextChars,
                    posScale?.SalesPrice,
                    posScale?.TareWeight,
                    posScale?.UnitPrice,
                    posScale?.CapStatisticsReporting,
                    posScale?.CapUpdateStatistics,
                    posScale?.CapCompareFirmwareVersion,
                    posScale?.CapUpdateFirmware,
                    posScale?.CapStatusUpdate,
                    posScale?.ScaleLiveWeight,
                    posScale?.StatusNotify,
                    posScale?.ZeroValid,
                    posScale?.CapFreezeValue,
                    posScale?.CapReadLiveWeightWithTare,
                    posScale?.CapSetPriceCalculationMode,
                    posScale?.CapSetUnitPriceWithWeightUnit,
                    posScale?.CapSpecialTare,
                    posScale?.CapTarePriority,
                    posScale?.MinimumWeight
                }
            };

            return JsonConvert.SerializeObject(logObj);
        }
    }


}
