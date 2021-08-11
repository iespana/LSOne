using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JscJobSubjob
    {
        private JscJob jscJob;
        private JscSubJob jscSubJob;
        private bool enabled;
        public RecordIdentifier ID { get; set; }
        public RecordIdentifier Job { get; set; }
        public RecordIdentifier SubJob { get; set; }
        public int Sequence { get; set; }
        public bool Enabled 
        { get { return enabled; } set { enabled = value; } }

        public JscJob JscJob
        {
            get { return jscJob; }
            set
            {
                jscJob = value;
                Job = value == null ? null :value.ID;
            }
        }

        public JscSubJob JscSubJob
        {
            get { return jscSubJob; }
            set
            {
                jscSubJob = value;
                SubJob = value == null ? null : value.ID;
            }
        }



    }

}
	


