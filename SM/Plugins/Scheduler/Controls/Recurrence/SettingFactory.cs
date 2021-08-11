using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal static class SettingFactory
    {
        //public static SettingBase CreateSetting(CronSchedule cron)
        //{
        //    //if (cron.
        //    //if (IsFixedNumber(cron.Months))
        //    //{
        //    //    return YearlySetting.CreateFromCron(cron);
        //    //}
        //    //else if (IsAny(cron.Months) && IsAny(cron.DaysOfMonth) && IsAny(cron.DaysOfWeek))
        //    //{
        //    //    return HourlySetting.CreateFromCron(cron);
        //    //}
        //    //else if (IsAny(cron.Months) && IsAny(cron.DaysOfWeek) && (
        //}

        private class ControlSetting
        {
            public Type ControlType { get; set; }
            public Type SettingType { get; set; }
        }

        
        private static ControlSetting[] controlsAndSettings = new ControlSetting[]
        {
            new ControlSetting { ControlType = typeof(HourlyControl), SettingType = typeof(HourlySetting) },
            new ControlSetting { ControlType = typeof(DailyControl), SettingType = typeof(DailySetting) },
            new ControlSetting { ControlType = typeof(WeeklyControl), SettingType = typeof(WeeklySetting) },
            new ControlSetting { ControlType = typeof(MonthlyControl), SettingType = typeof(MonthlySetting) },
            new ControlSetting { ControlType = typeof(YearlyControl), SettingType = typeof(YearlySetting) },
            new ControlSetting { ControlType = typeof(RangeControl), SettingType = typeof(RangeSetting) }
        };

        
        public static Type GetControlTypeBySettingType(Type settingType)
        {
            for (int i = 0; i < controlsAndSettings.Length; i++)
            {
                if (controlsAndSettings[i].SettingType == settingType)
                {
                    return controlsAndSettings[i].ControlType;
                }
            }

            throw new ArgumentOutOfRangeException("Unknown setting type " + settingType.FullName);
        }
    }
}
