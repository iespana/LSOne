using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{

    public class JscSchedulerLog
    {
        private JscJob jscJob;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier Job { get; set; }
        public DateTime RegTime { get; set; }
        public string Message { get; set; }

       

        public JscJob JscJob
        {
            get { return jscJob; }
            set
            {
                jscJob = value;
                Job = value == null ? Job : value.ID;
            }
        }
    }
}