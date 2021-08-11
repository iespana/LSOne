using System.Data.SqlClient;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Constants;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class HourlySetting : SettingBase
    {
        /// <summary>
        /// Gets or sets a flag indicating a recurrence of every hour (true) or recurrence of every minute (false).
        /// </summary>
        public bool EveryHour { get; set; }

        public bool RunsOnInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a recurrence of an event on every hour divisible by the
        /// value of this property. Only valid if EveryHour is true.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        /// Gets or sets a value indicating a recurrence of an event on every minute divisible by the
        /// value of this property. Only valid if EveryHour is false.
        /// </summary>
        public int Minutes { get; set; }


        /// <summary>
        /// Controls the last hour each day the job is run
        /// </summary>
        public int HoursFrom { get; set; }

        /// <summary>
        /// Controls the first hour each day the job is run
        /// </summary>
        public int HoursTo { get; set; }

        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.DaysOfMonth = "*";
            cron.DaysOfWeek = "*";
            cron.Months = "*";
            cron.Years = "*";

            if (EveryHour)
            {
                if (HoursFrom != HoursTo)
                {
                    string hours = string.Empty;
                    int currentHour = HoursFrom;
                    while (currentHour + Hours <= (HoursTo == 0 ? 24 : HoursTo))
                    {

                        if (currentHour + 2* Hours <= (HoursTo == 0 ? 24 : HoursTo))
                        {

                            hours += currentHour + ",";
                        }
                        else
                        {
                            hours += currentHour;
                        }
                        currentHour += Hours;
                    }
                    cron.Hours = hours;
                }
                else
                {


                    if (Hours == 1)
                    {
                        cron.Hours = "*";
                    }
                    else
                    {
                        cron.Hours = "*/" + Hours.ToString();
                    }
                }
            }
            else
            {
                if (HoursFrom != HoursTo)
                {
                    string hours = string.Empty;
                    int currentHour = HoursFrom;
                    while (currentHour + Hours <= (HoursTo == 0 ? 24 : HoursTo))
                    {
                        if (currentHour + 2 * Hours <= (HoursTo == 0 ? 24 : HoursTo))
                        {

                            hours += currentHour + ",";
                        }
                        else
                        {
                            hours += currentHour;
                        }
                        currentHour += Hours;
                    }
                    cron.Hours = hours;
                }
                else
                {
                    cron.Hours = "*";
                }
                if (Minutes == 1)
                {
                    cron.Minutes = "*";
                }
                else
                {
                    cron.Minutes = "*/" + Minutes.ToString();
                }
            }
        }


        public static HourlySetting FromCronSchedule(CronSchedule cron)
        {
            //if (!(IsAny(cron.Months) && IsAny(cron.DaysOfMonth) && IsAny(cron.DaysOfWeek)))
            //{
            //    return null;
            //}

            HourlySetting setting = new HourlySetting();
            
            setting.EveryHour = CronHelper.IsFixedNumber(cron.Minutes);
            if (cron.Hours.LastIndexOf(',') == -1)
            {
                if (setting.EveryHour)
                {
                    setting.Hours = CronHelper.ParseToInt32(cron.Hours);
                    setting.Minutes = 1;
                }
                else
                {
                    setting.Minutes = CronHelper.ParseToInt32(cron.Minutes);
                    setting.Hours = 1;
                }
            }
            else
            {
                string[] RunsOnHours = cron.Hours.Split(',');

           
                setting.HoursFrom = int.Parse(RunsOnHours.First());
                setting.Hours = int.Parse(RunsOnHours[1]) - int.Parse(RunsOnHours.First());
                setting.HoursTo = int.Parse(RunsOnHours.Last()) + setting.Hours;
                if (setting.EveryHour)
                {
                    setting.Minutes = 1;
                }
                else
                {
                    setting.Minutes = CronHelper.ParseToInt32(cron.Minutes);
                }

            }

            return setting;
        }

    }
}
