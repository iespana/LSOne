using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;
using LSRetail.StoreController.BusinessObjects.Scheduler;

namespace LSRetail.DD.Common
{
    /// <summary>
    /// Connection Information for a Host
    /// </summary>
    [Serializable]
    public class SOAPHostInfo : IDisposable
    {
        private string hoststr = string.Empty;
        private string hostname = string.Empty;
        private int port = AppConfig.RouterPortTCP;
        private NetMode mode = NetMode.TCP;
        private int index = -1;

        private string orghostname = string.Empty;
        private SOAPHostInfo backuphost = null;
        private SOAPHostInfo receivehost = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public SOAPHostInfo()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Copy current info to new object</param>
        public SOAPHostInfo(SOAPHostInfo info)
        {
            this.backuphost = info.BackupHost;
            this.hostname = info.HostName;
            this.hoststr = info.HostString;
            this.index = info.Index;
            this.mode = info.Mode;
            this.orghostname = info.OrginalHostName;
            this.port = info.Port;
            this.receivehost = info.ReceiveHost;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="host">Host Name</param>
        /// <param name="port">Port</param>
        /// <param name="mode">Network Mode</param>
        public SOAPHostInfo(string host, int port, NetMode mode)
        {
            this.hostname = host.ToUpperInvariant();
            this.port = port;
            this.mode = mode;
            this.hoststr = string.Format("{0}:{1}:{2}", this.hostname, this.port, this.mode.ToString());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostpath">Host name:Port:Mode</param>
        public SOAPHostInfo(string hostpath)
        {
            this.hoststr = hostpath.ToUpperInvariant();

            int pos = this.hoststr.IndexOf('%');
            if (pos != -1)
            {
                string primhost = this.hoststr.Substring(0, pos);
                string backhost = this.hoststr.Substring(pos + 1, this.hoststr.Length - (pos + 1));
                DDUtils.SplitHostString(primhost, out this.hostname, out this.port, out this.mode);
                this.backuphost = new SOAPHostInfo(backhost);
            }
            else
            {
                DDUtils.SplitHostString(this.hoststr, out this.hostname, out this.port, out this.mode);
            }
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
        /// Update Port and Mode for Host
        /// </summary>
        /// <param name="portid">New Port Id</param>
        /// <param name="netmode">New connection Mode</param>
        public void UpdatePortMode(int portid, NetMode netmode)
        {
            this.port = portid;
            this.mode = netmode;
            this.hoststr = string.Format("{0}:{1}:{2}", this.hostname, this.port, this.mode.ToString());
        }

        /// <summary>
        /// Get Host info that host name matches this host or any of the backup host names
        /// </summary>
        /// <param name="host">host name to match</param>
        /// <returns>Host info if found, else null</returns>
        public SOAPHostInfo GetHostEquals(string host)
        {
            if (host.Equals(this.hostname, StringComparison.InvariantCultureIgnoreCase))
                return this;

            // check backup hosts
            if (this.backuphost == null)
                return null;

            return this.backuphost.GetHostEquals(host);
        }

        /// <summary>
        /// Check if host name matches this host or any of the backup host names
        /// </summary>
        /// <param name="host">host name to match</param>
        /// <returns>True if same</returns>
        public bool HostEquals(string host)
        {
            if (host.Equals(this.hostname, StringComparison.InvariantCultureIgnoreCase))
                return true;

            // check backup hosts
            if (this.backuphost == null)
                return false;

            return this.backuphost.HostEquals(host);
        }

        #region Properties
        /// <summary>
        /// Host Name
        /// </summary>
        public string HostName
        {
            get { return hostname; }
            set { hostname = value.ToUpperInvariant(); }
        }
        /// <summary>
        /// Socket Port for Host
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        /// <summary>
        /// Destination index for Host, if Index is -1 then this is Source host, if 0 or greater, this is Destination host
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        /// <summary>
        /// Get Network Mode for host
        /// </summary>
        public NetMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        /// <summary>
        /// Original Host name before it was converted
        /// </summary>
        public string OrginalHostName
        {
            get { return orghostname; }
            set { orghostname = value; }
        }
        /// <summary>
        /// Get Backup Host info
        /// </summary>
        public SOAPHostInfo BackupHost
        {
            get { return backuphost; }
        }
        /// <summary>
        /// Get Host string for host
        /// </summary>
        public string HostString
        {
            get { return hoststr; }
        }
        /// <summary>
        /// The host that accepted the job when forwarded (used to find out if primary or backup host got the job)
        /// </summary>
        public SOAPHostInfo ReceiveHost
        {
            get { return receivehost; }
            set { receivehost = value; }
        }
        #endregion
    }

    /// <summary>
    /// Source / Destination Server Information Class
    /// </summary>
    [Serializable]
    public class SOAPHost : ISerializable, IDisposable
    {
        private int index = -1;
        private string hoststr = string.Empty;
        private List<SOAPHostInfo> hostpath = new List<SOAPHostInfo>();

        private DateTime regtime = DateTime.Now;
        private HostStatus status = HostStatus.None;
        private DBConnection dbconnection = null;

        private int trycount = 0;
        private string errhost = string.Empty;
        private string errmessage = string.Empty;
        private ErrorLevel errlevel = ErrorLevel.None;

        private string report = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public SOAPHost()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hoststr">Host string with routing path</param>
        /// <param name="dbcon">Database Connection String for the host</param>
        public SOAPHost(string hoststr, string dbcon)
        {
            this.dbconnection = new DBConnection(dbcon);
            if (hoststr == null)
                this.hoststr = string.Empty;
            else
                this.hoststr = hoststr;

            this.ProcessHosts();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hoststr">Host string with routing path</param>
        /// <param name="dbcon">Database Connection String for the host</param>
        /// <param name="index">Host Index</param>
        public SOAPHost(string hoststr, string dbcon, int index)
        {
            this.dbconnection = new DBConnection(dbcon);
            this.index = index;
            if (hoststr == null)
                this.hoststr = string.Empty;
            else
                this.hoststr = hoststr;

            this.ProcessHosts();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tocopy">SOAPHost object to copy</param>
        public SOAPHost(SOAPHost tocopy)
        {
            this.index = tocopy.index;
            this.hoststr = tocopy.hoststr;
            this.hostpath = new List<SOAPHostInfo>(tocopy.hostpath);
            this.regtime = tocopy.regtime;
            this.status = tocopy.status;
            this.dbconnection = tocopy.dbconnection;
            this.trycount = tocopy.trycount;
            this.errhost = tocopy.errhost;
            this.errmessage = tocopy.errmessage;
            this.errlevel = tocopy.errlevel;
            this.report = tocopy.report;
        }

        /// <summary>
        /// Constructor for Serialization object data
        /// </summary>
        /// <param name="info">SerializationInfo object</param>
        /// <param name="context">StreamingContext object</param>
        protected SOAPHost(SerializationInfo info, StreamingContext context)
        {
            this.hoststr = DDUtils.GetSerString(info, "host");
            this.index = DDUtils.GetSerInt32(info, "index");
            this.dbconnection = new DBConnection(DDUtils.GetSerString(info, "conn"));
            this.status = (HostStatus)DDUtils.StringToEnum(typeof(HostStatus), DDUtils.GetSerString(info, "stat"));
            this.errmessage = DDUtils.GetSerString(info, "errmsg");
            this.ProcessHosts();
        }

        /// <summary>
        /// Get Serialization object data
        /// </summary>
        /// <param name="info">SerializationInfo object</param>
        /// <param name="context">StreamingContext object</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("host", this.hoststr);
            info.AddValue("index", this.index);
            info.AddValue("conn", this.dbconnection.ConnString);
            info.AddValue("stat", this.status.ToString());
            info.AddValue("errmsg", this.errmessage);
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
        /// Generate string array with Data to use for DataRow fill
        /// </summary>
        /// <returns>String Array data for Data Row</returns>
        public object[] GetDataRow()
        {
            object[] row = 
            {
                this.Index,
                this.HostStringWithDBInfo,
                this.hoststr,
                this.status.ToString(),
                this.errmessage,
                this.errlevel.ToString(),
                this.errhost,
                this.trycount,
                this.regtime,
                this.dbconnection.ConnString
            };
            return row;
        }

        /// <summary>
        /// Update current host data with data from another object (copy data)
        /// </summary>
        /// <param name="updatedata">Host data to copy into current object</param>
        /// <param name="updatehostdata">Update Host Path and Database information also</param>
        public void UpdateHostData(SOAPHost updatedata, bool updatehostdata)
        {
            this.regtime = updatedata.regtime;
            this.status = updatedata.status;
            this.dbconnection = updatedata.dbconnection;
            this.trycount = updatedata.trycount;
            this.errhost = updatedata.errhost;
            this.errmessage = updatedata.errmessage;
            this.errlevel = updatedata.errlevel;
            this.report = updatedata.report;

            if (updatehostdata)
            {
                this.HostString = updatedata.hoststr;
                this.dbconnection.ConnString = updatedata.dbconnection.ConnString;
            }
        }

        /// <summary>
        /// Check if current host is the last host in the hoststring, it means that host is a source or destination host
        /// </summary>
        /// <returns>true if current host is the last host in hoststring</returns>
        public bool IsCurrentHost()
        {
            if (AppConfig.AppData == null)
                return true;
            if (this.hostpath.Count <= 0)
                return false;

            SOAPHostInfo h;
            try
            {
                h = this.hostpath[this.hostpath.Count - 1];
            }
            catch
            {
                return false;
            }
            return h.HostEquals(AppConfig.AppData.Host);
        }

        /// <summary>
        /// Change the Host info for current host, to match the network netmode for next host
        /// This is done so the result message can make it back to this host, 
        /// if both host are not running on same network protocol
        /// </summary>
        /// <param name="offlinehost">Offline host if any</param>
        public void MatchMeToDestHost(string offlinehost)
        {
            if (AppConfig.AppData == null)
                return;

            this.hoststr = "";
            for (int i = 0; i < this.hostpath.Count; i++)
            {
                SOAPHostInfo ho = this.hostpath[i];
                if (ho.HostName.Equals(AppConfig.AppData.Host, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (i == this.hostpath.Count - 1)
                        return;     // no next host

                    SOAPHostInfo next = this.hostpath[i + 1];
                    if (String.IsNullOrWhiteSpace(offlinehost))
                    {
                        if (String.IsNullOrWhiteSpace(AppConfig.GlobalConfigData.ReturnIPAddress) == false)
                        {
                            // replace host name with ip address
                            ho.HostName = AppConfig.GlobalConfigData.ReturnIPAddress;
                        }
                        
                        // set same network mode for this host as the next host has
                        if (next.Mode == NetMode.DDOLD)
                        {
                            ho.UpdatePortMode(AppConfig.GetRouterPortByMode(next.Mode), next.Mode);
                        }
                    }
                    else
                    {
                        if (next.HostName.Equals(offlinehost, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ho.UpdatePortMode(ho.Port, NetMode.OFF);
                        }
                    }
                }
                this.hoststr += this.MakeHostString(ho);
            }
        }

        /// <summary>
        /// Generate Host String out of Host Info
        /// </summary>
        /// <param name="hinfo">Host info</param>
        /// <returns>Host string</returns>
        private string MakeHostString(SOAPHostInfo hinfo)
        {
            string hstr;
            if (hinfo.BackupHost == null)
            {
                hstr = string.Format("{0}:{1}:{2};", hinfo.HostName, hinfo.Port, hinfo.Mode.ToString());
            }
            else
            {
                hstr = string.Format("{0}:{1}:{2}%", hinfo.HostName, hinfo.Port, hinfo.Mode.ToString());
                hstr += this.MakeHostString(hinfo.BackupHost);
            }
            return hstr;
        }

        /// <summary>
        /// Check if current host is in the hoststring, if so return hostinfo
        /// </summary>
        /// <returns>Host Info if found, else null</returns>
        public SOAPHostInfo GetThisHost()
        {
            if (AppConfig.AppData == null)
                return null;

            int ind = this.GetIndex(AppConfig.AppData.Host);
            if (ind == -1)
                return null;
            return this.hostpath[ind];
        }

        /// <summary>
        /// Gets the first host in hoststring
        /// </summary>
        /// <returns>host name, if any, else null</returns>
        public SOAPHostInfo GetFirstHostInList()
        {
            if (this.hostpath.Count > 0)
                return this.hostpath[0];
            return null;
        }

        /// <summary>
        /// Gets the last host in hoststring
        /// </summary>
        /// <returns>host name, if any, else null</returns>
        public SOAPHostInfo GetLastHostInList()
        {
            if (this.hostpath.Count > 0)
                return this.hostpath[this.hostpath.Count - 1];
            return null;
        }


        /// <summary>
        /// Gets next host after current host in hoststring
        /// </summary>
        /// <returns>next host name, if any, else null</returns>
        public SOAPHostInfo GetNextHostInList()
        {
            if (AppConfig.AppData == null)
                return null;

            string myhost = AppConfig.AppData.Host;
            this.ReplaceHost(string.Empty, AppConfig.AppData.Host);
            int ind = this.GetIndex(myhost.ToUpperInvariant());

            // current host is in list and not the last one
            if (ind != -1 && ind < this.hostpath.Count - 1)
                return this.hostpath[ind + 1];

            return null;
        }

        /// <summary>
        /// Gets previous host before current host in hoststring
        /// </summary>
        /// <returns>next host name, if any, else null</returns>
        public SOAPHostInfo GetPrevHostInList()
        {
            if (AppConfig.AppData == null)
                return null;

            string myhost = AppConfig.AppData.Host;
            int ind = this.GetIndex(myhost.ToUpperInvariant());
            if (ind == -1)
                return null;

            SOAPHostInfo hicur = this.hostpath[ind];
            if (ind != -1 && ind > 0)
            {
                SOAPHostInfo hiprev = this.hostpath[ind - 1];
                if ((hicur.Mode == NetMode.MAN || hiprev.Mode == NetMode.MAN) || (hiprev.Mode == NetMode.DDOLD || hicur.Mode == NetMode.DDOLD))
                {
                    hiprev.Port = hicur.Port;
                    hiprev.Mode = hicur.Mode;
                }
                return hiprev;
            }
            return null;
        }

        /// <summary>
        /// Gets the host with the specified name.
        /// </summary>
        /// <param name="hostName">The name of the host to get.</param>
        /// <returns>The host if found, null otherwise.</returns>
        public SOAPHostInfo GetHostByName(string hostName)
        {
            SOAPHostInfo result = null;

            var ind = GetIndex(hostName);
            if (ind >= 0)
            {
                result = this.hostpath[ind];
            }

            return result;
        }

        /// <summary>
        /// Gets the host path of this host as a read-only list.
        /// </summary>
        /// <returns>A list of all hosts in the host path of this host.</returns>
        public IList<SOAPHostInfo> GetHostPath()
        {
            return this.hostpath.AsReadOnly();
        }

        /// <summary>
        /// Process host string/path into SOAPHostInfo object
        /// </summary>
        public void ProcessHosts()
        {
            if (String.IsNullOrEmpty(this.hoststr))
                return;

            char[] ch = { ';' };
            this.hostpath.Clear();
            string[] strlist = this.hoststr.Split(ch, StringSplitOptions.RemoveEmptyEntries);
            foreach (string h in strlist)
            {
                if (String.IsNullOrEmpty(h))
                    continue;

                SOAPHostInfo hinfo = new SOAPHostInfo(h);
                this.hostpath.Add(hinfo);
            }
        }

        /// <summary>
        /// Find Host index in routing path by Host name
        /// </summary>
        /// <param name="host">Host to look for</param>
        /// <returns>Index</returns>
        public int GetIndex(string host)
        {
            int ind = 0;
            foreach (SOAPHostInfo hi in this.hostpath)
            {
                if (hi.HostEquals(host))
                    return ind;
                ind++;
            }
            return -1;
        }

        /// <summary>
        /// Replace Host in Host path with new Host.  This is used to re-route job to different path
        /// </summary>
        /// <param name="from">Host to look for</param>
        /// <param name="to">Host to replace with</param>
        /// <returns>True if host was replaced</returns>
        public bool ReplaceHost(string from, string to)
        {
            bool changed = false;
            this.hoststr = "";
            List<SOAPHostInfo> paths = new List<SOAPHostInfo>(this.hostpath);
            for (int x = 0; x < paths.Count; x++)
            {
                SOAPHostInfo hi = paths[x];
                string newhostname = DDUtils.GetHostNameFromIPAddress(hi.HostName);
                if (newhostname != hi.HostName)
                {
                    hi.OrginalHostName = hi.HostName;
                    hi.HostName = newhostname;
                    changed = true;
                }
                SOAPHostInfo ch = hi.GetHostEquals(from);
                if (ch != null)
                {
                    newhostname = to.ToUpperInvariant();
                    if (newhostname != hi.HostName)
                    {
                        AppConfig.WriteDbg(DebugLevel.DetailL1, string.Format("Change Alias Name {0} to host {1}", from, to));
                        ch.OrginalHostName = hi.HostName;
                        ch.HostName = newhostname;
                        changed = true;
                    }
                }

                // check if this host is same as any previous one, remove the previous host from the list
                for (int i = 0; i < x; i++)
                {
                    if (!this.hostpath[i].HostName.Equals(hi.HostName, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    AppConfig.WriteDbg(DebugLevel.DetailL1, string.Format("Same host {0} found before, remove the previous host", hi.HostName));
                    this.hostpath.RemoveAt(i);
                    break;
                }
            }

            paths = new List<SOAPHostInfo>(this.hostpath);
            foreach (SOAPHostInfo hi in paths)
            {
                this.hoststr += this.MakeHostString(hi);
            }
            return changed;
        }

        /// <summary>
        /// Reset Status of Host from any error Status to None
        /// </summary>
        public void ResetErrorStatus()
        {
            switch (this.status)
            {
                case HostStatus.ErrForward:
                case HostStatus.ErrInsert:
                case HostStatus.ErrPreProcess:
                case HostStatus.ErrProcess:
                case HostStatus.ErrReceive:
                case HostStatus.Error:
                    this.status = HostStatus.None;
                    break;
            }
        }

        /// <summary>
        /// Check if Host newstatus is Done (Status = Done|Unknown|Canceled)
        /// </summary>
        /// <returns>True if done</returns>
        public bool IsDone()
        {
            if (this.status == HostStatus.Done ||
                this.status == HostStatus.Unknown ||
                this.status == HostStatus.Canceled)
                return true;
            return false;
        }

        #region Properties
        /// <summary>
        /// Routing path with all host in one string, separated by ;
        /// </summary>
        public string HostString
        {
            set { hoststr = value; this.ProcessHosts(); }
            get { return hoststr; }
        }
        /// <summary>
        /// Last Host name in the routing path with Database server included
        /// </summary>
        public string HostStringWithDBInfo
        {
            get 
            { 
                return string.Format("{0} [SRV:{1} DB:{2} {3}]", 
                    this.GetLastHostInList().HostName, 
                    this.dbconnection.Server,
                    this.dbconnection.Database,
                    (this.dbconnection.Company == string.Empty) ? string.Empty : "C:" + this.dbconnection.Company); 
            }
        }
        /// <summary>
        /// Host index in list
        /// </summary>
        public int Index
        {
            set { index = value; }
            get { return index; }
        }
        /// <summary>
        /// Host that the Error message came from
        /// </summary>
        public string ErrHost
        {
            set { errhost = value; }
            get { return errhost; }
        }
        /// <summary>
        /// Error message from host
        /// </summary>
        public string ErrMessage
        {
            set { errmessage = DDUtils.StripNonValidXMLCharacters(value); }
            get { return errmessage; }
        }
        /// <summary>
        /// Error code from host
        /// </summary>
        public ErrorLevel ErrLevel
        {
            set { errlevel = value; }
            get { return errlevel; }
        }
        /// <summary>
        /// Job Report data
        /// </summary>
        public string Report
        {
            set { report = value; }
            get { return report; }
        }
        /// <summary>
        /// Time when some activity was executed at the host
        /// </summary>
        public DateTime RegTime
        {
            set { regtime = value; }
            get { return regtime; }
        }
        /// <summary>
        /// Status at host
        /// </summary>
        public HostStatus Status
        {
            set { status = value; }
            get { return status; }
        }
        /// <summary>
        /// DataConnection object for host
        /// </summary>
        public DBConnection DBConnection
        {
            set { dbconnection = value; }
            get { return dbconnection; }
        }
        /// <summary>
        /// Try count, used when there is an error
        /// </summary>
        public int TryCount
        {
            set { trycount = value; }
            get { return trycount; }
        }
        #endregion
    }
}