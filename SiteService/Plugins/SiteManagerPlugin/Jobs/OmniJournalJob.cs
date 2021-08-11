using LSOne.SiteService.Utilities;
using Quartz;

namespace LSOne.SiteService.Plugins.SiteManager.Jobs
{
    public class OmniJournalJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (SiteManagerPlugin.Instance != null)
            {
                Utils.Log(this, "Processing omni journals.", LogLevel.Trace);
                SiteManagerPlugin.Instance.ProcessOmniJournalJob();
            }
            else
            {
                Utils.Log(this, "SiteManagerPlugin instance is gone. Cannot process omni journal.", LogLevel.Trace);
            }
        }
    }
}
