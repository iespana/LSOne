using System.Reflection;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services
{
    /// <summary>
    /// The CCTV service can be used to integrate LS One to a CCTV security video system.
    /// This service has to be implemented by a partner. The default service is not connected to any hardware
    /// </summary>
    public partial class CCTVService : ICCTVService
    {
        /// <summary>
        /// The port where the CCTV system is accepting information from the POS
        /// </summary>
        protected int port;
        /// <summary>
        /// The host where the CCTV system is residing
        /// </summary>
        protected string hostName;
        /// <summary>
        /// 
        /// </summary>
        protected string camera;
        /// <summary>
        /// The Store ID that is sending the information
        /// </summary>
        protected string storeId;
        /// <summary>
        /// The terminal ID that is sending the information
        /// </summary>
        protected string terminalId;
        /// <summary>
        /// 
        /// </summary>
        protected string version;

        /// <summary>
        /// Each time an operation is run within the POS this operation is called and this implementation can choose if the 
        /// information the operation is giving will be logged into the security system or not
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The current transaction</param>
        /// <param name="operationId">The operation that is being run</param>
        /// <param name="mainOperation"></param>
        /// <param name="operationInfo">Information about the information</param>
        /// <param name="text">Any text the operation creates to send - if any</param>
        public virtual void CCTVOutput(IConnectionManager entry, IPosTransaction posTransaction, POSOperations operationId, bool mainOperation, OperationInfo operationInfo, string text)
        {            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="text">Any text the operation creates to send - if any</param>
        public virtual void CCTVMessageOutput(IConnectionManager entry, string text)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="text">Any text the operation creates to send - if any</param>
        public virtual void CCTVErrorOutput(IConnectionManager entry, string text)
        {
        }

        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set {  }
        }

        /// <summary>
        /// Initializes the CCTV service and sets the database connection for the service as well as the hardware profile settings for the CCTV
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618

            ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            this.port = settings.HardwareProfile.CctvPort;
            this.hostName = settings.HardwareProfile.CctvHostname;
            this.camera = settings.HardwareProfile.CctvCamera;
            Assembly asm = Assembly.GetEntryAssembly();
            if (asm != null)
            {
                version = asm.GetName().Version.ToString();
            }
            terminalId = (string)entry.CurrentTerminalID;
            storeId = (string)entry.CurrentStoreID;
        }
    }
}

