using System;
using System.Diagnostics;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class YearlySetting : SettingBase
    {
        public int YearStep { get; set; }
        public bool UseDayInMonth { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public WeekdaySequence WeekdaySequence { get; set; }
        public WeekdayCharm WeekdayCharm { get; set; }


        public override void ToCronSchedule(CronSchedule cron)
        {
            cron.DaysOfWeek = "*";

            if (UseDayInMonth)
            {
                cron.DaysOfMonth = Day.ToString();
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
                }
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
            cron.Months = Month.ToString();
            cron.Years = "*";
        }

        public static YearlySetting FromCronSchedule(CronSchedule cron)
        {
            YearlySetting setting = new YearlySetting();

            setting.UseDayInMonth = CronHelper.IsFixedNumber(cron.DaysOfMonth);
            if (setting.UseDayInMonth)
            {
                setting.Day = CronHelper.ParseToInt32(cron.DaysOfMonth);
            }
            else
            {
                // the case of WeekdayCharm == Day and cron.DaysOfMonth being a fixed number will
                // never actually be created because it is the same as UseDayInMonth == true and
                // Day = fixed number
                
                if (cron.DaysOfMonth == "L")
                {
                    setting.WeekdaySequence = WeekdaySequence.Last;
                    setting.WeekdayCharm = WeekdayCharm.Day;
                }
                else if (CronHelper.IsAny(cron.DaysOfMonth) && cron.DaysOfWeek.EndsWith("L"))
                {
                    setting.WeekdaySequence = WeekdaySequence.Last;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek.Replace("L", string.Empty)));
                }
                else if (cron.DaysOfMonth == "1-7")
                {
                    setting.WeekdaySequence = WeekdaySequence.First;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                }
                else if (cron.DaysOfMonth == "8-14")
                {
                    setting.WeekdaySequence = WeekdaySequence.Second;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                }
                else if (cron.DaysOfMonth == "15-21")
                {
                    setting.WeekdaySequence = WeekdaySequence.Third;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                }
                else if (cron.DaysOfMonth == "22-28")
                {
                    setting.WeekdaySequence = WeekdaySequence.Fourth;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                }
                else if (cron.DaysOfMonth == "*")
                {
                    setting.WeekdaySequence = WeekdaySequence.Last;
                    setting.WeekdayCharm = WeekdayCharm.FromDayOfWeek((DayOfWeek)int.Parse(cron.DaysOfWeek));
                }

            }

            setting.Month = CronHelper.ParseToInt32(cron.Months);

            return setting;
        }
    
    
    }


}
