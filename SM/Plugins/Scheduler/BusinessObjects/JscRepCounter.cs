using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscRepCounter
    {
        private JscJob jscJob;
        private JscSubJob jscSubJob;
        private JscLocation jscLocation;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier Job { get; set; }
        public RecordIdentifier SubJob { get; set; }
        public RecordIdentifier Location { get; set; }
        public int Counter { get; set; }

        public JscJob JscJob
        {
            get { return jscJob; }
            set
            {
                jscJob = value;
                Job = value == null ? Job : value.ID;
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

        public JscLocation JscLocation
        {
            get { return jscLocation; }
            set
            {
                jscLocation = value;
                Location = value == null ? Location : value.ID;
            }
        }
    }
}