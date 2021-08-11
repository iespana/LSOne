using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class SendJobDialog : DialogBase
    {
        private DataEntity emptyItem;
        private JscJob jobToSend;
        private JscLocation selectedLocation;
        private IConnectionManager remoteConnection;

        public SendJobDialog(JscJob jobToSend)
        {
            InitializeComponent();
            
            this.jobToSend = jobToSend;

            emptyItem = new DataEntity(RecordIdentifier.Empty, "");
            cmbLocation.SelectedData = emptyItem;
        }

        public JscJob Job { get; private set; }
        public Guid JobId { get; private set; }

        private void NewJobDialog_Shown(object sender, EventArgs e)
        {
            
        }

        private void cmbLocation_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> locations = new List<DataEntity>(DDProviders.LocationData.GetLocationsWhereConnectable(PluginEntry.DataModel));            

            cmbLocation.SetData(locations, null, true);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!ConnectRemoteConnection(false))
            {
                return;
            }

            JscJob jobToSendClone = DDProviders.JobData.GetJob(PluginEntry.DataModel, jobToSend.ID);
            jobToSendClone.JscJobTriggers = null; // We don't want to send the triggers since we don't want the job to start on the other end
            jobToSendClone.Source = null;
            jobToSendClone.JscSourceLocation = null;
            jobToSendClone.Destination = null;
            jobToSendClone.JscDestinationLocation = null;

            List<JscSubJob> subJobsToSend = DDProviders.JobData.GetJobSubjobValues(PluginEntry.DataModel, jobToSend.ID);
        
            // Check if there's no database design on the other end and send our design if no design exists
            List<DataSelector> remoteDesigns = DDProviders.DesignData.GetDatabaseDesignList(remoteConnection);
            RecordIdentifier tableDesignID = RecordIdentifier.Empty;

            foreach (JscSubJob subJob in subJobsToSend)
            {
                if (subJob.ReplicationMethod != ReplicationTypes.Procedure && subJob.TableFrom != RecordIdentifier.Empty)
                {
                    tableDesignID = subJob.TableFrom;
                    break;
                }
            }
           
            RecordIdentifier databaseDesignID = RecordIdentifier.Empty;
            JscTableDesign firstTableDesign = DDProviders.DesignData.GetTableDesign(PluginEntry.DataModel, tableDesignID);

            // If there's no subjob on the job with a from-table (e.g. all subjobs are procedure jobs) then we can't send any design information
            if (tableDesignID != RecordIdentifier.Empty && firstTableDesign != null)
            {

                
                databaseDesignID = firstTableDesign.DatabaseDesign;

                if (remoteDesigns.Count == 0)
                {

                    // Database
                    DDProviders.DesignData.Save(remoteConnection, DDProviders.DesignData.GetDatabaseDesign(PluginEntry.DataModel, databaseDesignID));

                    // Tables
                    IEnumerable<JscTableDesign> tableDesigns = DDProviders.DesignData.GetTableDesigns(PluginEntry.DataModel, databaseDesignID, false);
                    List<JscFieldDesign> fieldDesigns = new List<JscFieldDesign>();
                    List<JscLinkedFilter> linkedFilters = new List<JscLinkedFilter>();

                    foreach (JscTableDesign tableDesign in tableDesigns)
                    {
                        DDProviders.DesignData.Save(remoteConnection, tableDesign);
                        fieldDesigns.AddRange(DDProviders.DesignData.GetFieldDesigns(PluginEntry.DataModel, tableDesign.ID));
                        linkedFilters.AddRange(DDProviders.DesignData.GetLinkedFilters(PluginEntry.DataModel, tableDesign.ID));
                    }

                    // Fields                
                    foreach (JscFieldDesign fieldDesign in fieldDesigns)
                    {
                        DDProviders.DesignData.Save(remoteConnection, fieldDesign);
                    }

                    // Linked tables
                    IEnumerable<JscLinkedTable> linkedTables = DDProviders.DesignData.GetLinkedTables(PluginEntry.DataModel, databaseDesignID);

                    foreach (JscLinkedTable linkedTable in linkedTables)
                    {
                        DDProviders.DesignData.Save(remoteConnection, linkedTable);
                    }

                    // From table filter
                    foreach (JscLinkedFilter linkedFilter in linkedFilters)
                    {
                        DDProviders.DesignData.Save(remoteConnection, linkedFilter);
                    }
                }

                // If the destination only has one design that is not the same original design that we have here, we should try and map the table designs
                // over as much as possible.
                if (remoteDesigns.Count == 1 && remoteDesigns[0].GuidId != databaseDesignID)
                {
                    // Get remote table designs
                    IEnumerable<JscTableDesign> remoteTableDesigns = DDProviders.DesignData.GetTableDesigns(remoteConnection, remoteDesigns[0].GuidId, false);
                    IEnumerable<JscTableDesign> tableDesigns = DDProviders.DesignData.GetTableDesigns(PluginEntry.DataModel, databaseDesignID, false);

                    foreach (JscSubJob subJob in subJobsToSend)
                    {
                        JscTableDesign tableDesignFrom = tableDesigns.Single(p => p.ID == subJob.TableFrom);
                        JscTableDesign mappedDesign = remoteTableDesigns.FirstOrDefault(p => p.TableName == tableDesignFrom.TableName);
                        subJob.TableFrom = mappedDesign?.ID;
                    }
                }
                else if (remoteDesigns.Count != 0 && !remoteDesigns.Exists(p => p.GuidId == databaseDesignID))
                {
                    // If there's no matching design on the other end we can't assume that any one of them is the "correct" one. We simply clear all table designs
                    // from the subjobs.

                    foreach (JscSubJob subJob in subJobsToSend)
                    {
                        subJob.TableFrom = null;
                    }
                }
            }

            foreach (JscSubJob subJob in subJobsToSend)
            {
                if (subJob.TableFrom != null)
                {
                    subJob.JscSubJobFromTableFilters = DDProviders.JobData.GetSubJobFromTableFiltersList(PluginEntry.DataModel, subJob);
                }

                DDProviders.JobData.Save(remoteConnection, subJob);                        
            }

            DDProviders.JobData.Save(remoteConnection, jobToSendClone);

            Cursor.Current = Cursors.Default;

            MessageDialog.Show(Properties.Resources.JobSent, MessageBoxButtons.OK);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOK.Enabled = tbLogin.Text != "" && tbPassword.Text != "" && selectedLocation != null;
        }

        /// <summary>
        /// Instanciates <see cref="remoteConnection"/> and logs it into the LS One database that is provided with the selection location
        /// </summary>
        /// <param name="testConnection">If true then the connection is closed after logging in. Use this when you only want to test the connection</param>
        private bool ConnectRemoteConnection(bool testConnection)
        {

            bool connectionWorked = false;
            try
            {
                SqlConnectionStringBuilder connectionStringBuilder =
                    new SqlConnectionStringBuilder(selectedLocation.DBConnectionString.Substring(0, selectedLocation.DBConnectionString.IndexOf('|')));

                ConnectionType connectionType = ConnectionType.NamedPipes;

                switch (connectionStringBuilder.NetworkLibrary.ToUpperInvariant())
                {
                    case "DBMSLPCN":
                        connectionType = ConnectionType.SharedMemory;
                        break;

                    case "DBMSSOCN":                        
                        connectionType = ConnectionType.TCP_IP;
                        break;
                }
                
                remoteConnection = ConnectionManagerFactory.CreateConnectionManager();
                LoginResult result = remoteConnection.Login(
                    connectionStringBuilder.DataSource,
                    connectionStringBuilder.IntegratedSecurity,
                    connectionStringBuilder.UserID,
                    SecureStringHelper.FromString(connectionStringBuilder.Password),
                    connectionStringBuilder.InitialCatalog,
                    tbLogin.Text,
                    SecureStringHelper.FromString(tbPassword.Text),
                    connectionType,
                    ConnectionUsageType.UsageNormalClient,
                    PluginEntry.DataModel.Connection.DataAreaId);                

                if (result == LoginResult.Success)
                {
                    if (testConnection)
                    {
                        MessageDialog.Show(Properties.Resources.ConnectingToDatabaseWorked);                                                
                    }

                    connectionWorked = true;
                }
                else if (result == LoginResult.UserAuthenticationFailed)
                {
                    MessageDialog.Show(Properties.Resources.AuthenticationFailed);
                }
                else
                {
                    MessageDialog.Show(Properties.Resources.ConnectingToDatabaseFailed);
                }

                if (testConnection)
                {
                    remoteConnection.LogOff();
                }
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.ConnectingToDatabaseFailed);
            }

            return connectionWorked;
        }

        private void cmbLocation_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedDataID != RecordIdentifier.Empty)
            {
                selectedLocation = (JscLocation) cmbLocation.SelectedData;
            }

            CheckEnabled(this, EventArgs.Empty);
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ConnectRemoteConnection(true);
            Cursor.Current = Cursors.Default;
        }

        private void btnEditSourceLocation_Click(object sender, EventArgs e)
        {

        }
    }
}
