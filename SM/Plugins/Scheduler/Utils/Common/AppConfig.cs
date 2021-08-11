using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSRetail.StoreController.BusinessObjects.Scheduler.Common
{
    public class AppConfig
    {
        ConfigData configdata = new ConfigData();
        /// <summary>
        /// TCPS Router Port
        /// </summary>
        public static int RouterPortTCPS
        {
            get { return -1; }
        }
        /// <summary>
        /// TCP Router Port
        /// </summary>
        public static int RouterPortTCP
        {
            get { return -1; }
        }
        /// <summary>
        /// Get Socket Connection Binding Mode, there are 2 modes, Main netmode and Outgoing Mode
        /// The system can have different incoming and outgoing netmode if needed,
        /// f.ex when sending data between Workgroup and Domain
        /// </summary>
        /// <param name="mode">True gets Main Mode Binding else Outgoing Mode</param>
        /// <param name="filebinding">True if binding is for file transfer</param>
        /// <returns>Returns Binding Object</returns>
        public static Binding GetBinding(NetMode mode, bool filebinding)
        {
            Binding bind;
            switch (mode)
            {
                case NetMode.TCPS:
                    NetTcpBinding stcp = new NetTcpBinding();
                    if (filebinding)
                    {
                        stcp.MaxReceivedMessageSize = configdata.FileSize;
                        stcp.TransferMode = TransferMode.Streamed;
                    }
                    else
                    {
                        stcp.MaxReceivedMessageSize = configdata.DataStringSize;
                        stcp.ReaderQuotas.MaxStringContentLength = configdata.DataStringSize;
                        stcp.ReaderQuotas.MaxBytesPerRead = configdata.DataStringSize;
                    }
                    stcp.ListenBacklog = 100;
                    stcp.MaxConnections = configdata.MaxConnections;
                    bind = stcp;
                    break;

                case NetMode.HTTP:
                    BasicHttpBinding httpb = new BasicHttpBinding();
                    httpb.Security.Mode = BasicHttpSecurityMode.None;
                    httpb.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    if (filebinding)
                    {
                        httpb.MaxReceivedMessageSize = configdata.FileSize;
                        httpb.TransferMode = TransferMode.Streamed;
                    }
                    else
                    {
                        httpb.MaxReceivedMessageSize = configdata.DataStringSize;
                        httpb.ReaderQuotas.MaxStringContentLength = configdata.DataStringSize;
                        httpb.ReaderQuotas.MaxBytesPerRead = configdata.DataStringSize;
                    }
                    bind = httpb;
                    break;

                default:
                    NetTcpBinding ntcp = new NetTcpBinding();
                    ntcp.Security.Mode = SecurityMode.None;
                    ntcp.ReliableSession.Enabled = false;
                    ntcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                    if (filebinding)
                    {
                        ntcp.MaxReceivedMessageSize = configdata.FileSize;
                        ntcp.TransferMode = TransferMode.Streamed;
                    }
                    else
                    {
                        ntcp.MaxReceivedMessageSize = configdata.DataStringSize;
                        ntcp.ReaderQuotas.MaxStringContentLength = configdata.DataStringSize;
                        ntcp.ReaderQuotas.MaxBytesPerRead = configdata.DataStringSize;
                    }
                    ntcp.ListenBacklog = 100;
                    ntcp.MaxConnections = configdata.MaxConnections;
                    bind = ntcp;
                    break;
            }

            // global settings
            bind.SendTimeout = new TimeSpan(0, configdata.SendTimeout, 0);
            bind.OpenTimeout = new TimeSpan(0, 0, configdata.OpenTimeout);
            bind.ReceiveTimeout = new TimeSpan(0, configdata.ReveiveTimeout, 0);
            return bind;
        }
    }
}
