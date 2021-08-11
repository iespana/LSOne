using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.Properties;
using LSOne.ViewPlugins.Scheduler.Utils;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class JobSubJobsPage : UserControl, ITabView
    {
        private JobViewPageContext internalContext;
        private NewJobSubjobDialog newJobSubjobDialog = new NewJobSubjobDialog();
        private List<DataEntityEx<JscJob>> jobSelections;
        private RecordIdentifier selectedID;
        private bool dataIsModified;
        List<JscJobSubjob> jobSubJobs;
        //private bool sequenceModified;

        public JobSubJobsPage()
        {
            InitializeComponent();

            lvJobSubJobs.ContextMenuStrip = new ContextMenuStrip();
            lvJobSubJobs.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new JobSubJobsPage();
        }


        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                this.internalContext = (JobViewPageContext)internalContext;
            }
            PopulateSubJobJobCombo();

            JobToForm();
            IdleOneShotProcessing.PostRun(ClearDataIsModified);

            UpdateActions();
        }

        private void ClearDataIsModified(object arg)
        {
            dataIsModified = false;
        }

        private void UpdateActions()
        {
            bool isOtherJob = internalContext.Job.ID != GetSelectedJobForSubJobs().ID;
            btnOpenJob.Enabled = isOtherJob;

            lvJobSubJobs.ContentBackColor = !isOtherJob ? SystemColors.Window : SystemColors.Control;
            lvJobSubJobs.OddRowColor = !isOtherJob ? SystemColors.Window : SystemColors.Control;

            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);
            int selectedCount = lvJobSubJobs.Selection.Count;

            selectedID = selectedCount == 1 ? ((JscJobSubjob)lvJobSubJobs.Selection[0].Tag).ID : null;

            btnMoveUp.Enabled = !isOtherJob && selectedCount > 0 && lvJobSubJobs.Selection.GetRowIndex(0) > 0;
            btnMoveDown.Enabled = !isOtherJob && selectedCount > 0 && lvJobSubJobs.Selection.GetRowIndex(selectedCount - 1) < lvJobSubJobs.RowCount - 1;

            contextButtons.EditButtonEnabled = selectedCount == 1;
            contextButtons.AddButtonEnabled = hasPermission && !isOtherJob;
            contextButtons.RemoveButtonEnabled = hasPermission && !isOtherJob && selectedCount >= 1;

            cmbSubJobJob.Enabled = hasPermission;
        }

        private void PopulateSubJobJobCombo()
        {
            cmbSubJobJob.Items.Clear();
            foreach (var selection in GetJobSelections())
            {
                cmbSubJobJob.Items.Add(selection);
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu = lvJobSubJobs.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    contextButtons_EditButtonClicked);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtons.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    contextButtons_AddButtonClicked);

            item.Enabled = contextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    contextButtons_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.MoveUp,
                    400,
                    btnMoveUp_Click);

            item.Image = ContextButtons.GetMoveUpButtonImage();
            item.Enabled = btnMoveUp.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.MoveDown,
                    500,
                    btnMoveDown_Click);

            item.Image = ContextButtons.GetMoveDownButtonImage();
            item.Enabled = btnMoveDown.Enabled;

            menu.Items.Add(item);

            e.Cancel = menu.Items.Count == 0;
        }

        public bool DataIsModified()
        {
            object selected = cmbSubJobJob.SelectedItem;
            if (selected != null)
            {
                RecordIdentifier currentJobSubjob = RecordIdentifier.IsEmptyOrNull(internalContext.Job.SubjobJob)
                    ? internalContext.Job.ID
                    : internalContext.Job.SubjobJob;

                return currentJobSubjob != ((DataEntityEx<JscJob>)cmbSubJobJob.SelectedItem).Value.ID || dataIsModified;
                
            }
            return dataIsModified;
        }

        public bool SaveData()
        {
             JscJob selected = GetSelectedJobForSubJobs();
            bool isOtherJob = internalContext.Job.ID != selected.ID;
            if (isOtherJob)
            {
                internalContext.Job.SubjobJob = selected.ID;
            }
            else
            {
                internalContext.Job.SubjobJob = null;
                foreach (var row in lvJobSubJobs.Rows)
                {
                    var checkCell = row[0] as CheckBoxCell;
                    bool isChecked = checkCell != null && checkCell.Checked;
                    var jobSubjob = selected.JscJobSubjobs.FirstOrDefault(x => x.ID == ((JscJobSubjob)row.Tag).ID);
                    if (jobSubjob != null && jobSubjob.Enabled != isChecked)
                    {
                        jobSubjob.Enabled = isChecked;
                    }
                }
            }
            dataIsModified = false;
            
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            // TODO: Implement
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "SubJob")
            {
                if (changeHint == DataEntityChangeType.Edit)
                {
                    // Check if this is one of ours
                    var row = lvJobSubJobs.Rows.FirstOrDefault(r => ((JscJobSubjob) r.Tag).SubJob == (Guid)changeIdentifier);
                    if (row != null)
                    {
                        LoadSubjobs();
                    }
                }
            }
            //else if (objectName == "JobSubjob")
            //{
            //    if (DataIsModified())
            //    {
            //        SaveData();
            //    }

            //    if (changeIdentifier != RecordIdentifier.Empty)
            //    {
            //        // Make sure we refresh the Subjob object
            //        var jobSubjob = this.internalContext.JobModel.GetJobSubJob((Guid)changeIdentifier);
            //        if (jobSubjob != null)
            //        {
            //            this.internalContext.JobModel.Refresh(jobSubjob.JscSubJob);
            //        }
            //    }
            //    JobToForm();
            //    UpdateActions();
            //}
        }

        private void JobToForm()
        {
            SetSubJobJob(internalContext.Job.SubjobJob);
            LoadSubjobs();
        }

        private void LoadSubjobs()
        {
            var job = GetSelectedJobForSubJobs();
            job = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, job.ID);

            bool isOtherJob = internalContext.Job.ID != job.ID;

            Cursor.Current = Cursors.WaitCursor;

            jobSubJobs = new List<JscJobSubjob>(job.JscJobSubjobs);
            jobSubJobs.Sort((x, y) => x.Sequence.CompareTo(y.Sequence));

            RecordIdentifier currentSelectedID = selectedID;
            lvJobSubJobs.ClearRows();
            selectedID = currentSelectedID;

            foreach (var jobSubJob in jobSubJobs)
            {
                var row = new Row();
                SetRow(row, jobSubJob, isOtherJob);
                lvJobSubJobs.AddRow(row);

                if(selectedID == jobSubJob.ID)
                {
                    lvJobSubJobs.Selection.Set(lvJobSubJobs.Rows.Count - 1);
                }
            }

            lvJobSubJobs.AutoSizeColumns();
            lvJobSubJobs.Sort();
        }

        private void SetRow(Row row, JscJobSubjob jobSubJob, bool isOtherJob)
        {
            row.Clear();
            var subJob = jobSubJob.JscSubJob;

            CheckBoxCell cell = new CheckBoxCell(jobSubJob.Enabled) {Enabled = !isOtherJob};
            row.AddCell(cell);
            row.AddText(subJob.Description);
            row.AddText(Utils.Utils.EnumResourceString(Resources.ResourceManager, subJob.ReplicationMethod));

            row.AddText(subJob.ObjectName);

            string databaseDesign = string.Empty;
            if (subJob.JscTableFrom != null)
            {
                databaseDesign = subJob.JscTableFrom.JscDatabaseDesign.Description;
            }

            row.AddText(databaseDesign);
            CheckBoxCell move = new CheckBoxCell(jobSubJob.JscSubJob.MoveActions) { Enabled = false };
            row.AddCell(move);
            
            row.Tag = jobSubJob;
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvJobSubJobs.Selection.Count != 1)
                return;

            var jobSubJob = lvJobSubJobs.Selection[0].Tag as JscJobSubjob;
            if (jobSubJob != null) 
                ShowSubJob((Guid)jobSubJob.JscSubJob.ID,(Guid)this.internalContext.Job.ID);
        }

        private void ShowSubJob(Guid subJobId, Guid? hintJobId)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SubJobView(subJobId, hintJobId, jobSubJobs.ConvertAll(x => new DataEntity(x.JscSubJob.ID, x.JscSubJob.Description))));
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            if (newJobSubjobDialog.ShowDialog(PluginEntry.Framework.MainWindow, internalContext.CurrentSourceLocation, internalContext.Job) == DialogResult.OK)
            {
                if (newJobSubjobDialog.NewSubJobId != null)
                {
                    ShowSubJob(newJobSubjobDialog.NewSubJobId.Value, (Guid) internalContext.Job.ID);
                }

                // One or more JobSubjobs have been added, refresh all
                if (newJobSubjobDialog.AddedSubJob)
                {

                    internalContext.Job = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, internalContext.Job.ID);
                    ((DataEntityEx<JscJob>) cmbSubJobJob.SelectedItem).Value.JscJobSubjobs =
                        internalContext.Job.JscJobSubjobs;
                    JobToForm();
                    UpdateActions();
                    dataIsModified = true;
                    Guid? sourceLocationId = null;
                    if (newJobSubjobDialog.SourceLocation != null)
                    {
                        sourceLocationId = (Guid) newJobSubjobDialog.SourceLocation.ID;
                    }

                    if (internalContext.Job.Source != sourceLocationId)
                    {
                        internalContext.Job.JscSourceLocation = newJobSubjobDialog.SourceLocation;
                        // Notify main view of change in source
                        PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
                                                                               "Job.Source", internalContext.Job.ID,
                                                                               null);
                    }
                    dataIsModified = true;
                }
            }
        }

        private List<DataEntityEx<JscJob>> GetJobSelections()
        {
            if (jobSelections == null)
            {
                jobSelections = new List<DataEntityEx<JscJob>>();

                // First the current job
                var noneEntity = new DataEntityEx<JscJob>(RecordIdentifier.Empty, Resources.JobSubJobPageCurrentJob, internalContext.Job);
                jobSelections.Add(noneEntity);

                // Then all other jobs
                foreach (var job in DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobsSimple(PluginEntry.DataModel, false))
                {
                    if (job.ID != internalContext.Job.ID)
                    {

                        string text = job.Text;
                        jobSelections.Add(new DataEntityEx<JscJob>(job.ID, text, job));
                    }
                }
            }

            return jobSelections;
        }

        private void SetSubJobJob(RecordIdentifier jobId)
        {
            DataEntityEx<JscJob> selection = null;

            var jobs = GetJobSelections();
            if (jobId.IsEmpty || jobId == internalContext.Job.ID)
            {
                selection = jobs[0];
            }
            else
            {
                for (int i = 1; i < jobs.Count; i++)
                {
                    if ((Guid)jobs[i].ID == jobId)
                    {
                        selection = jobs[i];
                        break;
                    }
                }
            }

            cmbSubJobJob.SelectedItem = selection;
        }

        private void btnOpenJob_Click(object sender, EventArgs e)
        {
            ShowJob(GetSelectedJobForSubJobs());
        }

        private void ShowJob(JscJob job)
        {
            PluginEntry.Framework.ViewController.Add(new Views.JobView((Guid)job.ID));
        }

        private JscJob GetSelectedJobForSubJobs()
        {
            JscJob result = null;
            object selected = cmbSubJobJob.SelectedItem;
            if (selected != null)
            {
                result = ((DataEntityEx<JscJob>)cmbSubJobJob.SelectedItem).Value;
            }

            return result;
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            
            //bool doRemove = false;

            var jobSubjobs = new List<JscJobSubjob>();
            for (int i = 0; i < lvJobSubJobs.Selection.Count; i++)
            {
                jobSubjobs.Add((JscJobSubjob)lvJobSubJobs.Selection[i].Tag);
            }
            /*foreach (ListViewItem item in lvJobSubJobs.SelectedItems)
            {
                jobSubjobs.Add((JscJobSubjob)item.Tag);
            }*/
            string msg = null;
            if (lvJobSubJobs.Selection.Count == 1)
            {
                msg = string.Format(Properties.Resources.SubjobRemoveMsg, jobSubjobs[0].JscSubJob.Description);
            }
            else if (lvJobSubJobs.Selection.Count > 1)
            {
                msg = string.Format(Properties.Resources.SubjobRemoveManyMsg, jobSubjobs.Count);
            }
            /*if (jobSubjobs.Count == 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.SubjobRemoveMsg, jobSubjobs[0].JscSubJob.Description),
                        Properties.Resources.SubjobRemoveHeader
                    ) == DialogResult.Yes;
            }
            else if (jobSubjobs.Count > 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.SubjobRemoveManyMsg, jobSubjobs.Count),
                        Properties.Resources.SubjobRemoveHeader
                    ) == DialogResult.Yes;
            }*/

            if (msg != null)
            {
                if (QuestionDialog.Show(msg, Properties.Resources.SubjobRemoveHeader) == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    
                    JscJob selected = GetSelectedJobForSubJobs();
                    if (selected != null)
                    {
                        foreach (var jscJobSubjob in jobSubjobs)
                        {
                            var subjob = selected.JscJobSubjobs.FirstOrDefault(x => x.ID == jscJobSubjob.ID);
                            if (subjob != null)
                            {
                                selected.JscJobSubjobs.Remove(subjob);
                            }
                        }
                    }
                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, jobSubjobs);
                    LoadSubjobs();
                    dataIsModified = true;

                }
            }

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lvJobSubJobs.Selection.Count == 0)
                return;

            Debug.Assert(lvJobSubJobs.Selection.FirstSelectedRow > 0);

            for (int i = 0; i < lvJobSubJobs.Selection.Count; i++)
            {
                int selectedRowIndex = lvJobSubJobs.Selection.GetRowIndex(i);
                lvJobSubJobs.SwapRows(selectedRowIndex - 1, selectedRowIndex);
            }

            RenumberJobSubjobs();
            UpdateActions();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lvJobSubJobs.Selection.Count == 0)
                return;

            Debug.Assert(lvJobSubJobs.Rows.IndexOf(lvJobSubJobs.Selection[lvJobSubJobs.Selection.Count - 1]) < lvJobSubJobs.RowCount - 1);

            for (int i = lvJobSubJobs.Selection.Count - 1; i >= 0 ; i--)
            {
                int selectedRowIndex = lvJobSubJobs.Selection.GetRowIndex(i);
                lvJobSubJobs.SwapRows(selectedRowIndex, selectedRowIndex + 1);
            }

            RenumberJobSubjobs();
            UpdateActions();
        }

        private void RenumberJobSubjobs()
        {
            int sequence = 1;
            List<JscJobSubjob> newSubJobList = new List<JscJobSubjob>(lvJobSubJobs.Rows.Count);
            foreach (var jobSubjob in lvJobSubJobs.Rows.Where(r => r.Tag is JscJobSubjob).Select(r => r.Tag as JscJobSubjob))
            {
                jobSubjob.Sequence = sequence;
                newSubJobList.Add(jobSubjob);
                sequence++;
            }
            internalContext.Job.JscJobSubjobs = newSubJobList;
            //sequenceModified = true;
            dataIsModified = true;
        }

        private void cmbSubJobJob_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSubjobs();
            UpdateActions();
        }

        private void lvJobSubJobs_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtons.EditButtonEnabled)
            {
                contextButtons_EditButtonClicked(contextButtons, EventArgs.Empty);
            }
        }

        private void lvJobSubJobs_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvJobSubJobs_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            if (args.Cell is CheckBoxCell)
            {
                dataIsModified = true;
            }
        }
    }
}
