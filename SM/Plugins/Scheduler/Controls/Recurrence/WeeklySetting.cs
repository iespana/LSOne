using System;
using System.Text;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class WeeklySetting : SettingBase
    {
        public DayOfWeek[] DaysOfWeek { get; set; }

        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.DaysOfMonth = "*";
            cron.Months = "*";
            cron.Years = "*";
            if (DaysOfWeek.Length == 7)
            {
                cron.DaysOfWeek = "*";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < DaysOfWeek.Length; i++)
                {
                    if (i > 0)
                        sb.Append(",");
                    sb.Append((int)DaysOfWeek[i]);
                }
                cron.DaysOfWeek = sb.ToString();
            }
        }

        public static WeeklySetting FromCronSchedule(CronSchedule cron)
        {
            WeeklySetting setting = new WeeklySetting();

            if (CronHelper.IsAny(cron.DaysOfWeek))
            {
                setting.DaysOfWeek = new DayOfWeek[7];
                for (int i = 0; i < 7; i++)
                {
                    setting.DaysOfWeek[i] = (DayOfWeek)i;
                }
            }
            else
            {
                string[] days = cron.DaysOfWeek.Split(',');
                setting.DaysOfWeek = new DayOfWeek[days.Length];
                for (int i = 0; i < days.Length; i++)
                {
                    setting.DaysOfWeek[i] = (DayOfWeek)int.Parse(days[i]);
                }
            }

            return setting;
        }

    }


}
