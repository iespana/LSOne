using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;
using LSRetail.DD.Control;

namespace LSOne.Services.DD
{
    internal sealed class DataDirector : IDisposable
    {
        private const NetMode defaultNetMode = NetMode.TCP;
        private const int defaultTimeoutSeconds = 0;

        private readonly object monitor = new object();
        private delegate void TestConnectionDelegate();
        private delegate Guid ReadDesignDelegate(Guid locationId, bool readTablesAndFields, bool updateExistingDesign, string newDescription);

        private delegate Guid ReadDesignDelegateDirect(RecordIdentifier design, bool readTablesAndFields, bool updateExistingDesign, string newDescription);
        private delegate void ReadFieldDesignDelegate(Guid locationId, Guid tableId);

        private int timeoutSeconds;
        private string host;
        private int port;
        private NetMode netMode;
        private string connectionString;
        private CultureInfo cultureInfo;

        private bool cancelPending;

        private TransAutomClient privateDataClient;
        
        public DataDirector(string host, string connectionString, string cultureInfoName)
        {
            this.host = host;
            this.netMode = defaultNetMode;
            this.port = AppConfig.GetRouterPortByMode( netMode);
            this.connectionString = connectionString;
            this.timeoutSeconds = defaultTimeoutSeconds;
            this.cultureInfo = new CultureInfo(cultureInfoName);
        }

        public DataDirector(string host, NetMode netMode, string connectionString, string cultureInfoName)
        {
            this.host = host;
            this.netMode = netMode;
            port = AppConfig.GetRouterPortByMode(this.netMode);
            this.connectionString = connectionString;
            timeoutSeconds = defaultTimeoutSeconds;
            cultureInfo = new CultureInfo(cultureInfoName);
        }

        public DataDirector(string host, NetMode netMode, int port, string connectionString, string cultureInfoName)
        {
            this.host = host;
            this.netMode = netMode;
            this.port = port;
            this.connectionString = connectionString;
            this.timeoutSeconds = defaultTimeoutSeconds;
            this.cultureInfo = new CultureInfo(cultureInfoName);
        }

        public DataDirector(string host, NetMode netMode, int port, string connectionString, int timeoutSeconds, string cultureInfoName)
        {
            this.host = host;
            this.netMode = netMode;
            this.port = port;
            this.connectionString = connectionString;
            this.timeoutSeconds = timeoutSeconds;
            this.cultureInfo = new CultureInfo(cultureInfoName);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (privateDataClient != null)
                {
                    privateDataClient.Disconnect();
                    privateDataClient = null;
                }
               
            }
        }

        private LSRetail.DD.Control.TransAutomClient DataClient
        {
            get
            {
                if (privateDataClient == null)
                {
                    privateDataClient = new LSRetail.DD.Control.TransAutomClient();
                    privateDataClient.SetCopyright("Copyright: Landsteinar Strengur Development");

                    privateDataClient.SetConnTimeOut(timeoutSeconds * 1000);

                    string server = string.Format("{0}:{1}:{2}", host, port, netMode.ToString());
                    privateDataClient.CreateConnection(server, connectionString, true);
                }

                return privateDataClient;
            }

        }

        public bool IsBusy { get; private set; }

        public event EventHandler<ProgressEventArgs> ProgressReported;

