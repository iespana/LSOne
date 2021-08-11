using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscJob : DataEntity
    {
        private JscJob jscSubjobJob;
        private JscLocation jscDestinationLocation;
        private JscLocation jscSourceLocation;

        public JobType JobType { get; set; }
        public RecordIdentifier Source { get; set; }
        public RecordIdentifier Destination { get; set; }
        public ErrorHandling ErrorHandling { get; set; }
        public bool Enabled { get; set; }
        public RecordIdentifier SubjobJob { get; set; }
        public List<JscJobTrigger> JscJobTriggers { get; set; }
        public List<JscJobSubjob> JscJobSubjobs { get; set; }
        public string PluginPath { get; set; }
        public string PluginArguments { get; set; }
        public string JobTypeCode { get; set; }
        public bool UseCurrentLocation { get; set; }

        public JscLocation JscSourceLocation
        {
            get { return jscSourceLocation; }
            set
            {
                jscSourceLocation = value;

                Source = value == null ? Source : value.ID;
            }
        }

        public JscLocation JscDestinationLocation
        {
            get { return jscDestinationLocation; }
            set
            {
                jscDestinationLocation = value;

                Destination = value == null ? Destination : value.ID;
            }
        }

        public JscJob JscSubjobJob
        {
            get { return jscSubjobJob; }
            set
            {
                jscSubjobJob = value;

                SubjobJob = value == null ? SubjobJob : value.ID;
            }

        }
    }
}
