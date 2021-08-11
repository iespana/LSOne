using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;

using LSRetail.Scheduler.Common;

namespace LSRetail.DD.Common
{
    /// <summary>
    /// Main header pack information class, includes all information about the job
    /// </summary>
    [Serializable]
    public class SOAPPack : ISerializable , IDisposable
    {
        private Guid pguid = Guid.Empty;               // current pack id current host
        private string jobid = string.Empty;
        private int oldpackid = 0;                      // original pack id from sender host
        private string srchost = string.Empty;          // sender host name
        private string incomingclient = string.Empty;   // end point address for incoming client

        private JobType type = JobType.None;
        private PackStatus status = PackStatus.None;
        private bool gotfile = false;
        private bool senddbreportback = false;
        private int extrafield = 2;                     // 2 is not set, otherwise its 0=false 1=true
        private string isolevel = "Unspecified";
        private DDCompressMode comrpessmode = DDCompressMode.ZipStream;

        private DateTime createtime = DateTime.Now;
        private DateTime regtime = DateTime.Now;

        private int intparamvalue = 0;
        private string strparamvalue = string.Empty;

        private SOAPHost source = null;
        private List<SOAPHost> destinations = new List<SOAPHost>();

        private string receivedfile = string.Empty;
        private bool receivedfiledeleted = false;
        private string processedxmlfile = string.Empty;
        private string processedzipfile = string.Empty;
        private int zipfilecount = 1;
        private int zipfilesendindex = 1;
        private bool processedfiledeleted = false;
        private string md5hash = string.Empty;
        private string md5hashzip = string.Empty;

        private object statusLock = new object();
        private object destlistLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public SOAPPack()
        {
            if (AppConfig.GlobalConfigData != null)
                this.comrpessmode = AppConfig.GlobalConfigData.ZipMode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Pack type</param>
        public SOAPPack(JobType type)
        {
            this.type = type;
            if (AppConfig.GlobalConfigData != null)
                this.comrpessmode = AppConfig.GlobalConfigData.ZipMode;
        }