        public void StartTestConnection()
        {
            TestConnectionDelegate worker = TestConnection;
            AsyncCallback completedCallback = TestConnectionCompletedCallback;

            lock (monitor)
            {
                if (IsBusy)
                    throw new InvalidOperationException("The control is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                worker.BeginInvoke(completedCallback, async);
                IsBusy = true;
            }
        }

        public event EventHandler<AsyncCompletedEventArgs> StartTestConnectionCompleted;

        public void DoReadDesign(Guid locationId, bool readTablesAndFields, bool updateExistingDesign, string newDescription, RecordIdentifier designID =null)
        {
            lock (monitor)
            {
                if (IsBusy)
                    throw new InvalidOperationException("The control is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                if (designID == null)
                {
                    ReadDesign(locationId, readTablesAndFields, updateExistingDesign, newDescription);
                }
                else
                {
                    ReadDesign(designID, readTablesAndFields, updateExistingDesign, newDescription);
                }

                IsBusy = true;
            }
        }


        public event EventHandler<AsyncCompletedEventArgs> StartReadDesignCompleted;


        public void StartReadFieldDesign(Guid locationId, Guid tableId)
        {
            ReadFieldDesignDelegate worker = ReadFieldDesign;
            AsyncCallback completedCallback = ReadFieldDesignCompletedCallback;

            lock (monitor)
            {
                if (IsBusy)
                    throw new InvalidOperationException("The control is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                worker.BeginInvoke(locationId, tableId, completedCallback, async);
                IsBusy = true;
            }
        }
        public event EventHandler<AsyncCompletedEventArgs> StartReadFieldDesignCompleted;



        private void TestConnectionCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            TestConnectionDelegate worker = (TestConnectionDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            AsyncCompletedEventArgs completedArgs;
            try
            {
                // finish the asynchronous operation
                worker.EndInvoke(ar);
                completedArgs = new AsyncCompletedEventArgs(null, cancelPending, null);
            }
            catch (Exception ex)
            {
                completedArgs = new AsyncCompletedEventArgs(ex, false, null);
            }

            // clear the running task flag
            lock (monitor)
            {
                IsBusy = false;
            }

            // raise the completed event
            async.PostOperationCompleted(delegate(object e) { OnStartTestConnectionCompleted((AsyncCompletedEventArgs)e); }, completedArgs);
        }


        private void ReadDesignCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            ReadDesignDelegate worker = (ReadDesignDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            AsyncCompletedEventArgs completedArgs;
            try
            {
                // finish the asynchronous operation
                Guid databaseDesign = worker.EndInvoke(ar);
                completedArgs = new AsyncCompletedEventArgs(null, cancelPending, databaseDesign);
            }
            catch (Exception ex)
            {
                completedArgs = new AsyncCompletedEventArgs(ex, false, null);
            }

            // clear the running task flag
            lock (monitor)
            {
                IsBusy = false;
            }

            // raise the completed event
            async.PostOperationCompleted(delegate(object e) { OnReadDesignCompleted((AsyncCompletedEventArgs)e); }, completedArgs);
        }

        private void ReadDesignDirectCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            ReadDesignDelegateDirect worker = (ReadDesignDelegateDirect)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            AsyncCompletedEventArgs completedArgs;
            try
            {
                // finish the asynchronous operation
                Guid databaseDesign = worker.EndInvoke(ar);
                completedArgs = new AsyncCompletedEventArgs(null, cancelPending, databaseDesign);
            }
            catch (Exception ex)
            {
                completedArgs = new AsyncCompletedEventArgs(ex, false, null);
            }

            // clear the running task flag
            lock (monitor)
            {
                IsBusy = false;
            }

            // raise the completed event
            async.PostOperationCompleted(delegate(object e) { OnReadDesignCompleted((AsyncCompletedEventArgs)e); }, completedArgs);
        }


        private void ReadFieldDesignCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            ReadFieldDesignDelegate worker = (ReadFieldDesignDelegate)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            AsyncCompletedEventArgs completedArgs;
            try
            {
                // finish the asynchronous operation
                worker.EndInvoke(ar);
                completedArgs = new AsyncCompletedEventArgs(null, cancelPending, null);
            }
            catch (Exception ex)
            {
                completedArgs = new AsyncCompletedEventArgs(ex, false, null);
            }

            // clear the running task flag
            lock (monitor)
            {
                IsBusy = false;
            }

            // raise the completed event
            async.PostOperationCompleted(delegate(object e) { OnReadFieldDesignCompleted((AsyncCompletedEventArgs)e); }, completedArgs);
        }


        private void OnStartTestConnectionCompleted(AsyncCompletedEventArgs e)
        {
            if (StartTestConnectionCompleted != null)
                StartTestConnectionCompleted(this, e);
        }

        private void OnProgressReported(ProgressEventArgs e)
        {
            if (ProgressReported != null)
                ProgressReported(this, e);
        }

        private void OnReadDesignCompleted(AsyncCompletedEventArgs e)
        {
            if (StartReadDesignCompleted != null)
                StartReadDesignCompleted(this, e);
        }

        private void OnReadFieldDesignCompleted(AsyncCompletedEventArgs e)
        {
            if (StartReadFieldDesignCompleted != null)
                StartReadFieldDesignCompleted(this, e);
        }

        private void ReportProgress(string msg)
        {
            ReportProgress(msg, null);
        }

        private void ReportProgress(string msg, float? progress)
        {
            ProgressEventArgs e;
            if (progress.HasValue)
                e = new ProgressEventArgs { Message = msg, Percentage = Convert.ToInt32(100 * progress.Value) };
            else
                e = new ProgressEventArgs { Message = msg };
            OnProgressReported(e);
            if (e.Cancel)
                cancelPending = true;
        }

        private void TestConnection()
        {
            Thread.CurrentThread.CurrentUICulture = this.cultureInfo;

            DataClient.SetMsgId("TESTCONNECITON;Test current location");
            DataClient.SetRequestType(0);
            DataClient.SetResultType(1);
            DataClient.SetTable("#GPDD_TABLE");
            DataClient.AddField("ID", 8);
            DataClient.AddField("NAME", 16);
            DataClient.SetMaxRec(10);
            if (!DataClient.Send())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorSendFailedError);
            }

