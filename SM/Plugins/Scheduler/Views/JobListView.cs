using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Scheduler.Controls.Recurrence;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class JobListView : ViewBase
    {
        private Dialogs.NewJobDialog newJobDialog = new Dialogs.NewJobDialog();
        private List<JscJob> latestDataList;
        private int lastSelectedRow;

        public JobListView()
        {
            InitializeComponent();
            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            lvJobs.ContextMenuStrip = new ContextMenuStrip();
            lvJobs.ContextMenuStrip.Opening += lvJobs_Opening;
            lvJobs.SetSortColumn(colDescription, true);
            lastSelectedRow = -1;            
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.Jobs;
            
            RefreshData();
            UpdateActions();
        }

        private void RefreshData()
        {
            Cursor.Current = Cursors.WaitCursor;
            latestDataList = new List<JscJob>(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(PluginEntry.DataModel, chkShowDisabled.Checked));
            RefreshJobs();
        }

        private void RefreshJobs()
        {
            if (latestDataList == null)
                return;

            Cursor.Current = Cursors.WaitCursor;
            
            lvJobs.ClearRows();
            foreach (var job in latestDataList)
            {
                var row = GetRowFromJob(job);
                lvJobs.AddRow(row);
            }

            lvJobs.Sort();
            lvJobs.AutoSizeColumns(false);

            if (lastSelectedRow >= 0)
            {
                lvJobs.Selection.Set(lastSelectedRow);
                lvJobs.ScrollRowIntoView(lastSelectedRow);
            }
        }

        private Row GetRowFromJob(JscJob job)
        {
            var row = new Row();
            row.AddCell(new ExtendedCell("", job.Enabled ? null : Properties.Resources.Disabled12));
            row.AddText(job.Text);
            if (job.UseCurrentLocation)
            {
                row.AddText(Properties.Resources.ResourceManager.GetString("CurrentLocationDisplayText"));
            }
            else
            {
                row.AddText(job.JscSourceLocation != null ? job.JscSourceLocation.Text : string.Empty);
            }
            row.AddText(job.JscDestinationLocation != null ? job.JscDestinationLocation.Text : string.Empty);

            var triggerText = Properties.Resources.NoTrigger;
            var triggers = job.JscJobTriggers;
            if (triggers != null && triggers.Count > 0)
            {
                // Only use the first trigger for now
                var trigger = triggers[0];
                if (trigger.Enabled)
                {
                    var cron = CronSchedule.CreateFromTrigger(trigger);
                    switch (trigger.RecurrenceType)
                    {
                        case RecurrenceType.None:
                            var date = AsDate(
                                ToInt(trigger.Years),
                                ToInt(trigger.Months),
                                ToInt(trigger.DaysOfMonth),
                                ToInt(trigger.Hours),
                                ToInt(trigger.Minutes),
                                ToInt(trigger.Seconds));
                            triggerText = Properties.Resources.Once.Replace("%1",
                                date.ToShortDateString() + " " + date.ToLongTimeString());
                            break;
                        case RecurrenceType.Hourly:
                            var hourly = HourlySetting.FromCronSchedule(cron);
                            if (hourly.HoursFrom != hourly.HoursTo)
                            {
                                if (hourly.EveryHour)
                                    triggerText = Properties.Resources.HourlyHours.Replace("%1", hourly.Hours.ToString()) + string.Format(" From {0}:00  to {1}:00", hourly.HoursFrom, hourly.HoursTo);
                                else
                                    triggerText = Properties.Resources.HourlyMinutes.Replace("%1",
                                        hourly.Minutes.ToString()) + string.Format(" From {0}:00  to {1}:00", hourly.HoursFrom, hourly.HoursTo); 
                            }
                            else
                            {
                                if (hourly.EveryHour)
                                    triggerText = Properties.Resources.HourlyHours.Replace("%1", hourly.Hours.ToString());
                                else
                                    triggerText = Properties.Resources.HourlyMinutes.Replace("%1",
                                        hourly.Minutes.ToString());
                            }
                            break;
                        case RecurrenceType.Daily:
                            var daily = DailySetting.FromCronSchedule(cron);
                            if (daily.EveryDay)
                                triggerText = Properties.Resources.DailyDays.Replace("%1", daily.Days.ToString());
                            else
                                triggerText = Properties.Resources.DailyWeekdays;
                            break;
                        case RecurrenceType.Weekly:
                            string days = "";
                            var weekly = WeeklySetting.FromCronSchedule(cron);
                            foreach (var day in weekly.DaysOfWeek)
                            {
                                days += " ";
                                days += Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortestDayNames[(int) day];
                            }
                            triggerText = Properties.Resources.Weekly + days;
                            break;
                        case RecurrenceType.Monthly:
                            var monthly = MonthlySetting.FromCronSchedule(cron);
                            if (monthly.UseDayOfMonth)
                                triggerText = Properties.Resources.MonthlyWeekdayOfMonth
                                    .Replace("%1", monthly.DayOfMonth.ToString())
                                    .Replace("%2", monthly.Month.ToString());
                            else
                                triggerText = Properties.Resources.MonthlyWeekdayOfMonth
                                    .Replace("%1", monthly.WeekdaySequence.ToString())
                                    .Replace("%2", monthly.WeekdayCharm.ToString())
                                    .Replace("%3", monthly.Month.ToString());
                            break;
                        case RecurrenceType.Yearly:
                            var yearly = YearlySetting.FromCronSchedule(cron);
                            if (yearly.UseDayInMonth)
                                triggerText = string.Format("{0} {1} {2}",
                                    Properties.Resources.Yearly,
                                    yearly.Day,
                                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[yearly.Month]);
                            else
                                triggerText = string.Format("{0} {1} {2} {3}",
                                    Properties.Resources.Yearly,
                                    yearly.WeekdaySequence.ToString(),
                                    yearly.WeekdayCharm.ToString(),
                                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[yearly.Month]);
                            break;
                    }

                    bool active = true;
                    if (trigger.StartTime != null)
                        active = trigger.StartTime.Value < DateTime.Now;
                    if (active && trigger.EndTime != null)
                        active &= trigger.EndTime.Value > DateTime.Now;

                    triggerText = string.Format("{0}{1}",
                        (active ? "" : string.Format("[{0}] ", Properties.Resources.Inactive)),
                        triggerText);
                }
            }

            row.AddText(triggerText);
            row.Tag = job;
            return row;
        }

        private static int ToInt(string s)
        {
            int i;
            if (!Int32.TryParse(s, out i))
                return 0;
            return i;
        }

        private static DateTime AsDate(int year, int month, int day, int hour, int min, int sec)
        {
            try
            {
                return new DateTime(year, month, day, hour, min, sec);
            }
            catch { }
            return DateTime.MinValue;
        }

        private void UpdateActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);
            int selectedCount = lvJobs.Selection.Count;

            contextButtons.AddButtonEnabled = hasPermission;
            contextButtons.EditButtonEnabled = selectedCount == 1;
            contextButtons.RemoveButtonEnabled = hasPermission && selectedCount >= 1;

            btnRunJob.Enabled = selectedCount == 1 && PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobRun);
        }

        private void ShowJob(JscJob job)
        {
            PluginEntry.Framework.ViewController.Add(new Views.JobView((Guid)job.ID, latestDataList.OrderBy(x=>x.Text)));
        }

        private void EditCurrentLine()
        {
            if (lvJobs.Selection.Count != 1)
                return;

            JscJob job = lvJobs.Selection[0].Tag as JscJob;
            if (job != null)
            {
                ShowJob(job);
            }
        }

        private void ViewJobLogHandler(object sender, ContextBarClickEventArguments args)
        {
            PluginOperations.ShowJobLogView(this, EventArgs.Empty);
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            EditCurrentLine();
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            if (this.newJobDialog.ShowDialog(PluginEntry.Framework.MainWindow) != DialogResult.OK)
                return;
            
            // Show the new item in the list view
            var row = GetRowFromJob(this.newJobDialog.Job);
            lvJobs.AddRow(row);
            UpdateActions();

            // Open up the detail view page
            ShowJob(this.newJobDialog.Job);
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            bool doRemove = false;
            List<JscJob> jobsToRemove = new List<JscJob>();
            for (int i = 0; i < lvJobs.Selection.Count; i++)
            {
                jobsToRemove.Add((JscJob)lvJobs.Selection[i].Tag);
            }

            if (jobsToRemove.Count == 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.JobRemoveMsg, ((JscJob)lvJobs.Selection[0].Tag).Text),
                        Properties.Resources.JobRemoveHeader
                    ) == DialogResult.Yes;
            }
            else if (jobsToRemove.Count > 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.JobRemoveManyMsg, jobsToRemove.Count),
                        Properties.Resources.JobRemoveHeader
                    ) == DialogResult.Yes;
            }

            if (doRemove)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var job in jobsToRemove)
                {
                    // Check if any other job is using this jobs subjobs
                    if (DataProviderFactory.Instance.Get<IJobData, JscJob>().IsJobUsedForSubjobs(PluginEntry.DataModel, job.ID))
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
                        }
                        sb.Append(job.Text);
                    }
                }

                if (sb.Length > 0)
                {
                    string msg;
                    if (jobsToRemove.Count == 1)
                    {
                        msg = Properties.Resources.JobRemoveInUseMsg;
                    }
                    else
                    {
                        msg = Properties.Resources.JobRemoveManyInUseMsg;
                    }

                    MessageBox.Show(PluginEntry.Framework.MainWindow, string.Format(msg, sb), Properties.Resources.JobRemoveHeader, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    doRemove = false;
                }

                if (doRemove)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, jobsToRemove);
                    RefreshData();
                    UpdateActions();
                }
            }
        }

        private void lvJobs_SelectionChanged(object sender, EventArgs e)
        {
            if (lvJobs.Selection.Count > 0)
                lastSelectedRow = lvJobs.Selection.FirstSelectedRow;
            UpdateActions();
        }

        private void lvJobs_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtons.EditButtonEnabled)
            {
                EditCurrentLine();
            }
        }

        private void lvJobs_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvJobs.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                contextButtons_EditButtonClicked);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtons.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    contextButtons_AddButtonClicked);

            item.Enabled = contextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    contextButtons_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.JobRun,
                   500,
                   btnRunJob_Click);

            item.Image = Properties.Resources.run_options_16;
            item.Enabled = btnRunJob.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("JobListViewMenu", lvJobs.ContextMenuStrip, lvJobs);
            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRunJob_Click(object sender, EventArgs e)
        {
            if (lvJobs.Selection.Count != 1)
                return;

            JscJob job = lvJobs.Selection[0].Tag as JscJob;
            if (job != null)
            {
                PluginOperations.RunJob( job);
            }
        }

        private void chkShowDisabled_CheckedChanged(object sender, EventArgs e)
        {
            colImage.Width = (short)(chkShowDisabled.Checked ? 16 : 0);
            RefreshData();
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            base.OnDataChanged(changeAction, objectName, changeIdentifier, param);
            if (objectName == "Job")
            {
                LoadData(false);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobView))
                {
                    var viewJobLogItem = new ContextBarItem(Properties.Resources.JobLog, ViewJobLogHandler);
                    arguments.Add(viewJobLogItem, 300);
                }
            }
            base.OnSetupContextBarItems(arguments);
        }

        protected override string LogicalContextName
        {
            get { return Properties.Resources.Jobs; }
        }

        public override RecordIdentifier ID
        {
            get { return RecordIdentifier.Empty; }
        }
    }
}
