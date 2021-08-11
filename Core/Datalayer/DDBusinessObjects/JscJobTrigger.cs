using System;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.DDBusinessObjects
{

    public  class JscJobTrigger
    {
        private JscJob jscJob;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier Job { get; set; }
        public TriggerKind TriggerKind { get; set; }
        public bool Enabled { get; set; }
        public RecurrenceType RecurrenceType { get; set; }
        public string Seconds { get; set; }
        public string Minutes { get; set; }
        public string Hours { get; set; }
        public string Months { get; set; }
        public string Years { get; set; }
        public string DaysOfMonth { get; set; }
        public string DaysOfWeek { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Tag { get; set; }

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