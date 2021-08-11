using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using LSRetail.DD.Common;

namespace LSRetail.SiteManager.Plugins.Scheduler.Utils
{
    /// <summary>
    /// Application Config Data from the config file for DD Component
    /// </summary>
    [Serializable]
    public class AppConfData : IDisposable
    {
        private int port = -1;

        //[NonSerialized] private Process process = null;
        //[NonSerialized] private DDDebug dbg = new DDDebug();

        private string lasterror = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public AppConfData()
        {
            DbgLevel = 0;
            Type = AppType.None;
            PrivateConfig = null;
            SocketPort = 0;
            Router = 0;
            this.SetDebugForDebug();
        }

        /// <summary>
        /// Dispose of resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

        }

        [Conditional("DEBUG")]
        private void SetDebugForDebug()
        {
            this.DbgLevel = 15;
        }

        /// <summary>
        /// Get last Error message
        /// </summary>
        /// <returns>Error string</returns>
        public string GetLastError()
        {
            string err = this.lasterror;
            this.lasterror = string.Empty;
            return err;
        }

        /// <summary>
        /// Update local data from XML Data
        /// </summary>
        /// <param name="el">XML Element with data</param>
        /// <param name="baseport">Baseport for System</param>
        /// <returns>True if ok</returns>
        public bool LoadData(XElement el, int baseport)
        {
            if (el == null)
                return false;

            this.Port = Convert.ToInt32(el.Attribute("Port").Value);
            this.Type =
                (AppType)
                DDUtils.StringToEnum(typeof (AppType), DDUtils.GetXMLValue(el, "Type", AppType.None.ToString()));
            this.Router = DDUtils.GetXMLInt32(el, "Router", this.Router);
            this.DbgLevel = DDUtils.GetXMLInt32(el, "Debug", this.DbgLevel);



            this.SocketPort = baseport + this.Port;
            return true;
        }

        /// <summary>
        /// Convert Data into XElement object
        /// </summary>
        /// <returns>XElement</returns>
        public XElement GetXElement()
        {
            XElement el = new XElement("Program",
                                       new XElement("Type", this.Type.ToString()),
                                       new XElement("Router", this.Router),
                                       new XElement("Debug", this.DbgLevel)
                );
            el.SetAttributeValue("Port", port);

 
                el.Add(new XElement("Param"));

            return el;
        }

        #region Properties

