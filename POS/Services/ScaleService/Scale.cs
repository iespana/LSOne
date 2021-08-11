using System;
using LSOne.Services.OPOS;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    static public class Scale
    {
        static private OPOSScale posScale;

        static private bool deviceActive = false;

        public delegate void ScaleDataMessageDelegateX(decimal weight);
        static public event ScaleDataMessageDelegateX ScaleDataMessageEventX;

        public delegate void ScaleErrorMessageDelegateX(string message);
        static public event ScaleErrorMessageDelegateX ScaleErrorMessageEventX;

        static public bool DeviceActive
        {
            get { return deviceActive; }
        }

        internal static void Load()
        {
            try
            {
                if (!DLLEntry.Settings.HardwareProfile.ScaleConnected || deviceActive)
                {
                    return;
                }

                deviceActive = true;

                posScale = new OPOSScale(DLLEntry.Settings.HardwareProfile.ScaleDeviceName);
                posScale.ScaleDataMessageEvent += ScaleDataEvent;
                posScale.ScaleErrorMessageEvent += posScale_ScaleErrorMessageEvent;
               
                posScale.Initialize();
            }
            catch (Exception x)
            {
                posScale = null;

                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Scale", x);
                throw;
            }
        }

        internal static void Unload()
        {
            if (!deviceActive) { return; }

            try
            {
                posScale.Finalise();
                deviceActive = false;
            }
            catch (Exception x)
            {
                posScale = null;

                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.Scale", x);
                throw;
            }
        }

        static void posScale_ScaleErrorMessageEvent(string message)
        {
            ScaleErrorMessageEventX?.Invoke(message);
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        static void ScaleDataEvent(int weight)
        {
            ScaleDataMessageEventX?.Invoke(weight / 1000m);
        }

        /// <summary>
        /// ***********************************************************************************
        /// This method is a part of scale cetification. Changes will affect and void the scale certification hash codes.
        /// ***********************************************************************************
        /// </summary>
        internal static int GetScaleUnit()
        {
            return posScale.ScaleUnit();
        }

        internal static int GetTareWeight()
        {
            return posScale.TareWeight();
        }

        internal static void EnableScale(bool enable)
        {
            if (enable)
            {
                posScale.Enable();
            }
            else
            {
                posScale.Disable();
            }
        }

        public static bool ReadFromScaleEx(out int weight, int timeout, string weightUnit, string description, decimal unitPrice, int tareWeight, out decimal salesPrice)
        {
            return posScale.ReadFromScaleEx(out weight, timeout, weightUnit, description, unitPrice, tareWeight, out salesPrice);
        }

        /// <summary>
        /// Returns the <see cref="Scale"/> properties as json, including the underlying <see cref="LSOne.Services.OPOS.OPOSScale"/>.
        /// Useful for logging and debugging purposes.
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString()
        {
            return $"{{\"opos\": {posScale.ToJsonString()}, \"deviceActive\": {deviceActive.ToString().ToLower()}}}";
        }
    }
}
