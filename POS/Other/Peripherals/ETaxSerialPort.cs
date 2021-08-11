using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace LSOne.Peripherals
{
    /// <summary>
    /// This code is taken from the manufacturer provided connection code example for ETax device
    /// </summary>
    public class ETaxSerialPort : IDisposable
    {
        public static ETaxSerialPort Open(string portname, PortSettings settings)
        {
            var p = new SerialPort(portname, settings.Baud, settings.Parity, settings.DataBits, settings.StopBits);
            p.Handshake = settings.HardwareFlowControl ? Handshake.RequestToSend : Handshake.None;
            p.Open();
            return new ETaxSerialPort(p);
        }

        private SerialPort eTaxSerialPort;

        private ETaxSerialPort(SerialPort port)
        {
            eTaxSerialPort = port;
        }

        public void Dispose()
        {
            var p = eTaxSerialPort;
            eTaxSerialPort = null;
            if (p != null)
                p.Dispose();
        }

        public IEnumerable<char> Read(TimeSpan timeout)
        {
            eTaxSerialPort.ReadTimeout = (int)timeout.TotalMilliseconds;
            for (; ; )
            {
                int ic = eTaxSerialPort.ReadByte();
                if (ic < 0)
                    yield break;
                ic = ic & 127;
                char c = Encoding.ASCII.GetChars(new[] { (byte)ic }, 0, 1)[0];
                yield return c;
            }
        }

        public void Write(params string[] data)
        {
            foreach (var d in data)
            {
                var bytes = Encoding.Default.GetBytes(d);
                eTaxSerialPort.Write(bytes, 0, bytes.Length);
            }
        }
    }

    public class BaudRate
    {
        public int Rate { get; set; }

        public BaudRate(int rate) { Rate = rate; }

        public override string ToString() { return Rate.ToString(); }

        public static BaudRate[] All = new[]
                                           {
                                               new BaudRate(115200),
                                               new BaudRate(57600),
                                               new BaudRate(38400),
                                               new BaudRate(19200),
                                               new BaudRate(9600),
                                           };
    }

    public abstract class Grej
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ParityOptions : Grej
    {
        public Parity NativeValue { get; set; }
        public string EtaxValue { get; set; }

        public ParityOptions(string name, string etaxValue, Parity nativeValue)
        {
            Name = name;
            EtaxValue = etaxValue;
            NativeValue = nativeValue;
        }

        public static ParityOptions[] All = new[]
                                         {
                                             new ParityOptions("No parity", "NO", Parity.None),
                                             new ParityOptions("Even parity", "EVEN", Parity.Even),
                                             new ParityOptions("Odd parity", "ODD", Parity.Odd)
                                         };
    }

    public class FlowControl : Grej
    {
        public bool Value { get; set; }

        public FlowControl(string name, bool value)
        {
            Name = name;
            Value = value;
        }

        public static FlowControl[] All = new[]
                                              {
                                                  new FlowControl("No flow control", false),
                                                  new FlowControl("Hardware flow control", true),
                                              };
    }

    public class StopBit : Grej
    {
        public StopBits NativeValue { get; set; }
        public string EtaxValue { get; set; }

        public StopBit(string name, string etax, StopBits value)
        {
            Name = name;
            EtaxValue = etax;
            NativeValue = value;
        }

        public static StopBit[] All = new[]
                                          {
                                              new StopBit("1 stopbit", "1", StopBits.One),
                                              new StopBit("2 stopbits", "2", StopBits.Two),
                                          };
    }

    public class DataBitlength : Grej
    {
        public int NativeValue { get; set; }
        public string EtaxValue { get; set; }

        public DataBitlength(int length)
        {
            Name = length + " bits";
            EtaxValue = length.ToString();
            NativeValue = length;
        }

        public static DataBitlength[] All = new[]
                                          {
                                              new DataBitlength(8),
                                              new DataBitlength(9),
                                          };
    }

    public class PortSettings
    {
        public int Baud;
        public bool HardwareFlowControl = true;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.One;
        public int DataBits = 8;

        public static PortSettings GetDefault()
        {
            return new PortSettings
            {
                Baud = BaudRate.All[0].Rate,
                DataBits = DataBitlength.All[0].NativeValue,
                HardwareFlowControl = true,
                Parity = ParityOptions.All[0].NativeValue,
                StopBits = StopBit.All[0].NativeValue,
            };
        }
    }

    static class ControlByte
    {
        public const char STX = '\x02';
        public const char ETX = '\x03';
        public const char ENQ = '\x05';
        public const char ACK = '\x06';
        public const char NACK = '\x15';
        public const char BUSY = '\x16';
    }

    internal class Response
    {
        public static readonly Response ACK = new Response();
        public static readonly Response NACK = new Response();
        public static readonly Response BUSY = new Response();

        public string Data = String.Empty;
        public string ParsedData = String.Empty;

        public Response()
        {
        }

        public Response(string data)
        {
            Data = data;
            if (data[1] == 'I')
                ParsedData = ParseInformationResult(data);
            if (data[1] == 'H')
                ParsedData = ParseFiscalSignatureResult(data);
        }

        private static string ParseInformationResult(string data)
        {
            var parts = data.Substring(2, data.Length - 3).Split(';');
            var sb = new StringBuilder();
            try
            {
                sb.Append("Control unit serial number: ").AppendLine(parts[0]);
                sb.Append("Control unit type:          ").AppendLine(parts[1]);
                sb.Append("Control unit datetime:      ").AppendLine(parts[2]);
                sb.Append("Hardware revision:          ").AppendLine(parts[3]);
                sb.Append("Software version:           ").AppendLine(parts[4]);
                sb.Append("Journal card capacity:      ").AppendLine(parts[5]);
                sb.Append("Journal card free memory:   ").AppendLine(parts[6]);
                sb.Append("Journal card file system:   ").AppendLine(parts[7]);
            }
            catch (Exception e)
            {
                sb.Append("ERROR ").AppendLine(e.Message);
            }
            return sb.ToString();
        }

        private static string ParseFiscalSignatureResult(string data)
        {
            var parts = data.Substring(2, data.Length - 3).Split(';');
            var sb = new StringBuilder();
            try
            {
                sb.Append("Control unit serial number: ").AppendLine(parts[0]);
                sb.Append("Signature code:             ").AppendLine(parts[1]);
                sb.Append("Encryption code:            ").AppendLine(parts[2]);
            }
            catch (Exception e)
            {
                sb.Append("ERROR ").AppendLine(e.Message);
            }
            return sb.ToString();
        }
    }
}
