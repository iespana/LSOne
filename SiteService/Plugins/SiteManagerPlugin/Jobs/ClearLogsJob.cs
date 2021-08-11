using LSOne.SiteService.Utilities;
using Quartz;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public class ClearLogsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (SiteManagerPlugin.Instance != null)
            {
                Utils.Log(this, "Clearing old logs", LogLevel.Trace);
                SiteManagerPlugin.Instance.ClearOldLogs();
            }
            else
            {
                Utils.Log(this, "SiteManagerPlugin instance is gone. Cannot clear old logs.", LogLevel.Trace);
            }
        }
    }
}
