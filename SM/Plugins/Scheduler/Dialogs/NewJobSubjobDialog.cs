using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewJobSubjobDialog : DialogBase
    {
        private static bool beginsWith = false;

        private JscJob job;

        private List<DataEntity> selectedSubJobs;
        private List<DataEntity> subJobsToAdd;
        private List<DataEntity> subJobsToRemove;
        private List<JscSubJob> searchedSubJobs;

        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;
        
        public NewJobSubjobDialog()
        {
            InitializeComponent();

            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = Color.Maroon;

            greenStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = Color.DarkGreen };

            redStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = Color.Maroon };
        }

        public Guid? NewSubJobId { get; private set; }
        public JscLocation SourceLocation
        {
            get
            {
                return cmbSource.SelectedData as JscLocation;
            }
        }

        public bool AddedSubJob { get; set; }

        public DialogResult ShowDialog(IWin32Window owner, JscLocation sourceLocation, JscJob job)
        {
            this.job = job;
            NewSubJobId = null;

            cmbSource.SelectedData = sourceLocation;
            cmbSubjobs.SelectedData = new DataEntity("", "");
            selectedSubJobs = new List<DataEntity>();
            subJobsToRemove = new List<DataEntity>();
            subJobsToAdd = new List<DataEntity>();
            GetSelectedSubJobs(job);

            LoadPreviewLines();
            return base.ShowDialog(owner);
        }

        private void GetSelectedSubJobs(JscJob job)
        {

            var jobSubJobs = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobSubJobs(PluginEntry.DataModel, job);
            foreach (var subJob in jobSubJobs.Select(jobSubJob => jobSubJob.JscSubJob))
            {
                selectedSubJobs.Add(new DataEntity(subJob.ID, subJob.Description));
            }
        }

        private void cmbSource_DropDown(object sender, DropDownEventArgs e)
        {
            var location = ((DropDownFormComboBox)sender).SelectedData as JscLocation;
            Guid? selectedId = null;
            if (location != null)
            {
                selectedId = (Guid)location.ID;
            }
            e.ControlToEmbed = new LocationSelectPanel(DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations(PluginEntry.DataModel, true), selectedId);
        }

        private void cmbSource_FormatData(object sender, DropDownFormatDataArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (AddRemoveSubJobs())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private bool AddRemoveSubJobs()
        {
            int sequence = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetMaxJobSubjobSequence(PluginEntry.DataModel, this.job.ID) + 1;
            bool saved = false;
            foreach (DataEntity subJobEntity in subJobsToAdd)
            {
                var subJob = searchedSubJobs.FirstOrDefault(j => j.ID == subJobEntity.ID) ?? DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJob(PluginEntry.DataModel, subJobEntity.ID);

                JscJobSubjob jobSubjob = new JscJobSubjob();
                jobSubjob.JscJob = this.job;
                jobSubjob.JscSubJob = subJob;
                jobSubjob.Sequence = sequence++;
                jobSubjob.Enabled = true;
                DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, jobSubjob);
                saved = true;
            }

            if (subJobsToRemove.Count > 0)
            {
                List<JscJobSubjob> jobSubjobsInDb = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobSubJobs(PluginEntry.DataModel, job);
                List<JscJobSubjob> jobSubjobs = subJobsToRemove.Select(sjr => jobSubjobsInDb.FirstOrDefault(j => j.SubJob == sjr.ID)).ToList();
                DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, jobSubjobs);
                saved = true;
            }

            AddedSubJob = saved;
            return saved;
        }

        private void contextButtonAdd_Click(object sender, EventArgs e)
        {
            using (NewSubJobDialog dialog = new NewSubJobDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    NewSubJobId = (Guid)dialog.SubJob.ID;

                    int sequence = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetMaxJobSubjobSequence(PluginEntry.DataModel, job.ID) + 1;
                    JscJobSubjob jobSubjob = new JscJobSubjob();
                    jobSubjob.JscJob = job;
                    jobSubjob.JscSubJob = dialog.SubJob;
                    jobSubjob.Sequence = sequence++;
                    jobSubjob.Enabled = true;
                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Save(PluginEntry.DataModel, jobSubjob);

                    DialogResult = DialogResult.OK;
                }
            }
        }
        
        private void LoadPreviewLines()
        {
            lvlEditPreview.ClearRows();

            AddSelectedAndRemovedPreviewRows();

            AddNewPreviewRows();

            lvlEditPreview.AutoSizeColumns(true);
        }

        private void AddNewPreviewRows()
        {
            foreach (var line in subJobsToAdd)
            {
                var row = new Row();
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = Tuple.Create(RowTypeEnum.LineToAdd, line);

                var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                lvlEditPreview.AddRow(row);
            }
        }

        private void AddSelectedAndRemovedPreviewRows()
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            var selected = selectedSubJobs;
            var removed = subJobsToRemove;

            var selectedAndRemoved = new List<DataEntity>();

            foreach (DataEntity line in selected)
            {
                selectedAndRemoved.Add(line);
            }

            foreach (DataEntity line in removed)
            {
                selectedAndRemoved.Add(line);
            }

            selectedAndRemoved.Sort(CompareDataEntities);

            foreach (DataEntity line in selectedAndRemoved)
            {
                if (subJobsToRemove.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = Tuple.Create(RowTypeEnum.LineToRemove, line);

                    var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!subJobsToAdd.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddText(line.Text);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = Tuple.Create(selectedSubJobs.Exists(p => p.ID == line.ID) ? RowTypeEnum.SelectedLine : RowTypeEnum.LineToAdd, line);

                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
            }
        }

        private int CompareDataEntities(DataEntity dataEntity, DataEntity entity)
        {
            return dataEntity.Text.CompareTo(entity.Text);
        }

        private void cmbSubjobs_DropDown(object sender, DropDownEventArgs e)
        {
            var msp = new MultiSearchPanel(PluginEntry.DataModel, selectedSubJobs, 
                subJobsToAdd, subJobsToRemove, SearchTypeEnum.Custom, false) {BeginsWith = beginsWith};
            msp.SetSearchHandler(SubjobsOnSearch, e.DisplayText);
            e.DropDownFormBorderStyle = FormBorderStyle.SizableToolWindow;
            e.ControlToEmbed = msp;
        }

        private List<DataEntity> SubjobsOnSearch(object sender, MultiSearchArgs args)
        {
            var sourceLocation = cmbSource.SelectedData as JscLocation;
            Guid? databaseDesignId = null;
            if (sourceLocation != null)
            {
                var databaseDesign = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocationDatabaseDesign(PluginEntry.DataModel, sourceLocation);
                if (databaseDesign != null)
                {
                    databaseDesignId = (Guid)databaseDesign.ID;
                }
            }
            searchedSubJobs = DataProviderFactory.Instance.Get<IJobData, JscJob>().SearchSubJobsForJob(PluginEntry.DataModel, args.SearchText, args.StartRecord, args.EndRecord, args.BeginsWith, databaseDesignId, job.ID) as List<JscSubJob>;
            return searchedSubJobs != null ? searchedSubJobs.Select(j => new DataEntity(j.ID, j.Description)).ToList() : new List<DataEntity>();
        }

        private void cmbSubjobs_SelectedDataChanged(object sender, EventArgs e)
        {
            int prevAddedCount = subJobsToAdd.Count;

            selectedSubJobs = cmbSubjobs.SelectionList.Cast<DataEntity>().ToList();
            subJobsToAdd = cmbSubjobs.AddList.Cast<DataEntity>().ToList();
            subJobsToRemove = cmbSubjobs.RemoveList.Cast<DataEntity>().ToList();

            int curAddedCount = subJobsToAdd.Count;

            LoadPreviewLines();
            CheckEnabled();

            if (curAddedCount > prevAddedCount && !lvlEditPreview.RowIsOnScreen(lvlEditPreview.RowCount - 1))
            {
                lvlEditPreview.ScrollRowIntoView(lvlEditPreview.RowCount - 1);
            }
        }

        private void lvlEditPreview_CellAction(object sender, LSOne.Controls.EventArguments.CellEventArgs args)
        {
            var tuple = (Tuple<RowTypeEnum, DataEntity>)lvlEditPreview.Row(args.RowNumber).Tag;

            // Row type
            switch (tuple.Item1)
            {
                case RowTypeEnum.SelectedLine:
                    if (!subJobsToAdd.Exists(p => p.ID == tuple.Item2.ID))
                    {
                        selectedSubJobs.Remove((DataEntity)tuple.Item2);
                        subJobsToRemove.Add((DataEntity)tuple.Item2);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    subJobsToAdd.Remove((DataEntity)tuple.Item2);
                    selectedSubJobs.Remove((DataEntity)tuple.Item2);
                    break;
                case RowTypeEnum.LineToRemove:
                    // Undo a remove
                    subJobsToRemove.Remove((DataEntity)tuple.Item2);
                    selectedSubJobs.Add((DataEntity)tuple.Item2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = subJobsToAdd.Count > 0 || subJobsToRemove.Count > 0;
        }
    }
}
