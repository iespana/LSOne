using System;
using System.Text;

namespace LSOne.Peripherals.OPOS
{
    public static class OPOSCommon
    {
        public static string ConvertToBCD(string data, int charSet)
        {
            byte[] bytes = Encoding.GetEncoding(charSet).GetBytes(data);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                var s = Convert.ToString((int)b);
                s = s.PadLeft(3, '0');
                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}
