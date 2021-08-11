using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.DD;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetail.DD.Common;
using LSRetail.DD.Control;

namespace LSOne.Services
{
    public partial class DDService : IDDService
    {
        private string currentUICultureName;
        RunJobOperation runJobOperation;
        private Task<bool> task;

        public JobValidationResult RunJob(JscJob job, Control control, bool forceNormal, JscSchedulerLog log, JscLocation location)
        {
            // Make sure that the job can be run
            JobValidationResult validationResult = DataProviderFactory.Instance.Get<IJobData, JscJob>().ValidateForRun(DLLEntry.DataModel, job);
            if (validationResult.Message != JobValidationMessage.OK)
            {
                return validationResult;
            }

            if (runJobOperation == null)
            {
                runJobOperation = new RunJobOperation(control);
            }

            runJobOperation.RunJobAsync(job, forceNormal, log, location);
            return validationResult;
        }
        
        public JobValidationResult RunJob(JscJob job, Control control, bool forceNormal = false)
        {
            // Make sure that the job can be run
            JobValidationResult validationResult = DataProviderFactory.Instance.Get<IJobData, JscJob>().ValidateForRun(DLLEntry.DataModel, job);
            if (validationResult.Message != JobValidationMessage.OK)
            {
                return validationResult;
            }

            if (runJobOperation == null)
            {
                runJobOperation = new RunJobOperation(control);
            }

            runJobOperation.RunJobAsync(job, forceNormal);
            return validationResult;
        
        }

        public void RunJobUnsecure(Guid jobid, string jobText, Control control)
        {
            if (runJobOperation == null)
            {
                runJobOperation = new RunJobOperation(control);
            }
            runJobOperation.RunJobAsyncUnsecure(jobid, jobText);

        }

        public void ReadDesign(Guid locationId, bool readTablesAndFields, bool updateExistingDesign,
            string newDescription, RecordIdentifier designID,  JscLocation locationItem)
        {
            int? port = ParsePort(locationItem.DDPort);
            DataDirector dataDirector = CreateDataDirector(locationItem.DDHost, locationItem.DDNetMode, port, locationItem.DBConnectionString);
            dataDirector.DoReadDesign(locationId,readTablesAndFields,updateExistingDesign,newDescription,designID);
        }

        public void RunPostTransactionJob(IPosTransaction transaction)
        {
            if(!string.IsNullOrEmpty(DLLEntry.Settings.FunctionalityProfile.PostTransactionDDJob) 
            && !string.IsNullOrEmpty(DLLEntry.Settings.FunctionalityProfile.DDSchedulerLocation))
            {
                try
                {
                    List<DataDirectorTransactionJob> pendingJobs = TransactionProviders.DataDirectorTransactionJobData.GetPendingJobs(DLLEntry.DataModel);

                    DataDirectorTransactionJob currentJob = new DataDirectorTransactionJob();
                    currentJob.TransactionID = transaction.TransactionId;
                    currentJob.StoreID = transaction.StoreId;
                    currentJob.TerminalID = transaction.TerminalId;
                    currentJob.JobID = new Guid(DLLEntry.Settings.FunctionalityProfile.PostTransactionDDJob);
                    currentJob.CreatedTime = DateTime.Now;
                    currentJob.IsNew = true;

                    bool reducedTimeout = transaction is LogOnOffTransaction && !((LogOnOffTransaction)transaction).Logon;

                    if (task != null && !task.IsCompleted)
                    {
                        pendingJobs.Add(currentJob);
                    }
                    else
                    {
                        task = new Task<bool>(() => ScheduleDDJob(DLLEntry.DataModel, currentJob, DLLEntry.Settings.FunctionalityProfile.DDSchedulerLocation, reducedTimeout));
                    }

                    Task<bool> tempTask = task;
                    foreach (DataDirectorTransactionJob dataDirectorTransactionJob in pendingJobs)
                    {
                        tempTask = tempTask.ContinueWith(continuation => continuation.Result && ScheduleDDJob(DLLEntry.DataModel, dataDirectorTransactionJob, DLLEntry.Settings.FunctionalityProfile.DDSchedulerLocation, reducedTimeout));
                    }

                    if (task.Status == TaskStatus.Created || task.Status == TaskStatus.WaitingToRun)
                    {
                        task.Start();
                    }

                    DLLEntry.Settings.ResetToPreviousState();
                }
                catch (Exception e)
                {
                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), e);
                    throw e;
                }
            }
        }

        private DataDirector CreateDataDirector(string host, NetMode netMode, int? port, string connectionString)
        {
            DataDirector dataDirector;

            if (port != null)
            {
                dataDirector = new DataDirector(host, netMode, port.Value, connectionString, currentUICultureName);
            }
            else
            {
                dataDirector = new DataDirector(host, netMode, connectionString, currentUICultureName);
            }

            return dataDirector;
        }

        private int? ParsePort(string port)
        {
            int numPort;
            if (!int.TryParse(port, out numPort))
                return null;
            return numPort;
        }

        private bool ScheduleDDJob(IConnectionManager entry, DataDirectorTransactionJob job, string schedulerLocation, bool reducedTimeout)
        {
            TransAutomClient client = null;
            IConnectionManagerTemporary tempConnection = entry.CreateTemporaryConnection();

            try
            {
                client = new TransAutomClient();

                if (reducedTimeout)
                {
                    client.SetConnTimeOut(1000);
                }

                bool result;
                string[] parameters = { (string)job.TransactionID, (string)job.StoreID, (string)job.TerminalID };

                if (entry.IsCloud ||
                    File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "LS Retail", "Global", "localscheduler.txt")))
                {
                    result = client.SendSchedulerJob(schedulerLocation, job.JobID.ToString(), null, parameters);
                }
                else
                {
                    result = client.SendSchedulerJob(schedulerLocation, job.JobID.ToString(),
                        $"{(string)entry.CurrentStoreID}-{(string)entry.CurrentTerminalID}", parameters);
                }

                if (!result)
                {
                    tempConnection.ErrorLogger.LogMessage(LogMessageType.Error, "Send job to scheduler: " + client.GetLastErrorMessage());

                    if (job.IsNew)
                    {
                        TransactionProviders.DataDirectorTransactionJobData.Save(tempConnection, job);
                    }
                }
                else if (!job.IsNew)
                {
                    TransactionProviders.DataDirectorTransactionJobData.Delete(tempConnection, job.ID);
                }

                return result;
            }
            catch (Exception ex)
            {
                if (client != null)
                {
                    tempConnection.ErrorLogger.LogMessage(LogMessageType.Error, "Send job to scheduler: " + client.GetLastErrorMessage());
                }
                else
                {
                    tempConnection.ErrorLogger.LogMessage(LogMessageType.Error, "Send job to Scheduler", ex);
                }
            }
            finally
            {
                tempConnection.Close();
            }

            return false;
        }

        public IErrorLog ErrorLog
        {
            set {  }
        }

        public void Init(IConnectionManager entry)
        {
            LSRetail.DD.Common.AppConfig.Init(true);
            DLLEntry.DataModel = entry;

            currentUICultureName = Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
