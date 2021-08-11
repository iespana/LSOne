using LSOne.DataLayer.BusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class SchedulerSettings
    {
        public ServerSettings ServerSettings { get; set; }

        public void Validate()
        {
            ServerSettings.Validate();
        }
    }

    public class ServerSettings
    {
        public string Host { get; set; }
        public NetMode NetMode { get; set; }
        public string Port { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Host))
            {
                throw new ValidationException("Host name cannot be empty");
            }
        }
    }
}
