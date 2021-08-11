using LSOne.SiteService.Utilities;
using Quartz;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public class EMailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (SiteManagerPlugin.Instance != null)
            {
                Utils.Log(this, "Sending e-mails", LogLevel.Trace);
                SiteManagerPlugin.Instance.SendQueuedEMailEntries();
            }
            else
            {
                Utils.Log(this, "SiteManagerPlugin instance is gone. Not sending e-mails", LogLevel.Trace);
            }
        }
    } 
}