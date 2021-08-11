using LSOne.DataLayer.DDBusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    internal class JobViewPageContext
    {
        public JscJob Job { get; set; }

        public JscLocation CurrentSourceLocation { get; set; }

        public bool JobTriggersChanged { get; set; }
    }
}
