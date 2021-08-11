using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class RangeSetting : SettingBase
    {
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.Seconds = StartDateTime.Second.ToString();
            cron.Minutes = StartDateTime.Minute.ToString();
            cron.Hours = StartDateTime.Hour.ToString();
            cron.DaysOfMonth = StartDateTime.Day.ToString();
            cron.Months = StartDateTime.Month.ToString();
            cron.DaysOfWeek = "*";
            cron.Years = StartDateTime.Year.ToString();

            cron.StartDateTime = StartDateTime;
            cron.EndDateTime = EndDateTime;
        }
    }
}
