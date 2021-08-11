using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LSRetail.DD.Common;

namespace LSRetail.SiteManager.Plugins.Scheduler.Utils

{
    internal class DebugSockCallback
    {
      //  public IComDebugCallback callback = null;
        public int dbglevel = 0;
    }

    /// <summary>
    /// Debug class that handles debug messages, that can be sent to a socket, console or file
    /// </summary>
    public class DDDebug : IDisposable
    {
        private List<DebugSockCallback> callbacks = new List<DebugSockCallback>();
       
        private StreamWriter stream = null;
        private bool console = false;

        private int conlevel = 0;
        private int filelevel = 0;
        private int fileid = 0;
        private int filecount = 1;
        private int filesize = 20000000;

        private string execfile = string.Empty;
        private int port = 0;

        // thread lock object
        private object statusLock = new object();

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
                this.CloseDebugFile();
            }
        }

        /// <summary>
        /// Close Debug file, if open
        /// </summary>
        private void CloseDebugFile()
        {
            if (this.stream == null)
                return;

            this.stream.Flush();
            this.stream.Close();
            this.stream = null;
        }

        /// <summary>
        /// Set filename to save debug data to
        /// </summary>
        /// <param name="debugstart">true if debug startup</param>
        private void OpenDebugFile(bool debugstart)
        {
            string filename = Path.Combine(AppConfig.GetDataPath("Log"), String.Format("{0}-{1}-{2}.txt", this.execfile, this.port, this.fileid));

            try
            {
                this.CloseDebugFile();
                if (File.Exists(filename))
                {
                    for (int i = 1; i <= this.filecount; i++)
                    {
                        string ftmp = Path.Combine(AppConfig.GetDataPath("Log"), String.Format("{0}-{1}-{2}.txt", this.execfile, this.port, i));
                        string oldfile = ftmp.Replace(".txt", ".old.txt");

                        if (debugstart && File.Exists(oldfile))
                        {
                            string woldfile = ftmp.Replace(".txt", string.Format(".old.{0}.txt", DateTime.Now.ToFileTime()));
                            File.Move(oldfile, woldfile);
                        }

                        if (File.Exists(ftmp) == false)
                            continue;

                        if (debugstart)
                        {
                            oldfile = ftmp.Replace(".txt", string.Format(".{0}.txt", DateTime.Now.ToFileTime()));
                        }
                        else
                        {
                            if (File.Exists(oldfile))
                                File.Delete(oldfile);
                        }
                        File.Move(ftmp, oldfile);
                    }
                }
            }
            catch
            { 
                //AppConfig.WriteDbg(DebugLevel.Warning, "Failed to move old log file");
                throw;
            }

            try
            {
                this.stream = new StreamWriter(filename);
                this.stream.AutoFlush = true;
                this.stream.WriteLine("Date\tTime\tThread\tLevel\tMessage\tFunction\tLine");
            }
            catch
            {
             //   AppConfig.WriteDbg(DebugLevel.Error, "Failed to create Debug file" + filename);
                throw;
            }
        }

        /// <summary>
        /// Set Next Debug File Id
        /// </summary>
        private void SetNextDebugFileID()
        {
            if (this.fileid >= this.filecount)
                this.fileid = 1;
            else
                this.fileid++;
        }

        /// <summary>
        /// Check if we should log this information according to debug level settings
        /// </summary>
        /// <param name="setlevel">Debug level set for stream</param>
        /// <param name="requestlevel">Debug level to log</param>
        /// <returns>Returns true if we should display this info</returns>
        //private bool DoDebug(int setlevel, DebugLevel requestlevel)
        //{
        //    int l = (int)requestlevel;
        //    int ret = l & setlevel;
        //    return (ret != 0);
        //}

        ///// <summary>
        ///// Write Debug Data to stream (Consol and/or File)
        ///// </summary>
        ///// <param name="dblevel">Debug Level</param>
        ///// <param name="msg">Debug data</param>
        //public void Write(DebugLevel dblevel, string msg)
        //{
        //    // These will now show the file and line number
        //    DateTime now = DateTime.Now;

        //    lock (statusLock)
        //    {
        //        var callbackArray = this.callbacks.ToArray();
        //        foreach (DebugSockCallback cb in callbackArray)
        //        {
        //            if (!this.DoDebug(cb.dbglevel, dblevel)) 
        //                continue;

        //            string tcpdata = String.Format(@"{0}:{1}:{2}:{3} [{4:D4}] {5}",
        //                                           now.Hour.ToString("D2"),
        //                                           now.Minute.ToString("D2"),
        //                                           now.Second.ToString("D2"),
        //                                           now.Millisecond.ToString("D3"),
        //                                           Thread.CurrentThread.ManagedThreadId,
        //                                           msg
        //                );

        //            try
        //            {
        //                cb.callback.DebugStream(tcpdata);
        //            }
        //            catch
        //            {
        //                this.callbacks.Remove(cb);
        //            }
        //        }

        //        if (this.stream != null)
        //        {
        //            if (this.DoDebug(this.filelevel, dblevel))
        //            {
        //                string filedata = String.Format("{0}-{1}-{2}\t{3}:{4}:{5}:{6}\t{7:D4}\t{8}\t{9}",
        //                    now.Year,
        //                    now.Month.ToString("D2"),
        //                    now.Day.ToString("D2"),
        //                    now.Hour.ToString("D2"),
        //                    now.Minute.ToString("D2"),
        //                    now.Second.ToString("D2"),
        //                    now.Millisecond.ToString("D3"),
        //                    Thread.CurrentThread.ManagedThreadId,
        //                    (int)dblevel,
        //                    msg
        //                );

        //                try
        //                {
        //                    this.stream.WriteLine(filedata);
        //                    if (this.stream.BaseStream.Length > this.filesize)
        //                    {
        //                        this.SetNextDebugFileID();
        //                        this.OpenDebugFile(false);
        //                    }
        //                }
        //                catch
        //                {
        //                    try
        //                    {
        //                        this.stream.Flush();
        //                        this.stream.Close();
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    this.stream = null;
        //                }
        //            }
        //        }

        //        if (console)
        //        {
        //            if (this.DoDebug(this.conlevel, dblevel))
        //            {
        //                string condata = String.Format(@"{0}:{1}:{2}:{3} [{4:D4}] {5}",
        //                    now.Hour.ToString("D2"),
        //                    now.Minute.ToString("D2"),
        //                    now.Second.ToString("D2"),
        //                    now.Millisecond.ToString("D3"),
        //                    Thread.CurrentThread.ManagedThreadId,
        //                    msg
        //                );
        //                Console.WriteLine(condata);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Set Debug Level for Debug Mode
        /// </summary>
        /// <param name="mode">Debug mode to change</param>
        /// <param name="level">Debug level</param>
        /// <param name="exec">Name of Exec File</param>
        /// <param name="sockport">Socket sockport</param>
        //public void SetDebugLevel(DebugLevelMode mode, int level, string exec, int sockport)
        //{
        //    this.execfile = exec;
        //    this.port = sockport;

        //    switch (mode)
        //    {
        //        case DebugLevelMode.Console:
        //            this.conlevel = level;
        //            this.console = (this.conlevel != 0);
        //            break;
        //        case DebugLevelMode.File:
        //            this.filelevel = level;
        //            this.SetNextDebugFileID();
        //            if (this.filelevel == 0)
        //                this.CloseDebugFile();
        //            else
        //                this.OpenDebugFile(true);
        //            break;
        //        case DebugLevelMode.Socket:
        //            // this is done in list
        //            break;
        //    }
        //}

        /// <summary>
        /// Set Debug Level for Debug Mode
        /// </summary>
        /// <param name="callback">Callback connection for debugging stream</param>
        /// <param name="level">Debug level</param>
        /// <param name="exec">Name of Exec File</param>
        /// <param name="sockport">Socket sockport</param>
        public void SetDebugLevel(IComDebugCallback callback, int level, string exec, int sockport)
        {
            DebugSockCallback cbfound = null;
            lock (statusLock)
            {
                foreach (DebugSockCallback cb in this.callbacks)
                {
                    if (cb.callback != callback) 
                        continue;

                    cbfound = cb;
                    break;
                }

                if (cbfound == null)
                {
                    DebugSockCallback dsc = new DebugSockCallback();
                    dsc.callback = callback;
                    dsc.dbglevel = level;
                    this.callbacks.Add(dsc);
                    cbfound = dsc;
                }
                else
                {
                    cbfound.dbglevel = level;
                }

                if (level == 0)
                    this.callbacks.Remove(cbfound);
            }

            this.SetDebugLevel(DebugLevelMode.Socket, level, exec, sockport);
        }

        /// <summary>
        /// Set Filesize and File count for Debug files
        /// </summary>
        /// <param name="size">Max size of a debug file</param>
        /// <param name="count">Max number of debug files</param>
        public void SetFileValues(int size, int count)
        {
            this.filesize = size;
            this.filecount = count;
        }
    }
}
