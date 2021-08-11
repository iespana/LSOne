using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscSubJobFromTableFilter
    {
        private JscSubJob jscSubJob;
        private JscFieldDesign jscField;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier SubJob { get; set; }
        public RecordIdentifier Field { get; set; }
        public LookupCode FilterType { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public LinkedFilterType ApplyFilter { get; set; }

        public JscSubJob JscSubJob
        {
            get { return jscSubJob; }
            set
            {
                jscSubJob = value;
                SubJob = value == null ? SubJob : value.ID;
            }
        }

        public JscFieldDesign JscField
        {
            get { return jscField; }
            set
            {
                jscField = value;
                Field = value == null ? Field : value.ID;
            }
        }
    }
}