        /// <summary>
        /// Constructor for Serialization object data
        /// </summary>
        /// <param name="info">SerializationInfo object</param>
        /// <param name="context">StreamingContext object</param>
        protected SOAPPack(SerializationInfo info, StreamingContext context)
        {
            this.pguid = DDUtils.GetSerGuid(info, "guid");
            this.oldpackid = DDUtils.GetSerInt32(info, "srcid");
            this.srchost = DDUtils.GetSerString(info, "srchost");
            this.incomingclient = DDUtils.GetSerString(info, "client");
            this.jobid = DDUtils.GetSerString(info, "jobid");
            this.intparamvalue = DDUtils.GetSerInt32(info, "intparam");
            this.strparamvalue = DDUtils.GetSerString(info, "strparam");
            this.createtime = DDUtils.GetSerDateTime(info, "ctime");
            this.receivedfile = DDUtils.GetSerString(info, "rfile");
            this.processedxmlfile = DDUtils.GetSerString(info, "pxmlfile");
            this.processedzipfile = DDUtils.GetSerString(info, "pzipfile");
            this.zipfilecount = DDUtils.GetSerInt32(info, "pzipcnt");
            this.zipfilesendindex = DDUtils.GetSerInt32(info, "pzipindex");
            this.md5hash = DDUtils.GetSerString(info, "hash");
            this.md5hashzip = DDUtils.GetSerString(info, "ziphash");
            this.senddbreportback = DDUtils.GetSerBool(info, "report");
            this.extrafield = DDUtils.GetSerInt32(info, "extra");
            this.isolevel = DDUtils.GetSerString(info, "iso");
            this.comrpessmode = (DDCompressMode)DDUtils.StringToEnum(typeof(DDCompressMode), DDUtils.GetSerString(info, "cmode"));
            this.status = (PackStatus)DDUtils.StringToEnum(typeof(PackStatus), DDUtils.GetSerString(info, "stat"));
            this.type = (JobType)DDUtils.StringToEnum(typeof(JobType), DDUtils.GetSerString(info, "type"));
            this.source = (SOAPHost)info.GetValue("src", typeof(SOAPHost));
            this.destinations = (List<SOAPHost>)info.GetValue("dst", typeof(List<SOAPHost>));
            this.regtime = DateTime.Now;
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
            info.AddValue("srcid", this.oldpackid);
            info.AddValue("srchost", this.srchost);
            info.AddValue("client", this.incomingclient);
            info.AddValue("jobid", this.jobid);
            info.AddValue("intparam", this.intparamvalue);
            info.AddValue("strparam", this.strparamvalue);
            info.AddValue("ctime", this.createtime);
            info.AddValue("rfile", this.receivedfile);
            info.AddValue("pxmlfile", this.processedxmlfile);
            info.AddValue("pzipfile", this.processedzipfile);
            info.AddValue("pzipindex", this.zipfilesendindex);
            info.AddValue("pzipcnt", this.zipfilecount);
            info.AddValue("hash", this.md5hash);
            info.AddValue("ziphash", this.md5hashzip);
            info.AddValue("report", this.senddbreportback);
            info.AddValue("extra", this.extrafield);
            info.AddValue("iso", this.isolevel);
            info.AddValue("cmode", this.comrpessmode.ToString());
            info.AddValue("stat", this.status.ToString());
            info.AddValue("type", this.type.ToString());
            info.AddValue("src", this.source, typeof(SOAPHost));
            lock (this.destlistLock)
            {
                info.AddValue("dst", this.destinations, typeof (List<SOAPHost>));
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
        /// Generate string array with Data to use for DataRow fill
        /// </summary>
        /// <returns>String Array data for Data Row</returns>
        public object[] GetDataRow()
        {
            object[] row = 
            {
                this.pguid,
                this.oldpackid,
                this.srchost,
                this.jobid,
                this.status.ToString(),
                this.Source.HostStringWithDBInfo,
                this.Source.HostString,
                this.source.Status.ToString(),
                this.Source.ErrMessage,
                this.Source.ErrLevel.ToString(),
                this.source.TryCount,
                this.createtime,
                this.regtime,
                this.type.ToString(),
                this.Source.DBConnection.ConnString
            };
            return row;
        }

        /// <summary>
        /// Add Source Host information to header data
        /// </summary>
        /// <param name="hoststr">Hoststring with routing path to Source host</param>
        /// <param name="connstr">Database connection string for Source host</param>
        public void AddSource(string hoststr, string connstr)
        {
            this.source = new SOAPHost(hoststr, connstr);
        }

        /// <summary>
        /// Add Destination Host information to header data
        /// </summary>
        /// <param name="hoststr">Hoststring with routing path to Destination host</param>
        /// <param name="connstr">Database connection string for Destination host</param>
        public void AddDestination(string hoststr, string connstr)
        {
            lock (this.destlistLock)
            {
                SOAPHost ds = new SOAPHost(hoststr, connstr, this.destinations.Count);
                this.destinations.Add(ds);
            }
        }

        /// <summary>
        /// Update Host data in Pack with data from another host object
        /// </summary>
        /// <param name="index">Index of a host to update</param>
        /// <param name="newhostdata">Updated host data</param>
        /// <param name="updatehostdata">Update Host Path and Database information also</param>
        public void UpdateHostData(int index, SOAPHost newhostdata, bool updatehostdata = false)
        {
            SOAPHost host;
            lock (this.destlistLock)
            {
                if (index == -1)
                {
                    host = this.source;
                }
                else
                {
                    if (index >= this.destinations.Count)
                        return; // index out of range

                    host = this.destinations[index];
                }
                host.UpdateHostData(newhostdata, updatehostdata);
            }
        }

        /// <summary>
        /// Change the Host info for current host, to match the network netmode for next host
        /// This is done so the result message can make it back to this host, 
        /// if both host are not running on same network protocol
        /// </summary>
        /// <param name="index">Host index</param>
        /// <param name="offlinehost">Offline host if any</param>
        public void MatchMeToDestHost(int index, string offlinehost)
        {
            if (index == -1)
                return;

            lock (this.destlistLock)
            {
                if (index >= this.destinations.Count)
                    return; // index out of range

                this.destinations[index].MatchMeToDestHost(offlinehost);
            }
        }

        /// <summary>
        /// Get Host data by host index
        /// </summary>
        /// <param name="index">index to look for</param>
        /// <param name="resetstat">Reset host status and update try counter</param>
        /// <returns>Host Data</returns>
        public SOAPHost GetHostByIndex(int index, bool resetstat = false)
        {
            if (index == -1)
            {
                if (resetstat)
                {
                    this.source.ResetErrorStatus();
                    this.source.TryCount++;
                }
                return this.source;
            }

            lock (this.destlistLock)
            {
                if (index >= this.destinations.Count)
                    return null; // index out of range

                if (resetstat)
                {
                    this.destinations[index].ResetErrorStatus();
                    this.destinations[index].TryCount++;
                }
                return new SOAPHost(this.destinations[index]);
            }
        }

        /// <summary>
        /// Find destination host by HostString
        /// </summary>
        /// <param name="host">HostString to look for</param>
        /// <param name="index">List of Indexes found for host</param>
        /// <returns>Host Data, null if not found</returns>
        public SOAPHost GetDestHostByName(string host, out List<int> index)
        {
            index = new List<int>();
            SOAPHost mh = new SOAPHost(host, string.Empty);
            SOAPHost firsthostfound = null;
            List<SOAPHostInfo> hilist = new List<SOAPHostInfo>(mh.GetHostPath());
            lock (this.destlistLock)
            {
                for (int i = 0; i < this.destinations.Count; i++)
                {
                    SOAPHost ds = this.destinations[i];
                    List<SOAPHostInfo> dslist = new List<SOAPHostInfo>(ds.GetHostPath());
                    if (dslist.Count != hilist.Count)
                        continue;

                    bool same = true;
                    for (int x = 0; x < dslist.Count; x++)
                    {
                        if (dslist[x].HostName.Equals(hilist[x].HostName, StringComparison.InvariantCultureIgnoreCase) == false || 
                            dslist[x].Port != hilist[x].Port)
                        {
                            same = false;
                            break;
                        }
                    }

                    if (same == false) 
                        continue;

                    index.Add(i);
                    if (firsthostfound == null)
                        firsthostfound = ds;
                }
            }
            return firsthostfound;
        }

        /// <summary>
        /// Get Previous Host Info by host index
        /// </summary>
        /// <param name="index">index to look for</param>
        /// <returns>Host Info</returns>
        public SOAPHostInfo GetPrevHostInfoByIndex(int index)
        {
            SOAPHost host;
            lock (this.destlistLock)
            {
                if (index == -1)
                {
                    host = this.source;
                }
                else
                {
                    if (index >= this.destinations.Count)
                        return null; // index out of range
                    host = this.destinations[index];
                }
                host.ProcessHosts();
                SOAPHostInfo hostinfo = host.GetPrevHostInList();
                return hostinfo;
            }
        }

        /// <summary>
        /// Iterate through Destinations and check if all are done, if so then pack Status is set to Done
        /// </summary>
        public void SetPackDone()
        {
            // check source
            if (this.source.Status == HostStatus.Canceled)
            {
                this.status = PackStatus.Done;
                return;
            }

            // check destinations
            lock (this.destlistLock)
            {
                foreach (SOAPHost ds in this.destinations)
                {
                    if (ds.IsDone() == false)
                        return;
                }
            }
            this.status = PackStatus.Done;
        }

        /// <summary>
        /// Resets Host Status on all Destinations to None, if the newstatus is not already = Done
        /// </summary>
        public void ResetDestStatuses()
        {
            lock (this.destlistLock)
            {
                foreach (SOAPHost ds in this.destinations)
                {
                    // reset newstatus if it is not done back none, so this pack will be forwarded to final destination
                    if (ds.Status == HostStatus.Forwarded || ds.Status == HostStatus.Processed)
                        ds.Status = HostStatus.None;
                }
            }
        }

        /// <summary>
        /// Check if any destinations are totally same
        /// </summary>
        public void CheckDublicateDestinations()
        {
            lock (this.destlistLock)
            {
                if (this.destinations.Count == 1)
                    return; // no need to check only one destination

                for (int x = 0; x < this.destinations.Count; x++)
                {
                    SOAPHost ds = this.destinations[x];
                    for (int y = x + 1; y < this.destinations.Count; y++)
                    {
                        SOAPHost dd = this.destinations[y];
                        if (ds.HostString.Equals(dd.HostString, StringComparison.InvariantCulture) == false)
                            continue;

                        if (ds.DBConnection.CompareTo(dd.DBConnection) == false)
                            continue;

                        // same dd server and database so cancel one of them
                        dd.Status = HostStatus.Canceled;
                        dd.ErrMessage = string.Format("This Destination is same as one with index {0}, no need to process same data twice", ds.Index);
                    }
                }
            }
        }

        /// <summary>
        /// Set statues for destination that have same next host as the host to look for
        /// used for newstatus forward and errforward
        /// </summary>
        /// <param name="index">Index of host to look for</param>
        /// <param name="newstatus">Status to set</param>
        /// <param name="message">Host Server Message</param>
        public void SetDestHostStatus(int index, HostStatus newstatus, string message)
        {
            lock (this.destlistLock)
            {
                SOAPHostInfo hi = this.GetHostByIndex(index).GetNextHostInList();
                if (hi == null)
                    return;

                foreach (SOAPHost ds in this.destinations)
                {
                    if (ds.IsDone())
                        continue;

                    SOAPHostInfo dhi = ds.GetNextHostInList();
                    if (dhi == null)
                        continue;

                    if (!hi.HostName.Equals(dhi.HostName, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    ds.Status = newstatus;
                    ds.ErrMessage = message;
                }
            }
        }

        /// <summary>
        /// Check all Source and Destination for Aliases hosts, if found it will be replaces with current host
        /// </summary>
        public void CheckAliases()
        {
            char[] ch = { ';' };
            string[] hostlist = AppConfig.Aliases.Split(ch, StringSplitOptions.RemoveEmptyEntries);

            // check source host
            this.source.ProcessHosts();
            if (hostlist.Length == 0)
            {
                this.source.ReplaceHost(string.Empty, AppConfig.AppData.Host);
            }
            else
            {
                foreach (string st in hostlist)
                    this.source.ReplaceHost(st, AppConfig.AppData.Host);
            }

            // check destination hosts
            lock (this.destlistLock)
            {
                foreach (SOAPHost h in this.destinations)
                {
                    h.ProcessHosts();
                    if (hostlist.Length == 0)
                    {
                        h.ReplaceHost(string.Empty, AppConfig.AppData.Host);
                    }
                    else
                    {
                        foreach (string st in hostlist)
                            h.ReplaceHost(st, AppConfig.AppData.Host);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the Header Pack object to a XML file in the work folder
        /// </summary>
        /// <param name="folder">Folder to save Pack data to, default its workfolder</param>
        public void SaveMe(string folder = "")
        {
            if (this.pguid == Guid.Empty)
            {
                AppConfig.WriteDbg(DebugLevel.Main, "Don't save pack 0");
                return;
            }

            lock (this.statusLock)
            {
                this.regtime = DateTime.Now;
                string path = (String.IsNullOrEmpty(folder)) ? Path.Combine(AppConfig.WorkFolder, DDUtils.GetValidFilename(this.jobid)) : folder;
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                string file = DDUtils.GetFileName(this.pguid, "HEAD", "xml", path);

                while (true)
                {
                    TextWriter writer = null;
                    try
                    {
                        writer = new StreamWriter(file);
                        XmlSerializer ser = new XmlSerializer(typeof(SOAPPack));
                        ser.Serialize(writer, this);

                        AppConfig.WriteDbg(DebugLevel.Main, String.Format("-->Save Pack:{0} Stat:{1}",
                           this.pguid, this.status.ToString()));
                        AppConfig.WriteDbg(DebugLevel.DetailL1, String.Format("--->Source:{0} Stat:{1}",
                            this.source.HostString, this.source.Status.ToString()));
                        lock (this.destlistLock)
                        {
                            foreach (SOAPHost ds in destinations)
                                AppConfig.WriteDbg(DebugLevel.DetailL1, String.Format("---->Dest:{0} Stat:{1}",
                                                                                      ds.HostString, ds.Status.ToString()));
                        }
                        break;
                    }
                    catch (IOException iex)
                    {
                        AppConfig.WriteDbg(DebugLevel.Warning, "Failed to save XML Data: " + iex.Message);
                    }
                    catch (Exception e)
                    {
                        AppConfig.WriteDbg("Cannot save Job Data file", e);
                        break;
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Get XML Data for Pack
        /// </summary>
        /// <returns>XML Element Data</returns>
        public XElement GetXMLPackData()
        {
            XDocument xdoc;
            try
            {
                MemoryStream mem = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(SOAPPack));
                XmlTextWriter xtw = new XmlTextWriter(mem, Encoding.UTF8);
                xs.Serialize(xtw, this);
                mem = (MemoryStream)xtw.BaseStream;
                UTF8Encoding enc = new UTF8Encoding(false);

                string x = enc.GetString(mem.ToArray());
                string x2 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (x.StartsWith(x2))
                {
                    x = x.Remove(0, x2.Length);
                }

                xdoc = XDocument.Parse(x);
            }
            catch (Exception ex)
            {
                AppConfig.WriteDbg("Failed to generate monitor data for Pack:" + this.GuidPackId.ToString(), ex);
                xdoc = XDocument.Parse("<SOAPPack />");
            }

            return xdoc.Element("SOAPPack");
        }

        /// <summary>
        /// Check if there is any error at any of the Hosts
        /// </summary>
        /// <param name="errmsg">Error message if any</param>
        /// <returns>True if any error was found</returns>
        public ErrorLevel HasError(out string errmsg)
        {
            errmsg = string.Empty;

            if (this.source.ErrLevel != ErrorLevel.None)
            {
                errmsg = this.source.ErrMessage;
                return this.source.ErrLevel;
            }

            lock (this.destlistLock)
            {
                foreach (SOAPHost ds in this.Destinations)
                {
                    if (ds.ErrLevel == ErrorLevel.None)
                        continue;

                    errmsg = ds.ErrMessage;
                    return ds.ErrLevel;
                }
            }
            return ErrorLevel.None;
        }

        /// <summary>
        /// Check if any of the destination is on hold
        /// </summary>
        /// <returns>True if any destination is on hold</returns>
        public bool HasOnHold()
        {
            lock (this.destlistLock)
            {
                SOAPHost ds = this.destinations.Find(y => y.Status == HostStatus.OnHold);
                return (ds == null) ? false : true;
            }
        }

        /// <summary>
        /// Get copy of the Destination list
        /// </summary>
        /// <returns>Destination list</returns>
        public List<SOAPHost> GetDestinations()
        {
            return new List<SOAPHost>(destinations);            
        }

        #region Properties
        /// <summary>
        /// Pack id of the current job
        /// </summary>
        public Guid GuidPackId
        {
            set { pguid = value; }
            get { return pguid; }
        }
        /// <summary>
        /// Senders Pack id from the sender host
        /// </summary>
        public int OldPackId
        {
            set { oldpackid = value; }
            get { return oldpackid; }
        }
        /// <summary>
        /// Senders host name
        /// </summary>
        public string SrcHost
        {
            set { srchost = value; }
            get { return srchost; }
        }
        /// <summary>
        /// Incoming Client info, used for DDIncoming to find its way back to incoming client
        /// </summary>
        public string Client
        {
            set { incomingclient = value; }
            get { return incomingclient; }
        }
        /// <summary>
        /// Job id
        /// </summary>
        public string JobId
        {
            set { jobid = value; }
            get { return jobid; }
        }
        /// <summary>
        /// Header Pack Status
        /// </summary>
        public PackStatus Status
        {
            set { status = value; }
            get { return status; }
        }
        /// <summary>
        /// Pack Processing Type
        /// </summary>
        public JobType Type
        {
            set { type = value; }
            get { return type; }
        }
        /// <summary>
        /// Time when Header Pack was created at Source
        /// </summary>
        public DateTime CreateTime
        {
            set { createtime = value; }
            get { return createtime; }
        }
        /// <summary>
        /// Time when last activity was recorded in the Header Pack
        /// </summary>
        public DateTime RegTime
        {
            set { regtime = value; }
            get { return regtime; }
        }
        /// <summary>
        /// Incoming data from Source host
        /// </summary>
        public string ReceivedFile
        {
            set { receivedfile = value; }
            get { return receivedfile; }
        }
        /// <summary>
        /// Destination XML data to send to Destination host
        /// </summary>
        public string ProccesedXMLFile
        {
            set { processedxmlfile = value; }
            get { return processedxmlfile; }
        }
        /// <summary>
        /// Destination ZIP data to send to Destination host
        /// </summary>
        public string ProccesedZIPFile
        {
            set { processedzipfile = value; }
            get { return processedzipfile; }
        }
        /// <summary>
        /// Processed Zip file count, if more than 1 then the zip file has been split up
        /// and ZipFileIndex shows what files should be sent next (used for bad connection)
        /// </summary>
        public int ZipFileCount
        {
            set { zipfilecount = value; }
            get { return zipfilecount; }
        }
        /// <summary>
        /// Processed Zip file index to sent, if ZipFileCount is greater than 1
        /// </summary>
        public int ZipFileIndex
        {
            set { zipfilesendindex = value; }
            get { return zipfilesendindex; }
        }
        /// <summary>
        /// Received file has been deleted
        /// </summary>
        public bool RFileDeleted
        {
            set { receivedfiledeleted = value; }
            get { return receivedfiledeleted; }
        }
        /// <summary>
        /// Processed file has been deleted
        /// </summary>
        public bool PFileDeleted
        {
            set { processedfiledeleted = value; }
            get { return processedfiledeleted; }
        }
        /// <summary>
        /// MD5 Hash value for file to transfer
        /// </summary>
        public string MD5Hash
        {
            set { md5hash = value; }
            get { return md5hash; }
        }
        /// <summary>
        /// MD5 Hash value for Zip files to transfer
        /// </summary>
        public string ZipMD5Hashes
        {
            set { md5hashzip = value; }
            get { return md5hashzip; }
        }
        /// <summary>
        /// Waiting for incoming file, so if false this data cannot be processed
        /// </summary>
        public bool GotFile
        {
            set { gotfile = value; }
            get { return gotfile; }
        }
        /// <summary>
        /// Generate Database Progress Report for Job, and send back with result report
        /// </summary>
        public bool SendDBReportBack
        {
            set { senddbreportback = value; }
            get { return senddbreportback; }
        }
        /// <summary>
        /// Ignore any extra field in job that does not exist in destination table
        /// </summary>
        public Int32 IgnoreExtraField
        {
            set { extrafield = value; }
            get { return extrafield; }
        }
        /// <summary>
        /// Set SQL Isolation level
        /// </summary>
        public IsolationLevel IsoLevel
        {
            set { isolevel = value.ToString(); }
            get { return (IsolationLevel)DDUtils.StringToEnum(typeof(IsolationLevel), isolevel); }
        }
        /// <summary>
        /// Compress Mode for Data Transfer
        /// </summary>
        public DDCompressMode CompressMode
        {
            set { comrpessmode = value; }
            get { return comrpessmode; }
        }
        /// <summary>
        /// SOAPHost object for Source Host
        /// </summary>
        public SOAPHost Source
        {
            set { source = value; }
            get { return source; }
        }
        /// <summary>
        /// SOAPHost objects for Destination Hosts
        /// </summary>
        public List<SOAPHost> Destinations
        {
            set { destinations = value; }
            get { return destinations; }
        }
        /// <summary>
        /// Extra Integer Parameter value
        /// </summary>
        public int IntParamValue
        {
            set { intparamvalue = value; }
            get { return intparamvalue; }
        }
        /// <summary>
        /// Extra String Parameter value
        /// </summary>
        public string StrParamValue
        {
            set { strparamvalue = value; }
            get { return strparamvalue; }
        }
        #endregion
    }

}