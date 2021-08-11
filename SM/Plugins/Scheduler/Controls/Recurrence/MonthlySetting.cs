using System;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class MonthlySetting : SettingBase
    {
        public bool UseDayOfMonth { get; set; }
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public WeekdaySequence WeekdaySequence { get; set; }
        public WeekdayCharm WeekdayCharm { get; set; }

        // second minute hour dom month dow
        // second  This controls what second of the minute the command will run on,
        //         and is between '0' and '59'
        //	minute	This controls what minute of the hour the command will run on,
        //			and is between '0' and '59'
        //	hour	This controls what hour the command will run on, and is specified in
        //			the 24 hour clock, values must be between 0 and 23 (0 is midnight)
        //	dom		This is the Day of Month, that you want the command run on, e.g. to
        //			run a command on the 19th of each month, the dom would be 19.
        //	month	This is the month a specified command will run on, it may be specified
        //			numerically (1-12), or as the name of the month (e.g. May)
        //	dow		This is the Day of Week that you want a command to be run on, it can
        //			also be numeric (0-6) , with sunday as 0 and so on.

        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.Years = "*";
            if (UseDayOfMonth)
            {
                cron.DaysOfMonth = DayOfMonth.ToString();
                cron.DaysOfWeek = "*";
            }
            else
            {
                if (this.WeekdayCharm == WeekdayCharm.Day)
                {
                    if (this.WeekdaySequence == WeekdaySequence.First)
                    {
                        cron.DaysOfMonth = "1";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Second)
                    {
                        cron.DaysOfMonth = "2";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Third)
                    {
                        cron.DaysOfMonth = "3";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Fourth)
                    {
                        cron.DaysOfMonth = "4";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Last)
                    {
                        cron.DaysOfMonth = "L";
                    }
                    else
                    {
                        Debug.Assert(false, "Unknown WeekdaySequence");
                    }
                    cron.DaysOfWeek = "*";
                }
                //else if (this.WeekdayCharm == WeekdayCharm.Weekday)
                //{
                //    if (this.WeekdaySequence == WeekdaySequence.First)
                //    {
                //        cron.DaysOfMonth = "1W";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Second)
                //    {
                //        cron.DaysOfMonth = "2W";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Third)
                //    {
                //        cron.DaysOfMonth = "3W";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Fourth)
                //    {
                //        cron.DaysOfMonth = "4W";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Last)
                //    {
                //        cron.DaysOfMonth = "LW";
                //    }
                //    else
                //    {
                //        Debug.Assert(false, "Unknown WeekdaySequence");
                //    }
                //    throw new NotImplementedException("This feature is not implemented in the cron library yet");
                //}
                //else if (this.WeekdayCharm == WeekdayCharm.WeekendDay)
                //{
                //    if (this.WeekdaySequence == WeekdaySequence.First)
                //    {
                //        cron.DaysOfMonth = "1E";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Second)
                //    {
                //        cron.DaysOfMonth = "2E";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Third)
                //    {
                //        cron.DaysOfMonth = "3E";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Fourth)
                //    {
                //        cron.DaysOfMonth = "4E";
                //    }
                //    else if (this.WeekdaySequence == WeekdaySequence.Last)
                //    {
                //        cron.DaysOfMonth = "LE";
                //    }
                //    else
                //    {
                //        Debug.Assert(false, "Unknown WeekdaySequence");
                //    }
                //    throw new NotImplementedException("This feature is not implemented in the cron library yet");
                //}
                else if (this.WeekdayCharm.IsDayOfWeek())
                {
                    if (this.WeekdaySequence == WeekdaySequence.First)
                    {
                        cron.DaysOfMonth = "1-7";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Second)
                    {
                        cron.DaysOfMonth = "8-14";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Third)
                    {
                        cron.DaysOfMonth = "15-21";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Fourth)
                    {
                        cron.DaysOfMonth = "22-28";
                    }
                    else if (this.WeekdaySequence == WeekdaySequence.Last)
                    {
                        cron.DaysOfMonth = "*";
                    }
                    else
                    {
                        Debug.Assert(false, "Unknown WeekdaySequence");
                    }

                    cron.DaysOfWeek = ((int)this.WeekdayCharm.ToDayOfWeek()).ToString();
                    if (this.WeekdaySequence == WeekdaySequence.Last)
                    {
                        cron.DaysOfWeek += "L";
                    }
                }
            }

            if (Month == 1)
            {
                cron.Months = "*";
            }
            else
            {
                cron.Months = "*/" + Month.ToString();
            }
        }



        public static MonthlySetting FromCronSchedule(CronSchedule cron)
        {
            MonthlySetting setting = new MonthlySetting();

            setting.UseDayOfMonth = CronHelper.IsFixedNumber(cron.DaysOfMonth) && CronHelper.IsAny(cron.DaysOfWeek);
            if (setting.UseDayOfMonth)
            {
                setting.DayOfMonth = CronHelper.ParseToInt32(cron.DaysOfMonth);
            }
            else
            {
                if (CronHelper.IsAny(cron.DaysOfWeek))
                {
                    setting.WeekdayCharm = WeekdayCharm.Day;
                    if (CronHelper.IsFixedNumber(cron.DaysOfMonth))
                    {
                        int daysOfMonth = CronHelper.ParseToInt32(cron.DaysOfMonth);
                        setting.WeekdaySequence = WeekdaySequence.FromDay(daysOfMonth);
                    }
                    else if (cron.DaysOfMonth == "L")
                    {
                        setting.WeekdaySequence = WeekdaySequence.Last;
                    }
                    else
                        throw new ArgumentException("Invalid cron configuration");
                }
                else
                {
                    if (CronHelper.IsAny(cron.DaysOfMonth) && cron.DaysOfWeek.EndsWith("L"))
                    {
                        setting.WeekdaySequence = WeekdaySequence.Last;
                        setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek.Replace("L", string.Empty)));
                    }
                    else
                    {
                        if (cron.DaysOfMonth == "1-7")
                        {
                            setting.WeekdaySequence = WeekdaySequence.First;
                        }
                        else if (cron.DaysOfMonth == "8-14")
                        {
                            setting.WeekdaySequence = WeekdaySequence.Second;
                        }
                        else if (cron.DaysOfMonth == "15-21")
                        {
                            setting.WeekdaySequence = WeekdaySequence.Third;
                        }
                        else if (cron.DaysOfMonth == "22-28")
                        {
                            setting.WeekdaySequence = WeekdaySequence.Fourth;
                        }
                        else if (cron.DaysOfMonth == "*")
                        {
                            setting.WeekdaySequence = WeekdaySequence.Last;
                        }
                        setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                    }
                }
            }

            setting.Month = CronHelper.ParseToInt32(cron.Months);

            return setting;
        }
    }
}
