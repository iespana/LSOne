using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LSRetail.DD.Common;

namespace LSRetail.StoreController.BusinessObjects.Scheduler
{
    public class DDUtils
    {
        /// <summary>
        /// Get Router Port by network netmode
        /// </summary>
        /// <param name="mode">Network netmode to look for</param>
        /// <returns>Port number that matches the Network netmode</returns>
        public static int GetRouterPortByMode(NetMode mode)
        {
            switch (mode)
            {
                case NetMode.TCP: return -1;
                case NetMode.TCPS: return -1;
            }
            return -1;
        }
        /// <summary>
        /// Generate WCF Address string to listen or connect to Host
        /// </summary>
        /// <param name="host">Host name</param>
        /// <param name="port">Port number</param>
        /// <param name="conf">WCF Service name</param>
        /// <param name="mode">Network Mode</param>
        /// <returns>WCF Connection string</returns>
        static public string GetAddress(string host, int port, string conf, NetMode mode)
        {
            string netadd = "net.tcp";
            switch (mode)
            {
                case NetMode.HTTP:
                    netadd = "http";
                    break;
            }

            string add = string.Format("{0}://{1}:{2}/{3}", netadd, host.ToUpperInvariant(), port, conf);
            //AppConfig.WriteDbg(DebugLevel.DetailL2, "WCF Address: " + add + " M:" + mode.ToString());
            return add;
        }

        /// <summary>
        /// Generate WCF Address string to listen or connect to Host
        /// </summary>
        /// <param name="hostinfo">Host Info object</param>
        /// <param name="conf">WCF Service name</param>
        /// <returns>WCF Connection string</returns>
        static public string GetAddress(SOAPHostInfo hostinfo, string conf)
        {
            return GetAddress(hostinfo.HostName, hostinfo.Port, conf, hostinfo.Mode);
        }
        /// <summary>
        /// Split Host string into Hostname, Socket Port and Network Mode.  Checks also for offline status
        /// </summary>
        /// <param name="hoststr">Host string to split</param>
        /// <param name="hostname">Return Host Name</param>
        /// <param name="port">Return Socket Port</param>
        /// <param name="mode">Return Network Mode</param>
        static public void SplitHostString(string hoststr, out string hostname, out int port, out NetMode mode)
        {
            port = AppConfig.RouterPortTCP;
            mode = NetMode.TCP;

            string[] data;
            if (hoststr[0] == '[')
            {
                // ipv6 address
                int x = hoststr.IndexOf(']');
                x++;
                string ad = hoststr.Substring(0, x);
                hostname = ad.ToUpperInvariant();
                if (hoststr.Length > x)
                {
                    x++;
                    string rest = hoststr.Substring(x, hoststr.Length - x);
                    data = rest.Split(':');

                    // get port number
                    port = Convert.ToInt32(data[0]);
                    if (data.Length == 1)
                        return;

                    // get network mode
                    mode = (NetMode)StringToEnum(typeof(NetMode), data[1]);
                }
            }
            else
            {
                // get host name
                data = hoststr.Split(':');
                hostname = data[0].ToUpperInvariant();
                if (data.Length == 1)
                    return;

                // get port number
                port = Convert.ToInt32(data[1]);
                if (data.Length == 2)
                    return;

                // get network mode
                mode = (NetMode)StringToEnum(typeof(NetMode), data[2]);
            }
        }
        /// <summary>
        /// Convert any possible string-Value of a given enumeration
        /// type to its internal representation.
        /// </summary>
        /// <param name="t">Enum object to convert value to</param>
        /// <param name="Value">Enum Value String</param>
        /// <returns>Enum Value</returns>
        static public object StringToEnum(Type t, string Value)
        {
            return StringToEnum(t, Value, true);
        }
        /// <summary>
        /// Convert any possible string-Value of a given enumeration
        /// type to its internal representation.
        /// </summary>
        /// <param name="t">Enum object to convert value to</param>
        /// <param name="Value">Enum Value String</param>
        /// <param name="usedefault">if false the function returns NULL if value not found in Enum otherwise it returns first Enum value</param>
        /// <returns>Enum Value</returns>
        static public object StringToEnum(Type t, string Value, bool usedefault)
        {
            if (Value == null)
                return null;

            FieldInfo defaultfieldinfo = null;
            foreach (FieldInfo fi in t.GetFields())
            {
                if (fi.Name.Equals(Value, StringComparison.InvariantCultureIgnoreCase))
                    return fi.GetValue(null);   // We use null because enumeration values are static

                // save first value in enum as default if nothing is found
                if (usedefault && defaultfieldinfo == null && fi.FieldType == t)
                    defaultfieldinfo = fi;
            }

            if (defaultfieldinfo == null)
                return null;

            //AppConfig.WriteDbg(DebugLevel.Warning, string.Format("Enum Value {0} is not found in {1}, using default value {2}",
            //    Value, t, defaultfieldinfo.Name));

            return defaultfieldinfo.GetValue(null);
        }

    }
}
