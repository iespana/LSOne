
namespace LSRetail.DD.Common
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

    #region PackStatus (Enum)
    /// <summary>
    /// Processing status on Pack Data
    /// </summary>
    public enum PackStatus
    {
        /// <summary>
        /// Unknown Status
        /// </summary>
        None,
        /// <summary>
        /// New message from DDIncoming
        /// </summary>
        Incoming,
        /// <summary>
        /// New message from DDOffline
        /// </summary>
        Pulling,
        /// <summary>
        /// New message and received, has not been processed
        /// </summary>
        Received,
        /// <summary>
        /// Data has been processed at source host, but waiting for compression
        /// </summary>
        PreProcessed,
        /// <summary>
        /// Data has been processed and compressed at source host
        /// </summary>
        Processed,
        /// <summary>
        /// Data has been forwarded to next destination
        /// </summary>
        Forwarded,
        /// <summary>
        /// Data has been inserted into destination host, so process is completed
        /// </summary>
        Done
    }
    #endregion

    #region HostStatus (Enum)
    /// <summary>
    /// Host Status
    /// </summary>
    public enum HostStatus
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        None,
        /// <summary>
        /// Data has been received
        /// </summary>
        Received,
        /// <summary>
        /// Error in receiving the data
        /// </summary>
        ErrReceive,
        /// <summary>
        /// Data has been processed at Source host but waiting for compression
        /// </summary>
        PreProcessed,
        /// <summary>
        /// Data has been processed and compressed at Source host
        /// </summary>
        Processed,
        /// <summary>
        /// Error in pre-processing data at Source host
        /// </summary>
        ErrPreProcess,
        /// <summary>
        /// Error in processing data at Source host
        /// </summary>
        ErrProcess,
        /// <summary>
        /// Data has been forwarded to Destination
        /// </summary>
        Forwarded,
        /// <summary>
        /// Error in forwarding the data
        /// </summary>
        ErrForward,
        /// <summary>
        /// Data has been processed at the Destination
        /// </summary>
        Inserted,
        /// <summary>
        /// Error in processing data at destination host
        /// </summary>
        ErrInsert,
        /// <summary>
        /// Some other Error at destination that does not require retry, so job will be taken out of queue
        /// </summary>
        Error,
        /// <summary>
        /// Job Processing for this was Canceled, treat as done
        /// </summary>
        Canceled,
        /// <summary>
        /// Unknown status, current host will not process this item and will not get report about the status
        /// </summary>
        Unknown,
        /// <summary>
        /// Job and all related jobs are on Hold and will not be processed until put on None again
        /// </summary>
        OnHold,
        /// <summary>
        /// Data processing is done, final status result
        /// </summary>
        Done
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

    #region ErrorLevel (Enum)
    /// <summary>
    /// Error Level for Error message, used for Admin tool to know the seriousness of the error
    /// </summary>
    public enum ErrorLevel
    {
        None,
        Warning,
        Error,
        Connection,
        Database
    }
    #endregion

    #region DebugLevelMode (Enum)
    /// <summary>
    /// Debug Levels Modes, where to stream the Debug Data
    /// </summary>
    public enum DebugLevelMode
    {
        Console,
        File,
        Socket
    }
    #endregion

    #region DebugLevel (Enum)
    /// <summary>
    /// Debug Levels for Debug Stream to File, Console, Stream
    /// </summary>
    public enum DebugLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Main = 4,
        DetailL1 = 8,
        DetailL2 = 16,
        HeartBeat = 32
    }
    #endregion

    #region DBInsertUpdate (Enum)
    /// <summary>
    /// Used in DBServer for status of an Insert/Update record
    /// </summary>
    public enum DBInsertUpdate
    {
        Inserted,
        Updated,
        Skipped
    }
    #endregion

    #region DDCompressMode (Enum)
    /// <summary>
    /// Compression Mode supported by DD
    /// </summary>
    public enum DDCompressMode
    {
        None,
        ZipStream,
        ZipFile,
        SevenZip
    }
    #endregion
}
