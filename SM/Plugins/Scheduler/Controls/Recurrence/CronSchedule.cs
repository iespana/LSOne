using System;
using LSOne.DataLayer.DDBusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class CronSchedule
    {
        public RecurrenceType RecurrenceType { get; set; }

        public string Seconds { get; set; }
        public string Minutes { get; set; }
        public string Hours { get; set; }
        public string Months { get; set; }
        public string DaysOfMonth { get; set; }
        public string DaysOfWeek { get; set; }
        public string Years { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public override string ToString()
        {
            // second minute hour dom month dow
            return string.Format("{0} {1} {2} {3} {4} {5} {6}",
                Seconds,
                Minutes,
                Hours,
                DaysOfMonth,
                Months,
                DaysOfWeek,
                Years
            );
        }

        public DateTime GetOnceDateTime()
        {
            return new DateTime(int.Parse(Years), int.Parse(Months), int.Parse(DaysOfMonth), int.Parse(Hours), int.Parse(Minutes), int.Parse(Seconds));
        }

        internal static CronSchedule CreateFromTrigger(JscJobTrigger trigger)
        {
            var cron = new CronSchedule
            {
                RecurrenceType = trigger.RecurrenceType,
                Seconds = trigger.Seconds,
                Minutes = trigger.Minutes,
                Hours = trigger.Hours,
                DaysOfMonth = trigger.DaysOfMonth,
                DaysOfWeek = trigger.DaysOfWeek,
                Months = trigger.Months,
                Years = trigger.Years,
                StartDateTime = trigger.StartTime.Value,
                EndDateTime = trigger.EndTime
            };

            return cron;
        }
    }


}
