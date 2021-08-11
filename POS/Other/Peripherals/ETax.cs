using LSOne.Utilities.ErrorHandling;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace LSOne.Peripherals
{
    static public class ETax
    {
        static private bool deviceActive = false;
        private static readonly TimeSpan TIMEOUT = TimeSpan.FromMilliseconds(3000);

        public static void Connect(ETaxSerialPort port, int round = 0)
        {
            const string notConnectedMsg = "eTax not connected to port";
            if (round >= 3)
                throw new Exception(notConnectedMsg);

            port.Write(ControlByte.ENQ.ToString());
            var r = ReadResponse(port);
            if (r == Response.NACK)
                Connect(port, ++round);
            else if (r != Response.ACK)
                throw new Exception(notConnectedMsg);
        }

        public static string GetInfo(ETaxSerialPort port)
        {
            Connect(port);

            var command = ControlByte.STX + "I" + ControlByte.ETX;
            var checksum = CalculateChecksum(command);

            port.Write(command, checksum);
            var r = ReadResponse(port);
            return r.ParsedData;
        }

        internal static void SetNewPortSettings(ETaxSerialPort port)
        {
            var ps = PortSettings.GetDefault();
            Connect(port);

            var par = ps.Parity == Parity.None
                          ? "NO"
                          : ps.Parity == Parity.Even
                                ? "EVEN"
                                : "ODD";

            var sb = new StringBuilder();
            sb.Append(ControlByte.STX);
            sb.Append("C");
            sb.Append(ps.Baud.ToString().PadRight(8, ' '));
            sb.Append(ps.HardwareFlowControl ? "HARD    " : "NONE    ");
            sb.Append(par.PadRight(8, ' '));
            sb.Append(ps.StopBits == StopBits.One ? "1       " : "2       ");
            sb.Append(ps.DataBits.ToString().PadRight(8, ' '));
            sb.Append(ControlByte.ETX);

            var command = sb.ToString();
            var checksum = CalculateChecksum(command);
            port.Write(command, checksum);

            var r = ReadResponse(port);
            if (r != Response.ACK)
                throw new Exception();
        }

        internal static string GetTestFiscalSignature(ETaxSerialPort port)
        {
            string queryData = GetQueryTestValueForSaleReceiptOf5();

            string signature = string.Empty;
            int tryCount = 0;
            while (tryCount++ < 10 && string.IsNullOrEmpty(signature))
            {
                signature = ETax.GetFiscalSignature(port, queryData);
            }
            return signature;
        }

        internal static string GetFiscalSignature(ETaxSerialPort port, string queryData)
        {
            Connect(port);

            var command = ControlByte.STX
                + "H"
                + queryData // e.g. "2019053012071234567890ECRABC1234567890P00010000002ovning          0,00        110,0010,00;         10,00 0,00;          0,00 0,00;          0,00 0,00;          0,00"
                + ControlByte.ETX;
            var checksum = CalculateChecksum(command);

            port.Write(command, checksum);

            var r = ReadResponse(port);

            return r.ParsedData;
        }

        private static string GetQueryTestValueForSaleReceiptOf5()
        {
            string transDate = "20190530"; // transaction date e.g. '20090403'
            string transTime = "1244"; // transaction time e.g. '0230'
            string receiptId = "001000000216";//PadLeft(12);
            string terminalId = "P0001".PadLeft(16);
            string companyId = "0000022233";

            string query =
                transDate //"20090403" // transaction date e.g. '20090403'
                + transTime //"0230" // transaction time e.g. '0230'
                + companyId // id code of the company or personal id code e.g. '1234567890'
                + terminalId // "ECRABC1234567890" // cash register marking e.g. 'ECRABC1234567890'
                + receiptId // "         113" // receipt number e.g. '113'

                + "normal" // receipt type (normal, kopia, ovning, profo) e.g. ' profo'. Note: IF profo OR ovning => ACK; IF normal OR kopia => Response data
                + "          0,00" // ** return amount e.g. '   1500,12'
                + "         10,00" // sale amount e.g. '    2500,12'   <--------------------------- sale amount here

                + " 5,00" // 1st VAT batch as percentage e.g. ' 12,50' <---------------------------VAT %
                + ";"
                + "          0,48" // amount of 1st VAT batch e.g. '    2000,20' <---------------------------VAT amount from receipt

                + " 0,00" // 2nd VAT batch as percentage e.g. ' 12,50'
                + ";"
                + "          0,00" // amount of 2nd VAT batch e.g. '    2000,20'

                + " 0,00" // 3rd VAT batch as percentage e.g. ' 12,50'
                + ";"
                + "          0,00" // amount of 3rd VAT batch e.g. '    2000,20'

                + " 0,00" // 4th VAT batch as percentage e.g. ' 12,50'
                + ";"
                + "          0,00" // amount of 4th VAT batch e.g. '    2000,20'
                ;

            return query;
        }

        private static Response ReadResponse(ETaxSerialPort port)
        {
            var c = port.Read(TIMEOUT).First();
            if (c == ControlByte.ACK)
                return Response.ACK;
            if (c == ControlByte.NACK)
                return Response.NACK;
            if (c == ControlByte.BUSY)
                return Response.BUSY;
            if (c == ControlByte.STX)
                return GetDataResponse(port);
            throw new Exception("Unknown result");
        }

        private static Response GetDataResponse(ETaxSerialPort port)
        {
            var sb = new StringBuilder();
            sb.Append(ControlByte.STX);

            port.Read(TIMEOUT)
                .TakeWhile(c => c != ControlByte.ETX)
                .ToList()
                .ForEach(c => sb.Append(c));
            sb.Append(ControlByte.ETX);

            var data = sb.ToString();
            var expectedChecksum = CalculateChecksum(data);
            var checksum = String.Join("", port.Read(TIMEOUT).Take(2));
            if (!checksum.Equals(expectedChecksum, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Checksum mismatch");

            return new Response(data);
        }

        private static string CalculateChecksum(string data)
        {
            int x = Encoding.ASCII.GetBytes(data).Aggregate(0, (current, b) => current + b);
            return (x % 256).ToString("X2");
        }

        static public void Load()
        {
            try
            {
                if (!DLLEntry.Settings.HardwareProfile.ETaxConnected || deviceActive)
                {
                    return;
                }
                deviceActive = true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.ETax", x);
            }
        }

        static public void Unload()
        {
            if (!deviceActive) { return; }

            try
            {
                deviceActive = false;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "OPOS.ETax", x);
            }
        }
    }
}

