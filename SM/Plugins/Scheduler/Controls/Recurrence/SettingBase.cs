using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public abstract class SettingBase
    {
        public abstract void ToCronSchedule(CronSchedule cron);
    }
}