            ReportProgress(Properties.Resources.DataDirectorConnectedMsg);
            if (cancelPending)
                return;

            if (!DataClient.HasAnswer())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorNoAnswerError);
            }

            int numTables = 0;
            while (DataClient.NextValueList())
            {
                numTables++;
                if (numTables % 8 == 0)
                {
                    ReportProgress(string.Format(Properties.Resources.DataDirectorTablesReadMsg, numTables));
                    if (cancelPending)
                        return;
                }
            }
        }

        private Guid ReadDesign(Guid locationId, bool readTablesAndFields, bool updateExistingDesign,
            string newDescription)
        {
            JscLocation location = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(DLLEntry.DataModel, locationId);
            if (location == null)
                throw new ArgumentException("Location " + locationId.ToString() + " was not found");
            var reply  = ReadDesign(location.DatabaseDesign, readTablesAndFields, updateExistingDesign, newDescription);

            DataProviderFactory.Instance.Get<ILocationData, JscLocation>().Save(DLLEntry.DataModel, location);
            return reply;
        }
       

        private Guid ReadDesign(RecordIdentifier databaseDesignID, bool readTablesAndFields, bool updateExistingDesign,
                                string newDescription)
        {
            Thread.CurrentThread.CurrentUICulture = this.cultureInfo;

            // Make progress bar look intelligent
            float progressStep1ReadTables;
            float progressStep2SaveTables;
            float progressStep3ReadFields;
            float progressStep4SaveFields;

            if (readTablesAndFields)
            {
                progressStep1ReadTables = 0.05F;
                progressStep2SaveTables = 0.05F;
                progressStep3ReadFields = 0.7F;
                progressStep4SaveFields = 0.2F;
            }
            else
            {
                progressStep1ReadTables = 0.8F;
                progressStep2SaveTables = 0.2F;
                progressStep3ReadFields = 0; // Not used
                progressStep4SaveFields = 0; // Not used
            }

            // Progress step 1: Read table names
            float currentProgress = 0;

            float currentProgressSpace = progressStep1ReadTables;
            DataClient.SetMsgId("READDESIGN;Get location design");


            Guid currentDatabaseDesignID = Guid.Empty;
            IDictionary<string, TableDesignInfo> tableDesigns;
            PrepareDatabaseDesign(
                databaseDesignID, 
                updateExistingDesign, 
                newDescription, 
                out tableDesigns,
                out currentDatabaseDesignID);
            if (tableDesigns == null || cancelPending)
                return Guid.Empty;

            FetchAllTables(currentDatabaseDesignID, tableDesigns, currentProgress);
            if (cancelPending)
                return Guid.Empty;

            // Progress step 2: Save tables
            currentProgress += currentProgressSpace;
            currentProgressSpace += progressStep2SaveTables;
            ReportProgress(Properties.Resources.DataDirectorSavingTablesMsg, currentProgress);
            

            if (readTablesAndFields)
            {
                // Progress step 3: Read fields
                currentProgress += currentProgressSpace;
                currentProgressSpace = progressStep3ReadFields;
                FetchAllFields(tableDesigns, currentProgress, currentProgressSpace);
                if (cancelPending)
                    return Guid.Empty;

                currentProgress += currentProgressSpace;
                currentProgressSpace += progressStep4SaveFields;
                ReportProgress(Properties.Resources.DataDirectorSavingFieldsMsg, currentProgress);
                if (cancelPending)
                    return Guid.Empty;
            }

            return currentDatabaseDesignID;


        }


        private void ReadFieldDesign(Guid locationId, Guid tableDesignId)
        {
            Thread.CurrentThread.CurrentUICulture = this.cultureInfo;

            // Make progress bar look intelligent
            float progressStep1ReadFields = 0.7F;
            float progressStep2SaveFields = 0.2F;


            //??DataClient.SetMsgId("READDESIGN;Get location design");

            JscTableDesign tableDesign = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesign(DLLEntry.DataModel, tableDesignId);
            if (tableDesign == null)
            {
                throw new ArgumentException("Unknown table design " + tableDesignId.ToString());
            }

            float currentProgressSpace = progressStep1ReadFields;
            float currentProgress = 0;
            ReportProgress(Properties.Resources.DataDirectorReadingFieldsMsg, currentProgress);

            ReadFieldsOfTable(tableDesignId, tableDesign.TableName);
            if (cancelPending)
                return;

            currentProgress += currentProgressSpace;
            currentProgressSpace += progressStep2SaveFields;
            ReportProgress(Properties.Resources.DataDirectorSavingFieldsMsg, currentProgress);
        }



        private class TableDesignInfo
        {
            public JscTableDesign TableDesign { get; set; }
            public bool IsUsed { get; set; }
        }

        private class FieldDesignInfo
        {
            public JscFieldDesign FieldDesign { get; set; }
            public bool IsUsed { get; set; }
        }



        private void PrepareDatabaseDesign(JscLocation location, bool updateExistingDesign, string newDescription, out IDictionary<string, TableDesignInfo> tableDesigns, out Guid databaseDesignId)
        {
            // Start by loading all existing tables (if any) into a dictionary.
            tableDesigns = new Dictionary<string, TableDesignInfo>(StringComparer.InvariantCultureIgnoreCase);

            if (updateExistingDesign && location.DatabaseDesign != null)
            {
                databaseDesignId = (Guid)location.DatabaseDesign;
                foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(DLLEntry.DataModel, location.DatabaseDesign, false))
                {
                    tableDesigns.Add(tableDesign.TableName, new TableDesignInfo { TableDesign = tableDesign });
                }
            }
            else
            {
                JscDatabaseDesign databaseDesign = new JscDatabaseDesign();
                databaseDesign.Description = newDescription;
                databaseDesign.Enabled = true;
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(DLLEntry.DataModel, databaseDesign);
                databaseDesignId = (Guid)databaseDesign.ID;
            }
        }

        private void PrepareDatabaseDesign(RecordIdentifier dDesign, bool updateExistingDesign, string newDescription, out IDictionary<string, TableDesignInfo> tableDesigns, out Guid databaseDesignId)
        {
            // Start by loading all existing tables (if any) into a dictionary.
            tableDesigns = new Dictionary<string, TableDesignInfo>(StringComparer.InvariantCultureIgnoreCase);

            if (updateExistingDesign )
            {
                databaseDesignId = (Guid)dDesign;
                foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(DLLEntry.DataModel, dDesign, false))
                {
                    tableDesigns.Add(tableDesign.TableName, new TableDesignInfo { TableDesign = tableDesign });
                }
            }
            else
            {
                JscDatabaseDesign databaseDesign = new JscDatabaseDesign();
                databaseDesign.Description = newDescription;
                databaseDesign.Enabled = true;
                DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(DLLEntry.DataModel, databaseDesign);
                databaseDesignId = (Guid)databaseDesign.ID;
            }
        }

        
        private void FetchAllTables(Guid databaseDesignId, IDictionary<string, TableDesignInfo> tableDesigns, float initProgress)
        {
            // Query DD for all tables
            DataClient.SetRequestType(0);
            DataClient.SetResultType(1);
            DataClient.SetTable("#GPDD_TABLE");
            DataClient.AddField("ID", 8);
            DataClient.AddField("NAME", 16);
            if (!DataClient.Send())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorSendFailedError);
            }

            ReportProgress(Properties.Resources.DataDirectorConnectedMsg);
            if (cancelPending)
                return;

            if (!DataClient.HasAnswer())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorNoAnswerError);
            }

            int numTables = 0;
            while (DataClient.NextValueList())
            {
                numTables++;
                if (numTables % 8 == 0)
                {
                    ReportProgress(string.Format(Properties.Resources.DataDirectorTablesReadMsg, numTables), initProgress);
                    if (cancelPending)
                        return;
                }

                // Find table in dictionary
                string tableName = DataClient.GetValueNo(1) as string;
                TableDesignInfo info;
                if (!tableDesigns.TryGetValue(tableName, out info))
                {
                    // New table
                    JscTableDesign tableDesign = new JscTableDesign();
                    tableDesign.TableName = tableName;
                    tableDesign.DatabaseDesign = databaseDesignId;
                    tableDesign.Enabled = true;
                    DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(DLLEntry.DataModel, tableDesign);

                    // New dictionary entry
                    tableDesigns.Add(tableDesign.TableName, new TableDesignInfo { TableDesign = tableDesign, IsUsed = true });
                }
                else
                {
                    if (!info.TableDesign.Enabled)
                    {
                        info.TableDesign.Enabled = true;
                    }
                    info.IsUsed = true;
                }
            }

            // Mark all unused tables as disabled
            foreach (var tableDesignInfo in tableDesigns.Values)
            {
                if (!tableDesignInfo.IsUsed)
                {
                    tableDesignInfo.TableDesign.Enabled = false;
                }
            }
        }


        private void FetchAllFields(IDictionary<string, TableDesignInfo> tableDesigns, float initProgress, float progressSpace)
        {
            int numTables = 0;
            foreach (var tableDesignInfo in tableDesigns.Values)
            {
                numTables++;

                if (!tableDesignInfo.IsUsed)
                    continue;

                ReadFieldsOfTable((Guid)tableDesignInfo.TableDesign.ID, tableDesignInfo.TableDesign.TableName);

                float progress = initProgress + numTables * progressSpace / tableDesigns.Count;
                ReportProgress(string.Format(Properties.Resources.DataDirectorReadFieldsForTablesMsg, numTables), progress);
                if (cancelPending)
                    return;

            }

        }

        private void ReadFieldsOfTable(Guid tableDesignId, string tableName)
        {
            // Load existing fields into a dictionary for quick lookup
            IDictionary<string, FieldDesignInfo> fieldDesigns = new Dictionary<string, FieldDesignInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var fieldDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesigns(DLLEntry.DataModel, tableDesignId))
            {
                fieldDesigns.Add(fieldDesign.FieldName, new FieldDesignInfo { FieldDesign = fieldDesign });
            }

            // Request field types from DD for current table design
            DataClient.SetRequestType(0);
            DataClient.SetResultType(1);
            DataClient.SetTable("#GPDD_FIELD");

            DataClient.AddField("ID", 8);
            DataClient.AddField("NAME", 16);
            DataClient.AddField("TYPE", 8);
            DataClient.AddField("LENGTH", 8);
            DataClient.AddField("CLASS", 8);

            DataClient.AddParamField("NAME", 16, 1);
            DataClient.AddParam(tableName);

            if (!DataClient.Send())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorSendFailedError);
            }

            if (!DataClient.HasAnswer())
            {
                throw CreateDataDirectorException(Properties.Resources.DataDirectorNoAnswerError);
            }

            int sequence = 0;

            while (DataClient.NextValueList())
            {
                sequence++;

                // Get field values from DD
                string fieldName = DataClient.GetValueNo(1) as string;
                DDDataType fieldType = (DDDataType)DataClient.GetValueNo(2);
                int fieldLength = Convert.ToInt32(DataClient.GetValueNo(3));
                short fieldClass = Convert.ToInt16(DataClient.GetValueNo(4));

                // Insert or update field data
                FieldDesignInfo fieldDesignInfo;
                if (fieldDesigns.TryGetValue(fieldName, out fieldDesignInfo))
                {
                    fieldDesignInfo.IsUsed = true;

                    // Compare values and update if required
                    if (fieldDesignInfo.FieldDesign.Sequence != sequence)
                    {
                        fieldDesignInfo.FieldDesign.Sequence = sequence;
                    }
                    if (fieldDesignInfo.FieldDesign.DataType != fieldType)
                    {
                        fieldDesignInfo.FieldDesign.DataType = fieldType;
                    }
                    if (fieldDesignInfo.FieldDesign.Length.GetValueOrDefault() != fieldLength)
                    {
                        fieldDesignInfo.FieldDesign.Length = fieldLength;
                    }
                    if (fieldDesignInfo.FieldDesign.FieldClass != fieldClass)
                    {
                        fieldDesignInfo.FieldDesign.FieldClass = fieldClass;
                    }
                    if (!fieldDesignInfo.FieldDesign.Enabled)
                    {
                        fieldDesignInfo.FieldDesign.Enabled = true;
                    }
                }
                else
                {
                    fieldDesignInfo = new FieldDesignInfo();
                    fieldDesignInfo.IsUsed = true;
                    fieldDesignInfo.FieldDesign = new JscFieldDesign();
                    fieldDesignInfo.FieldDesign.FieldName = fieldName;
                    fieldDesignInfo.FieldDesign.DataType = fieldType;
                    if (fieldLength != 0)
                        fieldDesignInfo.FieldDesign.Length = fieldLength;
                    fieldDesignInfo.FieldDesign.Sequence = sequence;
                    fieldDesignInfo.FieldDesign.FieldClass = fieldClass;
                    fieldDesignInfo.FieldDesign.Enabled = true;
                    fieldDesignInfo.FieldDesign.TableDesign = tableDesignId;
                    DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(DLLEntry.DataModel, fieldDesignInfo.FieldDesign);
                    fieldDesigns.Add(fieldDesignInfo.FieldDesign.FieldName, fieldDesignInfo);
                }
            }

            // Mark all unused fields as disabled
            foreach (var fieldDesignInfo in fieldDesigns.Values)
            {
                if (!fieldDesignInfo.IsUsed)
                {
                    fieldDesignInfo.FieldDesign.Enabled = false;
                }
            }
        }



        private Exception CreateDataDirectorException(string msg)
        {
            return new DataDirectorException(string.Format(msg, DataClient.GetLastErrorMessage()));
        }
    }


    public class ProgressEventArgs : EventArgs
    {
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the percentage completed, ranged from 0-100.
        /// </summary>
        public int? Percentage { get; set; }
        public bool Cancel { get; set; }
    }

}
