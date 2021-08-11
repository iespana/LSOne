using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSOne.ViewPlugins.Scheduler.ViewPages;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class JobView : ViewBase
    {
        private Guid jobId;
        private JscJob job;
        private JobViewPageContext pageContext = new JobViewPageContext();

        private TabControl.Tab externalPlugin;
        private TabControl.Tab subJobPage;
        private TabControl.Tab triggerPage;
        private TabControl.Tab logPage;

        private ContextBarItem runJobItem;
        private ContextBarItem runJobAsNormalItem;

        IEnumerable<IDataEntity> recordBrowsingContext;

        public JobView(Guid jobId)
            : this()
        {
            this.jobId = jobId;
        }

        public JobView(Guid jobId, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
        {

            this.recordBrowsingContext = recordBrowsingContext;
            this.jobId = jobId;
        }

        private JobView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.RecordCursor;

            // NOTE: cmbErrorHandling is currently set as Visible = false
            
            ComboUtils.PopulateComboBoxItems<ErrorHandling>(cmbErrorHandling, Properties.Resources.ResourceManager);
        }
        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                var id = recordBrowsingContext.ElementAt(cursorIndex).ID;

                if (DataProviderFactory.Instance.Get<IJobData, JscJob>().Exists(PluginEntry.DataModel, id))
                {
                    return new JobView((Guid)recordBrowsingContext.ElementAt(cursorIndex).ID, recordBrowsingContext);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == jobId)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Job;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                if (job == null)
                {
                    return RecordIdentifier.Empty;
                }
                return job.ID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            job = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, jobId);
            if (!isRevert)
            {

                HeaderText = Properties.Resources.Job;
                if (job.JobType == JobType.ExternalPlugin)
                {
                    externalPlugin = new TabControl.Tab("Parameters", ExternalPlugin.CreateInstance);
                    tabControl.AddTab(externalPlugin);
                }
                else
                {
                    subJobPage = new TabControl.Tab(Properties.Resources.Subjobs, JobSubJobsPage.CreateInstance);
                    tabControl.AddTab(subJobPage);
                }

                cbExternalCommand.Checked = job.JobType == JobType.ExternalPlugin; 
                triggerPage = new TabControl.Tab(Properties.Resources.Triggers, JobTriggerPage.CreateInstance);
                tabControl.AddTab(triggerPage);
                logPage = new TabControl.Tab(Properties.Resources.Log, ViewPages.JobLogPage.CreateInstance);
                tabControl.AddTab(logPage);
            }

            cbUseCurrentLocation.Checked = job.UseCurrentLocation;
            tbJobTypeCode.Text = job.JobTypeCode;

               
           
            // Revert logic: Objects are updated in SaveData so no special measures needed.
            ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);

            JobToForm();

            pageContext.Job = job;
            pageContext.CurrentSourceLocation = (JscLocation)cmbSource.SelectedData;

            tabControl.SetData(isRevert, null, pageContext);

            UpdateActions();
        }

        private void JobToForm()
        {
            cmbSource.SelectedData = job.JscSourceLocation;
            cmbDestination.SelectedData = job.JscDestinationLocation;
            tbDescription.Text = job.Text;
            ComboUtils.SetComboSelection(cmbErrorHandling, (int) job.ErrorHandling);
            chkEnabled.Checked = job.Enabled;
            cmbCompressionMode.SelectedIndex = (int) job.CompressionMode;
            cmbIsolationLevel.SelectedIndex = (int) job.IsolationLevel;
            tbJobTypeCode.Text = job.JobTypeCode;
        }

        private bool JobFromForm()
        {
            job.JscSourceLocation = (JscLocation)cmbSource.SelectedData;
            job.JscDestinationLocation = (JscLocation)cmbDestination.SelectedData;
            if (tbDescription.Text.Trim().Length > 0)
            {
                job.Text = tbDescription.Text.Trim();
            }
            else
            {
                job.Text = null;
            }
            job.ErrorHandling = (ErrorHandling)ComboUtils.GetComboSelectionInt(cmbErrorHandling);

            // Check speficically for changed to Enabled, since we need to reload the triggers if we are enabling/disabling the job
            if(job.Enabled != chkEnabled.Checked)
            {
                pageContext.JobTriggersChanged = true;
            }

            job.Enabled = chkEnabled.Checked;
            job.UseCurrentLocation = cbUseCurrentLocation.Checked;
            job.JobTypeCode = tbJobTypeCode.Text;
            job.CompressionMode = (DDCompressionMode) cmbCompressionMode.SelectedIndex;
            job.IsolationLevel = (DDIsolationLevel) cmbIsolationLevel.SelectedIndex;

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            base.OnDataChanged(changeAction, objectName, changeIdentifier, param);
            tabControl.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);

            if (changeAction == DataEntityChangeType.Edit && objectName == "Job.Source" && changeIdentifier == job.ID)
            {
                // External change of job.Source property, refresh it
                cmbSource.SelectedData = job.JscSourceLocation;
            }
        }

        private void LocationSelectDropDown(object sender, DropDownEventArgs e)
        {
            var location = ((DropDownFormComboBox)sender).SelectedData as JscLocation;
            Guid? selectedId = null;
            if (location != null)
            {
                selectedId = (Guid) location.ID;
            }
            e.ControlToEmbed = new LocationSelectPanel(DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations (PluginEntry.DataModel, true), selectedId);
        }

        private void LocationSelectFormatData(object sender, DropDownFormatDataArgs e)
        {
            if (e.Data == null)
            {
                e.TextToDisplay = string.Empty;
            }
            else
            {
                e.TextToDisplay = ((JscLocation)e.Data).Text;
            }
        }

        protected override bool DataIsModified()
        {
            if (tabControl.IsModified())
            {
                return true;
            }

            if (GetComboLocationId(cmbSource) != job.Source)
            {
                return true;
            }

            if (GetComboLocationId(cmbDestination) != job.Destination)
            {
                return true;
            }

            if (tbDescription.Text != job.Text)
            {
                return true;
            }

            if (ComboUtils.GetComboSelectionInt(cmbErrorHandling) != (int) job.ErrorHandling)
            {
                return true;
            }

            if (chkEnabled.Checked != job.Enabled)
            {
                return true;
            }

            if (cbExternalCommand.Checked != (job.JobType == JobType.ExternalPlugin))
            {
                return true;
            }

            if (cbUseCurrentLocation.Checked != job.UseCurrentLocation)
            {
                return true;
            }

            if (cmbCompressionMode.SelectedIndex != (int) job.CompressionMode)
            {
                return true;
            }

            if (cmbIsolationLevel.SelectedIndex != (int) job.IsolationLevel)
            {
                return true;
            }

            if (tbJobTypeCode.Text != job.JobTypeCode)
            {
                return true;
            }

            return false;
        }

        private RecordIdentifier GetComboLocationId(DropDownFormComboBox cmb)
        {
            JscLocation location = (JscLocation)cmb.SelectedData;
            RecordIdentifier result;
            if (location != null)
            {
                result = location.ID;
            }
            else
            {
                result = RecordIdentifier.Empty;
            }

            return result;
        }

        protected override bool SaveData()
        {
            
            if (!base.SaveData())
                return false;

            if (!JobFromForm())
                return false;

            if (tabControl.IsModified())
            {
                tabControl.GetData();
            }

            JobFromForm();
            if (externalPlugin != null)
            {
                job.PluginPath = pageContext.Job.PluginPath;
                job.PluginArguments = pageContext.Job.PluginArguments;
            }

            job.JobType = cbExternalCommand.Checked ? JobType.ExternalPlugin : JobType.Replication;

            DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel,job );

            if(pageContext.JobTriggersChanged)
            {
                PluginOperations.NotifyTriggerChange((Guid)job.ID);
                pageContext.JobTriggersChanged = false;
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Job", job.ID, null);

            return true;
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Properties.Resources.Actions, this.GetType().ToString() + ".Actions"), 5);

            base.OnSetupContextBarHeaders(arguments);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobRun))
                {
                    runJobItem = new ContextBarItem(Properties.Resources.JobRun, RunJobHandler);
                    arguments.Add(runJobItem, 300);
                }
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobRun))
                {
                    runJobAsNormalItem = new ContextBarItem(Properties.Resources.JobRunAsNormal, RunJobHandlerAsNormal);
                    arguments.Add(runJobAsNormalItem, 310);
                }
            }

            if (arguments.CategoryKey == GetType() + ".Actions")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.DDBusinessObjects.SchedulerPermissions.JobRun))
                {
                    ContextBarItem sendJobItem = new ContextBarItem(Properties.Resources.SendJob, SendJobHandler);
                    arguments.Add(sendJobItem, 100);
                }
            }

            base.OnSetupContextBarItems(arguments);
        }
       
        private void SendJobHandler(object sender, ContextBarClickEventArguments args)
        {
            using (SendJobDialog dlg = new SendJobDialog(job))
            {
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }
        }

        private void RunJobHandler(object sender, ContextBarClickEventArguments args)
        {
            if (DataIsModified())
            {
                if (!SaveData())
                {
                    // Save failed for some reason, a message should already have been shown
                    return;
                }
            }

            PluginOperations.RunJob( job);
        }

        private void RunJobHandlerAsNormal(object sender, ContextBarClickEventArguments args)
        {
            if (DataIsModified())
            {
                if (!SaveData())
                {
                    // Save failed for some reason, a message should already have been shown
                    return;
                }
            }

            PluginOperations.RunJob(job, true);
        }

        private void tbDescription_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = string.Empty;

            e.Cancel = tbDescription.TextLength == 0;
            if (e.Cancel)
            {
                errorMsg = Properties.Resources.FieldCannotBeEmptyMsg;
            }
            errorProvider.SetError(tbDescription, errorMsg);
        }

        private void cmbSource_SelectedDataChanged(object sender, EventArgs e)
        {
            pageContext.CurrentSourceLocation = (JscLocation)cmbSource.SelectedData;
            UpdateActions();
        }

        private void cmbSource_RequestClear(object sender, EventArgs e)
        {
            cmbSource.SelectedData = null;
        }

        private void cmbDestination_RequestClear(object sender, EventArgs e)
        {
            cmbDestination.SelectedData = null;
        }

        private void cmbDestination_SelectedDataChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnEditSourceLocation.Enabled = cmbSource.SelectedData != null;
            btnEditDestLocation.Enabled = cmbDestination.SelectedData != null;
        }

        private void btnEditSourceLocation_Click(object sender, EventArgs e)
        {
            var location = cmbSource.SelectedData as JscLocation;
            if (location != null)
            {
                PluginOperations.ShowLocationView((Guid)location.ID);
            }
        }

        private void btnEditDestLocation_Click(object sender, EventArgs e)
        {
            var location = cmbDestination.SelectedData as JscLocation;
            if (location != null)
            {
                PluginOperations.ShowLocationView((Guid)location.ID);
            }
        }

        private void cbExternalCommand_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExternalCommand.Checked)
            {
                if (externalPlugin == null)
                {
                    externalPlugin = new TabControl.Tab("Parameters", ExternalPlugin.CreateInstance);
                    tabControl.InsertTab(externalPlugin, 0);
                }

                if (subJobPage != null)
                {
                    subJobPage.Visible = false;
                }

                tabControl.SelectedTab = externalPlugin;
                externalPlugin.Visible = true;

                cmbSource.Text = string.Empty;
                cmbSource.Enabled = false;
                cmbDestination.Text = string.Empty;
                cmbDestination.Enabled = false;
                btnEditSourceLocation.Enabled = false;
                btnEditDestLocation.Enabled = false;
            }
            else
            {
                if (subJobPage == null)
                {
                    subJobPage = new TabControl.Tab(Properties.Resources.Subjobs, JobSubJobsPage.CreateInstance);
                    tabControl.InsertTab(subJobPage, 0);
                }
                if (externalPlugin != null)
                {
                    externalPlugin.Visible = false;
                }

                tabControl.SelectedTab = subJobPage;
                subJobPage.Visible = Visible = true;

                cmbSource.Enabled = true;
                cmbDestination.Enabled = true;
                btnEditSourceLocation.Enabled = true;
                btnEditDestLocation.Enabled = true;
            }
            tabControl.Invalidate();
            tabControl.LoadAllTabs();
        }

        private void cbUseCurrentLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseCurrentLocation.Checked)
            {
                cmbSource.Text = string.Empty;
                cmbSource.Enabled = false;
                btnEditSourceLocation.Enabled = false;
                var parameters =  Providers.ParameterData.Get(PluginEntry.DataModel);
                if (parameters.CurrentLocation == RecordIdentifier.Empty)
                {
                    //TODO load configuration page
                }
            }
            else
            {
                cmbSource.Enabled = true;
                btnEditSourceLocation.Enabled = true;
            }
        }
    }
}
