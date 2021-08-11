using System;
using System.Security;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// Contains webservice binding configuration settings
    /// </summary>
    public class WebserviceConfiguration
    {
        public string Host { get; set; }
        public bool EnableTcp { get; set; }
        public bool EnableHttp { get; set; }
        public UInt16 TcpPort { get; set; }
        public UInt16 HttpPort { get; set; }
        public bool UseHttps { get; set; }
        public SecureString SSLThumbnail { get; set; }

        public WebserviceConfiguration()
        {
            TcpPort = 0;
            HttpPort = 0;
            UseHttps = false;
            SSLThumbnail = new SecureString();
            Host = "";
            EnableHttp = false;
            EnableTcp = false;
        }
    }
}
