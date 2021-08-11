using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewJobDialog : DialogBase
    {
        private DataEntity emptyItem;

        public NewJobDialog()
        {
            InitializeComponent();

            emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.JobDoNotCopyExisting);
        }

        public JscJob Job { get; private set; }
        public Guid JobId { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
          
            PopulateJobTypes();
            if (cmbJobType.Items.Count > 0)
            {
                cmbJobType.SelectedIndex = 0;
            }
            cmbCopyFrom.SelectedData = emptyItem;

            Job = null;
            JobId = Guid.Empty;

            UpdateActions();
        }

        private void NewJobDialog_Shown(object sender, EventArgs e)
        {
            tbDescription.Focus();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = tbDescription.TextLength > 0;
        }

        private void PopulateJobTypes()
        {
            cmbJobType.Items.Clear();
            ComboUtils.PopulateComboBoxItems<JobType>(cmbJobType,
                                                                                Properties
                                                                                        .Resources.ResourceManager,
                                                                                jobType =>
                                                                                jobType ==
                                                                                JobType
                                                                                        .Replication);
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = new List<DataEntity>();
            foreach (var dataSelector in DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobSelectorList(PluginEntry.DataModel))
            {
                items.Add(new DataEntity{ID = dataSelector.GuidId, Text = dataSelector.Text});
            }

            items.Insert(0, emptyItem);

            cmbCopyFrom.SetData(items, null, true);
        }


        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            // Create the new job
            JscJob job;

            IDataEntity copyFromEntity = cmbCopyFrom.SelectedData;
            if (copyFromEntity.ID.IsEmpty)
            {
                job = new JscJob();
                FormToJob(job);

                // Set default values
                job.ErrorHandling = ErrorHandling.SkipToNext;
                job.Enabled = true;
            }
            else
            {
                job = DataProviderFactory.Instance.Get<IJobData, JscJob>()
                    .GetJob(PluginEntry.DataModel, copyFromEntity.ID);

                FormToJob(job);

                job.ID = RecordIdentifier.Empty;

                foreach (var jscJobSubjob in job.JscJobSubjobs)
                {
                    jscJobSubjob.ID = RecordIdentifier.Empty;
                }
                foreach (var jscJobTrigger in job.JscJobTriggers)
                {
                    jscJobTrigger.ID = RecordIdentifier.Empty;
                }
              
            }

            if (!ValidateJob())
            {
                return;
            }

            // Save the new job
            DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, job);
            this.Job = job;
            this.JobId = (Guid)job.ID;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void FormToJob(JscJob job)
        {
            job.Text = tbDescription.Text;
            job.JobType = (JobType)ComboUtils.GetComboSelectionInt(cmbJobType);
        }

        private bool ValidateJob()
        {
            // Nothing yet
            return true;
        }
    }
}
