using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls.Recurrence;
using LSRetail.DD.Common;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class JobTriggerPage : UserControl, ITabView
    {
        private JobViewPageContext internalContext;
        private bool dataIsModified;

        public JobTriggerPage()
        {
            InitializeComponent();

            DateTime runOnceDate = DateTime.Now.AddHours(1);
            SetCombinedDateTime(runOnceDate, dtOnceDate, dtOnceTime);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new JobTriggerPage();
        }

        public void OnClose()
        {
        }


        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                this.internalContext = (JobViewPageContext)internalContext;
            }

            JobToForm();
            dataIsModified = false;

            UpdateActions();
        }


        public bool DataIsModified()
        {
            return dataIsModified;
        }

        public bool SaveData()
        {
            bool result = JobFromForm();
            if (result)
            {
                dataIsModified = false;
                internalContext.JobTriggersChanged = true;
            }

            return result;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //!throw new NotImplementedException();
        }

        public void SaveUserInterface()
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //!throw new NotImplementedException();
        }

        private void JobToForm()
        {
            JscJobTrigger trigger = internalContext.Job.JscJobTriggers.FirstOrDefault();

            if (trigger == null || trigger.StartTime == null)
            {
                rbManual.Checked = true;
            }
            else
            {
                CronSchedule cron = CronSchedule.CreateFromTrigger(trigger);
                if (cron.RecurrenceType == RecurrenceType.None)
                {
                    rbRunOnce.Checked = true;
                    DateTime onceDateTime = cron.GetOnceDateTime();
                    SetCombinedDateTime(onceDateTime, dtOnceDate, dtOnceTime);
                }
                else
                {
                    rbRecurrence.Checked = true;
                    recurrenceControl.Value = cron;
                }
            }
        }

        private bool JobFromForm()
        {
            bool result = true;

            if (rbManual.Checked)
            {
                foreach (var trigger in internalContext.Job.JscJobTriggers)
                {
                    DataProviderFactory.Instance.Get<IJobData, JscJob>().Delete(PluginEntry.DataModel, trigger);
                }

                internalContext.Job.JscJobTriggers.Clear();
            }
            else
            {
                JscJobTrigger trigger = internalContext.Job.JscJobTriggers.FirstOrDefault();
                if (trigger == null)
                {
                    trigger = new JscJobTrigger();
                    trigger.ID = Guid.NewGuid();
                    trigger.TriggerKind = TriggerKind.TimedSchedule;
                    trigger.Enabled = true;
                    internalContext.Job.JscJobTriggers.Add(trigger);
                }

                if (rbRunOnce.Checked)
                {
                    DateTime dateTime = GetCombinedDateTime(dtOnceDate.Value, dtOnceTime.Value);
                    trigger.RecurrenceType = RecurrenceType.None;
                    trigger.Seconds = dateTime.Second.ToString();
                    trigger.Minutes = dateTime.Minute.ToString();
                    trigger.Hours = dateTime.Hour.ToString();
                    trigger.Months = dateTime.Month.ToString();
                    trigger.Years = dateTime.Year.ToString();
                    trigger.DaysOfMonth = dateTime.Day.ToString();
                    trigger.DaysOfWeek = "*";
                    trigger.StartTime = dateTime;
                    trigger.EndTime = null;
                }
                else
                {
                    var cron = recurrenceControl.Value;
                    CronScheduleToTrigger(cron, trigger);
                }
            }

            return result;
        }

        private void CronScheduleToTrigger(CronSchedule cron, JscJobTrigger trigger)
        {
            trigger.RecurrenceType = cron.RecurrenceType;
            trigger.Seconds = cron.Seconds;
            trigger.Minutes = cron.Minutes;
            trigger.Hours = cron.Hours;
            trigger.DaysOfMonth = cron.DaysOfMonth;
            trigger.DaysOfWeek = cron.DaysOfWeek;
            trigger.Months = cron.Months;
            trigger.Years = cron.Years;
            trigger.StartTime = cron.StartDateTime;
            trigger.EndTime = cron.EndDateTime;
        }

        private void UpdateActions()
        {
            dtOnceDate.Enabled = rbRunOnce.Checked;
            dtOnceTime.Enabled = rbRunOnce.Checked;

            recurrenceControl.Enabled = rbRecurrence.Checked;
        }

        private DateTime GetCombinedDateTime(DateTime datePart, DateTime timePart)
        {
            return datePart.Date + timePart.TimeOfDay;
        }

        private void SetCombinedDateTime(DateTime dateTime, DateTimePicker dtDate, DateTimePicker dtTime)
        {
            dtDate.Value = dateTime.Date;
            dtTime.Value = dateTime;
        }

        private void rbManual_CheckedChanged(object sender, EventArgs e)
        {
            dataIsModified = true;
            UpdateActions();
        }

        private void rbRunOnce_CheckedChanged(object sender, EventArgs e)
        {
            dataIsModified = true;
            UpdateActions();
        }

        private void rbRecurrence_CheckedChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void recurrenceControl_ValueChanged(object sender, EventArgs e)
        {
            dataIsModified = true;
        }

        private void dtOnceDate_ValueChanged(object sender, EventArgs e)
        {
            dataIsModified = true;
        }

        private void dtOnceTime_ValueChanged(object sender, EventArgs e)
        {
            dataIsModified = true;
        }

        private void JobTriggerPage_Load(object sender, EventArgs e)
        {

        }

        private void recurrenceControl_Load(object sender, EventArgs e)
        {

        }
    }
}
