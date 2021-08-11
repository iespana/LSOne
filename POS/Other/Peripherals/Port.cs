using System;
using System.Runtime.InteropServices;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals
{
    public class Port
    {
        public struct DCB
        {
            public Int32 DCBlength;
            public Int32 BaudRate;
            public Int32 fBitFields; //'See Comments in Win32API.Txt
            public Int16 wReserved;
            public Int16 XonLim;
            public Int16 XoffLim;
            public Byte ByteSize;
            public Byte Parity;
            public Byte StopBits;
            public Byte XonChar;
            public Byte XoffChar;
            public Byte ErrorChar;
            public Byte EofChar;
            public Byte EvtChar;
            public Int16 wReserved1; // 'Reserved; Do Not Use
        }

        public struct COMMTIMEOUTS
        {
            public Int32 ReadIntervalTimeout;
            public Int32 ReadTotalTimeoutMultiplier;
            public Int32 ReadTotalTimeoutConstant;
            public Int32 WriteTotalTimeoutMultiplier;
            public Int32 WriteTotalTimeoutConstant;
        }


        public Int32 GENERIC_READ = -2147483648; //System.Convert.ToUInt32("H80000000", 16); //&H80000000;
        public Int32 GENERIC_WRITE = 1073741824; //1073741824; //System.Convert.ToInt32("&H40000000", 16); //&H40000000;
        public const Int32 OPEN_EXISTING = 3;
        public const Int32 FILE_ATTRIBUTE_NORMAL = 128; //&H80;
        public const Int32 NOPARITY = 0;
        public const Int32 ONESTOPBIT = 0;

        [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto)]
        public static extern System.IntPtr CreateFile(string lpFileName, Int32 dwDesiredAccess, Int32 dwSharedMode, IntPtr lpSecurityAttributes,
               Int32 dwCreationDisposition, Int32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "GetCommState", CharSet = CharSet.Auto)]
        public static extern bool GetCommState(IntPtr nCid, ref DCB lpDCB);

        [DllImport("kernel32.dll", EntryPoint = "SetCommState", CharSet = CharSet.Auto)]
        public static extern bool SetCommState(IntPtr nCid, ref DCB lpDCB);

        [DllImport("kernel32.dll", EntryPoint = "GetCommTimeouts", CharSet = CharSet.Auto)]
        public static extern bool GetCommTimeouts(IntPtr hFile, ref COMMTIMEOUTS lpCommTimeouts);

        [DllImport("kernel32.dll", EntryPoint = "SetCommTimeouts", CharSet = CharSet.Auto)]
        public static extern bool SetCommTimeouts(IntPtr hFile, ref COMMTIMEOUTS lpCommTimeouts);

        [DllImport("kernel32.dll", EntryPoint = "WriteFile", CharSet = CharSet.Auto)]
        public static extern bool WriteFile(IntPtr hFile, Byte[] lpBuffer, Int32 nNumberOfBytesToWrite, ref Int32 lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "ReadFile", CharSet = CharSet.Auto)]
        public static extern bool ReadFile(IntPtr hFile, Byte lpBuffer, Int32 nNumberOfBytesToRead, Int32 lpNumberOfBytesToRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr hObject);


        private Byte[] GetBuffer(string text)
        {
            Byte[] buffer = new Byte[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                buffer[i] = Convert.ToByte(text[i]);
            }

            return buffer;
        }

        public void WriteToPort(string textToPrint, string port)
        {
            IntPtr hParallelPort = new IntPtr();
            Boolean Success = true;
            DCB MyDCB = new DCB();
            COMMTIMEOUTS MyCommTimeouts = new COMMTIMEOUTS();
            Int32 BytesWritten = 0;
            Byte[] buffer = new Byte[5];
            //Dim Buffer() As Byte

            try
            {
                hParallelPort = CreateFile(port, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                //' Verify that the obtained handle is valid.
                if (hParallelPort.ToInt32() == -1)
                {
                    throw new Exception("Unable to obtain a handle to the LPT1 port");
                }

                MyDCB.BaudRate = 9600;
                MyDCB.ByteSize = 8;
                MyDCB.Parity = NOPARITY;
                MyDCB.StopBits = ONESTOPBIT;


                Success = GetCommTimeouts(hParallelPort, ref MyCommTimeouts);
                if (Success == false)
                {
                    throw new Exception("Unable to retrieve current time-out settings");
                }

                /*
                byte letter = 27;
                Buffer[0] = letter;
                letter = 112;
                Buffer[1] = letter;
                letter = 0;
                Buffer[2] = letter;
                letter = 100;
                Buffer[3] = letter;
                letter = 100;
                Buffer[4] = letter;
                */

                buffer = GetBuffer(textToPrint);
                int length = buffer.Length;

                Success = WriteFile(hParallelPort, buffer, length, ref BytesWritten, IntPtr.Zero);
                if (Success == false)
                {
                    throw new Exception("Unable to write to " + port);
                }
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, x.Message, x);
            }
            finally
            {
                //' Release the handle to LPT1.
                Success = CloseHandle(hParallelPort);
                if (Success == false)
                {
                    Console.WriteLine("Unable to release handle to LPT1");
                }
            }
        }
    }
}
