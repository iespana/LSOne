using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// This class determines wether the computer is a 32 or 64-bit. 
    /// </summary>
    public class Detecting64bit
    {
        /// <summary>
        /// This function maps the specified DLL file into the address space of the calling process.
        /// </summary>
        /// <param name="libraryName">The name specified is the file name of the module to be loaded</param>
        /// <returns>A handle to the module indicates success. NULL indicates failure. </returns>
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr LoadLibrary(string libraryName);

        /// <summary>
        /// This function returns the address of the specified exported DLL function.
        /// </summary>
        /// <param name="hwnd">Handle to the DLL module that contains the function.</param>
        /// <param name="procedureName">String containing the function name or specifies the funtion's ordinal value</param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public extern static IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        #region Public Functions

        /// <summary>
        /// Returns true if the computer is a 64 bit computer
        /// </summary>
        /// <returns>Returns true if the computer is a 64 bit computer</returns>
        public bool IsOS64Bit()
        {
            if (IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor()))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Private functions

        private IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                {
                    return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)fnPtr, typeof(IsWow64ProcessDelegate));
                }
            }

            return null;
        }

        private bool Is32BitProcessOn64BitProcessor()
        {
            IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

            if (fnDelegate == null)
            {
                return false;
            }

            bool isWow64;
            bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

            if (retVal == false)
            {
                return false;
            }

            return isWow64;
        }

        #endregion

    }
}
