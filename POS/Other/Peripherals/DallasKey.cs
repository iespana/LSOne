using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace LSOne.Peripherals
{
    public static class DallasKey
    {
        private static List<string> messages;
        private static SerialPort port;
        private static bool deviceActive;

        public static bool NewMessages { get; set; }

        public static List<string> Messages
        {
            get
            {
                if (messages == null)
                {
                    messages = new List<string>();
                }
                return messages;
            }
        }

        public static void Load()
        {
            if (!DLLEntry.Settings.HardwareProfile.DallasKeyConnected || deviceActive)
            {
                return;
            }
            port = new SerialPort();
            messages = new List<string>();
            port.BaudRate = DLLEntry.Settings.HardwareProfile.DallasBaudRate;
            port.PortName = DLLEntry.Settings.HardwareProfile.DallasComPort;
            port.Parity = DLLEntry.Settings.HardwareProfile.DallasParity;
            port.DataBits = DLLEntry.Settings.HardwareProfile.DallasDataBits;
            port.StopBits = DLLEntry.Settings.HardwareProfile.DallasStopBits;
            port.DataReceived += PortOnDataReceived;
            port.Open();
            deviceActive = true;
        }

        private static void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            string message = "";
            try
            {
                while (port.BytesToRead > 0)
                {
                    message = port.ReadLine().Trim();
                    if(message.StartsWith(DLLEntry.Settings.HardwareProfile.DallasMessagePrefix))
                    {
                        message = message.Remove(0, DLLEntry.Settings.HardwareProfile.DallasMessagePrefix.Length);
                    }
                    messages.Add(message);
                    NewMessages = true;
                }
            }
            catch (TimeoutException)
            {
            }
        }

        public static void UnLoad()
        {
            if(deviceActive)
            {
                port.Close();
                deviceActive = false;
            }
        }
    }
}
