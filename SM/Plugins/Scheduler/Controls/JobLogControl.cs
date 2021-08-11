using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.Properties;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class JobLogControl : UserControl
    {
        private Guid? jobId;
        private List<JscSchedulerLog> latestDataList;

        private List<JscLocation> locations;

        public JobLogControl()
        {
            InitializeComponent();
            locations = new List<JscLocation>();
            lvSubLogs.AutoSizeColumns();
            lvSubLogs.Invalidate();

            lvJobLog.ContextMenuStrip = new ContextMenuStrip();
            lvJobLog.ContextMenuStrip.Opening += ContextMenuStripOpening;

            DateTime today = DateTime.Today;
            dtpFromDate.Value = today.AddDays(-1);
            dtpToDate.Value = today;
        }

        public void Prepare(Guid? jobId)
        {
            this.jobId = jobId;
            short messageIndex = 2;
            if (jobId.HasValue)
            {
                lvJobLog.Columns.Remove(colJob);
                messageIndex = 1;
            }
            lvJobLog.SecondarySortColumn = messageIndex;

            Search();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        
        public void Search()
        {
            latestDataList = new List<JscSchedulerLog>(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobLog(PluginEntry.DataModel, dtpFromDate.Value, dtpToDate.Value, this.jobId));
            LoadLines();
        }
        private void ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
          

            ExtendedMenuItem item;
            ContextMenuStrip menu = (ContextMenuStrip)sender;

            menu.Items.Clear();
            
            item = new ExtendedMenuItem(Resources.ReExecute, 500, btnReExecute_Click);
            item.Enabled = btnReExecute.Enabled;
            menu.Items.Add(item);
            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadLines()
        {
            btnReExecute.Enabled = false;
            if (latestDataList == null)
                return;

            Cursor.Current = Cursors.WaitCursor;
            lvJobLog.ClearRows();
            foreach (var entry in latestDataList)
            {
                Row row = new Row();
                SetRow(row, entry);
                lvJobLog.AddRow(row);
            }
            btnReExecute.Enabled = lvSubLogs.Rows.Count > 1;
            lvJobLog.AutoSizeColumns();
        }

        private void LoadSubLines(IEnumerable<JscSchedulerSubLog> subLogEntries )
        {
           
            
            Cursor.Current = Cursors.WaitCursor;
            lvSubLogs.ClearRows();
            foreach (var entry in subLogEntries)
            {
                Row row = new Row();
                SetSubRow(row, entry);
                lvSubLogs.AddRow(row);
            }
            btnReExecute.Enabled =  lvSubLogs.Rows.Count > 0;
            lvSubLogs.AutoSizeColumns();
            lvSubLogs.Invalidate();
        }

        private void SetRow(Row row, JscSchedulerLog entry)
        {
            if (entry.JscJob == null && entry.Job != null && !entry.Job.IsEmpty)
            {
                entry.JscJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, entry.Job);
            }
            row.Clear();
            row.AddText(entry.RegTime.ToString("G"));
            if (!jobId.HasValue)
            {
                row.AddText(entry.JscJob.Text);
            }
            row.Tag = entry;
            row.AddText(entry.Message);
        }

        private void SetSubRow(Row row, JscSchedulerSubLog entry)
        {
            if (entry.JscSubJob == null && entry.SubJob != null && !entry.SubJob.IsEmpty)
            {
                entry.JscSubJob = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJob(PluginEntry.DataModel, entry.SubJob);
            }
            if (entry.JscLocation == null && entry.Location != null && !entry.Location.IsEmpty)
            {
                entry.JscLocation = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(PluginEntry.DataModel, entry.Location);
            }
            row.Clear();
            row.AddText(entry.JscSubJob == null ? entry.SubJob.ToString() : entry.JscSubJob.Description);
            row.AddText(entry.StartTime.ToString("G"));
            row.AddText(entry.EndTime.ToString("G"));
            row.AddText(entry.ReplicationCounterStart.ToString());
            row.AddText(entry.ReplicationCounterEnd.ToString());
            row.AddText(entry.RunAsNormal?"true": "false");
            row.AddText(entry.JscLocation == null ?entry.Location.ToString():entry.JscLocation.Text);
            row.Tag = entry;
        }

        private void lvJobLog_HeaderClicked(object sender, LSOne.Controls.EventArguments.ColumnEventArgs args)
        {
            if (args.Column == colTime)
            {
                lvJobLog.SecondarySortColumn = (short)(lvJobLog.Columns.Count - 1);
            }
            else if (args.Column == colJob)
            {
                lvJobLog.SecondarySortColumn = 0;
            }
            lvJobLog.Sort();
        }

        private void lvJobLog_SelectionChanged(object sender, EventArgs e)
        {
            RecordIdentifier logID = (lvJobLog.Selection.Count > 0) ? ((JscSchedulerLog)lvJobLog.Row(lvJobLog.Selection.FirstSelectedRow).Tag).ID : RecordIdentifier.Empty;
            btnReExecute.Enabled = false;

            if (logID != null && logID != RecordIdentifier.Empty)
            {
                btnReExecute.Enabled = true;
                var schedulerSubLogs = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetSubJobLog(PluginEntry.DataModel, logID);
                LoadSubLines(schedulerSubLogs);
               
                    foreach (var row in lvSubLogs.Rows)
                    {
                        JscSchedulerSubLog currentLog = (JscSchedulerSubLog) row.Tag;
                        if (!locations.Any(x => x.ID == currentLog.Location))
                        {

                            locations.Add(
                                DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                                    .GetLocation(PluginEntry.DataModel, currentLog.Location));
                        }
                    }

                    chkSpecificLocation.Enabled = locations.Count > 1;

                }
            
        }

        private void btnReExecute_Click(object sender, EventArgs e)
        {
            JscSchedulerLog logID = (lvJobLog.Selection.Count > 0) ? ((JscSchedulerLog)lvJobLog.Row(lvJobLog.Selection.FirstSelectedRow).Tag) : null;

            if (logID == null)
            {
                MessageDialog.Show(Properties.Resources.NoJobSelectedNothingIsRun, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            JscLocation location = null;
            if (chkSpecificLocation.Checked                )
            {
                LocationSelectorDialog dialog = new LocationSelectorDialog(locations);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    location = dialog.LocationItem;
                }
                else
                {
                    return;
                }
            }
                PluginOperations.RunJob(logID.JscJob,false,logID, location);
            
        }
    }
}
