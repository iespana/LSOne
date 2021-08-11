using System;
using System.ServiceModel;
using System.Xml.Linq;
using LSRetail.DD.Common;

namespace LSOne.Services.Utils
{

    #region AppType (Enum)
    /// <summary>
    /// Application Types for DD Components
    /// </summary>
    public enum AppType
    {
        /// <summary>
        /// No special app, just general program with no config
        /// </summary>
        None,
        /// <summary>
        /// DD Program Service Manager
        /// </summary>
        Service,
        /// <summary>
        /// DD Incoming component, to receive data from remote host or schedule
        /// </summary>
        Incoming,
        /// <summary>
        /// Offline Support
        /// </summary>
        Offline,
        /// <summary>
        /// DD Router component, main clock work of the system
        /// </summary>
        Router,
        /// <summary>
        /// DD Processing component
        /// </summary>
        Process,
        /// <summary>
        /// DD Scheduler component
        /// </summary>
        Scheduler,
        /// <summary>
        /// Monitoring component
        /// </summary>
        Monitor,
        /// <summary>
        /// Archive File Backup to a Database
        /// </summary>
        Backup,
        /// <summary>
        /// DD x86 Processing component
        /// </summary>
        Process86
    }
    #endregion
    #region DDConfigCommands (Enum)
    /// <summary>
    /// Config Commands available
    /// </summary>
    public enum DDConfigCommands
    {
        None,
        SystemCmd,
        ServiceCmd,
        QueueCmd,
        GetStatus,
        HasError,
        ErrorList,
        SchedulerCmd,
        MonitorCmd,
        PullReqest
    }
    #endregion

    #region DDServiceCommands (Enum)
    /// <summary>
    /// Actions for ConfigCommands.ServiceCmd
    /// </summary>
    public enum DDServiceCommands
    {
        None,
        GetConfig,
        UpdateConfig,
        InstallApp,
        KillApp,
        StopApp,
        StartApp,
        StopSystem,
        StartSystem,
        GetSystemStatus,
        ResetConfig
    }
    #endregion
    #region DDSystemCommands (Enum)
    /// <summary>
    /// Actions for ConfigCommands.SystemCmd
    /// </summary>
    public enum DDSystemCommands
    {
        None,
        Intro,
        GetLicenseInfo,
        Shutdown,
        DumpMemory,
        ConfigUpdate,
        Offline
    }
    #endregion

    #region DDQueueCommands (Enum)
    /// <summary>
    /// Actions for ConfigCommands.QueueCmd
    /// </summary>
    public enum DDQueueCommands
    {
        None,
        GetMainQue,
        GetGlobalQue,
        ResetJobQue,
        ModifyJob,
        GetProcessQue
    }
    #endregion
    [ServiceContract]
    public interface IComConfig : IDisposable
    {
        [OperationContract()]
        string SendConfigCmd(DDConfigCommands id, string path, string value);

        [OperationContract(IsOneWay = true)]
        void SendOnewayConfigCmd(DDConfigCommands id, string path, string value);

        [OperationContract()]
        string SendServiceCmd(DDServiceCommands cmd, string path, string value);

        [OperationContract()]
        string SendQueueCmd(DDQueueCommands cmd, string path, string typevalue, string hostvalue, string jobvalue,
                            Guid packid, int index, string newvalue);

        [OperationContract(IsOneWay = true)]
        void SendOnewayQueueCmd(DDQueueCommands cmd, string path, string typevalue, string hostvalue, string jobvalue,
                                Guid packid, int index, string newvalue);

        [OperationContract()]
        string SendSystemCmd(DDSystemCommands cmd, string path, string value);

        [OperationContract(IsOneWay = true)]
        void SendOnewaySystemCmd(DDSystemCommands cmd);

        [OperationContract(IsOneWay = true)]
        void SendIntro(int port, string host, AppType type);

