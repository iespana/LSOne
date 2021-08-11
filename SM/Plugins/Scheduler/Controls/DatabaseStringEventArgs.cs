using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class DatabaseStringEventArgs : EventArgs
    {
        public DatabaseString DatabaseString { get; set; }
    }
}
