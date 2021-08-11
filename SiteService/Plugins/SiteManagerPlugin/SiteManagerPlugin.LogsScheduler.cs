using LSOne.SiteService.Utilities;
using Quartz;
using System;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        private static IScheduler clearLogsScheduler = null;
        private static int daysToKeepLogs;

		protected virtual void UpdateClearLogSchedule(int daysToKeepLogsConfiguration)
        {
            try
            {
                Utils.Log(this, Utils.Starting);
                Utils.Log(this, "DaysToKeepLogs = " + daysToKeepLogsConfiguration, LogLevel.Debug);

                daysToKeepLogs = daysToKeepLogsConfiguration;
                if (daysToKeepLogs > 0)
                {
                    clearLogsScheduler = StartScheduler<ClearLogsJob>(clearLogsScheduler, "ClearLogs", TimeSpan.FromDays(Math.Min(7, daysToKeepLogs)));
                }
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

		internal void ClearOldLogs()
        {
			if(daysToKeepLogs > 0)
            {
                Utils.ClearOldLogs(daysToKeepLogs);
            }
        }
    }
}
