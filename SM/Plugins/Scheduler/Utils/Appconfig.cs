using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml.Linq;
using LSRetail.StoreController.BusinessObjects.Scheduler;

namespace LSRetail.SiteManager.Plugins.Scheduler.Utils
{
    /// <summary>
    /// Config data for all DD Components
    /// </summary>
    public static class AppConfig
    {
        private static string configpath = string.Empty;
        private static string configfile = "lsretail.config";
        private static string licensefile = "license.lic";

        private static GlobalConfig configdata = new GlobalConfig();
        private static bool configloaded = false;

        //private static DDDebug dbgclient = null;
        private static string workfolder = string.Empty;


        /// <summary>
        /// Get Path to sub folder within base config folder
        /// </summary>
        /// <param name="folder">Subfolder name</param>
        /// <returns>Path to configfolder\folder</returns>
        static public string GetAppConfigPath(string folder = "")
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LS Retail", "Data Director");
            if (folder.Length > 0)
                path = Path.Combine(path, folder);

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception )
                {
                    //AppConfig.WriteDbg(ex);
                }
            }
            return path;
        }

        /// <summary>
        /// Constructor for AppConfig
        /// </summary>
        /// <param name="client">Client Application</param>
        /// <returns>True if loaded ok</returns>
        public static bool Init(bool client = false)
        {
            if (configloaded)
                return true;

            configpath = Path.Combine(GetAppConfigPath(), configfile);
            XDocument xmlfile;
            try
            {
                xmlfile = XDocument.Load(configpath);
            }
            //catch (FileNotFoundException nf)
            //{
            //    //WriteDbg(DebugLevel.Warning, string.Format("Config File {0} does not exist, lets create default file ({1})", configpath, nf.Message));

            //    // check there is ddconfig.xml file available with default settings
            //    if (CreateSettingsConfigFile(configpath) == false)
            //    {
            //        // create hard coded config file
            //        CreateBasicConfigFile(configpath);
            //    }

            //    try
            //    {
            //        xmlfile = XDocument.Load(configpath);
            //    }
            //    catch (Exception e)
            //    {
            //        WriteDbg(string.Format("Config File {0} is invalid, to re-create default config file, delete the file and start DD again", configpath), e);
            //        return false;
            //    }
            //}
            catch (Exception )
            {
                //WriteDbg(string.Format("Config File {0} is invalid, to re-create default config file, delete the file and start DD again", configpath), e);
                return false;
            }

            configloaded = LoadConfig(xmlfile);
            if (client)
            {
                //if (configdata.DebugClientLevel > 0)
                //    SetClientDebug(configdata.DebugClientLevel);
            }
            return configloaded;
        }

        /// <summary>
        /// Load Data From XML File and update Global Config data
        /// </summary>
        /// <param name="xmlfile">Config File</param>
        /// <returns>True if ok</returns>
        private static bool LoadConfig(XDocument xmlfile)
        {
            configloaded = configdata.LoadBaseConfig(xmlfile, true);
            if (configloaded == false)
            {
                //WriteDbg(DebugLevel.Error, configdata.GetLastError());
                return false;
            }

            workfolder = Path.Combine(configdata.DataFolder, "Work");

            //WriteDbg(DebugLevel.DetailL1, "Config Data Loaded from " + configpath);
            return configloaded;
        }




     
      
        /// <summary>
        /// Write Debug Data to stream
        /// </summary>
        ///// <param name="level">Debug Level</param>
        ///// <param name="msg">Debug Message</param>
        ////public static void WriteDbg(DebugLevel level, string msg)
        //{
        //    if (level == DebugLevel.Error)
        //        msg = "ERROR: " + msg;
        //    if (level == DebugLevel.Warning)
        //        msg = "WARNING: " + msg;

        //    if (configdata.AppData != null && configdata.AppData.Debug != null)
        //    {
        //        // Get the frame one step up the call tree
        //        configdata.AppData.Debug.Write(level, msg);
        //    }
        //    else if (dbgclient != null)
        //    {
        //        dbgclient.Write(level, msg);
        //    }
        //    else
        //        Console.WriteLine(msg);
        //}

        /// <summary>
        /// Write Exception error to debug stream
        /// </summary>
        /// <param name="e">Exception data</param>
        public static void WriteDbg(Exception e)
        {
            // remove all new line commands so this prints out without problem
            string ms = e.Message.Replace("\r", "");
            ms = ms.Replace("\\n", "");

            //WriteDbg(DebugLevel.Error, ms + " >>" + e.StackTrace);
            //if (e.InnerException != null)
                //WriteDbg(e.InnerException);
        }

        /// <summary>
        /// Write Exception error to debug stream
        /// </summary>
        ///// <param name="message">Internal Error message</param>
        ///// <param name="e">Exception data</param>
        ////public static void WriteDbg(string message, Exception e)
        //{
        //    // remove all new line commands so this prints out without problem
        //    string ms = e.Message.Replace("\r", "");
        //    ms = ms.Replace("\\n", "");

        //    WriteDbg(DebugLevel.Error, string.Format("{0} >> {1} >> {2}", message, ms, e.StackTrace));
        //    if (e.InnerException != null)
        //        WriteDbg(e.InnerException);
        //}






        /// <summary>
        /// Get Router Port by network netmode
        /// </summary>
        /// <param name="mode">Network netmode to look for</param>
        /// <returns>Port number that matches the Network netmode</returns>
        public static int GetRouterPortByMode(NetMode mode)
        {
            switch (mode)
            {
                case NetMode.TCP: return configdata.RouterPortTCP;
                case NetMode.TCPS: return configdata.RouterPortTCPS;
            }
            return configdata.RouterPortTCP;
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
                    bind = ntcp;
                    break;
            }


            return bind;
        }

        /// <summary>
        /// Set Debug Level for Client Control Debugging
        /// </summary>
        /// <param name="level">Debug level</param>
        private static void SetClientDebug(int level)
        {
            if (dbgclient == null)
                dbgclient = new DDDebug();

            //Process pr = Process.GetCurrentProcess();
            //dbgclient.SetFileValues(configdata.DebugFileSize, configdata.DebugFileCount);
            //dbgclient.SetDebugLevel(DebugLevelMode.File, level, "Client", pr.Id);
        }







        /// <summary>
        /// Get Folder Path to Data Folder, optional including subfolder.  If Folder does not exist, it will be created.
        /// </summary>
        /// <param name="subfolder">Subfolder 1</param>
        /// <param name="subfolder2">Subfolder 2</param>
        /// <returns>Data Path</returns>
        public static string GetDataPath(string subfolder = "", string subfolder2 = "")
        {
            string path = DataFolder;
            if (!String.IsNullOrEmpty(subfolder))
                path = Path.Combine(DataFolder, subfolder);
            if (!String.IsNullOrEmpty(subfolder2))
                path = Path.Combine(path, subfolder2);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>
        /// Check if enough available Hard disk space is available to accept job
        /// </summary>
        /// <returns>True if ok</returns>
        public static bool HDFull()
        {
            DriveInfo dinfo = new DriveInfo(Path.GetPathRoot(workfolder));
            if (dinfo.AvailableFreeSpace < configdata.MinDiskSpace)
                return true;
            return false;
        }

        #region Properties
        /// <summary>
        /// Base DD Port, used to calculate the actual portid for other Components
        /// </summary>
        public static int BasePort
        {
            get { return configdata.BasePort; }
        }
        /// <summary>
        /// TCPS Router Port
        /// </summary>
        public static int RouterPortTCPS
        {
            get { return configdata.RouterPortTCPS; }
        }
        /// <summary>
        /// TCP Router Port
        /// </summary>
        public static int RouterPortTCP
        {
            get { return configdata.RouterPortTCP; }
        }
        /// <summary>
        /// Service Application
        /// </summary>
        public static int ServicePort
        {
            get { return configdata.ServicePort; }
        }

        /// <summary>
        /// Path to work folder, default is %AllUser%\App Data\LS Retail
        /// </summary>
        public static string WorkFolder
        {
            get { return workfolder; }
        }
        /// <summary>
        /// Path to application data folder
        /// </summary>
        public static string DataFolder
        {
            get { return configdata.DataFolder; }
        }

        /// <summary>
        /// Application data for DDRouter
        /// </summary>
        public static AppConfData MyRouter
        {
            get { return configdata.MyRouter; }
        }
        /// <summary>
        /// List of Apps Loaded
        /// </summary>
        public static IEnumerable<AppConfData> Apps
        {
            get { return configdata.Apps; }
        }
        /// <summary>
        /// Get Global Config Data for application
        /// </summary>
        public static GlobalConfig GlobalConfigData
        {
            get { return configdata; }
        }

        /// <summary>
        /// Get Name of the DD License file
        /// </summary>
        public static string LicenseFileName
        {
            get { return licensefile; }
        }
        #endregion
    }
}