        [OperationContract(IsOneWay = true)]
        void SendOffline(int port, string host, AppType type);
    }

    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single)]
    public class ComConfigService : IComConfig
    {
        /// <summary>
        /// Config Request Event
        /// </summary>
        public delegate string ConfRequestEventHandler(DDConfigCommands id, string path, string value);

        public event ConfRequestEventHandler OnConfRequest;

        public ComConfigService()
        {
            AppConfig.WriteDbg(DebugLevel.DetailL1, "Constructing ComConfigService");
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose actions
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        /// Send Config Command to System with return value
        /// </summary>
        /// <param name="id">Config Command Id</param>
        /// <param name="path">Routing Path</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>Return Value</returns>
        public string SendConfigCmd(DDConfigCommands id, string path, string value)
        {
            if (OnConfRequest != null)
                return OnConfRequest(id, path, value);
            return null;
        }

        /// <summary>
        /// Send Oneway Config Command to System
        /// </summary>
        /// <param name="id">Config Command Id</param>
        /// <param name="path">Routing Path</param>
        /// <param name="value">Parameter Value</param>
        public void SendOnewayConfigCmd(DDConfigCommands id, string path, string value)
        {
            if (OnConfRequest != null)
                OnConfRequest(id, path, value);
        }

        /// <summary>
        /// Send Service Command to System with return value
        /// </summary>
        /// <param name="cmd">Service Command</param>
        /// <param name="path">Routing Path</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>Return Value</returns>
        public string SendServiceCmd(DDServiceCommands cmd, string path, string value)
        {
            XDocument xdoc = new XDocument();
            XElement el = new XElement(cmd.ToString());
            if (String.IsNullOrEmpty(value) == false)
                el.Add(XElement.Parse(value));
            xdoc.Add(el);

            if (OnConfRequest != null)
                return OnConfRequest(DDConfigCommands.ServiceCmd, path, xdoc.ToString());
            return null;
        }

        /// <summary>
        /// Send Router Queue Command to Router with Return value
        /// </summary>
        /// <param name="cmd">Queue Command</param>
        /// <param name="path">Routing Path</param>
        /// <param name="typevalue">Main Queue Type</param>
        /// <param name="hostvalue">Host Queue Name</param>
        /// <param name="jobvalue">Job Queue Name</param>
        /// <param name="packid">Pack GuId</param>
        /// <param name="index">Host index</param>
        /// <param name="newvalue">New Value to update data with</param>
        /// <returns>Return value</returns>
        public string SendQueueCmd(DDQueueCommands cmd, string path, string typevalue, string hostvalue, string jobvalue,
                                   Guid packid, int index, string newvalue)
        {
            XDocument xdoc = new XDocument();
            XElement el = new XElement(cmd.ToString());
            xdoc.Add(el);

            el.Add(new XElement("Type", typevalue));
            el.Add(new XElement("Host", hostvalue));
            el.Add(new XElement("Job", jobvalue));
            el.Add(new XElement("Pack", packid));
            el.Add(new XElement("Index", index));
            el.Add(new XElement("New", newvalue));

            if (OnConfRequest != null)
                return OnConfRequest(DDConfigCommands.QueueCmd, path, xdoc.ToString());
            return null;
        }

        /// <summary>
        /// Send Router Queue Command to Router with Return value
        /// </summary>
        /// <param name="cmd">Queue Command</param>
        /// <param name="path">Routing Path</param>
        /// <param name="typevalue">Main Queue Type</param>
        /// <param name="hostvalue">Host Queue Name</param>
        /// <param name="jobvalue">Job Queue Name</param>
        /// <param name="packid">Pack GuId</param>
        /// <param name="index">Host index</param>
        /// <param name="newvalue">New Value to update data with</param>
        public void SendOnewayQueueCmd(DDQueueCommands cmd, string path, string typevalue, string hostvalue,
                                       string jobvalue, Guid packid, int index, string newvalue)
        {
            this.SendQueueCmd(cmd, path, typevalue, hostvalue, jobvalue, packid, index, newvalue);
        }

        /// <summary>
        /// Send System Command to System with return value
        /// </summary>
        /// <param name="cmd">System Command Id</param>
        /// <param name="path">Routing Path</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>Return Value</returns>
        public string SendSystemCmd(DDSystemCommands cmd, string path, string value)
        {
            XDocument xdoc = new XDocument();
            XElement el = new XElement(cmd.ToString());
            if (String.IsNullOrEmpty(value) == false)
                el.Add(XElement.Parse(value));
            xdoc.Add(el);

            if (OnConfRequest != null)
                return OnConfRequest(DDConfigCommands.SystemCmd, path, xdoc.ToString());
            return null;
        }

        /// <summary>
        /// Send One way System Command to System
        /// </summary>
        /// <param name="cmd">System Command</param>
        public void SendOnewaySystemCmd(DDSystemCommands cmd)
        {
            this.SendSystemCmd(cmd, string.Empty, string.Empty);
        }

        /// <summary>
        /// Send Introduction Message to Router, 
        /// every program that connects to the router needs to send this after connecting
        /// </summary>
        public void SendIntro(int port, string host, AppType type)
        {
            XDocument xdoc = new XDocument(new XElement(DDSystemCommands.Intro.ToString(),
                                                        new XElement("Port", port),
                                                        new XElement("Host", host),
                                                        new XElement("Type", type.ToString())));

            this.SendOnewayConfigCmd(DDConfigCommands.SystemCmd, string.Empty, xdoc.ToString());
        }

        /// <summary>
        /// Send Introduction Message to Router, 
        /// every program that connects to the router needs to send this after connecting
        /// </summary>
        public void SendOffline(int port, string host, AppType type)
        {
            XDocument xdoc = new XDocument(new XElement(DDSystemCommands.Offline.ToString(),
                                                        new XElement("Port", port),
                                                        new XElement("Host", host),
                                                        new XElement("Type", type.ToString())));

            this.SendOnewayConfigCmd(DDConfigCommands.SystemCmd, string.Empty, xdoc.ToString());
        }
    }
}