        /// <summary>
        /// Application Port id
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }






        /// <summary>
        /// Router ID if this is a processing comp
        /// </summary>
        public int Router { get; set; }

        /// <summary>
        /// Actual socket sockport, baseport + sockport
        /// </summary>
        public int SocketPort { get; set; }

        /// <summary>
        /// Local Application Configuration data
        /// </summary>
        public ILocalConfig PrivateConfig { get; set; }


        /// <summary>
        /// Application Type
        /// </summary>
        public AppType Type { get; set; }



        /// <summary>
        /// Set / Get Debug level for program at startup
        /// </summary>
        public int DbgLevel { get; set; }


        #endregion
    }

    /// <summary>
    /// Global Config data from BaseConfig Section
    /// </summary>
    [Serializable]
    public class GlobalConfig
    {
        private string version = string.Empty;
        private string datafolder = string.Empty;
        private string installfolder = string.Empty;
        private string archivefolder = string.Empty;

        private DDCompressMode zipmode = DDCompressMode.ZipStream;
        private ThreadPriority priority = ThreadPriority.Normal;
        private Int64 filesize = 268435455;
        private int datastringsize = 26214400;
        private Int64 mindiskspace = 200000000;
        private int zipfilesize = 0; // MB
        private int sevenzipsize = 0; // MB

  
        private int baseport = 16800;
        private int tcprouterport = -1;
        private int tcpsrouterport = -1;
        private int routerprgid = -1;
        private int serviceprgid = -1;
        
        private AppConfData router = null;

        private List<AppConfData> apps = new List<AppConfData>();






        /// <summary>
        /// Update Local data from XML Data
        /// </summary>
        /// <param name="xdoc">XML Data</param>
        /// <param name="fileload">True if Load config from file, else from memory</param>
        /// <returns>Return true if loaded successfully</returns>
        public bool LoadBaseConfig(XDocument xdoc, bool fileload)
        {
            XElement xel = xdoc.Element("DDConfig");
            if (xel == null)
                return false;

            try
            {
                if (fileload)
                    this.version = DDUtils.GetVersionNumber();
                else
                    this.version = xel.Attribute("Version").Value;
            }
            catch
            {
                this.version = "Data Director 3.00";
            }

            this.installfolder = DDUtils.GetInstallPath();
            this.datafolder = DDUtils.GetAppConfigPath();

            try
            {
                XElement el = xel.Element("BaseConfig");
                this.baseport = DDUtils.GetXMLInt32(el, "BasePort", this.baseport);
                this.routerprgid = DDUtils.GetXMLInt32(el, "RouterPrgId", this.routerprgid);
                this.serviceprgid = DDUtils.GetXMLInt32(el, "ServicePrgId", this.serviceprgid);
               this.filesize = DDUtils.GetXMLInt64(el, "FileSize", this.filesize);
                this.datastringsize = DDUtils.GetXMLInt32(el, "DataSize", this.datastringsize);
                this.zipfilesize = DDUtils.GetXMLInt32(el, "ZipFileSize", this.zipfilesize);
                this.sevenzipsize = DDUtils.GetXMLInt32(el, "SevenZipSize", this.sevenzipsize);
                this.zipmode =
                    (DDCompressMode)
                    DDUtils.StringToEnum(typeof (DDCompressMode),
                                         DDUtils.GetXMLValue(el, "ZipMode", this.zipmode.ToString()));
                this.priority =
                    (ThreadPriority)
                    DDUtils.StringToEnum(typeof (ThreadPriority),
                                         DDUtils.GetXMLValue(el, "Priority", this.priority.ToString()));
                         this.installfolder = DDUtils.GetXMLValue(el, "InstallPath", this.installfolder);
                this.datafolder = DDUtils.GetXMLValue(el, "DataPath", this.datafolder);
                this.archivefolder = DDUtils.GetXMLValue(el, "ArcPath", this.archivefolder);
             
            }
            catch (Exception)
            {
                return false;
            }
            return this.LoadAppConfig(xel);
        }




       
        /// <summary>
        /// Load the Application Config Section from the config file
        /// </summary>
        /// <param name="xel">XML Data</param>
        /// <returns>True if ok</returns>
        private bool LoadAppConfig(XElement xel)
        {

            XElement ee = xel.Element("AppConfig");
            foreach (XElement el in ee.Elements("Program"))
            {
                int p = Convert.ToInt32((string) el.Attribute("Port").Value) + AppConfig.BasePort;
                AppConfData ad = this.FindAppConfig(p);
                if (ad == null)
                {
                    ad = new AppConfData();
                    this.apps.Add(ad);
                }

                if (ad.LoadData(el, this.baseport) == false)
                {
                    return false;
                }

                if (ad.Type == AppType.Router)
                {
                    if (this.router == null)
                    {
                        LocalRtrConfig loc = (LocalRtrConfig) ad.PrivateConfig;
                        this.tcprouterport = loc.TCP;
                        this.tcpsrouterport = loc.TCPS;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Find Application Data based on the sockport id
        /// </summary>
        /// <param name="sockid">Application Port id</param>
        /// <returns>AppConfData object with application configuration</returns>
        public AppConfData FindAppConfig(int sockid)
        {
            bool getrouter = (sockid == AppConfig.RouterPortTCPS || sockid == AppConfig.RouterPortTCP);
            foreach (AppConfData ad in this.apps)
            {
                if (getrouter && ad.Type == AppType.Router)
                    return ad;

                if (ad.SocketPort == sockid)
                    return ad;
            }
            return null;
        }





        #region Properties

        /// <summary>
        /// Base DD Port, used to calculate the actual sockport for other Components
        /// </summary>
        public int BasePort
        {
            get { return baseport; }
            set { baseport = value; }
        }

        /// <summary>
        /// TCPS Router Port
        /// </summary>
        public int RouterPortTCPS
        {
            get { return tcpsrouterport; }
            set { tcpsrouterport = value; }
        }

        /// <summary>
        /// TCP Router Port
        /// </summary>
        public int RouterPortTCP
        {
            get { return tcprouterport; }
            set { tcprouterport = value; }
        }

        /// <summary>
        /// Service Application
        /// </summary>
        public int ServicePort
        {
            get { return baseport + serviceprgid; }
            set { serviceprgid = value; }
        }

        /// <summary>
        /// Path to install folder
        /// </summary>
        public string InstallFolder
        {
            get { return installfolder; }
            set { installfolder = value; }
        }

        /// <summary>
        /// Path to application data folder
        /// </summary>
        public string DataFolder
        {
            get { return datafolder; }
            set { datafolder = value; }
        }

        /// <summary>
        /// Path to archive folder where xml files will be moved to after processing
        /// </summary>
        public string ArchiveFolder
        {
            get { return archivefolder; }
            set { archivefolder = value; }
        }

        /// <summary>
        /// Application data for DDRouter
        /// </summary>
        public AppConfData MyRouter
        {
            get { return router; }
        }

        /// <summary>
        /// Program Id for Router App
        /// </summary>
        public int RouterID
        {
            get { return routerprgid; }
            set { routerprgid = value; }
        }

        /// <summary>
        /// List of Apps Loaded
        /// </summary>
        public IEnumerable<AppConfData> Apps
        {
            get { return apps; }
        }

     



    

        /// <summary>
        /// Max Files size that can be sent via WCF communication
        /// </summary>
        public Int64 FileSize
        {
            get { return filesize; }
            set { filesize = value; }
        }

        /// <summary>
        /// Max Data String that can be sent via standard Data Communication
        /// </summary>
        public int DataStringSize
        {
            get { return datastringsize; }
            set { datastringsize = value; }
        }

        /// <summary>
        /// Size of ZipFile, if = 0 then don't break up zip file
        /// </summary>
        public int ZipFileSize
        {
            get { return zipfilesize; }
            set { zipfilesize = value; }
        }

        /// <summary>
        /// Size of SevenZip, if = 0 then don't break up zip file
        /// </summary>
        public int SevenZipSize
        {
            get { return sevenzipsize; }
            set { sevenzipsize = value; }
        }

        /// <summary>
        /// Default Compression Mode for Data files
        /// </summary>
        public DDCompressMode ZipMode
        {
            get { return zipmode; }
            set { zipmode = value; }
        }

        /// <summary>
        /// Thread Priority
        /// </summary>
        public ThreadPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        

      

      

        /// <summary>
        /// Minimum Disk space that is allowed so job can be accepted
        /// </summary>
        public long MinDiskSpace
        {
            get { return mindiskspace; }
            set { mindiskspace = value; }
        }

     




        /// <summary>
        /// Get Program name and Version & Build numbers
        /// </summary>
        public string VersionInfo
        {
            get { return version; }
        }

        #endregion
    }

    /// <summary>
    /// Local Config Interface
    /// </summary>
    public interface ILocalConfig
    {
        /// <summary>
        /// Load Config Data from XML File
        /// </summary>
        /// <param name="el">XML Element that include the Local Config data</param>
        /// <returns>Returns true if ok</returns>
        bool LoadParams(XElement el);

        /// <summary>
        /// Generate XElement Data for Local Config Data
        /// </summary>
        /// <returns>XElement Param Data</returns>
        XElement MakeXML();

        /// <summary>
        /// Update Params with values from settings
        /// </summary>
        /// <param name="settings">Setting config data to use for update</param>
        void UpdateParams(ILocalConfig settings);
    }



}


  




   


 
