using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class DailySetting : SettingBase
    {
        /// <summary>
        /// Sets or gets a flag indicating a recurrence of every Days day (true) or if the recurrence is only on weekdays (false).
        /// </summary>
        public bool EveryDay { get; set; }

        /// <summary>
        /// If EveryDay is true, gets or sets a value indicating a recurrence of every Days days (e.g. Days = 2 means
        /// every other day, Days = 1 means every day).
        /// </summary>
        public int Days { get; set; }

        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.Years = "*";
            cron.Months = "*";
            if (EveryDay)
            {
                if (Days == 1)
                {
                    cron.DaysOfMonth = "*";
                }
                else
                {
                    cron.DaysOfMonth = "*/" + Days.ToString();
                }
                cron.DaysOfWeek = "*";
            }
            else
            {
                cron.DaysOfMonth = "*";
                cron.DaysOfWeek = "1,2,3,4,5";      // Monday - Friday
            }
        }


        public static DailySetting FromCronSchedule(CronSchedule cron)
        {
            DailySetting setting = new DailySetting();
            setting.EveryDay = cron.DaysOfWeek == "*";
            if (setting.EveryDay)
            {
                setting.Days = CronHelper.ParseToInt32(cron.DaysOfMonth);
            }

            return setting;
        }

    }


}
