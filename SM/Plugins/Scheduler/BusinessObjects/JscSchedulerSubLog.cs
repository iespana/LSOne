using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{

    public class JscSchedulerSubLog
    {
        private JscSubJob jscSubJob;
        private JscLocation jscLocation;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier JobLog { get; set; }
        public RecordIdentifier SubJob { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public bool RunAsNormal { get; set; }
        public int ReplicationCounterStart { get; set; }
        public int ReplicationCounterEnd { get; set; }
        public RecordIdentifier Location { get; set; }

        public JscLocation JscLocation
        {
            get { return jscLocation; }
            set
            {
                jscLocation = value;
                Location = value == null ? Location : value.ID;
            }
        }

        public JscSubJob JscSubJob
        {
            get { return jscSubJob; }
            set
            {
                jscSubJob = value;
                SubJob = value == null ? SubJob : value.ID;
            }
        }
    }
}