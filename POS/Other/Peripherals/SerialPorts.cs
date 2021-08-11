using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using LSRetailPosis.Settings.HardwareProfiles;

namespace LSRetailPosis.Peripherals
{
    static public class SerialPorts
    {
        static private SerialPort[] comPorts;

        static public SerialPort[] ComPorts
        {
            get { return comPorts; }
        }

        static public void Load()
        {
            comPorts = new SerialPort[]{new SerialPort(Comport.ComPort.PortName, Comport.ComPort.BaudRate, Comport.ComPort.Parity, Comport.ComPort.DataBits, Comport.ComPort.StopBits)};
        }
    }
}
