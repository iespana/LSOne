using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace LSRetail.DD.Common
{
    /// <summary>
    /// Result Data for Job process sent back to the source after job has been processed or forwarded
    /// </summary>
    [Serializable]
    public class SOAPResult : ISerializable , IDisposable
    {
        private Guid pguid = Guid.Empty;
        private int oldpackid = 0;
        private int oldsrcpackid = 0;
        private string srchost = string.Empty;
        private string incomingclient = string.Empty;   // end point address for incoming client

        private string resulthost = string.Empty;
        private string sendhost = string.Empty;
        private int index = -1;

        private string resxmlfile = string.Empty;
        private string reszipfile = string.Empty;
        private string hash = string.Empty;
        private int zipfilecount = 1;
        
        private string report = string.Empty;
        private ErrorLevel errlevel = ErrorLevel.None;
        private string srvmsg = string.Empty;
        
        private DateTime regtime = DateTime.Now;
        private HostStatus status = HostStatus.None;

        /// <summary>
        /// Constructor with data
        /// </summary>
        /// <param name="host">host sending the result message</param>
        /// <param name="packid">Source pack id</param>
        /// <param name="srchost">Source Host</param>
        /// <param name="index">Host Index</param>
        /// <param name="status">Host Status</param>
        public SOAPResult(string host, Guid packid, string srchost, int index, HostStatus status)
        {
            this.resulthost = host;
            this.srchost = srchost;
            this.sendhost = (AppConfig.AppData == null) ? Dns.GetHostName() : AppConfig.AppData.Host;
            this.pguid = packid;
            this.status = status;
            this.index = index;
        }

        /// <summary>
        /// Constructor with data
        /// </summary>
        /// <param name="pack">Pack Data</param>
        /// <param name="index">Host Index</param>
        /// <param name="status">Host Status</param>
        public SOAPResult(SOAPPack pack, int index, HostStatus status)
        {
            this.srchost = pack.SrcHost;
            this.pguid = pack.GuidPackId;
            this.oldpackid = pack.OldPackId;
            this.oldsrcpackid = pack.OldPackId;
            this.sendhost = (AppConfig.AppData == null) ? Dns.GetHostName() : AppConfig.AppData.Host;
            this.status = status;
            this.index = index;

            SOAPHost shost = pack.GetHostByIndex(index);
            SOAPHostInfo shi = shost.GetThisHost();
            if (shi == null)
                this.resulthost = Dns.GetHostName();
            else
                this.resulthost = (String.IsNullOrEmpty(shi.OrginalHostName) ? shi.HostName : shi.OrginalHostName);
        }

        /// <summary>
        /// Constructor with data
        /// </summary>
        /// <param name="pack">Pack Data</param>
        /// <param name="index">Host Index</param>
        /// <param name="status">Host Status</param>
        /// <param name="host">host sending the result message</param>
        public SOAPResult(SOAPPack pack, int index, HostStatus status, string host)
        {
            this.resulthost = host;
            this.srchost = pack.SrcHost;
            this.pguid = pack.GuidPackId;
            this.oldpackid = pack.OldPackId;
            this.oldsrcpackid = pack.OldPackId;
            this.sendhost = (AppConfig.AppData == null) ? Dns.GetHostName() : AppConfig.AppData.Host;
            this.status = status;
            this.index = index;
        }

        /// <summary>
        /// Constructor for Serialization object data
        /// </summary>
        /// <param name="info">SerializationInfo object</param>
        /// <param name="context">StreamingContext object</param>
        protected SOAPResult(SerializationInfo info, StreamingContext context)
        {
            this.pguid = DDUtils.GetSerGuid(info, "guid");
            this.oldpackid = DDUtils.GetSerInt32(info, "packid");
            this.oldsrcpackid = DDUtils.GetSerInt32(info, "srcid");
            this.srchost = DDUtils.GetSerString(info, "srchost");
            this.incomingclient = DDUtils.GetSerString(info, "client");
            this.index = DDUtils.GetSerInt32(info, "index");
            this.resulthost = DDUtils.GetSerString(info, "host");
            this.sendhost = DDUtils.GetSerString(info, "sender");
            this.resxmlfile = DDUtils.GetSerString(info, "rxmlfile");
            this.reszipfile = DDUtils.GetSerString(info, "rzipfile");
            this.hash = DDUtils.GetSerString(info, "hash");
            this.status = (HostStatus)DDUtils.StringToEnum(typeof(HostStatus), DDUtils.GetSerString(info, "stat"));
            this.errlevel = (ErrorLevel)DDUtils.StringToEnum(typeof(ErrorLevel), DDUtils.GetSerString(info, "errlvl"));
            this.srvmsg = DDUtils.GetSerString(info, "srvmsg");
            this.regtime = DDUtils.GetSerDateTime(info, "time");
            this.report = DDUtils.GetSerString(info, "report");
        }

        /// <summary>
        /// Get Serialization object data
        /// </summary>
        /// <param name="info">SerializationInfo object</param>
        /// <param name="context">StreamingContext object</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("guid", this.pguid);
            info.AddValue("packid", this.oldpackid);
            info.AddValue("srcid", this.oldsrcpackid);
            info.AddValue("srchost", this.srchost);
            info.AddValue("client", this.incomingclient);
            info.AddValue("index", this.index);
            info.AddValue("host", this.resulthost);
            info.AddValue("sender", this.sendhost);
            info.AddValue("rxmlfile", this.resxmlfile);
            info.AddValue("rzipfile", this.reszipfile);
            info.AddValue("hash", this.hash);
            info.AddValue("stat", this.status.ToString());
            info.AddValue("errlvl", this.errlevel.ToString());
            info.AddValue("srvmsg", this.srvmsg);
            info.AddValue("time", this.regtime);
            info.AddValue("report", this.report);
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
        /// Compare Result message to another Message
        /// </summary>
        /// <param name="result">Result Message to compare to this one</param>
        /// <returns>True if same, else false</returns>
        public bool Compare(SOAPResult result)
        {
            if (this.errlevel == result.ErrLevel && this.sendhost.Equals(result.Sender, StringComparison.InvariantCultureIgnoreCase) &&
                this.status == result.Status && this.srvmsg == result.ServerMessage)
                return true;

            return false;
        }

        /// <summary>
        /// Get an idea of how big the object is to know if it can be transferred
        /// </summary>
        /// <returns>Size of Result object</returns>
        public int GetSize()
        {
            int mysize = this.srchost.Length;
            mysize += this.incomingclient.Length;
            mysize += this.resulthost.Length;
            mysize += this.sendhost.Length;
            mysize += this.resxmlfile.Length;
            mysize += this.reszipfile.Length;
            mysize += this.hash.Length;
            if (this.srvmsg != null)
                mysize += this.srvmsg.Length;
            if (this.report != null)
                mysize += this.report.Length;
            mysize += (sizeof(int) * 3);
            mysize += 200; // other crap
            return mysize;
        }

        /// <summary>
        /// Set Error Status to Result Message
        /// </summary>
        /// <param name="errstatus">Error Status</param>
        /// <param name="errlvl">Error Level</param>
        /// <param name="errmsg">Error Message</param>
        public void SetErrorMessage(HostStatus errstatus, ErrorLevel errlvl, string errmsg)
        {
            this.status = errstatus;
            this.errlevel = errlvl;
            this.srvmsg = errmsg;
        }

        #region Properties
        /// <summary>
        /// Result Hosts Pack ID
        /// </summary>
        public Guid GuidPackId
        {
            get { return pguid; }
            set { pguid = value; }
        }
        /// <summary>
        /// Old Pack ID
        /// </summary>
        public int OldPackID
        {
            get { return oldpackid; }
            set { oldpackid = value; }
        }
        /// <summary>
        /// Old Source Pack ID
        /// </summary>
        public int OldSrcPackID
        {
            get { return oldsrcpackid; }
            set { oldsrcpackid = value; }
        }
        /// <summary>
        /// Source Host
        /// </summary>
        public string SrcHost
        {
            get { return srchost; }
            set { srchost = value; }
        }
        /// <summary>
        /// Host Index
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        /// <summary>
        /// Host that created the Result Data
        /// </summary>
        public string Host
        {
            get { return resulthost; }
            set { resulthost = value; }
        }
        /// <summary>
        /// Host that forwarded the Result Data
        /// </summary>
        public string Sender
        {
            get { return sendhost; }
            set { sendhost = value; }
        }
        /// <summary>
        /// Status
        /// </summary>
        public HostStatus Status
        {
            get { return status; }
        }
        /// <summary>
        /// Error Code
        /// </summary>
        public ErrorLevel ErrLevel
        {
            get { return errlevel; }
        }
        /// <summary>
        /// Server Message
        /// </summary>
        public string ServerMessage
        {
            get { return srvmsg; }
            set { srvmsg = value; }
        }
        /// <summary>
        /// Result XML File Name
        /// </summary>
        public string ResXMLFileName
        {
            get { return resxmlfile; }
            set { resxmlfile = value; }
        }
        /// <summary>
        /// Result ZIP File Name
        /// </summary>
        public string ResZIPFileName
        {
            get { return reszipfile; }
            set { reszipfile = value; }
        }
        /// <summary>
        /// Number of zip file if its split
        /// </summary>
        public int ZipFileCount
        {
            get { return zipfilecount; }
            set { zipfilecount = value; }
        }
        /// <summary>
        /// MD5 Hash value for the result file
        /// </summary>
        public string FileHash
        {
            get { return hash; }
            set { hash = value; }
        }
        /// <summary>
        /// Report data for job result
        /// </summary>
        public string Report
        {
            get { return report; }
            set { report = value; }
        }
        /// <summary>
        /// Incoming client id
        /// </summary>
        public string Client
        {
            get { return incomingclient; }
            set { incomingclient = value; }
        }
        /// <summary>
        /// Registration time of this Result message
        /// </summary>
        public DateTime RegTime
        {
            get { return regtime; }
        }
        #endregion
    }
}