using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO.JSON;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;
using LSOne.ViewPlugins.Scheduler.DataProviders;

namespace LSOne.ViewPlugins.Scheduler
{
    internal class JobCatalog
    {

        public Guid JobId { get; set; }
        public bool ForceNormalJob { get; set; }
        public string JobDescription { get; set; }
        public Guid JobLogID { get; set; }
        public Guid locationID { get; set; }

    }
    internal class RunJobOperation
    {
        
        private ActionStatus actionStatus;

        private object counterMonitor = new object();
        private int pendingJobCount;
        private int pendingErrors;

        public RunJobOperation()
        {
            var parentControl = PluginEntry.Framework.MainWindow as Control;
            if (parentControl != null)
            {
                actionStatus = new ActionStatus(parentControl, ActionStatusLayout.CenteredAutosize, Properties.Resources.Close, null);
                actionStatus.ActionClicked += new EventHandler(actionStatus_ActionClicked);
            }
        }


        public void RunJobAsync(JscJob job, bool forceNormal = false, JscSchedulerLog jobLogID = null,JscLocation locationID = null)
        {
            lock (counterMonitor)
            {
                pendingJobCount++;
                UpdateStatus();
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunJobThreadMain),

                new JobCatalog
                {
                    ForceNormalJob = forceNormal,
                    JobDescription = job.Text,
                    JobId = (Guid)job.ID,
                    locationID = locationID == null ? Guid.Empty : (Guid)locationID.ID,
                    JobLogID = jobLogID == null ? Guid.Empty : (Guid)jobLogID.ID,

                });

        }

        private void RunJobThreadMain(object obj)
        {
            JobCatalog catalog = (JobCatalog)obj;
            var jobId = catalog.JobId;
            var jobDescription = catalog.JobDescription;

            // Submit the job to the job scheduler service
            string errorMsg = null;
            string resultMsg = null;
            try
            {
                using (SchedulerClient client = CreateSchedulerClient())
                {
                    if (catalog.ForceNormalJob || catalog.JobLogID != Guid.Empty)
                    {
                        Dictionary<string, string> jobParams= new Dictionary<string, string>();

                        jobParams["JobID"] = jobId.ToString();

                        jobParams["ForceNormalReplication"] = catalog.ForceNormalJob ? "true": "false";
                        if (catalog.JobLogID != RecordIdentifier.Empty && catalog.JobLogID != Guid.Empty)
                        {
                            jobParams["LogEntry"] = catalog.JobLogID.ToString();
                        }
                        if (catalog.locationID != RecordIdentifier.Empty && catalog.locationID != Guid.Empty)
                        {
                            jobParams["Location"] = catalog.locationID.ToString();
                        }
                        string commandParams =  JsonConvert.SerializeObject(jobParams);
                       
                        resultMsg = client.RunJob(commandParams); 
                    }
                    else
                    {
                        resultMsg = client.RunJob(jobId);    
                    }
                    
                }
            }
            catch (Exception ex)
            {
                string detailMsg;
                bool doLogToDatabase = false;
                if (ex is System.ServiceModel.EndpointNotFoundException)
                {
                    detailMsg = Properties.Resources.JobRunNoResponseMsg;
                    doLogToDatabase = true;
                }
                else if (ex is SchedulerServiceException)
                {
                    detailMsg = string.Format(Properties.Resources.JobRunServiceError, ex.Message);
                }
                else
                {
                    detailMsg = string.Format(Properties.Resources.JobRunExceptionMsg, ex.GetType().FullName, ex.Message);
                    doLogToDatabase = true;
                }

                errorMsg = string.Format(Properties.Resources.JobRunErrorMsg, jobDescription, detailMsg);
                if (doLogToDatabase)
                {
                    LogToDatabase(jobId, errorMsg);
                }
            }

            lock (counterMonitor)
            {
                pendingJobCount--;
                if (errorMsg != null)
                {
                    pendingErrors++;
                }
                UpdateStatus();
            }
            
            if (errorMsg != null)
            {
                PluginEntry.Framework.LogMessage(LogMessageType.Error, errorMsg);
            }
            else
            {
                string msg;
                if (!string.IsNullOrWhiteSpace(resultMsg))
                {
                    msg = resultMsg;
                }
                else
                {
                    msg = string.Format(Properties.Resources.JobRunOkMsg, jobDescription);
                }
                PluginEntry.Framework.LogMessage(LogMessageType.Trace, msg);
            }
        }

        private void LogToDatabase(Guid jobId, string message)
        {

            var logEntry = new JscSchedulerLog();
            logEntry.Job = jobId;
            logEntry.Message = message;
            logEntry.RegTime = DateTime.Now;
            DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, logEntry);

        }



        private static SchedulerClient CreateSchedulerClient()
        {
            var serverSettings = PluginEntry.SchedulerSettings.ServerSettings;
            return CreateSchedulerClient(serverSettings);
        }

        private static SchedulerClient CreateSchedulerClient(ServerSettings serverSettings)
        {
            int port = 0;
            if (!string.IsNullOrWhiteSpace(serverSettings.Port))
            {
                if (!int.TryParse(serverSettings.Port, out port))
                {
                    port = 0;
                }
            }

            return new SchedulerClient(serverSettings.Host, port, serverSettings.NetMode);
        }


        private void UpdateStatus()
        {
            if (actionStatus == null)
            {
                return;
            }

            lock (counterMonitor)
            {
                string message = string.Empty;
                if (pendingJobCount > 0)
                {
                    message = string.Format(Properties.Resources.RunJobPendingStatusFormat, pendingJobCount);
                }
                if (pendingErrors > 0)
                {
                    if (message.Length > 0)
                    {
                        message += " ";
                    }
                    message += string.Format(Properties.Resources.RunJobErrorStatusFormat, pendingErrors);
                }

                if (message.Length > 0)
                {
                    actionStatus.Set(message.Trim(), pendingJobCount > 0, true);
                }
                else
                {
                    actionStatus.Clear();
                }
            }
        }

        private void actionStatus_ActionClicked(object sender, EventArgs e)
        {
            lock (counterMonitor)
            {
                pendingErrors = 0;
            }
            actionStatus.Clear();
        }


    }
}
